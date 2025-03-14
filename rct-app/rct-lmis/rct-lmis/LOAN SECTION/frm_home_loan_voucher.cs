using MongoDB.Bson;
using MongoDB.Driver;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace rct_lmis.LOAN_SECTION
{
    public partial class frm_home_loan_voucher : Form
    {
        private string clientNo;
        private string loggedInUsername;

        public frm_home_loan_voucher(string clientNumber)
        {
            InitializeComponent();
            loggedInUsername = UserSession.Instance.CurrentUser;
            clientNo = clientNumber;
        }

        LoadingFunction load = new LoadingFunction();

        private void LoadCollectors()
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<BsonDocument>("loan_collectors");
            var names = collection.Find(new BsonDocument())
                                  .Project(Builders<BsonDocument>.Projection.Include("Name"))
                                  .ToList()
                                  .Select(doc => doc["Name"].AsString)
                                  .ToList();

            // Add the default item
            names.Insert(0, "--select collector--");

            cbcollector.Items.Clear();
            cbcollector.Items.AddRange(names.ToArray());

            // Set the default selection to the first item
            cbcollector.SelectedIndex = 0;
        }

        private void LoadUserInfo(string username)
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<BsonDocument>("user_accounts"); // 'user_accounts' is the name of your collection

            var filter = Builders<BsonDocument>.Filter.Eq("Username", username);
            var user = collection.Find(filter).FirstOrDefault();

            if (user != null)
            {
                // Get the full name and split to get the first name
                var fullName = user.GetValue("FullName").AsString;

                // Set the first name
                lcurruser.Text = fullName;
            }
        }

        // Method to fetch the document
        private BsonDocument FetchDocument(string clientNo)
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<BsonDocument>("loan_disbursed");

            var filter = Builders<BsonDocument>.Filter.Eq("ClientNo", clientNo);
            var document = collection.Find(filter).FirstOrDefault();

            return document;
        }

        private string FormatAmortizationPeriod(string loanMode, int term)
        {
            switch (loanMode.ToUpper())
            {
                case "DAILY":
                    return "20 days"; // Fixed ruling for daily mode

                case "WEEKLY":
                    return $"{4 * term} weeks"; // Assuming 4 weeks per month

                case "SEMI-MONTHLY":
                    return $"{2 * term} cut-offs"; // 2 payments per month

                case "MONTHLY":
                    return $"{term} months"; // Term in months

                default:
                    return "N/A";
            }
        }

        private void ComputeEndPaymentDate()
        {
            // Step 1: Parse Start Payment Date
            if (!DateTime.TryParse(lstartpayment.Text, out DateTime startDate))
            {
                MessageBox.Show("Invalid start date format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Step 2: Get Loan Term & Payment Mode
            string loanMode = lloanmode.Text.ToUpper(); // DAILY, WEEKLY, etc.
            int term = ExtractNumericValue(lloanterm.Text); // Extract term from "3 months", etc.

            Console.WriteLine($"DEBUG: Start Date = {startDate:MM/dd/yyyy}");
            Console.WriteLine($"DEBUG: Loan Mode = {loanMode}");
            Console.WriteLine($"DEBUG: Loan Term = {term}");

            if (term <= 0)
            {
                MessageBox.Show("Invalid loan term format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Step 3: Compute Correct Maturity Date
            DateTime endDate = ComputeMaturityDate(startDate, loanMode, term);

            Console.WriteLine($"DEBUG: Computed End Date = {endDate:MM/dd/yyyy}");

            // Step 4: Display Correct Maturity Date in UI
            lendpayment.Text = endDate.ToString("MM/dd/yyyy");
        }

        // ✅ Extract numeric value from loan term (e.g., "1 months" -> 1, "3 weeks" -> 3)
        private int ExtractNumericValue(string termText)
        {
            if (string.IsNullOrWhiteSpace(termText)) return 0;
            Match match = Regex.Match(termText, @"\d+");
            return match.Success ? int.Parse(match.Value) : 0;
        }

        // ✅ Compute Maturity Date Based on Payment Mode
        private DateTime ComputeMaturityDate(DateTime startDate, string loanMode, int term)
        {
            switch (loanMode)
            {
                case "DAILY":
                    return ComputeDailyEndDate(startDate, 20 * term); // 20 working days per month

                case "WEEKLY":
                    return ComputeWeeklyEndDate(startDate, term * 4); // 4 weeks per month

                case "SEMI-MONTHLY":
                    return ComputeSemiMonthlyEndDate(startDate, term); // 2 cut-offs per month

                case "MONTHLY":
                    return startDate.AddMonths(term); // Directly add months

                default:
                    return startDate; // If unknown mode, return original date
            }
        }

        // ✅ Daily: Exclude weekends
        private DateTime ComputeDailyEndDate(DateTime startDate, int workingDays)
        {
            DateTime currentDate = startDate;
            int daysAdded = 0;

            while (daysAdded < workingDays)
            {
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    daysAdded++;
                }
                currentDate = currentDate.AddDays(1);
            }

            return currentDate.AddDays(-1); // Adjust for overshoot
        }

        // ✅ Weekly: Add weeks while keeping the same weekday
        private DateTime ComputeWeeklyEndDate(DateTime startDate, int totalWeeks)
        {
            return startDate.AddDays(totalWeeks * 7);
        }

        // ✅ Semi-Monthly: Add term * 2 payments (15 days per cut-off)
        private DateTime ComputeSemiMonthlyEndDate(DateTime startDate, int term)
        {
            return startDate.AddMonths(term).AddDays(-15); // Ends near the second cut-off
        }


        private int ExtractLoanTerm(string loanTermString)
        {
            if (string.IsNullOrWhiteSpace(loanTermString))
                return 0;

            var match = Regex.Match(loanTermString, @"\d+"); // Extracts numeric value
            return match.Success ? int.Parse(match.Value) : 0;
        }

        private void LoadDataToUI(string clientNumber)
        {
            var document = FetchDocument(clientNumber);

            if (document != null)
            {
                double loanAmount = document.Contains("LoanAmount") ? ConvertBsonValueToDouble(document["LoanAmount"]) : 0;
                double amortizedAmt = document.Contains("LoanAmortization") ? ConvertBsonValueToDouble(document["LoanAmortization"]) : 0;
                int loanTerm = document.Contains("LoanTerm") ? ExtractLoanTerm(document["LoanTerm"].AsString) : 0;
                string loanMode = document.Contains("PaymentMode") ? document["PaymentMode"].AsString : "N/A";

                loanLNno.Text = document.Contains("LoanNo") ? document["LoanNo"].AsString : "N/A";
                lloanamt.Text = FormatCurrency(loanAmount);
                lloanterm.Text = $"{loanTerm} months";
                lloaninterest.Text = document.Contains("LoanInterest") ? document["LoanInterest"].AsString : "N/A";

                lamotperiod.Text = FormatAmortizationPeriod(loanMode, loanTerm);
                lloantype.Text = document.Contains("LoanType") ? document["LoanType"].AsString : "N/A";
                lloanmode.Text = loanMode;
                lloanprocessfee.Text = document.Contains("cashProFee") ? FormatCurrency(ConvertBsonValueToDouble(document["cashProFee"])) :
                                      document.Contains("onlineProFee") ? FormatCurrency(ConvertBsonValueToDouble(document["onlineProFee"])) :
                                      document.Contains("bankProFee") ? FormatCurrency(ConvertBsonValueToDouble(document["bankProFee"])) : "N/A";

                lclientno.Text = document.Contains("ClientNo") ? document["ClientNo"].AsString : "N/A";
                lstartpayment.Text = document.Contains("StartPaymentDate") ? document["StartPaymentDate"].AsString : "N/A";

                if (document.Contains("cashName"))
                {
                    lclientname.Text = document["cashName"].AsString;
                    lpaymentmode.Text = document["PaymentMethod"].AsString;
                }
                else if (document.Contains("onlineName"))
                {
                    lclientname.Text = document["onlineName"].AsString;
                    lpaymentmode.Text = document["PaymentMethod"].AsString;
                }
                else if (document.Contains("bankName"))
                {
                    lclientname.Text = document["bankName"].AsString;
                    lpaymentmode.Text = document["PaymentMethod"].AsString;
                }
                else
                {
                    lclientname.Text = "N/A";
                }

                int amortizationPeriod = 0;
                if (loanMode == "WEEKLY")
                    amortizationPeriod = loanTerm * 4;
                else if (loanMode == "DAILY")
                    amortizationPeriod = loanTerm * 20; // Fix: 20 days for daily payments
                else if (loanMode == "SEMI-MONTHLY")
                    amortizationPeriod = loanTerm * 2;
                else if (loanMode == "MONTHLY")
                    amortizationPeriod = loanTerm;

                // Compute total loan with interest
                double interestRate = 0.05; // 5% interest
                double totalLoanWithInterest = loanAmount * (1 + interestRate);

                // Compute correct amortized amount per period
                double correctedAmortizedAmt = totalLoanWithInterest / amortizationPeriod;

                // Compute the correct total amount to pay
                double amountToPay = Math.Floor(correctedAmortizedAmt * amortizationPeriod);

                // Debugging logs
                //Console.WriteLine($"Loan Amount: {loanAmount}");
                //Console.WriteLine($"Total Loan with Interest: {totalLoanWithInterest}");
                //Console.WriteLine($"Amortized Amount: {correctedAmortizedAmt}");
                //Console.WriteLine($"Loan Term: {loanTerm} months");
                //Console.WriteLine($"Payment Mode: {loanMode}");
                //Console.WriteLine($"Amortization Period: {amortizationPeriod}");
                //Console.WriteLine($"Corrected Amount To Pay: {amountToPay}");


                ComputeEndPaymentDate();

                var dataTable = new DataTable();
                dataTable.Columns.Add("Transaction No.");
                dataTable.Columns.Add("Releasing Date");
                dataTable.Columns.Add("Loan Amount (Principal)");
                dataTable.Columns.Add("Processing Fee");
                dataTable.Columns.Add("Disbursed Amount");
                dataTable.Columns.Add("Amount To Pay");
                dataTable.Columns.Add("Amortized Amount");

                if (document.Contains("cashNo"))
                {
                    _ = dataTable.Rows.Add(
                        document["cashNo"].AsString,
                        document.Contains("cashDate") ? document["cashDate"].AsString : "N/A",
                        FormatCurrency(loanAmount),
                        FormatCurrency(ConvertBsonValueToDouble(document["cashProFee"])),
                        FormatCurrency(ConvertBsonValueToDouble(document["cashPoAmt"])),
                        FormatCurrency(amountToPay),
                        FormatCurrency(amortizedAmt)
                    );
                }
                else if (document.Contains("onlineRefNo"))
                {
                    _ = dataTable.Rows.Add(
                        document["onlineRefNo"].AsString,
                        document.Contains("onlineDate") ? document["onlineDate"].AsString : "N/A",
                        FormatCurrency(loanAmount),
                        FormatCurrency(ConvertBsonValueToDouble(document["onlineProFee"])),
                        FormatCurrency(ConvertBsonValueToDouble(document["onlinePoAmt"])),
                        FormatCurrency(amountToPay),
                        FormatCurrency(amortizedAmt)
                    );
                }
                else if (document.Contains("bankRefNo"))
                {
                    _ = dataTable.Rows.Add(
                        document["bankRefNo"].AsString,
                        document.Contains("bankDate") ? document["bankDate"].AsString : "N/A",
                        FormatCurrency(loanAmount),
                        FormatCurrency(ConvertBsonValueToDouble(document["bankProFee"])),
                        FormatCurrency(ConvertBsonValueToDouble(document["bankAmt"])),
                        FormatCurrency(amountToPay),
                        FormatCurrency(amortizedAmt)
                    );
                }
                else
                {
                    MessageBox.Show("No payment details found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                dgvdisburse.DataSource = dataTable;
            }
            else
            {
                MessageBox.Show("Data not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Helper method to convert BSON value to double
        private double ConvertBsonValueToDouble(BsonValue value)
        {
            if (value == null || value.IsBsonNull) return 0.0;

            string strValue = value.ToString().Replace("₱", "").Replace(",", "").Trim();  // Remove "₱" and commas

            if (double.TryParse(strValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
            {
                return result;
            }

            return 0.0;  // Return 0.0 if conversion fails
        }

        private string FormatCurrency(double amount)
        {
            // Format the amount as currency with the Peso sign
            return $"₱{amount:N2}";
        }

        private void LoadCollectorInfo(string collectorName)
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<BsonDocument>("loan_collectors");

            var filter = Builders<BsonDocument>.Filter.Eq("Name", collectorName);
            var collectorDoc = collection.Find(filter).FirstOrDefault();

            if (collectorDoc != null)
            {
                tarearoute.Text = collectorDoc.Contains("AreaRoute") ? collectorDoc["AreaRoute"].AsString : "N/A";
                tidno.Text = collectorDoc.Contains("GeneratedIDNumber") ? collectorDoc["GeneratedIDNumber"].AsString : "N/A";
                tdesignation.Text = collectorDoc.Contains("Role") ? collectorDoc["Role"].AsString : "N/A";
                tcontact.Text = collectorDoc.Contains("ContactNumber") ? collectorDoc["ContactNumber"].AsString : "N/A";
            }
            else
            {
                tarearoute.Text = "N/A";
                tidno.Text = "N/A";
                tdesignation.Text = "N/A";
                tcontact.Text = "N/A";
            }
        }

        private BsonDocument CreateLoanReleasedDocument(BsonDocument loanApprovedDetails)
        {
            string encoderName = UserSession.Instance.UserName;
            DateTime currentDateTime = DateTime.Now;

            double loanAmount = ExtractNumericValue(loanApprovedDetails, "LoanAmount");
            double processingFee = ExtractNumericValue(loanApprovedDetails, "cashProFee");
            double payoutAmount = ExtractNumericValue(loanApprovedDetails, "cashPoAmt");
            int loanTerm = ExtractIntegerValue(loanApprovedDetails, "LoanTerm");
            double loanInterest = ExtractNumericValue(loanApprovedDetails, "LoanInterest");

            // ✅ Validate and parse payout date
            DateTime payoutDate;
            string startPaymentDate = lstartpayment.Text.Trim();

            string[] dateFormats = { "MM/dd/yyyy", "M/d/yyyy", "yyyy-MM-dd", "dd/MM/yyyy", "d/M/yyyy" };

            if (!DateTime.TryParseExact(startPaymentDate, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out payoutDate))
            {
                throw new FormatException($"Invalid date format: '{startPaymentDate}'. Expected formats: {string.Join(", ", dateFormats)}");
            }

            // ✅ Construct document safely
            var document = new BsonDocument
            {
                { "LoanIDNo", loanApprovedDetails.GetValue("LoanNo", "").ToString() },
                { "loanAmt", loanAmount },
                { "loanTerm", loanTerm },
                { "loanInterest", loanInterest },
                { "LoanType", loanApprovedDetails.GetValue("LoanType", "").ToString() },
                { "Mode", loanApprovedDetails.GetValue("PaymentMode", "").ToString() },
                { "cashProFee", processingFee },
                { "cashPoAmt", payoutAmount },
                { "cashClnNo", loanApprovedDetails.GetValue("ClientNo", "").ToString() },
                { "payoutDate", payoutDate },
                { "clientName", loanApprovedDetails.GetValue("cashName", "").ToString() },
                { "paymentMode", loanApprovedDetails.GetValue("PaymentMode", "").ToString() },
                { "CollectorName", loanApprovedDetails.GetValue("CollectorName", "").ToString() },
                { "Encoder", encoderName },
                { "ReleasingDate", currentDateTime }
            };

            return document;
        }


        private double ExtractNumericValue(BsonDocument doc, string fieldName)
        {
            if (!doc.Contains(fieldName) || doc[fieldName].IsBsonNull)
                return 0.0;

            string value = doc[fieldName].ToString().Replace("₱", "").Replace(",", "").Trim();

            return double.TryParse(value, out double result) ? result : 0.0;
        }

        private int ExtractIntegerValue(BsonDocument doc, string fieldName)
        {
            if (!doc.Contains(fieldName) || doc[fieldName].IsBsonNull)
                return 0;

            string value = new string(doc[fieldName].ToString().TakeWhile(char.IsDigit).ToArray()); // Extract only numbers

            return int.TryParse(value, out int result) ? result : 0;
        }

        private void SaveDocumentToCollection(BsonDocument document)
        {
            var database = MongoDBConnection.Instance.Database;
            var releasedCollection = database.GetCollection<BsonDocument>("loan_released");

            document["days"] = lamotperiod.Text; // Ensure this field exists and is properly formatted
            releasedCollection.InsertOne(document);
        }


        private decimal ConvertBsonValueToDecimal(BsonValue bsonValue)
        {
            if (bsonValue.IsDouble)
            {
                return Convert.ToDecimal(bsonValue.AsDouble);
            }
            else if (bsonValue.IsInt32)
            {
                return Convert.ToDecimal(bsonValue.AsInt32);
            }
            else if (bsonValue.IsInt64)
            {
                return Convert.ToDecimal(bsonValue.AsInt64);
            }
            else
            {
                return 0; // or throw an exception based on your logic
            }
        }


        private int GetTotalWeeks(int days, DateTime startDate)
        {
            int totalWeeks = 0;
            int daysLeft = days;
            DateTime currentDate = startDate;

            while (daysLeft > 0)
            {
                int weekDays = 0;

                for (int i = 0; i < 7 && daysLeft > 0; i++)
                {
                    if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                    {
                        weekDays++;
                        daysLeft--;
                    }
                    currentDate = currentDate.AddDays(1);
                }

                if (weekDays > 0)
                    totalWeeks++;
            }

            return totalWeeks;
        }

        private void AddPaymentDetails(BsonDocument document)
        {
            var paymentDetails = new BsonDocument
             {
                 { "Transaction No.", "N/A" },
                 { "Releasing Date", "N/A" },
                 { "Loan Amount (Principal)", "N/A" },
                 { "Processing Fee", "N/A" },
                 { "Disbursed Amount", "N/A" },
                 { "Amount To Pay", "N/A" },
                 { "Amortized Amount", "N/A" } // Added Amortized Amount field
             };

            if (dgvdisburse.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvdisburse.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        paymentDetails["Transaction No."] = row.Cells["Transaction No."].Value?.ToString() ?? "N/A";
                        paymentDetails["Releasing Date"] = row.Cells["Releasing Date"].Value?.ToString() ?? "N/A";
                        paymentDetails["Loan Amount (Principal)"] = row.Cells["Loan Amount (Principal)"].Value?.ToString() ?? "N/A";
                        paymentDetails["Processing Fee"] = row.Cells["Processing Fee"].Value?.ToString() ?? "N/A";
                        paymentDetails["Disbursed Amount"] = row.Cells["Disbursed Amount"].Value?.ToString() ?? "N/A";
                        paymentDetails["Amount To Pay"] = row.Cells["Amount To Pay"].Value?.ToString() ?? "N/A";
                        paymentDetails["Amortized Amount"] = row.Cells["Amortized Amount"].Value?.ToString() ?? "N/A"; // Added Amortized Amount logic
                    }
                }
            }

            document["PaymentDetails"] = paymentDetails;
        }

        private void ClearUIFields()
        {
            // Clear the UI fields as needed
            loanLNno.Text = "";
            lloanamt.Text = "";
            lloanterm.Text = "";
            lloaninterest.Text = "";
            lamotperiod.Text = "";
            lloantype.Text = "";
            lloanmode.Text = "";
            lloanprocessfee.Text = "";
            lclientno.Text = "";
            lstartpayment.Text = "";
            lclientname.Text = "";
            lpaymentmode.Text = "";

            dgvdisburse.DataSource = null;  // Clear DataGridView
            lnorecord.Visible = true;
            lnorecord.Visible = true;
        }

        private void DeleteLatestDisbursedRecord()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loanLNno.Text))
                {
                    MessageBox.Show("No LoanNo specified. Please provide a valid LoanNo.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_disbursed");

                // Create filter to find the record based on LoanNo
                var filter = Builders<BsonDocument>.Filter.Eq("LoanNo", loanLNno.Text);

                // Find the record to delete
                var recordToDelete = collection.Find(filter).FirstOrDefault();

                if (recordToDelete != null)
                {
                    // Delete the record
                    var result = collection.DeleteOne(filter);

                    if (result.DeletedCount > 0)
                    {
                        MessageBox.Show($"Record with Loan No.: {loanLNno.Text} has been successfully cancelled and aborted.", "Loan Releasing Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Failed to delete record with LoanNo {loanLNno.Text}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"No record found with LoanNo {loanLNno.Text}.", "No Record Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting the record: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerateNextCVNo(IMongoCollection<BsonDocument> voucherCollection)
        {
            var lastVoucher = voucherCollection.Find(new BsonDocument())
                                .Sort(Builders<BsonDocument>.Sort.Descending("CVNo"))
                                .Limit(1)
                                .FirstOrDefault();

            string lastCVNo = lastVoucher != null ? lastVoucher["CVNo"].AsString : "RCT-CV00000";
            int nextNumber = int.Parse(lastCVNo.Replace("RCT-CV", "")) + 1;

            return $"RCT-CV{nextNumber:D5}";
        }

        private void SaveAccountingEntries(string loanNo)
        {
            var database = MongoDBConnection.Instance.Database;
            var disburseCollection = database.GetCollection<BsonDocument>("loan_disbursed");
            var accountCollection = database.GetCollection<BsonDocument>("loan_account_data");

            // ✅ Fetch the loan disbursement details based on LoanNo
            var filter = Builders<BsonDocument>.Filter.Eq("LoanNo", loanLNno.Text);
            var loanDisburseDocument = disburseCollection.Find(filter).FirstOrDefault();

            if (loanDisburseDocument == null)
            {
                MessageBox.Show($"Loan disbursement record not found for LoanNo: {loanNo}!",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string paymentMethod = loanDisburseDocument.GetValue("PaymentMethod", "").ToString().ToUpper();
            double loanAmount = ConvertToDouble(loanDisburseDocument.GetValue("PrincipalAmount", "0"));
            double processingFee = 0.0;
            double disbursedAmount = 0.0;
            string accountTitleDisbursement = "";

            switch (paymentMethod)
            {
                case "DISBURSE CASH":
                case "CASH":
                    processingFee = ConvertToDouble(loanDisburseDocument.GetValue("cashProFee", "0"));
                    disbursedAmount = ConvertToDouble(loanDisburseDocument.GetValue("cashPoAmt", "0"));
                    accountTitleDisbursement = "A100-1"; // ✅ Cash Account
                    break;

                case "DISBURSE ONLINE":
                case "ONLINE":
                    processingFee = ConvertToDouble(loanDisburseDocument.GetValue("onlineProFee", "0"));
                    disbursedAmount = ConvertToDouble(loanDisburseDocument.GetValue("onlinePoAmt", "0"));
                    accountTitleDisbursement = "A100-2"; // ✅ Online Payment Account
                    break;

                case "DISBURSE BANK TRANSFER":
                case "BANK TRANSFER":
                case "BANK":
                    processingFee = ConvertToDouble(loanDisburseDocument.GetValue("bankProFee", "0"));
                    disbursedAmount = ConvertToDouble(loanDisburseDocument.GetValue("bankAmt", "0"));
                    accountTitleDisbursement = "A100-3"; // ✅ Bank Transfer Account
                    break;

                default:
                    MessageBox.Show($"Invalid Payment Method detected: '{paymentMethod}'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }


            // ✅ Batch insert for efficiency
            var entries = new List<BsonDocument>
             {
                 new BsonDocument
                 {
                     { "AccountTitle", "A120-1" },
                     { "Debit", loanAmount },
                     { "Credit", 0 },
                     { "Reference", loanNo },
                     { "Date", DateTime.Now }
                 },
                 new BsonDocument
                 {
                     { "AccountTitle", "I400-2" },
                     { "Debit", 0 },
                     { "Credit", processingFee },
                     { "Reference", loanNo },
                     { "Date", DateTime.Now }
                 },
                 new BsonDocument
                 {
                     { "AccountTitle", accountTitleDisbursement },
                     { "Debit", 0 },
                     { "Credit", disbursedAmount },
                     { "Reference", loanNo },
                     { "Date", DateTime.Now }
                 },
                 new BsonDocument
                 {
                     { "AccountTitle", "A120-2" },
                     { "Debit", 0 },
                     { "Credit", 0 }, // Assuming interest is tracked later
                     { "Reference", loanNo },
                     { "Date", DateTime.Now }
                 },
                 new BsonDocument
                 {
                     { "AccountTitle", "L200-9" },
                     { "Debit", 0 },
                     { "Credit", 0 }, // Assuming unearned income tracking
                     { "Reference", loanNo },
                     { "Date", DateTime.Now }
                 }
             };

            if (entries.Count > 0)
            {
                try
                {
                    accountCollection.InsertMany(entries);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error inserting accounting entries: {ex.Message}",
                                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No accounting entries generated.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ✅ Helper Function to Convert MongoDB Values to Double
        private double ConvertToDouble(BsonValue value)
        {
            string strValue = value.ToString().Replace("₱", "").Replace(",", "").Trim();
            return double.TryParse(strValue, out double result) ? result : 0.0;
        }

        private void UpdateLoanApprovedCollection(double principalAmount, int loanTerm)
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var approvedCollection = database.GetCollection<BsonDocument>("loan_approved");

                var filter = Builders<BsonDocument>.Filter.Eq("ClientNo", lclientno.Text);
                var update = Builders<BsonDocument>.Update
                    .Set("LoanProcessStatus", "Loan Released")
                    .Set("PrincipalAmount", principalAmount)
                    .Set("LoanTerm", loanTerm);

                var result = approvedCollection.UpdateOne(filter, update);

                if (result.MatchedCount == 0)
                {
                    MessageBox.Show("No matching loan record found to update.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (result.ModifiedCount == 0)
                {
                    MessageBox.Show("The record was found but not modified.", "Update Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating loan_approved collection: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateLoanDisbursedCollection(BsonDocument loanReleasedDocument)
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var disbursedCollection = database.GetCollection<BsonDocument>("loan_disbursed");

                // ✅ Ensure collector details are available
                string collectorName = cbcollector.Text.Trim();
                string collectorId = tidno.Text.Trim();

                if (string.IsNullOrEmpty(collectorName) || string.IsNullOrEmpty(collectorId))
                {
                    MessageBox.Show("Collector Name or ID is missing. Please select a collector before proceeding.",
                                    "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ✅ Extract LoanNo safely
                string loanNo = loanLNno.Text.Trim();
                if (string.IsNullOrEmpty(loanNo))
                {
                    MessageBox.Show("Loan number is missing. Cannot proceed with update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ✅ Ensure MongoDB is storing the same LoanNo
                var filter = Builders<BsonDocument>.Filter.Eq("LoanNo", loanNo);
                var existingLoan = disbursedCollection.Find(filter).FirstOrDefault();

                if (existingLoan == null)
                {
                    MessageBox.Show($"Loan disbursement record not found for LoanNo: '{loanNo}'",
                                    "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ✅ Extract LoanTerm and LoanMode safely
                string loanTermStr = loanReleasedDocument.GetValue("LoanTerm", "0").ToString();
                int loanTerm = int.Parse(new string(loanTermStr.Where(char.IsDigit).ToArray())); // Extract digits only

                string loanMode = loanReleasedDocument.GetValue("PaymentMode", "").ToString().ToUpper().Trim();

                double loanAmount = loanReleasedDocument.GetValue("LoanAmount", 0.0).ToDouble();
                double interestRate = 0.05; // 5% interest

                // ✅ Calculate amortization period
                int amortizationPeriod = 0;
                switch (loanMode)
                {
                    case "WEEKLY":
                        amortizationPeriod = loanTerm * 4;
                        break;
                    case "DAILY":
                        amortizationPeriod = loanTerm * 20; // 20 days per month
                        break;
                    case "SEMI-MONTHLY":
                        amortizationPeriod = loanTerm * 2;
                        break;
                    case "MONTHLY":
                        amortizationPeriod = loanTerm;
                        break;
                    default:
                        MessageBox.Show($"Invalid Payment Mode: '{loanMode}'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }

                // ✅ Compute total loan with interest
                double totalLoanWithInterest = loanAmount * (1 + interestRate);

                // ✅ Compute correct amortized amount per period
                double correctedAmortizedAmt = totalLoanWithInterest / amortizationPeriod;

                // ✅ Compute the correct total amount to pay (using Math.Floor)
                double totalLoanAmountToPay = Math.Floor(correctedAmortizedAmt * amortizationPeriod);

                // ✅ Debugging (optional - remove after testing)
                MessageBox.Show($"DEBUG:\nLoanTerm: {loanTerm}\nLoanMode: {loanMode}\nLoanAmount: {loanAmount}\n" +
                                $"AmortizationPeriod: {amortizationPeriod}\nTotalLoanWithInterest: {totalLoanWithInterest}\n" +
                                $"CorrectedAmortizedAmt: {correctedAmortizedAmt}\nTotalLoanAmountToPay: {totalLoanAmountToPay}",
                                "Debug Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // ✅ Calculate Maturity Date
                DateTime releaseDate = DateTime.Now;
                DateTime maturityDate = releaseDate.AddMonths(loanTerm);

                var update = Builders<BsonDocument>.Update
                    .Set("LoanStatus", "UPDATED")
                    .Set("CollectorName", collectorName)
                    .Set("CollectorID", collectorId)
                    .Set("ReleasingDate", releaseDate)
                    .Set("MaturityDate", lendpayment.Text)
                    .Set("TotalLoanAmountToPay", totalLoanAmountToPay)
                    .CurrentDate("Date_Modified");

                var result = disbursedCollection.UpdateOne(filter, update);

                if (result.ModifiedCount == 0)
                {
                    MessageBox.Show("The record was found but not modified.", "Update Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating loan_disbursed collection: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private BsonDocument GetLoanApprovedDetails(string clientNumber)
        {
            var database = MongoDBConnection.Instance.Database;
            var approvedCollection = database.GetCollection<BsonDocument>("loan_disbursed");

            var filter = Builders<BsonDocument>.Filter.Eq("ClientNo", clientNumber);
            var loanApprovedDocument = approvedCollection.Find(filter).FirstOrDefault();

            if (loanApprovedDocument == null)
            {
                MessageBox.Show("No loan approved details found for this client number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return new BsonDocument(); // Return an empty document instead of throwing an exception
            }

            // ✅ Determine PaymentMethod
            string paymentMethod = loanApprovedDocument.GetValue("PaymentMethod", "").ToString();

            // ✅ Extract the correct poAmount field based on PaymentMethod
            string poAmountStr = "0";
            switch (paymentMethod)
            {
                case "Disburse Cash":
                    poAmountStr = loanApprovedDocument.GetValue("cashPoAmt", "0").ToString();
                    break;

                case "Disburse Online":
                    poAmountStr = loanApprovedDocument.GetValue("onlinePoAmt", "0").ToString();
                    break;

                case "Disburse Bank Transfer":
                    poAmountStr = loanApprovedDocument.GetValue("bankPoAmt", "0").ToString();
                    break;

                default:
                    MessageBox.Show("Invalid Payment Method in database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return new BsonDocument(); // Return empty document
            }

            // ✅ Clean and parse poAmount
            poAmountStr = poAmountStr.Replace("₱", "").Replace(",", "").Trim();

            if (!decimal.TryParse(poAmountStr, out decimal poAmountValue))
            {
                MessageBox.Show($"Invalid {paymentMethod} poAmount format: {poAmountStr}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new BsonDocument(); // Return empty document if parsing fails
            }

            return loanApprovedDocument;
        }



        private void frm_home_loan_voucher_Load(object sender, EventArgs e)
        {
            LoadCollectors();
            LoadUserInfo(loggedInUsername);
            if (!string.IsNullOrEmpty(clientNo))
            {
                LoadDataToUI(clientNo);
            }
        }

        private void cbcollector_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCollector = cbcollector.SelectedItem as string;

            if (selectedCollector == "--select collector--")
            {
                tarearoute.Text = "N/A";
                tidno.Text = "N/A";
                tdesignation.Text = "N/A";
                tcontact.Text = "N/A";
            }
            else
            {
                LoadCollectorInfo(selectedCollector);
            }
        }

        private void brelease_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do you want to release the disbursement process?",
                "Release Disbursement Process", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    frm_home_disburse disburseForm = new frm_home_disburse();
                    load.Show(this);
                    Thread.Sleep(4000); // Simulating processing delay (consider using async instead)

                    string clientNumber = lclientno.Text;
                    var loanApprovedDetails = GetLoanApprovedDetails(clientNumber);
                    var document = CreateLoanReleasedDocument(loanApprovedDetails);
                    SaveDocumentToCollection(document);

                    string accountId = document.GetValue("LoanNo", "").ToString();
                    double principalAmount = document.GetValue("loanAmt", 0.0).ToDouble();
                    int loanTerm = document.GetValue("loanTerm", 0).ToInt32();

                    UpdateLoanApprovedCollection(principalAmount, loanTerm);
                    UpdateLoanDisbursedCollection(document);
       
                    SaveAccountingEntries(loanLNno.Text);

                    load.Close();

                    MessageBox.Show(this, "Data saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ClearUIFields();

                    // ✅ Temporarily remove the FormClosing event before closing
                    this.FormClosing -= frm_home_loan_voucher_FormClosing;

                    // ✅ Close the form
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvdisburse_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvdisburse.ClearSelection();
        }

        private void bcancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Are you sure you want to abort the releasing process and delete the data?",
                "Abort Releasing Process", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    // Delete the latest disbursed record
                    DeleteLatestDisbursedRecord();

                    // Clear the UI fields after deletion
                    ClearUIFields();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while aborting the process: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void frm_home_loan_voucher_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Confirm with the user before closing the form
            DialogResult result = MessageBox.Show(this, "Are you sure you want to abort the releasing process and delete the latest disbursed data?",
                "Abort Releasing Process", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Delete the latest disbursed record
                    DeleteLatestDisbursedRecord();

                    // Optionally clear the UI fields before closing
                    ClearUIFields();

                    // Allow the form to close
                    e.Cancel = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while aborting the process: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Cancel the form closing if there's an error
                    e.Cancel = true;
                }
            }
            else if (result == DialogResult.Cancel)
            {
                // Cancel the form closing if the user chooses "Cancel"
                e.Cancel = true;
            }
            else
            {
                // Do nothing if the user selects "No", allowing the form to close without any action
                e.Cancel = false;
            }
        }

        private void bvoucher_Click(object sender, EventArgs e)
        {

        }

        private void bsoa_Click(object sender, EventArgs e)
        {

        }

        private void bvoucherclient_Click(object sender, EventArgs e)
        {
            try
            {
                string clientNumber = lclientno.Text;
                var loan_disbursed = GetLoanApprovedDetails(clientNumber);

                if (loan_disbursed == null)
                {
                    MessageBox.Show("No loan details found for this client.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    Title = "Save Disbursement Voucher",
                    FileName = $"Disbursement_Voucher_{clientNumber} - {lclientname.Text}.xlsx"
                };

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                string savePath = saveFileDialog.FileName;
                string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "docs", "cash_voucher.xlsx");

                if (!File.Exists(templatePath))
                {
                    MessageBox.Show($"Error: Template file not found at {templatePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                FileInfo templateFile = new FileInfo(templatePath);
                FileInfo saveFile = new FileInfo(savePath);

                using (ExcelPackage package = new ExcelPackage(templateFile))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    // ✅ Fetch loan details
                    string firstName = loan_disbursed.GetValue("FirstName", "")?.ToString() ?? "";
                    string lastName = loan_disbursed.GetValue("LastName", "")?.ToString() ?? "";
                    string middleName = loan_disbursed.GetValue("MiddleName", "")?.ToString() ?? "";
                    string fullName = $"{firstName} {(string.IsNullOrWhiteSpace(middleName) ? "" : middleName + " ")}{lastName}".Trim();

                    string loanNo = loan_disbursed.GetValue("LoanNo", "")?.ToString() ?? "";
                    string loanType = loan_disbursed.GetValue("LoanType", "")?.ToString() ?? "";
                    string startPaymentDate = loan_disbursed.GetValue("StartPaymentDate", "")?.ToString() ?? "";
                    string loanTerm = loan_disbursed.GetValue("LoanTerm", "")?.ToString() ?? "";
                    string paymentMethod = loan_disbursed.GetValue("PaymentMethod", lpaymentmode.Text)?.ToString() ?? "";
                    string accountId = loan_disbursed.GetValue("AccountId", "")?.ToString() ?? "";

                    // ✅ Determine payment details dynamically
                    string amount = "0", processingFee = "0", poAmount = "0";
                    string receiverName = fullName, paymentPlatform = "UNKNOWN", transactionDate = startPaymentDate;

                    switch (paymentMethod)
                    {
                        case "Disburse Cash":
                            amount = loan_disbursed.GetValue("LoanAmount", "0")?.ToString() ?? "0";
                            processingFee = loan_disbursed.GetValue("cashProFee", "0")?.ToString() ?? "0";
                            poAmount = loan_disbursed.GetValue("cashPoAmt", "0")?.ToString() ?? "0";
                            paymentPlatform = loan_disbursed.GetValue("PaymentMode", "CASH")?.ToString() ?? "CASH";
                            transactionDate = loan_disbursed.GetValue("StartPaymentDate", startPaymentDate)?.ToString() ?? startPaymentDate;
                            break;

                        case "Disburse Online":
                            amount = loan_disbursed.GetValue("LoanAmount", "0")?.ToString() ?? "0";
                            processingFee = loan_disbursed.GetValue("onlineProFee", "0")?.ToString() ?? "0";
                            poAmount = loan_disbursed.GetValue("onlinePoAmt", "0")?.ToString() ?? "0";
                            receiverName = loan_disbursed.GetValue("onlineName", fullName)?.ToString() ?? fullName;
                            paymentPlatform = loan_disbursed.GetValue("PaymentMode", "ONLINE TRANSFER")?.ToString() ?? "ONLINE TRANSFER";
                            transactionDate = loan_disbursed.GetValue("StartPaymentDate", startPaymentDate)?.ToString() ?? startPaymentDate;
                            break;

                        case "Disburse Bank Transfer":
                            amount = loan_disbursed.GetValue("LoanAmount", "0")?.ToString() ?? "0";
                            processingFee = loan_disbursed.GetValue("bankProFee", "0")?.ToString() ?? "0";
                            poAmount = loan_disbursed.GetValue("bankPoAmt", "0")?.ToString() ?? "0";
                            receiverName = loan_disbursed.GetValue("bankName", fullName)?.ToString() ?? fullName;
                            paymentPlatform = loan_disbursed.GetValue("PaymentMode", "BANK TRANSFER")?.ToString() ?? "BANK TRANSFER";
                            transactionDate = loan_disbursed.GetValue("StartPaymentDate", startPaymentDate)?.ToString() ?? startPaymentDate;
                            break;

                        default:
                            MessageBox.Show("Invalid Payment Method.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                    }

                    // ✅ Clean and validate amount format
                    poAmount = poAmount.Replace("₱", "").Replace(",", "").Trim();
                    if (!decimal.TryParse(poAmount, out decimal poAmountValue))
                    {
                        MessageBox.Show($"Error: Invalid poAmount format '{poAmount}'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // ✅ Fill Excel cells dynamically
                    worksheet.Cells["D4"].Value = receiverName;
                    worksheet.Cells["I4"].Value = DateTime.Now.ToString("MM/dd/yyyy");

                    worksheet.Cells["B6"].Value = $"{loanType} with Account No.: {loanNo.Substring(Math.Max(loanNo.Length - 5, 0))}\n" +
                                                   $"{startPaymentDate} - {DateTime.Parse(startPaymentDate).AddMonths(1):MM/dd/yyyy}\n" +
                                                   $"{paymentPlatform} / {loanTerm}\n{amount} starts on {transactionDate}";

                    worksheet.Cells["I3"].Value = $"CV{loanNo.Substring(Math.Max(loanNo.Length - 5, 0))}";
                    worksheet.Cells["G6"].Value = "Loans Receivable\nProcessing Fee\n\nDisbursed Amount";
                    worksheet.Cells["I6"].Value = $"{amount}\n{processingFee}";
                    worksheet.Cells["K6"].Value = $"\n\n\n₱ {poAmount}";

                    worksheet.Cells["E7"].Value = accountId;
                    worksheet.Cells["E8"].Value = receiverName;
                    worksheet.Cells["E9"].Value = "₱ " + poAmount;
                    worksheet.Cells["E10"].Value = lendpayment.Text;

                    worksheet.Cells["I7"].Value = amount;
                    worksheet.Cells["I8"].Value = loanTerm;
                    worksheet.Cells["I9"].Value = paymentPlatform;
                    worksheet.Cells["I10"].Value = transactionDate;

                    // ✅ Convert amount to words
                    worksheet.Cells["F11"].Value = ConvertAmountToWords(poAmountValue);

                    worksheet.Cells["D12"].Value = lcurruser.Text;
                    worksheet.Cells["K12"].Value = "₱ " + poAmount;

                    worksheet.Cells["H16"].Value = receiverName;
                    worksheet.Cells["K16"].Value = DateTime.Now.ToString("MM/dd/yyyy");

                    package.SaveAs(saveFile);
                }

                Process.Start(new ProcessStartInfo(savePath) { UseShellExecute = true });
                MessageBox.Show("Disbursement Voucher generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating disbursement voucher: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public static string ConvertAmountToWords(decimal amount)
        {
            if (amount == 0)
                return "Zero Pesos Only";

            string words = "";
            int integerPart = (int)amount;
            int decimalPart = (int)((amount - integerPart) * 100);

            words = ConvertToWords(integerPart) + " Pesos";

            if (decimalPart > 0)
            {
                words += $" and {ConvertToWords(decimalPart)} Centavos";
            }

            words += " Only";

            return words;
        }

        private static string ConvertToWords(int number)
        {
            if (number == 0)
                return "Zero";

            string[] unitsMap = { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
        "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };

            string[] tensMap = { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += ConvertToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += ConvertToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += ConvertToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words.Trim();
        }



        private void btest_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(clientNo))
            {
                load.Show(this);
                Thread.Sleep(1000);
                LoadDataToUI(clientNo);
                load.Close();
            }
        }
    }
}

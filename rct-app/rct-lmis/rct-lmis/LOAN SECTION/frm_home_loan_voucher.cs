using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace rct_lmis.LOAN_SECTION
{
    public partial class frm_home_loan_voucher : Form
    {
        private string _cashClnNo;

        public frm_home_loan_voucher(string cashClnNo)
        {
            InitializeComponent();
            _cashClnNo = cashClnNo;
            bvoucher.Enabled = false;
        }

        LoadingFunction load = new LoadingFunction();
        private string clientNumber;

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

        // Method to fetch the document
        private BsonDocument FetchDocument(string cashClnNo)
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<BsonDocument>("loan_disbursed");

            var filter = Builders<BsonDocument>.Filter.Eq("cashClnNo", cashClnNo);
            var document = collection.Find(filter).FirstOrDefault();

            return document;
        }

        private double GetAmountInterest(string cashClnNo)
        {
            var database = MongoDBConnection.Instance.Database;
            var loanDisbursedCollection = database.GetCollection<BsonDocument>("loan_disbursed");

            var filter = Builders<BsonDocument>.Filter.Eq("cashClnNo", cashClnNo);
            var document = loanDisbursedCollection.Find(filter).FirstOrDefault();

            if (document != null)
            {
                return document.Contains("AmountInterest") ? ConvertToDouble(document["AmountInterest"]) : 0;
            }
            return 0;
        }

        private string FormatAmortizationPeriod(string loanMode, int term, int days)
        {
            switch (loanMode.ToUpper())
            {
                case "DAILY":
                    // Display the days as-is
                    return $"{days} days";

                case "WEEKLY":
                    // Calculate total weeks, excluding weekends
                    int totalWeeks = GetTotalWeeks(days);
                    return $"{totalWeeks} weeks";

                case "SEMI-MONTHLY":
                    // Calculate the number of semi-monthly cut-offs (2 per month)
                    int semiMonthlyCutOffs = 2 * term;
                    return $"{semiMonthlyCutOffs} cut-offs";

                case "MONTHLY":
                    // Display the term in months
                    return $"{term} months";

                default:
                    return "N/A";
            }
        }

        private void ComputeEndPaymentDate()
        {
            // Step 1: Parse the lstartpayment.Text as a DateTime
            DateTime startDate;
            if (!DateTime.TryParse(lstartpayment.Text, out startDate))
            {
                MessageBox.Show("Invalid start date format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Step 2: Extract the total number of days from lamotperiod.Text
            int totalDays = 0;
            if (!int.TryParse(lamotperiod.Text.Split(' ')[0], out totalDays))
            {
                MessageBox.Show("Invalid amortization period format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Step 3: Compute the end date, excluding weekends (Saturday and Sunday)
            DateTime endDate = ComputeEndDateExcludingWeekends(startDate, totalDays);

            // Step 4: Set the computed end date in lendpayment.Text
            lendpayment.Text = endDate.ToString("MM/dd/yyyy");
        }

        // Helper method to compute the end date excluding weekends
        private DateTime ComputeEndDateExcludingWeekends(DateTime startDate, int totalDays)
        {
            DateTime currentDate = startDate;
            int addedDays = 0;

            while (addedDays < totalDays)
            {
                // Move to the next day
                currentDate = currentDate.AddDays(1);

                // Skip Saturday (DayOfWeek = 6) and Sunday (DayOfWeek = 0)
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    addedDays++; // Only count weekdays
                }
            }

            return currentDate;
        }


        private void LoadDataToUI(string cashClnNo)
        {
            var document = FetchDocument(cashClnNo);

            if (document != null)
            {
                // Fetch and convert data with type checks
                double loanAmount = document.Contains("loanAmt") ? ConvertBsonValueToDouble(document["loanAmt"]) : 0;
                double loanInterestAmt = document.Contains("loanInterestAmt") ? ConvertBsonValueToDouble(document["loanInterestAmt"]) : 0;
                double rfServiceFee = document.Contains("rfServiceFee") ? ConvertBsonValueToDouble(document["rfServiceFee"]) : 0;
                double amortizedAmt = document.Contains("amortizedAmt") ? ConvertBsonValueToDouble(document["amortizedAmt"]) : 0;
                int loanTerm = document.Contains("loanTerm") ? ConvertBsonValueToInt(document["loanTerm"]) : 0;
                string loanMode = document.Contains("Mode") ? document["Mode"].AsString : "N/A";

                // Populate common fields
                loanLNno.Text = document.Contains("LoanIDNo") ? document["LoanIDNo"].AsString : "N/A";
                lloanamt.Text = FormatCurrency(loanAmount);
                lloanterm.Text = loanTerm.ToString() + " months";
                lloaninterest.Text = document.Contains("loanInterest") ? document["loanInterest"].AsString : "N/A";

                // Interpret 'days' field based on the loan mode
                int days = document.Contains("days") ? ConvertBsonValueToInt(document["days"]) : 0;
                lamotperiod.Text = FormatAmortizationPeriod(loanMode, loanTerm, days);

                lloantype.Text = document.Contains("LoanType") ? document["LoanType"].AsString : "N/A";
                lloanmode.Text = loanMode;
                lloanprocessfee.Text = document.Contains("cashProFee") ? FormatCurrency(ConvertBsonValueToDouble(document["cashProFee"])) : "N/A";
                lloannotarialrate.Text = document.Contains("rfNotarialFee") ? FormatCurrency(ConvertBsonValueToDouble(document["rfNotarialFee"])) : "N/A";
                lloaninsurancerate.Text = document.Contains("rfInsuranceFee") ? FormatCurrency(ConvertBsonValueToDouble(document["rfInsuranceFee"])) : "N/A";
                lloanannotationrate.Text = document.Contains("rfAnnotationFee") ? FormatCurrency(ConvertBsonValueToDouble(document["rfAnnotationFee"])) : "N/A";
                lloanVAT.Text = document.Contains("rfVat") ? FormatCurrency(ConvertBsonValueToDouble(document["rfVat"])) : "N/A";
                lloanmisc.Text = document.Contains("rfMisc") ? FormatCurrency(ConvertBsonValueToDouble(document["rfMisc"])) : "N/A";
                ldocrate.Text = document.Contains("rfDocFee") ? FormatCurrency(ConvertBsonValueToDouble(document["rfDocFee"])) : "N/A";
                lclientno.Text = document.Contains("cashClnNo") ? document["cashClnNo"].AsString : "N/A";
                lstartpayment.Text = document.Contains("PaymentStartDate") ? document["PaymentStartDate"].AsString : "N/A";

                // Determine Client Name based on payment method
                if (document.Contains("cashName"))
                {
                    lclientname.Text = document["cashName"].AsString;
                    lpaymentmode.Text = "Cash";
                }
                else
                {
                    lclientname.Text = "N/A";
                }

                // Get AmountInterest from loan_disbursed collection
                double amountInterest = document.Contains("AmountInterest") ? ConvertBsonValueToDouble(document["AmountInterest"]) : 0;

                // Calculate AmountToPay
                double amountToPay = loanAmount + amountInterest;

                ComputeEndPaymentDate();

                // Populate DataGridView
                var dataTable = new DataTable();
                dataTable.Columns.Add("Transaction No.");
                dataTable.Columns.Add("Releasing Date");
                dataTable.Columns.Add("Loan Amount (Principal)");
                dataTable.Columns.Add("Processing Fee");
                dataTable.Columns.Add("Disbursed Amount");
                dataTable.Columns.Add("Amount To Pay");
                dataTable.Columns.Add("Amortized Amount"); // Added column

                // Populate based on payment details
                if (document.Contains("cashNo"))
                {
                    _ = dataTable.Rows.Add(
                        document["cashNo"].AsString,
                        document.Contains("cashDate") ? document["cashDate"].AsString : "N/A",
                        FormatCurrency(loanAmount),
                        FormatCurrency(ConvertBsonValueToDouble(document["cashProFee"])),
                        FormatCurrency(ConvertBsonValueToDouble(document["cashPoAmt"])),
                        FormatCurrency(amountToPay), // Amount To Pay
                        FormatCurrency(amortizedAmt) // Amortized Amount
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
        private double ConvertBsonValueToDouble(BsonValue bsonValue)
        {
            if (bsonValue.IsDouble)
            {
                return bsonValue.AsDouble;
            }
            if (bsonValue.IsString)
            {
                // Remove currency symbols and commas, then parse to double
                string sanitizedString = bsonValue.AsString.Replace("₱", "").Replace(",", "").Trim();
                if (double.TryParse(sanitizedString, out double result))
                {
                    return result;
                }
            }
            return 0;
        }

        // Helper method to convert BSON value to integer
        private int ConvertBsonValueToInt(BsonValue bsonValue)
        {
            if (bsonValue.IsInt32)
            {
                return bsonValue.AsInt32;
            }
            if (bsonValue.IsString)
            {
                if (int.TryParse(bsonValue.AsString, out int result))
                {
                    return result;
                }
            }
            return 0;
        }


        // Helper method to convert days to weeks excluding weekends
        private int GetWeeksFromDays(int days)
        {
            int totalWeeks = days / 7;
            int remainingDays = days % 7;

            // Calculate total number of weekends in full weeks
            int weekendsInFullWeeks = totalWeeks * 2;

            // Calculate weekends in remaining days
            int weekendsInRemainingDays = remainingDays >= 5 ? 2 : remainingDays >= 2 ? 1 : 0;

            // Total number of weeks excluding weekends
            return totalWeeks + (remainingDays >= 7 ? 1 : 0) - (weekendsInFullWeeks + weekendsInRemainingDays) / 7;
        }

        private double ConvertToDouble(BsonValue bsonValue)
        {
            switch (bsonValue.BsonType)
            {
                case BsonType.Double:
                    return bsonValue.AsDouble;
                case BsonType.String:
                    string cleanedString = CleanCurrencyString(bsonValue.AsString);
                    if (double.TryParse(cleanedString, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
                    {
                        return result;
                    }
                    break;
                case BsonType.Int32:
                    return bsonValue.AsInt32;
                case BsonType.Int64:
                    return bsonValue.AsInt64;
                default:
                    throw new InvalidCastException($"Unsupported BsonType: {bsonValue.BsonType}");
            }
            throw new FormatException($"Cannot convert BsonValue to double: {bsonValue}");
        }

        private string CleanCurrencyString(string value)
        {
            // Remove non-numeric characters (e.g., currency symbols) except decimal points and commas
            return Regex.Replace(value, @"[^\d.,-]", "");
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

        private BsonDocument CreateLoanReleasedDocument()
        {
            // Get the current user's full name
            string encoderName = UserSession.Instance.UserName;

            // Get the current date and time
            DateTime currentDateTime = DateTime.Now;

            // Get AmountInterest from loan_disbursed collection
            double amountInterest = GetAmountInterest(lclientno.Text);

            // Calculate AmountToPay
            double loanAmount = ConvertBsonValueToDouble(ExtractNumericValue(lloanamt.Text));
            double amountToPay = loanAmount + amountInterest;

            // Create BsonDocument
            var document = new BsonDocument
              {
                  { "LoanIDNo", loanLNno.Text },
                  { "loanAmt", loanAmount },
                  { "loanTerm", ExtractNumericValue(lloanterm.Text) },
                  { "loanInterest", ExtractNumericValue(lloaninterest.Text) },
                  { "days", ExtractNumericValueDays(lamotperiod.Text) },
                  { "LoanType", lloantype.Text },
                  { "Mode", lloanmode.Text },
                  { "cashProFee", ConvertBsonValueToDouble(ExtractNumericValue(lloanprocessfee.Text)) },
                  { "rfNotarialFee", ConvertBsonValueToDouble(ExtractNumericValue(lloannotarialrate.Text)) },
                  { "rfInsuranceFee", ConvertBsonValueToDouble(ExtractNumericValue(lloaninsurancerate.Text)) },
                  { "rfAnnotationFee", ConvertBsonValueToDouble(ExtractNumericValue(lloanannotationrate.Text)) },
                  { "rfVat", ConvertBsonValueToDouble(ExtractNumericValue(lloanVAT.Text)) },
                  { "rfMisc", ConvertBsonValueToDouble(ExtractNumericValue(lloanmisc.Text)) },
                  { "rfDocFee", ConvertBsonValueToDouble(ExtractNumericValue(ldocrate.Text)) },
                  { "cashClnNo", lclientno.Text },
                  { "payoutDate", DateTime.Parse(lstartpayment.Text) },
                  { "clientName", lclientname.Text },
                  { "paymentMode", lpaymentmode.Text },
                  { "CollectorName", cbcollector.SelectedItem?.ToString() ?? "N/A" },
                  { "AreaRoute", tarearoute.Text },
                  { "IDNo", tidno.Text },
                  { "Designation", tdesignation.Text },
                  { "Contact", tcontact.Text },
                  { "Encoder", encoderName },
                  { "ReleasingDate", currentDateTime },
                  { "AmountToPay", amountToPay } // Add AmountToPay field
              };

            AddPaymentDetails(document);

            return document;
        }



        private void SaveDocumentToCollection(BsonDocument document)
        {
            var database = MongoDBConnection.Instance.Database;
            var releasedCollection = database.GetCollection<BsonDocument>("loan_released");

            // Update the 'days' field
            document["days"] = ExtractNumericValueDays(lloanterm.Text);

            // Get the amortized amount from dgvdisburse
            if (dgvdisburse.DataSource is DataTable dataTable && dataTable.Rows.Count > 0)
            {
                var amortizedAmountObj = dataTable.Rows[0]["Amortized Amount"];
                if (amortizedAmountObj != null)
                {
                    // Convert the amortized amount to decimal and then to BsonValue
                    document["amortizedAmt"] = ConvertBsonValueToDecimal(amortizedAmountObj.ToString());
                }
            }

            // Insert the document into the loan_released collection
            releasedCollection.InsertOne(document);

            MessageBox.Show("Loan released information saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }



        // Helper method to extract numeric value from string (e.g., "80 days" -> 80)
        private int ExtractNumericValueDays(string loanTerm)
        {
            // Remove "days" and any extra spaces, then convert to an integer
            string numericPart = loanTerm.Replace("days", "").Trim();

            // Try to parse the string to an integer
            if (int.TryParse(numericPart, out int result))
            {
                return result;
            }

            // Return 0 if parsing fails
            return 0;
        }


        private int GetTotalWeeks(int days)
        {
            int totalWeeks = 0;
            int daysLeft = days;

            while (daysLeft > 0)
            {
                int weekDays = 0;
                for (int i = 0; i < 7; i++)
                {
                    if (daysLeft > 0)
                    {
                        DateTime currentDate = DateTime.Now.AddDays(i);
                        if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                        {
                            weekDays++;
                            daysLeft--;
                        }
                    }
                }
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
                 { "Amount To Pay", "N/A" }
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

                    }
                }
            }

            document["PaymentDetails"] = paymentDetails;
        }

        // Helper method to extract numeric values from formatted strings
        private double ExtractNumericValue(string formattedValue)
        {
            string cleanedValue = CleanCurrencyString(formattedValue);
            return ConvertToDouble(new BsonString(cleanedValue));
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
            lloannotarialrate.Text = "";
            lloaninsurancerate.Text = "";
            lloanannotationrate.Text = "";
            lloanVAT.Text = "";
            lloanmisc.Text = "";
            ldocrate.Text = "";
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
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_disbursed");

                // Find the latest record based on ObjectId (MongoDB _id field has a timestamp component)
                var filter = Builders<BsonDocument>.Filter.Empty; // No filter to get all records
                var latestRecord = collection.Find(filter)
                                             .Sort(Builders<BsonDocument>.Sort.Descending("_id"))
                                             .FirstOrDefault();  // Get the most recent record

                if (latestRecord != null)
                {
                    // Delete the most recent record
                    var deleteFilter = Builders<BsonDocument>.Filter.Eq("_id", latestRecord["_id"]);
                    collection.DeleteOne(deleteFilter);
                    //MessageBox.Show("The latest disbursed record has been successfully deleted.", "Record Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No disbursed record found to delete.", "No Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting the latest disbursed record: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveVoucherToCollection(BsonDocument document)
        {
            var database = MongoDBConnection.Instance.Database;
            var voucherCollection = database.GetCollection<BsonDocument>("loan_release_voucher");
            var accountsCollection = database.GetCollection<BsonDocument>("user_accounts");

            // Generate CV No.
            string cvNo = GenerateNextCVNo(voucherCollection);

            // Extract necessary fields
            string clientName = document.Contains("cashName") ? document["cashName"].AsString : string.Empty;
            string loanAccountNo = document.Contains("LoanIDNo") ? document["LoanIDNo"].AsString : string.Empty;

            // Fetch amortized amount from the document
            decimal amortization = document.Contains("amortizedAmt") ? ConvertBsonValueToDecimal(document["amortizedAmt"]) : 0;
            decimal loanAmount = document.Contains("loanAmt") ? ConvertBsonValueToDecimal(document["loanAmt"]) : 0;
            int loanTerm = document.Contains("loanTerm") ? ConvertBsonValueToIntTerm(document["loanTerm"]) : 0;
            string loanMode = document.Contains("Mode") ? document["Mode"].AsString : "N/A";
            DateTime paymentStartDate = document.Contains("payoutDate") ? document["payoutDate"].ToUniversalTime() : DateTime.MinValue;

            // Compute the Maturity Date
            DateTime maturityDate = CalculateMaturityDate(paymentStartDate, loanTerm, loanMode);

            // Convert the amount to words
            string amountInWords = ConvertAmountToWords(loanAmount);

            // Get the current user and date
            string preparedBy = UserSession.Instance.UserName;
            DateTime currentDate = DateTime.Now;

            // Fetch Finance Officer and General Manager from user_accounts collection
            var financeOfficer = GetUserByRole(accountsCollection, "Finance Officer");
            var generalManager = GetUserByRole(accountsCollection, "General Manager");

            // Create voucher document
            var voucherDocument = new BsonDocument
             {
                 { "CVNo", cvNo },
                 { "ClientName", clientName },
                 { "LoanAccountNo", loanAccountNo },
                 { "AmountLoanReleased", loanAmount },
                 { "MaturityDate", maturityDate },
                 { "Amortization", amortization },
                 { "LoanTerm", loanTerm },
                 { "PaymentMode", loanMode },
                 { "PaymentStartDate", paymentStartDate },
                 { "PreparedBy", preparedBy },
                 { "AmountInWords", amountInWords },
                 { "ReceivedBy", clientName },
                 { "Date", currentDate },
                 { "FinanceOfficer", financeOfficer ?? string.Empty },
                 { "GeneralManager", generalManager ?? string.Empty }
             };

            // Insert the voucher document into the loan_release_voucher collection
            voucherCollection.InsertOne(voucherDocument);
        }




        // Helper method to convert BSON values to decimal
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


        // Helper method to convert BSON values to int
        private int ConvertBsonValueToIntTerm(BsonValue bsonValue)
        {
            if (bsonValue.IsInt32)
            {
                return bsonValue.AsInt32;
            }
            else if (bsonValue.IsDouble)
            {
                return Convert.ToInt32(bsonValue.AsDouble);
            }
            else if (bsonValue.IsInt64)
            {
                return Convert.ToInt32(bsonValue.AsInt64);
            }
            else
            {
                return 0; // or throw an exception based on your logic
            }
        }

        // Remaining helper methods (e.g., CalculateMaturityDate, ComputeAmortization, etc.) remain unchanged.




        // Helper Method to generate the next CV No.
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

        // Helper Method to calculate Maturity Date
        private DateTime CalculateMaturityDate(DateTime startDate, int loanTerm, string loanMode)
        {
            switch (loanMode.ToUpper())
            {
                case "DAILY":
                    return startDate.AddDays(loanTerm * 30);  // Multiply by 30 for days
                case "WEEKLY":
                    return startDate.AddDays(loanTerm * 7 * 4); // Multiply by 4 weeks
                case "MONTHLY":
                    return startDate.AddMonths(loanTerm); // Add the term as months
                default:
                    return startDate; // Return startDate if loan mode is unrecognized
            }
        }


        // Helper Method to calculate Amortization
        private decimal ComputeAmortization(decimal loanAmount, int loanTerm, string loanMode)
        {
            if (loanTerm <= 0 || loanAmount <= 0) return 0;

            // Example logic for daily, weekly, monthly payments. Adjust accordingly.
            switch (loanMode.ToUpper())
            {
                case "DAILY":
                    return loanAmount / (loanTerm * 30); // Assuming 30 days per month
                case "WEEKLY":
                    return loanAmount / (loanTerm * 4);  // Assuming 4 weeks per month
                case "MONTHLY":
                    return loanAmount / loanTerm;        // Direct monthly amortization
                default:
                    return 0;
            }
        }


        // Helper Method to convert amount to words
        private string ConvertAmountToWords(decimal amount)
        {
           
            return NumberToWordsConverter.ConvertToWords(amount); // Use any library or your own logic
        }

        // Helper Method to get user by role
        private string GetUserByRole(IMongoCollection<BsonDocument> accountsCollection, string role)
        {
            var user = accountsCollection.Find(Builders<BsonDocument>.Filter.Eq("Role", role)).FirstOrDefault();
            return user != null ? user["UserName"].AsString : string.Empty;
        }



        private void frm_home_loan_voucher_Load(object sender, EventArgs e)
        {
            LoadCollectors();
            if (!string.IsNullOrEmpty(_cashClnNo))
            {
                LoadDataToUI(_cashClnNo);


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

                    // Show loading screen
                    load.Show(this);
                    Thread.Sleep(4000);

                    // Create the loan released document
                    var document = CreateLoanReleasedDocument();

                    // Save the loan released document to the loan_released collection
                    SaveDocumentToCollection(document);

                    // Save the voucher to the loan_release_voucher collection using the same document
                    SaveVoucherToCollection(document);

                    // Close the loading screen
                    load.Close();

                    MessageBox.Show(this, "Data saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Enable the voucher button
                    bvoucher.Enabled = true;

                    
                }
                catch (Exception ex)
                {
                    // Handle errors
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
    }
}

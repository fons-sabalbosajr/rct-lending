using MongoDB.Bson;
using MongoDB.Driver;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Data;
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
        private string _cashClnNo;

        public frm_home_loan_voucher(string cashClnNo)
        {
            InitializeComponent();
            _cashClnNo = cashClnNo;
            bvoucheroffice.Enabled = false;
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

        // Method to fetch the document
        private BsonDocument FetchDocument(string cashClnNo)
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<BsonDocument>("loan_disbursed");

            var filter = Builders<BsonDocument>.Filter.Eq("cashClnNo", cashClnNo);
            var document = collection.Find(filter).FirstOrDefault();

            return document;
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

            // Calculate AmountToPay
            double loanAmount = ConvertBsonValueToDouble(ExtractNumericValue(lloanamt.Text));
            
            // Create BsonDocument
            var document = new BsonDocument
              {
                  { "LoanIDNo", loanLNno.Text },
                  { "loanAmt", loanAmount },
                  { "loanTerm", ExtractNumericValue(lloanterm.Text) },
                  { "loanInterest", ExtractNumericValue(lloaninterest.Text) },
                  { "days", lamotperiod.Text },
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
                  { "ReleasingDate", currentDateTime }
                  //{ "AmountToPay", amountToPay }
              };

            AddPaymentDetails(document);

            return document;
        }

        private void SaveDocumentToCollection(BsonDocument document)
        {
            var database = MongoDBConnection.Instance.Database;
            var releasedCollection = database.GetCollection<BsonDocument>("loan_released");

            document["days"] = lamotperiod.Text;
            releasedCollection.InsertOne(document);
        }

        private double ExtractNumericValue(string formattedValue)
        {
            string cleanedValue = CleanCurrencyString(formattedValue);
            return ConvertToDouble(new BsonString(cleanedValue));
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

        private void SaveAccountingEntries(BsonDocument loanReleasedDocument)
        {
            var database = MongoDBConnection.Instance.Database;
            var accountCollection = database.GetCollection<BsonDocument>("loan_account_data");

            // Extract relevant data from the loan released document
            double loanAmount = loanReleasedDocument["loanAmt"].AsDouble;
            double processingFee = loanReleasedDocument["cashProFee"].AsDouble;
            double disbursedAmount = loanAmount - processingFee; // Assuming this calculation is correct

            // Create accounting entries for Amount 1
            var entry1 = new BsonDocument
              {
                  { "AccountTitle", "A120-1" },
                  { "Debit", loanAmount },
                  { "Credit", 0 },
                  { "Reference", loanReleasedDocument["LoanIDNo"].AsString },
                  { "Date", DateTime.Now }
              };
             accountCollection.InsertOne(entry1);

             var entry2 = new BsonDocument
              {
                  { "AccountTitle", "I400-2" },
                  { "Debit", 0 },
                  { "Credit", processingFee },
                  { "Reference", loanReleasedDocument["LoanIDNo"].AsString },
                  { "Date", DateTime.Now }
              };
                      accountCollection.InsertOne(entry2);

             var entry3 = new BsonDocument
              {
                  { "AccountTitle", "A100-4" },
                  { "Debit", 0 },
                  { "Credit", disbursedAmount },
                  { "Reference", loanReleasedDocument["LoanIDNo"].AsString },
                  { "Date", DateTime.Now }
              };
                      accountCollection.InsertOne(entry3);

             var entry4 = new BsonDocument
              {
                  { "AccountTitle", "A120-2" },
                  { "Debit", 0 },
                  { "Credit", 0 }, // Assuming interest to be accounted later
                  { "Reference", loanReleasedDocument["LoanIDNo"].AsString },
                  { "Date", DateTime.Now }
              };
                      accountCollection.InsertOne(entry4);

             var entry5 = new BsonDocument
              {
                  { "AccountTitle", "L200-9" },
                  { "Debit", 0 },
                  { "Credit", 0 }, // Assuming unearned income to be tracked separately
                  { "Reference", loanReleasedDocument["LoanIDNo"].AsString },
                  { "Date", DateTime.Now }
              };
            accountCollection.InsertOne(entry5);
        }



        private void UpdateLoanApprovedCollection(double principalAmount, int loanTerm)
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var approvedCollection = database.GetCollection<BsonDocument>("loan_approved");

                var filter = Builders<BsonDocument>.Filter.Eq("ClientNumber", lclientno.Text);
                var update = Builders<BsonDocument>.Update
                    .Set("LoanStatus", "Loan Released")
                    .Set("PrincipalAmount", principalAmount)
                    .Set("LoanTerm", loanTerm);

                // Apply the update to the loan_approved collection
                var result = approvedCollection.UpdateOne(filter, update);

                // Check if the update was successful
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
                MessageBox.Show($"An error occurred while updating the loan_approved collection: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private BsonDocument GetLoanApprovedDetails(string clientNumber)
        {
            var database = MongoDBConnection.Instance.Database;
            var approvedCollection = database.GetCollection<BsonDocument>("loan_approved");

            // Filter by ClientNumber
            var filter = Builders<BsonDocument>.Filter.Eq("ClientNumber", clientNumber);
            var loanApprovedDocument = approvedCollection.Find(filter).FirstOrDefault();

            if (loanApprovedDocument == null)
            {
                throw new Exception("No loan approved details found for this client number.");
            }

            return loanApprovedDocument;
        }


        private void ExportToExcel(BsonDocument document, BsonDocument loanApprovedDetails, string filePath)
        {
            // Construct the full address
            string fullAddress = $"{loanApprovedDetails["Street"]}, {loanApprovedDetails["Barangay"]}, {loanApprovedDetails["City"]}, {loanApprovedDetails["Province"]}";

            // Get the mobile number
            string mobileNumber = loanApprovedDetails["CP"].ToString();

            // Set the due date (start payment date)
            string dueDate = lstartpayment.Text;  // Use the start payment date from the form
            string amortized = dgvdisburse.Rows[0].Cells[6].Value.ToString();
            string loanamt = dgvdisburse.Rows[0].Cells[2].Value.ToString();
            string profee = dgvdisburse.Rows[0].Cells[3].Value.ToString();
            string loantopay = dgvdisburse.Rows[0].Cells[4].Value.ToString();

            using (ExcelPackage package = new ExcelPackage())
            {
                // Add a worksheet to the workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Cash Receiving Voucher");

                // Set paper size to A4
                worksheet.PrinterSettings.PaperSize = ePaperSize.A4;

                // Insert the image from the Resources folder
                string imagePath = Path.Combine(System.Windows.Forms.Application.StartupPath, "Resources", "rctheader.jpg");
                if (File.Exists(imagePath))
                {
                    // Add the image to the worksheet
                    var picture = worksheet.Drawings.AddPicture("HeaderImage", new FileInfo(imagePath));
                    picture.SetPosition(0, 0, 0, 0);  // Adjust position if needed (row, offset, column, offset)
                    picture.From.Column = 0;  // Starting column
                    picture.From.Row = 0;     // Starting row
                }
                else
                {
                    MessageBox.Show("Image file not found: " + imagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Start filling the worksheet from row 6 (after the image)
                int rowStart = 7;

                rowStart += 2;

                // Add the title "CASH RECEIVING VOUCHER"
                worksheet.Cells[rowStart, 1, rowStart, 7].Merge = true;  // Merge cells for the title
                worksheet.Cells[rowStart, 1].Value = "CASH RECEIVING VOUCHER";
                worksheet.Cells[rowStart, 1].Style.Font.Size = 16;  // Set larger font size for the title
                worksheet.Cells[rowStart, 1].Style.Font.Bold = true;  // Make it bold
                worksheet.Cells[rowStart, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;  // Center the text
                worksheet.Cells[rowStart, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                rowStart += 2;  // Add some space before entering the details

                // Set all other cells to use Arial font, size 9
                worksheet.Cells.Style.Font.Name = "Arial";
                worksheet.Cells.Style.Font.Size = 9;

                // Set the headers and values
                worksheet.Cells[rowStart, 1].Value = "Name of the Borrower:";
                worksheet.Cells[rowStart, 2].Value = document["clientName"].ToString();

                worksheet.Cells[rowStart + 1, 1].Value = "Address:";
                worksheet.Cells[rowStart + 1, 2].Value = fullAddress;  // Use the address from loan_approved

                worksheet.Cells[rowStart + 2, 1].Value = "Mobile:";
                worksheet.Cells[rowStart + 2, 2].Value = mobileNumber;  // Use the mobile number from loan_approved

                worksheet.Cells[rowStart + 4, 1].Value = "Loan Amount:";
                worksheet.Cells[rowStart + 4, 2].Value = loanamt;

                worksheet.Cells[rowStart + 5, 1].Value = "Processing Fee:";
                worksheet.Cells[rowStart + 5, 2].Value = profee;

                worksheet.Cells[rowStart + 6, 1].Value = "Advance Payment (if any):";
                worksheet.Cells[rowStart + 6, 2].Value = "N/A";  // You can update this if you have advance payment info

                worksheet.Cells[rowStart + 7, 1].Value = "Amount to Receive:";
                worksheet.Cells[rowStart + 7, 2].Value = loantopay;

                worksheet.Cells[rowStart + 9, 1].Value = "Daily Amortization:";
                worksheet.Cells[rowStart + 9, 2].Value = amortized;

                worksheet.Cells[rowStart + 10, 1].Value = "Due Date:";
                worksheet.Cells[rowStart + 10, 2].Value = dueDate;  // Set the Due Date as the Start Payment Date

                rowStart += 2;

                // Add a line for the signature
                worksheet.Cells[rowStart + 12, 1].Value = "___________________________________";
                worksheet.Cells[rowStart + 13, 1].Value = "Signature over Printed Name";

                // Auto-fit columns for readability
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Save the Excel file to the chosen path
                File.WriteAllBytes(filePath, package.GetAsByteArray());

                MessageBox.Show($"Excel file saved to {filePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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

                    load.Show(this);
                    Thread.Sleep(4000);

                    // Create and save the loan release document
                    var document = CreateLoanReleasedDocument();
                    SaveDocumentToCollection(document);

                    // Extract necessary fields from the document to update loan_approved
                    string accountId = document["LoanIDNo"].AsString;
                    double principalAmount = document["loanAmt"].ToDouble();
                    int loanTerm = document["loanTerm"].ToInt32();

                    // Update loan_approved collection with LoanStatus, PrincipalAmount, and LoanTerm
                    UpdateLoanApprovedCollection(principalAmount, loanTerm);
                    SaveAccountingEntries(document);
                    load.Close();

                    // Show success message
                    MessageBox.Show(this, "Data saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    bvoucheroffice.Enabled = true;
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
                    //DeleteLatestDisbursedRecord();

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
                    //DeleteLatestDisbursedRecord();

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
                // Retrieve the ClientNumber from the form (lclientno label)
                string clientNumber = lclientno.Text;

                // Fetch the loan approved details based on ClientNumber
                var loanApprovedDetails = GetLoanApprovedDetails(clientNumber);

                // Assuming that the loan release document has already been created or retrieved
                var document = CreateLoanReleasedDocument();

                // Show Save File Dialog to choose where to save the Excel file
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Excel Files|*.xlsx";
                    saveFileDialog.Title = "Save Cash Receiving Voucher";
                    saveFileDialog.FileName = "Cash_Receiving_Voucher.xlsx";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Export the document data to Excel, passing loanApprovedDetails and file path
                        ExportToExcel(document, loanApprovedDetails, saveFileDialog.FileName);

                        // Show success message
                        //MessageBox.Show(this, "Cash Receiving Voucher exported successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors
                MessageBox.Show($"An error occurred while exporting to Excel: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

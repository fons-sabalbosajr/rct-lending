﻿using MongoDB.Bson;
using MongoDB.Driver;
using rct_lmis.ADMIN_SECTION;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rct_lmis.LOAN_SECTION
{
    public partial class frm_home_loan_disburse : Form
    {
        public string AccountID { get; set; }

        private readonly IMongoCollection<BsonDocument> loanRateCollection;
     
        private DataTable dataTable = new DataTable();
        LoadingFunction load = new LoadingFunction();

        public frm_home_loan_disburse()
        {
            InitializeComponent();
            DefaultColor();
            var database = MongoDBConnection.Instance.Database;
            loanRateCollection = database.GetCollection<BsonDocument>("loan_rate");

            gpocash.Enabled = false;
            gpoonline.Enabled = false;
            gpobank.Enabled = false;

            dtpcash.Value = DateTime.Now;
            dtponline.Value = DateTime.Now;
            dtpbank.Value = DateTime.Now;
            dtpayoutdate.Value = DateTime.Now;
        }

        // Constructor with AccountID parameter
        public frm_home_loan_disburse(string accountId) : this()
        {
            this.AccountID = accountId;
        }

        private void frm_home_loan_disburse_Load(object sender, EventArgs e)
        {
            LoadDataToDataGridView();
            tloanaccno.Text = AccountID;
        }

        private string GenerateIncrementId()
        {
            try
            {
                // MongoDB connection and collection
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_disbursed");

                // Define filter and sort to find the highest AccountId
                var sort = Builders<BsonDocument>.Sort.Descending("AccountId");
                var lastRecord = collection.Find(new BsonDocument()).Sort(sort).FirstOrDefault();

                // Default starting ID number
                int nextIdNumber = 1;

                if (lastRecord != null && lastRecord.Contains("AccountId"))
                {
                    string lastAccountId = lastRecord["AccountId"].AsString;
                    var match = System.Text.RegularExpressions.Regex.Match(lastAccountId, @"RCT-2024DB-(\d+)");

                    if (match.Success && int.TryParse(match.Groups[1].Value, out int lastNumber))
                    {
                        nextIdNumber = lastNumber + 1;
                    }
                }

                // Generate new incremented disbursement ID
                string newIncrementId = $"RCT-2024DB-{nextIdNumber:D3}";  // Ensuring 3-digit format
                return newIncrementId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating increment ID: " + ex.Message);
                return "RCT-2024DB-001"; // Fallback ID
            }
        }


        private void FillUpCashGroupBox()
        {
            try
            {
                // Generate increment ID
                tcashno.Text = GenerateIncrementId();
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_approved");

                var filter = Builders<BsonDocument>.Filter.Eq("AccountId", tloanaccno.Text);
                var loanRecord = collection.Find(filter).FirstOrDefault();

                if (loanRecord != null)
                {
                    tcashclnno.Text = loanRecord.Contains("ClientNo") ? loanRecord["ClientNo"].AsString : string.Empty;
                    tcashname.Text = loanRecord.Contains("FirstName") && loanRecord.Contains("LastName")
                        ? $"{loanRecord["FirstName"].AsString} {loanRecord["LastName"].AsString}"
                        : string.Empty;

                    // Get processing fee from trfservicefee.Text
                    double processingFee;
                    if (!double.TryParse(trfservicefee.Text.Replace("₱", "").Replace(",", "").Trim(), out processingFee))
                    {
                        MessageBox.Show("Invalid processing fee value.");
                        return;
                    }
                    tcashprofee.Text = "₱ " + processingFee.ToString("N2");

                    // Calculate the cash amount
                    double principal;
                    if (!double.TryParse(tloanamt.Text.Replace("₱", "").Replace(",", "").Trim(), out principal))
                    {
                        MessageBox.Show("Invalid principal amount value.");
                        return;
                    }
                    tcashamt.Text = "₱ " + principal.ToString("N2");
                    tcashpoamt.Text = "₱ " + (principal - processingFee).ToString("N2");
                }
                else
                {
                    MessageBox.Show("Loan record not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filling up Cash GroupBox: " + ex.Message);
            }
        }

        private string GenerateIncrementIdOnline()
        {
            try
            {
                // MongoDB connection and collection
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_disbursed");

                // Define filter and sort to find the highest current ID with the "OLP" prefix
                var sort = Builders<BsonDocument>.Sort.Descending("cashNo");
                var filter = Builders<BsonDocument>.Filter.Regex("cashNo", new BsonRegularExpression("^RCT-DB-OLP-"));
                var lastRecord = collection.Find(filter).Sort(sort).FirstOrDefault();

                // Increment ID logic
                int nextIdNumber = 1;
                if (lastRecord != null && lastRecord.Contains("cashNo"))
                {
                    string lastId = lastRecord["cashNo"].AsString;
                    int lastIdNumber = int.Parse(lastId.Substring(lastId.LastIndexOf('-') + 1));
                    nextIdNumber = lastIdNumber + 1;
                }

                // Generate new increment ID
                string newIncrementId = $"RCT-DB-OLP-{nextIdNumber:D5}";
                return newIncrementId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating increment ID: " + ex.Message);
                return "RCT-DB-OLP-00001"; // Fallback ID
            }
        }
        private string GenerateIncrementIdBank()
        {
            try
            {
                // MongoDB connection and collection
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_disbursed");

                // Define filter and sort to find the highest current ID
                var sort = Builders<BsonDocument>.Sort.Descending("cashNo");
                var lastRecord = collection.Find(new BsonDocument()).Sort(sort).FirstOrDefault();

                // Increment ID logic
                int nextIdNumber = 1;
                if (lastRecord != null && lastRecord.Contains("cashNo"))
                {
                    string lastId = lastRecord["cashNo"].AsString;
                    int lastIdNumber = int.Parse(lastId.Substring(lastId.LastIndexOf('-') + 1));
                    nextIdNumber = lastIdNumber + 1;
                }

                // Generate new increment ID
                string newIncrementId = $"RCT-DB-BNK-{nextIdNumber:D5}";
                return newIncrementId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating increment ID: " + ex.Message);
                return "RCT-DB-BNK-00001"; // Fallback ID
            }
        }

        private void FillUpOnlineGroupBoxOnline()
        {
            try
            {
                // Set cashout platform
                cbonlineplatform.Text = "select platform";

                // Generate increment ID
                tponlinerefno.Text = GenerateIncrementId();

                // Fetch client number and name based on AccountID
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_approved");

                var filter = Builders<BsonDocument>.Filter.Eq("AccountId", tloanaccno.Text);
                var loanRecord = collection.Find(filter).FirstOrDefault();

                if (loanRecord != null)
                {
                    tponlineaccno.Text = loanRecord.Contains("ClientNo") ? loanRecord["ClientNo"].AsString : string.Empty;
                    tponlinename.Text = loanRecord.Contains("FirstName") && loanRecord.Contains("LastName")
                        ? $"{loanRecord["FirstName"].AsString} {loanRecord["LastName"].AsString}"
                        : string.Empty;

                    // Get processing fee from trfservicefee.Text
                    double processingFee;
                    if (!double.TryParse(trfservicefee.Text.Replace("₱", "").Replace(",", "").Trim(), out processingFee))
                    {
                        MessageBox.Show("Invalid processing fee value.");
                        return;
                    }

                    // Calculate the cash amount
                    double principal = double.Parse(tloanamt.Text.Replace("₱", "").Replace(",", "").Trim());
                    tponlineamt.Text = "₱ " + principal.ToString("N2");
                    tonlineprofee.Text = "₱ " + processingFee.ToString("N2");
                    tonlinepoamt.Text = "₱ " + (principal - processingFee).ToString("N2");
                }
                else
                {
                    MessageBox.Show("Loan record not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filling up Online GroupBox: " + ex.Message);
            }
        }

        private void FillUpBankGroupBoxBank()
        {
            try
            {
                // Set bank type platform
                cbbankplatform.Text = "Specify the bank type";

                // Generate increment ID
                tbankporefno.Text = GenerateIncrementId();

                // Fetch client number and name based on AccountID
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_approved");

                var filter = Builders<BsonDocument>.Filter.Eq("AccountId", tloanaccno.Text);
                var loanRecord = collection.Find(filter).FirstOrDefault();

                if (loanRecord != null)
                {
                    tbankpoaccno.Text = loanRecord.Contains("ClientNo") ? loanRecord["ClientNo"].AsString : string.Empty;
                    tbankname.Text = loanRecord.Contains("FirstName") && loanRecord.Contains("LastName")
                        ? $"{loanRecord["FirstName"].AsString} {loanRecord["LastName"].AsString}"
                        : string.Empty;

                    // Get processing fee from trfservicefee.Text
                    double processingFee;
                    if (!double.TryParse(trfservicefee.Text.Replace("₱", "").Replace(",", "").Trim(), out processingFee))
                    {
                        MessageBox.Show("Invalid processing fee value.");
                        return;
                    }

                    // Calculate the cash amount
                    double principal = double.Parse(tloanamt.Text.Replace("₱", "").Replace(",", "").Trim());
                    tbankpoamt.Text = "₱ " + principal.ToString("N2");
                    tbankpoprofee.Text = "₱ " + processingFee.ToString("N2");
                    tbankamt.Text = "₱ " + (principal - processingFee).ToString("N2");
                }
                else
                {
                    MessageBox.Show("Loan record not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filling up Bank GroupBox: " + ex.Message);
            }
        }

        private void LoadDataToDataGridView()
        {
            dataTable.Clear();

            var filter = Builders<BsonDocument>.Filter.Empty;
            var documents = loanRateCollection.Find(filter).ToList();

            string[] displayColumns = { "Term", "Principal", "Type", "Mode", "Interest Rate/Month" };

            foreach (string column in displayColumns)
            {
                if (!dataTable.Columns.Contains(column))
                    dataTable.Columns.Add(column);
            }
            dataTable.Columns.Add("FullDocument", typeof(BsonDocument));

            foreach (var doc in documents)
            {
                DataRow row = dataTable.NewRow();
                foreach (string column in displayColumns)
                {
                    if (doc.Contains(column))
                    {
                        var element = doc[column];
                        if (element.IsNumeric())
                        {
                            if (column == "Principal")
                            {
                                row[column] = "₱ " + Math.Round(element.ToDouble(), 0).ToString();
                            }
                            else if (column == "Interest Rate/Month")
                            {
                                row[column] = Math.Round(element.ToDouble(), 2) + "%"; // Add percentage symbol
                            }
                            else
                            {
                                row[column] = Math.Round(element.ToDouble(), 0);
                            }
                        }
                        else
                        {
                            if (column == "Term")
                            {
                                int termValue = int.Parse(element.ToString());
                                row[column] = termValue + (termValue == 1 ? " month" : " months"); // Add "month" or "months"
                            }
                            else
                            {
                                row[column] = element.ToString();
                            }
                        }
                    }
                }
                row["FullDocument"] = doc;
                dataTable.Rows.Add(row);
            }

            dgvloandata.DataSource = dataTable;
            if (dgvloandata.Columns.Count > 0)
                dgvloandata.Columns["FullDocument"].Visible = false;

            foreach (DataGridViewColumn column in dgvloandata.Columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                if (column.Name == "Principal" || column.Name == "Interest Rate/Month")
                {
                    column.DefaultCellStyle.Format = "N2";
                }
            }
        }

        private void ComputeAmortization(string loanMode)
        {
            try
            {
                // Check if loan term is empty, return without computing
                if (string.IsNullOrWhiteSpace(tloanterm.Text))
                {
                    return;
                }

                // Parse and validate loan amount
                string loanAmtText = tloanamt.Text.Replace("₱", "").Replace(",", "").Trim();
                if (!double.TryParse(loanAmtText, out double principal) || principal <= 0)
                {
                    MessageBox.Show("Invalid loan amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Parse and validate loan term
                if (!int.TryParse(tloanterm.Text.Trim(), out int term) || term <= 0)
                {
                    MessageBox.Show("Invalid loan term format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Parse user-defined interest amount
                string loanInterestText = tloaninterest.Text.Replace("%", "").Trim();
                if (!double.TryParse(loanInterestText, out double userInterestRate) || userInterestRate < 0)
                {
                    MessageBox.Show("Invalid interest rate.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Convert interest from percentage to decimal (e.g., 5% → 0.05)
                double interestRate = userInterestRate / 100;

                // Compute total interest amount for the entire term
                double totalInterest = principal * interestRate * term;

                // Determine the number of payments based on the mode
                int totalPayments;
                switch (loanMode.ToUpper())
                {
                    case "DAILY":
                        totalPayments = GetBusinessDaysInMonths(term);
                        break;

                    case "WEEKLY":
                        totalPayments = 4 * term; // 4 weeks per month
                        break;

                    case "SEMI-MONTHLY":
                        totalPayments = 2 * term; // 1st and 15th payments
                        break;

                    case "MONTHLY":
                        totalPayments = term; // 1 payment per month
                        break;

                    default:
                        MessageBox.Show("Invalid loan mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }

                // Display the total number of payments
                tdays.Text = totalPayments.ToString();

                // Compute the processing fee (e.g., 2% of loan amount)
                double processingFee = principal * 0.02;
                trfservicefee.Text = processingFee.ToString("N2");

                // Calculate amortized amount including interest
                double amortizedAmount = (principal + totalInterest) / totalPayments;

                // Display results
                tamortizedamt.Text = amortizedAmount.ToString("N2");
                tloaninterestamt.Text = totalInterest.ToString("N2");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error computing amortization: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
        // Compute amortization based on collection interest rate changes
        private void ComputeAmortizationInterest()
        {
            try
            {
                // Ensure loan term and loan amount are provided
                if (string.IsNullOrWhiteSpace(tloanterm.Text) || string.IsNullOrWhiteSpace(tloanamt.Text))
                {
                    return;
                }

                // Parse and validate loan amount
                string loanAmtText = tloanamt.Text.Replace("₱", "").Replace(",", "").Trim();
                if (!double.TryParse(loanAmtText, out double principal) || principal <= 0)
                {
                    return;
                }

                // Parse and validate loan term
                if (!int.TryParse(tloanterm.Text.Trim(), out int term) || term <= 0)
                {
                    return;
                }

                // Parse the interest rate from user input (tcolinterest)
                string colInterestText = tloaninterest.Text.Replace("%", "").Trim();
                if (!double.TryParse(colInterestText, out double userInterestRate) || userInterestRate < 0)
                {
                    return;
                }

                // Convert percentage to decimal (e.g., 5% → 0.05)
                double interestRate = userInterestRate / 100;

                // Compute total interest for the entire loan term
                double totalInterest = principal * interestRate * term;

                // Determine the loan mode (defaults to "MONTHLY" if empty)
                string loanMode = string.IsNullOrEmpty(lloanmode.Text) ? "MONTHLY" : lloanmode.Text.ToUpper();
                int totalPayments;

                switch (loanMode)
                {
                    case "DAILY":
                        totalPayments = GetBusinessDaysInMonths(term);
                        break;
                    case "WEEKLY":
                        totalPayments = 4 * term;
                        break;
                    case "SEMI-MONTHLY":
                        totalPayments = 2 * term;
                        break;
                    case "MONTHLY":
                        totalPayments = term;
                        break;
                    default:
                        return; // Invalid loan mode
                }

                // Display the total number of payments
                tdays.Text = totalPayments.ToString();

                // Compute processing fee dynamically (e.g., 2% of loan amount)
                double processingFee = principal * 0.02;
                trfservicefee.Text = processingFee.ToString("N2");

                // Compute the amortized amount including total interest
                double amortizedAmount = (principal + totalInterest) / totalPayments;

                // Display results in UI
                tamortizedamt.Text = amortizedAmount.ToString("N2");
                tloaninterestamt.Text = totalInterest.ToString("N2");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error computing amortization: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private string GetClientNumber(IMongoCollection<BsonDocument> loanApprovedCollection, string accountId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("AccountId", accountId);
            var document = loanApprovedCollection.Find(filter).FirstOrDefault();

            if (document != null && document.Contains("ClientNo"))
            {
                return document["ClientNo"].AsString;
            }

            return null; // Return null if not found or if field is missing
        }

        private void DisbursedInitial()
        {
            var database = MongoDBConnection.Instance.Database;
            var loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");
            var loanDisbursedCollection = database.GetCollection<BsonDocument>("loan_disbursed");
            var loanRateCollection = database.GetCollection<BsonDocument>("loan_rate");

            string accountId = tloanaccno.Text;
            string clientNumber = GetClientNumber(loanApprovedCollection, accountId);

            if (clientNumber == null)
            {
                MessageBox.Show("Client Number could not be found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ✅ Ensure LoanNo is unique and sequential
            var lastLoanDisbursedDocs = loanDisbursedCollection.Find(new BsonDocument())
                .Project(Builders<BsonDocument>.Projection.Include("LoanNo").Exclude("_id"))
                .ToList();

            int highestLoanNumber = 11080; // Default start value
            string highestLoanNo = "";

            foreach (var doc in lastLoanDisbursedDocs)
            {
                if (doc.Contains("LoanNo"))
                {
                    string loanNo = doc["LoanNo"].AsString;
                    var match = System.Text.RegularExpressions.Regex.Match(loanNo, @"RCT-2024-(\d+)");
                    if (match.Success && int.TryParse(match.Groups[1].Value, out int loanNum))
                    {
                        if (loanNum > highestLoanNumber)
                        {
                            highestLoanNumber = loanNum;
                            highestLoanNo = loanNo;
                        }
                    }
                }
            }

            highestLoanNumber++;
            string newLoanNo = $"RCT-2024-{highestLoanNumber}";

            // ✅ Ensure AccountId is unique and sequential
            var lastAccountDisbursed = loanDisbursedCollection.Find(new BsonDocument())
                .Sort(Builders<BsonDocument>.Sort.Descending("AccountId"))
                .Limit(1)
                .FirstOrDefault();

            int lastAccountNumber = 977; // Default start

            if (lastAccountDisbursed != null && lastAccountDisbursed.Contains("AccountId"))
            {
                string lastAccountId = lastAccountDisbursed["AccountId"].AsString;
                var match = System.Text.RegularExpressions.Regex.Match(lastAccountId, @"RCT-2024DB-(\d+)");
                if (match.Success && int.TryParse(match.Groups[1].Value, out int lastNumber))
                {
                    lastAccountNumber = lastNumber + 1;
                }
            }

            string newAccountId = $"RCT-2024DB-{lastAccountNumber}";

            // ✅ Prevent Duplicate Disbursement
            var existingFilter = Builders<BsonDocument>.Filter.Eq("ClientNo", clientNumber);
            var existingDisbursed = loanDisbursedCollection.Find(existingFilter).FirstOrDefault();

            if (existingDisbursed != null)
            {
                MessageBox.Show($"Loan already exists for this client:\n" +
                                $"LoanNo: {existingDisbursed["LoanNo"].AsString}\n" +
                                $"AccountId: {existingDisbursed["AccountId"].AsString}",
                                "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Parse Loan Amount
            double loanAmount = ParseCurrency(tloanamt.Text);
            if (loanAmount == 0)
            {
                MessageBox.Show("Invalid loan amount format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ✅ Parse Loan Interest
            double loanInterest = ParsePercentage(tloaninterest.Text);
            if (loanInterest == 0 && !string.IsNullOrWhiteSpace(tloaninterest.Text))
            {
                MessageBox.Show("Invalid loan interest format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ✅ Parse Loan Term
            int loanTerm;
            if (!int.TryParse(tloanterm.Text, out loanTerm))
            {
                MessageBox.Show("Invalid loan term format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ✅ Calculate Interest Amount
            double amountInterest = loanAmount * (loanInterest / 100) * loanTerm;

            // ✅ Get Payment Mode
            string selectedMode = GetSelectedMode();
            if (selectedMode == null)
            {
                MessageBox.Show("Please select a payment mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int totalDays = GetTotalDaysBasedOnMode(selectedMode, loanTerm);
            tdays.Text = totalDays.ToString();

            // ✅ Get Loan Rate Data
            var loanRateFilter = Builders<BsonDocument>.Filter.Eq("Principal", loanAmount);
            var loanRateDocument = loanRateCollection.Find(loanRateFilter).FirstOrDefault();

            if (loanRateDocument == null)
            {
                MessageBox.Show("Loan Rate data could not be found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ✅ Get Client Details
            var loanApprovedFilter = Builders<BsonDocument>.Filter.Eq("AccountId", accountId);
            var loanApprovedDoc = loanApprovedCollection.Find(loanApprovedFilter).FirstOrDefault();
            if (loanApprovedDoc == null)
            {
                MessageBox.Show("Loan approval record not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ✅ Construct Document for Disbursement
            var loanDisburseDocument = new BsonDocument
            {
                { "AccountId", newAccountId },
                { "LoanNo", newLoanNo },
                { "ClientNo", clientNumber },
                { "LoanType", loanApprovedDoc["LoanType"].AsString },
                { "LoanStatus", "Loan Released" },
                { "LastName", loanApprovedDoc["LastName"].AsString },
                { "FirstName", loanApprovedDoc["FirstName"].AsString },
                { "MiddleName", loanApprovedDoc["MiddleName"].AsString },
                { "CollectorName", loanApprovedDoc["CollectorName"].AsString },
                { "Barangay", loanApprovedDoc.Contains("Barangay") ? loanApprovedDoc["Barangay"].AsString : "" },
                { "City", loanApprovedDoc.Contains("City") ? loanApprovedDoc["City"].AsString : "" },
                { "Province", loanApprovedDoc.Contains("Province") ? loanApprovedDoc["Province"].AsString : "" },
                { "LoanTerm", $"{loanTerm} months" },
                { "LoanAmount", $"₱{loanAmount:N2}" },
                { "LoanAmortization", tamortizedamt.Text },
                { "LoanInterest", $"{loanInterest}%" },
                { "PaymentMode", selectedMode },
                { "StartPaymentDate", dtpayoutdate.Value.ToString("MM/dd/yyyy") },
                { "Date_Encoded", DateTime.UtcNow.ToString("MM/dd/yyyy") },
                { "LoanProcessStatus", "Loan Released" },
                { "Date_Modified", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss") },
                { "PrincipalAmount", $"₱{loanAmount:N2}" }
            };

            // ✅ Handle Payment Method
            if (cbpocash.Checked) AddCashPaymentFields(loanDisburseDocument);
            else if (cbpoonline.Checked) AddOnlinePaymentFields(loanDisburseDocument);
            else if (cbpobank.Checked) AddBankPaymentFields(loanDisburseDocument);
            else
            {
                MessageBox.Show("Please select a payment method.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ✅ Insert into `loan_disbursed`
            loanDisbursedCollection.InsertOne(loanDisburseDocument);

            MessageBox.Show("Loan successfully disbursed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            frm_home_loan_voucher voucherForm = new frm_home_loan_voucher(clientNumber);
            voucherForm.Show();
            ClearAll();
        }

        private double ParseCurrency(string currencyText)
        {
            if (string.IsNullOrWhiteSpace(currencyText)) return 0;

            // Remove currency symbols and commas, then parse
            currencyText = currencyText.Replace("₱", "").Replace(",", "").Trim();

            if (double.TryParse(currencyText, NumberStyles.Number, CultureInfo.InvariantCulture, out double result))
            {
                return result;
            }

            return 0; // Return 0 if parsing fails
        }

        private double ParsePercentage(string percentageText)
        {
            if (string.IsNullOrWhiteSpace(percentageText)) return 0;

            // Remove percentage sign and trim spaces
            percentageText = percentageText.Replace("%", "").Trim();

            if (double.TryParse(percentageText, NumberStyles.Number, CultureInfo.InvariantCulture, out double result))
            {
                return result;
            }

            return 0; // Return 0 if parsing fails
        }

        private string GetSelectedMode()
        {
            if (dgvloandata.SelectedRows.Count > 0)
            {
                var selectedRow = dgvloandata.SelectedRows[0];
                return selectedRow.Cells["Mode"].Value.ToString();
            }
            return null;
        }

        private int GetTotalDaysBasedOnMode(string mode, int term)
        {
            int totalDays = 0;

            switch (mode.ToUpper())
            {
                case "DAILY":
                    // Calculate total days excluding weekends
                    totalDays = GetBusinessDaysInMonths(term);
                    break;

                case "WEEKLY":
                    // Convert term in months to total weeks (4 weeks per month)
                    totalDays = term * 4 * 5; // 4 weeks per month, 7 days per week
                    break;

                case "SEMI-MONTHLY":
                    // Calculate total semi-monthly periods (2 periods per month)
                    totalDays = term * 2 * 15; // 2 periods per month, approx. 15 days each
                    break;

                case "MONTHLY":
                    // Total months = total days (1 payment per month)
                    totalDays = term * 30; // Approx. 30 days per month
                    break;

                default:
                    throw new ArgumentException("Invalid mode");
            }

            return totalDays;
        }

        // Helper method to calculate business days in a given number of months
        private int GetBusinessDaysInMonths(int months)
        {
            // Assuming 5 weekdays per week
            int weekdaysPerWeek = 5;
            int weeksPerMonth = 4;
            int totalBusinessDays = months * weeksPerMonth * weekdaysPerWeek;

            return totalBusinessDays;
        }

        private string GetLoanType(string clientNumber)
        {
            var loanCollections = MongoDBConnection.Instance.Database.GetCollection<BsonDocument>("loan_collections");
            var filter = Builders<BsonDocument>.Filter.Eq("ClientNo", clientNumber);
            var document = loanCollections.Find(filter).FirstOrDefault();

            if (document != null)
            {
                int latePayments = (int)document.GetValue("LatePayments", 0);
                if (latePayments >= 5)
                    return "Irregular Borrower";
                else
                    return "Regular Borrower";
            }

            return "First Time Borrower"; // Default if no records found
        }

        private void AddCashPaymentFields(BsonDocument document)
        {
            document.Add("cashNo", tcashno.Text);
            document.Add("cashName", tcashname.Text);
            document.Add("cashProFee", tcashprofee.Text);
            document.Add("cashAmt", tcashamt.Text);
            document.Add("cashPoAmt", tcashpoamt.Text);
            document.Add("cashDate", dtpcash.Value.ToString("MM/dd/yyyy"));
            document.Add("PaymentMethod", "Disburse Cash");

            // Remove irrelevant fields for cash payment
            RemoveIrrelevantFields(document, "cash");
        }

        private void AddOnlinePaymentFields(BsonDocument document)
        {
            document.Add("onlinePlatform", cbonlineplatform.Text);
            document.Add("onlineRefNo", tponlinerefno.Text);
            document.Add("onlineAccNo", tponlineaccno.Text);
            document.Add("onlineName", tponlinename.Text);
            document.Add("onlineAmt", tponlineamt.Text);
            document.Add("onlineProFee", tonlineprofee.Text);
            document.Add("onlinePoAmt", tonlinepoamt.Text);
            document.Add("onlineDate", dtponline.Value.ToString("MM/dd/yyyy"));
            document.Add("PaymentMethod", "Disburse Online");
            // Remove irrelevant fields for online payment
            RemoveIrrelevantFields(document, "online");
        }

        private void AddBankPaymentFields(BsonDocument document)
        {
            document.Add("bankPlatform", cbbankplatform.Text);
            document.Add("bankRefNo", tbankporefno.Text);
            document.Add("bankAccNo", tbankpoaccno.Text);
            document.Add("bankName", tbankname.Text);
            document.Add("bankPoAmt", tbankpoamt.Text);
            document.Add("bankProFee", tbankpoprofee.Text);
            document.Add("bankAmt", tbankamt.Text);
            document.Add("bankDate", dtpbank.Value.ToString("MM/dd/yyyy hh:mm tt"));
            document.Add("PaymentMethod", "Disburse Bank Transfer");
            // Remove irrelevant fields for bank payment
            RemoveIrrelevantFields(document, "bank");
        }

        private void RemoveIrrelevantFields(BsonDocument document, string paymentType)
        {
            if (paymentType != "cash")
            {
                document.Remove("cashNo");
                document.Remove("cashProFee");
                document.Remove("cashAmt");
                document.Remove("cashPoAmt");
                document.Remove("cashDate");
            }

            if (paymentType != "online")
            {
                document.Remove("onlinePlatform");
                document.Remove("onlineRefNo");
                document.Remove("onlineAccNo");
                document.Remove("onlineName");
                document.Remove("onlineAmt");
                document.Remove("onlineProFee");
                document.Remove("onlinePoAmt");
                document.Remove("onlineDate");
            }

            if (paymentType != "bank")
            {
                document.Remove("bankPlatform");
                document.Remove("bankRefNo");
                document.Remove("bankAccNo");
                document.Remove("bankName");
                document.Remove("bankPoAmt");
                document.Remove("bankProFee");
                document.Remove("bankAmt");
                document.Remove("bankDate");
            }
        }

        private string GenerateNewLoanID()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_disbursed");

                // Find the highest existing LoanNo with the correct format
                var lastLoanDocument = collection.Find(new BsonDocument())
                                                 .Sort(Builders<BsonDocument>.Sort.Descending("LoanNo"))
                                                 .Limit(1)
                                                 .FirstOrDefault();

                string newLoanID;
                int currentYear = DateTime.Now.Year;

                if (lastLoanDocument != null && lastLoanDocument.Contains("LoanNo"))
                {
                    string lastLoanNo = lastLoanDocument["LoanNo"].AsString;

                    // Extract last numeric part (XXXXX) from format RCT-YYYY-XXXXX
                    var parts = lastLoanNo.Split('-');
                    if (parts.Length == 3 && int.TryParse(parts[2], out int lastNumber))
                    {
                        newLoanID = $"RCT-{currentYear}-{(lastNumber + 1)}";
                    }
                    else
                    {
                        // If parsing fails, start fresh with 10001
                        newLoanID = $"RCT-{currentYear}-10001";
                    }
                }
                else
                {
                    // First entry, start with 10001
                    newLoanID = $"RCT-{currentYear}-10001";
                }

                return newLoanID;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating Loan ID: " + ex.Message);
                return string.Empty;
            }
        }


        private void ClearAll() 
        {
            // Clear the textboxes when unchecked
            tcashno.Text = string.Empty;
            tcashprofee.Text = string.Empty;
            tcashamt.Text = string.Empty;
            tcashpoamt.Text = string.Empty;
            tcashclnno.Text = string.Empty;
            tcashname.Text = string.Empty;

            // Disable the cash group box
            gpocash.Enabled = false;


            // Clear all text boxes in the online group box
            cbonlineplatform.Text = string.Empty;
            tponlinerefno.Text = string.Empty;
            tponlineaccno.Text = string.Empty;
            tponlinename.Text = string.Empty;
            tponlineamt.Text = string.Empty;
            tonlineprofee.Text = string.Empty;
            tonlinepoamt.Text = string.Empty;

            // Disable the online group box
            gpoonline.Enabled = false;


            cbbankplatform.Text = string.Empty;
            tbankporefno.Text = string.Empty;
            tbankpoaccno.Text = string.Empty;
            tbankname.Text = string.Empty;
            tbankpoamt.Text = string.Empty;
            tbankpoprofee.Text = string.Empty;
            tbankamt.Text = string.Empty;

            // Disable the bank group box
            gpobank.Enabled = false;

            tloanamt.Text = string.Empty;
            tloanterm.Text = string.Empty;
            tloaninterest.Text = string.Empty;
            tdays.Text = string.Empty;
            tloaninterestamt.Text = string.Empty;
            trfservicefee.Text = string.Empty;
            trfnotarialfee.Text = string.Empty;
            trfinsurancefee.Text = string.Empty;
            trfannotationfee.Text = string.Empty;
            trfvat.Text = string.Empty;
            trfmisc.Text = string.Empty;
            trfdocfee.Text = string.Empty;
            trfnotarialamt.Text = string.Empty;
            trfinsuranceamt.Text = string.Empty;
            trfannotationmt.Text = string.Empty;
            trfvatamt.Text = string.Empty;
            trfmiscamt.Text = string.Empty;
            trfdocamt.Text = string.Empty;
            tamortizedamt.Text = string.Empty;

        }

        private void tsearchamt_TextChanged(object sender, EventArgs e)
        {
            string searchText = tsearchamt.Text.Trim();

            try
            {
                if (!string.IsNullOrEmpty(searchText))
                {
                    string filterExpression = string.Format(
                        "Term LIKE '%{0}%' OR Principal LIKE '%{0}%' OR [Interest Rate/Month] LIKE '%{0}%' OR Type LIKE '%{0}%' OR Mode LIKE '%{0}%'",
                        searchText.Replace("'", "''")); // Replace single quotes to avoid SQL-like errors

                    DataView dv = new DataView(dataTable);
                    dv.RowFilter = filterExpression;
                    dgvloandata.DataSource = dv;
                }
                else
                {
                    dgvloandata.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error applying search filter: " + ex.Message);
            }
        }

        private void dgvloandata_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row index is selected
            {
                DataGridViewRow selectedRow = dgvloandata.Rows[e.RowIndex];
                if (selectedRow != null && selectedRow.Cells["FullDocument"].Value != null)
                {
                    BsonDocument fullDocument = selectedRow.Cells["FullDocument"].Value as BsonDocument;

                    if (fullDocument != null)
                    {
                        try
                        {
                            tloanamt.Text = fullDocument.Contains("Principal") ? "₱ " + fullDocument["Principal"].ToString() + ".00" : string.Empty;
                            tloanterm.Text = fullDocument.Contains("Term") ? fullDocument["Term"].ToString() : string.Empty;
                            tloaninterest.Text = fullDocument.Contains("Interest Rate/Month") ? fullDocument["Interest Rate/Month"].ToString() + "%" : string.Empty;
                            trfservicefee.Text = fullDocument.Contains("Processing Fee") ? fullDocument["Processing Fee"].ToString() + ".00" : string.Empty;

                            lloanmode.Text = fullDocument.Contains("Mode") ? fullDocument["Mode"].ToString() : string.Empty;

                            trfnotarialfee.Text = fullDocument.Contains("Notarial Rate") ? fullDocument["Notarial Rate"].ToString() + ".00" : string.Empty;
                            trfnotarialamt.Text = trfnotarialfee.Text;

                            trfinsurancefee.Text = fullDocument.Contains("Insurance Rate") ? fullDocument["Insurance Rate"].ToString() + ".00" : string.Empty;
                            trfinsuranceamt.Text = trfinsurancefee.Text;

                            trfannotationfee.Text = fullDocument.Contains("Annotation Rate") ? fullDocument["Annotation Rate"].ToString() + ".00" : string.Empty;
                            trfannotationmt.Text = trfannotationfee.Text;

                            trfvat.Text = fullDocument.Contains("Vat Rate") ? fullDocument["Vat Rate"].ToString() + ".00" : string.Empty;
                            trfvatamt.Text = trfvat.Text;

                            trfmisc.Text = fullDocument.Contains("Misc. Rate") ? fullDocument["Misc. Rate"].ToString() + ".00" : string.Empty;
                            trfmiscamt.Text = trfmisc.Text;

                            trfdocfee.Text = fullDocument.Contains("Doc Rate") ? fullDocument["Doc Rate"].ToString() + ".00" : string.Empty;
                            trfdocamt.Text = trfdocfee.Text;

                            // Get the Mode from the FullDocument or selectedRow
                            string loanMode = fullDocument.Contains("Mode") ? fullDocument["Mode"].ToString() : string.Empty;

                            // Compute amortization with the selected Mode
                            ComputeAmortization(loanMode); // Pass Mode to ComputeAmortization
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error processing fullDocument: " + ex.Message);
                        }
                    }
                    else
                    {
                        // Debug output
                        Console.WriteLine("FullDocument is null");
                    }
                }
                else
                {
                    // Debug output
                    Console.WriteLine("Selected row or FullDocument cell is null");
                }
            }
        }

        private void tloanamt_TextChanged(object sender, EventArgs e)
        {
            //ComputeAmortization();
        }

        private void tloanterm_TextChanged(object sender, EventArgs e)
        {
            //ComputeAmortization();
        }

        private void cbpocash_CheckedChanged(object sender, EventArgs e)
        {
            if (cbpocash.Checked)
            {
               

                // Uncheck other checkboxes
                cbpoonline.Checked = false;
                cbpobank.Checked = false;

                // Enable the cash group box
                gpocash.Enabled = true;

                // Fill up the GroupBox when checked
                FillUpCashGroupBox();
            }
            else if (!cbpoonline.Checked && !cbpobank.Checked)
            {
                cbpocash.Checked = true; // Ensure one is always checked
            }
            else
            {
                // Clear the textboxes when unchecked
                tcashno.Text = string.Empty;
                tcashprofee.Text = string.Empty;
                tcashamt.Text = string.Empty;
                tcashpoamt.Text = string.Empty;
                tcashclnno.Text = string.Empty;
                tcashname.Text = string.Empty;

                // Disable the cash group box
                gpocash.Enabled = false;
            }
        }


        private void cbpoonline_CheckedChanged(object sender, EventArgs e)
        {
            if (cbpoonline.Checked)
            {
              
                // Uncheck other checkboxes
                cbpocash.Checked = false;
                cbpobank.Checked = false;

                // Enable the online group box
                gpoonline.Enabled = true;

                // Fill up the Online GroupBox when checked
                FillUpOnlineGroupBoxOnline();
            }
            else if (!cbpocash.Checked && !cbpobank.Checked)
            {
                cbpoonline.Checked = true;
            }
            else
            {
                // Clear all text boxes in the online group box
                cbonlineplatform.Text = string.Empty;
                tponlinerefno.Text = string.Empty;
                tponlineaccno.Text = string.Empty;
                tponlinename.Text = string.Empty;
                tponlineamt.Text = string.Empty;
                tonlineprofee.Text = string.Empty;
                tonlinepoamt.Text = string.Empty;

                // Disable the online group box
                gpoonline.Enabled = false;
            }
        }

        private void cbpobank_CheckedChanged(object sender, EventArgs e)
        {
            if (cbpobank.Checked)
            {
                

                // Uncheck other checkboxes
                cbpocash.Checked = false;
                cbpoonline.Checked = false;

                // Enable the bank group box
                gpobank.Enabled = true;

                // Fill up the Bank GroupBox when checked
                FillUpBankGroupBoxBank();
            }
            else if (!cbpocash.Checked && !cbpoonline.Checked)
            {
                cbpobank.Checked = true;
            }
            else
            {
                // Clear Bank GroupBox textboxes if unchecked
                cbbankplatform.Text = string.Empty;
                tbankporefno.Text = string.Empty;
                tbankpoaccno.Text = string.Empty;
                tbankname.Text = string.Empty;
                tbankpoamt.Text = string.Empty;
                tbankpoprofee.Text = string.Empty;
                tbankamt.Text = string.Empty;

                // Disable the bank group box
                gpobank.Enabled = false;
            }
        }

        private void bloansave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to disburse all the transactions?",
                "Disburse Transaction", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Show loading indicator (assuming 'load' is a form or control)
                load.Show(this);
                Thread.Sleep(1000);
                load.Close();
                
                DisbursedInitial();
            }
        }

        private void bloanclear_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tloanaccno.Text))
            {
                MessageBox.Show("No LoanNo specified. Please provide a valid LoanNo.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Do you want to cancel all the transactions?",
                                "Cancel Transaction", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    var database = MongoDBConnection.Instance.Database;
                    var collection = database.GetCollection<BsonDocument>("loan_approved");

                    // Filter to find the record by LoanNo
                    var filter = Builders<BsonDocument>.Filter.Eq("LoanNo", tloanaccno.Text);

                    // Find the record
                    var recordToDelete = collection.Find(filter).FirstOrDefault();

                    if (recordToDelete != null)
                    {
                        // Delete the record
                        var result = collection.DeleteOne(filter);

                        if (result.DeletedCount > 0)
                        {
                            MessageBox.Show($"Loan record with LoanNo {tloanaccno.Text} has been successfully deleted.",
                                            "Record Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show($"Failed to delete the record with LoanNo {tloanaccno.Text}.",
                                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"No record found with LoanNo {tloanaccno.Text}.",
                                        "No Record Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while deleting the loan record: {ex.Message}",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Clear UI Fields and Close Form
                ClearAll();
                this.Close();
            }
        }


        private void frm_home_loan_disburse_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Clear the textboxes when unchecked
            tcashno.Text = string.Empty;
            tcashprofee.Text = string.Empty;
            tcashamt.Text = string.Empty;
            tcashpoamt.Text = string.Empty;
            tcashclnno.Text = string.Empty;
            tcashname.Text = string.Empty;

            // Disable the cash group box
            gpocash.Enabled = false;


            // Clear all text boxes in the online group box
            cbonlineplatform.Text = string.Empty;
            tponlinerefno.Text = string.Empty;
            tponlineaccno.Text = string.Empty;
            tponlinename.Text = string.Empty;
            tponlineamt.Text = string.Empty;
            tonlineprofee.Text = string.Empty;
            tonlinepoamt.Text = string.Empty;

            // Disable the online group box
            gpoonline.Enabled = false;


            cbbankplatform.Text = string.Empty;
            tbankporefno.Text = string.Empty;
            tbankpoaccno.Text = string.Empty;
            tbankname.Text = string.Empty;
            tbankpoamt.Text = string.Empty;
            tbankpoprofee.Text = string.Empty;
            tbankamt.Text = string.Empty;

            // Disable the bank group box
            gpobank.Enabled = false;

            tloanamt.Text = string.Empty;
            tloanterm.Text = string.Empty;
            tloaninterest.Text = string.Empty;
            tdays.Text = string.Empty;
            tloaninterestamt.Text = string.Empty;
            trfservicefee.Text = string.Empty;
            trfnotarialfee.Text = string.Empty;
            trfinsurancefee.Text = string.Empty;
            trfannotationfee.Text = string.Empty;
            trfvat.Text = string.Empty;
            trfmisc.Text = string.Empty;
            trfdocfee.Text = string.Empty;
            trfnotarialamt.Text = string.Empty;
            trfinsuranceamt.Text = string.Empty;
            trfannotationmt.Text = string.Empty;
            trfvatamt.Text = string.Empty;
            trfmiscamt.Text = string.Empty;
            trfdocamt.Text = string.Empty;
            tamortizedamt.Text = string.Empty;

            this.Hide();
            e.Cancel = true;
        }

        private void beditcash_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to edit the loan amounts? Please ask for assistance",
               "Edit Disbursement Amounts", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                frm_home_loan_editamt_pass editamounts = new frm_home_loan_editamt_pass();
                if (editamounts.ShowDialog() == DialogResult.OK)
                {
                    // If password is correct, disable the read-only mode of textboxes
                    tcashamt.ReadOnly = false;
                    tcashprofee.ReadOnly = false;
                    tcashpoamt.ReadOnly = false;
                    tcashclnno.ReadOnly = false;
                    

                    tcashamt.ForeColor = Color.Red;
                    tcashprofee.ForeColor = Color.Red;
                    tcashpoamt.ForeColor = Color.Red;
                    tcashclnno.ForeColor = Color.Red;

                    tponlineamt.ReadOnly = false;
                    tonlineprofee.ReadOnly = false;
                    tonlinepoamt.ReadOnly = false;
                    tponlinerefno.ReadOnly = false;
                    tponlineaccno.ReadOnly = false;

                    tponlineamt.ForeColor = Color.Red;
                    tonlineprofee.ForeColor = Color.Red;
                    tonlinepoamt.ForeColor = Color.Red;
                    tponlinerefno.ForeColor = Color.Red;
                    tponlineaccno.ForeColor = Color.Red;

                    tbankporefno.ReadOnly = false;
                    tbankpoaccno.ReadOnly = false;
                    tbankamt.ReadOnly = false;
                    tbankpoprofee.ReadOnly = false;
                    tbankpoamt.ReadOnly = false;

                    tbankporefno.ForeColor = Color.Red;
                    tbankpoaccno.ForeColor = Color.Red;
                    tbankamt.ForeColor = Color.Red;
                    tbankpoprofee.ForeColor = Color.Red;
                    tbankpoamt.ForeColor = Color.Red;

                    tamortizedamt.ReadOnly = false;
                    tpenaltymo.ReadOnly = false;
                    trfservicefee.ReadOnly = false;
                    tloanterm.ReadOnly = false;
                    tloaninterest.ReadOnly = false;

                    tamortizedamt.ForeColor = Color.Red;
                    tpenaltymo.ForeColor = Color.Red;
                    trfservicefee.ForeColor = Color.Red;
                    tloanterm.ForeColor = Color.Red;
                    tloaninterest.ForeColor = Color.Red;

                    leditact.Visible = true;
                    linkdeact.Visible = true;
                }
            }
        }

        private void linkdeact_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (MessageBox.Show("Do you want to edit the loan amounts? Please ask for assistance",
              "Edit Disbursement Amounts", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // If password is correct, disable the read-only mode of textboxes
                tcashamt.ReadOnly = true;
                tcashprofee.ReadOnly = true;
                tcashpoamt.ReadOnly = true;
                tcashclnno.ReadOnly = true;

                tcashamt.ForeColor = Color.Black;
                tcashprofee.ForeColor = Color.Black;
                tcashpoamt.ForeColor = Color.Black;
                tcashclnno.ForeColor = Color.Black;

                tponlineamt.ReadOnly = true;
                tonlineprofee.ReadOnly = true;
                tonlinepoamt.ReadOnly = true;
                tponlinerefno.ReadOnly = true;
                tponlineaccno.ReadOnly = true;

                tponlineamt.ForeColor = Color.Black;
                tonlineprofee.ForeColor = Color.Black;
                tonlinepoamt.ForeColor = Color.Black;
                tponlinerefno.ForeColor = Color.Black;
                tponlineaccno.ForeColor = Color.Black;

                tbankporefno.ReadOnly = true;
                tbankpoaccno.ReadOnly = true;
                tbankamt.ReadOnly = true;
                tbankpoprofee.ReadOnly = true;
                tbankpoamt.ReadOnly = true;

                tbankporefno.ForeColor = Color.Black;
                tbankpoaccno.ForeColor = Color.Black;
                tbankamt.ForeColor = Color.Black;
                tbankpoprofee.ForeColor = Color.Black;
                tbankpoamt.ForeColor = Color.Black;

                tamortizedamt.ReadOnly = true;
                tpenaltymo.ReadOnly = true;
                trfservicefee.ReadOnly = true;
                tloanterm.ReadOnly = true;
                tloaninterest.ReadOnly = true;
                tloaninterest.ForeColor = Color.Black;

                tamortizedamt.ForeColor = Color.Black;
                tpenaltymo.ForeColor = Color.Black;
                trfservicefee.ForeColor = Color.Black;
                tloanterm.ForeColor = Color.Black;

                MessageBox.Show("Editing Data has been deactivated.", "Editing Deactivated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                leditact.Visible = false;
                linkdeact.Visible = false;
            }
        }

        private void DefaultColor ()
        {
            tcashamt.ReadOnly = true;
            tcashprofee.ReadOnly = true;
            tcashpoamt.ReadOnly = true;
            tcashclnno.ReadOnly = true;

            tcashamt.ForeColor = Color.Black;
            tcashprofee.ForeColor = Color.Black;
            tcashpoamt.ForeColor = Color.Black;
            tcashclnno.ForeColor = Color.Black;

            tponlineamt.ReadOnly = true;
            tonlineprofee.ReadOnly = true;
            tonlinepoamt.ReadOnly = true;
            tponlinerefno.ReadOnly = true;
            tponlineaccno.ReadOnly = true;

            tponlineamt.ForeColor = Color.Black;
            tonlineprofee.ForeColor = Color.Black;
            tonlinepoamt.ForeColor = Color.Black;
            tponlinerefno.ForeColor = Color.Black;
            tponlineaccno.ForeColor = Color.Black;

            tbankporefno.ReadOnly = true;
            tbankpoaccno.ReadOnly = true;
            tbankamt.ReadOnly = true;
            tbankpoprofee.ReadOnly = true;
            tbankpoamt.ReadOnly = true;

            tbankporefno.ForeColor = Color.Black;
            tbankpoaccno.ForeColor = Color.Black;
            tbankamt.ForeColor = Color.Black;
            tbankpoprofee.ForeColor = Color.Black;
            tbankpoamt.ForeColor = Color.Black;

            tamortizedamt.ReadOnly = true;
            tpenaltymo.ReadOnly = true;
            trfservicefee.ReadOnly = true;
            tloanterm.ReadOnly = true;

            tamortizedamt.ForeColor = Color.Black;
            tpenaltymo.ForeColor = Color.Black;
            trfservicefee.ForeColor = Color.Black;
            tloanterm.ForeColor = Color.Black;

            leditact.Visible = false;
            linkdeact.Visible = false;
        }

        private void tloaninterest_TextChanged(object sender, EventArgs e)
        {
            ComputeAmortizationInterest();
        }
    }
}

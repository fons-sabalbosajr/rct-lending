using MongoDB.Bson;
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

                // Define filter and sort to find the highest current ID
                var sort = Builders<BsonDocument>.Sort.Descending("IncrementId");
                var lastRecord = collection.Find(new BsonDocument()).Sort(sort).FirstOrDefault();

                // Increment ID logic
                int nextIdNumber = 1;
                if (lastRecord != null && lastRecord.Contains("IncrementId"))
                {
                    string lastId = lastRecord["IncrementId"].AsString;
                    int lastIdNumber = int.Parse(lastId.Substring(lastId.LastIndexOf('-') + 1));
                    nextIdNumber = lastIdNumber + 1;
                }

                // Generate new increment ID
                string newIncrementId = $"RCT-DB-{nextIdNumber:D5}";
                return newIncrementId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating increment ID: " + ex.Message);
                return "RCT-DB-00001"; // Fallback ID
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
                    tcashclnno.Text = loanRecord.Contains("ClientNumber") ? loanRecord["ClientNumber"].AsString : string.Empty;
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
                var sort = Builders<BsonDocument>.Sort.Descending("IncrementId");
                var filter = Builders<BsonDocument>.Filter.Regex("IncrementId", new BsonRegularExpression("^RCT-DB-OLP-"));
                var lastRecord = collection.Find(filter).Sort(sort).FirstOrDefault();

                // Increment ID logic
                int nextIdNumber = 1;
                if (lastRecord != null && lastRecord.Contains("IncrementId"))
                {
                    string lastId = lastRecord["IncrementId"].AsString;
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


        private void FillUpOnlineGroupBoxOnline()
        {
            try
            {
                // Set cashout platform
                cbonlineplatform.Text = "select platform";

                // Generate increment ID
                tponlinerefno.Text = GenerateIncrementIdOnline();

                // Fetch client number and name based on AccountID
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_approved");

                var filter = Builders<BsonDocument>.Filter.Eq("AccountId", tloanaccno.Text);
                var loanRecord = collection.Find(filter).FirstOrDefault();

                if (loanRecord != null)
                {
                    tponlineaccno.Text = loanRecord.Contains("ClientNumber") ? loanRecord["ClientNumber"].AsString : string.Empty;
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

        private string GenerateIncrementIdBank()
        {
            try
            {
                // MongoDB connection and collection
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_disbursed");

                // Define filter and sort to find the highest current ID
                var sort = Builders<BsonDocument>.Sort.Descending("IncrementId");
                var lastRecord = collection.Find(new BsonDocument()).Sort(sort).FirstOrDefault();

                // Increment ID logic
                int nextIdNumber = 1;
                if (lastRecord != null && lastRecord.Contains("IncrementId"))
                {
                    string lastId = lastRecord["IncrementId"].AsString;
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

        private void FillUpBankGroupBoxBank()
        {
            try
            {
                // Set bank type platform
                cbbankplatform.Text = "Specify the bank type";

                // Generate increment ID
                tbankporefno.Text = GenerateIncrementIdBank();

                // Fetch client number and name based on AccountID
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_approved");

                var filter = Builders<BsonDocument>.Filter.Eq("AccountId", tloanaccno.Text);
                var loanRecord = collection.Find(filter).FirstOrDefault();

                if (loanRecord != null)
                {
                    tbankpoaccno.Text = loanRecord.Contains("ClientNumber") ? loanRecord["ClientNumber"].AsString : string.Empty;
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

        private void ComputeAmortization()
        {
            try
            {
                // Check if tloanterm.Text is empty, return without doing anything
                if (string.IsNullOrWhiteSpace(tloanterm.Text))
                {
                    return;
                }

                // Parse and validate loan amount
                string loanAmtText = tloanamt.Text.Replace("₱", "").Replace(",", "").Trim();
                if (!double.TryParse(loanAmtText, out double principal))
                {
                    //MessageBox.Show("Please enter a valid loan amount.");
                    return;
                }

                // Parse and validate loan term
                if (!int.TryParse(tloanterm.Text.Trim(), out int term))
                {
                    //MessageBox.Show("Please enter a valid loan term (in months).");
                    return;
                }

                // Fixed interest rate per month (as percentage)
                double interestRate = 0.05; // 5%

                // Calculate total interest amount for the entire term
                double totalInterest = principal * interestRate * term;

                // Calculate the total number of weekdays (excluding weekends)
                int totalDays = 5 * 4 * term;
                tdays.Text = totalDays.ToString();

                // Calculate amortized amount considering the total interest over the term
                double amortizedAmount = (principal + totalInterest) / totalDays;

                // Display the results
                tamortizedamt.Text = amortizedAmount.ToString("N2");
                tloaninterestamt.Text = totalInterest.ToString("N2");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error computing amortization: " + ex.Message);
            }
        }

        private string GetClientNumber(IMongoCollection<BsonDocument> loanApprovedCollection, string accountId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("AccountId", accountId);
            var document = loanApprovedCollection.Find(filter).FirstOrDefault();

            if (document != null && document.Contains("ClientNumber"))
            {
                return document["ClientNumber"].AsString;
            }

            return null; // Return null if not found or if field is missing
        }

        private void DisbursedInitial()
        {
            var database = MongoDBConnection.Instance.Database;
            var loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");
            var loanDisbursedCollection = database.GetCollection<BsonDocument>("loan_disbursed");
            var loanRateCollection = database.GetCollection<BsonDocument>("loan_rate");

            // Retrieve the ClientNumber from the loan_approved collection
            string accountId = tloanaccno.Text;
            string clientNumber = GetClientNumber(loanApprovedCollection, accountId);

            if (clientNumber == null)
            {
                MessageBox.Show("Client Number could not be found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if a document with the same ClientNumber already exists in loan_disbursed
            var filterExisting = Builders<BsonDocument>.Filter.Eq("cashClnNo", clientNumber);
            var existingDocument = loanDisbursedCollection.Find(filterExisting).FirstOrDefault();

            if (existingDocument != null)
            {
                // Collect the existing fields for detailed error message
                string existingOnlineRefNo = existingDocument.GetValue("onlineRefNo", "Not Available").AsString;
                string existingCashClnNo = existingDocument.GetValue("cashClnNo", "Not Available").AsString;

                MessageBox.Show($"This data detected that transaction already exists in our database:\n" +
                                $"Online Ref No: {existingOnlineRefNo}\n" +
                                $"Cash Client No: {existingCashClnNo}",
                                "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Exit the method to avoid adding duplicate data
            }

            // Convert loan amount to numeric value, removing the Peso sign
            double loanAmount;
            string loanAmtText = tloanamt.Text.Replace("₱", "").Replace(",", "").Trim();
            if (!double.TryParse(loanAmtText, NumberStyles.Number, CultureInfo.InvariantCulture, out loanAmount))
            {
                MessageBox.Show("Invalid loan amount format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Retrieve Loan Type and Mode
            var loanRateFilter = Builders<BsonDocument>.Filter.Eq("Principal", loanAmount);
            var loanRateDocument = loanRateCollection.Find(loanRateFilter).FirstOrDefault();

            if (loanRateDocument == null)
            {
                MessageBox.Show("Loan Rate data could not be found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string loanType = "First Time Borrower"; 
            string mode = loanRateDocument.GetValue("Mode", "Not Available").AsString;

            string encoder = UserSession.Instance.UserName;
            DateTime currentTime = DateTime.Now;

            // Prepare the loan disburse document
            var loanDisburseDocument = new BsonDocument
             {
                 { "LoanIDNo", GenerateNewLoanID() },
                 { "cashClnNo", clientNumber },
                 { "loanAmt", loanAmount },
                 { "loanTerm", tloanterm.Text },
                 { "loanInterest", tloaninterest.Text },
                 { "days", tdays.Text },
                 { "loanInterestAmt", tloaninterestamt.Text },
                 { "rfServiceFee", trfservicefee.Text },
                 { "rfNotarialFee", trfnotarialfee.Text },
                 { "rfInsuranceFee", trfinsurancefee.Text },
                 { "rfAnnotationFee", trfannotationfee.Text },
                 { "rfVat", trfvat.Text },
                 { "rfMisc", trfmisc.Text },
                 { "rfDocFee", trfdocfee.Text },
                 { "rfNotarialAmt", trfnotarialamt.Text },
                 { "rfInsuranceAmt", trfinsuranceamt.Text },
                 { "rfAnnotationAmt", trfannotationmt.Text },
                 { "rfVatAmt", trfvatamt.Text },
                 { "rfMiscAmt", trfmiscamt.Text },
                 { "rfDocAmt", trfdocamt.Text },
                 { "amortizedAmt", tamortizedamt.Text },
                 { "LoanType", loanType },
                 { "Mode", mode },
                 { "PaymentStartDate", dtpayoutdate.Value.ToString("MM/dd/yyyy") },
                 { "Encoder", encoder },  // Add encoder
                 { "DisbursementTime", currentTime }  // Add disbursement time
  
             };

            // Add payment fields based on selected payment method
            if (cbpocash.Checked)
            {
                AddCashPaymentFields(loanDisburseDocument);
            }
            else if (cbpoonline.Checked)
            {
                AddOnlinePaymentFields(loanDisburseDocument);
            }
            else if (cbpobank.Checked)
            {
                AddBankPaymentFields(loanDisburseDocument);
            }
            else
            {
                MessageBox.Show("Please select a payment method.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Insert the document as it is a new entry
            loanDisbursedCollection.InsertOne(loanDisburseDocument);

            // Update the loan_approved document
            var updateFilter = Builders<BsonDocument>.Filter.Eq("ClientNumber", clientNumber);
            var update = Builders<BsonDocument>.Update
                .Set("LoanStatus", "For Releasing Loan Disbursement")
                .Set("LoanType", loanType)
                .Set("DisbursementDate", DateTime.Now.ToString("f"));

            loanApprovedCollection.UpdateOne(updateFilter, update);

            // Notify user and clear form
            MessageBox.Show(this, "Transactions disbursed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            frm_home_loan_voucher voucherForm = new frm_home_loan_voucher(clientNumber);
            voucherForm.Show();
            ClearAll();
        }



        private void AddCashPaymentFields(BsonDocument document)
        {
            document.Add("cashNo", tcashno.Text);
            document.Add("cashName", tcashname.Text);
            document.Add("cashProFee", tcashprofee.Text);
            document.Add("cashAmt", tcashamt.Text);
            document.Add("cashPoAmt", tcashpoamt.Text);
            document.Add("cashDate", dtpcash.Value.ToString("MM/dd/yyyy"));

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
            // Generate a new Loan ID No.
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<BsonDocument>("loan_disbursed");

            var lastLoanDocument = collection.Find(new BsonDocument())
                                            .Sort(Builders<BsonDocument>.Sort.Descending("LoanIDNo"))
                                            .Limit(1)
                                            .FirstOrDefault();

            string newLoanID;
            if (lastLoanDocument != null && lastLoanDocument.Contains("LoanIDNo"))
            {
                string lastLoanID = lastLoanDocument["LoanIDNo"].AsString;
                int lastNumber = int.Parse(lastLoanID.Substring(10)); // Assuming "RCT-LNR1-" is fixed
                newLoanID = $"RCT-LNR1-{(lastNumber + 1).ToString("D8")}";
            }
            else
            {
                newLoanID = "RCT-LNR1-20240001";
            }

            return newLoanID;
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

                            // Compute amortization whenever the loan data changes
                            ComputeAmortization();
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
            ComputeAmortization();
        }

        private void tloanterm_TextChanged(object sender, EventArgs e)
        {
            ComputeAmortization();
        }

        private void cbpocash_CheckedChanged(object sender, EventArgs e)
        {
            if (cbpocash.Checked)
            {
                if (cbpoonline.Checked || cbpobank.Checked)
                {
                    MessageBox.Show("Only one option can be selected at a time. The other options will be unchecked.",
                        "Selection Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Uncheck other checkboxes
                cbpoonline.Checked = false;
                cbpobank.Checked = false;

                // Enable the cash group box
                gpocash.Enabled = true;

                // Fill up the GroupBox when checked
                FillUpCashGroupBox();
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
                if (cbpocash.Checked || cbpobank.Checked)
                {
                    MessageBox.Show("Only one option can be selected at a time. The other options will be unchecked.",
                        "Selection Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Uncheck other checkboxes
                cbpocash.Checked = false;
                cbpobank.Checked = false;

                // Enable the online group box
                gpoonline.Enabled = true;

                // Fill up the Online GroupBox when checked
                FillUpOnlineGroupBoxOnline();
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
                if (cbpocash.Checked || cbpoonline.Checked)
                {
                    MessageBox.Show("Only one option can be selected at a time. The other options will be unchecked.",
                        "Selection Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Uncheck other checkboxes
                cbpocash.Checked = false;
                cbpoonline.Checked = false;

                // Enable the bank group box
                gpobank.Enabled = true;

                // Fill up the Bank GroupBox when checked
                FillUpBankGroupBoxBank();
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
            if (MessageBox.Show("Do you want to disrbuse all the transactions?",
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
            if (MessageBox.Show("Do you want to cancel all the transactions?",
                "Cancel Transaction", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {

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
                frm_home_loan_editamounts editamounts = new frm_home_loan_editamounts();
                editamounts.ShowDialog(this);
            }
        }
    }
}

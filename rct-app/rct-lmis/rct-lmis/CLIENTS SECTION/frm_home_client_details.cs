using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rct_lmis.CLIENTS_SECTION
{
    public partial class frm_home_client_details : Form
    {
        private string loanId;
        private string clientNo;

        public frm_home_client_details(string loanId, string clientNo)
        {
            InitializeComponent();
            InitializeDgvLoan();
            laccno.Text = loanId;
            lclientno.Text = clientNo;
        }

        LoadingFunction load = new LoadingFunction();

        private void InitializeDgvLoan()
        {
            // Clear existing columns if any
            dgvloan.Columns.Clear();

            // Set columns for grouped information
            dgvloan.Columns.Add("AccountInfo", "Account Info");
            dgvloan.Columns.Add("LoanInfo", "Loan Info");
            dgvloan.Columns.Add("PaymentInfo", "Payment Info");
            dgvloan.Columns.Add("StatusInfo", "Status");

            // Optional: set column width for better readability
            foreach (DataGridViewColumn column in dgvloan.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            }
            dgvloan.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        private void ClearCoMakerFields()
        {
            tcomakeraccno.Text = string.Empty;
            tcomakerlname.Text = string.Empty;
            tcomakerfname.Text = string.Empty;
            tcomakermname.Text = string.Empty;
            tcomakeraddress.Text = string.Empty;
            tcomakerno.Text = string.Empty;
        }

        private void ClearCoBorrowerFields()
        {
            tborroweraccno.Text = string.Empty;
            tborrowerlname.Text = string.Empty;
            tborrowerfname.Text = string.Empty;
            tborrowermname.Text = string.Empty;
            tborroweraddress.Text = string.Empty;
            tborrowerno.Text = string.Empty;
        }

        private void LoadLoanData()
        {
            // MongoDB connection
            var database = MongoDBConnection.Instance.Database;
            var loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");
            var loanCoMakerCollection = database.GetCollection<BsonDocument>("loan_approved_comaker");
            var loanCoBorrowerCollection = database.GetCollection<BsonDocument>("loan_approved_coborrower");

            // Find the loan document matching the Loan ID and Client No
            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("LoanNo", laccno.Text),
                Builders<BsonDocument>.Filter.Eq("ClientNo", lclientno.Text)
            );

            var loanDocument = loanApprovedCollection.Find(filter).FirstOrDefault();

            if (loanDocument != null)
            {
                // Populate the textboxes with data from the loan document
                tclastname.Text = loanDocument.GetValue("LastName", "").AsString;
                tcfirstname.Text = loanDocument.GetValue("FirstName", "").AsString;
                tcmidname.Text = loanDocument.GetValue("MiddleName", "").AsString;

                // Merge address fields into a full address
                string fullAddress = $"{loanDocument.GetValue("Barangay", "").AsString}, " +
                                     $"{loanDocument.GetValue("City", "").AsString}, " +
                                     $"{loanDocument.GetValue("Province", "").AsString}";

                taddress.Text = fullAddress; // Set the merged address to the address textbox

                // Populate remaining fields
                tbrgy.Text = loanDocument.GetValue("Barangay", "").AsString;
                tcity.Text = loanDocument.GetValue("City", "").AsString;
                tprov.Text = loanDocument.GetValue("Province", "").AsString;
                tzipcode.Text = loanDocument.GetValue("ZipCode", "").AsString;
                tcontact.Text = loanDocument.GetValue("ContactNumber", "").AsString; 
                temailadd.Text = loanDocument.GetValue("Email", "").AsString;
                tothercontacts.Text = loanDocument.GetValue("AlternateContactNumber", "").AsString;
                toccupation.Text = loanDocument.GetValue("Occupation", "").AsString; 
                tcompname.Text = loanDocument.GetValue("CompanyName", "").AsString; 
                tworkaddress.Text = loanDocument.GetValue("WorkAddress", "").AsString; 
                tmonthlyincome.Text = loanDocument.GetValue("MonthlyIncome", "").AsString;
                tcstatus.Text = loanDocument.GetValue("CivilStatus", "").AsString;
                tgender.Text = loanDocument.GetValue("Gender", "").AsString;
                tdbirth.Text = loanDocument.GetValue("DateOfBirth", "").AsString;
                tbirthaddress.Text = loanDocument.GetValue("BirthAddress", "").AsString;
                tchildno.Text = loanDocument.GetValue("NumberOfChildren", "").AsString;
                tspouselname.Text = loanDocument.GetValue("SpouseLastName", "").AsString;
                tspousefname.Text = loanDocument.GetValue("SpouseFirstName", "").AsString;
                tspousemname.Text = loanDocument.GetValue("SpouseMiddleName", "").AsString;

                // Load Co-Maker data
                var coMakerFilter = Builders<BsonDocument>.Filter.And(
                    Builders<BsonDocument>.Filter.Eq("LoanId", loanDocument.GetValue("LoanNo", "").AsString),
                    Builders<BsonDocument>.Filter.Eq("ClientNo", loanDocument.GetValue("ClientNo", "").AsString)
                );

                var coMakerDocument = loanCoMakerCollection.Find(coMakerFilter).FirstOrDefault();
                if (coMakerDocument != null)
                {
                    tcomakeraccno.Text = coMakerDocument.GetValue("AccountNo", "N/A").AsString;
                    tcomakerlname.Text = coMakerDocument.GetValue("LastName", "N/A").AsString;
                    tcomakerfname.Text = coMakerDocument.GetValue("FirstName", "N/A").AsString;
                    tcomakermname.Text = coMakerDocument.GetValue("MiddleName", "N/A").AsString;
                    tcomakeraddress.Text = coMakerDocument.GetValue("Address", "N/A").AsString;
                    tcomakerno.Text = coMakerDocument.GetValue("ContactNumber", "N/A").AsString;
                }
                else
                {
                    tcomakeraccno.Text = "N/A";
                    tcomakerlname.Text = "N/A";
                    tcomakerfname.Text = "N/A";
                    tcomakermname.Text = "N/A";
                    tcomakeraddress.Text = "N/A";
                    tcomakerno.Text = "N/A";
                }

                // Load Co-Borrower data
                var coBorrowerFilter = Builders<BsonDocument>.Filter.And(
                    Builders<BsonDocument>.Filter.Eq("LoanId", loanDocument.GetValue("LoanNo", "").AsString),
                    Builders<BsonDocument>.Filter.Eq("ClientNo", loanDocument.GetValue("ClientNo", "").AsString)
                );

                var coBorrowerDocument = loanCoBorrowerCollection.Find(coBorrowerFilter).FirstOrDefault();
                if (coBorrowerDocument != null)
                {
                    tborroweraccno.Text = coBorrowerDocument.GetValue("AccountNo", "N/A").AsString;
                    tborrowerlname.Text = coBorrowerDocument.GetValue("LastName", "N/A").AsString;
                    tborrowerfname.Text = coBorrowerDocument.GetValue("FirstName", "N/A").AsString;
                    tborrowermname.Text = coBorrowerDocument.GetValue("MiddleName", "N/A").AsString;
                    tborroweraddress.Text = coBorrowerDocument.GetValue("Address", "N/A").AsString;
                    tborrowerno.Text = coBorrowerDocument.GetValue("ContactNumber", "N/A").AsString;
                }
                else
                {
                    tborroweraccno.Text = "N/A";
                    tborrowerlname.Text = "N/A";
                    tborrowerfname.Text = "N/A";
                    tborrowermname.Text = "N/A";
                    tborroweraddress.Text = "N/A";
                    tborrowerno.Text = "N/A";
                }
            }
            else
            {
                MessageBox.Show("No loan data found for the provided Loan ID and Client Number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLoanHistory()
        {
            // MongoDB connection
            var database = MongoDBConnection.Instance.Database;
            var loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");

            // Assuming you want to fetch loans related to the current user
            var clientNo = lclientno.Text; // Or another method to get the current client number
            var filter = Builders<BsonDocument>.Filter.Eq("ClientNo", clientNo);

            // Fetch all loan documents for the current client
            var loanDocuments = loanApprovedCollection.Find(filter).ToList();

            // Clear existing rows before adding new data
            dgvloan.Rows.Clear();

            // Loop through each loan document and populate the DataGridView
            foreach (var loanDocument in loanDocuments)
            {
                // Combine names into one field
                string fullName = $"{loanDocument.GetValue("LastName", "").AsString}, " +
                                  $"{loanDocument.GetValue("FirstName", "").AsString} " +
                                  $"{loanDocument.GetValue("MiddleName", "").AsString}";

                // Grouping Account Info
                string accountInfo = $"Client No: {loanDocument.GetValue("ClientNo", "").AsString}\n" +
                                     $"Name: {fullName}";

                // Grouping Loan Info
                string loanInfo = $"Term: {loanDocument.GetValue("LoanTerm", "").AsString}\n" +
                                  $"Amount: {loanDocument.GetValue("LoanAmount", "").AsString}\n" +
                                  $"Amortization: {loanDocument.GetValue("LoanAmortization", "").AsString}\n" +
                                  $"Balance: {loanDocument.GetValue("LoanBalance", "").AsString}";

                // Grouping Payment Info
                string paymentInfo = $"Payment Mode: {loanDocument.GetValue("PaymentMode", "").AsString}\n" +
                                     $"Start Date: {loanDocument.GetValue("StartPaymentDate", "").AsString}\n" +
                                     $"Maturity Date: {loanDocument.GetValue("MaturityDate", "").AsString}";

                // Grouping Status Info
                string statusInfo = $"Collector: {loanDocument.GetValue("CollectorName", "").AsString}\n" +
                                    $"Date Encoded: {loanDocument.GetValue("Date_Encoded", "").AsString}\n" +
                                    $"Process Status: {loanDocument.GetValue("LoanProcessStatus", "").AsString}";

                // Load grouped data into the DataGridView
                dgvloan.Rows.Add(accountInfo, loanInfo, paymentInfo, statusInfo);
            }

            // Optionally, set focus to the first cell
            if (dgvloan.Rows.Count > 0)
            {
                dgvloan.Rows[0].Selected = true;
                dgvloan.CurrentCell = dgvloan.Rows[0].Cells[0];
            }
        }





        private void frm_home_client_details_Load(object sender, EventArgs e)
        {
            LoadLoanData();
            LoadLoanHistory();
        }

        private async void bsaveall_Click(object sender, EventArgs e)
        {
            // Validate input before proceeding
            if (string.IsNullOrWhiteSpace(tclastname.Text) ||
                string.IsNullOrWhiteSpace(tcfirstname.Text) ||
                string.IsNullOrWhiteSpace(taddress.Text))
            {
                MessageBox.Show("Please fill in the required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // MongoDB connection
            var database = MongoDBConnection.Instance.Database;
            var loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");

            // Find the loan document matching the Loan ID
            var filter = Builders<BsonDocument>.Filter.Eq("LoanNo", laccno.Text);

            // Create an update definition for the fields you want to update
            var update = Builders<BsonDocument>.Update
                .Set("LastName", tclastname.Text)
                .Set("FirstName", tcfirstname.Text)
                .Set("MiddleName", tcmidname.Text)
                .Set("Barangay", tbrgy.Text)
                .Set("City", tcity.Text)
                .Set("Province", tprov.Text)
                .Set("ContactNumber", tcontact.Text)
                .Set("Email", temailadd.Text)
                .Set("AlternateContactNumber", tothercontacts.Text)
                .Set("Occupation", toccupation.Text)
                .Set("CompanyName", tcompname.Text)
                .Set("WorkAddress", tworkaddress.Text)
                .Set("MonthlyIncome", tmonthlyincome.Text)
                .Set("CivilStatus", tcstatus.Text)
                .Set("Gender", tgender.Text)
                .Set("DateOfBirth", tdbirth.Text)
                .Set("BirthAddress", tbirthaddress.Text)
                .Set("NumberOfChildren", tchildno.Text)
                .Set("SpouseLastName", tspouselname.Text)
                .Set("SpouseFirstName", tspousefname.Text)
                .Set("SpouseMiddleName", tspousemname.Text);

            // Run the update operation asynchronously
            var result = await loanApprovedCollection.UpdateOneAsync(filter, update);

            // Check if any documents were modified
            if (result.ModifiedCount > 0)
            {
                MessageBox.Show("Data updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadLoanData(); // Ensure this method is non-blocking
            }
            else
            {
                MessageBox.Show("No changes were made or document not found.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void cbsameborrow_CheckedChanged(object sender, EventArgs e)
        {
            if (cbsameborrow.Checked)
            {
                // Copy values from Co-Borrower to Co-Maker
                tborroweraccno.Text = tcomakeraccno.Text;
                tborrowerlname.Text = tcomakerlname.Text;
                tborrowerfname.Text = tcomakerfname.Text;
                tborrowermname.Text = tcomakermname.Text;
                tborroweraddress.Text = tcomakeraddress.Text;
                tborrowerno.Text = tcomakerno.Text;
            }
            else
            {
                // Clear Co-Maker fields if checkbox is unchecked
                ClearCoBorrowerFields();
            }
        }

        private void bcomakeradd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tcomakerlname.Text) ||
            string.IsNullOrWhiteSpace(tcomakerfname.Text) ||
            string.IsNullOrWhiteSpace(tcomakeraddress.Text))
            {
                MessageBox.Show("Please fill in the required fields for Co-Maker.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // MongoDB connection
            var database = MongoDBConnection.Instance.Database;
            var coMakerCollection = database.GetCollection<BsonDocument>("loan_approved_comaker");

            // Check if Co-Maker already exists
            var coMakerFilter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("LastName", tcomakerlname.Text),
                Builders<BsonDocument>.Filter.Eq("FirstName", tcomakerfname.Text),
                Builders<BsonDocument>.Filter.Eq("Address", tcomakeraddress.Text)
            );

            var existingCoMaker = coMakerCollection.Find(coMakerFilter).FirstOrDefault();
            if (existingCoMaker != null)
            {
                MessageBox.Show("This Co-Maker already exists.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Generate a unique ID for the Co-Maker
            string coMakerId = "CM-" + Guid.NewGuid().ToString().Substring(0, 8); // Example ID format

            // Create a document for Co-Maker
            var coMakerDocument = new BsonDocument
              {
                  { "CoMakerID", coMakerId },
                  { "LoanID", laccno.Text }, // You can get loanId from your form context
                  { "ClientNo", lclientno.Text }, // You can get clientNo from your form context
                  { "AccountNo", tcomakeraccno.Text },
                  { "LastName", tcomakerlname.Text },
                  { "FirstName", tcomakerfname.Text },
                  { "MiddleName", tcomakermname.Text },
                  { "Address", tcomakeraddress.Text },
                  { "ContactNo", tcomakerno.Text }
              };

            // Insert Co-Maker document
            coMakerCollection.InsertOne(coMakerDocument);
            MessageBox.Show("Co-Maker saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tborroweradd_Click(object sender, EventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(tborrowerlname.Text) ||
                string.IsNullOrWhiteSpace(tborrowerfname.Text) ||
                string.IsNullOrWhiteSpace(tborroweraddress.Text))
            {
                MessageBox.Show("Please fill in the required fields for Co-Borrower.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // MongoDB connection
            var database = MongoDBConnection.Instance.Database;
            var coBorrowerCollection = database.GetCollection<BsonDocument>("loan_approved_coborrower");

            // Check if Co-Borrower already exists
            var coBorrowerFilter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("LastName", tborrowerlname.Text),
                Builders<BsonDocument>.Filter.Eq("FirstName", tborrowerfname.Text),
                Builders<BsonDocument>.Filter.Eq("Address", tborroweraddress.Text)
            );

            var existingCoBorrower = coBorrowerCollection.Find(coBorrowerFilter).FirstOrDefault();
            if (existingCoBorrower != null)
            {
                MessageBox.Show("This Co-Borrower already exists.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Generate a unique ID for the Co-Borrower
            string coBorrowerId = "CB-" + Guid.NewGuid().ToString().Substring(0, 8); // Example ID format

            // Create a document for Co-Borrower
            var coBorrowerDocument = new BsonDocument
           {
               { "CoBorrowerID", coBorrowerId },
               { "LoanID", laccno.Text }, // You can get loanId from your form context
               { "ClientNo", lclientno.Text }, // You can get clientNo from your form context
               { "AccountNo", tborroweraccno.Text },
               { "LastName", tborrowerlname.Text },
               { "FirstName", tborrowerfname.Text },
               { "MiddleName", tborrowermname.Text },
               { "Address", tborroweraddress.Text },
               { "ContactNo", tborrowerno.Text }
           };

            // Insert Co-Borrower document
            coBorrowerCollection.InsertOne(coBorrowerDocument);
            MessageBox.Show("Co-Borrower saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bcomakerclear_Click(object sender, EventArgs e)
        {
            ClearCoMakerFields();
        }

        private void tborrowerclear_Click(object sender, EventArgs e)
        {
            ClearCoBorrowerFields();
        }

        private void bcopyaccno_Click(object sender, EventArgs e)
        {
            // Get the text from the Label control
            string accNo = laccno.Text;

            // Copy the text to the clipboard
            Clipboard.SetText(accNo);

            // Show a message box to notify the user
            MessageBox.Show("The account number has been copied to your clipboard.", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

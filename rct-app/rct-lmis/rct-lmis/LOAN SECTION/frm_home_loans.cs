using MongoDB.Bson;
using MongoDB.Driver;
using rct_lmis.LOAN_SECTION;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace rct_lmis
{
    public partial class frm_home_loans : Form
    {
        private string loggedInUsername;
        

        public frm_home_loans()
        {
            InitializeComponent();
            loggedInUsername = UserSession.Instance.CurrentUser;
        }

        LoadingFunction load = new LoadingFunction();
        frm_home_loan_new flnew = new frm_home_loan_new();
        frm_home_loan_add fladd = new frm_home_loan_add();

        private void LoadApprovedLoansData()
        {
            try
            {
                // Access the collections
                var database = MongoDBConnection.Instance.Database;
                var approvedLoansCollection = database.GetCollection<BsonDocument>("loan_approved");
                var applicationsCollection = database.GetCollection<BsonDocument>("loan_applications");
                var releasedCollection = database.GetCollection<BsonDocument>("loan_released");

                // Retrieve all documents from the collections
                var approvedDocuments = approvedLoansCollection.Find(new BsonDocument()).ToList();
                var applicationDocuments = applicationsCollection.Find(new BsonDocument()).ToList();
                var releasedDocuments = releasedCollection.Find(new BsonDocument()).ToList();

                // Create a DataTable to hold the data
                DataTable dataTable = new DataTable();

                // Define the columns to display
                dataTable.Columns.Add("AccountID");
                dataTable.Columns.Add("LoanType");
                dataTable.Columns.Add("PrincipalAmount");
                dataTable.Columns.Add("LoanTerm");
                dataTable.Columns.Add("LoanStatus");
                dataTable.Columns.Add("FullNameAndAddress");
                dataTable.Columns.Add("CBCP");
                dataTable.Columns.Add("Documents");

                // Add rows to the DataTable
                foreach (var approvedDoc in approvedDocuments)
                {
                    DataRow row = dataTable.NewRow();
                    var accountId = approvedDoc.Contains("AccountId") ? approvedDoc["AccountId"].ToString() : string.Empty;
                    row["AccountID"] = accountId;
                    row["LoanType"] = approvedDoc.Contains("LoanType") ? approvedDoc["LoanType"].ToString() : string.Empty;
                    row["PrincipalAmount"] = approvedDoc.Contains("PrincipalAmount") ? "₱ " + approvedDoc["PrincipalAmount"].ToString() + ".00" : string.Empty;
                    row["LoanTerm"] = approvedDoc.Contains("LoanTerm") ? approvedDoc["LoanTerm"].ToString() + " month/s" : string.Empty;

                    // Fetch status and documents from the loan_applications collection
                    var applicationDoc = applicationDocuments.FirstOrDefault(doc => doc.Contains("AccountId") && doc["AccountId"].ToString() == accountId);
                    row["LoanStatus"] = approvedDoc.Contains("LoanStatus") ? approvedDoc["LoanStatus"].ToString() : string.Empty;
                    row["Documents"] = applicationDoc != null && applicationDoc.Contains("docs") ? applicationDoc["docs"].ToString() : string.Empty;

                    // FullName and Address
                    string firstName = approvedDoc.Contains("FirstName") ? approvedDoc["FirstName"].ToString() : string.Empty;
                    string middleName = approvedDoc.Contains("MiddleName") ? approvedDoc["MiddleName"].ToString() : string.Empty;
                    string lastName = approvedDoc.Contains("LastName") ? approvedDoc["LastName"].ToString() : string.Empty;
                    string suffixName = approvedDoc.Contains("SuffixName") ? approvedDoc["SuffixName"].ToString() : string.Empty;
                    string fullName = $"{firstName} {middleName} {lastName} {suffixName}";

                    string street = approvedDoc.Contains("Street") ? approvedDoc["Street"].ToString() : string.Empty;
                    string barangay = approvedDoc.Contains("Barangay") ? approvedDoc["Barangay"].ToString() : string.Empty;
                    string city = approvedDoc.Contains("City") ? approvedDoc["City"].ToString() : string.Empty;
                    string province = approvedDoc.Contains("Province") ? approvedDoc["Province"].ToString() : string.Empty;
                    string address = $"{street}\n{barangay}\n{city}\n{province}";

                    // Concatenate full name and address with line breaks
                    row["FullNameAndAddress"] = $"{fullName}\n{address}";

                    row["CBCP"] = approvedDoc.Contains("CBCP") ? approvedDoc["CBCP"].ToString() : string.Empty;

                    // Split the documents into separate lines based on the comma separator
                    if (approvedDoc.Contains("docs"))
                    {
                        var documentsList = approvedDoc["docs"].ToString().Split(',');
                        row["Documents"] = string.Join("\n", documentsList);
                    }
                    else
                    {
                        row["Documents"] = string.Empty;
                    }

                    dataTable.Rows.Add(row);
                }

                // Bind the DataTable to the DataGridView
                dgvdata.DataSource = dataTable;

                // Set custom header texts
                dgvdata.Columns["AccountID"].HeaderText = "Account ID";
                dgvdata.Columns["LoanType"].HeaderText = "Loan Type";
                dgvdata.Columns["LoanType"].Width = 70;
                dgvdata.Columns["PrincipalAmount"].HeaderText = "Principal Amount";
                dgvdata.Columns["PrincipalAmount"].Width = 100;
                dgvdata.Columns["LoanTerm"].HeaderText = "Loan Term";
                dgvdata.Columns["LoanStatus"].HeaderText = "Loan Status";
                dgvdata.Columns["FullNameAndAddress"].HeaderText = "Client Name";
                dgvdata.Columns["FullNameAndAddress"].Width = 250;
                dgvdata.Columns["CBCP"].HeaderText = "Contact Number";
                dgvdata.Columns["Documents"].HeaderText = "Attached Documents";
                dgvdata.Columns["Documents"].Width = 275;

                // Set font size and style for the entire DataGridView
                dgvdata.DefaultCellStyle.Font = new Font("Segoe UI", 9);
                dgvdata.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);

                // Center align specific columns
                CenterAlignColumns("AccountID", "LoanType", "PrincipalAmount", "LoanTerm", "CBCP");

                lnorecord.Visible = dgvdata.Rows.Count == 0;

                // Configure the DataGridView for the FullNameAndAddress column
                if (dgvdata.Columns["FullNameAndAddress"] != null)
                {
                    dgvdata.Columns["FullNameAndAddress"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    dgvdata.Columns["FullNameAndAddress"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                // Configure the Documents column to be a link type
                if (dgvdata.Columns["Documents"] != null)
                {
                    dgvdata.Columns["Documents"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    dgvdata.Columns["Documents"].ReadOnly = true;
                    dgvdata.Columns["Documents"].SortMode = DataGridViewColumnSortMode.Automatic;
                }

                // Add button columns if not already added
                if (dgvdata.Columns["btnActions"] == null)
                {
                    DataGridViewButtonColumn viewDetailsButtonColumn = new DataGridViewButtonColumn
                    {
                        Name = "btnActions",
                        HeaderText = "Actions",
                        Text = "View Details",
                        UseColumnTextForButtonValue = true,
                        FlatStyle = FlatStyle.Standard,
                    };

                    dgvdata.Columns.Add(viewDetailsButtonColumn);
                }

                if (dgvdata.Columns["btnDisburse"] == null)
                {
                    DataGridViewButtonColumn disburseButtonColumn = new DataGridViewButtonColumn
                    {
                        Name = "btnDisburse",
                        HeaderText = "Disburse",
                        Text = "Disburse",
                        UseColumnTextForButtonValue = true,
                        FlatStyle = FlatStyle.Standard,
                    };

                    dgvdata.Columns.Add(disburseButtonColumn);
                }

                // Set width, padding, and font size for the button columns
                var btnColumn = dgvdata.Columns["btnActions"];
                if (btnColumn != null)
                {
                    btnColumn.Width = 120;
                    btnColumn.DefaultCellStyle.Padding = new Padding(30, 20, 30, 20);
                    btnColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    btnColumn.DefaultCellStyle.Font = new Font("Segoe UI", 9);
                }

                var btnColumnDisburse = dgvdata.Columns["btnDisburse"];
                if (btnColumnDisburse != null)
                {
                    btnColumnDisburse.Width = 120;
                    btnColumnDisburse.DefaultCellStyle.Padding = new Padding(30, 20, 30, 20);
                    btnColumnDisburse.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    btnColumnDisburse.DefaultCellStyle.Font = new Font("Segoe UI", 9);
                }

                // Hide the "Disburse" button column if any loan status is "Loan Released"
                bool shouldHideDisburseColumn = dataTable.AsEnumerable().Any(row => row.Field<string>("LoanStatus").Equals("Loan Released", StringComparison.OrdinalIgnoreCase));
                //dgvdata.Columns["btnDisburse"].Visible = !shouldHideDisburseColumn;

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading the approved loans data: " + ex.Message);
            }
        }



        private void CenterAlignColumns(params string[] columnNames)
        {
            foreach (var columnName in columnNames)
            {
                if (dgvdata.Columns[columnName] != null)
                {
                    dgvdata.Columns[columnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        private void LoadUserInfo(string username)
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<BsonDocument>("user_accounts"); // 'user_accounts' is the name of your collection

            var filter = Builders<BsonDocument>.Filter.Eq("Username", username);
            var user = collection.Find(filter).FirstOrDefault();

            if (user != null)
            {
                // Get the full name
                var fullName = user.GetValue("FullName").AsString;

                // Display the full name
                luser.Text = fullName;
            }
        }

     

        private void baddnew_Click(object sender, EventArgs e)
        {
            load.Show(this);
            Thread.Sleep(500);
            fladd.Show(this);
            load.Close();
        }

        private void frm_home_loans_Load(object sender, EventArgs e)
        {
            LoadApprovedLoansData();
            LoadUserInfo(loggedInUsername);
           

            ltotalloancount.Text = dgvdata.Rows.Count.ToString();
           
        }

        private void dgvdata_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvdata.ClearSelection();
            lnorecord.Visible = false;
        }

        private void dgvdata_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvdata.Columns[e.ColumnIndex].Name == "LoanStatus")
            {
                var statusValue = e.Value?.ToString();
                if (statusValue == "Application Approved")
                {
                    e.CellStyle.BackColor = Color.LightGreen;
                    e.CellStyle.ForeColor = Color.Black;
                }
            }
        }

        private void dgvdata_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
              
                if (dgvdata.Columns[e.ColumnIndex].Name == "btnActions")
                {
                    var selectedAccountId = dgvdata.Rows[e.RowIndex].Cells["AccountID"].Value.ToString();
                    frm_home_loan_new loanDetailsForm = new frm_home_loan_new
                    {
                        AccountID = selectedAccountId
                    };

                    load.Show(this);
                    Thread.Sleep(500);
                    loanDetailsForm.Show(this);
                    load.Close();
                }
               
                else if (dgvdata.Columns[e.ColumnIndex].Name == "btnDisburse")
                {
                    var selectedAccountId = dgvdata.Rows[e.RowIndex].Cells["AccountID"].Value.ToString();

                    // Pass the AccountID to the frm_home_loan_disburse form
                    frm_home_loan_disburse fdis = new frm_home_loan_disburse
                    {
                        AccountID = selectedAccountId // Set the AccountID
                    };

                    load.Show(this);
                    Thread.Sleep(500);
                    load.Close();
                    fdis.Show(this); // Open the frm_home_loan_disburse form
                }
            }
        }
    }
}

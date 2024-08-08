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
using System.Windows.Forms;

namespace rct_lmis
{
    public partial class frm_home_loans : Form
    {
        public frm_home_loans()
        {
            InitializeComponent();
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

                // Retrieve all documents from both collections
                var approvedDocuments = approvedLoansCollection.Find(new BsonDocument()).ToList();
                var applicationDocuments = applicationsCollection.Find(new BsonDocument()).ToList();

                // Create a DataTable to hold the data
                DataTable dataTable = new DataTable();

                // Define the columns to display
                dataTable.Columns.Add("AccountID");
                dataTable.Columns.Add("LoanType");
                dataTable.Columns.Add("Principal");
                dataTable.Columns.Add("Term");
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
                    row["Principal"] = approvedDoc.Contains("Principal") ? "₱ " + approvedDoc["Principal"].ToString() + ".00" : string.Empty;
                    row["Term"] = approvedDoc.Contains("Term") ? approvedDoc["Term"].ToString() + " month/s" : string.Empty;

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
                    if (row["Documents"] != null)
                    {
                        var documentsList = row["Documents"].ToString().Split(',');
                        row["Documents"] = string.Join("\n", documentsList);
                    }

                    dataTable.Rows.Add(row);
                }

                // Bind the DataTable to the DataGridView
                dgvdata.DataSource = dataTable;

                // Set custom header texts
                dgvdata.Columns["AccountID"].HeaderText = "Account ID";
                dgvdata.Columns["LoanType"].HeaderText = "Loan Type";
                dgvdata.Columns["LoanType"].Width = 70;
                dgvdata.Columns["Principal"].HeaderText = "Principal Amount";
                dgvdata.Columns["Principal"].Width = 100;
                dgvdata.Columns["Term"].HeaderText = "Loan Term";
                dgvdata.Columns["LoanStatus"].HeaderText = "Loan Status";
                dgvdata.Columns["FullNameAndAddress"].HeaderText = "Client Name";
                dgvdata.Columns["FullNameAndAddress"].Width = 250;
                dgvdata.Columns["CBCP"].HeaderText = "Contact Number";
                dgvdata.Columns["Documents"].HeaderText = "Attached Documents";
                dgvdata.Columns["Documents"].Width = 275;

                // Set font size and style for the entire DataGridView
                dgvdata.DefaultCellStyle.Font = new Font("Arial", 9);
                dgvdata.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);

                // Center align specific columns
                CenterAlignColumns("AccountID", "LoanType", "Principal", "Term", "CBCP");

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

                if (dgvdata.Columns["btnViewDetails"] == null)
                {
                    DataGridViewButtonColumn viewDetailsButtonColumn = new DataGridViewButtonColumn
                    {
                        Name = "btnViewDetails",
                        HeaderText = "Action",
                        Text = "View Details",
                        UseColumnTextForButtonValue = true,
                        FlatStyle = FlatStyle.Standard,
                    };

                    dgvdata.Columns.Add(viewDetailsButtonColumn);
                }

                // Set width, padding, and font size for the button column to avoid large size
                var btnColumn = dgvdata.Columns["btnViewDetails"];
                if (btnColumn != null)
                {
                    btnColumn.Width = 120;

                    // Set padding for the button column to avoid large size
                    foreach (DataGridViewRow row in dgvdata.Rows)
                    {
                        DataGridViewButtonCell buttonCell = row.Cells["btnViewDetails"] as DataGridViewButtonCell;
                        if (buttonCell != null)
                        {
                            buttonCell.Style.Padding = new Padding(2, 2, 2, 2); // Adjust padding to reduce button size
                            buttonCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter; // Center-align the button
                            buttonCell.Style.Font = new Font("Arial", 9); // Adjust font size if needed
                        }
                    }
                }

               

                // Add the "Disburse" button column if it doesn't exist
                if (dgvdata.Columns["btnDisburse"] == null)
                {
                    DataGridViewButtonColumn disburseButtonColumn = new DataGridViewButtonColumn
                    {
                        Name = "btnDisburse",
                        HeaderText = " ",
                        Text = "Disburse",
                        UseColumnTextForButtonValue = true,
                        FlatStyle = FlatStyle.Standard
                        
                    };

                    dgvdata.Columns.Add(disburseButtonColumn);
                }

                // Set width, padding, and font size for the "Disburse" button column
                var disburseColumn = dgvdata.Columns["btnDisburse"];
                if (disburseColumn != null)
                {
                    disburseColumn.Width = 120;

                    foreach (DataGridViewRow row in dgvdata.Rows)
                    {
                        DataGridViewButtonCell disburseButtonCell = row.Cells["btnDisburse"] as DataGridViewButtonCell;
                        if (disburseButtonCell != null)
                        {
                            disburseButtonCell.Style.Padding = new Padding(2, 2, 2, 2); // Adjust padding to reduce button size
                            disburseButtonCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter; // Center-align the button
                            disburseButtonCell.Style.Font = new Font("Arial", 9); // Adjust font size if needed
                        }
                    }
                }

               
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading approved loans data: " + ex.Message);
                MessageBox.Show("Error loading approved loans data. Please check the console for details.");
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

        }

        private void dgvdata_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
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
              
                if (dgvdata.Columns[e.ColumnIndex].Name == "btnViewDetails")
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
                    frm_home_loan_disburse fdis = new frm_home_loan_disburse
                    {
                        AccountID = selectedAccountId
                    };
                    load.Show(this);
                    Thread.Sleep(500);
                    load.Close();
                    fdis.Show(this);
                }
            }
        }
    }
}

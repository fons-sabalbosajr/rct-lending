using MongoDB.Bson;
using MongoDB.Driver;
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

namespace rct_lmis.LOAN_SECTION
{
    public partial class frm_home_loan_request : Form
    {

        private string loggedInUsername;

        public frm_home_loan_request()
        {
            InitializeComponent();
            LoadLoanApplicationsData();

            loggedInUsername = UserSession.Instance.CurrentUser;
        }

        LoadingFunction load = new LoadingFunction();

        private void LoadLoanApplicationsData()
        {
            try
            {
                // Access the loan_application collection
                var database = MongoDBConnection.Instance.Database;
                var loanAppCollection = database.GetCollection<BsonDocument>("loan_application");

                // Retrieve all documents
                var documents = loanAppCollection.Find(new BsonDocument()).ToList();

                // Create a DataTable to hold the data
                DataTable dataTable = new DataTable();

                // Define the columns to display
                dataTable.Columns.Add("AccountID");
                dataTable.Columns.Add("LoanType");
                dataTable.Columns.Add("Principal");
                dataTable.Columns.Add("Term");
                dataTable.Columns.Add("Status");
                dataTable.Columns.Add("FullNameAndAddress");
                dataTable.Columns.Add("CBCP");
                dataTable.Columns.Add("Documents");

                // Add rows to the DataTable
                foreach (var doc in documents)
                {
                    DataRow row = dataTable.NewRow();
                    row["AccountID"] = doc.Contains("AccountId") ? doc["AccountId"].ToString() : string.Empty;
                    row["LoanType"] = doc.Contains("LoanType") ? doc["LoanType"].ToString() : string.Empty;
                    row["Principal"] = doc.Contains("Principal") ? "₱ " + doc["Principal"].ToString() + ".00" : string.Empty;
                    row["Term"] = doc.Contains("Term") ? doc["Term"].ToString() + " month/s" : string.Empty;
                    row["Status"] = doc.Contains("LoanStatus") ? doc["LoanStatus"].ToString() : string.Empty;

                    string firstName = doc.Contains("FirstName") ? doc["FirstName"].ToString() : string.Empty;
                    string middleName = doc.Contains("MiddleName") ? doc["MiddleName"].ToString() : string.Empty;
                    string lastName = doc.Contains("LastName") ? doc["LastName"].ToString() : string.Empty;
                    string suffixName = doc.Contains("SuffixName") ? doc["SuffixName"].ToString() : string.Empty;
                    string fullName = $"{firstName} {middleName} {lastName} {suffixName}";

                    string street = doc.Contains("Street") ? doc["Street"].ToString() : string.Empty;
                    string barangay = doc.Contains("Barangay") ? doc["Barangay"].ToString() : string.Empty;
                    string city = doc.Contains("City") ? doc["City"].ToString() : string.Empty;
                    string province = doc.Contains("Province") ? doc["Province"].ToString() : string.Empty;
                    string address = $"{street}\n{barangay}\n{city}\n{province}";

                    // Concatenate full name and address with line breaks
                    row["FullNameAndAddress"] = $"{fullName}\n{address}";

                    row["CBCP"] = doc.Contains("CBCP") ? doc["CBCP"].ToString() : string.Empty;

                    // Split the documents into separate lines based on the comma separator
                    if (doc.Contains("docs"))
                    {
                        var documentsList = doc["docs"].ToString().Split(',');
                        row["Documents"] = string.Join("\n", documentsList);
                    }
                    else
                    {
                        row["Documents"] = string.Empty;
                    }

                    dataTable.Rows.Add(row);
                }

                // Apply keyword filtering based on the tsearch.Text textbox value
                string keyword = tsearch.Text.Trim().ToLower();  // Convert to lowercase for case-insensitive filtering

                if (!string.IsNullOrEmpty(keyword))
                {
                    // Filter the rows based on the keyword in relevant columns
                    var filteredRows = dataTable.AsEnumerable()
                        .Where(row => row["AccountID"].ToString().ToLower().Contains(keyword) ||
                                      row["LoanType"].ToString().ToLower().Contains(keyword) ||
                                      row["Principal"].ToString().ToLower().Contains(keyword) ||
                                      row["Term"].ToString().ToLower().Contains(keyword) ||
                                      row["Status"].ToString().ToLower().Contains(keyword) ||
                                      row["FullNameAndAddress"].ToString().ToLower().Contains(keyword) ||
                                      row["CBCP"].ToString().ToLower().Contains(keyword) ||
                                      row["Documents"].ToString().ToLower().Contains(keyword));

                    // Create a new DataTable to bind filtered data to the DataGridView
                    if (filteredRows.Any())
                    {
                        dataTable = filteredRows.CopyToDataTable();
                    }
                    else
                    {
                        // If no matching rows, clear the DataTable
                        dataTable.Rows.Clear();
                    }
                }

                // Bind the DataTable to the DataGridView
                dgvloanapps.DataSource = dataTable;

                // Set custom header texts and other configurations (same as before)
                dgvloanapps.Columns["AccountID"].HeaderText = "Account ID";
                dgvloanapps.Columns["LoanType"].HeaderText = "Loan Type";
                dgvloanapps.Columns["LoanType"].Width = 70;
                dgvloanapps.Columns["Principal"].HeaderText = "Principal Amount";
                dgvloanapps.Columns["Principal"].Width = 100;
                dgvloanapps.Columns["Term"].HeaderText = "Loan Term";
                dgvloanapps.Columns["Status"].HeaderText = "Application Status";
                dgvloanapps.Columns["FullNameAndAddress"].HeaderText = "Client Name";
                dgvloanapps.Columns["FullNameAndAddress"].Width = 250;
                dgvloanapps.Columns["CBCP"].HeaderText = "Contact Number";
                dgvloanapps.Columns["Documents"].HeaderText = "Attached Documents";
                dgvloanapps.Columns["Documents"].Width = 275;

                // Set font size and style for the entire DataGridView
                dgvloanapps.DefaultCellStyle.Font = new Font("Arial", 9);
                dgvloanapps.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);

                // Center align specific columns
                CenterAlignColumns("AccountID", "LoanType", "Principal", "Term", "CBCP");

                lnorecord.Visible = dgvloanapps.Rows.Count == 0;

                // Configure the DataGridView for the FullNameAndAddress column
                if (dgvloanapps.Columns["FullNameAndAddress"] != null)
                {
                    dgvloanapps.Columns["FullNameAndAddress"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    dgvloanapps.Columns["FullNameAndAddress"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                // Configure the Documents column to be a link type
                if (dgvloanapps.Columns["Documents"] != null)
                {
                    dgvloanapps.Columns["Documents"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    dgvloanapps.Columns["Documents"].ReadOnly = true;
                    dgvloanapps.Columns["Documents"].SortMode = DataGridViewColumnSortMode.Automatic;
                }

                // Add the button column for "View Details"
                if (dgvloanapps.Columns["btnViewDetails"] == null)
                {
                    DataGridViewButtonColumn viewDetailsButtonColumn = new DataGridViewButtonColumn
                    {
                        Name = "btnViewDetails",
                        HeaderText = "View Details",
                        Text = "View Details",
                        UseColumnTextForButtonValue = true,
                        FlatStyle = FlatStyle.Standard
                    };

                    dgvloanapps.Columns.Add(viewDetailsButtonColumn);
                }

                // Set width, padding, and font size for the button column
                var btnColumn = dgvloanapps.Columns["btnViewDetails"];
                if (btnColumn != null)
                {
                    btnColumn.Width = 120;
                }

                // Set padding for the button column
                foreach (DataGridViewRow row in dgvloanapps.Rows)
                {
                    DataGridViewButtonCell buttonCell = row.Cells["btnViewDetails"] as DataGridViewButtonCell;
                    if (buttonCell != null)
                    {
                        buttonCell.Style.Padding = new Padding(30, 15, 30, 15);
                        buttonCell.Style.Font = new Font("Arial", 9);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading loan applications data: " + ex.Message);
                MessageBox.Show("Error loading loan applications data. Please check the console for details.");
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

        private void CenterAlignColumns(params string[] columnNames)
        {
            foreach (string columnName in columnNames)
            {
                if (dgvloanapps.Columns[columnName] != null)
                {
                    dgvloanapps.Columns[columnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        private void frm_home_loan_request_Load(object sender, EventArgs e)
        {
            LoadLoanApplicationsData();
            LoadUserInfo(loggedInUsername);

            ltotalloancount.Text = dgvloanapps.Rows.Count.ToString();
        }

        private void dgvloanapps_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvloanapps.ClearSelection();
        }

        private void dgvloanapps_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvloanapps.Columns["btnViewDetails"].Index && e.RowIndex >= 0)
            {
                string accountId = dgvloanapps.Rows[e.RowIndex].Cells["AccountID"].Value.ToString();

                frm_home_loand_req_details req = new frm_home_loand_req_details(accountId);
                load.Show(this);
                Thread.Sleep(2000);
                load.Close();
                req.Show();
            }
        }

        private void dgvloanapps_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvloanapps.Columns[e.ColumnIndex].Name == "Status")
            {
                var statusValue = e.Value?.ToString();
                if (statusValue == "Approved Loan")
                {
                    e.CellStyle.BackColor = Color.LightGreen;
                    e.CellStyle.ForeColor = Color.Black;
                }
            }
        }

        private void dgvloanapps_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void frm_home_loan_request_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            
        }

        private void frm_home_loan_request_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
        }

        private void tsearch_TextChanged(object sender, EventArgs e)
        {
            LoadLoanApplicationsData();
        }
    }
}

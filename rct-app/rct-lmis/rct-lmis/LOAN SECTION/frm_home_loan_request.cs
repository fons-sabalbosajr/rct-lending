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

                // Define the merged columns
                dataTable.Columns.Add("AccountID");
                dataTable.Columns.Add("LoanDetails");
                dataTable.Columns.Add("ClientDetails");
                dataTable.Columns.Add("LoanStatus");
                dataTable.Columns.Add("OtherDetails");
                dataTable.Columns.Add("UploadedDocuments");

                // Add rows to the DataTable
                foreach (var doc in documents)
                {
                    DataRow row = dataTable.NewRow();

                    // Account Info
                    row["AccountID"] = doc.Contains("AccountId") ? doc["AccountId"].ToString().Replace("Account ID: ", "") : string.Empty;

                    // Loan Details: Merge LoanBalance, LoanCycle, LoanTerms, PreviousLoan, PaymentAmount, etc.
                    string loanBalance = doc.Contains("LoanBalance") ? "₱ " + doc["LoanBalance"].ToString() + ".00" : string.Empty;
                    string loanCycle = doc.Contains("LoanCycle") ? doc["LoanCycle"].ToString() : string.Empty;
                    string loanTerms = doc.Contains("LoanTerms") ? doc["LoanTerms"].ToString() + " months" : string.Empty;
                    string previousLoan = doc.Contains("PreviousLoan") ? doc["PreviousLoan"].ToString() : string.Empty;
                    string paymentAmount = doc.Contains("PaymentAmount") ? "₱ " + doc["PaymentAmount"].ToString() : string.Empty;
                    string paymentMode = doc.Contains("PaymentMode") ? doc["PaymentMode"].ToString() : string.Empty;
                    string renewalStatus = doc.Contains("RenewalStatus") ? doc["RenewalStatus"].ToString() : string.Empty;

                    row["LoanDetails"] = $"Balance: {loanBalance}\nCycle: {loanCycle} month(s)\nTerms: {loanTerms}\nPrevious Loan: {previousLoan}\nPayment Amount: {paymentAmount}\nPayment Mode: {paymentMode}\nRenewal Status: {renewalStatus}";

                    // Client Details: Merge ClientName, CloseLoanDate, DateEvaluated, and Amendments
                    string clientName = doc.Contains("ClientName") ? doc["ClientName"].ToString() : string.Empty;
                    string closeLoanDate = doc.Contains("CloseLoanDate") ? doc["CloseLoanDate"].ToString() : string.Empty;
                    string dateEvaluated = doc.Contains("DateEvaluated") ? doc["DateEvaluated"].ToString() : string.Empty;
                    string daysMissed = doc.Contains("DaysMissed") ? doc["DaysMissed"].ToString() : string.Empty;

                    string amendments = $"Amended From - Applicant: {doc["AmendedFromApplicant"]}\n" +
                                        $"Amended From - Borrower: {doc["AmendedFromBorrower"]}\n" +
                                        $"Amended From - Maker: {doc["AmendedFromMaker"]}\n" +
                                        $"Amended To - Applicant: {doc["AmendedToApplicant"]}\n" +
                                        $"Amended To - Borrower: {doc["AmendedToBorrower"]}\n" +
                                        $"Amended To - Maker: {doc["AmendedToMaker"]}";

                    row["ClientDetails"] = $"Client: {clientName}\nClose Loan Date: {closeLoanDate}\nDate Evaluated: {dateEvaluated}\nDays Missed: {daysMissed}\n\n{amendments}";

                    // Loan Status: Combine ORNumber, ReleaseSchedule, Remarks, Savings
                    string orNumber = doc.Contains("ORNumber") ? doc["ORNumber"].ToString() : string.Empty;
                    string releaseSchedule = doc.Contains("ReleaseSchedule") ? doc["ReleaseSchedule"].ToString() : string.Empty;
                    string remarks = doc.Contains("Remarks") ? doc["Remarks"].ToString() : string.Empty;
                    string savings = doc.Contains("Savings") ? doc["Savings"].ToString() : string.Empty;

                    row["LoanStatus"] = $"OR Number: {orNumber}\nRelease Schedule: {releaseSchedule}\nRemarks: {remarks}\nSavings: {savings}";

                    // Other Details: Combine SimilarApplicant, SimilarBorrower, SimilarMaker
                    bool similarApplicant = doc.Contains("SimilarApplicant") ? Convert.ToBoolean(doc["SimilarApplicant"]) : false;
                    bool similarBorrower = doc.Contains("SimilarBorrower") ? Convert.ToBoolean(doc["SimilarBorrower"]) : false;
                    bool similarMaker = doc.Contains("SimilarMaker") ? Convert.ToBoolean(doc["SimilarMaker"]) : false;

                    row["OtherDetails"] = $"Similar Applicant: {similarApplicant}\nSimilar Borrower: {similarBorrower}\nSimilar Maker: {similarMaker}";

                    // Documents: Merge document names
                    if (doc.Contains("UploadedDocs"))
                    {
                        var documentsList = doc["UploadedDocs"].AsBsonArray.Select(d => d["file_name"].ToString());
                        row["UploadedDocuments"] = string.Join("\n", documentsList);
                    }
                    else
                    {
                        row["UploadedDocuments"] = string.Empty;
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
                                      row["LoanDetails"].ToString().ToLower().Contains(keyword) ||
                                      row["ClientDetails"].ToString().ToLower().Contains(keyword) ||
                                      row["LoanStatus"].ToString().ToLower().Contains(keyword) ||
                                      row["OtherDetails"].ToString().ToLower().Contains(keyword) ||
                                      row["UploadedDocuments"].ToString().ToLower().Contains(keyword));

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

                // Set custom header texts and other configurations (merged columns)
                dgvloanapps.Columns["AccountID"].HeaderText = "Account ID";
                dgvloanapps.Columns["LoanDetails"].HeaderText = "Loan Details";
                dgvloanapps.Columns["ClientDetails"].HeaderText = "Client Details";
                dgvloanapps.Columns["LoanStatus"].HeaderText = "Loan Status";
                dgvloanapps.Columns["OtherDetails"].HeaderText = "Other Details";
                dgvloanapps.Columns["UploadedDocuments"].HeaderText = "Uploaded Documents";

                // Set widths of the columns
                dgvloanapps.Columns["LoanDetails"].Width = 200;
                dgvloanapps.Columns["ClientDetails"].Width = 250;
                dgvloanapps.Columns["LoanStatus"].Width = 200;
                dgvloanapps.Columns["OtherDetails"].Width = 180;
                dgvloanapps.Columns["UploadedDocuments"].Width = 275;

                // Align all columns to the left and add padding
                foreach (DataGridViewColumn column in dgvloanapps.Columns)
                {
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    column.DefaultCellStyle.Padding = new Padding(5, 0, 0, 0); // Set left padding
                }



                // Set font size and style for the entire DataGridView
                dgvloanapps.DefaultCellStyle.Font = new Font("Arial", 9);
                dgvloanapps.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);

                // Center align specific columns
                CenterAlignColumns("AccountID", "LoanDetails", "LoanStatus");

                lnorecord.Visible = dgvloanapps.Rows.Count == 0;

                // Configure the DataGridView for the merged ClientDetails column
                if (dgvloanapps.Columns["ClientDetails"] != null)
                {
                    dgvloanapps.Columns["ClientDetails"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    dgvloanapps.Columns["ClientDetails"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                // Configure the UploadedDocuments column to be a link type
                if (dgvloanapps.Columns["UploadedDocuments"] != null)
                {
                    dgvloanapps.Columns["UploadedDocuments"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    dgvloanapps.Columns["UploadedDocuments"].ReadOnly = true;
                    dgvloanapps.Columns["UploadedDocuments"].SortMode = DataGridViewColumnSortMode.Automatic;
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

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
                var database = MongoDBConnection.Instance.Database;
                var loanAppCollection = database.GetCollection<BsonDocument>("loan_application");
                var documents = loanAppCollection.Find(new BsonDocument()).ToList();

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("AccountID");
                dataTable.Columns.Add("LoanDetails");
                dataTable.Columns.Add("ClientDetails");
                dataTable.Columns.Add("LoanStatus");
                dataTable.Columns.Add("UploadedDocuments", typeof(string));

                foreach (var doc in documents)
                {
                    DataRow row = dataTable.NewRow();
                    row["AccountID"] = doc.Contains("AccountId") ? doc["AccountId"].ToString().Replace("Account ID: ", "") : string.Empty;

                    // Loan Details
                    string loanDetails = string.Empty;
                    AppendDetail(ref loanDetails, "Balance", doc, "LoanBalance", "₱ {0}.00");
                    AppendDetail(ref loanDetails, "Cycle", doc, "LoanCycle", "{0} month(s)");
                    AppendDetail(ref loanDetails, "Terms", doc, "LoanTerms", "{0} months");
                    AppendDetail(ref loanDetails, "Previous Loan", doc, "PreviousLoan");
                    AppendDetail(ref loanDetails, "Payment Amount", doc, "PaymentAmount", "₱ {0}");
                    AppendDetail(ref loanDetails, "Payment Mode", doc, "PaymentMode");
                    row["LoanDetails"] = loanDetails;

                    // Client Details
                    string clientDetails = string.Empty;
                    AppendDetail(ref clientDetails, "Client", doc, "ClientName");
                    AppendDetail(ref clientDetails, "Close Loan Date", doc, "CloseLoanDate");
                    AppendDetail(ref clientDetails, "Date Evaluated", doc, "DateEvaluated");
                    AppendDetail(ref clientDetails, "Days Missed", doc, "DaysMissed");
                    row["ClientDetails"] = clientDetails;

                    // Loan Status (Set default if missing)
                    string loanStatus = doc.Contains("LoanStatus") ? doc["LoanStatus"].ToString() : "FOR VERIFICATION AND APPROVAL";
                    row["LoanStatus"] = loanStatus;

                    // Uploaded Documents
                    if (doc.Contains("UploadedDocs"))
                    {
                        var documentsList = doc["UploadedDocs"].AsBsonArray
                            .Select(d => d["file_name"].ToString())
                            .ToList();
                        row["UploadedDocuments"] = string.Join(", ", documentsList);
                    }
                    else
                    {
                        row["UploadedDocuments"] = string.Empty;
                    }

                    dataTable.Rows.Add(row);
                }

                lnorecord.Visible = false;
                // Assign the DataTable to DataGridView
                dgvloanapps.DataSource = dataTable;
                dgvloanapps.Columns["AccountID"].HeaderText = "Account ID";
                dgvloanapps.Columns["LoanDetails"].HeaderText = "Loan Details";
                dgvloanapps.Columns["ClientDetails"].HeaderText = "Client Details";
                dgvloanapps.Columns["LoanStatus"].HeaderText = "Loan Status";
                dgvloanapps.Columns["UploadedDocuments"].HeaderText = "Uploaded Documents";
                dgvloanapps.Columns["UploadedDocuments"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                // Ensure Action Button is only added once
                if (!dgvloanapps.Columns.Contains("btnViewDetails"))
                {
                    DataGridViewButtonColumn btnViewDetails = new DataGridViewButtonColumn
                    {
                        Name = "btnViewDetails",
                        HeaderText = "Actions",
                        Text = "View Details",
                        UseColumnTextForButtonValue = true,
                        FlatStyle = FlatStyle.Standard,
                    };
                    btnViewDetails.DefaultCellStyle.Padding = new Padding(100, 35, 100, 35);
                    dgvloanapps.Columns.Add(btnViewDetails);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading loan applications data: " + ex.Message);
                MessageBox.Show("Error loading loan applications data. Please check the console for details.");
            }
        }


        private void AppendDetail(ref string details, string label, BsonDocument doc, string field, string format = "{0}")
        {
            if (doc.Contains(field) && !string.IsNullOrEmpty(doc[field].ToString()))
            {
                if (!string.IsNullOrEmpty(details)) details += "\n";
                details += $"{label}: " + string.Format(format, doc[field].ToString());
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
                string accountId = dgvloanapps.Rows[e.RowIndex].Cells["AccountID"].Value?.ToString();

                if (!string.IsNullOrEmpty(accountId))
                {
                  
                    frm_home_loand_req_details req = new frm_home_loand_req_details(accountId);
                    load.Show(this);
                    Thread.Sleep(1000);
                    load.Close();
                    req.Show();
                }
                else
                {
                    MessageBox.Show("Invalid Account ID.");
                }
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

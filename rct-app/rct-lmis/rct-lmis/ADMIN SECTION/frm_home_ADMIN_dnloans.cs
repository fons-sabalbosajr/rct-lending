using DnsClient.Protocol;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rct_lmis.ADMIN_SECTION
{
    public partial class frm_home_ADMIN_dnloans : Form
    {

        private string loggedInUsername;


        public frm_home_ADMIN_dnloans()
        {
            InitializeComponent();
            dtdateDenied.Value = DateTime.Today;
            loggedInUsername = UserSession.Instance.CurrentUser;
        }

        private void SetupDataGridView()
        {
            dgvloandata.Columns.Clear(); // Clear any existing columns to avoid duplication

            dgvloandata.Columns.Add("ClientInfo", "Client Details");
            dgvloandata.Columns.Add("LoanDetails", "Loan Details");
            dgvloandata.Columns.Add("LoanStatusDetails", "Loan Status");
            dgvloandata.Columns.Add("Countdown", "Auto Remove");

            var viewDetailsColumn = new DataGridViewButtonColumn
            {
                Name = "ViewDetails",
                HeaderText = "View Details",
                Text = "View Details",
                UseColumnTextForButtonValue = true,
                Width = 50 // Adjust width as needed
            };

            dgvloandata.Columns.Add(viewDetailsColumn);

            // ✅ Set left and right padding for the "View Details" button
            dgvloandata.Columns["ViewDetails"].DefaultCellStyle.Padding = new Padding(100, 5, 100, 5);

            // Ensure proper row height adjustment for multi-line content
            dgvloandata.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvloandata.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dgvloandata.Columns["Countdown"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }


        private void FilterDeniedLoansByStatus(string status)
        {
            try
            {
                DeleteExpiredLoans();
                var database = MongoDBConnection.Instance.Database;
                var loanDeniedCollection = database.GetCollection<BsonDocument>("loan_denied");

                // Apply filtering for loan status
                var filter = Builders<BsonDocument>.Filter.Eq("LoanStatus", status);

                var deniedLoans = loanDeniedCollection.Find(filter).ToList();
                BindDeniedLoansToDataGridView(deniedLoans);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filtering denied loans by status: " + ex.Message);
            }
        }

        private void FilterLoansByDate(DateTime selectedDate)
        {
            try
            {
                DeleteExpiredLoans();
                var database = MongoDBConnection.Instance.Database;
                var loanDeniedCollection = database.GetCollection<BsonDocument>("loan_denied");
                var startOfDay = selectedDate.Date;
                var endOfDay = startOfDay.AddDays(1).AddTicks(-1);

                var filter = Builders<BsonDocument>.Filter.Gte("DeniedDate", startOfDay) &
                             Builders<BsonDocument>.Filter.Lte("DeniedDate", endOfDay);

                if (cbstatus.SelectedItem != null)
                {
                    var statusFilter = Builders<BsonDocument>.Filter.Eq("LoanStatus", cbstatus.SelectedItem.ToString());
                    filter = filter & statusFilter;
                }

                var deniedLoans = loanDeniedCollection.Find(filter).ToList();

                if (dgvloandata.Columns.Count == 0)
                {
                    SetupDataGridView();
                }

                BindDeniedLoansToDataGridView(deniedLoans);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filtering loans by date: " + ex.Message);
            }
        }

        private void LoadAllDeniedLoans()
        {
            try
            {
                DeleteExpiredLoans();
                var database = MongoDBConnection.Instance.Database;
                var loanDeniedCollection = database.GetCollection<BsonDocument>("loan_denied");
                var deniedLoans = loanDeniedCollection.Find(Builders<BsonDocument>.Filter.Empty).ToList();
                BindDeniedLoansToDataGridView(deniedLoans);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading denied loans: " + ex.Message);
            }
        }

        private void DeleteExpiredLoans()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var loanDeniedCollection = database.GetCollection<BsonDocument>("loan_denied");
                var expirationThreshold = DateTime.UtcNow.AddDays(-30);
                var filter = Builders<BsonDocument>.Filter.Lt("DeniedDate", expirationThreshold);
                var result = loanDeniedCollection.DeleteMany(filter);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting expired loans: " + ex.Message);
            }
        }

        private void BindDeniedLoansToDataGridView(List<BsonDocument> deniedLoans)
        {
            dgvloandata.Rows.Clear();

            // Enable multi-line text
            dgvloandata.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvloandata.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            foreach (var loan in deniedLoans)
            {
                string clientInfo = $"Name: {loan.GetValue("ClientName", "").AsString}\r\n" +
                                    $"Address: {loan.GetValue("Address", "").AsString}";

                string loanDetails = $"Loan Amount: {loan.GetValue("LoanAmount", "").AsString}\r\n" +
                                     $"Description: {loan.GetValue("LoanDescription", "").AsString}";

                string loanStatusDetails = $"Status: {loan.GetValue("LoanStatus", "").AsString}\r\n" +
                                           $"Denied Date: {(loan.Contains("DeniedDate") ? loan["DeniedDate"].ToUniversalTime().ToString("MM/dd/yyyy") : "N/A")}\r\n" +
                                           $"Denied By: {loan.GetValue("DeniedBy", "N/A").AsString}";

                string countdown = loan.Contains("DeniedDate") ? CalculateCountdown(loan["DeniedDate"].ToUniversalTime()) : "Unknown";

                dgvloandata.Rows.Add(clientInfo, loanDetails, loanStatusDetails, countdown, "View Details");
            }
        }

        private string CalculateCountdown(DateTime denialDate)
        {
            var expirationDate = denialDate.AddDays(30);
            var remainingDays = (expirationDate - DateTime.UtcNow).Days;
            return remainingDays >= 0 ? $"{remainingDays} days remaining" : "Expired";
        }

        private void LoadUserInfo(string username)
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<BsonDocument>("user_accounts"); // 'user_accounts' is the name of your collection

            var filter = Builders<BsonDocument>.Filter.Eq("Username", username);
            var user = collection.Find(filter).FirstOrDefault();

            if (user != null)
            {
                // Get the full name and split to get the first name
                var fullName = user.GetValue("FullName").AsString;

                // Set the first name
                luser.Text = fullName;
            }
        }

        private void frm_home_ADMIN_dnloans_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            LoadAllDeniedLoans();
            //FilterLoansByDate(dtdateDenied.Value.Date);
            LoadUserInfo(loggedInUsername);
        }

        private void dtdateDenied_ValueChanged(object sender, EventArgs e)
        {
            FilterLoansByDate(dtdateDenied.Value.Date);
        }

        private void cbstatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var selectedStatus = cbstatus.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(selectedStatus) || selectedStatus == "No data")
                {
                    LoadAllDeniedLoans();
                }
                else
                {
                    FilterDeniedLoansByStatus(selectedStatus);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filtering loans by status: " + ex.Message);
            }
        }

        private void dgvloandata_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvloandata.ClearSelection();
        }

        private void brevert_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to revert the loan account and restore it to pending applications?",
                "Restore Account", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if (dgvloandata.SelectedRows.Count == 0)
                    {
                        MessageBox.Show("Please select a loan to revert.");
                        return;
                    }

                    var selectedRow = dgvloandata.SelectedRows[0];

                    // Extract Client Name to search for the loan
                    string clientInfo = selectedRow.Cells["ClientInfo"].Value.ToString();
                    string clientName = clientInfo.Split('\n')[0].Replace("Name: ", "").Trim();

                    var database = MongoDBConnection.Instance.Database;
                    var loanDeniedCollection = database.GetCollection<BsonDocument>("loan_denied");
                    var loanApplicationsCollection = database.GetCollection<BsonDocument>("loan_application");

                    // Find the loan document in loan_denied using ClientName
                    var filter = Builders<BsonDocument>.Filter.Eq("ClientName", clientName);
                    var deniedLoan = loanDeniedCollection.Find(filter).FirstOrDefault();

                    if (deniedLoan == null)
                    {
                        MessageBox.Show("Error: Loan not found in denied loans.");
                        return;
                    }

                    // Extract Denied Date before removing it
                    string deniedDate = deniedLoan.Contains("DeniedDate")
                        ? deniedLoan["DeniedDate"].ToUniversalTime().ToString("MM/dd/yyyy")
                        : "Unknown";

                    // Find the existing loan in loan_application
                    var existingLoanFilter = Builders<BsonDocument>.Filter.Eq("ClientName", clientName);
                    var existingLoan = loanApplicationsCollection.Find(existingLoanFilter).FirstOrDefault();

                    if (existingLoan == null)
                    {
                        MessageBox.Show("Error: Loan account not found in loan applications.");
                        return;
                    }

                    // Prepare update fields
                    var updateDefinition = Builders<BsonDocument>.Update
                        .Set("LoanStatus", "LOAN APPROVED")
                        .Set("ApprovalDate", DateTime.UtcNow) // Set the current date and time
                        .Set("ApprovedBy", luser.Text) // Replace with actual logged-in user
                        .Set("Remarks", $"Previously denied on {deniedDate}.");

                    // Apply the update to loan_application
                    loanApplicationsCollection.UpdateOne(existingLoanFilter, updateDefinition);

                    // Remove from loan_denied collection
                    loanDeniedCollection.DeleteOne(filter);

                    // Refresh DataGridView
                    LoadAllDeniedLoans();

                    MessageBox.Show($"Loan for {clientName} has been successfully restored and updated.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error reverting loan: " + ex.Message);
                }
            }
        }


    }
}

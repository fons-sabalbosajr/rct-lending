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
using System.Text.RegularExpressions;
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

        
        private async 
        
        Task
LoadApprovedLoansData(string loanStatusFilter = "--All Status--", string selectedYear = "")
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var approvedLoansCollection = database.GetCollection<BsonDocument>("loan_approved");
                var applicationsCollection = database.GetCollection<BsonDocument>("loan_applications");

                var filters = new List<FilterDefinition<BsonDocument>>();

                // Apply Loan Status filter
                if (loanStatusFilter != "--All Status--")
                    filters.Add(Builders<BsonDocument>.Filter.Eq("LoanStatus", loanStatusFilter));

                // Apply Year filter (based on selectedYear from cbyear)
                if (!string.IsNullOrEmpty(selectedYear) && selectedYear != "--All Year--")
                {
                    filters.Add(Builders<BsonDocument>.Filter.Regex("AccountId", new BsonRegularExpression($"RCT-{selectedYear}DB", "i")));
                }

                var finalFilter = filters.Count > 0 ? Builders<BsonDocument>.Filter.And(filters) : Builders<BsonDocument>.Filter.Empty;

                var approvedDocuments = approvedLoansCollection.Find(finalFilter).ToList();
                var applicationDocuments = applicationsCollection.Find(new BsonDocument()).ToList();


                // Loan status count tracker
                Dictionary<string, int> loanTypeCounts = new Dictionary<string, int>
                  {
                     { "UPDATED", 0 },
                     { "PAST DUE", 0 },
                     { "ARREARS", 0 },
                     { "LITIGATION", 0 },
                     { "DORMANT", 0 },
                     { "DEFAULT", 0 }
                 };

                // DataTable setup
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("AccountID");
                dataTable.Columns.Add("LoanType");
                dataTable.Columns.Add("PrincipalAmount");
                dataTable.Columns.Add("LoanTerm");
                dataTable.Columns.Add("LoanProcessStatus");
                dataTable.Columns.Add("FullNameAndAddress");
                dataTable.Columns.Add("CBCP");
                dataTable.Columns.Add("Documents");
                dataTable.Columns.Add("SortKey", typeof(int)); // Added column for numeric sort

                foreach (var approvedDoc in approvedDocuments)
                {
                    DataRow row = dataTable.NewRow();

                    var accountId = approvedDoc.Contains("AccountId") ? approvedDoc["AccountId"].ToString() : string.Empty;
                    row["AccountID"] = accountId;

                    string loanType = approvedDoc.Contains("LoanStatus") ? approvedDoc["LoanStatus"].ToString() : "DEFAULT";
                    row["LoanType"] = loanType;
                    if (loanTypeCounts.ContainsKey(loanType))
                        loanTypeCounts[loanType]++;
                    else
                        loanTypeCounts["DEFAULT"]++;

                    row["PrincipalAmount"] = approvedDoc.Contains("LoanAmount") ? approvedDoc["LoanAmount"].ToString() : string.Empty;
                    row["LoanTerm"] = approvedDoc.Contains("LoanTerm") ? approvedDoc["LoanTerm"].ToString() : string.Empty;

                    // Application document lookup
                    var applicationDoc = applicationDocuments.FirstOrDefault(doc => doc.Contains("AccountId") && doc["AccountId"].ToString() == accountId);
                    row["LoanProcessStatus"] = approvedDoc.Contains("LoanProcessStatus") ? approvedDoc["LoanProcessStatus"].ToString() : string.Empty;
                    row["Documents"] = applicationDoc != null && applicationDoc.Contains("docs") ? applicationDoc["docs"].ToString() : string.Empty;

                    // Full name & address
                    string fullName = $"{approvedDoc.GetValue("FirstName", "")} {approvedDoc.GetValue("MiddleName", "")} {approvedDoc.GetValue("LastName", "")} {approvedDoc.GetValue("SuffixName", "")}".Trim();
                    string address = $"{approvedDoc.GetValue("Street", "")}, {approvedDoc.GetValue("Barangay", "")}, {approvedDoc.GetValue("City", "")}, {approvedDoc.GetValue("Province", "")}".Trim();
                    row["FullNameAndAddress"] = $"{fullName}\n{address}";

                    row["CBCP"] = approvedDoc.Contains("CBCP") ? approvedDoc["CBCP"].ToString() : string.Empty;

                    // Format documents
                    if (approvedDoc.Contains("docs"))
                    {
                        var documentsList = approvedDoc["docs"].ToString().Split(',');
                        row["Documents"] = string.Join("\n", documentsList);
                    }

                    // Compute numeric SortKey
                    var match = Regex.Match(accountId, @"(\d+)$");
                    if (match.Success && int.TryParse(match.Value, out int sortVal))
                        row["SortKey"] = sortVal;
                    else
                        row["SortKey"] = int.MaxValue;

                    dataTable.Rows.Add(row);
                }

                // Sort using numeric SortKey
                DataView sortedView = dataTable.DefaultView;
                sortedView.Sort = "SortKey ASC";
                dgvdata.DataSource = sortedView.ToTable();

                // Hide SortKey column
                if (dgvdata.Columns.Contains("SortKey"))
                    dgvdata.Columns["SortKey"].Visible = false;

                // Setup column headers
                dgvdata.Columns["AccountID"].HeaderText = "Client No.";
                dgvdata.Columns["LoanType"].HeaderText = "Loan Type";
                dgvdata.Columns["PrincipalAmount"].HeaderText = "Principal Amount";
                dgvdata.Columns["LoanTerm"].HeaderText = "Loan Term";
                dgvdata.Columns["LoanProcessStatus"].HeaderText = "Loan Status";
                dgvdata.Columns["FullNameAndAddress"].HeaderText = "Client Name";
                dgvdata.Columns["CBCP"].HeaderText = "Contact Number";
                dgvdata.Columns["Documents"].HeaderText = "Attached Documents";

                // Style and alignment
                dgvdata.Columns["FullNameAndAddress"].Width = 250;
                dgvdata.Columns["Documents"].Width = 275;
                dgvdata.Columns["LoanType"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvdata.DefaultCellStyle.Font = new Font("Segoe UI", 9);
                dgvdata.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);

                // Update labels
                lstatusupdated.Text = $"UPDATED: {loanTypeCounts["UPDATED"]}";
                lstatuspastdue.Text = $"PAST DUE: {loanTypeCounts["PAST DUE"]}";
                lstatusarrears.Text = $"ARREARS: {loanTypeCounts["ARREARS"]}";
                lstatuslitigation.Text = $"LITIGATION: {loanTypeCounts["LITIGATION"]}";
                lstatusdormant.Text = $"DORMANT: {loanTypeCounts["DORMANT"]}";

                lstatusupdated.BackColor = Color.Green; lstatusupdated.ForeColor = Color.White;
                lstatuspastdue.BackColor = Color.Yellow; lstatuspastdue.ForeColor = Color.Black;
                lstatusarrears.BackColor = Color.Orange; lstatusarrears.ForeColor = Color.White;
                lstatuslitigation.BackColor = Color.Red; lstatuslitigation.ForeColor = Color.White;
                lstatusdormant.BackColor = Color.Gray; lstatusdormant.ForeColor = Color.White;

                lnorecord.Visible = dgvdata.Rows.Count == 0;

                // Action Buttons
                if (dgvdata.Columns["btnActions"] == null)
                {
                    dgvdata.Columns.Add(new DataGridViewButtonColumn
                    {
                        Name = "btnActions",
                        HeaderText = "Actions",
                        Text = "View",
                        UseColumnTextForButtonValue = true,
                        DefaultCellStyle = {
                    Padding = new Padding(2, 20, 2, 20),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                        Width = 100
                    });
                }

                if (dgvdata.Columns["btnDisburse"] == null)
                {
                    dgvdata.Columns.Add(new DataGridViewButtonColumn
                    {
                        Name = "btnDisburse",
                        HeaderText = "Disburse",
                        Text = "Disburse",
                        UseColumnTextForButtonValue = true,
                        DefaultCellStyle = {
                    Padding = new Padding(2, 20, 2, 20),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                        Width = 80
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading the approved loans data: " + ex.Message);
            }
        }


        private void LoadLoanStatusesToComboBox()
        {
            try
            {
                // Access the collections
                var database = MongoDBConnection.Instance.Database;
                var approvedLoansCollection = database.GetCollection<BsonDocument>("loan_approved");

                // Retrieve distinct LoanStatus values
                var statuses = approvedLoansCollection
                    .Find(new BsonDocument { { "LoanStatus", new BsonDocument("$exists", true) } })
                    .Project(Builders<BsonDocument>.Projection.Include("LoanStatus"))
                    .ToList()
                    .Select(doc => doc["LoanStatus"].AsString)
                    .Distinct()
                    .OrderBy(status => status) // Optional: Sort alphabetically
                    .ToList();

                // Add default status "--all status--" to the combo box
                statuses.Insert(0, "--all status--");

                // Bind statuses to the combo box
                cbstatus.DataSource = statuses;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading loan statuses: " + ex.Message);
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


        private void UpdateLoanTypeCounts()
        {
            Dictionary<string, int> loanTypeCounts = new Dictionary<string, int>
            {
                { "UPDATED", 0 },
                { "PAST DUE", 0 },
                { "ARREARS", 0 },
                { "LITIGATION", 0 },
                { "DORMANT", 0 }
            };

            // Count loan types based on the filtered DataGridView
            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                if (row.Cells["LoanType"].Value != null)
                {
                    string loanType = row.Cells["LoanType"].Value.ToString().ToUpper();
                    if (loanTypeCounts.ContainsKey(loanType))
                    {
                        loanTypeCounts[loanType]++;
                    }
                }
            }

            // Update the labels with new counts
            lstatusupdated.Text = $"UPDATED: {loanTypeCounts["UPDATED"]}";
            lstatuspastdue.Text = $"PAST DUE: {loanTypeCounts["PAST DUE"]}";
            lstatusarrears.Text = $"ARREARS: {loanTypeCounts["ARREARS"]}";
            lstatuslitigation.Text = $"LITIGATION: {loanTypeCounts["LITIGATION"]}";
            lstatusdormant.Text = $"DORMANT: {loanTypeCounts["DORMANT"]}";
        }

        private void PopulateYearsFromAccountIds()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var approvedLoansCollection = database.GetCollection<BsonDocument>("loan_approved");

                var approvedDocs = approvedLoansCollection.Find(new BsonDocument()).ToList();
                HashSet<string> uniqueYears = new HashSet<string>();

                foreach (var doc in approvedDocs)
                {
                    if (doc.Contains("AccountId"))
                    {
                        string accountId = doc["AccountId"].ToString();
                        var match = Regex.Match(accountId, @"RCT-(\d{4})DB", RegexOptions.IgnoreCase);
                        if (match.Success)
                            uniqueYears.Add(match.Groups[1].Value);
                    }
                }

                var sortedYears = uniqueYears
                    .Select(y => int.TryParse(y, out int yr) ? yr : 0)
                    .Where(y => y > 0)
                    .OrderByDescending(y => y)
                    .ToList();

                cbyear.Items.Clear();
                foreach (var year in sortedYears)
                    cbyear.Items.Add(year.ToString());

                // Automatically select the current year (2025)
                int currentYear = DateTime.Now.Year;
                if (sortedYears.Contains(currentYear))
                    cbyear.SelectedItem = currentYear.ToString();
                else if (sortedYears.Count > 0)
                    cbyear.SelectedItem = sortedYears[0].ToString(); // Select the first available year

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading year list: " + ex.Message);
            }
        }



        private void baddnew_Click(object sender, EventArgs e)
        {
            frm_home_addnew fladd = new frm_home_addnew();
            load.Show(this);
            Thread.Sleep(500);
           
            load.Close();
            fladd.ShowDialog();
        }


        private void frm_home_loans_Load(object sender, EventArgs e)
        {
            LoadApprovedLoansData();
            LoadLoanStatusesToComboBox();
            LoadUserInfo(loggedInUsername);

            PopulateYearsFromAccountIds(); // Will auto-select the latest year (2025)
            if (cbyear.SelectedItem != null)
            {
                LoadApprovedLoansData("--All Status--", cbyear.SelectedItem.ToString());
            }

            ltotalloancount.Text = dgvdata.Rows.Count.ToString();
        }

        private void dgvdata_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvdata.ClearSelection();
            lnorecord.Visible = false;
        }

        private void dgvdata_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvdata.Columns[e.ColumnIndex].Name == "LoanType")
            {
                if (e.Value != null)
                {
                    string loanType = e.Value.ToString().ToUpper().Trim(); // Ensure it's uppercase and no leading/trailing spaces

                    switch (loanType)
                    {
                        case "UPDATED":
                            e.CellStyle.BackColor = Color.Green;
                            e.CellStyle.ForeColor = Color.White;
                            break;
                        case "PAST DUE":
                            e.CellStyle.BackColor = Color.Yellow;
                            e.CellStyle.ForeColor = Color.Black;
                            break;
                        case "ARREARS":
                            e.CellStyle.BackColor = Color.Orange;
                            e.CellStyle.ForeColor = Color.White;
                            break;
                        case "LITIGATION":
                            e.CellStyle.BackColor = Color.Red;
                            e.CellStyle.ForeColor = Color.White;
                            break;
                        case "DORMANT":
                            e.CellStyle.BackColor = Color.Gray;
                            e.CellStyle.ForeColor = Color.White;
                            break;
                        default:
                            e.CellStyle.BackColor = Color.White;
                            e.CellStyle.ForeColor = Color.Black;
                            break;
                    }
                }
            }
        }


        private async void dgvdata_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Ensure a valid row and column index are clicked
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    // Check for "View Details" button click
                    if (dgvdata.Columns[e.ColumnIndex].Name == "btnActions")
                    {
                        var selectedAccountId = dgvdata.Rows[e.RowIndex].Cells["AccountID"]?.Value?.ToString();
                        if (!string.IsNullOrEmpty(selectedAccountId))
                        {
                            frm_home_loan_new loanDetailsForm = new frm_home_loan_new
                            {
                                AccountID = selectedAccountId // Pass the selected AccountID
                            };

                            bool shouldReload = false;

                            loanDetailsForm.OnLoanNumberUpdated += () =>
                            {
                                shouldReload = true;  // Set flag to reload only if updated
                                return Task.CompletedTask;
                            };

                            ShowLoadingIndicator();
                            loanDetailsForm.ShowDialog();
                            HideLoadingIndicator();

                            // Only reload if updated
                            if (shouldReload)
                            {
                                await LoadApprovedLoansData();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid Account ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    // Check for "Disburse" button click
                    else if (dgvdata.Columns[e.ColumnIndex].Name == "btnDisburse")
                    {
                        var loanStatus = dgvdata.Rows[e.RowIndex].Cells["LoanProcessStatus"]?.Value?.ToString();
                        var accountId = dgvdata.Rows[e.RowIndex].Cells["AccountID"]?.Value?.ToString();

                        if (!string.IsNullOrEmpty(loanStatus) && !string.IsNullOrEmpty(accountId))
                        {
                            if (loanStatus == "Loan Released")
                            {
                                // Safely retrieve column values and handle missing columns
                                var loanNo = dgvdata.Columns.Contains("LoanType")
                                    ? dgvdata.Rows[e.RowIndex].Cells["LoanType"]?.Value?.ToString()
                                    : "N/A";

                                var borrowerName = dgvdata.Columns.Contains("FullNameAndAddress")
                                    ? dgvdata.Rows[e.RowIndex].Cells["FullNameAndAddress"]?.Value?.ToString()
                                    : "N/A";

                                var loanAmount = dgvdata.Columns.Contains("PrincipalAmount")
                                    ? dgvdata.Rows[e.RowIndex].Cells["PrincipalAmount"]?.Value?.ToString()
                                    : "N/A";

                                var startDate = dgvdata.Columns.Contains("StartPaymentDate")
                                    ? dgvdata.Rows[e.RowIndex].Cells["StartPaymentDate"]?.Value?.ToString()
                                    : "N/A";

                                var maturityDate = dgvdata.Columns.Contains("MaturityDate")
                                    ? dgvdata.Rows[e.RowIndex].Cells["MaturityDate"]?.Value?.ToString()
                                    : "N/A";

                                var collectorName = dgvdata.Columns.Contains("CBCP")
                                    ? dgvdata.Rows[e.RowIndex].Cells["CBCP"]?.Value?.ToString()
                                    : "N/A";

                                string message =
                                    $"Sorry, this account has already been disbursed the applied loan. Details are as follows:\n\n" +
                                    $"Account ID: {accountId}\n" +
                                    $"Borrower Name: {borrowerName}\n" +
                                    $"Loan No: {loanNo}\n" +
                                    $"Loan Amount: {loanAmount}\n" +
                                    $"Payment Start Date: {startDate}\n" +
                                    $"Maturity Date: {maturityDate}\n" +
                                    $"Collector: {collectorName}\n";
                                MessageBox.Show(message, "Loan Already Disbursed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                // ✅ Pass AccountID properly
                                frm_home_loan_disburse fdis = new frm_home_loan_disburse(accountId);

                                ShowLoadingIndicator();
                                fdis.Show(this);
                                HideLoadingIndicator();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid Account ID or Loan Status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                // Log or display the error
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowLoadingIndicator()
        {
            // Implement a loading indicator (e.g., show a spinner or disable the form)
            Cursor = Cursors.WaitCursor;
        }

        private void HideLoadingIndicator()
        {
            // Hide the loading indicator
            Cursor = Cursors.Default;
        }

        private void baddloanex_Click(object sender, EventArgs e)
        {

            frm_home_loan_addex addex = new frm_home_loan_addex();

            load.Show(this);
            Thread.Sleep(500);
            load.Close();
            addex.ShowDialog();

        }

        private void cbstatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string selectedStatus = cbstatus.SelectedItem?.ToString(); // Null-safe check

                if (string.IsNullOrEmpty(selectedStatus))
                {
                    MessageBox.Show("Please select a valid status.");
                    return;
                }

                if (selectedStatus == "--all status--")
                {
                    // Load all data
                    string selectedYear = cbyear.SelectedItem?.ToString() ?? "";
                    LoadApprovedLoansData(selectedStatus, selectedYear);
                }
                else
                {
                    // Ensure the DataGridView is bound to a DataTable
                    if (dgvdata.DataSource is DataTable dataTable)
                    {
                        // Apply filter to the DataTable's DefaultView
                        string filterExpression = $"LoanType = '{selectedStatus}'";
                        dataTable.DefaultView.RowFilter = filterExpression;

                        // Debugging output
                        if (dataTable.DefaultView.Count == 0)
                        {
                            MessageBox.Show($"No records found for Loan Status: {selectedStatus}");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Data source is not a valid DataTable.");
                        return;
                    }
                }

                // Update the loan type counts based on the filtered DataGridView
                UpdateLoanTypeCounts();

                // Update the total loan count label
                ltotalloancount.Text = dgvdata.Rows.Count.ToString();


            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void tsearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = tsearch.Text.Trim().ToLower();

            // Reset filter if search text is empty
            if (string.IsNullOrWhiteSpace(searchText))
            {
                LoadApprovedLoansData();
                return;
            }

                // Apply search filter to DataGridView
             (dgvdata.DataSource as DataTable).DefaultView.RowFilter = string.Format(
                 "CONVERT(AccountID, 'System.String') LIKE '%{0}%' OR " +
                 "CONVERT(LoanType, 'System.String') LIKE '%{0}%' OR " +
                 "CONVERT(PrincipalAmount, 'System.String') LIKE '%{0}%' OR " +
                 "CONVERT(LoanTerm, 'System.String') LIKE '%{0}%' OR " +
                 "CONVERT(LoanProcessStatus, 'System.String') LIKE '%{0}%' OR " +
                 "CONVERT(FullNameAndAddress, 'System.String') LIKE '%{0}%' OR " +
                 "CONVERT(CBCP, 'System.String') LIKE '%{0}%' OR " +
                 "CONVERT(Documents, 'System.String') LIKE '%{0}%'",
                 searchText);

            // Recalculate and update loan type totals
            UpdateLoanTypeCounts();
        }

        private void cbyear_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedYear = cbyear.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedYear))
            {
                LoadApprovedLoansData("--All Status--", selectedYear);
            }
        }
    }
}

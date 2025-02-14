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
        frm_home_addnew fladd = new frm_home_addnew();

     
        private void LoadApprovedLoansData(string loanStatusFilter = "--All Status--")
        {
            try
            {
                // Access the collections
                var database = MongoDBConnection.Instance.Database;
                var approvedLoansCollection = database.GetCollection<BsonDocument>("loan_approved");
                var applicationsCollection = database.GetCollection<BsonDocument>("loan_applications");

                // Define the filter based on the ComboBox selection (LoanStatus)
                var filter = loanStatusFilter == "--All Status--"
                    ? Builders<BsonDocument>.Filter.Empty
                    : Builders<BsonDocument>.Filter.Eq("LoanStatus", loanStatusFilter);

                // Retrieve filtered documents based on LoanStatus filter
                var approvedDocuments = approvedLoansCollection.Find(filter).ToList();
                var applicationDocuments = applicationsCollection.Find(new BsonDocument()).ToList();

                // Dictionary to count loan types
                Dictionary<string, int> loanTypeCounts = new Dictionary<string, int>
               {
                   { "UPDATED", 0 },
                   { "PAST DUE", 0 },
                   { "ARREARS", 0 },
                   { "LITIGATION", 0 },
                   { "DORMANT", 0 }
               };

                // Create a DataTable to hold the data
                DataTable dataTable = new DataTable();

                // Define the columns to display
                dataTable.Columns.Add("AccountID");
                dataTable.Columns.Add("LoanType");
                dataTable.Columns.Add("PrincipalAmount");
                dataTable.Columns.Add("LoanTerm");
                dataTable.Columns.Add("LoanProcessStatus");
                dataTable.Columns.Add("FullNameAndAddress");
                dataTable.Columns.Add("CBCP");
                dataTable.Columns.Add("Documents");

                foreach (var approvedDoc in approvedDocuments)
                {
                    DataRow row = dataTable.NewRow();
                    var accountId = approvedDoc.Contains("AccountId") ? approvedDoc["AccountId"].ToString() : string.Empty;
                    row["AccountID"] = accountId;
                    string loanType = approvedDoc.Contains("LoanStatus") ? approvedDoc["LoanStatus"].ToString() : "DEFAULT";
                    row["LoanType"] = loanType;

                    // Count loan types
                    if (loanTypeCounts.ContainsKey(loanType))
                        loanTypeCounts[loanType]++;
                    else
                        loanTypeCounts["DEFAULT"] = loanTypeCounts.ContainsKey("DEFAULT") ? loanTypeCounts["DEFAULT"] + 1 : 1;

                    row["PrincipalAmount"] = approvedDoc.Contains("LoanAmount") ? approvedDoc["LoanAmount"].ToString() : string.Empty;
                    row["LoanTerm"] = approvedDoc.Contains("LoanTerm") ? approvedDoc["LoanTerm"].ToString() : string.Empty;

                    // Fetch status and documents from the loan_applications collection
                    var applicationDoc = applicationDocuments.FirstOrDefault(doc => doc.Contains("AccountId") && doc["AccountId"].ToString() == accountId);
                    row["LoanProcessStatus"] = approvedDoc.Contains("LoanProcessStatus") ? approvedDoc["LoanProcessStatus"].ToString() : string.Empty;
                    row["Documents"] = applicationDoc != null && applicationDoc.Contains("docs") ? applicationDoc["docs"].ToString() : string.Empty;

                    // FullName and Address
                    string firstName = approvedDoc.Contains("FirstName") ? approvedDoc["FirstName"].ToString() : string.Empty;
                    string middleName = approvedDoc.Contains("MiddleName") ? approvedDoc["MiddleName"].ToString() : string.Empty;
                    string lastName = approvedDoc.Contains("LastName") ? approvedDoc["LastName"].ToString() : string.Empty;
                    string suffixName = approvedDoc.Contains("SuffixName") ? approvedDoc["SuffixName"].ToString() : string.Empty;
                    string fullName = $"{firstName} {middleName} {lastName} {suffixName}".Trim();

                    string street = approvedDoc.Contains("Street") ? approvedDoc["Street"].ToString() : string.Empty;
                    string barangay = approvedDoc.Contains("Barangay") ? approvedDoc["Barangay"].ToString() : string.Empty;
                    string city = approvedDoc.Contains("City") ? approvedDoc["City"].ToString() : string.Empty;
                    string province = approvedDoc.Contains("Province") ? approvedDoc["Province"].ToString() : string.Empty;
                    string address = $"{street}, {barangay}, {city}, {province}".Trim();

                    // Concatenate full name and address
                    row["FullNameAndAddress"] = $"{fullName}\n{address}";

                    row["CBCP"] = approvedDoc.Contains("CBCP") ? approvedDoc["CBCP"].ToString() : string.Empty;

                    // Format documents list
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
                dgvdata.Columns["AccountID"].HeaderText = "Client No.";
                dgvdata.Columns["LoanType"].HeaderText = "Loan Type";
                dgvdata.Columns["PrincipalAmount"].HeaderText = "Principal Amount";
                dgvdata.Columns["LoanTerm"].HeaderText = "Loan Term";
                dgvdata.Columns["LoanProcessStatus"].HeaderText = "Loan Status";
                dgvdata.Columns["FullNameAndAddress"].HeaderText = "Client Name";
                dgvdata.Columns["CBCP"].HeaderText = "Contact Number";
                dgvdata.Columns["Documents"].HeaderText = "Attached Documents";

                // Adjust column widths, font styles, etc.
                dgvdata.Columns["FullNameAndAddress"].Width = 250;
                dgvdata.Columns["Documents"].Width = 275;
                dgvdata.Columns["LoanType"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Apply font styling
                dgvdata.DefaultCellStyle.Font = new Font("Segoe UI", 9);
                dgvdata.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);

                // Update the labels with loan type counts
                lstatusupdated.Text = $"UPDATED: {loanTypeCounts["UPDATED"]}";
                lstatuspastdue.Text = $"PAST DUE: {loanTypeCounts["PAST DUE"]}";
                lstatusarrears.Text = $"ARREARS: {loanTypeCounts["ARREARS"]}";
                lstatuslitigation.Text = $"LITIGATION: {loanTypeCounts["LITIGATION"]}";
                lstatusdormant.Text = $"DORMANT: {loanTypeCounts["DORMANT"]}";

                // Set background colors to match DataGridView formatting
                lstatusupdated.BackColor = Color.Green;
                lstatusupdated.ForeColor = Color.White;

                lstatuspastdue.BackColor = Color.Yellow;
                lstatuspastdue.ForeColor = Color.Black;

                lstatusarrears.BackColor = Color.Orange;
                lstatusarrears.ForeColor = Color.White;

                lstatuslitigation.BackColor = Color.Red;
                lstatuslitigation.ForeColor = Color.White;

                lstatusdormant.BackColor = Color.Gray;
                lstatusdormant.ForeColor = Color.White;

                lnorecord.Visible = dgvdata.Rows.Count == 0;

                if (dgvdata.Columns["btnActions"] == null)
                {
                    DataGridViewButtonColumn viewDetailsButtonColumn = new DataGridViewButtonColumn
                    {
                        Name = "btnActions",
                        HeaderText = "Actions",
                        Text = "View",
                        UseColumnTextForButtonValue = true,
                       
                    };
                    dgvdata.Columns.Add(viewDetailsButtonColumn);
                }

                // Adjust padding and alignment for the button
                dgvdata.Columns["btnActions"].DefaultCellStyle.Padding = new Padding(2, 20, 2, 20);
                dgvdata.Columns["btnActions"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvdata.Columns["btnActions"].Width = 100; // Set a reasonable width

                if (dgvdata.Columns["btnDisburse"] == null)
                {
                    DataGridViewButtonColumn disburseButtonColumn = new DataGridViewButtonColumn
                    {
                        Name = "btnDisburse",
                        HeaderText = "Disburse",
                        Text = "Disburse",
                        UseColumnTextForButtonValue = true,
                       
                    };
                    dgvdata.Columns.Add(disburseButtonColumn);
                }

                // Adjust padding and alignment for the disburse button
                dgvdata.Columns["btnDisburse"].DefaultCellStyle.Padding = new Padding(2, 20, 2, 20);
                dgvdata.Columns["btnDisburse"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvdata.Columns["btnDisburse"].Width = 80; // Set a reasonable width

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
            LoadLoanStatusesToComboBox();
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
            if (dgvdata.Columns[e.ColumnIndex].Name == "LoanType")
            {
                if (e.Value != null)
                {
                    string loanType = e.Value.ToString().ToUpper();

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

        private void dgvdata_CellClick(object sender, DataGridViewCellEventArgs e)
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

                            // Show loading indicator asynchronously
                            ShowLoadingIndicator();
                            loanDetailsForm.ShowDialog();
                            HideLoadingIndicator();
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
                                frm_home_loan_disburse fdis = new frm_home_loan_disburse
                                {
                                    AccountID = accountId // Pass the AccountID
                                };

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
                    LoadApprovedLoansData();
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
    }
}

using MongoDB.Bson;
using MongoDB.Driver;
using rct_lmis.CLIENTS_SECTION;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rct_lmis
{
    public partial class frm_home_clients : Form
    {
        private IMongoDatabase database;
        private IMongoCollection<BsonDocument> loanApprovedCollection;
        private IMongoCollection<BsonDocument> loanCollectorsCollection;
        private IMongoCollection<BsonDocument> loanRawdataCollection;
        private List<BsonDocument> rawDataList;


        public frm_home_clients()
        {
            InitializeComponent();
            database = MongoDBConnection.Instance.Database;
            loanRawdataCollection = database.GetCollection<BsonDocument>("loan_rawdata");
            loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");
            loanCollectorsCollection = database.GetCollection<BsonDocument>("loan_collectors");
            rawDataList = new List<BsonDocument>();
        }

        LoadingFunction load = new LoadingFunction();

        private void LoadDataToDataGridView()
        {
            lnorecord.Visible = false;

            // Fetch all collectors into a list for keyword matching
            var collectors = loanCollectorsCollection.Find(new BsonDocument()).ToList();

            var rawData = loanApprovedCollection.Find(new BsonDocument()).ToList();
            rawDataList = rawData;

            DataTable dt = new DataTable();
            dt.Columns.Add("Item No."); // Will be generated dynamically
            dt.Columns.Add("Collector Info");
            dt.Columns.Add("Client Info");
            dt.Columns.Add("Loan Term Info");
            dt.Columns.Add("Loan Amount Info");
            dt.Columns.Add("Amortization Info");
            dt.Columns.Add("Penalty");
            dt.Columns.Add("Total Collection");
            dt.Columns.Add("Loan Status");
            dt.Columns.Add("Loan Status Date Update");
            dt.Columns.Add("ClientNo");

            int itemNoCounter = 1; // Initialize counter for Item No.

            foreach (var doc in rawData)
            {
                DataRow row = dt.NewRow();
                row["Item No."] = itemNoCounter++; // Assign and increment Item No.

                // Merging address into Client Info
                string address = $"{doc.GetValue("Barangay", "")}, {doc.GetValue("City", "")}, {doc.GetValue("Province", "")}".Trim();
                row["Client Info"] = $"{doc.GetValue("FirstName", "")} {doc.GetValue("MiddleName", "")} {doc.GetValue("LastName", "")}\nClient No.: {doc.GetValue("ClientNo", "")}\nLoan ID: {doc.GetValue("LoanNo", "")}\nAddress: {address}";

                // Get the collector name from the loan_approved document
                string collectorNameFromLoan = doc.GetValue("CollectorName", "").ToString().Trim();

                // Find a matching collector based on a keyword in the collector's name
                var matchingCollector = collectors.FirstOrDefault(c =>
                    c.GetValue("Name", "").ToString().Trim().IndexOf(collectorNameFromLoan, StringComparison.OrdinalIgnoreCase) >= 0);

                if (matchingCollector != null)
                {
                    // Display the collector name from the loan_collectors collection
                    row["Collector Info"] = matchingCollector.GetValue("Name", "Unknown Collector").ToString();
                }
                else
                {
                    // If not found, display "Unknown Collector"
                    row["Collector Info"] = "Unknown Collector";
                }

                // Populate other fields
                row["Loan Term Info"] = $"{doc.GetValue("LoanTerm", "")}\n{doc.GetValue("PaymentMode", "")}";
                row["Loan Amount Info"] = $"Amount: {doc.GetValue("LoanAmount", "").ToString()}\nBalance: {doc.GetValue("LoanBalance", "").ToString()}";
                row["Amortization Info"] = $"Amortization: {doc.GetValue("LoanAmortization", "").ToString()}" +
                    $"\nDue: {doc.GetValue("LoanAmortization", "").ToString()}" + // Assuming you mean LoanAmortization here
                    $"\nMissed Days: {doc.GetValue("missed_day", "0")} days"; // Ensure that this field exists

                // Safely handle conversion of Penalty
                bool isPenaltyNumeric = decimal.TryParse(doc.GetValue("Penalty", "0").ToString().Replace("₱", "").Trim(), out decimal penaltyValue);
                row["Penalty"] = isPenaltyNumeric ? $"₱{penaltyValue:N2}" : "₱0.00";

                // Safely handle conversion of Total Collection (if applicable)
                bool isTotalCollectionNumeric = decimal.TryParse(doc.GetValue("TotalCollection", "0").ToString().Replace("₱", "").Trim(), out decimal totalCollectionValue);
                row["Total Collection"] = isTotalCollectionNumeric ? $"₱{totalCollectionValue:N2}" : "₱0.00";

                string loanStatus = doc.GetValue("LoanStatus", "").ToString();
                row["Loan Status"] = loanStatus;
                row["Loan Status Date Update"] = doc.GetValue("Date_Encoded", "").ToString();

                row["ClientNo"] = doc.GetValue("ClientNo", "").ToString();

                dt.Rows.Add(row);
            }

            dgvdata.DataSource = dt;

            // Center align the Item No column
            dgvdata.Columns["Item No."].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvdata.Columns["Loan Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvdata.Columns["Loan Status Date Update"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Adjust Client Info width
            dgvdata.Columns["Client Info"].Width = 300;
            dgvdata.Columns["Item No."].Width = 80;
            dgvdata.Columns["Loan Amount Info"].Width = 150;
            dgvdata.Columns["Amortization Info"].Width = 150;
            dgvdata.Columns["ClientNo"].Visible = false;

            // Add the button column explicitly with padding
            DataGridViewButtonColumn actionColumn = new DataGridViewButtonColumn();
            actionColumn.Name = "Actions";
            actionColumn.HeaderText = "Actions";
            actionColumn.Text = "View";  // Set the button text
            actionColumn.UseColumnTextForButtonValue = true;  // Ensure the button shows the text
            actionColumn.FlatStyle = FlatStyle.Standard;
            actionColumn.DefaultCellStyle.Padding = new Padding(30, 15, 20, 15);

            // Add the button column to the DataGridView
            dgvdata.Columns.Add(actionColumn);

            // Now, after setting the DataSource, highlight the rows
            HighlightApprovedLoans();

            UpdateLoanStatusLabels();
        }

        private void HighlightApprovedLoans()
        {
            // Retrieve the list of loan_approved documents
            var approvedLoans = loanApprovedCollection.Find(new BsonDocument()).ToList();

            var approvedLoanIds = approvedLoans
                .Select(doc =>
                {
                    var loanNo = doc.GetValue("LoanNo", "").ToString();
                    return !string.IsNullOrEmpty(loanNo) && loanNo.Length >= 5
                        ? loanNo.Substring(loanNo.Length - 5)
                        : null;
                })
                .Where(id => id != null)
                .ToList();

            // Highlight rows that are already in loan_approved collection
            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                string clientInfo = row.Cells["Client Info"].Value?.ToString() ?? "";

                // Extract Loan ID from Client Info safely
                string[] clientInfoParts = clientInfo.Split('\n');
                string loanId = clientInfoParts.Length > 2 && clientInfoParts[2].Contains(":")
                    ? clientInfoParts[2].Split(':')[1].Trim()
                    : string.Empty;

                // Check if loanId is not empty and at least 5 characters long before checking the last 5 digits
                if (!string.IsNullOrEmpty(loanId) && loanId.Length >= 5)
                {
                    string lastFiveDigits = loanId.Substring(loanId.Length - 5);

                    // Check if the last 5 digits of the loanId exist in approvedLoanIds
                    if (approvedLoanIds.Contains(lastFiveDigits))
                    {
                        row.DefaultCellStyle.BackColor = Color.LightYellow;
                    }
                }
            }
        }


        private void UpdateLoanStatusLabels()
        {
            // Count only the visible rows after filtering
            int totalRows = dgvdata.Rows.Count;
            int updatedCount = 0;
            int arrearsCount = 0;
            int litigationCount = 0;
            int pastdueCount = 0;
            int dormantCount = 0;

            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                // Check if the row is not new and is visible
                if (!row.IsNewRow && row.Visible)
                {
                    string loanStatus = row.Cells["Loan Status"].Value?.ToString() ?? "";

                    // Increment counters based on loan status
                    switch (loanStatus.ToUpper())  // Ensure case insensitivity
                    {
                        case "UPDATED":
                            updatedCount++;
                            break;
                        case "ARREARS":
                            arrearsCount++;
                            break;
                        case "PAST DUE":
                            pastdueCount++;
                            break;
                        case "LITIGATION":
                            litigationCount++;
                            break;
                        case "DORMANT":
                            dormantCount++;
                            break;
                    }
                }
            }

            // Update the label texts and background colors
            laccounttotal.Text = $"{totalRows}";

            // UPDATED status
            lstatusupdated.Text = $"UPDATED: {updatedCount}";
            lstatusupdated.BackColor = updatedCount > 0 ? Color.Green : Color.LightGray;  // Green or Gray if count is 0
            lstatusupdated.ForeColor = Color.White;

            // ARREARS status
            lstatusarrears.Text = $"ARREARS: {arrearsCount}";
            lstatusarrears.BackColor = arrearsCount > 0 ? Color.Yellow : Color.LightGray;  // Yellow or Gray if count is 0

            // PAST DUE status
            lstatuspastdue.Text = $"PAST DUE: {pastdueCount}";
            lstatuspastdue.BackColor = pastdueCount > 0 ? Color.Khaki : Color.LightGray;  // Yellow or Gray if count is 0


            // LITIGATION status
            lstatuslitigation.Text = $"LITIGATION: {litigationCount}";
            lstatuslitigation.BackColor = litigationCount > 0 ? Color.Orange : Color.LightGray;  // Orange or Gray if count is 0

            // DORMANT status
            lstatusdormant.Text = $"DORMANT: {dormantCount}";
            lstatusdormant.BackColor = dormantCount > 0 ? Color.Gray : Color.LightGray;  // Gray or LightGray if count is 0
            lstatusdormant.ForeColor = Color.White;

        }

        private decimal ConvertToDecimal(string value)
        {
            // Remove the currency symbol and any commas
            string cleanedValue = value.Replace("₱", "").Replace(",", "").Trim();

            // Attempt to parse the cleaned string to a decimal
            if (decimal.TryParse(cleanedValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
            {
                return result;
            }
            // Return 0 if parsing fails
            return 0;
        }


        private void FilterDataGridView(string keyword)
        {
            // If there is no data, return
            if (dgvdata.DataSource == null)
                return;

            // Convert DataSource to DataTable for filtering
            DataTable dt = dgvdata.DataSource as DataTable;
            if (dt == null) return;

            // Use DataView to filter rows based on the keyword in any column
            DataView dv = dt.DefaultView;
            dv.RowFilter = string.IsNullOrEmpty(keyword)
                ? string.Empty
                : $"Convert([Item No.], 'System.String') LIKE '%{keyword}%' OR " +
                  $"[Collector Info] LIKE '%{keyword}%' OR " +
                  $"[Client Info] LIKE '%{keyword}%' OR " +
                  $"[Loan Term Info] LIKE '%{keyword}%' OR " +
                  $"[Loan Amount Info] LIKE '%{keyword}%' OR " +
                  $"[Amortization Info] LIKE '%{keyword}%' OR " +
                  $"[Penalty] LIKE '%{keyword}%' OR " +
                  $"[Total Collection] LIKE '%{keyword}%' OR " +
                  $"[Loan Status] LIKE '%{keyword}%' OR " +
                  $"[Loan Status Date Update] LIKE '%{keyword}%'";

            dgvdata.DataSource = dv.ToTable();
        }

        private void refreshdata(string loanStatusFilter = null)
        {
            lnorecord.Visible = false;

            // Fetch all collectors into a list for keyword matching
            var collectors = loanCollectorsCollection.Find(new BsonDocument()).ToList();

            var rawData = loanApprovedCollection.Find(new BsonDocument()).ToList();
            rawDataList = rawData;

            DataTable dt = new DataTable();
            dt.Columns.Add("Item No."); // Will be generated dynamically
            dt.Columns.Add("Collector Info");
            dt.Columns.Add("Client Info");
            dt.Columns.Add("Loan Term Info");
            dt.Columns.Add("Loan Amount Info");
            dt.Columns.Add("Amortization Info");
            dt.Columns.Add("Penalty");
            dt.Columns.Add("Total Collection");
            dt.Columns.Add("Loan Status");
            dt.Columns.Add("Loan Status Date Update");
            dt.Columns.Add("ClientNo");

            int itemNoCounter = 1; // Initialize counter for Item No.

            foreach (var doc in rawData)
            {
                DataRow row = dt.NewRow();
                row["Item No."] = itemNoCounter++; // Assign and increment Item No.

                // Merging address into Client Info
                string address = $"{doc.GetValue("Barangay", "")}, {doc.GetValue("City", "")}, {doc.GetValue("Province", "")}".Trim();
                row["Client Info"] = $"{doc.GetValue("FirstName", "")} {doc.GetValue("MiddleName", "")} {doc.GetValue("LastName", "")}\nClient No.: {doc.GetValue("ClientNo", "")}\nLoan ID: {doc.GetValue("LoanNo", "")}\nAddress: {address}";

                // Get the collector name from the loan_approved document
                string collectorNameFromLoan = doc.GetValue("CollectorName", "").ToString().Trim();

                // Find a matching collector based on a keyword in the collector's name
                var matchingCollector = collectors.FirstOrDefault(c =>
                    c.GetValue("Name", "").ToString().Trim().IndexOf(collectorNameFromLoan, StringComparison.OrdinalIgnoreCase) >= 0);

                if (matchingCollector != null)
                {
                    // Display the collector name from the loan_collectors collection
                    row["Collector Info"] = matchingCollector.GetValue("Name", "Unknown Collector").ToString();
                }
                else
                {
                    // If not found, display "Unknown Collector"
                    row["Collector Info"] = "Unknown Collector";
                }

                // Populate other fields
                row["Loan Term Info"] = $"{doc.GetValue("LoanTerm", "")}\n{doc.GetValue("PaymentMode", "")}";
                row["Loan Amount Info"] = $"Amount: {doc.GetValue("LoanAmount", "").ToString()}\nBalance: {doc.GetValue("LoanBalance", "").ToString()}";
                row["Amortization Info"] = $"Amortization: {doc.GetValue("LoanAmortization", "").ToString()}" +
                    $"\nDue: {doc.GetValue("LoanAmortization", "").ToString()}" + // Assuming you mean LoanAmortization here
                    $"\nMissed Days: {doc.GetValue("missed_day", "0")} days"; // Ensure that this field exists

                // Safely handle conversion of Penalty
                bool isPenaltyNumeric = decimal.TryParse(doc.GetValue("Penalty", "0").ToString().Replace("₱", "").Trim(), out decimal penaltyValue);
                row["Penalty"] = isPenaltyNumeric ? $"₱{penaltyValue:N2}" : "₱0.00";

                // Safely handle conversion of Total Collection (if applicable)
                bool isTotalCollectionNumeric = decimal.TryParse(doc.GetValue("TotalCollection", "0").ToString().Replace("₱", "").Trim(), out decimal totalCollectionValue);
                row["Total Collection"] = isTotalCollectionNumeric ? $"₱{totalCollectionValue:N2}" : "₱0.00";

                string loanStatus = doc.GetValue("LoanStatus", "").ToString();
                row["Loan Status"] = loanStatus;
                row["Loan Status Date Update"] = doc.GetValue("Date_Encoded", "").ToString();

                row["ClientNo"] = doc.GetValue("ClientNo", "").ToString();

                dt.Rows.Add(row);
            }

            dgvdata.DataSource = dt;

            // Center align the Item No column
            dgvdata.Columns["Item No."].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvdata.Columns["Loan Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvdata.Columns["Loan Status Date Update"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Adjust Client Info width
            dgvdata.Columns["Client Info"].Width = 300;
            dgvdata.Columns["Item No."].Width = 80;
            dgvdata.Columns["Loan Amount Info"].Width = 150;
            dgvdata.Columns["Amortization Info"].Width = 150;
            dgvdata.Columns["ClientNo"].Visible = false;

            // Now, after setting the DataSource, highlight the rows
            HighlightApprovedLoans();
            UpdateLoanStatusLabels();
        }

        // Call this method after initializing MongoDB collections
        private void PopulateLoanStatusComboBox()
        {
            // Assuming loanRawdataCollection is your MongoDB collection for loan data
            var loanStatuses = loanRawdataCollection.Distinct<string>("loan_status", new BsonDocument()).ToList();

            // Clear the ComboBox before populating
            cbloanstatus.Items.Clear();

            // Add "Show All" option
            cbloanstatus.Items.Add("Show All");

            // Add the loan status values to the ComboBox
            foreach (var status in loanStatuses)
            {
                cbloanstatus.Items.Add(status);
            }

            // Set default selected item to "Show All"
            cbloanstatus.SelectedIndex = 0;
        }


        private void frm_home_clients_Load(object sender, EventArgs e)
        {
            LoadDataToDataGridView();
            PopulateLoanStatusComboBox();
        }

        private void tsearch_TextChanged(object sender, EventArgs e)
        {
            FilterDataGridView(tsearch.Text);
        }

        private void dgvdata_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Apply Loan Status color formatting
            if (dgvdata.Columns[e.ColumnIndex].Name == "Loan Status" && e.Value != null)
            {
                string status = e.Value.ToString();

                switch (status)
                {
                    case "UPDATED":
                        e.CellStyle.BackColor = Color.Green;
                        e.CellStyle.ForeColor = Color.White;
                        break;
                    case "ARREARS":
                        e.CellStyle.BackColor = Color.Yellow;
                        e.CellStyle.ForeColor = Color.Black;
                        break;
                    case "PAST DUE":
                        e.CellStyle.BackColor = Color.Khaki;
                        e.CellStyle.ForeColor = Color.Black;
                        break;
                    case "LITIGATION":
                        e.CellStyle.BackColor = Color.Orange;
                        e.CellStyle.ForeColor = Color.Black;
                        break;
                    case "DORMANT":
                        e.CellStyle.BackColor = Color.Gray;
                        e.CellStyle.ForeColor = Color.White;
                        break;
                }
            }
        }

        private void brefresh_Click(object sender, EventArgs e)
        {
            load.Show(this);
            Thread.Sleep(1000);
            refreshdata();
            load.Close();
        }

        private void dgvdata_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvdata.ClearSelection();
        }

        private void dgvdata_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the click was on the Actions column
            if (e.ColumnIndex == dgvdata.Columns["Actions"].Index && e.RowIndex >= 0)
            {
                string clientInfo = dgvdata.Rows[e.RowIndex].Cells["Client Info"].Value?.ToString() ?? ""; // Get Client Info safely
                string[] clientInfoLines = clientInfo.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                // Extract Loan ID (if the line exists)
                string loanId = clientInfoLines.Length > 2 ? clientInfoLines[2].Replace("Loan ID: ", "").Trim() : "";

                // Only try to access ClientNo if it exists
                string clientNo = dgvdata.Columns.Contains("ClientNo")
                    ? dgvdata.Rows[e.RowIndex].Cells["ClientNo"].Value?.ToString()
                    : "";

                // Open LoanDetailsForm if ClientNo and LoanId are valid
                if (!string.IsNullOrEmpty(loanId) && !string.IsNullOrEmpty(clientNo))
                {
                    frm_home_client_details detailsForm = new frm_home_client_details(loanId, clientNo);
                    detailsForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Client information or Loan ID is missing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dgvdata_DataBindingComplete_1(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvdata.ClearSelection();
        }

        private void cbloanstatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected loan status from the ComboBox
            string selectedLoanStatus = cbloanstatus.SelectedItem?.ToString();

            // Check if "Show All" is selected
            if (selectedLoanStatus == "Show All")
            {
                dgvdata.Refresh();
            }
            else
            {
                // Load filtered data based on selected loan status
                refreshdata(selectedLoanStatus);
            }
        }
    }
}

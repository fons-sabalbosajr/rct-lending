using MongoDB.Bson;
using MongoDB.Driver;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace rct_lmis.ADMIN_SECTION
{
    public partial class frm_home_ADMIN_rawdata : Form
    {
        private IMongoDatabase database;
        private IMongoCollection<BsonDocument> loanRawdataCollection;
        private IMongoCollection<BsonDocument> loanApprovedCollection;
        private IMongoCollection<BsonDocument> loanCollectorsCollection;

        private List<BsonDocument> rawDataList;

        public frm_home_ADMIN_rawdata()
        {
            InitializeComponent();
            database = MongoDBConnection.Instance.Database;
            loanRawdataCollection = database.GetCollection<BsonDocument>("loan_rawdata");
            loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");
            loanCollectorsCollection = database.GetCollection<BsonDocument>("loan_approved");
            rawDataList = new List<BsonDocument>();
        }

        LoadingFunction load = new LoadingFunction();

        private void frm_home_ADMIN_rawdata_Load(object sender, EventArgs e)
        {
            LoadDataToDataGridView();
            PopulateLoanStatusComboBox();
        }

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



        private async void LoadDataToDataGridView(string loanStatusFilter = null)
        {
            try
            {
                // MongoDB connection
                var database = MongoDBConnection.Instance.Database;
                var loanRawdataCollection = database.GetCollection<BsonDocument>("loan_rawdata");
                var loanCollectorsCollection = database.GetCollection<BsonDocument>("loan_collectors");

                // Fetch all collectors into a list for keyword matching
                var collectors = await loanCollectorsCollection.Find(new BsonDocument()).ToListAsync();

                // Define a filter for loan status if provided
                FilterDefinition<BsonDocument> filter = string.IsNullOrEmpty(loanStatusFilter)
                    ? new BsonDocument()
                    : Builders<BsonDocument>.Filter.Eq("loan_status", loanStatusFilter);

                // Retrieve loan data based on the filter
                var rawData = await loanRawdataCollection.Find(filter).ToListAsync();
                rawDataList = rawData;

                // Populate DataTable with data
                DataTable dt = new DataTable();
                dt.Columns.Add("Item No.", typeof(int));
                dt.Columns.Add("Collector Info");
                dt.Columns.Add("Client Info");
                dt.Columns.Add("Loan Term Info");
                dt.Columns.Add("Loan Amount Info");
                dt.Columns.Add("Amortization Info");
                dt.Columns.Add("Penalty");
                dt.Columns.Add("Total Collection");
                dt.Columns.Add("Loan Status");
                dt.Columns.Add("Loan Status Date Update");

                int itemNoCounter = 1;
                foreach (var doc in rawData)
                {
                    DataRow row = dt.NewRow();
                    row["Item No."] = itemNoCounter++;

                    // Get the collector name from loan_rawdata document
                    string collectorNameFromLoan = doc.GetValue("collector_name", "").ToString().Trim();

                    // Try to find a matching collector based on a keyword in the collector's name
                    var matchingCollector = collectors.FirstOrDefault(c =>
                        c.GetValue("Name", "").ToString().Trim().IndexOf(collectorNameFromLoan, StringComparison.OrdinalIgnoreCase) >= 0);

                    // Display the matched collector's name or fallback to the original collector name if no match is found
                    row["Collector Info"] = matchingCollector != null
                        ? matchingCollector.GetValue("Name", "Unknown Collector").ToString()
                        : collectorNameFromLoan;

                    // Populate other fields
                    row["Client Info"] = $"{doc.GetValue("client_name", "").ToString().Trim()}\nContact No: {doc.GetValue("contact_no", "").ToString().Trim()}\nLoan ID: {doc.GetValue("loan_id", 0)}";
                    row["Loan Term Info"] = $"{doc.GetValue("loan_term", 0)} months\n{doc.GetValue("payment_mode", "").ToString().Trim()}";
                    row["Loan Amount Info"] = $"Amount: {ConvertToDouble(doc.GetValue("loan_amount", 0)):N2}\nBalance: {ConvertToDouble(doc.GetValue("loan_balance", 0)):N2}";
                    row["Amortization Info"] = $"Amortization: {ConvertToDouble(doc.GetValue("loan_amortization", 0)):N2}\nDue: {ConvertToDouble(doc.GetValue("amortization_due", 0)):N2}\nMissed Days: {doc.GetValue("missed_day", 0).ToInt32()} days";
                    row["Penalty"] = ConvertToDouble(doc.GetValue("penalty", 0)).ToString("N2");
                    row["Total Collection"] = ConvertToDouble(doc.GetValue("total_collection", 0)).ToString("N2");
                    row["Loan Status"] = doc.GetValue("loan_status", "").ToString().Trim();
                    row["Loan Status Date Update"] = doc.GetValue("loan_status_date_updated", "").ToString().Trim();

                    dt.Rows.Add(row);
                }

                // Sort the DataTable by "Item No."
                DataView dv = dt.DefaultView;
                dv.Sort = "Item No. ASC";
                DataTable sortedDt = dv.ToTable();

                dgvdata.DataSource = sortedDt;

                // Center align specific columns
                dgvdata.Columns["Item No."].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvdata.Columns["Loan Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvdata.Columns["Loan Status Date Update"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Adjust widths
                dgvdata.Columns["Client Info"].Width = 200;
                dgvdata.Columns["Item No."].Width = 80;
                dgvdata.Columns["Loan Amount Info"].Width = 150;
                dgvdata.Columns["Amortization Info"].Width = 150;

                // Add button column if not already added
                if (!dgvdata.Columns.Contains("Actions"))
                {
                    DataGridViewButtonColumn actionColumn = new DataGridViewButtonColumn
                    {
                        Name = "Actions",
                        HeaderText = "Actions",
                        Text = "View",
                        UseColumnTextForButtonValue = true,
                        FlatStyle = FlatStyle.Standard,
                        DefaultCellStyle = { Padding = new Padding(30, 15, 20, 15) }
                    };
                    dgvdata.Columns.Add(actionColumn);
                }

                // Highlight rows and update labels
                HighlightApprovedLoans();
                UpdateLoanStatusLabels();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }



        private void ApplySearchFilter(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                // If no keyword is provided, reset to the full data
                LoadDataToDataGridView(); // Reloads the full dataset
                return;
            }

            var collectors = loanCollectorsCollection.Find(new BsonDocument()).ToList();

            // Create a new DataTable to hold the filtered results
            DataTable filteredTable = new DataTable();
            filteredTable.Columns.Add("Item No.", typeof(int));
            filteredTable.Columns.Add("Collector Info");
            filteredTable.Columns.Add("Client Info");
            filteredTable.Columns.Add("Loan Term Info");
            filteredTable.Columns.Add("Loan Amount Info");
            filteredTable.Columns.Add("Amortization Info");
            filteredTable.Columns.Add("Penalty");
            filteredTable.Columns.Add("Total Collection");
            filteredTable.Columns.Add("Loan Status");
            filteredTable.Columns.Add("Loan Status Date Update");

            // Loop through the rawDataList (which is a List<BsonDocument>)
            foreach (var doc in rawDataList)
            {
                // Get the necessary fields and combine them for filtering
                string collectorInfo = $"{doc.GetValue("collector_name", "").ToString().Trim()} {doc.GetValue("area_route", "").ToString().Trim()}".ToLower();


                string clientInfo = $"{doc.GetValue("client_name", "").ToString().Trim()} Contact No: {doc.GetValue("contact_no", "").ToString().Trim()} Loan ID: {doc.GetValue("loan_id", 0)}".ToLower();
                string loanStatus = doc.GetValue("loan_status", "").ToString().Trim().ToLower();
                string loanAmountInfo = $"Amount: {ConvertToDouble(doc.GetValue("loan_amount", 0)):N2} Balance: {ConvertToDouble(doc.GetValue("loan_balance", 0)):N2}".ToLower();

                // Combine all fields into a single string for keyword search
                string combinedText = $"{collectorInfo} {clientInfo} {loanStatus} {loanAmountInfo}";

                // Check if the keyword exists in the combined text
                if (combinedText.Contains(keyword.ToLower()))
                {
                    // If it matches, create a new row for the filtered data
                    DataRow row = filteredTable.NewRow();
                    row["Item No."] = doc.GetValue("item_no", 0).ToInt32();
                    row["Collector Info"] = collectorInfo;
                    row["Client Info"] = clientInfo;
                    row["Loan Term Info"] = $"{doc.GetValue("loan_term", 0)} months\n{doc.GetValue("payment_mode", "").ToString().Trim()}";
                    row["Loan Amount Info"] = loanAmountInfo;
                    row["Amortization Info"] = $"Amortization: {ConvertToDouble(doc.GetValue("loan_amortization", 0)):N2}\nDue: {ConvertToDouble(doc.GetValue("amortization_due", 0)):N2}\nMissed Days: {doc.GetValue("missed_day", 0)} days";
                    row["Penalty"] = ConvertToDouble(doc.GetValue("penalty", 0)).ToString("N2");
                    row["Total Collection"] = ConvertToDouble(doc.GetValue("total_collection", 0)).ToString("N2");
                    row["Loan Status"] = loanStatus;
                    row["Loan Status Date Update"] = doc.GetValue("loan_status_date_updated", "").ToString().Trim();

                    filteredTable.Rows.Add(row);
                }
            }

            // Set the filtered data as the new DataSource
            dgvdata.DataSource = filteredTable;

            // Reapply styling and column settings if needed
            dgvdata.Columns["Item No."].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvdata.Columns["Loan Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvdata.Columns["Loan Status Date Update"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }


        private double ConvertToDouble(object value)
        {
            if (value == null) return 0.0;
            string strValue = value.ToString().Trim().Replace(",", "");
            if (double.TryParse(strValue, out double result))
            {
                return result;
            }
            return 0.0;
        }



        private void HighlightApprovedLoans()
        {
            // Retrieve the list of loan_approved documents
            var approvedLoans = loanApprovedCollection.Find(new BsonDocument()).ToList();

            // Extract the last 1 to 5 digits from LoanNo in loan_approved
            var approvedLoanIds = approvedLoans.Select(doc =>
            {
                string loanNo = doc.GetValue("LoanNo", "").ToString();
                // Extract the last 1 to 5 digits dynamically
                return loanNo.Length >= 1 ? loanNo.Substring(Math.Max(loanNo.Length - 4, 0)) : string.Empty;
            }).ToList();

            // Highlight rows that are already in loan_approved collection
            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                string clientInfo = row.Cells["Client Info"].Value?.ToString();

                if (!string.IsNullOrEmpty(clientInfo))
                {
                    // Extract Loan ID from Client Info safely
                    string[] clientInfoParts = clientInfo.Split('\n');
                    string loanId = clientInfoParts.Length > 2 ? clientInfoParts[2].Split(':')[1].Trim() : string.Empty;

                    // Ensure loanId is not empty
                    if (!string.IsNullOrEmpty(loanId))
                    {
                        // Extract the last 1 to 5 digits dynamically
                        string lastDigits = loanId.Length >= 1 ? loanId.Substring(Math.Max(loanId.Length - 4, 0)) : string.Empty;

                        // Check if the last 1 to 5 digits of the loanId exist in approvedLoanIds
                        if (approvedLoanIds.Contains(lastDigits))
                        {
                            row.DefaultCellStyle.BackColor = Color.LightYellow; // Set background color to light yellow
                        }
                    }
                }
            }
        }



        private void refreshdata()
        {
            var rawData = loanRawdataCollection.Find(new BsonDocument()).ToList();
            rawDataList = rawData;

            DataTable dt = new DataTable();
            dt.Columns.Add("Item No.");
            dt.Columns.Add("Collector Info");
            dt.Columns.Add("Client Info");
            dt.Columns.Add("Loan Term Info");
            dt.Columns.Add("Loan Amount Info");
            dt.Columns.Add("Amortization Info");
            dt.Columns.Add("Penalty");
            dt.Columns.Add("Total Collection");
            dt.Columns.Add("Loan Status");
            dt.Columns.Add("Loan Status Date Update");

            foreach (var doc in rawData)
            {
                DataRow row = dt.NewRow();

                row["Item No."] = doc.GetValue("item_no", "").ToString();
                row["Collector Info"] = $"{doc.GetValue("collector_name", "")}\n{doc.GetValue("area_route", "")}";
                row["Client Info"] = $"{doc.GetValue("client_name", "")}\nContact No: {doc.GetValue("contact_no", "")}\nLoan ID: {doc.GetValue("loan_id", "")}";
                row["Loan Term Info"] = $"{doc.GetValue("loan_term", "")} months\n{doc.GetValue("payment_mode", "")}";

                // Safely handle conversion of Loan Amount
                double loanAmount = 0;
                if (!double.TryParse(doc.GetValue("loan_amount", "").ToString(), out loanAmount))
                {
                    loanAmount = 0;  // Default to 0 if parsing fails
                }

                // Safely handle conversion of Loan Balance
                double loanBalance = 0;
                if (!double.TryParse(doc.GetValue("loan_balance", "").ToString(), out loanBalance))
                {
                    loanBalance = 0;  // Default to 0 if parsing fails
                }

                row["Loan Amount Info"] = $"Amount: {loanAmount:N2}\nBalance: {loanBalance:N2}";

                // Safely handle conversion of Amortization Due
                double amortizationDue = 0;
                if (!double.TryParse(doc.GetValue("amortization_due", "").ToString(), out amortizationDue))
                {
                    amortizationDue = 0;  // Default to 0 if parsing fails
                }

                // Safely handle conversion of Loan Amortization
                double loanAmortization = 0;
                if (!double.TryParse(doc.GetValue("loan_amortization", "").ToString(), out loanAmortization))
                {
                    loanAmortization = 0;  // Default to 0 if parsing fails
                }

                row["Amortization Info"] = $"Amortization: {loanAmortization:N2}" +
                    $"\nDue: {amortizationDue:N2}" +
                    $"\nMissed Days: {doc.GetValue("missed_day", "")} days";

                // Safely handle conversion of Penalty
                double penaltyValue = 0;
                if (!double.TryParse(doc.GetValue("penalty", "").ToString(), out penaltyValue))
                {
                    penaltyValue = 0;  // Default to 0 if parsing fails
                }
                row["Penalty"] = penaltyValue.ToString("N2");

                // Safely handle conversion of Total Collection
                double totalCollectionValue = 0;
                if (!double.TryParse(doc.GetValue("total_collection", "").ToString(), out totalCollectionValue))
                {
                    totalCollectionValue = 0;  // Default to 0 if parsing fails
                }
                row["Total Collection"] = totalCollectionValue.ToString("N2");

                string loanStatus = doc.GetValue("loan_status", "").ToString();
                row["Loan Status"] = loanStatus;
                row["Loan Status Date Update"] = doc.GetValue("loan_status_date_updated", "").ToString();

                dt.Rows.Add(row);
            }

            dgvdata.DataSource = dt;

            // Center align the Item No column
            dgvdata.Columns["Item No."].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvdata.Columns["Loan Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvdata.Columns["Loan Status Date Update"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Adjust Client Info width
            dgvdata.Columns["Client Info"].Width = 200;
            dgvdata.Columns["Item No."].Width = 80;
            dgvdata.Columns["Loan Amount Info"].Width = 150;
            dgvdata.Columns["Amortization Info"].Width = 150;

            // Update Loan Status Labels
            UpdateLoanStatusLabels();
            HighlightApprovedLoans();
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




        private void tsearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = tsearch.Text.ToLower();
            FilterData(keyword);
        }

        private void FilterData(string keyword)
        {
            var filteredData = rawDataList.Where(doc =>
                doc.ToString().ToLower().Contains(keyword)
            ).ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add("Item No.");
            dt.Columns.Add("Collector Info");
            dt.Columns.Add("Client Info");
            dt.Columns.Add("Loan Term Info");
            dt.Columns.Add("Loan Amount Info");
            dt.Columns.Add("Amortization Info");
            dt.Columns.Add("Penalty");
            dt.Columns.Add("Total Collection");
            dt.Columns.Add("Loan Status");
            dt.Columns.Add("Loan Status Date Update");
            dt.Columns.Add("Actions");

            foreach (var doc in filteredData)
            {
                DataRow row = dt.NewRow();

                // Same logic as LoadDataToDataGridView method
                row["Item No."] = doc.GetValue("item_no", "").ToString();
                row["Collector Info"] = $"{doc.GetValue("collector_name", "")}\n{doc.GetValue("area_route", "")}";
                row["Client Info"] = $"{doc.GetValue("client_name", "")}\nContact No: {doc.GetValue("contact_no", "")}";
                row["Loan Term Info"] = $"{doc.GetValue("loan_term", "")} months\n{doc.GetValue("payment_mode", "")}";
                row["Loan Amount Info"] = $"Amount: {Convert.ToDouble(doc.GetValue("loan_amount", 0)):N2}\nBalance: {Convert.ToDouble(doc.GetValue("loan_balance", 0)):N2}";
                row["Amortization Info"] = $"Amortization: {Convert.ToDouble(doc.GetValue("loan_amortization", 0)):N2}\nDue: {Convert.ToDouble(doc.GetValue("amortization_due", 0)):N2}";
                row["Penalty"] = Convert.ToDouble(doc.GetValue("penalty", 0)).ToString("N2");
                row["Total Collection"] = Convert.ToDouble(doc.GetValue("total_collection", 0)).ToString("N2");

                string loanStatus = doc.GetValue("loan_status", "").ToString();
                row["Loan Status"] = loanStatus;
                row["Loan Status Date Update"] = doc.GetValue("loan_status_date_updated", "").ToString();

                dt.Rows.Add(row);
            }

            dgvdata.DataSource = dt;

            // Center align and adjust columns
            dgvdata.Columns["Item No."].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvdata.Columns["Client Info"].Width = 200;

            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                string status = row.Cells["Loan Status"].Value.ToString();
                switch (status)
                {
                    case "UPDATED":
                        row.Cells["Loan Status"].Style.BackColor = Color.Green;
                        break;
                    case "ARREARS":
                        row.Cells["Loan Status"].Style.BackColor = Color.Yellow;
                        break;
                    case "LITIGATION":
                        row.Cells["Loan Status"].Style.BackColor = Color.Orange;
                        break;
                    case "DORMANT":
                        row.Cells["Loan Status"].Style.BackColor = Color.Gray;
                        break;
                }

                DataGridViewButtonCell actionButton = new DataGridViewButtonCell();
                actionButton.Value = "View";
                row.Cells["Actions"] = actionButton;
            }

            dgvdata.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
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

        private async void dgvdata_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvdata.Columns["Actions"].Index)
            {
                // Get the selected row data
                DataGridViewRow selectedRow = dgvdata.Rows[e.RowIndex];

                // Retrieve the LoanNo and LoanStatus from the selected row
                string loanNo = selectedRow.Cells["Item No."].Value.ToString();  // Assuming Item No. corresponds to LoanNo
                string loanStatus = selectedRow.Cells["Loan Status"].Value.ToString();  // Get Loan Status

                // Check if the data already exists in the loan_approved collection
                var filter = Builders<BsonDocument>.Filter.Eq("LoanNo", loanNo); // Assuming LoanNo is stored as a string

                var existingData = await loanApprovedCollection.Find(filter).FirstOrDefaultAsync();

                if (existingData == null) // If no existing data is found
                {
                    // Open the new form and pass the relevant data
                    frm_home_ADMIN_rawdata_details loanDetailsForm = new frm_home_ADMIN_rawdata_details(loanNo, loanStatus);
                    loanDetailsForm.ShowDialog();  // Show the new form
                }
                else
                {
                    MessageBox.Show("This loan record already exists in the loan_approved collection.", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void tsearch_TextChanged_1(object sender, EventArgs e)
        {
            ApplySearchFilter(tsearch.Text.Trim());
        }

        private void cbloanstatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected loan status from the ComboBox
            string selectedLoanStatus = cbloanstatus.SelectedItem?.ToString();

            // Check if "Show All" is selected
            if (selectedLoanStatus == "Show All")
            {
                // Load all data without filtering
                LoadDataToDataGridView();
            }
            else
            {
                // Load filtered data based on selected loan status
                LoadDataToDataGridView(selectedLoanStatus);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using MongoDB.Bson;
using MongoDB.Driver;
using rct_lmis.DISBURSEMENT_SECTION;
using static MongoDB.Driver.WriteConcern;

namespace rct_lmis.ADMIN_SECTION
{
    public partial class frm_home_ADMIN_collectconfig : Form
    {
        private readonly IMongoCollection<BsonDocument> loanDisbursedCollection;
        private readonly IMongoCollection<BsonDocument> collectionConfigCollection;
        private readonly IMongoCollection<BsonDocument> loanCollectorsCollection;
        private DataTable dt = new DataTable(); // Store original CSV data
        private DataView dv; // DataView for filtering
        private IMongoDatabase database;
        private List<BsonDocument> loanData;


        public frm_home_ADMIN_collectconfig()
        {
            InitializeComponent();
            database = MongoDBConnection.Instance.Database;
            if (database != null)
            {
                loanDisbursedCollection = database.GetCollection<BsonDocument>("loan_disbursed");
                collectionConfigCollection = database.GetCollection<BsonDocument>("collection_config");
                loanCollectorsCollection = database.GetCollection<BsonDocument>("loan_collectors");
            }

            UpdateButtonStates();
        }

        private bool isClientNoUpdated = false;

        private async void LoadLoanDisbursedData()
        {
            try
            {
                // Fetch loan disbursed data from MongoDB asynchronously
                var collection = database.GetCollection<BsonDocument>("loan_disbursed");
                var loanData = await collection.Find(new BsonDocument()).ToListAsync();

                var collectionLoanCollections = database.GetCollection<BsonDocument>("loan_collections");
                var loanCollectionsData = await collectionLoanCollections.Find(new BsonDocument()).ToListAsync();

                // Extract names from loan_collections for comparison
                var loanCollectionsNames = loanCollectionsData.Select(lc => lc.Contains("Name") ? lc["Name"].AsString.Trim().ToUpper() : "").ToHashSet();

                // Extract unique CollectorNames and populate ComboBox
                var collectorNames = loanData.Select(loan => loan.Contains("CollectorName") ? loan["CollectorName"].AsString : "")
                                             .Where(name => !string.IsNullOrEmpty(name))
                                             .Distinct()
                                             .OrderBy(name => name)
                                             .ToList();
                collectorNames.Insert(0, "--All Collectors--");
                cbcollector.DataSource = collectorNames;

                // Initialize DataTable for DataGridView
                DataTable dt = new DataTable();
                dt.Columns.Add("ClientNo");
                dt.Columns.Add("Client Name");
                dt.Columns.Add("CollectorName");

                foreach (var loan in loanData)
                {
                    string clientNo = loan.Contains("ClientNo") ? loan["ClientNo"].AsString : "";
                    string lastName = loan.Contains("LastName") ? loan["LastName"].AsString : "";
                    string firstName = loan.Contains("FirstName") ? loan["FirstName"].AsString : "";
                    string middleName = loan.Contains("MiddleName") ? loan["MiddleName"].AsString : "";
                    string collectorName = loan.Contains("CollectorName") ? loan["CollectorName"].AsString : "";
                    string fullName = $"{firstName} {middleName} {lastName}".Trim().ToUpper();

                    DataRow row = dt.NewRow();
                    row["ClientNo"] = clientNo;
                    row["Client Name"] = fullName;
                    row["CollectorName"] = collectorName;
                    dt.Rows.Add(row);
                }

                dgvloans.DataSource = dt;
                dgvloans.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvloans.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dgvloans.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                dgvloans.RowPostPaint += (sender, e) =>
                {
                    string fullName = dgvloans.Rows[e.RowIndex].Cells["Client Name"].Value.ToString().ToUpper();
                    if (loanCollectionsNames.Contains(fullName))
                    {
                        dgvloans.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                    }
                };

                // Add Context Menu for Right-Click
                ContextMenuStrip contextMenu = new ContextMenuStrip();
                ToolStripMenuItem viewDetailsItem = new ToolStripMenuItem("View Collection Summary");
                viewDetailsItem.Click += (sender, e) =>
                {
                    if (dgvloans.SelectedRows.Count > 0)
                    {
                        string clientNo = dgvloans.SelectedRows[0].Cells["ClientNo"].Value.ToString();
                        var collectionSummary = loanCollectionsData.FirstOrDefault(lc => lc.Contains("ClientNo") && lc["ClientNo"].AsString == clientNo);
                        if (collectionSummary != null)
                        {
                            string summary = $"Client No: {clientNo}\n" +
                                             $"Name: {collectionSummary["Name"]}\n" +
                                             $"Total Loan Amount: {collectionSummary["TotalLoanToPay"]}\n" +
                                             $"Total Collected: {collectionSummary["TotalCollected"]}\n" +
                                             $"Remaining Balance: {collectionSummary["RunningBalance"]}\n";
                            MessageBox.Show(summary, "Collection Summary", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No collection data found for this client.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                };
                contextMenu.Items.Add(viewDetailsItem);
                dgvloans.ContextMenuStrip = contextMenu;

                // Attach event handler for search
                tsearchdata.TextChanged += (sender, e) =>
                {
                    string selectedCollector = cbcollector.SelectedItem?.ToString();
                    if (selectedCollector == "--All Collectors--") selectedCollector = null;

                    FilterLoansByCollector(loanData, selectedCollector, dt, tsearchdata.Text.Trim().ToUpper(), loanCollectionsNames);
                };

                // Attach event handler for ComboBox selection change
                cbcollector.SelectedIndexChanged += (sender, e) =>
                {
                    string selectedCollector = cbcollector.SelectedItem?.ToString();
                    if (selectedCollector == "--All Collectors--") selectedCollector = null;

                    FilterLoansByCollector(loanData, selectedCollector, dt, tsearchdata.Text.Trim().ToUpper(), loanCollectionsNames);
                };

                // Initial data load
                FilterLoansByCollector(loanData, null, dt, "", loanCollectionsNames);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading loan data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FilterLoansByCollector(List<BsonDocument> loanData, string selectedCollector, DataTable dt, string searchQuery, HashSet<string> loanCollectionsNames)
        {
            if (loanData == null)
            {
                MessageBox.Show("Loan data is not available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dt.Rows.Clear();

            foreach (var loan in loanData)
            {
                string clientNo = loan.Contains("ClientNo") ? loan["ClientNo"].AsString : "";
                string lastName = loan.Contains("LastName") ? loan["LastName"].AsString : "";
                string firstName = loan.Contains("FirstName") ? loan["FirstName"].AsString : "";
                string middleName = loan.Contains("MiddleName") ? loan["MiddleName"].AsString : "";
                string collectorName = loan.Contains("CollectorName") ? loan["CollectorName"].AsString : "";
                string fullName = $"{firstName} {middleName} {lastName}".Trim().ToUpper();

                // Check collector filter
                if (!string.IsNullOrEmpty(selectedCollector) && collectorName != selectedCollector)
                {
                    continue;
                }

                // Check search filter
                if (!string.IsNullOrEmpty(searchQuery) &&
                    !clientNo.ToUpper().Contains(searchQuery) &&
                    !fullName.Contains(searchQuery) &&
                    !collectorName.ToUpper().Contains(searchQuery))
                {
                    continue;
                }

                DataRow row = dt.NewRow();
                row["ClientNo"] = clientNo;
                row["Client Name"] = fullName;
                row["CollectorName"] = collectorName;
                dt.Rows.Add(row);

                if (loanCollectionsNames.Contains(fullName))
                {
                    dgvloans.Rows[dt.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightGreen;
                }
            }

            dgvloans.DataSource = dt;
        }

        private DataTable LoadCsvToDataTable(string filePath)
        {
            DataTable dt = new DataTable();
            int totalRows = 0;
            int processedRows = 0;

            try
            {
                this.Invoke(new Action(() =>
                {
                    pbloading.Visible = true;
                    lpercent.Visible = true;
                    pbloading.Maximum = 0;
                    pbloading.Value = 0;
                    lpercent.Text = "Processing... 0%";
                }));

                using (TextFieldParser parser = new TextFieldParser(filePath))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    bool isFirstRow = true;
                    List<string[]> allFields = new List<string[]>();

                    while (!parser.EndOfData)
                    {
                        allFields.Add(parser.ReadFields());
                        totalRows++;
                    }

                    this.Invoke(new Action(() =>
                    {
                        pbloading.Maximum = totalRows;
                    }));

                    List<DataRow> rowsToInsert = new List<DataRow>();

                    for (int rowIndex = 0; rowIndex < allFields.Count; rowIndex++)
                    {
                        string[] fields = allFields[rowIndex];
                        if (isFirstRow)
                        {
                            foreach (string column in fields)
                            {
                                dt.Columns.Add(column.Trim());
                            }

                            if (!dt.Columns.Contains("trans_no"))
                            {
                                dt.Columns.Add("trans_no");
                            }
                            if (!dt.Columns.Contains("collector_id"))
                            {
                                dt.Columns.Add("collector_id");
                            }

                            isFirstRow = false;
                        }
                        else
                        {
                            if (fields.Length < dt.Columns.Count - 1)
                            {
                                Array.Resize(ref fields, dt.Columns.Count - 1);
                            }

                            DataRow row = dt.NewRow();
                            for (int i = 0; i < fields.Length; i++)
                            {
                                row[i] = fields[i];
                            }

                            string loanAccountName = row["loan_account_name"].ToString();
                            if (loanAccountName.IndexOf("CANCELLED", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                row["trans_no"] = DBNull.Value;
                            }
                            else
                            {
                                string transNo = GenerateTransactionNumber(loanAccountName);
                                row["trans_no"] = string.IsNullOrEmpty(transNo) ? (object)DBNull.Value : transNo;
                            }

                            // Fetch collector_id based on collector_incharge
                            string collectorInCharge = row["collector_incharge"].ToString().Trim();
                            if (!string.IsNullOrEmpty(collectorInCharge))
                            {
                                string collectorId = GetCollectorId(collectorInCharge);
                                row["collector_id"] = string.IsNullOrEmpty(collectorId) ? (object)DBNull.Value : collectorId;
                            }

                            if (row["trans_no"] != DBNull.Value)
                            {
                                rowsToInsert.Add(row);
                            }

                            processedRows++;

                            this.Invoke(new Action(() =>
                            {
                                if (processedRows <= pbloading.Maximum)
                                {
                                    pbloading.Value = processedRows;
                                    lpercent.Text = $"{processedRows} of {totalRows} rows processed.";
                                }
                            }));

                            Application.DoEvents();
                        }
                    }

                    foreach (var row in rowsToInsert)
                    {
                        dt.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading CSV: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Invoke(new Action(() =>
            {
                pbloading.Visible = false;
                lpercent.Visible = false;
                pbloading.Value = pbloading.Maximum;
                lpercent.Text = "Upload Complete!";
            }));

            return dt;
        }

        private void SetupDataGridView()
        {
            dgvdata.AllowUserToAddRows = false;
            dgvdata.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Ensure the checkbox column is not duplicated
            if (dgvdata.Columns["chkSelect"] == null)
            {
                DataGridViewCheckBoxColumn chkColumn = new DataGridViewCheckBoxColumn
                {
                    Name = "chkSelect",
                    HeaderText = "",
                    Width = 50,
                    ReadOnly = false
                };

                dgvdata.Columns.Insert(0, chkColumn);
            }
        }

        private void BindDataTableToDataGridView(DataTable dt)
        {
            dgvdata.DataSource = dt;
            SetupDataGridView();
        }

        // Dictionary to store last generated trans_no for each loan_account_name
        private Dictionary<string, int> loanAccountNameTransNoMap = new Dictionary<string, int>();

        private string GenerateTransactionNumber(string loanAccountName)
        {
            try
            {
                if (loanDisbursedCollection == null || collectionConfigCollection == null)
                {
                    MessageBox.Show("Database not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                string[] nameParts = loanAccountName.Split(',');
                if (nameParts.Length < 1) return null;

                string lastName = nameParts[0].Trim();
                if (string.IsNullOrEmpty(lastName)) return null;

                var clientFilter = Builders<BsonDocument>.Filter.Regex("LastName", new BsonRegularExpression($"(?i)^{lastName}", "i"));
                var loanRecord = loanDisbursedCollection.Find(clientFilter).FirstOrDefault();

                if (loanRecord == null)
                {
                    Console.WriteLine("No matching client found.");
                    return null;
                }

                string clientNo = loanRecord.GetValue("ClientNo", "").AsString;

                var transFilter = Builders<BsonDocument>.Filter.Regex("trans_no", new BsonRegularExpression($"{clientNo}-COL-", "i"));
                var lastTrans = collectionConfigCollection.Find(transFilter)
                    .Sort(Builders<BsonDocument>.Sort.Descending("trans_no"))
                    .FirstOrDefault();

                if (loanAccountNameTransNoMap.ContainsKey(loanAccountName))
                {
                    loanAccountNameTransNoMap[loanAccountName]++;
                }
                else
                {
                    loanAccountNameTransNoMap[loanAccountName] = 1;
                }

                int nextColNumber = loanAccountNameTransNoMap[loanAccountName];
                return $"{clientNo}-COL-{nextColNumber:D4}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating Transaction Number: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void FilterLoansByCollector(List<BsonDocument> loanData, string selectedCollector, DataTable dt)
        {
            // Clear the existing rows before adding filtered ones
            dt.Clear();

            // If the selectedCollector is "--all collectors--", show all data
            if (selectedCollector == "--all collectors--")
            {
                // Add all data to DataTable without filtering
                foreach (var loan in loanData)
                {
                    string clientNo = loan.Contains("ClientNo") ? loan["ClientNo"].AsString : "";
                    string lastName = loan.Contains("LastName") ? loan["LastName"].AsString : "";
                    string firstName = loan.Contains("FirstName") ? loan["FirstName"].AsString : "";
                    string collectorName = loan.Contains("CollectorName") ? loan["CollectorName"].AsString : "";

                    string fullName = $"{lastName}, {firstName}"; // Merge LastName and FirstName

                    // Add to DataTable
                    dt.Rows.Add(clientNo, fullName, collectorName);
                }
            }
            else
            {
                // Filter loan data based on selected CollectorName
                var filteredLoans = loanData.Where(loan =>
                    string.IsNullOrEmpty(selectedCollector) || loan.Contains("CollectorName") && loan["CollectorName"].AsString == selectedCollector
                ).ToList();

                foreach (var loan in filteredLoans)
                {
                    string clientNo = loan.Contains("ClientNo") ? loan["ClientNo"].AsString : "";
                    string lastName = loan.Contains("LastName") ? loan["LastName"].AsString : "";
                    string firstName = loan.Contains("FirstName") ? loan["FirstName"].AsString : "";
                    string collectorName = loan.Contains("CollectorName") ? loan["CollectorName"].AsString : "";

                    string fullName = $"{lastName}, {firstName}"; // Merge LastName and FirstName

                    // Add to DataTable
                    dt.Rows.Add(clientNo, fullName, collectorName);
                }
            }

            // Refresh the DataGridView with the filtered or all data
            dgvloans.DataSource = dt;
        }

        // Function to get collector_id from loan_collectors collection
        private string GetCollectorId(string collectorIncharge)
        {
            try
            {
                if (loanCollectorsCollection == null)
                {
                    MessageBox.Show("Loan collectors database not initialized!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                var filter = Builders<BsonDocument>.Filter.Regex("CollectorName", new BsonRegularExpression($"(?i)^{collectorIncharge}$", "i"));
                var collectorRecord = loanCollectorsCollection.Find(filter).FirstOrDefault();

                if (collectorRecord != null && collectorRecord.Contains("GeneratedIDNumber"))
                {
                    return collectorRecord["GeneratedIDNumber"].AsString;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching Collector ID: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return null; // Return null if no collector found
        }

        // Function to update total cash received
        private void UpdateTotalCashReceived()
        {
            decimal totalCashReceived = 0;
            int rowCount = 0; // Variable to hold the count of rows

            foreach (DataRowView row in dv)
            {
                // Count the number of rows
                rowCount++;

                if (row["cash_received_dr"] != DBNull.Value)
                {
                    totalCashReceived += Convert.ToDecimal(row["cash_received_dr"]);
                }
            }

            // Update the labels dynamically
            lcolcount.Text = $"Total Cash Received: ₱{totalCashReceived:N2}";
            lcountprocess.Text = $"Total Collections Processed: {rowCount}";
        }

        private void UpdateButtonStates()
        {
            // Check if there are any rows in the DataGridView
            if (dgvdata.Rows.Count > 0)
            {
                // Enable buttons if data exists
                bdessiminate.Enabled = true;
                bclear.Enabled = true;
                chkSelectAll.Enabled = true;
                bupdate.Enabled = true;
                pbloading.Visible = true;
                lpercent.Visible = true;
            }
            else
            {
                // Disable buttons if no data
                bdessiminate.Enabled = false;
                bclear.Enabled = false;
                chkSelectAll.Enabled = false;  
                bupdate.Enabled = false;
                pbloading.Visible = false;
                lpercent.Visible = false;
            }
        }


        private void frm_home_ADMIN_collectconfig_Load(object sender, EventArgs e)
        {
            // Initialize DataTable to avoid null reference
            dt = new DataTable();
            LoadLoanDisbursedData();
        }

        private async void bupload_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    // Initialize the progress bar
                    pbloading.Value = 0;
                    pbloading.Maximum = 0;
                    lpercent.Text = "0%";

                    // Load CSV in a background task to prevent UI freezing
                    await Task.Run(async () =>
                    {
                        // Load CSV data to DataTable
                        DataTable loadedData = LoadCsvToDataTable(filePath);

                        // Fetch existing loan collections from MongoDB
                        var collectionLoanCollections = database.GetCollection<BsonDocument>("loan_collections");
                        var loanCollectionsData = await collectionLoanCollections.Find(new BsonDocument()).ToListAsync();
                        var existingClientNos = loanCollectionsData.Select(lc => lc.Contains("ClientNo") ? lc["ClientNo"].AsString : "").ToHashSet();

                        // Safely update the UI from the background thread
                        this.Invoke(new Action(() =>
                        {
                            dt = loadedData;

                            // Call BindDataTableToDataGridView to update DataGridView
                            BindDataTableToDataGridView(dt);

                            dv = new DataView(dt);

                            // Set the maximum value of the progress bar
                            pbloading.Maximum = dt.Rows.Count;

                            // Update the total row count in lcountprocess
                            lcountprocess.Text = $"Total Collections Processed: {dt.Rows.Count}";

                            // Sum the 'cash_received_dr' column and update lcolcount
                            decimal totalCashReceived = 0;
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row["cash_received_dr"] != DBNull.Value)
                                {
                                    totalCashReceived += Convert.ToDecimal(row["cash_received_dr"]);
                                }
                            }
                            lcolcount.Text = $"Total Cash Received: ₱{totalCashReceived:N2}";

                            UpdateButtonStates();

                            // Enable word wrapping for each cell in the DataGridView
                            foreach (DataGridViewColumn column in dgvdata.Columns)
                            {
                                column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                            }

                            // Enable AutoSize for rows and columns
                            dgvdata.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                            dgvdata.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                            dgvdata.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                            // Adjust row heights for better visual display
                            foreach (DataGridViewRow row in dgvdata.Rows)
                            {
                                row.Height = 50;
                            }

                            // Highlight rows in yellow if the loan_account_name exists in loan_collections
                            foreach (DataGridViewRow row in dgvdata.Rows)
                            {
                                string clientNo = row.Cells["loan_account_name"].Value?.ToString();
                                if (!string.IsNullOrEmpty(clientNo) && existingClientNos.Contains(clientNo))
                                {
                                    row.DefaultCellStyle.BackColor = Color.Yellow;
                                }
                            }

                            // Update progress bar and percentage
                            int processedRows = 0;
                            foreach (DataRow row in dt.Rows)
                            {
                                processedRows++;

                                // Update progress bar and percentage
                                this.Invoke(new Action(() =>
                                {
                                    pbloading.Value = processedRows;
                                    lpercent.Text = $"{(processedRows * 100 / dt.Rows.Count)}%";
                                }));
                            }
                        }));
                    });
                }
            }
        }



        private void ApplyFilter(string filterText)
        {
            if (dv != null)
            {
                filterText = filterText.Replace("'", "''");

                if (string.IsNullOrEmpty(filterText))
                {
                    dv.RowFilter = ""; // Clear filter
                }
                else
                {
                    dv.RowFilter = $"CONVERT(trans_no, 'System.String') LIKE '%{filterText}%' OR " +
                                   $"CONVERT(or_no, 'System.String') LIKE '%{filterText}%' OR " +
                                   $"loan_account_name LIKE '%{filterText}%' OR " +
                                   $"collector_incharge LIKE '%{filterText}%'";

                }

                dgvdata.DataSource = dv;

                // Recalculate totalCashReceived based on filtered data
                UpdateTotalCashReceived();

                UpdateButtonStates();
            }
        }

        private void FilterLoansByCollector(List<BsonDocument> loanData, string selectedCollector, DataTable dt, string searchQuery = "")
        {
            if (loanData == null)
            {
                MessageBox.Show("Loan data is not available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dt.Rows.Clear();

            foreach (var loan in loanData)
            {
                string clientNo = loan.Contains("ClientNo") ? loan["ClientNo"].AsString : "";
                string lastName = loan.Contains("LastName") ? loan["LastName"].AsString : "";
                string firstName = loan.Contains("FirstName") ? loan["FirstName"].AsString : "";
                string collectorName = loan.Contains("CollectorName") ? loan["CollectorName"].AsString : "";

                string fullName = $"{lastName}, {firstName}";

                // Check collector filter
                if (!string.IsNullOrEmpty(selectedCollector) && collectorName != selectedCollector)
                {
                    continue;
                }

                // Check search filter
                if (!string.IsNullOrEmpty(searchQuery) &&
                    !clientNo.ToLower().Contains(searchQuery) &&
                    !fullName.ToLower().Contains(searchQuery) &&
                    !collectorName.ToLower().Contains(searchQuery))
                {
                    continue;
                }

                dt.Rows.Add(clientNo, fullName, collectorName);
            }

            dgvloans.DataSource = dt;
        }


        private void tsearch_TextChanged(object sender, EventArgs e)
        {
            // Call the ApplyFilter function whenever the filter text changes
            ApplyFilter(tsearch.Text.Trim());
        }

        private HashSet<string> modifiedRows = new HashSet<string>();

        private void dgvloans_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string clientNo = dgvloans.Rows[e.RowIndex].Cells["ClientNo"].Value.ToString();
                modifiedRows.Add(clientNo); // Track edited rows
            }
        }

        private async void bupdateloanacc_Click(object sender, EventArgs e)
        {
            if (modifiedRows.Count == 0)
            {
                MessageBox.Show("No changes detected.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var collection = database.GetCollection<BsonDocument>("loan_disbursed");

                foreach (DataGridViewRow row in dgvloans.Rows)
                {
                    if (row.IsNewRow) continue; // Skip new row placeholders

                    string clientNo = row.Cells["ClientNo"].Value?.ToString();
                    if (string.IsNullOrEmpty(clientNo) || !modifiedRows.Contains(clientNo))
                        continue; // Skip unmodified rows

                    string clientName = row.Cells["Client Name"].Value?.ToString();
                    string collectorName = row.Cells["CollectorName"].Value?.ToString();

                    var filter = Builders<BsonDocument>.Filter.Eq("ClientNo", clientNo);
                    var update = Builders<BsonDocument>.Update
                        .Set("Client Name", clientName)
                        .Set("CollectorName", collectorName);

                    await collection.UpdateOneAsync(filter, update);
                }

                MessageBox.Show("Loan accounts updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                modifiedRows.Clear();
                await ReloadLoanDisbursedData(); // Ensuring data reload is awaited
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating loan data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ReloadLoanDisbursedData()
        {
            try
            {
                // Fetch loan collections data from MongoDB asynchronously
                var collectionLoanCollections = database.GetCollection<BsonDocument>("loan_collections");
                var loanCollectionsData = await collectionLoanCollections.Find(new BsonDocument()).ToListAsync();

                // Extract names from loan_collections for comparison
                var loanCollectionsNames = loanCollectionsData.Select(lc => lc.Contains("Name") ? lc["Name"].AsString.Trim().ToUpper() : "").ToHashSet();

                // Reapply row highlighting based on collection names
                foreach (DataGridViewRow row in dgvloans.Rows)
                {
                    string fullName = row.Cells["Client Name"].Value.ToString().ToUpper();
                    if (loanCollectionsNames.Contains(fullName))
                    {
                        row.DefaultCellStyle.BackColor = Color.LightGreen;  // Highlight matching rows
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White;  // Reset row color for non-matching rows
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reloading loan disbursed data for highlighting: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void UpdateTransactionPrefix(string oldPrefix, string newPrefix)
        {
            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                string transNo = row.Cells["trans_no"].Value.ToString();
                if (transNo.StartsWith(oldPrefix))
                {
                    row.Cells["trans_no"].Value = newPrefix + transNo.Substring(oldPrefix.Length);
                }
            }
        }



        private void bupdate_Click(object sender, EventArgs e)
        {
            // Ensure a row is selected
            if (dgvdata.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to update!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Ensure data and trans_no column exist
            if (dt == null || !dt.Columns.Contains("trans_no"))
            {
                MessageBox.Show("No data loaded or 'trans_no' column missing!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Get the selected transaction number
            string selectedTransNo = dgvdata.SelectedRows[0].Cells["trans_no"].Value.ToString();
            if (string.IsNullOrWhiteSpace(selectedTransNo))
            {
                MessageBox.Show("Selected transaction number is empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Extract the prefix (everything before the last "-COL-XXXX")
            int lastIndex = selectedTransNo.LastIndexOf("-COL-");
            if (lastIndex == -1)
            {
                MessageBox.Show("Invalid transaction number format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string oldPrefix = selectedTransNo.Substring(0, lastIndex);

            // Prompt user for the new prefix
            string newPrefix = Microsoft.VisualBasic.Interaction.InputBox($"Current Prefix: {oldPrefix}\nEnter New Prefix:", "Update Prefix", oldPrefix);
            if (string.IsNullOrWhiteSpace(newPrefix) || newPrefix == oldPrefix)
            {
                MessageBox.Show("Invalid or same prefix!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Save the current filter text
            string filterText = tsearch.Text.Trim();

            // Update transaction numbers
            UpdateTransactionPrefix(oldPrefix, newPrefix);

            // Reapply the filter after updating the prefix
            ApplyFilter(filterText);

            // Show success message
            MessageBox.Show($"Updated all transaction numbers from '{oldPrefix}' to '{newPrefix}' successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bclear_Click(object sender, EventArgs e)
        {
            // Clear the DataGridView by setting its DataSource to null
            dgvdata.DataSource = null;

            // Clear the DataTable and DataView objects as well
            dt = null;
            dv = null;

            // Reset the progress bar and other related UI elements
            pbloading.Value = 0;
            pbloading.Maximum = 0;
            lpercent.Text = "0%";
            lcountprocess.Text = "Total Collections Processed: 0";
            lcolcount.Text = "Total Cash Received: ₱0.00";

            // Disable the 'bdessiminate' and 'bclear' buttons as there is no data
            UpdateButtonStates();
        }


        private decimal ParseCurrency(string amount)
        {
            if (string.IsNullOrEmpty(amount))
                return 0.0m;

            amount = amount.Replace("₱", "").Replace(",", "").Trim();
            return decimal.TryParse(amount, out decimal result) ? result : 0.0m;
        }



        private async Task<dynamic> GetLoanDetailsAsync(string clientNo)
        {
            var loanCollection = database.GetCollection<BsonDocument>("loan_disbursed");
            var filter = Builders<BsonDocument>.Filter.Eq("ClientNo", clientNo);
            var loanDocument = await loanCollection.Find(filter).FirstOrDefaultAsync();

            if (loanDocument == null)
            {
                Console.WriteLine($"No data found for ClientNo: {clientNo}");
                return null;
            }

            return new
            {
                LoanID = loanDocument.Contains("LoanNo") ? loanDocument["LoanNo"].AsString : "N/A",
                Name = $"{loanDocument["FirstName"].AsString} {loanDocument["MiddleName"].AsString} {loanDocument["LastName"].AsString}",
                Address = $"{loanDocument["Barangay"].AsString}, {loanDocument["City"].AsString}, {loanDocument["Province"].AsString}",
                Contact = "N/A", // No contact info in your data
                LoanAmount = loanDocument.Contains("LoanAmount") ? ParseCurrency(loanDocument["LoanAmount"].AsString) : 0.0m,
                LoanTerm = loanDocument.Contains("LoanTerm") ? loanDocument["LoanTerm"].AsString : "N/A",
                PaymentStartDate = loanDocument.Contains("StartPaymentDate") ? loanDocument["StartPaymentDate"].AsString : "N/A",
                PaymentMaturityDate = loanDocument.Contains("MaturityDate") ? loanDocument["MaturityDate"].AsString : "N/A",
                PaymentsMode = loanDocument.Contains("PaymentMode") ? loanDocument["PaymentMode"].AsString : "N/A",
                Amortization = loanDocument.Contains("LoanAmortization") ? ParseCurrency(loanDocument["LoanAmortization"].AsString) : 0.0m,
                AmortizationPrincipal = 0.0m,  // No explicit principal amortization in your data
                AmortizationInterest = 0.0m,  // No explicit interest in your data
                PaymentStatus = loanDocument.Contains("LoanProcessStatus") ? loanDocument["LoanProcessStatus"].AsString : "No payments made yet",
                RunningBalance = loanDocument.Contains("LoanBalance") ? ParseCurrency(loanDocument["LoanBalance"].AsString) : 0.0m,
                TotalLoanToPay = loanDocument.Contains("LoanAmount") ? ParseCurrency(loanDocument["LoanAmount"].AsString) : 0.0m,
                PrincipalBalance = loanDocument.Contains("LoanBalance") ? ParseCurrency(loanDocument["LoanBalance"].AsString) : 0.0m
            };
        }



        private async Task SaveLoanCollectionDataAsync(BsonDocument collectionData)
        {
            var collection = database.GetCollection<BsonDocument>("loan_collections");
            await collection.InsertOneAsync(collectionData);
        }


        private async void bdessiminate_Click(object sender, EventArgs e)
        {
            var selectedRows = dgvdata.Rows.Cast<DataGridViewRow>()
                .Where(row => row.Cells["chkSelect"].Value != null && (bool)row.Cells["chkSelect"].Value)
                .ToList();

            if (selectedRows.Count == 0)
            {
                MessageBox.Show("No rows selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show($"Do you want to save collection data for {selectedRows.Count} selected clients?",
                                                 "Confirm Save",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);

            if (confirmResult != DialogResult.Yes)
            {
                return;
            }

            frm_home_ZProgress progressForm = new frm_home_ZProgress();
            progressForm.Show();

            int totalRows = selectedRows.Count;
            int progressCount = 0;
            DateTime todayDate = DateTime.Now;

            List<BsonDocument> batchData = new List<BsonDocument>();

            foreach (DataGridViewRow row in selectedRows)
            {
                progressCount++;
                int progressPercent = (int)((progressCount / (double)totalRows) * 100);
                progressForm.UpdateProgress(progressCount, totalRows);

                string transNo = row.Cells["trans_no"].Value?.ToString() ?? string.Empty;
                string[] transParts = transNo.Split('-');
                string clientNo = (transParts.Length >= 3) ? $"{transParts[0]}-{transParts[1]}-{transParts[2]}" : transNo;

                var loanDetails = await GetLoanDetailsAsync(clientNo);
                if (loanDetails == null)
                {
                    MessageBox.Show($"Loan details not found for ClientNo: {clientNo}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue;
                }

                string collectionDate = row.Cells["date"].Value?.ToString() ?? "N/A";
                string collector = row.Cells["collector_incharge"].Value?.ToString() ?? "N/A";
                string paymentMode = "Cash";

                decimal principalDue = 0.0m;
                decimal totalCollected = 0.0m;
                decimal actualCollection = 0.0m;
                decimal collectionPayment = 0.0m;
                decimal principalPaid = 0.0m;

                if (row.Cells["cashonhand_dr"].Value != null)
                {
                    decimal.TryParse(row.Cells["cashonhand_dr"].Value.ToString(), out principalDue);
                    totalCollected = principalDue;
                    actualCollection = principalDue;
                    collectionPayment = principalDue;
                    principalPaid = principalDue;
                }

                DateTime parsedCollectionDate = DateTime.TryParse(collectionDate, out var validCollectionDate) ? validCollectionDate : todayDate;

                // Create the collection data for loan details
                var collectionData = new BsonDocument
                  {
                      { "ClientNo", clientNo },
                      { "LoanID", loanDetails.LoanID },
                      { "Name", loanDetails.Name },
                      { "ClientNumber", clientNo },
                      { "Address", loanDetails.Address ?? "N/A" },
                      { "Contact", loanDetails.Contact ?? "N/A" },
                      { "LoanAmount", loanDetails.LoanAmount },
                      { "LoanTerm", loanDetails.LoanTerm },
                      { "PaymentStartDate", loanDetails.PaymentStartDate != null ? DateTime.Parse(loanDetails.PaymentStartDate) : DateTime.MinValue },
                      { "PaymentMaturityDate", loanDetails.PaymentMaturityDate != null ? DateTime.Parse(loanDetails.PaymentMaturityDate) : DateTime.MinValue },
                      { "PaymentsMode", loanDetails.PaymentsMode ?? "N/A" },
                      { "Amortization", loanDetails.Amortization },
                      { "AmortizationPrincipal", loanDetails.AmortizationPrincipal },
                      { "AmortizationInterest", loanDetails.AmortizationInterest },
                      { "PaymentStatus", loanDetails.PaymentStatus ?? "No payments made yet" },
                      { "CollectionDate", parsedCollectionDate },
                      { "Collector", collector },
                      { "PaymentMode", paymentMode },
                      { "PrincipalDue", principalDue },
                      { "CollectedInterest", 0.0m },
                      { "TotalCollected", totalCollected },
                      { "ActualCollection", actualCollection },
                      { "CollectionReferenceNo", "N/A" },
                      { "DateReceived", todayDate },
                      { "DateProcessed", todayDate },
                      { "CollectionPayment", collectionPayment },
                      { "RunningBalance", loanDetails.RunningBalance },
                      { "TotalLoanToPay", loanDetails.TotalLoanToPay },
                      { "Bank", "N/A" },
                      { "Branch", "N/A" },
                      { "InterestPaid", 0.0m },
                      { "PrincipalPaid", principalPaid },
                      { "PrincipalBalance", loanDetails.PrincipalBalance }
                  };

                // Create accounting data to be inserted into loan_accounting_collection
                var accountingData = new BsonDocument
                  {
                      { "ClientNo", clientNo },
                      { "LoanID", loanDetails.LoanID },
                      { "CollectionDate", parsedCollectionDate },
                      { "Collector", collector },
                      { "PaymentMode", paymentMode },

                      // Loans Receivable: Credit
                      { "LoansReceivableCredit", loanDetails.TotalLoanToPay }, // Credit to Loans Receivable account

                      // Cash: Debit
                      { "CashDebit", totalCollected }, // Debit to Cash account

                      // Interest Receivable: Credit (if applicable)
                      { "InterestReceivableCredit", loanDetails.AmortizationInterest }, // Credit to Interest Receivable account

                      // Principal Paid: Debit (reduces loan balance)
                      { "PrincipalDebit", principalPaid }, // Debit to reduce principal balance

                      // Interest Income: Credit (income earned)
                      { "InterestIncomeCredit", loanDetails.AmortizationInterest }, // Credit to Interest Income account

                      // Loans Receivable: Debit (reduce loan balance)
                      { "LoansReceivableDebit", principalPaid } // Debit to reduce loan balance
                  };

                // Insert accounting data into "loan_accounting_collection"
                await InsertAccountingDataAsync(accountingData);

                batchData.Add(collectionData);

                if (batchData.Count >= 100)
                {
                    await SaveLoanCollectionDataBatchAsync(batchData);
                    batchData.Clear();
                }
            }

            if (batchData.Count > 0)
            {
                await SaveLoanCollectionDataBatchAsync(batchData);
            }

            progressForm.Close();
            MessageBox.Show("Collections data has been successfully disseminated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await ReloadLoanDisbursedData();
        }

        private async Task InsertAccountingDataAsync(BsonDocument accountingData)
        {
            var accountingCollection = database.GetCollection<BsonDocument>("loan_accounting_collection");
            await accountingCollection.InsertOneAsync(accountingData);
        }


        private async Task SaveLoanCollectionDataBatchAsync(List<BsonDocument> batchData)
        {
            var collection = database.GetCollection<BsonDocument>("loan_collections");
            if (batchData.Count > 0)
            {
                await collection.InsertManyAsync(batchData);
            }
        }


        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkSelectAll.Checked;

            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                row.Cells["chkSelect"].Value = isChecked;
            }
        }

        private void dgvdata_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvdata.Columns["chkSelect"].Index && e.RowIndex >= 0)
            {
                bool allChecked = dgvdata.Rows.Cast<DataGridViewRow>()
                    .All(row => row.Cells["chkSelect"].Value != null && (bool)row.Cells["chkSelect"].Value);

                chkSelectAll.CheckedChanged -= chkSelectAll_CheckedChanged; // Temporarily remove event
                chkSelectAll.Checked = allChecked;
                chkSelectAll.CheckedChanged += chkSelectAll_CheckedChanged; // Reattach event
            }
        }

        private void tsearchdata_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void dgvloans_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvdata.ClearSelection();
        }
    }
}

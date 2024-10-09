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
        private List<BsonDocument> rawDataList;

        public frm_home_ADMIN_rawdata()
        {
            InitializeComponent();
            database = MongoDBConnection.Instance.Database;
            loanRawdataCollection = database.GetCollection<BsonDocument>("loan_rawdata");
            loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");
            rawDataList = new List<BsonDocument>();
        }

        LoadingFunction load = new LoadingFunction();

        private void frm_home_ADMIN_rawdata_Load(object sender, EventArgs e)
        {
            LoadDataToDataGridView();
        }

        private void LoadDataToDataGridView()
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
                row["Loan Amount Info"] = $"Amount: {Convert.ToDouble(doc.GetValue("loan_amount", 0)):N2}\nBalance: {Convert.ToDouble(doc.GetValue("loan_balance", 0)):N2}";
                row["Amortization Info"] = $"Amortization: {Convert.ToDouble(doc.GetValue("loan_amortization", 0)):N2}" +
                    $"\nDue: {Convert.ToDouble(doc.GetValue("amortization_due", 0)):N2}" +
                    $"\nMissed Days: {doc.GetValue("missed_day", "")} days";

                // Safely handle conversion of Penalty
                bool isPenaltyNumeric = double.TryParse(doc.GetValue("penalty", 0).ToString(), out double penaltyValue);
                row["Penalty"] = isPenaltyNumeric ? penaltyValue.ToString("N2") : "0.00";

                // Safely handle conversion of Total Collection
                bool isTotalCollectionNumeric = double.TryParse(doc.GetValue("total_collection", 0).ToString(), out double totalCollectionValue);
                row["Total Collection"] = isTotalCollectionNumeric ? totalCollectionValue.ToString("N2") : "0.00";

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
            var approvedLoanIds = approvedLoans.Select(doc => doc.GetValue("LoanNo", "").ToString().Substring(doc.GetValue("LoanNo", "").ToString().Length - 5)).ToList();

            // Highlight rows that are already in loan_approved collection
            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                string clientInfo = row.Cells["Client Info"].Value.ToString();

                // Extract Loan ID from Client Info safely
                string[] clientInfoParts = clientInfo.Split('\n');
                string loanId = clientInfoParts.Length > 2 ? clientInfoParts[2].Split(':')[1].Trim() : string.Empty;

                // Check if loanId is not empty and at least 5 characters long before checking the last 5 digits
                if (!string.IsNullOrEmpty(loanId) && loanId.Length >= 5)
                {
                    string lastFiveDigits = loanId.Substring(loanId.Length - 5);

                    // Log the last 5 digits detected
                    //Console.WriteLine($"Detected Last 5 Digits: {lastFiveDigits}");

                    // Check if the last 5 digits of the loanId exist in approvedLoanIds
                    if (approvedLoanIds.Contains(lastFiveDigits))
                    {
                        row.DefaultCellStyle.BackColor = Color.LightYellow;  // Set background color to light yellow
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
            dt.Columns.Add("Actions");

            foreach (var doc in rawData)
            {
                DataRow row = dt.NewRow();

                row["Item No."] = doc.GetValue("item_no", "").ToString();
                row["Collector Info"] = $"{doc.GetValue("collector_name", "")}\n{doc.GetValue("area_route", "")}";
                row["Client Info"] = $"{doc.GetValue("client_name", "")}\nContact No: {doc.GetValue("contact_no", "")}\nLoan ID: {doc.GetValue("loan_id", "")}";
                row["Loan Term Info"] = $"{doc.GetValue("loan_term", "")} months\n{doc.GetValue("payment_mode", "")}";
                row["Loan Amount Info"] = $"Amount: {Convert.ToDouble(doc.GetValue("loan_amount", 0)):N2}\nBalance: {Convert.ToDouble(doc.GetValue("loan_balance", 0)):N2}";
                row["Amortization Info"] = $"Amortization: {Convert.ToDouble(doc.GetValue("loan_amortization", 0)):N2}" +
                  $"\nDue: {Convert.ToDouble(doc.GetValue("amortization_due", 0)):N2}" +
                  $"\nMissed Days: {doc.GetValue("missed_day", "")} days";

                // Safely handle conversion of Penalty
                bool isPenaltyNumeric = double.TryParse(doc.GetValue("penalty", 0).ToString(), out double penaltyValue);
                row["Penalty"] = isPenaltyNumeric ? penaltyValue.ToString("N2") : "0.00";

                // Safely handle conversion of Total Collection
                bool isTotalCollectionNumeric = double.TryParse(doc.GetValue("total_collection", 0).ToString(), out double totalCollectionValue);
                row["Total Collection"] = isTotalCollectionNumeric ? totalCollectionValue.ToString("N2") : "0.00";

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

            UpdateLoanStatusLabels();
        }


        private void UpdateLoanStatusLabels()
        {
            int totalRows = dgvdata.Rows.Count;
            int updatedCount = 0;
            int arrearsCount = 0;
            int litigationCount = 0;
            int dormantCount = 0;

            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                string loanStatus = row.Cells["Loan Status"].Value.ToString();

                switch (loanStatus)
                {
                    case "UPDATED":
                        updatedCount++;
                        break;
                    case "ARREARS":
                        arrearsCount++;
                        break;
                    case "LITIGATION":
                        litigationCount++;
                        break;
                    case "DORMANT":
                        dormantCount++;
                        break;
                }
            }

            // Set the text and background color of the labels
            laccounttotal.Text = $"{totalRows}";

            // UPDATED status
            lstatusupdated.Text = $"UPDATED: {updatedCount}";
            lstatusupdated.BackColor = Color.Green; // Green background for UPDATED

            // ARREARS status
            lstatusarrears.Text = $"ARREARS: {arrearsCount}";
            lstatusarrears.BackColor = Color.Yellow; // Yellow background for ARREARS

            // LITIGATION status
            lstatuslitigation.Text = $"LITIGATION: {litigationCount}";
            lstatuslitigation.BackColor = Color.Orange; // Orange background for LITIGATION

            // DORMANT status
            lstatusdormant.Text = $"DORMANT: {dormantCount}";
            lstatusdormant.BackColor = Color.Gray; // Gray background for DORMANT
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
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvdata.Columns["Actions"].Index)
            {
                // Get the selected row data
                DataGridViewRow selectedRow = dgvdata.Rows[e.RowIndex];

                // Retrieve the loan_id, client_name, or other relevant data
                string loanId = selectedRow.Cells["Item No."].Value.ToString();  // Example: Get the Item No.
                string loanstatus = selectedRow.Cells["Loan Status"].Value.ToString();  // Example: Get the Client Info

                // Open the new form and pass the relevant data
                frm_home_ADMIN_rawdata_details loanDetailsForm = new frm_home_ADMIN_rawdata_details(loanId, loanstatus);
                loanDetailsForm.ShowDialog();  // Show the new form
            }
        }
    }
}

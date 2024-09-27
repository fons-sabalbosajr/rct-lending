using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;


namespace rct_lmis.DISBURSEMENT_SECTION
{
    public partial class frm_home_disburse_collections : Form
    {
        private string _loanId;
        private IMongoCollection<BsonDocument> _loanDisbursedCollection;
        private DataTable _loanCollectionTable;

        public frm_home_disburse_collections(string loanId)
        {
            InitializeComponent();
            _loanId = loanId;

            // MongoDB connection initialization
            var database = MongoDBConnection.Instance.Database;
            _loanDisbursedCollection = database.GetCollection<BsonDocument>("loan_collections");

            // Initialize DataTable for binding to DataGridView
            _loanCollectionTable = new DataTable();
        }

        private void LoadLoanCollections()
        {
            // Ensure the _loanCollectionTable is initialized with the correct columns
            if (_loanCollectionTable.Columns.Count == 0)
            {
                // Add columns for each category: Client, Loan, Payment, Collection Information
                _loanCollectionTable.Columns.Add("Client Information", typeof(string));
                _loanCollectionTable.Columns.Add("Loan Information", typeof(string));
                _loanCollectionTable.Columns.Add("Payment Information", typeof(string));
                _loanCollectionTable.Columns.Add("Collection Information", typeof(string));
                _loanCollectionTable.Columns.Add("Remarks", typeof(string));
            }

            // Clear existing rows before loading new data
            _loanCollectionTable.Rows.Clear();
            dgvdata.DataSource = null; // Clear existing data source

            // Query to get the loan collections based on LoanID
            var filter = Builders<BsonDocument>.Filter.Eq("LoanID", _loanId);
            var loanCollections = _loanDisbursedCollection.Find(filter).ToList();

            foreach (var collection in loanCollections)
            {
                // Client Information
                string collectionDate = collection.Contains("CollectionDate") ? collection["CollectionDate"].ToUniversalTime().ToString("yyyy-MM-dd") : "";
                string accountId = collection.Contains("AccountId") ? collection["AccountId"].AsString : "";
                string name = collection.Contains("Name") ? collection["Name"].AsString : "";

                // Concatenate Client Information fields into a single string
                string clientInfo = $"Col. Date: {collectionDate}\n" +
                                    $"Col. No.: {accountId}\n" +
                                    $"Name: {name}";

                // Loan Information
                string loanAmount = collection.Contains("LoanAmount") ? ((double)collection["LoanAmount"].AsDecimal128).ToString("F2") : "0.00";
                string paymentsMode = collection.Contains("PaymentsMode") ? collection["PaymentsMode"].AsString : "";
                string loanToPay = collection.Contains("LoantoPay") ? ((double)collection["LoantoPay"].AsDecimal128).ToString("F2") : "0.00";
                string amortization = collection.Contains("Amortization") ? ((double)collection["Amortization"].AsDecimal128).ToString("F2") : "0.00";
                string runningBalance = collection.Contains("RunningBalance") ? ((double)collection["RunningBalance"].AsDecimal128).ToString("F2") : "0.00";

                // Concatenate Loan Information fields into a single string
                string loanInfo = $"Loan Amount: {loanAmount}\n" +
                                  $"Loan to Pay: {loanToPay}\n" +
                                  $"Amortization: {amortization}\n" +
                                  $"Running Balance: {runningBalance}";

                // Payment Information
                string dateReceived = collection.Contains("DateReceived") ? collection["DateReceived"].AsString : "";
                string amountPaid = collection.Contains("AmountPaid") ? ((double)collection["AmountPaid"].AsDecimal128).ToString("F2") : "0.00";
                string penalty = collection.Contains("CollectedPenalty") ? ((double)collection["CollectedPenalty"].AsDecimal128).ToString("F2") : "0.00";
                string paymentMode = collection.Contains("PaymentMode") ? collection["PaymentMode"].AsString : "";
                
                // Concatenate Payment Information fields into a single string
                string paymentInfo = $"Date Received: {dateReceived}\n" +
                                     $"Amount Paid: {amountPaid}\n" +
                                     $"Penalty: {penalty}\n" +
                                     $"Payment Mode: {paymentMode}";
                                    
                // Collection Information
                string collector = collection.Contains("Collector") ? collection["Collector"].AsString : "";
                string area = collection.Contains("Area") ? collection["Area"].AsString : "";

                // Determine Collection Status
                string collectionStatus;
                DateTime collDate = collection["CollectionDate"].ToUniversalTime();
                DateTime receivedDate = DateTime.Parse(dateReceived);

                if (paymentsMode == "DAILY")
                {
                    // Compare dates (ignore time)
                    if (collDate.Date == receivedDate.Date)
                    {
                        collectionStatus = "Paid on Time";
                    }
                    else
                    {
                        collectionStatus = "Over Due";
                    }
                }
                else if (paymentsMode == "WEEKLY")
                {
                    // Check if the collection date is within the same week as the date received
                    if (collDate.Date >= receivedDate.Date && collDate.Date < receivedDate.Date.AddDays(7))
                    {
                        collectionStatus = "Paid on Time";
                    }
                    else
                    {
                        collectionStatus = "Over Due";
                    }
                }
                else if (paymentsMode == "SEMI-MONTHLY")
                {
                    // Assuming semi-monthly means twice a month (e.g., 1st and 15th)
                    DateTime firstPaymentDue = new DateTime(collDate.Year, collDate.Month, 1);
                    DateTime secondPaymentDue = new DateTime(collDate.Year, collDate.Month, 15);
                    if ((receivedDate.Date == firstPaymentDue.Date) || (receivedDate.Date == secondPaymentDue.Date))
                    {
                        collectionStatus = "Paid on Time";
                    }
                    else
                    {
                        collectionStatus = "Over Due";
                    }
                }
                else if (paymentsMode == "MONTHLY")
                {
                    // Check if the collection date is the same as the received date (monthly)
                    if (collDate.Date == receivedDate.Date)
                    {
                        collectionStatus = "Paid on Time";
                    }
                    else
                    {
                        collectionStatus = "Over Due";
                    }
                }
                else
                {
                    // For any other payment modes, default to "Over Due"
                    collectionStatus = "Over Due";
                }

                // Concatenate Collection Information fields into a single string
                string collectionInfo = $"Collector: {collector}\n" +
                                        $"Area Route: {area}\n" +
                                        $"Collection Status: {collectionStatus}";

                // Remarks Logic
                string paymentStartDateStr = collection.Contains("PaymentStartDate") ? collection["PaymentStartDate"].AsString : "";
                DateTime paymentStartDate = DateTime.Parse(paymentStartDateStr);

                // Calculate remarks
                string remarks;
                if (paymentStartDate.Date == receivedDate.Date)
                {
                    // Calculate total days missed (could be negative)
                    int daysMissed = (collDate.Date - paymentStartDate.Date).Days;
                    remarks = $"Total Days Missed: {daysMissed} on {paymentStartDate:MM/dd/yyyy}";
                }
                else
                {
                    remarks = "Payment Completed";
                }

                // Add the concatenated information to the DataTable
                _loanCollectionTable.Rows.Add(clientInfo, loanInfo, paymentInfo, collectionInfo, remarks);
            }

            // Bind data to DataGridView
            dgvdata.DataSource = _loanCollectionTable;

            // Set columns to fill the whole row
            dgvdata.Columns["Client Information"].Width = 300;
            dgvdata.Columns["Payment Information"].Width = 300;
            dgvdata.Columns["Collection Information"].Width = 200;
            dgvdata.Columns["Remarks"].Width = 200;
            dgvdata.Columns["Loan Information"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


            // Check if the button column is already added to avoid duplicates
            if (!dgvdata.Columns.Contains("ViewDetails"))
            {
                // Add the button column directly to DataGridView, not the DataTable
                DataGridViewButtonColumn viewDetailsButton = new DataGridViewButtonColumn();
                viewDetailsButton.Name = "ViewDetails";
                viewDetailsButton.HeaderText = "View Details";
                viewDetailsButton.Text = "View Details";
                viewDetailsButton.UseColumnTextForButtonValue = true; // Display text on the button
                viewDetailsButton.FlatStyle = FlatStyle.Standard; // Optional: style for button
                //viewDetailsButton.Width = 80; // Set the width of the button column
                dgvdata.Columns.Add(viewDetailsButton);
            }

            // Change the style of the Collection Status in the Collection Information
            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                string collectionInfo = row.Cells[3].Value.ToString(); // Assuming the Collection Information is in the 4th column
                string[] collectionInfoParts = collectionInfo.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                string statusLine = collectionInfoParts.Last(); // Get the last line for status
                string collectorLine = collectionInfoParts[0]; // Get the collector information
                string areaLine = collectionInfoParts[1]; // Get the area information

                // Set the base text for the Collection Information
                row.Cells[3].Value = $"{collectorLine}\n{areaLine}\n{statusLine}";

                // Apply styling based on Collection Status
                if (statusLine.Contains("Paid on Time"))
                {
                    row.Cells[3].Style.ForeColor = Color.Green;
                    row.Cells[3].Style.Font = new Font(dgvdata.Font, FontStyle.Bold);
                }
                else if (statusLine.Contains("Over Due"))
                {
                    row.Cells[3].Style.ForeColor = Color.Red;
                    row.Cells[3].Style.Font = new Font(dgvdata.Font, FontStyle.Bold);
                }
            }


            // Show or hide the 'lnorecord' label depending on the number of rows
            lnorecord.Visible = dgvdata.Rows.Count == 0;
        }

        private void SearchInDataGrid(string keyword)
        {
            DataView dv = _loanCollectionTable.DefaultView;
            string filter = string.Empty;

            // Loop through all columns to search in any column
            foreach (DataColumn col in _loanCollectionTable.Columns)
            {
                if (!string.IsNullOrEmpty(filter))
                {
                    filter += " OR "; // Add OR between column filters
                }

                // Using LIKE to perform partial matching in each column
                filter += $"{col.ColumnName} LIKE '%{keyword}%'";
            }

            // Apply filter to the DataView
            dv.RowFilter = filter;

            // Update DataGridView with filtered results
            dgvdata.DataSource = dv;

            // Check if any rows are displayed after filtering
            if (dgvdata.Rows.Count == 0)
            {
                lnorecord.Visible = true;
            }
            else
            {
                lnorecord.Visible = false;
            }
        }

        private void frm_home_disburse_collections_Load(object sender, EventArgs e)
        {
            laccountid.Text = _loanId;
            LoadLoanCollections();
        }

        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the click is in the button column (last column)
            if (e.ColumnIndex == dgvdata.Columns.Count - 1 && e.RowIndex >= 0)
            {
                // Retrieve the data from the current row to show details
                string details = ""; // Replace with your logic to fetch the details
                                     // You can access other cells in the row to show more info
                string remarks = dgvdata.Rows[e.RowIndex].Cells["Remarks"].Value.ToString();
                // Create your details string or a new form to show details
                details = $"Details for Row {e.RowIndex + 1}:\n{remarks}";

                // For example, show a message box with details
                MessageBox.Show(details, "View Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tsearch_TextChanged(object sender, EventArgs e)
        {
            SearchInDataGrid(tsearch.Text);
        }

        private void dgvdata_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvdata.ClearSelection();
        }

        private void bnew_Click(object sender, EventArgs e)
        {
            frm_home_disburse_collections_add add = new frm_home_disburse_collections_add();
            add.ShowDialog(this);
        }

        private void dgvdata_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.ColumnIndex < dgvdata.Columns.Count &&
                dgvdata.Columns[e.ColumnIndex].Name == "ViewDetails")
            {
                // Check if the cell has a style
                if (dgvdata.Rows[e.RowIndex].Cells[e.ColumnIndex].Style != null)
                {
                    // Retrieve the existing cell style
                    DataGridViewCellStyle cellStyle = dgvdata.Rows[e.RowIndex].Cells[e.ColumnIndex].Style;

                    // Apply padding: 20px left/right, 15px top/bottom
                    cellStyle.Padding = new Padding(40, 40, 40, 40);
                }
            }
        }
    }
}

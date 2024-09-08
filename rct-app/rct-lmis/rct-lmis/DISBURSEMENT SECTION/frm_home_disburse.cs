using MongoDB.Bson;
using MongoDB.Driver;
using rct_lmis.DISBURSEMENT_SECTION;
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

namespace rct_lmis
{
    public partial class frm_home_disburse : Form
    {

        private IMongoCollection<BsonDocument> loanDisbursedCollection;
        private IMongoCollection<BsonDocument> loanApprovedCollection;

        public frm_home_disburse()
        {
            InitializeComponent();

            // Initialize MongoDB connections
            var database = MongoDBConnection.Instance.Database;
            loanDisbursedCollection = database.GetCollection<BsonDocument>("loan_disbursed");
            loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");
        }
        LoadingFunction load = new LoadingFunction();

        // Function to calculate Maturity Date excluding weekends
        private DateTime CalculateMaturityDate(DateTime startDate, int days)
        {
            DateTime currentDate = startDate;
            int addedDays = 0;

            while (addedDays < days)
            {
                currentDate = currentDate.AddDays(1);
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    addedDays++;
                }
            }

            return currentDate;
        }


        private async Task LoadLoanDisbursedData(string searchQuery = "")
        {
            try
            {
                // Create a filter for search, or use an empty filter if no search query is provided
                var filter = Builders<BsonDocument>.Filter.Empty;

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    // Search by LoanIDNo or cashName (client name)
                    filter = Builders<BsonDocument>.Filter.Or(
                        Builders<BsonDocument>.Filter.Regex("LoanIDNo", new BsonRegularExpression(searchQuery, "i")),
                        Builders<BsonDocument>.Filter.Regex("cashName", new BsonRegularExpression(searchQuery, "i"))
                    );
                }

                // Retrieve data from MongoDB based on the filter
                var loanDisbursedList = await loanDisbursedCollection.Find(filter).ToListAsync();

                // Create DataTable to populate DataGridView
                DataTable loanDisbursedTable = new DataTable();
                loanDisbursedTable.Columns.Add("Loan ID No");
                loanDisbursedTable.Columns.Add("Disbursement Reference No.");
                loanDisbursedTable.Columns.Add("Client Info");
                loanDisbursedTable.Columns.Add("Loan Amount");
                loanDisbursedTable.Columns.Add("Payment Start Date");
                loanDisbursedTable.Columns.Add("Date Disbursed");

                // Check if no data is found
                if (loanDisbursedList.Count == 0)
                {
                    // No records found, show the lnorecord label
                    lnorecord.Text = "No records found!";
                    lnorecord.Visible = true;

                    // Clear the DataGridView if no results
                    dgvdata.DataSource = null;
                    dgvdata.Rows.Clear();
                    return;
                }
                else
                {
                    // Hide the lnorecord label if data is found
                    lnorecord.Visible = false;
                }

                // Fill the DataTable with data from MongoDB
                foreach (var loan in loanDisbursedList)
                {
                    DataRow row = loanDisbursedTable.NewRow();
                    row["Loan ID No"] = loan.GetValue("LoanIDNo").ToString();
                    row["Disbursement Reference No."] = loan.GetValue("cashNo").ToString();

                    // Get additional client info from the loan_approved collection
                    var clientInfo = await GetClientInfo(loan.GetValue("cashClnNo").ToString());

                    // Display Client Name, Address, CP, and Loan Status in the "Client Info" column
                    row["Client Info"] = $"{clientInfo["ClientName"]} \n" +
                                         $"{clientInfo["Address"]} \n" +
                                         $"CP: {clientInfo["ContactNumber"]} \n" +
                                         $"Status: {clientInfo["LoanStatus"]}";

                    // Get the Mode of Payment
                    string modeOfPayment = loan.GetValue("Mode").ToString();

                    // Format Loan Amount with Philippine Peso
                    decimal loanAmount = Convert.ToDecimal(loan.GetValue("loanAmt").ToString());
                    decimal amortizedAmt = Convert.ToDecimal(loan.GetValue("amortizedAmt").ToString());
                    row["Loan Amount"] = $"Loan Amount: ₱ {loanAmount:N2} \n" +
                                         $"Mode of Payment: {modeOfPayment} \n" +
                                         $"Amortization: ₱ {amortizedAmt:N2}";

                    // Get the Payment Start Date and convert it to DateTime
                    DateTime paymentStartDate = DateTime.Parse(loan.GetValue("PaymentStartDate").ToString());

                    // Get the 'days' value for this loan and calculate Maturity Date
                    int days = Convert.ToInt32(loan.GetValue("days").ToString());

                    // Calculate Maturity Date based on the 'days' value, excluding weekends
                    DateTime maturityDate = CalculateMaturityDate(paymentStartDate, days);

                    // Format Payment Start Date and Maturity Date for display
                    row["Payment Start Date"] = $"Start Date: {paymentStartDate.ToString("MM/dd/yyyy")} \n" +
                                                 $"Maturity Date: {maturityDate.ToString("MM/dd/yyyy")}";

                    // Get DisbursementTime and Encoder
                    DateTime disbursementTime = DateTime.Parse(loan.GetValue("DisbursementTime").ToString());
                    string encoder = loan.GetValue("Encoder").ToString();

                    // Format Date Disbursed
                    row["Date Disbursed"] = $"Date Encoded: {disbursementTime.ToString("MM/dd/yyyy hh:mm tt")} \n" +
                                            $"Encoded by: {encoder}";

                    // Add the row to the data table
                    loanDisbursedTable.Rows.Add(row);
                }

                // Bind the DataTable to the DataGridView
                dgvdata.DataSource = loanDisbursedTable;

                // Add the "View Details" button to the DataGridView if not already added
                if (dgvdata.Columns["ViewDetails"] == null)
                {
                    AddViewDetailsButton();
                    var btnColumndetails = dgvdata.Columns["Client Info"];
                    if (btnColumndetails != null)
                    {
                        btnColumndetails.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        btnColumndetails.Width = 200;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading loan disbursement data: " + ex.Message);
            }
        }

        private void AddViewDetailsButton()
        {
            // Add the View Details button
            DataGridViewButtonColumn btnDetailsColumn = new DataGridViewButtonColumn
            {
                Name = "ViewDetails",
                Text = "View Details",
                UseColumnTextForButtonValue = true,
                HeaderText = "Actions",
                Width = 200 // Adjust width as needed          
            };
            dgvdata.Columns.Add(btnDetailsColumn);

            var btnColumndetails = dgvdata.Columns["ViewDetails"];
            if (btnColumndetails != null)
            {
                btnColumndetails.Width = 120;
                btnColumndetails.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                btnColumndetails.DefaultCellStyle.Font = new Font("Segoe UI", 8);
            }

            // Add the View Collections button
            DataGridViewButtonColumn btnCollectionsColumn = new DataGridViewButtonColumn
            {
                Name = "ViewCollections",
                Text = "View Collections",
                UseColumnTextForButtonValue = true,
                HeaderText = " ",
                Width = 120 // Adjust width as needed          
            };
            dgvdata.Columns.Add(btnCollectionsColumn);

            var btnColumnCollections = dgvdata.Columns["ViewCollections"];
            if (btnColumnCollections != null)
            {
                btnColumnCollections.Width = 120;
                btnColumnCollections.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                btnColumnCollections.DefaultCellStyle.Font = new Font("Segoe UI", 8);
            }
        }


        // Method to get client info from the loan_approved collection
        private async Task<Dictionary<string, string>> GetClientInfo(string clientNumber)
        {
            var clientInfo = new Dictionary<string, string>();
            try
            {
                // Create a filter for ClientNumber
                var filter = Builders<BsonDocument>.Filter.Eq("ClientNumber", clientNumber);
                var loanApprovedDoc = await loanApprovedCollection.Find(filter).FirstOrDefaultAsync();

                if (loanApprovedDoc != null)
                {
                    // Format the client name
                    string clientName = $"{loanApprovedDoc.GetValue("FirstName")} {loanApprovedDoc.GetValue("MiddleName")} {loanApprovedDoc.GetValue("LastName")} {loanApprovedDoc.GetValue("SuffixName")}".Trim();

                    // Format the address
                    string address = $"{loanApprovedDoc.GetValue("Street")}, {loanApprovedDoc.GetValue("Barangay")}, {loanApprovedDoc.GetValue("City")}, {loanApprovedDoc.GetValue("Province")}".Trim();

                    string contactNumber = loanApprovedDoc.GetValue("CP").ToString();
                    string loanStatus = loanApprovedDoc.GetValue("LoanStatus").ToString();

                    clientInfo.Add("ClientName", clientName);
                    clientInfo.Add("Address", address);
                    clientInfo.Add("ContactNumber", contactNumber);
                    clientInfo.Add("LoanStatus", loanStatus);
                }
                else
                {
                    clientInfo.Add("ClientName", "N/A");
                    clientInfo.Add("Address", "N/A");
                    clientInfo.Add("ContactNumber", "N/A");
                    clientInfo.Add("LoanStatus", "N/A");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving client info: " + ex.Message);
            }
            return clientInfo;
        }


        private async Task PopulateComboBoxWithCashNames()
        {
            try
            {
                // Create a filter to retrieve all documents
                var filter = Builders<BsonDocument>.Filter.Empty;

                // Retrieve data from MongoDB
                var loanDisbursedList = await loanDisbursedCollection.Find(filter).ToListAsync();

                // Add the default item first
                cbstatus.Items.Clear();
                cbstatus.Items.Add("--all payee--");

                // Populate the ComboBox with cashName values
                foreach (var loan in loanDisbursedList)
                {
                    var cashName = loan.GetValue("cashName").ToString();
                    if (!cbstatus.Items.Contains(cashName))
                    {
                        cbstatus.Items.Add(cashName);
                    }
                }

                // Set default selection
                cbstatus.SelectedIndex = 0; // Select the default item
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error populating ComboBox: " + ex.Message);
            }
        }

        private async void frm_home_disburse_Load(object sender, EventArgs e)
        {
            await LoadLoanDisbursedData();
            await PopulateComboBoxWithCashNames();
        }


        private void dgvdata_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvdata.ClearSelection();

        }

        private void beditentries_Click(object sender, EventArgs e)
        {
            frm_home_disburse_editCV ncv = new frm_home_disburse_editCV();

            load.Show(this);
            Thread.Sleep(300);
            ncv.Show();
            load.Close();

        }

        private void tsearch_TextChanged(object sender, EventArgs e)
        {
            // Call LoadLoanDisbursedData with the search query from tsearch.Text
            _ = LoadLoanDisbursedData(tsearch.Text);
        }

        private void dgvdata_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.ColumnIndex < dgvdata.Columns.Count &&
                dgvdata.Columns[e.ColumnIndex].Name == "ViewDetails")
            {
                if (dgvdata.Rows[e.RowIndex].Cells[e.ColumnIndex].Style != null)
                {
                    DataGridViewCellStyle cellStyle = dgvdata.Rows[e.RowIndex].Cells[e.ColumnIndex].Style;
                    cellStyle.Padding = new Padding(20, 15, 20, 15);
                }
            }

            if (e.ColumnIndex >= 0 && e.ColumnIndex < dgvdata.Columns.Count &&
               dgvdata.Columns[e.ColumnIndex].Name == "ViewCollections")
            {
                if (dgvdata.Rows[e.RowIndex].Cells[e.ColumnIndex].Style != null)
                {
                    DataGridViewCellStyle cellStyle = dgvdata.Rows[e.RowIndex].Cells[e.ColumnIndex].Style;
                    cellStyle.Padding = new Padding(20, 15, 20, 15);
                }
            }

            // Check if the column is the Client Info column
            if (e.ColumnIndex == dgvdata.Columns["Client Info"].Index && e.RowIndex >= 0)
            {
                // Get the current value of the cell
                string cellValue = e.Value?.ToString();
                if (string.IsNullOrEmpty(cellValue))
                    return;

                // Set the background color based on the LoanStatus value
                if (cellValue.Contains("Loan Released"))
                {
                    e.CellStyle.BackColor = Color.LightGreen;
                }
                else if (cellValue.Contains("Pending"))
                {
                    e.CellStyle.BackColor = Color.Yellow;
                }
                else if (cellValue.Contains("Denied"))
                {
                    e.CellStyle.BackColor = Color.LightCoral;
                }
                else
                {
                    e.CellStyle.BackColor = Color.White; // Default background color
                }

                // Optional: Set text color for better contrast
                e.CellStyle.ForeColor = Color.Black;
            }
        }

        private void dgvdata_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is in the "ViewDetails" column
            if (e.ColumnIndex == dgvdata.Columns["ViewDetails"].Index && e.RowIndex >= 0)
            {
                // Get the Loan ID No from the clicked row
                string loanId = dgvdata.Rows[e.RowIndex].Cells["Disbursement Reference No."].Value.ToString();

                // Open the detail form
                frm_home_disburse_details detailForm = new frm_home_disburse_details(loanId);
                detailForm.ShowDialog();
            }

            // Check if the clicked cell is in the "ViewDetails" column
            if (e.ColumnIndex == dgvdata.Columns["ViewCollections"].Index && e.RowIndex >= 0)
            {
                // Get the Loan ID No from the clicked row
                string loanId = dgvdata.Rows[e.RowIndex].Cells["Disbursement Reference No."].Value.ToString();

                // Open the detail form
                frm_home_disburse_collections collections = new frm_home_disburse_collections(loanId);
                collections.ShowDialog();
            }
        }
    }
}

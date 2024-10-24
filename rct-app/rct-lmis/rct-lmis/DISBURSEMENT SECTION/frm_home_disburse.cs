using MongoDB.Bson;
using MongoDB.Driver;
using rct_lmis.DISBURSEMENT_SECTION;
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
using System.Windows.Forms;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;


namespace rct_lmis
{
    public partial class frm_home_disburse : Form
    {
        private IMongoCollection<BsonDocument> loanDisbursedCollection;
        private IMongoCollection<BsonDocument> loanApprovedCollection;
        private string loggedInUsername;

        public frm_home_disburse()
        {
            InitializeComponent();
          
            // Initialize MongoDB connections
            var database = MongoDBConnection.Instance.Database;
            loanDisbursedCollection = database.GetCollection<BsonDocument>("loan_disbursed");
            loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");

            loggedInUsername = UserSession.Instance.CurrentUser;

            dtdate.Value = DateTime.Now;
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


        public async Task LoadLoanDisbursedData(string searchQuery = "", string selectedCashName = "", DateTime? selectedDate = null)
        {
            try
            {
                // Create a base filter (default: no filters, retrieves all documents)
                var filter = Builders<BsonDocument>.Filter.Empty;

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    var searchFilter = Builders<BsonDocument>.Filter.Or(
                        Builders<BsonDocument>.Filter.Regex("LoanNo", new BsonRegularExpression(searchQuery, "i")),
                        Builders<BsonDocument>.Filter.Regex("AccountId", new BsonRegularExpression(searchQuery, "i")),
                        // Add more filters here based on the fields
                        Builders<BsonDocument>.Filter.Regex("LoanTerm", new BsonRegularExpression(searchQuery, "i")),
                        Builders<BsonDocument>.Filter.Regex("LoanAmount", new BsonRegularExpression(searchQuery.Replace("₱", "").Replace(",", ""), "i")),
                        Builders<BsonDocument>.Filter.Regex("LoanAmortization", new BsonRegularExpression(searchQuery.Replace("₱", "").Replace(",", ""), "i"))
                    );

                    filter = Builders<BsonDocument>.Filter.And(filter, searchFilter);
                }

                // Apply filters from ComboBox selection and DateTimePicker if applicable
                if (!string.IsNullOrEmpty(selectedCashName) && selectedCashName != "--all payee--")
                {
                    var cashNameFilter = Builders<BsonDocument>.Filter.Eq("CollectorName", selectedCashName);
                    filter = Builders<BsonDocument>.Filter.And(filter, cashNameFilter);
                }

                if (selectedDate.HasValue)
                {
                    var startDate = selectedDate.Value.Date;
                    var endDate = startDate.AddDays(1).AddTicks(-1); // End of the day
                    var dateFilter = Builders<BsonDocument>.Filter.And(
                        Builders<BsonDocument>.Filter.Gte("Date_Encoded", startDate),
                        Builders<BsonDocument>.Filter.Lte("Date_Encoded", endDate)
                    );
                    filter = Builders<BsonDocument>.Filter.And(filter, dateFilter);
                }

                // Retrieve data from MongoDB based on the combined filter
                var loanDisbursedList = await loanDisbursedCollection.Find(filter).ToListAsync();

                if (loanDisbursedList.Count == 0)
                {
                    lnorecord.Text = "No records found!";
                    lnorecord.Visible = true;

                    // Clear the DataGridView if no results
                    dgvdata.DataSource = null;
                    dgvdata.Rows.Clear();
                    return;
                }
                else
                {
                    lnorecord.Visible = false;
                }

                // Sort the list by StartPaymentDate
                var sortedLoanDisbursedList = loanDisbursedList
                    .Where(loan => loan.Contains("StartPaymentDate"))
                    .OrderByDescending(loan =>
                    {
                        // Safely parse dates with error handling
                        if (DateTime.TryParse(loan["StartPaymentDate"].AsString, out DateTime startDate))
                        {
                            return startDate;
                        }
                        return DateTime.MinValue; // Return a default value if parsing fails
                    })
                    .ToList();

                // Create DataTable to populate DataGridView
                DataTable loanDisbursedTable = new DataTable();
                loanDisbursedTable.Columns.Add("Disbursement No.");
                loanDisbursedTable.Columns.Add("Account ID");
                loanDisbursedTable.Columns.Add("ClientNo"); // Add ClientNo column
                loanDisbursedTable.Columns.Add("Client Info");
                loanDisbursedTable.Columns.Add("Loan Amount");
                loanDisbursedTable.Columns.Add("Loan Status");
                loanDisbursedTable.Columns.Add("Encoded Details");

                foreach (var loan in sortedLoanDisbursedList)
                {
                    DataRow row = loanDisbursedTable.NewRow();

                    // Safely retrieve values from BsonDocument
                    row["Disbursement No."] = loan.Contains("AccountId") ? loan["AccountId"].AsString : "N/A";
                    row["Account ID"] = loan.Contains("LoanNo") ? loan["LoanNo"].AsString : "N/A";
                    row["ClientNo"] = loan.Contains("ClientNo") ? loan["ClientNo"].AsString : "N/A"; // Retrieve ClientNo

                    string clientName = $"{loan.GetValue("LastName", "")}, {loan.GetValue("FirstName", "")} {loan.GetValue("MiddleName", "")}";
                    string address = $"{loan.GetValue("Barangay", "")}, {loan.GetValue("City", "")}, {loan.GetValue("Province", "")}";
                    row["Client Info"] = $"{clientName} \n" +
                                          $"{address}";

                    // Safely convert to decimal for loan amounts and amortization
                    decimal loanAmount;
                    decimal.TryParse(loan.GetValue("LoanAmount", "0").AsString.Replace("₱", "").Replace(",", ""), out loanAmount);

                    decimal loanAmortization;
                    decimal.TryParse(loan.GetValue("LoanAmortization", "0").AsString.Replace("₱", "").Replace(",", ""), out loanAmortization);

                    row["Loan Amount"] = $"Loan Amount: ₱ {loanAmount:N2} \n" +
                                         $"Loan Term: {loan.GetValue("LoanTerm", "N/A")} \n" +
                                         $"Amortization: ₱ {loanAmortization:N2}\n" +
                                         $"Payment Mode: {loan.GetValue("PaymentMode", "N/A")}";

                    // Safely retrieve and format the date fields
                    DateTime startPaymentDate;
                    DateTime.TryParse(loan.GetValue("StartPaymentDate", DateTime.MinValue.ToString()).AsString, out startPaymentDate);

                    DateTime maturityDate;
                    DateTime.TryParse(loan.GetValue("MaturityDate", DateTime.MinValue.ToString()).AsString, out maturityDate);

                    row["Loan Status"] = $"Start Date: {startPaymentDate:MM/dd/yyyy} \n" +
                                         $"Maturity Date: {maturityDate:MM/dd/yyyy}\n" +
                                         $"Loan Status: {loan.GetValue("LoanProcessStatus", "N/A")}\n" +
                                         $"Collector: {loan.GetValue("CollectorName", "N/A")}";

                    row["Encoded Details"] = $"Date Encoded: {loan.GetValue("Date_Encoded", "N/A")}";

                    // Add the row to the data table
                    loanDisbursedTable.Rows.Add(row);
                }

                // Bind DataTable to DataGridView
                dgvdata.DataSource = loanDisbursedTable;

                // Call method to adjust button columns after data is loaded
                AdjustButtonColumns();

                if (dgvdata.Columns["ViewDetails"] == null)
                {
                    AddViewDetailsButton();
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Date format error: {ex.Message}", "Date Parsing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (MongoException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        // Add or Adjust button columns display index
        private void AdjustButtonColumns()
        {
            var btnColumndetails = dgvdata.Columns["ViewDetails"];
            var btnColumnCollections = dgvdata.Columns["ViewCollections"];

            if (btnColumndetails != null)
            {
                btnColumndetails.DisplayIndex = dgvdata.ColumnCount - 2; // Position second to last
                btnColumndetails.Width = 120;
                btnColumndetails.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                btnColumndetails.DefaultCellStyle.Font = new Font("Segoe UI", 8);
            }

            if (btnColumnCollections != null)
            {
                btnColumnCollections.DisplayIndex = dgvdata.ColumnCount - 1; // Position last
                btnColumnCollections.Width = 120;
                btnColumnCollections.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                btnColumnCollections.DefaultCellStyle.Font = new Font("Segoe UI", 8);
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
                Width = 120
            };
            dgvdata.Columns.Add(btnDetailsColumn);

            // Add the View Collections button
            DataGridViewButtonColumn btnCollectionsColumn = new DataGridViewButtonColumn
            {
                Name = "ViewCollections",
                Text = "View Collections",
                UseColumnTextForButtonValue = true,
                HeaderText = " ",
                Width = 120
            };
            dgvdata.Columns.Add(btnCollectionsColumn);

            // Adjust the columns' display order to ensure buttons are on the right
            AdjustButtonColumns();
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
                    var cashName = loan.GetValue("LoanProcessStatus").ToString();
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
                //luser.Text = fullName;
            }
        }

        private async void frm_home_disburse_Load(object sender, EventArgs e)
        {
            await LoadLoanDisbursedData();
            await PopulateComboBoxWithCashNames();

           //LoadUserInfo(loggedInUsername);
        }


        private void dgvdata_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvdata.ClearSelection();

        }

        private void tsearch_TextChanged(object sender, EventArgs e)
        {
            
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
                string loanId = dgvdata.Rows[e.RowIndex].Cells["Disbursement No."].Value.ToString();

                // Get the ClientNo from the clicked row
                string clientNo = dgvdata.Rows[e.RowIndex].Cells["ClientNo"].Value.ToString();

                // Open the detail form with both Loan ID and Client No
                frm_home_disburse_details detailForm = new frm_home_disburse_details(loanId, clientNo);
                detailForm.ShowDialog(this);
            }

            // Check if the clicked cell is in the "ViewCollections" column
            if (e.ColumnIndex == dgvdata.Columns["ViewCollections"].Index && e.RowIndex >= 0)
            {
                // Get the ClientNo from the clicked row
                string clientNo = dgvdata.Rows[e.RowIndex].Cells["ClientNo"].Value.ToString();
                string loanId = dgvdata.Rows[e.RowIndex].Cells["Disbursement No."].Value.ToString();

                // Open the collections form with the Client No
                frm_home_disburse_collections collectionsForm = new frm_home_disburse_collections(clientNo, loanId);
                collectionsForm.ShowDialog(this);
            }
        }


        private async void cbstatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCashName = cbstatus.SelectedItem.ToString();
            await LoadLoanDisbursedData(searchQuery: "", selectedCashName: selectedCashName);
        }

        private void bhelp_Click(object sender, EventArgs e)
        {
            // Display an informational MessageBox to guide the user about the frm_home_disburse form.
            string helpMessage =
                "Form: Loan Disbursement Information\n\n" +
                "This form allows you to view and search through disbursed loans.\n" +
                "You can filter the loans based on the loan ID or client name, " +
                "as well as by the disbursement date using the date picker.\n\n" +
                "Features:\n" +
                "- Search loans by Loan ID or Client Name using the search bar.\n" +
                "- Filter loans by Disbursement Date using the Date Picker.\n" +
                "- View details about each loan, including the Loan Amount, Mode of Payment, " +
                "and Client Info.\n" +
                "- Use the 'View Details' button to see more information about a specific loan.\n\n" +
                "Instructions:\n" +
                "1. Enter a Loan ID or Client Name in the search box to filter the list.\n" +
                "2. Use the Date Picker to filter loans disbursed on a specific date.\n" +
                "3. Click the 'View Details' button in the table to see more loan details.\n\n" +
                "If no records are found, the message 'No records found!' will appear.";

            MessageBox.Show(helpMessage, "Loan Disbursement Form Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bexport_Click(object sender, EventArgs e)
        {
            try
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                // Initialize the SaveFileDialog to prompt the user for the Excel file location
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    Title = "Save as Excel File",
                    FileName = "LoanDisbursements.xlsx" // Default filename
                };

                if (dgvdata.Rows.Count == 0)
                {
                    MessageBox.Show("No records to export.", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Create a new Excel package
                    using (ExcelPackage package = new ExcelPackage())
                    {
                        // Add a new worksheet to the Excel workbook
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("LoanDisbursements");

                        // Set paper size to A4
                        worksheet.PrinterSettings.PaperSize = ePaperSize.A4;

                        // Set font to Arial size 9 for the entire worksheet
                        worksheet.Cells.Style.Font.Name = "Arial";
                        worksheet.Cells.Style.Font.Size = 9;

                        // Insert the image from the Resources folder
                        string imagePath = Path.Combine(System.Windows.Forms.Application.StartupPath, "Resources", "rctheader.jpg");
                        if (File.Exists(imagePath))
                        {
                            var picture = worksheet.Drawings.AddPicture("HeaderImage", imagePath);
                            picture.SetPosition(0, 0, 0, 0);
                            picture.From.Column = 2;
                            picture.From.Row = 0;
                        }
                        else
                        {
                            MessageBox.Show("Image file not found: " + imagePath);
                        }

                        // Add title at the top (in row 7)
                        string title = $"DISBURSEMENT LIST AS OF {DateTime.Now:MMMM dd, yyyy}";
                        worksheet.Cells[8, 1].Value = title; // Set title in row 7
                        worksheet.Cells[8, 1, 8, dgvdata.Columns.Count].Merge = true; // Merge cells for title
                        worksheet.Cells[8, 1].Style.Font.Bold = true;  // Make title bold
                        worksheet.Cells[8, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[8, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        worksheet.Cells[8, 1].Style.Font.Size = 12; // Set title font size

                        // Add column headers starting at row 9
                        int headerRow = 9;
                        int columnIndex = 1;

                        // Define column widths
                        Dictionary<string, double> columnWidths = new Dictionary<string, double>
                 {
                     { "Loan ID No", 20 },
                     { "Disbursement Reference No.", 25 },
                     { "Client Info", 45 },
                     { "Loan Amount", 25 },
                     { "Payment Start Date", 25 },
                     { "Encoded Details", 30 }
                 };

                        for (int i = 0; i < dgvdata.Columns.Count; i++)
                        {
                            // Exclude "View Details" and "View Collections" columns
                            if (dgvdata.Columns[i].HeaderText != "Actions" && dgvdata.Columns[i].HeaderText != " ")
                            {
                                worksheet.Cells[headerRow, columnIndex].Value = dgvdata.Columns[i].HeaderText;
                                worksheet.Cells[headerRow, columnIndex].Style.Font.Bold = true;  // Make header text bold
                                worksheet.Cells[headerRow, columnIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                worksheet.Cells[headerRow, columnIndex].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                // Set the column width based on the defined widths
                                if (columnWidths.TryGetValue(dgvdata.Columns[i].HeaderText, out double width))
                                {
                                    worksheet.Column(columnIndex).Width = width; // Set specific width
                                }
                                columnIndex++; // Increment column index for each relevant column
                            }
                        }

                        // Add data rows starting from row 9
                        for (int i = 0; i < dgvdata.Rows.Count; i++)
                        {
                            columnIndex = 1; // Reset column index for each row
                            for (int j = 0; j < dgvdata.Columns.Count; j++)
                            {
                                // Check if the column is not "View Details" or "View Collections"
                                if (dgvdata.Columns[j].HeaderText != "Actions" && dgvdata.Columns[j].HeaderText != " ")
                                {
                                    worksheet.Cells[i + 10, columnIndex].Value = dgvdata.Rows[i].Cells[j].Value?.ToString();
                                    columnIndex++;
                                }
                            }
                        }

                        // Save the Excel file
                        FileInfo fi = new FileInfo(saveFileDialog.FileName);
                        package.SaveAs(fi);

                        MessageBox.Show("File exported successfully!", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting to Excel: " + ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void dtdate_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = dtdate.Value;
            //await LoadLoanDisbursedData(searchQuery: "", selectedCashName: "", selectedDate: selectedDate);
        }
    }
}

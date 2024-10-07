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

                // Apply search query filter if provided (for LoanIDNo or cashName)
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    var searchFilter = Builders<BsonDocument>.Filter.Or(
                        Builders<BsonDocument>.Filter.Regex("LoanIDNo", new BsonRegularExpression(searchQuery, "i")),
                        Builders<BsonDocument>.Filter.Regex("cashName", new BsonRegularExpression(searchQuery, "i"))
                    );
                    filter = Builders<BsonDocument>.Filter.And(filter, searchFilter);
                }

                // Apply cashName filter from ComboBox selection (excluding '--all payee--')
                if (!string.IsNullOrEmpty(selectedCashName) && selectedCashName != "--all payee--")
                {
                    var cashNameFilter = Builders<BsonDocument>.Filter.Eq("cashName", selectedCashName);
                    filter = Builders<BsonDocument>.Filter.And(filter, cashNameFilter);
                }

                // Apply DisbursementTime filter based on DateTimePicker (if a date is selected)
                if (selectedDate.HasValue)
                {
                    var startDate = selectedDate.Value.Date;
                    var endDate = startDate.AddDays(1).AddTicks(-1); // End of the day

                    var dateFilter = Builders<BsonDocument>.Filter.And(
                        Builders<BsonDocument>.Filter.Gte("DisbursementTime", startDate),
                        Builders<BsonDocument>.Filter.Lte("DisbursementTime", endDate)
                    );
                    filter = Builders<BsonDocument>.Filter.And(filter, dateFilter);
                }

                // Retrieve data from MongoDB based on the combined filter
                var loanDisbursedList = await loanDisbursedCollection.Find(filter).ToListAsync();

                // Debugging: Check if data is retrieved
                if (loanDisbursedList.Count == 0)
                {
                    MessageBox.Show("No data retrieved from MongoDB.");
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

                // Create DataTable to populate DataGridView
                DataTable loanDisbursedTable = new DataTable();
                loanDisbursedTable.Columns.Add("Loan ID No");
                loanDisbursedTable.Columns.Add("Disbursement Reference No.");
                loanDisbursedTable.Columns.Add("Client Info");
                loanDisbursedTable.Columns.Add("Loan Amount");
                loanDisbursedTable.Columns.Add("Payment Start Date");
                loanDisbursedTable.Columns.Add("Encoded Details");

                // Fill the DataTable with data from MongoDB
                foreach (var loan in loanDisbursedList)
                {
                    DataRow row = loanDisbursedTable.NewRow();
                    row["Loan ID No"] = loan.Contains("LoanIDNo") ? loan.GetValue("LoanIDNo").ToString() : "N/A";
                    row["Disbursement Reference No."] = loan.Contains("cashNo") ? loan.GetValue("cashNo").ToString() : "N/A";

                    // Get additional client info from the loan_approved collection
                    var clientInfo = await GetClientInfo(loan.Contains("cashClnNo") ? loan.GetValue("cashClnNo").ToString() : string.Empty);

                    // Ensure clientInfo is not null and has the required keys
                    if (clientInfo == null)
                    {
                        MessageBox.Show("Client information could not be found.");
                        continue; // Skip this loan if no client info
                    }

                    // Prepare client info safely
                    string clientName = clientInfo.ContainsKey("ClientName") ? clientInfo["ClientName"] : "N/A";
                    string address = clientInfo.ContainsKey("Address") ? clientInfo["Address"] : "N/A";
                    string contactNumber = clientInfo.ContainsKey("ContactNumber") ? clientInfo["ContactNumber"] : "N/A";
                    string loanStatus = clientInfo.ContainsKey("LoanStatus") ? clientInfo["LoanStatus"] : "N/A";

                    // Get DisbursementTime and Encoder safely
                    DateTime disbursementTime = loan.Contains("DisbursementTime") ? DateTime.Parse(loan.GetValue("DisbursementTime").ToString()) : DateTime.MinValue;
                    string encoder = loan.Contains("Encoder") ? loan.GetValue("Encoder").ToString() : "N/A";

                    // Display Client Name, Address, CP, and Loan Status in the "Client Info" column
                    row["Client Info"] = $"{clientName} \n" +
                                         $"{address} \n" +
                                         $"CP: {contactNumber} \n" +
                                         $"Status: {loanStatus} \n" +
                                         $"Loan Date Released: {disbursementTime:MM/dd/yyyy hh:mm tt}";

                    // Get the Mode of Payment safely
                    string modeOfPayment = loan.Contains("Mode") ? loan.GetValue("Mode").ToString() : "N/A";

                    // Format Loan Amount with Philippine Peso safely
                    decimal loanAmount = loan.Contains("loanAmt") ? Convert.ToDecimal(loan.GetValue("loanAmt").ToString()) : 0;
                    decimal amortizedAmt = loan.Contains("amortizedAmt") ? Convert.ToDecimal(loan.GetValue("amortizedAmt").ToString()) : 0;
                    string loanTerm = loan.Contains("loanTerm") ? loan.GetValue("loanTerm").ToString() : "N/A"; // If it's in loan document

                    row["Loan Amount"] = $"Loan Amount: ₱ {loanAmount:N2} \n" +
                                         $"Mode of Payment: {modeOfPayment} \n" +
                                         $"Loan Term: {loanTerm} months \n" +
                                         $"Amortization: ₱ {amortizedAmt:N2}";

                    // Get the Payment Start Date and convert it to DateTime safely
                    DateTime paymentStartDate = loan.Contains("PaymentStartDate") ? DateTime.Parse(loan.GetValue("PaymentStartDate").ToString()) : DateTime.MinValue;

                    // Get the 'days' value for this loan and calculate Maturity Date safely
                    int days = loan.Contains("days") ? Convert.ToInt32(loan.GetValue("days").ToString()) : 0;

                    // Calculate Maturity Date based on the 'days' value, excluding weekends
                    DateTime maturityDate = CalculateMaturityDate(paymentStartDate, days);

                    // Format Payment Start Date and Maturity Date for display
                    row["Payment Start Date"] = $"Start Date: {paymentStartDate:MM/dd/yyyy} \n" +
                                                 $"Maturity Date: {maturityDate:MM/dd/yyyy}";

                    // Format Date Encoded safely
                    row["Encoded Details"] = $"Date Encoded: {disbursementTime:MM/dd/yyyy hh:mm tt} \n" +
                                             $"Encoded by: {encoder}";

                    // Add the row to the data table
                    loanDisbursedTable.Rows.Add(row);
                }

                // Bind the DataTable to the DataGridView
                dgvdata.DataSource = loanDisbursedTable;

                // Debugging: Check if DataGridView has rows
                if (dgvdata.Rows.Count == 0)
                {
                    MessageBox.Show("DataTable has no rows.");
                }

                // Add the "View Details" button to the DataGridView if not already added
                if (dgvdata.Columns["ViewDetails"] == null)
                {
                    AddViewDetailsButton();
                    var btnColumndetails = dgvdata.Columns["Client Info"];
                    if (btnColumndetails != null)
                    {
                        btnColumndetails.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        btnColumndetails.Width = 200; // Adjust width as needed
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
                string loanId = dgvdata.Rows[e.RowIndex].Cells["Loan ID No"].Value.ToString();

                // Open the detail form
                frm_home_disburse_collections collections = new frm_home_disburse_collections(loanId);
                collections.ShowDialog();
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

                        // Add column headers starting at row 8 (since the image and title occupy rows 0 to 7)
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
                                    var cellValue = dgvdata.Rows[i].Cells[j].Value?.ToString() ?? string.Empty;

                                    // Set cell value
                                    worksheet.Cells[i + 10, columnIndex].Value = cellValue;

                                    // Enable word wrap for relevant columns
                                    worksheet.Cells[i + 10, columnIndex].Style.WrapText = true; // Enable word wrap

                                    columnIndex++; // Increment cell index only for relevant columns
                                }
                            }
                        }

                        // Auto-fit the columns
                        worksheet.Cells[worksheet.Dimension.Address].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        // Add footer
                        int footerStartRow = dgvdata.Rows.Count + 12; // Set footer start row
                        worksheet.Cells[footerStartRow, 1].Value = "Prepared by: \n";
                        worksheet.Cells[footerStartRow + 1, 1].Value = "______________________________________ \n";
                        worksheet.Cells[footerStartRow + 2, 1].Value = UserSession.Instance.UserName + "\n\n"; // Current user FullName logged in

                        worksheet.Cells[footerStartRow + 4, 1].Value = "Approved by:\n";
                        worksheet.Cells[footerStartRow + 5, 1].Value = "______________________________________\n";
                        worksheet.Cells[footerStartRow + 6, 1].Value = "Managing Head"; // Placeholder for the managing head

                        // Save the file to the selected location
                        FileInfo fi = new FileInfo(saveFileDialog.FileName);
                        package.SaveAs(fi);

                        // Notify the user of successful export
                        MessageBox.Show("Data exported successfully to Excel!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting data to Excel: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private async void dtdate_ValueChangedAsync(object sender, EventArgs e)
        {
            DateTime selectedDate = dtdate.Value;
            //await LoadLoanDisbursedData(searchQuery: "", selectedCashName: "", selectedDate: selectedDate);
        }
    }
}

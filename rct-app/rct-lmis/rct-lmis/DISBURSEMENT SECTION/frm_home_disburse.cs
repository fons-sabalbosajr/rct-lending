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
using System.Windows.Controls;
using System.Text.RegularExpressions;
using MailKit.Search;


namespace rct_lmis
{
    public partial class frm_home_disburse : Form
    {
        private IMongoCollection<BsonDocument> loanDisbursedCollection;
        private IMongoCollection<BsonDocument> loanApprovedCollection;
        private IMongoCollection<BsonDocument> loanCollectionsCollection;
        private string loggedInUsername;

        public frm_home_disburse()
        {
            InitializeComponent();
          
            // Initialize MongoDB connections
            var database = MongoDBConnection.Instance.Database;
            loanDisbursedCollection = database.GetCollection<BsonDocument>("loan_disbursed");
            loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");
            loanCollectionsCollection = database.GetCollection<BsonDocument>("loan_collections");
            loggedInUsername = UserSession.Instance.CurrentUser;

            dtdate.Value = DateTime.Now;
        }
        LoadingFunction load = new LoadingFunction();

        private int ExtractNumericPart(string accountId)
        {
            // Example: Extracts "100" from "RCT-2024-DB-100"
            Match match = Regex.Match(accountId, @"\d+$");
            return match.Success ? int.Parse(match.Value) : 0; // Convert to int for proper sorting
        }


        public async Task LoadLoanDisbursedData(string searchQuery = "", string selectedLoanStatus = "--all status--", string selectedYear = "")
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Empty;

                if (!string.IsNullOrEmpty(selectedLoanStatus) && selectedLoanStatus != "--all status--")
                {
                    var normalizedStatus = selectedLoanStatus.Trim().ToLower();
                    var statusFilter = Builders<BsonDocument>.Filter.Regex("LoanStatus", new BsonRegularExpression($"^{normalizedStatus}$", "i"));
                    filter = Builders<BsonDocument>.Filter.And(filter, statusFilter);
                }

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    var searchTerms = searchQuery.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var searchFilter = Builders<BsonDocument>.Filter.Or(
                        Builders<BsonDocument>.Filter.Regex("LoanNo", new BsonRegularExpression(searchQuery, "i")),
                        Builders<BsonDocument>.Filter.Regex("AccountId", new BsonRegularExpression(searchQuery, "i")),
                        Builders<BsonDocument>.Filter.Regex("LoanTerm", new BsonRegularExpression(searchQuery, "i")),
                        Builders<BsonDocument>.Filter.Regex("LoanAmount", new BsonRegularExpression(searchQuery.Replace("₱", "").Replace(",", ""), "i")),
                        Builders<BsonDocument>.Filter.Regex("LoanAmortization", new BsonRegularExpression(searchQuery.Replace("₱", "").Replace(",", ""), "i")),
                        Builders<BsonDocument>.Filter.And(searchTerms.Select(term => Builders<BsonDocument>.Filter.Regex("LastName", new BsonRegularExpression(term, "i"))).ToArray()),
                        Builders<BsonDocument>.Filter.And(searchTerms.Select(term => Builders<BsonDocument>.Filter.Regex("FirstName", new BsonRegularExpression(term, "i"))).ToArray()),
                        Builders<BsonDocument>.Filter.And(searchTerms.Select(term => Builders<BsonDocument>.Filter.Regex("MiddleName", new BsonRegularExpression(term, "i"))).ToArray())
                    );
                    filter = Builders<BsonDocument>.Filter.And(filter, searchFilter);
                }

                // Filter by Year (selectedYear is passed)
                if (!string.IsNullOrEmpty(selectedYear))
                {
                    var yearRegex = new BsonRegularExpression($"^RCT-{selectedYear}DB", "i");
                    var yearFilter = Builders<BsonDocument>.Filter.Regex("AccountId", yearRegex);
                    filter = Builders<BsonDocument>.Filter.And(filter, yearFilter);
                }

                var loanDisbursedList = await loanDisbursedCollection.Find(filter).ToListAsync();

                if (loanDisbursedList.Count == 0)
                {
                    lnorecord.Text = "No records found!";
                    lnorecord.Visible = true;
                    dgvdata.DataSource = null;
                    dgvdata.Rows.Clear();
                    return;
                }

                lnorecord.Visible = false;
               
                // Get ClientNo from loan_collections collection
                var collectionClientNos = (await loanCollectionsCollection
                    .Find(Builders<BsonDocument>.Filter.Empty)
                    .Project(Builders<BsonDocument>.Projection.Include("ClientNo"))
                    .ToListAsync())
                    .Select(doc => doc.GetValue("ClientNo", "").ToString())
                    .ToHashSet(); // HashSet for fast lookups

                // Convert to a list with extracted numeric values
                var sortedList = loanDisbursedList
                    .Select(loan => new
                    {
                        Loan = loan,
                        NumericAccountId = ExtractNumericPart(loan.GetValue("AccountId", "0").ToString()), // Extract numeric part for sorting
                        IsPriority = collectionClientNos.Contains(loan.GetValue("ClientNo", "").ToString()) // Check priority
                    })
                    .OrderByDescending(x => x.IsPriority) // Prioritize loans in loan_collections
                    .ThenBy(x => x.NumericAccountId) // Then sort numerically
                    .Select(x => x.Loan)
                    .ToList();

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Disbursement No.");
                dataTable.Columns.Add("Account ID");
                dataTable.Columns.Add("ClientNo");
                dataTable.Columns.Add("Client Info");
                dataTable.Columns.Add("Loan Amount");
                dataTable.Columns.Add("Loan Status");
                dataTable.Columns.Add("Encoded Details");

                foreach (var loan in sortedList)
                {
                    DataRow row = dataTable.NewRow();
                    row["Disbursement No."] = loan.GetValue("AccountId", "N/A").ToString();
                    row["Account ID"] = loan.GetValue("LoanNo", "N/A").ToString();
                    row["ClientNo"] = loan.GetValue("ClientNo", "N/A").ToString();

                    string clientName = $"{loan.GetValue("LastName", "")}, {loan.GetValue("FirstName", "")} {loan.GetValue("MiddleName", "")}";
                    string address = $"{loan.GetValue("Barangay", "")}, {loan.GetValue("City", "")}, {loan.GetValue("Province", "")}";
                    row["Client Info"] = $"{clientName}\n{address}";

                    decimal.TryParse(loan.GetValue("LoanAmount", "0").AsString.Replace("₱", "").Replace(",", ""), out decimal loanAmount);
                    decimal.TryParse(loan.GetValue("LoanAmortization", "0").AsString.Replace("₱", "").Replace(",", ""), out decimal loanAmortization);

                    row["Loan Amount"] = $"Loan Amount: ₱{loanAmount:N2}\n" +
                                        $"Loan Term: {loan.GetValue("LoanTerm", "N/A")}\n" +
                                        $"Amortization: ₱{loanAmortization:N2}\n" +
                                        $"Payment Mode: {loan.GetValue("PaymentMode", "N/A")}";

                    row["Loan Status"] = loan.GetValue("LoanStatus", "N/A").ToString();
                    row["Encoded Details"] = $"Date Encoded: {loan.GetValue("Date_Encoded", "N/A")}";

                    dataTable.Rows.Add(row);
                }

                dgvdata.DataSource = dataTable;

                dgvdata.Columns["Disbursement No."].Width = 150;
                dgvdata.Columns["Account ID"].Width = 150;
                dgvdata.Columns["ClientNo"].Width = 150;
                dgvdata.Columns["Client Info"].Width = 250;
                dgvdata.Columns["Loan Amount"].Width = 300;
                dgvdata.Columns["Loan Status"].Width = 150;
                dgvdata.Columns["Encoded Details"].Width = 200;

                dgvdata.DefaultCellStyle.Font = new Font("Segoe UI", 9);
                dgvdata.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                dgvdata.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                lnorecord.Visible = dgvdata.Rows.Count == 0;

                if (dgvdata.Columns["ViewDetails"] == null)
                {
                    AddViewDetailsButton(); // Ensure buttons are added
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                Width = 100
            };
            dgvdata.Columns.Insert(0, btnDetailsColumn); // Insert at first column (leftmost)

            // Add the View Collections button
            DataGridViewButtonColumn btnCollectionsColumn = new DataGridViewButtonColumn
            {
                Name = "ViewCollections",
                Text = "View Collections",
                UseColumnTextForButtonValue = true,
                HeaderText = " ",
                Width = 100
            };
            dgvdata.Columns.Insert(5, btnCollectionsColumn); // Insert as second column

            AdjustButtonColumns();
        }

        private void AdjustButtonColumns()
        {
            var btnColumndetails = dgvdata.Columns["ViewDetails"];
            var btnColumnCollections = dgvdata.Columns["ViewCollections"];

            if (btnColumndetails != null)
            {
                btnColumndetails.DisplayIndex = 0; // Leftmost position
                btnColumndetails.Width = 100;
                btnColumndetails.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                btnColumndetails.DefaultCellStyle.Font = new Font("Segoe UI", 8);
            }

            if (btnColumnCollections != null)
            {
                btnColumnCollections.DisplayIndex = 1; // Second column
                btnColumnCollections.Width = 100;
                btnColumnCollections.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                btnColumnCollections.DefaultCellStyle.Font = new Font("Segoe UI", 8);
            }
        }

        private async Task LoadLoanStatusFilter()
        {
            try
            {
                cbstatus.SelectedIndexChanged -= cbstatus_SelectedIndexChanged;

                var distinctStatuses = await loanDisbursedCollection
                    .Distinct<string>("LoanStatus", Builders<BsonDocument>.Filter.Empty)
                    .ToListAsync();

                cbstatus.Items.Clear();
                cbstatus.Items.Add("--all status--");

                foreach (var status in distinctStatuses.Distinct())
                {
                    if (!cbstatus.Items.Contains(status))
                        cbstatus.Items.Add(status);
                }

                cbstatus.SelectedItem = "--all status--";

                cbstatus.SelectedIndexChanged += cbstatus_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Loan Status filter: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                cbstatus.Items.Add("--all status--");

                // Populate the ComboBox with cashName values
                foreach (var loan in loanDisbursedList)
                {
                    var cashName = loan.GetValue("LoanStatus").ToString();
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

        private void PopulateYearsFromAccountIds()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var approvedLoansCollection = database.GetCollection<BsonDocument>("loan_disbursed");

                var approvedDocs = approvedLoansCollection.Find(new BsonDocument()).ToList();
                HashSet<string> validYears = new HashSet<string>();

                foreach (var doc in approvedDocs)
                {
                    if (doc.Contains("AccountId"))
                    {
                        string accountId = doc["AccountId"].ToString();
                        var match = Regex.Match(accountId, @"RCT-(\d{4})DB", RegexOptions.IgnoreCase);
                        if (match.Success)
                        {
                            var year = match.Groups[1].Value;
                            if (year == "2024" || year == "2025")
                                validYears.Add(year);
                        }
                    }
                }

                var sortedYears = validYears
                    .Select(y => int.Parse(y))
                    .OrderByDescending(y => y)
                    .ToList();

                cbyear.Items.Clear();
                foreach (var year in sortedYears)
                    cbyear.Items.Add(year.ToString());

                // Default to 2025 if available, else first available
                if (cbyear.Items.Contains("2025"))
                    cbyear.SelectedItem = "2025";
                else if (cbyear.Items.Count > 0)
                    cbyear.SelectedItem = cbyear.Items[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading year list: " + ex.Message);
            }
        }


        private async void frm_home_disburse_Load(object sender, EventArgs e)
        {
            // Step 1: Populate years (with only 2024 & 2025)
            PopulateYearsFromAccountIds();

            // Step 2: Select default year = 2025
            if (cbyear.Items.Contains("2025"))
                cbyear.SelectedItem = "2025";
            else if (cbyear.Items.Count > 0)
                cbyear.SelectedIndex = 0;

            string selectedYear = cbyear.SelectedItem?.ToString();

            // Step 3: Load data for selected year (2025 expected)
            if (!string.IsNullOrEmpty(selectedYear))
            {
                await LoadLoanDisbursedData("", "--all status--", selectedYear);
            }

            // Step 4: Load loan statuses AFTER year-based data
            await LoadLoanStatusFilter();
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

            if (dgvdata.Columns[e.ColumnIndex].Name == "Loan Status")
            {
                string loanStatus = e.Value?.ToString(); // Get the value of the cell

                // Check for the LoanStatus value and apply the background color
                if (loanStatus != null)
                {
                    switch (loanStatus)
                    {
                        case "UPDATED":
                            e.CellStyle.BackColor = Color.Green;
                            break;
                        case "PAST DUE":
                            e.CellStyle.BackColor = Color.Yellow;
                            break;
                        case "ARREARS":
                            e.CellStyle.BackColor = Color.Orange;
                            break;
                        case "LITIGATION":
                            e.CellStyle.BackColor = Color.Red;
                            break;
                        case "DORMANT":
                            e.CellStyle.BackColor = Color.Gray;
                            break;
                        default:
                            e.CellStyle.BackColor = Color.White; // Default color
                            break;
                    }
                }
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


        private void cbstatus_SelectedIndexChanged(object sender, EventArgs e)
        {

            // Get the selected loan status from the combo box
            string selectedLoanStatus = cbstatus.SelectedItem.ToString();

            // Reload the data with the selected status
            _ = LoadLoanDisbursedData(selectedLoanStatus: selectedLoanStatus);
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
          
        }

        private async void cbyear_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedYear = cbyear.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedYear))
            {
                MessageBox.Show("Please select a valid year.", "Year Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Optionally reset search and status filter when year changes
            string searchQuery = tsearch?.Text?.Trim() ?? ""; // assuming you have a txtSearch textbox
            string selectedLoanStatus = cbstatus?.SelectedItem?.ToString() ?? "--all status--"; // assuming you have a cbstatus combobox

            await LoadLoanDisbursedData(searchQuery, selectedLoanStatus, selectedYear);
        }

    }
}

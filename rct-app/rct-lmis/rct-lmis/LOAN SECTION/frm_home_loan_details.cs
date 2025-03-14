using ClosedXML.Excel;
using MongoDB.Bson;
using MongoDB.Driver;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using rct_lmis.DISBURSEMENT_SECTION;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rct_lmis.LOAN_SECTION
{
    public partial class frm_home_loan_new : Form
    {
        public string AccountID { get; set; }

        private DataTable _loanCollectionTable;

        public frm_home_loan_new()
        {
            InitializeComponent();
            InitializeDataGridView();

            // Initialize DataTable for binding to DataGridView
            _loanCollectionTable = new DataTable();
        }

        private async void frm_home_loan_new_Load(object sender, EventArgs e)
        {
            laccno.Text = $"{AccountID}";
            await LoadLoanDetailsAsync();
            await LoadLoanCollectionsAsync();
        }

        private void InitializeDataGridView()
        {
            dgvuploads.Columns.Clear();

            // Add Document Name column and set its width
            var documentNameColumn = new DataGridViewTextBoxColumn
            {
                Name = "DocumentName",
                HeaderText = "Document Name",
                Width = 400 // Set the desired width here
            };
            dgvuploads.Columns.Add(documentNameColumn);


            var viewFileButtonColumn = new DataGridViewButtonColumn
            {
                Name = "ViewFile",
                HeaderText = "View File",
                Text = "View File",
                UseColumnTextForButtonValue = true,
                Width = 100 // Adjust width to make the button smaller
            };
            dgvuploads.Columns.Add(viewFileButtonColumn);

            dgvuploads.Columns.Add("DocumentLink", "Document Link");
            dgvuploads.Columns["DocumentLink"].Visible = false;

            // Adjust the DataGridView button's padding
            foreach (DataGridViewColumn column in dgvuploads.Columns)
            {
                column.DefaultCellStyle.Padding = new Padding(2); // Smaller padding
            }

            
           
        }

        private DataTable CreateLoanDataTable()
        {
            DataTable table = new DataTable();

            // Define columns
            table.Columns.Add("Loan ID", typeof(string));
            table.Columns.Add("Loan Details", typeof(string));
            table.Columns.Add("Amortization", typeof(string));
            table.Columns.Add("Repayment", typeof(string));

            return table;
        }

        private void ConfigureDataGridView()
        {
            // Set the wrapping for the "Document Name" column
            dgvuploads.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // Hide the "Document Link" column
            dgvuploads.Columns["DocumentLink"].Visible = false;

            // Optional: Adjust column width to fit the content
            dgvuploads.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }

        private void LoadDocsIntoDataGridView(string[] docsArray, string[] docLinksArray)
        {
            dgvuploads.Rows.Clear();

            // Check if both document names and links arrays are valid
            if (docsArray == null || docLinksArray == null || docsArray.Length == 0 || docLinksArray.Length == 0)
            {
                MessageBox.Show("Document names or links are missing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Load each document name and link into the DataGridView
            for (int i = 0; i < docsArray.Length; i++)
            {
                string docName = docsArray[i];
                string docLink = (i < docLinksArray.Length) ? docLinksArray[i] : string.Empty;

                // Add both the document name and document link to the row
                int rowIndex = dgvuploads.Rows.Add();
                dgvuploads.Rows[rowIndex].Cells["DocumentName"].Value = docName;
                dgvuploads.Rows[rowIndex].Cells["DocumentLink"].Value = docLink; // Store the document link in the hidden column
                dgvuploads.Rows[rowIndex].Cells["ViewFile"].Value = "View File"; // Set the button text
            }

            // Configure the DataGridView after loading data
            ConfigureDataGridView();
        }

        private async Task LoadLoanDetailsAsync()
        {
            string accountId = laccno.Text;
            
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_approved");

                var filter = Builders<BsonDocument>.Filter.Eq("AccountId", accountId);
                var document = await collection.Find(filter).FirstOrDefaultAsync();

                if (document != null)
                {
                    // Client Info
                    taccname.Text = $"{document.GetValue("FirstName", "")} {document.GetValue("MiddleName", "")} {document.GetValue("LastName", "")}".Trim();
                    trepname.Text = $"{document.GetValue("FirstName", "")} {document.GetValue("MiddleName", "")} {document.GetValue("LastName", "")}".Trim();
                    trepaddress.Text = $"{document.GetValue("Barangay", "")}, {document.GetValue("City", "")}, {document.GetValue("Province", "")}".Trim(); // Complete address
                    taccaddress.Text = $"{document.GetValue("Barangay", "")}, {document.GetValue("City", "")}, {document.GetValue("Province", "")}".Trim(); // Complete address
                    taccbrgy.Text = document.GetValue("Barangay", "").ToString();
                    tacctown.Text = document.GetValue("City", "").ToString();
                    taccprov.Text = document.GetValue("Province", "").ToString();
                    tacccontactno.Text = document.GetValue("ContactNumber", "").ToString();
                    taccemail.Text = document.GetValue("Email", "").ToString();
                    lclientno.Text = document.GetValue("ClientNo", "").ToString();

                    // Loan Info
                    string loanStatus = document.GetValue("LoanProcessStatus", "Not Available").ToString();
                    laccstatus.Text = loanStatus;

                    // Update lloanstatus based on LoanProcessStatus value
                    if (loanStatus == "For Releasing Loan Disbursement")
                    {
                        lloanstatus.Text = "FOR DISBURSEMENT";
                    }
                    else if (loanStatus == "Loan Released")
                    {
                        lloanstatus.Text = "ACTIVE";
                    }
                    else
                    {
                        lloanstatus.Text = "UNKNOWN STATUS"; // Default case
                    }

                    // Additional Info
                    trepname.Text = $"{document.GetValue("FirstName", "")} {document.GetValue("MiddleName", "")} {document.GetValue("LastName", "")}".Trim();
                    trepaddress.Text = $"{document.GetValue("Barangay", "")}, {document.GetValue("City", "")}, {document.GetValue("Province", "")}".Trim();
                    trepcontact.Text = document.GetValue("ContactNumber", "").ToString();

                    // Loan details
                    laccno.Text = document.GetValue("LoanNo", "").ToString();
                    trepcurrloan.Text = document.GetValue("LoanAmount", "").ToString();
                    treploanbalance.Text = document.GetValue("LoanBalance", "").ToString();
                    treploanpenalty.Text = document.GetValue("Penalty", "").ToString();
                    trepcollector.Text = document.GetValue("CollectorName", "").ToString();

                    // Loan Dates
                    treprepaydate.Text = document.GetValue("StartPaymentDate", "").ToString();
                  
                    // Loading document info into DataGridView (if applicable)
                    if (document.TryGetValue("docs", out BsonValue docsValue) && document.TryGetValue("doc-link", out BsonValue docLinksValue))
                    {
                        if (docsValue.IsBsonArray && docLinksValue.IsBsonArray)
                        {
                            var docsArray = docsValue.AsBsonArray.Select(d => d.AsString).ToArray();
                            var docLinksArray = docLinksValue.AsBsonArray.Select(l => l.AsString).ToArray();

                            LoadDocsIntoDataGridView(docsArray, docLinksArray);
                        }
                        else
                        {
                            MessageBox.Show("Document data is missing or incorrect.");
                        }
                    }
                    else
                    {
                        //MessageBox.Show("Document data is missing or incorrect.");
                    }

                    // Count the number of rows (documents) for the AccountId and set it in treploantotal.Text
                    long loanCount = await collection.CountDocumentsAsync(filter);
                    treploantotal.Text = loanCount.ToString();

                    // Step 1: Add columns first
                    CreateLoanDataTable();

                 
                    // Step 2: Load loan details into DataGridView
                    await LoadLoanDetailsToDataGridViewAsync();

                }
                else
                {
                    MessageBox.Show("No loan details found for the specified Account ID.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading loan details: {ex.Message}");
            }
        }

        private async Task LoadLoanDetailsToDataGridViewAsync()
        {
            try
            {
                // Validate if laccno.Text is not empty
                string loanNo = laccno.Text.Trim();
                if (string.IsNullOrEmpty(loanNo))
                {
                    MessageBox.Show("Please enter a Loan Number.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Step 1: Setup DataTable and bind it to DataGridView
                DataTable loanDataTable = CreateLoanDataTable();
                dgvdataamort.DataSource = loanDataTable;

                // Access the MongoDB database and fetch loan details from loan_disbursed
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_disbursed");

                // Filter by LoanNo from laccno.Text
                var filter = Builders<BsonDocument>.Filter.Eq("LoanNo", loanNo);
                var documents = await collection.Find(filter).ToListAsync();

                if (documents != null && documents.Count > 0)
                {
                    lnorecorddis.Visible = false;
                    foreach (var document in documents)
                    {
                        DataRow row = loanDataTable.NewRow();

                        // Loan ID
                        row["Loan ID"] = document.GetValue("LoanNo", "").ToString();

                        // Loan Details: LoanAmount, LoanBalance, LoanTerm, LoanInterest
                        row["Loan Details"] = $"Amount: {document.GetValue("LoanAmount", "₱0.00")}\n" +
                                              $"Balance: {document.GetValue("LoanBalance", "₱0.00")}\n" +
                                              $"Term: {document.GetValue("LoanTerm", "N/A")}\n" +
                                              $"Interest: {document.GetValue("LoanInterest", "₱0.00")}";

                        // Amortization: LoanAmortization, MissedDays, Penalty
                        string missedDays = "0"; // Placeholder, calculate if needed
                        row["Amortization"] = $"Amortization: {document.GetValue("LoanAmortization", "₱0.00")}\n" +
                                              $"Missed: {missedDays} days\n" +
                                              $"Penalty: {document.GetValue("Penalty", "₱0.00")}";

                        // Repayment: PaymentMode, StartPaymentDate, MaturityDate
                        row["Repayment"] = $"Mode: {document.GetValue("PaymentMode", "N/A")}\n" +
                                           $"Start: {document.GetValue("StartPaymentDate", "N/A")}\n" +
                                           $"Maturity: {document.GetValue("MaturityDate", "N/A")}";

                        // Add row to the DataTable
                        loanDataTable.Rows.Add(row);
                    }

                    // Update total loan count
                    treploantotal.Text = documents.Count.ToString();
                }
                else
                {
                    // No records found for the LoanNo, clear the DataGridView
                    loanDataTable.Clear();
                    dgvdataamort.DataSource = loanDataTable;
                    treploantotal.Text = "0";
                    lnorecorddis.Visible = true;
                    //MessageBox.Show($"No loan details found for Loan No: {loanNo}", "Loan Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading loan details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private async Task LoadLoanCollectionsAsync()
        {
            string clientNo = lclientno.Text; // Retrieve ClientNo

            // Initialize the connection to the database
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<BsonDocument>("loan_collections"); // The collection for loan collections

            // Create a filter to search based on ClientNo
            var filter = Builders<BsonDocument>.Filter.Eq("ClientNo", clientNo);

            // Fetch the loan collections for the specific client
            var loanCollections = await collection.Find(filter).ToListAsync(); // Fetch the data

            // Ensure the _loanCollectionTable is initialized
            if (_loanCollectionTable.Columns.Count == 0)
            {
                _loanCollectionTable.Columns.Add("Client Information", typeof(string));
                _loanCollectionTable.Columns.Add("Loan Information", typeof(string));
                _loanCollectionTable.Columns.Add("Payment Information", typeof(string));
                _loanCollectionTable.Columns.Add("Collection Information", typeof(string));
                _loanCollectionTable.Columns.Add("Remarks", typeof(string));
                _loanCollectionTable.Columns.Add("CollectionDateTemp", typeof(DateTime)); // Add temporary column for sorting
            }

            // Clear existing rows before loading new data
            _loanCollectionTable.Rows.Clear();
            dgvdatadis.DataSource = null; // Clear existing data source

            double totalAmountPaid = 0;
            double totalPenalty = 0;
            double totalLoanAmount = 0;
            double runningBalance = 0;  // Initialize running balance for calculations

            foreach (var collectionRecord in loanCollections)
            {
                // Client Information
                DateTime? collectionDate = collectionRecord.Contains("CollectionDate") ? collectionRecord["CollectionDate"].ToUniversalTime() : (DateTime?)null;
                string collectionDateStr = collectionDate.HasValue ? collectionDate.Value.ToString("MM/dd/yyyy") : "";

                string clientNumber = collectionRecord.Contains("ClientNo") ? collectionRecord["ClientNo"].AsString : "";
                string name = collectionRecord.Contains("Name") ? collectionRecord["Name"].AsString : "";

                string clientInfo = $"Col. Date: {collectionDateStr}\n" +
                                     $"Client No.: {clientNo}\n" +
                                     $"Name: {name}";

                // Loan Information
                double loanAmount = collectionRecord.Contains("LoanAmount") ? (double)collectionRecord["LoanAmount"].AsDecimal128 : 0.00;
                totalLoanAmount = loanAmount;  // Store loan amount to compute general balance later

                string amortization = collectionRecord.Contains("Amortization") ? ((double)collectionRecord["Amortization"].AsDecimal128).ToString("F2") : "0.00";

                // Calculate Running Balance as Loan Amount - Total Amount Paid
                double amountPaid = collectionRecord.Contains("ActualCollection") ? (double)collectionRecord["ActualCollection"].AsDecimal128 : 0.00;
                runningBalance = loanAmount - totalAmountPaid; // Running balance: Loan Amount - Total Amount Paid

                string runningBalanceStr = runningBalance <= 0 ? "Settled" : runningBalance.ToString("F2"); // Check if balance is zero or negative

                string loanInfo = $"Loan Amount: {loanAmount:F2}\n" +
                                  $"Amortization: {amortization}\n" +
                                  $"Running Balance: {runningBalanceStr}";

                // Payment Information
                string dateReceived = collectionRecord.Contains("DateReceived") ? collectionRecord["DateReceived"].ToUniversalTime().ToString("MM/dd/yyyy") : "";  // Fix to DateReceived field

                totalAmountPaid += amountPaid;  // Add amount paid to total payments

                string penalty = collectionRecord.Contains("CollectedPenalty") ? ((double)collectionRecord["CollectedPenalty"].AsDecimal128).ToString("F2") : "";
                string paymentMode = collectionRecord.Contains("PaymentMode") ? collectionRecord["PaymentMode"].AsString : "";

                string paymentInfo = $"Date Received: {dateReceived}\n" +
                                     $"Amount Paid: {amountPaid:F2}\n" +
                                     $"Penalty: {penalty}\n" +
                                     $"Payment Mode: {paymentMode}";

                // Collection Information
                string collector = collectionRecord.Contains("Collector") ? collectionRecord["Collector"].AsString : "";
                string area = collectionRecord.Contains("Address") ? collectionRecord["Address"].AsString : "";

                string collectionInfo = $"Collector: {collector}\n" +
                                        $"Address: {area}";

                // Remarks: Always Include Excess Amount Paid, Even if Settled
                string remarks = "";
                double excessAmount = totalAmountPaid - totalLoanAmount; // Calculate excess amount paid over total loan amount

                if (runningBalance <= 0)
                {
                    // Loan is fully paid
                    if (excessAmount > 0)
                    {
                        remarks = $"Loan is fully paid.\nExcess amount paid: {excessAmount:F2}";
                        lgenbal.Text = $"Excess payment: {excessAmount:F2}";  // Display excess payment in the UI
                    }
                    else
                    {
                        remarks = "Loan is fully paid";
                        lgenbal.Text = "0.00";
                    }
                }
                else
                {
                    // Loan still has a remaining balance
                    remarks = $"Remaining Balance: {runningBalance:F2}";
                    lgenbal.Text = $"Remaining Balance: {runningBalance:F2}";
                    // Also add the excess amount if it exists in an installment
                    if (excessAmount > 0)
                    {
                        remarks += $"\nExcess Amount Paid: {excessAmount:F2}";
                        lgenbal.Text += $"\nExcess Amount Paid: {excessAmount:F2}";
                    }
                }

                // Add data to DataTable, including CollectionDateTemp for sorting
                _loanCollectionTable.Rows.Add(clientInfo, loanInfo, paymentInfo, collectionInfo, remarks, collectionDate.HasValue ? (object)collectionDate.Value : DBNull.Value);
            }

            ltotalpayments.Text = "Total Payment Collection: " + loanCollections.Count.ToString();

            // Total Amount Paid Calculation
            double totalAmountPaidFromCollections = 0;
            foreach (DataGridViewRow row in dgvdatadis.Rows)
            {
                double amountPaid = Convert.ToDouble(row.Cells["AmountPaidColumn"].Value); // Replace with actual column name
                totalAmountPaidFromCollections += amountPaid;
            }
            ltotalamtpaid.Text = "Total Amount Paid: " + (totalAmountPaid + totalAmountPaidFromCollections).ToString("F2");

            // Remaining Balance (General Balance)
            double totalLoanAmountFromCollections = 0;
            foreach (DataGridViewRow row in dgvdatadis.Rows)
            {
                double loanAmount = Convert.ToDouble(row.Cells["LoanAmountColumn"].Value); // Replace with actual column name
                totalLoanAmountFromCollections += loanAmount;
            }

            // Penalty Calculation
            double totalPenaltyFromCollections = 0;
            foreach (DataGridViewRow row in dgvdatadis.Rows)
            {
                double penalty = Convert.ToDouble(row.Cells["PenaltyColumn"].Value); // Replace with actual column name
                totalPenaltyFromCollections += penalty;
            }
            lpenaltytotal.Text = "Generated Penalty: " + (totalPenalty + totalPenaltyFromCollections).ToString("F2");

            // Sort the DataTable by CollectionDateTemp in descending order
            DataView view = _loanCollectionTable.DefaultView;
            view.Sort = "CollectionDateTemp ASC";  // Sort by CollectionDateTemp in descending order
            DataTable sortedTable = view.ToTable();

            // Bind sorted data to DataGridView
            dgvdatadis.DataSource = sortedTable;

            // Enable word wrapping for DataGridView cells
            foreach (DataGridViewColumn col in dgvdatadis.Columns)
            {
                col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }

            // Auto-size row height based on content
            foreach (DataGridViewRow row in dgvdatadis.Rows)
            {
                row.Height = Math.Max(row.Height, 30); // Adjust height based on content
            }

            // Hide the temporary CollectionDateTemp column after sorting
            dgvdatadis.Columns["CollectionDateTemp"].Visible = false;
        }

        // Filter rows in DataGridView based on tdissearch.Text
        private void FilterDataGridView()
        {
            string filterText = tdissearch.Text.Trim().ToLower();

            // Apply filtering logic
            (dgvdatadis.DataSource as DataTable).DefaultView.RowFilter = string.Join(" OR ", dgvdatadis.Columns.Cast<DataGridViewColumn>()
                .Where(col => col.Visible)
                .Select(col => $"[{col.HeaderText}] LIKE '%{filterText}%'"));
        }

        private void ExportToExcel()
        {
            if (dgvdatadis.Rows.Count == 0)
            {
                MessageBox.Show("No data available to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Excel Files (*.xlsx)|*.xlsx";

                    string clientNo = dgvdatadis.Rows[0].Cells["ClientNo"].Value?.ToString() ?? "LoanCollectionsReport";
                    sfd.FileName = $"{clientNo}_LoanCollectionsReport.xlsx";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (var package = new ExcelPackage())
                        {
                            var worksheet = package.Workbook.Worksheets.Add("Loan Collections");

                            int rowIndex = 1;

                            // Add Primary Details
                            AddDetail(worksheet, ref rowIndex, "Client Name:", "ClientName");
                            AddDetail(worksheet, ref rowIndex, "Client Number:", lclientno.Text);
                            AddDetail(worksheet, ref rowIndex, "Loan Amount:", "LoanAmount", true);
                            AddDetail(worksheet, ref rowIndex, "Loan Term:", "LoanTerm");
                            AddDetail(worksheet, ref rowIndex, "Start Date:", "StartDate", false, true);
                            AddDetail(worksheet, ref rowIndex, "Maturity Date:", "MaturityDate", false, true);

                            rowIndex += 2; // Space before summary

                            // Add Summary
                            worksheet.Cells[rowIndex, 1].Value = "Summary of Loan Account";
                            worksheet.Cells[rowIndex, 1].Style.Font.Size = 14;
                            worksheet.Cells[rowIndex, 1].Style.Font.Bold = true;
                            rowIndex++;

                            AddDetail(worksheet, ref rowIndex, "Total Amount Paid:", "TotalPaid", true);
                            AddDetail(worksheet, ref rowIndex, "Remaining Balance:", "RemainingBalance", true);

                            rowIndex += 2; // Space before table

                            // Add Table Headers
                            for (int col = 0; col < dgvdatadis.Columns.Count; col++)
                            {
                                var cell = worksheet.Cells[rowIndex, col + 1];
                                cell.Value = dgvdatadis.Columns[col].HeaderText;
                                cell.Style.Font.Bold = true;
                                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                cell.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            }
                            rowIndex++;

                            // Add Data Rows
                            for (int row = 0; row < dgvdatadis.Rows.Count; row++)
                            {
                                for (int col = 0; col < dgvdatadis.Columns.Count; col++)
                                {
                                    object cellValue = dgvdatadis.Rows[row].Cells[col].Value ?? "";
                                    var cell = worksheet.Cells[rowIndex + row, col + 1];
                                    cell.Value = cellValue.ToString();
                                    cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    cell.Style.WrapText = true;
                                }
                            }

                            // Auto-fit columns
                            worksheet.Cells.AutoFitColumns();

                            // Save the Excel file
                            File.WriteAllBytes(sfd.FileName, package.GetAsByteArray());
                            MessageBox.Show("Data exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void AddDetail(ExcelWorksheet worksheet, ref int rowIndex, string label, string columnName, bool isCurrency = false, bool isDate = false)
        {
            worksheet.Cells[rowIndex, 1].Value = label;
            var cellValue = dgvdatadis.Rows[0].Cells[columnName]?.Value?.ToString() ?? "N/A";

            if (isCurrency && decimal.TryParse(cellValue, out decimal currencyValue))
            {
                worksheet.Cells[rowIndex, 2].Value = currencyValue;
                worksheet.Cells[rowIndex, 2].Style.Numberformat.Format = "₱#,##0.00"; // PHP Currency Format
            }
            else if (isDate && DateTime.TryParse(cellValue, out DateTime dateValue))
            {
                worksheet.Cells[rowIndex, 2].Value = dateValue;
                worksheet.Cells[rowIndex, 2].Style.Numberformat.Format = "MM/dd/yyyy"; // Date Format
            }
            else
            {
                worksheet.Cells[rowIndex, 2].Value = cellValue;
            }

            worksheet.Cells[rowIndex, 1, rowIndex, 2].Style.Font.Bold = true;
            rowIndex++;
        }


        private void dgvuploads_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is in the "ViewFile" button column
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvuploads.Columns["ViewFile"].Index)
            {
                // Retrieve the corresponding document link from the hidden column
                var linkCell = dgvuploads.Rows[e.RowIndex].Cells["DocumentLink"];
                if (linkCell?.Value != null && !string.IsNullOrWhiteSpace(linkCell.Value.ToString()))
                {
                    string docLink = linkCell.Value.ToString();

                    try
                    {
                        // Open the document link in the default browser
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = docLink,
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error opening the file: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("No document link found for this file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void dgvuploads_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvuploads.ClearSelection();
        }


        private void bcopyaccno_Click(object sender, EventArgs e)
        {
            // Get the text from the Label control
            string accNo = laccno.Text;

            // Copy the text to the clipboard
            Clipboard.SetText(accNo);

            // Show a message box to notify the user
            MessageBox.Show("The account number has been copied to your clipboard.", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bgemamt_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This function is not yet available. Please wait for further updates", "Feature not yet available", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bgenSOA_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This function is not yet available. Please wait for further updates", "Feature not yet available", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bgenledger_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This function is not yet available. Please wait for further updates", "Feature not yet available", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bgenremind_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This function is not yet available. Please wait for further updates", "Feature not yet available", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bgendemandinit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This function is not yet available. Please wait for further updates", "Feature not yet available", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void bgendemandfinal_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This function is not yet available. Please wait for further updates", "Feature not yet available", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvdataamort_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvdataamort.ClearSelection();
        }

        private void tdissearch_TextChanged(object sender, EventArgs e)
        {
            FilterDataGridView();
        }

        private async void bdisexport_Click(object sender, EventArgs e)
        {
            try
            {
                // Assuming clientno is coming from lclientno.Text
                string clientno = lclientno.Text.Trim();

                if (!string.IsNullOrEmpty(clientno))
                {
                    string savePath = "path_to_save\\SOA_" + clientno + ".xlsx"; // Customize the save path as required

                    // Now you only need to pass the clientno
                    frm_home_disburse_collections frmDisburseCollections = new frm_home_disburse_collections(clientno);

                    // Call GenerateSOA from frm_home_disburse_collections
                    await frmDisburseCollections.GenerateSOA(clientno, savePath);
                }
                else
                {
                    MessageBox.Show("Please enter a valid Client Number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

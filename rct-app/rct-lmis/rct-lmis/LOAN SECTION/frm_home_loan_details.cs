using ClosedXML.Excel;
using Microsoft.VisualBasic;
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
        public event Func<Task> OnLoanNumberUpdated;

        public frm_home_loan_new()
        {
            InitializeComponent();
            InitializeDataGridView();

            // Initialize DataTable for binding to DataGridView
            _loanCollectionTable = new DataTable();

            laccupdate.Visible = false;

            SetUpLoanNoAutoComplete();
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

                // Match the AccountId with laccno.Text
                var filter = Builders<BsonDocument>.Filter.Eq("AccountId", accountId);
                var document = await collection.Find(filter).FirstOrDefaultAsync();

                if (document != null)
                {
                    // LoanNo check
                    string loanNo = document.GetValue("LoanNo", "").ToString();
                    if (string.IsNullOrEmpty(loanNo))
                    {
                        // If LoanNo is empty, hide TabLoanDetails and show the update button
                        TabLoanDetails.Visible = false;
                        bupdate.Visible = true; // Make the update button visible
                        bcopyaccno.Visible = false;
                        laccupdate.Visible = true;
                        lloanaccno.ReadOnly = false;
                        lclientno.Visible = false;
                        lloanstatus.Visible = false;
                        laccstatus.Visible = false;
                        //lloanaccno.Focus(); // Focus on the laccno text box

                        return;
                    }
                    else
                    {
                        // If LoanNo is not empty, show TabLoanDetails and hide the update button
                        TabLoanDetails.Visible = true;
                        bcopyaccno.Visible = true;
                        bupdate.Visible = false; // Hide the update button
                        laccupdate.Visible = false;
                        lloanaccno.ReadOnly = true;
                        lclientno.Visible = true;
                        lloanstatus.Visible = true;
                        laccstatus.Visible = true;
                    }

                    // Set up AutoComplete for lloanaccno.Text with existing LoanNos
                   

                    // Client Info
                    lloanaccno.Text = document.GetValue("LoanNo", "").ToString();
                    taccname.Text = $"{document.GetValue("FirstName", "")} {document.GetValue("MiddleName", "")} {document.GetValue("LastName", "")}".Trim();
                    taccaddress.Text = $"{document.GetValue("Barangay", "")}, {document.GetValue("City", "")}, {document.GetValue("Province", "")}".Trim(); // Complete address
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
                        lloanstatus.Text = "LOAN ACTIVE";
                    }
                    else if (loanStatus == "Loan for Updating")
                    {
                        lloanstatus.Text = "FOR UPDATING";
                    }
                    else if (loanStatus == "Loan Updated")
                    {
                        lloanstatus.Text = "LOAN ACTIVE";
                    }
                    else
                    {
                        lloanstatus.Text = "UNKNOWN STATUS"; // Default case
                    }

                    // Loan Details
                    string loanAmount = document.GetValue("LoanAmount", "").ToString();
                    trepcurrloan.Text = string.IsNullOrEmpty(loanAmount) ? "No Amount Provided" : loanAmount;

                    string loanBalance = document.GetValue("LoanBalance", "").ToString();
                    treploanbalance.Text = string.IsNullOrEmpty(loanBalance) ? "No Balance Provided" : loanBalance;

                    string startPaymentDate = document.GetValue("StartPaymentDate", "").ToString();
                    treprepaydate.Text = string.IsNullOrEmpty(startPaymentDate) ? "No Start Date Provided" : startPaymentDate;

                    trepcollector.Text = document.GetValue("CollectorName", "").ToString();

                    // Document Info
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

        private async void SetUpLoanNoAutoComplete()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_approved");

                // Load LoanNo values
                var filter = Builders<BsonDocument>.Filter.Exists("LoanNo", true);
                var projection = Builders<BsonDocument>.Projection.Include("LoanNo");
                var loanNoDocuments = await collection.Find(filter).Project(projection).ToListAsync();

                // Extract and clean LoanNo strings
                var loanNos = loanNoDocuments
                    .Select(doc => doc.GetValue("LoanNo", "").ToString())
                    .Where(loanNo => !string.IsNullOrEmpty(loanNo))
                    .ToList();

                // Sort by year in descending order (most recent first)
                loanNos = loanNos
                    .OrderByDescending(loanNo =>
                    {
                        var parts = loanNo.Split('-');
                        if (parts.Length >= 2 && int.TryParse(parts[1], out int year))
                            return year;
                        return 0;
                    })
                    .ToList();

                // Assign to autocomplete
                AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();
                autoCompleteCollection.AddRange(loanNos.ToArray());

                lloanaccno.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                lloanaccno.AutoCompleteSource = AutoCompleteSource.CustomSource;
                lloanaccno.AutoCompleteCustomSource = autoCompleteCollection;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting up AutoComplete: {ex.Message}");
            }
        }



        private async Task LoadLoanDetailsToDataGridViewAsync()
        {
            try
            {
                string loanNo = lloanaccno.Text.Trim();
                if (string.IsNullOrEmpty(loanNo))
                {
                    MessageBox.Show("Please enter a Loan Number.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataTable loanDataTable = CreateLoanDataTable();
                dgvdataamort.DataSource = loanDataTable;

                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_disbursed");

                var filter = Builders<BsonDocument>.Filter.Eq("LoanNo", loanNo);
                var documents = await collection.Find(filter).ToListAsync();

                if (documents != null && documents.Count > 0)
                {
                    lnorecorddis.Visible = false;

                    foreach (var doc in documents)
                    {
                        DataRow row = loanDataTable.NewRow();

                        string loanId = doc.GetValue("LoanNo", "").ToString();
                        string loanAmount = doc.GetValue("LoanAmount", "₱0.00").ToString();
                        string loanBalance = doc.GetValue("LoanBalance", "₱0.00").ToString();
                        string loanTerm = doc.GetValue("LoanTerm", "N/A").ToString();

                        // Support both "LoanInterest" and "LoanInterestAmount"
                        string loanInterest = doc.Contains("LoanInterest")
                            ? doc.GetValue("LoanInterest", "₱0.00").ToString()
                            : doc.GetValue("LoanInterestAmount", "₱0.00").ToString();

                        // Support both "LoanAmortization" (preferred) or fallback
                        string amortization = doc.GetValue("LoanAmortization", "₱0.00").ToString();
                        string penalty = doc.GetValue("Penalty", "₱0.00").ToString();

                        // Fallback between "StartPaymentDate"/"DateStart", "MaturityDate"/"DateEnd"
                        string startDate = doc.Contains("StartPaymentDate")
                            ? doc.GetValue("StartPaymentDate", "N/A").ToString()
                            : doc.GetValue("DateStart", "N/A").ToString();

                        string maturityDate = doc.Contains("MaturityDate")
                            ? doc.GetValue("MaturityDate", "N/A").ToString()
                            : doc.GetValue("DateEnd", "N/A").ToString();

                        string paymentMode = doc.GetValue("PaymentMode", "N/A").ToString();

                        // Basic formatting
                        row["Loan ID"] = loanId;

                        row["Loan Details"] = $"Amount: {loanAmount}\n" +
                                              $"Balance: {loanBalance}\n" +
                                              $"Term: {loanTerm}\n" +
                                              $"Interest: {loanInterest}";

                        row["Amortization"] = $"Amortization: {amortization}\n" +
                                              $"Missed: 0 days\n" +
                                              $"Penalty: {penalty}";

                        row["Repayment"] = $"Mode: {paymentMode}\n" +
                                           $"Start: {startDate}\n" +
                                           $"Maturity: {maturityDate}";

                        loanDataTable.Rows.Add(row);
                    }

                    treploantotal.Text = documents.Count.ToString();
                }
                else
                {
                    loanDataTable.Clear();
                    dgvdataamort.DataSource = loanDataTable;
                    treploantotal.Text = "0";
                    lnorecorddis.Visible = true;
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

            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<BsonDocument>("loan_collections");

            var filter = Builders<BsonDocument>.Filter.Eq("ClientNo", clientNo);
            var loanCollections = await collection.Find(filter).ToListAsync();

            if (_loanCollectionTable.Columns.Count == 0)
            {
                _loanCollectionTable.Columns.Add("Client Information", typeof(string));
                _loanCollectionTable.Columns.Add("Loan Information", typeof(string));
                _loanCollectionTable.Columns.Add("Payment Information", typeof(string));
                _loanCollectionTable.Columns.Add("Collection Information", typeof(string));
                _loanCollectionTable.Columns.Add("Remarks", typeof(string));
                _loanCollectionTable.Columns.Add("CollectionDateTemp", typeof(DateTime));
            }

            _loanCollectionTable.Rows.Clear();
            dgvdatadis.DataSource = null;

            double totalAmountPaid = 0;
            double totalPenalty = 0;
            double totalLoanAmount = 0;
            double runningBalance = 0;

            foreach (var collectionRecord in loanCollections)
            {
                DateTime? collectionDate = collectionRecord.Contains("CollectionDate") ? collectionRecord["CollectionDate"].ToUniversalTime() : (DateTime?)null;
                string collectionDateStr = collectionDate.HasValue ? collectionDate.Value.ToString("MM/dd/yyyy") : "";

                string clientNumber = collectionRecord.Contains("ClientNo") ? collectionRecord["ClientNo"].AsString : "";
                string name = collectionRecord.Contains("Name") ? collectionRecord["Name"].AsString : "";

                string clientInfo = $"Col. Date: {collectionDateStr}\nClient No.: {clientNo}\nName: {name}";

                double loanAmount = collectionRecord.Contains("LoanAmount") ? (double)collectionRecord["LoanAmount"].AsDecimal128 : 0.00;
                totalLoanAmount = loanAmount;

                string amortization = collectionRecord.Contains("Amortization") ? ((double)collectionRecord["Amortization"].AsDecimal128).ToString("F2") : "0.00";

                double amountPaid = collectionRecord.Contains("ActualCollection") ? (double)collectionRecord["ActualCollection"].AsDecimal128 : 0.00;
                runningBalance = loanAmount - totalAmountPaid;

                string runningBalanceStr = runningBalance <= 0 ? "Settled" : runningBalance.ToString("F2");

                string loanInfo = $"Loan Amount: {loanAmount:F2}\nAmortization: {amortization}\nRunning Balance: {runningBalanceStr}";

                string dateReceived = collectionRecord.Contains("DateReceived") ? collectionRecord["DateReceived"].ToUniversalTime().ToString("MM/dd/yyyy") : "";

                totalAmountPaid += amountPaid;

                string penalty = collectionRecord.Contains("CollectedPenalty") ? ((double)collectionRecord["CollectedPenalty"].AsDecimal128).ToString("F2") : "0.00";
                string paymentMode = collectionRecord.Contains("PaymentMode") ? collectionRecord["PaymentMode"].AsString : "";

                string paymentInfo = $"Date Received: {dateReceived}\nAmount Paid: {amountPaid:F2}\nPenalty: {penalty}\nPayment Mode: {paymentMode}";

                string collector = collectionRecord.Contains("Collector") ? collectionRecord["Collector"].AsString : "";
                string area = collectionRecord.Contains("Address") ? collectionRecord["Address"].AsString : "";

                string collectionInfo = $"Collector: {collector}\nAddress: {area}";

                string remarks = "";
                double excessAmount = totalAmountPaid - totalLoanAmount;

                if (runningBalance <= 0)
                {
                    if (excessAmount > 0)
                    {
                        remarks = $"Loan is fully paid.\nExcess amount paid: {excessAmount:F2}";
                        lgenbal.Text = $"Excess payment: {excessAmount:F2}";
                    }
                    else
                    {
                        remarks = "Loan is fully paid";
                        lgenbal.Text = "0.00";
                    }
                }
                else
                {
                    remarks = $"Remaining Balance: {runningBalance:F2}";
                    lgenbal.Text = $"Remaining Balance: {runningBalance:F2}";
                    if (excessAmount > 0)
                    {
                        remarks += $"\nExcess Amount Paid: {excessAmount:F2}";
                        lgenbal.Text += $"\nExcess Amount Paid: {excessAmount:F2}";
                    }
                }

                _loanCollectionTable.Rows.Add(clientInfo, loanInfo, paymentInfo, collectionInfo, remarks, collectionDate.HasValue ? (object)collectionDate.Value : DBNull.Value);
            }

            // Sort the DataTable by CollectionDateTemp ascending
            DataView view = _loanCollectionTable.DefaultView;
            view.Sort = "CollectionDateTemp ASC";
            DataTable sortedTable = view.ToTable();
            dgvdatadis.DataSource = sortedTable;

            // Auto-format DataGridView
            foreach (DataGridViewColumn col in dgvdatadis.Columns)
            {
                col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }

            foreach (DataGridViewRow row in dgvdatadis.Rows)
            {
                row.Height = Math.Max(row.Height, 30);
            }

            if (dgvdatadis.Columns.Contains("CollectionDateTemp"))
            {
                dgvdatadis.Columns["CollectionDateTemp"].Visible = false;
            }

            // Set total collection count
            ltotalpayments.Text = "Total Payment Collection: " + (loanCollections?.Count ?? 0).ToString();

            if (dgvdatadis.Rows.Count == 0)
            {
                // No data
                ltotalamtpaid.Text = "Total Amount Paid: 0.00";
                lpenaltytotal.Text = "Generated Penalty: 0.00";
                lgenbal.Text = "Remaining Balance: 0.00";
            }
            else
            {
                // Calculate from dgv
                double totalAmountPaidFromCollections = 0;
                double totalLoanAmountFromCollections = 0;
                double totalPenaltyFromCollections = 0;

                foreach (DataGridViewRow row in dgvdatadis.Rows)
                {
                    if (row.Cells["AmountPaidColumn"].Value != null)
                        totalAmountPaidFromCollections += Convert.ToDouble(row.Cells["AmountPaidColumn"].Value);

                    if (row.Cells["LoanAmountColumn"].Value != null)
                        totalLoanAmountFromCollections += Convert.ToDouble(row.Cells["LoanAmountColumn"].Value);

                    if (row.Cells["PenaltyColumn"].Value != null)
                        totalPenaltyFromCollections += Convert.ToDouble(row.Cells["PenaltyColumn"].Value);
                }

                ltotalamtpaid.Text = "Total Amount Paid: " + (totalAmountPaid + totalAmountPaidFromCollections).ToString("F2");
                lpenaltytotal.Text = "Generated Penalty: " + (totalPenalty + totalPenaltyFromCollections).ToString("F2");
            }
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
            string accNo = lloanaccno.Text;

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

        private async Task UpdateLoanNoAsync()
        {
            string accountId = laccno.Text;  // Account ID from the textbox
            string loanNo = lloanaccno.Text;     // New LoanNo (also from the same textbox)

            if (string.IsNullOrEmpty(loanNo))
            {
                MessageBox.Show("Please enter a valid Loan Number.");
                return;  // Exit if LoanNo is empty
            }

            try
            {
                var database = MongoDBConnection.Instance.Database;

                // Collections
                var loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");
                var loanDisbursedCollection = database.GetCollection<BsonDocument>("loan_disbursed");

                // Filter by AccountId
                var filter = Builders<BsonDocument>.Filter.Eq("AccountId", accountId);

                // Create the update definition for LoanNo, LoanProcessStatus, and Date_Modified
                var update = Builders<BsonDocument>.Update
                    .Set("LoanNo", loanNo)                        // Update LoanNo
                    .Set("LoanProcessStatus", "Updated")          // Set LoanProcessStatus to "Updated"
                    .Set("Date_Modified", DateTime.UtcNow);       // Set Date_Modified to current UTC time

                // Perform the update operation on loan_approved collection
                var resultApproved = await loanApprovedCollection.UpdateOneAsync(filter, update);

                // Perform the update operation on loan_disbursed collection
                var resultDisbursed = await loanDisbursedCollection.UpdateOneAsync(filter, update);

                // Check if any document was updated in both collections
                if (resultApproved.ModifiedCount > 0 || resultDisbursed.ModifiedCount > 0)
                {
                    MessageBox.Show("Loan Number updated successfully!");
                    TabLoanDetails.Visible = true;  // Show TabLoanDetails
                    bupdate.Visible = false;
                    bcopyaccno.Visible = true;
                    laccupdate.Visible = false;

                    await LoadLoanDetailsAsync();   // Reload loan details to reflect the update
                    //if (OnLoanNumberUpdated != null)
                    //{
                    //    await OnLoanNumberUpdated.Invoke();
                    //}
                }
                else
                {
                    MessageBox.Show("No matching loan found or no update needed.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating Loan Number: {ex.Message}");
            }
        }

        private async void bupdate_Click(object sender, EventArgs e)
        {

            string loanNo = lloanaccno.Text.Trim();

            if (string.IsNullOrEmpty(loanNo))
            {
                MessageBox.Show("Loan number cannot be empty.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await UpdateLoanNoAsync();
        }


        private void lloanaccno_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Prevent accidental Enter from triggering anything
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // Suppress Enter key
            }
        }

        private void lloanaccno_Enter(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(lloanaccno.Text))
            {
                string currentYear = DateTime.Now.Year.ToString();
                lloanaccno.Text = $"RCT-{currentYear}-";
                // Optionally move cursor to the end:
                lloanaccno.SelectionStart = lloanaccno.Text.Length;
            }
        }
    }
}

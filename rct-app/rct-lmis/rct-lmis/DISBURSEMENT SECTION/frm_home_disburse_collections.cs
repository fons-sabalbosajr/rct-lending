using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Globalization;

namespace rct_lmis.DISBURSEMENT_SECTION
{
    public partial class frm_home_disburse_collections : Form
    {
        private string _loanId;
        private string _clientno;
        private IMongoCollection<BsonDocument> _loanDisbursedCollection;
        private IMongoCollection<BsonDocument> _loanVoucherCollection;
        private IMongoCollection<BsonDocument> _loanCollection;
        private DataTable _loanCollectionTable;

        public frm_home_disburse_collections(string loanId, string clientno)
        {
            InitializeComponent();
            _loanId = loanId;
            _clientno = clientno;
            // MongoDB connection initialization
            var database = MongoDBConnection.Instance.Database;
            _loanDisbursedCollection = database.GetCollection<BsonDocument>("loan_collections");
           
            // Initialize DataTable for binding to DataGridView
            _loanCollectionTable = new DataTable();
        }

        public void LoadLoanCollections()
        {
            // Ensure the _loanCollectionTable is initialized with the correct columns
            if (_loanCollectionTable.Columns.Count == 0)
            {
                // Add columns for each category: Client, Loan, Payment, Collection Information, and Remarks
                _loanCollectionTable.Columns.Add("Client Information", typeof(string));
                _loanCollectionTable.Columns.Add("Loan Information", typeof(string));
                _loanCollectionTable.Columns.Add("Payment Information", typeof(string));
                _loanCollectionTable.Columns.Add("Collection Information", typeof(string));
                _loanCollectionTable.Columns.Add("Remarks", typeof(string));
                // Add a column for PaymentStartDate
                _loanCollectionTable.Columns.Add("PaymentStartDate", typeof(DateTime));
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
                string loanToPay = collection.Contains("TotalLoanToPay") ? ((double)collection["TotalLoanToPay"].AsDecimal128).ToString("F2") : "0.00";
                string amortization = collection.Contains("Amortization") ? ((double)collection["Amortization"].AsDecimal128).ToString("F2") : "0.00";
                string runningBalance = collection.Contains("RunningBalance") ? ((double)collection["RunningBalance"].AsDecimal128).ToString("F2") : "0.00";

                // Concatenate Loan Information fields into a single string
                string loanInfo = $"Loan Amount: {loanAmount}\n" +
                                  $"Loan to Pay: {loanToPay}\n" +
                                  $"Amortization: {amortization}\n" +
                                  $"Running Balance: {runningBalance}";

                // Payment Information
                string dateReceived = collection.Contains("DateReceived") ?
                                      collection["DateReceived"].AsBsonDateTime.ToLocalTime().ToString("MM/dd/yyyy") : "";
                string amountPaid = collection.Contains("ActualCollection") ?
                                    ((double)collection["ActualCollection"].AsDecimal128).ToString("F2") : "0.00";
                string penalty = collection.Contains("CollectedPenalty") ?
                                 ((double)collection["CollectedPenalty"].AsDecimal128).ToString("F2") : "";
                string paymentMode = collection.Contains("PaymentMode") ?
                                     collection["PaymentMode"].AsString : "";

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
                    collectionStatus = collDate.Date == receivedDate.Date ? "Paid on Time" : "Over Due";
                }
                else if (paymentsMode == "WEEKLY")
                {
                    collectionStatus = (collDate.Date >= receivedDate.Date && collDate.Date < receivedDate.Date.AddDays(7)) ? "Paid on Time" : "Over Due";
                }
                else if (paymentsMode == "SEMI-MONTHLY")
                {
                    DateTime firstPaymentDue = new DateTime(collDate.Year, collDate.Month, 1);
                    DateTime secondPaymentDue = new DateTime(collDate.Year, collDate.Month, 15);
                    collectionStatus = (receivedDate.Date == firstPaymentDue.Date || receivedDate.Date == secondPaymentDue.Date) ? "Paid on Time" : "Over Due";
                }
                else if (paymentsMode == "MONTHLY")
                {
                    collectionStatus = collDate.Date == receivedDate.Date ? "Paid on Time" : "Over Due";
                }
                else
                {
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
                if (receivedDate.Date == collDate.Date)
                {
                    remarks = "Payment Completed";
                }
                else
                {
                    remarks = $"Total Days Missed: {(collDate.Date - paymentStartDate.Date).Days} on {paymentStartDate:MM/dd/yyyy}";
                }

                // Add the concatenated information to the DataTable
                _loanCollectionTable.Rows.Add(clientInfo, loanInfo, paymentInfo, collectionInfo, remarks, paymentStartDate);
            }

            // Sort the DataTable by PaymentStartDate (recent to oldest)
            DataView view = _loanCollectionTable.DefaultView;
            view.Sort = "PaymentStartDate DESC"; // Sort in descending order
            DataTable sortedTable = view.ToTable(); // Create a new DataTable with sorted data

            // Bind sorted data to DataGridView
            dgvdata.DataSource = sortedTable;

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
                dgvdata.Columns.Add(viewDetailsButton);
            }

            // Move the View Details button column to the end
            dgvdata.Columns["ViewDetails"].DisplayIndex = dgvdata.Columns.Count - 1;

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

        private void GenerateSOA()
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                var database = MongoDBConnection.Instance.Database;
                _loanCollection = database.GetCollection<BsonDocument>("loan_collections");
                var loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");
                var loanDisbursedCollection = database.GetCollection<BsonDocument>("loan_disbursed");

                string loanId = laccountid.Text; // The LoanID
                string borrowerName = "";
                string clientID = "";
                string address = "";
                string contactNo = "";
                double loanAmount = 0.0;
                string interestRate = "";
                double loanTerm = 0.0;
                string days = "";
                double loantopay = 0.0;

                // Fetch loan disbursement details using LoanIDNo
                var disbursementFilter = Builders<BsonDocument>.Filter.Eq("LoanIDNo", loanId);
                var loanDisbursement = loanDisbursedCollection.Find(disbursementFilter).FirstOrDefault();

                if (loanDisbursement == null)
                {
                    MessageBox.Show("Loan disbursement record not found.");
                    return;
                }

                // Extract client ID and loan amount from loan disbursement collection
                borrowerName = loanDisbursement.Contains("cashName") ? loanDisbursement["cashName"].AsString : "Unknown Name";
                clientID = loanDisbursement.Contains("cashClnNo") ? loanDisbursement["cashClnNo"].AsString : "Unknown ClientID";
                loanAmount = loanDisbursement.Contains("loanAmt") ? loanDisbursement["loanAmt"].ToDouble() : 0.0;
                interestRate = loanDisbursement.Contains("loanInterest") ? loanDisbursement["loanInterest"].AsString : "N/A";
                loantopay = loanDisbursement.Contains("loanAmt") ? loanDisbursement["loanAmt"].ToDouble() : 0.0;

                // Safely parse loanTerm
                loanTerm = loanDisbursement.Contains("loanTerm") ? loanDisbursement["loanTerm"].ToDouble() : 0.0;
                days = loanDisbursement.Contains("days") ? loanDisbursement["days"].AsString : "0";

                // Now, fetch address and contact number from loan approved collection using cashClnNo
                var approvedFilter = Builders<BsonDocument>.Filter.Eq("ClientNumber", clientID);
                var loanApproved = loanApprovedCollection.Find(approvedFilter).FirstOrDefault();

                if (loanApproved == null)
                {
                    MessageBox.Show("Loan approved record not found.");
                    return;
                }

                // Extract address and contact number from loan approved collection
                address = $"{loanApproved["Street"]}, {loanApproved["Barangay"]}, {loanApproved["City"]}, {loanApproved["Province"]}";
                contactNo = loanApproved.Contains("CP") ? loanApproved["CP"].AsString : "Unknown Contact";

                // Create Excel package
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add($"SOA-{borrowerName}");

                    // Set the worksheet to A4 size
                    worksheet.PrinterSettings.PaperSize = ePaperSize.A4;
                    worksheet.PrinterSettings.FitToPage = true;

                    // Format the top part of the worksheet
                    worksheet.Cells["A7"].Value = "STATEMENT OF ACCOUNT";
                    worksheet.Cells["A7"].Style.Font.Bold = true;
                    worksheet.Cells["A7"].Style.Font.Size = 12; // Make title larger
                    worksheet.Cells["A8"].Value = $"as of {DateTime.Now:MMMM dd, yyyy}";

                    // Loan and Borrower Information
                    worksheet.Cells["A10"].Value = "Account ID:";
                    worksheet.Cells["B10"].Value = loanId;
                    worksheet.Cells["A11"].Value = "ClientID:";
                    worksheet.Cells["B11"].Value = clientID;
                    worksheet.Cells["A12"].Value = "Name:";
                    worksheet.Cells["B12"].Value = borrowerName;
                    worksheet.Cells["A13"].Value = "Address:";
                    worksheet.Cells["B13"].Value = address;
                    worksheet.Cells["C13"].Value = "Contact No:";
                    worksheet.Cells["D13"].Value = contactNo;

                    worksheet.Cells["B10:E13"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells["A8:E13"].Style.Font.SetFromFont("Arial", 9);

                    // Loan Information
                    worksheet.Cells["A15"].Value = "LOAN INFORMATION";
                    worksheet.Cells["A15"].Style.Font.Bold = true;

                    worksheet.Cells["A16"].Value = "Loan Amount:";
                    worksheet.Cells["B16"].Value = loanAmount;
                    worksheet.Cells["C16"].Value = "Interest Rate:";
                    worksheet.Cells["D16"].Value = interestRate;
                    worksheet.Cells["A17"].Value = "Loan Term:";
                    worksheet.Cells["B17"].Value = $"{loanTerm} months"; // Display term with 'months'
                    worksheet.Cells["A18"].Value = "No. of Days:";
                    worksheet.Cells["B18"].Value = $"{days} days";
                    worksheet.Cells["A19"].Value = "Amount to Pay:";
                    worksheet.Cells["B19"].Value = loantopay;

                    // Date Released
                    worksheet.Cells["C17"].Value = "Date Released:";
                    worksheet.Cells["D17"].Value = loanDisbursement.Contains("cashDate") && DateTime.TryParse(loanDisbursement["cashDate"].AsString, out var cashDate)
                        ? cashDate.ToString("MM/dd/yyyy")
                        : "N/A";

                    // Maturity Date
                    // Fetch the PaymentMaturityDate from loan_collections collection
                    var maturityDateFilter = Builders<BsonDocument>.Filter.Eq("LoanID", loanId);
                    var loanCollectionMaturity = _loanCollection.Find(maturityDateFilter).FirstOrDefault();

                    // Extract the PaymentMaturityDate from the loan_collections collection
                    worksheet.Cells["C19"].Value = "Payment Maturity Date:";
                    worksheet.Cells["D19"].Value = loanCollectionMaturity.Contains("PaymentMaturityDate") && DateTime.TryParse(loanCollectionMaturity["PaymentMaturityDate"].AsString, out var paymentMaturityDate)
                       ? paymentMaturityDate.ToString("MM/dd/yyyy")
                       : "N/A";

                    worksheet.Cells["A20"].Value = "Amortization:";
                    worksheet.Cells["B20"].Value = GetDoubleValue(loanDisbursement, "amortizedAmt");
                    worksheet.Cells["C20"].Value = "Payment Mode:";
                    worksheet.Cells["D20"].Value = loanDisbursement.Contains("Mode") ? loanDisbursement["Mode"].ToString() : "N/A";
                    worksheet.Cells["C18"].Value = "Payment Start Date:";
                    worksheet.Cells["D18"].Value = loanDisbursement.Contains("PaymentStartDate") && DateTime.TryParse(loanDisbursement["PaymentStartDate"].AsString, out var paymentStartDate)
                        ? paymentStartDate.ToString("MM/dd/yyyy")
                        : "N/A";

                    worksheet.Cells["B16:B21"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells["E16"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worksheet.Cells["A15"].Style.Font.Bold = true;
                    worksheet.Cells["A15"].Style.Font.Size = 12;
                    worksheet.Cells["A16:E21"].Style.Font.SetFromFont("Arial", 9);

                    // Collection details headers
                    worksheet.Cells["A23"].Value = "COLLECTION DETAILS";
                    worksheet.Cells["A23"].Style.Font.Bold = true;
                    worksheet.Cells["A23"].Style.Font.Size = 12;

                    worksheet.Cells["A24"].Value = "Transaction Date";
                    worksheet.Cells["B24"].Value = "Amort. Amount";
                    worksheet.Cells["C24"].Value = "Running Amort. Due";
                    worksheet.Cells["D24"].Value = "Credit";
                    worksheet.Cells["E24"].Value = "Running Credit";
                    worksheet.Cells["F24"].Value = "Status";
                    worksheet.Cells["G24"].Value = "Missed Day?";
                    worksheet.Cells["H24"].Value = "Deficiency/Overpaid";
                    worksheet.Cells["I24"].Value = "Missed Day Amort. Amount";

                    // Set header styles
                    worksheet.Cells["A24:I24"].Style.Font.Bold = true;
                    worksheet.Cells["A24:I24"].Style.Font.Size = 9;
                    worksheet.Cells["A24:I24"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells["A25:I1000"].Style.Font.SetFromFont("Arial", 9);

                    double runningPrincipalDue = 0;

                    int row = 25;
                    var collectionFilter = Builders<BsonDocument>.Filter.Eq("LoanID", loanId);
                    var loanCollections = _loanCollection.Find(collectionFilter).ToList();
                    foreach (var collection in loanCollections)
                    {
                        string collectionDate = collection.Contains("CollectionDate") && collection["CollectionDate"].IsBsonDateTime
                                                ? collection["CollectionDate"].AsDateTime.ToUniversalTime().ToString("MM/dd/yyyy")
                                                : "";

                        double amortization = GetDoubleValue(collection, "Amortization");
                        double amountPaid = GetDoubleValue(collection, "ActualCollection");
                        double runningBalance = GetDoubleValue(collection, "RunningBalance");
                        string status = collection.Contains("PaymentStatus") ? collection["PaymentStatus"].ToString() : "N/A";

                        // Correctly check DateReceived
                        string missedDay = (collection.Contains("DateReceived") && collection["DateReceived"].IsBsonDateTime &&
                                            collection["CollectionDate"].AsDateTime.ToUniversalTime().Date >
                                            collection["DateReceived"].AsDateTime.ToUniversalTime().Date) ? "Y" : "N";

                        // Calculate Deficiency/Overpaid or Excess Payment
                        double excessPayment = amountPaid - amortization;

                        // Update runningPrincipalDue by adding the current AmountPaid
                        runningPrincipalDue += amountPaid;

                        worksheet.Cells[row, 1].Value = collectionDate;
                        worksheet.Cells[row, 2].Value = amortization;
                        worksheet.Cells[row, 3].Value = runningPrincipalDue;
                        worksheet.Cells[row, 4].Value = amountPaid;
                        worksheet.Cells[row, 5].Value = runningBalance;
                        worksheet.Cells[row, 6].Value = status;
                        worksheet.Cells[row, 7].Value = missedDay;
                        worksheet.Cells[row, 8].Value = excessPayment;
                        worksheet.Cells[row, 9].Value = missedDay == "Y" ? amortization : 0;

                        // Format cells
                        worksheet.Cells[row, 2].Style.Numberformat.Format = "#,##0.00";
                        worksheet.Cells[row, 3].Style.Numberformat.Format = "#,##0.00";
                        worksheet.Cells[row, 4].Style.Numberformat.Format = "#,##0.00";
                        worksheet.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";
                        worksheet.Cells[row, 8].Style.Numberformat.Format = "#,##0.00"; // Excess Payment
                        worksheet.Cells[row, 9].Style.Numberformat.Format = "#,##0.00"; // Missed Day Amortization

                        row++;
                    }

                    // Center the image in Column B
                    string imagePath = Path.Combine(Application.StartupPath, "Resources", "rctheader.jpg");
                    if (File.Exists(imagePath))
                    {
                        var picture = worksheet.Drawings.AddPicture("HeaderImage", imagePath);
                        picture.SetPosition(0, 0, 0, 0);
                        picture.From.Column = 1;
                        picture.From.Row = 0;
                    }
                    else
                    {
                        MessageBox.Show("Image file not found: " + imagePath);
                    }

                    // Set column widths
                    worksheet.Cells.AutoFitColumns();

                    // Adjusting the worksheet cells
                    worksheet.Cells["A1:I" + row].AutoFitColumns(); // Adjust columns

                    // Enable Word Wrap for all cells
                    worksheet.Cells.Style.WrapText = true;


                    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.Filter = "Excel Files|*.xlsx"; // Set the file type filter
                        saveFileDialog.Title = "Save an Excel File"; // Set the title of the dialog
                        saveFileDialog.FileName = $"Statement_of_Account_{borrowerName}.xlsx"; // Default file name

                        // Show the dialog and check if the user clicked Save
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            // Save the Excel package to the specified file
                            FileInfo excelFile = new FileInfo(saveFileDialog.FileName);
                            package.SaveAs(excelFile);

                            MessageBox.Show($"Statement of Account for {borrowerName} has been generated at {saveFileDialog.FileName}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message + ex.StackTrace}");
            }
        }


        private double GetDoubleValue(BsonDocument collection, string fieldName)
        {
            if (collection.Contains(fieldName))
            {
                var value = collection[fieldName];
                if (value.BsonType == BsonType.Double)
                {
                    return value.AsDouble;
                }
                else if (value.BsonType == BsonType.Int32)
                {
                    return value.AsInt32;
                }
                else if (value.BsonType == BsonType.Int64)
                {
                    return value.AsInt64;
                }
                else if (double.TryParse(value.ToString(), out var result))
                {
                    return result;
                }
            }
            return 0.0;
        }


        private void frm_home_disburse_collections_Load(object sender, EventArgs e)
        {
            laccountid.Text = _loanId;
            lclientno.Text = _clientno;
            LoadLoanCollections();
        }

        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvdata.Columns.Count - 1 && e.RowIndex >= 0)
            {
                string details = "";
                string remarks = dgvdata.Rows[e.RowIndex].Cells["Remarks"].Value.ToString();
                // Create your details string or a new form to show details
                details = $"Details for Row {e.RowIndex + 1}:\n{remarks}";

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
            frm_home_disburse_collections_add addCollectionForm = new frm_home_disburse_collections_add(_loanId);
            addCollectionForm.ShowDialog(this);
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
                    cellStyle.Padding = new Padding(40, 30, 40, 30);
                }
            }
        }

        private void bsoa_Click(object sender, EventArgs e)
        {
            // Show a confirmation dialog before generating the SOA
            DialogResult result = MessageBox.Show(
                "Are you sure you want to generate the Statement of Account?",
                "Confirm Generation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            // If the user clicks 'Yes', proceed to generate the SOA
            if (result == DialogResult.Yes)
            {
                GenerateSOA(); // Call the method to generate the SOA
            }
            else
            {
                MessageBox.Show("Statement of Account generation canceled.");
            }
        }

        private void bpayadvance_Click(object sender, EventArgs e)
        {

        }
    }
}

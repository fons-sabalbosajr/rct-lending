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
using System.Threading.Tasks;

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
        private string clientno;
        private ContextMenuStrip contextMenuStrip;

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

            // Create context menu strip
            contextMenuStrip = new ContextMenuStrip();

            // Create "Delete Row" menu item
            var deleteRowMenuItem = new ToolStripMenuItem("Delete Row");
            deleteRowMenuItem.Click += DeleteRowMenuItem_Click;

            // Add menu item to context menu
            contextMenuStrip.Items.Add(deleteRowMenuItem);

            // Assign the context menu to DataGridView
            dgvdata.ContextMenuStrip = contextMenuStrip;
        }

        public frm_home_disburse_collections(string clientno)
        {
            _clientno = clientno;
        }

        private void DeleteRowMenuItem_Click(object sender, EventArgs e)
        {
            // Get the selected row index
            int selectedRowIndex = dgvdata.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = dgvdata.Rows[selectedRowIndex];

            // Get the client number (or unique identifier) from the selected row
            string clientNo = selectedRow.Cells["Client Information"].Value.ToString();

            // Ask for confirmation before deleting
            var result = MessageBox.Show("Are you sure you want to delete this row?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Call the method to delete the data from MongoDB
                DeleteLoanCollectionData(clientNo);

                // Remove the row from the DataGridView
                dgvdata.Rows.RemoveAt(selectedRowIndex);

                // Reload the data in DataGridView after deleting
                LoadLoanCollections();
            }
            else
            {
                MessageBox.Show("Deletion canceled.");
            }
        }

        private void DeleteLoanCollectionData(string clientNo)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("ClientNo", clientNo);

            try
            {
                // Delete the document from MongoDB
                var deleteResult = _loanDisbursedCollection.DeleteOne(filter);

                // Check if the delete was successful
                if (deleteResult.DeletedCount > 0)
                {
                    MessageBox.Show("Record deleted successfully.");
                }
                else
                {
                    MessageBox.Show("No record found to delete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting record: {ex.Message}");
            }
        }

        public void LoadLoanCollections()
        {
            // Ensure the _loanCollectionTable is initialized with the correct columns
            if (_loanCollectionTable.Columns.Count == 0)
            {
                _loanCollectionTable.Columns.Add("Client Information", typeof(string));
                _loanCollectionTable.Columns.Add("Loan Information", typeof(string));
                _loanCollectionTable.Columns.Add("Payment Information", typeof(string));
                _loanCollectionTable.Columns.Add("Collection Information", typeof(string));
                _loanCollectionTable.Columns.Add("Remarks", typeof(string));
                _loanCollectionTable.Columns.Add("CollectionDateTemp", typeof(DateTime)); // Add temporary column for sorting
                _loanCollectionTable.Columns.Add("BalanceTemp", typeof(double)); // Add column for sorting by balance
            }

            // Clear existing rows before loading new data
            _loanCollectionTable.Rows.Clear();
            dgvdata.DataSource = null; // Clear existing data source

            double totalAmountPaid = 0;
            double totalLoanAmount = 0;
            double runningBalance = 0;  // Initialize running balance for calculations
            double totalPenalty = 0; // Initialize total penalty calculation

            var filter = Builders<BsonDocument>.Filter.Eq("ClientNo", laccountid.Text);
            var loanCollections = _loanDisbursedCollection.Find(filter).ToList();
            _loanCollectionTable.Rows.Clear();

            foreach (var collection in loanCollections)
            {
                // Client Information
                DateTime? collectionDate = collection.Contains("CollectionDate") ? collection["CollectionDate"].ToUniversalTime() : (DateTime?)null;
                string collectionDateStr = collectionDate.HasValue ? collectionDate.Value.ToString("MM/dd/yyyy") : "";

                string clientNo = collection.Contains("ClientNo") ? collection["ClientNo"].AsString : "";
                string name = collection.Contains("Name") ? collection["Name"].AsString : "";

                string clientInfo = $"Col. Date: {collectionDateStr}\n" +
                                     $"Client No.: {clientNo}\n" +
                                     $"Name: {name}";

                // Loan Information
                double loanAmount = collection.Contains("LoanAmount") ? (double)collection["LoanAmount"].ToDouble() : 0.00;
                totalLoanAmount = loanAmount;  // Store loan amount to compute general balance later

                string amortization = collection.Contains("Amortization") ? ((double)collection["Amortization"].AsDecimal128).ToString("F2") : "0.00";

                // Calculate Running Balance as Loan Amount - Total Amount Paid
                double amountPaid = collection.Contains("ActualCollection") ? (double)collection["ActualCollection"].AsDecimal128 : 0.00;
                totalAmountPaid += amountPaid; // Accumulate total amount paid
                runningBalance = loanAmount - totalAmountPaid; // Running balance: Loan Amount - Total Amount Paid

                string runningBalanceStr = runningBalance <= 0 ? "Settled" : runningBalance.ToString("F2"); // Check if balance is zero or negative

                string loanInfo = $"Loan Amount: {loanAmount:F2}\n" +
                                  $"Amortization: {amortization}\n" +
                                  $"Running Balance: {runningBalanceStr}";

                // Payment Information
                string dateReceived = collection.Contains("DateReceived") ? collection["DateReceived"].ToUniversalTime().ToString("MM/dd/yyyy") : "";

                string penalty = collection.Contains("CollectedPenalty") ? ((double)collection["CollectedPenalty"].AsDecimal128).ToString("F2") : "";
                totalPenalty += string.IsNullOrEmpty(penalty) ? 0.00 : Convert.ToDouble(penalty); // Add to total penalty if present

                string paymentMode = collection.Contains("PaymentMode") ? collection["PaymentMode"].AsString : "";

                string paymentInfo = $"Date Received: {dateReceived}\n" +
                                     $"Amount Paid: {amountPaid:F2}\n" +
                                     $"Penalty: {penalty}\n" +
                                     $"Payment Mode: {paymentMode}";

                // Collection Information
                string collector = collection.Contains("Collector") ? collection["Collector"].AsString : "";
                string area = collection.Contains("Address") ? collection["Address"].AsString : "";

                string collectionInfo = $"Collector: {collector}\n" +
                                        $"Address: {area}";

                // Remarks: Always Include Excess Amount Paid, Even if Settled
                string remarks = "";
                double excessAmount = totalAmountPaid - totalLoanAmount; // Calculate excess amount paid over total loan amount

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

                // Extract balance value for sorting
                double balanceValue = runningBalance > 0 ? runningBalance : 0;

                // Add data to DataTable, including CollectionDateTemp and BalanceTemp for sorting
                DataRow row = _loanCollectionTable.NewRow();
                row["Client Information"] = clientInfo;
                row["Loan Information"] = loanInfo;
                row["Payment Information"] = paymentInfo;
                row["Collection Information"] = collectionInfo;
                row["Remarks"] = remarks;
                row["CollectionDateTemp"] = collectionDate.HasValue ? (object)collectionDate.Value : DBNull.Value;
                row["BalanceTemp"] = balanceValue;

                _loanCollectionTable.Rows.Add(row);
            }

          
            // Total Amount Paid Calculation
            ltotalamtpaid.Text = "Total Amount Paid: " + totalAmountPaid.ToString("F2");

            // Penalty Calculation (If no penalties, display 0.00)
            lpenaltytotal.Text = "Generated Penalty: " + totalPenalty.ToString("F2");

            // Sort DataTable by BalanceTemp in descending order, then CollectionDateTemp in ascending order
            DataView view = _loanCollectionTable.DefaultView;
            view.Sort = "BalanceTemp DESC, CollectionDateTemp ASC";
            DataTable sortedTable = view.ToTable();
            dgvdata.DataSource = sortedTable;

            // Total Payments Text (Count total rows in dgvdata)
            ltotalpayments.Text = "Total Payment Collection: " + dgvdata.Rows.Count.ToString();

            // Highlight rows with excess payment after binding
            for (int i = 0; i < dgvdata.Rows.Count; i++)
            {
                var remarks = dgvdata.Rows[i].Cells["Remarks"].Value.ToString();
                if (remarks.Contains("Excess amount paid"))
                {
                    dgvdata.Rows[i].DefaultCellStyle.BackColor = Color.Khaki;
                }
            }

            // Hide the temporary sorting columns
            dgvdata.Columns["BalanceTemp"].Visible = false;
            dgvdata.Columns["CollectionDateTemp"].Visible = false;
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

        public async Task GenerateSOA(string tloanno, string savePath)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (ExcelPackage package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Statement of Account-" + tloanno);

                    // Set up initial worksheet formatting
                    worksheet.Cells.Style.Font.Name = "Aptos Narrow";
                    worksheet.Cells.Style.Font.Size = 10;

                    // Set up column width similar to DataGridView
                    worksheet.Column(1).Width = 20;  // Collection Date
                    worksheet.Column(2).Width = 15;  // LoanID
                    worksheet.Column(3).Width = 30;  // Name
                    worksheet.Column(4).Width = 15;  // Amount to Pay
                    worksheet.Column(5).Width = 15;  // Amount Paid
                    worksheet.Column(6).Width = 15;  // Remaining Balance
                    worksheet.Column(7).Width = 15;  // Amount Settled
                    worksheet.Column(8).Width = 30;  // Remarks
                    worksheet.Column(9).Width = 20;  // Collector

                    // Set up cell borders similar to DataGridView
                    var headerRange = worksheet.Cells["A11:I11"];
                    headerRange.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    headerRange.Style.Font.Bold = true;

                    Console.WriteLine($"Querying for LoanID: {tloanno}");

                    // Fetch loan details
                    var filter = Builders<BsonDocument>.Filter.Eq("ClientNo", tloanno);
                    var loanDetails = await _loanDisbursedCollection.Find(filter).FirstOrDefaultAsync();

                    if (loanDetails != null)
                    {
                        try
                        {
                            Console.WriteLine($"Found LoanID: {loanDetails["ClientNo"]}");

                            // Fetching values based on provided data structure
                            string loanClient = GetStringValue(loanDetails, "Name");
                            decimal loantopay = GetDecimalValue(loanDetails, "TotalLoanToPay");
                            decimal remainingBalance = loantopay;
                            decimal totalSettled = 0; // Amount settled so far
                            string paymentStartDate = GetFormattedDate(loanDetails, "PaymentStartDate");  // Added Payment Start Date
                            string maturityDate = GetFormattedDate(loanDetails, "PaymentMaturityDate");

                            // Add loan information to worksheet
                            worksheet.Cells["A1"].Value = "Statement of Account";
                            worksheet.Cells["A1"].Style.Font.Bold = true;
                            worksheet.Cells["A1"].Style.Font.Size = 11;

                            worksheet.Cells["A3"].Value = "Client Name:";
                            worksheet.Cells["B3"].Value = loanClient;

                            worksheet.Cells["A4"].Value = "Client No:";
                            worksheet.Cells["B4"].Value = tloanno;

                            worksheet.Cells["A5"].Value = "Loan Payable:";
                            worksheet.Cells["B5"].Value = loantopay.ToString("₱#,##0.00");

                            worksheet.Cells["A6"].Value = "Payment Start Date:";  // Added Payment Start Date
                            worksheet.Cells["B6"].Value = paymentStartDate;

                            worksheet.Cells["A7"].Value = "Maturity Date:";
                            worksheet.Cells["B7"].Value = maturityDate;

                            // Add headers for Payment History
                            worksheet.Cells["A11"].Value = "Collection Date";
                            worksheet.Cells["B11"].Value = "ClientNo";
                            worksheet.Cells["C11"].Value = "Name";
                            worksheet.Cells["D11"].Value = "Amount to Pay";
                            worksheet.Cells["E11"].Value = "Amount Paid";
                            worksheet.Cells["F11"].Value = "Remaining Balance";
                            worksheet.Cells["G11"].Value = "Amount Settled";
                            worksheet.Cells["H11"].Value = "Remarks";
                            worksheet.Cells["I11"].Value = "Collector";
                            worksheet.Cells["A11:I11"].Style.Font.Bold = true;
                            worksheet.Cells["A11:I11"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            int row = 12;
                            decimal totalPaid = 0;

                            // Fetch collections for this loan
                            var collectionFilter = Builders<BsonDocument>.Filter.Eq("ClientNo", tloanno);
                            var loanCollections = await _loanDisbursedCollection.Find(collectionFilter).ToListAsync();

                            foreach (var collection in loanCollections)
                            {
                                DateTime paymentDate = GetDateValue(collection, "CollectionDate");
                                decimal amountPaid = GetDecimalValue(collection, "TotalCollected");
                                string remarks = GetStringValue(collection, "Remarks");
                                string collector = GetStringValue(collection, "Collector");

                                totalPaid += amountPaid;

                                // Update remaining balance
                                remainingBalance -= amountPaid;

                                // Update the cumulative "Amount Settled"
                                totalSettled += amountPaid;

                                // Check if Amount Settled exceeds Amount to Pay
                                string settledStatus = totalSettled > loantopay
                                    ? $"Payment Exceeded: {totalSettled - loantopay:₱#,##0.00}"
                                    : totalSettled.ToString("₱#,##0.00");

                                worksheet.Cells[$"A{row}"].Value = paymentDate.ToString("MM/dd/yyyy");
                                worksheet.Cells[$"B{row}"].Value = tloanno; // LoanID
                                worksheet.Cells[$"C{row}"].Value = loanClient; // Name
                                worksheet.Cells[$"D{row}"].Value = loantopay.ToString("₱#,##0.00"); // Amount to Pay
                                worksheet.Cells[$"E{row}"].Value = amountPaid.ToString("₱#,##0.00"); // Amount Paid
                                worksheet.Cells[$"F{row}"].Value = remainingBalance <= 0 ? "Settled/No Balance" : remainingBalance.ToString("₱#,##0.00"); // Remaining Balance
                                worksheet.Cells[$"G{row}"].Value = settledStatus; // Amount Settled

                                // Apply font color change for excess payment (Amount Paid cell)
                                if (remainingBalance <= 0)
                                {
                                    // Make "Amount Paid" red if there's an overpayment
                                    worksheet.Cells[$"E{row}"].Style.Font.Color.SetColor(System.Drawing.Color.Red);
                                    worksheet.Cells[$"F{row}"].Style.Font.Color.SetColor(System.Drawing.Color.Green);
                                    worksheet.Cells[$"G{row}"].Style.Font.Color.SetColor(System.Drawing.Color.Green);
                                }

                                worksheet.Cells[$"H{row}"].Value = remarks; // Remarks
                                worksheet.Cells[$"I{row}"].Value = collector; // Collector

                                // Apply style like DataGridView (e.g., borders, fonts)
                                worksheet.Cells[$"A{row}:I{row}"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[$"A{row}:I{row}"].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                                row++;
                            }

                            // Display total payments
                            worksheet.Cells[$"A{row}:B{row}"].Style.Font.Bold = true;
                            worksheet.Cells[$"A{row}:B{row}"].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            row += 2;

                            // Check for overpayment
                            decimal excessAmount = totalPaid - loantopay;
                            string balanceStatus = excessAmount > 0
                                ? $"Overpaid by {excessAmount:₱#,##0.00}"
                                : $"Balance Remaining: {(loantopay - totalPaid):₱#,##0.00}";

                            worksheet.Cells[$"A{row}"].Value = "Loan Balance Status:";
                            worksheet.Cells[$"B{row}"].Value = balanceStatus;
                            worksheet.Cells[$"B{row}"].Style.Font.Bold = true;

                            if (excessAmount > 0)
                            {
                                worksheet.Cells[$"B{row}"].Style.Font.Color.SetColor(System.Drawing.Color.Red);
                            }
                            else 
                            {
                                worksheet.Cells[$"B{row}"].Style.Font.Color.SetColor(System.Drawing.Color.Green);
                            }

                            worksheet.Cells.AutoFitColumns();

                            // Save the file
                            FileInfo fileInfo = new FileInfo(savePath);
                            await package.SaveAsAsync(fileInfo);
                            MessageBox.Show("SOA generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            // Detailed exception handling for data retrieval or formatting errors
                            MessageBox.Show($"An error occurred while processing loan details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Console.WriteLine("Loan details not found.");
                        }
                    }
                    else
                    {
                        // If loanDetails is null
                        MessageBox.Show("Loan details not found for the given loan number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                // Generic error handling for the entire process
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        // Helper Methods
        private string GetStringValue(BsonDocument document, string key)
        {
            return document.Contains(key) ? document[key].AsString : string.Empty;
        }

        public DateTime GetDateValue(BsonDocument document, string fieldName)
        {
            var fieldValue = document[fieldName];

            if (fieldValue.IsBsonDateTime)
            {
                return fieldValue.ToUniversalTime();  // If the field is a BsonDateTime, use it directly
            }
            else if (fieldValue.BsonType == BsonType.String)
            {
                // If the field is a string, attempt to parse it into DateTime
                if (DateTime.TryParse(fieldValue.ToString(), out DateTime dateValue))
                {
                    return dateValue.ToUniversalTime();
                }
                else
                {
                    throw new InvalidCastException($"Invalid date format in field '{fieldName}'.");
                }
            }
            else
            {
                throw new InvalidCastException($"Field '{fieldName}' is not a valid DateTime.");
            }
        }

        private string GetFormattedDate(BsonDocument document, string key)
        {
            return document.Contains(key) ? document[key].ToUniversalTime().ToString("MM/dd/yyyy") : "N/A";
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

        private decimal GetDecimalValue(BsonDocument document, string field)
        {
            if (document.Contains(field) && document[field] != BsonNull.Value)
            {
                // Safely extract and return the decimal value
                return document[field].ToDecimal();
            }
            else
            {
                // Return 0 if the field is missing or null
                return 0;
            }
        }

        private void UpdateLoanCollectionData(string clientNo, string columnName, string editedValue)
        {
            // Ask for user confirmation before proceeding
            var result = MessageBox.Show("Are you sure you want to update this record?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var filter = Builders<BsonDocument>.Filter.Eq("ClientNo", clientNo);
                var update = Builders<BsonDocument>.Update.Set(columnName, editedValue); // Update the field

                try
                {
                    // Update the document in MongoDB
                    var updateResult = _loanDisbursedCollection.UpdateOne(filter, update);

                    // Check if the update was successful
                    if (updateResult.ModifiedCount > 0)
                    {
                        MessageBox.Show("Record updated successfully.");

                        // Reload the data in DataGridView after the update
                        LoadLoanCollections(); // This will refresh the DataGridView
                    }
                    else
                    {
                        MessageBox.Show("No record found to update.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating record: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Update canceled.");
            }
        }


        private void frm_home_disburse_collections_Load(object sender, EventArgs e)
        {
            laccountid.Text = _loanId;
            lclientno.Text = _clientno;
            LoadLoanCollections();


            // Step 1: Retrieve the loan collection data
            var filter = Builders<BsonDocument>.Filter.Eq("ClientNo", laccountid.Text);
            var loanCollections = _loanDisbursedCollection.Find(filter).ToList();

            double totalAmountPaid = 0;
            double totalLoanAmount = 0;
            double excessAmount = 0;

            // Step 2: Calculate total paid amount and excess amount
            foreach (var collection in loanCollections)
            {
                double loanAmount = collection.Contains("LoanAmount") ? (double)collection["LoanAmount"].ToDouble() : 0.00;
                totalLoanAmount = loanAmount;  // Store loan amount to compute general balance later

                double amountPaid = collection.Contains("ActualCollection") ? (double)collection["ActualCollection"].AsDecimal128 : 0.00;
                totalAmountPaid += amountPaid;  // Add amount paid to total payments

                // Calculate excess amount
                excessAmount = totalAmountPaid - totalLoanAmount;
            }

            // Step 3: Check if there is an overpayment
            if (excessAmount > 0)
            {
              
                // Show an overpayment warning if detected
                MessageBox.Show($"This loan account has an overpayment of ₱{excessAmount:F2}. Please review the payment history for adjustments.",
                                "Overpayment Detected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                bnew.Enabled = true;
            }
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
            // Step 1: Retrieve the loan collection data
            var filter = Builders<BsonDocument>.Filter.Eq("ClientNo", laccountid.Text);
            var loanCollections = _loanDisbursedCollection.Find(filter).ToList();

            double totalAmountPaid = 0;
            double totalLoanAmount = 0;
            double excessAmount = 0;

            // Step 2: Calculate total paid amount and excess amount
            foreach (var collection in loanCollections)
            {
                double loanAmount = collection.Contains("LoanAmount") ? (double)collection["LoanAmount"].AsDecimal128 : 0.00;
                totalLoanAmount = loanAmount;  // Store loan amount to compute general balance later

                double amountPaid = collection.Contains("ActualCollection") ? (double)collection["ActualCollection"].AsDecimal128 : 0.00;
                totalAmountPaid += amountPaid;  // Add amount paid to total payments

                // Calculate excess amount
                excessAmount = totalAmountPaid - totalLoanAmount;
            }

            // Step 3: Check if there is an overpayment
            if (excessAmount > 0)
            {
                // Show an overpayment warning if detected
                MessageBox.Show($"This loan account has an overpayment of ₱{excessAmount:F2}. Please review the payment history for adjustments.",
                                "Overpayment Detected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Step 4: No overpayment, proceed to open the add collection form
                frm_home_disburse_collections_add addCollectionForm = new frm_home_disburse_collections_add(_loanId);
                addCollectionForm.ShowDialog(this);
            }
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

        private async void bsoa_Click(object sender, EventArgs e)
        {
            // Get the Loan Number from your input field
            string tloanno = laccountid.Text.Trim(); // Replace tloannoTextbox with your actual Loan No textbox name

            if (string.IsNullOrEmpty(tloanno))
            {
                MessageBox.Show("Please enter a valid Loan Number before generating the SOA.",
                                "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Stop execution if the Loan No is missing
            }

            // Prompt the user for confirmation
            DialogResult result = MessageBox.Show(
                "Are you sure you want to generate the Statement of Account?",
                "Confirm Generation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Let the user select a location to save the file
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Excel Files|*.xlsx";
                    saveFileDialog.Title = "Save Statement of Account";
                    saveFileDialog.FileName = $"SOA_{tloanno}.xlsx"; // Default filename

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string savePath = saveFileDialog.FileName;
                        await GenerateSOA(tloanno, savePath); // Call the GenerateSOA method with parameters
                    }
                }
            }
            else
            {
                MessageBox.Show("Statement of Account generation canceled.");
            }
        }


        private void bpayadvance_Click(object sender, EventArgs e)
        {

        }

        private void dgvdata_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Get the edited row's data
            int rowIndex = e.RowIndex;
            if (rowIndex >= 0)
            {
                // Retrieve the edited value from the DataGridView cell
                string columnName = dgvdata.Columns[e.ColumnIndex].Name;
                var editedValue = dgvdata.Rows[rowIndex].Cells[e.ColumnIndex].Value.ToString();

                // Find the ClientNo (or another unique identifier) for the record
                string clientNo = dgvdata.Rows[rowIndex].Cells["Client Information"].Value.ToString();

                // Call Update function
                UpdateLoanCollectionData(clientNo, columnName, editedValue);
            }
        }

        private void bconfig_Click(object sender, EventArgs e)
        {
            string clientNo = laccountid.Text;
            string loanId = laccountid.Text;

            
            frm_home_disburse_loan_config loanConfigForm = new frm_home_disburse_loan_config(clientNo, loanId);
            loanConfigForm.ShowDialog(); 
        }
    }
}

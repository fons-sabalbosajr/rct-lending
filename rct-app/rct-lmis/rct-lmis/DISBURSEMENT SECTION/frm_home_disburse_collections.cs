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
using System.Collections.Generic;

namespace rct_lmis.DISBURSEMENT_SECTION
{
    public partial class frm_home_disburse_collections : Form
    {
        private string _loanId;
        private string _clientno;
        private IMongoCollection<BsonDocument> _loanDisbursedCollection;
        private IMongoCollection<BsonDocument> _loanVoucherCollection;
        private IMongoCollection<BsonDocument> _loanAccountCyclesCollection;
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
            _loanCollection = database.GetCollection<BsonDocument>("loan_collections");
            _loanAccountCyclesCollection = database.GetCollection<BsonDocument>("loan_account_cycles");
            _loanDisbursedCollection = database.GetCollection<BsonDocument>("loan_disbursed");

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


        private void EnsureLoanCollectionTableSchema()
        {
            if (_loanCollectionTable.Columns.Count == 0)
            {
                _loanCollectionTable.Columns.Add("Client Information", typeof(string));
                _loanCollectionTable.Columns.Add("Loan Information", typeof(string));
                _loanCollectionTable.Columns.Add("Payment Information", typeof(string));
                _loanCollectionTable.Columns.Add("Collection Information", typeof(string));
                _loanCollectionTable.Columns.Add("Remarks", typeof(string));
                _loanCollectionTable.Columns.Add("CollectionDateTemp", typeof(DateTime));
                _loanCollectionTable.Columns.Add("BalanceTemp", typeof(double));
            }
        }


        public void LoadLoanCollections()
        {
            EnsureLoanCollectionTableSchema();

            _loanCollectionTable.Rows.Clear();
            dgvdata.DataSource = null;

            double totalAmountPaid = 0;
            double totalLoanAmount = 0;
            double runningBalance = 0;
            double totalPenalty = 0;

            string selectedLoanNo = cbloanno.Text?.Trim();
            if (string.IsNullOrEmpty(selectedLoanNo))
            {
                dgvdata.DataSource = _loanCollectionTable; // show empty structure
                return;
            }

            // First, fetch the loan metadata (loan amount, etc) from either loan_account_cycles or loan_disbursed
            var loanFilter = Builders<BsonDocument>.Filter.Eq("LoanNo", selectedLoanNo);
            var loanDoc = _loanAccountCyclesCollection.Find(loanFilter).FirstOrDefault();
            if (loanDoc == null)
            {
                loanDoc = _loanDisbursedCollection.Find(loanFilter).FirstOrDefault();
            }

            if (loanDoc == null)
            {
                MessageBox.Show("Loan details not found for selected LoanNo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvdata.DataSource = _loanCollectionTable;
                return;
            }

            // Extract loan amount from loanDoc
            if (loanDoc.Contains("LoanAmount"))
            {
                // LoanAmount can be string with currency or number, normalize:
                string loanAmtRaw = loanDoc["LoanAmount"].ToString().Replace("₱", "").Replace(",", "").Trim();
                double.TryParse(loanAmtRaw, out totalLoanAmount);
            }

            // Now fetch loan collections/payments for this loan number
            // Assuming your collection name is _loanCollectionsCollection (not shown in your sample)
            var collectionsFilter = Builders<BsonDocument>.Filter.Eq("LoanNo", selectedLoanNo);
            var loanCollections = _loanCollection.Find(collectionsFilter).ToList();

            if (loanCollections.Count == 0)
            {
                dgvdata.DataSource = _loanCollectionTable;

                lgenbal.Text = "Remaing Balance:" + totalLoanAmount.ToString("F2");
                ltotalamtpaid.Text = "Total Amount Paid: 0.00";
                lpenaltytotal.Text = "Generated Penalty: 0.00";
                ltotalpayments.Text = "Total Payment Collection: 0";

                return;
            }

            foreach (var collection in loanCollections)
            {
                // Collection Date
                DateTime? collectionDate = null;
                if (collection.Contains("CollectionDate"))
                {
                    if (DateTime.TryParse(collection["CollectionDate"].ToString(), out DateTime parsedColDate))
                    {
                        collectionDate = parsedColDate;
                    }
                }
                string collectionDateStr = collectionDate?.ToString("MM/dd/yyyy") ?? "";

                string clientNo = collection.GetValue("ClientNo", "").AsString;
                string name = collection.GetValue("FullName", "").AsString;

                string clientInfo = $"Col. Date: {collectionDateStr}\nClient No.: {clientNo}\nName: {name}";

                // Loan Information
                double loanAmount = totalLoanAmount;
                string amortization = collection.Contains("DailyAmortization")
                    ? collection["DailyAmortization"].ToDecimal().ToString("F2")
                    : "0.00";

                double amountPaid = collection.Contains("AmountPaid")
                    ? (double)collection["AmountPaid"].ToDecimal()
                    : 0.00;
                totalAmountPaid += amountPaid;
                runningBalance = loanAmount - totalAmountPaid;
                string runningBalanceStr = runningBalance <= 0 ? "Settled" : runningBalance.ToString("F2");

                string loanInfo = $"Loan Amount: {loanAmount:F2}\nAmortization: {amortization}\nRunning Balance: {runningBalanceStr}";

                // Payment Information
                string dateReceived = collection.Contains("CollectionDate") && collection["CollectionDate"].IsString
                    ? DateTime.TryParse(collection["CollectionDate"].AsString, out DateTime parsedDate)
                        ? parsedDate.ToString("MM/dd/yyyy")
                        : ""
                    : "";

                string penalty = collection.Contains("CollectedPenalty")
                    ? collection["CollectedPenalty"].ToDecimal().ToString("F2")
                    : "0.00";
                totalPenalty += double.TryParse(penalty, out double penVal) ? penVal : 0.00;

                string paymentInfo = $"Date Received: {dateReceived}\nAmount Paid: {amountPaid:F2}\nPenalty: {penalty}";

                // Collection Info
                string collector = collection.Contains("Collector") ? collection["Collector"].AsString : "";
                string area = collection.Contains("Address") ? collection["Address"].AsString : "";

                string collectionInfo = $"Collector: {collector}\nAddress: {area}";


                // Remarks & Balance
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

                double balanceValue = runningBalance > 0 ? runningBalance : 0;

                // Add to DataTable
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


            // Summary and binding
            ltotalamtpaid.Text = "Total Amount Paid: " + totalAmountPaid.ToString("F2");
            lpenaltytotal.Text = "Generated Penalty: " + totalPenalty.ToString("F2");

            DataView view = _loanCollectionTable.DefaultView;
            view.Sort = "BalanceTemp DESC, CollectionDateTemp ASC";
            DataTable sortedTable = view.ToTable();

            dgvdata.DataSource = sortedTable;
            ltotalpayments.Text = "Total Payment Collection: " + dgvdata.Rows.Count.ToString();

            for (int i = 0; i < dgvdata.Rows.Count; i++)
            {
                var remark = dgvdata.Rows[i].Cells["Remarks"].Value?.ToString() ?? "";
                if (remark.Contains("Excess amount paid"))
                {
                    dgvdata.Rows[i].DefaultCellStyle.BackColor = Color.Khaki;
                }
            }

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
            
            //Console.WriteLine("Account ID: " + _clientno);
            //Console.WriteLine("Client No: " + _loanId);

            LoadLoanCollections();
            LoadLoanNumbersToComboBox();

            var filter = Builders<BsonDocument>.Filter.Eq("ClientNo", laccountid.Text);
            var loanCollections = _loanDisbursedCollection.Find(filter).ToList();

            double totalAmountPaid = 0;
            double totalLoanAmount = 0;
            double excessAmount = 0;

            foreach (var collection in loanCollections)
            {
                // Parse LoanAmount safely
                string rawLoanAmount = collection.GetValue("LoanAmount", "0").ToString().Replace("₱", "").Replace(",", "").Trim();
                double loanAmount = double.TryParse(rawLoanAmount, out double parsedLoan) ? parsedLoan : 0.00;
                totalLoanAmount = loanAmount;

                // Parse ActualCollection safely
                double amountPaid = 0.00;
                if (collection.Contains("ActualCollection"))
                {
                    var rawValue = collection["ActualCollection"];
                    if (rawValue.IsString)
                    {
                        string raw = rawValue.AsString.Replace("₱", "").Replace(",", "").Trim();
                        double.TryParse(raw, out amountPaid);
                    }
                    else if (rawValue.IsDecimal128)
                    {
                        amountPaid = (double)rawValue.AsDecimal128;
                    }
                    else if (rawValue.IsDouble)
                    {
                        amountPaid = rawValue.AsDouble;
                    }
                }

                totalAmountPaid += amountPaid;
                excessAmount = totalAmountPaid - totalLoanAmount;
            }

            if (excessAmount > 0)
            {
                MessageBox.Show($"This loan account has an overpayment of ₱{excessAmount:F2}. Please review the payment history for adjustments.",
                                "Overpayment Detected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                bnew.Enabled = true;
            }
        }


        private void LoadLoanNumbersToComboBox()
        {
            // Clear and add the placeholder first
            cbloanno.Items.Clear();
            cbloanno.Items.Add("--select loan cycle no--");
            cbloanno.SelectedIndex = 0;
            cbloanno.Text = "--select loan cycle no--";

            bnew.Enabled = false;
            bpayadvance.Enabled = false;

            string clientNo = laccountid.Text.Trim();

            if (string.IsNullOrEmpty(clientNo))
            {
                MessageBox.Show("ClientNo is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Step 1: Find the AccountId from loan_disbursed using ClientNo
            var clientFilter = Builders<BsonDocument>.Filter.Eq("ClientNo", clientNo);
            var loanDisbursedDoc = _loanDisbursedCollection.Find(clientFilter).FirstOrDefault();

            if (loanDisbursedDoc == null || !loanDisbursedDoc.Contains("AccountId"))
            {
                MessageBox.Show("No loan disbursed record found for this ClientNo.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string accountId = loanDisbursedDoc["AccountId"].AsString.Trim();

            HashSet<string> loanNumbers = new HashSet<string>();

            // Step 2: Query loan_account_cycles by AccountId
            var cycleFilter = Builders<BsonDocument>.Filter.Eq("AccountId", accountId);
            var cycleLoans = _loanAccountCyclesCollection.Find(cycleFilter).ToList();

            foreach (var loan in cycleLoans)
            {
                if (loan.TryGetValue("LoanNo", out BsonValue loanNoVal))
                {
                    string loanNo = loanNoVal.AsString.Trim();
                    if (!string.IsNullOrEmpty(loanNo)) loanNumbers.Add(loanNo);
                }
            }

            // Step 3: Query loan_disbursed by AccountId (same AccountId found)
            var disbursedFilter = Builders<BsonDocument>.Filter.Eq("AccountId", accountId);
            var disbursedLoans = _loanDisbursedCollection.Find(disbursedFilter).ToList();

            foreach (var loan in disbursedLoans)
            {
                if (loan.TryGetValue("LoanNo", out BsonValue loanNoVal))
                {
                    string loanNo = loanNoVal.AsString.Trim();
                    if (!string.IsNullOrEmpty(loanNo)) loanNumbers.Add(loanNo);
                }
            }

            if (loanNumbers.Count > 0)
            {
                foreach (string loanNo in loanNumbers.OrderBy(n => n))
                {
                    cbloanno.Items.Add(loanNo);
                }
            }
            else
            {
                MessageBox.Show("No loan cycles or disbursed loans found for this client.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


        double ParseCurrencyString(BsonValue val)
        {
            if (val == null || val.IsBsonNull) return 0.0;

            string raw = val.AsString;
            if (string.IsNullOrWhiteSpace(raw)) return 0.0;

            // Remove currency symbols and commas
            string cleaned = raw.Replace("₱", "").Replace(",", "").Trim();

            if (double.TryParse(cleaned, out double result))
                return result;

            return 0.0;
        }


        private void bnew_Click(object sender, EventArgs e)
        {
            if (cbloanno.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select a valid Loan Number.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string clientNo = laccountid.Text.Trim();
            string loanNo = cbloanno.SelectedItem?.ToString() ?? "";

            if (string.IsNullOrEmpty(clientNo) || string.IsNullOrEmpty(loanNo) || loanNo == "--select loan cycle no--")
            {
                MessageBox.Show("Please select a valid client and loan number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Search in both collections
            var filter = Builders<BsonDocument>.Filter.Eq("LoanNo", loanNo);
            var loanDisbursedDoc = _loanDisbursedCollection.Find(filter).FirstOrDefault();
            var loanCycleDoc = _loanAccountCyclesCollection.Find(filter).FirstOrDefault();

            // Check overpayment using whichever has ActualCollection
            double totalAmountPaid = 0;
            double totalLoanAmount = 0;

            if (loanDisbursedDoc != null)
            {
                double loanAmount = loanDisbursedDoc.Contains("LoanAmount") ? ParseCurrencyString(loanDisbursedDoc["LoanAmount"]) : 0.00;
                totalLoanAmount = loanAmount;

                double amountPaid = loanDisbursedDoc.Contains("ActualCollection") ? ParseCurrencyString(loanDisbursedDoc["ActualCollection"]) : 0.00;
                totalAmountPaid += amountPaid;
            }

            // Optional: If your loanCycleDoc also contains payments in future, include here

            double excessAmount = totalAmountPaid - totalLoanAmount;

            if (excessAmount > 0)
            {
                MessageBox.Show($"This loan account has an overpayment of ₱{excessAmount:F2}. Please review the payment history for adjustments.",
                                "Overpayment Detected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            frm_home_disburse_collections_add addCollectionForm = new frm_home_disburse_collections_add(clientNo, loanNo);
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
            string clientNo = laccountid.Text;
            frm_home_disburse_collections_addex addex = new frm_home_disburse_collections_addex(clientNo);
            addex.ShowDialog();
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

        private void cbloanno_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbloanno.SelectedIndex <= 0) // index 0 is "--select loan cycle no--"
            {
                dgvdata.DataSource = null;
                bnew.Enabled = false;
                bpayadvance.Enabled = false;
                return;
            }



            if (cbloanno.SelectedItem == null) return;


            string selectedLoanNo = cbloanno.Text.Trim();
            if (string.IsNullOrEmpty(selectedLoanNo)) return;

            var filter = Builders<BsonDocument>.Filter.Eq("LoanNo", selectedLoanNo);

            // Try in loan_account_cycles first
            var loanDoc = _loanAccountCyclesCollection.Find(filter).FirstOrDefault();

            // Fallback to loan_disbursed
            if (loanDoc == null)
            {
                loanDoc = _loanDisbursedCollection.Find(filter).FirstOrDefault();
            }

            if (loanDoc != null)
            {
                // Optional: Check for ClientNo or AccountId presence
                bool hasClientNo = loanDoc.Contains("ClientNo") && loanDoc["ClientNo"].IsString;
                bool hasAccountId = loanDoc.Contains("AccountId") && loanDoc["AccountId"].IsString;

                if (hasClientNo || hasAccountId)
                {
                    LoadLoanCollections();  // <-- NO parameter here

                    bnew.Enabled = true;
                    bpayadvance.Enabled = true;
                    return;
                }
            }

            // Loan not found or invalid
            MessageBox.Show("Loan record not found or invalid loan data.", "No Match", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            dgvdata.DataSource = null;
            bnew.Enabled = false;
            bpayadvance.Enabled = false;
        }

    }
}

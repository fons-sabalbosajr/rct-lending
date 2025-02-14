using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rct_lmis.DISBURSEMENT_SECTION
{
    public partial class frm_home_disburse_loan_config : Form
    {
        private IMongoCollection<BsonDocument> _loanDisbursedCollection;
        private IMongoCollection<BsonDocument> _loanCollectionCollection;
        private IMongoCollection<BsonDocument> _loanCollectionsRenewCollection;
        private IMongoCollection<BsonDocument> _loanCollectionsRenewDataCollection;

        private DataTable _loanCollectionTable;

        public frm_home_disburse_loan_config(string clientNo, string loanId)
        {
            InitializeComponent();
            lclientno.Text = clientNo;
            lclientnoadd.Text = clientNo;

            var database = MongoDBConnection.Instance.Database;
            _loanDisbursedCollection = database.GetCollection<BsonDocument>("loan_disbursed");
            _loanCollectionCollection = database.GetCollection<BsonDocument>("loan_collections");
            _loanCollectionsRenewCollection = database.GetCollection<BsonDocument>("loan_collections_renew");
            _loanCollectionsRenewDataCollection = database.GetCollection<BsonDocument>("loan_collections_renew_data");

            _loanCollectionTable = new DataTable();

            btransfercol.Enabled = false;
        }

        private void LoadLoanDetails()
        {
            // Clear previous data
            dgvloancurrent.DataSource = null;
            dgvloancurrent.Rows.Clear();

            // Filter data by ClientNo
            var filter = Builders<BsonDocument>.Filter.Eq("ClientNo", lclientno.Text);
            var loanDetails = _loanDisbursedCollection.Find(filter).ToList();

            // Create a DataTable and define the columns
            DataTable loanTable = new DataTable();
            loanTable.Columns.Add("Loan ID", typeof(string));
            loanTable.Columns.Add("Client Name", typeof(string));
            loanTable.Columns.Add("Loan Amt & Bal", typeof(string));
            loanTable.Columns.Add("Start & Maturity Date", typeof(string));

            // Iterate through the loan data and populate the DataTable
            foreach (var loan in loanDetails)
            {
                string loanNo = loan.Contains("LoanNo") ? loan["LoanNo"].AsString : "N/A";  // Ensure LoanNo is fetched properly
                string clientName = $"{loan.GetValue("LastName", "").AsString}, {loan.GetValue("FirstName", "").AsString} {loan.GetValue("MiddleName", "").AsString}".Trim();

                string loanAmount = loan.Contains("LoanAmount") ? loan["LoanAmount"].AsString : "₱0.00";
                string loanBalance = loan.Contains("LoanBalance") ? loan["LoanBalance"].ToString() : "₱0.00";
                string loanDetailsStr = $"Loan Amt: {loanAmount}\nBal.: {loanBalance}";

                string startPayment = loan.Contains("StartPaymentDate") ? loan["StartPaymentDate"].AsString : "N/A";
                string maturityDate = loan.Contains("MaturityDate") ? loan["MaturityDate"].AsString : "N/A";
                string dateDetails = $"Start: {startPayment}\nMaturity: {maturityDate}";

                // Add row to the DataTable
                loanTable.Rows.Add(loanNo, clientName, loanDetailsStr, dateDetails);
            }

            // Bind the DataTable to the DataGridView
            dgvloancurrent.DataSource = loanTable;

            // Format the DataGridView for multiline text in cells
            dgvloancurrent.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvloancurrent.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Clear selection upon load
            dgvloancurrent.ClearSelection();
        }

        public void LoadLoanCollections()
        {
            // Ensure the _loanCollectionTable is initialized with the correct columns
            if (_loanCollectionTable.Columns.Count == 0)
            {
                // Add the checkbox column first
                DataGridViewCheckBoxColumn checkboxColumn = new DataGridViewCheckBoxColumn();
                checkboxColumn.Name = "Select";
                checkboxColumn.HeaderText = "Select";
                checkboxColumn.Width = 25; // Adjust the width as needed
                checkboxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; // Allow manual resizing
                dgvcolcurrent.Columns.Add(checkboxColumn);

                // Add other columns after the checkbox column
                _loanCollectionTable.Columns.Add("Col. Date", typeof(string));
                _loanCollectionTable.Columns.Add("Amortization", typeof(string));
                _loanCollectionTable.Columns.Add("Payment", typeof(string));
                _loanCollectionTable.Columns.Add("Collector", typeof(string));
                _loanCollectionTable.Columns.Add("Remarks", typeof(string));
                _loanCollectionTable.Columns.Add("CollectionDateTemp", typeof(DateTime)); // Add temporary column for sorting
                _loanCollectionTable.Columns.Add("BalanceTemp", typeof(double)); // Add column for sorting by balance
            }

            // Clear existing rows before loading new data
            _loanCollectionTable.Rows.Clear();
            dgvcolcurrent.DataSource = null; // Clear existing data source

            double totalAmountPaid = 0;
            double totalLoanAmount = 0;
            double runningBalance = 0;  // Initialize running balance for calculations
            double totalPenalty = 0; // Initialize total penalty calculation

            // Filter by ClientNo (assuming lclientno.Text holds the ClientNo)
            var filter = Builders<BsonDocument>.Filter.Eq("ClientNo", lclientno.Text);
            var loanCollections = _loanCollectionCollection.Find(filter).ToList();  // Assuming _loanCollectionCollection is your MongoDB collection for loan collections

            _loanCollectionTable.Rows.Clear();

            foreach (var collection in loanCollections)
            {
                // Client Information
                DateTime? collectionDate = collection.Contains("CollectionDate") ? collection["CollectionDate"].ToUniversalTime() : (DateTime?)null;
                string collectionDateStr = collectionDate.HasValue ? collectionDate.Value.ToString("MM/dd/yyyy") : "";

                string clientNo = collection.Contains("ClientNo") ? collection["ClientNo"].AsString : "";
                string name = collection.Contains("Name") ? collection["Name"].AsString : "";

                string clientInfo = $"Col. Date: {collectionDateStr}";

                // Loan Information
                double loanAmount = collection.Contains("LoanAmount") ? (double)collection["LoanAmount"].ToDouble() : 0.00;
                totalLoanAmount = loanAmount;  // Store loan amount to compute general balance later

                string amortization = collection.Contains("Amortization") ? ((double)collection["Amortization"].AsDecimal128).ToString("F2") : "0.00";

                // Calculate Running Balance as Loan Amount - Total Amount Paid
                double amountPaid = collection.Contains("ActualCollection") ? (double)collection["ActualCollection"].AsDecimal128 : 0.00;
                totalAmountPaid += amountPaid; // Accumulate total amount paid
                runningBalance = loanAmount - totalAmountPaid; // Running balance: Loan Amount - Total Amount Paid

                string runningBalanceStr = runningBalance <= 0 ? "Settled" : runningBalance.ToString("F2"); // Check if balance is zero or negative

                string loanInfo = $"Amort.: {amortization}\n";

                // Payment Information
                string dateReceived = collection.Contains("DateReceived") ? collection["DateReceived"].ToUniversalTime().ToString("MM/dd/yyyy") : "";

                string penalty = collection.Contains("CollectedPenalty") ? ((double)collection["CollectedPenalty"].AsDecimal128).ToString("F2") : "";
                totalPenalty += string.IsNullOrEmpty(penalty) ? 0.00 : Convert.ToDouble(penalty); // Add to total penalty if present

                string paymentMode = collection.Contains("PaymentMode") ? collection["PaymentMode"].AsString : "";

                string paymentInfo = $"Amt. Paid: {amountPaid:F2}\n" +
                                     $"Mode: {paymentMode}\n" +
                                     $"Penalty: {penalty}";

                // Collection Information
                string collector = collection.Contains("Collector") ? collection["Collector"].AsString : "";
                string area = collection.Contains("Address") ? collection["Address"].AsString : "";

                string collectionInfo = $"Collector: {collector}";

                // Remarks: Always Include Excess Amount Paid, Even if Settled
                string remarks = "";
                double excessAmount = totalAmountPaid - totalLoanAmount; // Calculate excess amount paid over total loan amount

                if (runningBalance <= 0)
                {
                    if (excessAmount > 0)
                    {
                        remarks = $"Loan settled.\nExcess amount: {excessAmount:F2}";
                        lgenbal.Text = $"Excess payment: {excessAmount:F2}";
                    }
                    else
                    {
                        remarks = "Loan settled";
                        lgenbal.Text = "Balance: 0.00";
                    }
                }
                else
                {
                    remarks = $"Bal.: {runningBalance:F2}";
                    lgenbal.Text = $"Bal.: {runningBalance:F2}";
                    if (excessAmount > 0)
                    {
                        remarks += $"\nExcess Amount: {excessAmount:F2}";
                        lgenbal.Text += $"\nExcess Amount: {excessAmount:F2}";
                    }
                }

                // Extract balance value for sorting
                double balanceValue = runningBalance > 0 ? runningBalance : 0;

                // Add data to DataTable, including CollectionDateTemp and BalanceTemp for sorting
                DataRow row = _loanCollectionTable.NewRow();
                row["Col. Date"] = clientInfo;
                row["Amortization"] = loanInfo;
                row["Payment"] = paymentInfo;
                row["Collector"] = collectionInfo;
                row["Remarks"] = remarks;
                row["CollectionDateTemp"] = collectionDate.HasValue ? (object)collectionDate.Value : DBNull.Value;
                row["BalanceTemp"] = balanceValue;

                _loanCollectionTable.Rows.Add(row);
            }

            // Total Amount Paid Calculation
            ltotalamtpaid.Text = "Total Paid: " + totalAmountPaid.ToString("F2");

            // Penalty Calculation (If no penalties, display 0.00)
            lpenaltytotal.Text = "Penalty: " + totalPenalty.ToString("F2");

            // Sort DataTable by BalanceTemp in descending order, then CollectionDateTemp in ascending order
            DataView view = _loanCollectionTable.DefaultView;
            view.Sort = "BalanceTemp DESC, CollectionDateTemp ASC";
            DataTable sortedTable = view.ToTable();
            dgvcolcurrent.DataSource = sortedTable;

            // Total Payments Text (Count total rows in dgvdata)
            ltotalpayments.Text = "Payments No. Total: " + dgvcolcurrent.Rows.Count.ToString();

            // Highlight rows with excess payment after binding
            for (int i = 0; i < dgvcolcurrent.Rows.Count; i++)
            {
                var remarks = dgvcolcurrent.Rows[i].Cells["Remarks"].Value.ToString();
                if (remarks.Contains("Excess amount"))
                {
                    dgvcolcurrent.Rows[i].DefaultCellStyle.BackColor = Color.Khaki;
                }
            }

            // Hide the temporary sorting columns
            dgvcolcurrent.Columns["BalanceTemp"].Visible = false;
            dgvcolcurrent.Columns["CollectionDateTemp"].Visible = false;

            dgvcolcurrent.Columns["Select"].Width = 40; // Adjust the width of the checkbox column as needed
            dgvcolcurrent.Columns["Select"].Resizable = DataGridViewTriState.False;

            dgvcolcurrent.ClearSelection();
        }

        private void SelectExcessPayments()
        {
            // Iterate through all rows in the DataGridView
            foreach (DataGridViewRow row in dgvcolcurrent.Rows)
            {
                // Check if the remarks column contains "Excess amount paid"
                var remarks = row.Cells["Remarks"].Value.ToString();
                bool isExcess = remarks.Contains("Excess amount");

                // Set the "Select" checkbox based on whether it's an excess payment
                row.Cells["Select"].Value = isExcess && challexcess.Checked;
            }
        }


        private void LoadLoanDetailsFromDatabase(string loanNo)
        {
            if (string.IsNullOrEmpty(loanNo)) return;

            var filter = Builders<BsonDocument>.Filter.Eq("LoanNo", loanNo);
            var loanData = _loanDisbursedCollection.Find(filter).FirstOrDefault();

            if (loanData != null)
            {
                // Populate textboxes with MongoDB values
                tloannocurrent.Text = loanData.Contains("LoanNo") ? loanData["LoanNo"].AsString : "";
                tnamecurrent.Text = loanData.Contains("FirstName") && loanData.Contains("LastName")
                    ? $"{loanData["FirstName"].AsString} {loanData["LastName"].AsString}"
                    : "";

                tloanamtcurrent.Text = loanData.Contains("LoanAmount") ? loanData["LoanAmount"].AsString : "₱0.00";
                tbalcurrent.Text = loanData.Contains("LoanBalance") ? loanData["LoanBalance"].AsString : "₱0.00";

                cbstatuscurrent.Text = loanData.Contains("LoanStatus") ? loanData["LoanStatus"].AsString : "N/A";
                rtremarkscurrent.Text = loanData.Contains("LoanProcessStatus") ? loanData["LoanProcessStatus"].AsString : "";

                // Auto-select Collection Status based on Current Balance
                double currentBalance = 0;
                string currentBalanceStr = tbalcurrent.Text.Replace("₱", "").Trim(); // Remove ₱ symbol

                if (double.TryParse(currentBalanceStr, out currentBalance))
                {
                    double loanAmount = 0;
                    string loanAmountStr = tloanamtcurrent.Text.Replace("₱", "").Trim();

                    // Check if loanAmount can be parsed
                    if (double.TryParse(loanAmountStr, out loanAmount))
                    {
                        if (currentBalance == 0)
                            cbcolstatus.SelectedItem = "Settled"; // No balance, loan is fully paid
                        else if (currentBalance < 0)
                            cbcolstatus.SelectedItem = "Overpaid"; // Negative balance, overpayment
                        else if (currentBalance > 0 && currentBalance < loanAmount)
                            cbcolstatus.SelectedItem = "Not yet Settled"; // Partial balance remaining
                        else
                            cbcolstatus.SelectedItem = "Not yet Settled"; // Balance still due
                    }
                    else
                    {
                        // Default status when LoanAmount is not parsable
                        cbcolstatus.SelectedItem = "Not yet settled";
                    }
                }
                else
                {
                    cbcolstatus.SelectedItem = "Not yet settled"; // Default if parsing fails
                }
            }
            else
            {
                MessageBox.Show("Loan details not found in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Helper function to remove non-numeric characters (keeps only digits and decimal point)
        private string RemoveNonNumericChars(string input)
        {
            // Remove all non-digit and non-decimal characters
            string cleanInput = new string(input.Where(c => char.IsDigit(c) || c == '.').ToArray());

            // If the cleaned input starts with a dot, remove it (invalid format)
            if (cleanInput.StartsWith("."))
            {
                cleanInput = cleanInput.Substring(1); // Remove the leading dot
            }

            return cleanInput;
        }

        private void GenerateLoanAccountID()
        {
            // Extract the Loan ID from the selected row in dgvloancurrent
            if (dgvloancurrent.SelectedRows.Count > 0)
            {
                string baseAccountID = dgvloancurrent.SelectedRows[0].Cells["Loan ID"].Value.ToString(); // Get the Loan ID from the selected row

                // Check if the base account ID already contains a "-R" (renewal version)
                string pattern = @"-R(\d+)$"; // Regex pattern to find the renewal part, e.g., "-R1", "-R2", etc.
                var match = Regex.Match(baseAccountID, pattern);

                if (match.Success)
                {
                    // Extract the renewal version number from the match (e.g., "1" from "-R1")
                    int renewalNumber = int.Parse(match.Groups[1].Value);

                    // Increment the renewal number by 1 for the next renewal
                    renewalNumber++;

                    // Update the new account ID with the incremented renewal number
                    tloannoadd.Text = $"{baseAccountID}-R{renewalNumber}";
                }
                else
                {
                    // If no renewal version exists, add the first renewal as "-R1"
                    tloannoadd.Text = $"{baseAccountID}-R1";
                }
            }
            else
            {
                // Handle case when no row is selected
                MessageBox.Show("Please select a loan from the list to generate an account ID.");
            }
        }

        private void ClearLoanAddFields()
        {
            //tloannoadd.Clear();
            tnameadd.Clear();
            tloanamountadd.Clear();
            tbaladd.Clear();
            rtremarksadd.Clear();
            cbstatusadd.SelectedIndex = -1;
            cbbalstatusadd.SelectedIndex = -1;
            dtstart.Value = DateTime.Now;
            dtmaturity.Value = DateTime.Now;
        }

        private void SaveLoanCollectionData()
        {
            // 1. Retrieve LoanNo from tloannoadd.Text (LoanNo is already in the first column of dgvloanadd)
            string loanNo = tloannoadd.Text.Trim();

            // 2. Gather relevant values from the dgvloanadd (first row of the grid)
            // Assuming the row has been added successfully to dgvloanadd
            if (dgvloanadd.Rows.Count > 0)
            {
                // Retrieve the first row (you can iterate through rows if you need to handle multiple rows)
                DataGridViewRow row = dgvloanadd.Rows[0];  // or use your specific logic to select the correct row

                string clientName = row.Cells["Client Name"].Value.ToString();
                string loanAmtBal = row.Cells["Loan Amt & Bal"].Value.ToString();
                string startMaturityDate = row.Cells["Start & Maturity Date"].Value.ToString();
                string remarks = row.Cells["Remarks"].Value.ToString();

                // 3. Create a new collection document for loan_collections_renew
                var loanCollectionRenewData = new BsonDocument
                {
                    { "LoanNo", loanNo },
                    { "ClientNo", lclientnoadd.Text },
                    { "LoanType", "RENEWAL" }, // As per the given sample data
                    { "LoanStatus", "UPDATED" }, // As per the given sample data
                    { "ClientName", clientName },
                    { "LoanAmount", loanAmtBal },
                    { "StartPaymentDate", dtstart.Value.ToString("MM/dd/yyyy") },
                    { "MaturityDate", dtmaturity.Value.ToString("MM/dd/yyyy") },
                    { "Remarks", remarks },
                    { "DateModified", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") }
                };

                // 4. Save the loan collection data into the "loan_collections_renew" collection
                try
                {
                    // Insert the loan collection data (main document) into loan_collections_renew
                    _loanCollectionsRenewCollection.InsertOne(loanCollectionRenewData);

                    // 5. Loop through dgvcoladd to save collection data for each row
                    List<BsonDocument> collectionDetails = new List<BsonDocument>();

                    foreach (DataGridViewRow collectionRow in dgvcoladd.Rows)
                    {
                        if (collectionRow.Cells["Col. Date"].Value != null)
                        {
                            string colDate = collectionRow.Cells["Col. Date"].Value.ToString();
                            string amortization = collectionRow.Cells["Amortization"].Value.ToString();
                            string payment = collectionRow.Cells["Payment"].Value.ToString();
                            string collector = collectionRow.Cells["Collector"].Value.ToString();
                            string remark = collectionRow.Cells["Remarks"].Value.ToString();

                            // Convert payment to numeric value
                            double paymentValue = 0.00;
                            double.TryParse(payment.Replace("₱", "").Trim(), out paymentValue);

                            // Create a BsonDocument for each collection row for loan_collections_renew_data
                            var collectionDoc = new BsonDocument
                            {
                                { "LoanNo", loanNo },
                                { "ClientNo", lclientnoadd.Text },
                                { "CollectionDate", colDate },
                                { "Amortization", amortization },
                                { "Payment", payment },
                                { "Collector", collector },
                                { "Remarks", remark },
                                { "PaymentValue", paymentValue }
                            };

                            collectionDetails.Add(collectionDoc);  // Add the collection document to the list
                        }
                    }

                    // Insert the collection details into the "loan_collections_renew_data" collection
                    if (collectionDetails.Count > 0)
                    {
                        _loanCollectionsRenewDataCollection.InsertMany(collectionDetails);
                    }

                    // Notify user that the data was saved successfully
                    MessageBox.Show("Loan Collection Data saved successfully!");
                }
                catch (Exception ex)
                {
                    // Handle errors
                    MessageBox.Show($"Error saving loan collection data: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("No loan details found in the loan grid.");
            }
        }




        private void frm_home_disburse_loan_config_Load(object sender, EventArgs e)
        {
            LoadLoanDetails();
            LoadLoanCollections();
        }

        private void dgvloancurrent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the row index is valid
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvloancurrent.Rows[e.RowIndex];

                // Ensure the column exists before accessing it
                if (dgvloancurrent.Columns.Contains("Loan ID")) // Updated to match your column name
                {
                    // Get the selected Loan ID from the clicked row
                    string selectedLoanNo = row.Cells["Loan ID"].Value?.ToString() ?? ""; // Match column name "Loan ID"

                    // Load the loan details from the database based on the selected loan number
                    LoadLoanDetailsFromDatabase(selectedLoanNo);
                }
                else
                {
                    MessageBox.Show("Error: 'Loan ID' column not found.", "Column Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void bupdatecurrent_Click(object sender, EventArgs e)
        {
            // Validate required fields (LoanNo should not be empty)
            if (string.IsNullOrEmpty(tloannocurrent.Text))
            {
                MessageBox.Show("Please select a loan to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Get updated values from the UI
            string loanNo = tloannocurrent.Text;
            string clientName = tnamecurrent.Text;
            string loanAmountStr = tloanamtcurrent.Text.Replace("₱", "").Trim();
            string balanceStr = tbalcurrent.Text.Replace("₱", "").Trim();
            string loanStatus = cbstatuscurrent.Text;
            string loanRemarks = rtremarkscurrent.Text;
            string collectionStatus = cbcolstatus.SelectedItem.ToString();

            // Convert loanAmount and balance to double
            double loanAmount = 0, balance = 0;
            double.TryParse(loanAmountStr, out loanAmount);
            double.TryParse(balanceStr, out balance);

            // Prepare the updated loan document
            var updateDefinition = Builders<BsonDocument>.Update
                .Set("LoanAmount", loanAmount)
                .Set("LoanBalance", balance)
                .Set("LoanStatus", loanStatus)
                .Set("LoanProcessStatus", loanRemarks)
                .Set("LoanCollectionStatus", collectionStatus)
                .Set("LastModified", DateTime.Now);

            // Filter to find the loan based on LoanNo
            var filter = Builders<BsonDocument>.Filter.Eq("LoanNo", loanNo);

            try
            {
                // Update the document in the database
                var result = _loanDisbursedCollection.UpdateOne(filter, updateDefinition);

                // Check if the update was successful
                if (result.ModifiedCount > 0)
                {
                    MessageBox.Show("Loan details updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Optionally, refresh the data grid view or UI after updating
                    LoadLoanDetails(); // Reload loan details
                }
                else
                {
                    MessageBox.Show("No loan details were updated. Please check if the LoanNo exists.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating the loan details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btransfercol_Click(object sender, EventArgs e)
        {
            // Ensure dgvcoladd has the necessary columns
            if (dgvcoladd.Columns.Count == 0)
            {
                dgvcoladd.Columns.Add("Col. Date", "Col. Date");
                dgvcoladd.Columns.Add("Amortization", "Amortization");
                dgvcoladd.Columns.Add("Payment", "Payment");
                dgvcoladd.Columns.Add("Collector", "Collector");
                dgvcoladd.Columns.Add("Remarks", "Remarks");
            }

            GenerateLoanAccountID();

            // Initialize totals
            double totalAmountPaidAdd = 0;
            double totalPenaltyAdd = 0;
            double totalLoanAmountAdd = 0;
            double runningBalanceAdd = 0;

            // Convert ltotalamtpaidadd.Text to a numeric value
            double totalPaidValue = 0.00;
            double.TryParse(ltotalamtpaidadd.Text.Replace("₱", "").Trim(), out totalPaidValue);

            // Count how many rows will be transferred
            int rowsToTransfer = dgvcolcurrent.Rows.Cast<DataGridViewRow>().Count(row => Convert.ToBoolean(row.Cells["Select"].Value));

            // Show the progress form
            frm_progress_popup progressForm = new frm_progress_popup();
            progressForm.Show(); // This will show the form and keep it visible during the process

            // Initialize progress bar
            progressForm.SetProgress(0, rowsToTransfer);

            int rowIndex = 0;

            foreach (DataGridViewRow row in dgvcolcurrent.Rows)
            {
                if (Convert.ToBoolean(row.Cells["Select"].Value))  // Assuming there's a checkbox column named "Select"
                {
                    // Get the values from dgvcolcurrent
                    string clientInfo = row.Cells["Col. Date"].Value.ToString();
                    string amortization = row.Cells["Amortization"].Value.ToString();
                    string payment = row.Cells["Payment"].Value.ToString();
                    string collector = row.Cells["Collector"].Value.ToString();
                    string remarks = row.Cells["Remarks"].Value.ToString();

                    // Convert Payment column value to a numeric value
                    double paymentValue = 0.00;
                    double.TryParse(payment.Replace("₱", "").Trim(), out paymentValue);

                    // Calculate the excess amount
                    double excessAmount = totalPaidValue - paymentValue;

                    // Modify the Remarks column value based on excess amount
                    if (excessAmount > 0)
                    {
                        remarks = $"Loan settled. Excess amount: ₱{excessAmount:F2}";
                    }

                    // Add the data to dgvcoladd with updated Remarks
                    dgvcoladd.Rows.Add(clientInfo, amortization, payment, collector, remarks);

                    // Clean the Payment and Remarks to extract numeric values
                    double amountPaid = 0.00;
                    payment = RemoveNonNumericChars(payment); // Remove non-numeric characters
                    if (double.TryParse(payment, out amountPaid))
                    {
                        totalAmountPaidAdd += amountPaid; // Add to total amount paid
                    }
                    else
                    {
                        Console.WriteLine($"Invalid payment value: {payment}"); // Log invalid payment
                    }

                    // Handle Penalty safely (assumed to be in Remarks column or another specific column)
                    double penalty = 0.00;
                    if (double.TryParse(remarks, out penalty)) // Adjust if penalty is in another column
                    {
                        totalPenaltyAdd += penalty; // Add to total penalty
                    }

                    // Handle Loan Amount (Amortization)
                    double loanAmount = 0.00;
                    if (double.TryParse(amortization, out loanAmount)) // Adjust based on your column for loan amount
                    {
                        totalLoanAmountAdd += loanAmount; // Add to total loan amount
                    }

                    // Calculate running balance
                    double runningBalance = amountPaid - loanAmount;
                    runningBalanceAdd += runningBalance; // Add to running balance

                    // Update the progress bar
                    rowIndex++;
                    progressForm.SetProgress(rowIndex, rowsToTransfer);  // Update the progress bar

                    // Optional: Add a small delay to make the progress smoother (you can remove it if unnecessary)
                    System.Threading.Thread.Sleep(10);
                }
            }

            // Update the totals in the respective labels for dgvcoladd
            ltotalpaymentsadd.Text = "Payments No. Total: " + dgvcoladd.Rows.Count.ToString();
            ltotalamtpaidadd.Text = "Total Paid: " + totalAmountPaidAdd.ToString("F2");
            lgenbaladd.Text = "Balance: " + runningBalanceAdd.ToString("F2");
            lpenaltytotaladd.Text = "Penalty: " + totalPenaltyAdd.ToString("F2");

            // Close the progress form after the process is completed
            progressForm.Close();
        }

        private void challexcess_CheckedChanged(object sender, EventArgs e)
        {
            SelectExcessPayments();
            btransfercol.Enabled = true;
        }

        private void bupdatecol_Click(object sender, EventArgs e)
        {
            // Get the selected rows from the DataGridView (assuming the "Select" checkbox is used for selection)
            foreach (DataGridViewRow row in dgvcolcurrent.SelectedRows)
            {
                // Check if the "Select" checkbox is checked
                bool isSelected = Convert.ToBoolean(row.Cells["Select"].Value);
                if (isSelected)
                {
                    // Get the data from the selected row
                    string clientInfo = row.Cells["Col. Date"].Value.ToString();
                    string amortization = row.Cells["Amortization"].Value.ToString();
                    string payment = row.Cells["Payment"].Value.ToString();
                    string collector = row.Cells["Collector"].Value.ToString();
                    string remarks = row.Cells["Remarks"].Value.ToString();

                    // Parse the required values (you can adjust how you parse them based on your data structure)
                    double amountPaid = 0.00;
                    if (double.TryParse(payment, out amountPaid)) // Payment field
                    {
                        // Update the corresponding MongoDB record
                        string clientNo = lclientno.Text; // Get ClientNo from UI (or wherever it's stored)

                        // Filter by ClientNo and matching information to find the correct record
                        var filter = Builders<BsonDocument>.Filter.Eq("ClientNo", clientNo) &
                                     Builders<BsonDocument>.Filter.Eq("Col. Date", clientInfo);
                        var update = Builders<BsonDocument>.Update
                            .Set("ActualCollection", amountPaid) // Assuming ActualCollection holds the payment amount
                            .Set("Amortization", amortization) // Update amortization (if needed)
                            .Set("Collector", collector) // Update the collector info
                            .Set("Remarks", remarks); // Update remarks

                        // Execute the update on the MongoDB collection
                        var result = _loanCollectionCollection.UpdateOne(filter, update);

                        if (result.ModifiedCount > 0)
                        {
                            MessageBox.Show("Record updated successfully.");
                        }
                        else
                        {
                            MessageBox.Show("No records were updated.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid payment value.");
                    }
                }
            }

            // Refresh the DataGridView with the latest data from MongoDB
            LoadLoanCollections();
        }

        private void badd_Click(object sender, EventArgs e)
        {
            // Ensure dgvloanadd has the necessary columns
            if (dgvloanadd.Columns.Count == 0)
            {
                dgvloanadd.Columns.Add("Loan ID", "Loan ID");
                dgvloanadd.Columns.Add("Client Name", "Client Name");
                dgvloanadd.Columns.Add("Loan Amt & Bal", "Loan Amt & Bal");
                dgvloanadd.Columns.Add("Start & Maturity Date", "Start & Maturity Date");
                dgvloanadd.Columns.Add("Remarks", "Remarks");
            }

            // Retrieve values from input fields
            string loanID = tloannoadd.Text.Trim();
            string clientName = tnameadd.Text.Trim();

            // Group Loan Amount and Balance
            string loanAmtBal = $"Loan Amt: {tloanamountadd.Text.Trim()}\nBal.: {tbaladd.Text.Trim()}";

            // Group Start Payment Date and Maturity Date
            string startMaturityDate = $"Start: {dtstart.Value.ToShortDateString()}\nMaturity: {dtmaturity.Value.ToShortDateString()}";

            // Get Remarks (including Status and Balance Status)
            string remarks = $"{rtremarksadd.Text.Trim()}\n\nStatus: {cbstatusadd.Text.Trim()}\nBalance Status: {cbbalstatusadd.Text.Trim()}";

            // Add the data as a new row in dgvloanadd
            dgvloanadd.Rows.Add(loanID, clientName, loanAmtBal, startMaturityDate, remarks);

            // Clear input fields after adding the row (optional)
            ClearLoanAddFields();
            dgvloanadd.ClearSelection();
        }

        private void bdeleteadd_Click(object sender, EventArgs e)
        {
            if (dgvloanadd.SelectedRows.Count > 0) // Check if a row is selected
            {
                foreach (DataGridViewRow row in dgvloanadd.SelectedRows)
                {
                    if (!row.IsNewRow) // Ensure it's not a new empty row
                    {
                        dgvloanadd.Rows.Remove(row); // Remove selected row
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete.", "Delete Row", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void bsaveadd_Click(object sender, EventArgs e)
        {
            // Show a confirmation dialog before saving the loan collection data
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to save the loan collection data?",
                                                        "Confirm Save",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Question);

            // If user clicks 'Yes', proceed with saving
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    // Call the SaveLoanCollectionData function to save the data
                    SaveLoanCollectionData();
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur during the save process
                    MessageBox.Show($"Error: {ex.Message}", "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // If user clicks 'No', do nothing (save is canceled)
                MessageBox.Show("Save operation canceled.", "Cancel", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void bclear_Click(object sender, EventArgs e)
        {
            // Clear all rows in dgvcoladd
            dgvcoladd.Rows.Clear();

            // Reset total values to zero
            ltotalpaymentsadd.Text = "Payments No. Total: 0";
            ltotalamtpaidadd.Text = "Total Paid: 0.00";
            lgenbaladd.Text = "Balance: 0.00";
            lpenaltytotaladd.Text = "Penalty: 0.00";
        }

        // Function to update Excess Amount for other rows
        private void dgvcoladd_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the changed cell is in the "Payment" column (where "Amt. Paid" is located)
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvcoladd.Columns["Payment"].Index)
            {
                // Get the row that was modified
                DataGridViewRow row = dgvcoladd.Rows[e.RowIndex];

                // Retrieve the "Amt. Paid" value from the current row
                string payment = row.Cells["Payment"].Value.ToString();

                // Clean and extract the numeric value for Amt. Paid
                double amountPaid = 0.00;
                payment = RemoveNonNumericChars(payment); // Remove non-numeric characters
                double.TryParse(payment, out amountPaid);

                // Retrieve the current total paid amount (ltotalamtpaidadd.Text)
                double totalPaidValue = 0.00;
                double.TryParse(ltotalamtpaidadd.Text.Replace("₱", "").Trim(), out totalPaidValue);

                // Calculate the excess amount for the changed row
                double excessAmount = totalPaidValue - amountPaid;

                // Update the Remarks column with the new excess amount for the changed row
                string updatedRemarks = $"New Loan Trans.: ₱{Math.Max(0, excessAmount):F2}";
                row.Cells["Remarks"].Value = updatedRemarks;

                // Now update the Remarks for all subsequent rows (Row 2, Row 3, etc.)
                UpdateExcessAmountForOtherRows(e.RowIndex, amountPaid, totalPaidValue);

                // Update the total Amt. Paid value (ltotalamtpaidadd)
                UpdateTotalAmtPaid();
            }
        }

        private void UpdateTotalAmtPaid()
        {
            double totalAmtPaid = 0.00;

            // Loop through each row and sum the Amt. Paid values
            foreach (DataGridViewRow row in dgvcoladd.Rows)
            {
                string payment = row.Cells["Payment"].Value.ToString();
                double amountPaid = 0.00;

                // Clean and extract the numeric value for Amt. Paid
                payment = RemoveNonNumericChars(payment); // Remove non-numeric characters
                double.TryParse(payment, out amountPaid);

                totalAmtPaid += amountPaid; // Add to the total Amt. Paid
            }

            // Update the ltotalamtpaidadd label with the total Amt. Paid value
            ltotalamtpaidadd.Text = "Total Paid: ₱" + totalAmtPaid.ToString("F2");

            // Update lgenbaladd and lpenaltytotaladd as zero
            lgenbaladd.Text = "Balance: ₱0.00";
            lpenaltytotaladd.Text = "Penalty: ₱0.00";
        }

        private void UpdateExcessAmountForOtherRows(int changedRowIndex, double updatedAmountPaid, double totalPaidValue)
        {
            // Adjust the balance for the changed row (the one where Amt. Paid was updated)
            DataGridViewRow changedRow = dgvcoladd.Rows[changedRowIndex];
            string payment = changedRow.Cells["Payment"].Value.ToString();
            payment = RemoveNonNumericChars(payment); // Remove non-numeric characters
            double currentAmountPaid = 0.00;
            double.TryParse(payment, out currentAmountPaid);

            // Subtract the difference in payment from the total balance
            double updatedExcessAmount = totalPaidValue - updatedAmountPaid;

            // If the updated excess is less than 0, set it to 0
            updatedExcessAmount = Math.Max(0, updatedExcessAmount);

            // Update the Remarks column for the changed row
            changedRow.Cells["Remarks"].Value = $"New Loan Trans.: ₱{updatedExcessAmount:F2}";

            // Now update the Remarks for the following rows (e.g., Row 2, Row 3, etc.)
            double cumulativePaid = totalPaidValue;  // Start with the updated total paid amount

            for (int i = changedRowIndex + 1; i < dgvcoladd.Rows.Count; i++)
            {
                DataGridViewRow row = dgvcoladd.Rows[i];

                // Retrieve the payment value for this row
                string nextPayment = row.Cells["Payment"].Value.ToString();
                nextPayment = RemoveNonNumericChars(nextPayment);
                double nextAmountPaid = 0.00;
                double.TryParse(nextPayment, out nextAmountPaid);

                // Retrieve the amortization (loan amount) for this row
                string amortization = row.Cells["Amortization"].Value.ToString();
                double loanAmount = 0.00;
                double.TryParse(amortization, out loanAmount);

                // Update the cumulative total paid up to this row
                cumulativePaid += nextAmountPaid;

                // Calculate the excess amount for this row based on the updated value of previous rows
                double newExcessAmount = cumulativePaid - loanAmount;

                // If the excess amount is less than 0, we set it to 0
                newExcessAmount = Math.Max(0, newExcessAmount);

                // Update the Remarks column for this row (Row 3)
                row.Cells["Remarks"].Value = $"New Loan Trans.: ₱{newExcessAmount:F2}";
            }
        }
    }
}

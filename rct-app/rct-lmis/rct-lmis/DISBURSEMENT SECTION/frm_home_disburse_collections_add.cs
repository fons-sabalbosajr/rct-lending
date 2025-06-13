using Microsoft.Identity.Client;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rct_lmis.DISBURSEMENT_SECTION
{
    public partial class frm_home_disburse_collections_add : Form
    {

        private IMongoCollection<BsonDocument> _loanApprovedCollection;
        private IMongoCollection<BsonDocument> _loanCollectionsCollection;
        private IMongoCollection<BsonDocument> _loanDisbursedCollection;
        private IMongoCollection<BsonDocument> _loanCollectorsCollection;
        private IMongoCollection<BsonDocument> _loanAccountCollection;
        private IMongoCollection<BsonDocument> _loanRemainingBalance;
        private IMongoCollection<BsonDocument> _loanAccountCycleCollection;
        private frm_home_disburse_collections _parentForm;
        private List<string> _accountIdList;
        private string name;
        private string loggedInUsername;
        private string _clientNo;
        private readonly string _loanNo;

        public frm_home_disburse_collections_add(string clientNo, string loanNo)
        {
            InitializeComponent();
            dtdate.Value = DateTime.Now;
            dtcoldate.Value = DateTime.Now;
            _clientNo = clientNo;
            _loanNo = loanNo;

            
            loggedInUsername = UserSession.Instance.CurrentUser;


            // Initialize MongoDB connection for loan_approved collection
            var database = MongoDBConnection.Instance.Database;
            _loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");
            _loanCollectionsCollection = database.GetCollection<BsonDocument>("loan_collections");
            _loanDisbursedCollection = database.GetCollection<BsonDocument>("loan_disbursed");
            _loanCollectorsCollection = database.GetCollection<BsonDocument>("loan_collectors");
            _loanAccountCollection = database.GetCollection<BsonDocument>("loan_account_data");
            _loanRemainingBalance = database.GetCollection<BsonDocument>("loan_remaining_balance");
            _loanAccountCycleCollection = database.GetCollection<BsonDocument>("loan_account_cycles");

            LoadPaymentModes();
            
            bsave.Enabled = false;
            bcancel.Enabled = false;
        }

        LoadingFunction load = new LoadingFunction();

        public async Task LoadClientName(string clientNo)
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<BsonDocument>("loan_disbursed");

            // Find the document by ClientNo
            var filter = Builders<BsonDocument>.Filter.Eq("ClientNo", clientNo);
            var result = await collection.Find(filter).FirstOrDefaultAsync();

            if (result != null)
            {
                // Concatenate the full name
                string firstName = result["FirstName"].AsString;
                string middleName = result["MiddleName"].AsString;
                string lastName = result["LastName"].AsString;

                // Set the tname.Text with full name
                tname.Text = $"{firstName} {middleName} {lastName}".Trim();
            }
            else
            {
                // Handle case when no result is found
                MessageBox.Show("Client not found.");
            }
        }

        private void LoadCollectors()
        {
            try
            {
                // Clear the cbcollector combo box
                cbcollector.Items.Clear();

                // Add default item
                cbcollector.Items.Add("--select collector--");
                cbcollector.SelectedIndex = 0;

                // Get all collectors from loan_collectors collection
                var collectors = _loanCollectorsCollection.Find(new BsonDocument()).ToList();

                // Loop through the collectors and add their names to cbcollector
                foreach (var collector in collectors)
                {
                    // Ensure that the collector has a Name field
                    if (collector.Contains("Name") && collector["Name"] != BsonNull.Value)
                    {
                        string collectorName = collector.GetValue("Name").AsString;
                        cbcollector.Items.Add(collectorName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading collectors: " + ex.Message);
            }
        }

        private void LoadPaymentModes()
        {
            // Clear any existing items
            cbpaymentmode.Items.Clear();

            // Add common payment modes
            cbpaymentmode.Items.Add("--select payment mode--");
            cbpaymentmode.Items.Add("Cash");
            cbpaymentmode.Items.Add("Check");
            cbpaymentmode.Items.Add("Bank Transfer");
            cbpaymentmode.Items.Add("Online Payment");
           
            // Set the default selected item
            cbpaymentmode.SelectedIndex = 0;
        }

        private decimal ConvertToDecimal(string input)
        {
            if (decimal.TryParse(input, NumberStyles.Any, new CultureInfo("en-PH"), out var result))
                return result;
            return 0m;
        }

        private async void LoadLoanDisbursedData()
        {
            if (string.IsNullOrEmpty(_loanNo))
            {
                MessageBox.Show("Loan number not provided.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("LoanNo", _loanNo);
                var loanDisbursed = await _loanDisbursedCollection.Find(filter).FirstOrDefaultAsync();
                var loanAccountCycle = await _loanAccountCycleCollection.Find(filter).FirstOrDefaultAsync();

                if (loanDisbursed == null && loanAccountCycle == null)
                {
                    MessageBox.Show($"No loan data found for LoanNo: {_loanNo}", "Data Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // --- Load loanDisbursed (if found) ---
                if (loanDisbursed != null)
                {
                    laccountid.Text = loanDisbursed.GetValue("AccountId", "N/A").ToString();
                    tloanid.Text = loanDisbursed.GetValue("LoanNo", "N/A").ToString();
                    tclientno.Text = loanDisbursed.GetValue("ClientNo", "N/A").ToString();

                    string lastName = loanDisbursed.GetValue("LastName", "").ToString();
                    string firstName = loanDisbursed.GetValue("FirstName", "").ToString();
                    string middleName = loanDisbursed.GetValue("MiddleName", "").ToString();
                    tname.Text = string.IsNullOrWhiteSpace($"{lastName}{firstName}") ? "N/A" : $"{lastName}, {firstName} {middleName}".Trim();

                    string barangay = loanDisbursed.GetValue("Barangay", "N/A").ToString();
                    string city = loanDisbursed.GetValue("City", "N/A").ToString();
                    taddress.Text = $"{barangay}, {city}";

                    tcontact.Text = loanDisbursed.GetValue("ContactNo", "N/A").ToString();

                    tloanamt.Text = FormatCurrency(loanDisbursed.GetValue("LoanAmount", "₱0.00").ToString());
                    tterm.Text = loanDisbursed.GetValue("LoanTerm", "N/A").ToString();
                    tpaymode.Text = loanDisbursed.GetValue("PaymentMode", "N/A").ToString();
                    tpayamort.Text = FormatCurrency(loanDisbursed.GetValue("LoanAmortization", "₱0.00").ToString());
                    tloanbal.Text = FormatCurrency(loanDisbursed.GetValue("LoanBalance", "₱0.00").ToString());
                }

                // --- Override with loanAccountCycle financials (if found) ---
                if (loanAccountCycle != null)
                {
                    tloanid.Text = loanAccountCycle.GetValue("LoanNo", tloanid.Text).ToString();
                    tclientno.Text = _clientNo; // trusted value from session/context

                    decimal loanAmount = ConvertToDecimal(loanAccountCycle.GetValue("LoanAmount", "0").ToString());
                    decimal loanBalance = ConvertToDecimal(loanAccountCycle.GetValue("LoanBalance", "0").ToString());
                    decimal loanInterest = ConvertToDecimal(loanAccountCycle.GetValue("LoanInterest", "0").ToString());

                    tloanamt.Text = loanAmount.ToString("C", new CultureInfo("en-PH"));
                    tloanbal.Text = loanBalance.ToString("C", new CultureInfo("en-PH"));

                    string termStr = loanAccountCycle.GetValue("LoanTerm", "0").ToString();
                    string numericTerm = new string(termStr.Where(char.IsDigit).ToArray());
                    if (!int.TryParse(numericTerm, out int termMonths) || termMonths <= 0)
                    {
                        MessageBox.Show("Invalid or missing loan term. Cannot compute due amounts.", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    decimal principalDue = loanAmount / termMonths;
                    decimal interestDue = (loanAmount * (loanInterest / 100)) / termMonths;

                    tprincipaldue.Text = principalDue.ToString("C", new CultureInfo("en-PH"));
                    tcolinterest.Text = interestDue.ToString("C", new CultureInfo("en-PH"));

                    tterm.Text = loanAccountCycle.GetValue("LoanTerm", tterm.Text).ToString();
                    tpaymode.Text = loanAccountCycle.GetValue("PaymentMode", tpaymode.Text).ToString();

                    tpayamort.Text = FormatCurrency(tpayamort.Text); // preserve fallback

                    DateTime? startDate = ParseDateOrNull(loanAccountCycle.GetValue("StartPaymentDate", "").ToString());
                    DateTime? maturityDate = ParseDateOrNull(loanAccountCycle.GetValue("MaturityDate", "").ToString());

                    tpaystart.Text = startDate.HasValue ? startDate.Value.ToString("MM/dd/yyyy") : "N/A";
                    tpaymature.Text = maturityDate.HasValue ? maturityDate.Value.ToString("MM/dd/yyyy") : "N/A";
                }

                // --- Check Collections Status ---
                var collectionFilter = Builders<BsonDocument>.Filter.Eq("LoanNo", _loanNo);
                bool hasCollections = await _loanCollectionsCollection.Find(collectionFilter).AnyAsync();

                decimal finalBalance = ConvertToDecimal(tloanbal.Text.Replace("₱", "").Replace(",", "").Trim());
                tpaymentstatus.Text = !hasCollections ? "No collections have been made"
                                      : finalBalance > 0 ? "With Pending Balance"
                                      : "Fully Paid";

                ComputeLoanBalance();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading loan data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private string FormatCurrency(string value)
        {
            if (decimal.TryParse(value.Replace("₱", "").Replace(",", "").Trim(), out decimal result))
                return result.ToString("C", new CultureInfo("en-PH"));
            return "₱0.00";
        }


        private DateTime? ParseDateOrNull(string dateStr)
        {
            if (DateTime.TryParse(dateStr, out DateTime parsed))
                return parsed;
            return null;
        }



        private int CalculateWeekdaysElapsed(DateTime startDate, DateTime endDate)
        {
            int weekdaysElapsed = 0;
            DateTime currentDate = startDate;

            while (currentDate < endDate)
            {
                currentDate = currentDate.AddDays(1);
                if (currentDate.DayOfWeek != DayOfWeek.Sunday) // Exclude Sundays
                {
                    weekdaysElapsed++;
                }
            }

            return weekdaysElapsed;
        }


        private void ComputeLoanBalance()
        {
            try
            {
                if (string.IsNullOrEmpty(tloanid.Text))
                {
                    MessageBox.Show("Loan ID is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Abort if Start Payment Date is missing or invalid
                if (string.IsNullOrWhiteSpace(tpaystart.Text) || tpaystart.Text.Trim().ToUpper() == "N/A")
                {
                    //MessageBox.Show("Start Payment Date is missing or invalid. Aborting computation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DateTime startPaymentDate;
                if (!DateTime.TryParseExact(tpaystart.Text,
                    new[] { "MM/dd/yyyy", "M/d/yyyy" },
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out startPaymentDate))
                {
                    MessageBox.Show("Invalid Start Payment Date format. Aborting computation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                decimal loanAmount = ConvertToDecimalSafe(tloanamt.Text);
                if (loanAmount <= 0)
                {
                    MessageBox.Show("Invalid Loan Amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                decimal amortization = ConvertToDecimalSafe(tpayamort.Text);
                if (amortization <= 0)
                {
                    MessageBox.Show("Invalid Amortization.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DateTime currentDate = DateTime.Now;
                int daysElapsed = CalculateWeekdaysElapsed(startPaymentDate, currentDate);

                if (daysElapsed <= 0)
                {
                    MessageBox.Show("No days elapsed since the start date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string loanTermRaw = tterm.Text.Trim();
                if (string.IsNullOrEmpty(loanTermRaw) || !int.TryParse(new string(loanTermRaw.Where(char.IsDigit).ToArray()), out int loanTerm) || loanTerm <= 0)
                {
                    MessageBox.Show("Invalid Loan Term. Loan term must be greater than zero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                decimal principalDue = (amortization * 0.833333333m) * daysElapsed;
                decimal interestDue = (amortization * 0.166666667m) * daysElapsed;
                decimal penalty = (principalDue + interestDue) * 0.03m * (daysElapsed / 30m);
                decimal totalAmortization = principalDue + interestDue + penalty;

                tcolpenalty.Text = penalty.ToString("C", new CultureInfo("en-PH"));
                tprincipaldue.Text = principalDue.ToString("C", new CultureInfo("en-PH"));
                tcolinterest.Text = interestDue.ToString("C", new CultureInfo("en-PH"));
                tcoltotal.Text = totalAmortization.ToString("C", new CultureInfo("en-PH"));
                tloanbal.Text = loanAmount.ToString("C", new CultureInfo("en-PH"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error computing loan balance: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private decimal ConvertToDecimalSafe(string value)
        {
            decimal result = 0;
            try
            {
                // Try to parse the value, remove currency symbol and commas
                string cleanedValue = value.Replace("₱", "").Replace(",", "").Trim();
                decimal.TryParse(cleanedValue, out result);
            }
            catch (Exception)
            {
                // Return 0 if parsing fails
                result = 0;
            }
            return result;
        }

        private async Task<bool> SaveLoanCollectionDataAsync()
        {
            string loanIdNo = tloanid.Text.Trim();
            string clientNo = tclientno.Text.Trim();
            string fullName = tname.Text.Trim();
            string amountPaidText = tcolactual.Text.Trim();
            string collectionDate = dtcoldate.Text.Trim();

            if (string.IsNullOrEmpty(loanIdNo) || string.IsNullOrEmpty(clientNo) ||
                string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(amountPaidText))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(amountPaidText, out decimal amountPaid) || amountPaid <= 0)
            {
                MessageBox.Show("Invalid amount paid.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Retrieve loan info
            var loanInfo = GetLoanDisbursementInfo(loanIdNo);
            if (loanInfo == null)
            {
                MessageBox.Show($"Loan not found for Loan ID: {loanIdNo}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Parse loan amount
            decimal loanAmount = loanInfo.Contains("LoanAmount") && decimal.TryParse(loanInfo["LoanAmount"].ToString(), out var amt) ? amt : 0;

            // Parse interest rate
            decimal loanInterestRate = 0m;
            if (loanInfo.TryGetValue("LoanInterest", out var li))
            {
                string interestStr = li.ToString().Replace("%", "").Trim();
                if (decimal.TryParse(interestStr, out var rate)) loanInterestRate = rate / 100;
            }

            // Parse loan term
            int loanTerm = 0;
            if (loanInfo.TryGetValue("LoanTerm", out var lt))
            {
                string termDigits = System.Text.RegularExpressions.Regex.Match(lt.ToString(), @"\d+").Value;
                if (!string.IsNullOrEmpty(termDigits)) int.TryParse(termDigits, out loanTerm);
            }

            if (loanTerm <= 0)
            {
                MessageBox.Show("Loan term is invalid or missing. Cannot calculate amortization.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Calculate amortization
            decimal totalLoanToPay = loanAmount * (1 + loanInterestRate);
            decimal dailyAmortization = totalLoanToPay / loanTerm;
            decimal amortizationInterest = (loanAmount * loanInterestRate) / loanTerm;
            decimal amortizationPrincipal = dailyAmortization - amortizationInterest;

            // Build collection document
            var newCollection = new BsonDocument
              {
                  { "LoanNo", loanIdNo },
                  { "ClientNo", clientNo },
                  { "FullName", fullName },
                  { "AmountPaid", amountPaid },
                  { "CollectionDate", collectionDate },
                  { "DailyAmortization", dailyAmortization },
                  { "AmortizationInterest", amortizationInterest },
                  { "AmortizationPrincipal", amortizationPrincipal },
                  { "CreatedAt", DateTime.Now }
              };

            try
            {
                await _loanCollectionsCollection.InsertOneAsync(newCollection);
                MessageBox.Show("Loan collection saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //LoadLoanCollections(); // Refresh the DataGridView
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving loan collection: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Function to retrieve loan information from the loan_disbursed collection
        private BsonDocument GetLoanDisbursementInfo(string loanNo)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("LoanNo", loanNo);
            var doc = _loanDisbursedCollection.Find(filter).FirstOrDefault();
            if (doc != null) return doc;

            doc = _loanAccountCycleCollection.Find(filter).FirstOrDefault();
            return doc;  // may still be null
        }



        // Helper method to retrieve the latest running balance for a loan
        private decimal GetLatestRunningBalance(string loanId)
        {
            try
            {
                // Filter to find the loan collections for the given LoanID, ordered by the latest collection date
                var filter = Builders<BsonDocument>.Filter.Eq("LoanID", loanId);
                var sort = Builders<BsonDocument>.Sort.Descending("CollectionDate");

                // Find the most recent collection entry for the loan
                var latestCollection = _loanCollectionsCollection.Find(filter).Sort(sort).FirstOrDefault();

                // If no previous collection is found, return the current loan balance from the textbox
                if (latestCollection == null)
                {
                    decimal initialBalance = ParseCurrency(tloanbal.Text); // Default to initial loan balance
                    if (initialBalance == 0)
                    {
                        MessageBox.Show($"No previous collections found. Defaulting to initial balance.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return initialBalance;
                }

                // Ensure RunningBalance exists and retrieve its value
                if (latestCollection.Contains("RunningBalance"))
                {
                    // Convert the RunningBalance to decimal
                    decimal previousRunningBalance = ((BsonDecimal128)latestCollection["RunningBalance"]).ToDecimal();
                    return previousRunningBalance;
                }
                else
                {
                    MessageBox.Show($"Running balance not available for LoanID: {loanId}. Defaulting to 0.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return 0; // Return 0 if RunningBalance field is missing
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving latest running balance: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0; // Return 0 in case of error
            }
        }

        // Helper method to parse currency
        private decimal ParseCurrency(string text)
        {
            try
            {
                // Remove currency symbols and commas from the string, then attempt to parse as a decimal
                text = text.Replace("₱", "").Replace(",", "").Trim();
                return decimal.TryParse(text, out decimal value) ? value : 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error parsing currency value: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0; // Return 0 in case of error
            }
        }

        private void SaveLoanAccountData(decimal principalPaid, decimal interestPaid, string reference)
        {
            var principalDocument = new BsonDocument
             {
                 { "AccountTitle", "A120-1" }, // Loans Receivable (Principal)
                 { "Debit", principalPaid },
                 { "Credit", 0 },
                 { "Reference", reference },
                 { "Date", DateTime.Now }
             };

            var interestDocument = new BsonDocument
             {
                 { "AccountTitle", "A120-2" }, // Loans Receivable (Interest)
                 { "Debit", interestPaid },
                 { "Credit", 0 },
                 { "Reference", reference },
                 { "Date", DateTime.Now }
             };

            _loanAccountCollection.InsertOne(principalDocument);
            _loanAccountCollection.InsertOne(interestDocument);
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
                name = fullName;
            }
        }


        private async void frm_home_disburse_collections_add_LoadAsync(object sender, EventArgs e)
        {
            //GenerateCollectionId();
            LoadUserInfo(loggedInUsername);

            clientnotest.Text = _clientNo;
            await LoadClientName(_clientNo);

            LoadCollectors();
            LoadLoanDisbursedData();
        }

        private void bcopyaccno_Click(object sender, EventArgs e)
        {
            string accNo = laccountid.Text;
            Clipboard.SetText(accNo);
            MessageBox.Show("The account number has been copied to your clipboard.", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void cbpaymentmode_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected payment mode
            string selectedPaymentMode = cbpaymentmode.SelectedItem.ToString();

            if (selectedPaymentMode == "Cash")
            {
                // Show Reference No. and Date of Payment Received
                tcolrefno.Visible = true;
                dtcoldate.Visible = true;
                lcolrefno.Visible = true;
                lcoldaterec.Visible = true;


                // Hide Bank and Branch
                tcolbank.Visible = false;
                tcolbranch.Visible = false;
                lcolbank.Visible = false;
                lcolbranch.Visible = false;

                bsave.Enabled = true;
                bcancel.Enabled = true;

                tcolpayamt.Focus();
            }
            else if (selectedPaymentMode == "Check" || selectedPaymentMode == "Bank Transfer" || selectedPaymentMode == "Online Payment")
            {
                // Show all fields for Check or Bank Transfer
                tcolrefno.Visible = true;
                lcolrefno.Visible= true;

                dtcoldate.Visible = true;
                lcoldaterec.Visible= true;

                tcolbank.Visible = true;
                lcolbank.Visible= true;

                tcolbranch.Visible = true;
                lcolbranch.Visible = true;

                bsave.Enabled = true;
                bcancel.Enabled = true;

                tcolpayamt.Focus();
            }
            else if (selectedPaymentMode == "Mobile Payment")
            {
                // Show Reference No. and Date of Payment Received, hide Bank and Branch
                tcolrefno.Visible = true;
                lcolrefno.Visible = true;

                dtcoldate.Visible = true;
                lcoldaterec.Visible = true;

                tcolbank.Visible = false;
                lcolbank.Visible = false;

                tcolbranch.Visible = false;
                lcolbranch.Visible = false;

                bsave.Enabled = true;
                bcancel.Enabled = true;

                tcolpayamt.Focus();
            }
            else
            {
                // Hide all fields if no valid payment mode is selected
                tcolrefno.Visible = false;
                lcolrefno.Visible = false;

                dtcoldate.Visible = false;
                lcoldaterec.Visible = false;

                tcolbank.Visible = false;
                lcolbank.Visible = false;

                tcolbranch.Visible = false;
                lcolbranch.Visible = false;
            }
        }

        private void tcolpayamt_TextChanged(object sender, EventArgs e)
        {
            // Set tcolpayamt.Text to tcolactual.Text
            tcolactual.Text = tcolpayamt.Text;
            bamtfull.Visible = true;
            // Set the current date and time in mm/dd/yyyy hh:mm AM/PM format
            //tcoldaterec.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");

            if (string.IsNullOrEmpty(tcolpayamt.Text)) 
            {
                bamtfull.Visible=false;
            }
        }

        private void tname_TextChanged(object sender, EventArgs e)
        {
            
        }

        private async void bsave_Click(object sender, EventArgs e)
        {
            // Show confirmation dialog
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to save the loan collection data?", "Confirm Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                bool isSaved = await SaveLoanCollectionDataAsync(); // ✅ Ensure save is asynchronous

                if (isSaved)
                {
                    decimal principalPaid = ParseCurrency(tcolpaid.Text);
                    decimal interestPaid = CalculateInterestPaid();
                    string reference = GetLoanIDNo();

                    // ✅ Save loan account data
                    SaveLoanAccountData(principalPaid, interestPaid, reference);

                    MessageBox.Show("Loan collection data and account data saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //GenerateCollectionId();

                    await Task.Delay(500); // ✅ Wait for DB update
                    //_parentForm.LoadLoanCollections(); // ✅ Reload updated data
                }
                else
                {
                    MessageBox.Show("Failed to save data. Try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Save operation cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private decimal CalculateInterestPaid()
        {
            return ParseCurrency(tcolinterest.Text);
        }

        private string GetLoanIDNo()
        {
            return tloanid.Text.Trim();
        }


        private void bcancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to cancel entering collection transaction", "Cancel Entry",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Clear all textboxes
                tloanid.Text = "";
                tname.Text = "";
                tclientno.Text = "";
                taddress.Text = "";
                tcontact.Text = "";
                tloanamt.Text = "";
                tterm.Text = "";
                tpaystart.Text = "";
                tpaymature.Text = "";
                tpaymode.Text = "";
                tpayamort.Text = "";
                tloanbal.Text = "";
                tpaymentstatus.Text = "";

                // Clear additional textboxes for collection details
                tprincipaldue.Text = "";
                tcolinterest.Text = "";
                tcolpenalty.Text = "";
                tcoltotal.Text = "";
                tcolpaid.Text = "";
                tcolactual.Text = "";
                tcolrefno.Text = "";
                tcolpayamt.Text = "";
                tcolbank.Text = "";
                tcolbranch.Text = "";

                this.Close();
            }
        }


        private void bamtfull_Click(object sender, EventArgs e)
        {
            string amt = tcolpaid.Text;
            tcolpayamt.Text = amt;
        }

        private void frm_home_disburse_collections_add_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want to cancel entering collection transaction", "Cancel Entry",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
               
                e.Cancel = false; 
            }
            else
            {
                e.Cancel = true; 
            }
        }

        private void cbcollector_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void brefresh_Click(object sender, EventArgs e)
        {
            load.Show(this);
            Thread.Sleep(1000);
            LoadLoanDisbursedData();
            load.Close();
        }

        private void tpayamort_TextChanged(object sender, EventArgs e)
        {
            tcolpaid.Text = this.tpayamort.Text;
        }
    }
}


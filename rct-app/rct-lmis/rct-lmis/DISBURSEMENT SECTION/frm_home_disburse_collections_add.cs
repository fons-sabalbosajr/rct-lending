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
        private frm_home_disburse_collections _parentForm;
        private List<string> _accountIdList;
        private string name;
        private string loggedInUsername;
        private string _clientNo;

        public frm_home_disburse_collections_add(string clientNo)
        {
            InitializeComponent();
            dtdate.Value = DateTime.Now;
            dtcoldate.Value = DateTime.Now;
            _clientNo = clientNo;
            loggedInUsername = UserSession.Instance.CurrentUser;


            // Initialize MongoDB connection for loan_approved collection
            var database = MongoDBConnection.Instance.Database;
            _loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");
            _loanCollectionsCollection = database.GetCollection<BsonDocument>("loan_collections");
            _loanDisbursedCollection = database.GetCollection<BsonDocument>("loan_disbursed");
            _loanCollectorsCollection = database.GetCollection<BsonDocument>("loan_collectors");
            _loanAccountCollection = database.GetCollection<BsonDocument>("loan_account_data");
            _loanRemainingBalance = database.GetCollection<BsonDocument>("loan_remaining_balance");

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


        private void ClearAndDisableFields()
        {
            tloanamt.Text = string.Empty;
            tterm.Text = string.Empty;
            tpaystart.Text = string.Empty;
            tpaymature.Text = string.Empty;
            tpaymode.Text = string.Empty;
            tpayamort.Text = string.Empty;
            tloanbal.Text = string.Empty;
            tpaymentstatus.Text = string.Empty;
            tprincipaldue.Text = string.Empty;
            tcolinterest.Text = string.Empty;
            tcolpenalty.Text = string.Empty;
            tcoltotal.Text = string.Empty;
            tcolpaid.Text = string.Empty;
            tcolactual.Text = string.Empty;
            tcolrefno.Text = string.Empty;
            tcolpayamt.Text = string.Empty;
            tcolbank.Text = string.Empty;
            tcolbranch.Text = string.Empty;

            bsave.Enabled = false;
            bcancel.Enabled = false;
        }

        private decimal GetLatestPrincipalBalance(string loanId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("LoanID", loanId);
            var sort = Builders<BsonDocument>.Sort.Descending("CollectionDate");
            var latestCollection = _loanCollectionsCollection.Find(filter).Sort(sort).FirstOrDefault();

            if (latestCollection != null && latestCollection.Contains("PrincipalBalance"))
            {
                return latestCollection["PrincipalBalance"].ToDecimal();
            }

            return 0;
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
            if (string.IsNullOrEmpty(input)) return 0;
            input = input.Replace("₱", "").Replace(",", "").Trim();
            if (decimal.TryParse(input, out decimal result))
            {
                return result;
            }
            return 0; // Return 0 if conversion fails
        }

        private async void LoadLoanDisbursedData()
        {
            string fullName = tname.Text.Trim();

            try
            {
                if (string.IsNullOrEmpty(fullName))
                {
                    MessageBox.Show("Please enter a valid full name.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var nameParts = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (nameParts.Length < 2)
                {
                    MessageBox.Show("Full name must include at least a first name and a last name.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string firstName = nameParts[0];
                string lastName = nameParts[nameParts.Length - 1];
                string middleName = nameParts.Length > 2 ? string.Join(" ", nameParts.Skip(1).Take(nameParts.Length - 2)) : "";

                var filter = Builders<BsonDocument>.Filter.And(
                    Builders<BsonDocument>.Filter.Eq("FirstName", firstName),
                    Builders<BsonDocument>.Filter.Eq("LastName", lastName)
                );

                if (!string.IsNullOrEmpty(middleName))
                {
                    filter = Builders<BsonDocument>.Filter.And(filter, Builders<BsonDocument>.Filter.Eq("MiddleName", middleName));
                }

                var loanDisbursed = await _loanDisbursedCollection.Find(filter).FirstOrDefaultAsync();

                if (loanDisbursed != null)
                {
                    try
                    {
                        tclientno.Text = loanDisbursed.GetValue("ClientNo", "").ToString();
                        tloanid.Text = loanDisbursed.GetValue("LoanNo", "").ToString();
                        tloanamt.Text = loanDisbursed.GetValue("LoanAmount", "").ToString();
                        tterm.Text = loanDisbursed.GetValue("LoanTerm", "").ToString();
                        tpaymode.Text = loanDisbursed.GetValue("PaymentMode", "").ToString();
                        tpayamort.Text = loanDisbursed.GetValue("LoanAmortization", "").ToString();

                        string barangay = loanDisbursed.GetValue("Barangay", "").ToString();
                        string city = loanDisbursed.GetValue("City", "").ToString();
                        taddress.Text = $"{barangay}, {city}";
                        tcontact.Text = loanDisbursed.GetValue("ContactNo", "").ToString();

                        // ✅ Convert Loan Amount & Interest Safely
                        decimal loanAmount = ConvertToDecimal(loanDisbursed.GetValue("LoanAmount", "0").ToString().Replace("₱", "").Trim());
                        decimal loanInterest = ConvertToDecimal(loanDisbursed.GetValue("LoanInterest", "0").ToString().TrimEnd('%'));

                        // ✅ Extract Loan Term Numeric Value
                        string loanTermRaw = loanDisbursed.GetValue("LoanTerm", "0 months").ToString();
                        int loanTerm = int.Parse(new string(loanTermRaw.Where(char.IsDigit).ToArray())); // Extract numeric part

                        // ✅ Parse Start Payment Date
                        string startPaymentDateStr = loanDisbursed.GetValue("StartPaymentDate", "").ToString();
                        DateTime startPaymentDate = DateTime.TryParse(startPaymentDateStr, out var spd) ? spd : DateTime.MinValue;
                        tpaystart.Text = startPaymentDate != DateTime.MinValue ? startPaymentDate.ToString("MM/dd/yyyy") : "N/A";

                        // ✅ Parse & Format Maturity Date (Ensure MM/dd/yyyy)
                        var maturityDateValue = loanDisbursed.GetValue("MaturityDate", BsonNull.Value);
                        if (maturityDateValue != BsonNull.Value)
                        {
                            DateTime maturityDate = maturityDateValue.ToUniversalTime();
                            tpaymature.Text = maturityDate.ToString("MM/dd/yyyy");
                        }
                        else
                        {
                            tpaymature.Text = "N/A";
                        }

                        // ✅ Compute Principal & Interest Due
                        decimal principalDue = loanAmount / loanTerm;
                        decimal interestDue = (loanAmount * (loanInterest / 100)) / loanTerm;

                        tprincipaldue.Text = principalDue.ToString("C", new CultureInfo("en-PH"));
                        tcolinterest.Text = interestDue.ToString("C", new CultureInfo("en-PH"));

                        ComputeLoanBalance();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, $"Error processing loan data: {ex.Message}", "Processing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"No loan disbursed data found for the name: {fullName}", "Data Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading loan disbursed data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private DateTime CalculateMaturityDate(DateTime startDate, int weekdaysToAdd)
        {
            DateTime currentDate = startDate;
            int weekdaysAdded = 0;

            while (weekdaysAdded < weekdaysToAdd)
            {
                currentDate = currentDate.AddDays(1);
                if (currentDate.DayOfWeek != DayOfWeek.Sunday)  // Skip Sundays
                {
                    weekdaysAdded++;
                }
            }

            // Check if the calculated end date is off by a day
            // For example, if it's ending on a Sunday, subtract one more day
            if (currentDate.DayOfWeek == DayOfWeek.Sunday)
            {
                currentDate = currentDate.AddDays(-1);  // Subtract 1 day to get the correct end date
            }

            return currentDate; // Return the corrected maturity date
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

                if (string.IsNullOrEmpty(tpaystart.Text))
                {
                    MessageBox.Show("Start Payment Date is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DateTime currentDate = DateTime.Now;
                decimal loanAmount = ConvertToDecimal(tloanamt.Text.Replace("₱", "").Replace(",", ""));
                decimal amortization = ConvertToDecimal(tpayamort.Text.Replace("₱", "").Replace(",", ""));

                int daysElapsed = CalculateWeekdaysElapsed(DateTime.Parse(tpaystart.Text), currentDate); // Calculate weekdays excluding Sundays

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

        private void GenerateCollectionId()
        {
            try
            {
                // Ensure the ClientNo is not empty
                if (string.IsNullOrEmpty(clientnotest.Text))
                {
                    MessageBox.Show("ClientNo is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clientnotest.Text = _clientNo;  // Assume _clientNo has the ClientNo of the user

                // Get the ClientNo from the loan_approved collection based on clientnotest.Text
                var filterApproved = Builders<BsonDocument>.Filter.Eq("ClientNo", clientnotest.Text);
                var loanApproved = _loanApprovedCollection.Find(filterApproved).FirstOrDefault();

                if (loanApproved != null)
                {
                    // Extracting the base ClientNo
                    string clientNo = loanApproved["ClientNo"].ToString();

                    // Find the last collection for this ClientNo in loan_collections
                    var filterCollections = Builders<BsonDocument>.Filter.Regex("ClientNo", new BsonRegularExpression($"^{clientNo}-COL-"));
                    var sort = Builders<BsonDocument>.Sort.Descending("ClientNo");
                    var lastCollection = _loanCollectionsCollection.Find(filterCollections).Sort(sort).FirstOrDefault();

                    // Extract and increment the collection number
                    int collectionNumber = 1; // Default if no collections exist yet
                    if (lastCollection != null)
                    {
                        string lastCollectionId = lastCollection["ClientNo"].ToString();
                        string lastNumberStr = lastCollectionId.Substring(lastCollectionId.LastIndexOf("-COL-") + 5);

                        // Try to parse the last collection number
                        if (int.TryParse(lastNumberStr, out int lastNumber))
                        {
                            collectionNumber = lastNumber + 1; // Increment the last number
                        }
                        else
                        {
                            MessageBox.Show("Error parsing last collection number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Format the new CollectionId
                    string newCollectionId = $"{clientNo}-COL-{collectionNumber:D4}"; // Ensures the number is formatted to four digits

                    // Assign the new CollectionId to the laccountid label
                    laccountid.Text = newCollectionId; // Update the label with the new ID
                }
                else
                {
                    MessageBox.Show("ClientNo not found in loan_approved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine("ClientNo not found in loan_approved.");
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Format error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (MongoException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void SaveRemainingBalance(decimal principalBalance, string loanIdNo, decimal loanAmount, int loanTerm, decimal amortizedAmt)
        {
            try
            {
                var remainingBalanceDocument = new BsonDocument
                 {
                     { "LoanID", loanIdNo },
                     { "RemainingPrincipalBalance", principalBalance },
                     { "LoanAmount", loanAmount },
                     { "LoanTerm", loanTerm },
                     { "Amortization", amortizedAmt },
                     { "LastUpdated", DateTime.Now }
                 };

                // Insert the remaining balance document into the existing _loanRemainingBalance collection
                _loanRemainingBalance.InsertOne(remainingBalanceDocument);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save remaining balance: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private async Task<bool> SaveLoanCollectionDataAsync()
        {
            try
            {
                // 🚨 Validate Required Fields
                if (string.IsNullOrWhiteSpace(tloanid.Text) ||
                    string.IsNullOrWhiteSpace(tname.Text) ||
                    string.IsNullOrWhiteSpace(tloanbal.Text) ||
                    string.IsNullOrWhiteSpace(tcolpayamt.Text))
                {
                    MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // 🛠 Helper function to parse currency safely
                decimal ParseCurrency(string text)
                {
                    text = text.Replace("₱", "").Replace(",", "").Trim();
                    return decimal.TryParse(text, out decimal value) ? value : 0;
                }

                // ✅ Parse numeric values safely
                decimal loanAmount = ParseCurrency(tloanamt.Text);
                decimal collectionPaymentAmount = ParseCurrency(tcolpayamt.Text);
                string loanIdNo = tloanid.Text.Trim();

                // 🔍 Retrieve loan information from loan_disbursed collection
                var loanInfo = GetLoanDisbursementInfo(loanIdNo);

                if (loanInfo == null)
                {
                    MessageBox.Show($"Loan disbursement not found for Loan ID: {loanIdNo}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // ✅ Retrieve Loan Interest Rate & Term
                decimal loanInterestRate = loanInfo.Contains("LoanInterest")
                    ? decimal.Parse(loanInfo["LoanInterest"].ToString().Replace("%", "").Trim()) / 100
                    : 0m;  // Default 0%

                int loanTerm = loanInfo.Contains("LoanTerm")
                    ? int.Parse(loanInfo["LoanTerm"].ToString().Split(' ')[0])
                    : 0; // Default 0 days

                // 📌 Calculate Total Loan to Pay
                decimal totalLoanToPay = loanAmount + (loanAmount * loanInterestRate);

                // 🔄 Retrieve Previous Running Balance
                decimal previousRunningBalance = GetLatestRunningBalance(loanIdNo);

                // 📊 Calculate Amortization Breakdown
                decimal dailyAmortization = totalLoanToPay / loanTerm;
                decimal amortizationInterest = (loanAmount * loanInterestRate) / loanTerm;
                decimal amortizationPrincipal = dailyAmortization - amortizationInterest;

                // 📌 Allocate Payment Between Interest & Principal
                decimal allocatedInterest = Math.Min(collectionPaymentAmount, amortizationInterest);
                decimal remainingPayment = collectionPaymentAmount - allocatedInterest;
                decimal allocatedPrincipal = Math.Min(remainingPayment, amortizationPrincipal);

                // 🔄 Update Running Balance
                decimal runningBalance = previousRunningBalance - collectionPaymentAmount;

                // 🚨 Validate Payment Amount
                if (collectionPaymentAmount > totalLoanToPay)
                {
                    MessageBox.Show("The payment amount exceeds the total loan amount to pay.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // 📝 Prepare Loan Collection Document
                var loanCollectionDocument = new BsonDocument
                 {
                     { "ClientNo", laccountid.Text.Trim() },
                     { "LoanID", loanIdNo },
                     { "Name", tname.Text.Trim() },
                     { "ClientNumber", tclientno.Text.Trim() },
                     { "Address", taddress.Text.Trim() },
                     { "Contact", tcontact.Text.Trim() },
                     { "LoanAmount", loanAmount },
                     { "LoanTerm", loanTerm },
                     { "PaymentStartDate", loanInfo.GetValue("PaymentStartDate", string.Empty).ToString() },
                     { "PaymentMaturityDate", tpaymature.Text.Trim() },
                     { "PaymentsMode", tpaymode.Text.Trim() },
                     { "Amortization", dailyAmortization },
                     { "AmortizationPrincipal", amortizationPrincipal },
                     { "AmortizationInterest", amortizationInterest },
                     { "PaymentStatus", tpaymentstatus.Text.Trim() },
                     { "CollectionDate", dtdate.Value },
                     { "Collector", cbcollector.SelectedItem?.ToString() ?? string.Empty },
                     { "PaymentMode", cbpaymentmode.SelectedItem?.ToString() ?? string.Empty },
                     { "PrincipalDue", allocatedPrincipal },
                     { "CollectedInterest", allocatedInterest },
                     { "TotalCollected", collectionPaymentAmount },
                     { "ActualCollection", Math.Min(collectionPaymentAmount, totalLoanToPay) },
                     { "CollectionReferenceNo", tcolrefno.Text.Trim() },
                     { "DateReceived", dtdate.Value },
                     { "CollectionPayment", collectionPaymentAmount },
                     { "RunningBalance", runningBalance },
                     { "TotalLoanToPay", totalLoanToPay },
                     { "Bank", tcolbank.Text.Trim() },
                     { "Branch", tcolbranch.Text.Trim() },
                     { "InterestPaid", allocatedInterest },
                     { "PrincipalPaid", allocatedPrincipal },
                     { "PrincipalBalance", Math.Max(0, totalLoanToPay - collectionPaymentAmount) } // Ensure non-negative balance
                 };

                // 📌 Save to MongoDB
                await _loanCollectionsCollection.InsertOneAsync(loanCollectionDocument);

              
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }





        // Function to retrieve loan information from the loan_disbursed collection
        private BsonDocument GetLoanDisbursementInfo(string loanIdNo)
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_disbursed");

                var filter = Builders<BsonDocument>.Filter.Eq("LoanNo", loanIdNo);
                var loanDisbursed = collection.Find(filter).FirstOrDefault();

                if (loanDisbursed != null)
                {
                    return loanDisbursed;
                }
                else
                {
                    MessageBox.Show($"Loan disbursement not found for LoanIDNo: {loanIdNo}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return new BsonDocument(); // Return empty document if no data is found
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving loan disbursement info: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new BsonDocument(); // Return empty document if an exception occurs
            }
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
            GenerateCollectionId();
            LoadUserInfo(loggedInUsername);

            string clientNo = clientnotest.Text;
            await LoadClientName(clientNo);

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

                    GenerateCollectionId();

                    await Task.Delay(500); // ✅ Wait for DB update
                    _parentForm.LoadLoanCollections(); // ✅ Reload updated data
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


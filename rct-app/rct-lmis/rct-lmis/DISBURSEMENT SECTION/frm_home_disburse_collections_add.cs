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
        private frm_home_disburse_collections _parentForm;
        private List<string> _accountIdList;
        private string name;
        private string loggedInUsername;

        public frm_home_disburse_collections_add(frm_home_disburse_collections parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
            dtdate.Value = DateTime.Now;
            loggedInUsername = UserSession.Instance.CurrentUser;
            // Initialize MongoDB connection for loan_approved collection
            var database = MongoDBConnection.Instance.Database;
            _loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");
            _loanCollectionsCollection = database.GetCollection<BsonDocument>("loan_collections");
            _loanDisbursedCollection = database.GetCollection<BsonDocument>("loan_disbursed");
            _loanCollectorsCollection = database.GetCollection<BsonDocument>("loan_collectors");
            _loanAccountCollection = database.GetCollection<BsonDocument>("loan_account_data");
            // Load AccountId values for autocomplete
            LoadAccountIdsForAutocomplete();

            // Set up the tlnno TextBox for autocomplete
            SetupAutocompleteForTlnno();
            LoadPaymentModes();
            LoadAreaRoutes();

            bsave.Enabled = false;
            bcancel.Enabled = false;
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
            tcoldaterec.Text = string.Empty;
            tcolpayamt.Text = string.Empty;
            tcolbank.Text = string.Empty;
            tcolbranch.Text = string.Empty;

            bsave.Enabled = false;
            bcancel.Enabled = false;
        }

        private void LoadAreaRoutes()
        {
            try
            {
                // Clear the combo box items
                cbarea.Items.Clear();

                // Add the default item
                cbarea.Items.Add("--select area--");

                // Set the default selected item
                cbarea.SelectedIndex = 0;

                // Get all documents from loan_collectors collection
                var collectors = _loanCollectorsCollection.Find(new BsonDocument()).ToList();

                // Loop through the collectors and add their AreaRoute to the combo box
                foreach (var collector in collectors)
                {
                    string areaRoute = collector.GetValue("AreaRoute").AsString;
                    cbarea.Items.Add(areaRoute);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading area routes: " + ex.Message);
            }
        }

        private void LoadCollectorsByAreaRoute(string areaRoute)
        {
            try
            {
                // Clear the cbcollector combo box
                cbcollector.Items.Clear();

                // Add default item
                cbcollector.Items.Add("--select collector--");
                cbcollector.SelectedIndex = 0;

                // Check if a valid area route is selected
                if (areaRoute != "--select area--")
                {
                    // Set up a filter to match the selected AreaRoute
                    var filter = Builders<BsonDocument>.Filter.Eq("AreaRoute", areaRoute);

                    // Get collectors that match the selected AreaRoute
                    var collectors = _loanCollectorsCollection.Find(filter).ToList();

                    // Loop through the filtered collectors and add their names to cbcollector
                    foreach (var collector in collectors)
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

        private decimal ConvertToDecimal(string currencyString)
        {
            // Remove the '₱' symbol and commas, then convert to decimal
            return decimal.Parse(currencyString.Replace("₱", "").Replace(",", "").Trim());
        }

        private void LoadAccountIdsForAutocomplete()
        {
            // Retrieve the cashName field from the loan_approved collection
            var borrowers = _loanDisbursedCollection.Find(new BsonDocument())
                                                   .Project(Builders<BsonDocument>.Projection
                                                   .Include("cashName"))
                                                   .ToList();

            _accountIdList = new List<string>();

            foreach (var doc in borrowers)
            {
                // Check if cashName field exists
                if (doc.Contains("cashName"))
                {
                    string cashName = doc["cashName"].AsString;

                    // Add the cashName to the list
                    if (!string.IsNullOrEmpty(cashName))
                    {
                        _accountIdList.Add(cashName);
                    }
                }
            }
        }

        private void SetupAutocompleteForTlnno()
        {
            AutoCompleteStringCollection accountIdCollection = new AutoCompleteStringCollection();
            accountIdCollection.AddRange(_accountIdList.ToArray());

            tname.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tname.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tname.AutoCompleteCustomSource = accountIdCollection;
        }

        private void LoadLoanApprovedDataByClientNo(string clientNumber)
        {
            try
            {
                // Trim the client number to avoid issues with extra spaces
                clientNumber = clientNumber.Trim();

                // Query to find the document based on the ClientNumber
                var filter = Builders<BsonDocument>.Filter.Eq("ClientNumber", clientNumber);

                // Find the document in the loan_approved collection
                var loanApproved = _loanApprovedCollection.Find(filter).FirstOrDefault();

                if (loanApproved != null)
                {
                    // Populate textboxes with data from the loan_approved document
                    tlnno.Text = loanApproved["AccountId"].AsString;
                    //tclientno.Text = loanApproved["ClientNumber"].AsString;

                    // Full name from FirstName, MiddleName, LastName, SuffixName
                    string retrievedFullName = $"{loanApproved["FirstName"].AsString} {loanApproved["MiddleName"].AsString} {loanApproved["LastName"].AsString} {loanApproved["SuffixName"].AsString}";
                    tname.Text = retrievedFullName.Trim(); // Trim to remove any extra spaces

                    // Address from Street, Barangay, City, Province
                    string fullAddress = $"{loanApproved["Street"].AsString}, {loanApproved["Barangay"].AsString}, {loanApproved["City"].AsString}, {loanApproved["Province"].AsString}";
                    taddress.Text = fullAddress;

                    tcontact.Text = loanApproved["CP"].AsString;
                }
                else
                {
                    // Clear text boxes if no data is found
                    tlnno.Text = string.Empty;
                    tname.Text = string.Empty;
                    taddress.Text = string.Empty;
                    tcontact.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateCollectionNo(string loanId)
        {
            // Query to find the latest collection for the loanId
            var filter = Builders<BsonDocument>.Filter.Eq("LoanIDNo", loanId);
            var sort = Builders<BsonDocument>.Sort.Descending("CollectionNo");

            var latestCollection = _loanCollectionsCollection.Find(filter)
                                                             .Sort(sort)
                                                             .FirstOrDefault();

            string newCollectionNo;

            if (latestCollection != null && latestCollection.Contains("CollectionNo"))
            {
                // Get the latest CollectionNo (e.g., RCT-LNR1-COL-00001)
                string latestCollectionNo = latestCollection["CollectionNo"].AsString;

                // Split and increment the last part (the number after "COL-")
                string[] parts = latestCollectionNo.Split('-');
                int collectionNumber = int.Parse(parts.Last()); // Get the last part as an integer
                collectionNumber++; // Increment the collection number

                // Rebuild the new Collection No. e.g., RCT-LNR1-COL-00002
                newCollectionNo = $"{parts[0]}-{parts[1]}-{parts[2]}-{collectionNumber:D5}";
            }
            else
            {
                // If there are no previous collections, start from 00001
                newCollectionNo = $"{loanId}-COL-00001";
            }

            // Set the generated Collection No to laccountid label
            laccountid.Text = newCollectionNo;
        }

        private void LoadLoanDisbursedData(string clientNumber)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("cashName", clientNumber);
                var loanDisbursed = _loanDisbursedCollection.Find(filter).FirstOrDefault();

                if (loanDisbursed != null)
                {
                    try
                    {
                        // Populate textboxes with data
                        tclientno.Text = loanDisbursed.GetValue("cashClnNo", "").ToString();
                        tloanid.Text = loanDisbursed.GetValue("LoanIDNo", "").ToString();
                        tloanamt.Text = loanDisbursed.GetValue("loanAmt", "").ToString();
                        tterm.Text = loanDisbursed.GetValue("loanTerm", "").ToString();
                        tpaystart.Text = loanDisbursed.GetValue("PaymentStartDate", "").ToString();
                        tpaymode.Text = loanDisbursed.GetValue("Mode", "").ToString();
                        tpayamort.Text = loanDisbursed.GetValue("amortizedAmt", "").ToString();
                        tcolpaid.Text = loanDisbursed.GetValue("amortizedAmt", "").ToString();
                        tcoltotal.Text = loanDisbursed.GetValue("amortizedAmt", "").ToString();

                        // Convert and clean currency values
                        decimal cashAmt = ConvertToDecimal(loanDisbursed.GetValue("cashAmt", "0").ToString());
                        decimal loanInterestAmt = ConvertToDecimal(loanDisbursed.GetValue("loanInterestAmt", "0").ToString());
                        int days = int.Parse(loanDisbursed.GetValue("days", "0").ToString());

                        // Compute Principal Due and Interest
                        decimal principalDue = cashAmt / days;
                        decimal interestDue = loanInterestAmt / days;

                        // Set computed values with Philippine Peso sign
                        tprincipaldue.Text = principalDue.ToString("C", new CultureInfo("en-PH"));
                        tcolinterest.Text = interestDue.ToString("C", new CultureInfo("en-PH"));

                        // Automatically compute interest, maturity date, and balance
                        ComputeInterestAndMaturity();
                        ComputeLoanBalance();
                    }
                    catch (Exception ex) 
                    {
                        MessageBox.Show(this, ex.Message);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading loan disbursed data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Exception in LoadLoanDisbursedData: {ex}");
            }
        }

        private string GetPaymentStatus(decimal loanPaid, decimal loanAmount)
        {
            if (loanPaid >= loanAmount)
            {
                return "Paid";
            }
            else if (loanPaid > 0 && loanPaid < loanAmount)
            {
                return "Payment Complete";
            }
            else
            {
                return "Due";
            }
        }

        private void ComputeInterestAndMaturity()
        {
            try
            {
                if (int.TryParse(tterm.Text, out int loanTermInMonths))
                {
                    // Calculate maturity date based on the start date and term in months (convert to days)
                    DateTime startDate = DateTime.Parse(tpaystart.Text); // Payment start date from the textbox
                    int totalDays = ConvertMonthsToDays(loanTermInMonths);

                    DateTime maturityDate = CalculateMaturityDate(startDate, totalDays);

                    // Display Maturity Date in dd/MM/yyyy format
                    tpaymature.Text = maturityDate.ToString("MM/dd/yyyy");
                }
                else
                {
                    tcolinterest.Text = string.Empty;
                    tpaymature.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error computing interest or maturity date: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private int ConvertMonthsToDays(int months)
        {
            // Assume 30 days per month and ignore the 31st day if it exists
            return months * 30;
        }

        private DateTime CalculateMaturityDate(DateTime startDate, int days)
        {
            DateTime currentDate = startDate;
            int addedDays = 0;

            while (addedDays < days)
            {
                currentDate = currentDate.AddDays(1);

                // Skip weekends (Saturday and Sunday)
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    addedDays++;
                }
            }

            return currentDate;
        }

        // Compute Loan Balance: Loan Amount - Payment
        private void ComputeLoanBalance()
        {
            try
            {
                // Ensure Account ID is populated
                if (string.IsNullOrEmpty(laccountid.Text))
                {
                    MessageBox.Show("Account ID is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Retrieve the loan disbursed data based on LoanID
                var filter = Builders<BsonDocument>.Filter.Eq("LoanIDNo", tloanid.Text);
                var loanDisbursed = _loanDisbursedCollection.Find(filter).FirstOrDefault();

                if (loanDisbursed != null)
                {
                    try
                    {
                        // Parse values from the loan_disbursed collection
                        decimal cashAmt = loanDisbursed["loanAmt"].ToDecimal();
                        decimal loanInterest = decimal.Parse(loanDisbursed["loanInterest"].ToString().TrimEnd('%'));
                        decimal loanTerm = decimal.Parse(loanDisbursed["loanTerm"].ToString());

                        // Convert loanInterest to percentage
                        decimal interestRate = loanInterest / 100;
                        decimal interestAmount = cashAmt * interestRate * loanTerm;

                        // Retrieve latest collections records
                        var collectionFilter = Builders<BsonDocument>.Filter.Eq("LoanID", tloanid.Text);
                        var collections = _loanCollectionsCollection
                            .Find(collectionFilter)
                            .Sort(Builders<BsonDocument>.Sort.Descending("CollectionDate"))
                            .ToList();

                        // Check if any collections exist
                        if (collections.Count > 0)
                        {
                            // Get the latest running balance from the most recent collection
                            decimal runningBalance = collections.First()["RunningBalance"].ToDecimal(); // Use First() since sorted in descending order
                            tloanbal.Text = runningBalance.ToString("C", new CultureInfo("en-PH"));
                        }
                        else
                        {
                            // If no collections, compute initial balance
                            decimal runningBalance = (cashAmt + interestAmount);
                            tloanbal.Text = runningBalance.ToString("C", new CultureInfo("en-PH"));
                        }

                        // Calculate total paid
                        decimal totalPaid = collections.Sum(c => c["CollectionPayment"].ToDecimal());

                        // Get payment status
                        string paymentStatus = GetPaymentStatus(totalPaid, cashAmt + interestAmount);
                        tpaymentstatus.Text = paymentStatus;

                        // Debugging output
                        Console.WriteLine($"Total Loan Balance: {tloanbal.Text}, Payment Status: {paymentStatus}");
                    }
                    catch (FormatException fex)
                    {
                        MessageBox.Show($"Error parsing loan details: {fex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Console.WriteLine($"Error parsing values: {fex}");
                    }
                }
                else
                {
                    MessageBox.Show("Loan details not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine("Loan details not found for the given LoanIDNo.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error computing loan balance: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Exception caught in ComputeLoanBalance: {ex}");
            }
        }



        private void GenerateCollectionId()
        {
            // Get the AccountId from the loan_approved collection based on the laccountid.Text
            var filterApproved = Builders<BsonDocument>.Filter.Eq("AccountId", laccountid.Text);
            var loanApproved = _loanApprovedCollection.Find(filterApproved).FirstOrDefault();

            if (loanApproved != null)
            {
                // Extracting the base AccountId (e.g., "RCT-2024-0001")
                string accountId = loanApproved["AccountId"].ToString().Split(new string[] { "-COL-" }, StringSplitOptions.None)[0];

                // Find the last collection for this AccountId in loan_collections
                var filterCollections = Builders<BsonDocument>.Filter.Regex("AccountId", new BsonRegularExpression(accountId + "-COL-")); // Correctly uses a string
                var sort = Builders<BsonDocument>.Sort.Descending("AccountId");
                var lastCollection = _loanCollectionsCollection.Find(filterCollections).Sort(sort).FirstOrDefault();

                // Extract and increment the collection number
                int collectionNumber = 1; // Default if no collections exist yet
                if (lastCollection != null)
                {
                    string lastCollectionId = lastCollection["AccountId"].ToString();
                    string lastNumberStr = lastCollectionId.Substring(lastCollectionId.LastIndexOf("-COL-") + 5);
                    collectionNumber = int.Parse(lastNumberStr) + 1; // Increment the last number
                }

                // Format the new CollectionId
                string newCollectionId = $"{accountId}-COL-{collectionNumber:D4}"; // Ensures the number is formatted to four digits

                // Assign the new CollectionId to the laccountid label
                laccountid.Text = newCollectionId; // Update the label with the new ID
            }
            else
            {
                Console.WriteLine("AccountId not found in loan_approved.");
            }
        }


        private bool SaveLoanCollectionData()
        {
            try
            {
                // Perform validation on required fields
                if (string.IsNullOrWhiteSpace(tloanid.Text) || string.IsNullOrWhiteSpace(tname.Text) ||
                    string.IsNullOrWhiteSpace(tclientno.Text) || string.IsNullOrWhiteSpace(tlnno.Text) ||
                    string.IsNullOrWhiteSpace(tloanbal.Text) || string.IsNullOrWhiteSpace(tcolpayamt.Text))
                {
                    MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Helper function to remove "₱" and commas and convert to decimal
                decimal ParseCurrency(string text)
                {
                    text = text.Replace("₱", "").Replace(",", "").Trim();
                    return decimal.TryParse(text, out decimal value) ? value : 0;
                }

                // Validate and safely convert numeric fields
                decimal loanAmount = ParseCurrency(tloanamt.Text);
                decimal collectionPaymentAmount = ParseCurrency(tcolpayamt.Text);

                // Get loan interest rate from loan_disbursed collection
                var loanIdNo = tloanid.Text.Trim();
                var loanInfo = GetLoanDisbursementInfo(loanIdNo);

                if (loanInfo == null)
                {
                    MessageBox.Show("Loan information not found for the given Loan ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // Retrieve the loan interest rate
                decimal loanInterestRate = decimal.Parse(loanInfo["loanInterest"].ToString().Replace("%", "").Trim()) / 100; // Convert to decimal

                int loanTerm = int.Parse(loanInfo["loanTerm"].ToString()); // Loan Term in months

                // Calculate Total Loan to Pay
                decimal totalLoanToPay = loanAmount + (loanAmount * loanInterestRate * loanTerm);

                // 1. Retrieve the latest RunningBalance for this loan from loan_collections
                decimal previousRunningBalance = GetLatestRunningBalance(loanIdNo);

                // 2. Calculate the new running balance (previous running balance - amount paid)
                decimal runningBalance = previousRunningBalance - collectionPaymentAmount;

                // Create a new BsonDocument to store the form data
                var loanCollectionDocument = new BsonDocument
                 {
                     { "AccountId", laccountid.Text.Trim() },
                     { "LoanID", loanIdNo },
                     { "Name", tname.Text.Trim() },
                     { "ClientNumber", tclientno.Text.Trim() },
                     { "LoanNumber", tlnno.Text.Trim() },
                     { "Address", taddress.Text.Trim() },
                     { "Contact", tcontact.Text.Trim() },
                     { "LoanAmount", loanAmount },
                     { "LoanTerm", loanTerm },
                     { "PaymentStartDate", loanInfo["PaymentStartDate"].ToString() },
                     { "PaymentMaturityDate", tpaymature.Text.Trim() },
                     { "PaymentsMode", tpaymode.Text.Trim() },
                     { "Amortization", ParseCurrency(tpayamort.Text) },
                     { "PaymentStatus", tpaymentstatus.Text.Trim() },
                     { "CollectionDate", dtdate.Value },
                     { "Area", cbarea.SelectedItem?.ToString() ?? string.Empty },
                     { "Collector", cbcollector.SelectedItem?.ToString() ?? string.Empty },
                     { "PaymentMode", cbpaymentmode.SelectedItem?.ToString() ?? string.Empty },
                     { "PrincipalDue", ParseCurrency(tprincipaldue.Text) },
                     { "CollectedInterest", ParseCurrency(tcolinterest.Text) },
                     { "CollectedPenalty", ParseCurrency(tcolpenalty.Text) },
                     { "TotalCollected", ParseCurrency(tcoltotal.Text) },
                     { "ActualCollection", ParseCurrency(tcolactual.Text) },
                     { "CollectionReferenceNo", tcolrefno.Text.Trim() },
                     { "DateReceived", tcoldaterec.Text.Trim() },
                     { "CollectionPayment", collectionPaymentAmount },
                     { "RunningBalance", runningBalance },
                     { "TotalLoanToPay", totalLoanToPay },  // Add Total Loan to Pay here
                     { "Bank", tcolbank.Text.Trim() },
                     { "Branch", tcolbranch.Text.Trim() }
                 };

                _loanCollectionsCollection.InsertOne(loanCollectionDocument);

                return true;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"An error occurred while saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Function to retrieve loan information from the loan_disbursed collection
        private BsonDocument GetLoanDisbursementInfo(string loanIdNo)
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<BsonDocument>("loan_disbursed");

            var filter = Builders<BsonDocument>.Filter.Eq("LoanIDNo", loanIdNo);
            return collection.Find(filter).FirstOrDefault();
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

                // If there is no previous collection, assume a running balance of 0 or the initial loan balance
                if (latestCollection == null)
                {
                    return ParseCurrency(tloanbal.Text);  // Default to initial loan balance if no prior collection is found
                }

                // Ensure RunningBalance exists and retrieve its value
                if (latestCollection.Contains("RunningBalance"))
                {
                    // Convert the RunningBalance to decimal if it exists
                    decimal previousRunningBalance = ((BsonDecimal128)latestCollection["RunningBalance"]).ToDecimal();
                    return previousRunningBalance;
                }
                else
                {
                    // Return 0 if RunningBalance does not exist
                    return 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving latest running balance: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        // Helper method to parse currency
        private decimal ParseCurrency(string text)
        {
            text = text.Replace("₱", "").Replace(",", "").Trim();
            return decimal.TryParse(text, out decimal value) ? value : 0;
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


        private void frm_home_disburse_collections_add_Load(object sender, EventArgs e)
        {
            GenerateCollectionId();
            LoadUserInfo(loggedInUsername);
        }

        private void bcopyaccno_Click(object sender, EventArgs e)
        {
            string accNo = laccountid.Text;
            Clipboard.SetText(accNo);
            MessageBox.Show("The account number has been copied to your clipboard.", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tclientno_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tclientno.Text.Trim()))
            {
                LoadLoanApprovedDataByClientNo(tclientno.Text.Trim());
            }
        }

        private void cbarea_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected AreaRoute
            string selectedAreaRoute = cbarea.SelectedItem.ToString();

            // Load collectors based on the selected AreaRoute
            LoadCollectorsByAreaRoute(selectedAreaRoute);

            cbcollector.Enabled = true;
        }

        private void cbpaymentmode_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected payment mode
            string selectedPaymentMode = cbpaymentmode.SelectedItem.ToString();

            if (selectedPaymentMode == "Cash")
            {
                // Show Reference No. and Date of Payment Received
                tcolrefno.Visible = true;
                tcoldaterec.Visible = true;
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

                tcoldaterec.Visible = true;
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

                tcoldaterec.Visible = true;
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

                tcoldaterec.Visible = false;
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
            tcoldaterec.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");

            if (string.IsNullOrEmpty(tcolpayamt.Text)) 
            {
                bamtfull.Visible=false;
            }
        }

        private void tname_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tname.Text))
            {
                ClearAndDisableFields();

                tclientno.Text = string.Empty;
                tname.Text = string.Empty;
                taddress.Text = string.Empty;
                tcontact.Text = string.Empty;

            }
            else 
            {
                LoadLoanDisbursedData(tname.Text);
                
            }
        }

        private void bsave_Click(object sender, EventArgs e)
        {
            // Show a confirmation prompt
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to save the loan collection data?", "Confirm Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // If the user clicks 'Yes', proceed with saving
            if (dialogResult == DialogResult.Yes)
            {
                bool isSaved = SaveLoanCollectionData();

                if (isSaved)
                {
                    decimal principalPaid = ParseCurrency(tcolpaid.Text);
                    decimal interestPaid = CalculateInterestPaid();
                    string reference = GetLoanIDNo();

                    // Save loan account data
                    SaveLoanAccountData(principalPaid, interestPaid, reference);

                    MessageBox.Show("Loan collection data and account data saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    _parentForm.LoadLoanCollections();
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
                tlnno.Text = "";
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
                tcoldaterec.Text = "";
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
    }
}

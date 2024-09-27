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

        private List<string> _accountIdList; // To store AccountId values

        public frm_home_disburse_collections_add()
        {
            InitializeComponent();

            dtdate.Value = DateTime.Now;

            // Initialize MongoDB connection for loan_approved collection
            var database = MongoDBConnection.Instance.Database;
            _loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");
            _loanCollectionsCollection = database.GetCollection<BsonDocument>("loan_collections");
            _loanDisbursedCollection = database.GetCollection<BsonDocument>("loan_disbursed");
            _loanCollectorsCollection = database.GetCollection<BsonDocument>("loan_collectors");

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
                        int days = int.Parse(loanDisbursed.GetValue("days", "0").ToString());  // Assuming days is the loan term

                        // Compute Principal Due and Interest
                        decimal principalDue = cashAmt / days;
                        decimal interestDue = loanInterestAmt / days;

                        // Set computed values with Philippine Peso sign
                        tprincipaldue.Text = principalDue.ToString("C", new System.Globalization.CultureInfo("en-PH"));
                        tcolinterest.Text = interestDue.ToString("C", new System.Globalization.CultureInfo("en-PH"));

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
                return "Partially Paid";
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
                // Debug hint to check if laccountid.Text is populated
                if (string.IsNullOrEmpty(laccountid.Text))
                {
                    MessageBox.Show("Account ID is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Retrieve the loan disbursed data based on the LoanID
                var filter = Builders<BsonDocument>.Filter.Eq("LoanIDNo", tloanid.Text);
                var loanDisbursed = _loanDisbursedCollection.Find(filter).FirstOrDefault();

                if (loanDisbursed != null)
                {
                    try
                    {
                        // Parse the necessary values from the loan_disbursed collection
                        decimal cashAmt = loanDisbursed["loanAmt"].ToDecimal();  // e.g., 15000
                        decimal loanInterest = decimal.Parse(loanDisbursed["loanInterest"].ToString().TrimEnd('%'));  // Convert interest like "5%" to 5
                        decimal loanTerm = decimal.Parse(loanDisbursed["loanTerm"].ToString());  // Convert term like "4" to decimal

                        // Convert loanInterest to percentage
                        decimal interestRate = loanInterest / 100;

                        // Calculate the interest amount: cashAmt * loanInterest * loanTerm
                        decimal interestAmount = cashAmt * interestRate * loanTerm;

                        // Debug hint to check interest amount
                        Console.WriteLine($"Interest Amount: {interestAmount}");

                        // Retrieve the latest collections records
                        var collectionFilter = Builders<BsonDocument>.Filter.Eq("LoanID", tloanid.Text);
                        var collections = _loanCollectionsCollection.Find(collectionFilter).Sort(Builders<BsonDocument>.Sort.Descending("CollectionDate")).ToList();

                        decimal totalPaid = collections.Sum(c => c["CollectionPayment"].ToDecimal()); // Assuming "CollectionPayment" field for the payments

                        decimal runningBalance = 0;

                        // Check if there are any collections
                        if (collections.Count > 0)
                        {
                            // Get the latest running balance from the most recent collection
                            runningBalance = collections.Last()["RunningBalance"].ToDecimal();
                        }
                        else
                        {
                            // If no records, compute the initial balance
                            runningBalance = (cashAmt + interestAmount) - totalPaid;
                        }

                        // Display the computed loan balance in tloanbal
                        tloanbal.Text = runningBalance.ToString("C", new CultureInfo("en-PH"));

                        // Get the payment status based on totalPaid and loan amount + interest
                        string paymentStatus = GetPaymentStatus(totalPaid, cashAmt + interestAmount);

                        // Display payment status in tpaymentstatus.Text
                        tpaymentstatus.Text = paymentStatus;

                        // Debug hint for balance and payment status
                        Console.WriteLine($"Total Loan Balance: {runningBalance}, Payment Status: {paymentStatus}");
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
                string accountId = loanApproved["AccountId"].ToString();  // RCT-2024-0001 part

                // Find the last collection for this AccountId in loan_collections
                var filterCollections = Builders<BsonDocument>.Filter.Regex("CollectionId", new BsonRegularExpression(accountId + "-COL-"));
                var sort = Builders<BsonDocument>.Sort.Descending("CollectionId");
                var lastCollection = _loanCollectionsCollection.Find(filterCollections).Sort(sort).FirstOrDefault();

                // Extract and increment the collection number
                int collectionNumber = 1;  // Default if no collections exist yet
                if (lastCollection != null)
                {
                    string lastCollectionId = lastCollection["CollectionId"].ToString();
                    string lastNumberStr = lastCollectionId.Substring(lastCollectionId.LastIndexOf("-COL-") + 5);
                    collectionNumber = int.Parse(lastNumberStr) + 1;
                }

                // Format the new CollectionId
                string newCollectionId = $"{accountId}-COL-{collectionNumber:D4}";

                // Assign the new CollectionId to the laccountid label
                laccountid.Text = newCollectionId;
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
                decimal amortization = ParseCurrency(tpayamort.Text);
                decimal collectionPaymentAmount = ParseCurrency(tcolpayamt.Text);

                // 1. Retrieve the latest RunningBalance for this loan from loan_collections
                decimal previousRunningBalance = GetLatestRunningBalance(tloanid.Text);

                // 2. Calculate the new running balance (previous running balance - amount paid)
                decimal runningBalance = previousRunningBalance - collectionPaymentAmount;

                // Create a new BsonDocument to store the form data
                var loanCollectionDocument = new BsonDocument
                 {
                     { "AccountId", laccountid.Text.Trim() },
                     { "LoanID", tloanid.Text.Trim() },
                     { "Name", tname.Text.Trim() },
                     { "ClientNumber", tclientno.Text.Trim() },
                     { "LoanNumber", tlnno.Text.Trim() },
                     { "Address", taddress.Text.Trim() },
                     { "Contact", tcontact.Text.Trim() },
                     { "LoanAmount", loanAmount },
                     { "LoanTerm", tterm.Text.Trim() },
                     { "PaymentStartDate", tpaystart.Text.Trim() },
                     { "PaymentMaturityDate", tpaymature.Text.Trim() },
                     { "PaymentsMode", tpaymode.Text.Trim() },
                     { "Amortization", amortization },
                     { "PaymentStatus", tpaymentstatus.Text.Trim() },

                     // DateTime picker for dtdate (use appropriate format or conversion)
                     { "CollectionDate", dtdate.Value },

                     // ComboBoxes for cbarea, cbcollector, cbpaymentmode
                     { "Area", cbarea.SelectedItem?.ToString() ?? string.Empty },
                     { "Collector", cbcollector.SelectedItem?.ToString() ?? string.Empty },
                     { "PaymentMode", cbpaymentmode.SelectedItem?.ToString() ?? string.Empty },

                     // Collection details
                     { "PrincipalDue", ParseCurrency(tprincipaldue.Text) },
                     { "CollectedInterest", ParseCurrency(tcolinterest.Text) },
                     { "CollectedPenalty", ParseCurrency(tcolpenalty.Text) },
                     { "TotalCollected", ParseCurrency(tcoltotal.Text) },
                     { "AmountPaid", ParseCurrency(tcolpaid.Text) },
                     { "ActualCollection", ParseCurrency(tcolactual.Text) },
                     { "CollectionReferenceNo", tcolrefno.Text.Trim() },
                     { "DateReceived", tcoldaterec.Text.Trim() },
                     { "CollectionPayment", collectionPaymentAmount },
                     { "RunningBalance", runningBalance },  // 3. Save the new running balance
                     { "Bank", tcolbank.Text.Trim() },
                     { "Branch", tcolbranch.Text.Trim() }
                 };

                // Insert the document into the loan_collections collection
                _loanCollectionsCollection.InsertOne(loanCollectionDocument);

                // If the insert was successful, return true
                return true;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"An error occurred while saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
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

                // If there is no previous collection, assume a running balance of 0 or the initial loan balance
                if (latestCollection == null)
                {
                    return ParseCurrency(tloanbal.Text);  // Default to initial loan balance if no prior collection is found
                }

                decimal previousRunningBalance = latestCollection.Contains("RunningBalance")
                    ? ((BsonDecimal128)latestCollection["RunningBalance"]).ToDecimal() : 0;

                return previousRunningBalance;
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

        private void frm_home_disburse_collections_add_Load(object sender, EventArgs e)
        {
            GenerateCollectionId();
        }

        private void bcopyaccno_Click(object sender, EventArgs e)
        {
            string accNo = laccountid.Text;
            Clipboard.SetText(accNo);
            MessageBox.Show("The account number has been copied to your clipboard.", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tlnno_TextChanged(object sender, EventArgs e)
        {
          
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

            // Set the current date and time in mm/dd/yyyy hh:mm AM/PM format
            tcoldaterec.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
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
                // Call the SaveLoanCollectionData function
                bool isSaved = SaveLoanCollectionData();

                // If the save was successful, show a success message
                if (isSaved)
                {
                    MessageBox.Show("Loan collection data saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // If the user clicks 'No', you can optionally handle the action here (if needed)
                MessageBox.Show("Save operation cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

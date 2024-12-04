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

        public async  Task LoadClientName(string clientNo)
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
            // Preserve the original full name
            string fullName = tname.Text.Trim();

            try
            {
                if (string.IsNullOrEmpty(fullName))
                {
                    MessageBox.Show("Please enter a valid full name.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Split the full name into first, middle, and last names
                var nameParts = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (nameParts.Length < 2)
                {
                    MessageBox.Show("Full name must include at least a first name and a last name.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string firstName = nameParts[0];
                string lastName = nameParts[nameParts.Length - 1];
                string middleName = nameParts.Length > 2 ? string.Join(" ", nameParts.Skip(1).Take(nameParts.Length - 2)) : "";

                // Create filter for MongoDB query
                var filter = Builders<BsonDocument>.Filter.And(
                    Builders<BsonDocument>.Filter.Eq("FirstName", firstName),
                    Builders<BsonDocument>.Filter.Eq("LastName", lastName)
                );

                // Add middle name to filter if it exists
                if (!string.IsNullOrEmpty(middleName))
                {
                    filter = Builders<BsonDocument>.Filter.And(filter, Builders<BsonDocument>.Filter.Eq("MiddleName", middleName));
                }

                // Query the loan_disbursed collection
                var loanDisbursed = await _loanDisbursedCollection.Find(filter).FirstOrDefaultAsync();

                if (loanDisbursed != null)
                {
                    try
                    {
                        // Populate textboxes with data from the loan_disbursed document
                        tclientno.Text = loanDisbursed.GetValue("ClientNo", "").ToString();
                        tloanid.Text = loanDisbursed.GetValue("LoanNo", "").ToString();
                        tloanamt.Text = loanDisbursed.GetValue("LoanAmount", "").ToString();
                        tterm.Text = loanDisbursed.GetValue("LoanTerm", "").ToString();
                        tpaystart.Text = loanDisbursed.GetValue("StartPaymentDate", "").ToString();
                        //tpaymature.Text = loanDisbursed.GetValue("MaturityDate", "").ToString();
                        tpaymode.Text = loanDisbursed.GetValue("PaymentMode", "").ToString();
                        tpayamort.Text = loanDisbursed.GetValue("LoanAmortization", "").ToString();
                        tcolpaid.Text = loanDisbursed.GetValue("LoanAmortization", "").ToString();
                        tcoltotal.Text = loanDisbursed.GetValue("LoanAmortization", "").ToString();

                        // Extract Address and Contact No
                        string barangay = loanDisbursed.GetValue("Barangay", "").ToString();
                        string city = loanDisbursed.GetValue("City", "").ToString();
                        string address = $"{barangay}, {city}"; // Combine Barangay and City
                        taddress.Text = address; // Assign to Address textbox

                        // Assuming there is a field for Contact No in the loan_disbursed collection
                        tcontact.Text = loanDisbursed.GetValue("ContactNo", "").ToString();

                        // Convert and clean currency values
                        decimal loanAmount = ConvertToDecimal(loanDisbursed.GetValue("LoanAmount", "0").ToString());
                        decimal loanInterest = ConvertToDecimal(loanDisbursed.GetValue("LoanInterest", "0").ToString().TrimEnd('%'));
                        int loanTerm = int.Parse(loanDisbursed.GetValue("LoanTerm", "0").ToString().Split(' ')[0]);

                        // Compute Principal Due and Interest
                        decimal principalDue = loanAmount / loanTerm;
                        decimal interestDue = (loanAmount * (loanInterest / 100)) / loanTerm;

                        // Set computed values with Philippine Peso sign
                        tprincipaldue.Text = principalDue.ToString("C", new CultureInfo("en-PH"));
                        tcolinterest.Text = interestDue.ToString("C", new CultureInfo("en-PH"));

                        // Retrieve the latest payment from loan_collections
                        var latestPayment = GetLatestPayment(loanDisbursed.GetValue("LoanNo", "").ToString());

                        if (latestPayment != null)
                        {
                            decimal actualCollection = ConvertToDecimal(latestPayment.GetValue("ActualCollection", "0").ToString());
                            DateTime dateReceived = latestPayment.GetValue("DateReceived").ToUniversalTime();
                            decimal amortizedAmt = ConvertToDecimal(loanDisbursed.GetValue("LoanAmortization", "0").ToString());
                            decimal previousPrincipalBalance = GetLatestPrincipalBalance(loanDisbursed.GetValue("LoanNo", "").ToString());

                            // Update payment status based on actual collection
                            if (actualCollection >= amortizedAmt)
                            {
                                tpaymentstatus.Text = $"Payment Completed ({dateReceived:MM/dd/yyyy})";
                            }
                            else
                            {
                                decimal balance = previousPrincipalBalance;
                                decimal amtPaid = actualCollection;
                                tpaymentstatus.Text = $"Payment not cleared ({dateReceived:MM/dd/yyyy}) with balance {balance.ToString("C", new CultureInfo("en-PH"))}";
                                tcolpaid.Text = amtPaid.ToString("C", new CultureInfo("en-PH"));
                            }
                        }
                        else
                        {
                            tpaymentstatus.Text = "No payments made yet";
                        }

                      
                        ComputeLoanBalance();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.Message);
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
                Console.WriteLine($"Exception in LoadLoanDisbursedData: {ex}");
            }
        }


        private BsonDocument GetLatestPayment(string loanId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("LoanNo", loanId); // Use "LoanNo" field as per the sample data
            var sort = Builders<BsonDocument>.Sort.Descending("DateReceived");
            return _loanCollectionsCollection.Find(filter).Sort(sort).FirstOrDefault();
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

                // Parse Start Payment Date
                DateTime startPaymentDate;
                if (!DateTime.TryParse(tpaystart.Text, out startPaymentDate))
                {
                    MessageBox.Show("Start Payment Date format is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DateTime currentDate = DateTime.Now; // Today's Date

                // Loan Amount
                decimal loanAmount;
                if (!decimal.TryParse(tloanamt.Text.Replace("₱", "").Replace(",", ""), out loanAmount))
                {
                    MessageBox.Show("Loan Amount format is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Loan Term (in months) - Get from the loan data and convert to days
                string loanTerm = tterm.Text; // e.g. "4 months"
                int termInDays = 0;

                // Convert loan term from months to days (assuming 30 days in a month)
                if (loanTerm.Contains("month"))
                {
                    int months;
                    if (int.TryParse(loanTerm.Split(' ')[0], out months))
                    {
                        termInDays = months * 20; // 30 days per month
                    }
                    else
                    {
                        MessageBox.Show("Term format is invalid. Please enter a valid term (e.g., '4 months').", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid term format. Please enter term in months.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Calculate the Maturity Date
                DateTime maturityDate = startPaymentDate.AddDays(termInDays);

                // Update the UI with the Maturity Date
                tpaymature.Text = maturityDate.ToString("MM/dd/yyyy");

                // Amortization (daily) - Get from tamort.Text
                decimal amortization;
                if (!decimal.TryParse(tpayamort.Text.Replace("₱", "").Replace(",", ""), out amortization))
                if (!decimal.TryParse(tpayamort.Text.Replace("₱", "").Replace(",", ""), out amortization))
                {
                    MessageBox.Show("Amortization format is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Calculate missed days (excluding Sundays)
                int totalMissedDays = 0;
                for (DateTime date = startPaymentDate; date < currentDate; date = date.AddDays(1))
                {
                    if (date.DayOfWeek != DayOfWeek.Sunday)
                    {
                        totalMissedDays++;
                    }
                }

                if (totalMissedDays == 0)
                {
                    tcolpenalty.Text = "₱0.00"; // No penalty if no missed days
                    tprincipaldue.Text = "₱0.00";
                    tcolinterest.Text = "₱0.00";
                    tcoltotal.Text = "₱0.00";
                    return;
                }

                // Principal Due = Amortization * Missed Days
                decimal principalDue = amortization * totalMissedDays;

                // Interest (assuming daily interest is calculated like amortization)
                decimal interest = amortization * totalMissedDays;

                // Penalty Calculation - Applying 3% penalty on total amortization missed
                decimal penalty = (principalDue + interest) * 0.03m;

                // Total Amortization = Principal Due + Interest + Penalty
                decimal totalAmortization = principalDue + interest + penalty;

                // Update the UI with the calculated values
                tcolpenalty.Text = penalty.ToString("C", new CultureInfo("en-PH"));
                tprincipaldue.Text = principalDue.ToString("C", new CultureInfo("en-PH"));
                tcolinterest.Text = interest.ToString("C", new CultureInfo("en-PH"));
                tcoltotal.Text = totalAmortization.ToString("C", new CultureInfo("en-PH"));

                // Set Loan Balance in the UI
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
                // Ensure the laccountid.Text is not empty
                if (string.IsNullOrEmpty(laccountid.Text))
                {
                    MessageBox.Show("Account ID is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clientnotest.Text = _clientNo;

                // Get the AccountId from the loan_approved collection based on laccountid.Text
                var filterApproved = Builders<BsonDocument>.Filter.Eq("ClientNo", clientnotest.Text); // Use _loanId here
                var loanApproved = _loanApprovedCollection.Find(filterApproved).FirstOrDefault();

                if (loanApproved != null)
                {
                    //Console.WriteLine($"Found loanApproved: {loanApproved.ToJson()}"); // Log the found document

                    // Extracting the base AccountId
                    string accountId = loanApproved["ClientNo"].ToString();

                    // Find the last collection for this AccountId in loan_collections
                    var filterCollections = Builders<BsonDocument>.Filter.Regex("ClientNo", new BsonRegularExpression($"^{accountId}-COL-"));
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
                    string newCollectionId = $"{accountId}-COL-{collectionNumber:D4}"; // Ensures the number is formatted to four digits

                    // Assign the new CollectionId to the laccountid label
                    laccountid.Text = newCollectionId; // Update the label with the new ID
                }
                else
                {
                    MessageBox.Show("Account ID not found in loan_approved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Console.WriteLine("AccountId not found in loan_approved.");
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



        private bool SaveLoanCollectionData()
        {
            try
            {
                // Perform validation on required fields
                if (string.IsNullOrWhiteSpace(tloanid.Text) || string.IsNullOrWhiteSpace(tname.Text) ||
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

                // Get loan information from loan_disbursed collection
                string loanIdNo = tloanid.Text.Trim();
                var loanInfo = GetLoanDisbursementInfo(loanIdNo);

                if (loanInfo == null)
                {
                    MessageBox.Show($"Loan disbursement not found for Loan ID: {loanIdNo}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // Safely retrieve loan interest rate and term
                decimal loanInterestRate = 0;
                int loanTerm = 0;

                if (loanInfo.TryGetValue("LoanInterest", out var loanInterest))
                {
                    loanInterestRate = decimal.Parse(loanInterest.ToString().Replace("%", "").Trim()) / 100; // Convert to decimal
                }
                else
                {
                    MessageBox.Show($"Loan Interest not found for Loan ID: {loanIdNo}. Defaulting to 0%.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (loanInfo.TryGetValue("LoanTerm", out var term))
                {
                    // Parse Loan Term in days (or months depending on your logic)
                    loanTerm = int.Parse(term.ToString().Split(' ')[0]);
                }
                else
                {
                    MessageBox.Show($"Loan Term not found for Loan ID: {loanIdNo}. Defaulting to 0 days.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // Calculate Total Loan to Pay
                decimal totalLoanToPay = loanAmount + (loanAmount * loanInterestRate);

                // Retrieve the latest RunningBalance and previous balance for this loan from loan_collections
                decimal previousRunningBalance = GetLatestRunningBalance(loanIdNo);

                // Calculate Amortization components (principal and interest)
                decimal dailyAmortization = totalLoanToPay / loanTerm; // Total amortization per day
                decimal amortizationInterest = (loanAmount * loanInterestRate) / loanTerm; // Daily interest
                decimal amortizationPrincipal = dailyAmortization - amortizationInterest; // Daily principal

                // Allocate payment to interest and principal based on ActualPayment
                decimal allocatedInterest = Math.Min(collectionPaymentAmount, amortizationInterest);
                decimal remainingPayment = collectionPaymentAmount - allocatedInterest;
                decimal allocatedPrincipal = Math.Min(remainingPayment, amortizationPrincipal);

                // Update balances
                decimal runningBalance = previousRunningBalance - collectionPaymentAmount;

                // Ensure the ActualCollection does not exceed the TotalLoanToPay
                if (collectionPaymentAmount > totalLoanToPay)
                {
                    MessageBox.Show("The payment amount exceeds the total loan amount to pay.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Create a new BsonDocument to store the form data
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
                      { "PrincipalBalance", totalLoanToPay - collectionPaymentAmount }
                  };

                _loanCollectionsCollection.InsertOne(loanCollectionDocument);

                // Save remaining balance if applicable
                if ((totalLoanToPay - collectionPaymentAmount) > 0)
                {
                    SaveRemainingBalance(totalLoanToPay - collectionPaymentAmount, loanIdNo, loanAmount, loanTerm, dailyAmortization);
                }

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
                    GenerateCollectionId();
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
    }
}


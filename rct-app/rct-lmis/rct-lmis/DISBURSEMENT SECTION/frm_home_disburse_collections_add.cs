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
        private List<string> _accountIdList; // To store AccountId values

        public frm_home_disburse_collections_add()
        {
            InitializeComponent();

            // Initialize MongoDB connection for loan_approved collection
            var database = MongoDBConnection.Instance.Database;
            _loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");
            _loanCollectionsCollection = database.GetCollection<BsonDocument>("loan_collections");
            _loanDisbursedCollection = database.GetCollection<BsonDocument>("loan_disbursed");

            // Load AccountId values for autocomplete
            LoadAccountIdsForAutocomplete();

            // Set up the tlnno TextBox for autocomplete
            SetupAutocompleteForTlnno();
        }

        private DateTime CalculateMaturityDate(DateTime startDate, int days)
        {
            DateTime currentDate = startDate;
            int addedDays = 0;

            while (addedDays < days)
            {
                currentDate = currentDate.AddDays(1);
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    addedDays++;
                }
            }

            return currentDate;
        }

        private void LoadAccountIdsForAutocomplete()
        {
            // Retrieve all AccountId values from loan_approved collection
            var accountIds = _loanApprovedCollection.Find(new BsonDocument())
                                                    .Project(Builders<BsonDocument>.Projection.Include("AccountId"))
                                                    .ToList();

            _accountIdList = new List<string>();

            foreach (var doc in accountIds)
            {
                if (doc.Contains("AccountId"))
                {
                    _accountIdList.Add(doc["AccountId"].AsString);
                }
            }
        }

        private void SetupAutocompleteForTlnno()
        {
            AutoCompleteStringCollection accountIdCollection = new AutoCompleteStringCollection();
            accountIdCollection.AddRange(_accountIdList.ToArray());

            tlnno.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tlnno.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tlnno.AutoCompleteCustomSource = accountIdCollection;
        }

        private void LoadLoanApprovedData(string accountId)
        {
            // Query to find the document based on AccountId
            var filter = Builders<BsonDocument>.Filter.Eq("AccountId", accountId);
            var loanApproved = _loanApprovedCollection.Find(filter).FirstOrDefault();

            if (loanApproved != null)
            {
                // Populate textboxes with data from loan_approved document
                tlnno.Text = loanApproved["AccountId"].AsString;
                tclientno.Text = loanApproved["ClientNumber"].AsString;

                // Full name from FirstName, MiddleName, LastName, SuffixName
                string fullName = $"{loanApproved["FirstName"].AsString} {loanApproved["MiddleName"].AsString} {loanApproved["LastName"].AsString} {loanApproved["SuffixName"].AsString}";
                tname.Text = fullName.Trim(); // Trim to remove any extra spaces

                // Address from Street, Barangay, City, Province
                string fullAddress = $"{loanApproved["Street"].AsString}, {loanApproved["Barangay"].AsString}, {loanApproved["City"].AsString}, {loanApproved["Province"].AsString}";
                taddress.Text = fullAddress;

                tcontact.Text = loanApproved["CP"].AsString;
            }
            else
            {
                // Clear text boxes if no data is found
                tclientno.Text = string.Empty;
                tname.Text = string.Empty;
                taddress.Text = string.Empty;
                tcontact.Text = string.Empty;
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
            var filter = Builders<BsonDocument>.Filter.Eq("cashClnNo", clientNumber);
            var loanDisbursed = _loanDisbursedCollection.Find(filter).FirstOrDefault();

            if (loanDisbursed != null)
            {
                // Populate textboxes
                tloanamt.Text = loanDisbursed["loanAmt"].ToString();
                tterm.Text = loanDisbursed["loanTerm"].AsString;
                tpaystart.Text = loanDisbursed["PaymentStartDate"].ToString();
                tpaymode.Text = loanDisbursed["Mode"].ToString();
                tpayamort.Text = loanDisbursed["amortizedAmt"].ToString();

                // Automatically compute interest, maturity date, and balance
                ComputeInterestAndMaturity();
                ComputeLoanBalance();
            }
        }

        private void ClearLoanDisbursedFields()
        {
            tloanamt.Text = string.Empty;
            tterm.Text = string.Empty;
            tpaystart.Text = string.Empty;
            tpaymature.Text = string.Empty;
            tpaymode.Text = string.Empty;
            tpayamort.Text = string.Empty;
            tloanbal.Text = string.Empty;
            tpaymentstatus.Text = string.Empty;
            tcoldue.Text = string.Empty;
            tcolinterest.Text = string.Empty;
            tcoltotal.Text = string.Empty;
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
                if (decimal.TryParse(tloanamt.Text, out decimal loanAmount) && int.TryParse(tterm.Text, out int loanTerm))
                {
                    // Compute interest per term
                    decimal interestPerTerm = loanAmount / loanTerm;
                    tcolinterest.Text = interestPerTerm.ToString("C", new CultureInfo("en-PH")); // Philippine Peso

                    // Calculate maturity date based on the start date and term in days (excluding weekends)
                    DateTime startDate = DateTime.Parse(tpaystart.Text); // Payment start date from the textbox
                    int days = loanTerm; // Convert term to days if needed

                    DateTime maturityDate = CalculateMaturityDate(startDate, days);
                    tpaymature.Text = maturityDate.ToString("MM/dd/yyyy"); // Format the maturity date
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

        // Compute Loan Balance: Loan Amount - Payment
        private void ComputeLoanBalance()
        {
            try
            {
                if (decimal.TryParse(tloanamt.Text, out decimal loanAmount) && decimal.TryParse(tcolpayamt.Text, out decimal payment))
                {
                    decimal loanBalance = loanAmount - payment;
                    tloanbal.Text = loanBalance.ToString("C", new CultureInfo("en-PH")); // Philippine Peso
                }
                else
                {
                    tloanbal.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error computing loan balance: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void frm_home_disburse_collections_add_Load(object sender, EventArgs e)
        {

        }

        private void bcopyaccno_Click(object sender, EventArgs e)
        {
            string accNo = laccountid.Text;
            Clipboard.SetText(accNo);
            MessageBox.Show("The account number has been copied to your clipboard.", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tlnno_TextChanged(object sender, EventArgs e)
        {
            // When the text changes, attempt to load data if the entered text matches an AccountId
            if (_accountIdList.Contains(tlnno.Text))
            {
                LoadLoanApprovedData(tlnno.Text);
                GenerateCollectionNo(tlnno.Text);

            }
            else
            {
                // Clear other text boxes if AccountId not found
                tclientno.Text = string.Empty;
                tname.Text = string.Empty;
                taddress.Text = string.Empty;
                tcontact.Text = string.Empty;
            }
        }

        private void tclientno_TextChanged(object sender, EventArgs e)
        {
            LoadLoanDisbursedData(tclientno.Text);
        }
    }
}

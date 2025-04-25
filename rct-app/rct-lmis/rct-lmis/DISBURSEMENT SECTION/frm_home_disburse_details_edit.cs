using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Driver;

namespace rct_lmis.DISBURSEMENT_SECTION
{
    public partial class frm_home_disburse_details_edit : Form
    {
        private string _loanId;
        private string _accountId;
        private bool _isNewLoan;
        private readonly string _startPaymentDate;
        private IMongoCollection<BsonDocument> _loanDisbursedCollection;
        private IMongoCollection<BsonDocument> _loanRateCollection;
        private IMongoCollection<BsonDocument> _loanApprovedCollection;
        private IMongoCollection<BsonDocument> _loanCollectorsCollection;
        private IMongoCollection<BsonDocument> _loanAccountCyclesCollection;
        private string _clientNo;
        private frm_home_disburse_details parentForm;


        public frm_home_disburse_details_edit(string clientNo, string accountId, string loanNo, bool isNewLoan, string startPaymentDate, frm_home_disburse_details parent)
        {
            InitializeComponent();
            var database = MongoDBConnection.Instance.Database;
            _loanDisbursedCollection = database.GetCollection<BsonDocument>("loan_disbursed");
            _loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");
            _loanCollectorsCollection = database.GetCollection<BsonDocument>("loan_collectors");
            _loanAccountCyclesCollection = database.GetCollection<BsonDocument>("loan_account_cycles");
            _loanRateCollection = database.GetCollection<BsonDocument>("loan_rate");

            _loanId = loanNo;
            _accountId = accountId;
            _clientNo = clientNo;
            _isNewLoan = isNewLoan;
            _startPaymentDate = startPaymentDate;
            parentForm = parent;
        }


        public async Task InitializeAsync()
        {
            await ComputeLoanDetails();
            await LoadAutoCompleteData();
        }

        private async void frm_home_disburse_details_edit_Load(object sender, EventArgs e)
        {
            laccountid.Text = _accountId;
            tloanid.Text = _loanId;
            tclientno.Text = _clientNo; // Show the passed-in client number
            ldatestart.Text = _startPaymentDate;
            await InitializeAsync();
            await LoadCollectors();

            if (_isNewLoan)
            {
                ClearFields();
                bsave.Visible = false; // Hide Save
                badd.Visible = true;   // Show Add
            }
            else
            {
                await LoadLoanDetailsFromUI();
                bsave.Visible = true;  // Show Save
                badd.Visible = false;  // Hide Add
            }
        }

        // Clears all the fields when adding a new loan transaction
        private void ClearFields()
        {
            tloantype.Clear();
            tloanstatus.Clear();
            tloanterm.Clear();
            tloanamount.Clear();
            tloanbalance.Clear();
            tloanprincipal.Clear();
            tloaninterest.Clear();
            tloanpenalty.Clear();
            cbpaymentmode.SelectedIndex = -1;
            cbcollector.SelectedIndex = -1;
            dtstartdate.Value = DateTime.Now;
            dtmatdate.Value = DateTime.Now;
        }

        // Load all collectors from 'loan_collectors' into cbcollector
        private async Task LoadCollectors()
        {
            try
            {
                cbcollector.Items.Clear(); // Clear existing items
                var collectors = await _loanCollectorsCollection.Find(new BsonDocument()).ToListAsync();

                foreach (var collector in collectors)
                {
                    string collectorName = collector.GetValue("Name", "").ToString();
                    cbcollector.Items.Add(collectorName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading collectors: " + ex.Message);
            }
        }

        private async Task LoadLoanDetailsFromUI()
        {
            try
            {
                string accountId = laccountid.Text;
                string startPaymentDate = ldatestart.Text;

                if (string.IsNullOrWhiteSpace(accountId) || string.IsNullOrWhiteSpace(startPaymentDate))
                {
                    MessageBox.Show("AccountId or StartPaymentDate is missing.");
                    return;
                }

                // Find in loan_account_cycles by AccountId and StartPaymentDate
                var cycleFilter = Builders<BsonDocument>.Filter.And(
                    Builders<BsonDocument>.Filter.Eq("AccountId", accountId),
                    Builders<BsonDocument>.Filter.Eq("StartPaymentDate", startPaymentDate)
                );
                var cycleDoc = await _loanAccountCyclesCollection.Find(cycleFilter).FirstOrDefaultAsync();

                // Find in loan_disbursed by AccountId and StartPaymentDate (or DateStart)
                var disbursedFilter = Builders<BsonDocument>.Filter.And(
                    Builders<BsonDocument>.Filter.Eq("AccountId", accountId),
                    Builders<BsonDocument>.Filter.Eq("StartPaymentDate", startPaymentDate)
                // Or use "DateStart" depending on your schema consistency
                );
                var disbursedDoc = await _loanDisbursedCollection.Find(disbursedFilter).FirstOrDefaultAsync();

                if (cycleDoc == null && disbursedDoc == null)
                {
                    MessageBox.Show($"No matching loan found for AccountId: {accountId} and StartPaymentDate: {startPaymentDate}");
                    return;
                }

                // Call LoadLoanDetails passing LoanNo and StartPaymentDate
                await LoadLoanDetails(accountId, startPaymentDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading loan details: " + ex.Message);
            }
        }


        private async Task LoadLoanDetails(string accountId, string dateStart)
        {
            try
            {
                var cycleFilter = Builders<BsonDocument>.Filter.And(
                    Builders<BsonDocument>.Filter.Eq("AccountId", accountId),
                    Builders<BsonDocument>.Filter.Eq("StartPaymentDate", dateStart)
                );
                var cycleDoc = await _loanAccountCyclesCollection.Find(cycleFilter).FirstOrDefaultAsync();

                var disbursedFilter = Builders<BsonDocument>.Filter.And(
                    Builders<BsonDocument>.Filter.Eq("AccountId", accountId),
                    Builders<BsonDocument>.Filter.Eq("StartPaymentDate", dateStart)
                );
                var disbursedDoc = await _loanDisbursedCollection.Find(disbursedFilter).FirstOrDefaultAsync();

                if (cycleDoc == null && disbursedDoc == null)
                {
                    MessageBox.Show($"No matching loan found for LoanNo: {accountId} and AccountId: {dateStart}");
                    return;
                }

                // Loan Type - prefer disbursedDoc
                tloantype.Text = cycleDoc?.GetValue("LoanType", null)?.ToString()
                     ?? disbursedDoc?.GetValue("LoanType", null)?.ToString()
                     ?? "N/A";

                // Loan Status - prefer cycleDoc
                tloanstatus.Text = cycleDoc?.GetValue("LoanStatus", null)?.ToString()
                     ?? disbursedDoc?.GetValue("LoanStatus", null)?.ToString()
                     ?? "N/A";

                tloanterm.Text = cycleDoc?.GetValue("LoanTerm", null)?.ToString()
                     ?? disbursedDoc?.GetValue("LoanTerm", null)?.ToString()
                     ?? "N/A";

                // Loan Amount - cycleDoc fallback disbursedDoc
                tloanamount.Text = cycleDoc?.GetValue("LoanAmount", null)?.ToString()
                    ?? disbursedDoc?.GetValue("LoanAmount", null)?.ToString()
                    ?? "₱0.00";

                // Loan Balance - cycleDoc fallback disbursedDoc
                tloanbalance.Text = cycleDoc?.GetValue("LoanBalance", null)?.ToString()
                    ?? disbursedDoc?.GetValue("LoanBalance", null)?.ToString()
                    ?? "₱0.00";

                // Principal Amount - cycleDoc fallback disbursedDoc
                tloanprincipal.Text = cycleDoc?.GetValue("PrincipalAmount", null)?.ToString()
                    ?? disbursedDoc?.GetValue("PrincipalAmount", null)?.ToString()
                    ?? "₱0.00";

                // Loan Interest - cycleDoc fallback disbursedDoc
                tloaninterest.Text = cycleDoc?.GetValue("LoanInterest", null)?.ToString()
                    ?? disbursedDoc?.GetValue("LoanInterest", null)?.ToString()
                    ?? "₱0.00";

                // Penalty - cycleDoc fallback disbursedDoc
                tloanpenalty.Text = cycleDoc?.GetValue("Penalty", null)?.ToString()
                    ?? disbursedDoc?.GetValue("Penalty", null)?.ToString()
                    ?? "₱0.00";

                // Payment Mode - cycleDoc fallback disbursedDoc
                cbpaymentmode.Text = cycleDoc?.GetValue("PaymentMode", null)?.ToString()
                    ?? disbursedDoc?.GetValue("PaymentMode", null)?.ToString()
                    ?? "N/A";


                // Parse dates carefully from either document
                string startPaymentDateStr = cycleDoc?.GetValue("StartPaymentDate", "")?.ToString()
                    ?? disbursedDoc?.GetValue("StartPaymentDate", "")?.ToString();

                if (DateTime.TryParse(startPaymentDateStr, out DateTime startDate))
                    dtstartdate.Value = startDate;

                string maturityDateStr = cycleDoc?.GetValue("MaturityDate", "")?.ToString()
                    ?? disbursedDoc?.GetValue("MaturityDate", "")?.ToString();

                if (DateTime.TryParse(maturityDateStr, out DateTime maturityDate))
                    dtmatdate.Value = maturityDate;

                await LoadAssignedCollector();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading loan details: " + ex.Message);
            }
        }




        private async Task LoadAutoCompleteData()
        {
            try
            {
                // Get distinct LoanNo
                var loanNos = await _loanDisbursedCollection.Distinct<string>("LoanNo", FilterDefinition<BsonDocument>.Empty).ToListAsync();
                var loanNoSource = new AutoCompleteStringCollection();
                loanNoSource.AddRange(loanNos.ToArray());
                tloanid.AutoCompleteCustomSource = loanNoSource;

                // Get distinct LoanTerm
                var loanTerms = await _loanDisbursedCollection.Distinct<string>("LoanTerm", FilterDefinition<BsonDocument>.Empty).ToListAsync();
                var loanTermSource = new AutoCompleteStringCollection();
                loanTermSource.AddRange(loanTerms.ToArray());
                tloanterm.AutoCompleteCustomSource = loanTermSource;

                // Get distinct LoanAmount (strings or convert to string)
                var loanAmounts = await _loanDisbursedCollection.Distinct<string>("LoanAmount", FilterDefinition<BsonDocument>.Empty).ToListAsync();
                var loanAmountSource = new AutoCompleteStringCollection();
                loanAmountSource.AddRange(loanAmounts.ToArray());
                tloanamount.AutoCompleteCustomSource = loanAmountSource;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading autocomplete data: " + ex.Message);
            }
        }


        private async Task ComputeLoanDetails()
        {
            try
            {
                // Default values
                decimal principalAmount = 0m;
                decimal interestAmount = 0m;
                decimal penaltyAmount = 0m;

                // Parse loan amount, else default to 0
                if (!decimal.TryParse(tloanamount.Text.Replace("₱", "").Trim(), out decimal loanAmount))
                {
                    loanAmount = 0m;
                }

                // Parse loan term, else default to 0
                if (!int.TryParse(tloanterm.Text.Trim(), out int loanTermMonths))
                {
                    loanTermMonths = 0;
                }

                string loanMode = cbpaymentmode.Text.Trim().ToUpper();

                if (loanAmount > 0 && loanTermMonths > 0 && !string.IsNullOrEmpty(loanMode))
                {
                    var filter = Builders<BsonDocument>.Filter.And(
                        Builders<BsonDocument>.Filter.Eq("Principal", loanAmount),
                        Builders<BsonDocument>.Filter.Eq("Term", loanTermMonths),
                        Builders<BsonDocument>.Filter.Eq("Mode", loanMode)
                    );

                    var loanRateDoc = await _loanRateCollection.Find(filter).FirstOrDefaultAsync();

                    if (loanRateDoc != null)
                    {
                        decimal interestRatePerMonth = loanRateDoc.GetValue("Interest Rate/Month", 0).ToDecimal();

                        principalAmount = loanAmount;
                        interestAmount = loanAmount * (interestRatePerMonth / 100m) * loanTermMonths;
                        penaltyAmount = 0m; // temp penalty 0
                    }
                    // If rate doc not found, keep zero defaults
                }
                // else inputs invalid, keep zero defaults

                tloanprincipal.Text = principalAmount.ToString("₱#,##0.00");
                tloaninterest.Text = interestAmount.ToString("₱#,##0.00");
                tloanpenalty.Text = penaltyAmount.ToString("₱#,##0.00");
            }
            catch (Exception ex)
            {
                // On exception, set zeros
                tloanprincipal.Text = "₱0.00";
                tloaninterest.Text = "₱0.00";
                tloanpenalty.Text = "₱0.00";
                // Optionally log ex.Message somewhere, but no MessageBox to avoid annoying user
            }
        }


        // Load the specific collector assigned to the loan from 'loan_approved'
        private async Task LoadAssignedCollector()
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("LoanNo", _loanId);
                var approvedLoan = await _loanApprovedCollection.Find(filter).FirstOrDefaultAsync();

                if (approvedLoan != null)
                {
                    string assignedCollector = approvedLoan.GetValue("CollectorName", "N/A").ToString();

                    if (!string.IsNullOrEmpty(assignedCollector) && cbcollector.Items.Contains(assignedCollector))
                    {
                        cbcollector.SelectedItem = assignedCollector;
                    }
                    else
                    {
                        cbcollector.Text = assignedCollector; // Set as text if not in the list
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading assigned collector: " + ex.Message);
            }
        }

        private string GenerateLoanCycleNo(string accountId)
        {
            if (string.IsNullOrWhiteSpace(accountId))
                accountId = "ACC";

            string baseId = new string(accountId.Where(char.IsLetterOrDigit).ToArray()).ToUpper();
            baseId = baseId.Length >= 3 ? baseId.Substring(0, 3) : baseId.PadRight(3, 'X');

            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string randomStr = new string(Enumerable.Repeat(chars, 4)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            string dateTimePart = DateTime.Now.ToString("yyyyMMddHHmmss");

            return $"LC-{baseId}-{randomStr}-{dateTimePart}";
        }

        private async Task<bool> UpdateLoanTransaction()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tloanid.Text))
                {
                    MessageBox.Show("Loan ID (LoanNo) is required. Please enter a valid Loan ID before saving.", "Missing Loan ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                var filter = Builders<BsonDocument>.Filter.And(
                    Builders<BsonDocument>.Filter.Eq("AccountId", laccountid.Text),
                    Builders<BsonDocument>.Filter.Eq("StartPaymentDate", ldatestart.Text)
                );

                var update = Builders<BsonDocument>.Update
                    .Set("LoanNo", tloanid.Text)
                    .Set("LoanType", tloantype.Text)
                    .Set("LoanStatus", tloanstatus.Text)
                    .Set("LoanTerm", tloanterm.Text)
                    .Set("LoanAmount", tloanamount.Text)
                    .Set("LoanBalance", tloanbalance.Text)
                    .Set("PrincipalAmount", tloanprincipal.Text)
                    .Set("LoanInterest", tloaninterest.Text)
                    .Set("Penalty", tloanpenalty.Text)
                    .Set("PaymentMode", cbpaymentmode.Text)
                    .Set("CollectorName", cbcollector.Text)
                    .Set("StartPaymentDate", dtstartdate.Value.ToString("MM/dd/yyyy"))
                    .Set("MaturityDate", dtmatdate.Value.ToString("MM/dd/yyyy"))
                    .Set("Date_Encoded", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));

                // Attempt to update loan_disbursed first
                var result = await _loanDisbursedCollection.UpdateOneAsync(filter, update);

                // If loan_disbursed update failed, try loan_account_cycles
                if (result.ModifiedCount == 0)
                {
                    var altResult = await _loanAccountCyclesCollection.UpdateOneAsync(filter, update);

                    if (altResult.ModifiedCount == 0)
                    {
                        MessageBox.Show("No matching loan found in both loan_disbursed and loan_account_cycles collections. Update failed.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }

                // Now check if a loan cycle exists (you can keep this scoped to AccountId only if desired)
                var cycleFilter = Builders<BsonDocument>.Filter.Eq("AccountId", laccountid.Text);
                var cycleExists = await _loanAccountCyclesCollection.Find(cycleFilter).AnyAsync();

                if (!cycleExists)
                {
                    string loanCycleNo = GenerateLoanCycleNo(laccountid.Text);

                    var loanCycleDoc = new BsonDocument
                     {
                         { "LoanCycleNo", loanCycleNo },
                         { "AccountId", laccountid.Text },
                         { "LoanNo", tloanid.Text },
                         { "CycleDate", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") },
                         { "LoanStatus", tloanstatus.Text },
                         { "LoanTerm", tloanterm.Text },
                         { "LoanAmount", tloanamount.Text },
                         { "LoanBalance", tloanbalance.Text },
                         { "LoanType", tloantype.Text },
                         { "PrincipalAmount", tloanprincipal.Text },
                         { "LoanInterest", tloaninterest.Text },
                         { "Penalty", tloanpenalty.Text },
                         { "PaymentMode", cbpaymentmode.Text },
                         { "CollectorName", cbcollector.Text },
                         { "StartPaymentDate", dtstartdate.Value.ToString("MM/dd/yyyy") },
                         { "MaturityDate", dtmatdate.Value.ToString("MM/dd/yyyy") }
                     };

                    await _loanAccountCyclesCollection.InsertOneAsync(loanCycleDoc);
                }

                MessageBox.Show("Loan transaction updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating loan transaction: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        private async Task<bool> AddLoanCycle()
        {
            try
            {
                // Define a filter to check if the loan cycle already exists
                var filter = Builders<BsonDocument>.Filter.And(
                    Builders<BsonDocument>.Filter.Eq("AccountId", laccountid.Text),
                    Builders<BsonDocument>.Filter.Eq("LoanNo", tloanid.Text)
                );

                // Check if a loan cycle with the same AccountId and LoanNo already exists
                var exists = await _loanAccountCyclesCollection.Find(filter).AnyAsync();

                if (exists)
                {
                    MessageBox.Show("Loan cycle already exists. Duplicate entry prevented.", "Duplicate Detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;  // Return false to indicate duplicate found
                }

                // Generate LoanCycleNo before insert
                string loanCycleNo = GenerateLoanCycleNo(laccountid.Text);

                // Insert new loan cycle document since it does not exist
                var loanCycleDoc = new BsonDocument
                 {
                     { "LoanCycleNo", loanCycleNo },
                     { "AccountId", laccountid.Text },
                     { "LoanNo", tloanid.Text },
                     { "CycleDate", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") },
                     { "LoanStatus", tloanstatus.Text },
                     { "LoanTerm", tloanterm.Text },
                     { "LoanType", tloantype.Text },
                     { "LoanAmount", tloanamount.Text },
                     { "LoanBalance", tloanbalance.Text },
                     { "PrincipalAmount", tloanprincipal.Text },
                     { "LoanInterest", tloaninterest.Text },
                     { "Penalty", tloanpenalty.Text },
                     { "PaymentMode", cbpaymentmode.Text },
                     { "CollectorName", cbcollector.Text },
                     { "StartPaymentDate", dtstartdate.Value.ToString("MM/dd/yyyy") },
                     { "MaturityDate", dtmatdate.Value.ToString("MM/dd/yyyy") }
                 };

                await _loanAccountCyclesCollection.InsertOneAsync(loanCycleDoc);

                MessageBox.Show("Loan cycle added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh grid after adding
                

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding loan cycle: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        private async void bsave_Click(object sender, EventArgs e)
        {
            bool success = await UpdateLoanTransaction();
            if (success)
            {
                // Call parent to refresh grid
                if (parentForm != null)
                {
                    await parentForm.RefreshLoanGrid(laccountid.Text);
                }
            }
        }

        private async void badd_Click(object sender, EventArgs e)
        {
            bool success = await AddLoanCycle();
            if (success)
            {
                // Call parent to refresh grid
                if (parentForm != null)
                {
                    await parentForm.RefreshLoanGrid(laccountid.Text);
                }
            }
        }

        private async void tloanamount_TextChanged(object sender, EventArgs e)
        {
            await ComputeLoanDetails();
        }
    }
}

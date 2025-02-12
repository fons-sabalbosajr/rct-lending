using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Driver;

namespace rct_lmis.DISBURSEMENT_SECTION
{
    public partial class frm_home_disburse_details_edit : Form
    {
        private string _loanId;
        private bool _isNewLoan;
        private IMongoCollection<BsonDocument> _loanDisbursedCollection;
        private IMongoCollection<BsonDocument> _loanApprovedCollection;
        private IMongoCollection<BsonDocument> _loanCollectorsCollection;

        public frm_home_disburse_details_edit(string loanId, bool isNewLoan = false)
        {
            InitializeComponent();
            var database = MongoDBConnection.Instance.Database;
            _loanDisbursedCollection = database.GetCollection<BsonDocument>("loan_disbursed");
            _loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");
            _loanCollectorsCollection = database.GetCollection<BsonDocument>("loan_collectors");
            _loanId = loanId;
            _isNewLoan = isNewLoan;
        }

        private async void frm_home_disburse_details_edit_Load(object sender, EventArgs e)
        {
            if (_isNewLoan)
            {
                ClearFields();
                laccountid.Text = _loanId;
                bsave.Visible = false; // Hide the save button (for existing loans)
                badd.Visible = true;  // Show the add button for saving the new loan
            }
            else
            {
                // For adding new loan transaction:
                laccountid.Text = _loanId;
                await LoadCollectors();
                await LoadLoanDetails();
                badd.Visible = false; // Hide the add button when editing an existing loan
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

        // Load loan details, including the assigned collector
        private async Task LoadLoanDetails()
        {
            try
            {
                // Find loan details by LoanNo
                var filter = Builders<BsonDocument>.Filter.Eq("LoanNo", _loanId);
                var loanDocument = await _loanDisbursedCollection.Find(filter).FirstOrDefaultAsync();

                if (loanDocument != null)
                {
                    // Populate textboxes
                    tloantype.Text = loanDocument.GetValue("LoanType", "N/A").ToString();
                    tloanstatus.Text = loanDocument.GetValue("LoanStatus", "N/A").ToString();
                    tloanterm.Text = loanDocument.GetValue("LoanTerm", "N/A").ToString();
                    tloanamount.Text = loanDocument.GetValue("LoanAmount", "₱0.00").ToString();
                    tloanbalance.Text = loanDocument.GetValue("LoanBalance", "₱0.00").ToString();
                    tloanprincipal.Text = loanDocument.GetValue("PrincipalAmount", "₱0.00").ToString();
                    tloaninterest.Text = loanDocument.GetValue("LoanInterest", "0%").ToString();
                    tloanpenalty.Text = loanDocument.GetValue("Penalty", "₱0.00").ToString();
                    cbpaymentmode.Text = loanDocument.GetValue("PaymentMode", "N/A").ToString();

                    // Populate date pickers
                    DateTime startDate, maturityDate;
                    if (DateTime.TryParse(loanDocument.GetValue("StartPaymentDate", "").ToString(), out startDate))
                    {
                        dtstartdate.Value = startDate;
                    }

                    if (DateTime.TryParse(loanDocument.GetValue("MaturityDate", "").ToString(), out maturityDate))
                    {
                        dtmatdate.Value = maturityDate;
                    }

                    // Fetch the assigned collector from loan_approved
                    await LoadAssignedCollector();
                }
                else
                {
                    MessageBox.Show("Loan details not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading loan details: " + ex.Message);
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

        private async void bsave_Click(object sender, EventArgs e)
        {
            await UpdateLoanDetails();
        }

        private async Task AddNewLoanTransaction()
        {
            try
            {
                var newLoan = new BsonDocument
                 {
                     { "LoanNo", Guid.NewGuid().ToString() }, // Generate new LoanNo or use your own logic
                     { "LoanType", tloantype.Text },
                     { "LoanStatus", tloanstatus.Text },
                     { "LoanTerm", tloanterm.Text },
                     { "LoanAmount", tloanamount.Text },
                     { "LoanBalance", tloanbalance.Text },
                     { "PrincipalAmount", tloanprincipal.Text },
                     { "LoanInterest", tloaninterest.Text },
                     { "Penalty", tloanpenalty.Text },
                     { "PaymentMode", cbpaymentmode.Text },
                     { "CollectorName", cbcollector.Text },
                     { "StartPaymentDate", dtstartdate.Value.ToString("MM/dd/yyyy") },
                     { "MaturityDate", dtmatdate.Value.ToString("MM/dd/yyyy") },
                     { "Date_Encoded", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") }
                 };

                // Assuming _loanDisbursedCollection is your MongoDB collection
                await _loanDisbursedCollection.InsertOneAsync(newLoan);

                MessageBox.Show("New loan transaction added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Close the form after saving the new loan
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding new loan transaction: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task UpdateLoanDetails()
        {
            try
            {
                if (string.IsNullOrEmpty(_loanId))
                {
                    MessageBox.Show("Invalid Loan ID. Cannot update record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Update the loan details in loan_disbursed collection
                var filterDisbursed = Builders<BsonDocument>.Filter.Eq("LoanNo", _loanId);
                var updateDisbursed = Builders<BsonDocument>.Update
                    .Set("LoanType", tloantype.Text)
                    .Set("LoanStatus", tloanstatus.Text)
                    .Set("LoanTerm", tloanterm.Text)
                    .Set("LoanAmount", tloanamount.Text)
                    .Set("LoanBalance", tloanbalance.Text)
                    .Set("PrincipalAmount", tloanprincipal.Text) // Assuming this is the correct field
                    .Set("LoanInterest", tloaninterest.Text)
                    .Set("Penalty", tloanpenalty.Text)
                    .Set("PaymentMode", cbpaymentmode.Text)
                    .Set("StartPaymentDate", dtstartdate.Value.ToString("MM/dd/yyyy"))
                    .Set("MaturityDate", dtmatdate.Value.ToString("MM/dd/yyyy"))
                    .Set("Date_Modified", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));

                // Update the loan_disbursed collection
                var resultDisbursed = await _loanDisbursedCollection.UpdateOneAsync(filterDisbursed, updateDisbursed);

                if (resultDisbursed.ModifiedCount == 0)
                {
                    MessageBox.Show("No changes were made to the loan details in loan_disbursed.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Update the collector in loan_approved collection
                var filterApproved = Builders<BsonDocument>.Filter.Eq("LoanNo", _loanId);
                var updateApproved = Builders<BsonDocument>.Update
                    .Set("CollectorName", cbcollector.Text) // Update collector name in loan_approved
                    .Set("Date_Modified", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));

                // Update the loan_approved collection
                var resultApproved = await _loanApprovedCollection.UpdateOneAsync(filterApproved, updateApproved);

                if (resultApproved.ModifiedCount > 0)
                {
                    MessageBox.Show("Loan details and collector updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadLoanDetails(); // Reload the updated loan details
                }
                else
                {
                    MessageBox.Show("No changes were made to the collector in loan_approved.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating loan details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void badd_ClickAsync(object sender, EventArgs e)
        {
            // Perform validation and saving of the new loan transaction
            await AddNewLoanTransaction();
        }
    }
}

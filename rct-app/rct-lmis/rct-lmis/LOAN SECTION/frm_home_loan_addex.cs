using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rct_lmis.LOAN_SECTION
{
    public partial class frm_home_loan_addex : Form
    {
        private IMongoCollection<BsonDocument> collection;
        private readonly IMongoCollection<BsonDocument> loanDisbursedCollection;
        private readonly IMongoCollection<BsonDocument> loanApprovedCollection;
        private readonly IMongoCollection<BsonDocument> loanRateCollection;
        private readonly IMongoCollection<BsonDocument> loanCollectorsCollection;

        public frm_home_loan_addex()
        {
            InitializeComponent();

            var database = MongoDBConnection.Instance.Database;
            loanDisbursedCollection = database.GetCollection<BsonDocument>("loan_disbursed");
            loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");
            loanRateCollection = database.GetCollection<BsonDocument>("loan_rate");
            loanCollectorsCollection = database.GetCollection<BsonDocument>("loan_collectors");

            dtstartdate.Value = DateTime.Now; // Reset to today
            dtenddate.Value = DateTime.Now; // Reset to 1 month later or adjust as neede
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
                var collectors = loanCollectorsCollection.Find(new BsonDocument()).ToList();

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

        private async Task<List<string>> GetDistinctValuesAsync(string fieldName)
        {
            var filter = Builders<BsonDocument>.Filter.Empty;
            var distinctValues = await loanDisbursedCollection.Distinct<string>(fieldName, filter).ToListAsync();
            return distinctValues;
        }

        private async Task<List<string>> GetDistinctPrincipalsAsync()
        {
            var distinctPrincipals = await loanRateCollection.Distinct<double>("Principal", Builders<BsonDocument>.Filter.Empty).ToListAsync();

            // Convert each principal to string to use in AutoComplete
            return distinctPrincipals.Select(principal => principal.ToString()).ToList();
        }

        private async Task<List<string>> GetAvailableTermsAsync(double principal)
        {
            // Filter based on the selected principal amount
            var filter = Builders<BsonDocument>.Filter.Eq("Principal", principal);
            var terms = await loanRateCollection.Find(filter)
                                                 .Project<BsonDocument>(Builders<BsonDocument>.Projection.Include("Term"))
                                                 .ToListAsync();

            // Extract the terms and append " months" to each
            return terms.Select(termDoc => $"{termDoc["Term"]} months").ToList();
        }

        private async Task SetupAutocomplete(Guna.UI2.WinForms.Guna2TextBox textBox, string fieldName)
        {
            var values = await GetDistinctValuesAsync(fieldName);
            var autoCompleteSource = new AutoCompleteStringCollection();
            autoCompleteSource.AddRange(values.ToArray());
            textBox.AutoCompleteCustomSource = autoCompleteSource;
            textBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private async Task SetupAutocompleteLoanAmount()
        {
            var principalValues = await GetDistinctPrincipalsAsync();
            var autoCompleteSource = new AutoCompleteStringCollection();
            autoCompleteSource.AddRange(principalValues.ToArray());

            tloanamt.AutoCompleteCustomSource = autoCompleteSource;
            tloanamt.AutoCompleteMode = AutoCompleteMode.Suggest;
            tloanamt.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private async Task SetupLoanTermAutocomplete(double principal)
        {
            var availableTerms = await GetAvailableTermsAsync(principal);
            var autoCompleteSource = new AutoCompleteStringCollection();
            autoCompleteSource.AddRange(availableTerms.ToArray());

            tloanterm.AutoCompleteCustomSource = autoCompleteSource;
            tloanterm.AutoCompleteMode = AutoCompleteMode.SuggestAppend; // Allows for appending text
            tloanterm.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private async Task<string> GetLatestIncrementedFieldAsync(string fieldName, string basePrefix, int leadingZeroCount)
        {
            var sort = Builders<BsonDocument>.Sort.Descending(fieldName);
            var projection = Builders<BsonDocument>.Projection.Include(fieldName).Exclude("_id");
            var latestDocument = await loanDisbursedCollection.Find(Builders<BsonDocument>.Filter.Exists(fieldName))
                                                              .Sort(sort)
                                                              .Project(projection)
                                                              .FirstOrDefaultAsync();

            if (latestDocument != null)
            {
                string latestValue = latestDocument[fieldName].AsString;

                // Detect the correct prefix dynamically
                string detectedPrefix = latestValue.Substring(0, latestValue.Length - leadingZeroCount);

                return IncrementId(latestValue, detectedPrefix, leadingZeroCount);
            }

            // Default if no documents found
            return $"{basePrefix}{1.ToString().PadLeft(leadingZeroCount, '0')}";
        }

        private string IncrementId(string id, string prefix, int leadingZeroCount)
        {
            if (!id.StartsWith(prefix))
            {
                throw new ArgumentException($"ID does not start with the expected prefix: {prefix}");
            }

            // Extract the numeric part from the ID
            string numericPart = id.Substring(prefix.Length);

            if (int.TryParse(numericPart, out int increment))
            {
                return $"{prefix}{(increment + 1).ToString().PadLeft(leadingZeroCount, '0')}";
            }

            // Default fallback
            return $"{prefix}{1.ToString().PadLeft(leadingZeroCount, '0')}";
        }

        private async Task SetLatestAccountAndClientIdsAsync()
        {
            string currentYear = DateTime.Now.Year.ToString();

            // Define prefixes based on current year
            string accountPrefix = $"RCT-{currentYear}DB-";
            string clientPrefix = $"RCT-{currentYear}-CL";

            // Fetch latest incremented AccountId and ClientNo
            taccountid.Text = await GetLatestIncrementedFieldAsync("AccountId", accountPrefix, 3) ?? "No AccountId found";
            tclientno.Text = await GetLatestIncrementedFieldAsync("ClientNo", clientPrefix, 4) ?? "No ClientNo found";
        }


        private async Task<List<string>> GetDistinctCollectorNamesAsync()
        {
            var filter = Builders<BsonDocument>.Filter.Empty; // You can add a filter if needed
            var distinctValues = await loanCollectorsCollection.Distinct<string>("Name", filter).ToListAsync();
            return distinctValues;
        }


        private async Task SetupCollectorAutocomplete(Guna.UI2.WinForms.Guna2TextBox textBox)
        {
            var values = await GetDistinctCollectorNamesAsync(); // Fetch distinct collector names
            var autoCompleteSource = new AutoCompleteStringCollection();
            autoCompleteSource.AddRange(values.ToArray());

            textBox.AutoCompleteCustomSource = autoCompleteSource; // Set the custom source
            textBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend; // Suggest and append mode
            textBox.AutoCompleteSource = AutoCompleteSource.CustomSource; // Use custom source
        }


        private async Task<BsonDocument> GetLoanRateDetailsAsync(double principal, int term)
        {
            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("Principal", principal),
                Builders<BsonDocument>.Filter.Eq("Term", term)
            );

            // Get the loan rate details
            return await loanRateCollection.Find(filter).FirstOrDefaultAsync();
        }

        private async Task CalculateLoanDetails(double principal, int term)
        {
            var loanRateDetails = await GetLoanRateDetailsAsync(principal, term);

            if (loanRateDetails != null)
            {
                // Retrieve necessary values
                double interestRate = loanRateDetails.GetValue("Interest Rate/Month").AsDouble;
                double processingFee = loanRateDetails.GetValue("Processing Fee").AsDouble;

                // Calculate interest amount
                double interestAmount = (principal * (interestRate / 100)) * term; // Total interest for the selected term
                double amortization = principal / term; // Assuming equal monthly payments

                // Update the textboxes (you can adjust the names as needed)
                tloaninterest.Text = $"{interestRate}%"; // Set interest rate in percentage
                tloaninterestamt.Text = $"₱{interestAmount:F2}"; ; // Format as currency
                tloanamort.Text = $"{amortization:F2}"; // Format as currency
                tloanprocessfee.Text = $"{processingFee:F2}"; // Format as currency
            }
        }



        private async void frm_home_loan_addex_Load(object sender, EventArgs e)
        {
            await SetupAutocomplete(tloanno, "LoanNo");
            await SetupAutocomplete(tclientlastname, "LastName");
            await SetupAutocomplete(tclientfirstname, "FirstName");
            await SetupAutocomplete(tclientmiddlename, "MiddleName");
            await SetupAutocomplete(tbrgy, "Barangay");
            await SetupAutocomplete(tcity, "City ");
            await SetupAutocomplete(tprovince, "Province");
            
            await SetupAutocompleteLoanAmount();
            await SetLatestAccountAndClientIdsAsync();

            LoadCollectors();
           
        }

        private void bcancel_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to cancel?", "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                ClearFormFields();
                this.Close(); // Close the form if the user confirms
            }
        }

        private async void bsave_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if LoanNo (tloanno.Text) is empty
                if (string.IsNullOrEmpty(tloanno.Text))
                {
                    MessageBox.Show("Loan Number cannot be empty.");
                    return; // Stop the execution if LoanNo is empty
                }

                // Validate required fields before proceeding
                if (!ValidateFormFields())
                {
                    return; // Stop execution if validation fails
                }

                // Create a new document for loan_disbursed
                var loanDisbursedDocument = new BsonDocument
                 {
                     { "AccountId", taccountid.Text },
                     { "LoanNo", tloanno.Text },
                     { "ClientNo", tclientno.Text },
                     { "LoanType", cbloantype.SelectedItem?.ToString() ?? "" }, // Handle null ComboBox values
                     { "LoanStatus", cbloanstatus.SelectedItem?.ToString() ?? "" },
                     { "LastName", tclientlastname.Text },
                     { "FirstName", tclientfirstname.Text },
                     { "MiddleName", tclientmiddlename.Text },
                     { "Barangay", tbrgy.Text },
                     { "City", tcity.Text },
                     { "Province", tprovince.Text },
                     { "CollectorName", cbcollector.Text },
                     { "LoanAmount", tloanamt.Text.Replace("₱", "").Replace(",", "").Trim() },
                     { "LoanTerm", tloanterm.Text },
                     { "LoanInterestRate", tloaninterest.Text.Replace("%", "").Trim() },
                     { "LoanInterestAmount", tloaninterestamt.Text.Replace("₱", "").Replace(",", "").Trim() },
                     { "ProcessingFee", tloanprocessfee.Text.Replace("₱", "").Replace(",", "").Trim() },
                     { "LoanBalance", tloanbal.Text.Replace("₱", "").Replace(",", "").Trim() },
                     { "LoanAmortization", tloanamort.Text.Replace("₱", "").Replace(",", "").Trim() },
                     { "DateStart", dtstartdate.Value.ToString("MM/dd/yyyy") },
                     { "DateEnd", dtenddate.Value.ToString("MM/dd/yyyy") },
                     { "Date_Encoded", DateTime.Now.ToString("MM/dd/yyyy") },
                     { "DateApproved", DateTime.Now.ToString("MM/dd/yyyy") },
                     { "ApprovedBy", "Admin" }
                 };

                // Insert into loan_disbursed collection
                await loanDisbursedCollection.InsertOneAsync(loanDisbursedDocument);

                // Create a new document for loan_approved
                var loanApprovedDocument = new BsonDocument(loanDisbursedDocument); // Copy the same data

                // Insert into loan_approved collection
                // await loanApprovedCollection.InsertOneAsync(loanApprovedDocument);

                MessageBox.Show("Loan information saved successfully!");
                ClearFormFields(); // Implement this method to reset the form if needed
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        // Method to validate required fields
        private bool ValidateFormFields()
        {
            if (string.IsNullOrWhiteSpace(taccountid.Text) ||
                string.IsNullOrWhiteSpace(tloanno.Text) ||
                string.IsNullOrWhiteSpace(tclientno.Text) ||
                cbloantype.SelectedItem == null ||
                cbloanstatus.SelectedItem == null ||
                string.IsNullOrWhiteSpace(tclientlastname.Text) ||
                string.IsNullOrWhiteSpace(tclientfirstname.Text) ||
                string.IsNullOrWhiteSpace(tbrgy.Text) ||
                string.IsNullOrWhiteSpace(tcity.Text) ||
                string.IsNullOrWhiteSpace(tprovince.Text) ||
                string.IsNullOrWhiteSpace(cbcollector.Text) ||
                string.IsNullOrWhiteSpace(tloanamt.Text) ||
                string.IsNullOrWhiteSpace(tloanterm.Text) ||
                string.IsNullOrWhiteSpace(tloaninterest.Text) ||
                string.IsNullOrWhiteSpace(tloaninterestamt.Text) ||
                string.IsNullOrWhiteSpace(tloanprocessfee.Text) ||
                string.IsNullOrWhiteSpace(tloanbal.Text) ||
                string.IsNullOrWhiteSpace(tloanamort.Text))
            {
                MessageBox.Show("Please fill in all required fields before saving.", "Missing Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }


        private void ClearFormFields()
        {
            // Reset all form fields as necessary
            taccountid.Clear();
            tloanno.Clear();
            tclientno.Clear();
            cbloantype.SelectedIndex = 0; // Assuming it's a ComboBox
            cbloanstatus.SelectedIndex = 0; // Assuming it's a ComboBox
            tclientlastname.Clear();
            tclientfirstname.Clear();
            tclientmiddlename.Clear();
            tbrgy.Clear();
            tcity.Clear();
            tprovince.Clear();
            
            tloanamt.Clear();
            tloanterm.Clear();
            tloaninterest.Clear();
            tloaninterestamt.Clear();
            tloanprocessfee.Clear();
            tloanbal.Clear();
            tloanamort.Clear();
            tloanamountpay.Clear();
            dtstartdate.Value = DateTime.Now; // Reset to today
            dtenddate.Value = DateTime.Now; // Reset to 1 month later or adjust as needed
                                                         // Add more fields as necessary
        }

        private async void tloanamt_TextChangedAsync(object sender, EventArgs e)
        {
            string selectedAmount = tloanamt.Text.Trim();

            // Check if the entered amount is empty or invalid
            if (string.IsNullOrWhiteSpace(selectedAmount) ||
                !double.TryParse(selectedAmount.Replace("₱", "").Replace(",", ""), out _))
            {
                ResetLoanAmountDetails();
                return;
            }

            // Fetch the document with the matching Principal
            var filter = Builders<BsonDocument>.Filter.Eq("Principal", selectedAmount);
            var loanConfig = await loanRateCollection.Find(filter).FirstOrDefaultAsync();

            if (loanConfig != null && loanConfig.Contains("Term"))
            {
                var termValue = loanConfig["Term"];

                // Support multiple terms (if term is stored as an array)
                var termList = termValue.IsBsonArray
                    ? termValue.AsBsonArray.Select(t => t.ToString()).ToList()
                    : new List<string> { termValue.ToString() };

                var autoCompleteTermSource = new AutoCompleteStringCollection();
                autoCompleteTermSource.AddRange(termList.ToArray());

                tloanterm.AutoCompleteCustomSource = autoCompleteTermSource;
                tloanterm.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                tloanterm.AutoCompleteSource = AutoCompleteSource.CustomSource;
            }
            else
            {
                // If no matching loan configuration is found, reset fields
                ResetLoanAmountDetails();
            }
        }

        private void tloanno_TextChanged(object sender, EventArgs e)
        {
            // Check if the current text is only numbers and no prefix
            if (System.Text.RegularExpressions.Regex.IsMatch(tloanno.Text, @"^\d+$"))
            {
                // Prepend "RCT-2024-" to the numeric text
                tloanno.Text = $"RCT-2024-{tloanno.Text}";

                // Move the cursor to the end of the text
                tloanno.SelectionStart = tloanno.Text.Length;
            }
        }

        private async void tloanamt_Leave(object sender, EventArgs e)
        {
            // Parse the selected loan amount from tloanamt.Text
            if (double.TryParse(tloanamt.Text, out double selectedPrincipal))
            {
                // Set up the loan term autocomplete based on the selected principal
                await SetupLoanTermAutocomplete(selectedPrincipal);
            }
        }

        private void tloanterm_Leave(object sender, EventArgs e)
        {

        }

        private async void tloanterm_TextChanged(object sender, EventArgs e)
        {
            // Check if tloanterm is empty or invalid
            if (string.IsNullOrWhiteSpace(tloanterm.Text) ||
                !int.TryParse(tloanterm.Text.Split(' ')[0], out int selectedTerm) ||
                selectedTerm <= 0)
            {
                ResetLoanAmountDetails();
                return;
            }

            // Check if tloanamt contains a valid number
            if (!double.TryParse(tloanamt.Text.Replace("₱", "").Replace(",", "").Trim(), out double selectedPrincipal) ||
                selectedPrincipal <= 0)
            {
                ResetLoanAmountDetails();
                return;
            }

            // If both values are valid, calculate loan details
            await CalculateLoanDetails(selectedPrincipal, selectedTerm);
        }

        private void ResetLoanAmountDetails()
        {
            tloanterm.Text = "N/A";
            tloaninterest.Text = "";
            tloaninterestamt.Text = "";
            tloanamort.Text = "";
            tloanprocessfee.Text = "";
        }
    }
}

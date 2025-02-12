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

        private async Task<string> GetLatestIncrementedFieldAsync(string fieldName, string prefix, int leadingZeroCount)
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
                return IncrementId(latestValue, prefix, leadingZeroCount);
            }

            // Default value if no documents are found
            return $"{prefix}{1.ToString().PadLeft(leadingZeroCount, '0')}";
        }

        private string IncrementId(string id, string prefix, int leadingZeroCount)
        {
            // Check if the ID starts with the expected prefix
            if (!id.StartsWith(prefix))
            {
                throw new ArgumentException($"ID does not start with the expected prefix: {prefix}");
            }

            // Extract the numeric part from the ID by removing the prefix
            string numericPart = id.Substring(prefix.Length);

            // Special handling for AccountId format: strip non-numeric characters if needed
            if (numericPart.Contains("-"))
            {
                numericPart = numericPart.Split('-')[1];  // For example, "RCT-2024DB-977" becomes "977"
            }

            if (int.TryParse(numericPart, out int increment))
            {
                // Increment and format with leading zeros
                return $"{prefix}{(increment + 1).ToString().PadLeft(leadingZeroCount, '0')}";
            }

            // Handle cases where the numeric part is not an integer
            return $"{prefix}{1.ToString().PadLeft(leadingZeroCount, '0')}";
        }

        private async Task SetLatestAccountAndClientIdsAsync()
        {
            // Set the appropriate prefixes
            string accountPrefix = "RCT-2024DB-"; // Adjust as necessary
            string clientPrefix = "RCT-2024-CL"; // Adjust as necessary

            // Retrieve and set latest AccountId and ClientNo
            taccountid.Text = await GetLatestIncrementedFieldAsync("AccountId", accountPrefix, 3) ?? "No AccountId found";
            tclientno.Text = await GetLatestIncrementedFieldAsync("ClientNo", clientPrefix, 4) ?? "No ClientNo found"; // Specify leading zero count for ClientNo
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
            await SetupAutocomplete(tcollector, "CollectorName");

            await SetupAutocompleteLoanAmount();
            await SetLatestAccountAndClientIdsAsync();
            await SetupCollectorAutocomplete(tcollector);
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
                // Create a new document for loan_disbursed
                var loanDisbursedDocument = new BsonDocument
                {
                     { "AccountId", taccountid.Text },
                     { "LoanNo", tloanno.Text },
                     { "ClientNo", tclientno.Text },
                     { "LoanType", cbloantype.SelectedItem.ToString() }, // Assuming this is a ComboBox
                     { "LoanStatus", cbloanstatus.SelectedItem.ToString() }, // Assuming this is a ComboBox
                     { "LastName", tclientlastname.Text },
                     { "FirstName", tclientfirstname.Text },
                     { "MiddleName", tclientmiddlename.Text },
                     { "Barangay", tbrgy.Text },
                     { "City", tcity.Text },
                     { "Province", tprovince.Text },
                     { "CollectorName", tcollector.Text },
                     { "LoanAmount", tloanamt.Text.Replace("₱", "").Replace(",", "").Trim() }, // Assuming currency format
                     { "LoanTerm", tloanterm.Text },
                     { "LoanInterestRate", tloaninterest.Text.Replace("%", "").Trim() }, // Assuming interest is in percentage
                     { "LoanInterestAmount", tloaninterestamt.Text.Replace("₱", "").Replace(",", "").Trim() }, // Assuming currency format
                     { "ProcessingFee", tloanprocessfee.Text.Replace("₱", "").Replace(",", "").Trim() }, // Assuming currency format
                     { "LoanBalance", tloanbal.Text.Replace("₱", "").Replace(",", "").Trim() }, // Manual encode
                     { "LoanAmortization", tloanamort.Text.Replace("₱", "").Replace(",", "").Trim() }, // Assuming currency format
                     { "DateStart", dtstartdate.Value.ToString("MM/dd/yyyy") }, // Assuming this is a DateTimePicker
                     { "DateEnd", dtenddate.Value.ToString("MM/dd/yyyy") }, // Assuming this is a DateTimePicker
                     { "Date_Encoded", DateTime.Now.ToString("MM/dd/yyyy") },
                     { "DateApproved", DateTime.Now.ToString("MM/dd/yyyy") },
                     { "ApprovedBy", "Admin" } // Replace with actual user/role if needed// Encoding date
                 };

                         // Insert the loan_disbursed document into the collection
                 await loanDisbursedCollection.InsertOneAsync(loanDisbursedDocument);

                         // Create a new document for loan_approved
                 var loanApprovedDocument = new BsonDocument
                 {
                         { "AccountId", taccountid.Text },
                         { "LoanNo", tloanno.Text },
                         { "ClientNo", tclientno.Text },
                         { "LoanType", cbloantype.SelectedItem.ToString() }, // Assuming this is a ComboBox
                         { "LoanStatus", cbloanstatus.SelectedItem.ToString() }, // Assuming this is a ComboBox
                         { "LastName", tclientlastname.Text },
                         { "FirstName", tclientfirstname.Text },
                         { "MiddleName", tclientmiddlename.Text },
                         { "Barangay", tbrgy.Text },
                         { "City", tcity.Text },
                         { "Province", tprovince.Text },
                         { "CollectorName", tcollector.Text },
                         { "LoanAmount", tloanamt.Text.Replace("₱", "").Replace(",", "").Trim() }, // Assuming currency format
                         { "LoanTerm", tloanterm.Text },
                         { "LoanInterestRate", tloaninterest.Text.Replace("%", "").Trim() }, // Assuming interest is in percentage
                         { "LoanInterestAmount", tloaninterestamt.Text.Replace("₱", "").Replace(",", "").Trim() }, // Assuming currency format
                         { "ProcessingFee", tloanprocessfee.Text.Replace("₱", "").Replace(",", "").Trim() }, // Assuming currency format
                         { "LoanBalance", tloanbal.Text.Replace("₱", "").Replace(",", "").Trim() }, // Manual encode
                         { "LoanAmortization", tloanamort.Text.Replace("₱", "").Replace(",", "").Trim() }, // Assuming currency format
                         { "DateStart", dtstartdate.Value.ToString("MM/dd/yyyy") }, // Assuming this is a DateTimePicker
                         { "DateEnd", dtenddate.Value.ToString("MM/dd/yyyy") }, // Assuming this is a DateTimePicker
                         { "Date_Encoded", DateTime.Now.ToString("MM/dd/yyyy") },
                         { "DateApproved", DateTime.Now.ToString("MM/dd/yyyy") },
                         { "ApprovedBy", "Admin" } // Replace with actual user/role if needed// Encoding date
                 };

                    // Insert the loan_approved document into the collection
                    //await loanApprovedCollection.InsertOneAsync(loanApprovedDocument);

                    // Optionally clear the form fields or provide feedback
                    MessageBox.Show("Loan information saved successfully!");
                    ClearFormFields(); // Implement this method to reset the form if needed
                }
                catch (Exception ex)
                {
                    // Handle exceptions and show error messages
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
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
            tcollector.Clear();
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
            string selectedAmount = tloanamt.Text;

            // Fetch the document with the matching Principal
            var filter = Builders<BsonDocument>.Filter.Eq("Principal", selectedAmount);
            var loanConfig = await loanRateCollection.Find(filter).FirstOrDefaultAsync();

            if (loanConfig != null && loanConfig.Contains("Term"))
            {
                // Retrieve the Term for the selected Principal
                var termList = new List<string> { loanConfig["Term"].ToString() }; // Adjust if multiple terms exist

                var autoCompleteTermSource = new AutoCompleteStringCollection();
                autoCompleteTermSource.AddRange(termList.ToArray());

                tloanterm.AutoCompleteCustomSource = autoCompleteTermSource;
                tloanterm.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                tloanterm.AutoCompleteSource = AutoCompleteSource.CustomSource;
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

        private async void tloanterm_Leave(object sender, EventArgs e)
        {
           
        }

        private async void tloanterm_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(tloanterm.Text.Split(' ')[0], out int selectedTerm)) // Get the term number from the selected value
            {
                // Parse the selected loan amount from tloanamt.Text
                if (double.TryParse(tloanamt.Text.Replace("₱", "").Replace(",", "").Trim(), out double selectedPrincipal))
                {
                    // Calculate loan details based on selected principal and term
                    await CalculateLoanDetails(selectedPrincipal, selectedTerm);
                }
            }
        }
    }
}

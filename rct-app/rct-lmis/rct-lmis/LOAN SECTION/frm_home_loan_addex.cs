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
        private double _actualLoanPayable = 0;

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

            return await loanRateCollection.Find(filter).FirstOrDefaultAsync();
        }

        private async Task CalculateLoanDetails(double principal, int term)
        {
            var loanRateDetails = await GetLoanRateDetailsAsync(principal, term);

            if (loanRateDetails != null)
            {
                double interestRatePerMonth = loanRateDetails.GetValue("Interest Rate/Month").ToDouble();
                double processingFeeAmount = loanRateDetails.GetValue("Processing Fee").ToDouble(); // Fixed amount

                double totalInterestAmount = (principal * (interestRatePerMonth / 100)) * term;
                double totalPayable = principal + totalInterestAmount;
                int totalPayments = GetTotalPayments(term, cbpaymentmode.Text.Trim());
                double amortizationPerPayment = totalPayable / totalPayments;

                // Update fields
                tloaninterest.Text = $"{interestRatePerMonth:F2}%";
                tloaninterestamt.Text = $"₱{totalInterestAmount:N2}";
                tloanprocessfee.Text = $"₱{processingFeeAmount:N2}";  // Display fixed amount
                tloanamort.Text = $"₱{amortizationPerPayment:N2}";
                _actualLoanPayable = totalPayable;
                tloanamountpay.Text = $"₱{_actualLoanPayable:N2}";

                UpdateLoanBalance();
                UpdateMaturityDate();
            }
            else
            {
                ResetLoanAmountDetails();
            }
        }


        private int GetTotalPayments(int termInMonths, string paymentMode)
        {
            switch (paymentMode.ToUpper())
            {
                case "DAILY":
                    return termInMonths * 22; // 22 working days/month approx.
                case "WEEKLY":
                    return termInMonths * 4;
                case "SEMI-MONTHLY":
                    return termInMonths * 2;
                case "MONTHLY":
                    return termInMonths;
                default:
                    return termInMonths;
            }
        }

        private void UpdateLoanBalance()
        {
            string rawPaidText = tloanpaid.Text.Replace("₱", "").Replace(",", "").Trim();

            if (double.TryParse(rawPaidText, out double amountPaid))
            {
                double loanBalance = _actualLoanPayable - amountPaid;
                loanBalance = Math.Max(0, loanBalance); // Avoid negative values

                tloanbal.Text = $"₱{loanBalance:N2}";
                tloanamountpay.Text = $"₱{loanBalance:N2}";
            }
            else
            {
                // Invalid input, fallback to full payable
                tloanbal.Text = $"₱{_actualLoanPayable:N2}";
                tloanamountpay.Text = $"₱{_actualLoanPayable:N2}";
            }
        }



        private async Task TryComputeLoan()
        {
            if (!double.TryParse(tloanamt.Text.Replace("₱", "").Replace(",", "").Trim(), out double principal) || principal <= 0)
            {
                ResetLoanAmountDetails();
                return;
            }

            if (!int.TryParse(tloanterm.Text.Split(' ')[0], out int term) || term <= 0)
            {
                ResetLoanAmountDetails();
                return;
            }

            if (string.IsNullOrWhiteSpace(cbpaymentmode.Text))
            {
                ResetLoanAmountDetails();
                return;
            }

            await CalculateLoanDetails(principal, term);
        }

        private void UpdateMaturityDate()
        {
            if (!int.TryParse(tloanterm.Text.Split(' ')[0], out int termInMonths))
            {
                return;
            }

            dtenddate.Value = dtstartdate.Value.AddMonths(termInMonths);
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
                if (string.IsNullOrEmpty(tloanno.Text))
                {
                    MessageBox.Show("Loan Number cannot be empty.");
                    return;
                }

                if (!ValidateFormFields())
                {
                    return;
                }

                string platform = cbplatform.SelectedItem?.ToString() ?? "CASH";
                string platformType = cbpaymenttype.Visible ? cbpaymenttype.SelectedItem?.ToString() ?? "" : "";

                var loanDisbursedDocument = new BsonDocument
                {
                    { "AccountId", taccountid.Text },
                    { "LoanNo", tloanno.Text },
                    { "ClientNo", tclientno.Text },
                    { "LoanType", cbloantype.SelectedItem?.ToString() ?? "" },
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
                    { "Penalty", tloanpenalty.Text.Replace("₱", "").Replace(",", "").Trim() },
                    { "Payment_Platform", platform },
                    { "Payment_Platform_Type", platformType },
                    { "DateStart", dtstartdate.Value.ToString("MM/dd/yyyy") },
                    { "DateEnd", dtenddate.Value.ToString("MM/dd/yyyy") },
                    { "Date_Encoded", DateTime.Now.ToString("MM/dd/yyyy") },
                    { "DateApproved", DateTime.Now.ToString("MM/dd/yyyy") },
                    { "ApprovedBy", "Ralp Daag" }
                };

                await loanDisbursedCollection.InsertOneAsync(loanDisbursedDocument);

                var loanApprovedDocument = new BsonDocument
                {
                    { "AccountId", taccountid.Text },
                    { "LoanNo", tloanno.Text },
                    { "ClientNo", tclientno.Text },
                    { "LoanType", cbloantype.SelectedItem?.ToString() ?? "" },
                    { "LoanStatus", cbloanstatus.SelectedItem?.ToString() ?? "" },
                    { "LastName", tclientlastname.Text },
                    { "FirstName", tclientfirstname.Text },
                    { "MiddleName", tclientmiddlename.Text },
                    { "CollectorName", cbcollector.Text },
                    { "Barangay", tbrgy.Text },
                    { "City", tcity.Text },
                    { "Province", tprovince.Text },
                    { "LoanTerm", tloanterm.Text },
                    { "LoanAmount", tloanamt.Text.Replace("₱", "").Replace(",", "").Trim() },
                    { "LoanAmortization", tloanamort.Text.Replace("₱", "").Replace(",", "").Trim() },
                    { "LoanBalance", tloanbal.Text.Replace("₱", "").Replace(",", "").Trim() },
                    { "Penalty", tloanpenalty.Text.Replace("₱", "").Replace(",", "").Trim() },
                    { "LoanInterest", tloaninterestamt.Text },
                    { "PaymentMode", cbpaymentmode.SelectedItem?.ToString() ?? "" },
                    { "StartPaymentDate", dtstartdate.Value.ToString("MM/dd/yyyy") },
                    { "MaturityDate", dtenddate.Value.ToString("MM/dd/yyyy") },
                    { "Date_Encoded", DateTime.Now.ToString("MM/dd/yyyy") },
                    { "LoanProcessStatus", "Updated" },
                    { "PrincipalAmount",tloanamt.Text.Replace("₱", "").Replace(",", "").Trim() },
                    { "Date_Modified", DateTime.Now }
                };

                // Insert to loan_approved collection
                await loanApprovedCollection.InsertOneAsync(loanApprovedDocument);


                MessageBox.Show("Loan information saved successfully!");
                ClearFormFields();
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
            string selectedAmountText = tloanamt.Text.Trim();

            if (string.IsNullOrWhiteSpace(selectedAmountText) ||
                !double.TryParse(selectedAmountText.Replace("₱", "").Replace(",", ""), out double selectedAmount) ||
                selectedAmount <= 0)
            {
                ResetLoanAmountDetails();
                return;
            }

            var filter = Builders<BsonDocument>.Filter.Eq("Principal", selectedAmount);
            var loanConfig = await loanRateCollection.Find(filter).FirstOrDefaultAsync();

            if (loanConfig != null && loanConfig.Contains("Term"))
            {
                var termValue = loanConfig["Term"];

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
            await TryComputeLoan();
        }

        private void ResetLoanAmountDetails()
        {
            tloanterm.Text = "";
            tloaninterest.Text = "";
            tloaninterestamt.Text = "";
            tloanamort.Text = "";
            tloanprocessfee.Text = "";
            tloanamountpay.Text = "";
            tloanbal.Text = "";
        }


        private async void cbpaymentmode_SelectedIndexChanged(object sender, EventArgs e)
        {
            await TryComputeLoan();
        }

        private void tloanbal_TextChanged(object sender, EventArgs e)
        {
            UpdateLoanBalance();
        }

        private void tloanpaid_TextChanged(object sender, EventArgs e)
        {
            // Temporarily detach event to prevent recursion
            tloanpaid.TextChanged -= tloanpaid_TextChanged;

            string rawText = tloanpaid.Text.Replace("₱", "").Replace(",", "").Trim();
            int caretPosition = tloanpaid.SelectionStart;

            if (double.TryParse(rawText, out double amountPaid))
            {
                // Only reformat with commas (no .00)
                string[] parts = rawText.Split('.');
                string integerPart = parts[0];
                string formatted = $"₱{int.Parse(integerPart):N0}";

                // Append decimal if user typed it
                if (parts.Length > 1)
                {
                    formatted += "." + parts[1];
                }

                tloanpaid.Text = formatted;
                tloanpaid.SelectionStart = Math.Min(caretPosition + (tloanpaid.Text.Length - rawText.Length), tloanpaid.Text.Length);
            }
            else
            {
                // Invalid input, just keep raw text without prefix
                tloanpaid.Text = rawText;
                tloanpaid.SelectionStart = rawText.Length;
            }

            // Reattach event
            tloanpaid.TextChanged += tloanpaid_TextChanged;

            // Update balance only if input is valid
            UpdateLoanBalance();
        }

        private void dtstartdate_ValueChanged(object sender, EventArgs e)
        {
            UpdateMaturityDate();
        }

        private void tloanpaid_Leave(object sender, EventArgs e)
        {
            string rawText = tloanpaid.Text.Replace("₱", "").Replace(",", "").Trim();
            if (double.TryParse(rawText, out double amountPaid))
            {
                tloanpaid.Text = $"₱{amountPaid:N2}";
                UpdateLoanBalance();
            }
        }

        private void cbplatform_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedPlatform = cbplatform.SelectedItem?.ToString();

            // Clear existing items and hide controls by default
            cbpaymenttype.Items.Clear();
            cbpaymenttype.Visible = false;
            lspecifytype.Visible = false;

            if (selectedPlatform == "E-WALLET/ONLINE")
            {
                // Show and populate with e-wallet options
                cbpaymenttype.Visible = true;
                lspecifytype.Visible = true;
                cbpaymenttype.Items.AddRange(new string[]
                {
                   "GCash",
                   "Maya/Paymaya",
                   "Shopee Pay",
                   "Lazada Wallet",
                   "GoTyme"
                });
            }
            else if (selectedPlatform == "BANK TRANSFER")
            {
                // Show and populate with bank options
                cbpaymenttype.Visible = true;
                lspecifytype.Visible = true;
                cbpaymenttype.Items.AddRange(new string[]
                {
                     "BDO",
                     "BPI",
                     "Landbank",
                     "ChinaBank",
                     "Eastwest Bank",
                     "Metro Bank",
                     "Security Bank"
                });
            }

            cbpaymenttype.SelectedIndex = -1; // Optional: reset selection
        }
    }
}

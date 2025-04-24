using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rct_lmis.DISBURSEMENT_SECTION
{
    public partial class frm_home_disburse_collections_addex : Form
    {
        private string _clientNo;
        private IMongoCollection<BsonDocument> _loanDisbursedCollection;
        private IMongoCollection<BsonDocument> _loanCollectorsCollection;

        public frm_home_disburse_collections_addex(string clientNo)
        {
            InitializeComponent();
            _clientNo = clientNo;

            var database = MongoDBConnection.Instance.Database;
            _loanDisbursedCollection = database.GetCollection<BsonDocument>("loan_disbursed");
            _loanCollectorsCollection = database.GetCollection<BsonDocument>("loan_collectors");
        }

        private async void frm_home_disburse_collections_addex_LoadAsync(object sender, EventArgs e)
        {
            clientnotest.Text = _clientNo;
            await LoadClientName(_clientNo);
            LoadLoanDisbursedData();

            LoadCollectors();
        }

        private decimal ParseDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0;
            value = value.Replace("₱", "").Replace(",", "").Trim();
            return decimal.TryParse(value, out var result) ? result : 0;
        }

        private int ExtractNumeric(string input)
        {
            var digits = new string(input.Where(char.IsDigit).ToArray());
            return int.TryParse(digits, out int result) ? result : 0;
        }

        private DateTime ParseDate(string value)
        {
            return DateTime.TryParse(value, out DateTime result) ? result : DateTime.MinValue;
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
                string lastName = nameParts[nameParts.Length - 1]; // C# 7.3 compatible
                string middleName = nameParts.Length > 2 ? string.Join(" ", nameParts.Skip(1).Take(nameParts.Length - 2)) : "";

                var filter = Builders<BsonDocument>.Filter.And(
                    Builders<BsonDocument>.Filter.Eq("FirstName", firstName),
                    Builders<BsonDocument>.Filter.Eq("LastName", lastName)
                );


                if (!string.IsNullOrEmpty(middleName))
                    filter = Builders<BsonDocument>.Filter.And(filter, Builders<BsonDocument>.Filter.Eq("MiddleName", middleName));

                var loanDisbursed = await _loanDisbursedCollection.Find(filter).FirstOrDefaultAsync();

                if (loanDisbursed != null)
                {
                    try 
                    {
                        // Assign simple values safely
                        tclientno.Text = SafeString(loanDisbursed, "ClientNo");
                        tloanid.Text = SafeString(loanDisbursed, "LoanNo");
                        tloanamt.Text = SafeString(loanDisbursed, "LoanAmount");
                        tterm.Text = SafeString(loanDisbursed, "LoanTerm");
                        tpaymode.Text = SafeString(loanDisbursed, "PaymentMode");
                        tpayamort.Text = SafeString(loanDisbursed, "LoanAmortization");




                        // Extract Address (handle possible nulls)
                        string barangay = loanDisbursed.GetValue("Barangay", "").ToString();
                        string city = loanDisbursed.GetValue("City", "").ToString();

                        // Parse and compute values safely
                        decimal loanAmount = SafeDecimal(loanDisbursed, "LoanAmount");
                        decimal loanInterest = SafeDecimal(loanDisbursed, "LoanInterest");


                        string loanTermRaw = SafeString(loanDisbursed, "LoanTerm", "0 months");
                        int loanTerm = int.TryParse(new string(loanTermRaw.Where(char.IsDigit).ToArray()), out int parsedTerm) ? parsedTerm : 0;

                        string startPaymentDateStr = SafeString(loanDisbursed, "StartPaymentDate", "");
                        DateTime startPaymentDate = DateTime.TryParse(startPaymentDateStr, out var spd) ? spd : DateTime.MinValue;
                        tpaystart.Text = startPaymentDate != DateTime.MinValue ? startPaymentDate.ToString("MM/dd/yyyy") : "N/A";

                        string maturityDateStr = loanDisbursed.GetValue("MaturityDate", "").ToString();
                        if (DateTime.TryParse(maturityDateStr, out DateTime maturityDate))
                        {
                            tpaymature.Text = maturityDate.ToString("MM/dd/yyyy");
                        }
                        else
                        {
                            tpaymature.Text = "N/A"; // Handle invalid dates gracefully
                        }


                        decimal principalDue = loanTerm > 0 ? loanAmount / loanTerm : 0;
                        decimal interestDue = loanTerm > 0 ? (loanAmount * (loanInterest / 100)) / loanTerm : 0;

                        tprincipaldue.Text = principalDue.ToString("C", new CultureInfo("en-PH"));
                        tcolinterest.Text = interestDue.ToString("C", new CultureInfo("en-PH"));


                        ComputeLoanBalance();
                    }
                    catch (Exception ex) 
                    {
                        MessageBox.Show($"Error loading loan disbursed data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    // ✅ Check if the account is incomplete
                    bool isIncomplete = string.IsNullOrWhiteSpace(loanDisbursed.GetValue("LoanAmount", "").ToString()) ||
                                        string.IsNullOrWhiteSpace(loanDisbursed.GetValue("LoanAmortization", "").ToString()) ||
                                        string.IsNullOrWhiteSpace(loanDisbursed.GetValue("StartPaymentDate", "").ToString());

                    if (isIncomplete)
                    {
                        lwarning.Text = "⚠️ This account has no recorded loan transaction. Please verify with loan officer.";
                        bsave.Enabled = false; // Disable save button

                        lwarning.BackColor = Color.LightYellow;
                        lwarning.ForeColor = Color.DarkRed;
                    }
                    else
                    {
                        lwarning.Text = ""; // Clear warning if data is complete
                        bsave.Enabled = true; // Enable save button
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading loan disbursed data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string SafeString(BsonDocument doc, string key, string fallback = "N/A")
        {
            if (doc.Contains(key) && doc[key] != BsonNull.Value)
            {
                string value = doc[key].ToString().Trim();
                return string.IsNullOrEmpty(value) ? fallback : value;
            }
            return fallback;
        }

        private decimal SafeDecimal(BsonDocument doc, string key)
        {
            var raw = SafeString(doc, key, "0");
            return decimal.TryParse(raw, out var result) ? result : 0m;
        }


        private decimal ConvertToDecimal(string input)
        {
            return decimal.TryParse(input, out decimal result) ? result : 0;
        }

        private void ComputeLoanBalance()
        {
            try
            {
                // Check Loan ID
                if (string.IsNullOrWhiteSpace(tloanid.Text))
                {
                    // Silent return — no loan selected
                    ClearComputationFields("Loan ID missing");
                    return;
                }

                // Try parsing Start Payment Date safely
                if (!DateTime.TryParse(tpaystart.Text, out DateTime startDate))
                {
                    // No need to show message — just clear output fields
                    ClearComputationFields("Invalid or empty start date");
                    return;
                }

                DateTime currentDate = DateTime.Now;

                // Parse amounts safely
                decimal loanAmount = ConvertToDecimal(tloanamt.Text?.Replace("₱", "").Replace(",", "").Trim());
                decimal amortization = ConvertToDecimal(tpayamort.Text?.Replace("₱", "").Replace(",", "").Trim());

                if (chexists.Checked)
                {
                    // Show loan balance only
                    tcolpenalty.Text = "₱0.00";
                    tprincipaldue.Text = "₱0.00";
                    tcolinterest.Text = "₱0.00";
                    tcoltotal.Text = "₱0.00";
                    tloanbal.Text = loanAmount.ToString("C", new CultureInfo("en-PH"));
                }
                else
                {
                    int daysElapsed = CalculateWeekdaysElapsed(startDate, currentDate);

                    decimal principalDue = (amortization * 0.833333333m) * daysElapsed;
                    decimal interestDue = (amortization * 0.166666667m) * daysElapsed;
                    decimal penalty = (principalDue + interestDue) * 0.03m * (daysElapsed / 30m);
                    decimal totalAmortization = principalDue + interestDue + penalty;

                    // Populate computed fields
                    tcolpenalty.Text = penalty.ToString("C", new CultureInfo("en-PH"));
                    tprincipaldue.Text = principalDue.ToString("C", new CultureInfo("en-PH"));
                    tcolinterest.Text = interestDue.ToString("C", new CultureInfo("en-PH"));
                    tcoltotal.Text = totalAmortization.ToString("C", new CultureInfo("en-PH"));
                    tloanbal.Text = loanAmount.ToString("C", new CultureInfo("en-PH"));
                }

                UpdatePaymentStatus(loanAmount);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error computing loan balance: {ex.Message}", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ClearComputationFields(string reason = "")
        {
            tcolpenalty.Text = "₱0.00";
            tprincipaldue.Text = "₱0.00";
            tcolinterest.Text = "₱0.00";
            tcoltotal.Text = "₱0.00";
            tloanbal.Text = "₱0.00";
            // Optional: Log to debug output for devs
            Console.WriteLine($"[INFO] Skipped loan balance computation: {reason}");
        }

        private void UpdatePaymentStatus(decimal loanAmount)
        {
            decimal loanBalance = ConvertToDecimal(tloanbal.Text.Replace("₱", "").Replace(",", ""));

            if (loanAmount == loanBalance)
            {
                tpaymentstatus.Text = "No collection exists";
            }
            else
            {
                decimal collectedAmount = loanAmount - loanBalance;
                tpaymentstatus.Text = $"Collected: ₱{collectedAmount:F2}";
            }
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

        private void chexists_CheckedChanged(object sender, EventArgs e)
        {
            ComputeLoanBalance();
        }

        private void bcancel_Click(object sender, EventArgs e)
        {
            // Prompt the user with a confirmation dialog
            var result = MessageBox.Show("Are you sure you want to cancel the transaction?", "Confirm Cancellation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            // If the user clicks "Yes"
            if (result == DialogResult.Yes)
            {
                // Perform any necessary cleanup actions or reset fields
                ResetFormFields();

                // Close the form
                this.Close();
            }
            // If the user clicks "No", do nothing (just return)
            else
            {
                return;
            }
        }

        private void ResetFormFields()
        {
            // Reset all the fields to their default or empty state
            tclientno.Text = "";
            tloanid.Text = "";
            tloanamt.Text = "";
            tterm.Text = "";
            tpaymode.Text = "";
            tpayamort.Text = "";
            tpaystart.Text = "";
            tpaymature.Text = "";
            tprincipaldue.Text = "";
            tcolinterest.Text = "";
            tcolpenalty.Text = "";
            tcoltotal.Text = "";
            tloanbal.Text = "";
            lwarning.Text = ""; // Reset the warning text
            bsave.Enabled = true; // Re-enable the save button
        }
    }
}

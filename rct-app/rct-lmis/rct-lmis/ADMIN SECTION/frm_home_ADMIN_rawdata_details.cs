using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace rct_lmis.ADMIN_SECTION
{
    public partial class frm_home_ADMIN_rawdata_details : Form
    {
        private string loanId;
        private string loanstatus;
        private IMongoDatabase database;
        private IMongoCollection<BsonDocument> loanRawdataCollection;
        private IMongoCollection<BsonDocument> loanApprovedCollection;

        public frm_home_ADMIN_rawdata_details(string loanId, string loanstatus)
        {
            InitializeComponent();
            litemno.Text = loanId;  // Assuming loanId is the same as item_no in this case
            lloanstatus.Text = loanstatus;

            database = MongoDBConnection.Instance.Database;
            loanRawdataCollection = database.GetCollection<BsonDocument>("loan_rawdata");
            loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");
        }

        LoadingFunction load = new LoadingFunction();

        private void LoanStatusColor()
        {
            // Get the current loan status text from the label
            string status = lloanstatus.Text;

            switch (status.ToUpper())  // Make it case-insensitive by converting to uppercase
            {
                case "UPDATED":
                    lloanstatus.BackColor = Color.Green;
                    lloanstatus.ForeColor = Color.White;
                    break;
                case "ARREARS":
                    lloanstatus.BackColor = Color.Yellow;
                    lloanstatus.ForeColor = Color.Black;
                    break;
                case "LITIGATION":
                    lloanstatus.BackColor = Color.Orange;
                    lloanstatus.ForeColor = Color.Black;
                    break;
                case "DORMANT":
                    lloanstatus.BackColor = Color.Gray;
                    lloanstatus.ForeColor = Color.White;
                    break;
                default:
                    // Optional: Set a default color for unhandled statuses
                    lloanstatus.BackColor = Color.LightGray;
                    lloanstatus.ForeColor = Color.Black;
                    break;
            }
        }


        private async void frm_home_ADMIN_rawdata_details_Load(object sender, EventArgs e)
        {
            // Set loan status color
            LoanStatusColor();

            // Call method to load data
            await LoadLoanDataAsync();
        }

        private async Task LoadLoanDataAsync()
        {
            // Convert item_no from string to integer for the query
            int itemNo = int.Parse(litemno.Text);

            // Query MongoDB for the document that matches the item_no in loan_rawdata
            var filter = Builders<BsonDocument>.Filter.Eq("item_no", itemNo);
            var loanData = await loanRawdataCollection.Find(filter).FirstOrDefaultAsync();

            if (loanData != null)
            {
                // Populate labels and textboxes with data from the loanData document
                lloanid.Text = loanData["loan_id"].ToString();
                tclientname.Text = loanData["client_name"].ToString();
                taddress.Text = loanData["address"].ToString();

                // Format loan term as "x months"
                tloanterm.Text = $"{loanData["loan_term"]} months";

                // Format monetary fields with PHP sign and two decimals
                tloanamount.Text = FormatAsCurrency(loanData["loan_amount"].ToDecimal());
                tloanamt.Text = FormatAsCurrency(loanData["loan_amortization"].ToDecimal());
                tloanbal.Text = FormatAsCurrency(loanData["loan_balance"].ToDecimal());
                tloanpenalty.Text = loanData.Contains("penalty") ? FormatAsCurrency(loanData["penalty"].ToDecimal()) : "₱0.00";

                // Format interest as percentage
                tloaninterest.Text = loanData.Contains("loan_interest") ? $"{loanData["loan_interest"].ToDecimal():P2}" : "0%";

                // Format payment mode
                tloanpaymode.Text = loanData["payment_mode"].ToString();

                // Format date fields to MM/dd/yyyy
                tloanstartday.Text = FormatDate(loanData["start_payment_date"].ToString());
                tloanendday.Text = FormatDate(loanData["maturity_date"].ToString());

                // Fetch collector name from loan_collectors collection
                string collectorName = loanData["collector_name"].ToString();
                var collectorsCollection = database.GetCollection<BsonDocument>("loan_collectors");
                var collectorFilter = Builders<BsonDocument>.Filter.Eq("collector_name", collectorName);
                var collectorData = await collectorsCollection.Find(collectorFilter).FirstOrDefaultAsync();

                if (collectorData != null)
                {
                    // Populate tcollector with the name from loan_collectors
                    tcollector.Text = collectorData["Name"].ToString();
                }
                else
                {
                    // If no matching collector is found, display the original collector name
                    tcollector.Text = collectorName;
                }
            }
            else
            {
                MessageBox.Show("Loan data not found for the given Item No.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Helper method to format decimal values as Philippine Peso
        private string FormatAsCurrency(decimal amount)
        {
            return string.Format(CultureInfo.CreateSpecificCulture("en-PH"), "₱{0:N2}", amount);
        }

        // Helper method to format dates as MM/dd/yyyy
        private string FormatDate(string dateString)
        {
            DateTime parsedDate = DateTime.Parse(dateString);
            return parsedDate.ToString("MM/dd/yyyy");
        }

        // Helper function to generate incremental Account ID
        private async Task<string> GenerateAccountIDAsync()
        {
            var lastEntry = await loanApprovedCollection.Find(new BsonDocument())
                .Sort(Builders<BsonDocument>.Sort.Descending("AccountId"))
                .Limit(1)
                .FirstOrDefaultAsync();

            if (lastEntry != null && lastEntry.Contains("AccountId"))
            {
                string lastId = lastEntry["AccountId"].ToString();
                int lastNum = int.Parse(lastId.Split('-')[2]);
                return $"RCT-2024-{(lastNum + 1):D3}";
            }

            return "RCT-2024-001";
        }

        // Helper function to generate Loan No
        private string GenerateLoanNo(int loanId)
        {
            return $"RCT-2024-{loanId}";
        }

        // Helper function to generate incremental Client No
        private async Task<string> GenerateClientNoAsync()
        {
            var lastEntry = await loanApprovedCollection.Find(new BsonDocument())
                .Sort(Builders<BsonDocument>.Sort.Descending("ClientNo"))
                .Limit(1)
                .FirstOrDefaultAsync();

            if (lastEntry != null && lastEntry.Contains("ClientNo"))
            {
                string lastClientNo = lastEntry["ClientNo"].ToString();
                int lastNum = int.Parse(lastClientNo.Split(new[] { "CL" }, StringSplitOptions.None)[1]);
                return $"RCT-2024-CL{(lastNum + 1):D4}";
            }

            return "RCT-2024-CL0001";
        }

        // General Save Data function to save data to loan_approved collection
        private async void GenerelSaveData()
        {
            try
            {
                var loanType = "New";  // Assuming a default loan type for now
                var status = lloanstatus.Text;

                // Generate required IDs
                string accountId = await GenerateAccountIDAsync();
                int loanId = int.Parse(lloanid.Text);  // Assuming loan ID is stored in lloanid.Text
                string loanNo = GenerateLoanNo(loanId);
                string clientNo = await GenerateClientNoAsync();

                // Split the client's name into Last Name, First Name, and Middle Name
                string fullName = tclientname.Text.Trim();
                string lastName = string.Empty;
                string firstName = string.Empty;
                string middleName = string.Empty;

                if (fullName.Contains(","))
                {
                    // Split by comma for Last Name, First Name Middle Name format
                    string[] nameParts = fullName.Split(',');
                    lastName = nameParts[0].Trim();  // Last Name before the comma
                    string[] firstAndMiddle = nameParts[1].Trim().Split(' ');

                    if (firstAndMiddle.Length > 1)
                    {
                        // If there are more than one word in the second part, we assume Middle Name exists
                        firstName = firstAndMiddle[0].Trim();
                        middleName = string.Join(" ", firstAndMiddle.Skip(1)).Trim();  // Combine remaining parts as Middle Name
                    }
                    else
                    {
                        firstName = firstAndMiddle[0].Trim();  // No Middle Name, just First Name
                    }
                }
                else
                {
                    MessageBox.Show("Client name format is incorrect. Please provide Last Name, First Name Middle Name format.", "Invalid Name Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Split the address into Barangay, City, and Province
                string fullAddress = taddress.Text.Trim();
                string barangay = string.Empty;
                string city = string.Empty;
                string province = string.Empty;

                if (fullAddress.Contains(","))
                {
                    // Split by comma for Barangay, City, Province format
                    string[] addressParts = fullAddress.Split(',');
                    barangay = addressParts[0].Trim();  // Barangay before the first comma
                    city = addressParts.Length > 1 ? addressParts[1].Trim() : string.Empty;  // City between commas
                    province = addressParts.Length > 2 ? addressParts[2].Trim() : string.Empty;  // Province after the last comma
                }
                else
                {
                    MessageBox.Show("Address format is incorrect. Please provide Barangay, City, Province format.", "Invalid Address Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Create BsonDocument for the new loan_approved entry
                var approvedLoanData = new BsonDocument
                 {
                     { "AccountId", accountId },
                     { "LoanNo", loanNo },
                     { "ClientNo", clientNo },
                     { "LoanType", loanType },
                     { "LoanStatus", status },
                     { "LastName", lastName },
                     { "FirstName", firstName },
                     { "MiddleName", middleName },
                     { "CollectorName", tcollector.Text },
                     { "Barangay", barangay },
                     { "City", city },
                     { "Province", province },
                     { "LoanTerm", tloanterm.Text },
                     { "LoanAmount", tloanamount.Text },
                     { "LoanAmortization", tloanamt.Text },
                     { "LoanBalance", tloanbal.Text },
                     { "Penalty", tloanpenalty.Text },
                     { "LoanInterest", tloaninterest.Text },
                     { "PaymentMode", tloanpaymode.Text },
                     { "StartPaymentDate", tloanstartday.Text },
                     { "MaturityDate", tloanendday.Text },
                     { "Date_Encoded", DateTime.Now.ToString("MM/dd/yyyy") },
                     { "LoanProcessStatus", "Loan Released" },
                 };

                // Save to loan_approved collection
                await loanApprovedCollection.InsertOneAsync(approvedLoanData);

            }
            catch (Exception e)
            {
                MessageBox.Show("There was a problem saving the transaction: " + e.Message, "Transaction Not Saved", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void badd_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save the transaction?", "Save Transaction", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                load.Show(this);
                Thread.Sleep(100);
                GenerelSaveData();
                load.Close();


                MessageBox.Show("Transaction has been saved successfully!", "Transaction Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

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
        private IMongoCollection<BsonDocument> loanDisburseCollection;

        public frm_home_ADMIN_rawdata_details(string loanId, string loanstatus)
        {
            InitializeComponent();
            litemno.Text = loanId;  // Assuming loanId is the same as item_no in this case
            lloanstatus.Text = loanstatus;

            database = MongoDBConnection.Instance.Database;
            loanRawdataCollection = database.GetCollection<BsonDocument>("loan_rawdata");
            loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");
            loanDisburseCollection = database.GetCollection<BsonDocument>("loan_disbursed");
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

        private async Task<string> GenerateDisburseIDAsync()
        {
            var lastEntry = await loanDisburseCollection.Find(new BsonDocument())
                .Sort(Builders<BsonDocument>.Sort.Descending("AccountId"))
                .Limit(1)
                .FirstOrDefaultAsync();

            if (lastEntry != null && lastEntry.Contains("AccountId"))
            {
                string lastId = lastEntry["AccountId"].ToString();
                int lastNum = int.Parse(lastId.Split('-')[2]);
                return $"RCT-DB2024-{(lastNum + 1):D3}";
            }

            return "RCT-2024DB-001";
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

        private async Task<string> GenerateClientNoDisburseAsync()
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

                // Check if a record with the same LoanNo or ClientNo already exists in the loan_approved collection
                var filter = Builders<BsonDocument>.Filter.Or(
                    Builders<BsonDocument>.Filter.Eq("LoanNo", loanNo),
                    Builders<BsonDocument>.Filter.Eq("ClientNo", clientNo)
                );

                var existingLoan = await loanApprovedCollection.Find(filter).FirstOrDefaultAsync();
                if (existingLoan != null)
                {
                    // Record with the same LoanNo or ClientNo already exists
                    MessageBox.Show("A loan with the same LoanNo or ClientNo already exists in the system.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;  // Stop the process here
                }

                // Split the client's name into Last Name, First Name, and Middle Name
                string fullName = tclientname.Text.Trim();
                string lastName = string.Empty;
                string firstName = string.Empty;
                string middleName = string.Empty;

                // Remove extra spaces and handle potential formatting issues
                if (fullName.Contains(","))
                {
                    string[] nameParts = fullName.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (nameParts.Length >= 2)
                    {
                        lastName = nameParts[0].Trim();
                        string[] firstAndMiddle = nameParts[1].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (firstAndMiddle.Length > 1)
                        {
                            firstName = firstAndMiddle[0].Trim();
                            middleName = string.Join(" ", firstAndMiddle.Skip(1)).Trim();
                        }
                        else
                        {
                            firstName = firstAndMiddle[0].Trim();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Client name format is incorrect. Please provide Last Name, First Name Middle Name format.", "Invalid Name Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Client name format is incorrect. Please provide Last Name, First Name Middle Name format.", "Invalid Name Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string fullAddress = taddress.Text.Trim();
                string barangay = string.Empty;
                string city = string.Empty;
                string province = string.Empty;

                string[] addressParts = fullAddress.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Handle address formats
                if (fullAddress.Contains(","))
                {
                    // Existing logic for comma-separated values
                    addressParts = fullAddress.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    barangay = addressParts[0].Trim();
                    city = addressParts.Length > 1 ? addressParts[1].Trim() : string.Empty;
                    province = addressParts.Length > 2 ? addressParts[2].Trim() : string.Empty;
                }
                else if (addressParts.Length >= 2)
                {
                    // Assuming last part is the province if there are at least 2 parts
                    barangay = addressParts[0].Trim();
                    city = addressParts[1].Trim();

                    // If there are more than 2 parts, the remaining are considered part of the province
                    if (addressParts.Length > 2)
                    {
                        province = string.Join(" ", addressParts.Skip(2)).Trim();
                    }
                }
                else
                {
                    MessageBox.Show("Address format is incorrect. Please provide either 'Barangay, City' or 'Barangay City Province' format.", "Invalid Address Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                MessageBox.Show("Loan data has been saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show("There was a problem saving the transaction: " + e.Message, "Transaction Not Saved", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void GenerelDisbursedData()
        {
            try
            {
                var loanType = "New";  // Assuming a default loan type for now
                var status = lloanstatus.Text;

                // Generate required IDs
                string disburseId = await GenerateDisburseIDAsync();
                int loanId = int.Parse(lloanid.Text);  // Assuming loan ID is stored in lloanid.Text
                string loanNo = GenerateLoanNo(loanId);
                string clientNo = await GenerateClientNoDisburseAsync();

                // Check if the loan already exists in the loanDisburseCollection
                var existingDisburse = await loanDisburseCollection.Find(new BsonDocument
                 {
                     { "LoanNo", loanNo },
                     { "ClientNo", clientNo }
                 }).FirstOrDefaultAsync();

                if (existingDisburse != null)
                {
                    MessageBox.Show("A disbursement with this Loan Number or Client Number already exists.", "Duplicate Disbursement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Stop further execution
                }

                // Split the client's name into Last Name, First Name, and Middle Name
                string fullName = tclientname.Text.Trim();
                string lastName = string.Empty;
                string firstName = string.Empty;
                string middleName = string.Empty;

                // Remove extra spaces and handle potential formatting issues
                if (fullName.Contains(","))
                {
                    string[] nameParts = fullName.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (nameParts.Length >= 2)
                    {
                        lastName = nameParts[0].Trim();
                        string[] firstAndMiddle = nameParts[1].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (firstAndMiddle.Length > 1)
                        {
                            firstName = firstAndMiddle[0].Trim();
                            middleName = string.Join(" ", firstAndMiddle.Skip(1)).Trim();
                        }
                        else
                        {
                            firstName = firstAndMiddle[0].Trim();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Client name format is incorrect. Please provide Last Name, First Name Middle Name format.", "Invalid Name Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Client name format is incorrect. Please provide Last Name, First Name Middle Name format.", "Invalid Name Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string fullAddress = taddress.Text.Trim();
                string barangay = string.Empty;
                string city = string.Empty;
                string province = string.Empty;

                string[] addressParts = fullAddress.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Handle address formats
                if (fullAddress.Contains(","))
                {
                    // Existing logic for comma-separated values
                    addressParts = fullAddress.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    barangay = addressParts[0].Trim();
                    city = addressParts.Length > 1 ? addressParts[1].Trim() : string.Empty;
                    province = addressParts.Length > 2 ? addressParts[2].Trim() : string.Empty;
                }
                else if (addressParts.Length >= 2)
                {
                    // Assuming last part is the province if there are at least 2 parts
                    barangay = addressParts[0].Trim();
                    city = addressParts[1].Trim();

                    // If there are more than 2 parts, the remaining are considered part of the province
                    if (addressParts.Length > 2)
                    {
                        province = string.Join(" ", addressParts.Skip(2)).Trim();
                    }
                }
                else
                {
                    MessageBox.Show("Address format is incorrect. Please provide either 'Barangay, City' or 'Barangay City Province' format.", "Invalid Address Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Create BsonDocument for the new loan_disburse entry
                var approvedLoanData = new BsonDocument
                 {
                     { "AccountId", disburseId },
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
                     { "LoanProcessStatus", "Loan Released" }
                 };

                // Save to loanDisburseCollection
                await loanDisburseCollection.InsertOneAsync(approvedLoanData);

                //MessageBox.Show("Disbursement data has been saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show("There was a problem saving the transaction: " + e.Message, "Transaction Not Saved", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void DeleteLoanDataAsync()
        {
            try
            {
                // Get the item number to be deleted
                int itemNo = int.Parse(litemno.Text.Trim());

                if (itemNo == 0)
                {
                    MessageBox.Show("Please provide a valid Item No to delete.", "Invalid Item No", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Confirm the deletion with the user
                var confirmResult = MessageBox.Show("Are you sure you want to delete this loan data?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    // Create a filter to match the item_no in the loan_rawdata collection
                    var filter = Builders<BsonDocument>.Filter.Eq("item_no", itemNo);

                    // Delete the loan data document from loan_rawdata collection
                    var deleteResult = await loanRawdataCollection.DeleteOneAsync(filter);

                    if (deleteResult.DeletedCount > 0)
                    {
                        MessageBox.Show("Loan data deleted successfully.", "Delete Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Optionally, clear the fields after successful deletion
                        ClearLoanDataFields();
                    }
                    else
                    {
                        MessageBox.Show("Loan data not found or could not be deleted.", "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was a problem deleting the loan data: " + ex.Message, "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper method to clear fields after deletion
        private void ClearLoanDataFields()
        {
            lloanid.Text = string.Empty;
            tclientname.Text = string.Empty;
            taddress.Text = string.Empty;
            tloanterm.Text = string.Empty;
            tloanamount.Text = string.Empty;
            tloanamt.Text = string.Empty;
            tloanbal.Text = string.Empty;
            tloanpenalty.Text = string.Empty;
            tloaninterest.Text = string.Empty;
            tloanpaymode.Text = string.Empty;
            tloanstartday.Text = string.Empty;
            tloanendday.Text = string.Empty;
            tcollector.Text = string.Empty;
        }


        private void badd_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to import the account?", "Save Transaction", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                load.Show(this);
                Thread.Sleep(100);
                GenerelSaveData();
                GenerelDisbursedData();
                load.Close();


                MessageBox.Show("Transactions has been imported successfully!", "Transaction Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void bdelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete the account?", "Save Transaction", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                load.Show(this);
                Thread.Sleep(100);
                load.Close();
                DeleteLoanDataAsync();
                this.Close();
            }
        }
    }
}

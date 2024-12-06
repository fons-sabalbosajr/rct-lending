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

        public frm_home_ADMIN_rawdata_details(string loanId, string loanstatus, string itemno)
        {
            InitializeComponent();
            lloanid.Text = loanId;  // Assuming loanId is the same as item_no in this case
            lloanstatus.Text = loanstatus;
            litemno.Text = itemno;

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
            try
            {
                // Convert Loan ID from TextBox to an integer
                if (int.TryParse(lloanid.Text, out int loanId))
                {
                    // Query MongoDB for the document that matches the loan_id
                    var filter = Builders<BsonDocument>.Filter.Eq("loan_id", loanId);
                    var loanData = await loanRawdataCollection.Find(filter).FirstOrDefaultAsync();

                    if (loanData != null)
                    {
                        // Use try-catch for fields that might be missing
                        try
                        {
                            tclientname.Text = loanData.Contains("client_name") ? loanData["client_name"].ToString() : "N/A";
                        }
                        catch
                        {
                            tclientname.Text = "N/A";  // Default value if field is missing
                        }

                        try
                        {
                            taddress.Text = loanData.Contains("address") ? loanData["address"].ToString() : "N/A";
                        }
                        catch
                        {
                            taddress.Text = "N/A";  // Default value if field is missing
                        }

                        try
                        {
                            tloanterm.Text = loanData.Contains("loan_term") ? $"{loanData["loan_term"]} months" : "N/A";
                        }
                        catch
                        {
                            tloanterm.Text = "N/A";  // Default value if field is missing
                        }

                        try
                        {
                            tloanamount.Text = loanData.Contains("loan_amount") ? FormatAsCurrency(loanData["loan_amount"].ToDecimal()) : "₱0.00";
                        }
                        catch
                        {
                            tloanamount.Text = "₱0.00";  // Default value if field is missing
                        }

                        try
                        {
                            tloanamt.Text = loanData.Contains("loan_amortization") ? FormatAsCurrency(loanData["loan_amortization"].ToDecimal()) : "₱0.00";
                        }
                        catch
                        {
                            tloanamt.Text = "₱0.00";  // Default value if field is missing
                        }

                        try
                        {
                            tloanbal.Text = loanData.Contains("loan_balance") ? FormatAsCurrency(loanData["loan_balance"].ToDecimal()) : "₱0.00";
                        }
                        catch
                        {
                            tloanbal.Text = "₱0.00";  // Default value if field is missing
                        }

                        try
                        {
                            tloanpenalty.Text = loanData.Contains("penalty") ? FormatAsCurrency(loanData["penalty"].ToDecimal()) : "₱0.00";
                        }
                        catch
                        {
                            tloanpenalty.Text = "₱0.00";  // Default value if field is missing
                        }

                        try
                        {
                            tloaninterest.Text = loanData.Contains("loan_interest") ? $"{loanData["loan_interest"].ToDecimal():P2}" : "0%";
                        }
                        catch
                        {
                            tloaninterest.Text = "0%";  // Default value if field is missing
                        }

                        try
                        {
                            tloanpaymode.Text = loanData.Contains("payment_mode") ? loanData["payment_mode"].ToString() : "N/A";
                        }
                        catch
                        {
                            tloanpaymode.Text = "N/A";  // Default value if field is missing
                        }

                        try
                        {
                            tloanstartday.Text = loanData.Contains("start_payment_date") ? FormatDate(loanData["start_payment_date"].ToString()) : "N/A";
                        }
                        catch
                        {
                            tloanstartday.Text = "N/A";  // Default value if field is missing
                        }

                        try
                        {
                            tloanendday.Text = loanData.Contains("maturity_date") ? FormatDate(loanData["maturity_date"].ToString()) : "N/A";
                        }
                        catch
                        {
                            tloanendday.Text = "N/A";  // Default value if field is missing
                        }

                        // Safely load collector name
                        string collectorName = loanData.Contains("collector_name") ? loanData["collector_name"].ToString() : "N/A";
                        var collectorsCollection = database.GetCollection<BsonDocument>("loan_collectors");
                        var collectorFilter = Builders<BsonDocument>.Filter.Eq("collector_name", collectorName);
                        var collectorData = await collectorsCollection.Find(collectorFilter).FirstOrDefaultAsync();

                        tcollector.Text = collectorData != null ? collectorData["Name"].ToString() : collectorName;
                    }
                    else
                    {
                        MessageBox.Show("Loan data not found for the given Loan ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Loan ID. Please enter a numeric value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private async Task<bool> GenerelSaveData()
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
                    MessageBox.Show("A loan with the same LoanNo or ClientNo already exists in the system.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false; // Indicate failure
                }

                // Split the client's name into Last Name, First Name, and Middle Name
                string fullName = tclientname.Text.Trim();
                string lastName = string.Empty;
                string firstName = string.Empty;
                string middleName = string.Empty;

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
                        throw new FormatException("Client name format is incorrect. Please provide 'Last Name, First Name Middle Name' format.");
                    }
                }
                else
                {
                    throw new FormatException("Client name format is incorrect. Please provide 'Last Name, First Name Middle Name' format.");
                }

                string fullAddress = taddress.Text.Trim();
                string barangay = string.Empty;
                string city = string.Empty;
                string province = string.Empty;

                if (!string.IsNullOrEmpty(fullAddress))
                {
                    string[] addressParts = fullAddress.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    barangay = addressParts.Length > 0 ? addressParts[0].Trim() : string.Empty;
                    city = addressParts.Length > 1 ? addressParts[1].Trim() : string.Empty;
                    province = addressParts.Length > 2 ? addressParts[2].Trim() : string.Empty;
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
                    { "LoanProcessStatus", "Loan Released" }
                };

                await loanApprovedCollection.InsertOneAsync(approvedLoanData);
                return true;
            }
            catch (FormatException fe)
            {
                MessageBox.Show(fe.Message, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("There was a problem saving the transaction: " + e.Message, "Transaction Not Saved", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Indicate failure
            }
        }


        private async Task<bool>GenerelDisbursedData()
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
                var filter = Builders<BsonDocument>.Filter.Or(
                    Builders<BsonDocument>.Filter.Eq("LoanNo", loanNo),
                    Builders<BsonDocument>.Filter.Eq("ClientNo", clientNo)
                );

                var existingDisburse = await loanDisburseCollection.Find(filter).FirstOrDefaultAsync();
                if (existingDisburse != null)
                {
                    MessageBox.Show("A disbursement with this Loan Number or Client Number already exists.", "Duplicate Disbursement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false; // Indicate failure
                }

                // Split the client's name into Last Name, First Name, and Middle Name
                string fullName = tclientname.Text.Trim();
                string lastName = string.Empty;
                string firstName = string.Empty;
                string middleName = string.Empty;

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
                        throw new FormatException("Client name format is incorrect. Please provide 'Last Name, First Name Middle Name' format.");
                    }
                }
                else
                {
                    throw new FormatException("Client name format is incorrect. Please provide 'Last Name, First Name Middle Name' format.");
                }

                string fullAddress = taddress.Text.Trim();
                string barangay = string.Empty;
                string city = string.Empty;
                string province = string.Empty;

                if (!string.IsNullOrEmpty(fullAddress))
                {
                    string[] addressParts = fullAddress.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    barangay = addressParts.Length > 0 ? addressParts[0].Trim() : string.Empty;
                    city = addressParts.Length > 1 ? addressParts[1].Trim() : string.Empty;
                    province = addressParts.Length > 2 ? addressParts[2].Trim() : string.Empty;
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

                await loanDisburseCollection.InsertOneAsync(approvedLoanData);
                return true;
            }
            catch (FormatException fe)
            {
                MessageBox.Show(fe.Message, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("There was a problem saving the disbursement transaction: " + e.Message, "Transaction Not Saved", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Indicate failure
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


        private async void badd_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do you want to import the account?", "Save Transaction", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bool isSaveSuccessful = false;
                bool isDisburseSuccessful = false;

                // Run the data-saving tasks asynchronously
                await Task.Run(async () =>
                {
                    isSaveSuccessful = await GenerelSaveData();
                    if (isSaveSuccessful)
                    {
                        isDisburseSuccessful = await GenerelDisbursedData();
                    }
                });

                // Show success message only if both operations succeed
                if (isSaveSuccessful && isDisburseSuccessful)
                {
                    MessageBox.Show(this, "Transactions have been imported successfully!", "Transaction Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
        }




        private async void bdelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do you want to delete the account?", "Save Transaction", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Show a loading indicator if needed (or just use a MessageBox to indicate progress)
                load.Show(this);

                // Run the delete operation asynchronously
                await Task.Run(() => DeleteLoanDataAsync());

                load.Close();

                MessageBox.Show(this, "Account has been deleted successfully!", "Transaction Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }
    }
}

using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rct_lmis.LOAN_SECTION
{
    public partial class frm_home_loand_req_details : Form
    {
        private string accountId;
        private string loggedInUsername;
        LoadingFunction load = new LoadingFunction();

        public frm_home_loand_req_details(string accountId)
        {
            InitializeComponent();
            this.accountId = accountId;
            loggedInUsername = UserSession.Instance.CurrentUser;
            LoadLoanDetails();
        }

        private void LoadLoanDetails()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var loanAppCollection = database.GetCollection<BsonDocument>("loan_application");
                var filter = Builders<BsonDocument>.Filter.Eq("AccountId", accountId);
                var document = loanAppCollection.Find(filter).FirstOrDefault();

                if (document != null)
                {
                    tlaccountno.Text = document.Contains("AccountId") ? document["AccountId"].ToString() : "N/A";
                    tloanstatus.Text = document.Contains("Status") ? document["Status"].ToString() : "Unknown Status";
                    taccountstatus.Text = document.Contains("LoanStatus") ? document["LoanStatus"].ToString() : "Unknown Status";

                    // Hide buttons based on Loan Status
                    if (taccountstatus.Text == "LOAN APPROVED")
                    {
                        bapproved.Visible = false;
                        bdisburse.Visible = true;
                    }
                    else
                    {
                        bdisburse.Visible = true;
                        bdisburse.Visible = false;
                    }

                    if (taccountstatus.Text == "LOAN DENIED")
                    {
                        bdisburse.Visible = false;
                        bapproved.Visible = true;
                    }
                    else
                    {
                        bdisburse.Visible = true;
                        bapproved.Visible = false;
                    }

                    // ✅ Properly handling ApplicationDate
                    if (document.Contains("ApplicationDate") && document["ApplicationDate"].IsBsonDateTime)
                    {
                        var appDate = document["ApplicationDate"].ToUniversalTime();
                        tloanappdate.Text = appDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        tloanappdate.Text = "N/A";
                    }

                    tfname.Text = document.Contains("ClientName") ? document["ClientName"].ToString() : "N/A";
                    taddress.Text = document.Contains("Address") ? document["Address"].ToString() : "N/A";

                    // Handle CI Checkbox
                    cbCInvest.Checked = document.Contains("CI") && document["CI"].ToString() == "✔️";

                    // ✅ Properly handling CIDate
                    if (document.Contains("CIDate") && document["CIDate"].IsBsonDateTime)
                    {
                        var ciDate = document["CIDate"].ToUniversalTime();
                        tcidate.Text = ciDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        tcidate.Text = "N/A";
                    }

                    // Convert Loan Amount to PHP Currency
                    if (document.Contains("LoanAmount") && decimal.TryParse(document["LoanAmount"].ToString(), out decimal loanAmt))
                    {
                        tloanamt.Text = loanAmt.ToString("C", new System.Globalization.CultureInfo("en-PH")); // PHP currency format
                    }
                    else
                    {
                        tloanamt.Text = "₱0.00";
                    }

                    tcollector.Text = document.Contains("CollectionInCharge") ? document["CollectionInCharge"].ToString() : "N/A";
                    tloandesc.Text = document.Contains("LoanDescription") ? document["LoanDescription"].ToString() : "N/A";

                    // ✅ Handle Remarks (Findings)
                    if (document.Contains("Remarks") && !string.IsNullOrWhiteSpace(document["Remarks"].ToString()))
                    {
                        lfindings.Visible = true;
                        tfindings.Text = document["Remarks"].ToString();
                        tfindings.Visible = true; // Show the field if there are remarks
                    }
                    else
                    {
                        lfindings.Visible = false;
                        tfindings.Text = string.Empty;
                        tfindings.Visible = false; // Hide the field if no remarks exist
                    }

                    LoadUploadedDocuments(document); // Assuming this method handles file processing
                }
                else
                {
                    MessageBox.Show("Loan details not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading loan details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUploadedDocuments(BsonDocument document)
        {
            dgvuploads.Columns.Clear();
            dgvuploads.Columns.Add("DocumentName", "Document Name");
            dgvuploads.Columns.Add("DocumentLink", "Document Link");

            DataGridViewLinkColumn viewFileColumn = new DataGridViewLinkColumn
            {
                Name = "ViewFile",
                HeaderText = "View File",
                Text = "View File",
                UseColumnTextForLinkValue = true
            };
            dgvuploads.Columns.Add(viewFileColumn);

            dgvuploads.Rows.Clear();

            if (document.Contains("UploadedDocs") && document["UploadedDocs"].IsBsonArray)
            {
                var uploadedDocs = document["UploadedDocs"].AsBsonArray;
                foreach (var doc in uploadedDocs)
                {
                    if (doc.IsBsonDocument)
                    {
                        var docBson = doc.AsBsonDocument;
                        string fileName = docBson.Contains("file_name") ? docBson["file_name"].ToString() : "Unknown File";
                        string fileLink = docBson.Contains("file_link") ? docBson["file_link"].ToString() : "No Link Available";
                        dgvuploads.Rows.Add(fileName, fileLink, "View File");
                    }
                }
            }

            // Enable text wrapping in Document Link column
            dgvuploads.Columns["DocumentLink"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvuploads.Columns["DocumentName"].Width = 200; // Set width for Document Link column
            dgvuploads.Columns["DocumentLink"].Width = 400; // Set width for Document Link column
            dgvuploads.Columns["ViewFile"].Width = 100; // Set width for View File column

            dgvuploads.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }



        private void frm_home_loand_req_details_Load(object sender, EventArgs e)
        {
            LoadUserInfo(loggedInUsername);
        }

        private void LoadUserInfo(string username)
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<BsonDocument>("user_accounts"); // 'user_accounts' is the name of your collection

            var filter = Builders<BsonDocument>.Filter.Eq("Username", username);
            var user = collection.Find(filter).FirstOrDefault();

            if (user != null)
            {
                // Get the full name
                var fullName = user.GetValue("FullName").AsString;

                // Display the full name
                luser.Text = fullName;
            }
        }


        private void dgvuploads_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvuploads.Columns["ViewFile"].Index && e.RowIndex >= 0)
            {
                string fileLink = dgvuploads.Rows[e.RowIndex].Cells["DocumentLink"].Value?.ToString();

                if (string.IsNullOrEmpty(fileLink))
                {
                    MessageBox.Show("File link not available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    Process.Start(new ProcessStartInfo { FileName = fileLink, UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to open file link.\nError: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void bapproved_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to approve the pending application?", "Approve pending account", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    load.Show(this);
                    await Task.Delay(2000); // Prevents UI freezing
                    load.Close();

                    var database = MongoDBConnection.Instance.Database;
                    var loanAppCollection = database.GetCollection<BsonDocument>("loan_application");

                    string accountId = tlaccountno.Text.Trim(); // Ensure no leading/trailing spaces
                    string approvedBy = luser.Text; // Assuming this holds the logged-in user

                    if (string.IsNullOrEmpty(accountId))
                    {
                        MessageBox.Show("No loan application selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var filter = Builders<BsonDocument>.Filter.Eq("AccountId", accountId);
                    var update = Builders<BsonDocument>.Update
                        .Set("LoanStatus", "LOAN APPROVED")
                        .Set("ApprovedBy", approvedBy)
                        .Set("ApprovalDate", BsonDateTime.Create(DateTime.UtcNow)); // Store as UTC timestamp

                    var result = loanAppCollection.UpdateOne(filter, update);

                    if (result.ModifiedCount > 0)
                    {
                        MessageBox.Show(this, "Loan application approved and details updated successfully.",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Update UI elements safely
                        Invoke((MethodInvoker)delegate
                        {
                            bapproved.Visible = false;
                            bdisburse.Visible = true;
                        });

                        LoadLoanDetails();

                        // Refresh DataGridView in frm_home_loan_request
                        foreach (Form form in System.Windows.Forms.Application.OpenForms)
                        {
                            if (form is frm_home_loan_request homeLoanForm)
                            {
                                homeLoanForm.LoadLoanApplicationsData();
                                System.Windows.Forms.Application.DoEvents(); // Ensures UI updates properly
                                break;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "No matching record found or no changes made.",
                            "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        Invoke((MethodInvoker)delegate
                        {
                            bdisburse.Visible = false;
                            bapproved.Visible = true;
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating loan details: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private async void bdeny_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to disapproved the pending application?", "Disapprove pending account", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    load.Show(this);
                    await Task.Delay(2000);
                    load.Close();

                    var database = MongoDBConnection.Instance.Database;
                    var loanAppCollection = database.GetCollection<BsonDocument>("loan_application");
                    var loanDeniedCollection = database.GetCollection<BsonDocument>("loan_denied");

                    string accountId = tlaccountno.Text.Trim(); // Assuming this holds the Account ID
                    string deniedBy = luser.Text; // Assuming this holds the name of the logged-in user

                    if (string.IsNullOrEmpty(accountId))
                    {
                        MessageBox.Show("No loan application selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Find the document by AccountId
                    var filter = Builders<BsonDocument>.Filter.Eq("AccountId", accountId);
                    var loanDocument = loanAppCollection.Find(filter).FirstOrDefault();

                    if (loanDocument != null)
                    {
                        // Update LoanStatus to "LOAN DENIED"
                        var update = Builders<BsonDocument>.Update.Set("LoanStatus", "LOAN DENIED");
                        loanAppCollection.UpdateOne(filter, update);

                        // Add DeniedDate and DeniedBy to the document before inserting it into loan_denied
                        loanDocument.Set("LoanStatus", "LOAN DENIED");
                        loanDocument.Set("DeniedDate", DateTime.UtcNow); // Store in UTC for consistency
                        loanDocument.Set("DeniedBy", deniedBy);

                        // Move document to loan_denied collection
                        loanDeniedCollection.InsertOne(loanDocument);

                        // Delete from loan_application collection
                        //loanAppCollection.DeleteOne(filter); // Make sure this is uncommented if you want removal

                        // Refresh DataGridView in frm_home_loan_request
                        foreach (Form form in System.Windows.Forms.Application.OpenForms)
                        {
                            if (form is frm_home_loan_request homeLoanForm)
                            {
                                homeLoanForm.LoadLoanApplicationsData();
                                System.Windows.Forms.Application.DoEvents(); // Force UI update
                                break;
                            }
                        }

                        MessageBox.Show(this, "Loan application denied. \n The data has been stored in the Denied List.",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Loan application not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error denying loan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } 
        }

        private void bdisburse_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to proceed to disbursement process for this account?",
                                "Disburse Approved Account",
                                MessageBoxButtons.YesNoCancel,
                                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    var database = MongoDBConnection.Instance.Database;
                    var loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");
                    var loanAppCollection = database.GetCollection<BsonDocument>("loan_application");

                    // ✅ Find the highest ClientNo
                    var lastClient = loanApprovedCollection.Find(new BsonDocument()).ToList();
                    int highestClientNumber = 978; // Default starting number

                    foreach (var doc in lastClient)
                    {
                        if (doc.Contains("ClientNo"))
                        {
                            string clientNo = doc["ClientNo"].AsString;
                            var match = System.Text.RegularExpressions.Regex.Match(clientNo, @"RCT-2024-CL(\d+)");

                            if (match.Success && int.TryParse(match.Groups[1].Value, out int clientNumber))
                            {
                                if (clientNumber > highestClientNumber)
                                {
                                    highestClientNumber = clientNumber;
                                }
                            }
                        }
                    }

                    // ✅ Increment ClientNo
                    int newClientNumber = highestClientNumber + 1;
                    string newClientNo = $"RCT-2024-CL{newClientNumber}";

                    // ✅ Find the highest LoanNo
                    int highestLoanNumber = 9992; // Default starting number

                    foreach (var doc in lastClient) // Reusing the fetched data
                    {
                        if (doc.Contains("LoanNo"))
                        {
                            string loanNo = doc["LoanNo"].AsString;
                            var match = System.Text.RegularExpressions.Regex.Match(loanNo, @"RCT-2024-(\d+)");

                            if (match.Success && int.TryParse(match.Groups[1].Value, out int loanNumber))
                            {
                                if (loanNumber > highestLoanNumber)
                                {
                                    highestLoanNumber = loanNumber;
                                }
                            }
                        }
                    }

                    // ✅ Increment LoanNo
                    int newLoanNumber = highestLoanNumber + 1;
                    string newLoanNo = $"RCT-2024-{newLoanNumber}";

                    // ✅ Find the highest AccountId
                    int highestAccountNumber = 978; // Default starting number

                    foreach (var doc in lastClient) // Reusing the fetched data
                    {
                        if (doc.Contains("AccountId"))
                        {
                            string accountId = doc["AccountId"].AsString;
                            var match = System.Text.RegularExpressions.Regex.Match(accountId, @"RCT-2024DB-(\d+)");

                            if (match.Success && int.TryParse(match.Groups[1].Value, out int accountNumber))
                            {
                                if (accountNumber > highestAccountNumber)
                                {
                                    highestAccountNumber = accountNumber;
                                }
                            }
                        }
                    }

                    // ✅ Increment AccountId
                    int newAccountNumber = highestAccountNumber + 1;
                    string newAccountId = $"RCT-2024DB-{newAccountNumber}";

                    // ✅ Fetch Loan Application Data
                    var filter = Builders<BsonDocument>.Filter.Eq("AccountId", tlaccountno.Text);
                    var document = loanAppCollection.Find(filter).FirstOrDefault();

                    if (document == null)
                    {
                        MessageBox.Show("Loan application not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // ✅ Proper Name Parsing
                    string clientName = document.Contains("ClientName") ? document["ClientName"].AsString : "";
                    string[] nameParts = clientName.Split(' ');

                    string firstName = "";
                    string lastName = "";
                    string middleName = "";
                    string suffix = "";

                    if (nameParts.Length == 2)
                    {
                        firstName = nameParts[0];
                        lastName = nameParts[1];
                    }
                    else if (nameParts.Length == 3)
                    {
                        firstName = nameParts[0];

                        if (nameParts[2].ToUpper() == "JR" || nameParts[2].ToUpper() == "SR")
                        {
                            lastName = nameParts[1];
                            suffix = nameParts[2];
                        }
                        else
                        {
                            middleName = nameParts[1];
                            lastName = nameParts[2];
                        }
                    }
                    else if (nameParts.Length == 4)
                    {
                        firstName = nameParts[0];
                        middleName = nameParts[1];
                        lastName = nameParts[2];
                        suffix = nameParts[3];
                    }

                    // ✅ Prepare new document for `loan_approved`
                    var newLoanApproved = new BsonDocument
                      {
                          { "AccountId", newAccountId },
                          { "LoanNo", newLoanNo },
                          { "ClientNo", newClientNo },
                          { "LoanType", "New" },
                          { "LoanStatus", "UPDATED" },
                          { "FirstName", firstName },
                          { "MiddleName", middleName },
                          { "LastName", lastName },
                          { "Suffix", suffix },
                          { "CollectorName", document.Contains("CollectionInCharge") ? document["CollectionInCharge"].AsString : "" },
                          { "CompleteAddress", document.Contains("Address") ? document["Address"].AsString : "" },
                          { "Barangay", "" },
                          { "City", document.Contains("Address") ? document["Address"].AsString : "" },
                          { "Province", "" },
                          { "LoanTerm", "" },
                          { "LoanAmount", document.Contains("LoanAmount") ? document["LoanAmount"].ToString() : "0" },
                          { "LoanAmortization", "" },
                          { "LoanBalance", "" },
                          { "Penalty", "" },
                          { "LoanInterest", "" },
                          { "PaymentMode", "" },
                          { "StartPaymentDate", "" },
                          { "MaturityDate", "" },
                          { "Date_Encoded", DateTime.UtcNow.ToString("MM/dd/yyyy") },
                          { "LoanProcessStatus", "Loan Disbursed" },
                          { "AlternateContactNumber", "" },
                          { "BirthAddress", "" },
                          { "CivilStatus", "" },
                          { "CompanyName", "" },
                          { "ContactNumber", "" },
                          { "DateOfBirth", "" },
                          { "Email", "" },
                          { "Gender", "" },
                          { "MonthlyIncome", "" },
                          { "NumberOfChildren", "" },
                          { "Occupation", "" },
                          { "SpouseFirstName", "" },
                          { "SpouseLastName", "" },
                          { "SpouseMiddleName", "" },
                          { "WorkAddress", "" },
                          { "Date_Modified", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss") },
                          { "Date_Disbursed", DateTime.UtcNow.ToString("MM/dd/yyyy") },
                          { "Disbursed_by", luser.Text },
                          { "ApplicationDate", document.Contains("ApplicationDate") && document["ApplicationDate"].IsBsonDateTime
                              ? document["ApplicationDate"].ToUniversalTime().ToString("MM/dd/yyyy")
                              : "N/A" },
                          { "CIDate", document.Contains("CIDate") && document["CIDate"].IsBsonDateTime
                              ? document["CIDate"].ToUniversalTime().ToString("MM/dd/yyyy")
                              : "N/A" },
                          { "CI", document.Contains("CI") ? document["CI"].ToString() : "" },
                          { "LoanDescription", document.Contains("LoanDescription") ? document["LoanDescription"].AsString : "" },
                          { "Remarks", document.Contains("Remarks") ? document["Remarks"].AsString : "" }
                      };

                    // ✅ Insert into `loan_approved`
                    loanApprovedCollection.InsertOne(newLoanApproved);

                    MessageBox.Show("Loan successfully approved for disbursement.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // ✅ Open Disbursement Form
                    frm_home_loan_disburse fdis = new frm_home_loan_disburse(newAccountId);
                    fdis.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error processing disbursement: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

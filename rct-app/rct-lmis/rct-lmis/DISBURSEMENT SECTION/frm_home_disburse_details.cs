using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rct_lmis.DISBURSEMENT_SECTION
{
    public partial class frm_home_disburse_details : Form
    {
        private string _loanId;
        private string clientNo;
        private IMongoCollection<BsonDocument> _loanDisbursedCollection;
        private IMongoCollection<BsonDocument> _loanApprovedCollection;
        private IMongoCollection<BsonDocument> _loanReleaseVoucherCollection;

        private static string[] Scopes = { DriveService.Scope.DriveFile };
        private static string ApplicationName = "rct-lmis";
        private DriveService service;


        // Folder IDs
        private static string DocsFolderId = "1-4E3R3L-lNcJ0E98RrcWRRb9As_8kDl3";
        private static string ImagesFolderId = "1e8QCXl7eYLHjNORQjw3pz1U6AASOuZjE";
        private List<string> filePaths = new List<string>();

        public frm_home_disburse_details(string loanId, string clientNo)
        {
            InitializeComponent();
            _loanId = loanId;
            this.clientNo = clientNo;
            // Initialize the MongoDB collections
            var database = MongoDBConnection.Instance.Database;
            _loanDisbursedCollection = database.GetCollection<BsonDocument>("loan_disbursed");
            _loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");
            _loanReleaseVoucherCollection = database.GetCollection<BsonDocument>("loan_released_voucher");

            // Load details
            LoadDetails();
   
            //for uploading
            InitializeDataGridView();
            InitializeMongoDBUpload();
            InitializeGoogleDrive();
        }

        private async void LoadDetails()
        {
            try
            {
                // Display Loan ID No on the label
                laccountid.Text = _loanId;

                // Fetch loan details based on Loan ID
                LoanDetails loanDetails = await GetLoanDetails(laccountid.Text);

                // Populate the textboxes with the loan details
                if (loanDetails != null)
                {
                    tlnno.Text = !string.IsNullOrEmpty(loanDetails.LoanIDNo) ? loanDetails.LoanIDNo : "n/a";
                    tclientno.Text = !string.IsNullOrEmpty(loanDetails.cashClnNo) ? loanDetails.cashClnNo : "n/a";
                    tname.Text = !string.IsNullOrEmpty(loanDetails.cashName) ? loanDetails.cashName : "n/a";

                    // Populate additional details
                    tloantype.Text = !string.IsNullOrEmpty(loanDetails.LoanType) ? loanDetails.LoanType : "n/a";
                    tloanstatus.Text = !string.IsNullOrEmpty(loanDetails.LoanStatus) ? loanDetails.LoanStatus : "n/a";
                    tloanamount.Text = !string.IsNullOrEmpty(loanDetails.LoanAmount) ? loanDetails.LoanAmount : "0.00";
                    tloanprincipal.Text = !string.IsNullOrEmpty(loanDetails.LoanPrincipal) ? loanDetails.LoanPrincipal : "0.00";  // Loan Principal added
                    tloanbalance.Text = !string.IsNullOrEmpty(loanDetails.LoanBalance) ? loanDetails.LoanBalance : "0.00";
                    tloanamort.Text = !string.IsNullOrEmpty(loanDetails.LoanAmortization) ? loanDetails.LoanAmortization : "0.00";
                    tloanpenalty.Text = !string.IsNullOrEmpty(loanDetails.Penalty) ? loanDetails.Penalty : "0.00";
                    tloanpaymode.Text = !string.IsNullOrEmpty(loanDetails.PaymentMode) ? loanDetails.PaymentMode : "n/a";
                    tloancollector.Text = !string.IsNullOrEmpty(loanDetails.CollectorName) ? loanDetails.CollectorName : "n/a";

                    // Populate Loan Term
                    tloanterm.Text = !string.IsNullOrEmpty(loanDetails.LoanTerm) ? loanDetails.LoanTerm : "n/a";

                    // Calculate and display the interest (LoanAmount * LoanInterest)
                    decimal loanAmount;
                    decimal loanInterestPercentage;
                    if (decimal.TryParse(loanDetails.LoanAmount.Replace("₱", "").Replace(",", ""), out loanAmount) &&
                        decimal.TryParse(loanDetails.LoanInterest.Replace("%", "").Trim(), out loanInterestPercentage))
                    {
                        decimal loanInterestAmount = loanAmount * (loanInterestPercentage / 100);
                        tloaninterest.Text = loanInterestAmount.ToString("0.00");
                    }
                    else
                    {
                        tloaninterest.Text = "0.00";  // Fallback in case of parsing errors
                    }

                    // Set the StartPaymentDate and MaturityDate fields
                    DateTime startDate;
                    if (DateTime.TryParse(loanDetails.StartPaymentDate, out startDate))
                    {
                        dtstartpay.Value = startDate;
                    }
                    else
                    {
                        dtstartpay.Value = DateTime.Now;  // Default value if parsing fails
                    }

                    DateTime maturityDate;
                    if (DateTime.TryParse(loanDetails.MaturityDate, out maturityDate))
                    {
                        dtendpay.Value = maturityDate;
                    }
                    else
                    {
                        dtendpay.Value = DateTime.Now;  // Default value if parsing fails
                    }

                    // Fetch additional details from loan_approved based on cashClnNo
                    await LoadApprovedDetails(loanDetails.cashClnNo);
                    await LoadVoucherDetailsAsync();
                }
                else
                {
                    MessageBox.Show("No loan details found.");
                }
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show("Error loading loan details: " + ex.Message);
            }
        }


        private async Task<LoanDetails> GetLoanDetails(string loanId)
        {
            // Query the loan_disbursed collection to find the details by AccountId
            var filter = Builders<BsonDocument>.Filter.Eq("AccountId", loanId);
            var loanDisbursed = await _loanDisbursedCollection.Find(filter).FirstOrDefaultAsync();

            if (loanDisbursed != null)
            {
                // Parse the start and maturity dates
                DateTime startDate, maturityDate;
                bool hasValidStartDate = DateTime.TryParse(loanDisbursed.GetValue("StartPaymentDate", "").ToString(), out startDate);
                bool hasValidMaturityDate = DateTime.TryParse(loanDisbursed.GetValue("MaturityDate", "").ToString(), out maturityDate);

                // Calculate the loan term in months if LoanTerm is "0 months"
                string loanTerm = loanDisbursed.GetValue("LoanTerm", "0 months").ToString();
                if (loanTerm == "0 months" && hasValidStartDate && hasValidMaturityDate)
                {
                    int monthsDifference = ((maturityDate.Year - startDate.Year) * 12) + maturityDate.Month - startDate.Month;
                    loanTerm = $"{monthsDifference} months";
                }

                // Map the result to LoanDetails object
                return new LoanDetails
                {
                    LoanIDNo = loanDisbursed.GetValue("LoanNo", "n/a").ToString(),
                    cashClnNo = loanDisbursed.GetValue("ClientNo", "n/a").ToString(),
                    cashName = loanDisbursed.GetValue("FirstName", "").ToString() + " " +
                               loanDisbursed.GetValue("MiddleName", "").ToString() + " " +
                               loanDisbursed.GetValue("LastName", "").ToString(),
                    LoanType = loanDisbursed.GetValue("LoanType", "n/a").ToString(),
                    LoanStatus = loanDisbursed.GetValue("LoanStatus", "n/a").ToString(),
                    LoanAmount = loanDisbursed.GetValue("LoanAmount", "0.00").ToString(),
                    LoanPrincipal = loanDisbursed.GetValue("PrincipalAmount", "0.00").ToString(),  // LoanPrincipal added
                    LoanBalance = loanDisbursed.GetValue("LoanBalance", "0.00").ToString(),
                    LoanAmortization = loanDisbursed.GetValue("LoanAmortization", "0.00").ToString(),
                    Penalty = loanDisbursed.GetValue("Penalty", "0.00").ToString(),
                    LoanInterest = loanDisbursed.GetValue("LoanInterest", "0.00").ToString(),
                    PaymentMode = loanDisbursed.GetValue("PaymentMode", "n/a").ToString(),
                    CollectorName = loanDisbursed.GetValue("CollectorName", "n/a").ToString(),
                    StartPaymentDate = hasValidStartDate ? startDate.ToString("MM/dd/yyyy") : "n/a",
                    MaturityDate = hasValidMaturityDate ? maturityDate.ToString("MM/dd/yyyy") : "n/a",
                    LoanTerm = loanTerm  // Use calculated or existing LoanTerm
                };
            }

            return null;
        }




        // Fetch Approved Details (Address, Docs) from loan_approved
        private async Task LoadApprovedDetails(string cashClnNo)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("ClientNo", tclientno.Text); // Search by ClientNo
            var loanApproved = await _loanApprovedCollection.Find(filter).FirstOrDefaultAsync();

            if (loanApproved != null)
            {
              
                // Display address
                string address = $"{loanApproved["Barangay"]}, {loanApproved["City"]}, {loanApproved["Province"]}";
                tadd.Text = address;

                // Display contact info (if available)
                if (loanApproved.Contains("CP"))
                {
                    tcontact.Text = loanApproved["CP"].ToString();
                }
                else
                {
                    tcontact.Text = "N/A"; // If no contact info is available
                }

                

                // Check if the "docs" field is present and is a string or array
                if (loanApproved.Contains("docs") && loanApproved.Contains("doc-link"))
                {
                    var docs = loanApproved["docs"];
                    var docLinks = loanApproved["doc-link"];

                    string[] docsArray = docs.IsBsonArray ? docs.AsBsonArray.Select(d => d.AsString).ToArray() : docs.AsString.Split(',');
                    string[] docLinksArray = docLinks.IsBsonArray ? docLinks.AsBsonArray.Select(d => d.AsString).ToArray() : docLinks.AsString.Split(',');

                    dgvuploads.Rows.Clear(); // Clear any existing rows

                    if (docsArray.Length > 0 && docLinksArray.Length > 0)
                    {
                        for (int i = 0; i < docsArray.Length; i++)
                        {
                            _ = dgvuploads.Rows.Add(docsArray[i].Trim(), docLinksArray[i].Trim());
                        }

                        // Hide the no record message if data is available
                        lnorecordattachment.Visible = false;
                    }
                    else
                    {
                        // Show the no record message
                        lnorecordattachment.Visible = true;
                        lnorecordattachment.Text = "No documents available.";
                    }
                }
                else
                {
                    // Show the no record message
                    lnorecordattachment.Visible = true;
                    lnorecordattachment.Text = "No documents available.";
                }
            }
            else
            {
                // Show the no record message
                lnorecordattachment.Visible = true;
                lnorecordattachment.Text = "Loan approved details not found.";
            }
        }



        private void InitializeDgvdataColumns()
        {
            dgvdata.Columns.Clear(); // Clear existing columns

            // Add columns for the voucher details
            _ = dgvdata.Columns.Add("ClientName", "Client Name");
            _ = dgvdata.Columns.Add("docs", "Document Name");

            // Create and add the View File button column
            DataGridViewButtonColumn viewFileColumn = new DataGridViewButtonColumn
            {
                Name = "ViewFile",
                Text = "View File",
                UseColumnTextForButtonValue = true,
                HeaderText = "Actions",
                Width = 100 // Width of the View File button column
            };
            _ = dgvdata.Columns.Add(viewFileColumn);

            // Add the hidden doc-link column for internal use
            _ = dgvdata.Columns.Add("doc-link", "Document Link");
            dgvdata.Columns["doc-link"].Visible = false; // Hide the column
        }


        private async Task LoadVoucherDetailsAsync()
        {
            string clientNo = tclientno.Text;

            try
            {
                // Filter by ClientNo
                var filter = Builders<BsonDocument>.Filter.Eq("ClientNo", clientNo);
                var loanReleaseVouchers = await _loanReleaseVoucherCollection.Find(filter).ToListAsync();

                dgvdata.Rows.Clear(); // Clear existing rows

                if (loanReleaseVouchers.Count > 0)
                {
                    foreach (var voucher in loanReleaseVouchers)
                    {
                        // Extract values from the voucher document
                        string clientName = voucher.GetValue("ClientName", "").ToString();
                        string docLink = voucher.GetValue("doc-link", "").ToString();
                        string docs = voucher.GetValue("docs", "").ToString();

                        // Add a new row with the voucher details
                        int rowIndex = dgvdata.Rows.Add(clientName, docs, "View File");

                        // Attach the document link to the View File button's Tag property
                        dgvdata.Rows[rowIndex].Cells["ViewFile"].Tag = docLink;
                    }

                    // Hide the no record label if data is available
                    lnorecordvoucher.Visible = false;
                }
                else
                {
                    // Show the no record label if no data is available
                    lnorecordvoucher.Visible = true;
                    lnorecordvoucher.Text = "No vouchers available.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading voucher details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<bool> UpdateLoanDetailsAsync(string loanID)
        {
            try
            {
                // Check if the loan exists before updating
                var filter = Builders<BsonDocument>.Filter.Eq("LoanNo", tlnno.Text);
                var existingLoan = await _loanDisbursedCollection.Find(filter).FirstOrDefaultAsync();

                if (existingLoan == null)
                {
                    MessageBox.Show("No loan found with the provided Loan ID.");
                    return false;
                }

                // Calculate loan principal, which could be entered or derived.
                decimal loanAmount;
                decimal.TryParse(tloanamount.Text.Replace("₱", "").Replace(",", ""), out loanAmount);

                decimal loanInterestPercentage;
                decimal.TryParse(tloaninterest.Text.Replace("₱", "").Replace(",", ""), out loanInterestPercentage);

                // Assuming Principal is the Loan Amount minus any interest
                decimal loanPrincipal = loanAmount - loanInterestPercentage;

                // Create an update definition with the new values from the textboxes
                var update = Builders<BsonDocument>.Update
                    .Set("LoanType", tloantype.Text)
                    .Set("LoanStatus", tloanstatus.Text)
                    .Set("LoanAmount", tloanamount.Text)
                    .Set("LoanBalance", tloanbalance.Text)
                    .Set("LoanAmortization", tloanamort.Text)
                    .Set("Penalty", tloanpenalty.Text)
                    .Set("LoanInterest", tloaninterest.Text)
                    .Set("PaymentMode", tloanpaymode.Text)
                    .Set("CollectorName", tloancollector.Text)
                    .Set("LoanTerm", tloanterm.Text)
                    //.Set("PrincipalAmount", loanPrincipal.ToString("₱#,##0.00"))
                    .Set("PrincipalAmount", tloanprincipal.Text)  // Updating Loan Principal
                    .Set("StartPaymentDate", dtstartpay.Value.ToString("MM/dd/yyyy"))
                    .Set("MaturityDate", dtendpay.Value.ToString("MM/dd/yyyy"))
                    .Set("LoanProcessStatus", "Loan Updated")
                    .Set("Date_Encoded", DateTime.Now.ToString("MM/dd/yyyy"));

                // Execute the update operation
                var result = await _loanDisbursedCollection.UpdateOneAsync(filter, update);

                // Log the result of the update
                if (result.ModifiedCount > 0)
                {
                    //MessageBox.Show("Loan details updated successfully.");

                    return true;
                }
                else
                {
                    MessageBox.Show("No changes were made. The loan details are the same as before.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating loan details: " + ex.Message);
                return false;
            }
        }





        private void frm_home_disburse_details_Load(object sender, EventArgs e)
        {
            InitializeDgvdataColumns();   
            lnorecordvoucher.Visible = false;

            dgvuploads.Columns.Clear();

            _ = dgvuploads.Columns.Add("docs", "Attachments");

            DataGridViewButtonColumn viewFileColumn = new DataGridViewButtonColumn
            {
                Name = "View File",
                Text = "View File",
                UseColumnTextForButtonValue = true,
                Width = 50,
            };
            _ = dgvuploads.Columns.Add(viewFileColumn);


            var btnColumndetails = dgvuploads.Columns["View File"];
            if (btnColumndetails != null)
            {
                btnColumndetails.Width = 120;
                btnColumndetails.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                btnColumndetails.DefaultCellStyle.Font = new Font("Segoe UI", 8);
            }

            bclear.Enabled = false;
            bsubmit.Enabled = false;
        }

        private void dgvuploads_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvuploads.Columns["View File"].Index && e.RowIndex >= 0)
            {
                // Get the URL from the Tag property of the button
                string url = dgvuploads.Rows[e.RowIndex].Cells["View File"].Tag?.ToString();

                if (!string.IsNullOrEmpty(url))
                {
                    try
                    {
                        // Open the URL in the default browser
                        _ = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        _ = MessageBox.Show($"Error opening file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void dgvuploads_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.ColumnIndex < dgvuploads.Columns.Count &&
               dgvuploads.Columns[e.ColumnIndex].Name == "View File")
            {
                if (dgvuploads.Rows[e.RowIndex].Cells[e.ColumnIndex].Style != null)
                {
                    DataGridViewCellStyle cellStyle = dgvuploads.Rows[e.RowIndex].Cells[e.ColumnIndex].Style;
                    cellStyle.Padding = new Padding(20, 5, 20, 5);
                }
            }
        }

        private void dgvuploads_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvuploads.ClearSelection();
        }

        private void dgvdata_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvdata.Columns["ViewFile"].Index && e.RowIndex >= 0)
            {
                // Get the URL from the Tag property of the button
                string url = dgvdata.Rows[e.RowIndex].Cells["ViewFile"].Tag?.ToString();

                if (!string.IsNullOrEmpty(url))
                {
                    try
                    {
                        // Open the URL in the default browser
                        _ = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        _ = MessageBox.Show($"Error opening file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void dgvdata_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvdata.ClearSelection();
        }




        ///--------------------------------------------------------------------------- UPLOADING ----------------------------------------------------------------------------------------

        private string FormatFileSize(long fileSize)
        {
            if (fileSize >= 1073741824)
                return string.Format("{0:##.##} GB", fileSize / 1073741824.0);
            else if (fileSize >= 1048576)
                return string.Format("{0:##.##} MB", fileSize / 1048576.0);
            else if (fileSize >= 1024)
                return string.Format("{0:##.##} KB", fileSize / 1024.0);
            else
                return string.Format("{0} Bytes", fileSize);
        }

        private void InitializeDataGridView()
        {
            dgvattach.AutoGenerateColumns = false;
            _ = dgvattach.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "FileName",
                HeaderText = "File Name",
                DataPropertyName = "FileName",
                Width = 300
            });
            _ = dgvattach.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Size",
                HeaderText = "Size",
                DataPropertyName = "Size",
                Width = 150
            });
            _ = dgvattach.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Status",
                HeaderText = "Status",
                DataPropertyName = "Status",
                Width = 150
            });
            DataGridViewButtonColumn deleteButtonColumn = new DataGridViewButtonColumn()
            {
                Name = "Delete",
                HeaderText = "Action",
                Text = "Delete",
                UseColumnTextForButtonValue = true, // This will show "Delete" on all buttons
                Width = 100
            };
            _ = dgvattach.Columns.Add(deleteButtonColumn);
        }

        private void InitializeMongoDBUpload()
        {
            var database = MongoDBConnection.Instance.Database;
            _ = database.GetCollection<Application>("loan_application");
        }

        private async void InitializeGoogleDrive()
        {
            try
            {
                UserCredential credential;
                using (var stream = new FileStream("google_drive_credentials.json", FileMode.Open, FileAccess.Read))
                {
                    var clientSecrets = GoogleClientSecrets.Load(stream).Secrets;
                    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                        clientSecrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore("token.json", true));
                }

                // Create Drive API service
                service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Error initializing Google Drive service: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<string> UploadFileToDrive(string filePath, string destinationFolderId)
        {
            try
            {
                var fileMetadata = new Google.Apis.Drive.v3.Data.File
                {
                    Name = Path.GetFileName(filePath),
                    Parents = new List<string> { destinationFolderId }
                };

                FilesResource.CreateMediaUpload request;
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    request = service.Files.Create(fileMetadata, stream, GetMimeType(filePath));
                    request.Fields = "id";
                    _ = await request.UploadAsync();
                }

                var file = request.ResponseBody;
                return file.Id;
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"An error occurred while uploading the file to Google Drive: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void baddfile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileName in openFileDialog.FileNames)
                {
                    filePaths.Add(fileName);

                    FileInfo fileInfo = new FileInfo(fileName);
                    long fileSize = fileInfo.Length; // File size in bytes

                    string fileSizeString = FormatFileSize(fileSize); // Convert to readable format

                    _ = dgvattach.Rows.Add(Path.GetFileName(fileName), fileSizeString, "Ready to Upload", "Action");
                    lnofile.Visible = false;
                    lfilesready.Visible = true;

                    bclear.Enabled = true;
                    bsubmit.Enabled = true;
                }
            }
        }

        private void bclear_Click(object sender, EventArgs e)
        {
            dgvattach.Rows.Clear();
            filePaths.Clear();
            lfilesready.Visible = false;
            lnofile.Visible = true;
            bsubmit.Enabled = false;
            bclear.Enabled = false;
        }

        private async void bsubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (filePaths.Count == 0)
                {
                    _ = MessageBox.Show("Please select files to upload.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                mainProgressBar.Maximum = filePaths.Count;
                mainProgressBar.Value = 0;

                statusLabel.Text = "Uploading files...";
                statusLabel.ForeColor = Color.Black;

                // Collections to hold file details
                List<string> fileNames = new List<string>();
                List<string> fileLinks = new List<string>();

                for (int i = 0; i < filePaths.Count; i++)
                {
                    string filePath = filePaths[i];

                    if (string.IsNullOrEmpty(filePath))
                    {
                        _ = MessageBox.Show("Please fill in all fields and select a file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string mimeType = GetMimeType(filePath);
                    string destinationFolderId = GetDestinationFolderId(mimeType);
                    string originalFileName = Path.GetFileName(filePath);

                    // Update status in DataGridView (Invoke required for cross-thread access)
                    _ = dgvattach.Invoke((MethodInvoker)(() =>
                    {
                        if (i < dgvattach.Rows.Count)
                            dgvattach.Rows[i].Cells["Status"].Value = "Uploading";
                    }));

                    // Upload file to Google Drive
                    string fileId = await UploadFileToDrive(filePath, destinationFolderId);

                    // Update status in DataGridView (Invoke required for cross-thread access)
                    _ = dgvattach.Invoke((MethodInvoker)(() =>
                    {
                        if (i < dgvattach.Rows.Count)
                        {
                            if (fileId != null)
                                dgvattach.Rows[i].Cells["Status"].Value = "Upload Done";
                            else
                                dgvattach.Rows[i].Cells["Status"].Value = "Failed";
                        }
                    }));

                    // Update progress bar (Invoke required for cross-thread access)
                    _ = mainProgressBar.Invoke((MethodInvoker)(() =>
                    {
                        mainProgressBar.Value = i + 1;
                    }));

                    // Save data to MongoDB for successful uploads
                    if (fileId != null)
                    {
                        // Generate Google Drive link
                        string fileLink = $"https://drive.google.com/file/d/{fileId}/view?usp=sharing";

                        // Collect file details
                        fileNames.Add(originalFileName);
                        fileLinks.Add(fileLink);
                    }
                }

                // Combine file details
                string combinedFileNames = string.Join(", ", fileNames);
                string combinedFileLinks = string.Join(", ", fileLinks);

                // MongoDB collections
                var database = MongoDBConnection.Instance.Database;
                var loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");
                var loanReleasedVoucherCollection = database.GetCollection<BsonDocument>("loan_released_voucher");

                // Update loan_released_voucher collection with new data
                var voucherUpdate = Builders<BsonDocument>.Update
                    .Set("ClientNo", tclientno.Text)
                    .Set("ClientName", tname.Text)
                    .Set("docs", combinedFileNames)
                    .Set("doc-link", combinedFileLinks)
                    .Set("DateUploaded", DateTime.UtcNow);


                var voucherFilter = Builders<BsonDocument>.Filter.Eq("AccountId", laccountid.Text);
                _ = await loanReleasedVoucherCollection.UpdateOneAsync(voucherFilter, voucherUpdate, new UpdateOptions { IsUpsert = true });

                // Ensure docs and doc-link are arrays in loan_approved
                var approvedDocument = await loanApprovedCollection.Find(Builders<BsonDocument>.Filter.Eq("ClientNumber", tclientno.Text)).FirstOrDefaultAsync();

                if (approvedDocument != null)
                {
                    // Convert docs and doc-link to arrays if they are strings
                    var docsArray = approvedDocument.Contains("docs")
                        ? (approvedDocument["docs"].IsBsonArray ? approvedDocument["docs"].AsBsonArray : new BsonArray(approvedDocument["docs"].AsString.Split(new[] { ", " }, StringSplitOptions.None)))
                        : new BsonArray();

                    var docLinksArray = approvedDocument.Contains("doc-link")
                        ? (approvedDocument["doc-link"].IsBsonArray ? approvedDocument["doc-link"].AsBsonArray : new BsonArray(approvedDocument["doc-link"].AsString.Split(new[] { ", " }, StringSplitOptions.None)))
                        : new BsonArray();

                    // Add new file details to arrays
                    var updatedDocsArray = docsArray.AddRange(fileNames);
                    var updatedDocLinksArray = docLinksArray.AddRange(fileLinks);

                    var approvedUpdate = Builders<BsonDocument>.Update
                        .Set("docs", updatedDocsArray)
                        .Set("doc-link", updatedDocLinksArray);

                    _ = await loanApprovedCollection.UpdateOneAsync(
                        Builders<BsonDocument>.Filter.Eq("ClientNumber", tclientno.Text),
                        approvedUpdate
                    );
                }
                else
                {
                    _ = MessageBox.Show("Loan approved details not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Clear form fields after successful upload
                filePaths.Clear();
                _ = dgvattach.Invoke((MethodInvoker)(() => dgvattach.Rows.Clear()));

                // Update status label
                statusLabel.Text = "Upload completed!";
                statusLabel.ForeColor = Color.Green;

                _ = MessageBox.Show(this, "Data and files uploaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Error uploading files and saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
            {
                mimeType = regKey.GetValue("Content Type").ToString();
            }
            return mimeType;
        }

        private string GetDestinationFolderId(string mimeType)
        {
            if (mimeType.StartsWith("image/"))
            {
                return ImagesFolderId;
            }
            else
            {
                return DocsFolderId;
            }
        }

        private void dgvattach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the click is on the delete button column
            if (e.ColumnIndex == dgvattach.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                // Ensure there are rows in the DataGridView
                if (dgvattach.Rows.Count > 0 && e.RowIndex < dgvattach.Rows.Count)
                {
                    // Show confirmation dialog
                    var confirmResult = MessageBox.Show("Are you sure to delete this file?",
                                                        "Confirm Delete",
                                                        MessageBoxButtons.YesNo);
                    if (confirmResult == DialogResult.Yes)
                    {
                        // Ensure the index is valid before removing from filePaths
                        if (e.RowIndex >= 0 && e.RowIndex < filePaths.Count)
                        {
                            // Remove the corresponding file path from the list
                            filePaths.RemoveAt(e.RowIndex);
                        }

                        // Remove the row from the DataGridView
                        dgvattach.Rows.RemoveAt(e.RowIndex);

                        // Optionally, check if there are no more files and update the lnofile label visibility
                        if (filePaths.Count == 0)
                        {
                            lnofile.Visible = true;
                            lfilesready.Visible = false;
                            bsubmit.Enabled = false;
                            bclear.Enabled = false;
                        }
                    }
                }
            }
        }

        private async void bsaveloan_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate if necessary fields are filled
                if (string.IsNullOrEmpty(tlnno.Text) || string.IsNullOrEmpty(tloanamount.Text))
                {
                    MessageBox.Show("Loan Number and Loan Amount are required fields.");
                    return;
                }

                // Call the update function asynchronously
                bool isUpdated = await UpdateLoanDetailsAsync(tlnno.Text);

                // Notify the user of the result
                if (isUpdated)
                {
                    MessageBox.Show("Loan details updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDetails();  // Ensure LoadDetails() doesn't block the UI
                }
                else
                {
                    MessageBox.Show("No changes were made to the loan details.", "No Changes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                
                // Notify user about the error
                MessageBox.Show("Error saving loan details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void bsavegeninfo_Click(object sender, EventArgs e)
        {

        }
    }

    // Define the LoanDetails class to hold the loan information
    public class LoanDetails
    {
        public string LoanIDNo { get; set; }
        public string cashClnNo { get; set; }
        public string cashName { get; set; }
        public string LoanType { get; set; }
        public string LoanStatus { get; set; }
        public string LoanAmount { get; set; }
        public string LoanPrincipal { get; set; }
        public string LoanBalance { get; set; }
        public string LoanAmortization { get; set; }
        public string Penalty { get; set; }
        public string LoanInterest { get; set; }
        public string PaymentMode { get; set; }
        public string CollectorName { get; set; }
        public string StartPaymentDate { get; set; }
        public string MaturityDate { get; set; }
        public string LoanTerm { get; set; }  // Add LoanTerm
    }

}

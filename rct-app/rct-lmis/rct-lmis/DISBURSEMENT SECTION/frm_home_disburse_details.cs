﻿using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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

        public frm_home_disburse_details(string loanId)
        {
            InitializeComponent();
            _loanId = loanId;

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
                    tlnno.Text = loanDetails.LoanIDNo;
                    tclientno.Text = loanDetails.cashClnNo;
                    tname.Text = loanDetails.cashName;

                    // Fetch additional details from loan_approved based on cashClnNo
                    await LoadApprovedDetails(loanDetails.cashClnNo);
                    await LoadVoucherDetailsAsync();
                }
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show("Error loading loan details: " + ex.Message);
            }
        }

        private async Task<LoanDetails> GetLoanDetails(string loanId)
        {
            // Query the loan_disbursed collection to find the details by LoanIDNo
            var filter = Builders<BsonDocument>.Filter.Eq("cashNo", loanId);
            var loanDisbursed = await _loanDisbursedCollection.Find(filter).FirstOrDefaultAsync();

            if (loanDisbursed != null)
            {
                // Map the result to LoanDetails object
                return new LoanDetails
                {
                    LoanIDNo = loanDisbursed["LoanIDNo"].AsString,
                    cashClnNo = loanDisbursed["cashClnNo"].AsString,
                    cashName = loanDisbursed["cashName"].AsString,
                };
            }

            return null;
        }

        // Fetch Approved Details (Address, Docs) from loan_approved
        private async Task LoadApprovedDetails(string cashClnNo)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("ClientNumber", tclientno.Text);
            var loanApproved = await _loanApprovedCollection.Find(filter).FirstOrDefaultAsync();

            if (loanApproved != null)
            {
                // Display address
                string address = $"{loanApproved["Barangay"]}, {loanApproved["City"]}, {loanApproved["Province"]}";
                string contact = $"{loanApproved["CP"]}";
                tadd.Text = address;
                tcontact.Text = contact;

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
    }

    // Define the LoanDetails class to hold the loan information
    public class LoanDetails
    {
        public string LoanIDNo { get; set; }
        public string cashClnNo { get; set; }
        public string cashName { get; set; }
        public string Address { get; set; }
        public string CP { get; set; }
        public string Doc { get; set; } // The document link
    }
}
﻿using DnsClient.Protocol;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MongoDB.Bson;
using MongoDB.Driver;
using rct_lmis.ADMIN_SECTION;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using OfficeOpenXml.Style;

namespace rct_lmis.LOAN_SECTION
{
    public partial class frm_home_addnew : Form
    {
      
        private readonly IMongoCollection<BsonDocument> loanApprovedCollection;
        private IMongoDatabase database;
        private string loggedInUsername;

        private frm_home homeForm;

        public frm_home_addnew()
        {
            InitializeComponent();
            loggedInUsername = UserSession.Instance.CurrentUser;
            database = MongoDBConnection.Instance.Database;
            if (database != null)
            {
                loanApprovedCollection = database.GetCollection<BsonDocument>("loan_approved");
            }

            InitializeDataGridView();
            InitializeDataGridViewRenew();
            InitializeMongoDBUpload();
            InitializeGoogleDrive();

            LoadNextAccountId();
            bsubmit.Enabled = false;
            lfilesready.Visible = false;
            bclear.Enabled = false;
        }
        private static string[] Scopes = { DriveService.Scope.DriveFile };
        private static string ApplicationName = "rct-lmis";
        private DriveService service;


        // Folder IDs
        private static string DocsFolderId = "1kMd3QjEw95oJsMSAK9xwEf-I3_MKlMBj";
        private static string ImagesFolderId = "1O_-PLQyRAjUV7iy6d3PN5rLXznOzxean";
        private List<string> filePaths = new List<string>();

        private void InitializeDataGridView()
        {
            dgvuploads.AutoGenerateColumns = false;
            dgvuploads.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "FileName",
                HeaderText = "File Name",
                DataPropertyName = "FileName",
                Width = 300
            });
            dgvuploads.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Size",
                HeaderText = "Size",
                DataPropertyName = "Size",
                Width = 150
            });
            dgvuploads.Columns.Add(new DataGridViewTextBoxColumn()
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
            dgvuploads.Columns.Add(deleteButtonColumn);
        }

        private void InitializeDataGridViewRenew()
        {
            dgvruploads.AutoGenerateColumns = false;
            dgvruploads.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "FileName",
                HeaderText = "File Name",
                DataPropertyName = "FileName",
                Width = 300
            });
            dgvruploads.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Size",
                HeaderText = "Size",
                DataPropertyName = "Size",
                Width = 150
            });
            dgvruploads.Columns.Add(new DataGridViewTextBoxColumn()
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
            dgvruploads.Columns.Add(deleteButtonColumn);
        }

        private void InitializeMongoDBUpload()
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<Application>("loan_application");
        }

        private void InitializeGoogleDrive()
        {
            try
            {
                GoogleCredential credential;
                using (var stream = new FileStream("rct-credentials.json", FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream)
                        .CreateScoped(DriveService.ScopeConstants.Drive);
                }

                // Create Drive API service
                service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Your Application Name",
                });

                //MessageBox.Show("Google Drive Initialized Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing Google Drive service: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    await request.UploadAsync();
                }

                var file = request.ResponseBody;
                return file.Id;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while uploading the file to Google Drive: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
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

        private void LoadNextAccountId()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var loanApplicationCollection = database.GetCollection<BsonDocument>("loan_application");

                var allAccountIds = loanApplicationCollection.Find(new BsonDocument())
                    .Project(Builders<BsonDocument>.Projection.Include("AccountId"))
                    .ToList();

                int maxNumber = 0;
                string prefix = "";

                foreach (var doc in allAccountIds)
                {
                    var accountId = doc.GetValue("AccountId", "").AsString.Trim();

                    // Match format like RCT-2025DB-048 or RCT-2025DB-034-R1
                    var match = Regex.Match(accountId, @"^(RCT-\d{4}DB-)(\d{3,})(?:-R\d+)?$");
                    if (match.Success)
                    {
                        prefix = match.Groups[1].Value; // e.g., RCT-2025DB-
                        if (int.TryParse(match.Groups[2].Value, out int num) && num > maxNumber)
                        {
                            maxNumber = num;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(prefix))
                {
                    int nextNumber = maxNumber + 1;
                    lloanno.Text = $"{prefix}{nextNumber:D3}";
                }
                else
                {
                    lloanno.Text = "RCT-2025DB-001"; // fallback if nothing matches
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading next account ID: " + ex.Message);
                MessageBox.Show("Error loading next account ID. Please check the console for details.");
            }
        }




        private void LoadClientNames()
        {
            try
            {
                var clientDocuments = loanApprovedCollection.Find(new BsonDocument())
                    .Project(Builders<BsonDocument>.Projection
                        .Include("FirstName")
                        .Include("MiddleName")
                        .Include("LastName")
                        .Include("LoanStatus")
                        .Include("PaymentMode")
                        .Include("LoanBalance"))
                    .ToList();

                AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();

                foreach (var client in clientDocuments)
                {
                    string firstName = client.Contains("FirstName") ? client["FirstName"].AsString.Trim() : "";
                    string middleName = client.Contains("MiddleName") ? client["MiddleName"].AsString.Trim() : "";
                    string lastName = client.Contains("LastName") ? client["LastName"].AsString.Trim() : "";

                    // Ensure no extra spaces when MiddleName is missing
                    string fullName = string.IsNullOrWhiteSpace(middleName)
                        ? $"{firstName} {lastName}"  // No middle name case
                        : $"{firstName} {middleName} {lastName}"; // With middle name

                    autoCompleteCollection.Add(fullName);
                }

                trclientname.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                trclientname.AutoCompleteSource = AutoCompleteSource.CustomSource;
                trclientname.AutoCompleteCustomSource = autoCompleteCollection;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading client names: " + ex.Message);
            }
        }

        private void LoadLoanDetails(string fullName)
        {
            try
            {
                // Ensure fullName matches "Last, First Middle" format
                string[] nameParts = fullName.Split(',');
                if (nameParts.Length < 2) return;  // Invalid format, exit

                string lastName = nameParts[0].Trim();
                string[] firstMiddle = nameParts[1].Trim().Split(' ');

                string firstName = firstMiddle[0]; // First Name
                string middleName = firstMiddle.Length > 1 ? firstMiddle[1] : ""; // Middle Name (optional)

                // Create MongoDB filter
                var filter = Builders<BsonDocument>.Filter.And(
                    Builders<BsonDocument>.Filter.Eq("FirstName", firstName),
                    Builders<BsonDocument>.Filter.Eq("LastName", lastName)
                );

                var clientRecord = loanApprovedCollection.Find(filter).FirstOrDefault();

                if (clientRecord != null)
                {
                    //Populate Fields
                    trpaymentmode.Text = clientRecord.Contains("PaymentMode") ? clientRecord["PaymentMode"].AsString : "";
                    trloanbal.Text = clientRecord.Contains("LoanBalance") ? clientRecord["LoanBalance"].AsString : "";
                    trprevloan.Text = clientRecord.Contains("LoanAmount") ? clientRecord["LoanAmount"].AsString : "";

                    // Match LoanStatus in ComboBox
                    string loanStatus = clientRecord.Contains("LoanStatus") ? clientRecord["LoanStatus"].AsString : "";
                    int index = cbrstatus.Items.IndexOf(loanStatus);
                    cbrstatus.SelectedIndex = index >= 0 ? index : -1; // Set only if found
                }
                else
                {
                    MessageBox.Show("Client data not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading loan details: " + ex.Message);
            }
        }

        private void LoadClientNamesNew()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var loanAppCollection = database.GetCollection<BsonDocument>("loan_application");

                // Fetch client names
                var clientDocuments = loanAppCollection.Find(new BsonDocument()).ToList();
                AutoCompleteStringCollection clientNames = new AutoCompleteStringCollection();

                foreach (var doc in clientDocuments)
                {
                    string firstName = doc.Contains("FirstName") ? doc["FirstName"].AsString : "";
                    string middleName = doc.Contains("MiddleName") ? doc["MiddleName"].AsString : "";
                    string lastName = doc.Contains("LastName") ? doc["LastName"].AsString : "";

                    // Format: "Last, First Middle" (e.g., BALDERAMA, VERNA SISON)
                    string fullName = $"{lastName}, {firstName} {middleName}".Trim();
                    clientNames.Add(fullName);
                }

                // Apply autocomplete settings to the textbox
                tnclientname.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                tnclientname.AutoCompleteSource = AutoCompleteSource.CustomSource;
                tnclientname.AutoCompleteCustomSource = clientNames;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading client names: " + ex.Message);
            }
        }

        private void GenerateRenewalLoanID()
        {
            try
            {
                // Ensure input is not empty
                if (string.IsNullOrWhiteSpace(trclientname.Text)) return;

                // Split client name correctly
                string[] nameParts = trclientname.Text.Trim().Split(' ');
                if (nameParts.Length < 2) return;

                string firstName = nameParts[0];
                string lastName = nameParts[nameParts.Length - 1]; // Last part is assumed to be LastName

                var filter = Builders<BsonDocument>.Filter.And(
                    Builders<BsonDocument>.Filter.Regex("FirstName", new BsonRegularExpression($"^{firstName}", "i")),
                    Builders<BsonDocument>.Filter.Regex("LastName", new BsonRegularExpression($"^{lastName}", "i"))
                );

                var clientRecord = loanApprovedCollection.Find(filter).FirstOrDefault();

                if (clientRecord != null)
                {
                    string accountId = clientRecord.Contains("AccountId") ? clientRecord["AccountId"].AsString : "";
                    if (string.IsNullOrEmpty(accountId)) return;

                    string baseRenewalId = accountId + "-R"; // e.g., "RCT-2024DB-001-R"

                    // Find latest renewal that starts with the baseRenewalId
                    var renewalFilter = Builders<BsonDocument>.Filter.Regex("LoanNo", new BsonRegularExpression("^" + baseRenewalId));
                    var latestRenewal = loanApprovedCollection
                        .Find(renewalFilter)
                        .Sort(Builders<BsonDocument>.Sort.Descending("LoanNo"))
                        .FirstOrDefault();

                    int nextNumber = 1; // Default renewal number if no previous record exists

                    if (latestRenewal != null && latestRenewal.Contains("LoanNo"))
                    {
                        string latestLoanNo = latestRenewal["LoanNo"].AsString;
                        Match match = Regex.Match(latestLoanNo, @"-R(\d+)$");

                        if (match.Success && int.TryParse(match.Groups[1].Value, out int lastNumber))
                        {
                            nextNumber = lastNumber + 1;
                        }
                    }

                    string newRenewalId = $"{baseRenewalId}{nextNumber}"; // e.g., "RCT-2024DB-001-R1"
                    lloannorenew.Text = "Account ID: " + newRenewalId;
                }
                else
                {
                    lloannorenew.Text = "Account ID: Not Found";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating renewal ID: " + ex.Message);
            }
        }

        private void LoadCollectors()
        {
            try
            {
                var loanCollectorsCollection = database.GetCollection<BsonDocument>("loan_collectors");

                var filter = Builders<BsonDocument>.Filter.Empty; // Get all documents
                var collectors = loanCollectorsCollection.Find(filter).ToList();

                cbcollectors.Items.Clear(); // Clear existing items before adding new ones
                cbcollectorincharge.Items.Clear();

                foreach (var collector in collectors)
                {
                    if (collector.Contains("Name"))
                    {
                        string name = collector["Name"].AsString;
                        cbcollectors.Items.Add(name);
                        cbcollectorincharge.Items.Add(name);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading collectors: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUserInfo(string username)
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<BsonDocument>("user_accounts"); // 'user_accounts' is the name of your collection

            var filter = Builders<BsonDocument>.Filter.Eq("Username", username);
            var user = collection.Find(filter).FirstOrDefault();

            if (user != null)
            {
                // Get the full name and split to get the first name
                var fullName = user.GetValue("FullName").AsString;

                // Set the first name
                lveruser.Text = fullName;
            }
        }


        LoadingFunction load = new LoadingFunction();
        private void frm_home_addnew_Load(object sender, EventArgs e)
        {
            LoadClientNames();
            LoadCollectors();
            LoadClientNamesNew();
            LoadUserInfo(loggedInUsername);
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

                    dgvuploads.Rows.Add(Path.GetFileName(fileName), fileSizeString, "Ready to Upload", "Action");
                    lnofile.Visible = false;
                    lfilesready.Visible = true;
                    bclear.Enabled = true;
                    bsubmit.Enabled = true;
                }
            }
        }

        private void bclear_Click(object sender, EventArgs e)
        {
            dgvuploads.Rows.Clear();
            filePaths.Clear();
            lfilesready.Visible = false;
            lnofile.Visible = true;
            bclear.Enabled = false;
            bsubmit.Enabled = false;
        }

        private void dgvuploads_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the click is on the delete button column
            if (e.ColumnIndex == dgvuploads.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                // Ensure there are rows in the DataGridView
                if (dgvuploads.Rows.Count > 0 && e.RowIndex < dgvuploads.Rows.Count)
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
                        dgvuploads.Rows.RemoveAt(e.RowIndex);

                        // Optionally, check if there are no more files and update the lnofile label visibility
                        if (filePaths.Count == 0)
                        {
                            lnofile.Visible = true;
                            lfilesready.Visible = false;
                        }
                    }
                }
            }
        }

        private string LoadCurrentUserFullName()
        {
            string username = loggedInUsername; // Replace with the actual username variable
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<BsonDocument>("user_accounts");

            var filter = Builders<BsonDocument>.Filter.Eq("Username", username);
            var user = collection.Find(filter).FirstOrDefault();

            return user != null ? user.GetValue("FullName").AsString : "Unknown User";
        }

        private async Task<string> GenerateNextAccountId()
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<BsonDocument>("loan_application");

            // Find the latest Account ID
            var sort = Builders<BsonDocument>.Sort.Descending("_id");
            var lastRecord = await collection.Find(new BsonDocument()).Sort(sort).Limit(1).FirstOrDefaultAsync();

            if (lastRecord != null && lastRecord.Contains("AccountId"))
            {
                string lastAccountId = lastRecord["AccountId"].AsString;
                Match match = Regex.Match(lastAccountId, @"RCT-(\d{4})DB-(\d+)");

                if (match.Success)
                {
                    int year = int.Parse(match.Groups[1].Value);
                    int lastNumber = int.Parse(match.Groups[2].Value);
                    int newNumber = lastNumber + 1;

                    return $"Account ID: RCT-{year}DB-{newNumber:D4}";
                }
            }

            // Default starting value if no records exist
            return $"Account ID: RCT-{DateTime.UtcNow.Year}DB-0001";
        }

        private async Task<string> GenerateNextAccountIdApp()
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<BsonDocument>("loan_application");

            // Find the latest Account ID
            var sort = Builders<BsonDocument>.Sort.Descending("_id");
            var lastRecord = await collection.Find(new BsonDocument()).Sort(sort).Limit(1).FirstOrDefaultAsync();

            if (lastRecord != null && lastRecord.Contains("AccountId"))
            {
                string lastAccountId = lastRecord["AccountId"].AsString;
                Match match = Regex.Match(lastAccountId, @"RCT-(\d{4})DB-(\d+)");

                if (match.Success)
                {
                    int year = int.Parse(match.Groups[1].Value);
                    int lastNumber = int.Parse(match.Groups[2].Value);
                    int newNumber = lastNumber + 1;

                    return $"Account ID: RCT-{year}DB-{newNumber:D4}";
                }
            }

            // Default starting value if no records exist
            return $"Account ID: RCT-{DateTime.UtcNow.Year}DB-0001";
        }

        private async void bsubmit_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                // Validate required fields before proceeding
                if (string.IsNullOrWhiteSpace(tnclientname.Text) || string.IsNullOrWhiteSpace(tnaddress.Text))
                {
                    MessageBox.Show("Client Name and Address are required!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (filePaths.Count == 0)
                {
                    MessageBox.Show("Please select files to upload.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.Cursor = Cursors.WaitCursor;
                pbloading.Visible = true;
                pbloading.Maximum = filePaths.Count;
                pbloading.Value = 0;
                lstatus.Visible = true;
                lstatus.Text = "Initializing upload...";
                lstatus.ForeColor = Color.Black;

                string currentUser = LoadCurrentUserFullName();
                string selectedCollector = cbcollectors.SelectedItem?.ToString() ?? "N/A";
                string ciStatus = cbCInvest.Checked ? "✔️" : "❌";
                DateTime ciDate = dtci.Value;

                if (!decimal.TryParse(tloannewamt.Text, out decimal loanAmount))
                {
                    MessageBox.Show("Invalid Loan Amount!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string loanDescription = rtloandesc.Text.Trim();
                string rawAccountId = await GenerateNextAccountId();
                string cleanAccountId = rawAccountId.Replace("Account ID: ", "");

                // **Hardcoded Routed User Details (Owner)**
                string routedUserFullName = "Ralph Daag";
                string routedUserPosition = "Administrator";
                string routedUserDesignation = "Owner";

                List<BsonDocument> uploadedFiles = new List<BsonDocument>();

                for (int i = 0; i < filePaths.Count; i++)
                {
                    string filePath = filePaths[i];
                    if (string.IsNullOrEmpty(filePath))
                    {
                        MessageBox.Show("Please fill in all fields and select a file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string mimeType = GetMimeType(filePath);
                    string destinationFolderId = GetDestinationFolderId(mimeType);
                    string originalFileName = Path.GetFileName(filePath);

                    dgvuploads.Invoke((MethodInvoker)(() =>
                    {
                        if (i < dgvuploads.Rows.Count)
                            dgvuploads.Rows[i].Cells["Status"].Value = "Uploading...";
                    }));

                    lstatus.Invoke((MethodInvoker)(() =>
                    {
                        lstatus.Text = $"Uploading file {i + 1} of {filePaths.Count}...";
                    }));

                    string fileId = await UploadFileToDrive(filePath, destinationFolderId);

                    dgvuploads.Invoke((MethodInvoker)(() =>
                    {
                        if (i < dgvuploads.Rows.Count)
                        {
                            dgvuploads.Rows[i].Cells["Status"].Value = fileId != null ? "Upload Done" : "Failed";
                        }
                    }));

                    pbloading.Invoke((MethodInvoker)(() =>
                    {
                        pbloading.Value = i + 1;
                    }));

                    if (fileId != null)
                    {
                        string fileLink = $"https://drive.google.com/file/d/{fileId}/view?usp=sharing";

                        uploadedFiles.Add(new BsonDocument
                {
                    { "file_name", originalFileName },
                    { "file_type", mimeType },
                    { "file_link", fileLink }
                });
                    }
                }

                // Save data to MongoDB
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_application");

                var filter = Builders<BsonDocument>.Filter.Eq("AccountId", cleanAccountId);
                var update = Builders<BsonDocument>.Update
                    .Set("AccountId", lloanno.Text.Replace("Account ID: ", ""))
                    .Set("ClientName", tnclientname.Text)
                    .Set("Address", tnaddress.Text)
                    .Set("ApplicationDate", DateTime.UtcNow)
                    .Set("Status", "NEW")
                    .Set("LoanStatus", "FOR VERIFICATION AND APPROVAL")
                    .Set("EncodedBy", currentUser)
                    .Set("CollectionInCharge", selectedCollector)
                    .Set("CI", ciStatus)
                    .Set("CIDate", ciDate)
                    .Set("LoanAmount", loanAmount)
                    .Set("LoanDescription", loanDescription)
                    .Set("UploadedDocs", uploadedFiles)

                    // **Hardcoded Routed User (Always Ralph Daag)**
                    .Set("RoutedTo", new BsonDocument
                    {
                         { "FullName", routedUserFullName },
                         { "Position", routedUserPosition },
                         { "Designation", routedUserDesignation }
                    });

                await collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

                lloanno.Text = await GenerateNextAccountIdApp();

                filePaths.Clear();
                tnclientname.Text = string.Empty;
                tnaddress.Text = string.Empty;
                cbcollectors.SelectedIndex = -1;
                cbCInvest.Checked = false;
                dtci.Value = DateTime.Today;
                tloannewamt.Text = string.Empty;
                rtloandesc.Text = string.Empty;
                dgvuploads.Invoke((MethodInvoker)(() => dgvuploads.Rows.Clear()));

                lstatus.Invoke((MethodInvoker)(() =>
                {
                    lstatus.Text = "Upload completed!";
                    lstatus.ForeColor = Color.Green;
                }));

                pbloading.Invoke((MethodInvoker)(() =>
                {
                    pbloading.Visible = false;
                }));

                MessageBox.Show(this, "Data and files uploaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lstatus.Text = "";
                lstatus.Visible = false;
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error uploading files and saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void frm_home_addnew_FormClosing(object sender, FormClosingEventArgs e)
        {
            dgvuploads.Rows.Clear();
            filePaths.Clear();
            lfilesready.Visible = false;
            lnofile.Visible = true;

            this.Hide();
            e.Cancel = true;
        }

        private void brenewclear_Click(object sender, EventArgs e)
        {
            lloannorenew.Text = "";
            trsavings.Text = "";
            trclientname.Text = "";
            trprevloan.Text = "";
            trpaymentmode.Text = "";
            trterms.Text = "";
            trcycle.Text = "";
            dtrdateeval.Value = DateTime.Now;
            cbrstatus.SelectedItem = -1;
            trclose.Text = "";
            trdaysmissed.Text = "";
            trpayment.Text = "";
            trloanbal.Text = "";
            trrebates.Text = "";
            trpaidto.Text = "";
            trorno.Text = "";
            trpaymentdelay.Text = "";
            cbsimilarapplicant.CheckState = CheckState.Unchecked;
            cbsimilarborrower.CheckState = CheckState.Unchecked;
            cbsimilarmaker.CheckState = CheckState.Unchecked;
            tramendfromapplicant.Text = "";
            tramendfromborrower.Text = "";
            tramendfrommaker.Text = "";
            tramendtoapplicant.Text = "";
            tramendtoborrower.Text = "";
            tramendtomaker.Text = "";
            trreleasesched.Text = "";
            trremarks.Text = "";


            dgvruploads.Rows.Clear();
            lrreadyfiles.Visible = false;
            lrnofile.Visible = true;
            lrstatusupload.Visible = false;
            pbrloading.Visible = false;

            brenewsubmit.Enabled = false;
            brenewprint.Enabled = false;

            brenewclear.Enabled = false;
        }

        private void trclientname_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(trclientname.Text))
            {
                // Clear fields if no name is selected
                trpaymentmode.Text = string.Empty;
                cbrstatus.SelectedIndex = -1;
                trloanbal.Text = string.Empty;
                lloannorenew.Text = string.Empty;
                lloannorenew.Visible = false;
                brenewclear.Enabled = false; // Disable button when empty
                trprevloan.Text = string.Empty;
            }
            else
            {
                lloannorenew.Visible = true;
                brenewclear.Enabled = true;
                GenerateRenewalLoanID();

                // Fetch and populate loan details
                LoadLoanDetails(trclientname.Text);
            }
        }

        private void brenewadd_Click(object sender, EventArgs e)
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

                    dgvruploads.Rows.Add(Path.GetFileName(fileName), fileSizeString, "Ready to Upload", "Action");
                    lrnofile.Visible = false;
                    lrreadyfiles.Visible = true;
                }
            }
        }

        private async Task UpdatePendingCount()
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_application");

                string todayDate = DateTime.Now.ToString("MM/dd/yyyy");

                var filter = Builders<BsonDocument>.Filter.And(
                    Builders<BsonDocument>.Filter.Eq("ApplicationStatus", "PENDING RENEWAL LOAN APPLICATION"),
                    Builders<BsonDocument>.Filter.Eq("DateEvaluated", todayDate)
                );

                long count = await collection.CountDocumentsAsync(filter);

                // Update lcountpending in frm_home
                homeForm?.UpdatePendingCountLabel(count.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving pending count: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void brenewsubmit_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                // Validate required fields before proceeding
                if (string.IsNullOrWhiteSpace(trclientname.Text) || string.IsNullOrWhiteSpace(trprevloan.Text))
                {
                    MessageBox.Show("Client Name and Previous Loan Data are required!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (filePaths.Count == 0)
                {
                    MessageBox.Show("Please select files to upload.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                pbrloading.Maximum = filePaths.Count;
                pbrloading.Value = 0;

                lrstatusupload.Text = "Uploading files...";
                lrstatusupload.ForeColor = Color.Black;

                // Collections to hold file details
                List<BsonDocument> uploadedFiles = new List<BsonDocument>();

                for (int i = 0; i < filePaths.Count; i++)
                {
                    string filePath = filePaths[i];

                    if (string.IsNullOrEmpty(filePath))
                    {
                        MessageBox.Show("Please fill in all fields and select a file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string mimeType = GetMimeType(filePath);
                    string destinationFolderId = GetDestinationFolderId(mimeType);
                    string originalFileName = Path.GetFileName(filePath);

                    // Update status in DataGridView
                    dgvruploads.Invoke((MethodInvoker)(() =>
                    {
                        if (i < dgvruploads.Rows.Count)
                            dgvruploads.Rows[i].Cells["Status"].Value = "Uploading";
                    }));

                    // Upload file to Google Drive
                    string fileId = await UploadFileToDrive(filePath, destinationFolderId);

                    // Update status in DataGridView
                    dgvruploads.Invoke((MethodInvoker)(() =>
                    {
                        if (i < dgvruploads.Rows.Count)
                        {
                            dgvruploads.Rows[i].Cells["Status"].Value = fileId != null ? "Upload Done" : "Failed";
                        }
                    }));

                    // Update progress bar
                    pbrloading.Invoke((MethodInvoker)(() =>
                    {
                        pbrloading.Value = i + 1;
                    }));

                    // Save file details for MongoDB
                    if (fileId != null)
                    {
                        string fileLink = $"https://drive.google.com/file/d/{fileId}/view?usp=sharing";

                        uploadedFiles.Add(new BsonDocument
                {
                    { "file_name", originalFileName },
                    { "file_type", mimeType },
                    { "file_link", fileLink }
                });
                    }
                }

                // Hardcoded Routed User (Always Ralph Daag)
                string routedUserFullName = "Ralph Daag";
                string routedUserPosition = "Administrator";
                string routedUserDesignation = "Owner";

                // Additional fields
                string collectorInCharge = cbcollectorincharge.SelectedItem?.ToString() ?? "";
                bool creditInvestigation = cbCIrenew.Checked;
                string creditInvestigationDate = dtcirenew.Value.ToString("MM/dd/yyyy");
                string renewAmount = trenewamt.Text;
                string renewLoanDescription = rtloandescrenew.Text;

                // Save data to MongoDB (loan_application collection)
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_application");

                var filter = Builders<BsonDocument>.Filter.Eq("AccountId", lloanno.Text);

                var update = Builders<BsonDocument>.Update
                    .Set("AccountId", lloannorenew.Text.Replace("Account ID: ", "").Trim())
                    .Set("Savings", trsavings.Text)
                    .Set("ClientName", trclientname.Text)
                    .Set("PreviousLoan", trprevloan.Text)
                    .Set("PaymentMode", trpaymentmode.Text)
                    .Set("LoanTerms", trterms.Text)
                    .Set("LoanCycle", trcycle.Text)
                    .Set("ApplicationDate", dtrdateeval.Value.ToString("MM/dd/yyyy"))
                    .Set("RenewalStatus", cbrstatus.SelectedItem?.ToString() ?? "")
                    .Set("CloseLoanDate", trclose.Value.ToString("MM/dd/yyyy"))
                    .Set("DaysMissed", trdaysmissed.Text)
                    .Set("PaymentAmount", trpayment.Text)
                    .Set("LoanBalance", trloanbal.Text)
                    .Set("Rebates", trrebates.Text)
                    .Set("PaidToDate", trpaidto.Text)
                    .Set("ORNumber", trorno.Text)
                    .Set("PaymentDelay", trpaymentdelay.Text)
                    .Set("SimilarApplicant", cbsimilarapplicant.Checked)
                    .Set("SimilarBorrower", cbsimilarborrower.Checked)
                    .Set("SimilarMaker", cbsimilarmaker.Checked)
                    .Set("AmendedFromApplicant", tramendfromapplicant.Text)
                    .Set("AmendedFromBorrower", tramendfromborrower.Text)
                    .Set("AmendedFromMaker", tramendfrommaker.Text)
                    .Set("AmendedToApplicant", tramendtoapplicant.Text)
                    .Set("AmendedToBorrower", tramendtoborrower.Text)
                    .Set("AmendedToMaker", tramendtomaker.Text)
                    .Set("ReleaseSchedule", trreleasesched.Text)
                    .Set("Remarks", trremarks.Text)
                    .Set("UploadedDocs", uploadedFiles)
                    .Set("LoanStatus", "PENDING RENEWAL LOAN APPLICATION") // Default status
                    .Set("RoutedTo", new BsonDocument
                    {
                        { "FullName", routedUserFullName },
                        { "Position", routedUserPosition },
                        { "Designation", routedUserDesignation }
                    })
                    .Set("CollectorInCharge", collectorInCharge)
                    .Set("CreditInvestigation", creditInvestigation)
                    .Set("CreditInvestigationDate", creditInvestigationDate)
                    .Set("LoanAmount", renewAmount)
                    .Set("LoanDescription", renewLoanDescription);

                await collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

                // Clear form fields after successful upload
                filePaths.Clear();
                dgvruploads.Invoke((MethodInvoker)(() => dgvruploads.Rows.Clear()));

                // Update status label
                lrstatusupload.Text = "Upload completed!";
                lrstatusupload.ForeColor = Color.Green;

                MessageBox.Show(this, "Data and files uploaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await UpdatePendingCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error uploading files and saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvruploads_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the click is on the delete button column
            if (e.ColumnIndex == dgvruploads.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                // Ensure there are rows in the DataGridView
                if (dgvruploads.Rows.Count > 0 && e.RowIndex < dgvruploads.Rows.Count)
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
                        dgvruploads.Rows.RemoveAt(e.RowIndex);

                        // Optionally, check if there are no more files and update the lnofile label visibility
                        if (filePaths.Count == 0)
                        {
                            lrnofile.Visible = true;
                            lrreadyfiles.Visible = false;
                        }
                    }
                }
            }
        }

        private void trclientname_MouseLeave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(trclientname.Text))
            {
                lloannorenew.Visible = true;
                brenewclear.Enabled = true;
                GenerateRenewalLoanID();
                LoadLoanDetails(trclientname.Text);
            }
        }

        private void brenewprint_Click(object sender, EventArgs e)
        {
            try
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                // Prompt user to select a save location
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    Title = "Save Verification Sheet",
                    FileName = $"Verification_Sheet_{DateTime.Now:yyyyMMdd}.xlsx" // Dynamic filename
                };

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return; // User canceled

                string savePath = saveFileDialog.FileName;

                string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "docs", "verification_sheet.xlsx");
                //MessageBox.Show($"Trying to access: {templatePath}", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);

                FileInfo templateFile = new FileInfo(templatePath);
                FileInfo saveFile = new FileInfo(savePath);

                using (ExcelPackage package = new ExcelPackage(templateFile))
                {
                    int sheetCount = package.Workbook.Worksheets.Count;
                    //MessageBox.Show($"Total Sheets Found: {sheetCount}", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (sheetCount == 0)
                    {
                        MessageBox.Show("❌ No sheets found in the Excel file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    ExcelWorksheet sheet = package.Workbook.Worksheets[0]; // First sheet

                    // Populate form fields dynamically
                    sheet.Cells["L4"].Value = trsavings.Text;
                    sheet.Cells["D5"].Value = trclientname.Text;
                    sheet.Cells["D6"].Value = trprevloan.Text;
                    sheet.Cells["H6"].Value = trpaymentmode.Text;
                    sheet.Cells["J6"].Value = trterms.Text;
                    sheet.Cells["L6"].Value = trcycle.Text;
                    sheet.Cells["D7"].Value = dtrdateeval.Value.ToString("MM/dd/yyyy");
                    sheet.Cells["H9"].Value = trclose.Value.ToString("MM/dd/yyyy");
                    sheet.Cells["J9"].Value = trdaysmissed.Text;
                    sheet.Cells["L9"].Value = trpayment.Text;
                    sheet.Cells["D10"].Value = trloanbal.Text;
                    sheet.Cells["F10"].Value = trrebates.Text;
                    sheet.Cells["F11"].Value = trorno.Text;
                    sheet.Cells["K11"].Value = trpaymentdelay.Text;

                    // Signature Verification
                    sheet.Cells["D15"].Value = cbsimilarapplicant.Checked ? "✓" : "";
                    sheet.Cells["D16"].Value = cbsimilarborrower.Checked ? "✓" : "";
                    sheet.Cells["D17"].Value = cbsimilarmaker.Checked ? "✓" : "";

                    sheet.Cells["G15"].Value = tramendfromapplicant.Text;
                    sheet.Cells["J15"].Value = tramendtoapplicant.Text;
                    sheet.Cells["G16"].Value = tramendfromborrower.Text;
                    sheet.Cells["J16"].Value = tramendtoborrower.Text;
                    sheet.Cells["G17"].Value = tramendfrommaker.Text;
                    sheet.Cells["J17"].Value = tramendtomaker.Text;

                    // Additional fields
                    sheet.Cells["D9"].Value = cbrstatus.SelectedItem?.ToString() ?? "";
                    //sheet.Cells["G11"].Value = cbCIrenew.Checked ? "✓" : "";
                    //sheet.Cells["H11"].Value = dtcirenew.Value.ToString("MM/dd/yyyy");
                    //sheet.Cells["H12"].Value = trenewamt.Text;

                    sheet.Cells["E20"].Value = trreleasesched.Text;
                    sheet.Cells["C21"].Value = trremarks.Text;
                    //sheet.Cells["G16"].Value = rtloandescrenew.Text;

                    // Prepared & Approved By
                    sheet.Cells["D23"].Value = lveruser.Text;
                    sheet.Cells["I23"].Value = lverowner.Text;

                    // Save the updated Excel file
                    package.SaveAs(saveFile);
                }

                // Open the saved file
                Process.Start(new ProcessStartInfo(savePath) { UseShellExecute = true });

                MessageBox.Show("Verification Sheet generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating verification sheet: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

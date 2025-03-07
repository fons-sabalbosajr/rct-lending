using DnsClient.Protocol;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

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
                var collection = database.GetCollection<BsonDocument>("loan_approved");

                var sort = Builders<BsonDocument>.Sort.Descending("AccountId");
                var lastLoanApplication = collection.Find(new BsonDocument()).Sort(sort).FirstOrDefault();

                string nextAccountId;

                if (lastLoanApplication != null)
                {
                    var lastAccountId = lastLoanApplication.GetValue("AccountId", "").AsString;

                    var parts = lastAccountId.Split('-');
                    if (parts.Length == 3 && int.TryParse(parts[2], out int numericPart))
                    {
                        numericPart++;
                        nextAccountId = $"{parts[0]}-{parts[1]}-{numericPart:D4}";
                    }
                    else
                    {
                        // Fallback if parsing fails
                        throw new FormatException("Invalid AccountId format.");
                    }
                }
                else
                {
                    // If there are no documents, start with the first AccountId
                    nextAccountId = "RCT-2024-0001";
                }

                // Set the new AccountId to the label
                lloanno.Text = "Account ID: " + nextAccountId;
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
                    // ✅ Populate Fields
                    trpaymentmode.Text = clientRecord.Contains("PaymentMode") ? clientRecord["PaymentMode"].AsString : "";
                    trloanbal.Text = clientRecord.Contains("LoanBalance") ? clientRecord["LoanBalance"].AsString : "";
                    trprevloan.Text = clientRecord.Contains("LoanAmount") ? clientRecord["LoanAmount"].AsString : "";

                    // ✅ Match LoanStatus in ComboBox
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

                foreach (var collector in collectors)
                {
                    if (collector.Contains("Name"))
                    {
                        string name = collector["Name"].AsString;
                        cbcollectors.Items.Add(name);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading collectors: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        LoadingFunction load = new LoadingFunction();
        private void frm_home_addnew_Load(object sender, EventArgs e)
        {
            LoadClientNames();
            LoadCollectors();
            LoadClientNamesNew();
            lfilesready.Visible = false;
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

                // Ensure progress bar and status label are visible
                pbloading.Visible = true;
                pbloading.Maximum = filePaths.Count;
                pbloading.Value = 0;
                lstatus.Visible = true;
                lstatus.Text = "Initializing upload...";
                lstatus.ForeColor = Color.Black;

                // Get the full name of the current user
                string currentUser = LoadCurrentUserFullName();

                // Get the selected collector
                string selectedCollector = cbcollectors.SelectedItem?.ToString() ?? "N/A";

                // Get Credit Investigation status
                string ciStatus = cbCInvest.Checked ? "✔️" : "❌";

                // Get CI Date
                DateTime ciDate = dtci.Value;

                // Get Loan Amount (ensure it's a valid decimal)
                decimal loanAmount;
                if (!decimal.TryParse(tloannewamt.Text, out loanAmount))
                {
                    MessageBox.Show("Invalid Loan Amount!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get Loan Description
                string loanDescription = rtloandesc.Text.Trim();

                // Generate the next Account ID and remove "Account ID: " prefix
                string rawAccountId = await GenerateNextAccountId();
                string cleanAccountId = rawAccountId.Replace("Account ID: ", ""); // Remove the prefix

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
                    dgvuploads.Invoke((MethodInvoker)(() =>
                    {
                        if (i < dgvuploads.Rows.Count)
                            dgvuploads.Rows[i].Cells["Status"].Value = "Uploading...";
                    }));

                    // Update status label
                    lstatus.Invoke((MethodInvoker)(() =>
                    {
                        lstatus.Text = $"Uploading file {i + 1} of {filePaths.Count}...";
                    }));

                    // Upload file to Google Drive
                    string fileId = await UploadFileToDrive(filePath, destinationFolderId);

                    // Update status in DataGridView
                    dgvuploads.Invoke((MethodInvoker)(() =>
                    {
                        if (i < dgvuploads.Rows.Count)
                        {
                            dgvuploads.Rows[i].Cells["Status"].Value = fileId != null ? "Upload Done" : "Failed";
                        }
                    }));

                    // Update progress bar
                    pbloading.Invoke((MethodInvoker)(() =>
                    {
                        pbloading.Value = i + 1;
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

                // Save data to MongoDB (loan_application collection)
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_application");

                var filter = Builders<BsonDocument>.Filter.Eq("AccountId", cleanAccountId);
                var update = Builders<BsonDocument>.Update
                    .Set("AccountId", cleanAccountId) // Save cleaned Account ID
                    .Set("ClientName", tnclientname.Text)
                    .Set("Address", tnaddress.Text)
                    .Set("ApplicationDate", DateTime.UtcNow) // Store current date & time
                    .Set("Status", "NEW") // Set status as "NEW"
                    .Set("LoanStatus", "FOR VERIFICATION AND APPROVAL") // Loan status
                    .Set("EncodedBy", currentUser) // Store full name of the user
                    .Set("CollectionInCharge", selectedCollector) // Save selected collector
                    .Set("CI", ciStatus) // Save Credit Investigation status
                    .Set("CIDate", ciDate) // Save CI Date
                    .Set("LoanAmount", loanAmount) // Save Loan Amount
                    .Set("LoanDescription", loanDescription) // Save Loan Description
                    .Set("UploadedDocs", uploadedFiles); // Store all uploaded files

                await collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

                // Update Account ID field with the next available ID
                lloanno.Text = await GenerateNextAccountIdApp();

                // Clear form fields after successful upload
                filePaths.Clear();
                tnclientname.Text = string.Empty;
                tnaddress.Text = string.Empty;
                cbcollectors.SelectedIndex = -1; // Reset collectors dropdown
                cbCInvest.Checked = false; // Uncheck checkbox
                dtci.Value = DateTime.Today; // Reset CI Date to today
                tloannewamt.Text = string.Empty; // Clear loan amount
                rtloandesc.Text = string.Empty; // Clear loan description
                dgvuploads.Invoke((MethodInvoker)(() => dgvuploads.Rows.Clear()));

                // Update status label
                lstatus.Invoke((MethodInvoker)(() =>
                {
                    lstatus.Text = "Upload completed!";
                    lstatus.ForeColor = Color.Green;
                }));

                // Hide progress bar after upload
                pbloading.Invoke((MethodInvoker)(() =>
                {
                    pbloading.Visible = false;
                }));

                MessageBox.Show(this, "Data and files uploaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                // Save data to MongoDB (loan_application collection)
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<BsonDocument>("loan_application");

                var filter = Builders<BsonDocument>.Filter.Eq("AccountId", lloanno.Text);

                var update = Builders<BsonDocument>.Update
                    .Set("AccountId", lloannorenew.Text)
                    .Set("Savings", trsavings.Text)
                    .Set("ClientName", trclientname.Text)
                    .Set("PreviousLoan", trprevloan.Text)
                    .Set("PaymentMode", trpaymentmode.Text)
                    .Set("LoanTerms", trterms.Text)
                    .Set("LoanCycle", trcycle.Text)
                    .Set("DateEvaluated", dtrdateeval.Value.ToString("MM/dd/yyyy"))
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
                    .Set("ApplicationStatus", "PENDING RENEWAL LOAN APPLICATION"); // Default status

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

                // Fetch and populate loan details only after name is confirmed
                LoadLoanDetails(trclientname.Text);
            }
        }
    }
}

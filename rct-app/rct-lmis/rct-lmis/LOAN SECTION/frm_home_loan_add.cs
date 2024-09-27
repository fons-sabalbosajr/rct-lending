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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace rct_lmis.LOAN_SECTION
{
    public partial class frm_home_loan_add : Form
    {
        private IMongoCollection<BsonDocument> collection;
        private readonly IMongoCollection<BsonDocument> loanRateCollection;

        public frm_home_loan_add()
        {
            InitializeComponent();
            bsavetransaction.Enabled = false;
            InitializeDataGridView();
            InitializeMongoDBUpload();
            InitializeGoogleDrive();
            SetupAutoCompleteProvince();
            LoadNextAccountId();

            var database = MongoDBConnection.Instance.Database;
            loanRateCollection = database.GetCollection<BsonDocument>("loan_rate");

        }
        private static string[] Scopes = { DriveService.Scope.DriveFile };
        private static string ApplicationName = "rct-lmis";
        private DriveService service;


        // Folder IDs
        private static string DocsFolderId = "1kMd3QjEw95oJsMSAK9xwEf-I3_MKlMBj";
        private static string ImagesFolderId = "1O_-PLQyRAjUV7iy6d3PN5rLXznOzxean";
        private List<string> filePaths = new List<string>();


        LoadingFunction load = new LoadingFunction();

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

        private void InitializeMongoDBUpload()
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<Application>("loan_application");
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
                MessageBox.Show($"Error initializing Google Drive service: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupAutoCompleteProvince()
        {
            // List of provinces in the Philippines
            string[] provinces = new string[]
            {
                "Abra", "Agusan del Norte", "Agusan del Sur", "Aklan", "Albay",
                "Antique", "Apayao", "Aurora", "Basilan", "Bataan", "Batanes",
                "Batangas", "Benguet", "Biliran", "Bohol", "Bukidnon",
                "Bulacan", "Cagayan", "Camarines Norte", "Camarines Sur",
                "Camiguin", "Capiz", "Catanduanes", "Cavite", "Cebu",
                "Compostela Valley", "Cotabato", "Davao del Norte", "Davao del Sur",
                "Davao Occidental", "Davao Oriental", "Dinagat Islands", "Eastern Samar",
                "Guimaras", "Ifugao", "Ilocos Norte", "Ilocos Sur", "Iloilo",
                "Isabela", "Kalinga", "La Union", "Laguna", "Lanao del Norte",
                "Lanao del Sur", "Leyte", "Maguindanao", "Marinduque", "Masbate",
                "Misamis Occidental", "Misamis Oriental", "Mountain Province",
                "Negros Occidental", "Negros Oriental", "Northern Samar",
                "Nueva Ecija", "Nueva Vizcaya", "Occidental Mindoro", "Oriental Mindoro",
                "Palawan", "Pampanga", "Pangasinan", "Quezon", "Quirino",
                "Rizal", "Romblon", "Samar", "Sarangani", "Siquijor",
                "Sorsogon", "South Cotabato", "Southern Leyte", "Sultan Kudarat",
                "Sulu", "Surigao del Norte", "Surigao del Sur", "Tarlac",
                "Tawi-Tawi", "Zambales", "Zamboanga del Norte", "Zamboanga del Sur",
                "Zamboanga Sibugay"
            };

            // Create an AutoCompleteStringCollection and add the provinces to it
            AutoCompleteStringCollection autoCompleteProvinces = new AutoCompleteStringCollection();
            autoCompleteProvinces.AddRange(provinces);

            // Set up the textbox for autocomplete
            tbrprovince.AutoCompleteCustomSource = autoCompleteProvinces;
            tbrprovince.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tbrprovince.AutoCompleteSource = AutoCompleteSource.CustomSource;


            tbrprovpr.AutoCompleteCustomSource = autoCompleteProvinces;
            tbrprovpr.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tbrprovpr.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private async void bsubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (filePaths.Count == 0)
                {
                    MessageBox.Show("Please select files to upload.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                mainProgressBar.Maximum = filePaths.Count;
                mainProgressBar.Value = 0;

                statusLabel.Text = "Uploading files...";
                statusLabel.ForeColor = Color.Black;

                // Collections to hold file details
                List<string> fileNames = new List<string>();
                List<string> fileTypes = new List<string>();
                List<string> fileLinks = new List<string>();

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

                    // Update status in DataGridView (Invoke required for cross-thread access)
                    dgvuploads.Invoke((MethodInvoker)(() =>
                    {
                        if (i < dgvuploads.Rows.Count)
                            dgvuploads.Rows[i].Cells["Status"].Value = "Uploading";
                    }));

                    // Upload file to Google Drive
                    string fileId = await UploadFileToDrive(filePath, destinationFolderId);

                    // Update status in DataGridView (Invoke required for cross-thread access)
                    dgvuploads.Invoke((MethodInvoker)(() =>
                    {
                        if (i < dgvuploads.Rows.Count)
                        {
                            if (fileId != null)
                                dgvuploads.Rows[i].Cells["Status"].Value = "Upload Done";
                            else
                                dgvuploads.Rows[i].Cells["Status"].Value = "Failed";
                        }
                    }));

                    // Update progress bar (Invoke required for cross-thread access)
                    mainProgressBar.Invoke((MethodInvoker)(() =>
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
                        fileTypes.Add(mimeType);
                        fileLinks.Add(fileLink);
                    }
                }

                // Combine file details
                string combinedFileNames = string.Join(", ", fileNames);
                string combinedFileTypes = string.Join(", ", fileTypes);
                string combinedFileLinks = string.Join(", ", fileLinks);

                // Update loan_application document with file details
                var filter = Builders<Application>.Filter.Eq("AccountId", laccountid.Text);
                var update = Builders<Application>.Update
                    .Set("docs", combinedFileNames)
                    .Set("doc_type", combinedFileTypes)
                    .Set("doc-link", combinedFileLinks);

                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<Application>("loan_application");
                await collection.UpdateOneAsync(filter, update);

                // Clear form fields after successful upload
                filePaths.Clear();
                dgvuploads.Invoke((MethodInvoker)(() => dgvuploads.Rows.Clear()));

                // Update status label
                statusLabel.Text = "Upload completed!";
                statusLabel.ForeColor = Color.Green;

                MessageBox.Show(this, "Data and files uploaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error uploading files and saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private string GetTextBoxValueOrDefault(Guna.UI2.WinForms.Guna2TextBox textBox, string defaultValue = "")
        {
            return string.IsNullOrWhiteSpace(textBox.Text) ? defaultValue : textBox.Text;
        }


        private void frm_home_loan_add_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void frm_home_loan_add_Load(object sender, EventArgs e)
        {
          

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

        private void buploadform_Click(object sender, EventArgs e)
        {
            var application = new Application
            {
                LoanType = rbnew.Checked ? "New" :
                           rbrenewal.Checked ? "Renewal" : "Not Specified", // Default value

                RentType = rbrentown.Checked ? "Owned" :
                           rbrentrf.Checked ? "Rented" :
                           rbrentrl.Checked ? "Living with Relatives" : "Not Specified", // Default value

                RBLate = dtbrbdate.Value,
                RSDate = dtbrsdate.Value,
                AccountId = laccountid.Text,
                Status = cbstatus.SelectedItem?.ToString() ?? "n/a",
                CStatus = cbcstatus.SelectedItem?.ToString() ?? "n/a",
                Gender = cbgender.SelectedItem?.ToString() ?? "n/a",
                CGender = cbcgender.SelectedItem?.ToString() ?? "n/a",
                LastName = GetTextBoxValueOrDefault(tbrlname),
                FirstName = GetTextBoxValueOrDefault(tbrfname),
                MiddleName = GetTextBoxValueOrDefault(tbrmname),
                SuffixName = GetTextBoxValueOrDefault(tbrsname),
                Street = GetTextBoxValueOrDefault(tbrstreet),
                Barangay = GetTextBoxValueOrDefault(tbrbrgy),
                City = GetTextBoxValueOrDefault(tbrcity),
                Province = GetTextBoxValueOrDefault(tbrprovince),
                StreetPR = GetTextBoxValueOrDefault(tbrstreetpr),
                BarangayPR = GetTextBoxValueOrDefault(tbrbrgypr),
                CityPR = GetTextBoxValueOrDefault(tbrcitypr),
                ProvincePR = GetTextBoxValueOrDefault(tbrprovpr),
                Fee = GetTextBoxValueOrDefault(trfee),
                StayLength = GetTextBoxValueOrDefault(tstaylength),
                Business = GetTextBoxValueOrDefault(tbusiness),
                Income = GetTextBoxValueOrDefault(tincome),
                CP = GetTextBoxValueOrDefault(tbrcp),
                Spouse = GetTextBoxValueOrDefault(tspouse),
                Occupation = GetTextBoxValueOrDefault(tbroccupation),
                SpIncome = GetTextBoxValueOrDefault(tbrspincome),
                SpCP = GetTextBoxValueOrDefault(tbrspcp),
                CBLName = GetTextBoxValueOrDefault(tcblname),
                CBFName = GetTextBoxValueOrDefault(tcbfname),
                CBMName = GetTextBoxValueOrDefault(tcbmname),
                CBSName = GetTextBoxValueOrDefault(tcbsname),
                CBStreet = GetTextBoxValueOrDefault(tcstreet),
                CBBarangay = GetTextBoxValueOrDefault(tcbrgy),
                CBCity = GetTextBoxValueOrDefault(tccity),
                CBProvince = GetTextBoxValueOrDefault(tcprov),
                CBAge = GetTextBoxValueOrDefault(tcage),
                CBIncome = GetTextBoxValueOrDefault(tcincome),
                CBCP = GetTextBoxValueOrDefault(tccp),
                ApplicationDate = DateTime.Now,
                LoanStatus = "Pending",
            };

            SaveApplication(application);
        }


        private void SaveApplication(Application application)
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<Application>("loan_application");

                load.Show(this);
                Thread.Sleep(1000);
                collection.InsertOne(application);
                load.Close();

                MessageBox.Show(this, "Application saved successfully. Please proceed to the uploading of documents");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the application: {ex.Message}");
            }
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
                laccountid.Text = nextAccountId;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading next account ID: " + ex.Message);
                MessageBox.Show("Error loading next account ID. Please check the console for details.");
            }
        }


        private void bclearinfo_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you want to clear all fields?", "Reset Encoding", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes) 
            {
                // Clear RadioButtons
                rbnew.Checked = false;
                rbrenewal.Checked = false;
                rbrentown.Checked = false;
                rbrentrf.Checked = false;
                rbrentrl.Checked = false;

                // Clear DateTimePickers
                dtbrbdate.Value = DateTime.Now;
                dtbrsdate.Value = DateTime.Now;

                // Clear Labels
                laccountid.Text = string.Empty;

                // Clear ComboBoxes
                cbstatus.SelectedIndex = -1;
                cbcstatus.SelectedIndex = -1;
                cbgender.SelectedIndex = -1;
                cbcgender.SelectedIndex = -1;

                // Clear TextBoxes
                tbrlname.Text = string.Empty;
                tbrfname.Text = string.Empty;
                tbrmname.Text = string.Empty;
                tbrsname.Text = string.Empty;
                tbrstreet.Text = string.Empty;
                tbrbrgy.Text = string.Empty;
                tbrcity.Text = string.Empty;
                tbrprovince.Text = string.Empty;
                tbrstreetpr.Text = string.Empty;
                tbrbrgypr.Text = string.Empty;
                tbrcitypr.Text = string.Empty;
                tbrprovpr.Text = string.Empty;
                trfee.Text = string.Empty;
                tstaylength.Text = string.Empty;
                tbusiness.Text = string.Empty;
                tincome.Text = string.Empty;
                tbrcp.Text = string.Empty;
                tspouse.Text = string.Empty;
                tbroccupation.Text = string.Empty;
                tbrspincome.Text = string.Empty;
                tbrspcp.Text = string.Empty;
                tcblname.Text = string.Empty;
                tcbfname.Text = string.Empty;
                tcbmname.Text = string.Empty;
                tcbsname.Text = string.Empty;
                tcstreet.Text = string.Empty;
                tcbrgy.Text = string.Empty;
                tccity.Text = string.Empty;
                tcprov.Text = string.Empty;
                tcage.Text = string.Empty;
                tcincome.Text = string.Empty;
                tccp.Text = string.Empty;
            }
        }

        // Event handler to populate text boxes with selected row value


        private void GenerelSaveData() 
        {
            try 
            {
                var application = new Application
                {
                    LoanType = rbnew.Checked ? "New" : rbrenewal.Checked ? "Renewal" : string.Empty,
                    RentType = rbrentown.Checked ? "Owned" : rbrentrf.Checked ? "Rented" : rbrentrl.Checked ? "Living with Relatives" : string.Empty,
                    RBLate = dtbrbdate.Value,
                    RSDate = dtbrsdate.Value,
                    AccountId = laccountid.Text,
                    Status = cbstatus.SelectedItem?.ToString() ?? "n/a",
                    CStatus = cbcstatus.SelectedItem?.ToString() ?? "n/a",
                    Gender = cbgender.SelectedItem?.ToString() ?? "n/a",
                    CGender = cbcgender.SelectedItem?.ToString() ?? "n/a",
                    LastName = GetTextBoxValueOrDefault(tbrlname),
                    FirstName = GetTextBoxValueOrDefault(tbrfname),
                    MiddleName = GetTextBoxValueOrDefault(tbrmname),
                    SuffixName = GetTextBoxValueOrDefault(tbrsname),
                    Street = GetTextBoxValueOrDefault(tbrstreet),
                    Barangay = GetTextBoxValueOrDefault(tbrbrgy),
                    City = GetTextBoxValueOrDefault(tbrcity),
                    Province = GetTextBoxValueOrDefault(tbrprovince),
                    StreetPR = GetTextBoxValueOrDefault(tbrstreetpr),
                    BarangayPR = GetTextBoxValueOrDefault(tbrbrgypr),
                    CityPR = GetTextBoxValueOrDefault(tbrcitypr),
                    ProvincePR = GetTextBoxValueOrDefault(tbrprovpr),
                    Fee = GetTextBoxValueOrDefault(trfee),
                    StayLength = GetTextBoxValueOrDefault(tstaylength),
                    Business = GetTextBoxValueOrDefault(tbusiness),
                    Income = GetTextBoxValueOrDefault(tincome),
                    CP = GetTextBoxValueOrDefault(tbrcp),
                    Spouse = GetTextBoxValueOrDefault(tspouse),
                    Occupation = GetTextBoxValueOrDefault(tbroccupation),
                    SpIncome = GetTextBoxValueOrDefault(tbrspincome),
                    SpCP = GetTextBoxValueOrDefault(tbrspcp),
                    CBLName = GetTextBoxValueOrDefault(tcblname),
                    CBFName = GetTextBoxValueOrDefault(tcbfname),
                    CBMName = GetTextBoxValueOrDefault(tcbmname),
                    CBSName = GetTextBoxValueOrDefault(tcbsname),
                    CBStreet = GetTextBoxValueOrDefault(tcstreet),
                    CBBarangay = GetTextBoxValueOrDefault(tcbrgy),
                    CBCity = GetTextBoxValueOrDefault(tccity),
                    CBProvince = GetTextBoxValueOrDefault(tcprov),
                    CBAge = GetTextBoxValueOrDefault(tcage),
                    CBIncome = GetTextBoxValueOrDefault(tcincome),
                    CBCP = GetTextBoxValueOrDefault(tccp),
                    ApplicationDate = DateTime.Now
                };

                SaveApplication(application);
               
            }
            catch (Exception e) 
            {
                MessageBox.Show("There is a problem in saving transaction" + e.Message, "Transaction not saved");
            }
        }

        private void DeleteApplication()
        {
            try
            {
                // Access the loan_application collection
                var database = MongoDBConnection.Instance.Database;
                var loanApplicationCollection = database.GetCollection<BsonDocument>("loan_application");

                // Fetch the last document added in the loan_application collection
                var filter = Builders<BsonDocument>.Filter.Empty;
                var lastDocument = loanApplicationCollection.Find(filter).SortByDescending(doc => doc["_id"]).FirstOrDefault();

                if (lastDocument != null)
                {
                    // Delete the last document
                    loanApplicationCollection.DeleteOne(Builders<BsonDocument>.Filter.Eq("_id", lastDocument["_id"]));
                }
                else
                {
                    MessageBox.Show("No documents found in the loan_application collection.", "Deletion Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting the application: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void bloansave_Click(object sender, EventArgs e)
        {
            load.Show(this);
            Thread.Sleep(2000);
            load.Close();
            MessageBox.Show("Update successful!", "Loan Application Amount Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cbsameaddress_CheckedChanged(object sender, EventArgs e)
        {
            if (cbsameaddress.Checked)
            {
                // Copy values from the primary address textboxes to the permanent address textboxes
                tbrstreetpr.Text = tbrstreet.Text;
                tbrbrgypr.Text = tbrbrgy.Text;
                tbrcitypr.Text = tbrcity.Text;
                tbrprovpr.Text = tbrprovince.Text;
            }
            else
            {
                // Clear the permanent address textboxes if the checkbox is unchecked
                tbrstreetpr.Clear();
                tbrbrgypr.Clear();
                tbrcitypr.Clear();
                tbrprovpr.Clear();
            }
        }

        private void babort_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to cancel the transaction?", "Cancel Transaction", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes) 
            {
                load.Show(this);
                Thread.Sleep(1000);
                DeleteApplication();
                load.Close();
                MessageBox.Show("The last saved application has been successfully deleted.", "Deletion Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void bsavetransaction_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save the transaction?", "Save Transaction", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                load.Show(this);
                Thread.Sleep(1000);
                GenerelSaveData();
                load.Close();
                MessageBox.Show("Transaction has been saved?", "Transaction Save successfully!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }
    }

    public class Application
    {
        
        public string LoanType { get; set; }
        public string RentType { get; set; }
        public DateTime RBLate { get; set; }
        public DateTime RSDate { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string AccountId { get; set; }
        public string Status { get; set; }
        public string CStatus { get; set; }
        public string Gender { get; set; }
        public string CGender { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SuffixName { get; set; }
        public string Street { get; set; }
        public string Barangay { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string StreetPR { get; set; }
        public string BarangayPR { get; set; }
        public string CityPR { get; set; }
        public string ProvincePR { get; set; }
        public string Fee { get; set; }
        public string StayLength { get; set; }
        public string Business { get; set; }
        public string Income { get; set; }
        public string CP { get; set; }
        public string Spouse { get; set; }
        public string Occupation { get; set; }
        public string SpIncome { get; set; }
        public string SpCP { get; set; }
        public string CBLName { get; set; }
        public string CBFName { get; set; }
        public string CBMName { get; set; }
        public string CBSName { get; set; }
        public string CBStreet { get; set; }
        public string CBBarangay { get; set; }
        public string CBCity { get; set; }
        public string CBProvince { get; set; }
        public string CBAge { get; set; }
        public string CBIncome { get; set; }
        public string CBCP { get; set; }
        public string LoanStatus { get; internal set; }
    }
}

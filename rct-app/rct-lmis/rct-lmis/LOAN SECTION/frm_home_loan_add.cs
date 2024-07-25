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

                MessageBox.Show("Data and files uploaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void frm_home_loan_add_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void frm_home_loan_add_Load(object sender, EventArgs e)
        {
            LoadDataToDataGridView();
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
                   rbrenewal.Checked ? "Renewal" : string.Empty,
                
                RentType = rbrentown.Checked ? "Owned" :
                   rbrentrf.Checked ? "Rented" :
                   rbrentrl.Checked ? "Living with Relatives" : string.Empty,

                RBLate = dtbrbdate.Value,
                RSDate = dtbrsdate.Value,
                AccountId = laccountid.Text,
                Status = cbstatus.SelectedItem?.ToString(),
                CStatus = cbcstatus.SelectedItem?.ToString(),
                Gender = cbgender.SelectedItem?.ToString(),
                CGender = cbcgender.SelectedItem?.ToString(),
                LastName = tbrlname.Text,
                FirstName = tbrfname.Text,
                MiddleName = tbrmname.Text,
                SuffixName = tbrsname.Text,
                Street = tbrstreet.Text,
                Barangay = tbrbrgy.Text,
                City = tbrcity.Text,
                Province = tbrprovince.Text,
                StreetPR = tbrstreetpr.Text,
                BarangayPR = tbrbrgypr.Text,
                CityPR = tbrcitypr.Text,
                ProvincePR = tbrprovpr.Text,
                Fee = trfee.Text,
                StayLength = tstaylength.Text,
                Business = tbusiness.Text,
                Income = tincome.Text,
                CP = tbrcp.Text,
                Spouse = tspouse.Text,
                Occupation = tbroccupation.Text,
                SpIncome = tbrspincome.Text,
                SpCP = tbrspcp.Text,
                CBLName = tcblname.Text,
                CBFName = tcbfname.Text,
                CBMName = tcbmname.Text,
                CBSName = tcbsname.Text,
                CBStreet = tcstreet.Text,
                CBBarangay = tcbrgy.Text,
                CBCity = tccity.Text,
                CBProvince = tcprov.Text,
                CBAge = tcage.Text,
                CBIncome = tcincome.Text,
                CBCP = tccp.Text,
                ApplicationDate = DateTime.Now
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

                MessageBox.Show("Application saved successfully. Please proceed to the uploading of documents");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the application: {ex.Message}");
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

        private void LoadDataToDataGridView()
        {
            var filter = Builders<BsonDocument>.Filter.Empty; // Retrieve all documents
            var documents = loanRateCollection.Find(filter).ToList();
            DataTable dataTable = new DataTable();

            // Define the columns to display in the DataGridView
            string[] displayColumns = { "Term", "Principal", "Type", "Mode", "Interest Rate/Month", "Processing" };

            // Create the columns in the DataTable
            foreach (string column in displayColumns)
            {
                dataTable.Columns.Add(column);
            }
            dataTable.Columns.Add("FullDocument", typeof(BsonDocument)); // Hidden column to store the entire document

            if (documents.Count > 0)
            {
                // Add rows to the DataTable
                foreach (var doc in documents)
                {
                    DataRow row = dataTable.NewRow();
                    foreach (string column in displayColumns)
                    {
                        if (doc.Contains(column))
                        {
                            var element = doc[column];
                            if (element.IsNumeric())
                            {
                                if (column == "Principal")
                                {
                                    // Format Principal with ₱ text
                                    row[column] = "₱ " + Math.Round(element.ToDouble(), 0).ToString();
                                }
                                else
                                {
                                    // Round numeric values to the nearest ones
                                    row[column] = Math.Round(element.ToDouble(), 0);
                                }
                            }
                            else
                            {
                                row[column] = element.ToString();
                            }
                        }
                    }
                    row["FullDocument"] = doc; // Store the entire document in the hidden column
                    dataTable.Rows.Add(row);
                }
            }

            dgvloandata.DataSource = dataTable;

            if (dgvloandata.Columns.Count > 0)
            {
                dgvloandata.Columns["FullDocument"].Visible = false; // Hide the full document column
            }

            // Center align all columns and set specific numeric formatting
            foreach (DataGridViewColumn column in dgvloandata.Columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Format numeric columns to show two decimal places except "Term"
                if (column.Name != "Term" && (column.Name == "Principal" || column.Name == "Interest Rate/Month" || column.Name == "Processing"))
                {
                    column.DefaultCellStyle.Format = "N2";
                }
            }

            // Debug output to verify the DataTable structure and data
            //Console.WriteLine("DataTable Columns:");
            foreach (DataColumn col in dataTable.Columns)
            {
                Console.WriteLine($"Column: {col.ColumnName}");
            }

            //Console.WriteLine("DataTable Rows:");
            foreach (DataRow row in dataTable.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    Console.Write($"{item} ");
                }
                Console.WriteLine();
            }
        }

        private void SaveLoan()
        {
            if (dgvloandata.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvloandata.SelectedRows[0];
                BsonDocument fullDocument = selectedRow.Cells["FullDocument"].Value as BsonDocument;

                if (fullDocument != null)
                {
                    // Access the loan_application collection
                    var database = MongoDBConnection.Instance.Database;
                    var loanApplicationCollection = database.GetCollection<BsonDocument>("loan_application");

                    // Fetch the last row added in the loan_application collection
                    var filter = Builders<BsonDocument>.Filter.Empty;
                    var lastDocument = loanApplicationCollection.Find(filter).SortByDescending(doc => doc["_id"]).FirstOrDefault();

                    if (lastDocument != null)
                    {
                        try
                        {
                            var updateDefinition = Builders<BsonDocument>.Update
                        .Set("Principal", tloanamt.Text.Replace("₱ ", "").Replace(".00", ""))
                        .Set("Term", tloanterm.Text.Replace(" month/s", ""))
                        .Set("Interest Rate/Month", tloaninterest.Text.Replace(".00", ""))
                        .Set("Processing Fee", trfservicefee.Text.Replace(".00", ""))
                        .Set("Notarial Rate", trfnotarialfee.Text.Replace(".00", ""))
                        .Set("Insurance Rate", trfinsurancefee.Text.Replace(".00", ""))
                        .Set("Annotation Rate", trfannotationfee.Text.Replace(".00", ""))
                        .Set("Vat Rate", trfvat.Text.Replace(".00", ""))
                        .Set("Misc. Rate", trfmisc.Text.Replace(".00", ""))
                        .Set("Doc Rate", trfdocfee.Text.Replace(".00", ""))
                        .Set("Penalty Rate", tpenaltymo.Text.Replace(".00", ""))
                        .Set("Status", "For Approval and Verification");

                            // Update the last document in the collection
                            loanApplicationCollection.UpdateOne(
                                Builders<BsonDocument>.Filter.Eq("_id", lastDocument["_id"]),
                                updateDefinition
                            );

                            // Check the status and disable tabPage4 if necessary
                            if (updateDefinition != null)
                            {
                                string status = "For Approval and Verification"; // Hardcoded status check for this example
                                if (status == "For Approval and Verification")
                                {
                                    guna2TabControl1.TabPages["tabPage4"].Enabled = false;
                                    papprove.Visible = true;
                                }
                                else
                                {
                                    guna2TabControl1.TabPages["tabPage4"].Enabled = true;
                                    papprove.Visible = false;
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error updating document: " + ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No documents found in the loan_application collection.");
                    }
                }
                else
                {
                    MessageBox.Show("Selected row or FullDocument is null.");
                }
            }
            else
            {
                MessageBox.Show("Please select a row to update.");
            }
        }

        // Event handler to populate text boxes with selected row value
        private void bsavetransaction_Click(object sender, EventArgs e)
        {

        }

        private void bloanclear_Click(object sender, EventArgs e)
        {
            tloanamt.Clear();
            tloanterm.Clear();
            tloaninterest.Clear();
            trfservicefee.Clear();
            trfnotarialfee.Clear();
            trfinsurancefee.Clear();
            trfannotationfee.Clear();
            trfvat.Clear();
            trfmisc.Clear();
            trfdocfee.Clear();

            trfnotarialamt.Clear();
            trfinsuranceamt.Clear();
            trfannotationmt.Clear();
            trfvatamt.Clear();
            trfmiscamt.Clear();
            trfdocamt.Clear();
            tamortizedamt.Clear();
            tpenaltymo.Clear();


        }

        private void dgvloandata_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvloandata.ClearSelection();
        }

        private void dgvloandata_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row index is selected
            {
                DataGridViewRow selectedRow = dgvloandata.Rows[e.RowIndex];
                if (selectedRow != null && selectedRow.Cells["FullDocument"].Value != null)
                {
                    BsonDocument fullDocument = selectedRow.Cells["FullDocument"].Value as BsonDocument;

                    if (fullDocument != null)
                    {
                        // Debug output
                        //Console.WriteLine("FullDocument: " + fullDocument.ToJson());

                        try
                        {
                            tloanamt.Text = fullDocument.Contains("Principal") ? "₱ " + fullDocument["Principal"].ToString() + ".00" : string.Empty;
                            tloanterm.Text = fullDocument.Contains("Term") ? fullDocument["Term"].ToString() + " month/s" : string.Empty;
                            tloaninterest.Text = fullDocument.Contains("Interest Rate/Month") ? fullDocument["Interest Rate/Month"].ToString() + ".00" : string.Empty;
                            trfservicefee.Text = fullDocument.Contains("Processing Fee") ? fullDocument["Processing Fee"].ToString() + ".00" : string.Empty;
                            
                            
                            trfnotarialfee.Text = fullDocument.Contains("Notarial Rate") ? fullDocument["Notarial Rate"].ToString() + ".00" : string.Empty;
                            trfnotarialamt.Text = trfannotationfee.Text;

                            trfinsurancefee.Text = fullDocument.Contains("Insurance Rate") ? fullDocument["Insurance Rate"].ToString() + ".00" : string.Empty;
                            trfinsuranceamt.Text = trfinsurancefee.Text;

                            trfannotationfee.Text = fullDocument.Contains("Annotation Rate") ? fullDocument["Annotation Rate"].ToString() + ".00" : string.Empty;
                            trfannotationmt.Text = trfannotationfee.Text;

                            trfvat.Text = fullDocument.Contains("Vat Rate") ? fullDocument["Vat Rate"].ToString() + ".00" : string.Empty;
                            trfvatamt.Text = trfvat.Text;

                            trfmisc.Text = fullDocument.Contains("Misc. Rate") ? fullDocument["Misc. Rate"].ToString() + ".00" : string.Empty;
                            trfmiscamt.Text = trfmisc.Text;

                            trfdocfee.Text = fullDocument.Contains("Doc Rate") ? fullDocument["Doc Rate"].ToString() + ".00" : string.Empty;
                            trfdocamt.Text = trfdocfee.Text;

                            // Assuming you have logic to compute these amounts, otherwise set them as empty or with computed values.
                            tamortizedamt.Text = string.Empty;
                            tpenaltymo.Text = fullDocument.Contains("Penalty Rate") ? fullDocument["Penalty Rate"].ToString() + ".00" : string.Empty;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error processing fullDocument: " + ex.Message);
                        }
                    }
                    else
                    {
                        // Debug output
                        Console.WriteLine("FullDocument is null");
                    }
                }
                else
                {
                    // Debug output
                    Console.WriteLine("Selected row or FullDocument cell is null");
                }
            }
        }

        private void bloansave_Click(object sender, EventArgs e)
        {
            load.Show(this);
            Thread.Sleep(2000);
            SaveLoan();
            load.Close();
            MessageBox.Show("Update successful!", "Loan Application Amount Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}

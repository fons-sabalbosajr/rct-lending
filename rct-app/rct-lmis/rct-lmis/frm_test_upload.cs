using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;

namespace rct_lmis
{
    public partial class frm_test_upload : Form
    {
        private IMongoCollection<BsonDocument> collection;
        private static string[] Scopes = { DriveService.Scope.DriveFile };
        private static string ApplicationName = "rct-lmis";
        private DriveService service;


        // Folder IDs
        private static string DocsFolderId = "1kMd3QjEw95oJsMSAK9xwEf-I3_MKlMBj";
        private static string ImagesFolderId = "1O_-PLQyRAjUV7iy6d3PN5rLXznOzxean";

        private List<string> filePaths = new List<string>();

        public frm_test_upload()
        {
            InitializeComponent();
            InitializeMongoDB();
            InitializeGoogleDrive();
            InitializeDataGridView();
        }

        private void frm_test_upload_Load(object sender, EventArgs e)
        {
        }

        private void InitializeMongoDB()
        {
            var database = MongoDBConnection.Instance.Database;
            collection = database.GetCollection<BsonDocument>("test-data");
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


        private void InitializeDataGridView()
        {
            dgvFiles.AutoGenerateColumns = false;
            dgvFiles.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "FileName",
                HeaderText = "File Name",
                DataPropertyName = "FileName",
                Width = 300
            });
            dgvFiles.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Status",
                HeaderText = "Status",
                DataPropertyName = "Status",
                Width = 150
            });
        }


        private async void bupload_Click(object sender, EventArgs e)
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

                for (int i = 0; i < filePaths.Count; i++)
                {
                    string filePath = filePaths[i];
                    string name = tname.Text;
                    string address = taddress.Text;

                    if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(name))
                    {
                        MessageBox.Show("Please fill in all fields and select a file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string mimeType = GetMimeType(filePath);
                    string destinationFolderId = GetDestinationFolderId(mimeType);
                    string originalFileName = System.IO.Path.GetFileName(filePath);
                    string newFileName = $"{name} - {originalFileName}";

                    // Update status in DataGridView (Invoke required for cross-thread access)
                    dgvFiles.Invoke((MethodInvoker)(() =>
                    {
                        if (i < dgvFiles.Rows.Count)
                            dgvFiles.Rows[i].Cells["Status"].Value = "Uploading";
                    }));

                    // Upload file to Google Drive
                    string fileId = await UploadFileToDrive(filePath, newFileName, destinationFolderId);

                    // Update status in DataGridView (Invoke required for cross-thread access)
                    dgvFiles.Invoke((MethodInvoker)(() =>
                    {
                        if (i < dgvFiles.Rows.Count)
                        {
                            if (fileId != null)
                                dgvFiles.Rows[i].Cells["Status"].Value = "Upload Done";
                            else
                                dgvFiles.Rows[i].Cells["Status"].Value = "Failed";
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

                        // Save data to MongoDB
                        var document = new BsonDocument
                {
                    { "Name", name },
                    { "Address", address },
                    { "FilePath", fileLink },
                };

                        await collection.InsertOneAsync(document);
                    }
                }

                // Clear form fields after successful upload
                tname.Invoke((MethodInvoker)(() => tname.Text = ""));
                taddress.Invoke((MethodInvoker)(() => taddress.Text = ""));
                filePaths.Clear();
                dgvFiles.Invoke((MethodInvoker)(() => dgvFiles.Rows.Clear()));

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



        private async Task<string> UploadFileToDrive(string filePath, string newFileName, string folderId)
        {
            try
            {
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = newFileName,
                    Parents = new List<string> { folderId }
                };

                FilesResource.CreateMediaUpload request;

                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    request = service.Files.Create(fileMetadata, stream, GetMimeType(filePath));
                    request.Fields = "id";

                    await request.UploadAsync();
                }

                return request.ResponseBody?.Id;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error uploading file to Google Drive: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnAddFiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileName in openFileDialog.FileNames)
                {
                    filePaths.Add(fileName);
                    dgvFiles.Rows.Add(System.IO.Path.GetFileName(fileName), "Pending");
                }
            }
        }
    }
}

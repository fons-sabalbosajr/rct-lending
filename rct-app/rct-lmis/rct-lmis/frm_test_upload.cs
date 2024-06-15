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
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace rct_lmis
{
    public partial class frm_test_upload : Form
    {

        private IMongoCollection<BsonDocument> collection;
        private static string[] Scopes = { DriveService.Scope.DriveFile };
        private static string ApplicationName = "rct-lmis";
        private DriveService service;

        public frm_test_upload()
        {
            InitializeComponent();
            InitializeMongoDB();
            InitializeGoogleDrive();
        }

        private void frm_test_upload_Load(object sender, EventArgs e)
        {

        }

        private void InitializeMongoDB()
        {
            var database = MongoDBConnection.Instance.Database;
            collection = database.GetCollection<BsonDocument>("test-data");
        }

        [Obsolete]
        private async void InitializeGoogleDrive()
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

        private async void bupload_Click(object sender, EventArgs e)
        {
            try
            {
                // Open file dialog to select file
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    // Initialize progress bar and label
                    pbloading.Value = 0;
                    lpercent.Text = "0%";

                    // Upload file to Google Drive
                    string fileId = await UploadFileToDrive(filePath);

                    // Save data to MongoDB
                    var document = new BsonDocument
                    {
                        { "Name", tname.Text },
                        { "Address", taddress.Text },
                        { "FilePath", fileId }
                    };

                    await collection.InsertOneAsync(document);

                    MessageBox.Show("Data and file uploaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error uploading file and saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Reset progress bar and label
                pbloading.Value = 0;
                lpercent.Text = "0%";
            }
        }

        private async Task<string> UploadFileToDrive(string filePath)
        {
            try
            {
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = Path.GetFileName(filePath),
                    Parents = new List<string> { "1kMd3QjEw95oJsMSAK9xwEf-I3_MKlMBj" } // Folder ID
                };

                FilesResource.CreateMediaUpload request;

                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    request = service.Files.Create(fileMetadata, stream, GetMimeType(filePath));
                    request.Fields = "id";

                    request.ProgressChanged += (IUploadProgress progress) =>
                    {
                        switch (progress.Status)
                        {
                            case UploadStatus.Uploading:
                                int progressPercentage = (int)((progress.BytesSent * 100) / stream.Length);
                                UpdateProgressBar(progressPercentage);
                                break;
                            case UploadStatus.Completed:
                                UpdateProgressBar(100);
                                break;
                            case UploadStatus.Failed:
                                MessageBox.Show($"Error uploading file to Google Drive: {progress.Exception.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                        }
                    };

                    await request.UploadAsync();
                }

                return request.ResponseBody.Id;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error uploading file to Google Drive: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void UpdateProgressBar(int progress)
        {
            BeginInvoke(new Action(() =>
            {
                pbloading.Value = progress;
                lpercent.Text = $"{progress}%";
            }));
        }

        private string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
            {
                mimeType = regKey.GetValue("Content Type").ToString();
            }
            return mimeType;
        }
    }
}

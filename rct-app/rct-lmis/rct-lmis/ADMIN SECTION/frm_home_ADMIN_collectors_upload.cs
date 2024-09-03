using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Identity.Client;
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

namespace rct_lmis.ADMIN_SECTION
{
    public partial class frm_home_ADMIN_collectors_upload : Form
    {

        public static string StoredAccountID { get; set; }

        public frm_home_ADMIN_collectors_upload()
        {
            InitializeComponent();
            InitializeDataGridView();
            InitializeGoogleDrive();
        }

        private List<string> filePaths = new List<string>();
        LoadingFunction load = new LoadingFunction();

        private static string DocsFolderId = "1L_dUl-Xdp3Unl10mGdApVuqZL-W_0YDd";
        private static string ImagesFolderId = "1pHCftymmkGcbEMLtwErESXcD2HteMqIK";

        private static string[] Scopes = { DriveService.Scope.DriveFile };
        private static string ApplicationName = "rct-lmis";
        private DriveService service;

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


        private void frm_home_ADMIN_collectors_upload_Load(object sender, EventArgs e)
        {
            laccountid.Text = StoredAccountID;
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
                    buploaddoc.Enabled = true;
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

        private async void buploaddoc_Click(object sender, EventArgs e)
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
                var filter = Builders<Application>.Filter.Eq("GeneratedIDNumber", laccountid.Text);
                var update = Builders<Application>.Update
                    .Set("docs", combinedFileNames)
                    .Set("doc_type", combinedFileTypes)
                    .Set("doc-link", combinedFileLinks);

                var database = MongoDBConnection.Instance.Database;
                var collection = database.GetCollection<Application>("loan_collectors");
                await collection.UpdateOneAsync(filter, update);

                // Clear form fields after successful upload
                filePaths.Clear();
                dgvuploads.Invoke((MethodInvoker)(() => dgvuploads.Rows.Clear()));

                // Update status label
                statusLabel.Text = "Upload completed!";
                statusLabel.ForeColor = Color.Green;

                MessageBox.Show(this, "Data and files uploaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bclear.Enabled = false;
                buploaddoc.Enabled = false;
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

        private void dgvuploads_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvuploads.ClearSelection();
        }
    }
}

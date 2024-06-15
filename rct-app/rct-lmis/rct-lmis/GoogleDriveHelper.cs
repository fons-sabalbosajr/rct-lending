using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Upload;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rct_lmis
{
    public static class GoogleDriveHelper
    {
        // Define the MIME type for different file types
        private static Dictionary<string, string> MimeTypes = new Dictionary<string, string>()
        {
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".png", "image/png" },
            { ".gif", "image/gif" },
            { ".pdf", "application/pdf" },
            { ".doc", "application/msword" },
            { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { ".txt", "text/plain" }
            // Add more MIME types as needed
        };

        // Uploads a file to Google Drive
        public static async Task<string> UploadFile(FileStream fileStream, string filePath, string folderId)
        {
            try
            {
                UserCredential credential;

                // Your client secret JSON file path obtained from Google Cloud Console
                string credPath = "path-to-your-client-secret-file.json";

                // Scopes for the Drive API
                string[] scopes = { DriveService.Scope.Drive };

                // Load credentials from JSON file
                using (var stream = new FileStream(credPath, FileMode.Open, FileAccess.Read))
                {
                    string credPathFromJson = "token.json";
                    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPathFromJson, true));
                }

                // Create Drive API service
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Your Application Name",
                });

                // File's metadata
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = Path.GetFileName(filePath),
                    Parents = new List<string> { folderId } // Folder ID where the file should be uploaded
                };

                // File's content
                string mimeType = GetMimeType(filePath);
                var streamUpload = new MemoryStream(System.IO.File.ReadAllBytes(filePath));
                FilesResource.CreateMediaUpload request;
                request = service.Files.Create(fileMetadata, streamUpload, mimeType);
                request.Fields = "id";
                request.SupportsTeamDrives = true;
                request.ProgressChanged += Request_ProgressChanged;
                request.ResponseReceived += Request_ResponseReceived;

                // Upload the file
                await request.UploadAsync();

                // Get the file ID
                var file = request.ResponseBody;

                return file.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to upload file to Google Drive: {ex.Message}");
            }
        }

        private static string GetMimeType(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            return MimeTypes.ContainsKey(extension) ? MimeTypes[extension] : "application/octet-stream";
        }

        private static void Request_ProgressChanged(Google.Apis.Upload.IUploadProgress progress)
        {
            Console.WriteLine($"Upload progress: {progress.Status}");
        }

        private static void Request_ResponseReceived(Google.Apis.Drive.v3.Data.File obj)
        {
            Console.WriteLine($"File ID: {obj.Id}");
        }
    }
}

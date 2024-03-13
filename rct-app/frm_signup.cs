using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace rct_app
{
    public partial class frm_signup : Form
    {
        private const string SpreadsheetId = "1PrdywMS1OvXOkqY_2SG7rVM_uvaHqUzlPEkqOgh4RjE";
        private const string SheetName = "sh_login";

        private readonly SheetsService _sheetsService;

        public frm_signup()
        {
            InitializeComponent();
            _sheetsService = InitializeSheetsService();
        }

        private SheetsService InitializeSheetsService()
        {
            GoogleCredential credential;
            using (var stream = new FileStream(@"D:\RCT_Lending_System\rct-lending\rct-app\pkgfiles\utility-range-354823-c23e2274c58a.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(new[] { SheetsService.Scope.Spreadsheets });
            }

            return new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "YourAppName",
            });
        }

        private Task<int> GetLastIdNumberFromSheet()
        {
            // Define the range for the ID column (assuming it's column A)
            string range = $"{SheetName}!A:A";

            // Request the values from the spreadsheet
            SpreadsheetsResource.ValuesResource.GetRequest request =
                _sheetsService.Spreadsheets.Values.Get(SpreadsheetId, range);

            // Execute the request
            ValueRange response = request.Execute();
            IList<IList<object>> values = response.Values;

            // Determine the last ID number
            int lastId = 0;
            if (values != null && values.Count > 0)
            {
                lastId = int.Parse(values.Last()[0].ToString());
            }

            return Task.FromResult(lastId);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            frm_login login = new frm_login();
            login.ShowDialog();

            this.Close();
        }

        private void frm_signup_Load(object sender, EventArgs e)
        {

        }

        private async void bsignup_ClickAsync(object sender, EventArgs e)
        {
            // Get the last ID number
            int lastId = await GetLastIdNumberFromSheet();

            // Increment the ID number
            int newId = lastId + 1;

            // Get user input (name, username, password)
            string name = tname.Text.Trim();
            string username = tuser.Text.Trim();
            string password = tpass.Text.Trim();

            // Add the new credential to the spreadsheet with the incremented ID number
            await AddUserToSheet(newId, name, username, password);

            // Show success message
            MessageBox.Show("Sign up successful!");

            // Clear input fields
            tname.Text = "";
            tuser.Text = "";
            tpass.Text = "";
        }
        private async Task AddUserToSheet(int newId, string name, string username, string password)
        {
            // Encrypt the password
            string encryptedPassword = EncryptPassword(password);

            // Prepare user data
            var values = new List<object> { newId, name, username, encryptedPassword };

            // Define the range to write to (assuming first empty row)
            string range = $"{SheetName}!A:A";

            // Prepare the request
            var valueRange = new ValueRange { Values = new List<IList<object>> { values } };
            SpreadsheetsResource.ValuesResource.AppendRequest appendRequest =
                _sheetsService.Spreadsheets.Values.Append(valueRange, SpreadsheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;

            // Execute the request
            await appendRequest.ExecuteAsync();
        }

        private string EncryptPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }
}

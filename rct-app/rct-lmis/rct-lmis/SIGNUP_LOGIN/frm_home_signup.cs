using rct_lmis.SIGNUP_LOGIN;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.IO;

namespace rct_lmis
{
    public partial class frm_home_signup : Form
    {
        public frm_home_signup()
        {
            InitializeComponent();
        }

        LoadingFunction load  = new LoadingFunction();

        private void frm_home_signup_Load(object sender, EventArgs e)
        {

        }

        private void pback_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Cancel sign-up?", "Cancel Sign-Up", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void pbeye_Click(object sender, EventArgs e)
        {
            tpassword.UseSystemPasswordChar = false;
        }

        private void pbeye_MouseDown(object sender, MouseEventArgs e)
        {
            tpassword.PasswordChar = (char)0;
        }

        private void pbeye_MouseUp(object sender, MouseEventArgs e)
        {
             tpassword.PasswordChar = '•';
        }

        private void beyeconfirm_MouseDown(object sender, MouseEventArgs e)
        {
            tcpass.PasswordChar = (char)0;
        }

        private void beyeconfirm_MouseUp(object sender, MouseEventArgs e)
        {
            tcpass.PasswordChar = '•';
        }

        [Obsolete]
        private async void bconfirmsignup_Click(object sender, EventArgs e)
        {
            string fullName = tfullname.Text;
            string email = temail.Text;
            string password = tpassword.Text;
            string confirmPassword = tcpass.Text;

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            await SaveUserData(fullName, email, password);
            //await SendSignupNotificationEmail(fullName, tusername.Text, email);

            load.Show(this);
            await Task.Delay(1000); // Use Task.Delay instead of Thread.Sleep for async methods
            load.Close();

            MessageBox.Show(this, "User signed up successfully.");

            tfullname.Clear();
            temail.Clear();
            tusername.Clear();
            tpassword.Clear();
            tcpass.Clear();

        }

        private async Task SaveUserData(string fullName, string email, string password) 
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<User>("usersign_up");

            var user = new User
            {
                FullName = fullName,
                Email = email,
                Password = password
            };

            await collection.InsertOneAsync(user);
        }

        [Obsolete]
        private async Task SendSignupNotificationEmail(string fullname, string username, string email)
        {
            try
            {
                string[] scopes = { "https://mail.google.com/" };
                UserCredential credential;

                // JSON content from credentials.json
                var jsonContent = @"
               {
                    ""installed"": {
                    ""client_id"": ""469204387808-le9sim3fsoql61v419gj7g3rkhddla79.apps.googleusercontent.com"",
                    ""project_id"": ""rct-lmis"",
                    ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
                    ""token_uri"": ""https://oauth2.googleapis.com/token"",
                    ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
                    ""client_secret"": ""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"",
                    ""redirect_uris"": [
                    ""http://localhost""
                    ]
                    }
                }";

                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonContent)))
                {
                    string credPath = "token.json";
                    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true));
                }

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(fullname, email));
                message.To.Add(new MailboxAddress("RCT-LMIS Admin", "lmisrct@gmail.com"));
                message.Subject = "RCT-LMIS User Sign Up for Approval";
                message.Body = new TextPart("plain")
                {
                    Text = $"User sign up to the system and waiting for Master Admin's Approval\n\n" +
                           $"Name: {fullname}\n" +
                           $"Username: {username}\n" +
                           $"Email Address: {email}"
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(new SaslMechanismOAuth2(credential.UserId, credential.Token.AccessToken));
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                MessageBox.Show("Sign up notification email sent successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending notification email: {ex.Message}");
            }
        }

    }
}

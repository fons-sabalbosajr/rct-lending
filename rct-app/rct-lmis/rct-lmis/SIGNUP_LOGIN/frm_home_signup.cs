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

        private async Task SendSignupNotificationEmail(string fullName, string username, string email, string password)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("RCT LMIS", "no-reply@rct-lmis.com"));
            message.To.Add(new MailboxAddress("Admin", "racatom.lmis@gmail.com"));
            message.Subject = "New User Registration Notification";

            // Create the email body
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
            <div style='font-family: Arial, sans-serif; color: #333;'>
                <h2 style='color: #333;'>New User Registration</h2>
                <p>Dear Admin,</p>
                <p>We are pleased to inform you that a new user has successfully registered on the <strong>RCT LMIS</strong> platform. Below are the details of the new registration:</p>
                
                <table style='width: 100%; border-collapse: collapse; margin-top: 20px;'>
                    <tr>
                        <td style='padding: 10px; border: 1px solid #ddd; background-color: #f9f9f9;'><strong>Full Name:</strong></td>
                        <td style='padding: 10px; border: 1px solid #ddd;'>{fullName}</td>
                    </tr>
                    <tr>
                        <td style='padding: 10px; border: 1px solid #ddd; background-color: #f9f9f9;'><strong>Username:</strong></td>
                        <td style='padding: 10px; border: 1px solid #ddd;'>{username}</td>
                    </tr>
                    <tr>
                        <td style='padding: 10px; border: 1px solid #ddd; background-color: #f9f9f9;'><strong>Email:</strong></td>
                        <td style='padding: 10px; border: 1px solid #ddd;'>{email}</td>
                    </tr>
                    <tr>
                        <td style='padding: 10px; border: 1px solid #ddd; background-color: #f9f9f9;'><strong>Password:</strong></td>
                        <td style='padding: 10px; border: 1px solid #ddd;'>{password}</td>
                    </tr>
                </table>
                
                <p style='margin-top: 20px;'>Please take the necessary steps to manage this new account as appropriate.</p>
                <p>Sincerely,</p>
                <p><strong>RCT LMIS Support Team</strong></p>
            </div>",

                TextBody = $@"
            New User Registration

            Dear Admin,

            We are pleased to inform you that a new user has successfully registered on the RCT LMIS platform. Below are the details of the new registration:

            Full Name: {fullName}
            Username: {username}
            Email: {email}
            Password: {password}

            Please take the necessary steps to manage this new account as appropriate.

            Sincerely,
            RCT LMIS Support Team
        "
            };

            message.Body = bodyBuilder.ToMessageBody();

            try
            {
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    // Set the SMTP server
                    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                    // Authenticate using your email and app password
                    client.Authenticate("racatom.lmis@gmail.com", "gngcbsarkosbifvq"); // Replace with your actual app password

                    await client.SendAsync(message);
                    client.Disconnect(true);
                }
                //MessageBox.Show("Sign-up notification email sent successfully.", "Email Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending email: {ex.Message}", "Email Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



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

            load.Show(this);
            await Task.Delay(1000);
            load.Close();

            await SaveUserData(fullName, email, password);
            await SendSignupNotificationEmail(fullName, tusername.Text, email, password);

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
    }
}

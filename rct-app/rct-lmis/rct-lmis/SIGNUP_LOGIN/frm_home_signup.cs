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
    }
}

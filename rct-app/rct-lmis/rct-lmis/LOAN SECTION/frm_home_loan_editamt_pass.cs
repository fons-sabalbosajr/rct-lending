using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rct_lmis.LOAN_SECTION
{
    public partial class frm_home_loan_editamt_pass : Form
    {
        public frm_home_loan_editamt_pass()
        {
            InitializeComponent();
        }

        private void bproceed_Click(object sender, EventArgs e)
        {
            // Get the database connection
            var database = MongoDBConnection.Instance.Database;
            var userCollection = database.GetCollection<BsonDocument>("user_accounts");

            // Fetch the current logged-in user based on the stored session
            string currentUser = UserSession.Instance.CurrentUser;

            var filter = Builders<BsonDocument>.Filter.Eq("Username", currentUser);
            var user = userCollection.Find(filter).FirstOrDefault();

            if (user != null)
            {
                // Compare the entered password with the stored password
                string storedPassword = user["Password"].AsString;
                if (tpass.Text == storedPassword)
                {
                    // Password is correct, return OK to the calling form
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    // Password is incorrect, show error
                    MessageBox.Show("Incorrect password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // User not found, show error
                MessageBox.Show("User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frm_home_loan_editamt_pass_Load(object sender, EventArgs e)
        {

        }

        private void chShow_CheckedChanged(object sender, EventArgs e)
        {
            if (chShow.Checked)
            {
                // Show the password as plain text
                tpass.UseSystemPasswordChar = false;
            }
            else
            {
                // Hide the password (replace it with '*')
                tpass.UseSystemPasswordChar = true;
                tpass.PasswordChar = '•';
            }
        }
    }
}

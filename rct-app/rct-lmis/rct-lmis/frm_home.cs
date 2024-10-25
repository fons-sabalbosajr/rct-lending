using CrystalDecisions.Shared;
using Guna.UI2.WinForms;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using rct_lmis.ADMIN_SECTION;
using rct_lmis.LOAN_SECTION;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace rct_lmis
{
    public partial class frm_home : Form
    {
        int pwidth;
        bool isShow;
        private Form currChildForm;
        private Guna2Button currentbtn;
        private Panel leftpanel;

        private string loggedInUsername;
        private string _username;
        private string userPosition;

        LoadingFunction load = new LoadingFunction();

        private void ShowVersion()
        {
            Version ver = (ApplicationDeployment.IsNetworkDeployed) ?
               ApplicationDeployment.CurrentDeployment.CurrentVersion :
               Assembly.GetExecutingAssembly().GetName().Version;
            grpupdates.Text = "What's New in " + "ver." + ver.Major + "." + ver.Minor + "." + ver.Build;
        }

        public frm_home(string username)
        {
            InitializeComponent();
            customUI();
            ShowVersion();
            leftpanel = new Panel();
            leftpanel.Size = new Size(5, 45);
            pleft.Controls.Add(leftpanel);

            ttime.Start();
            loggedInUsername = username;
            _username = username;

            ldate.Text = DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt");
        }

        #region "DISPLAY CUSTOM"
        private struct RGBColors
        {
            public static Color col = Color.FromArgb(84, 140, 168);
        }

        private void ActivateButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                btndisabled();
                currentbtn = (Guna2Button)senderBtn;
                currentbtn.BackColor = Color.White;
                currentbtn.ForeColor = Color.PaleTurquoise;
                currentbtn.TextAlign = HorizontalAlignment.Left;
              
                leftpanel.BackColor = color;
                leftpanel.Location = new Point(0, currentbtn.Location.Y);
                leftpanel.Visible = true;
                leftpanel.BringToFront();
            }
        }
        private void btndisabled()
        {
            if (currentbtn != null)
            {
                currentbtn.BackColor = Color.White;
                currentbtn.ForeColor = Color.White;
                currentbtn.TextAlign = HorizontalAlignment.Left;     
            }
        }
        #endregion

        #region "PANELS"
        private void customUI()
        {
           
        }
        private void hidepsub()
        {
           
        }
        private void showpsub(Panel psub)
        {
            if (psub.Visible == false)
            {
                hidepsub();
                psub.Visible = true;
            }
            else
            {
                psub.Visible = false;
            }
        }
        #endregion

        #region "SLIDE MENU"
        private void tslide_Tick(object sender, EventArgs e)
        {
            if (isShow)
            {
                pleft.Width = pleft.Width + 140;
                if (pleft.Width >= pwidth)
                {
                    tslide.Stop();
                    isShow = false;
                    bdashboard.Text = "Dashboard";
                    bloans.Text = "Loans";
                    bpayments.Text = "Payments";
                    bsafekeeping.Text = "Safekeeping";
                    bclient.Text = "Clients";
                    bcrbooks.Text = "Cash Receipt Books";
                    bdisburse.Text = "Disbursements";
                    bjentries.Text = "Journal Entries";
                    badmin.Text = "Administrator";
                    blogout.Text = "Logout";
                    this.Refresh();
                }
            }
            else
            {
                pleft.Width = pleft.Width - 10;
                if (pleft.Width <= 60)
                {
                    tslide.Stop();
                    isShow = true;
                    bdashboard.Text = "";
                    bloans.Text = "";
                    bpayments.Text = "";
                    bsafekeeping.Text = "";
                    bclient.Text = "";
                    bcrbooks.Text = "";
                    bdisburse.Text = "";
                    bjentries.Text = "";
                    badmin.Text = "";
                    blogout.Text = "";
                    this.Refresh();
                }
            }
        }
        #endregion

        #region "CHILD FORM"
        private void ChildForm(Form childForm)
        {
            if (currChildForm != null)
            {
                currChildForm.Close();
            }
            currChildForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            pbody.Controls.Add(childForm);
            pbody.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }
        #endregion


        private void Logout(string username)
        {
            try
            {
                var database = MongoDBConnection.Instance.Database;
                var loginStatusCollection = database.GetCollection<LoginStatus>("login-status");

                // Filter to find the latest login record for the user
                var filter = Builders<LoginStatus>.Filter.Eq(ls => ls.Username, username) & Builders<LoginStatus>.Filter.Eq(ls => ls.IsLoggedIn, true);
                var update = Builders<LoginStatus>.Update
                    .Set(ls => ls.IsLoggedIn, false)
                    .Set(ls => ls.LogoutTime, DateTime.UtcNow);

                // Update the latest login record to set logout time
                loginStatusCollection.UpdateOne(filter, update);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating logout time: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUserInfo(string username)
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<BsonDocument>("user_accounts"); // 'user_accounts' is the name of your collection

            var filter = Builders<BsonDocument>.Filter.Eq("Username", username);
            var user = collection.Find(filter).FirstOrDefault();

            if (user != null)
            {


                // Get the full name and split to get the first name
                var fullName = user.GetValue("FullName").AsString;
                var firstName = fullName.Split(' ')[0]; // Split by space and take the first part

                // Set the first name
                lfname.Text = firstName;

                // Get user position
                userPosition = user.GetValue("Position").AsString;

                // Hide Administrator button if user is not an Administrator
                if (userPosition != "Administrator")
                {
                    badmin.Visible = false; // Optionally, make it invisible
                    bviewapplications.Visible = false;
                }

                // Load the photo
                if (user.Contains("Photo"))
                {
                    var photoData = user.GetValue("Photo").AsBsonBinaryData; // Assuming "Photo" is the field name storing the binary data of the photo

                    // Load the photo from binary data
                    using (var ms = new MemoryStream(photoData.Bytes))
                    {
                        pbphoto.Image = Image.FromStream(ms);
                    }
                }

                // Update the user session
                UserSession.Instance.CurrentUser = username;
            }
        }

        private void LoadTotalAnnouncements()
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<Announcement>("announcements");

            // Get the first day of the current month
            var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);

            // Debug output to console (for debugging purposes)
            Console.WriteLine($"First day of month: {firstDayOfMonth}");
            Console.WriteLine($"First day of next month: {firstDayOfNextMonth}");

            // Filter to count announcements within the current month
            var filter = Builders<Announcement>.Filter.And(
                Builders<Announcement>.Filter.Gte(a => a.PostedDate, firstDayOfMonth),
                Builders<Announcement>.Filter.Lt(a => a.PostedDate, firstDayOfNextMonth)
            );

            var totalAnnouncements = collection.CountDocuments(filter);

            // Debug output to console (for debugging purposes)
            Console.WriteLine($"Total announcements in the current month: {totalAnnouncements}");

            // Update the label in the UI thread
            this.Invoke((MethodInvoker)delegate
            {
                //Console.WriteLine("Updating UI...");
                if (totalAnnouncements > 0)
                {
                    lcountann.Text = totalAnnouncements.ToString();
                    lcountann.Visible = true; // Ensure the badge is visible if there are announcements
                    //Console.WriteLine("Badge should be visible");
                }
                else
                {
                    lcountann.Visible = false; // Hide the badge if there are no recent announcements
                    //Console.WriteLine("Badge should be hidden");
                }
            });
        }


        private void LoadDashboard()
        {
            ActivateButton(null, RGBColors.col);
            load.Show(this);
            Thread.Sleep(1000);

            frm_home_dashboard dashboardForm = new frm_home_dashboard(_username);

            ChildForm(dashboardForm);
            load.Close();
        }

        private void frm_home_Load(object sender, EventArgs e)
        {
            LoadUserInfo(loggedInUsername);
            LoadDashboard();
            LoadTotalAnnouncements();
        }

        private void bmenu_Click(object sender, EventArgs e)
        {
            tslide.Start();
        }

        private void frm_home_FormClosing(object sender, FormClosingEventArgs e)
        {
            load.Show(this);
            Thread.Sleep(500);
            Logout(loggedInUsername); // Call the Logout method
            load.Close();

            frm_home_login li = new frm_home_login();
            li.Show();
            this.Hide();
            e.Cancel = true;
        }

        private void bdashboard_Click(object sender, EventArgs e)
        {
            LoadDashboard();
        }

        private void bloans_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.col);
            load.Show(this);
            Thread.Sleep(1000);
            ChildForm(new frm_home_loans());
            load.Close();
        }

        private void bviewapplications_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.col);
            load.Show(this);
            Thread.Sleep(1000);
            ChildForm(new frm_home_loan_request());
            load.Close();
        }

        private void bpayments_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.col);
            load.Show(this);
            Thread.Sleep(1000);
            ChildForm(new frm_home_payments());
            load.Close();
        }

        private void bsafekeeping_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.col);
            load.Show(this);
            Thread.Sleep(1000);
            ChildForm(new frm_home_safekeeping());
            load.Close();
        }

        private void bclient_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.col);
            load.Show(this);
            Thread.Sleep(1000);
            ChildForm(new frm_home_clients());
            load.Close();
        }

        private void bcrbooks_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.col);
            load.Show(this);
            Thread.Sleep(1000);
            ChildForm(new frm_home_receiptbooks());
            load.Close();
        }

        private void bdisburse_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.col);
            load.Show(this);
            Thread.Sleep(1000);
            ChildForm(new frm_home_disburse());
            load.Close();
        }

        private void bjentries_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.col);
            load.Show(this);
            Thread.Sleep(1000);
            ChildForm(new frm_home_journals());
            load.Close();
        }

        private void badmin_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.col);
            load.Show(this);
            Thread.Sleep(1000);
            ChildForm(new frm_home_ADMIN());
            load.Close();
        }

        private void blogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to logout?", "Logout", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Logout(loggedInUsername); // Call the Logout method

                frm_home_login fl = new frm_home_login();

                load.Show(this);
                Thread.Sleep(1000);
                fl.Show(this);
                this.Hide();
                load.Close();
            }
        }

        private void ttime_Tick(object sender, EventArgs e)
        {
            ldate.Text = DateTime.Now.ToString("f");
        }
    }

    public class Announcement
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime PostedDate { get; set; }
    }
}

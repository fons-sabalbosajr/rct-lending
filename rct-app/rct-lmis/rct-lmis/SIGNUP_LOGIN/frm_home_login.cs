using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Deployment.Application;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace rct_lmis
{
    public partial class frm_home_login : Form
    {
        private void ShowVersion()
        {
            Version ver = (ApplicationDeployment.IsNetworkDeployed) ?
               ApplicationDeployment.CurrentDeployment.CurrentVersion :
               Assembly.GetExecutingAssembly().GetName().Version;

            lversion.Text = "ver." + ver.Major + "." + ver.Minor + "." + ver.Build;

            ///auto complete
            tuser.Focus();
            LoadUsernames();
        }

        public frm_home_login()
        {
            InitializeComponent();
            ShowVersion();
        }

        public static string empno = "";
        public static string username = "";
        public static string cip = "";
        public static string role = "";
        public static string profname = "";

        public int ID = 0;
        private Form currChildForm;

        public static string getusername = "";
        public static string getpassword = "";

        LoadingFunction load = new LoadingFunction();


        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int con, int val);

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void getIP()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    cip = "Active IP Address: " + ip.ToString();
                }
                lip.Text = cip;
            }
        }

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

            pright.Controls.Add(childForm);
            pright.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        void fadeIn(object sender, EventArgs e)
        {
            if (Opacity >= 1)
                tfade.Stop();
            else
                Opacity += 0.05;
        }

        private void LoadUsernames()
        {
            var database = MongoDBConnection.Instance.Database;
            var collection = database.GetCollection<BsonDocument>("user_accounts"); // 'users' is the name of your collection

            var distinctUsernames = collection.Distinct<string>("Username", Builders<BsonDocument>.Filter.Empty).ToList();

            var autoComplete = new AutoCompleteStringCollection();
            autoComplete.AddRange(distinctUsernames.ToArray());

            tuser.AutoCompleteCustomSource = autoComplete;
            tuser.AutoCompleteMode = AutoCompleteMode.Suggest;
            tuser.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }


        private void login()
        {
            string username = tuser.Text;
            string password = tpass.Text;

            bool loginSuccessful = false;

            if (string.IsNullOrEmpty(tuser.Text))
            {
                _ = MessageBox.Show("Please enter your username", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tuser.Clear();
                tuser.Focus();
            }
            else if (string.IsNullOrEmpty(tpass.Text))
            {
                _ = MessageBox.Show("Please enter your password", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tpass.Clear();
                tpass.Focus();
            }
            else
            {
                try
                {
                    var database = MongoDBConnection.Instance.Database;
                    var userCollection = database.GetCollection<BsonDocument>("user_accounts");
                    var loginStatusCollection = database.GetCollection<LoginStatus>("login-status");

                    var filter = Builders<BsonDocument>.Filter.Eq("Username", username) & Builders<BsonDocument>.Filter.Eq("Password", password);
                    var user = userCollection.Find(filter).FirstOrDefault();

                    if (user != null)
                    {
                        loginSuccessful = true;

                        // Set the current user in the session
                        UserSession.Instance.CurrentUser = username;


                        // Insert login status
                        var loginStatus = new LoginStatus
                        {
                            UserId = user["_id"].AsObjectId.ToString(), // Convert ObjectId to string
                            Name = user["FullName"].AsString,
                            Username = username,
                            IsLoggedIn = true,
                            LoginTime = DateTime.UtcNow
                        };
                        loginStatusCollection.InsertOne(loginStatus);

                        load.Show(this);
                        Thread.Sleep(1000);

                        // Pass the username to the home form
                        frm_home fh = new frm_home(username);
                        fh.Show();
                        load.Close();

                        this.Hide();
                    }
                    else
                    {
                        _ = MessageBox.Show("Invalid username or password", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show(ex.Message);
                }
            }
            if (!loginSuccessful)
            {
                // Handle unsuccessful login
                tpass.Clear();
                tpass.Focus();
            }
        }



        private void frm_home_login_Load(object sender, EventArgs e)
        {
            getIP();
            //loadusernames();
            this.Opacity = 0;
            tfade.Interval = 10;
            tfade.Tick += new EventHandler(fadeIn);
            tfade.Start();

            int Out;
            if (InternetGetConnectedState(out Out, 0) == true)
            {
                lstatus.Visible = false;
            }
            else
            {
                lstatus.Visible = true;
            }


        }

        private void pbclose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Quit Application?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Environment.Exit(0);
            }
        }

        private void tfade_Tick(object sender, EventArgs e)
        {

        }

        private void bsignup_Click(object sender, EventArgs e)
        {
            try
            {
                load.Show(this);
                Thread.Sleep(200);
                ChildForm(new frm_home_signup());
                load.Close();
            }
            catch (Exception)
            {
                _ = MessageBox.Show("Sign-up Window is not Available. Contact the Developer immediately.");
            }
        }

        private void pright_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void pbg_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void blogin_Click(object sender, EventArgs e)
        {
            login();
            //frm_home fh = new frm_home(username);
            //fh.Show(this);
        }

        private void pbeye_MouseDown(object sender, MouseEventArgs e)
        {
            tpass.PasswordChar = (char)0;
        }

        private void pbeye_MouseUp(object sender, MouseEventArgs e)
        {
            tpass.PasswordChar = '•';
        }

        private void pbeye_Click(object sender, EventArgs e)
        {
            tpass.PasswordChar = (char)0;
        }

        private void tpass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                blogin.PerformClick();
        }
    }
}

public class LoginStatus
{
    public ObjectId Id { get; set; }
    public string UserId { get; set; }
    public string Name { get; set; }
    public string Username { get; set; } // Added Username
    public bool IsLoggedIn { get; set; }
    public DateTime LoginTime { get; set; }
    public DateTime? LogoutTime { get; set; } // Changed to nullable to allow for null values
}

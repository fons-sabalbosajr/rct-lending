using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

            lversion.Text = ver.Major + "." + ver.Minor + "." + ver.Build;

            ///auto complete
            tuser.Focus();
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

        z_loadingform load = new z_loadingform();

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
                    cip = ip.ToString();
                }
                lip.Text = cip;
            }
        }

        void fadeIn(object sender, EventArgs e)
        {
            if (Opacity >= 1)
                tfade.Stop();
            else
                Opacity += 0.05;
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
                MessageBox.Show("Sign-up Window is not Available. Contact the Developer immediately.");
            }
        }
    }
}

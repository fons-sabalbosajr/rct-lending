using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rct_lmis
{
    public partial class frm_splashscreen : Form
    {
        public frm_splashscreen()
        {
            InitializeComponent();
        }

        frm_home_login fl = new frm_home_login();


        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int con, int val);

        void fadeIn(object sender, EventArgs e)
        {
            if (Opacity >= 1)
                tfade.Stop();
            else
                Opacity += 0.05;
        }

        void fadeOut(object sender, EventArgs e)
        {
            if (Opacity <= 0)
            {
                tfade.Stop();
                Close();
            }
            else
                Opacity -= 0.05;
        }

        private void frm_splashscreen_Load(object sender, EventArgs e)
        {
            try
            {
                int Out;
                if (InternetGetConnectedState(out Out, 0) == true)
                {
                    this.Opacity = 0;
                    tfade.Interval = 10;
                    tfade.Tick += new EventHandler(fadeIn);
                    tfade.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to load. Connection has not established. Check your connection settings. Error(" + ex.Message + ")", 
                    "Connection Lost", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
        }

        private void tfade_Tick(object sender, EventArgs e)
        {

        }

        private void tslide_Tick(object sender, EventArgs e)
        {
            ploading.Width += 3;
            if (ploading.Width >= 300)
            {
                try
                {
                    tslide.Stop();
                    fl.Show(this);
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void frm_splashscreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            tfade.Tick += new EventHandler(fadeOut);
            tfade.Start();

            if (Opacity == 0)
                e.Cancel = false;
        }

        private void frm_splashscreen_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}

using CrystalDecisions.Shared;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
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

        LoadingFunction load = new LoadingFunction();

        private void ShowVersion()
        {
            Version ver = (ApplicationDeployment.IsNetworkDeployed) ?
               ApplicationDeployment.CurrentDeployment.CurrentVersion :
               Assembly.GetExecutingAssembly().GetName().Version;
            grpupdates.Text = "What's New in " + "ver." + ver.Major + "." + ver.Minor + "." + ver.Build;
        }

        public frm_home()
        {
            InitializeComponent();
            customUI();
            ShowVersion();
            leftpanel = new Panel();
            leftpanel.Size = new Size(5, 45);
            pleft.Controls.Add(leftpanel);

            ldate.Text = DateTime.Now.ToString("f");
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
            psubutil.Visible = false;
        }
        private void hidepsub()
        {
            if (psubutil.Visible == true)
                psubutil.Visible = false;
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
                    butilities.Text = "Utilities";
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
                    butilities.Text = "";
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

        private void frm_home_Load(object sender, EventArgs e)
        {
            
        }

        private void butilities_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.col);
            showpsub(psubutil);
        }

        private void bmenu_Click(object sender, EventArgs e)
        {
            tslide.Start();
        }

        private void frm_home_FormClosing(object sender, FormClosingEventArgs e)
        {
            frm_home_login li = new frm_home_login();
            li.Show();
            this.Hide();
            e.Cancel = true;
        }

        private void bdashboard_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.col);
            load.Show(this);
            Thread.Sleep(1000);
            ChildForm(new frm_home_dashboard());
            load.Close();
        }

        private void bloans_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.col);
            load.Show(this);
            Thread.Sleep(1000);
            ChildForm(new frm_home_loans());
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
                frm_home_login fl = new frm_home_login();

                load.Show(this);
                Thread.Sleep(1000);
                fl.Show(this);
                this.Hide();
                load.Close();
            }
        }
    }
}

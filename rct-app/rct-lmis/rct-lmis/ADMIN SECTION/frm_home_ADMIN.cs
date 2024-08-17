using Guna.UI2.WinForms;
using rct_lmis.ADMIN_SECTION;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace rct_lmis
{
    public partial class frm_home_ADMIN : Form
    {

        int pwidth;
        bool isShow;
        private Form currChildForm;
        private Guna2Button currentbtn;
        private Panel leftpanel;


        public frm_home_ADMIN()
        {
            InitializeComponent();
            customUI();
            leftpanel = new Panel();
            leftpanel.Size = new Size(5, 45);
            pleft.Controls.Add(leftpanel);
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
           
            psubdata.Visible = false;
            psubacc.Visible = false;
        }
        private void hidepsub()
        {
            if (psubdata.Visible == true)
                psubdata.Visible = false;

            if (psubacc.Visible == true)
                psubacc.Visible = false;
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

        LoadingFunction load = new LoadingFunction();


        private void frm_home_ADMIN_Load(object sender, EventArgs e)
        {

        }

        private void baccounting_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.col);
            
        }

        private void bdatabase_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.col);
            showpsub(psubdata);
        }

        private void bconfigrate_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.col);
            load.Show(this);
            Thread.Sleep(1000);
            ChildForm(new frm_home_ADMIN_loanrates());
            load.Close();
        }

        private void bconfigcollector_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.col);
            showpsub(psubacc);
        }

        private void bsignupacc_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.col);
            load.Show(this);
            Thread.Sleep(1000);
            ChildForm(new frm_home_ADMIN_signupaccounts());
            load.Close();
        }

        private void buseraccounts_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.col);
            load.Show(this);
            Thread.Sleep(1000);
            ChildForm(new frm_home_ADMIN_useraccounts());
            load.Close();
        }

        private void bannouncement_Click(object sender, EventArgs e)
        {
            frm_ADMIN_annoucement  ann = new frm_ADMIN_annoucement();

            ActivateButton(sender, RGBColors.col);
            load.Show(this);
            Thread.Sleep(1000);
            load.Close();
            ann.ShowDialog();
            
        }

        private void bdatadelstaff_Click(object sender, EventArgs e)
        {
           
        }

        private void bapprovedloans_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.col);
            load.Show(this);
            Thread.Sleep(1000);
            ChildForm(new frm_home_ADMIN_aploans());
            load.Close();
        }

        private void bdeniedloans_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.col);
            load.Show(this);
            Thread.Sleep(1000);
            ChildForm(new frm_home_ADMIN_dnloans());
            load.Close();
        }
    }
}

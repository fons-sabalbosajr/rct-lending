﻿using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            psubaccounting.Visible = false;
            psubdata.Visible = false;
        }
        private void hidepsub()
        {
            if (psubaccounting.Visible == true)
                psubaccounting.Visible = false;

            if (psubdata.Visible == true)
                psubdata.Visible = false;
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


        private void frm_home_ADMIN_Load(object sender, EventArgs e)
        {

        }

        private void baccounting_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.col);
            showpsub(psubaccounting);
        }

        private void bdatabase_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.col);
            showpsub(psubdata);
        }
    }
}
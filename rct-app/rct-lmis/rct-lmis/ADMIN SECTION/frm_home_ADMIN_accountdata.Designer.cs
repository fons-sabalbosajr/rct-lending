namespace rct_lmis.ADMIN_SECTION
{
    partial class frm_home_ADMIN_accountdata
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bhelp = new Guna.UI2.WinForms.Guna2CircleButton();
            this.ltitle = new System.Windows.Forms.Label();
            this.pleft = new System.Windows.Forms.Panel();
            this.pbloading = new Guna.UI2.WinForms.Guna2ProgressBar();
            this.bupload = new Guna.UI2.WinForms.Guna2Button();
            this.bdel = new Guna.UI2.WinForms.Guna2Button();
            this.taccgrp = new Guna.UI2.WinForms.Guna2TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.taccname = new Guna.UI2.WinForms.Guna2TextBox();
            this.taccgrpcode = new Guna.UI2.WinForms.Guna2TextBox();
            this.tacccode = new Guna.UI2.WinForms.Guna2TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.bloansave = new Guna.UI2.WinForms.Guna2Button();
            this.taccno = new Guna.UI2.WinForms.Guna2TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.pnavtop = new System.Windows.Forms.Panel();
            this.bexport = new Guna.UI2.WinForms.Guna2Button();
            this.tsearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.lnorecord = new System.Windows.Forms.Label();
            this.dgvdata = new Guna.UI2.WinForms.Guna2DataGridView();
            this.badd = new Guna.UI2.WinForms.Guna2Button();
            this.panel1.SuspendLayout();
            this.pleft.SuspendLayout();
            this.pnavtop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvdata)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.panel1.Controls.Add(this.bhelp);
            this.panel1.Controls.Add(this.ltitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1200, 52);
            this.panel1.TabIndex = 2;
            // 
            // bhelp
            // 
            this.bhelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bhelp.BorderColor = System.Drawing.Color.LightGray;
            this.bhelp.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bhelp.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bhelp.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bhelp.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bhelp.FillColor = System.Drawing.Color.White;
            this.bhelp.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.bhelp.ForeColor = System.Drawing.Color.White;
            this.bhelp.Image = global::rct_lmis.Properties.Resources.icons8_help_48;
            this.bhelp.ImageSize = new System.Drawing.Size(28, 28);
            this.bhelp.Location = new System.Drawing.Point(1158, 11);
            this.bhelp.Name = "bhelp";
            this.bhelp.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.bhelp.Size = new System.Drawing.Size(30, 30);
            this.bhelp.TabIndex = 4;
            this.bhelp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.bhelp.TextOffset = new System.Drawing.Point(10, 0);
            this.bhelp.Click += new System.EventHandler(this.bhelp_Click);
            // 
            // ltitle
            // 
            this.ltitle.AutoSize = true;
            this.ltitle.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ltitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.ltitle.Location = new System.Drawing.Point(16, 11);
            this.ltitle.Name = "ltitle";
            this.ltitle.Size = new System.Drawing.Size(364, 30);
            this.ltitle.TabIndex = 2;
            this.ltitle.Text = "ACCOUNT TITLES CONFIGURATION";
            // 
            // pleft
            // 
            this.pleft.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pleft.Controls.Add(this.badd);
            this.pleft.Controls.Add(this.pbloading);
            this.pleft.Controls.Add(this.bupload);
            this.pleft.Controls.Add(this.bdel);
            this.pleft.Controls.Add(this.taccgrp);
            this.pleft.Controls.Add(this.label6);
            this.pleft.Controls.Add(this.taccname);
            this.pleft.Controls.Add(this.taccgrpcode);
            this.pleft.Controls.Add(this.tacccode);
            this.pleft.Controls.Add(this.label4);
            this.pleft.Controls.Add(this.label3);
            this.pleft.Controls.Add(this.label1);
            this.pleft.Controls.Add(this.bloansave);
            this.pleft.Controls.Add(this.taccno);
            this.pleft.Controls.Add(this.label34);
            this.pleft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pleft.Location = new System.Drawing.Point(0, 52);
            this.pleft.Name = "pleft";
            this.pleft.Size = new System.Drawing.Size(353, 648);
            this.pleft.TabIndex = 3;
            // 
            // pbloading
            // 
            this.pbloading.Location = new System.Drawing.Point(20, 310);
            this.pbloading.Name = "pbloading";
            this.pbloading.Size = new System.Drawing.Size(315, 10);
            this.pbloading.TabIndex = 140;
            this.pbloading.Text = "Loading";
            this.pbloading.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.pbloading.Visible = false;
            // 
            // bupload
            // 
            this.bupload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bupload.BorderRadius = 4;
            this.bupload.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bupload.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bupload.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bupload.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bupload.FillColor = System.Drawing.Color.SeaGreen;
            this.bupload.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bupload.ForeColor = System.Drawing.Color.White;
            this.bupload.Location = new System.Drawing.Point(249, 273);
            this.bupload.Name = "bupload";
            this.bupload.Size = new System.Drawing.Size(92, 31);
            this.bupload.TabIndex = 139;
            this.bupload.Text = "Upload CSV";
            this.bupload.Click += new System.EventHandler(this.bupload_Click);
            // 
            // bdel
            // 
            this.bdel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bdel.BorderRadius = 4;
            this.bdel.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bdel.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bdel.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bdel.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bdel.FillColor = System.Drawing.Color.Maroon;
            this.bdel.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bdel.ForeColor = System.Drawing.Color.White;
            this.bdel.Location = new System.Drawing.Point(178, 273);
            this.bdel.Name = "bdel";
            this.bdel.Size = new System.Drawing.Size(65, 31);
            this.bdel.TabIndex = 138;
            this.bdel.Text = "Delete";
            this.bdel.Click += new System.EventHandler(this.bdel_Click);
            // 
            // taccgrp
            // 
            this.taccgrp.AutoCompleteCustomSource.AddRange(new string[] {
            "Asset",
            "Liability",
            "Capital",
            "Income",
            "Expense"});
            this.taccgrp.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.taccgrp.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.taccgrp.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.taccgrp.DefaultText = "";
            this.taccgrp.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.taccgrp.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.taccgrp.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.taccgrp.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.taccgrp.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.taccgrp.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.taccgrp.ForeColor = System.Drawing.Color.DimGray;
            this.taccgrp.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.taccgrp.IconLeftSize = new System.Drawing.Size(15, 15);
            this.taccgrp.Location = new System.Drawing.Point(127, 63);
            this.taccgrp.Name = "taccgrp";
            this.taccgrp.PasswordChar = '\0';
            this.taccgrp.PlaceholderText = "--";
            this.taccgrp.SelectedText = "";
            this.taccgrp.Size = new System.Drawing.Size(191, 25);
            this.taccgrp.TabIndex = 137;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label6.Location = new System.Drawing.Point(17, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 15);
            this.label6.TabIndex = 136;
            this.label6.Text = "Account Group:";
            // 
            // taccname
            // 
            this.taccname.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.taccname.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.taccname.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.taccname.DefaultText = "";
            this.taccname.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.taccname.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.taccname.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.taccname.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.taccname.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.taccname.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.taccname.ForeColor = System.Drawing.Color.DimGray;
            this.taccname.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.taccname.IconLeftSize = new System.Drawing.Size(15, 15);
            this.taccname.Location = new System.Drawing.Point(148, 186);
            this.taccname.Multiline = true;
            this.taccname.Name = "taccname";
            this.taccname.PasswordChar = '\0';
            this.taccname.PlaceholderText = "--";
            this.taccname.SelectedText = "";
            this.taccname.Size = new System.Drawing.Size(188, 63);
            this.taccname.TabIndex = 135;
            // 
            // taccgrpcode
            // 
            this.taccgrpcode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.taccgrpcode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.taccgrpcode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.taccgrpcode.DefaultText = "";
            this.taccgrpcode.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.taccgrpcode.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.taccgrpcode.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.taccgrpcode.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.taccgrpcode.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.taccgrpcode.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.taccgrpcode.ForeColor = System.Drawing.Color.DimGray;
            this.taccgrpcode.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.taccgrpcode.IconLeftSize = new System.Drawing.Size(15, 15);
            this.taccgrpcode.Location = new System.Drawing.Point(148, 146);
            this.taccgrpcode.Name = "taccgrpcode";
            this.taccgrpcode.PasswordChar = '\0';
            this.taccgrpcode.PlaceholderText = "--";
            this.taccgrpcode.SelectedText = "";
            this.taccgrpcode.Size = new System.Drawing.Size(188, 25);
            this.taccgrpcode.TabIndex = 133;
            // 
            // tacccode
            // 
            this.tacccode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tacccode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tacccode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tacccode.DefaultText = "";
            this.tacccode.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tacccode.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tacccode.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tacccode.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tacccode.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tacccode.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tacccode.ForeColor = System.Drawing.Color.DimGray;
            this.tacccode.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tacccode.IconLeftSize = new System.Drawing.Size(15, 15);
            this.tacccode.Location = new System.Drawing.Point(148, 110);
            this.tacccode.Name = "tacccode";
            this.tacccode.PasswordChar = '\0';
            this.tacccode.PlaceholderText = "--";
            this.tacccode.SelectedText = "";
            this.tacccode.Size = new System.Drawing.Size(188, 25);
            this.tacccode.TabIndex = 132;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label4.Location = new System.Drawing.Point(17, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 15);
            this.label4.TabIndex = 130;
            this.label4.Text = "Account No.:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Location = new System.Drawing.Point(17, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 15);
            this.label3.TabIndex = 129;
            this.label3.Text = "Account Group Code:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(17, 186);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 15);
            this.label1.TabIndex = 128;
            this.label1.Text = "Account Name:";
            // 
            // bloansave
            // 
            this.bloansave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bloansave.BorderRadius = 4;
            this.bloansave.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bloansave.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bloansave.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bloansave.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bloansave.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.bloansave.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bloansave.ForeColor = System.Drawing.Color.White;
            this.bloansave.Location = new System.Drawing.Point(101, 273);
            this.bloansave.Name = "bloansave";
            this.bloansave.Size = new System.Drawing.Size(71, 31);
            this.bloansave.TabIndex = 127;
            this.bloansave.Text = "Update";
            this.bloansave.Click += new System.EventHandler(this.bloansave_Click);
            // 
            // taccno
            // 
            this.taccno.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.taccno.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.taccno.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.taccno.DefaultText = "";
            this.taccno.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.taccno.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.taccno.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.taccno.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.taccno.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.taccno.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.taccno.ForeColor = System.Drawing.Color.DimGray;
            this.taccno.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.taccno.IconLeftSize = new System.Drawing.Size(15, 15);
            this.taccno.Location = new System.Drawing.Point(127, 28);
            this.taccno.Name = "taccno";
            this.taccno.PasswordChar = '\0';
            this.taccno.PlaceholderText = "--";
            this.taccno.SelectedText = "";
            this.taccno.Size = new System.Drawing.Size(191, 25);
            this.taccno.TabIndex = 122;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.BackColor = System.Drawing.Color.Transparent;
            this.label34.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label34.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label34.Location = new System.Drawing.Point(17, 115);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(86, 15);
            this.label34.TabIndex = 77;
            this.label34.Text = "Account Code:";
            // 
            // pnavtop
            // 
            this.pnavtop.Controls.Add(this.bexport);
            this.pnavtop.Controls.Add(this.tsearch);
            this.pnavtop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnavtop.Location = new System.Drawing.Point(353, 52);
            this.pnavtop.Name = "pnavtop";
            this.pnavtop.Size = new System.Drawing.Size(847, 63);
            this.pnavtop.TabIndex = 4;
            // 
            // bexport
            // 
            this.bexport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bexport.BorderColor = System.Drawing.Color.Gainsboro;
            this.bexport.BorderRadius = 2;
            this.bexport.BorderThickness = 1;
            this.bexport.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bexport.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bexport.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bexport.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bexport.FillColor = System.Drawing.Color.White;
            this.bexport.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.bexport.ForeColor = System.Drawing.Color.White;
            this.bexport.Image = global::rct_lmis.Properties.Resources.icons8_export_excel_48;
            this.bexport.ImageSize = new System.Drawing.Size(28, 28);
            this.bexport.Location = new System.Drawing.Point(800, 13);
            this.bexport.Name = "bexport";
            this.bexport.Size = new System.Drawing.Size(35, 35);
            this.bexport.TabIndex = 13;
            this.bexport.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.bexport.Click += new System.EventHandler(this.bexport_Click);
            // 
            // tsearch
            // 
            this.tsearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tsearch.DefaultText = "";
            this.tsearch.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tsearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tsearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tsearch.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tsearch.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tsearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tsearch.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tsearch.IconLeft = global::rct_lmis.Properties.Resources.icons8_search_48;
            this.tsearch.Location = new System.Drawing.Point(14, 11);
            this.tsearch.Name = "tsearch";
            this.tsearch.PasswordChar = '\0';
            this.tsearch.PlaceholderText = "search any keyword...";
            this.tsearch.SelectedText = "";
            this.tsearch.Size = new System.Drawing.Size(378, 36);
            this.tsearch.TabIndex = 9;
            this.tsearch.TextChanged += new System.EventHandler(this.tsearch_TextChanged);
            // 
            // lnorecord
            // 
            this.lnorecord.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lnorecord.AutoSize = true;
            this.lnorecord.BackColor = System.Drawing.Color.White;
            this.lnorecord.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lnorecord.ForeColor = System.Drawing.Color.DimGray;
            this.lnorecord.Location = new System.Drawing.Point(721, 214);
            this.lnorecord.Name = "lnorecord";
            this.lnorecord.Size = new System.Drawing.Size(116, 20);
            this.lnorecord.TabIndex = 16;
            this.lnorecord.Text = "no record found";
            // 
            // dgvdata
            // 
            this.dgvdata.AllowUserToAddRows = false;
            this.dgvdata.AllowUserToDeleteRows = false;
            this.dgvdata.AllowUserToResizeColumns = false;
            this.dgvdata.AllowUserToResizeRows = false;
            dataGridViewCellStyle16.BackColor = System.Drawing.Color.White;
            this.dgvdata.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle16;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvdata.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle17;
            this.dgvdata.ColumnHeadersHeight = 30;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvdata.DefaultCellStyle = dataGridViewCellStyle18;
            this.dgvdata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvdata.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvdata.Location = new System.Drawing.Point(353, 115);
            this.dgvdata.Name = "dgvdata";
            this.dgvdata.ReadOnly = true;
            this.dgvdata.RowHeadersVisible = false;
            this.dgvdata.RowTemplate.Height = 30;
            this.dgvdata.Size = new System.Drawing.Size(847, 585);
            this.dgvdata.TabIndex = 15;
            this.dgvdata.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.White;
            this.dgvdata.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvdata.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvdata.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvdata.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvdata.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvdata.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvdata.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvdata.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.dgvdata.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvdata.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvdata.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvdata.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvdata.ThemeStyle.HeaderStyle.Height = 30;
            this.dgvdata.ThemeStyle.ReadOnly = true;
            this.dgvdata.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvdata.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvdata.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvdata.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvdata.ThemeStyle.RowsStyle.Height = 30;
            this.dgvdata.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            this.dgvdata.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvdata.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvdata_CellClick);
            this.dgvdata.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvdata_DataBindingComplete);
            // 
            // badd
            // 
            this.badd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.badd.BorderRadius = 4;
            this.badd.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.badd.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.badd.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.badd.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.badd.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.badd.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.badd.ForeColor = System.Drawing.Color.White;
            this.badd.Location = new System.Drawing.Point(20, 273);
            this.badd.Name = "badd";
            this.badd.Size = new System.Drawing.Size(75, 31);
            this.badd.TabIndex = 141;
            this.badd.Text = "Insert";
            this.badd.Click += new System.EventHandler(this.badd_Click);
            // 
            // frm_home_ADMIN_accountdata
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.lnorecord);
            this.Controls.Add(this.dgvdata);
            this.Controls.Add(this.pnavtop);
            this.Controls.Add(this.pleft);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_home_ADMIN_accountdata";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Account Titles";
            this.Load += new System.EventHandler(this.frm_home_ADMIN_accountdata_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pleft.ResumeLayout(false);
            this.pleft.PerformLayout();
            this.pnavtop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvdata)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Guna.UI2.WinForms.Guna2CircleButton bhelp;
        private System.Windows.Forms.Label ltitle;
        private System.Windows.Forms.Panel pleft;
        private Guna.UI2.WinForms.Guna2TextBox taccno;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Panel pnavtop;
        private Guna.UI2.WinForms.Guna2Button bexport;
        private Guna.UI2.WinForms.Guna2TextBox tsearch;
        private System.Windows.Forms.Label lnorecord;
        private Guna.UI2.WinForms.Guna2DataGridView dgvdata;
        private Guna.UI2.WinForms.Guna2TextBox taccname;
        private Guna.UI2.WinForms.Guna2TextBox taccgrpcode;
        private Guna.UI2.WinForms.Guna2TextBox tacccode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2Button bloansave;
        private System.Windows.Forms.Label label6;
        private Guna.UI2.WinForms.Guna2TextBox taccgrp;
        private Guna.UI2.WinForms.Guna2Button bdel;
        private Guna.UI2.WinForms.Guna2Button bupload;
        private Guna.UI2.WinForms.Guna2ProgressBar pbloading;
        private Guna.UI2.WinForms.Guna2Button badd;
    }
}
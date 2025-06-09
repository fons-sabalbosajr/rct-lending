namespace rct_lmis.DISBURSEMENT_SECTION
{
    partial class frm_home_disburse_details_edit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_home_disburse_details_edit));
            this.panel1 = new System.Windows.Forms.Panel();
            this.ldatestart = new System.Windows.Forms.Label();
            this.badd = new Guna.UI2.WinForms.Guna2Button();
            this.bsave = new Guna.UI2.WinForms.Guna2Button();
            this.laccountid = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.tloantype = new Guna.UI2.WinForms.Guna2TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tloanstatus = new Guna.UI2.WinForms.Guna2TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tloanterm = new Guna.UI2.WinForms.Guna2TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tloanbalance = new Guna.UI2.WinForms.Guna2TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tloaninterest = new Guna.UI2.WinForms.Guna2TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tloanamount = new Guna.UI2.WinForms.Guna2TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tloanprincipal = new Guna.UI2.WinForms.Guna2TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tloanpenalty = new Guna.UI2.WinForms.Guna2TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.dtstartdate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.dtmatdate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.cbcollector = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cbpaymentmode = new System.Windows.Forms.ComboBox();
            this.tloanid = new Guna.UI2.WinForms.Guna2TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tclientno = new Guna.UI2.WinForms.Guna2TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.AliceBlue;
            this.panel1.Controls.Add(this.ldatestart);
            this.panel1.Controls.Add(this.badd);
            this.panel1.Controls.Add(this.bsave);
            this.panel1.Controls.Add(this.laccountid);
            this.panel1.Controls.Add(this.label27);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(684, 50);
            this.panel1.TabIndex = 4;
            // 
            // ldatestart
            // 
            this.ldatestart.AutoSize = true;
            this.ldatestart.BackColor = System.Drawing.Color.Transparent;
            this.ldatestart.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ldatestart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ldatestart.Location = new System.Drawing.Point(330, 17);
            this.ldatestart.Name = "ldatestart";
            this.ldatestart.Size = new System.Drawing.Size(55, 15);
            this.ldatestart.TabIndex = 113;
            this.ldatestart.Text = "DateStart";
            this.ldatestart.Visible = false;
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
            this.badd.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.badd.ForeColor = System.Drawing.Color.White;
            this.badd.Location = new System.Drawing.Point(577, 10);
            this.badd.Name = "badd";
            this.badd.Size = new System.Drawing.Size(82, 31);
            this.badd.TabIndex = 38;
            this.badd.Text = "Add";
            this.badd.Click += new System.EventHandler(this.badd_Click);
            // 
            // bsave
            // 
            this.bsave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bsave.BorderRadius = 4;
            this.bsave.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bsave.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bsave.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bsave.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bsave.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.bsave.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bsave.ForeColor = System.Drawing.Color.White;
            this.bsave.Location = new System.Drawing.Point(577, 10);
            this.bsave.Name = "bsave";
            this.bsave.Size = new System.Drawing.Size(82, 31);
            this.bsave.TabIndex = 37;
            this.bsave.Text = "Update";
            this.bsave.Click += new System.EventHandler(this.bsave_Click);
            // 
            // laccountid
            // 
            this.laccountid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.laccountid.AutoSize = true;
            this.laccountid.BackColor = System.Drawing.Color.Transparent;
            this.laccountid.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.laccountid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.laccountid.Location = new System.Drawing.Point(71, 15);
            this.laccountid.Name = "laccountid";
            this.laccountid.Size = new System.Drawing.Size(121, 20);
            this.laccountid.TabIndex = 36;
            this.laccountid.Text = "RCT-2024-0001";
            // 
            // label27
            // 
            this.label27.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label27.AutoSize = true;
            this.label27.BackColor = System.Drawing.Color.Transparent;
            this.label27.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label27.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label27.Location = new System.Drawing.Point(12, 15);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(64, 19);
            this.label27.TabIndex = 35;
            this.label27.Text = "Loan No:";
            // 
            // tloantype
            // 
            this.tloantype.AutoCompleteCustomSource.AddRange(new string[] {
            "NEW",
            "RENEWAL"});
            this.tloantype.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tloantype.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tloantype.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tloantype.DefaultText = "";
            this.tloantype.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tloantype.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tloantype.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tloantype.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tloantype.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tloantype.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tloantype.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tloantype.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tloantype.Location = new System.Drawing.Point(386, 133);
            this.tloantype.Name = "tloantype";
            this.tloantype.PasswordChar = '\0';
            this.tloantype.PlaceholderText = "";
            this.tloantype.SelectedText = "";
            this.tloantype.Size = new System.Drawing.Size(174, 25);
            this.tloantype.TabIndex = 86;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label7.Location = new System.Drawing.Point(299, 138);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 15);
            this.label7.TabIndex = 85;
            this.label7.Text = "Loan Type:";
            // 
            // tloanstatus
            // 
            this.tloanstatus.AutoCompleteCustomSource.AddRange(new string[] {
            "UPDATED",
            "PAST DUE",
            "LITIGATION",
            "DORMANT"});
            this.tloanstatus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tloanstatus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tloanstatus.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tloanstatus.DefaultText = "";
            this.tloanstatus.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tloanstatus.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tloanstatus.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tloanstatus.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tloanstatus.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tloanstatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tloanstatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tloanstatus.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tloanstatus.Location = new System.Drawing.Point(386, 99);
            this.tloanstatus.Name = "tloanstatus";
            this.tloanstatus.PasswordChar = '\0';
            this.tloanstatus.PlaceholderText = "";
            this.tloanstatus.SelectedText = "";
            this.tloanstatus.Size = new System.Drawing.Size(174, 25);
            this.tloanstatus.TabIndex = 88;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(300, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 15);
            this.label1.TabIndex = 87;
            this.label1.Text = "Loan Status:";
            // 
            // tloanterm
            // 
            this.tloanterm.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tloanterm.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tloanterm.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tloanterm.DefaultText = "";
            this.tloanterm.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tloanterm.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tloanterm.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tloanterm.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tloanterm.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tloanterm.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tloanterm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tloanterm.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tloanterm.Location = new System.Drawing.Point(106, 99);
            this.tloanterm.Name = "tloanterm";
            this.tloanterm.PasswordChar = '\0';
            this.tloanterm.PlaceholderText = "";
            this.tloanterm.SelectedText = "";
            this.tloanterm.Size = new System.Drawing.Size(174, 25);
            this.tloanterm.TabIndex = 90;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(19, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 15);
            this.label2.TabIndex = 89;
            this.label2.Text = "Loan Term:";
            // 
            // tloanbalance
            // 
            this.tloanbalance.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tloanbalance.DefaultText = "";
            this.tloanbalance.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tloanbalance.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tloanbalance.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tloanbalance.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tloanbalance.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tloanbalance.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tloanbalance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tloanbalance.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tloanbalance.Location = new System.Drawing.Point(386, 167);
            this.tloanbalance.Name = "tloanbalance";
            this.tloanbalance.PasswordChar = '\0';
            this.tloanbalance.PlaceholderText = "";
            this.tloanbalance.SelectedText = "";
            this.tloanbalance.Size = new System.Drawing.Size(174, 25);
            this.tloanbalance.TabIndex = 92;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Location = new System.Drawing.Point(300, 172);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 15);
            this.label3.TabIndex = 91;
            this.label3.Text = "Loan Balance:";
            // 
            // tloaninterest
            // 
            this.tloaninterest.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tloaninterest.DefaultText = "";
            this.tloaninterest.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tloaninterest.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tloaninterest.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tloaninterest.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tloaninterest.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tloaninterest.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tloaninterest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tloaninterest.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tloaninterest.Location = new System.Drawing.Point(106, 203);
            this.tloaninterest.Name = "tloaninterest";
            this.tloaninterest.PasswordChar = '\0';
            this.tloaninterest.PlaceholderText = "";
            this.tloaninterest.SelectedText = "";
            this.tloaninterest.Size = new System.Drawing.Size(174, 25);
            this.tloaninterest.TabIndex = 94;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label4.Location = new System.Drawing.Point(20, 208);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 15);
            this.label4.TabIndex = 93;
            this.label4.Text = "Interest:";
            // 
            // tloanamount
            // 
            this.tloanamount.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tloanamount.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tloanamount.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tloanamount.DefaultText = "";
            this.tloanamount.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tloanamount.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tloanamount.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tloanamount.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tloanamount.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tloanamount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tloanamount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tloanamount.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tloanamount.Location = new System.Drawing.Point(106, 133);
            this.tloanamount.Name = "tloanamount";
            this.tloanamount.PasswordChar = '\0';
            this.tloanamount.PlaceholderText = "";
            this.tloanamount.SelectedText = "";
            this.tloanamount.Size = new System.Drawing.Size(174, 25);
            this.tloanamount.TabIndex = 96;
            this.tloanamount.TextChanged += new System.EventHandler(this.tloanamount_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label5.Location = new System.Drawing.Point(19, 138);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 15);
            this.label5.TabIndex = 95;
            this.label5.Text = "Loan Amount:";
            // 
            // tloanprincipal
            // 
            this.tloanprincipal.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tloanprincipal.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tloanprincipal.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tloanprincipal.DefaultText = "";
            this.tloanprincipal.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tloanprincipal.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tloanprincipal.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tloanprincipal.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tloanprincipal.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tloanprincipal.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tloanprincipal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tloanprincipal.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tloanprincipal.Location = new System.Drawing.Point(106, 167);
            this.tloanprincipal.Name = "tloanprincipal";
            this.tloanprincipal.PasswordChar = '\0';
            this.tloanprincipal.PlaceholderText = "";
            this.tloanprincipal.SelectedText = "";
            this.tloanprincipal.Size = new System.Drawing.Size(174, 25);
            this.tloanprincipal.TabIndex = 98;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label6.Location = new System.Drawing.Point(20, 172);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 15);
            this.label6.TabIndex = 97;
            this.label6.Text = "Principal:";
            // 
            // tloanpenalty
            // 
            this.tloanpenalty.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tloanpenalty.DefaultText = "";
            this.tloanpenalty.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tloanpenalty.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tloanpenalty.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tloanpenalty.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tloanpenalty.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tloanpenalty.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tloanpenalty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tloanpenalty.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tloanpenalty.Location = new System.Drawing.Point(106, 245);
            this.tloanpenalty.Name = "tloanpenalty";
            this.tloanpenalty.PasswordChar = '\0';
            this.tloanpenalty.PlaceholderText = "";
            this.tloanpenalty.SelectedText = "";
            this.tloanpenalty.Size = new System.Drawing.Size(174, 25);
            this.tloanpenalty.TabIndex = 100;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label8.Location = new System.Drawing.Point(19, 242);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 30);
            this.label8.TabIndex = 99;
            this.label8.Text = "Current\r\nPenalty:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label9.Location = new System.Drawing.Point(300, 208);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(111, 15);
            this.label9.TabIndex = 101;
            this.label9.Text = "Start Payment Date:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label10.Location = new System.Drawing.Point(300, 250);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(82, 15);
            this.label10.TabIndex = 102;
            this.label10.Text = "Maturity Date:";
            // 
            // dtstartdate
            // 
            this.dtstartdate.Checked = true;
            this.dtstartdate.FillColor = System.Drawing.Color.White;
            this.dtstartdate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtstartdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtstartdate.Location = new System.Drawing.Point(425, 203);
            this.dtstartdate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtstartdate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtstartdate.Name = "dtstartdate";
            this.dtstartdate.Size = new System.Drawing.Size(135, 25);
            this.dtstartdate.TabIndex = 103;
            this.dtstartdate.Value = new System.DateTime(2025, 2, 8, 15, 34, 35, 234);
            // 
            // dtmatdate
            // 
            this.dtmatdate.Checked = true;
            this.dtmatdate.FillColor = System.Drawing.Color.White;
            this.dtmatdate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtmatdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtmatdate.Location = new System.Drawing.Point(425, 245);
            this.dtmatdate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtmatdate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtmatdate.Name = "dtmatdate";
            this.dtmatdate.Size = new System.Drawing.Size(135, 25);
            this.dtmatdate.TabIndex = 104;
            this.dtmatdate.Value = new System.DateTime(2025, 2, 8, 15, 34, 35, 234);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label11.Location = new System.Drawing.Point(19, 287);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(63, 30);
            this.label11.TabIndex = 105;
            this.label11.Text = "Collector\r\nIncharged:";
            // 
            // cbcollector
            // 
            this.cbcollector.BackColor = System.Drawing.Color.Transparent;
            this.cbcollector.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbcollector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbcollector.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbcollector.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbcollector.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbcollector.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbcollector.ItemHeight = 30;
            this.cbcollector.Location = new System.Drawing.Point(106, 284);
            this.cbcollector.Name = "cbcollector";
            this.cbcollector.Size = new System.Drawing.Size(174, 36);
            this.cbcollector.TabIndex = 106;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label12.Location = new System.Drawing.Point(299, 295);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(91, 15);
            this.label12.TabIndex = 107;
            this.label12.Text = "Payment Mode:";
            // 
            // cbpaymentmode
            // 
            this.cbpaymentmode.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbpaymentmode.FormattingEnabled = true;
            this.cbpaymentmode.Items.AddRange(new object[] {
            "MONTHLY",
            "SEMI-MONTHLY",
            "WEEKLY",
            "DAILY"});
            this.cbpaymentmode.Location = new System.Drawing.Point(425, 291);
            this.cbpaymentmode.Name = "cbpaymentmode";
            this.cbpaymentmode.Size = new System.Drawing.Size(135, 23);
            this.cbpaymentmode.TabIndex = 108;
            // 
            // tloanid
            // 
            this.tloanid.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tloanid.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tloanid.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tloanid.DefaultText = "";
            this.tloanid.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tloanid.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tloanid.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tloanid.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tloanid.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tloanid.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tloanid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tloanid.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tloanid.Location = new System.Drawing.Point(106, 66);
            this.tloanid.Name = "tloanid";
            this.tloanid.PasswordChar = '\0';
            this.tloanid.PlaceholderText = "";
            this.tloanid.SelectedText = "";
            this.tloanid.Size = new System.Drawing.Size(174, 25);
            this.tloanid.TabIndex = 110;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label13.Location = new System.Drawing.Point(19, 70);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(50, 15);
            this.label13.TabIndex = 109;
            this.label13.Text = "Loan ID:";
            // 
            // tclientno
            // 
            this.tclientno.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tclientno.DefaultText = "";
            this.tclientno.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tclientno.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tclientno.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tclientno.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tclientno.Enabled = false;
            this.tclientno.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tclientno.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tclientno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tclientno.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tclientno.Location = new System.Drawing.Point(386, 68);
            this.tclientno.Name = "tclientno";
            this.tclientno.PasswordChar = '\0';
            this.tclientno.PlaceholderText = "";
            this.tclientno.SelectedText = "";
            this.tclientno.Size = new System.Drawing.Size(174, 25);
            this.tclientno.TabIndex = 112;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label14.Location = new System.Drawing.Point(299, 72);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(60, 15);
            this.label14.TabIndex = 111;
            this.label14.Text = "Client No:";
            // 
            // frm_home_disburse_details_edit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 361);
            this.Controls.Add(this.tclientno);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.tloanid);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.cbpaymentmode);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cbcollector);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.dtmatdate);
            this.Controls.Add(this.dtstartdate);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tloanpenalty);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tloanprincipal);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tloanamount);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tloaninterest);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tloanbalance);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tloanterm);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tloanstatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tloantype);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_home_disburse_details_edit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Loan Data";
            this.Load += new System.EventHandler(this.frm_home_disburse_details_edit_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label laccountid;
        private System.Windows.Forms.Label label27;
        private Guna.UI2.WinForms.Guna2Button bsave;
        private Guna.UI2.WinForms.Guna2TextBox tloantype;
        private System.Windows.Forms.Label label7;
        private Guna.UI2.WinForms.Guna2TextBox tloanstatus;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2TextBox tloanterm;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2TextBox tloanbalance;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2TextBox tloaninterest;
        private System.Windows.Forms.Label label4;
        private Guna.UI2.WinForms.Guna2TextBox tloanamount;
        private System.Windows.Forms.Label label5;
        private Guna.UI2.WinForms.Guna2TextBox tloanprincipal;
        private System.Windows.Forms.Label label6;
        private Guna.UI2.WinForms.Guna2TextBox tloanpenalty;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtstartdate;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtmatdate;
        private System.Windows.Forms.Label label11;
        private Guna.UI2.WinForms.Guna2ComboBox cbcollector;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cbpaymentmode;
        private Guna.UI2.WinForms.Guna2Button badd;
        private Guna.UI2.WinForms.Guna2TextBox tloanid;
        private System.Windows.Forms.Label label13;
        private Guna.UI2.WinForms.Guna2TextBox tclientno;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label ldatestart;
    }
}
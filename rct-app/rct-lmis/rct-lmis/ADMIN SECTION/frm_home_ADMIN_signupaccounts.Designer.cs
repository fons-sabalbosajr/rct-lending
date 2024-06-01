namespace rct_lmis.ADMIN_SECTION
{
    partial class frm_home_ADMIN_signupaccounts
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ltitle = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lnodata = new System.Windows.Forms.Label();
            this.dgvdata = new Guna.UI2.WinForms.Guna2DataGridView();
            this.pright = new System.Windows.Forms.Panel();
            this.grpactivate = new System.Windows.Forms.GroupBox();
            this.cbstaffpos = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.bupload = new Guna.UI2.WinForms.Guna2Button();
            this.label9 = new System.Windows.Forms.Label();
            this.tsystemID = new Guna.UI2.WinForms.Guna2TextBox();
            this.cbuserlevel = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tpassnew = new Guna.UI2.WinForms.Guna2TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tusernamenew = new Guna.UI2.WinForms.Guna2TextBox();
            this.bactivate = new Guna.UI2.WinForms.Guna2Button();
            this.bdeluser = new Guna.UI2.WinForms.Guna2Button();
            this.bapproveuser = new Guna.UI2.WinForms.Guna2Button();
            this.tpass = new Guna.UI2.WinForms.Guna2TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.temail = new Guna.UI2.WinForms.Guna2TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tname = new Guna.UI2.WinForms.Guna2TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tnamenew = new Guna.UI2.WinForms.Guna2TextBox();
            this.pbphoto = new System.Windows.Forms.PictureBox();
            this.bhelp = new Guna.UI2.WinForms.Guna2CircleButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvdata)).BeginInit();
            this.pright.SuspendLayout();
            this.grpactivate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbphoto)).BeginInit();
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
            this.panel1.Size = new System.Drawing.Size(1000, 52);
            this.panel1.TabIndex = 2;
            // 
            // ltitle
            // 
            this.ltitle.AutoSize = true;
            this.ltitle.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ltitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.ltitle.Location = new System.Drawing.Point(5, 11);
            this.ltitle.Name = "ltitle";
            this.ltitle.Size = new System.Drawing.Size(210, 30);
            this.ltitle.TabIndex = 2;
            this.ltitle.Text = "SIGNUP ACCOUNTS";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lnodata);
            this.panel2.Controls.Add(this.dgvdata);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 52);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(607, 648);
            this.panel2.TabIndex = 3;
            // 
            // lnodata
            // 
            this.lnodata.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lnodata.AutoSize = true;
            this.lnodata.BackColor = System.Drawing.Color.White;
            this.lnodata.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lnodata.ForeColor = System.Drawing.Color.DimGray;
            this.lnodata.Location = new System.Drawing.Point(246, 60);
            this.lnodata.Name = "lnodata";
            this.lnodata.Size = new System.Drawing.Size(108, 19);
            this.lnodata.TabIndex = 15;
            this.lnodata.Text = "no record found";
            // 
            // dgvdata
            // 
            this.dgvdata.AllowUserToAddRows = false;
            this.dgvdata.AllowUserToDeleteRows = false;
            this.dgvdata.AllowUserToResizeColumns = false;
            this.dgvdata.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvdata.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(96)))), ((int)(((byte)(114)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(96)))), ((int)(((byte)(114)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvdata.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvdata.ColumnHeadersHeight = 30;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvdata.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvdata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvdata.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvdata.Location = new System.Drawing.Point(0, 0);
            this.dgvdata.Name = "dgvdata";
            this.dgvdata.ReadOnly = true;
            this.dgvdata.RowHeadersVisible = false;
            this.dgvdata.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvdata.RowTemplate.Height = 40;
            this.dgvdata.Size = new System.Drawing.Size(607, 648);
            this.dgvdata.TabIndex = 5;
            this.dgvdata.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.White;
            this.dgvdata.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvdata.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvdata.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvdata.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvdata.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvdata.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvdata.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvdata.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(96)))), ((int)(((byte)(114)))));
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
            this.dgvdata.ThemeStyle.RowsStyle.Height = 40;
            this.dgvdata.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvdata.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvdata.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvdata_CellClick);
            this.dgvdata.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvdata_DataBindingComplete);
            // 
            // pright
            // 
            this.pright.Controls.Add(this.grpactivate);
            this.pright.Controls.Add(this.bdeluser);
            this.pright.Controls.Add(this.bapproveuser);
            this.pright.Controls.Add(this.tpass);
            this.pright.Controls.Add(this.label6);
            this.pright.Controls.Add(this.temail);
            this.pright.Controls.Add(this.label3);
            this.pright.Controls.Add(this.tname);
            this.pright.Controls.Add(this.label1);
            this.pright.Dock = System.Windows.Forms.DockStyle.Right;
            this.pright.Location = new System.Drawing.Point(607, 52);
            this.pright.Name = "pright";
            this.pright.Size = new System.Drawing.Size(393, 648);
            this.pright.TabIndex = 4;
            // 
            // grpactivate
            // 
            this.grpactivate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpactivate.Controls.Add(this.label2);
            this.grpactivate.Controls.Add(this.tnamenew);
            this.grpactivate.Controls.Add(this.cbstaffpos);
            this.grpactivate.Controls.Add(this.label10);
            this.grpactivate.Controls.Add(this.bupload);
            this.grpactivate.Controls.Add(this.pbphoto);
            this.grpactivate.Controls.Add(this.label9);
            this.grpactivate.Controls.Add(this.tsystemID);
            this.grpactivate.Controls.Add(this.cbuserlevel);
            this.grpactivate.Controls.Add(this.label8);
            this.grpactivate.Controls.Add(this.label5);
            this.grpactivate.Controls.Add(this.tpassnew);
            this.grpactivate.Controls.Add(this.label4);
            this.grpactivate.Controls.Add(this.tusernamenew);
            this.grpactivate.Controls.Add(this.bactivate);
            this.grpactivate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpactivate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.grpactivate.Location = new System.Drawing.Point(15, 198);
            this.grpactivate.Name = "grpactivate";
            this.grpactivate.Size = new System.Drawing.Size(352, 367);
            this.grpactivate.TabIndex = 110;
            this.grpactivate.TabStop = false;
            this.grpactivate.Text = "Account Activation";
            // 
            // cbstaffpos
            // 
            this.cbstaffpos.DropDownHeight = 120;
            this.cbstaffpos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbstaffpos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbstaffpos.FormattingEnabled = true;
            this.cbstaffpos.IntegralHeight = false;
            this.cbstaffpos.Items.AddRange(new object[] {
            "Staff",
            "Team Leader",
            "Supervisor",
            "Administrator",
            "Developer"});
            this.cbstaffpos.Location = new System.Drawing.Point(129, 287);
            this.cbstaffpos.Name = "cbstaffpos";
            this.cbstaffpos.Size = new System.Drawing.Size(133, 23);
            this.cbstaffpos.TabIndex = 122;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label10.Location = new System.Drawing.Point(7, 291);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 15);
            this.label10.TabIndex = 121;
            this.label10.Text = "Staff Position:";
            // 
            // bupload
            // 
            this.bupload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bupload.BorderRadius = 4;
            this.bupload.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bupload.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bupload.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bupload.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bupload.FillColor = System.Drawing.Color.SteelBlue;
            this.bupload.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bupload.ForeColor = System.Drawing.Color.White;
            this.bupload.Location = new System.Drawing.Point(129, 75);
            this.bupload.Name = "bupload";
            this.bupload.Size = new System.Drawing.Size(120, 29);
            this.bupload.TabIndex = 111;
            this.bupload.Text = "Upload Photo";
            this.bupload.Click += new System.EventHandler(this.bupload_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label9.Location = new System.Drawing.Point(7, 161);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 15);
            this.label9.TabIndex = 118;
            this.label9.Text = "System ID:";
            // 
            // tsystemID
            // 
            this.tsystemID.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tsystemID.DefaultText = "";
            this.tsystemID.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tsystemID.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tsystemID.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tsystemID.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tsystemID.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tsystemID.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tsystemID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tsystemID.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tsystemID.IconLeftSize = new System.Drawing.Size(18, 18);
            this.tsystemID.Location = new System.Drawing.Point(129, 156);
            this.tsystemID.Name = "tsystemID";
            this.tsystemID.PasswordChar = '\0';
            this.tsystemID.PlaceholderText = "";
            this.tsystemID.SelectedText = "";
            this.tsystemID.Size = new System.Drawing.Size(209, 25);
            this.tsystemID.TabIndex = 119;
            // 
            // cbuserlevel
            // 
            this.cbuserlevel.DropDownHeight = 120;
            this.cbuserlevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbuserlevel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbuserlevel.FormattingEnabled = true;
            this.cbuserlevel.IntegralHeight = false;
            this.cbuserlevel.Items.AddRange(new object[] {
            "Staff",
            "Team Leader",
            "Supervisor",
            "Administrator"});
            this.cbuserlevel.Location = new System.Drawing.Point(129, 122);
            this.cbuserlevel.Name = "cbuserlevel";
            this.cbuserlevel.Size = new System.Drawing.Size(133, 23);
            this.cbuserlevel.TabIndex = 117;
            this.cbuserlevel.SelectedIndexChanged += new System.EventHandler(this.cbuserlevel_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label8.Location = new System.Drawing.Point(7, 125);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 15);
            this.label8.TabIndex = 116;
            this.label8.Text = "User Hierarchy:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label5.Location = new System.Drawing.Point(7, 256);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 15);
            this.label5.TabIndex = 114;
            this.label5.Text = "Activated Password:";
            // 
            // tpassnew
            // 
            this.tpassnew.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tpassnew.DefaultText = "";
            this.tpassnew.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tpassnew.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tpassnew.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tpassnew.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tpassnew.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tpassnew.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tpassnew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tpassnew.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tpassnew.IconLeftSize = new System.Drawing.Size(18, 18);
            this.tpassnew.Location = new System.Drawing.Point(129, 251);
            this.tpassnew.Name = "tpassnew";
            this.tpassnew.PasswordChar = '\0';
            this.tpassnew.PlaceholderText = "";
            this.tpassnew.SelectedText = "";
            this.tpassnew.Size = new System.Drawing.Size(209, 25);
            this.tpassnew.TabIndex = 115;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label4.Location = new System.Drawing.Point(7, 223);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 15);
            this.label4.TabIndex = 112;
            this.label4.Text = "Activated Username:";
            // 
            // tusernamenew
            // 
            this.tusernamenew.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tusernamenew.DefaultText = "";
            this.tusernamenew.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tusernamenew.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tusernamenew.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tusernamenew.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tusernamenew.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tusernamenew.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tusernamenew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tusernamenew.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tusernamenew.IconLeftSize = new System.Drawing.Size(18, 18);
            this.tusernamenew.Location = new System.Drawing.Point(129, 218);
            this.tusernamenew.Name = "tusernamenew";
            this.tusernamenew.PasswordChar = '\0';
            this.tusernamenew.PlaceholderText = "";
            this.tusernamenew.SelectedText = "";
            this.tusernamenew.Size = new System.Drawing.Size(209, 25);
            this.tusernamenew.TabIndex = 113;
            // 
            // bactivate
            // 
            this.bactivate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bactivate.BorderRadius = 4;
            this.bactivate.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bactivate.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bactivate.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bactivate.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bactivate.FillColor = System.Drawing.Color.SeaGreen;
            this.bactivate.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bactivate.ForeColor = System.Drawing.Color.White;
            this.bactivate.Location = new System.Drawing.Point(234, 320);
            this.bactivate.Name = "bactivate";
            this.bactivate.Size = new System.Drawing.Size(102, 29);
            this.bactivate.TabIndex = 111;
            this.bactivate.Text = "Activate";
            this.bactivate.Click += new System.EventHandler(this.bactivate_Click);
            // 
            // bdeluser
            // 
            this.bdeluser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bdeluser.BorderRadius = 4;
            this.bdeluser.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bdeluser.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bdeluser.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bdeluser.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bdeluser.FillColor = System.Drawing.Color.Firebrick;
            this.bdeluser.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bdeluser.ForeColor = System.Drawing.Color.White;
            this.bdeluser.Location = new System.Drawing.Point(123, 136);
            this.bdeluser.Name = "bdeluser";
            this.bdeluser.Size = new System.Drawing.Size(102, 29);
            this.bdeluser.TabIndex = 109;
            this.bdeluser.Text = "Delete";
            this.bdeluser.Click += new System.EventHandler(this.bdeluser_Click);
            // 
            // bapproveuser
            // 
            this.bapproveuser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bapproveuser.BorderRadius = 4;
            this.bapproveuser.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bapproveuser.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bapproveuser.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bapproveuser.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bapproveuser.FillColor = System.Drawing.Color.SeaGreen;
            this.bapproveuser.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bapproveuser.ForeColor = System.Drawing.Color.White;
            this.bapproveuser.Location = new System.Drawing.Point(15, 136);
            this.bapproveuser.Name = "bapproveuser";
            this.bapproveuser.Size = new System.Drawing.Size(102, 29);
            this.bapproveuser.TabIndex = 108;
            this.bapproveuser.Text = "Approved";
            this.bapproveuser.Click += new System.EventHandler(this.bapproveuser_Click);
            // 
            // tpass
            // 
            this.tpass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tpass.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tpass.DefaultText = "";
            this.tpass.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tpass.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tpass.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tpass.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tpass.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tpass.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tpass.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tpass.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tpass.IconLeftSize = new System.Drawing.Size(18, 18);
            this.tpass.Location = new System.Drawing.Point(109, 87);
            this.tpass.Name = "tpass";
            this.tpass.PasswordChar = '\0';
            this.tpass.PlaceholderText = "";
            this.tpass.SelectedText = "";
            this.tpass.Size = new System.Drawing.Size(241, 25);
            this.tpass.TabIndex = 107;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label6.Location = new System.Drawing.Point(12, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 15);
            this.label6.TabIndex = 106;
            this.label6.Text = "Password:";
            // 
            // temail
            // 
            this.temail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.temail.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.temail.DefaultText = "";
            this.temail.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.temail.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.temail.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.temail.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.temail.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.temail.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.temail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.temail.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.temail.IconLeftSize = new System.Drawing.Size(18, 18);
            this.temail.Location = new System.Drawing.Point(109, 54);
            this.temail.Name = "temail";
            this.temail.PasswordChar = '\0';
            this.temail.PlaceholderText = "";
            this.temail.SelectedText = "";
            this.temail.Size = new System.Drawing.Size(241, 25);
            this.temail.TabIndex = 99;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Location = new System.Drawing.Point(12, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 15);
            this.label3.TabIndex = 98;
            this.label3.Text = "Email Address:";
            // 
            // tname
            // 
            this.tname.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tname.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tname.DefaultText = "";
            this.tname.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tname.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tname.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tname.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tname.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tname.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tname.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tname.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tname.IconLeftSize = new System.Drawing.Size(18, 18);
            this.tname.Location = new System.Drawing.Point(109, 23);
            this.tname.Name = "tname";
            this.tname.PasswordChar = '\0';
            this.tname.PlaceholderText = "";
            this.tname.SelectedText = "";
            this.tname.Size = new System.Drawing.Size(241, 25);
            this.tname.TabIndex = 97;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(12, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 15);
            this.label1.TabIndex = 96;
            this.label1.Text = "Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(7, 192);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 15);
            this.label2.TabIndex = 123;
            this.label2.Text = "Name:";
            // 
            // tnamenew
            // 
            this.tnamenew.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tnamenew.DefaultText = "";
            this.tnamenew.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tnamenew.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tnamenew.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tnamenew.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tnamenew.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tnamenew.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tnamenew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tnamenew.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tnamenew.IconLeftSize = new System.Drawing.Size(18, 18);
            this.tnamenew.Location = new System.Drawing.Point(129, 187);
            this.tnamenew.Name = "tnamenew";
            this.tnamenew.PasswordChar = '\0';
            this.tnamenew.PlaceholderText = "";
            this.tnamenew.SelectedText = "";
            this.tnamenew.Size = new System.Drawing.Size(209, 25);
            this.tnamenew.TabIndex = 124;
            // 
            // pbphoto
            // 
            this.pbphoto.BackColor = System.Drawing.Color.White;
            this.pbphoto.Location = new System.Drawing.Point(27, 29);
            this.pbphoto.Name = "pbphoto";
            this.pbphoto.Size = new System.Drawing.Size(75, 75);
            this.pbphoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbphoto.TabIndex = 16;
            this.pbphoto.TabStop = false;
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
            this.bhelp.Location = new System.Drawing.Point(958, 11);
            this.bhelp.Name = "bhelp";
            this.bhelp.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.bhelp.Size = new System.Drawing.Size(30, 30);
            this.bhelp.TabIndex = 4;
            this.bhelp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.bhelp.TextOffset = new System.Drawing.Point(10, 0);
            // 
            // frm_home_ADMIN_signupaccounts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pright);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_home_ADMIN_signupaccounts";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SignUp Accounts";
            this.Load += new System.EventHandler(this.frm_home_ADMIN_signupaccounts_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvdata)).EndInit();
            this.pright.ResumeLayout(false);
            this.pright.PerformLayout();
            this.grpactivate.ResumeLayout(false);
            this.grpactivate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbphoto)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Guna.UI2.WinForms.Guna2CircleButton bhelp;
        private System.Windows.Forms.Label ltitle;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pright;
        private Guna.UI2.WinForms.Guna2DataGridView dgvdata;
        private System.Windows.Forms.Label lnodata;
        private Guna.UI2.WinForms.Guna2TextBox temail;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2TextBox tname;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2TextBox tpass;
        private System.Windows.Forms.Label label6;
        private Guna.UI2.WinForms.Guna2Button bactivate;
        private System.Windows.Forms.GroupBox grpactivate;
        private Guna.UI2.WinForms.Guna2Button bdeluser;
        private Guna.UI2.WinForms.Guna2Button bapproveuser;
        private Guna.UI2.WinForms.Guna2TextBox tusernamenew;
        private System.Windows.Forms.Label label4;
        private Guna.UI2.WinForms.Guna2Button bupload;
        private System.Windows.Forms.PictureBox pbphoto;
        private System.Windows.Forms.Label label9;
        private Guna.UI2.WinForms.Guna2TextBox tsystemID;
        private System.Windows.Forms.ComboBox cbuserlevel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private Guna.UI2.WinForms.Guna2TextBox tpassnew;
        private System.Windows.Forms.ComboBox cbstaffpos;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2TextBox tnamenew;
    }
}
namespace rct_lmis.LOAN_SECTION
{
    partial class frm_home_loand_req_details
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_home_loand_req_details));
            this.panel1 = new System.Windows.Forms.Panel();
            this.laccno = new System.Windows.Forms.Label();
            this.bdeny = new Guna.UI2.WinForms.Guna2Button();
            this.bapproved = new Guna.UI2.WinForms.Guna2Button();
            this.dgvdisapproved = new Guna.UI2.WinForms.Guna2TabControl();
            this.tabGen = new System.Windows.Forms.TabPage();
            this.tfname = new Guna.UI2.WinForms.Guna2TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.taddress = new Guna.UI2.WinForms.Guna2TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tlaccountno = new Guna.UI2.WinForms.Guna2TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabDocs = new System.Windows.Forms.TabPage();
            this.dgvuploads = new Guna.UI2.WinForms.Guna2DataGridView();
            this.label24 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.dgvdisapproved.SuspendLayout();
            this.tabGen.SuspendLayout();
            this.tabDocs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvuploads)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.AliceBlue;
            this.panel1.Controls.Add(this.laccno);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1061, 50);
            this.panel1.TabIndex = 3;
            // 
            // laccno
            // 
            this.laccno.AutoSize = true;
            this.laccno.BackColor = System.Drawing.Color.Transparent;
            this.laccno.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.laccno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.laccno.Location = new System.Drawing.Point(12, 15);
            this.laccno.Name = "laccno";
            this.laccno.Size = new System.Drawing.Size(268, 20);
            this.laccno.TabIndex = 19;
            this.laccno.Text = " LOAN APPLICATION INFORMATION";
            // 
            // bdeny
            // 
            this.bdeny.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bdeny.BorderRadius = 4;
            this.bdeny.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bdeny.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bdeny.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bdeny.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bdeny.FillColor = System.Drawing.Color.Maroon;
            this.bdeny.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bdeny.ForeColor = System.Drawing.Color.White;
            this.bdeny.Location = new System.Drawing.Point(464, 184);
            this.bdeny.Name = "bdeny";
            this.bdeny.Size = new System.Drawing.Size(280, 31);
            this.bdeny.TabIndex = 32;
            this.bdeny.Text = "DENY";
            this.bdeny.Click += new System.EventHandler(this.bdeny_Click);
            // 
            // bapproved
            // 
            this.bapproved.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bapproved.BorderRadius = 4;
            this.bapproved.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bapproved.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bapproved.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bapproved.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bapproved.FillColor = System.Drawing.Color.SeaGreen;
            this.bapproved.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bapproved.ForeColor = System.Drawing.Color.White;
            this.bapproved.Location = new System.Drawing.Point(135, 184);
            this.bapproved.Name = "bapproved";
            this.bapproved.Size = new System.Drawing.Size(280, 31);
            this.bapproved.TabIndex = 31;
            this.bapproved.Text = "APPROVE";
            this.bapproved.Click += new System.EventHandler(this.bapproved_Click);
            // 
            // dgvdisapproved
            // 
            this.dgvdisapproved.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.dgvdisapproved.Controls.Add(this.tabGen);
            this.dgvdisapproved.Controls.Add(this.tabDocs);
            this.dgvdisapproved.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvdisapproved.ItemSize = new System.Drawing.Size(150, 30);
            this.dgvdisapproved.Location = new System.Drawing.Point(0, 50);
            this.dgvdisapproved.Name = "dgvdisapproved";
            this.dgvdisapproved.SelectedIndex = 0;
            this.dgvdisapproved.Size = new System.Drawing.Size(1061, 415);
            this.dgvdisapproved.TabButtonHoverState.BorderColor = System.Drawing.Color.Empty;
            this.dgvdisapproved.TabButtonHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(70)))));
            this.dgvdisapproved.TabButtonHoverState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.dgvdisapproved.TabButtonHoverState.ForeColor = System.Drawing.Color.White;
            this.dgvdisapproved.TabButtonHoverState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(70)))));
            this.dgvdisapproved.TabButtonIdleState.BorderColor = System.Drawing.Color.Empty;
            this.dgvdisapproved.TabButtonIdleState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            this.dgvdisapproved.TabButtonIdleState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.dgvdisapproved.TabButtonIdleState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(160)))), ((int)(((byte)(167)))));
            this.dgvdisapproved.TabButtonIdleState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            this.dgvdisapproved.TabButtonSelectedState.BorderColor = System.Drawing.Color.Empty;
            this.dgvdisapproved.TabButtonSelectedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(37)))), ((int)(((byte)(49)))));
            this.dgvdisapproved.TabButtonSelectedState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.dgvdisapproved.TabButtonSelectedState.ForeColor = System.Drawing.Color.White;
            this.dgvdisapproved.TabButtonSelectedState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(132)))), ((int)(((byte)(255)))));
            this.dgvdisapproved.TabButtonSize = new System.Drawing.Size(150, 30);
            this.dgvdisapproved.TabButtonTextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.dgvdisapproved.TabIndex = 4;
            this.dgvdisapproved.TabMenuBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            // 
            // tabGen
            // 
            this.tabGen.Controls.Add(this.bdeny);
            this.tabGen.Controls.Add(this.tfname);
            this.tabGen.Controls.Add(this.bapproved);
            this.tabGen.Controls.Add(this.label5);
            this.tabGen.Controls.Add(this.taddress);
            this.tabGen.Controls.Add(this.label3);
            this.tabGen.Controls.Add(this.tlaccountno);
            this.tabGen.Controls.Add(this.label1);
            this.tabGen.Location = new System.Drawing.Point(154, 4);
            this.tabGen.Name = "tabGen";
            this.tabGen.Padding = new System.Windows.Forms.Padding(3);
            this.tabGen.Size = new System.Drawing.Size(903, 407);
            this.tabGen.TabIndex = 0;
            this.tabGen.Text = "General";
            this.tabGen.UseVisualStyleBackColor = true;
            // 
            // tfname
            // 
            this.tfname.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tfname.DefaultText = "";
            this.tfname.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tfname.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tfname.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tfname.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tfname.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tfname.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tfname.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tfname.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tfname.Location = new System.Drawing.Point(135, 48);
            this.tfname.Name = "tfname";
            this.tfname.PasswordChar = '\0';
            this.tfname.PlaceholderText = "";
            this.tfname.SelectedText = "";
            this.tfname.Size = new System.Drawing.Size(690, 25);
            this.tfname.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label5.Location = new System.Drawing.Point(18, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 15);
            this.label5.TabIndex = 7;
            this.label5.Text = "Full Name:";
            // 
            // taddress
            // 
            this.taddress.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.taddress.DefaultText = "";
            this.taddress.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.taddress.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.taddress.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.taddress.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.taddress.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.taddress.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.taddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.taddress.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.taddress.Location = new System.Drawing.Point(135, 85);
            this.taddress.Multiline = true;
            this.taddress.Name = "taddress";
            this.taddress.PasswordChar = '\0';
            this.taddress.PlaceholderText = "";
            this.taddress.SelectedText = "";
            this.taddress.Size = new System.Drawing.Size(690, 76);
            this.taddress.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Location = new System.Drawing.Point(18, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "Address:";
            // 
            // tlaccountno
            // 
            this.tlaccountno.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tlaccountno.DefaultText = "";
            this.tlaccountno.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tlaccountno.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tlaccountno.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tlaccountno.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tlaccountno.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tlaccountno.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tlaccountno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tlaccountno.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tlaccountno.Location = new System.Drawing.Point(135, 14);
            this.tlaccountno.Name = "tlaccountno";
            this.tlaccountno.PasswordChar = '\0';
            this.tlaccountno.PlaceholderText = "";
            this.tlaccountno.SelectedText = "";
            this.tlaccountno.Size = new System.Drawing.Size(172, 25);
            this.tlaccountno.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(18, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Account Number:";
            // 
            // tabDocs
            // 
            this.tabDocs.Controls.Add(this.dgvuploads);
            this.tabDocs.Controls.Add(this.label24);
            this.tabDocs.Location = new System.Drawing.Point(154, 4);
            this.tabDocs.Name = "tabDocs";
            this.tabDocs.Padding = new System.Windows.Forms.Padding(3);
            this.tabDocs.Size = new System.Drawing.Size(903, 407);
            this.tabDocs.TabIndex = 1;
            this.tabDocs.Text = "Documents";
            this.tabDocs.UseVisualStyleBackColor = true;
            // 
            // dgvuploads
            // 
            this.dgvuploads.AllowUserToAddRows = false;
            this.dgvuploads.AllowUserToDeleteRows = false;
            this.dgvuploads.AllowUserToResizeColumns = false;
            this.dgvuploads.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvuploads.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvuploads.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dgvuploads.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvuploads.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvuploads.ColumnHeadersHeight = 30;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvuploads.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvuploads.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvuploads.Location = new System.Drawing.Point(8, 31);
            this.dgvuploads.Name = "dgvuploads";
            this.dgvuploads.ReadOnly = true;
            this.dgvuploads.RowHeadersVisible = false;
            this.dgvuploads.RowTemplate.Height = 50;
            this.dgvuploads.Size = new System.Drawing.Size(885, 329);
            this.dgvuploads.TabIndex = 70;
            this.dgvuploads.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.White;
            this.dgvuploads.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvuploads.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvuploads.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvuploads.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvuploads.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvuploads.ThemeStyle.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dgvuploads.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvuploads.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.dgvuploads.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvuploads.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvuploads.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvuploads.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvuploads.ThemeStyle.HeaderStyle.Height = 30;
            this.dgvuploads.ThemeStyle.ReadOnly = true;
            this.dgvuploads.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvuploads.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvuploads.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvuploads.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvuploads.ThemeStyle.RowsStyle.Height = 50;
            this.dgvuploads.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            this.dgvuploads.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvuploads.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvuploads_CellContentClick);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.BackColor = System.Drawing.Color.Transparent;
            this.label24.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label24.Location = new System.Drawing.Point(6, 10);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(141, 17);
            this.label24.TabIndex = 54;
            this.label24.Text = "Applicant Documents";
            // 
            // frm_home_loand_req_details
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1061, 465);
            this.Controls.Add(this.dgvdisapproved);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_home_loand_req_details";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Application Information";
            this.Load += new System.EventHandler(this.frm_home_loand_req_details_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.dgvdisapproved.ResumeLayout(false);
            this.tabGen.ResumeLayout(false);
            this.tabGen.PerformLayout();
            this.tabDocs.ResumeLayout(false);
            this.tabDocs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvuploads)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Guna.UI2.WinForms.Guna2Button bdeny;
        private Guna.UI2.WinForms.Guna2Button bapproved;
        private System.Windows.Forms.Label laccno;
        private Guna.UI2.WinForms.Guna2TabControl dgvdisapproved;
        private System.Windows.Forms.TabPage tabGen;
        private System.Windows.Forms.TabPage tabDocs;
        private Guna.UI2.WinForms.Guna2TextBox tlaccountno;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2TextBox taddress;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2TextBox tfname;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label24;
        private Guna.UI2.WinForms.Guna2DataGridView dgvuploads;
    }
}
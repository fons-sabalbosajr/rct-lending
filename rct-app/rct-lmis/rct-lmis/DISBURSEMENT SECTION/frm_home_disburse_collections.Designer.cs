namespace rct_lmis.DISBURSEMENT_SECTION
{
    partial class frm_home_disburse_collections
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_home_disburse_collections));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lclientno = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.laccountid = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.laccno = new System.Windows.Forms.Label();
            this.bpayadvance = new Guna.UI2.WinForms.Guna2Button();
            this.bnew = new Guna.UI2.WinForms.Guna2Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.bsoa = new Guna.UI2.WinForms.Guna2Button();
            this.bconfig = new Guna.UI2.WinForms.Guna2Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.cbloanno = new Guna.UI2.WinForms.Guna2ComboBox();
            this.dtdate = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.tsearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.lnorecord = new System.Windows.Forms.Label();
            this.dgvdata = new Guna.UI2.WinForms.Guna2DataGridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lgenbal = new System.Windows.Forms.Label();
            this.lpenaltytotal = new System.Windows.Forms.Label();
            this.ltotalamtpaid = new System.Windows.Forms.Label();
            this.ltotalpayments = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvdata)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.AliceBlue;
            this.panel1.Controls.Add(this.lclientno);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.laccountid);
            this.panel1.Controls.Add(this.label27);
            this.panel1.Controls.Add(this.laccno);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1184, 50);
            this.panel1.TabIndex = 4;
            // 
            // lclientno
            // 
            this.lclientno.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lclientno.AutoSize = true;
            this.lclientno.BackColor = System.Drawing.Color.Transparent;
            this.lclientno.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lclientno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lclientno.Location = new System.Drawing.Point(952, 16);
            this.lclientno.Name = "lclientno";
            this.lclientno.Size = new System.Drawing.Size(96, 20);
            this.lclientno.TabIndex = 38;
            this.lclientno.Text = "RCT-CL0000";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(561, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 19);
            this.label1.TabIndex = 37;
            this.label1.Text = "Client No.:";
            // 
            // laccountid
            // 
            this.laccountid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.laccountid.AutoSize = true;
            this.laccountid.BackColor = System.Drawing.Color.Transparent;
            this.laccountid.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.laccountid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.laccountid.Location = new System.Drawing.Point(632, 16);
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
            this.label27.Location = new System.Drawing.Point(863, 16);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(93, 19);
            this.label27.TabIndex = 35;
            this.label27.Text = "Disb. Ref. No.:";
            // 
            // laccno
            // 
            this.laccno.AutoSize = true;
            this.laccno.BackColor = System.Drawing.Color.Transparent;
            this.laccno.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.laccno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.laccno.Location = new System.Drawing.Point(12, 15);
            this.laccno.Name = "laccno";
            this.laccno.Size = new System.Drawing.Size(280, 20);
            this.laccno.TabIndex = 19;
            this.laccno.Text = "DISBURSEMENT COLLECTION DETAILS";
            // 
            // bpayadvance
            // 
            this.bpayadvance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bpayadvance.BorderRadius = 3;
            this.bpayadvance.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bpayadvance.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bpayadvance.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bpayadvance.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bpayadvance.FillColor = System.Drawing.Color.SteelBlue;
            this.bpayadvance.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bpayadvance.ForeColor = System.Drawing.Color.White;
            this.bpayadvance.Location = new System.Drawing.Point(928, 12);
            this.bpayadvance.Name = "bpayadvance";
            this.bpayadvance.Size = new System.Drawing.Size(108, 30);
            this.bpayadvance.TabIndex = 39;
            this.bpayadvance.Text = "Add Old Collections";
            this.toolTip1.SetToolTip(this.bpayadvance, "Generate Advance Payment");
            this.bpayadvance.Click += new System.EventHandler(this.bpayadvance_Click);
            // 
            // bnew
            // 
            this.bnew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bnew.BorderRadius = 3;
            this.bnew.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bnew.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bnew.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bnew.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bnew.FillColor = System.Drawing.Color.SeaGreen;
            this.bnew.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bnew.ForeColor = System.Drawing.Color.White;
            this.bnew.Location = new System.Drawing.Point(831, 12);
            this.bnew.Name = "bnew";
            this.bnew.Size = new System.Drawing.Size(88, 30);
            this.bnew.TabIndex = 31;
            this.bnew.Text = "New Collection";
            this.toolTip1.SetToolTip(this.bnew, "New Collection Transaction");
            this.bnew.Click += new System.EventHandler(this.bnew_Click);
            // 
            // bsoa
            // 
            this.bsoa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bsoa.BorderRadius = 4;
            this.bsoa.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bsoa.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bsoa.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bsoa.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bsoa.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.bsoa.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bsoa.ForeColor = System.Drawing.Color.White;
            this.bsoa.Location = new System.Drawing.Point(1042, 11);
            this.bsoa.Name = "bsoa";
            this.bsoa.Size = new System.Drawing.Size(93, 31);
            this.bsoa.TabIndex = 184;
            this.bsoa.Text = "Print SOA";
            this.toolTip1.SetToolTip(this.bsoa, "Print Statement of Account");
            this.bsoa.Click += new System.EventHandler(this.bsoa_Click);
            // 
            // bconfig
            // 
            this.bconfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bconfig.BorderRadius = 3;
            this.bconfig.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bconfig.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bconfig.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bconfig.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bconfig.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.bconfig.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bconfig.ForeColor = System.Drawing.Color.White;
            this.bconfig.Image = global::rct_lmis.Properties.Resources.icons8_utilities_60;
            this.bconfig.Location = new System.Drawing.Point(1141, 12);
            this.bconfig.Name = "bconfig";
            this.bconfig.Size = new System.Drawing.Size(31, 30);
            this.bconfig.TabIndex = 185;
            this.toolTip1.SetToolTip(this.bconfig, "Collection Configuration");
            this.bconfig.Click += new System.EventHandler(this.bconfig_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.cbloanno);
            this.panel2.Controls.Add(this.bconfig);
            this.panel2.Controls.Add(this.bpayadvance);
            this.panel2.Controls.Add(this.bsoa);
            this.panel2.Controls.Add(this.dtdate);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.bnew);
            this.panel2.Controls.Add(this.tsearch);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 50);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1184, 55);
            this.panel2.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Location = new System.Drawing.Point(523, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 30);
            this.label2.TabIndex = 187;
            this.label2.Text = "Select Loan\r\nCycle ID:";
            // 
            // cbloanno
            // 
            this.cbloanno.BackColor = System.Drawing.Color.Transparent;
            this.cbloanno.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbloanno.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbloanno.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbloanno.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbloanno.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.cbloanno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbloanno.ItemHeight = 30;
            this.cbloanno.Location = new System.Drawing.Point(593, 9);
            this.cbloanno.Name = "cbloanno";
            this.cbloanno.Size = new System.Drawing.Size(227, 36);
            this.cbloanno.TabIndex = 186;
            this.cbloanno.SelectedIndexChanged += new System.EventHandler(this.cbloanno_SelectedIndexChanged);
            // 
            // dtdate
            // 
            this.dtdate.Animated = true;
            this.dtdate.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dtdate.Checked = true;
            this.dtdate.FillColor = System.Drawing.Color.White;
            this.dtdate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtdate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dtdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtdate.Location = new System.Drawing.Point(330, 9);
            this.dtdate.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtdate.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtdate.Name = "dtdate";
            this.dtdate.Size = new System.Drawing.Size(148, 36);
            this.dtdate.TabIndex = 9;
            this.dtdate.Value = new System.DateTime(2024, 9, 17, 0, 0, 0, 0);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.DimGray;
            this.label11.Location = new System.Drawing.Point(250, 12);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(69, 30);
            this.label11.TabIndex = 10;
            this.label11.Text = "Search Date\r\nDisbursed:";
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
            this.tsearch.Location = new System.Drawing.Point(14, 10);
            this.tsearch.Name = "tsearch";
            this.tsearch.PasswordChar = '\0';
            this.tsearch.PlaceholderText = "search any keyword...";
            this.tsearch.SelectedText = "";
            this.tsearch.Size = new System.Drawing.Size(226, 36);
            this.tsearch.TabIndex = 8;
            this.tsearch.TextChanged += new System.EventHandler(this.tsearch_TextChanged);
            // 
            // lnorecord
            // 
            this.lnorecord.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lnorecord.AutoSize = true;
            this.lnorecord.BackColor = System.Drawing.Color.White;
            this.lnorecord.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnorecord.ForeColor = System.Drawing.Color.DarkGray;
            this.lnorecord.Location = new System.Drawing.Point(505, 160);
            this.lnorecord.Name = "lnorecord";
            this.lnorecord.Size = new System.Drawing.Size(177, 17);
            this.lnorecord.TabIndex = 13;
            this.lnorecord.Text = "no collection records found.";
            // 
            // dgvdata
            // 
            this.dgvdata.AllowUserToAddRows = false;
            this.dgvdata.AllowUserToDeleteRows = false;
            this.dgvdata.AllowUserToResizeColumns = false;
            this.dgvdata.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvdata.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvdata.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvdata.ColumnHeadersHeight = 30;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvdata.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvdata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvdata.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvdata.Location = new System.Drawing.Point(0, 105);
            this.dgvdata.Name = "dgvdata";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvdata.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvdata.RowHeadersVisible = false;
            this.dgvdata.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvdata.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvdata.RowTemplate.Height = 75;
            this.dgvdata.Size = new System.Drawing.Size(1184, 522);
            this.dgvdata.TabIndex = 12;
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
            this.dgvdata.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvdata.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvdata.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvdata.ThemeStyle.HeaderStyle.Height = 30;
            this.dgvdata.ThemeStyle.ReadOnly = false;
            this.dgvdata.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvdata.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvdata.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvdata.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvdata.ThemeStyle.RowsStyle.Height = 75;
            this.dgvdata.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.Silver;
            this.dgvdata.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvdata.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvdata_CellContentClick);
            this.dgvdata.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvdata_CellEndEdit);
            this.dgvdata.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvdata_CellFormatting);
            this.dgvdata.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvdata_DataBindingComplete);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel3.Controls.Add(this.lgenbal);
            this.panel3.Controls.Add(this.lpenaltytotal);
            this.panel3.Controls.Add(this.ltotalamtpaid);
            this.panel3.Controls.Add(this.ltotalpayments);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 627);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1184, 34);
            this.panel3.TabIndex = 14;
            // 
            // lgenbal
            // 
            this.lgenbal.AutoSize = true;
            this.lgenbal.BackColor = System.Drawing.Color.Transparent;
            this.lgenbal.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lgenbal.ForeColor = System.Drawing.Color.Black;
            this.lgenbal.Location = new System.Drawing.Point(459, 10);
            this.lgenbal.Name = "lgenbal";
            this.lgenbal.Size = new System.Drawing.Size(86, 13);
            this.lgenbal.TabIndex = 188;
            this.lgenbal.Text = "General Balance:";
            // 
            // lpenaltytotal
            // 
            this.lpenaltytotal.AutoSize = true;
            this.lpenaltytotal.BackColor = System.Drawing.Color.Transparent;
            this.lpenaltytotal.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lpenaltytotal.ForeColor = System.Drawing.Color.Black;
            this.lpenaltytotal.Location = new System.Drawing.Point(760, 10);
            this.lpenaltytotal.Name = "lpenaltytotal";
            this.lpenaltytotal.Size = new System.Drawing.Size(44, 13);
            this.lpenaltytotal.TabIndex = 187;
            this.lpenaltytotal.Text = "Penalty:";
            // 
            // ltotalamtpaid
            // 
            this.ltotalamtpaid.AutoSize = true;
            this.ltotalamtpaid.BackColor = System.Drawing.Color.Transparent;
            this.ltotalamtpaid.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ltotalamtpaid.ForeColor = System.Drawing.Color.Black;
            this.ltotalamtpaid.Location = new System.Drawing.Point(227, 10);
            this.ltotalamtpaid.Name = "ltotalamtpaid";
            this.ltotalamtpaid.Size = new System.Drawing.Size(99, 13);
            this.ltotalamtpaid.TabIndex = 186;
            this.ltotalamtpaid.Text = "Total Amount Paid:";
            // 
            // ltotalpayments
            // 
            this.ltotalpayments.AutoSize = true;
            this.ltotalpayments.BackColor = System.Drawing.Color.Transparent;
            this.ltotalpayments.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ltotalpayments.ForeColor = System.Drawing.Color.Black;
            this.ltotalpayments.Location = new System.Drawing.Point(12, 10);
            this.ltotalpayments.Name = "ltotalpayments";
            this.ltotalpayments.Size = new System.Drawing.Size(111, 13);
            this.ltotalpayments.TabIndex = 185;
            this.ltotalpayments.Text = "Total Payments Made:";
            // 
            // frm_home_disburse_collections
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 661);
            this.Controls.Add(this.dgvdata);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.lnorecord);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_home_disburse_collections";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "View Collections";
            this.Load += new System.EventHandler(this.frm_home_disburse_collections_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvdata)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label27;
        private Guna.UI2.WinForms.Guna2Button bnew;
        private System.Windows.Forms.Label laccno;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lnorecord;
        private Guna.UI2.WinForms.Guna2DataGridView dgvdata;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtdate;
        private System.Windows.Forms.Label label11;
        private Guna.UI2.WinForms.Guna2TextBox tsearch;
        private Guna.UI2.WinForms.Guna2Button bpayadvance;
        private Guna.UI2.WinForms.Guna2Button bsoa;
        private System.Windows.Forms.Label lclientno;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label ltotalpayments;
        private System.Windows.Forms.Label lgenbal;
        private System.Windows.Forms.Label lpenaltytotal;
        private System.Windows.Forms.Label ltotalamtpaid;
        public System.Windows.Forms.Label laccountid;
        private Guna.UI2.WinForms.Guna2Button bconfig;
        private Guna.UI2.WinForms.Guna2ComboBox cbloanno;
        private System.Windows.Forms.Label label2;
    }
}
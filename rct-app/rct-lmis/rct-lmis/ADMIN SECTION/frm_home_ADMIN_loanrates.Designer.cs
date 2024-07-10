namespace rct_lmis.ADMIN_SECTION
{
    partial class frm_home_ADMIN_loanrates
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
            this.bhelp = new Guna.UI2.WinForms.Guna2CircleButton();
            this.ltitle = new System.Windows.Forms.Label();
            this.pleft = new System.Windows.Forms.Panel();
            this.pbloading = new Guna.UI2.WinForms.Guna2ProgressBar();
            this.bloansave = new Guna.UI2.WinForms.Guna2Button();
            this.bloanclear = new Guna.UI2.WinForms.Guna2Button();
            this.label13 = new System.Windows.Forms.Label();
            this.cbmode = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cblterms = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cbloanamt = new Guna.UI2.WinForms.Guna2TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbldocfee = new Guna.UI2.WinForms.Guna2TextBox();
            this.cblmisfee = new Guna.UI2.WinForms.Guna2TextBox();
            this.cbvatfee = new Guna.UI2.WinForms.Guna2TextBox();
            this.cblannfee = new Guna.UI2.WinForms.Guna2TextBox();
            this.cblinfee = new Guna.UI2.WinForms.Guna2TextBox();
            this.cblnfee = new Guna.UI2.WinForms.Guna2TextBox();
            this.cblsfee = new Guna.UI2.WinForms.Guna2TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbltype = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label34 = new System.Windows.Forms.Label();
            this.beditrate = new Guna.UI2.WinForms.Guna2Button();
            this.buploadcsv = new Guna.UI2.WinForms.Guna2Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pnavtop = new System.Windows.Forms.Panel();
            this.bexport = new Guna.UI2.WinForms.Guna2Button();
            this.bprint = new Guna.UI2.WinForms.Guna2Button();
            this.cbsort = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tsearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.dgvdata = new Guna.UI2.WinForms.Guna2DataGridView();
            this.lnorecord = new System.Windows.Forms.Label();
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
            this.panel1.Size = new System.Drawing.Size(1300, 52);
            this.panel1.TabIndex = 1;
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
            this.bhelp.Location = new System.Drawing.Point(1258, 11);
            this.bhelp.Name = "bhelp";
            this.bhelp.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.bhelp.Size = new System.Drawing.Size(30, 30);
            this.bhelp.TabIndex = 4;
            this.bhelp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.bhelp.TextOffset = new System.Drawing.Point(10, 0);
            // 
            // ltitle
            // 
            this.ltitle.AutoSize = true;
            this.ltitle.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ltitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.ltitle.Location = new System.Drawing.Point(19, 11);
            this.ltitle.Name = "ltitle";
            this.ltitle.Size = new System.Drawing.Size(320, 30);
            this.ltitle.TabIndex = 2;
            this.ltitle.Text = "LOAN RATES CONFIGURATION";
            // 
            // pleft
            // 
            this.pleft.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pleft.Controls.Add(this.pbloading);
            this.pleft.Controls.Add(this.bloansave);
            this.pleft.Controls.Add(this.bloanclear);
            this.pleft.Controls.Add(this.label13);
            this.pleft.Controls.Add(this.cbmode);
            this.pleft.Controls.Add(this.label12);
            this.pleft.Controls.Add(this.cblterms);
            this.pleft.Controls.Add(this.cbloanamt);
            this.pleft.Controls.Add(this.label10);
            this.pleft.Controls.Add(this.cbldocfee);
            this.pleft.Controls.Add(this.cblmisfee);
            this.pleft.Controls.Add(this.cbvatfee);
            this.pleft.Controls.Add(this.cblannfee);
            this.pleft.Controls.Add(this.cblinfee);
            this.pleft.Controls.Add(this.cblnfee);
            this.pleft.Controls.Add(this.cblsfee);
            this.pleft.Controls.Add(this.label9);
            this.pleft.Controls.Add(this.label8);
            this.pleft.Controls.Add(this.label7);
            this.pleft.Controls.Add(this.label6);
            this.pleft.Controls.Add(this.label5);
            this.pleft.Controls.Add(this.label4);
            this.pleft.Controls.Add(this.label3);
            this.pleft.Controls.Add(this.cbltype);
            this.pleft.Controls.Add(this.label34);
            this.pleft.Controls.Add(this.beditrate);
            this.pleft.Controls.Add(this.buploadcsv);
            this.pleft.Controls.Add(this.label1);
            this.pleft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pleft.Location = new System.Drawing.Point(0, 52);
            this.pleft.Name = "pleft";
            this.pleft.Size = new System.Drawing.Size(353, 648);
            this.pleft.TabIndex = 2;
            // 
            // pbloading
            // 
            this.pbloading.Location = new System.Drawing.Point(18, 51);
            this.pbloading.Name = "pbloading";
            this.pbloading.Size = new System.Drawing.Size(315, 10);
            this.pbloading.TabIndex = 129;
            this.pbloading.Text = "Loading";
            this.pbloading.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.pbloading.Visible = false;
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
            this.bloansave.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bloansave.ForeColor = System.Drawing.Color.White;
            this.bloansave.Location = new System.Drawing.Point(105, 444);
            this.bloansave.Name = "bloansave";
            this.bloansave.Size = new System.Drawing.Size(75, 31);
            this.bloansave.TabIndex = 127;
            this.bloansave.Text = "Save";
            // 
            // bloanclear
            // 
            this.bloanclear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bloanclear.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.bloanclear.BorderRadius = 4;
            this.bloanclear.BorderThickness = 1;
            this.bloanclear.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bloanclear.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bloanclear.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bloanclear.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bloanclear.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.bloanclear.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bloanclear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.bloanclear.Location = new System.Drawing.Point(24, 444);
            this.bloanclear.Name = "bloanclear";
            this.bloanclear.Size = new System.Drawing.Size(75, 31);
            this.bloanclear.TabIndex = 128;
            this.bloanclear.Text = "Clear";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label13.Location = new System.Drawing.Point(26, 136);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(57, 30);
            this.label13.TabIndex = 126;
            this.label13.Text = "Mode of\r\nPayment:";
            // 
            // cbmode
            // 
            this.cbmode.BackColor = System.Drawing.Color.Transparent;
            this.cbmode.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbmode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbmode.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbmode.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbmode.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbmode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbmode.ItemHeight = 20;
            this.cbmode.Items.AddRange(new object[] {
            "DAILY",
            "WEEKLY",
            "BI-MONTHLY",
            "MONTHLY",
            "QUARTERLY",
            "ANNUAL"});
            this.cbmode.Location = new System.Drawing.Point(99, 138);
            this.cbmode.Name = "cbmode";
            this.cbmode.Size = new System.Drawing.Size(145, 26);
            this.cbmode.StartIndex = 0;
            this.cbmode.TabIndex = 125;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label12.Location = new System.Drawing.Point(26, 109);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 15);
            this.label12.TabIndex = 124;
            this.label12.Text = "Terms:";
            // 
            // cblterms
            // 
            this.cblterms.BackColor = System.Drawing.Color.Transparent;
            this.cblterms.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cblterms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cblterms.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cblterms.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cblterms.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cblterms.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cblterms.ItemHeight = 20;
            this.cblterms.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12"});
            this.cblterms.Location = new System.Drawing.Point(99, 103);
            this.cblterms.Name = "cblterms";
            this.cblterms.Size = new System.Drawing.Size(145, 26);
            this.cblterms.StartIndex = 0;
            this.cblterms.TabIndex = 123;
            // 
            // cbloanamt
            // 
            this.cbloanamt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.cbloanamt.DefaultText = "";
            this.cbloanamt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.cbloanamt.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.cbloanamt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.cbloanamt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.cbloanamt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbloanamt.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cbloanamt.ForeColor = System.Drawing.Color.DimGray;
            this.cbloanamt.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbloanamt.IconLeftSize = new System.Drawing.Size(15, 15);
            this.cbloanamt.Location = new System.Drawing.Point(132, 178);
            this.cbloanamt.Name = "cbloanamt";
            this.cbloanamt.PasswordChar = '\0';
            this.cbloanamt.PlaceholderText = "--";
            this.cbloanamt.SelectedText = "";
            this.cbloanamt.Size = new System.Drawing.Size(124, 25);
            this.cbloanamt.TabIndex = 122;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label10.Location = new System.Drawing.Point(26, 183);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(83, 15);
            this.label10.TabIndex = 121;
            this.label10.Text = "Loan Amount:";
            // 
            // cbldocfee
            // 
            this.cbldocfee.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.cbldocfee.DefaultText = "";
            this.cbldocfee.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.cbldocfee.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.cbldocfee.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.cbldocfee.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.cbldocfee.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbldocfee.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cbldocfee.ForeColor = System.Drawing.Color.DimGray;
            this.cbldocfee.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbldocfee.IconLeftSize = new System.Drawing.Size(15, 15);
            this.cbldocfee.Location = new System.Drawing.Point(132, 397);
            this.cbldocfee.Name = "cbldocfee";
            this.cbldocfee.PasswordChar = '\0';
            this.cbldocfee.PlaceholderText = "--";
            this.cbldocfee.SelectedText = "";
            this.cbldocfee.Size = new System.Drawing.Size(124, 25);
            this.cbldocfee.TabIndex = 120;
            // 
            // cblmisfee
            // 
            this.cblmisfee.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.cblmisfee.DefaultText = "";
            this.cblmisfee.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.cblmisfee.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.cblmisfee.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.cblmisfee.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.cblmisfee.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cblmisfee.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cblmisfee.ForeColor = System.Drawing.Color.DimGray;
            this.cblmisfee.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cblmisfee.IconLeftSize = new System.Drawing.Size(15, 15);
            this.cblmisfee.Location = new System.Drawing.Point(132, 366);
            this.cblmisfee.Name = "cblmisfee";
            this.cblmisfee.PasswordChar = '\0';
            this.cblmisfee.PlaceholderText = "--";
            this.cblmisfee.SelectedText = "";
            this.cblmisfee.Size = new System.Drawing.Size(124, 25);
            this.cblmisfee.TabIndex = 119;
            // 
            // cbvatfee
            // 
            this.cbvatfee.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.cbvatfee.DefaultText = "";
            this.cbvatfee.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.cbvatfee.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.cbvatfee.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.cbvatfee.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.cbvatfee.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbvatfee.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cbvatfee.ForeColor = System.Drawing.Color.DimGray;
            this.cbvatfee.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbvatfee.IconLeftSize = new System.Drawing.Size(15, 15);
            this.cbvatfee.Location = new System.Drawing.Point(132, 335);
            this.cbvatfee.Name = "cbvatfee";
            this.cbvatfee.PasswordChar = '\0';
            this.cbvatfee.PlaceholderText = "--";
            this.cbvatfee.SelectedText = "";
            this.cbvatfee.Size = new System.Drawing.Size(124, 25);
            this.cbvatfee.TabIndex = 118;
            // 
            // cblannfee
            // 
            this.cblannfee.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.cblannfee.DefaultText = "";
            this.cblannfee.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.cblannfee.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.cblannfee.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.cblannfee.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.cblannfee.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cblannfee.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cblannfee.ForeColor = System.Drawing.Color.DimGray;
            this.cblannfee.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cblannfee.IconLeftSize = new System.Drawing.Size(15, 15);
            this.cblannfee.Location = new System.Drawing.Point(132, 304);
            this.cblannfee.Name = "cblannfee";
            this.cblannfee.PasswordChar = '\0';
            this.cblannfee.PlaceholderText = "--";
            this.cblannfee.SelectedText = "";
            this.cblannfee.Size = new System.Drawing.Size(124, 25);
            this.cblannfee.TabIndex = 117;
            // 
            // cblinfee
            // 
            this.cblinfee.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.cblinfee.DefaultText = "";
            this.cblinfee.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.cblinfee.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.cblinfee.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.cblinfee.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.cblinfee.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cblinfee.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cblinfee.ForeColor = System.Drawing.Color.DimGray;
            this.cblinfee.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cblinfee.IconLeftSize = new System.Drawing.Size(15, 15);
            this.cblinfee.Location = new System.Drawing.Point(132, 273);
            this.cblinfee.Name = "cblinfee";
            this.cblinfee.PasswordChar = '\0';
            this.cblinfee.PlaceholderText = "--";
            this.cblinfee.SelectedText = "";
            this.cblinfee.Size = new System.Drawing.Size(124, 25);
            this.cblinfee.TabIndex = 116;
            // 
            // cblnfee
            // 
            this.cblnfee.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.cblnfee.DefaultText = "";
            this.cblnfee.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.cblnfee.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.cblnfee.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.cblnfee.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.cblnfee.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cblnfee.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cblnfee.ForeColor = System.Drawing.Color.DimGray;
            this.cblnfee.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cblnfee.IconLeftSize = new System.Drawing.Size(15, 15);
            this.cblnfee.Location = new System.Drawing.Point(132, 240);
            this.cblnfee.Name = "cblnfee";
            this.cblnfee.PasswordChar = '\0';
            this.cblnfee.PlaceholderText = "--";
            this.cblnfee.SelectedText = "";
            this.cblnfee.Size = new System.Drawing.Size(124, 25);
            this.cblnfee.TabIndex = 115;
            // 
            // cblsfee
            // 
            this.cblsfee.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.cblsfee.DefaultText = "";
            this.cblsfee.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.cblsfee.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.cblsfee.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.cblsfee.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.cblsfee.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cblsfee.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cblsfee.ForeColor = System.Drawing.Color.DimGray;
            this.cblsfee.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cblsfee.IconLeftSize = new System.Drawing.Size(15, 15);
            this.cblsfee.Location = new System.Drawing.Point(132, 209);
            this.cblsfee.Name = "cblsfee";
            this.cblsfee.PasswordChar = '\0';
            this.cblsfee.PlaceholderText = "--";
            this.cblsfee.SelectedText = "";
            this.cblsfee.Size = new System.Drawing.Size(124, 25);
            this.cblsfee.TabIndex = 114;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label9.Location = new System.Drawing.Point(26, 402);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 15);
            this.label9.TabIndex = 85;
            this.label9.Text = "Doc. Rate:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label8.Location = new System.Drawing.Point(26, 371);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 15);
            this.label8.TabIndex = 84;
            this.label8.Text = "Misc. Rate:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label7.Location = new System.Drawing.Point(26, 340);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 15);
            this.label7.TabIndex = 83;
            this.label7.Text = "VAT Rate:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label6.Location = new System.Drawing.Point(26, 309);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 15);
            this.label6.TabIndex = 82;
            this.label6.Text = "Annotation Rate:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label5.Location = new System.Drawing.Point(26, 278);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 15);
            this.label5.TabIndex = 81;
            this.label5.Text = "Insurace Rate:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label4.Location = new System.Drawing.Point(26, 245);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 15);
            this.label4.TabIndex = 80;
            this.label4.Text = "Notarial Fee:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Location = new System.Drawing.Point(26, 214);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 15);
            this.label3.TabIndex = 79;
            this.label3.Text = "Service Fee:";
            // 
            // cbltype
            // 
            this.cbltype.BackColor = System.Drawing.Color.Transparent;
            this.cbltype.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbltype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbltype.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbltype.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbltype.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbltype.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbltype.ItemHeight = 20;
            this.cbltype.Items.AddRange(new object[] {
            "Regular",
            "Discounted"});
            this.cbltype.Location = new System.Drawing.Point(99, 70);
            this.cbltype.Name = "cbltype";
            this.cbltype.Size = new System.Drawing.Size(145, 26);
            this.cbltype.StartIndex = 0;
            this.cbltype.TabIndex = 78;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.BackColor = System.Drawing.Color.Transparent;
            this.label34.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label34.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label34.Location = new System.Drawing.Point(26, 76);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(34, 15);
            this.label34.TabIndex = 77;
            this.label34.Text = "Type:";
            // 
            // beditrate
            // 
            this.beditrate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.beditrate.BorderRadius = 4;
            this.beditrate.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.beditrate.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.beditrate.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.beditrate.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.beditrate.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.beditrate.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.beditrate.ForeColor = System.Drawing.Color.White;
            this.beditrate.Location = new System.Drawing.Point(240, 11);
            this.beditrate.Name = "beditrate";
            this.beditrate.Size = new System.Drawing.Size(89, 31);
            this.beditrate.TabIndex = 33;
            this.beditrate.Text = "Edit Rates";
            // 
            // buploadcsv
            // 
            this.buploadcsv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buploadcsv.BorderRadius = 4;
            this.buploadcsv.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.buploadcsv.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.buploadcsv.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.buploadcsv.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.buploadcsv.FillColor = System.Drawing.Color.SeaGreen;
            this.buploadcsv.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buploadcsv.ForeColor = System.Drawing.Color.White;
            this.buploadcsv.Location = new System.Drawing.Point(125, 11);
            this.buploadcsv.Name = "buploadcsv";
            this.buploadcsv.Size = new System.Drawing.Size(109, 31);
            this.buploadcsv.TabIndex = 32;
            this.buploadcsv.Text = "Upload File";
            this.buploadcsv.Click += new System.EventHandler(this.buploadcsv_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.label1.Location = new System.Drawing.Point(18, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 21);
            this.label1.TabIndex = 5;
            this.label1.Text = "Rate Details";
            // 
            // pnavtop
            // 
            this.pnavtop.Controls.Add(this.bexport);
            this.pnavtop.Controls.Add(this.bprint);
            this.pnavtop.Controls.Add(this.cbsort);
            this.pnavtop.Controls.Add(this.label11);
            this.pnavtop.Controls.Add(this.tsearch);
            this.pnavtop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnavtop.Location = new System.Drawing.Point(353, 52);
            this.pnavtop.Name = "pnavtop";
            this.pnavtop.Size = new System.Drawing.Size(947, 63);
            this.pnavtop.TabIndex = 3;
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
            this.bexport.Location = new System.Drawing.Point(859, 13);
            this.bexport.Name = "bexport";
            this.bexport.Size = new System.Drawing.Size(35, 35);
            this.bexport.TabIndex = 13;
            this.bexport.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // bprint
            // 
            this.bprint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bprint.BorderColor = System.Drawing.Color.Gainsboro;
            this.bprint.BorderRadius = 2;
            this.bprint.BorderThickness = 1;
            this.bprint.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bprint.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bprint.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bprint.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bprint.FillColor = System.Drawing.Color.White;
            this.bprint.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.bprint.ForeColor = System.Drawing.Color.White;
            this.bprint.Image = global::rct_lmis.Properties.Resources.icons8_print_48;
            this.bprint.ImageSize = new System.Drawing.Size(28, 28);
            this.bprint.Location = new System.Drawing.Point(900, 13);
            this.bprint.Name = "bprint";
            this.bprint.Size = new System.Drawing.Size(35, 35);
            this.bprint.TabIndex = 12;
            this.bprint.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cbsort
            // 
            this.cbsort.BackColor = System.Drawing.Color.Transparent;
            this.cbsort.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbsort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbsort.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbsort.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbsort.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cbsort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbsort.ItemHeight = 30;
            this.cbsort.Items.AddRange(new object[] {
            "default",
            "name",
            "account no.",
            "top loan amount"});
            this.cbsort.Location = new System.Drawing.Point(472, 12);
            this.cbsort.Name = "cbsort";
            this.cbsort.Size = new System.Drawing.Size(140, 36);
            this.cbsort.StartIndex = 0;
            this.cbsort.TabIndex = 11;
            this.cbsort.TextOffset = new System.Drawing.Point(10, 0);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.DimGray;
            this.label11.Location = new System.Drawing.Point(405, 23);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(61, 15);
            this.label11.TabIndex = 10;
            this.label11.Text = "View/Sort:";
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
            this.tsearch.Location = new System.Drawing.Point(14, 12);
            this.tsearch.Name = "tsearch";
            this.tsearch.PasswordChar = '\0';
            this.tsearch.PlaceholderText = "search any keyword...";
            this.tsearch.SelectedText = "";
            this.tsearch.Size = new System.Drawing.Size(377, 36);
            this.tsearch.TabIndex = 9;
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
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvdata.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvdata.ColumnHeadersHeight = 25;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvdata.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvdata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvdata.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvdata.Location = new System.Drawing.Point(353, 115);
            this.dgvdata.Name = "dgvdata";
            this.dgvdata.ReadOnly = true;
            this.dgvdata.RowHeadersVisible = false;
            this.dgvdata.RowTemplate.Height = 20;
            this.dgvdata.Size = new System.Drawing.Size(947, 585);
            this.dgvdata.TabIndex = 4;
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
            this.dgvdata.ThemeStyle.HeaderStyle.Height = 25;
            this.dgvdata.ThemeStyle.ReadOnly = true;
            this.dgvdata.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvdata.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvdata.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvdata.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvdata.ThemeStyle.RowsStyle.Height = 20;
            this.dgvdata.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            this.dgvdata.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            // 
            // lnorecord
            // 
            this.lnorecord.AutoSize = true;
            this.lnorecord.BackColor = System.Drawing.Color.White;
            this.lnorecord.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lnorecord.ForeColor = System.Drawing.Color.DimGray;
            this.lnorecord.Location = new System.Drawing.Point(828, 203);
            this.lnorecord.Name = "lnorecord";
            this.lnorecord.Size = new System.Drawing.Size(116, 20);
            this.lnorecord.TabIndex = 14;
            this.lnorecord.Text = "no record found";
            // 
            // frm_home_ADMIN_loanrates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 700);
            this.Controls.Add(this.lnorecord);
            this.Controls.Add(this.dgvdata);
            this.Controls.Add(this.pnavtop);
            this.Controls.Add(this.pleft);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_home_ADMIN_loanrates";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Loan Rates Config";
            this.Load += new System.EventHandler(this.frm_home_ADMIN_loanrates_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pleft.ResumeLayout(false);
            this.pleft.PerformLayout();
            this.pnavtop.ResumeLayout(false);
            this.pnavtop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvdata)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label ltitle;
        private Guna.UI2.WinForms.Guna2CircleButton bhelp;
        private System.Windows.Forms.Panel pleft;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnavtop;
        private Guna.UI2.WinForms.Guna2ComboBox cbsort;
        private System.Windows.Forms.Label label11;
        private Guna.UI2.WinForms.Guna2TextBox tsearch;
        private Guna.UI2.WinForms.Guna2Button bexport;
        private Guna.UI2.WinForms.Guna2Button bprint;
        private Guna.UI2.WinForms.Guna2DataGridView dgvdata;
        private System.Windows.Forms.Label lnorecord;
        private Guna.UI2.WinForms.Guna2Button beditrate;
        private Guna.UI2.WinForms.Guna2Button buploadcsv;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2ComboBox cbltype;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private Guna.UI2.WinForms.Guna2TextBox cblsfee;
        private System.Windows.Forms.Label label12;
        private Guna.UI2.WinForms.Guna2ComboBox cblterms;
        private Guna.UI2.WinForms.Guna2TextBox cbloanamt;
        private System.Windows.Forms.Label label10;
        private Guna.UI2.WinForms.Guna2TextBox cbldocfee;
        private Guna.UI2.WinForms.Guna2TextBox cblmisfee;
        private Guna.UI2.WinForms.Guna2TextBox cbvatfee;
        private Guna.UI2.WinForms.Guna2TextBox cblannfee;
        private Guna.UI2.WinForms.Guna2TextBox cblinfee;
        private Guna.UI2.WinForms.Guna2TextBox cblnfee;
        private System.Windows.Forms.Label label13;
        private Guna.UI2.WinForms.Guna2ComboBox cbmode;
        private Guna.UI2.WinForms.Guna2Button bloansave;
        private Guna.UI2.WinForms.Guna2Button bloanclear;
        private Guna.UI2.WinForms.Guna2ProgressBar pbloading;
    }
}
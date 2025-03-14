namespace rct_lmis.ADMIN_SECTION
{
    partial class frm_home_ADMIN_collectconfig
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tsearchdata = new Guna.UI2.WinForms.Guna2TextBox();
            this.chkSelectAll = new Guna.UI2.WinForms.Guna2CheckBox();
            this.lpercent = new System.Windows.Forms.Label();
            this.bupdate = new Guna.UI2.WinForms.Guna2Button();
            this.pbloading = new System.Windows.Forms.ProgressBar();
            this.bupdateloanacc = new Guna.UI2.WinForms.Guna2Button();
            this.cbcollector = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.guna2VSeparator1 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.tsearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.lnorecord = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.bupload = new Guna.UI2.WinForms.Guna2Button();
            this.lcolcount = new System.Windows.Forms.Label();
            this.bclear = new Guna.UI2.WinForms.Guna2Button();
            this.bdessiminate = new Guna.UI2.WinForms.Guna2Button();
            this.lcountprocess = new System.Windows.Forms.Label();
            this.dgvdata = new Guna.UI2.WinForms.Guna2DataGridView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dgvloans = new Guna.UI2.WinForms.Guna2DataGridView();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvdata)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvloans)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel1.Controls.Add(this.tsearchdata);
            this.panel1.Controls.Add(this.chkSelectAll);
            this.panel1.Controls.Add(this.lpercent);
            this.panel1.Controls.Add(this.bupdate);
            this.panel1.Controls.Add(this.pbloading);
            this.panel1.Controls.Add(this.bupdateloanacc);
            this.panel1.Controls.Add(this.cbcollector);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.guna2VSeparator1);
            this.panel1.Controls.Add(this.tsearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 52);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1200, 52);
            this.panel1.TabIndex = 3;
            // 
            // tsearchdata
            // 
            this.tsearchdata.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tsearchdata.DefaultText = "";
            this.tsearchdata.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tsearchdata.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tsearchdata.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tsearchdata.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tsearchdata.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tsearchdata.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tsearchdata.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tsearchdata.IconLeft = global::rct_lmis.Properties.Resources.icons8_search_48;
            this.tsearchdata.Location = new System.Drawing.Point(8, 7);
            this.tsearchdata.Name = "tsearchdata";
            this.tsearchdata.PasswordChar = '\0';
            this.tsearchdata.PlaceholderText = "search name..";
            this.tsearchdata.SelectedText = "";
            this.tsearchdata.Size = new System.Drawing.Size(181, 36);
            this.tsearchdata.TabIndex = 172;
            this.tsearchdata.TextChanged += new System.EventHandler(this.tsearchdata_TextChanged);
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkSelectAll.CheckedState.BorderRadius = 0;
            this.chkSelectAll.CheckedState.BorderThickness = 0;
            this.chkSelectAll.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkSelectAll.Location = new System.Drawing.Point(395, 30);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(96, 17);
            this.chkSelectAll.TabIndex = 171;
            this.chkSelectAll.Text = "Select All Data";
            this.chkSelectAll.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.chkSelectAll.UncheckedState.BorderRadius = 0;
            this.chkSelectAll.UncheckedState.BorderThickness = 0;
            this.chkSelectAll.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // lpercent
            // 
            this.lpercent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lpercent.AutoSize = true;
            this.lpercent.BackColor = System.Drawing.Color.Transparent;
            this.lpercent.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lpercent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lpercent.Location = new System.Drawing.Point(901, 27);
            this.lpercent.Name = "lpercent";
            this.lpercent.Size = new System.Drawing.Size(23, 15);
            this.lpercent.TabIndex = 161;
            this.lpercent.Text = "0%";
            this.lpercent.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // bupdate
            // 
            this.bupdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bupdate.Animated = true;
            this.bupdate.BorderColor = System.Drawing.Color.Transparent;
            this.bupdate.BorderRadius = 2;
            this.bupdate.BorderThickness = 1;
            this.bupdate.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bupdate.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bupdate.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bupdate.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bupdate.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.bupdate.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bupdate.ForeColor = System.Drawing.Color.White;
            this.bupdate.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.bupdate.ImageSize = new System.Drawing.Size(15, 15);
            this.bupdate.Location = new System.Drawing.Point(1059, 10);
            this.bupdate.Name = "bupdate";
            this.bupdate.Size = new System.Drawing.Size(126, 33);
            this.bupdate.TabIndex = 170;
            this.bupdate.Text = "UPDATE TRANS NO.";
            this.bupdate.Click += new System.EventHandler(this.bupdate_Click);
            // 
            // pbloading
            // 
            this.pbloading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbloading.Location = new System.Drawing.Point(904, 10);
            this.pbloading.Name = "pbloading";
            this.pbloading.Size = new System.Drawing.Size(149, 15);
            this.pbloading.TabIndex = 165;
            // 
            // bupdateloanacc
            // 
            this.bupdateloanacc.Animated = true;
            this.bupdateloanacc.BorderColor = System.Drawing.Color.Transparent;
            this.bupdateloanacc.BorderRadius = 2;
            this.bupdateloanacc.BorderThickness = 1;
            this.bupdateloanacc.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bupdateloanacc.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bupdateloanacc.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bupdateloanacc.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bupdateloanacc.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.bupdateloanacc.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bupdateloanacc.ForeColor = System.Drawing.Color.White;
            this.bupdateloanacc.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.bupdateloanacc.ImageSize = new System.Drawing.Size(15, 15);
            this.bupdateloanacc.Location = new System.Drawing.Point(308, 9);
            this.bupdateloanacc.Name = "bupdateloanacc";
            this.bupdateloanacc.Size = new System.Drawing.Size(68, 33);
            this.bupdateloanacc.TabIndex = 169;
            this.bupdateloanacc.Text = "UPDATE";
            this.bupdateloanacc.Click += new System.EventHandler(this.bupdateloanacc_Click);
            // 
            // cbcollector
            // 
            this.cbcollector.BackColor = System.Drawing.Color.Transparent;
            this.cbcollector.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbcollector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbcollector.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbcollector.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbcollector.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.cbcollector.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbcollector.ItemHeight = 30;
            this.cbcollector.Location = new System.Drawing.Point(195, 7);
            this.cbcollector.Name = "cbcollector";
            this.cbcollector.Size = new System.Drawing.Size(108, 36);
            this.cbcollector.TabIndex = 166;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.label3.Location = new System.Drawing.Point(391, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(152, 21);
            this.label3.TabIndex = 168;
            this.label3.Text = "Preview Collections";
            // 
            // guna2VSeparator1
            // 
            this.guna2VSeparator1.Location = new System.Drawing.Point(375, 6);
            this.guna2VSeparator1.Name = "guna2VSeparator1";
            this.guna2VSeparator1.Size = new System.Drawing.Size(10, 36);
            this.guna2VSeparator1.TabIndex = 166;
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
            this.tsearch.Location = new System.Drawing.Point(568, 7);
            this.tsearch.Name = "tsearch";
            this.tsearch.PasswordChar = '\0';
            this.tsearch.PlaceholderText = "search any keyword...";
            this.tsearch.SelectedText = "";
            this.tsearch.Size = new System.Drawing.Size(287, 36);
            this.tsearch.TabIndex = 10;
            this.tsearch.TextChanged += new System.EventHandler(this.tsearch_TextChanged);
            // 
            // lnorecord
            // 
            this.lnorecord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnorecord.AutoSize = true;
            this.lnorecord.BackColor = System.Drawing.Color.Transparent;
            this.lnorecord.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnorecord.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lnorecord.Location = new System.Drawing.Point(725, 12);
            this.lnorecord.Name = "lnorecord";
            this.lnorecord.Size = new System.Drawing.Size(460, 30);
            this.lnorecord.TabIndex = 160;
            this.lnorecord.Text = "Easily upload a CSV file of loan collection records, preview the data in a DataGr" +
    "idView,\r\nand submit it to the database for accurate record-keeping.";
            this.lnorecord.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.lnorecord);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1200, 52);
            this.panel2.TabIndex = 161;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(320, 30);
            this.label1.TabIndex = 2;
            this.label1.Text = "COLLECTION CONFIGURATION";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel3.Controls.Add(this.bupload);
            this.panel3.Controls.Add(this.lcolcount);
            this.panel3.Controls.Add(this.bclear);
            this.panel3.Controls.Add(this.bdessiminate);
            this.panel3.Controls.Add(this.lcountprocess);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 752);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1200, 48);
            this.panel3.TabIndex = 162;
            // 
            // bupload
            // 
            this.bupload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bupload.Animated = true;
            this.bupload.BorderColor = System.Drawing.Color.Transparent;
            this.bupload.BorderRadius = 2;
            this.bupload.BorderThickness = 1;
            this.bupload.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bupload.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bupload.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bupload.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bupload.FillColor = System.Drawing.Color.SteelBlue;
            this.bupload.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bupload.ForeColor = System.Drawing.Color.White;
            this.bupload.Image = global::rct_lmis.Properties.Resources.icons8_add_48__2_;
            this.bupload.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.bupload.ImageSize = new System.Drawing.Size(15, 15);
            this.bupload.Location = new System.Drawing.Point(820, 7);
            this.bupload.Name = "bupload";
            this.bupload.Size = new System.Drawing.Size(140, 33);
            this.bupload.TabIndex = 159;
            this.bupload.Text = "Upload CSV File";
            this.bupload.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.bupload.Click += new System.EventHandler(this.bupload_Click);
            // 
            // lcolcount
            // 
            this.lcolcount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lcolcount.AutoSize = true;
            this.lcolcount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lcolcount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lcolcount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lcolcount.Location = new System.Drawing.Point(389, 25);
            this.lcolcount.Name = "lcolcount";
            this.lcolcount.Size = new System.Drawing.Size(125, 15);
            this.lcolcount.TabIndex = 165;
            this.lcolcount.Text = "Total Collection: 0 PHP";
            this.lcolcount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // bclear
            // 
            this.bclear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bclear.Animated = true;
            this.bclear.BorderColor = System.Drawing.Color.Transparent;
            this.bclear.BorderRadius = 2;
            this.bclear.BorderThickness = 1;
            this.bclear.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bclear.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bclear.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bclear.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bclear.FillColor = System.Drawing.Color.Brown;
            this.bclear.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bclear.ForeColor = System.Drawing.Color.White;
            this.bclear.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.bclear.ImageSize = new System.Drawing.Size(15, 15);
            this.bclear.Location = new System.Drawing.Point(1094, 7);
            this.bclear.Name = "bclear";
            this.bclear.Size = new System.Drawing.Size(86, 33);
            this.bclear.TabIndex = 164;
            this.bclear.Text = "CLEAR";
            this.bclear.Click += new System.EventHandler(this.bclear_Click);
            // 
            // bdessiminate
            // 
            this.bdessiminate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bdessiminate.Animated = true;
            this.bdessiminate.BorderColor = System.Drawing.Color.Transparent;
            this.bdessiminate.BorderRadius = 2;
            this.bdessiminate.BorderThickness = 1;
            this.bdessiminate.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bdessiminate.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bdessiminate.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bdessiminate.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bdessiminate.FillColor = System.Drawing.Color.SeaGreen;
            this.bdessiminate.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bdessiminate.ForeColor = System.Drawing.Color.White;
            this.bdessiminate.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.bdessiminate.ImageSize = new System.Drawing.Size(15, 15);
            this.bdessiminate.Location = new System.Drawing.Point(966, 7);
            this.bdessiminate.Name = "bdessiminate";
            this.bdessiminate.Size = new System.Drawing.Size(122, 33);
            this.bdessiminate.TabIndex = 160;
            this.bdessiminate.Text = "DESSIMINATE";
            this.bdessiminate.Click += new System.EventHandler(this.bdessiminate_Click);
            // 
            // lcountprocess
            // 
            this.lcountprocess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lcountprocess.AutoSize = true;
            this.lcountprocess.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lcountprocess.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lcountprocess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lcountprocess.Location = new System.Drawing.Point(389, 7);
            this.lcountprocess.Name = "lcountprocess";
            this.lcountprocess.Size = new System.Drawing.Size(142, 15);
            this.lcountprocess.TabIndex = 163;
            this.lcountprocess.Text = "Total Records Processed: 0";
            this.lcountprocess.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dgvdata
            // 
            this.dgvdata.AllowUserToAddRows = false;
            this.dgvdata.AllowUserToResizeRows = false;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.White;
            this.dgvdata.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle13;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvdata.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.dgvdata.ColumnHeadersHeight = 30;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvdata.DefaultCellStyle = dataGridViewCellStyle15;
            this.dgvdata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvdata.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvdata.Location = new System.Drawing.Point(382, 104);
            this.dgvdata.Name = "dgvdata";
            this.dgvdata.RowHeadersVisible = false;
            this.dgvdata.RowTemplate.Height = 30;
            this.dgvdata.Size = new System.Drawing.Size(818, 648);
            this.dgvdata.TabIndex = 163;
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
            this.dgvdata.ThemeStyle.ReadOnly = false;
            this.dgvdata.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvdata.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvdata.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvdata.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvdata.ThemeStyle.RowsStyle.Height = 30;
            this.dgvdata.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.Silver;
            this.dgvdata.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvdata.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvdata_CellValueChanged);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(377, 104);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(5, 648);
            this.panel4.TabIndex = 164;
            // 
            // dgvloans
            // 
            this.dgvloans.AllowUserToAddRows = false;
            this.dgvloans.AllowUserToResizeRows = false;
            dataGridViewCellStyle16.BackColor = System.Drawing.Color.White;
            this.dgvloans.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle16;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvloans.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle17;
            this.dgvloans.ColumnHeadersHeight = 30;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvloans.DefaultCellStyle = dataGridViewCellStyle18;
            this.dgvloans.Dock = System.Windows.Forms.DockStyle.Left;
            this.dgvloans.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvloans.Location = new System.Drawing.Point(0, 104);
            this.dgvloans.Name = "dgvloans";
            this.dgvloans.RowHeadersVisible = false;
            this.dgvloans.RowTemplate.Height = 30;
            this.dgvloans.Size = new System.Drawing.Size(377, 648);
            this.dgvloans.TabIndex = 165;
            this.dgvloans.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.White;
            this.dgvloans.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvloans.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvloans.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvloans.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvloans.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvloans.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvloans.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvloans.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.dgvloans.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvloans.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvloans.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvloans.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvloans.ThemeStyle.HeaderStyle.Height = 30;
            this.dgvloans.ThemeStyle.ReadOnly = false;
            this.dgvloans.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvloans.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvloans.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvloans.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvloans.ThemeStyle.RowsStyle.Height = 30;
            this.dgvloans.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.Silver;
            this.dgvloans.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvloans.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvloans_CellEndEdit);
            this.dgvloans.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvloans_DataBindingComplete);
            // 
            // frm_home_ADMIN_collectconfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Controls.Add(this.dgvdata);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.dgvloans);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_home_ADMIN_collectconfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frm_home_ADMIN_collectconfig";
            this.Load += new System.EventHandler(this.frm_home_ADMIN_collectconfig_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvdata)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvloans)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Guna.UI2.WinForms.Guna2Button bupload;
        private System.Windows.Forms.Label lnorecord;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lcountprocess;
        private System.Windows.Forms.Label lcolcount;
        private Guna.UI2.WinForms.Guna2Button bclear;
        private Guna.UI2.WinForms.Guna2Button bdessiminate;
        private Guna.UI2.WinForms.Guna2DataGridView dgvdata;
        private Guna.UI2.WinForms.Guna2TextBox tsearch;
        private System.Windows.Forms.Label lpercent;
        private System.Windows.Forms.ProgressBar pbloading;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2VSeparator guna2VSeparator1;
        private Guna.UI2.WinForms.Guna2DataGridView dgvloans;
        private Guna.UI2.WinForms.Guna2ComboBox cbcollector;
        private Guna.UI2.WinForms.Guna2Button bupdateloanacc;
        private Guna.UI2.WinForms.Guna2Button bupdate;
        private Guna.UI2.WinForms.Guna2CheckBox chkSelectAll;
        private Guna.UI2.WinForms.Guna2TextBox tsearchdata;
    }
}
namespace rct_lmis.ADMIN_SECTION
{
    partial class frm_home_ADMIN_dnloans
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bhelp = new Guna.UI2.WinForms.Guna2CircleButton();
            this.ltitle = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvloandata = new Guna.UI2.WinForms.Guna2DataGridView();
            this.pnavtop = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.dtdateDenied = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.bexport = new Guna.UI2.WinForms.Guna2Button();
            this.cbstatus = new Guna.UI2.WinForms.Guna2ComboBox();
            this.tsearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.bdelforever = new Guna.UI2.WinForms.Guna2Button();
            this.lnorecord = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvloandata)).BeginInit();
            this.pnavtop.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.bhelp);
            this.panel1.Controls.Add(this.ltitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1200, 52);
            this.panel1.TabIndex = 3;
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
            this.bhelp.Location = new System.Drawing.Point(1158, 9);
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
            this.ltitle.Location = new System.Drawing.Point(19, 9);
            this.ltitle.Name = "ltitle";
            this.ltitle.Size = new System.Drawing.Size(165, 30);
            this.ltitle.TabIndex = 2;
            this.ltitle.Text = "DENIED LOANS";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(738, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(419, 30);
            this.label1.TabIndex = 5;
            this.label1.Text = "Note: All \"Denied Loans\" will be deleted in 30 days. Please back up your data\r\nby" +
    " clicking the \"Export to Excel\"button. Click the \"?\" button for more information" +
    ".";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dgvloandata
            // 
            this.dgvloandata.AllowUserToAddRows = false;
            this.dgvloandata.AllowUserToDeleteRows = false;
            this.dgvloandata.AllowUserToResizeColumns = false;
            this.dgvloandata.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.dgvloandata.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvloandata.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvloandata.ColumnHeadersHeight = 30;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvloandata.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvloandata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvloandata.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvloandata.Location = new System.Drawing.Point(0, 115);
            this.dgvloandata.Name = "dgvloandata";
            this.dgvloandata.ReadOnly = true;
            this.dgvloandata.RowHeadersVisible = false;
            this.dgvloandata.RowTemplate.Height = 70;
            this.dgvloandata.Size = new System.Drawing.Size(1200, 535);
            this.dgvloandata.TabIndex = 8;
            this.dgvloandata.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.White;
            this.dgvloandata.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvloandata.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvloandata.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvloandata.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvloandata.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvloandata.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvloandata.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvloandata.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.dgvloandata.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvloandata.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvloandata.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvloandata.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvloandata.ThemeStyle.HeaderStyle.Height = 30;
            this.dgvloandata.ThemeStyle.ReadOnly = true;
            this.dgvloandata.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvloandata.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvloandata.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvloandata.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvloandata.ThemeStyle.RowsStyle.Height = 70;
            this.dgvloandata.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            this.dgvloandata.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvloandata.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvloandata_DataBindingComplete);
            // 
            // pnavtop
            // 
            this.pnavtop.Controls.Add(this.bdelforever);
            this.pnavtop.Controls.Add(this.label2);
            this.pnavtop.Controls.Add(this.dtdateDenied);
            this.pnavtop.Controls.Add(this.label3);
            this.pnavtop.Controls.Add(this.bexport);
            this.pnavtop.Controls.Add(this.cbstatus);
            this.pnavtop.Controls.Add(this.tsearch);
            this.pnavtop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnavtop.Location = new System.Drawing.Point(0, 52);
            this.pnavtop.Name = "pnavtop";
            this.pnavtop.Size = new System.Drawing.Size(1200, 63);
            this.pnavtop.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(564, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 30);
            this.label2.TabIndex = 132;
            this.label2.Text = "Date\r\nApproved";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dtdateDenied
            // 
            this.dtdateDenied.Checked = true;
            this.dtdateDenied.CustomFormat = "";
            this.dtdateDenied.FillColor = System.Drawing.Color.White;
            this.dtdateDenied.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtdateDenied.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtdateDenied.Location = new System.Drawing.Point(629, 11);
            this.dtdateDenied.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtdateDenied.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtdateDenied.Name = "dtdateDenied";
            this.dtdateDenied.Size = new System.Drawing.Size(200, 36);
            this.dtdateDenied.TabIndex = 7;
            this.dtdateDenied.Value = new System.DateTime(2024, 8, 17, 10, 23, 23, 846);
            this.dtdateDenied.ValueChanged += new System.EventHandler(this.dtdateDenied_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Location = new System.Drawing.Point(323, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 15);
            this.label3.TabIndex = 131;
            this.label3.Text = "Status";
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
            this.bexport.Location = new System.Drawing.Point(1151, 13);
            this.bexport.Name = "bexport";
            this.bexport.Size = new System.Drawing.Size(35, 35);
            this.bexport.TabIndex = 13;
            this.bexport.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cbstatus
            // 
            this.cbstatus.BackColor = System.Drawing.Color.Transparent;
            this.cbstatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbstatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbstatus.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbstatus.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbstatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbstatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbstatus.ItemHeight = 20;
            this.cbstatus.Location = new System.Drawing.Point(368, 16);
            this.cbstatus.Name = "cbstatus";
            this.cbstatus.Size = new System.Drawing.Size(190, 26);
            this.cbstatus.TabIndex = 130;
            this.cbstatus.SelectedIndexChanged += new System.EventHandler(this.cbstatus_SelectedIndexChanged);
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
            this.tsearch.Size = new System.Drawing.Size(303, 36);
            this.tsearch.TabIndex = 9;
            // 
            // bdelforever
            // 
            this.bdelforever.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bdelforever.BorderColor = System.Drawing.Color.Gainsboro;
            this.bdelforever.BorderRadius = 2;
            this.bdelforever.BorderThickness = 1;
            this.bdelforever.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bdelforever.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bdelforever.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bdelforever.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bdelforever.FillColor = System.Drawing.Color.White;
            this.bdelforever.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bdelforever.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.bdelforever.ImageSize = new System.Drawing.Size(28, 28);
            this.bdelforever.Location = new System.Drawing.Point(1028, 13);
            this.bdelforever.Name = "bdelforever";
            this.bdelforever.Size = new System.Drawing.Size(117, 35);
            this.bdelforever.TabIndex = 133;
            this.bdelforever.Text = "Delete Forever";
            // 
            // lnorecord
            // 
            this.lnorecord.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lnorecord.AutoSize = true;
            this.lnorecord.BackColor = System.Drawing.Color.White;
            this.lnorecord.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lnorecord.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lnorecord.Location = new System.Drawing.Point(575, 150);
            this.lnorecord.Name = "lnorecord";
            this.lnorecord.Size = new System.Drawing.Size(123, 15);
            this.lnorecord.TabIndex = 134;
            this.lnorecord.Text = "---no record found---";
            this.lnorecord.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lnorecord.Visible = false;
            // 
            // frm_home_ADMIN_dnloans
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 650);
            this.Controls.Add(this.lnorecord);
            this.Controls.Add(this.dgvloandata);
            this.Controls.Add(this.pnavtop);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_home_ADMIN_dnloans";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Denied Loans";
            this.Load += new System.EventHandler(this.frm_home_ADMIN_dnloans_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvloandata)).EndInit();
            this.pnavtop.ResumeLayout(false);
            this.pnavtop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2CircleButton bhelp;
        private System.Windows.Forms.Label ltitle;
        private Guna.UI2.WinForms.Guna2DataGridView dgvloandata;
        private System.Windows.Forms.Panel pnavtop;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtdateDenied;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2Button bexport;
        private Guna.UI2.WinForms.Guna2ComboBox cbstatus;
        private Guna.UI2.WinForms.Guna2TextBox tsearch;
        private Guna.UI2.WinForms.Guna2Button bdelforever;
        private System.Windows.Forms.Label lnorecord;
    }
}
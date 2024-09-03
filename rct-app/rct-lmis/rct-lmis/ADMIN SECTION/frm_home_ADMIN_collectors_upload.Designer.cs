namespace rct_lmis.ADMIN_SECTION
{
    partial class frm_home_ADMIN_collectors_upload
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_home_ADMIN_collectors_upload));
            this.panel1 = new System.Windows.Forms.Panel();
            this.laccountid = new System.Windows.Forms.Label();
            this.ltitle = new System.Windows.Forms.Label();
            this.bclear = new Guna.UI2.WinForms.Guna2Button();
            this.baddfile = new Guna.UI2.WinForms.Guna2Button();
            this.buploaddoc = new Guna.UI2.WinForms.Guna2Button();
            this.lnofile = new System.Windows.Forms.Label();
            this.dgvuploads = new Guna.UI2.WinForms.Guna2DataGridView();
            this.statusLabel = new System.Windows.Forms.Label();
            this.mainProgressBar = new Guna.UI2.WinForms.Guna2ProgressBar();
            this.lfilesready = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvuploads)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.laccountid);
            this.panel1.Controls.Add(this.ltitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(583, 38);
            this.panel1.TabIndex = 3;
            // 
            // laccountid
            // 
            this.laccountid.AutoSize = true;
            this.laccountid.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.laccountid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.laccountid.Location = new System.Drawing.Point(463, 14);
            this.laccountid.Name = "laccountid";
            this.laccountid.Size = new System.Drawing.Size(70, 13);
            this.laccountid.TabIndex = 3;
            this.laccountid.Text = "Collector ID:";
            // 
            // ltitle
            // 
            this.ltitle.AutoSize = true;
            this.ltitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ltitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.ltitle.Location = new System.Drawing.Point(12, 10);
            this.ltitle.Name = "ltitle";
            this.ltitle.Size = new System.Drawing.Size(213, 20);
            this.ltitle.TabIndex = 2;
            this.ltitle.Text = "Upload Collectors Attachment";
            // 
            // bclear
            // 
            this.bclear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bclear.BorderRadius = 4;
            this.bclear.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bclear.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bclear.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bclear.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bclear.Enabled = false;
            this.bclear.FillColor = System.Drawing.Color.Maroon;
            this.bclear.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bclear.ForeColor = System.Drawing.Color.White;
            this.bclear.Location = new System.Drawing.Point(113, 289);
            this.bclear.Name = "bclear";
            this.bclear.Size = new System.Drawing.Size(69, 28);
            this.bclear.TabIndex = 73;
            this.bclear.Text = "Clear";
            this.bclear.Click += new System.EventHandler(this.bclear_Click);
            // 
            // baddfile
            // 
            this.baddfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.baddfile.BorderRadius = 4;
            this.baddfile.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.baddfile.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.baddfile.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.baddfile.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.baddfile.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.baddfile.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baddfile.ForeColor = System.Drawing.Color.White;
            this.baddfile.Location = new System.Drawing.Point(16, 289);
            this.baddfile.Name = "baddfile";
            this.baddfile.Size = new System.Drawing.Size(91, 28);
            this.baddfile.TabIndex = 72;
            this.baddfile.Text = "Browse Files";
            this.baddfile.Click += new System.EventHandler(this.baddfile_Click);
            // 
            // buploaddoc
            // 
            this.buploaddoc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buploaddoc.BackColor = System.Drawing.Color.Transparent;
            this.buploaddoc.BorderColor = System.Drawing.Color.Transparent;
            this.buploaddoc.BorderRadius = 2;
            this.buploaddoc.BorderThickness = 1;
            this.buploaddoc.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.buploaddoc.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.buploaddoc.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.buploaddoc.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.buploaddoc.Enabled = false;
            this.buploaddoc.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.buploaddoc.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buploaddoc.ForeColor = System.Drawing.Color.White;
            this.buploaddoc.ImageSize = new System.Drawing.Size(28, 28);
            this.buploaddoc.Location = new System.Drawing.Point(466, 289);
            this.buploaddoc.Name = "buploaddoc";
            this.buploaddoc.Size = new System.Drawing.Size(105, 28);
            this.buploaddoc.TabIndex = 154;
            this.buploaddoc.Text = "Add/Upload";
            this.buploaddoc.Click += new System.EventHandler(this.buploaddoc_Click);
            // 
            // lnofile
            // 
            this.lnofile.AutoSize = true;
            this.lnofile.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lnofile.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lnofile.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lnofile.Location = new System.Drawing.Point(252, 92);
            this.lnofile.Name = "lnofile";
            this.lnofile.Size = new System.Drawing.Size(78, 13);
            this.lnofile.TabIndex = 155;
            this.lnofile.Text = "no file found.";
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
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvuploads.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvuploads.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvuploads.Location = new System.Drawing.Point(12, 44);
            this.dgvuploads.Name = "dgvuploads";
            this.dgvuploads.ReadOnly = true;
            this.dgvuploads.RowHeadersVisible = false;
            this.dgvuploads.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvuploads.RowTemplate.Height = 35;
            this.dgvuploads.Size = new System.Drawing.Size(559, 194);
            this.dgvuploads.TabIndex = 156;
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
            this.dgvuploads.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvuploads.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvuploads.ThemeStyle.RowsStyle.Height = 35;
            this.dgvuploads.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            this.dgvuploads.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvuploads.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvuploads_CellClick);
            this.dgvuploads.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvuploads_DataBindingComplete);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.statusLabel.Location = new System.Drawing.Point(9, 249);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(62, 13);
            this.statusLabel.TabIndex = 158;
            this.statusLabel.Text = "uploading...";
            this.statusLabel.Visible = false;
            // 
            // mainProgressBar
            // 
            this.mainProgressBar.Location = new System.Drawing.Point(12, 242);
            this.mainProgressBar.Name = "mainProgressBar";
            this.mainProgressBar.Size = new System.Drawing.Size(559, 5);
            this.mainProgressBar.TabIndex = 157;
            this.mainProgressBar.Text = "guna2ProgressBar1";
            this.mainProgressBar.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.mainProgressBar.Visible = false;
            // 
            // lfilesready
            // 
            this.lfilesready.AutoSize = true;
            this.lfilesready.BackColor = System.Drawing.Color.Transparent;
            this.lfilesready.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lfilesready.ForeColor = System.Drawing.Color.Maroon;
            this.lfilesready.Location = new System.Drawing.Point(372, 250);
            this.lfilesready.Name = "lfilesready";
            this.lfilesready.Size = new System.Drawing.Size(199, 26);
            this.lfilesready.TabIndex = 159;
            this.lfilesready.Text = "your files are ready for uploading.\r\nplease complete remaining requirements.";
            this.lfilesready.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lfilesready.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.label1.Location = new System.Drawing.Point(396, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Collector ID:";
            // 
            // frm_home_ADMIN_collectors_upload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 329);
            this.Controls.Add(this.lfilesready);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.mainProgressBar);
            this.Controls.Add(this.lnofile);
            this.Controls.Add(this.dgvuploads);
            this.Controls.Add(this.buploaddoc);
            this.Controls.Add(this.bclear);
            this.Controls.Add(this.baddfile);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_home_ADMIN_collectors_upload";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Collector Attachments";
            this.Load += new System.EventHandler(this.frm_home_ADMIN_collectors_upload_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvuploads)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label ltitle;
        private Guna.UI2.WinForms.Guna2Button bclear;
        private Guna.UI2.WinForms.Guna2Button baddfile;
        private Guna.UI2.WinForms.Guna2Button buploaddoc;
        private System.Windows.Forms.Label lnofile;
        private Guna.UI2.WinForms.Guna2DataGridView dgvuploads;
        private System.Windows.Forms.Label statusLabel;
        private Guna.UI2.WinForms.Guna2ProgressBar mainProgressBar;
        private System.Windows.Forms.Label lfilesready;
        private System.Windows.Forms.Label laccountid;
        private System.Windows.Forms.Label label1;
    }
}
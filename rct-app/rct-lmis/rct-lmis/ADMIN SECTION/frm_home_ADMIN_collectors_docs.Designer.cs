namespace rct_lmis.ADMIN_SECTION
{
    partial class frm_home_ADMIN_collectors_docs
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
            this.bsubmit = new Guna.UI2.WinForms.Guna2Button();
            this.bclear = new Guna.UI2.WinForms.Guna2Button();
            this.baddfile = new Guna.UI2.WinForms.Guna2Button();
            this.lnofile = new System.Windows.Forms.Label();
            this.dgvuploads = new Guna.UI2.WinForms.Guna2DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.mainProgressBar = new Guna.UI2.WinForms.Guna2ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.dgvuploads)).BeginInit();
            this.SuspendLayout();
            // 
            // bsubmit
            // 
            this.bsubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bsubmit.BorderRadius = 4;
            this.bsubmit.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bsubmit.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bsubmit.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bsubmit.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bsubmit.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.bsubmit.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bsubmit.ForeColor = System.Drawing.Color.White;
            this.bsubmit.Location = new System.Drawing.Point(682, 423);
            this.bsubmit.Name = "bsubmit";
            this.bsubmit.Size = new System.Drawing.Size(82, 31);
            this.bsubmit.TabIndex = 72;
            this.bsubmit.Text = "SUBMIT";
            // 
            // bclear
            // 
            this.bclear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bclear.BorderRadius = 4;
            this.bclear.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bclear.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bclear.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bclear.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bclear.FillColor = System.Drawing.Color.Maroon;
            this.bclear.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bclear.ForeColor = System.Drawing.Color.White;
            this.bclear.Location = new System.Drawing.Point(683, 6);
            this.bclear.Name = "bclear";
            this.bclear.Size = new System.Drawing.Size(90, 31);
            this.bclear.TabIndex = 77;
            this.bclear.Text = "Clear";
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
            this.baddfile.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baddfile.ForeColor = System.Drawing.Color.White;
            this.baddfile.Location = new System.Drawing.Point(565, 6);
            this.baddfile.Name = "baddfile";
            this.baddfile.Size = new System.Drawing.Size(112, 31);
            this.baddfile.TabIndex = 75;
            this.baddfile.Text = "Browse Files";
            // 
            // lnofile
            // 
            this.lnofile.AutoSize = true;
            this.lnofile.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lnofile.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lnofile.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lnofile.Location = new System.Drawing.Point(346, 153);
            this.lnofile.Name = "lnofile";
            this.lnofile.Size = new System.Drawing.Size(89, 19);
            this.lnofile.TabIndex = 74;
            this.lnofile.Text = "no file found.";
            // 
            // dgvuploads
            // 
            this.dgvuploads.AllowUserToAddRows = false;
            this.dgvuploads.AllowUserToDeleteRows = false;
            this.dgvuploads.AllowUserToResizeColumns = false;
            this.dgvuploads.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.dgvuploads.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvuploads.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dgvuploads.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvuploads.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvuploads.ColumnHeadersHeight = 30;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvuploads.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvuploads.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvuploads.Location = new System.Drawing.Point(15, 46);
            this.dgvuploads.Name = "dgvuploads";
            this.dgvuploads.ReadOnly = true;
            this.dgvuploads.RowHeadersVisible = false;
            this.dgvuploads.RowTemplate.Height = 35;
            this.dgvuploads.Size = new System.Drawing.Size(757, 348);
            this.dgvuploads.TabIndex = 76;
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
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 17);
            this.label1.TabIndex = 73;
            this.label1.Text = "Upload Requirements";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(11, 419);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(62, 13);
            this.statusLabel.TabIndex = 79;
            this.statusLabel.Text = "uploading...";
            this.statusLabel.Visible = false;
            // 
            // mainProgressBar
            // 
            this.mainProgressBar.Location = new System.Drawing.Point(12, 403);
            this.mainProgressBar.Name = "mainProgressBar";
            this.mainProgressBar.Size = new System.Drawing.Size(761, 14);
            this.mainProgressBar.TabIndex = 78;
            this.mainProgressBar.Text = "guna2ProgressBar1";
            this.mainProgressBar.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.mainProgressBar.Visible = false;
            // 
            // frm_home_ADMIN_collectors_docs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.mainProgressBar);
            this.Controls.Add(this.bsubmit);
            this.Controls.Add(this.bclear);
            this.Controls.Add(this.baddfile);
            this.Controls.Add(this.lnofile);
            this.Controls.Add(this.dgvuploads);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_home_ADMIN_collectors_docs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Upload Collector Documents";
            ((System.ComponentModel.ISupportInitialize)(this.dgvuploads)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button bsubmit;
        private Guna.UI2.WinForms.Guna2Button bclear;
        private Guna.UI2.WinForms.Guna2Button baddfile;
        private System.Windows.Forms.Label lnofile;
        private Guna.UI2.WinForms.Guna2DataGridView dgvuploads;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label statusLabel;
        private Guna.UI2.WinForms.Guna2ProgressBar mainProgressBar;
    }
}
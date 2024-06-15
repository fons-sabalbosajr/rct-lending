
namespace rct_lmis
{
    partial class frm_home_dashboard
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
            this.pusers = new Guna.UI2.WinForms.Guna2Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvusersonline = new Guna.UI2.WinForms.Guna2DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.pusers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvusersonline)).BeginInit();
            this.SuspendLayout();
            // 
            // pusers
            // 
            this.pusers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pusers.BorderColor = System.Drawing.Color.Silver;
            this.pusers.BorderRadius = 10;
            this.pusers.BorderThickness = 1;
            this.pusers.Controls.Add(this.label1);
            this.pusers.Controls.Add(this.dgvusersonline);
            this.pusers.FillColor = System.Drawing.Color.White;
            this.pusers.Location = new System.Drawing.Point(813, 22);
            this.pusers.Name = "pusers";
            this.pusers.Size = new System.Drawing.Size(264, 263);
            this.pusers.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(9, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Current Users";
            // 
            // dgvusersonline
            // 
            this.dgvusersonline.AllowUserToAddRows = false;
            this.dgvusersonline.AllowUserToDeleteRows = false;
            this.dgvusersonline.AllowUserToResizeColumns = false;
            this.dgvusersonline.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvusersonline.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvusersonline.Anchor = System.Windows.Forms.AnchorStyles.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvusersonline.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvusersonline.ColumnHeadersHeight = 20;
            this.dgvusersonline.ColumnHeadersVisible = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvusersonline.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvusersonline.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvusersonline.Location = new System.Drawing.Point(8, 34);
            this.dgvusersonline.Name = "dgvusersonline";
            this.dgvusersonline.RowHeadersVisible = false;
            this.dgvusersonline.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvusersonline.RowTemplate.Height = 35;
            this.dgvusersonline.Size = new System.Drawing.Size(249, 222);
            this.dgvusersonline.TabIndex = 1;
            this.dgvusersonline.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.White;
            this.dgvusersonline.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvusersonline.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvusersonline.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvusersonline.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvusersonline.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvusersonline.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvusersonline.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvusersonline.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.White;
            this.dgvusersonline.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvusersonline.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvusersonline.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvusersonline.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvusersonline.ThemeStyle.HeaderStyle.Height = 20;
            this.dgvusersonline.ThemeStyle.ReadOnly = false;
            this.dgvusersonline.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvusersonline.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvusersonline.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvusersonline.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvusersonline.ThemeStyle.RowsStyle.Height = 35;
            this.dgvusersonline.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.White;
            this.dgvusersonline.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvusersonline.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvusersonline_CellFormatting);
            this.dgvusersonline.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvusersonline_DataBindingComplete);
            this.dgvusersonline.MouseLeave += new System.EventHandler(this.dgvusersonline_MouseLeave);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(47, 56);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Test upload";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frm_home_dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 650);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pusers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_home_dashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dashboard";
            this.Load += new System.EventHandler(this.frm_home_dashboard_Load);
            this.pusers.ResumeLayout(false);
            this.pusers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvusersonline)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pusers;
        private Guna.UI2.WinForms.Guna2DataGridView dgvusersonline;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}

namespace rct_lmis
{
    partial class frm_home_clients
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
            this.guna2Separator1 = new Guna.UI2.WinForms.Guna2Separator();
            this.pheaders = new System.Windows.Forms.Panel();
            this.lnorecord = new System.Windows.Forms.Label();
            this.dgvdata = new Guna.UI2.WinForms.Guna2DataGridView();
            this.guna2Button2 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.tsearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.bhelp = new Guna.UI2.WinForms.Guna2CircleButton();
            this.panel1.SuspendLayout();
            this.pheaders.SuspendLayout();
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
            this.panel1.TabIndex = 2;
            // 
            // ltitle
            // 
            this.ltitle.AutoSize = true;
            this.ltitle.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ltitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.ltitle.Location = new System.Drawing.Point(19, 11);
            this.ltitle.Name = "ltitle";
            this.ltitle.Size = new System.Drawing.Size(160, 30);
            this.ltitle.TabIndex = 2;
            this.ltitle.Text = "LOAN CLIENTS";
            // 
            // guna2Separator1
            // 
            this.guna2Separator1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2Separator1.Location = new System.Drawing.Point(6, 56);
            this.guna2Separator1.Name = "guna2Separator1";
            this.guna2Separator1.Size = new System.Drawing.Size(1287, 10);
            this.guna2Separator1.TabIndex = 3;
            // 
            // pheaders
            // 
            this.pheaders.BackColor = System.Drawing.Color.White;
            this.pheaders.Controls.Add(this.guna2Button2);
            this.pheaders.Controls.Add(this.guna2Button1);
            this.pheaders.Controls.Add(this.guna2Separator1);
            this.pheaders.Controls.Add(this.tsearch);
            this.pheaders.Dock = System.Windows.Forms.DockStyle.Top;
            this.pheaders.Location = new System.Drawing.Point(0, 52);
            this.pheaders.Name = "pheaders";
            this.pheaders.Size = new System.Drawing.Size(1300, 66);
            this.pheaders.TabIndex = 3;
            // 
            // lnorecord
            // 
            this.lnorecord.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lnorecord.AutoSize = true;
            this.lnorecord.BackColor = System.Drawing.Color.White;
            this.lnorecord.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnorecord.ForeColor = System.Drawing.Color.Silver;
            this.lnorecord.Location = new System.Drawing.Point(550, 156);
            this.lnorecord.Name = "lnorecord";
            this.lnorecord.Size = new System.Drawing.Size(176, 25);
            this.lnorecord.TabIndex = 13;
            this.lnorecord.Text = "No records found.";
            // 
            // dgvdata
            // 
            this.dgvdata.AllowUserToAddRows = false;
            this.dgvdata.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvdata.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvdata.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvdata.ColumnHeadersHeight = 4;
            this.dgvdata.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvdata.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvdata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvdata.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvdata.Location = new System.Drawing.Point(0, 118);
            this.dgvdata.Name = "dgvdata";
            this.dgvdata.ReadOnly = true;
            this.dgvdata.RowHeadersVisible = false;
            this.dgvdata.Size = new System.Drawing.Size(1300, 582);
            this.dgvdata.TabIndex = 12;
            this.dgvdata.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.White;
            this.dgvdata.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvdata.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvdata.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvdata.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvdata.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvdata.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvdata.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvdata.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.White;
            this.dgvdata.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvdata.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvdata.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvdata.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvdata.ThemeStyle.HeaderStyle.Height = 4;
            this.dgvdata.ThemeStyle.ReadOnly = true;
            this.dgvdata.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvdata.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvdata.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvdata.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvdata.ThemeStyle.RowsStyle.Height = 22;
            this.dgvdata.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            this.dgvdata.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvdata.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvdata_CellDoubleClick);
            // 
            // guna2Button2
            // 
            this.guna2Button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2Button2.BorderColor = System.Drawing.Color.Gainsboro;
            this.guna2Button2.BorderRadius = 2;
            this.guna2Button2.BorderThickness = 1;
            this.guna2Button2.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button2.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button2.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button2.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button2.FillColor = System.Drawing.Color.White;
            this.guna2Button2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button2.ForeColor = System.Drawing.Color.White;
            this.guna2Button2.Image = global::rct_lmis.Properties.Resources.icons8_print_48;
            this.guna2Button2.ImageSize = new System.Drawing.Size(28, 28);
            this.guna2Button2.Location = new System.Drawing.Point(1251, 15);
            this.guna2Button2.Name = "guna2Button2";
            this.guna2Button2.Size = new System.Drawing.Size(35, 35);
            this.guna2Button2.TabIndex = 34;
            this.guna2Button2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // guna2Button1
            // 
            this.guna2Button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2Button1.BorderColor = System.Drawing.Color.Gainsboro;
            this.guna2Button1.BorderRadius = 2;
            this.guna2Button1.BorderThickness = 1;
            this.guna2Button1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button1.FillColor = System.Drawing.Color.White;
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button1.ForeColor = System.Drawing.Color.White;
            this.guna2Button1.Image = global::rct_lmis.Properties.Resources.icons8_export_excel_48;
            this.guna2Button1.ImageSize = new System.Drawing.Size(28, 28);
            this.guna2Button1.Location = new System.Drawing.Point(1210, 15);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(35, 35);
            this.guna2Button1.TabIndex = 33;
            this.guna2Button1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
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
            this.tsearch.Location = new System.Drawing.Point(11, 14);
            this.tsearch.Name = "tsearch";
            this.tsearch.PasswordChar = '\0';
            this.tsearch.PlaceholderText = "search any keyword...";
            this.tsearch.SelectedText = "";
            this.tsearch.Size = new System.Drawing.Size(477, 36);
            this.tsearch.TabIndex = 1;
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
            this.bhelp.Location = new System.Drawing.Point(1256, 11);
            this.bhelp.Name = "bhelp";
            this.bhelp.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.bhelp.Size = new System.Drawing.Size(30, 30);
            this.bhelp.TabIndex = 3;
            this.bhelp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.bhelp.TextOffset = new System.Drawing.Point(10, 0);
            // 
            // frm_home_clients
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 700);
            this.Controls.Add(this.lnorecord);
            this.Controls.Add(this.dgvdata);
            this.Controls.Add(this.pheaders);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_home_clients";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frm_home_clients";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pheaders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvdata)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Guna.UI2.WinForms.Guna2CircleButton bhelp;
        private System.Windows.Forms.Label ltitle;
        private Guna.UI2.WinForms.Guna2Separator guna2Separator1;
        private System.Windows.Forms.Panel pheaders;
        private Guna.UI2.WinForms.Guna2Button guna2Button2;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Guna.UI2.WinForms.Guna2TextBox tsearch;
        private System.Windows.Forms.Label lnorecord;
        private Guna.UI2.WinForms.Guna2DataGridView dgvdata;
    }
}
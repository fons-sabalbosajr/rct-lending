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
            this.ltitle = new System.Windows.Forms.Label();
            this.pleft = new System.Windows.Forms.Panel();
            this.pnavtop = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.cbsort = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.guna2DataGridView1 = new Guna.UI2.WinForms.Guna2DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.bexport = new Guna.UI2.WinForms.Guna2Button();
            this.bprint = new Guna.UI2.WinForms.Guna2Button();
            this.tsearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.bhelp = new Guna.UI2.WinForms.Guna2CircleButton();
            this.panel1.SuspendLayout();
            this.pleft.SuspendLayout();
            this.pnavtop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2DataGridView1)).BeginInit();
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
            this.pleft.Controls.Add(this.label1);
            this.pleft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pleft.Location = new System.Drawing.Point(0, 52);
            this.pleft.Name = "pleft";
            this.pleft.Size = new System.Drawing.Size(424, 648);
            this.pleft.TabIndex = 2;
            // 
            // pnavtop
            // 
            this.pnavtop.Controls.Add(this.bexport);
            this.pnavtop.Controls.Add(this.bprint);
            this.pnavtop.Controls.Add(this.cbsort);
            this.pnavtop.Controls.Add(this.label11);
            this.pnavtop.Controls.Add(this.tsearch);
            this.pnavtop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnavtop.Location = new System.Drawing.Point(424, 52);
            this.pnavtop.Name = "pnavtop";
            this.pnavtop.Size = new System.Drawing.Size(876, 63);
            this.pnavtop.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.label1.Location = new System.Drawing.Point(19, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 21);
            this.label1.TabIndex = 5;
            this.label1.Text = "Rate Details";
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
            // guna2DataGridView1
            // 
            this.guna2DataGridView1.AllowUserToAddRows = false;
            this.guna2DataGridView1.AllowUserToDeleteRows = false;
            this.guna2DataGridView1.AllowUserToResizeColumns = false;
            this.guna2DataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.guna2DataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.guna2DataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.guna2DataGridView1.ColumnHeadersHeight = 4;
            this.guna2DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.guna2DataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.guna2DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2DataGridView1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.guna2DataGridView1.Location = new System.Drawing.Point(424, 115);
            this.guna2DataGridView1.Name = "guna2DataGridView1";
            this.guna2DataGridView1.ReadOnly = true;
            this.guna2DataGridView1.RowHeadersVisible = false;
            this.guna2DataGridView1.Size = new System.Drawing.Size(876, 585);
            this.guna2DataGridView1.TabIndex = 4;
            this.guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.guna2DataGridView1.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.guna2DataGridView1.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.guna2DataGridView1.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.guna2DataGridView1.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.guna2DataGridView1.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2DataGridView1.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.guna2DataGridView1.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.guna2DataGridView1.ThemeStyle.HeaderStyle.Height = 4;
            this.guna2DataGridView1.ThemeStyle.ReadOnly = true;
            this.guna2DataGridView1.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.guna2DataGridView1.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.guna2DataGridView1.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2DataGridView1.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.guna2DataGridView1.ThemeStyle.RowsStyle.Height = 22;
            this.guna2DataGridView1.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.guna2DataGridView1.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Location = new System.Drawing.Point(828, 203);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 20);
            this.label2.TabIndex = 14;
            this.label2.Text = "no record found";
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
            this.bexport.Location = new System.Drawing.Point(788, 13);
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
            this.bprint.Location = new System.Drawing.Point(829, 13);
            this.bprint.Name = "bprint";
            this.bprint.Size = new System.Drawing.Size(35, 35);
            this.bprint.TabIndex = 12;
            this.bprint.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
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
            // frm_home_ADMIN_loanrates
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 700);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.guna2DataGridView1);
            this.Controls.Add(this.pnavtop);
            this.Controls.Add(this.pleft);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_home_ADMIN_loanrates";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Loan Rates Config";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pleft.ResumeLayout(false);
            this.pleft.PerformLayout();
            this.pnavtop.ResumeLayout(false);
            this.pnavtop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2DataGridView1)).EndInit();
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
        private Guna.UI2.WinForms.Guna2DataGridView guna2DataGridView1;
        private System.Windows.Forms.Label label2;
    }
}
namespace rct_lmis.ADMIN_SECTION
{
    partial class frm_home_ADMIN_routes
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ltitle = new System.Windows.Forms.Label();
            this.badd = new Guna.UI2.WinForms.Guna2Button();
            this.bedit = new Guna.UI2.WinForms.Guna2Button();
            this.dgvarea = new System.Windows.Forms.DataGridView();
            this.tarea = new Guna.UI2.WinForms.Guna2TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bupdate = new Guna.UI2.WinForms.Guna2Button();
            this.bdel = new Guna.UI2.WinForms.Guna2Button();
            this.tremarks = new Guna.UI2.WinForms.Guna2TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tidno = new Guna.UI2.WinForms.Guna2TextBox();
            this.breset = new Guna.UI2.WinForms.Guna2Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvarea)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.panel1.Controls.Add(this.ltitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(792, 52);
            this.panel1.TabIndex = 3;
            // 
            // ltitle
            // 
            this.ltitle.AutoSize = true;
            this.ltitle.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ltitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.ltitle.Location = new System.Drawing.Point(19, 12);
            this.ltitle.Name = "ltitle";
            this.ltitle.Size = new System.Drawing.Size(154, 30);
            this.ltitle.TabIndex = 2;
            this.ltitle.Text = "AREA ROUTES";
            // 
            // badd
            // 
            this.badd.Animated = true;
            this.badd.BorderColor = System.Drawing.Color.Transparent;
            this.badd.BorderRadius = 2;
            this.badd.BorderThickness = 1;
            this.badd.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.badd.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.badd.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.badd.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.badd.FillColor = System.Drawing.Color.SteelBlue;
            this.badd.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.badd.ForeColor = System.Drawing.Color.White;
            this.badd.Image = global::rct_lmis.Properties.Resources.icons8_add_48__2_;
            this.badd.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.badd.ImageSize = new System.Drawing.Size(15, 15);
            this.badd.Location = new System.Drawing.Point(689, 107);
            this.badd.Name = "badd";
            this.badd.Size = new System.Drawing.Size(67, 31);
            this.badd.TabIndex = 159;
            this.badd.Text = "Add";
            this.badd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.badd.Click += new System.EventHandler(this.badd_Click);
            // 
            // bedit
            // 
            this.bedit.BackColor = System.Drawing.Color.Transparent;
            this.bedit.BorderColor = System.Drawing.Color.Transparent;
            this.bedit.BorderRadius = 2;
            this.bedit.BorderThickness = 1;
            this.bedit.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bedit.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bedit.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bedit.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bedit.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.bedit.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.bedit.ForeColor = System.Drawing.Color.White;
            this.bedit.ImageSize = new System.Drawing.Size(28, 28);
            this.bedit.Location = new System.Drawing.Point(690, 70);
            this.bedit.Name = "bedit";
            this.bedit.Size = new System.Drawing.Size(66, 31);
            this.bedit.TabIndex = 156;
            this.bedit.Text = "Edit";
            this.bedit.Click += new System.EventHandler(this.bedit_Click);
            // 
            // dgvarea
            // 
            this.dgvarea.AllowUserToAddRows = false;
            this.dgvarea.AllowUserToDeleteRows = false;
            this.dgvarea.AllowUserToResizeColumns = false;
            this.dgvarea.AllowUserToResizeRows = false;
            this.dgvarea.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvarea.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvarea.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvarea.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvarea.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvarea.Location = new System.Drawing.Point(24, 70);
            this.dgvarea.Name = "dgvarea";
            this.dgvarea.ReadOnly = true;
            this.dgvarea.RowHeadersVisible = false;
            this.dgvarea.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvarea.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvarea.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvarea.Size = new System.Drawing.Size(391, 338);
            this.dgvarea.TabIndex = 160;
            // 
            // tarea
            // 
            this.tarea.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tarea.DefaultText = "";
            this.tarea.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tarea.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tarea.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tarea.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tarea.Enabled = false;
            this.tarea.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tarea.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tarea.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tarea.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tarea.Location = new System.Drawing.Point(492, 107);
            this.tarea.Name = "tarea";
            this.tarea.PasswordChar = '\0';
            this.tarea.PlaceholderText = "n/a";
            this.tarea.SelectedText = "";
            this.tarea.Size = new System.Drawing.Size(177, 24);
            this.tarea.TabIndex = 164;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(429, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 15);
            this.label1.TabIndex = 163;
            this.label1.Text = "Area:";
            // 
            // bupdate
            // 
            this.bupdate.Animated = true;
            this.bupdate.BorderColor = System.Drawing.Color.Transparent;
            this.bupdate.BorderRadius = 2;
            this.bupdate.BorderThickness = 1;
            this.bupdate.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bupdate.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bupdate.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bupdate.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bupdate.Enabled = false;
            this.bupdate.FillColor = System.Drawing.Color.SeaGreen;
            this.bupdate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.bupdate.ForeColor = System.Drawing.Color.White;
            this.bupdate.ImageSize = new System.Drawing.Size(28, 28);
            this.bupdate.Location = new System.Drawing.Point(689, 144);
            this.bupdate.Name = "bupdate";
            this.bupdate.Size = new System.Drawing.Size(67, 31);
            this.bupdate.TabIndex = 157;
            this.bupdate.Text = "Update";
            this.bupdate.Click += new System.EventHandler(this.bupdate_Click);
            // 
            // bdel
            // 
            this.bdel.Animated = true;
            this.bdel.BorderColor = System.Drawing.Color.Transparent;
            this.bdel.BorderRadius = 2;
            this.bdel.BorderThickness = 1;
            this.bdel.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bdel.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bdel.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bdel.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bdel.Enabled = false;
            this.bdel.FillColor = System.Drawing.Color.Brown;
            this.bdel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.bdel.ForeColor = System.Drawing.Color.White;
            this.bdel.ImageSize = new System.Drawing.Size(28, 28);
            this.bdel.Location = new System.Drawing.Point(690, 181);
            this.bdel.Name = "bdel";
            this.bdel.Size = new System.Drawing.Size(67, 31);
            this.bdel.TabIndex = 165;
            this.bdel.Text = "Delete";
            this.bdel.Click += new System.EventHandler(this.bdel_Click);
            // 
            // tremarks
            // 
            this.tremarks.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tremarks.DefaultText = "";
            this.tremarks.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tremarks.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tremarks.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tremarks.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tremarks.Enabled = false;
            this.tremarks.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tremarks.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tremarks.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tremarks.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tremarks.Location = new System.Drawing.Point(492, 144);
            this.tremarks.Multiline = true;
            this.tremarks.Name = "tremarks";
            this.tremarks.PasswordChar = '\0';
            this.tremarks.PlaceholderText = "n/a";
            this.tremarks.SelectedText = "";
            this.tremarks.Size = new System.Drawing.Size(177, 116);
            this.tremarks.TabIndex = 167;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(429, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 15);
            this.label2.TabIndex = 166;
            this.label2.Text = "Remarks:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label12.Location = new System.Drawing.Point(429, 77);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(48, 15);
            this.label12.TabIndex = 161;
            this.label12.Text = "Area ID:";
            // 
            // tidno
            // 
            this.tidno.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tidno.DefaultText = "";
            this.tidno.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tidno.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tidno.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tidno.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tidno.Enabled = false;
            this.tidno.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tidno.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tidno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tidno.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tidno.Location = new System.Drawing.Point(492, 70);
            this.tidno.Name = "tidno";
            this.tidno.PasswordChar = '\0';
            this.tidno.PlaceholderText = "n/a";
            this.tidno.SelectedText = "";
            this.tidno.Size = new System.Drawing.Size(177, 24);
            this.tidno.TabIndex = 162;
            // 
            // breset
            // 
            this.breset.BackColor = System.Drawing.Color.Transparent;
            this.breset.BorderColor = System.Drawing.Color.Transparent;
            this.breset.BorderRadius = 2;
            this.breset.BorderThickness = 1;
            this.breset.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.breset.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.breset.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.breset.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.breset.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.breset.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.breset.ForeColor = System.Drawing.Color.White;
            this.breset.ImageSize = new System.Drawing.Size(28, 28);
            this.breset.Location = new System.Drawing.Point(492, 266);
            this.breset.Name = "breset";
            this.breset.Size = new System.Drawing.Size(107, 30);
            this.breset.TabIndex = 168;
            this.breset.Text = "Reset ID (DEV)";
            this.breset.Click += new System.EventHandler(this.breset_Click);
            // 
            // frm_home_ADMIN_routes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 433);
            this.Controls.Add(this.breset);
            this.Controls.Add(this.tremarks);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bdel);
            this.Controls.Add(this.tarea);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tidno);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.dgvarea);
            this.Controls.Add(this.badd);
            this.Controls.Add(this.bedit);
            this.Controls.Add(this.bupdate);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_home_ADMIN_routes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Area Routes Configuration";
            this.Load += new System.EventHandler(this.frm_home_ADMIN_routes_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvarea)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label ltitle;
        private Guna.UI2.WinForms.Guna2Button badd;
        private Guna.UI2.WinForms.Guna2Button bedit;
        private System.Windows.Forms.DataGridView dgvarea;
        private Guna.UI2.WinForms.Guna2TextBox tarea;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2Button bupdate;
        private Guna.UI2.WinForms.Guna2Button bdel;
        private Guna.UI2.WinForms.Guna2TextBox tremarks;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label12;
        private Guna.UI2.WinForms.Guna2TextBox tidno;
        private Guna.UI2.WinForms.Guna2Button breset;
    }
}
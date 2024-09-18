
namespace rct_lmis.LOAN_SECTION
{
    partial class frm_home_loan_editamt_pass
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
            this.label39 = new System.Windows.Forms.Label();
            this.tpass = new Guna.UI2.WinForms.Guna2TextBox();
            this.bproceed = new Guna.UI2.WinForms.Guna2Button();
            this.chShow = new Guna.UI2.WinForms.Guna2CheckBox();
            this.SuspendLayout();
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.BackColor = System.Drawing.Color.Transparent;
            this.label39.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.label39.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label39.Location = new System.Drawing.Point(87, 12);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(317, 20);
            this.label39.TabIndex = 140;
            this.label39.Text = "Please enter your password to edit amounts";
            // 
            // tpass
            // 
            this.tpass.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tpass.DefaultText = "";
            this.tpass.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tpass.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tpass.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tpass.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tpass.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tpass.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tpass.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tpass.Location = new System.Drawing.Point(77, 43);
            this.tpass.Name = "tpass";
            this.tpass.PasswordChar = '\0';
            this.tpass.PlaceholderText = "";
            this.tpass.SelectedText = "";
            this.tpass.Size = new System.Drawing.Size(345, 41);
            this.tpass.TabIndex = 141;
            // 
            // bproceed
            // 
            this.bproceed.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bproceed.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bproceed.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bproceed.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bproceed.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bproceed.ForeColor = System.Drawing.Color.White;
            this.bproceed.Location = new System.Drawing.Point(144, 115);
            this.bproceed.Name = "bproceed";
            this.bproceed.Size = new System.Drawing.Size(180, 39);
            this.bproceed.TabIndex = 142;
            this.bproceed.Text = "Edit Amount";
            this.bproceed.Click += new System.EventHandler(this.bproceed_Click);
            // 
            // chShow
            // 
            this.chShow.AutoSize = true;
            this.chShow.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chShow.CheckedState.BorderRadius = 0;
            this.chShow.CheckedState.BorderThickness = 0;
            this.chShow.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chShow.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chShow.Location = new System.Drawing.Point(314, 90);
            this.chShow.Name = "chShow";
            this.chShow.Size = new System.Drawing.Size(108, 19);
            this.chShow.TabIndex = 143;
            this.chShow.Text = "Show Password";
            this.chShow.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.chShow.UncheckedState.BorderRadius = 0;
            this.chShow.UncheckedState.BorderThickness = 0;
            this.chShow.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.chShow.CheckedChanged += new System.EventHandler(this.chShow_CheckedChanged);
            // 
            // frm_home_loan_editamt_pass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 168);
            this.Controls.Add(this.chShow);
            this.Controls.Add(this.bproceed);
            this.Controls.Add(this.tpass);
            this.Controls.Add(this.label39);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_home_loan_editamt_pass";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Enter Password";
            this.Load += new System.EventHandler(this.frm_home_loan_editamt_pass_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label39;
        private Guna.UI2.WinForms.Guna2TextBox tpass;
        private Guna.UI2.WinForms.Guna2Button bproceed;
        private Guna.UI2.WinForms.Guna2CheckBox chShow;
    }
}
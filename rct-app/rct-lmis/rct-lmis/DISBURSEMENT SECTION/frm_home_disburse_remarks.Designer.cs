
namespace rct_lmis.DISBURSEMENT_SECTION
{
    partial class frm_home_disburse_remarks
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
            this.laccno = new System.Windows.Forms.Label();
            this.tremarks = new Guna.UI2.WinForms.Guna2TextBox();
            this.bsave = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // laccno
            // 
            this.laccno.AutoSize = true;
            this.laccno.BackColor = System.Drawing.Color.Transparent;
            this.laccno.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.laccno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.laccno.Location = new System.Drawing.Point(16, 14);
            this.laccno.Name = "laccno";
            this.laccno.Size = new System.Drawing.Size(80, 20);
            this.laccno.TabIndex = 20;
            this.laccno.Text = "REMARKS";
            // 
            // tremarks
            // 
            this.tremarks.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tremarks.DefaultText = "";
            this.tremarks.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tremarks.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tremarks.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tremarks.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tremarks.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tremarks.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tremarks.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tremarks.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tremarks.Location = new System.Drawing.Point(20, 37);
            this.tremarks.Multiline = true;
            this.tremarks.Name = "tremarks";
            this.tremarks.PasswordChar = '\0';
            this.tremarks.PlaceholderText = "n/a";
            this.tremarks.ReadOnly = true;
            this.tremarks.SelectedText = "";
            this.tremarks.Size = new System.Drawing.Size(679, 138);
            this.tremarks.TabIndex = 93;
            // 
            // bsave
            // 
            this.bsave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.bsave.BorderRadius = 4;
            this.bsave.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bsave.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bsave.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bsave.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bsave.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.bsave.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bsave.ForeColor = System.Drawing.Color.White;
            this.bsave.Location = new System.Drawing.Point(617, 181);
            this.bsave.Name = "bsave";
            this.bsave.Size = new System.Drawing.Size(82, 31);
            this.bsave.TabIndex = 118;
            this.bsave.Text = "Save";
            this.bsave.Click += new System.EventHandler(this.bsave_Click);
            // 
            // frm_home_disburse_remarks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 224);
            this.Controls.Add(this.bsave);
            this.Controls.Add(this.tremarks);
            this.Controls.Add(this.laccno);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_home_disburse_remarks";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Remarks";
            this.Load += new System.EventHandler(this.frm_home_disburse_remarks_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label laccno;
        private Guna.UI2.WinForms.Guna2TextBox tremarks;
        private Guna.UI2.WinForms.Guna2Button bsave;
    }
}
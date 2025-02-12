namespace rct_lmis.ADMIN_SECTION
{
    partial class frm_home_ZProgress
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
            this.lstatus = new System.Windows.Forms.Label();
            this.pbloading = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // lstatus
            // 
            this.lstatus.AutoSize = true;
            this.lstatus.Location = new System.Drawing.Point(229, 69);
            this.lstatus.Name = "lstatus";
            this.lstatus.Size = new System.Drawing.Size(92, 13);
            this.lstatus.TabIndex = 1;
            this.lstatus.Text = "Processing data...";
            // 
            // pbloading
            // 
            this.pbloading.Location = new System.Drawing.Point(87, 33);
            this.pbloading.Name = "pbloading";
            this.pbloading.Size = new System.Drawing.Size(393, 23);
            this.pbloading.TabIndex = 2;
            // 
            // frm_home_ZProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 104);
            this.Controls.Add(this.pbloading);
            this.Controls.Add(this.lstatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_home_ZProgress";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Processing...";
            this.Load += new System.EventHandler(this.frm_home_ZProgress_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lstatus;
        private System.Windows.Forms.ProgressBar pbloading;
    }
}
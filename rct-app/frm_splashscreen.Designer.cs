
namespace rct_app
{
    partial class frm_splashscreen
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
            this.bopen = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bopen
            // 
            this.bopen.Location = new System.Drawing.Point(196, 285);
            this.bopen.Name = "bopen";
            this.bopen.Size = new System.Drawing.Size(157, 46);
            this.bopen.TabIndex = 0;
            this.bopen.Text = "Welcome";
            this.bopen.UseVisualStyleBackColor = true;
            this.bopen.Click += new System.EventHandler(this.bopen_Click);
            // 
            // frm_splashscreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 415);
            this.Controls.Add(this.bopen);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_splashscreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Welcome";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bopen;
    }
}


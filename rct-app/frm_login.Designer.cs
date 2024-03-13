
namespace rct_app
{
    partial class frm_login
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
            this.blogin = new System.Windows.Forms.Button();
            this.bsignup = new System.Windows.Forms.Button();
            this.tpass = new System.Windows.Forms.TextBox();
            this.tuser = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // blogin
            // 
            this.blogin.Location = new System.Drawing.Point(55, 163);
            this.blogin.Name = "blogin";
            this.blogin.Size = new System.Drawing.Size(102, 33);
            this.blogin.TabIndex = 0;
            this.blogin.Text = "Log in";
            this.blogin.UseVisualStyleBackColor = true;
            // 
            // bsignup
            // 
            this.bsignup.Location = new System.Drawing.Point(177, 163);
            this.bsignup.Name = "bsignup";
            this.bsignup.Size = new System.Drawing.Size(102, 33);
            this.bsignup.TabIndex = 1;
            this.bsignup.Text = "Sign Up";
            this.bsignup.UseVisualStyleBackColor = true;
            this.bsignup.Click += new System.EventHandler(this.bsignup_Click);
            // 
            // tpass
            // 
            this.tpass.Location = new System.Drawing.Point(55, 118);
            this.tpass.Name = "tpass";
            this.tpass.Size = new System.Drawing.Size(384, 20);
            this.tpass.TabIndex = 2;
            // 
            // tuser
            // 
            this.tuser.Location = new System.Drawing.Point(55, 65);
            this.tuser.Name = "tuser";
            this.tuser.Size = new System.Drawing.Size(384, 20);
            this.tuser.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Enter Your Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(52, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Password";
            // 
            // frm_login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 295);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tuser);
            this.Controls.Add(this.tpass);
            this.Controls.Add(this.bsignup);
            this.Controls.Add(this.blogin);
            this.Name = "frm_login";
            this.Text = "LOG IN";
            this.Load += new System.EventHandler(this.frm_login_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button blogin;
        private System.Windows.Forms.Button bsignup;
        private System.Windows.Forms.TextBox tpass;
        private System.Windows.Forms.TextBox tuser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

namespace rct_app
{
    partial class frm_signup
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tname = new System.Windows.Forms.TextBox();
            this.tuser = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.bsignup = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tpass = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Enter Username";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Name:";
            // 
            // tname
            // 
            this.tname.Location = new System.Drawing.Point(38, 103);
            this.tname.Name = "tname";
            this.tname.Size = new System.Drawing.Size(391, 20);
            this.tname.TabIndex = 9;
            // 
            // tuser
            // 
            this.tuser.Location = new System.Drawing.Point(38, 158);
            this.tuser.Name = "tuser";
            this.tuser.Size = new System.Drawing.Size(160, 20);
            this.tuser.TabIndex = 8;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(160, 225);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(102, 33);
            this.button2.TabIndex = 7;
            this.button2.Text = "Back to Log in";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // bsignup
            // 
            this.bsignup.Location = new System.Drawing.Point(38, 225);
            this.bsignup.Name = "bsignup";
            this.bsignup.Size = new System.Drawing.Size(102, 33);
            this.bsignup.TabIndex = 6;
            this.bsignup.Text = "Sign up";
            this.bsignup.UseVisualStyleBackColor = true;
            this.bsignup.Click += new System.EventHandler(this.bsignup_ClickAsync);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Sign UP/Create Account";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(266, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Enter Password";
            // 
            // tpass
            // 
            this.tpass.Location = new System.Drawing.Point(269, 158);
            this.tpass.Name = "tpass";
            this.tpass.Size = new System.Drawing.Size(160, 20);
            this.tpass.TabIndex = 13;
            // 
            // frm_signup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 305);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tpass);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tname);
            this.Controls.Add(this.tuser);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.bsignup);
            this.Name = "frm_signup";
            this.Text = "Sign UP";
            this.Load += new System.EventHandler(this.frm_signup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tname;
        private System.Windows.Forms.TextBox tuser;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button bsignup;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tpass;
    }
}
namespace rct_lmis
{
    partial class frm_test_upload
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lpercent = new System.Windows.Forms.Label();
            this.pbloading = new Guna.UI2.WinForms.Guna2ProgressBar();
            this.bupload = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.rtpath = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.bopen = new System.Windows.Forms.Button();
            this.taddress = new System.Windows.Forms.TextBox();
            this.tname = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lpercent);
            this.groupBox1.Controls.Add(this.pbloading);
            this.groupBox1.Controls.Add(this.bupload);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.rtpath);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.bopen);
            this.groupBox1.Controls.Add(this.taddress);
            this.groupBox1.Controls.Add(this.tname);
            this.groupBox1.Location = new System.Drawing.Point(31, 35);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(429, 266);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Details";
            // 
            // lpercent
            // 
            this.lpercent.AutoSize = true;
            this.lpercent.Location = new System.Drawing.Point(256, 236);
            this.lpercent.Name = "lpercent";
            this.lpercent.Size = new System.Drawing.Size(62, 13);
            this.lpercent.TabIndex = 11;
            this.lpercent.Text = "uploading...";
            // 
            // pbloading
            // 
            this.pbloading.Location = new System.Drawing.Point(195, 210);
            this.pbloading.Name = "pbloading";
            this.pbloading.Size = new System.Drawing.Size(123, 23);
            this.pbloading.TabIndex = 10;
            this.pbloading.Text = "guna2ProgressBar1";
            this.pbloading.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            // 
            // bupload
            // 
            this.bupload.Location = new System.Drawing.Point(114, 210);
            this.bupload.Name = "bupload";
            this.bupload.Size = new System.Drawing.Size(75, 23);
            this.bupload.TabIndex = 3;
            this.bupload.Text = "Upload File";
            this.bupload.UseVisualStyleBackColor = true;
            this.bupload.Click += new System.EventHandler(this.bupload_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Address:";
            // 
            // rtpath
            // 
            this.rtpath.Location = new System.Drawing.Point(33, 104);
            this.rtpath.Name = "rtpath";
            this.rtpath.Size = new System.Drawing.Size(285, 96);
            this.rtpath.TabIndex = 1;
            this.rtpath.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Name:";
            // 
            // bopen
            // 
            this.bopen.Location = new System.Drawing.Point(33, 210);
            this.bopen.Name = "bopen";
            this.bopen.Size = new System.Drawing.Size(75, 23);
            this.bopen.TabIndex = 2;
            this.bopen.Text = "Open File";
            this.bopen.UseVisualStyleBackColor = true;
            // 
            // taddress
            // 
            this.taddress.Location = new System.Drawing.Point(82, 57);
            this.taddress.Multiline = true;
            this.taddress.Name = "taddress";
            this.taddress.Size = new System.Drawing.Size(236, 38);
            this.taddress.TabIndex = 5;
            // 
            // tname
            // 
            this.tname.Location = new System.Drawing.Point(82, 31);
            this.tname.Name = "tname";
            this.tname.Size = new System.Drawing.Size(181, 20);
            this.tname.TabIndex = 4;
            // 
            // frm_test_upload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 513);
            this.Controls.Add(this.groupBox1);
            this.Name = "frm_test_upload";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frm_test_upload";
            this.Load += new System.EventHandler(this.frm_test_upload_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button bupload;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox rtpath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bopen;
        private System.Windows.Forms.TextBox taddress;
        private System.Windows.Forms.TextBox tname;
        private Guna.UI2.WinForms.Guna2ProgressBar pbloading;
        private System.Windows.Forms.Label lpercent;
    }
}
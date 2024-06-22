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
            this.btnAddFiles = new System.Windows.Forms.Button();
            this.bupload = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.taddress = new System.Windows.Forms.TextBox();
            this.tname = new System.Windows.Forms.TextBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.mainProgressBar = new Guna.UI2.WinForms.Guna2ProgressBar();
            this.dgvFiles = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnAddFiles);
            this.groupBox1.Controls.Add(this.dgvFiles);
            this.groupBox1.Controls.Add(this.bupload);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.taddress);
            this.groupBox1.Controls.Add(this.tname);
            this.groupBox1.Location = new System.Drawing.Point(6, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(668, 329);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Details";
            // 
            // btnAddFiles
            // 
            this.btnAddFiles.Location = new System.Drawing.Point(506, 110);
            this.btnAddFiles.Name = "btnAddFiles";
            this.btnAddFiles.Size = new System.Drawing.Size(75, 27);
            this.btnAddFiles.TabIndex = 10;
            this.btnAddFiles.Text = "Add File";
            this.btnAddFiles.UseVisualStyleBackColor = true;
            this.btnAddFiles.Click += new System.EventHandler(this.btnAddFiles_Click);
            // 
            // bupload
            // 
            this.bupload.Location = new System.Drawing.Point(587, 110);
            this.bupload.Name = "bupload";
            this.bupload.Size = new System.Drawing.Size(75, 27);
            this.bupload.TabIndex = 3;
            this.bupload.Text = "Upload File";
            this.bupload.UseVisualStyleBackColor = true;
            this.bupload.Click += new System.EventHandler(this.bupload_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Address:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Name:";
            // 
            // taddress
            // 
            this.taddress.Location = new System.Drawing.Point(59, 99);
            this.taddress.Multiline = true;
            this.taddress.Name = "taddress";
            this.taddress.Size = new System.Drawing.Size(236, 38);
            this.taddress.TabIndex = 5;
            // 
            // tname
            // 
            this.tname.Location = new System.Drawing.Point(59, 73);
            this.tname.Name = "tname";
            this.tname.Size = new System.Drawing.Size(236, 20);
            this.tname.TabIndex = 4;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(11, 365);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(62, 13);
            this.statusLabel.TabIndex = 11;
            this.statusLabel.Text = "uploading...";
            // 
            // mainProgressBar
            // 
            this.mainProgressBar.Location = new System.Drawing.Point(12, 347);
            this.mainProgressBar.Name = "mainProgressBar";
            this.mainProgressBar.Size = new System.Drawing.Size(662, 14);
            this.mainProgressBar.TabIndex = 10;
            this.mainProgressBar.Text = "guna2ProgressBar1";
            this.mainProgressBar.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            // 
            // dgvFiles
            // 
            this.dgvFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFiles.Location = new System.Drawing.Point(6, 143);
            this.dgvFiles.Name = "dgvFiles";
            this.dgvFiles.Size = new System.Drawing.Size(656, 159);
            this.dgvFiles.TabIndex = 13;
            // 
            // frm_test_upload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 513);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.mainProgressBar);
            this.Name = "frm_test_upload";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frm_test_upload";
            this.Load += new System.EventHandler(this.frm_test_upload_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button bupload;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox taddress;
        private System.Windows.Forms.TextBox tname;
        private Guna.UI2.WinForms.Guna2ProgressBar mainProgressBar;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.DataGridView dgvFiles;
        private System.Windows.Forms.Button btnAddFiles;
    }
}
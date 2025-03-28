﻿
namespace rct_lmis
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_splashscreen));
            this.tfade = new System.Windows.Forms.Timer(this.components);
            this.ploading = new System.Windows.Forms.Panel();
            this.pdec = new System.Windows.Forms.Panel();
            this.tslide = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.guna2PictureBox2 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.pdec.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tfade
            // 
            this.tfade.Enabled = true;
            this.tfade.Interval = 5;
            this.tfade.Tick += new System.EventHandler(this.tfade_Tick);
            // 
            // ploading
            // 
            this.ploading.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(96)))), ((int)(((byte)(114)))));
            this.ploading.Location = new System.Drawing.Point(0, 0);
            this.ploading.Name = "ploading";
            this.ploading.Size = new System.Drawing.Size(70, 10);
            this.ploading.TabIndex = 3;
            // 
            // pdec
            // 
            this.pdec.BackColor = System.Drawing.Color.Gainsboro;
            this.pdec.Controls.Add(this.ploading);
            this.pdec.Location = new System.Drawing.Point(164, 419);
            this.pdec.Name = "pdec";
            this.pdec.Size = new System.Drawing.Size(300, 10);
            this.pdec.TabIndex = 7;
            // 
            // tslide
            // 
            this.tslide.Enabled = true;
            this.tslide.Interval = 1;
            this.tslide.Tick += new System.EventHandler(this.tslide_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label1.Location = new System.Drawing.Point(164, 386);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 19);
            this.label1.TabIndex = 9;
            this.label1.Text = "Loading..";
            // 
            // guna2PictureBox2
            // 
            this.guna2PictureBox2.Image = global::rct_lmis.Properties.Resources.icons8_banking_loan_with_money_bag_in_hand_721;
            this.guna2PictureBox2.ImageRotate = 0F;
            this.guna2PictureBox2.Location = new System.Drawing.Point(164, 127);
            this.guna2PictureBox2.Name = "guna2PictureBox2";
            this.guna2PictureBox2.Size = new System.Drawing.Size(84, 80);
            this.guna2PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.guna2PictureBox2.TabIndex = 10;
            this.guna2PictureBox2.TabStop = false;
            // 
            // guna2PictureBox1
            // 
            this.guna2PictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2PictureBox1.Image = global::rct_lmis.Properties.Resources.RCT_splashscreen;
            this.guna2PictureBox1.ImageRotate = 0F;
            this.guna2PictureBox1.Location = new System.Drawing.Point(0, 0);
            this.guna2PictureBox1.Name = "guna2PictureBox1";
            this.guna2PictureBox1.Size = new System.Drawing.Size(600, 500);
            this.guna2PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.guna2PictureBox1.TabIndex = 8;
            this.guna2PictureBox1.TabStop = false;
            // 
            // frm_splashscreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.ClientSize = new System.Drawing.Size(600, 500);
            this.Controls.Add(this.guna2PictureBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pdec);
            this.Controls.Add(this.guna2PictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frm_splashscreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_splashscreen_FormClosing);
            this.Load += new System.EventHandler(this.frm_splashscreen_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frm_splashscreen_MouseDown);
            this.pdec.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tfade;
        private System.Windows.Forms.Panel ploading;
        private System.Windows.Forms.Panel pdec;
        private System.Windows.Forms.Timer tslide;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox2;
    }
}
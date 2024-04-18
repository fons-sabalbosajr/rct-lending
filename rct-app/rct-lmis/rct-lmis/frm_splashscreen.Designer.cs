
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
            this.tfade = new System.Windows.Forms.Timer(this.components);
            this.ploading = new System.Windows.Forms.Panel();
            this.pdec = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tslide = new System.Windows.Forms.Timer(this.components);
            this.pdec.SuspendLayout();
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
            this.pdec.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pdec.Location = new System.Drawing.Point(0, 490);
            this.pdec.Name = "pdec";
            this.pdec.Size = new System.Drawing.Size(600, 10);
            this.pdec.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(230, 130);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 30);
            this.label1.TabIndex = 8;
            this.label1.Text = "WELCOME";
            // 
            // tslide
            // 
            this.tslide.Enabled = true;
            this.tslide.Interval = 1;
            this.tslide.Tick += new System.EventHandler(this.tslide_Tick);
            // 
            // frm_splashscreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.ClientSize = new System.Drawing.Size(600, 500);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pdec);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_splashscreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_splashscreen_FormClosing);
            this.Load += new System.EventHandler(this.frm_splashscreen_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frm_splashscreen_MouseDown);
            this.pdec.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tfade;
        private System.Windows.Forms.Panel ploading;
        private System.Windows.Forms.Panel pdec;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer tslide;
    }
}
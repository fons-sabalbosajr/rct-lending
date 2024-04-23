
namespace rct_lmis
{
    partial class z_loadingform
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
            this.pbody = new Guna.UI2.WinForms.Guna2Panel();
            this.pbgif = new Guna.UI2.WinForms.Guna2PictureBox();
            this.lloading = new System.Windows.Forms.Label();
            this.tfade = new System.Windows.Forms.Timer(this.components);
            this.pbody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbgif)).BeginInit();
            this.SuspendLayout();
            // 
            // pbody
            // 
            this.pbody.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.pbody.BorderRadius = 2;
            this.pbody.Controls.Add(this.pbgif);
            this.pbody.Controls.Add(this.lloading);
            this.pbody.CustomBorderColor = System.Drawing.Color.SteelBlue;
            this.pbody.CustomBorderThickness = new System.Windows.Forms.Padding(1);
            this.pbody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbody.Location = new System.Drawing.Point(0, 0);
            this.pbody.Name = "pbody";
            this.pbody.Size = new System.Drawing.Size(300, 100);
            this.pbody.TabIndex = 7;
            this.pbody.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbody_MouseDown);
            // 
            // pbgif
            // 
            this.pbgif.BackColor = System.Drawing.Color.Transparent;
            this.pbgif.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbgif.FillColor = System.Drawing.Color.Transparent;
            this.pbgif.Image = global::rct_lmis.Properties.Resources.loading;
            this.pbgif.ImageRotate = 0F;
            this.pbgif.Location = new System.Drawing.Point(52, 12);
            this.pbgif.Name = "pbgif";
            this.pbgif.Size = new System.Drawing.Size(70, 70);
            this.pbgif.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbgif.TabIndex = 5;
            this.pbgif.TabStop = false;
            this.pbgif.UseTransparentBackground = true;
            // 
            // lloading
            // 
            this.lloading.AutoSize = true;
            this.lloading.Font = new System.Drawing.Font("Microsoft Tai Le", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lloading.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.lloading.Location = new System.Drawing.Point(143, 38);
            this.lloading.Name = "lloading";
            this.lloading.Size = new System.Drawing.Size(93, 23);
            this.lloading.TabIndex = 4;
            this.lloading.Text = "Loading...";
            // 
            // tfade
            // 
            this.tfade.Enabled = true;
            this.tfade.Interval = 5;
            this.tfade.Tick += new System.EventHandler(this.tfade_Tick);
            // 
            // z_loadingform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 100);
            this.Controls.Add(this.pbody);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "z_loadingform";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Please Wait...";
            this.Load += new System.EventHandler(this.z_loadingform_Load);
            this.pbody.ResumeLayout(false);
            this.pbody.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbgif)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pbody;
        private Guna.UI2.WinForms.Guna2PictureBox pbgif;
        private System.Windows.Forms.Label lloading;
        private System.Windows.Forms.Timer tfade;
    }
}
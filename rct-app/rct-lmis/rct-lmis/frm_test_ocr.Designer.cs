namespace rct_lmis
{
    partial class frm_test_ocr
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
            this.bupload = new Guna.UI2.WinForms.Guna2Button();
            this.pbphoto = new Guna.UI2.WinForms.Guna2PictureBox();
            this.tpath = new Guna.UI2.WinForms.Guna2TextBox();
            this.tname = new Guna.UI2.WinForms.Guna2TextBox();
            this.taddress = new Guna.UI2.WinForms.Guna2TextBox();
            this.tage = new Guna.UI2.WinForms.Guna2TextBox();
            this.bgetdata = new Guna.UI2.WinForms.Guna2Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbstatus = new Guna.UI2.WinForms.Guna2ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbphoto)).BeginInit();
            this.SuspendLayout();
            // 
            // bupload
            // 
            this.bupload.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bupload.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bupload.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bupload.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bupload.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.bupload.ForeColor = System.Drawing.Color.White;
            this.bupload.Location = new System.Drawing.Point(40, 28);
            this.bupload.Name = "bupload";
            this.bupload.Size = new System.Drawing.Size(180, 45);
            this.bupload.TabIndex = 0;
            this.bupload.Text = "Upload File";
            this.bupload.Click += new System.EventHandler(this.bupload_Click);
            // 
            // pbphoto
            // 
            this.pbphoto.ImageRotate = 0F;
            this.pbphoto.Location = new System.Drawing.Point(40, 79);
            this.pbphoto.Name = "pbphoto";
            this.pbphoto.Size = new System.Drawing.Size(504, 496);
            this.pbphoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbphoto.TabIndex = 1;
            this.pbphoto.TabStop = false;
            // 
            // tpath
            // 
            this.tpath.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tpath.DefaultText = "";
            this.tpath.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tpath.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tpath.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tpath.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tpath.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tpath.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tpath.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tpath.Location = new System.Drawing.Point(243, 37);
            this.tpath.Name = "tpath";
            this.tpath.PasswordChar = '\0';
            this.tpath.PlaceholderText = "";
            this.tpath.SelectedText = "";
            this.tpath.Size = new System.Drawing.Size(301, 36);
            this.tpath.TabIndex = 2;
            // 
            // tname
            // 
            this.tname.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tname.DefaultText = "";
            this.tname.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tname.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tname.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tname.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tname.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tname.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tname.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tname.Location = new System.Drawing.Point(673, 130);
            this.tname.Name = "tname";
            this.tname.PasswordChar = '\0';
            this.tname.PlaceholderText = "";
            this.tname.SelectedText = "";
            this.tname.Size = new System.Drawing.Size(301, 36);
            this.tname.TabIndex = 4;
            // 
            // taddress
            // 
            this.taddress.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.taddress.DefaultText = "";
            this.taddress.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.taddress.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.taddress.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.taddress.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.taddress.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.taddress.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.taddress.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.taddress.Location = new System.Drawing.Point(673, 183);
            this.taddress.Name = "taddress";
            this.taddress.PasswordChar = '\0';
            this.taddress.PlaceholderText = "";
            this.taddress.SelectedText = "";
            this.taddress.Size = new System.Drawing.Size(301, 36);
            this.taddress.TabIndex = 5;
            // 
            // tage
            // 
            this.tage.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tage.DefaultText = "";
            this.tage.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tage.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tage.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tage.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tage.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tage.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tage.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tage.Location = new System.Drawing.Point(673, 238);
            this.tage.Name = "tage";
            this.tage.PasswordChar = '\0';
            this.tage.PlaceholderText = "";
            this.tage.SelectedText = "";
            this.tage.Size = new System.Drawing.Size(301, 36);
            this.tage.TabIndex = 6;
            // 
            // bgetdata
            // 
            this.bgetdata.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bgetdata.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bgetdata.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bgetdata.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bgetdata.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.bgetdata.ForeColor = System.Drawing.Color.White;
            this.bgetdata.Location = new System.Drawing.Point(596, 79);
            this.bgetdata.Name = "bgetdata";
            this.bgetdata.Size = new System.Drawing.Size(180, 45);
            this.bgetdata.TabIndex = 8;
            this.bgetdata.Text = "Get Data";
            this.bgetdata.Click += new System.EventHandler(this.bgetdata_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(607, 138);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(607, 183);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "Address";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(607, 238);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 20);
            this.label3.TabIndex = 11;
            this.label3.Text = "Age";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(607, 303);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 20);
            this.label4.TabIndex = 12;
            this.label4.Text = "Civil Status";
            // 
            // cbstatus
            // 
            this.cbstatus.BackColor = System.Drawing.Color.Transparent;
            this.cbstatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbstatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbstatus.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbstatus.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbstatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbstatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbstatus.ItemHeight = 30;
            this.cbstatus.Items.AddRange(new object[] {
            "Single",
            "Married"});
            this.cbstatus.Location = new System.Drawing.Point(701, 295);
            this.cbstatus.Name = "cbstatus";
            this.cbstatus.Size = new System.Drawing.Size(140, 36);
            this.cbstatus.StartIndex = 0;
            this.cbstatus.TabIndex = 13;
            // 
            // frm_test_ocr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1056, 630);
            this.Controls.Add(this.cbstatus);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bgetdata);
            this.Controls.Add(this.tage);
            this.Controls.Add(this.taddress);
            this.Controls.Add(this.tname);
            this.Controls.Add(this.tpath);
            this.Controls.Add(this.pbphoto);
            this.Controls.Add(this.bupload);
            this.Name = "frm_test_ocr";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frm_test_ocr";
            this.Load += new System.EventHandler(this.frm_test_ocr_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbphoto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button bupload;
        private Guna.UI2.WinForms.Guna2PictureBox pbphoto;
        private Guna.UI2.WinForms.Guna2TextBox tpath;
        private Guna.UI2.WinForms.Guna2TextBox tname;
        private Guna.UI2.WinForms.Guna2TextBox taddress;
        private Guna.UI2.WinForms.Guna2TextBox tage;
        private Guna.UI2.WinForms.Guna2Button bgetdata;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private Guna.UI2.WinForms.Guna2ComboBox cbstatus;
    }
}
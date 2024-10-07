namespace rct_lmis.ADMIN_SECTION
{
    partial class frm_home_ADMIN_collectors
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ltitle = new System.Windows.Forms.Label();
            this.pleft = new System.Windows.Forms.Panel();
            this.trecbookno = new Guna.UI2.WinForms.Guna2TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tbankaccountno = new Guna.UI2.WinForms.Guna2TextBox();
            this.bedit = new Guna.UI2.WinForms.Guna2Button();
            this.buploaddoc = new Guna.UI2.WinForms.Guna2Button();
            this.badd = new Guna.UI2.WinForms.Guna2Button();
            this.trole = new Guna.UI2.WinForms.Guna2TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.dtdateemp = new System.Windows.Forms.DateTimePicker();
            this.bsave = new Guna.UI2.WinForms.Guna2Button();
            this.cbempstatus = new System.Windows.Forms.ComboBox();
            this.temail = new Guna.UI2.WinForms.Guna2TextBox();
            this.tcontactnoalt = new Guna.UI2.WinForms.Guna2TextBox();
            this.tcontactno = new Guna.UI2.WinForms.Guna2TextBox();
            this.taddress = new Guna.UI2.WinForms.Guna2TextBox();
            this.tname = new Guna.UI2.WinForms.Guna2TextBox();
            this.cbarea = new System.Windows.Forms.ComboBox();
            this.tidno = new Guna.UI2.WinForms.Guna2TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvdatacollector = new Guna.UI2.WinForms.Guna2DataGridView();
            this.lnorecord = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.bcreate = new Guna.UI2.WinForms.Guna2Button();
            this.bexport = new Guna.UI2.WinForms.Guna2Button();
            this.brefresh = new Guna.UI2.WinForms.Guna2CircleButton();
            this.tsearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.listarea_routes = new System.Windows.Forms.ListBox();
            this.bremoveroute = new Guna.UI2.WinForms.Guna2Button();
            this.panel1.SuspendLayout();
            this.pleft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvdatacollector)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.panel1.Controls.Add(this.bexport);
            this.panel1.Controls.Add(this.brefresh);
            this.panel1.Controls.Add(this.tsearch);
            this.panel1.Controls.Add(this.ltitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1200, 52);
            this.panel1.TabIndex = 2;
            // 
            // ltitle
            // 
            this.ltitle.AutoSize = true;
            this.ltitle.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ltitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.ltitle.Location = new System.Drawing.Point(19, 12);
            this.ltitle.Name = "ltitle";
            this.ltitle.Size = new System.Drawing.Size(298, 30);
            this.ltitle.TabIndex = 2;
            this.ltitle.Text = "COLLECTORS INFORMATION";
            // 
            // pleft
            // 
            this.pleft.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pleft.Controls.Add(this.bremoveroute);
            this.pleft.Controls.Add(this.listarea_routes);
            this.pleft.Controls.Add(this.trecbookno);
            this.pleft.Controls.Add(this.label14);
            this.pleft.Controls.Add(this.tbankaccountno);
            this.pleft.Controls.Add(this.bcreate);
            this.pleft.Controls.Add(this.bedit);
            this.pleft.Controls.Add(this.buploaddoc);
            this.pleft.Controls.Add(this.badd);
            this.pleft.Controls.Add(this.trole);
            this.pleft.Controls.Add(this.label13);
            this.pleft.Controls.Add(this.dtdateemp);
            this.pleft.Controls.Add(this.bsave);
            this.pleft.Controls.Add(this.cbempstatus);
            this.pleft.Controls.Add(this.temail);
            this.pleft.Controls.Add(this.tcontactnoalt);
            this.pleft.Controls.Add(this.tcontactno);
            this.pleft.Controls.Add(this.taddress);
            this.pleft.Controls.Add(this.tname);
            this.pleft.Controls.Add(this.cbarea);
            this.pleft.Controls.Add(this.tidno);
            this.pleft.Controls.Add(this.label10);
            this.pleft.Controls.Add(this.label9);
            this.pleft.Controls.Add(this.label8);
            this.pleft.Controls.Add(this.label7);
            this.pleft.Controls.Add(this.label6);
            this.pleft.Controls.Add(this.label5);
            this.pleft.Controls.Add(this.label4);
            this.pleft.Controls.Add(this.label3);
            this.pleft.Controls.Add(this.label2);
            this.pleft.Controls.Add(this.label12);
            this.pleft.Controls.Add(this.label34);
            this.pleft.Controls.Add(this.label1);
            this.pleft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pleft.Location = new System.Drawing.Point(0, 52);
            this.pleft.Name = "pleft";
            this.pleft.Size = new System.Drawing.Size(346, 648);
            this.pleft.TabIndex = 3;
            // 
            // trecbookno
            // 
            this.trecbookno.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.trecbookno.DefaultText = "";
            this.trecbookno.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.trecbookno.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.trecbookno.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.trecbookno.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.trecbookno.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.trecbookno.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.trecbookno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.trecbookno.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.trecbookno.Location = new System.Drawing.Point(164, 482);
            this.trecbookno.Name = "trecbookno";
            this.trecbookno.PasswordChar = '\0';
            this.trecbookno.PlaceholderText = "n/a";
            this.trecbookno.SelectedText = "";
            this.trecbookno.Size = new System.Drawing.Size(168, 24);
            this.trecbookno.TabIndex = 159;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label14.Location = new System.Drawing.Point(21, 487);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(101, 15);
            this.label14.TabIndex = 158;
            this.label14.Text = "Receipt Book No.:";
            // 
            // tbankaccountno
            // 
            this.tbankaccountno.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbankaccountno.DefaultText = "";
            this.tbankaccountno.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tbankaccountno.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tbankaccountno.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tbankaccountno.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tbankaccountno.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tbankaccountno.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.tbankaccountno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tbankaccountno.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tbankaccountno.Location = new System.Drawing.Point(11, 548);
            this.tbankaccountno.Multiline = true;
            this.tbankaccountno.Name = "tbankaccountno";
            this.tbankaccountno.PasswordChar = '\0';
            this.tbankaccountno.PlaceholderText = "BPI - xxxxxxx, BDO - xxxxxx";
            this.tbankaccountno.SelectedText = "";
            this.tbankaccountno.Size = new System.Drawing.Size(321, 50);
            this.tbankaccountno.TabIndex = 156;
            // 
            // bedit
            // 
            this.bedit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bedit.BackColor = System.Drawing.Color.Transparent;
            this.bedit.BorderColor = System.Drawing.Color.Transparent;
            this.bedit.BorderRadius = 2;
            this.bedit.BorderThickness = 1;
            this.bedit.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bedit.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bedit.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bedit.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bedit.Enabled = false;
            this.bedit.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.bedit.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.bedit.ForeColor = System.Drawing.Color.White;
            this.bedit.ImageSize = new System.Drawing.Size(28, 28);
            this.bedit.Location = new System.Drawing.Point(191, 6);
            this.bedit.Name = "bedit";
            this.bedit.Size = new System.Drawing.Size(59, 23);
            this.bedit.TabIndex = 154;
            this.bedit.Text = "Edit";
            this.bedit.Click += new System.EventHandler(this.bedit_Click);
            // 
            // buploaddoc
            // 
            this.buploaddoc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buploaddoc.BackColor = System.Drawing.Color.Transparent;
            this.buploaddoc.BorderColor = System.Drawing.Color.Transparent;
            this.buploaddoc.BorderRadius = 2;
            this.buploaddoc.BorderThickness = 1;
            this.buploaddoc.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.buploaddoc.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.buploaddoc.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.buploaddoc.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.buploaddoc.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.buploaddoc.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.buploaddoc.ForeColor = System.Drawing.Color.White;
            this.buploaddoc.ImageSize = new System.Drawing.Size(28, 28);
            this.buploaddoc.Location = new System.Drawing.Point(9, 604);
            this.buploaddoc.Name = "buploaddoc";
            this.buploaddoc.Size = new System.Drawing.Size(162, 30);
            this.buploaddoc.TabIndex = 153;
            this.buploaddoc.Text = "Upload Documents";
            this.buploaddoc.Click += new System.EventHandler(this.buploaddoc_Click);
            // 
            // badd
            // 
            this.badd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.badd.BackColor = System.Drawing.Color.Transparent;
            this.badd.BorderColor = System.Drawing.Color.Transparent;
            this.badd.BorderRadius = 2;
            this.badd.BorderThickness = 1;
            this.badd.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.badd.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.badd.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.badd.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.badd.Enabled = false;
            this.badd.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.badd.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.badd.ForeColor = System.Drawing.Color.White;
            this.badd.ImageSize = new System.Drawing.Size(28, 28);
            this.badd.Location = new System.Drawing.Point(118, 6);
            this.badd.Name = "badd";
            this.badd.Size = new System.Drawing.Size(66, 23);
            this.badd.TabIndex = 14;
            this.badd.Text = "Create";
            this.badd.Click += new System.EventHandler(this.badd_Click);
            // 
            // trole
            // 
            this.trole.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.trole.DefaultText = "";
            this.trole.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.trole.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.trole.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.trole.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.trole.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.trole.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.trole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.trole.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.trole.Location = new System.Drawing.Point(164, 452);
            this.trole.Name = "trole";
            this.trole.PasswordChar = '\0';
            this.trole.PlaceholderText = "n/a";
            this.trole.SelectedText = "";
            this.trole.Size = new System.Drawing.Size(168, 24);
            this.trole.TabIndex = 149;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.label13.Location = new System.Drawing.Point(7, 367);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(204, 21);
            this.label13.TabIndex = 148;
            this.label13.Text = "Employment Information";
            // 
            // dtdateemp
            // 
            this.dtdateemp.CustomFormat = "";
            this.dtdateemp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtdateemp.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtdateemp.Location = new System.Drawing.Point(164, 423);
            this.dtdateemp.Name = "dtdateemp";
            this.dtdateemp.Size = new System.Drawing.Size(121, 23);
            this.dtdateemp.TabIndex = 147;
            this.dtdateemp.Value = new System.DateTime(2024, 8, 28, 0, 0, 0, 0);
            // 
            // bsave
            // 
            this.bsave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bsave.Animated = true;
            this.bsave.BorderColor = System.Drawing.Color.Transparent;
            this.bsave.BorderRadius = 2;
            this.bsave.BorderThickness = 1;
            this.bsave.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bsave.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bsave.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bsave.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bsave.Enabled = false;
            this.bsave.FillColor = System.Drawing.Color.SeaGreen;
            this.bsave.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.bsave.ForeColor = System.Drawing.Color.White;
            this.bsave.ImageSize = new System.Drawing.Size(28, 28);
            this.bsave.Location = new System.Drawing.Point(256, 6);
            this.bsave.Name = "bsave";
            this.bsave.Size = new System.Drawing.Size(76, 23);
            this.bsave.TabIndex = 152;
            this.bsave.Text = "Update";
            this.bsave.Click += new System.EventHandler(this.bsave_Click);
            // 
            // cbempstatus
            // 
            this.cbempstatus.FormattingEnabled = true;
            this.cbempstatus.Items.AddRange(new object[] {
            "Active",
            "Inactive",
            "Resigned"});
            this.cbempstatus.Location = new System.Drawing.Point(164, 395);
            this.cbempstatus.Name = "cbempstatus";
            this.cbempstatus.Size = new System.Drawing.Size(121, 21);
            this.cbempstatus.TabIndex = 146;
            // 
            // temail
            // 
            this.temail.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.temail.DefaultText = "";
            this.temail.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.temail.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.temail.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.temail.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.temail.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.temail.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.temail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.temail.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.temail.Location = new System.Drawing.Point(126, 340);
            this.temail.Name = "temail";
            this.temail.PasswordChar = '\0';
            this.temail.PlaceholderText = "n/a";
            this.temail.SelectedText = "";
            this.temail.Size = new System.Drawing.Size(206, 24);
            this.temail.TabIndex = 145;
            // 
            // tcontactnoalt
            // 
            this.tcontactnoalt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tcontactnoalt.DefaultText = "";
            this.tcontactnoalt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tcontactnoalt.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tcontactnoalt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tcontactnoalt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tcontactnoalt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tcontactnoalt.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tcontactnoalt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tcontactnoalt.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tcontactnoalt.Location = new System.Drawing.Point(126, 310);
            this.tcontactnoalt.Name = "tcontactnoalt";
            this.tcontactnoalt.PasswordChar = '\0';
            this.tcontactnoalt.PlaceholderText = "n/a";
            this.tcontactnoalt.SelectedText = "";
            this.tcontactnoalt.Size = new System.Drawing.Size(206, 24);
            this.tcontactnoalt.TabIndex = 144;
            // 
            // tcontactno
            // 
            this.tcontactno.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tcontactno.DefaultText = "";
            this.tcontactno.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tcontactno.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tcontactno.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tcontactno.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tcontactno.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tcontactno.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tcontactno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tcontactno.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tcontactno.Location = new System.Drawing.Point(126, 281);
            this.tcontactno.Name = "tcontactno";
            this.tcontactno.PasswordChar = '\0';
            this.tcontactno.PlaceholderText = "n/a";
            this.tcontactno.SelectedText = "";
            this.tcontactno.Size = new System.Drawing.Size(206, 24);
            this.tcontactno.TabIndex = 143;
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
            this.taddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.taddress.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.taddress.Location = new System.Drawing.Point(126, 225);
            this.taddress.Multiline = true;
            this.taddress.Name = "taddress";
            this.taddress.PasswordChar = '\0';
            this.taddress.PlaceholderText = "n/a";
            this.taddress.SelectedText = "";
            this.taddress.Size = new System.Drawing.Size(206, 50);
            this.taddress.TabIndex = 142;
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
            this.tname.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tname.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tname.Location = new System.Drawing.Point(126, 195);
            this.tname.Name = "tname";
            this.tname.PasswordChar = '\0';
            this.tname.PlaceholderText = "n/a";
            this.tname.SelectedText = "";
            this.tname.Size = new System.Drawing.Size(206, 24);
            this.tname.TabIndex = 141;
            this.tname.Enter += new System.EventHandler(this.tname_Enter);
            // 
            // cbarea
            // 
            this.cbarea.FormattingEnabled = true;
            this.cbarea.Items.AddRange(new object[] {
            "Area 1",
            "Area 2",
            "Area 3"});
            this.cbarea.Location = new System.Drawing.Point(104, 70);
            this.cbarea.Name = "cbarea";
            this.cbarea.Size = new System.Drawing.Size(121, 21);
            this.cbarea.TabIndex = 140;
            this.cbarea.SelectedIndexChanged += new System.EventHandler(this.cbarea_SelectedIndexChanged);
            // 
            // tidno
            // 
            this.tidno.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tidno.DefaultText = "";
            this.tidno.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tidno.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tidno.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tidno.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tidno.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tidno.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tidno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tidno.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tidno.Location = new System.Drawing.Point(104, 161);
            this.tidno.Name = "tidno";
            this.tidno.PasswordChar = '\0';
            this.tidno.PlaceholderText = "n/a";
            this.tidno.SelectedText = "";
            this.tidno.Size = new System.Drawing.Size(228, 24);
            this.tidno.TabIndex = 139;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label10.Location = new System.Drawing.Point(9, 530);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(162, 15);
            this.label10.TabIndex = 136;
            this.label10.Text = "Bank Account Details (if any):";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label9.Location = new System.Drawing.Point(21, 459);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(101, 15);
            this.label9.TabIndex = 135;
            this.label9.Text = "Role/Designation:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label8.Location = new System.Drawing.Point(21, 345);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 15);
            this.label8.TabIndex = 134;
            this.label8.Text = "Email Address:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label7.Location = new System.Drawing.Point(21, 315);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 15);
            this.label7.TabIndex = 133;
            this.label7.Text = "Alternate C. No.:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label6.Location = new System.Drawing.Point(22, 427);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(132, 15);
            this.label6.TabIndex = 132;
            this.label6.Text = "Employment Start Date:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label5.Location = new System.Drawing.Point(21, 397);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 15);
            this.label5.TabIndex = 131;
            this.label5.Text = "Employment Status:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label4.Location = new System.Drawing.Point(21, 230);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 15);
            this.label4.TabIndex = 130;
            this.label4.Text = "Address:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Location = new System.Drawing.Point(21, 288);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 15);
            this.label3.TabIndex = 129;
            this.label3.Text = "Contact Number:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(21, 199);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 15);
            this.label2.TabIndex = 128;
            this.label2.Text = "Name:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label12.Location = new System.Drawing.Point(21, 166);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(68, 15);
            this.label12.TabIndex = 124;
            this.label12.Text = "ID Number:";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.BackColor = System.Drawing.Color.Transparent;
            this.label34.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label34.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label34.Location = new System.Drawing.Point(21, 72);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(68, 15);
            this.label34.TabIndex = 77;
            this.label34.Text = "Area Route:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.label1.Location = new System.Drawing.Point(5, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 21);
            this.label1.TabIndex = 5;
            this.label1.Text = "Collector Profile";
            // 
            // dgvdatacollector
            // 
            this.dgvdatacollector.AllowUserToAddRows = false;
            this.dgvdatacollector.AllowUserToDeleteRows = false;
            this.dgvdatacollector.AllowUserToResizeColumns = false;
            this.dgvdatacollector.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvdatacollector.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvdatacollector.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvdatacollector.ColumnHeadersHeight = 30;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvdatacollector.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvdatacollector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvdatacollector.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvdatacollector.Location = new System.Drawing.Point(346, 52);
            this.dgvdatacollector.Name = "dgvdatacollector";
            this.dgvdatacollector.ReadOnly = true;
            this.dgvdatacollector.RowHeadersVisible = false;
            this.dgvdatacollector.RowTemplate.Height = 30;
            this.dgvdatacollector.Size = new System.Drawing.Size(854, 648);
            this.dgvdatacollector.TabIndex = 6;
            this.dgvdatacollector.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.White;
            this.dgvdatacollector.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvdatacollector.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvdatacollector.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvdatacollector.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvdatacollector.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvdatacollector.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvdatacollector.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvdatacollector.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.dgvdatacollector.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvdatacollector.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvdatacollector.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvdatacollector.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvdatacollector.ThemeStyle.HeaderStyle.Height = 30;
            this.dgvdatacollector.ThemeStyle.ReadOnly = true;
            this.dgvdatacollector.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvdatacollector.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvdatacollector.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvdatacollector.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvdatacollector.ThemeStyle.RowsStyle.Height = 30;
            this.dgvdatacollector.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            this.dgvdatacollector.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvdatacollector.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvdatacollector_CellClick);
            this.dgvdatacollector.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvdatacollector_CellContentClick);
            this.dgvdatacollector.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvdatacollector_DataBindingComplete);
            // 
            // lnorecord
            // 
            this.lnorecord.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lnorecord.AutoSize = true;
            this.lnorecord.BackColor = System.Drawing.Color.White;
            this.lnorecord.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lnorecord.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lnorecord.Location = new System.Drawing.Point(727, 115);
            this.lnorecord.Name = "lnorecord";
            this.lnorecord.Size = new System.Drawing.Size(101, 15);
            this.lnorecord.TabIndex = 138;
            this.lnorecord.Text = "no records found.";
            // 
            // bcreate
            // 
            this.bcreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bcreate.Animated = true;
            this.bcreate.BorderColor = System.Drawing.Color.Transparent;
            this.bcreate.BorderRadius = 2;
            this.bcreate.BorderThickness = 1;
            this.bcreate.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bcreate.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bcreate.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bcreate.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bcreate.FillColor = System.Drawing.Color.SteelBlue;
            this.bcreate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.bcreate.ForeColor = System.Drawing.Color.White;
            this.bcreate.Image = global::rct_lmis.Properties.Resources.icons8_add_48__2_;
            this.bcreate.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.bcreate.ImageSize = new System.Drawing.Size(15, 15);
            this.bcreate.Location = new System.Drawing.Point(45, 6);
            this.bcreate.Name = "bcreate";
            this.bcreate.Size = new System.Drawing.Size(67, 23);
            this.bcreate.TabIndex = 155;
            this.bcreate.Text = "New";
            this.bcreate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.bcreate, "Create New");
            this.bcreate.Click += new System.EventHandler(this.bcreate_Click);
            // 
            // bexport
            // 
            this.bexport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bexport.BorderColor = System.Drawing.Color.Gainsboro;
            this.bexport.BorderRadius = 2;
            this.bexport.BorderThickness = 1;
            this.bexport.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bexport.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bexport.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bexport.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bexport.FillColor = System.Drawing.Color.White;
            this.bexport.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.bexport.ForeColor = System.Drawing.Color.White;
            this.bexport.Image = global::rct_lmis.Properties.Resources.icons8_export_excel_48;
            this.bexport.ImageSize = new System.Drawing.Size(28, 28);
            this.bexport.Location = new System.Drawing.Point(1117, 10);
            this.bexport.Name = "bexport";
            this.bexport.Size = new System.Drawing.Size(35, 35);
            this.bexport.TabIndex = 13;
            this.bexport.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // brefresh
            // 
            this.brefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.brefresh.BorderColor = System.Drawing.Color.LightGray;
            this.brefresh.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.brefresh.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.brefresh.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.brefresh.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.brefresh.FillColor = System.Drawing.Color.White;
            this.brefresh.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.brefresh.ForeColor = System.Drawing.Color.White;
            this.brefresh.Image = global::rct_lmis.Properties.Resources.icons8_refresh_48;
            this.brefresh.ImageSize = new System.Drawing.Size(28, 28);
            this.brefresh.Location = new System.Drawing.Point(1158, 12);
            this.brefresh.Name = "brefresh";
            this.brefresh.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.brefresh.Size = new System.Drawing.Size(30, 30);
            this.brefresh.TabIndex = 4;
            this.brefresh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.brefresh.TextOffset = new System.Drawing.Point(10, 0);
            this.toolTip1.SetToolTip(this.brefresh, "Refresh Data");
            this.brefresh.Click += new System.EventHandler(this.brefresh_Click);
            // 
            // tsearch
            // 
            this.tsearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tsearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tsearch.DefaultText = "";
            this.tsearch.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.tsearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.tsearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tsearch.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.tsearch.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tsearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tsearch.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.tsearch.IconLeft = global::rct_lmis.Properties.Resources.icons8_search_48;
            this.tsearch.Location = new System.Drawing.Point(808, 9);
            this.tsearch.Name = "tsearch";
            this.tsearch.PasswordChar = '\0';
            this.tsearch.PlaceholderText = "search any keyword...";
            this.tsearch.SelectedText = "";
            this.tsearch.Size = new System.Drawing.Size(303, 36);
            this.tsearch.TabIndex = 9;
            // 
            // listarea_routes
            // 
            this.listarea_routes.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listarea_routes.FormattingEnabled = true;
            this.listarea_routes.Location = new System.Drawing.Point(104, 97);
            this.listarea_routes.Name = "listarea_routes";
            this.listarea_routes.Size = new System.Drawing.Size(228, 56);
            this.listarea_routes.TabIndex = 160;
            // 
            // bremoveroute
            // 
            this.bremoveroute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bremoveroute.BackColor = System.Drawing.Color.Transparent;
            this.bremoveroute.BorderColor = System.Drawing.Color.Transparent;
            this.bremoveroute.BorderRadius = 2;
            this.bremoveroute.BorderThickness = 1;
            this.bremoveroute.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bremoveroute.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bremoveroute.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bremoveroute.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bremoveroute.Enabled = false;
            this.bremoveroute.FillColor = System.Drawing.Color.Gainsboro;
            this.bremoveroute.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.bremoveroute.ForeColor = System.Drawing.Color.White;
            this.bremoveroute.Image = global::rct_lmis.Properties.Resources.icons8_close_window_48;
            this.bremoveroute.Location = new System.Drawing.Point(231, 68);
            this.bremoveroute.Name = "bremoveroute";
            this.bremoveroute.Size = new System.Drawing.Size(28, 23);
            this.bremoveroute.TabIndex = 161;
            this.toolTip1.SetToolTip(this.bremoveroute, "Remove Route");
            this.bremoveroute.Click += new System.EventHandler(this.bremoveroute_Click);
            // 
            // frm_home_ADMIN_collectors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.lnorecord);
            this.Controls.Add(this.dgvdatacollector);
            this.Controls.Add(this.pleft);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_home_ADMIN_collectors";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Collectors";
            this.Load += new System.EventHandler(this.frm_home_ADMIN_collectors_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pleft.ResumeLayout(false);
            this.pleft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvdatacollector)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Guna.UI2.WinForms.Guna2CircleButton brefresh;
        private System.Windows.Forms.Label ltitle;
        private Guna.UI2.WinForms.Guna2Button bexport;
        private Guna.UI2.WinForms.Guna2TextBox tsearch;
        private System.Windows.Forms.Panel pleft;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2DataGridView dgvdatacollector;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbarea;
        private Guna.UI2.WinForms.Guna2TextBox tidno;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lnorecord;
        private Guna.UI2.WinForms.Guna2TextBox trole;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.DateTimePicker dtdateemp;
        private System.Windows.Forms.ComboBox cbempstatus;
        private Guna.UI2.WinForms.Guna2TextBox temail;
        private Guna.UI2.WinForms.Guna2TextBox tcontactnoalt;
        private Guna.UI2.WinForms.Guna2TextBox tcontactno;
        private Guna.UI2.WinForms.Guna2TextBox taddress;
        private Guna.UI2.WinForms.Guna2TextBox tname;
        private Guna.UI2.WinForms.Guna2Button badd;
        private Guna.UI2.WinForms.Guna2Button bsave;
        private Guna.UI2.WinForms.Guna2Button buploaddoc;
        private Guna.UI2.WinForms.Guna2Button bedit;
        private Guna.UI2.WinForms.Guna2Button bcreate;
        private System.Windows.Forms.ToolTip toolTip1;
        private Guna.UI2.WinForms.Guna2TextBox tbankaccountno;
        private Guna.UI2.WinForms.Guna2TextBox trecbookno;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ListBox listarea_routes;
        private Guna.UI2.WinForms.Guna2Button bremoveroute;
    }
}
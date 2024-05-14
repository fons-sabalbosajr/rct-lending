
namespace rct_lmis
{
    partial class frm_home_loans
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.baddnew = new Guna.UI2.WinForms.Guna2Button();
            this.ltitle = new System.Windows.Forms.Label();
            this.pbot = new System.Windows.Forms.Panel();
            this.lnorecord = new System.Windows.Forms.Label();
            this.dgvdata = new Guna.UI2.WinForms.Guna2DataGridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.bmore = new Guna.UI2.WinForms.Guna2Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.luser = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ltotal = new System.Windows.Forms.Label();
            this.guna2VSeparator3 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.guna2VSeparator2 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.pheaders = new System.Windows.Forms.Panel();
            this.bexport = new Guna.UI2.WinForms.Guna2Button();
            this.bprint = new Guna.UI2.WinForms.Guna2Button();
            this.cbsort = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.guna2Separator1 = new Guna.UI2.WinForms.Guna2Separator();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbcollector = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cbstatus = new Guna.UI2.WinForms.Guna2ComboBox();
            this.tsearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.bhelp = new Guna.UI2.WinForms.Guna2CircleButton();
            this.panel1.SuspendLayout();
            this.pbot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvdata)).BeginInit();
            this.panel3.SuspendLayout();
            this.pheaders.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.panel1.Controls.Add(this.bhelp);
            this.panel1.Controls.Add(this.baddnew);
            this.panel1.Controls.Add(this.ltitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1300, 52);
            this.panel1.TabIndex = 0;
            // 
            // baddnew
            // 
            this.baddnew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.baddnew.BorderRadius = 4;
            this.baddnew.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.baddnew.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.baddnew.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.baddnew.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.baddnew.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.baddnew.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baddnew.ForeColor = System.Drawing.Color.White;
            this.baddnew.Location = new System.Drawing.Point(1077, 5);
            this.baddnew.Name = "baddnew";
            this.baddnew.Size = new System.Drawing.Size(209, 41);
            this.baddnew.TabIndex = 14;
            this.baddnew.Text = "NEW LOAN TRANSACTION";
            this.toolTip1.SetToolTip(this.baddnew, "Add Loan Transaction");
            this.baddnew.Click += new System.EventHandler(this.baddnew_Click);
            // 
            // ltitle
            // 
            this.ltitle.AutoSize = true;
            this.ltitle.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ltitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.ltitle.Location = new System.Drawing.Point(19, 11);
            this.ltitle.Name = "ltitle";
            this.ltitle.Size = new System.Drawing.Size(83, 30);
            this.ltitle.TabIndex = 2;
            this.ltitle.Text = "LOANS";
            // 
            // pbot
            // 
            this.pbot.BackColor = System.Drawing.Color.White;
            this.pbot.Controls.Add(this.lnorecord);
            this.pbot.Controls.Add(this.dgvdata);
            this.pbot.Controls.Add(this.panel3);
            this.pbot.Controls.Add(this.pheaders);
            this.pbot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbot.Location = new System.Drawing.Point(0, 52);
            this.pbot.Name = "pbot";
            this.pbot.Size = new System.Drawing.Size(1300, 798);
            this.pbot.TabIndex = 3;
            // 
            // lnorecord
            // 
            this.lnorecord.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lnorecord.AutoSize = true;
            this.lnorecord.BackColor = System.Drawing.Color.Transparent;
            this.lnorecord.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnorecord.ForeColor = System.Drawing.Color.Silver;
            this.lnorecord.Location = new System.Drawing.Point(550, 156);
            this.lnorecord.Name = "lnorecord";
            this.lnorecord.Size = new System.Drawing.Size(176, 25);
            this.lnorecord.TabIndex = 9;
            this.lnorecord.Text = "No records found.";
            // 
            // dgvdata
            // 
            this.dgvdata.AllowUserToAddRows = false;
            this.dgvdata.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.dgvdata.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvdata.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvdata.ColumnHeadersHeight = 4;
            this.dgvdata.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvdata.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvdata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvdata.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvdata.Location = new System.Drawing.Point(0, 66);
            this.dgvdata.Name = "dgvdata";
            this.dgvdata.ReadOnly = true;
            this.dgvdata.RowHeadersVisible = false;
            this.dgvdata.Size = new System.Drawing.Size(1300, 699);
            this.dgvdata.TabIndex = 2;
            this.dgvdata.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.White;
            this.dgvdata.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvdata.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvdata.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvdata.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvdata.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvdata.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvdata.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvdata.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.White;
            this.dgvdata.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvdata.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvdata.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvdata.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvdata.ThemeStyle.HeaderStyle.Height = 4;
            this.dgvdata.ThemeStyle.ReadOnly = true;
            this.dgvdata.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.dgvdata.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvdata.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvdata.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvdata.ThemeStyle.RowsStyle.Height = 22;
            this.dgvdata.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            this.dgvdata.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvdata.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvdata_CellDoubleClick);
            this.dgvdata.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvdata_DataBindingComplete);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel3.Controls.Add(this.bmore);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.luser);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.ltotal);
            this.panel3.Controls.Add(this.guna2VSeparator3);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.guna2VSeparator2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 765);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1300, 33);
            this.panel3.TabIndex = 1;
            // 
            // bmore
            // 
            this.bmore.BorderRadius = 2;
            this.bmore.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bmore.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bmore.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bmore.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bmore.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(66)))), ((int)(((byte)(87)))));
            this.bmore.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bmore.ForeColor = System.Drawing.Color.White;
            this.bmore.Image = global::rct_lmis.Properties.Resources.icons8_more_48;
            this.bmore.Location = new System.Drawing.Point(910, 7);
            this.bmore.Name = "bmore";
            this.bmore.Size = new System.Drawing.Size(30, 20);
            this.bmore.TabIndex = 0;
            this.bmore.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.bmore, "View collectors status");
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label10.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(760, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(107, 15);
            this.label10.TabIndex = 13;
            this.label10.Text = "Juan Dela Cruz (45)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.DimGray;
            this.label9.Location = new System.Drawing.Point(677, 10);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(81, 15);
            this.label9.TabIndex = 12;
            this.label9.Text = "Top Collector:";
            // 
            // luser
            // 
            this.luser.AutoSize = true;
            this.luser.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.luser.ForeColor = System.Drawing.Color.DimGray;
            this.luser.Location = new System.Drawing.Point(1017, 10);
            this.luser.Name = "luser";
            this.luser.Size = new System.Drawing.Size(78, 15);
            this.luser.TabIndex = 11;
            this.luser.Text = "staff/encoder";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(524, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 15);
            this.label8.TabIndex = 10;
            this.label8.Text = "For Penalty (23)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Gold;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label7.Location = new System.Drawing.Point(467, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 15);
            this.label7.TabIndex = 9;
            this.label7.Text = "Due (50)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(398, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 15);
            this.label6.TabIndex = 8;
            this.label6.Text = "Active(36)";
            // 
            // ltotal
            // 
            this.ltotal.AutoSize = true;
            this.ltotal.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ltotal.ForeColor = System.Drawing.Color.DimGray;
            this.ltotal.Location = new System.Drawing.Point(89, 9);
            this.ltotal.Name = "ltotal";
            this.ltotal.Size = new System.Drawing.Size(22, 15);
            this.ltotal.TabIndex = 7;
            this.ltotal.Text = "---";
            // 
            // guna2VSeparator3
            // 
            this.guna2VSeparator3.Location = new System.Drawing.Point(940, 9);
            this.guna2VSeparator3.Name = "guna2VSeparator3";
            this.guna2VSeparator3.Size = new System.Drawing.Size(10, 17);
            this.guna2VSeparator3.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.DimGray;
            this.label5.Location = new System.Drawing.Point(296, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 15);
            this.label5.TabIndex = 5;
            this.label5.Text = "Loans per Status:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.DimGray;
            this.label4.Location = new System.Drawing.Point(948, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 15);
            this.label4.TabIndex = 4;
            this.label4.Text = "Active User:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DimGray;
            this.label3.Location = new System.Drawing.Point(14, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "Total Loans:";
            // 
            // guna2VSeparator2
            // 
            this.guna2VSeparator2.Location = new System.Drawing.Point(285, 8);
            this.guna2VSeparator2.Name = "guna2VSeparator2";
            this.guna2VSeparator2.Size = new System.Drawing.Size(10, 17);
            this.guna2VSeparator2.TabIndex = 2;
            // 
            // pheaders
            // 
            this.pheaders.BackColor = System.Drawing.Color.White;
            this.pheaders.Controls.Add(this.bexport);
            this.pheaders.Controls.Add(this.bprint);
            this.pheaders.Controls.Add(this.cbsort);
            this.pheaders.Controls.Add(this.label11);
            this.pheaders.Controls.Add(this.guna2Separator1);
            this.pheaders.Controls.Add(this.label2);
            this.pheaders.Controls.Add(this.label1);
            this.pheaders.Controls.Add(this.cbcollector);
            this.pheaders.Controls.Add(this.cbstatus);
            this.pheaders.Controls.Add(this.tsearch);
            this.pheaders.Dock = System.Windows.Forms.DockStyle.Top;
            this.pheaders.Location = new System.Drawing.Point(0, 0);
            this.pheaders.Name = "pheaders";
            this.pheaders.Size = new System.Drawing.Size(1300, 66);
            this.pheaders.TabIndex = 0;
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
            this.bexport.Location = new System.Drawing.Point(1210, 15);
            this.bexport.Name = "bexport";
            this.bexport.Size = new System.Drawing.Size(35, 35);
            this.bexport.TabIndex = 11;
            this.bexport.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.bexport, "Export to Excel");
            // 
            // bprint
            // 
            this.bprint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bprint.BorderColor = System.Drawing.Color.Gainsboro;
            this.bprint.BorderRadius = 2;
            this.bprint.BorderThickness = 1;
            this.bprint.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bprint.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bprint.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bprint.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bprint.FillColor = System.Drawing.Color.White;
            this.bprint.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.bprint.ForeColor = System.Drawing.Color.White;
            this.bprint.Image = global::rct_lmis.Properties.Resources.icons8_print_48;
            this.bprint.ImageSize = new System.Drawing.Size(28, 28);
            this.bprint.Location = new System.Drawing.Point(1251, 15);
            this.bprint.Name = "bprint";
            this.bprint.Size = new System.Drawing.Size(35, 35);
            this.bprint.TabIndex = 10;
            this.bprint.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.bprint, "Print Table");
            // 
            // cbsort
            // 
            this.cbsort.BackColor = System.Drawing.Color.Transparent;
            this.cbsort.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbsort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbsort.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbsort.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbsort.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cbsort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbsort.ItemHeight = 30;
            this.cbsort.Items.AddRange(new object[] {
            "default",
            "name",
            "account no.",
            "top loan amount"});
            this.cbsort.Location = new System.Drawing.Point(469, 14);
            this.cbsort.Name = "cbsort";
            this.cbsort.Size = new System.Drawing.Size(140, 36);
            this.cbsort.StartIndex = 0;
            this.cbsort.TabIndex = 8;
            this.cbsort.TextOffset = new System.Drawing.Point(10, 0);
            this.toolTip1.SetToolTip(this.cbsort, "Select sort");
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.DimGray;
            this.label11.Location = new System.Drawing.Point(402, 25);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(61, 15);
            this.label11.TabIndex = 7;
            this.label11.Text = "View/Sort:";
            // 
            // guna2Separator1
            // 
            this.guna2Separator1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2Separator1.Location = new System.Drawing.Point(6, 56);
            this.guna2Separator1.Name = "guna2Separator1";
            this.guna2Separator1.Size = new System.Drawing.Size(1287, 10);
            this.guna2Separator1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Location = new System.Drawing.Point(871, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "Collector:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(624, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Status:";
            // 
            // cbcollector
            // 
            this.cbcollector.BackColor = System.Drawing.Color.Transparent;
            this.cbcollector.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbcollector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbcollector.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbcollector.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbcollector.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cbcollector.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbcollector.ItemHeight = 30;
            this.cbcollector.Items.AddRange(new object[] {
            "--all collectors--",
            "Juan Dela Cruz",
            "Maria Dela Cruz",
            "Jose Dela Cruz"});
            this.cbcollector.Location = new System.Drawing.Point(933, 14);
            this.cbcollector.Name = "cbcollector";
            this.cbcollector.Size = new System.Drawing.Size(217, 36);
            this.cbcollector.StartIndex = 0;
            this.cbcollector.TabIndex = 4;
            this.cbcollector.TextOffset = new System.Drawing.Point(10, 0);
            this.toolTip1.SetToolTip(this.cbcollector, "Select Collector");
            // 
            // cbstatus
            // 
            this.cbstatus.BackColor = System.Drawing.Color.Transparent;
            this.cbstatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbstatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbstatus.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbstatus.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbstatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cbstatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbstatus.ItemHeight = 30;
            this.cbstatus.Items.AddRange(new object[] {
            "--all status--",
            "Active",
            "Due",
            "For Collection",
            "Critical",
            "For Legal Process"});
            this.cbstatus.Location = new System.Drawing.Point(669, 14);
            this.cbstatus.Name = "cbstatus";
            this.cbstatus.Size = new System.Drawing.Size(186, 36);
            this.cbstatus.StartIndex = 0;
            this.cbstatus.TabIndex = 3;
            this.cbstatus.TextOffset = new System.Drawing.Point(10, 0);
            this.toolTip1.SetToolTip(this.cbstatus, "Select Status");
            // 
            // tsearch
            // 
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
            this.tsearch.Location = new System.Drawing.Point(11, 14);
            this.tsearch.Name = "tsearch";
            this.tsearch.PasswordChar = '\0';
            this.tsearch.PlaceholderText = "search any keyword...";
            this.tsearch.SelectedText = "";
            this.tsearch.Size = new System.Drawing.Size(377, 36);
            this.tsearch.TabIndex = 1;
            this.toolTip1.SetToolTip(this.tsearch, "Search Keyword");
            // 
            // bhelp
            // 
            this.bhelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bhelp.BorderColor = System.Drawing.Color.LightGray;
            this.bhelp.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bhelp.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bhelp.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bhelp.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bhelp.FillColor = System.Drawing.Color.White;
            this.bhelp.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.bhelp.ForeColor = System.Drawing.Color.White;
            this.bhelp.Image = global::rct_lmis.Properties.Resources.icons8_help_48;
            this.bhelp.ImageSize = new System.Drawing.Size(28, 28);
            this.bhelp.Location = new System.Drawing.Point(1033, 11);
            this.bhelp.Name = "bhelp";
            this.bhelp.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.bhelp.Size = new System.Drawing.Size(30, 30);
            this.bhelp.TabIndex = 3;
            this.bhelp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.bhelp.TextOffset = new System.Drawing.Point(10, 0);
            this.toolTip1.SetToolTip(this.bhelp, "Need help?");
            // 
            // frm_home_loans
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1300, 850);
            this.Controls.Add(this.pbot);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frm_home_loans";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frm_home_loans";
            this.Load += new System.EventHandler(this.frm_home_loans_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pbot.ResumeLayout(false);
            this.pbot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvdata)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.pheaders.ResumeLayout(false);
            this.pheaders.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label ltitle;
        private System.Windows.Forms.Panel pbot;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel pheaders;
        private Guna.UI2.WinForms.Guna2DataGridView dgvdata;
        private System.Windows.Forms.Panel panel3;
        private Guna.UI2.WinForms.Guna2VSeparator guna2VSeparator3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2VSeparator guna2VSeparator2;
        private Guna.UI2.WinForms.Guna2ComboBox cbcollector;
        private Guna.UI2.WinForms.Guna2ComboBox cbstatus;
        private Guna.UI2.WinForms.Guna2TextBox tsearch;
        private Guna.UI2.WinForms.Guna2Button baddnew;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2Button bmore;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label luser;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label ltotal;
        private Guna.UI2.WinForms.Guna2Separator guna2Separator1;
        private Guna.UI2.WinForms.Guna2ComboBox cbsort;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lnorecord;
        private Guna.UI2.WinForms.Guna2Button bprint;
        private Guna.UI2.WinForms.Guna2Button bexport;
        private Guna.UI2.WinForms.Guna2CircleButton bhelp;
    }
}
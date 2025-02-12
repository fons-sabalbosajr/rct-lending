using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rct_lmis.ADMIN_SECTION
{
    public partial class frm_home_ZProgress : Form
    {
        public frm_home_ZProgress()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        public void UpdateProgress(int progress, int totalRows)
        {
            pbloading.Value = progress;
            lstatus.Text = $"Processing {progress} of {totalRows} records...";
            Application.DoEvents(); // Force UI to update
        }

        private void frm_home_ZProgress_Load(object sender, EventArgs e)
        {
            pbloading.Minimum = 0;
            pbloading.Maximum = 100;
            pbloading.Step = 1;
        }
    }
}

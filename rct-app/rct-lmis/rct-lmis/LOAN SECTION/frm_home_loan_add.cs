using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rct_lmis.LOAN_SECTION
{
    public partial class frm_home_loan_add : Form
    {
        public frm_home_loan_add()
        {
            InitializeComponent();
        }

        private void frm_home_loan_add_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void bsecID_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show(this, "Do you want to add a secondary ID/Document?", 
                "Add Second ID/Document", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                grpIDsec.Enabled = true;
                cbidtypesec.Focus();
            }
        }
    }
}

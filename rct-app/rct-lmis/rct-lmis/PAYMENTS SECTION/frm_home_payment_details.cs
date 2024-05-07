using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rct_lmis.PAYMENTS_SECTION
{
    public partial class frm_home_payment_details : Form
    {
        public frm_home_payment_details()
        {
            InitializeComponent();
        }

        private void dgvdatapay_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            lnorecord.Visible = false;
        }
    }
}

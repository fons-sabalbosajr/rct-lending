using rct_lmis.PAYMENTS_SECTION;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rct_lmis
{
    public partial class frm_home_payments : Form
    {
        public frm_home_payments()
        {
            InitializeComponent();
        }
        LoadingFunction load = new LoadingFunction();
        frm_home_payment_details pd = new frm_home_payment_details();

        private void dgvdata_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            load.Show(this);
            Thread.Sleep(500);
            pd.Show(this);
            load.Close();
        }

        private void dgvdata_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            lnorecord.Visible = false;
        }

        private void frm_home_payments_Load(object sender, EventArgs e)
        {

        }

        private void bexport_Click(object sender, EventArgs e)
        {
            frm_home_payment_details fpd = new frm_home_payment_details();
            fpd.Show(this);
        }
    }
}

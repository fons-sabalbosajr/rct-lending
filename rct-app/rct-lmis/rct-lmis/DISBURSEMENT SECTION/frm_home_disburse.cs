using rct_lmis.DISBURSEMENT_SECTION;
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
    public partial class frm_home_disburse : Form
    {
        public frm_home_disburse()
        {
            InitializeComponent();
        }
        LoadingFunction load = new LoadingFunction();

        private void frm_home_disburse_Load(object sender, EventArgs e)
        {

        }

        private void dgvdata_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvdata.ClearSelection();
        }

        private void dgvdata_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void beditentries_Click(object sender, EventArgs e)
        {
            frm_home_disburse_editCV ncv = new frm_home_disburse_editCV();

            load.Show(this);
            Thread.Sleep(300);
            ncv.Show();
            load.Close();

        }
    }
}

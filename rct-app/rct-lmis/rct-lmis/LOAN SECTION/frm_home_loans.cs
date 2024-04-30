using rct_lmis.LOAN_SECTION;
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
    public partial class frm_home_loans : Form
    {
        public frm_home_loans()
        {
            InitializeComponent();
        }
        LoadingFunction load = new LoadingFunction();
        frm_home_loan_new flnew = new frm_home_loan_new();
        frm_home_loan_add fladd = new frm_home_loan_add();
    


        private void baddnew_Click(object sender, EventArgs e)
        {
            load.Show(this);
            Thread.Sleep(500);
            fladd.Show(this);
            load.Close();
        }

        private void frm_home_loans_Load(object sender, EventArgs e)
        {

        }

        private void dgvdata_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            lnorecord.Visible = false;
        }

        private void dgvdata_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            load.Show(this);
            Thread.Sleep(500);
            flnew.Show(this);
            load.Close();
        }
    }
}

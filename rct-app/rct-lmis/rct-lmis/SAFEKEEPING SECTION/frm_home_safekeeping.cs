using rct_lmis.SAFEKEEPING_SECTION;
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
    public partial class frm_home_safekeeping : Form
    {
        public frm_home_safekeeping()
        {
            InitializeComponent();
        }

        LoadingFunction load = new LoadingFunction();
        private void dgvdata_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frm_home_safekeeping_details fsd = new frm_home_safekeeping_details();
            load.Show(this);
            Thread.Sleep(500);
            fsd.Show(this);
            load.Close();
        }

        private void bnewsf_Click(object sender, EventArgs e)
        {
            frm_home_safekeeping_new fsdn = new frm_home_safekeeping_new();
            load.Show(this);
            Thread.Sleep(500);
            fsdn.Show(this);
            load.Close();

        }
    }
}

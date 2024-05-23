using rct_lmis.CLIENTS_SECTION;
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
    public partial class frm_home_clients : Form
    {
        public frm_home_clients()
        {
            InitializeComponent();
        }

        LoadingFunction load = new LoadingFunction();

        private void dgvdata_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frm_home_client_details cd = new frm_home_client_details();

            load.Show(this);
            Thread.Sleep(500);
            cd.Show(this);
            load.Close();
        }
    }
}

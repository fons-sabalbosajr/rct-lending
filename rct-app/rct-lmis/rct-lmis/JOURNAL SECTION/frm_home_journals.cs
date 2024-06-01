using rct_lmis.JOURNAL_SECTION;
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
    public partial class frm_home_journals : Form
    {
        public frm_home_journals()
        {
            InitializeComponent();
        }

        LoadingFunction load = new LoadingFunction();

        private void frm_home_journals_Load(object sender, EventArgs e)
        {

        }

        private void baddnew_Click(object sender, EventArgs e)
        {
            frm_home_journals_new jnew = new frm_home_journals_new();

            load.Show(this);
            Thread.Sleep(1000);
            jnew.Show(this);
            load.Close();
        }
    }
}

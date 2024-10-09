using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rct_lmis.CLIENTS_SECTION
{
    public partial class frm_home_client_details : Form
    {
        public frm_home_client_details(string loanId, string clientNo)
        {
            InitializeComponent();

            laccno.Text = loanId;
            lclientno.Text = clientNo;
        }

        LoadingFunction load = new LoadingFunction();



        private void frm_home_client_details_Load(object sender, EventArgs e)
        {

        }

        private void label42_Click(object sender, EventArgs e)
        {

        }
    }
}

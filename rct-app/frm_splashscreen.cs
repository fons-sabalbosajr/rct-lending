using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rct_app
{
    public partial class frm_splashscreen : Form
    {
        public frm_splashscreen()
        {
            InitializeComponent();
        }

        private void bopen_Click(object sender, EventArgs e)
        {
            this.Hide();  
            frm_login login = new frm_login();
            login.ShowDialog();

            this.Close();
        }
    }
}

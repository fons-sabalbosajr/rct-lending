using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace rct_app
{
    public partial class frm_login : Form
    {
        public frm_login()
        {
            InitializeComponent();
        }

     



        private void bsignup_Click(object sender, EventArgs e)
        {
            frm_signup signup = new frm_signup();
            signup.ShowDialog();
        }

        private void frm_login_Load(object sender, EventArgs e)
        {

        }
    }
}

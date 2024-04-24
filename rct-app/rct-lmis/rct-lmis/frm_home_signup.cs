using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rct_lmis
{
    public partial class frm_home_signup : Form
    {
        public frm_home_signup()
        {
            InitializeComponent();
        }

        private void frm_home_signup_Load(object sender, EventArgs e)
        {

        }

        private void pback_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Cancel sign-up?", "Cancel Sign-Up", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void pbeye_Click(object sender, EventArgs e)
        {
            tpassword.UseSystemPasswordChar = false;
        }

        private void pbeye_MouseDown(object sender, MouseEventArgs e)
        {
            tpassword.PasswordChar = (char)0;
        }

        private void pbeye_MouseUp(object sender, MouseEventArgs e)
        {
             tpassword.PasswordChar = '•';
        }
    }
}

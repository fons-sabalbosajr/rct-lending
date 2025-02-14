using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rct_lmis.DISBURSEMENT_SECTION
{
    public partial class frm_progress_popup : Form
    {
        public frm_progress_popup()
        {
            InitializeComponent();
        }

        private void frm_progress_popup_Load(object sender, EventArgs e)
        {

        }

        public void SetProgress(int value, int max)
        {
            progressBar1.Value = value;
            progressBar1.Maximum = max;
            label1.Text = $"Processing... {value}/{max}"; // Optional: Display the current progress
        }
    }
}

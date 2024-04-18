using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rct_lmis
{
    class LoadingFunction
    {
        z_loadingform wait;
        Thread load;
        public void Show()
        {
            load = new Thread(new ThreadStart(LoadingProcess));
            load.Start();
        }
        public void Show(System.Windows.Forms.Form parent)
        {
            load = new Thread(new ParameterizedThreadStart(LoadingProcess));
            load.Start(parent);
        }

        public void Close()
        {
            if (wait != null)
            {
                wait.BeginInvoke(new System.Threading.ThreadStart(wait.CloseWaitForm));
                wait = null;
                load = null;
            }
        }
        private void LoadingProcess()
        {
            z_loadingform wait= new z_loadingform();
            wait.ShowDialog();
        }

        private void LoadingProcess(object parent)
        {
            Form parent1 = parent as Form;
            wait = new z_loadingform(parent1);
            wait.ShowDialog();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class MonitorForm : Form
    {
        public MonitorForm()
        {
            InitializeComponent();
        }

        private void MonitorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*
            for (int i = 0; i < this.Controls.Count; i++)
            {
                (this.Controls[i] as IfMonitor).StopUpdate();
            }
            */
            e.Cancel = true;
            this.Hide();
        }
        public new void Show()
        {
            if (!this.Visible)
            {
                for (int i = 0; i < this.Controls.Count; i++)
                {
                    (this.Controls[i] as IfMonitor).StartUpdate();
                }
                base.Show();
            }
            else
            {
                this.Focus();
            }
        }
    }
}

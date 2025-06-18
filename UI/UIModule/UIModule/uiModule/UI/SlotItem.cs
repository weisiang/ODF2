using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class SlotItem : UserControl
    {
        public SlotItem()
        {
            InitializeComponent();
        }
        public void DisableControls()
        {
            this.GlassId.Enabled = false;
            this.ProcessFlag.Enabled = false;
            this.Button_Copy.Enabled = false;
            this.cb_priority.Enabled = false;
        }

        private void SlotNo_Click(object sender, EventArgs e)
        {
            if(ProcessFlag.Enabled == true)
            {
                ProcessFlag.Enabled = false;
                GlassId.Enabled = false;
                Button_Copy.Enabled = false;
                ProcessFlag.Checked = false;
                this.cb_priority.Enabled = false;
                GlassId.Text = "";
            }
            else if(ProcessFlag.Enabled == false)
            {
                ProcessFlag.Enabled = true;
                GlassId.Enabled = true;
                Button_Copy.Enabled = true;
                ProcessFlag.Checked = true;
                this.cb_priority.Enabled = true;
                GlassId.Text = "";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class GlassEdit : Form
    {
        CommonData.HIRATA.GlassData cv_RegistGlass = null;
        public GlassEdit()
        {
        }
        public void Regist(CommonData.HIRATA.GlassData m_Glass)
        {
            cv_RegistGlass = m_Glass;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            cv_RegistGlass = null;
            this.Hide();
        }

    }
}

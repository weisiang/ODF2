using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BaseAp;

namespace UI
{
    public partial class MgvForm : Form
    {
        int cv_PortId ;
        public MgvForm(int m_PortId)
        {
            InitializeComponent();
            cv_PortId = m_PortId;
            this.FormClosing += new FormClosingEventHandler(MgvForm_FormClosing);
        }
        public new void Show()
        {
            base.Show();
        }
        public void MgvForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        private void cv_BtSave_Click(object sender, EventArgs e)
        {
        }
        public void Clear()
        {
            cv_TbOpid.Text = "";
            cv_TbCstId.Text = "";
        }

        private void cv_BtRequest_Click(object sender, EventArgs e)
        {//CommonData.Result m_Result ,int m_PortId, string m_Opid, string m_CstSeq, uint m_ticket
            CommonData.HIRATA.MDPopOpidForm obj = new CommonData.HIRATA.MDPopOpidForm();
            obj.PortId = cv_PortId;
            obj.PType = CommonData.HIRATA.MmfEventClientEventType.etReply;
            obj.Reply.CstId = cv_TbCstId.Text.Trim();
            obj.Reply.OpId = cv_TbOpid.Text.Trim();
            UIController.SendOpidReply(CommonData.HIRATA.Result.OK, cv_PortId, obj.Reply.OpId, obj.Reply.CstId, 111);
            this.Clear();
            this.Close();
        }
    }
}

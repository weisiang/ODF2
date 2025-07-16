using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KgsCommon;
using CommonData.HIRATA;
using BaseAp;

namespace UI.GUI
{
    public partial class PortUI : UserControl , PortReflash
    {
        int cv_SlotCount = 0;
        int cv_Id = 0;
        GlassEdit cv_EditForm = new GlassEdit();
        public PortUI(int m_Id, int m_SlotCount)
        {
            InitializeComponent();
            cv_Id = m_Id;
            cv_SlotCount = m_SlotCount;
            this.gb_port.Text = "PORT " + Convert.ToString(m_Id);
            cv_glassDataView.Rows.Add(cv_SlotCount);
            cv_glassDataView.AutoGenerateColumns = false;
            //cv_glassDataView.RowHeadersVisible = false;
            cv_glassDataView.Rows[0].Cells[0].Value = "";

            for (int i = 0; i < m_SlotCount; i++)
            {
                cv_glassDataView.Rows[i].Cells[0].Value = i + 1;
                ToolStripMenuItem tmp = new ToolStripMenuItem();
                tmp.Text = (i + 1).ToString();
                tmp.Click += dELETEToolStripMenuItem_Click;
                (cv_menuDataEdit.Items[0] as ToolStripMenuItem).DropDownItems.Add(tmp);
            }
            AddObjToPermissionList(button1, UiForm.enumGroup.Group2);
            AddObjToPermissionList(cv_13slot, UiForm.enumGroup.Group2);
            AddObjToPermissionList(cv_25slot, UiForm.enumGroup.Group2);
        }
        private void AddObjToPermissionList(Control m_Control , UiForm.enumGroup m_Group)
        {
            UiForm.AddUiObjToEnableList(m_Control, m_Group);
        }
        public void SetSlotButton(bool m_Is25Slot)
        {
            if(m_Is25Slot)
            {
                cv_13slot.BackColor = Color.Gray;
                cv_25slot.BackColor = Color.Green;
            }
            else 
            {
                cv_13slot.BackColor = Color.Green;
                cv_25slot.BackColor = Color.Gray;
            }
        }
        public void ResetDataVier(int m_Slot)
        {
            cv_SlotCount = m_Slot;
            while (cv_glassDataView.Rows.Count > 2)
            {
                cv_glassDataView.Rows.RemoveAt(1);
            }
            cv_glassDataView.Rows.Add(cv_SlotCount);
            cv_glassDataView.Rows[0].Cells[0].Value = "";

            for (int i = 0; i < cv_SlotCount; i++)
            {
                cv_glassDataView.Rows[i].Cells[0].Value = i + 1;
                ToolStripMenuItem tmp = new ToolStripMenuItem();
                tmp.Text = (i + 1).ToString();
                tmp.Click += dELETEToolStripMenuItem_Click;
                (cv_menuDataEdit.Items[0] as ToolStripMenuItem).DropDownItems.Add(tmp);
            }
        }
        public void refresh(PortData m_DataMap)
        {
            int slot = 0;
            if (m_DataMap != null)
            {
                if (m_DataMap.PPortStatus != PortStaus.LDRQ && m_DataMap.PPortStatus != PortStaus.UDCM)
                {
                    if (!string.IsNullOrEmpty(m_DataMap.PLotId))
                    {
                        if (lbl_LotId.Text != "ID : " + m_DataMap.PLotId)
                            lbl_LotId.Text = "ID : " + m_DataMap.PLotId;
                    }
                    else
                    {
                        if (lbl_LotId.Text != "")
                            lbl_LotId.Text = "";
                    }
                }

                for (int i = 0; i < cv_SlotCount; i++)
                {
                    slot = Convert.ToInt32( cv_glassDataView.Rows[i].Cells[0].Value.ToString());
                    GlassData glass_data = null;
                    bool has_sensor = false;
                    if (m_DataMap.GetSlotData(slot , ref glass_data , ref has_sensor) )
                    {
                        if(glass_data != null)
                        {
                            if(glass_data.PHasData)
                            cv_glassDataView.Rows[i].Cells[1].Value = glass_data.PId;
                            else
                            cv_glassDataView.Rows[i].Cells[1].Value = "";
                        }
                        else
                        {
                            cv_glassDataView.Rows[i].Cells[1].Value = "";
                        }
                        CommonStaticData.ChangeDataViewItemColor(ref cv_glassDataView, i, glass_data);
                    }
                    else
                    {
                        UiForm.WriteLog( LogLevelType.Error , "Port UI refresh ERROR. Slot Un-match");
                    }
                }
            }
            cv_glassDataView.ClearSelection();
        }

        private void cv_glassDataView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //e.RowIndex
                if (cv_glassDataView.Rows[e.RowIndex].Cells[0].Value != null)
                {
                    string tmp = cv_glassDataView.Rows[e.RowIndex].Cells[0].Value.ToString();
                    cv_menuDataEdit.Show(Cursor.Position);
                }
            }
        }

        private void dELETEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItem tmp = sender as ToolStripItem;
            int slot = Convert.ToInt32(tmp.Text.Trim());
            UiForm.cv_GlassDataForm.Register(ActionTarget.Port, cv_Id, slot);
            UiForm.cv_GlassDataForm.Show();
        }

        private void cv_glassDataView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
           cv_glassDataView.Rows[e.RowIndex].Selected = false; 
        }

        private void cv_menuDataEdit_Opening(object sender, CancelEventArgs e)
        {
            //if need check contextMenuStrip show condition , add code here.
            /*
            ContextMenuStrip tmp = sender as ContextMenuStrip;
            ToolStripMenuItem action_item = (tmp.Items[0] as ToolStripMenuItem);
            foreach(object obj in action_item.DropDownItems)
            {
                string slot = (obj as ToolStripItem).Text;
            }
            */
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!lgcBase.PSystemData.PapiConnect)
            {
                CommonStaticData.PopForm("Port cancel can't exe , buz robot disconnect!!", true, false);
            }
            CommandData command_obj = null;
            APIEnum.LoadPortCommand command = APIEnum.LoadPortCommand.Unload;
            command_obj = new CommandData(APIEnum.CommandType.LoadPort, 
                command.ToString(), APIEnum.CommnadDevice.P, cv_Id);
            CommonData.HIRATA.MDApiCommand obj = new MDApiCommand();
            obj.CommandData = command_obj;
            string rtn;
            object tmp = null;
            uint ticket;
            /*
            if (!UiForm.cv_mmfController.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDApiCommand).Name, obj, out ticket, out rtn, out tmp, 3000))
            {
                CommonStaticData.PopForm("Manual Port cancel time out!!", true, false);
            }
            */
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Port port = UiForm.GetPort(cv_Id);
            if (port.cv_Data.PEfemPortType != 4)
            {
                setPortType(4);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Port port = UiForm.GetPort(cv_Id);
            if (port.cv_Data.PEfemPortType != 0)
            {
                setPortType(0);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (!lgcBase.PSystemData.PapiConnect)
            {
                CommonStaticData.PopForm("Port cancel can't exe , buz robot disconnect!!", true, false);
            }
            CommandData command_obj = null;
            APIEnum.CommonCommand command = APIEnum.CommonCommand.Home;
            command_obj = new CommandData(APIEnum.CommandType.Common,
                command.ToString(), APIEnum.CommnadDevice.P, cv_Id);
            CommonData.HIRATA.MDApiCommand obj = new MDApiCommand();
            obj.CommandData = command_obj;
            string rtn;
            object tmp = null;
            uint ticket;
            /*
            if (!UiForm.cv_mmfController.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDApiCommand).Name, obj, out ticket, out rtn, out tmp, 3000))
            {
                CommonStaticData.PopForm("Manual Port cancel time out!!", true, false);
            }
            */
        }
        private void setPortType(int m_PortType)
        {
            CommonData.HIRATA.MDChangePortSlotType obj = new MDChangePortSlotType();
            obj.PPortId = cv_Id;
            obj.PSlotType = m_PortType;
            UIController.triggerUiEvent(typeof(MDChangePortSlotType).Name, obj);
        }
    }
}

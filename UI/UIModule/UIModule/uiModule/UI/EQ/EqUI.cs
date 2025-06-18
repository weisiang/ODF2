using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using KgsCommon;
using System.Text;
using System.Windows.Forms;
using CommonData.HIRATA;

namespace UI.GUI
{
    public partial class EqUI : UserControl , EqReflash
    {
        int cv_SlotCount = 0;
        public EqUI(int m_id, int m_slotNumber, string m_alias = "")
        {
            InitializeComponent();
            cv_SlotCount = m_slotNumber;
            setEqTitle(m_id, m_alias);
            setGlassDataView();
        }
        private void setEqTitle(int m_Id , string m_Alias)
        {
            cv_GBName.Text = "EQ_" + m_Id.ToString();
            if (!string.IsNullOrEmpty(m_Alias))
            {
                cv_GBName.Text += " ( " + m_Alias + " )";
            }
        }
        private void setGlassDataView()
        {
            dataGridView1.Rows.Add(cv_SlotCount);
            for (int i = 0; i < cv_SlotCount; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = i + 1;
            }
        }
        public void refresh(EqData m_DataMap)
        {
            int slot = 0;
            if (m_DataMap != null)
            {
                for (int i = 0; i < cv_SlotCount; i++)
                {
                    GlassData glass_data = null;
                    slot = Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value.ToString());
                    bool has_sensor = false;
                    if (m_DataMap.GetSlotData(slot, ref glass_data, ref has_sensor))
                    {
                        if (glass_data != null)
                        {
                            dataGridView1.Rows[i].Cells[1].Value = glass_data.PId;
                        }
                        else
                        {
                            dataGridView1.Rows[i].Cells[1].Value = "";
                        }
                        CommonStaticData.ChangeDataViewItemColor(ref dataGridView1, i, glass_data);
                    }
                    else
                    {
                        UiForm.WriteLog(LogLevelType.Error , "EQ UI refresh ERROR. Slot Un-match");
                    }
                }
            }
            dataGridView1.ClearSelection();
        }
        public void updateInlineMode(CommonData.HIRATA.EquipmentInlineMode m_Mode)
        {
            if (cv_inlineMode.Text != m_Mode.ToString())
            {
                switch (m_Mode)
                {
                    case CommonData.HIRATA.EquipmentInlineMode.None:
                        cv_inlineMode.Text = "None";
                        cv_inlineMode.BackColor = Color.Gray;
                        break;
                    case CommonData.HIRATA.EquipmentInlineMode.Remote:
                        cv_inlineMode.Text = "Remote";
                        cv_inlineMode.BackColor = Color.Lime;
                        break;
                    case CommonData.HIRATA.EquipmentInlineMode.Local:
                        cv_inlineMode.Text = "Local";
                        cv_inlineMode.BackColor = Color.Red;
                        break;
                };
            }
        }

        public void updateMainStatus(CommonData.HIRATA.EquipmentStatus m_status)
        {
            if (cv_mainStatus.Text != m_status.ToString())
            {
                switch (m_status)
                {
                    case CommonData.HIRATA.EquipmentStatus.None:
                        cv_mainStatus.Text = "None";
                        cv_mainStatus.BackColor = CommonStaticData.g_EquipmentDownColor;
                        break;
                    case CommonData.HIRATA.EquipmentStatus.Run:
                        cv_mainStatus.Text = "Run";
                        cv_mainStatus.BackColor = CommonStaticData.g_EquipmentRunColor;
                        break;
                    case CommonData.HIRATA.EquipmentStatus.Down:
                        cv_mainStatus.Text = "Down";
                        cv_mainStatus.BackColor = CommonStaticData.g_EquipmentDownColor;
                        break;
                    case CommonData.HIRATA.EquipmentStatus.Idle:
                        cv_mainStatus.Text = "Idle";
                        cv_mainStatus.BackColor = CommonStaticData.g_EquipmentIdleColor;
                        break;
                };
            }
        }

        public void updateSubStatus(List<CommonData.HIRATA.EquipmentSubStatus> m_subStatusList)
        {
            int[] notOccur = new int[5] {0,0,0,0,0};
            if (m_subStatusList!= null &&  m_subStatusList.Count != 0 )
            {
                foreach (CommonData.HIRATA.EquipmentSubStatus item in m_subStatusList)
                {
                    notOccur[Convert.ToInt16(item)] = 1;
                    switch (item)
                    {
                        case CommonData.HIRATA.EquipmentSubStatus.Init:
                            cv_init.BackColor = Color.DeepPink;
                            break;
                        case CommonData.HIRATA.EquipmentSubStatus.Standby:
                            cv_stabdby.BackColor = Color.DeepPink;
                            break;
                        case CommonData.HIRATA.EquipmentSubStatus.Stop:
                            cv_stop.BackColor = Color.DeepPink;
                            break;
                        case CommonData.HIRATA.EquipmentSubStatus.Warning:
                            cv_warning.BackColor = Color.DeepPink;
                            break;
                    };
                }
            }
            for(int i =1; i < 5; i++)
            {
                if(notOccur[i] == 0 )
                {
                    CommonData.HIRATA.EquipmentSubStatus tmp = (CommonData.HIRATA.EquipmentSubStatus)notOccur[i];
                    switch (tmp)
                    {
                        case CommonData.HIRATA.EquipmentSubStatus.Init:
                            cv_init.BackColor = Color.Gray;
                            break;
                        case CommonData.HIRATA.EquipmentSubStatus.Standby:
                            cv_stabdby.BackColor = Color.Gray;
                            break;
                        case CommonData.HIRATA.EquipmentSubStatus.Stop:
                            cv_stop.BackColor = Color.Gray;
                            break;
                        case CommonData.HIRATA.EquipmentSubStatus.Warning:
                            cv_warning.BackColor = Color.Gray;
                            break;
                    };
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].Selected = false;
        }
    }
}
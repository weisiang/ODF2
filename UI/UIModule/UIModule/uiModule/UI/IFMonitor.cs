using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using KgsCommon;
using CommonData.HIRATA;
using BaseAp;

namespace UI
{
    public partial class IfMonitor : UserControl
    {
        static KXmlItem cv_PortAddress = null;
        public enum RobotPortOffset { None = -1 , Node3 = 1 ,  Node4 = 2 , Node5 = 3 , Node6_Up=4 ,
            Node6_Low=5 , Node7 = 6 , Node8 = 7 , Node9=8 , Node10=9 ,};
        public enum EqPortOffset
        {
            None = -1, Node3 = 1, Node4 = 2, Node5 = 3, Node6_Up = 4,
            Node6_Low = 5, Node7 = 6, Node8 = 7, Node9 = 8, Node10 = 9,
        };
        
        private string cv_EqName = "";
        private EqGifTimeChartId cv_TimeChartId = EqGifTimeChartId.None;
        private int cv_RobotPortStart = 0;
        private int cv_EqPortStart = 0;
        private Dictionary<int, Label> cv_RobotLabels = new Dictionary<int, Label>();
        private Dictionary<int, Label> cv_EqLabels = new Dictionary<int, Label>();
        private KTimer cv_Timer = null;
        // label size 120 , 15
        public IfMonitor()
        {
            InitializeComponent();
            InitAddKxml();
            InitSinglaLabel();
            AddEqNameToComboBox();
            InitTimer();
            UiForm.AddUiObjToEnableList(cv_BtUpForceCom, UiForm.enumGroup.Group4);
            UiForm.AddUiObjToEnableList(cv_BtUpForceIni, UiForm.enumGroup.Group4);
            UiForm.AddUiObjToEnableList(cv_btnReset, UiForm.enumGroup.Group4);
        }
        private void InitAddKxml()
        {
            if(cv_PortAddress == null)
            {
                cv_PortAddress = new KXmlItem();
                cv_PortAddress.LoadFromFile(CommonData.HIRATA.CommonStaticData.g_SysGifPortAddrFileFile);
            }
        }
        private void InitSinglaLabel()
        {
            for (int i = (int)RobotSideBitAddressOffset.Force_Initial_Request; i >= 0; i--)
            {
                Label tmp = new Label();
                tmp.BackColor = Color.Gray;
                tmp.Width = 120;
                tmp.Height = 25;
                tmp.AutoSize = false;
                tmp.Text = ((RobotSideBitAddressOffset)i).ToString();
                tmp.Dock = DockStyle.Top;
                tmp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                tmp.TextAlign = ContentAlignment.MiddleLeft;
                cv_RobotGroup.Controls.Add(tmp);
                cv_RobotLabels.Add(i, tmp);
                tmp.DoubleClick += OnLabelCline;
            }
            for (int i = (int)EqSideBitAddressOffset.Spare4; i >= 0; i--)
            {
                Label tmp = new Label();
                tmp.BackColor = Color.Gray;
                tmp.Width = 120;
                tmp.Height = 25;
                tmp.AutoSize = false;
                tmp.Text = ((EqSideBitAddressOffset)i).ToString();
                tmp.Dock = DockStyle.Top;
                tmp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                tmp.TextAlign = ContentAlignment.MiddleLeft;
                cv_EqGroup.Controls.Add(tmp);
                cv_EqLabels.Add(i, tmp);
                tmp.DoubleClick += OnLabelCline;
            }
        }
        private void OnLabelCline(object sender, EventArgs e)
        {
            Label lbl = sender as Label;
            if(lbl.Parent == cv_RobotGroup)
            {
                RobotSideBitAddressOffset item = RobotSideBitAddressOffset.Active_Standby;
                if(Enum.TryParse<RobotSideBitAddressOffset>( lbl.Text , out item))
                {
                    int value = UiForm.cv_Mio.GetPortValue(cv_RobotPortStart + (int)item);
                    UiForm.cv_Mio.SetPortValue(cv_RobotPortStart + (int)item, value == 1 ? 0 : 1);
                }
            }
            else
            {
                EqSideBitAddressOffset item = EqSideBitAddressOffset.Equipment_Ready;
                if (Enum.TryParse<EqSideBitAddressOffset>(lbl.Text, out item))
                {
                    int value = UiForm.cv_Mio.GetPortValue(cv_EqPortStart + (int)item);
                    UiForm.cv_Mio.SetPortValue(cv_EqPortStart + (int)item, value == 1 ? 0 : 1);
                }
            }
        }
    
        private void InitTimer()
        {
            if(cv_Timer == null)
            {
                cv_Timer = new KTimer();
                cv_Timer.Interval = 200;
                cv_Timer.ThreadEventEnabled = false;
                cv_Timer.OnTimer += UpdateUi;
                cv_Timer.Enabled = true ;
                cv_Timer.Open();
            }
        }
        private void UpdateUi()
        {
            UpdateSinglaLabel();
            UpdateStep();
        }
        private void UpdateSinglaLabel()
        {
            if (string.IsNullOrEmpty(cv_EqName)) return;
            foreach (KeyValuePair<int, Label> pair in cv_EqLabels)
            {
                pair.Value.BackColor = (UiForm.cv_Mio.GetPortValue(cv_EqPortStart + pair.Key) == 1 ? Color.Lime : Color.Gray);
            }
            foreach (KeyValuePair<int, Label> pair in cv_RobotLabels)
            {
                pair.Value.BackColor = (UiForm.cv_Mio.GetPortValue(cv_RobotPortStart + pair.Key) == 1 ? Color.Lime : Color.Gray);
            }
        }
        private void UpdateStep()
        {
            if(cv_TimeChartId != EqGifTimeChartId.None)
            {
                if (UiForm.cv_TimeChartStep != null)
                {
                    if (UiForm.cv_TimeChartStep.ContainsKey(cv_TimeChartId))
                    {
                        lbl_Step.Text = UiForm.cv_TimeChartStep[cv_TimeChartId].Key.ToString();
                        lbl_StepName.Text = UiForm.cv_TimeChartStep[cv_TimeChartId].Value;
                    }
                }
            }
        }
        private void AddEqNameToComboBox()
        {
            for(int i = 1 ; i <= CommonData.HIRATA.CommonStaticData.g_EqNumber ; i++)
            {
                if(UiForm.GetEq(i).cv_SlotCount > 1)
                {
                    for(int j= 1 ; j <= UiForm.GetEq(i).cv_SlotCount  ; j++)
                    {
                        cv_EqNameGroup.Items.Add(UiForm.GetEq(i).cv_Alias + (j==1 ? "_Low" : "_Up"));
                    }
                }
                else
                {
                    cv_EqNameGroup.Items.Add(UiForm.GetEq(i).cv_Alias);
                }
            }
        }
        private void cv_EqNameGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            string eq_name = cv_EqNameGroup.Text.Trim();
            switch(eq_name)
            {
                case "SDP1":
                    cv_TimeChartId = EqGifTimeChartId.TIMECHART_ID_SDP1;
                    cv_EqGroup.Text = eq_name;
                    cv_EqName = eq_name;
                    ChangePortStart();
                    break;
                case "SDP2":
                    cv_TimeChartId = EqGifTimeChartId.TIMECHART_ID_SDP2;
                    cv_EqGroup.Text = eq_name;
                    cv_EqName = eq_name;
                    ChangePortStart();
                    break;
                case "SDP3":
                    cv_TimeChartId = EqGifTimeChartId.TIMECHART_ID_SDP3;
                    cv_EqGroup.Text = eq_name;
                    cv_EqName = eq_name;
                    ChangePortStart();
                    break;
                case "SDP4":
                    cv_TimeChartId = EqGifTimeChartId.TIMECHART_ID_SDP4;
                    cv_EqGroup.Text = eq_name;
                    cv_EqName = eq_name;
                    ChangePortStart();
                    break;
                case "SDP5":
                    cv_TimeChartId = EqGifTimeChartId.TIMECHART_ID_SDP5;
                    cv_EqGroup.Text = eq_name;
                    cv_EqName = eq_name;
                    ChangePortStart();
                    break;
                case "SDP6":
                    cv_TimeChartId = EqGifTimeChartId.TIMECHART_ID_SDP6;
                    cv_EqGroup.Text = eq_name;
                    cv_EqName = eq_name;
                    ChangePortStart();
                    break;
                case "AOI1":
                    cv_TimeChartId = EqGifTimeChartId.TIMECHART_ID_AOI1;
                    cv_EqGroup.Text = eq_name;
                    cv_EqName = eq_name;
                    ChangePortStart();
                    break;
                case "IJP1":
                    cv_TimeChartId = EqGifTimeChartId.TIMECHART_ID_IJP1;
                    cv_EqGroup.Text = eq_name;
                    cv_EqName = eq_name;
                    ChangePortStart();
                    break;
                case "IJP2":
                    cv_TimeChartId = EqGifTimeChartId.TIMECHART_ID_IJP2;
                    cv_EqGroup.Text = eq_name;
                    cv_EqName = eq_name;
                    ChangePortStart();
                    break;
                case "VAS1_Low":
                    cv_TimeChartId = EqGifTimeChartId.TIMECHART_ID_VAS1_DOWN;
                    cv_EqGroup.Text = eq_name;
                    cv_EqName = eq_name;
                    ChangePortStart();
                    break;
                case "VAS1_Up":
                    cv_TimeChartId = EqGifTimeChartId.TIMECHART_ID_VAS1_UP;
                    cv_EqGroup.Text = eq_name;
                    cv_EqName = eq_name;
                    ChangePortStart();
                    break;
                case "VAS2_Low":
                    cv_TimeChartId = EqGifTimeChartId.TIMECHART_ID_VAS2_DOWN;
                    cv_EqGroup.Text = eq_name;
                    cv_EqName = eq_name;
                    ChangePortStart();
                    break;
                case "VAS2_Up":
                    cv_TimeChartId = EqGifTimeChartId.TIMECHART_ID_VAS2_UP;
                    cv_EqGroup.Text = eq_name;
                    cv_EqName = eq_name;
                    ChangePortStart();
                    break;
                case "UV1":
                    cv_TimeChartId = EqGifTimeChartId.TIMECHART_ID_UV_1;
                    cv_EqGroup.Text = eq_name;
                    cv_EqName = eq_name;
                    ChangePortStart();
                    break;
                case "UV2":
                    cv_TimeChartId = EqGifTimeChartId.TIMECHART_ID_UV_2;
                    cv_EqGroup.Text = eq_name;
                    cv_EqName = eq_name;
                    ChangePortStart();
                    break;
                default:
                    cv_TimeChartId = EqGifTimeChartId.None;
                    cv_EqGroup.Text = "";
                    cv_EqName = eq_name;
                    ChangePortStart();
                    break;
            };
        }
        private void ChangePortStart()
        {
            if(cv_PortAddress != null)
            {
                string item_name = "TimerChart" + ((int)cv_TimeChartId).ToString();
                if(cv_PortAddress.IsItemExist(item_name))
                {
                    cv_RobotPortStart =CommonData.HIRATA.CommonStaticData.GetIntFormStr(cv_PortAddress.ItemsByName[item_name].Attributes["RbBitStart"]);
                    cv_EqPortStart = CommonData.HIRATA.CommonStaticData.GetIntFormStr(cv_PortAddress.ItemsByName[item_name].Attributes["EqBitStart"]);
                }
                else
                {
                    cv_RobotPortStart = 0;
                    cv_EqPortStart = 0;
                }
            }
        }
        public void setGroupBoxColor(int m_R , int m_G , int m_B)
        {
            cv_GbUpStream.BackColor = Color.FromArgb(m_R, m_G, m_B);
        }
        public void StopUpdate()
        {
            if(cv_Timer != null)
            {
                cv_Timer.Enabled = false;
                cv_Timer.Close();
            }
        }
        public void StartUpdate()
        {
            if(cv_Timer != null)
            {
                cv_Timer.Enabled = true; ;
                cv_Timer.Open();
            }
        }
        private void cv_BtUpForceCom_Click(object sender, EventArgs e)
        {
            /*
            if(UiForm.PSystemData.POperationModeLeft != OperationMode.Manual)// || UiForm.PSystemData.PSystemStatus== EquipmentStatus.Down)
            {
                CommonStaticData.PopForm("Please change to manual mode " , true , false);
                return;
            }

            CommonData.HIRATA.MDForceCom obj = new CommonData.HIRATA.MDForceCom();
            obj.cv_TimeChartId = (int)cv_TimeChartId;
            UiForm.cv_mmfController.SendMmfNotifyObject(typeof(CommonData.HIRATA.MDForceCom).Name, obj, KParseObjToXmlPropertyType.Field);
            */
        }
        private void cv_BtUpForceIni_Click(object sender, EventArgs e)
        {
            /*
            if (UiForm.PSystemData.POperationModeLeft != OperationMode.Manual)// || UiForm.PSystemData.PSystemStatus== EquipmentStatus.Down)
            {
                CommonStaticData.PopForm("Please change to manual mode ", true, false);
                return;
            }
            CommonData.HIRATA.MDForceIni obj = new CommonData.HIRATA.MDForceIni();
            obj.cv_TimeChartId = (int)cv_TimeChartId;
            UiForm.cv_mmfController.SendMmfNotifyObject(typeof(CommonData.HIRATA.MDForceIni).Name, obj, KParseObjToXmlPropertyType.Field);
            */
        }
        private void cv_btnReset_Click(object sender, EventArgs e)
        {
            /*
            if (UiForm.PSystemData.POperationModeLeft != OperationMode.Manual)// || UiForm.PSystemData.PSystemStatus== EquipmentStatus.Down)
            {
                CommonStaticData.PopForm("Please change to manual mode ", true, false);
                return;
            }
            CommonData.HIRATA.MDResetTimeChart obj = new CommonData.HIRATA.MDResetTimeChart();
            obj.cv_TimeChartId = (int)cv_TimeChartId;
            UiForm.cv_mmfController.SendMmfNotifyObject(typeof(CommonData.HIRATA.MDResetTimeChart).Name, obj, KParseObjToXmlPropertyType.Field);
            */
        }
    }
}

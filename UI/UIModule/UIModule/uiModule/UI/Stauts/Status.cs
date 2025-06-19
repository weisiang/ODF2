using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonData;
using BaseAp;


namespace UI
{
    public partial class StatusTable : UserControl
    {
        Dictionary<CommonData.HIRATA.APIEnum.EventCommand, Label> cv_map = new Dictionary<CommonData.HIRATA.APIEnum.EventCommand, Label>();
        ToolTip Pressure = new ToolTip();
        ToolTip Vacuum = new ToolTip();
        ToolTip Ionizer1 = new ToolTip();
        ToolTip Ionizer2 = new ToolTip();
        ToolTip Ionizer3 = new ToolTip();
        ToolTip Ionizer4 = new ToolTip();
        ToolTip Ionizer5 = new ToolTip();
        ToolTip Ionizer6 = new ToolTip();
        ToolTip Ionizer7 = new ToolTip();
        ToolTip Ionizer8 = new ToolTip();
        ToolTip FFU1 = new ToolTip();
        ToolTip FFU2 = new ToolTip();
        ToolTip FFU3 = new ToolTip();
        ToolTip FFU4 = new ToolTip();
        ToolTip FFU5 = new ToolTip();
        ToolTip FFU6 = new ToolTip();
        ToolTip FFU7 = new ToolTip();
        ToolTip FFU8 = new ToolTip();
        ToolTip FFU9 = new ToolTip();
        ToolTip FFU10 = new ToolTip();
        ToolTip FFU11 = new ToolTip();
        ToolTip Door = new ToolTip();
        ToolTip EMO = new ToolTip();
        ToolTip Power = new ToolTip();
        ToolTip RobotMode = new ToolTip();
        ToolTip RobotEnable = new ToolTip();
        public StatusTable()
        {
            InitializeComponent();
            InitDic();
            SetCircle();
        }
        private void InitDic()
        {
            cv_map.Add(CommonData.HIRATA.APIEnum.EventCommand.Pressure, lbl_pressure);
            cv_map.Add(CommonData.HIRATA.APIEnum.EventCommand.Vacuum, lbl_Vacuum);
            cv_map.Add(CommonData.HIRATA.APIEnum.EventCommand.Ionizer1, lbl_Ionizer1);
            cv_map.Add(CommonData.HIRATA.APIEnum.EventCommand.Ionizer2, lbl_Ionizer2);
            cv_map.Add(CommonData.HIRATA.APIEnum.EventCommand.Ionizer3, lbl_Ionizer3);
            cv_map.Add(CommonData.HIRATA.APIEnum.EventCommand.Ionizer4, lbl_Ionizer4);
            cv_map.Add(CommonData.HIRATA.APIEnum.EventCommand.Ionizer5, lbl_Ionizer5);
            cv_map.Add(CommonData.HIRATA.APIEnum.EventCommand.Ionizer6, lbl_Ionizer6);
            cv_map.Add(CommonData.HIRATA.APIEnum.EventCommand.Ionizer7, lbl_Ionizer7);
            cv_map.Add(CommonData.HIRATA.APIEnum.EventCommand.Ionizer8, lbl_Ionizer8);
            cv_map.Add(CommonData.HIRATA.APIEnum.EventCommand.FFU1, lbl_FFU1);
            cv_map.Add(CommonData.HIRATA.APIEnum.EventCommand.FFU2, lbl_FFU2);
            cv_map.Add(CommonData.HIRATA.APIEnum.EventCommand.FFU3, lbl_FFU3);
            cv_map.Add(CommonData.HIRATA.APIEnum.EventCommand.FFU4, lbl_FFU4);
            cv_map.Add(CommonData.HIRATA.APIEnum.EventCommand.FFU5, lbl_FFU5);
            cv_map.Add(CommonData.HIRATA.APIEnum.EventCommand.FFU6, lbl_FFU6);
            cv_map.Add(CommonData.HIRATA.APIEnum.EventCommand.FFU7, lbl_FFU7);
            cv_map.Add(CommonData.HIRATA.APIEnum.EventCommand.FFU8, lbl_FFU8);
            cv_map.Add(CommonData.HIRATA.APIEnum.EventCommand.FFU9, lbl_FFU9);
            cv_map.Add(CommonData.HIRATA.APIEnum.EventCommand.FFU10, lbl_FFU10);
            cv_map.Add(CommonData.HIRATA.APIEnum.EventCommand.FFU11, lbl_FFU11);
            cv_map.Add(CommonData.HIRATA.APIEnum.EventCommand.Door, lbl_Door);
            cv_map.Add(CommonData.HIRATA.APIEnum.EventCommand.EMO, lbl_Emo);
            cv_map.Add(CommonData.HIRATA.APIEnum.EventCommand.Power, lbl_power);
            cv_map.Add(CommonData.HIRATA.APIEnum.EventCommand.RobotMode, lbl_RobotMode);
            cv_map.Add(CommonData.HIRATA.APIEnum.EventCommand.RobotEnable, lbl_RobotEnable);

            Pressure.SetToolTip(lbl_pressure, "Pressure");
            Vacuum.SetToolTip(lbl_Vacuum, "Vacuum");
            Ionizer1.SetToolTip(lbl_Ionizer1, "Ionizer1");
            Ionizer2.SetToolTip(lbl_Ionizer2, "Ionizer2");
            Ionizer3.SetToolTip(lbl_Ionizer3, "Ionizer3");
            Ionizer4.SetToolTip(lbl_Ionizer4, "Ionizer4");
            Ionizer5.SetToolTip(lbl_Ionizer5, "Ionizer5");
            Ionizer6.SetToolTip(lbl_Ionizer6, "Ionizer6");
            Ionizer7.SetToolTip(lbl_Ionizer7, "Ionizer7");
            Ionizer8.SetToolTip(lbl_Ionizer8, "Ionizer8");
            FFU1.SetToolTip(lbl_FFU1, "FFU1");
            FFU2.SetToolTip(lbl_FFU2, "FFU2");
            FFU3.SetToolTip(lbl_FFU3, "FFU3");
            FFU4.SetToolTip(lbl_FFU4, "FFU4");
            FFU5.SetToolTip(lbl_FFU5, "FFU5");
            FFU6.SetToolTip(lbl_FFU6, "FFU6");
            FFU7.SetToolTip(lbl_FFU7, "FFU7");
            FFU8.SetToolTip(lbl_FFU8, "FFU8");
            FFU9.SetToolTip(lbl_FFU9, "FFU9");
            FFU10.SetToolTip(lbl_FFU10, "FFU10");
            FFU11.SetToolTip(lbl_FFU11, "FFU11");
            Door.SetToolTip(lbl_Door, "Door");
            EMO.SetToolTip(lbl_Emo, "EMO");
            Power.SetToolTip(lbl_power, "Power");
            RobotMode.SetToolTip(lbl_RobotMode, "RobotMode");
            RobotEnable.SetToolTip(lbl_RobotEnable, "RobotEnable");
        }
        private void SetCircle()
        {
            foreach (Label lbl in cv_map.Values)
            {
                MakeCircleLabel(lbl);
                lbl.BackColor = Color.Red;
            }
        }
        private void MakeCircleLabel(Label m_Label)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(m_Label.ClientRectangle);
            Region reg = new Region(path);
            m_Label.Region = reg;
        }
        public void ProcessEfemEventSingle(CommonData.HIRATA.APIEnum.EventCommand m_Type, int m_Value)
        {
            if(cv_map.ContainsKey(m_Type))
            {
                int normal = GetNormal(m_Type);
                cv_map[m_Type].BackColor = (normal == m_Value ? Color.Green : Color.Red);
            }
        }
        private int GetNormal(CommonData.HIRATA.APIEnum.EventCommand  m_Type)
        {
            int rtn = 0;
            switch (m_Type)
            {
                case CommonData.HIRATA.APIEnum.EventCommand.Pressure:
                    rtn = 1;
                    break;
                case CommonData.HIRATA.APIEnum.EventCommand.Vacuum:
                    rtn = 1;
                    break;
                case CommonData.HIRATA.APIEnum.EventCommand.Ionizer1:
                    rtn = 1;
                    break;
                case CommonData.HIRATA.APIEnum.EventCommand.Ionizer2:
                    rtn = 1;
                    break;
                case CommonData.HIRATA.APIEnum.EventCommand.Ionizer3:
                    rtn = 1;
                    break;
                case CommonData.HIRATA.APIEnum.EventCommand.Ionizer4:
                    rtn = 1;
                    break;
                case CommonData.HIRATA.APIEnum.EventCommand.Ionizer5:
                    rtn = 1;
                    break;
                case CommonData.HIRATA.APIEnum.EventCommand.Ionizer6:
                    rtn = 1;
                    break;
                case CommonData.HIRATA.APIEnum.EventCommand.Ionizer7:
                    rtn = 1;
                    break;
                case CommonData.HIRATA.APIEnum.EventCommand.Ionizer8:
                    rtn = 1;
                    break;
                case CommonData.HIRATA.APIEnum.EventCommand.FFU1:
                    rtn = 1;
                    break;
                case CommonData.HIRATA.APIEnum.EventCommand.FFU2:
                    rtn = 1;
                    break;
                case CommonData.HIRATA.APIEnum.EventCommand.FFU3:
                    rtn = 1;
                    break;
                case CommonData.HIRATA.APIEnum.EventCommand.FFU4:
                    rtn = 1;
                    break;
                case CommonData.HIRATA.APIEnum.EventCommand.FFU5:
                    rtn = 1;
                    break;
                case CommonData.HIRATA.APIEnum.EventCommand.FFU6:
                    rtn = 1;
                    break;
                case CommonData.HIRATA.APIEnum.EventCommand.FFU7:
                    rtn = 1;
                    break;
                case CommonData.HIRATA.APIEnum.EventCommand.FFU8:
                    rtn = 1;
                    break;
                case CommonData.HIRATA.APIEnum.EventCommand.FFU9:
                    rtn = 1;
                    break;
                case CommonData.HIRATA.APIEnum.EventCommand.FFU10:
                    rtn = 1;
                    break;
                case CommonData.HIRATA.APIEnum.EventCommand.FFU11:
                    rtn = 1;
                    break;
                case CommonData.HIRATA.APIEnum.EventCommand.Door:
                    rtn = 1;
                    break;
                case CommonData.HIRATA.APIEnum.EventCommand.Power:
                    rtn = 1;
                    break;
                case CommonData.HIRATA.APIEnum.EventCommand.RobotEnable:
                    rtn = 1;
                    break;
                case CommonData.HIRATA.APIEnum.EventCommand.RobotMode:
                    rtn = 1;
                    break;
                case CommonData.HIRATA.APIEnum.EventCommand.EMO:
                    rtn = 1;
                    break;
                default:
                    rtn = 1;
                    break;
            };
            return rtn;
        }
        public void ProcessEfemEvent(CommonData.HIRATA.MDEfemStatus m_Obj)
        {
            if (m_Obj != null)
            {
                cv_map[CommonData.HIRATA.APIEnum.EventCommand.Pressure].BackColor = (m_Obj.cv_Pressure == 1 ? Color.Green : Color.Red);
                cv_map[CommonData.HIRATA.APIEnum.EventCommand.Vacuum].BackColor = (m_Obj.cv_Vacuum == 1 ? Color.Green : Color.Red);
                cv_map[CommonData.HIRATA.APIEnum.EventCommand.Ionizer1].BackColor = (m_Obj.cv_Ionizer1 == 1 ? Color.Green : Color.Red);
                cv_map[CommonData.HIRATA.APIEnum.EventCommand.Ionizer2].BackColor = (m_Obj.cv_Ionizer2 == 1 ? Color.Green : Color.Red);
                cv_map[CommonData.HIRATA.APIEnum.EventCommand.Ionizer3].BackColor = (m_Obj.cv_Ionizer3 == 1 ? Color.Green : Color.Red);
                cv_map[CommonData.HIRATA.APIEnum.EventCommand.Ionizer4].BackColor = (m_Obj.cv_Ionizer4 == 1 ? Color.Green : Color.Red);
                cv_map[CommonData.HIRATA.APIEnum.EventCommand.Ionizer5].BackColor = (m_Obj.cv_Ionizer5 == 1 ? Color.Green : Color.Red);
                cv_map[CommonData.HIRATA.APIEnum.EventCommand.Ionizer6].BackColor = (m_Obj.cv_Ionizer6 == 1 ? Color.Green : Color.Red);
                cv_map[CommonData.HIRATA.APIEnum.EventCommand.Ionizer7].BackColor = (m_Obj.cv_Ionizer7 == 1 ? Color.Green : Color.Red);
                cv_map[CommonData.HIRATA.APIEnum.EventCommand.Ionizer8].BackColor = (m_Obj.cv_Ionizer8 == 1 ? Color.Green : Color.Red);
                cv_map[CommonData.HIRATA.APIEnum.EventCommand.FFU1].BackColor = (m_Obj.cv_FFU1 == 1 ? Color.Green : Color.Red);
                cv_map[CommonData.HIRATA.APIEnum.EventCommand.FFU2].BackColor = (m_Obj.cv_FFU2 == 1 ? Color.Green : Color.Red);
                cv_map[CommonData.HIRATA.APIEnum.EventCommand.FFU3].BackColor = (m_Obj.cv_FFU3 == 1 ? Color.Green : Color.Red);
                cv_map[CommonData.HIRATA.APIEnum.EventCommand.FFU4].BackColor = (m_Obj.cv_FFU4 == 1 ? Color.Green : Color.Red);
                cv_map[CommonData.HIRATA.APIEnum.EventCommand.FFU5].BackColor = (m_Obj.cv_FFU5 == 1 ? Color.Green : Color.Red);
                cv_map[CommonData.HIRATA.APIEnum.EventCommand.FFU6].BackColor = (m_Obj.cv_FFU6 == 1 ? Color.Green : Color.Red);
                cv_map[CommonData.HIRATA.APIEnum.EventCommand.FFU7].BackColor = (m_Obj.cv_FFU7 == 1 ? Color.Green : Color.Red);
                cv_map[CommonData.HIRATA.APIEnum.EventCommand.FFU8].BackColor = (m_Obj.cv_FFU8 == 1 ? Color.Green : Color.Red);
                cv_map[CommonData.HIRATA.APIEnum.EventCommand.FFU9].BackColor = (m_Obj.cv_FFU9 == 1 ? Color.Green : Color.Red);
                cv_map[CommonData.HIRATA.APIEnum.EventCommand.FFU10].BackColor = (m_Obj.cv_FFU10 == 1 ? Color.Green : Color.Red);
                cv_map[CommonData.HIRATA.APIEnum.EventCommand.FFU11].BackColor = (m_Obj.cv_FFU11 == 1 ? Color.Green : Color.Red);
                cv_map[CommonData.HIRATA.APIEnum.EventCommand.Door].BackColor = (m_Obj.cv_Door == 1 ? Color.Green : Color.Red);
                cv_map[CommonData.HIRATA.APIEnum.EventCommand.EMO].BackColor = (m_Obj.cv_EMO == 1 ? Color.Green : Color.Red);
                cv_map[CommonData.HIRATA.APIEnum.EventCommand.Power].BackColor = (m_Obj.cv_Power == 1 ? Color.Green : Color.Red);
                cv_map[CommonData.HIRATA.APIEnum.EventCommand.RobotMode].BackColor = (m_Obj.cv_RobotMode == 1 ? Color.Green : Color.Red);
                cv_map[CommonData.HIRATA.APIEnum.EventCommand.RobotEnable].BackColor = (m_Obj.cv_RobotEnable == 1 ? Color.Green : Color.Red);
            }
        }
        public void SetGlassCount()
        {
            if (cv_txtProductionCount.Text != lgcBase.cv_GlassCountData.PProductCount.ToString())
            {
                cv_txtProductionCount.Text = lgcBase.cv_GlassCountData.PProductCount.ToString();
            }
            if (cv_txtDummyCount.Text != lgcBase.cv_GlassCountData.PDummyCount.ToString())
            {
                cv_txtDummyCount.Text = lgcBase.cv_GlassCountData.PDummyCount.ToString();
            }
            if (cv_txtHistoryCount.Text != lgcBase.cv_GlassCountData.PHistoryCount.ToString())
            {
                cv_txtHistoryCount.Text = lgcBase.cv_GlassCountData.PHistoryCount.ToString();
            }
        }
    }
}

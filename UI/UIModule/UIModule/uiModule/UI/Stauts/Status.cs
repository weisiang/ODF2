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

namespace UI
{
    public partial class StatusTable : UserControl
    {
        Dictionary<CommonData.HIRATA.APIEnum.EventCommand, Label> cv_map = new Dictionary<CommonData.HIRATA.APIEnum.EventCommand, Label>();
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
            if (cv_txtProductionCount.Text != UiForm.cv_GlassCountData.PProductCount.ToString())
            {
                cv_txtProductionCount.Text = UiForm.cv_GlassCountData.PProductCount.ToString();
            }
            if (cv_txtDummyCount.Text != UiForm.cv_GlassCountData.PDummyCount.ToString())
            {
                cv_txtDummyCount.Text = UiForm.cv_GlassCountData.PDummyCount.ToString();
            }
            if (cv_txtHistoryCount.Text != UiForm.cv_GlassCountData.PHistoryCount.ToString())
            {
                cv_txtHistoryCount.Text = UiForm.cv_GlassCountData.PHistoryCount.ToString();
            }
        }
    }
}

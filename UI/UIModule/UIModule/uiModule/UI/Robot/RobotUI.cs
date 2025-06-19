using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonData.HIRATA;
using UI;
using BaseAp;

namespace UI.GUI
{
    public partial class RobotUI : UserControl , RobotReflash
    {
        public enum RobotGlassShape { rgsRectangle, rgsCircle };
        RobotGlassShape cv_UpArm;
        RobotGlassShape cv_DownArm;
        Control cv_UpObj = null;
        Control cv_DownObj = null;
        Control cv_Uplbl = null;
        Control cv_Downlbl = null;
        int cv_SlotCount = 0;
        int cv_Id = 0;

        public RobotUI(int m_id, int m_slotCount, RobotGlassShape m_UpShage, RobotGlassShape m_DownShape)
        {
            InitializeComponent();
            cv_SlotCount = m_slotCount;
            cv_Id = m_id;
            cv_Downlbl = lbl_down;
            cv_Uplbl = lbl_up;
            setcircleWafer(this.lbl_RobotStatus);
            setcircleWafer(this.picB_DownWafer);
            setcircleWafer(this.picB_UpWafer);
            setcircleWafer(this.lbl_up);
            setcircleWafer(this.lbl_down);
            ProcessShape(m_UpShage, m_DownShape);
            gb_Robot.Text = "ROBOT " + m_id.ToString();

                ToolStripMenuItem tmp = new ToolStripMenuItem();
                tmp.Text = CommonData.HIRATA.RobotArm.rbaDown.ToString();
                tmp.Click += dELETEToolStripMenuItem_Click;
                (cv_menuDataEdit.Items[0] as ToolStripMenuItem).DropDownItems.Add(tmp);
                ToolStripMenuItem tmp2 = new ToolStripMenuItem();
                tmp2.Text = CommonData.HIRATA.RobotArm.rbaUp.ToString();
                tmp2.Click += dELETEToolStripMenuItem_Click;
                (cv_menuDataEdit.Items[0] as ToolStripMenuItem).DropDownItems.Add(tmp2);
        }
        private void ProcessShape(RobotGlassShape m_UpShage, RobotGlassShape m_DownShape)
        {
            if (m_UpShage == RobotGlassShape.rgsRectangle) lbl_up.Visible = false;
            if (m_DownShape == RobotGlassShape.rgsRectangle) lbl_down.Visible = false;
            cv_UpArm = m_UpShage;
            cv_DownArm = m_DownShape;
            processShape(CommonData.HIRATA.RobotArm.rbaUp, cv_UpArm);
            processShape(CommonData.HIRATA.RobotArm.rbaDown, cv_DownArm);
        }
        private void processShape(CommonData.HIRATA.RobotArm m_Arm, RobotGlassShape m_Shape)
        {
            if (m_Arm == CommonData.HIRATA.RobotArm.rbaUp)
            {
                if(m_Shape == RobotGlassShape.rgsRectangle)
                {
                    picB_UpWafer.Visible = false;
                    lbl_UpGlass.Visible = true;
                    lbl_UpDataId.Visible = false;
                    cv_UpObj = lbl_UpGlass;
                }
                else
                {
                    picB_UpWafer.Visible = true;
                    lbl_UpGlass.Visible = false;
                    lbl_UpDataId.Visible = true;
                    cv_UpObj = picB_UpWafer;

                }
            }
            else
            {
                if (m_Shape == RobotGlassShape.rgsRectangle)
                {
                    picB_DownWafer.Visible = false;
                    lbl_DownGlass.Visible = true;
                    lbl_DownDataId.Visible = false;
                    cv_DownObj = lbl_DownGlass;
                }
                else
                {
                    picB_DownWafer.Visible = true;
                    lbl_DownGlass.Visible = false;
                    lbl_DownDataId.Visible = true;
                    cv_DownObj = picB_DownWafer;
                }
            }
        }
        private void setcircleWafer(Control m_Obj)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(m_Obj.ClientRectangle);
            Region reg = new Region(path);
            m_Obj.Region = reg;
        }
        public void refresh(RobotData m_Data)
        {
            if (lgcBase.PSystemData.PapiConnect)
            {
                if (lgcBase.PSystemData.PRobot1Status == CommonData.HIRATA.EquipmentStatus.Down)
                {
                    lbl_RobotStatus.BackColor = Color.Red;
                }
                else if (lgcBase.PSystemData.PRobot1Status == CommonData.HIRATA.EquipmentStatus.Idle)
                {
                    lbl_RobotStatus.BackColor = Color.Yellow;
                }
                else if (lgcBase.PSystemData.PRobot1Status == CommonData.HIRATA.EquipmentStatus.Run)
                {
                    lbl_RobotStatus.BackColor = Color.Lime;
                }
                else if (lgcBase.PSystemData.PRobot1Status == CommonData.HIRATA.EquipmentStatus.Stop)
                {
                    lbl_RobotStatus.BackColor = Color.Pink;
                }
            }
            else
            {
                lbl_RobotStatus.BackColor = Color.Red;
            }
            for (int i = 1; i <= cv_SlotCount ; i++)
            {
                RobotArm arm = (CommonData.HIRATA.RobotArm)i;
                GlassData glass = null;
                bool has_sensor = false;
                if (arm == CommonData.HIRATA.RobotArm.rbaDown)
                {
                    if (m_Data.GetSlotData((int)arm, ref glass , ref has_sensor))
                    {
                        if (glass.PHasData)
                        {
                            if (cv_DownArm == RobotGlassShape.rgsRectangle)
                            {
                                cv_DownObj.Text = glass.PId;
                                lbl_DownDataId.Visible = false;
                            }
                            else
                            {
                                cv_DownObj.Text = "";
                                lbl_DownDataId.Text = glass.PId;
                                lbl_DownDataId.Visible = true;
                            }
                        }
                        else
                        {
                            if (cv_DownArm == RobotGlassShape.rgsRectangle)
                            {
                                cv_DownObj.Text = "";
                                lbl_DownDataId.Visible = false;
                            }
                            else
                            {
                                cv_DownObj.Text = "";
                                lbl_DownDataId.Text = "";
                                lbl_DownDataId.Visible = true;
                            }
                        }
                    }
                    cv_DownObj.Visible = true;
                    if(cv_DownArm == RobotGlassShape.rgsCircle)
                        CommonStaticData.ChangeRobotItemColor(ref cv_Downlbl, glass.PHasData, glass.PHasSensor);
                    else
                        CommonStaticData.ChangeRobotItemColor(ref cv_DownObj, glass.PHasData, glass.PHasSensor);
                }
                else
                {
                    if (m_Data.GetSlotData((int)arm, ref glass, ref has_sensor))
                    {
                        if (glass.PHasData)
                        {
                            if (cv_UpArm == RobotGlassShape.rgsRectangle)
                            {
                                cv_UpObj.Text = glass.PId;
                                lbl_UpDataId.Visible = false;
                            }
                            else
                            {
                                lbl_UpDataId.Text = glass.PId;
                                lbl_UpDataId.Visible = true;
                            }
                        }
                        else
                        {
                            if (cv_UpArm == RobotGlassShape.rgsRectangle)
                            {
                                cv_UpObj.Text = "";
                                lbl_UpDataId.Visible = false;
                            }
                            else
                            {
                                lbl_UpDataId.Text = "";
                                lbl_UpDataId.Visible = true;
                            }
                        }
                        cv_UpObj.Visible = true;
                        if(cv_UpArm == RobotGlassShape.rgsCircle)
                        CommonStaticData.ChangeRobotItemColor(ref cv_Uplbl, glass.PHasData, glass.PHasSensor);
                        else
                        CommonStaticData.ChangeRobotItemColor(ref cv_UpObj, glass.PHasData, glass.PHasSensor);
                    }
                }
            }
        }

        private void dELETEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItem tmp = sender as ToolStripItem;
            int slot = (int)Enum.Parse( typeof( CommonData.HIRATA.RobotArm) ,    tmp.Text.Trim());
            UiForm.cv_GlassDataForm.Register(ActionTarget.Robot, cv_Id, slot);
            UiForm.cv_GlassDataForm.Show();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonData.HIRATA;
using UI.GUI;

namespace UI
{
    class Robot : Obj
    {
        public RobotData cv_Data = null;
        public Robot(int m_Id, int m_SlotCount , RobotUI.RobotGlassShape m_UpShage, RobotUI.RobotGlassShape m_DownShape)
            : base(m_Id, m_SlotCount)
        {
            InitUI(m_Id, m_SlotCount, m_UpShage, m_DownShape);
        }
        private void InitUI(int m_Id, int m_SlotCount, RobotUI.RobotGlassShape m_UpShage, RobotUI.RobotGlassShape m_DownShape)
        {
            if (cv_Ui == null)
            {
                cv_Ui = new RobotUI(cv_Id, cv_SlotCount, m_UpShage, m_DownShape);
            }
        }
        public override void reFresh()
        {
            (cv_Ui as RobotUI).refresh(cv_Data);
        }

    }
}

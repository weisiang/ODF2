using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonData.HIRATA;
using UI.GUI;

namespace UI
{
    class Aligner : Obj
    {
        public AlignerData cv_Data = null;
        public Aligner(int m_Id, int m_SlotCount)
            : base(m_Id, m_SlotCount)
        {
            //InitData();
            InitUI();
            //InitComm();
        }
        protected override void InitUI()
        {
            if (cv_Ui == null)
            {
                cv_Ui = new AlignerUI(cv_Id, cv_SlotCount);
            }
        }
        /*
        protected override void InitComm()
        {
            if (cv_Comm == null)
            {
                cv_Comm = new AlignerComm();
            }
        }
        protected override void InitData()
        {
            if (cv_Data == null)
            {
                cv_Data = new AlignerData(cv_Id, cv_SlotCount);
            }
        }
        */
        public override void reFresh()
        {
            (cv_Ui as AlignerUI).refresh(cv_Data);
        }
    }
}

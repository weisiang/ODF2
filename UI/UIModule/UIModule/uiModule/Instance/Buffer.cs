using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonData.HIRATA;
using UI.GUI;

namespace UI
{
    class Buffer : Obj
    {
        public BufferData cv_Data = null;
        public Buffer(int m_Id, int m_SlotCount)
            : base(m_Id, m_SlotCount)
        {
            InitUI();
        }
        protected override void InitUI()
        {
            if (cv_Ui == null)
            {
                cv_Ui = new BufferUI(cv_Id, cv_SlotCount);
            }
        }
        public override void reFresh()
        {
            (cv_Ui as BufferUI).refresh(cv_Data);
        }
    }
}

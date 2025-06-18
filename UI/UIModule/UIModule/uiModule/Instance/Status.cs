using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI
{
    class Status : Obj
    {
        public Status(int m_Id, int m_SlotCount)
            : base(m_Id, m_SlotCount)
        {
            InitUI();
        }
        protected override void InitUI()
        {
            if (cv_Ui == null)
            {
                cv_Ui = new StatusTable();
            }
        }
        public StatusTable GetUI()
        {
            return cv_Ui as StatusTable;
        }
    }
}

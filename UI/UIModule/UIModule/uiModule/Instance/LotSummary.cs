using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace UI
{
    class LotSummary : Obj
    {
        public LotSummary(int m_Id, int m_SlotCount, BindingList<CommonData.HIRATA.PortData> m_list)
            : base(m_Id, m_SlotCount)
        {
            InitUI(m_list);
        }
        private void InitUI(BindingList<CommonData.HIRATA.PortData> m_list)
        {
            if (cv_Ui == null)
            {
                cv_Ui = new LotSummaryUI(m_list);
            }
        }
        public override void reFresh()
        {
            (cv_Ui as LotSummaryUI).reFresh();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.GUI;
using CommonData.HIRATA;

namespace UI
{
    class Eq : Obj
    {
        public EqData cv_Data = null;
        public string cv_Alias = "";
        public Eq(int m_Id, int m_Node , int m_Stage , int m_TimeChartId ,  int m_SlotCount , string m_alias = "")
            : base(m_Id, m_SlotCount)
        {
            cv_Node = m_Node;
            cv_Stage = m_Stage;
            cv_TimeChartId = m_TimeChartId;
            cv_Alias = m_alias;
            InitUI(cv_Id, cv_SlotCount , m_alias);
        }
        private void InitUI(int m_id, int m_slotNumber, string m_alias = "")
        {
            if (cv_Ui == null)
            {
                cv_Ui = new EqUI(cv_Id, cv_SlotCount , cv_Alias);
            }
        }
        public override void reFresh()
        {
            cv_Ui.Refresh();
        }

    }

}

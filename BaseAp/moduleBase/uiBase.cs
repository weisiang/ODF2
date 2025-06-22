using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KgsCommon;
using CommonData.HIRATA;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BaseAp
{
    public class uiBase : moduleBase
    {
        public uiBase(FdModule m_Module) : base(m_Module)
        {
        }
        ~uiBase()
        {
        }
        protected override void DerivedOnTimer()
        {
            WriteLog1(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog1(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
    }
}

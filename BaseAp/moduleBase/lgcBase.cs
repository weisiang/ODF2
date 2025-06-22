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
    public class lgcBase: moduleBase
    {

        public static CommonData.HIRATA.SystemData cv_SystemData = new SystemData();
        public static SystemData PSystemData
        {
            get { return cv_SystemData; }
            set { cv_SystemData = value; }
        }
        public static AlarmData cv_Alarms = new AlarmData();
        public static RecipeData cv_Recipes = new RecipeData();
        public static TimeOutData cv_TimeoutData = new TimeOutData();
        public static GlassCountData cv_GlassCountData = new GlassCountData();



        public lgcBase(FdModule m_Module):base(m_Module)
        {
            Global.LogIniPathname = CommonData.HIRATA.CommonStaticData.g_LgcModuleLogsIniFile;
            Global.SystemIniPathname = CommonData.HIRATA.CommonStaticData.g_LgcModuleSystemIniFile;
        }

        ~lgcBase()
        {
        }
        protected override void ModuleInit()
        {
        }
        protected override void DerivedOnTimer()
        {
            WriteLog1(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            /*
            try
            {
                if (SysUtils.MilliSecondsBetween(SysUtils.Now(), cv_MemDateTime) > 30000)
                {
                    WriteLog(LogLevelType.General, "Mem : " + CommonData.HIRATA.CommonStaticData.GetMemUsage().ToString());

                    cv_MemDateTime = SysUtils.Now();
                }
                DoModuleCommInit();
                //DerivedOnTimer();
            }
            catch (Exception ex)
            {
            }
            */
            WriteLog1(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
    }
}

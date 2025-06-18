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
    public class lgcBase
    {
        protected FdModule cv_Module = FdModule.None;
        protected System.Timers.Timer cv_Timer;
        public static KMemoLog cv_Log;
        public static KMemoryIOClient cv_Mio = null;
        public static BaseEventController cv_mmfController = null;
        public lgcBase(FdModule m_Module)
        {
            cv_Module = m_Module;
            CommonData.HIRATA.CommonStaticData.Init(cv_Module.ToString());
            initMio();
            initLog();
            ModuleInit();
            InitTimer();
        }
        public lgcBase()
        {
        }

        ~lgcBase()
        {
        }
        protected virtual void ModuleInit()
        {
        }
        private void initMio()
        {
            cv_Mio = new KMemoryIOClient();
            cv_Mio.ServerName = "KGSMEMORYIODEMO";
            cv_Mio.Open();
        }
        protected virtual void initLog()
        {
            if (cv_Log == null)
            {
                string enviPath = CommonData.HIRATA.CommonStaticData.g_RootLogsFolderPath + CommonData.HIRATA.CommonStaticData.g_FDModuleName;
                cv_Log = new KMemoLog();
                cv_Log.LoadFromIni(CommonData.HIRATA.CommonStaticData.g_ModuleLogsIniFile, CommonData.HIRATA.CommonStaticData.g_FDModuleName);
                cv_Log.LogFileName = enviPath + "\\" + CommonData.HIRATA.CommonStaticData.g_FDModuleName + ".log";
                cv_Log.SaveToIni(CommonData.HIRATA.CommonStaticData.g_ModuleLogsIniFile, CommonData.HIRATA.CommonStaticData.g_FDModuleName);
            }
        }
        public static void WriteLog(LogLevelType m_Type, string m_str, CommonData.HIRATA.FunInOut m_FunInOut = CommonData.HIRATA.FunInOut.None)
        {
            string log = "";
            int level = (int)(SamekLogLevelType)Enum.Parse(typeof(SamekLogLevelType), m_Type.ToString());
            if (m_Type == LogLevelType.NormalFunctionInOut)
            {
                if (m_FunInOut != CommonData.HIRATA.FunInOut.None)
                {
                    log = "[FUN_" + m_FunInOut.ToString() + " ]" + m_str;
                    if (m_FunInOut == FunInOut.Leave)
                    {
                        log += "\n---------------------------------------------";
                    }
                }
            }
            else if (m_Type == LogLevelType.TimerFunction)
            {
                if (m_FunInOut != CommonData.HIRATA.FunInOut.None)
                {
                    log = "[Timer FUN_" + m_FunInOut.ToString() + " ]" + m_str;
                }
            }
            else
            {
                log = "[" + m_Type.ToString() + " ]" + m_str;
            }

            if (cv_Log != null)
            {
                lock (cv_Log)
                {
                    try
                    {
                        cv_Log.WriteLog(log, level);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }
        private void InitTimer()
        {
            if (cv_Timer == null)
            {
                cv_Timer = new System.Timers.Timer();
                cv_Timer.Interval = 500;
                cv_Timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimer);
            }
        }
        private void OnTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            DerivedOnTimer();
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
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        //derived class can override this timer function , if need.
        protected virtual void DerivedOnTimer()
        {
        }
    }
}

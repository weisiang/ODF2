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
    public class moduleBase
    {
        protected FdModule cv_Module = FdModule.None;
        protected System.Timers.Timer cv_Timer;
        public  KMemoLog cv_Log;
        public  KMemoryIOClient cv_Mio1 = null;

        public moduleBase(FdModule m_Module)
        {
            cv_Module = m_Module;
            ModuleInit();
            initMio();
            initLog();
            InitTimer();
        }

        ~moduleBase()
        {
        }
        protected virtual void ModuleInit()
        {
        }
        private void initMio()
        {
            bool need_mio = true;
            string inifile = "";
            if (cv_Module == FdModule.CIM)
            {
                inifile += CommonData.HIRATA.CommonStaticData.g_CimModuleSystemIniFile;
            }
            else if (cv_Module == FdModule.UI)
            {
                inifile += CommonData.HIRATA.CommonStaticData.g_UiModuleSystemIniFile;
            }
            else if (cv_Module == FdModule.LGC)
            {
                inifile += CommonData.HIRATA.CommonStaticData.g_LgcModuleSystemIniFile;
            }
            if (inifile != "" )
            {
                KIniFile ini = new KIniFile(inifile);
                need_mio = ini.ReadString("ModuleMio", "Need", "1") == "1" ? true : false;
            }

            if (need_mio && (inifile != "" ))
            {
                cv_Mio1 = new KMemoryIOClient();
                cv_Mio1.ServerName = "KGSMEMORYIODEMO";
                cv_Mio1.Open();
            }
        }
        protected virtual void initLog()
        {
            if (cv_Log == null)
            {

                string logini="";
                string enviPath = CommonData.HIRATA.CommonStaticData.g_RootLogsFolderPath;
                if (cv_Module == FdModule.CIM)
                {
                    enviPath += CommonData.HIRATA.CommonStaticData.g_CimModule;
                    logini = CommonData.HIRATA.CommonStaticData.g_CimModuleLogsIniFile;
                }
                else if (cv_Module == FdModule.UI)
                {
                    enviPath += CommonData.HIRATA.CommonStaticData.g_UiModule;
                    logini = CommonData.HIRATA.CommonStaticData.g_UiModuleLogsIniFile;
                }
                else if (cv_Module == FdModule.LGC)
                {
                    enviPath += CommonData.HIRATA.CommonStaticData.g_LgcModule;
                    logini = CommonData.HIRATA.CommonStaticData.g_LgcModuleLogsIniFile;
                }

                cv_Log = new KMemoLog();
                cv_Log.LoadFromIni(logini, cv_Module.ToString());
                cv_Log.LogFileName = enviPath + "\\" + cv_Module.ToString() + ".log";
                cv_Log.SaveToIni(logini, cv_Module.ToString());
            }
        }
        public  void WriteLog1(LogLevelType m_Type, string m_str, CommonData.HIRATA.FunInOut m_FunInOut = CommonData.HIRATA.FunInOut.None)
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
            WriteLog1(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            DerivedOnTimer();
            WriteLog1(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        //derived class can override this timer function , if need.
        protected virtual void DerivedOnTimer()
        {
        }
    }
}

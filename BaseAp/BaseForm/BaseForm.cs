using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using KgsCommon;
using CommonData.HIRATA;
using System.Reflection;

namespace BaseAp
{

    public partial class BaseForm : Form
    {
        protected KDateTime cv_MemDateTime = SysUtils.Now();
        public static CommonData.HIRATA.SystemData cv_SystemData = new SystemData();
        public static SystemData PSystemData
        {
            get { return cv_SystemData; }
            set { cv_SystemData = value; }
        }

        public static AlarmData cv_Alarms = new AlarmData();
        public static AccountData cv_AccountData = new AccountData();
        public static RecipeData cv_Recipes = new RecipeData();
        public static TimeOutData cv_TimeoutData = new TimeOutData();
        public static GlassCountData cv_GlassCountData = new GlassCountData();

        protected FdModule cv_Module = FdModule.None;
        protected System.Timers.Timer cv_Timer;
        public static KMemoLog cv_Log;
        public static KMemoryIOClient cv_Mio = null;
        public static BaseEventController cv_mmfController = null;

        public BaseForm(string[] args, FdModule m_Module)
        {
            InitializeComponent();
            cv_Module = m_Module;
            CommonData.HIRATA.CommonStaticData.Init(cv_Module.ToString());
            ModuleInit();
            initMio();
            initLog();
            InitTimer();
        }
        public BaseForm()
        {
            InitializeComponent();
        }

        ~BaseForm()
        {
        }

        /// <summary>
        /// Each module override this function to decide Account , Alarm , Recipe Data.
        ///And call this function by derived class.
        /// </summary>
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
                cv_Timer.SynchronizingObject = this;
            }
        }
        private void OnTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            DerivedOnTimer();
            try
            {
                if (SysUtils.MilliSecondsBetween(SysUtils.Now(), cv_MemDateTime) > 30000)
                {
                    WriteLog(LogLevelType.General, "Mem : " + CommonData.HIRATA.CommonStaticData.GetMemUsage().ToString());

                    cv_MemDateTime = SysUtils.Now();
                }
                //DoModuleCommInit();
                //DerivedOnTimer();
            }
            catch (Exception ex)
            {
            }
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        //derived class can override this timer function , if need.
        protected virtual void DerivedOnTimer()
        {
        }
    }
}

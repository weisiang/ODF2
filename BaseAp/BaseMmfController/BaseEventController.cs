using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KgsCommon;
using System.Reflection;
using CommonData.HIRATA;
using System.Threading;
using System.Timers;

namespace BaseAp
{
    public class BaseEventController
    {

        object lockObj = new object();
        protected FdModule cv_module;
        public delegate void deleSubscription(FdModule module, string messageId, Object obj);
        public Dictionary<string, deleSubscription> subscriptionMap = new Dictionary<string, deleSubscription>();
        public Queue<EventObj>eventQueue = new Queue<EventObj>();
        KTimer cv_timer = null;

        public  KMemoLog cv_ControllerLog;
        public BaseEventController(FdModule m_Module)
        {
            cv_module = m_Module;
            logsSetting();
            addSubScription();
            linkEvent();
            IniEventTimer();
        }
        public virtual void linkEvent()
        {

        }
        ~BaseEventController()
        {
            if(cv_timer != null)
            {
                cv_timer.Close();
            }
        }
        public virtual void addSubScription()
        {
            subscriptionMap.Add(typeof(CommonData.HIRATA.MDLgcStart).Name, ProcessLgcStart);  //from UI do initial action.
            subscriptionMap.Add(typeof(CommonData.HIRATA.MDCimStart).Name, ProcessCimStart);  //from UI do initial action.
            subscriptionMap.Add(typeof(CommonData.HIRATA.MDUiStart).Name, ProcessUiStart);  //from UI do initial action.
            if (cv_module == FdModule.UI)
            {
                subscriptionMap.Add(typeof(CommonData.HIRATA.SystemData).Name, ProcessMmfEvent);  //from UI do initial action.
            }
            else
            {
                subscriptionMap.Add(typeof(CommonData.HIRATA.SystemData).Name, ProcessSystemData);  //from UI do initial action.
            }
        }

        public void receiveSubcription(FdModule module, string messageId, Object obj)
        {
            Console.WriteLine("["+ cv_module.ToString() + "]receiveSubcription : " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            if(subscriptionMap.ContainsKey(messageId))
            {
                EventObj tmp = new EventObj();
                tmp.messageId = messageId;
                tmp.module = module;
                tmp.obj = obj;
                if(eventQueue != null)
                {
                    lock (lockObj)
                    {
                        eventQueue.Enqueue(tmp);
                    }
                }
            }
        }

        private void IniEventTimer()
        {
            if(cv_timer == null)
            {
                cv_timer = new KTimer();
                cv_timer.ThreadEventEnabled = true;
                cv_timer.Interval = 5;
                cv_timer.OnTimer += OnThread;
                cv_timer.Open();
                cv_timer.Enabled = true;
            }
        }

        private void OnThread()
        {
            Console.WriteLine("OnThread : " + System.Threading.Thread.CurrentThread.ManagedThreadId + ". time : " + SysUtils.Now().LongTimeString());
            if (eventQueue.Count > 0)
            {
                if (subscriptionMap.ContainsKey(eventQueue.Peek().messageId))
                {
                    string msgid = eventQueue.Peek().messageId;
                    EventObj obj = null;
                    lock (lockObj)
                    {
                        obj = eventQueue.Dequeue();
                    }
                    if ((obj != null) && (msgid != ""))
                    {
                        subscriptionMap[msgid](obj.module, msgid, obj.obj);
                    }
                }
                else
                {
                    lock (lockObj)
                    {
                        eventQueue.Dequeue();
                    }
                }
            }
        }

        #region virtual receive lgc , ui , cim start 
        protected virtual void ProcessUiStart(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
        }
        protected virtual void ProcessLgcStart(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
        }
        protected virtual void ProcessCimStart(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
        }
        #endregion



        #region relative log function
        void logsSetting()
        {
            string logini="";
            string enviPath = CommonData.HIRATA.CommonStaticData.g_RootLogsFolderPath;
            if (cv_module == FdModule.CIM)
            {
                enviPath += CommonData.HIRATA.CommonStaticData.g_CimModule;
                logini = CommonData.HIRATA.CommonStaticData.g_CimModuleLogsIniFile;
            }
            else if (cv_module == FdModule.UI)
            {
                enviPath += CommonData.HIRATA.CommonStaticData.g_UiModule;
                logini = CommonData.HIRATA.CommonStaticData.g_UiModuleLogsIniFile;
            }
            else if (cv_module == FdModule.LGC)
            {
                enviPath += CommonData.HIRATA.CommonStaticData.g_LgcModule;
                logini = CommonData.HIRATA.CommonStaticData.g_LgcModuleLogsIniFile;
            }

            /*
            KFileLog cv_MmfClientLog;
            cv_MmfClientLog = new KFileLog();
            cv_MmfClientLog.LoadFromIni(Global.LogIniPathname, "MmfEventClient");
            cv_MmfClientLog.LogFileName = enviPath + "\\MmfClient.log";
            cv_MmfClientLog.SaveToIni(Global.LogIniPathname, "MmfEventClient");
            cv_MmfClientLog = null;
            */

            cv_ControllerLog = new KMemoLog();
            cv_ControllerLog.LoadFromIni(logini, "CONTROLLER");
            cv_ControllerLog.LogFileName = enviPath + "\\Controller.log";
            cv_ControllerLog.SaveToIni(logini, "CONTROLLER");
            cv_ControllerLog.WriteLog("[" + cv_module.ToString() +"]  Create Controller Log");

            /*
            Global.DebugLog = new KFileLog();
            Global.DebugLog.LoadFromIni(Global.LogIniPathname, "DebugLog");
            Global.DebugLog.LogFileName = enviPath + "\\Debug.log";
            Global.DebugLog.SaveToIni(Global.LogIniPathname, "DebugLog");
            Global.DebugLog.WriteLog("Create DebugLog");
            */
        }
        public void WriteLog(LogLevelType m_Type, string m_str, CommonData.HIRATA.FunInOut m_FunInOut = CommonData.HIRATA.FunInOut.None)
        {
            string log = "";
            int level = (int)(SamekLogLevelType)Enum.Parse(typeof(SamekLogLevelType), m_Type.ToString());
            if (m_Type == LogLevelType.NormalFunctionInOut)
            {
                if (m_FunInOut != CommonData.HIRATA.FunInOut.None)
                {
                    log = "[FUN_" + m_FunInOut.ToString() + " ]" + m_str;
                    if(m_FunInOut == FunInOut.Leave)
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

            if (cv_ControllerLog != null)
            {
                lock (cv_ControllerLog)
                {
                    try
                    {
                        cv_ControllerLog.WriteLog(log, level);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }
        #endregion


        #region base demand event. CIM or UI module send request wonder change common data. then Lgc receive and process.
        protected virtual void ProcessOnlineReq(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessInitialize(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessAlarmChange(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            /*
            CommonData.HIRATA.AlarmData obj = m_Object as CommonData.HIRATA.AlarmData;
            BaseForm.cv_Alarms.Clone(obj);
            */
            string log = "";
            log += "Alarm list : " + Environment.NewLine;
            for (int i = 0; i < lgcBase.cv_Alarms.cv_AlarmList.Count; i++)
            {
                log += "Code : " + lgcBase.cv_Alarms.cv_AlarmList[i].PCode;
                log += "Code : " + lgcBase.cv_Alarms.cv_AlarmList[i].PCode;
                log += ". Level : " + lgcBase.cv_Alarms.cv_AlarmList[i].PLevel.ToString();
                log += ". Time : " + lgcBase.cv_Alarms.cv_AlarmList[i].PTime.ToString() + Environment.NewLine;
            }
            if (!string.IsNullOrEmpty(log))
            {
                WriteLog(LogLevelType.Detail, log);
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessSetTimeOut(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessRecipeAction(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            MDRecipeAction obj = m_Object as MDRecipeAction;
            string log = "Recipe Action : " + obj.PAction.ToString(); 
            if (!string.IsNullOrEmpty(log))
            {
                WriteLog(LogLevelType.Detail, log);
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessOntMode(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }




        protected virtual void ProcessChangePortSlotType(Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessAccountChange(Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            /*
            CommonData.HIRATA.AccountData obj = m_Object as CommonData.HIRATA.AccountData;
            BaseForm.cv_AccountData.Clone(obj);
            */
            string log = "";
            log += "cv_CurAccountId = " + BaseForm.cv_AccountData.PCurAccountId + Environment.NewLine;
            log += "cv_CurPermission = " + BaseForm.cv_AccountData.cv_CurPermission.ToString() + Environment.NewLine;
            log += "Account list : " + Environment.NewLine;
            for (int i = 0; i < BaseForm.cv_AccountData.cv_AccountList.Count; i++ )
            {
                log += "Id : " + BaseForm.cv_AccountData.cv_AccountList[i].PId;
                log += ". Pw : " + BaseForm.cv_AccountData.cv_AccountList[i].PPw;
                log += ". Permission : " + BaseForm.cv_AccountData.cv_AccountList[i].PPermission.ToString() + Environment.NewLine;
            }
            if(!string.IsNullOrEmpty(log))
            {
                WriteLog(LogLevelType.Detail, log);
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessLogInOut(Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CommonData.HIRATA.MDLogInOut obj = m_Object as CommonData.HIRATA.MDLogInOut;
            string log = "LogInOut Action : " + obj.PAction.ToString();
            if (!string.IsNullOrEmpty(log))
            {
                WriteLog(LogLevelType.Detail, log);
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessAlarmAction( Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            MDAlarmAction obj = m_Object as MDAlarmAction;
            string log = "";
            log += "Alarm list : " + Environment.NewLine;
            for (int i = 0; i < lgcBase.cv_Alarms.cv_AlarmList.Count; i++)
            {
                log += "Code : " + lgcBase.cv_Alarms.cv_AlarmList[i].PCode;
                log += ". Level : " + lgcBase.cv_Alarms.cv_AlarmList[i].PLevel.ToString();
                log += ". Time : " + lgcBase.cv_Alarms.cv_AlarmList[i].PTime.ToString() + Environment.NewLine;
            }
            if (!string.IsNullOrEmpty(log))
            {
                WriteLog(LogLevelType.Detail, log);
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessRecipeChange(Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            /*
            RecipeData obj = m_Object as RecipeData;
            BaseForm.cv_Recipes.Clone(obj);
            */
            string log = "";
            log += "Current Recipe : " + lgcBase.cv_Recipes.PCurRecipeId + Environment.NewLine;
            log += "Recipe list : " + Environment.NewLine;
            for (int i = 0; i < lgcBase.cv_Recipes.cv_RecipeList.Count; i++)
            {
                log += "Id : " + lgcBase.cv_Recipes.cv_RecipeList[i].PId;
                log += ". Flow : " + lgcBase.cv_Recipes.cv_RecipeList[i].PFlow.ToString() + Environment.NewLine;
            }
            if (!string.IsNullOrEmpty(log))
            {
                WriteLog(LogLevelType.Detail, log);
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessRecipeReq( Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessAlarmReq( Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessAccountReq(Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessMmfEvent(FdModule module, string m_MessageId, Object m_Object)
        {

        }
        protected virtual void ProcessSystemData(FdModule module, string messageId, Object obj)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            /*
            SystemData obj = m_Object as SystemData;
            BaseForm.PSystemData.Clone(obj);
            */
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessTimeoutData(Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            /*
            TimeOutData obj = m_Object as TimeOutData;
            BaseForm.cv_TimeoutData.Clone(obj);
            */
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessOperatorModeChange(Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessSystemDataReq(Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessGlassCountData(Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            /*
            GlassCountData obj = m_Object as GlassCountData;
            BaseForm.cv_GlassCountData.Clone(obj);
            */
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessGlassCountDataReq(Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessTimeOutReq(Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessRobotAction(Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessShowOcrDecide(Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessOcrDecideReply(Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected virtual void ProcessRobotJobAction( Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        #endregion

    }
}

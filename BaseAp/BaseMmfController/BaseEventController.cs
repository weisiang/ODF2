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

        public delegate void deleSubscription(FdModule module, string messageId, Object obj);
        public Dictionary<string, deleSubscription> subscriptionMap = new Dictionary<string, deleSubscription>();
        public Queue<EventObj>eventQueue = new Queue<EventObj>();
        //public System.Timers.Timer eventTimer;
        System.Threading.Thread eventThread;

        public  KMemoLog cv_ControllerLog;
        public BaseEventController(FdModule m_module)
        {
            InitialBase();
            logsSetting();
            addSubScription();
            linkEvent();
        }
        public virtual void linkEvent()
        {
        }
        ~BaseEventController()
        {
            if(eventThread != null)
            {
                eventThread.Abort();
                eventThread = null;
            }
            /*
            if(eventTimer != null)
            {
                eventTimer.Stop();
                eventTimer = null;
            }
            */
        }
        public virtual void addSubScription()
        {
        }

        public void receiveSubcription(FdModule module, string messageId, Object obj)
        {
            if(subscriptionMap.ContainsKey(messageId))
            {
                subscriptionMap[messageId](module, messageId, obj);
                EventObj tmp = new EventObj();
                tmp.messageId = messageId;
                tmp.module = module;
                tmp.obj = obj;
                if(eventQueue != null)
                {
                    eventQueue.Enqueue(tmp);
                }
            }
        }

        private void IniEventTimer()
        {
            /*
            if(eventTimer == null)
            {
                eventTimer = new System.Timers.Timer();
                eventTimer.Interval = 5;
                eventTimer.Elapsed += EventTimer_Elapsed;
                eventTimer.Start();
            }
            */
            if(eventThread == null)
            {
                eventThread = new Thread(OnThread);
                eventThread.Start();
            }
        }

        private void EventTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            /*
            if(eventQueue.Count >0)
            {
                if(subscriptionMap.ContainsKey(eventQueue.Peek().messageId))
                {
                    EventObj obj = eventQueue.Dequeue();
                    string msgid = obj.messageId;
                    subscriptionMap[msgid](obj.module.ToString(), msgid, obj.obj);
                }
                else
                {
                    eventQueue.Dequeue();
                }
            }
            */
        }
        private void OnThread()
        {
            if(eventQueue.Count >0)
            {
                if(subscriptionMap.ContainsKey(eventQueue.Peek().messageId))
                {
                    EventObj obj = eventQueue.Dequeue();
                    string msgid = obj.messageId;
                    subscriptionMap[msgid](obj.module, msgid, obj.obj);
                }
                else
                {
                    eventQueue.Dequeue();
                }
            }
        }

        #region alarm , account , recipe , timeout , glassCount , systemData event link and process functions.
        private void LinkCommonDataEvent(AlarmData cv_Alarms, AccountData cv_AccountData, RecipeData cv_Recipes, TimeOutData cv_TimeoutData, GlassCountData cv_GlassCountData, SystemData cv_SystemData)
        {
            if(cv_AccountData != null)
            {
                cv_AccountData.EventLogInOut += OnLogInOutEvent;
                cv_AccountData.EventAccountChange += OnAccountChangeEvent;
            }
            if(cv_Alarms != null)
            {
                cv_Alarms.EventAlarmAction += OnAlarmActionEvent;
                cv_Alarms.EventAlarmCharge += OnAlarmChange;
            }
            if(cv_Recipes != null)
            {
                cv_Recipes.EventRecipeAction += OnRecipeActionEvent;
            }
            if(cv_TimeoutData != null)
            {
                cv_TimeoutData.EventTimeOutDataChange += OnTimeOutDataChange;
            }
            if(cv_GlassCountData != null)
            {
                cv_GlassCountData.EventGlassCountChange += OnGlassCountDataChange;
            }
            if(cv_SystemData != null)
            {
                cv_SystemData.OnSystemDataChange += OnSystemDataChange;
                cv_SystemData.OnRobot1StatusChange += OnRobot1StatusChange;
                cv_SystemData.OnRobot2StatusChange += OnRobot2StatusChange;
                cv_SystemData.OnSystemStatusChange += OnSystemStatusChange;
                cv_SystemData.OnSystemStatusChange += OnSystemStatusChange;
                cv_SystemData.OnApiConnected += OnApiConnected;
                cv_SystemData.OnApiDisconnected += OnApiDisconnected;
                cv_SystemData.OnOperationModeChangeLeft += OnOperationModeChangeLeft;
                cv_SystemData.OnOperationModeChangeRight += OnOperationModeChangeRight;
                cv_SystemData.OnPlcConnected += OnPlcConnected ;
                cv_SystemData.OnPlcDisconnected += OnPlcDisconnected ;
                cv_SystemData.OnBclive +=  OnBclive;
                cv_SystemData.OnBcDie +=  OnBcDie;
            }
        }

        #region Link log in/out Event & Event function;
        //Trigger this event When AccountData login/out successful.(UI must override)
        protected virtual void OnLogInOutEvent(LogInOut m_Action, CommonData.HIRATA.AccountItem m_CurAccount)
        {
        }

        //Trigger this event When AccountData change.(UI must override)
        protected virtual void OnAccountChangeEvent()
        {
        }
        #endregion

        #region Link Alarm Event & Event function
        //Trigger this event When AlarmData add/del successful.(LGC must override)
        protected virtual void OnAlarmActionEvent(AlarmStatus m_Action, List<CommonData.HIRATA.AlarmItem> m_Alarms)
        {
        }

        //Trigger this event When AlarmData change.(LGC must override)
        protected virtual void OnAlarmChange()
        {
        }
        #endregion

        #region Link Recipe Event & Event function
        //Trigger this event When RecipeData add/del/Modify successful.(LGC must override)
        protected virtual void OnRecipeActionEvent(DataEidtAction m_Action, List<RecipeItem> m_Recipes)
        {
        }
        //Trigger this event When RecipeData change.(LGC must override)
        #endregion

        #region Link time out data Event & Event function
        //Trigger this event When Timeout data change.(LGC must override)
        protected virtual void OnTimeOutDataChange()
        {
        }
        #endregion

        #region Link glass count Event & Event function
        protected virtual void OnGlassCountDataChange()
        {
        }
        #endregion
        
        #region Link system data , status , robot status Event & Event function
        protected virtual void OnSystemDataChange()
        {
        }
        protected virtual void OnRobot1StatusChange()
        {
        }
        protected virtual void OnRobot2StatusChange()
        {
        }
        protected virtual void OnSystemStatusChange()
        {
        }
        protected virtual void OnApiConnected()
        {
        }
        protected virtual void OnApiDisconnected()
        {
        }
        protected virtual void  OnOperationModeChangeRight()
        {
        }
        protected virtual void  OnOperationModeChangeLeft()
        {
        }
        protected virtual void  OnPlcConnected()
        {
        }
        protected virtual void  OnPlcDisconnected()
        {
        }
        protected virtual void  OnBclive()
        {
        }
        protected virtual void  OnBcDie()
        {
        }
        #endregion

        #endregion


        void logsSetting()
        {
            string enviPath = CommonData.HIRATA.CommonStaticData.g_RootLogsFolderPath + CommonData.HIRATA.CommonStaticData.g_FDModuleName;
            KFileLog cv_MmfClientLog;
            cv_MmfClientLog = new KFileLog();
            cv_MmfClientLog.LoadFromIni(Global.LogIniPathname, "MmfEventClient");
            cv_MmfClientLog.LogFileName = enviPath + "\\MmfClient.log";
            cv_MmfClientLog.SaveToIni(Global.LogIniPathname, "MmfEventClient");
            cv_MmfClientLog = null;

            cv_ControllerLog = new KMemoLog();
            cv_ControllerLog.LoadFromIni(Global.LogIniPathname, "CONTROLLER");
            cv_ControllerLog.LogFileName = enviPath + "\\Controller.log";
            cv_ControllerLog.SaveToIni(Global.LogIniPathname, "CONTROLLER");
            cv_ControllerLog.WriteLog("Create Controller Log");

            Global.DebugLog = new KFileLog();
            Global.DebugLog.LoadFromIni(Global.LogIniPathname, "DebugLog");
            Global.DebugLog.LogFileName = enviPath + "\\Debug.log";
            Global.DebugLog.SaveToIni(Global.LogIniPathname, "DebugLog");
            Global.DebugLog.WriteLog("Create DebugLog");
        }
        void InitialBase()
        {
            Global.LogIniPathname = CommonData.HIRATA.CommonStaticData.g_ModuleLogsIniFile;
            Global.SystemIniPathname = CommonData.HIRATA.CommonStaticData.g_ModuleSystemIniFile;
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
        public void WriteDebugLog(List<string> m_Logs, int m_Level)
        {
            if (Global.DebugLog != null)
            {
                Global.DebugLog.WriteLog(m_Logs, m_Level);
            }
        }
        public void WriteDebugLog(string m_Log, int m_Level)
        {
            if (Global.DebugLog != null)
            {
                Global.DebugLog.WriteLog(m_Log, m_Level);
            }
        }
        protected virtual void AssignProcessFunctions()
        {
        }
 

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
        protected virtual void ProcessSystemData(Object m_Object)
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

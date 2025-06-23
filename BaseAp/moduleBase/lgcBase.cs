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
            LinkCommonDataEvent(lgcBase.cv_Alarms, BaseForm.cv_AccountData, lgcBase.cv_Recipes,
                lgcBase.cv_TimeoutData, lgcBase.cv_GlassCountData, lgcBase.PSystemData);
        }

        ~lgcBase()
        {
        }
        public virtual void LinkCommonDataEvent(AlarmData cv_Alarms, AccountData cv_AccountData, RecipeData cv_Recipes, TimeOutData cv_TimeoutData, GlassCountData cv_GlassCountData, SystemData cv_SystemData)
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonData.HIRATA;
using BaseAp;

namespace LGC
{
    public partial class LGCController
    {

        #region Link log in/out Event & Event function;
        //Trigger this event When AccountData login/out successful.(UI must override)
        protected override void OnLogInOutEvent(LogInOut m_Action, CommonData.HIRATA.AccountItem m_CurAccount)
        {
        }

        //Trigger this event When AccountData change.(UI must override)
        protected override void OnAccountChangeEvent()
        {
        }
        #endregion

        #region Link Alarm Event & Event function
        //Trigger this event When AlarmData add/del successful.(LGC must override)
        protected override void OnAlarmActionEvent(AlarmStatus m_Action, List<CommonData.HIRATA.AlarmItem> m_Alarms)
        {
        }

        //Trigger this event When AlarmData change.(LGC must override)
        protected override void OnAlarmChange()
        {
        }
        #endregion

        #region Link Recipe Event & Event function
        //Trigger this event When RecipeData add/del/Modify successful.(LGC must override)
        protected override void OnRecipeActionEvent(DataEidtAction m_Action, List<RecipeItem> m_Recipes)
        {
        }
        //Trigger this event When RecipeData change.(LGC must override)
        #endregion

        #region Link time out data Event & Event function
        //Trigger this event When Timeout data change.(LGC must override)
        protected override void OnTimeOutDataChange()
        {
        }
        #endregion

        #region Link glass count Event & Event function
        protected override void OnGlassCountDataChange()
        {
        }
        #endregion

        #region Link system data , status , robot status Event & Event function
        protected override void OnSystemDataChange()
        {
        }
        protected override void OnRobot1StatusChange()
        {
        }
        protected override void OnRobot2StatusChange()
        {
        }
        protected override void OnSystemStatusChange()
        {
        }
        protected override void OnApiConnected()
        {
        }
        protected override void OnApiDisconnected()
        {
        }
        protected override void OnOperationModeChangeRight()
        {
        }
        protected override void OnOperationModeChangeLeft()
        {
        }
        protected override void OnPlcConnected()
        {
        }
        protected override void OnPlcDisconnected()
        {
        }
        protected override void OnBclive()
        {
        }
        protected override void OnBcDie()
        {
        }
        #endregion

        /*change system data request.
         * cv_SystemOnlineMode     ,ProcessOnlineReq
         * cv_apiInlineMode        ,ProcessRobotInlineChange
         * cv_Initializing         ,ProcessInitialize
         * cv_OperationModeLeft    ,ProcessOperatorModeChange
         * cv_OperationModeRight   ,ProcessOperatorModeChange
         * cv_OcrMode              ,ProcessOcrMode
         * cv_OntMode              ,ProcessOntMode
         * cv_DataCheckRule        ,ProcessGlassCheck
                IsCheckRecipe
                IsCheckId
                IsCheckSlot
                IsCheckSeq

        * cv_Robot1Speed          ,ProcessApiCommand
        * cv_Robot2Speed          ,ProcessApiCommand
        * cv_FFUSpeed             ,ProcessApiCommand


        * MDSetTimeOut          : ProcessSetTimeOut
        * accountdata           : Ui responsable for this function. So LGC and CIM modules don't care this.
        * alarm                 , ProcessBcAlarm
        * glass count           , need add reset history count function.
        * recipe data.          , ProcessRecipeAction
        */

        #region rewrite base demand functions.
        protected override void ProcessOnlineReq(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessOnlineReq(m_SourceModule , m_MessageId ,m_Object);
            string log = "[Recv Online Req]\n";
            CommonData.HIRATA.MDOnlineRequest obj = m_Object as CommonData.HIRATA.MDOnlineRequest;
            log += "obj content : Cur Mode :" + obj.PCurMode.ToString() + " Change Mode : " + obj.PChangeMode.ToString() + Environment.NewLine;
            if (m_SourceModule == FdModule.UI)
            {
                if (obj.PChangeMode == OnlineMode.Offline)
                {
                    BaseForm.PSystemData.PSystemOnlineMode = obj.PChangeMode;
                    log += "Change online mode successful";
                }
                else if (obj.PChangeMode == OnlineMode.Control)
                {
                    if (BaseForm.PSystemData.PPlcConnect && BaseForm.PSystemData.PBcAlive)
                    {
                        BaseForm.PSystemData.PSystemOnlineMode = obj.PChangeMode;
                        log += "Change online mode successful";
                    }
                    else
                    {
                        SendMsgToUi(true, false, 10000, "Can't change CIM mode , please check BC status / PLC connection ");
                        log += "Change online mode fail , check BC/PLC status";
                    }
                }
                else
                {
                    SendMsgToUi(true, false, 10000, "Can't change CIM mode : " + obj.PChangeMode.ToString());
                    log += "Change online mode fail , check UI' msg content : " + obj.PChangeMode.ToString();
                }
            }
            WriteLog(LogLevelType.General, log);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessInitialize(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessInitialize(m_SourceModule , m_MessageId, m_Object);
            CommonData.HIRATA.MDInitial obj = m_Object as CommonData.HIRATA.MDInitial;
            if (BaseForm.PSystemData.PapiConnect && BaseForm.PSystemData.PapiInlineMode != EquipmentInlineMode.None)
            {
                if(obj.PSide == enSideGroup.Left)
                {
                    LgcModule.GetRobotBySide(obj.PSide).SetInitilize(obj.PSide, obj.cv_IsForce);
                    LgcModule.GetAlignerBySide(obj.PSide).cv_Data.PPreAction = AlignerPreAction.None;
                }
                else if(obj.PSide == enSideGroup.Right)
                {
                    LgcModule.GetRobotBySide(obj.PSide).SetInitilize(obj.PSide, obj.cv_IsForce);
                    LgcModule.GetAlignerBySide(obj.PSide).cv_Data.PPreAction = AlignerPreAction.None;
                }
                else if(obj.PSide == enSideGroup.Both)
                {
                    LgcModule.GetRobotBySide(obj.PSide).SetInitilize(obj.PSide, obj.cv_IsForce);
                    LgcModule.GetAlignerBySide(obj.PSide).cv_Data.PPreAction = AlignerPreAction.None;
                    LgcModule.GetRobotBySide(obj.PSide).SetInitilize(obj.PSide, obj.cv_IsForce);
                    LgcModule.GetAlignerBySide(obj.PSide).cv_Data.PPreAction = AlignerPreAction.None;
                }
                //LgcModule.GetRobotById(1).SetInitilize(enSideGroup.Left, obj.cv_IsForce);
                //LgcModule.GetAlignerById(1).cv_Data.PPreAction = AlignerPreAction.None;
                for (int i = (int)EqGifTimeChartId.TIMECHART_ID_SDP1; i <= (int)EqGifTimeChartId.TIMECHART_ID_VAS2_DOWN; i++)
                {
                    Eq eq = LgcModule.GetEqById(i);
                    if( (obj.PSide == eq.PSideGroup) || obj.PSide == enSideGroup.Both )
                    {
                        cv_TimeChart.RestartTimeChart(i);
                    }
                }
            }
            else
            {
                LgcModule.ShowMsg("Initialize failure , please check Robot connect / Robot current mode.", true, false);
            }
            SendTimeChartStepMsg();
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessAlarmChange(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            //base.ProcessAlarmChange(m_SourceModule, m_Type, m_MessageId, m_RequestNotifyMessageId, m_Ticket, m_Object);
            AlarmData obj = m_Object as AlarmData;
            for (int i = 0; i < obj.cv_AlarmList.Count; i++)
            {
                if (obj.cv_AlarmList[i].PStatus == AlarmStatus.Clean)
                {
                    BaseForm.cv_Alarms.DelAlarm(obj.cv_AlarmList[i]);
                }
            }
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessOntMode(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CommonData.HIRATA.MDOntMode obj = m_Object as CommonData.HIRATA.MDOntMode;
            bool is_on = obj.PIsOn;
            BaseForm.PSystemData.PONT = is_on;
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessSetTimeOut(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessSetTimeOut(m_SourceModule , m_MessageId , m_Object);
            CommonData.HIRATA.MDSetTimeOut obj = m_Object as CommonData.HIRATA.MDSetTimeOut;

            BaseForm.cv_TimeoutData.cv_IdleDelayTime = obj.PIdleTIme;
            BaseForm.cv_TimeoutData.cv_IntervalTime = obj.PIntervalTIme;
            BaseForm.cv_TimeoutData.cv_T0Time = obj.PT0TIme;
            BaseForm.cv_TimeoutData.cv_T1Time = obj.PT1TIme;
            BaseForm.cv_TimeoutData.cv_T3Time = obj.PT3TIme;
            BaseForm.cv_TimeoutData.cv_TsTime = obj.PTsTIme;
            BaseForm.cv_TimeoutData.cv_TeTime = obj.PTeTIme;
            BaseForm.cv_TimeoutData.cv_ApiT3TIme = obj.PApiT3TIme;
            BaseForm.cv_TimeoutData.cv_TmTime = obj.PTmTIme;
            BaseForm.cv_TimeoutData.cv_TmTime = obj.PTmTIme;
            BaseForm.cv_TimeoutData.SaveToFile();
            SetTimeChartTimeOut();

            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override  void ProcessRecipeAction(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessRecipeAction(m_SourceModule , m_MessageId ,  m_Object);
            string log = "";
            CommonData.HIRATA.MDRecipeAction obj = m_Object as CommonData.HIRATA.MDRecipeAction;
            if (obj.PAction == CommonData.HIRATA.DataEidtAction.Add)
            {
                foreach (CommonData.HIRATA.RecipeItem recipe in obj.Recipes)
                {
                    if (BaseForm.cv_Recipes.AddRecipe(recipe))
                    {
                        /*
                        CommonData.HIRATA.MDBCRecipeBodyReport report = new MDBCRecipeBodyReport();
                        report.PAction = RecipeBodyReportType.New;
                        report.PUnit = 0;
                        report.PRecipe = recipe;
                        SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCRecipeBodyReport).Name, report,
                            KParseObjToXmlPropertyType.Field);
                        */
                    }
                }
            }
            else if (obj.PAction == CommonData.HIRATA.DataEidtAction.Del)
            {
                foreach (CommonData.HIRATA.RecipeItem recipe in obj.Recipes)
                {
                    if (BaseForm.cv_Recipes.DelRecipe(recipe))
                    {
                        /*
                        CommonData.HIRATA.MDBCRecipeBodyReport report = new MDBCRecipeBodyReport();
                        report.PAction = RecipeBodyReportType.Delete;
                        report.PUnit = 0;
                        report.PRecipe = recipe;
                        SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCRecipeBodyReport).Name, report,
                            KParseObjToXmlPropertyType.Field);
                        */
                    }
                }
            }
            else if (obj.PAction == CommonData.HIRATA.DataEidtAction.Edit)
            {
                foreach (CommonData.HIRATA.RecipeItem recipe in obj.Recipes)
                {
                    if (BaseForm.cv_Recipes.ModifyRecipe(recipe))
                    {
                        /*
                        CommonData.HIRATA.MDBCRecipeBodyReport report = new MDBCRecipeBodyReport();
                        report.PAction = RecipeBodyReportType.Modity;
                        report.PUnit = 0;
                        report.PRecipe = recipe;
                        SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCRecipeBodyReport).Name, report,
                            KParseObjToXmlPropertyType.Field);
                        */
                    }
                }
            }
            else if (obj.PAction == CommonData.HIRATA.DataEidtAction.SetCur)
            {
                foreach (CommonData.HIRATA.RecipeItem recipe in obj.Recipes)
                {
                    if (BaseForm.cv_Recipes.IsRecipeExist(recipe.PId))
                    {
                        BaseForm.cv_Recipes.SetCurRecipe(recipe.PId);
                    }
                    /*
                    if (LgcForm.cv_Recipes.ModifyRecipe(recipe))
                    {
                        CommonData.HIRATA.MDBCRecipeBodyReport report = new MDBCRecipeBodyReport();
                        report.PAction = RecipeBodyReportType.Modity;
                        report.PUnit = 0;
                        report.PRecipe = recipe;
                        SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCRecipeBodyReport).Name, report,
                            KParseObjToXmlPropertyType.Field);
                    }
                    */
                }
            }
            WriteLog(LogLevelType.General, log);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        #endregion


    }
}

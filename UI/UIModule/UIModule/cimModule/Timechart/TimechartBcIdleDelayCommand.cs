using System;
using System.Collections.Generic;
using System.Text;
using KgsCommon;
using BaseAp;

namespace CIM
{
    class TimechartBcIdleDelayCommand : TimechartControllerBase.TimechartInstanceBase
    {
        public static int TIMECHART_ID_BcDateTimeCalibration = 1;
        public static int TIMECHART_ID_BcDisplayMessage = 2;
        public static int TIMECHART_ID_BcFoupDataDownload = 3;
        public static int TIMECHART_ID_BcIdleDelayCommand = 4;
        public static int TIMECHART_ID_BcIndexIntervalCommand = 5;
        public static int TIMECHART_ID_BcPortCommand = 6;
        public static int TIMECHART_ID_BcRecipeBodyQuery = 7;
        public static int TIMECHART_ID_BcRecipeExistCommand = 8;
        public static int TIMECHART_ID_EqAlarmReport = 9;
        public static int TIMECHART_ID_EqFetchReport = 10;
        public static int TIMECHART_ID_EqLastWorkProcessStartReport = 11;
        public static int TIMECHART_ID_EqRecipeListReport = 12;
        public static int TIMECHART_ID_EqReceiveReport = 13;
        public static int TIMECHART_ID_EqRecipeBodyReport = 14;
        public static int TIMECHART_ID_EqRecipeExistReport = 15;
        public static int TIMECHART_ID_EqSendReport = 16;
        public static int TIMECHART_ID_EqStoreReport = 17;
        public static int TIMECHART_ID_EqVCRReadReport = 18;
        public static int TIMECHART_ID_EqWorkDataRemoveReport = 19;
        public static int TIMECHART_ID_EqWorkDataRequest = 20;
        public static int TIMECHART_ID_EqWorkDataUpdateReport = 21;


        public static int STEP_ID_BcIdleDelayCommand = 1;

        int cv_Value;
        int cv_Port = 0x03D;

        public TimechartBcIdleDelayCommand(TimechartControllerBase m_TimechartController, int m_TimechartId, Dictionary<string, int> m_VarPortMap)
            : base(m_TimechartController, m_TimechartId, m_VarPortMap)
        {
            cv_Value = cv_MemoryIoClient.GetPortValue(cv_Port);

            AssignRunningStepEventFunction(STEP_ID_BcIdleDelayCommand, OnRunning_BcIdleDelayCommand);

            //AssignEnterStepEventFunction(STEP_ID_BcIdleDelayCommand, OnEnter_BcIdleDelayCommand);
        }

        public override void OnTimeout(int m_TimeoutId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        void OnRunning_BcIdleDelayCommand(int m_StepId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            string log = "[Process][OnRunning_BcIdleDelayCommand] : \n";
            try
            {
                if (cv_Value != cv_MemoryIoClient.GetPortValue(cv_Port))
                {
                    cv_Value = cv_MemoryIoClient.GetPortValue(cv_Port);
                    int node = (cv_MemoryIoClient.GetPortValue(0x003C) & 0x1F00) >> 8;
                    log += "index change to " + cv_Value.ToString() + " node : " + node.ToString();
                    if (node == 2)
                    {
                        log += "Enter BcIdleDelayCommand";
                        BcIdleDelayCommand();
                    }
                    else
                    {
                        log += "[Warning] index change but node number Error!!!";
                    }
                    CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail, log);
                }
            }
            catch (Exception ex)
            {
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        void OnEnter_BcIdleDelayCommand(int m_StepId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        void BcIdleDelayCommand()
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                int time;
                time = (cv_MemoryIoClient.GetPortValue(0x03C) & 0x00FF);
                string log = "[Process][BcIdleDelayCommand] : \n";
                log += "Download Idle delay time : " + time.ToString();
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail, log);
                CommonData.HIRATA.MDBCIdleDelayTime obj = new CommonData.HIRATA.MDBCIdleDelayTime();
                obj.PIdleDelayTime = time;
                CIMController.triggerCimEvent(typeof(CommonData.HIRATA.MDBCIdleDelayTime).Name, obj);
            }
            catch (Exception ex)
            {
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
    }
}

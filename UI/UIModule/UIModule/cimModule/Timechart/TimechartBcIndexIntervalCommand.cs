using System;
using System.Collections.Generic;
using System.Text;
using KgsCommon;
using BaseAp;

namespace CIM
{
    class TimechartBcIndexIntervalCommand : TimechartControllerBase.TimechartInstanceBase
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


        public static int STEP_ID_BcIndexIntervalCommand = 1;



        int cv_Value;
        int cv_Port = 0x006A;

        public TimechartBcIndexIntervalCommand(TimechartControllerBase m_TimechartController, int m_TimechartId, Dictionary<string, int> m_VarPortMap)
            : base(m_TimechartController, m_TimechartId, m_VarPortMap)
        {
            cv_Value = cv_MemoryIoClient.GetPortValue(cv_Port);
        
            AssignRunningStepEventFunction(STEP_ID_BcIndexIntervalCommand, OnRunning_BcIndexIntervalCommand);

            //AssignEnterStepEventFunction(STEP_ID_BcIndexIntervalCommand, OnEnter_BcIndexIntervalCommand);
        }

        void OnRunning_BcIndexIntervalCommand(int m_StepId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                string log = "[Process][OnRunning_BcIndexIntervalCommand] : \n";
                if (cv_Value != cv_MemoryIoClient.GetPortValue(cv_Port))
                {
                    cv_Value = cv_MemoryIoClient.GetPortValue(cv_Port);
                    int node = (cv_MemoryIoClient.GetPortValue(0x0069) & 0x1F00) >> 8 ;
                    log += "index change to " + cv_Value.ToString() + " node : " + node.ToString();
                    if (node == 2)
                    {
                        log += "Enter BcIndexIntervalCommand";
                        BcIndexIntervalCommand();
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

        void OnEnter_BcIndexIntervalCommand(int m_StepId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        void BcIndexIntervalCommand()
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                int interval;
                interval = (cv_MemoryIoClient.GetPortValue(0x0069) & 0x000F);

                string log = "[Process][BcIndexIntervalCommand] : \n";
                log += "Download Interval time : " + interval.ToString();
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail, log);

                CommonData.HIRATA.MDBCIndexInterval obj = new CommonData.HIRATA.MDBCIndexInterval();
                obj.PInterval = interval;
                CIMController.triggerCimEvent(typeof(CommonData.HIRATA.MDBCIndexInterval).Name, obj);
            }
            catch (Exception ex)
            {
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
    }
}

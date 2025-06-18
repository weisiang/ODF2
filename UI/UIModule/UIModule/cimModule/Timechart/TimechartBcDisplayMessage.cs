using System;
using System.Collections.Generic;
using System.Text;
using KgsCommon;
using BaseAp;

namespace CIM
{
    class TimechartBcDisplayMessage : TimechartControllerBase.TimechartInstanceBase
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


        public static int STEP_ID_BcDisplayMessage = 1;

        int cv_Value;
        int cv_Port = 0x002F;

        public TimechartBcDisplayMessage(TimechartControllerBase m_TimechartController, int m_TimechartId, Dictionary<string, int> m_VarPortMap)
            : base(m_TimechartController, m_TimechartId, m_VarPortMap)
        {
            cv_Value = cv_MemoryIoClient.GetPortValue(cv_Port);
           // AssignRunningStepEventFunction(STEP_ID_BcDisplayMessage, OnRunning_BcDisplayMessage);
            AssignEnterStepEventFunction(STEP_ID_BcDisplayMessage, OnEnter_BcDisplayMessage);
        }

        void OnRunning_BcDisplayMessage(int m_StepId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                if (cv_Value != cv_MemoryIoClient.GetPortValue(cv_Port))
                {
                    int node = (cv_MemoryIoClient.GetPortValue(0x05) & 0x1F00) >> 8;
                    cv_Value = cv_MemoryIoClient.GetPortValue(cv_Port);
                    CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail, "[Process][OnRunning_BcDateTimeCalibration] index change to " + cv_Value.ToString() + 
                        " nodex : " + node.ToString());
                    if (node == 2 || node == 0)
                    {
                        BcDisplayMessage();
                    }
                }
            }
            catch (Exception ex)
            {
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        void OnEnter_BcDisplayMessage(int m_StepId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        void BcDisplayMessage()
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                string log = "[Process][BcDisplayMessage] : \n";
                int node, flag, type, interval;
                int word_5_value = cv_MemoryIoClient.GetPortValue(0x05);
                node = (word_5_value & 0x1F00) >> 8;
                flag = word_5_value & 0x0001;
                type = (word_5_value & 0x0010) >> 4;
                interval = cv_MemoryIoClient.GetPortValue(0x06) & 0x00FF;
                string msg = cv_MemoryIoClient.GetBinaryLengthData(0x07, 40);

                CommonData.HIRATA.MDBCMsg obj = new CommonData.HIRATA.MDBCMsg();
                obj.PBuzzer = flag == 1 ? true : false;
                obj.PMsgType = (CommonData.HIRATA.BcMsgType)type;
                obj.PIntervalSec = interval;
                obj.PMsg = msg;
                log += "Buzzer : " + obj.PBuzzer.ToString() + " MsgType : " + obj.PMsgType + " IntervalSec : " + obj.PIntervalSec + " Msg : " + obj.PMsg;
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail, log);
                CIMController.triggerCimEvent(typeof(CommonData.HIRATA.MDBCMsg).Name, obj);
            }
            catch (Exception ex)
            {
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
    }
}

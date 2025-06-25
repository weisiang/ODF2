using System;
using System.Collections.Generic;
using System.Text;
using KgsCommon;
using BaseAp;

namespace CIM
{
    class TimechartBcRecipeExistCommand : TimechartControllerBase.TimechartInstanceBase
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


        public static int STEP_ID_BcRecipeExistCommand = 1;

        int cv_Value;
        int cv_Port = 0x00BC;

        public TimechartBcRecipeExistCommand(TimechartControllerBase m_TimechartController, int m_TimechartId, Dictionary<string, int> m_VarPortMap)
            : base(m_TimechartController, m_TimechartId, m_VarPortMap)
        {
            cv_Value = cv_MemoryIoClient.GetPortValue(cv_Port);
            AssignRunningStepEventFunction(STEP_ID_BcRecipeExistCommand, OnRunning_BcRecipeExistCommand);
            //AssignEnterStepEventFunction(STEP_ID_BcRecipeExistCommand, OnEnter_BcRecipeExistCommand);
        }

        void OnRunning_BcRecipeExistCommand(int m_StepId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                string log = "[Process OnRunning_BcRecipeExistCommand]";

                if (cv_Value != cv_MemoryIoClient.GetPortValue(cv_Port))
                {
                    cv_Value = cv_MemoryIoClient.GetPortValue(cv_Port);
                    int node = (cv_MemoryIoClient.GetPortValue(0x00A2) & 0x1F00) >> 8;
                    log += "index change to " + cv_Value.ToString() + " node : " + node.ToString();

                    if (node == 2)
                    {
                        log += "Enter BcRecipeExistCommand";
                        BcRecipeExistCommand();
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

        void OnEnter_BcRecipeExistCommand(int m_StepId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        void BcRecipeExistCommand()
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                string log = "[Process][BcRecipeExistCommand]";
                int port;
                port = (cv_MemoryIoClient.GetPortValue(0x00A2) & 0x00F0) >> 4;
                CommonData.HIRATA.MDBCRecipeExist obj = new CommonData.HIRATA.MDBCRecipeExist();
                obj.PPortId = port;

                for (int i = 0; i < 25; i++)
                {
                    obj.cv_Recipes.Add(cv_MemoryIoClient.GetPortValue(0x00A3 + i));
                }
                log += "Port : " + port.ToString() + " Form slot 1 : " + string.Join(",", obj.cv_Recipes);

                CIMController.triggerCimEvent(typeof(CommonData.HIRATA.MDBCRecipeExist).Name, obj);
            }
            catch (Exception ex)
            {
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
    }
}

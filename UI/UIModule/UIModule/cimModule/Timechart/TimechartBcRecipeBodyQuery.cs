using System;
using System.Collections.Generic;
using System.Text;
using KgsCommon;
using BaseAp;

namespace CIM
{
    class TimechartBcRecipeBodyQuery : TimechartControllerBase.TimechartInstanceBase
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



        public static int STEP_ID_BcRecipeBodyQuery = 1;


        int cv_Value;
        int cv_Port = 0x050;

        public TimechartBcRecipeBodyQuery(TimechartControllerBase m_TimechartController, int m_TimechartId, Dictionary<string, int> m_VarPortMap)
            : base(m_TimechartController, m_TimechartId, m_VarPortMap)
        {
            cv_Value = cv_MemoryIoClient.GetPortValue(cv_Port);

            AssignRunningStepEventFunction(STEP_ID_BcRecipeBodyQuery, OnRunning_BcRecipeBodyQuery);

            //AssignEnterStepEventFunction(STEP_ID_BcRecipeBodyQuery, OnEnter_BcRecipeBodyQuery);
        }

        void OnRunning_BcRecipeBodyQuery(int m_StepId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                string log = "[Process OnRunning_BcRecipeBodyQuery]";
                if (cv_Value != cv_MemoryIoClient.GetPortValue(cv_Port))
                {
                    cv_Value = cv_MemoryIoClient.GetPortValue(cv_Port);
                    int node = (cv_MemoryIoClient.GetPortValue(0x004E) & 0x1F00) >> 8;
                    log += "index change to " + cv_Value.ToString() + " node : " + node.ToString();

                    if (node == 2)
                    {
                        log += "Enter BcRecipeBodyQuery";
                        BcRecipeBodyQuery();
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

        void OnEnter_BcRecipeBodyQuery(int m_StepId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        void BcRecipeBodyQuery()
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                string log = "[Process][BcRecipeBodyQuery]";
                int no;
                no = cv_MemoryIoClient.GetPortValue(0x04F);
                log += "Recipe query : " + no.ToString();
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail, log);
                CommonData.HIRATA.MDBCRecipeBodyQuery obj = new CommonData.HIRATA.MDBCRecipeBodyQuery();
                obj.PRecipeId = no;
                CIMController.triggerCimEvent(typeof(CommonData.HIRATA.MDBCRecipeBodyQuery).Name, obj);
            }
            catch (Exception ex)
            {
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
    }
}

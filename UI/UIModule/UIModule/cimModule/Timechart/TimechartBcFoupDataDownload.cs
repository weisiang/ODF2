using System;
using System.Collections.Generic;
using System.Text;
using KgsCommon;
using BaseAp;

namespace CIM
{
    class TimechartBcFoupDataDownload : TimechartControllerBase.TimechartInstanceBase
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


        public static int STEP_ID_BcFoupDataDownload = 1;

        int cv_Value1;
        int cv_Value2;
        int cv_Value3;
        int cv_Value4;
        int cv_Value5;
        int cv_Value6;
        int cv_Port1 = 0x0B30;
        int cv_Port2 = 0x1181;
        int cv_Port3 = 0x17D2;
        int cv_Port4 = 0x1E23;
        int cv_Port5 = 0x2474;
        int cv_Port6 = 0x2AC5;
        int port1 = 0x04E0 ; //port 2 : 0B31 , p3 : 1182, P4 : 17D3 , P5 1E24 , P6 : 2475
        int port2 = 0x0B31;
        int port3 = 0x1182;
        int port4 = 0x17D3;
        int port5 = 0x1E24;
        int port6 = 0x2475;

        public TimechartBcFoupDataDownload(TimechartControllerBase m_TimechartController, int m_TimechartId, Dictionary<string, int> m_VarPortMap)
            : base(m_TimechartController, m_TimechartId, m_VarPortMap)
        {
            cv_Value1 = cv_MemoryIoClient.GetPortValue(cv_Port1);
            cv_Value2 = cv_MemoryIoClient.GetPortValue(cv_Port2);
            cv_Value3 = cv_MemoryIoClient.GetPortValue(cv_Port3);
            cv_Value4 = cv_MemoryIoClient.GetPortValue(cv_Port4);
            cv_Value5 = cv_MemoryIoClient.GetPortValue(cv_Port5);
            cv_Value6 = cv_MemoryIoClient.GetPortValue(cv_Port6);
        
            AssignRunningStepEventFunction(STEP_ID_BcFoupDataDownload, OnRunning_BcFoupDataDownload);
            //AssignEnterStepEventFunction(STEP_ID_BcFoupDataDownload, OnEnter_BcFoupDataDownload);
        }

        void OnRunning_BcFoupDataDownload(int m_StepId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                string log = "[Process][OnRunning_BcFoupDataDownload] : \n";
                if (cv_Value1 != cv_MemoryIoClient.GetPortValue(cv_Port1))
                {
                    cv_Value1 = cv_MemoryIoClient.GetPortValue(cv_Port1);
                    int node = (cv_MemoryIoClient.GetPortValue(port1) & 0xF000) >> 12;
                    int port = (cv_MemoryIoClient.GetPortValue(port1) & 0x0F00) >> 8;
                    log += "Port 1 ,index change to " + cv_Value1.ToString()  + " node : " + node.ToString() + " Port : " + port.ToString();
                    if (node == 2 && port == 1)
                    {
                        log += "Enter BcFoupDataDownload";
                        BcFoupDataDownload(1);
                    }
                    else
                    {
                        log += "[Warning] index change but node or port number Error!!!";
                    }
                    CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail , log);
                }
                else if (cv_Value2 != cv_MemoryIoClient.GetPortValue(cv_Port2))
                {
                    cv_Value2 = cv_MemoryIoClient.GetPortValue(cv_Port2);
                    int node = (cv_MemoryIoClient.GetPortValue(port2) & 0xF000) >> 12;
                    int port = (cv_MemoryIoClient.GetPortValue(port2) & 0x0F00) >> 8;
                    log += "Port 2 ,index change to " + cv_Value2.ToString() + " node : " + node.ToString() + " Port : " + port.ToString();
                    if (node == 2 && port == 2)
                    {
                        log += "Enter BcFoupDataDownload";
                        BcFoupDataDownload(2);
                    }
                    else
                    {
                        log += "[Warning] index change but node or port number Error!!!";
                    }
                    CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail, log);
                }
                else if (cv_Value3 != cv_MemoryIoClient.GetPortValue(cv_Port3))
                {
                    cv_Value3 = cv_MemoryIoClient.GetPortValue(cv_Port3);
                    int node = (cv_MemoryIoClient.GetPortValue(port3) & 0xF000) >> 12;
                    int port = (cv_MemoryIoClient.GetPortValue(port3) & 0x0F00) >> 8;
                    log += "Port 3 ,index change to " + cv_Value3.ToString() + " node : " + node.ToString() + " Port : " + port.ToString();
                    if (node == 2 && port == 3)
                    {
                        log += "Enter BcFoupDataDownload";
                        BcFoupDataDownload(3);
                    }
                    else
                    {
                        log += "[Warning] index change but node or port number Error!!!";
                    }
                    CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail, log);
                }
                else if (cv_Value4 != cv_MemoryIoClient.GetPortValue(cv_Port4))
                {
                    cv_Value4 = cv_MemoryIoClient.GetPortValue(cv_Port4);
                    int node = (cv_MemoryIoClient.GetPortValue(port4) & 0xF000) >> 12;
                    int port = (cv_MemoryIoClient.GetPortValue(port4) & 0x0F00) >> 8;
                    log += "Port 4 ,index change to " + cv_Value4.ToString() + " node : " + node.ToString() + " Port : " + port.ToString();
                    if (node == 2 && port == 4)
                    {
                        log += "Enter BcFoupDataDownload";
                        BcFoupDataDownload(4);
                    }
                    else
                    {
                        log += "[Warning] index change but node or port number Error!!!";
                    }
                    CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail, log);
                }
                else if (cv_Value5 != cv_MemoryIoClient.GetPortValue(cv_Port5))
                {
                    cv_Value5 = cv_MemoryIoClient.GetPortValue(cv_Port5);
                    int node = (cv_MemoryIoClient.GetPortValue(port5) & 0xF000) >> 12;
                    int port = (cv_MemoryIoClient.GetPortValue(port5) & 0x0F00) >> 8;
                    log += "Port 5 ,index change to " + cv_Value5.ToString() + " node : " + node.ToString() + " Port : " + port.ToString();
                    if (node == 2 && port == 5)
                    {
                        log += "Enter BcFoupDataDownload";
                        BcFoupDataDownload(5);
                    }
                    else
                    {
                        log += "[Warning] index change but node or port number Error!!!";
                    }
                    CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail, log);
                }
                else if (cv_Value6 != cv_MemoryIoClient.GetPortValue(cv_Port6))
                {
                    cv_Value6 = cv_MemoryIoClient.GetPortValue(cv_Port6);
                    int node = (cv_MemoryIoClient.GetPortValue(port6) & 0xF000) >> 12;
                    int port = (cv_MemoryIoClient.GetPortValue(port6) & 0x0F00) >> 8;
                    log += "Port 6 ,index change to " + cv_Value6.ToString() + " node : " + node.ToString() + " Port : " + port.ToString();
                    if (node == 2 && port == 6)
                    {
                        log += "Enter BcFoupDataDownload";
                        BcFoupDataDownload(6);
                    }
                    else
                    {
                        log += "[Warning] index change but node or port number Error!!!";
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

        void OnEnter_BcFoupDataDownload(int m_StepId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        void BcFoupDataDownload(int m_PortId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                string log = "[Process][BcFoupDataDownload] sne MMF event Port : " + m_PortId;
                int start_port = port1 + 1617 * (m_PortId - 1);
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail, log);
                CommonData.HIRATA.PortData port_data = new CommonData.HIRATA.PortData(cv_MemoryIoClient, start_port);
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail, port_data.GetPortDataStr());
                CIMController.triggerCimEvent(typeof(CommonData.HIRATA.PortData).Name, port_data);
            }
            catch (Exception ex)
            {
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
    }
}

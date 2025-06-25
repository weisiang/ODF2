using System;
using System.Collections.Generic;
using System.Text;
using KgsCommon;
using BaseAp;

namespace CIM
{
    class TimechartBcPortCommand : TimechartControllerBase.TimechartInstanceBase
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



        public static int STEP_ID_BcPortCommand = 1;

        int cv_Value1;
        int cv_Value2;
        int cv_Value3;
        int cv_Value4;
        int cv_Value5;
        int cv_Value6;
        int cv_Value7;
        int cv_Value8;
        int cv_Port1 = 0x31;
        int cv_Port2 = 0x33;
        int cv_Port3 = 0x35;
        int cv_Port4 = 0x37;
        int cv_Port5 = 0x39;
        int cv_Port6 = 0x3B;
        int cv_Port7 = 0x3D;
        int cv_Port8 = 0x3F;
        int port1 = 0x30;
        int port2 = 0x32;
        int port3 = 0x34;
        int port4 = 0x36;
        int port5 = 0x38;
        int port6 = 0x3A;
        int port7 = 0x3C;
        int port8 = 0x3E;


        public TimechartBcPortCommand(TimechartControllerBase m_TimechartController, int m_TimechartId, Dictionary<string, int> m_VarPortMap)
            : base(m_TimechartController, m_TimechartId, m_VarPortMap)
        {
            cv_Value1 = cv_MemoryIoClient.GetPortValue(cv_Port1);
            cv_Value2 = cv_MemoryIoClient.GetPortValue(cv_Port2);
            cv_Value3 = cv_MemoryIoClient.GetPortValue(cv_Port3);
            cv_Value4 = cv_MemoryIoClient.GetPortValue(cv_Port4);
            cv_Value5 = cv_MemoryIoClient.GetPortValue(cv_Port5);
            cv_Value6 = cv_MemoryIoClient.GetPortValue(cv_Port6);
            cv_Value7 = cv_MemoryIoClient.GetPortValue(cv_Port7);
            cv_Value8 = cv_MemoryIoClient.GetPortValue(cv_Port8);
            AssignRunningStepEventFunction(STEP_ID_BcPortCommand, OnRunning_BcPortCommand);
            //AssignEnterStepEventFunction(STEP_ID_BcPortCommand, OnEnter_BcPortCommand);
        }

        void OnRunning_BcPortCommand(int m_StepId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                string log = "[Process][OnRunning_BcPortCommand] : \n";
                if (cv_Value1 != cv_MemoryIoClient.GetPortValue(cv_Port1))
                {
                    cv_Value1 = cv_MemoryIoClient.GetPortValue(cv_Port1);
                    int node = (cv_MemoryIoClient.GetPortValue(port1) & 0x0F00) >> 8;
                    int port = (cv_MemoryIoClient.GetPortValue(port1) & 0x00F0) >> 4;
                    log += "Port 1 ,index change to " + cv_Value1.ToString() + " node : " + node.ToString() + " Port : " + port.ToString();
                    if (node == 2 && port == 1)
                    {
                        log += " Enter BcPortCommand";
                        BcPortCommand(1);
                    }
                    else
                    {
                        log += "[Warning] index change but node or port number Error!!!";
                    }
                    CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail, log);
                }
                else if (cv_Value2 != cv_MemoryIoClient.GetPortValue(cv_Port2))
                {
                    cv_Value2 = cv_MemoryIoClient.GetPortValue(cv_Port2);
                    int node = (cv_MemoryIoClient.GetPortValue(port2) & 0x0F00) >> 8;
                    int port = (cv_MemoryIoClient.GetPortValue(port2) & 0x00F0) >> 4;
                    log += "Port 2 ,index change to " + cv_Value2.ToString() + " node : " + node.ToString() + " Port : " + port.ToString();
                    if (node == 2 && port == 2)
                    {
                        log += " Enter BcPortCommand";
                        BcPortCommand(2);
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
                    int node = (cv_MemoryIoClient.GetPortValue(port3) & 0x0F00) >> 8;
                    int port = (cv_MemoryIoClient.GetPortValue(port3) & 0x00F0) >> 4;
                    log += "Port 3 ,index change to " + cv_Value3.ToString() + " node : " + node.ToString() + " Port : " + port.ToString();
                    if (node == 2 && port == 3)
                    {
                        log += " Enter BcPortCommand";
                        BcPortCommand(3);
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
                    int node = (cv_MemoryIoClient.GetPortValue(port4) & 0x0F00) >> 8;
                    int port = (cv_MemoryIoClient.GetPortValue(port4) & 0x00F0) >> 4;
                    log += "Port 3 ,index change to " + cv_Value4.ToString() + " node : " + node.ToString() + " Port : " + port.ToString();
                    if (node == 2 && port == 4)
                    {
                        log += " Enter BcPortCommand";
                        BcPortCommand(4);
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
                    int node = (cv_MemoryIoClient.GetPortValue(port5) & 0x0F00) >> 8;
                    int port = (cv_MemoryIoClient.GetPortValue(port5) & 0x00F0) >> 4;
                    log += "Port 5 ,index change to " + cv_Value5.ToString() + " node : " + node.ToString() + " Port : " + port.ToString();
                    if (node == 2 && port == 5)
                    {
                        log += " Enter BcPortCommand";
                        BcPortCommand(5);
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
                    int node = (cv_MemoryIoClient.GetPortValue(port6) & 0x0F00) >> 8;
                    int port = (cv_MemoryIoClient.GetPortValue(port6) & 0x00F0) >> 4;
                    log += "Port 6 ,index change to " + cv_Value6.ToString() + " node : " + node.ToString() + " Port : " + port.ToString();
                    if (node == 2 && port == 6)
                    {
                        log += " Enter BcPortCommand";
                        BcPortCommand(6);
                    }
                    else
                    {
                        log += "[Warning] index change but node or port number Error!!!";
                    }
                    CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail, log);
                }
                else if (cv_Value6 != cv_MemoryIoClient.GetPortValue(cv_Port7))
                {
                    cv_Value6 = cv_MemoryIoClient.GetPortValue(cv_Port7);
                    int node = (cv_MemoryIoClient.GetPortValue(port7) & 0x0F00) >> 8;
                    int port = (cv_MemoryIoClient.GetPortValue(port7) & 0x00F0) >> 4;
                    log += "Port 7 ,index change to " + cv_Value7.ToString() + " node : " + node.ToString() + " Port : " + port.ToString();
                    if (node == 2 && port == 7)
                    {
                        log += " Enter BcPortCommand";
                        BcPortCommand(7);
                    }
                    else
                    {
                        log += "[Warning] index change but node or port number Error!!!";
                    }
                    CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail, log);
                }
                else if (cv_Value6 != cv_MemoryIoClient.GetPortValue(cv_Port8))
                {
                    cv_Value6 = cv_MemoryIoClient.GetPortValue(cv_Port8);
                    int node = (cv_MemoryIoClient.GetPortValue(port8) & 0x0F00) >> 8;
                    int port = (cv_MemoryIoClient.GetPortValue(port8) & 0x00F0) >> 4;
                    log += "Port 8 ,index change to " + cv_Value8.ToString() + " node : " + node.ToString() + " Port : " + port.ToString();
                    if (node == 2 && port == 8)
                    {
                        log += " Enter BcPortCommand";
                        BcPortCommand(8);
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

        void OnEnter_BcPortCommand(int m_StepId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        void BcPortCommand(int m_Port)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                string log = "[Process][BcPortCommand]";
                int node, port, cmd;
                node = (cv_MemoryIoClient.GetPortValue(port1 + ((m_Port - 1) << 1)) & 0x1F00) >> 8;
                port = (cv_MemoryIoClient.GetPortValue(port1 + ((m_Port - 1) << 1)) & 0x00F0) >> 4;
                cmd = cv_MemoryIoClient.GetPortValue(port1 + ((m_Port - 1) << 1)) & 0x000F;

                CommonData.HIRATA.MDBCPortCommand obj = new CommonData.HIRATA.MDBCPortCommand();
                obj.PPortCommand = (CommonData.HIRATA.BCPortCommand)cmd;
                obj.PPortId = port;

                CIMController.triggerCimEvent(typeof(CommonData.HIRATA.MDBCPortCommand).Name, obj);

                log += "node : " + node.ToString() + " Port : " + port.ToString() + " cmd : " + cmd.ToString() + " " + obj.PPortCommand.ToString();
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail, log);
            }
            catch (Exception ex)
            {
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
    }
}

// Copyright (c) 2000-2018, Kingroup Systems Corporation. All rights reserved.
//
// History:
// Date         Reference       Person          Descriptions
// ---------- 	-------------- 	--------------  ---------------------------
// 2018/07/17    	            Cassius         Initial Implementation
//
//---------------------------------------------------------------------------
// 若需修改請回報
//---------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using KgsCommon;
using System.Runtime.InteropServices;
using CommonData.HIRATA;

namespace LGC
{
    public class TimechartControllerBase
    {
        public class TimechartInstanceBase
        {
            public int cv_WriteDataStartPort;
            public int cv_ReadDataStartPort;
            public int cv_RobotBitStart;
            public int cv_EqBitStart;

            public KDateTime cv_DataTime = SysUtils.Now();
            public EqInterFaceType cv_ActionType = EqInterFaceType.None;
            public static int cv_IndexDelay = 200;
            public static int cv_Ts = 4000;
            public static int cv_Te = 4000;
            public static int cv_T0 = 4000;
            public static int cv_T1 = 5000;
            public static int cv_T3 = 10000;
            public delegate void TimechartEventHandler(int m_StepId);
            protected Dictionary<int, TimechartEventHandler> cv_TimechartRunningStepEventSet = new Dictionary<int, TimechartEventHandler>();
            protected Dictionary<int, TimechartEventHandler> cv_TimechartEnterStepEventSet = new Dictionary<int, TimechartEventHandler>();
            protected Dictionary<int, TimechartEventHandler> cv_TimechartLeaveStepEventSet = new Dictionary<int, TimechartEventHandler>();
           
            protected TimechartControllerBase cv_TimechartController;
            protected KTimeCharts cv_Timechart;
            protected KMemoryIOClient cv_MemoryIoClient;

            protected Dictionary<string, int> cv_VarPortMap;
            public int TimechartId 
            {
                get
                {
                    return cv_TimechartId;
                }
            }
            public int cv_TimechartId = 0;

            public TimechartInstanceBase(TimechartControllerBase m_TimechartController, int m_TimechartId, Dictionary<string, int> m_VarPortMap)
            {
                cv_TimechartController = m_TimechartController;
                cv_Timechart = cv_TimechartController.GetTimeChart();
                cv_MemoryIoClient = cv_TimechartController.GetmemoryIoClient();

                cv_TimechartId = m_TimechartId;
                cv_VarPortMap = m_VarPortMap;
            }

            public void GetBinaryLengthData(string m_VarName, ref byte[] m_Data, int m_WordLength, bool m_WordSwap)
            {
                if (cv_VarPortMap.ContainsKey(m_VarName))
                {
                    int port = cv_VarPortMap[m_VarName];
                    cv_MemoryIoClient.GetBinaryLengthData(port, ref m_Data, m_WordLength, m_WordSwap);
                }
            }

            public bool GetBitIndex(string m_VarName, int m_Index)
            {
                bool flag = false;
                if (cv_VarPortMap.ContainsKey(m_VarName))
                {
                    int port = cv_VarPortMap[m_VarName];
                    flag = cv_MemoryIoClient.GetBitIndex(port, m_Index);
                }

                return flag;
            }

            public int GetPortValue(string m_VarName)
            {
                int value = 0;
                if (cv_VarPortMap.ContainsKey(m_VarName))
                {
                    int port = cv_VarPortMap[m_VarName];
                    value = cv_MemoryIoClient.GetPortValue(port);
                }

                return value;
            }

            public void SetBinaryLengthData(string m_VarName, byte[] m_Data, int m_WordLength, bool m_WordSwap)
            {
                if (cv_VarPortMap.ContainsKey(m_VarName))
                {
                    int port = cv_VarPortMap[m_VarName];
                    cv_MemoryIoClient.SetBinaryLengthData(port, m_Data, m_WordLength, m_WordSwap);
                }
            }

            public void SetBitIndex(string m_VarName, int m_Index, bool m_Value)
            {
                if (cv_VarPortMap.ContainsKey(m_VarName))
                {
                    int port = cv_VarPortMap[m_VarName];
                    cv_MemoryIoClient.SetBitIndex(port, m_Index, m_Value);
                }
            }

            public void SetPortValue(string m_VarName, int m_Value)
            {
                if (cv_VarPortMap.ContainsKey(m_VarName))
                {
                    int port = cv_VarPortMap[m_VarName];
                    cv_MemoryIoClient.SetPortValue(port, m_Value);
                }
            }


            public int GetCurrentStep(int m_TimeChartId)
            {
                return cv_Timechart.GetCurrentStep(m_TimeChartId);
            }

            public void JumpToStep(int m_TimeChartId, int m_StepId)
            {
                cv_Timechart.JumpToStep(m_TimeChartId, m_StepId);
            }

            public void RestartTimeChart(int m_TimeChartId)
            {
                cv_Timechart.RestartTimeChart(m_TimeChartId);
            }

            public void RestartTimeCharts()
            {
                cv_Timechart.RestartTimeCharts();
            }

            public void SetTrigger(int m_TimeChartId)
            {
                cv_Timechart.SetTrigger(m_TimeChartId);
            }

            public void SetTrigger(int m_TimeChartId, int m_StepId)
            {
                cv_Timechart.SetTrigger(m_TimeChartId, m_StepId);
            }

            public void StartTimeout(int m_TimeChartId, int m_Id, int m_Interval, bool m_Repeat)
            {
                cv_Timechart.StartTimeout(m_TimeChartId, m_Id, m_Interval, m_Repeat);
            }

            public void StopTimeout(int m_TimeChartId, int m_Id)
            {
                cv_Timechart.StopTimeout(m_TimeChartId, m_Id);
            }

            protected void AssignRunningStepEventFunction(int m_StepId, TimechartEventHandler m_Handler)
            {
                lock (cv_TimechartRunningStepEventSet)
                {
                    cv_TimechartRunningStepEventSet.Add(m_StepId, m_Handler);
                }
            }

            protected void AssignEnterStepEventFunction(int m_StepId, TimechartEventHandler m_Handler)
            {
                lock (cv_TimechartEnterStepEventSet)
                {
                    cv_TimechartEnterStepEventSet.Add(m_StepId, m_Handler);
                }
            }

            protected void AssignLeaveStepEventFunction(int m_StepId, TimechartEventHandler m_Handler)
            {
                lock (cv_TimechartLeaveStepEventSet)
                {
                    cv_TimechartLeaveStepEventSet.Add(m_StepId, m_Handler);
                }
            }

            public virtual void OnTimeout(int m_TimeoutId)
            {
            }

            internal void OnRunning(int m_StepId)
            {
                if (cv_TimechartRunningStepEventSet.Count <= 0)
                {
                    return;
                }

                TimechartEventHandler handler = null;
                lock (cv_TimechartRunningStepEventSet)
                {
                    if (cv_TimechartRunningStepEventSet.ContainsKey(m_StepId))
                    {
                        handler = cv_TimechartRunningStepEventSet[m_StepId];
                    }
                }

                if (handler != null)
                {
                    handler(m_StepId);
                }
            }

            internal void OnEnterStep(int m_StepId)
            {
                string log = "[Enter] Time chart Id : " + cv_TimechartId.ToString() + " Step : " + m_StepId.ToString();
                SysUtils.DebugWindowTrace(CommonData.HIRATA.CommonStaticData.g_FDModuleName, "TimeChart", 1, log);
                if (cv_TimechartEnterStepEventSet.Count <= 0)
                {
                    return;
                }

                TimechartEventHandler handler = null;
                lock (cv_TimechartEnterStepEventSet)
                {
                    if (cv_TimechartEnterStepEventSet.ContainsKey(m_StepId))
                    {
                        handler = cv_TimechartEnterStepEventSet[m_StepId];
                    }
                }

                if (handler != null)
                {
                    handler(m_StepId);
                }
            }

            internal void OnLeaveStep(int m_StepId)
            {
                string log = "[Leave] Time chart Id : " + cv_TimechartId.ToString() + " Step : " + m_StepId.ToString();
                SysUtils.DebugWindowTrace(CommonData.HIRATA.CommonStaticData.g_FDModuleName, "TimeChart", 1, log);
                if (cv_TimechartLeaveStepEventSet.Count <= 0)
                {
                    return;
                }

                TimechartEventHandler handler = null;
                lock (cv_TimechartLeaveStepEventSet)
                {
                    if (cv_TimechartLeaveStepEventSet.ContainsKey(m_StepId))
                    {
                        handler = cv_TimechartLeaveStepEventSet[m_StepId];
                    }
                }

                if (handler != null)
                {
                    handler(m_StepId);
                }
            }
        }


        private Dictionary<int, TimechartInstanceBase> cv_TimechartInstanceSet = new Dictionary<int, TimechartInstanceBase>();
        private KTimeCharts cv_Timechart = new KTimeCharts();
        private KMemoryIOClient cv_MemoryIoClient = new KMemoryIOClient();

        public TimechartInstanceBase GetTimeChartInstance(int m_Id)
        {
            TimechartInstanceBase rtn = null;
            if(cv_TimechartInstanceSet.ContainsKey(m_Id))
            {
                rtn = cv_TimechartInstanceSet[m_Id];
            }
            return rtn;
        }

        public TimechartControllerBase(string m_TimechartXmlPathname)
        {
            KIniFile ini = new KIniFile(GlobalBase.SystemIniPathname);
            string memory_io_server_id = ini.ReadString("Config", "MIOS", "KGSMEMORYIODEMO");
            ini.WriteString("Config", "MIOS", memory_io_server_id);

            cv_MemoryIoClient.ServerName = memory_io_server_id;
            cv_MemoryIoClient.Open();

            cv_Timechart.OnSetPortValue += OnSetPortValue;
            cv_Timechart.OnSetBitIndex += OnSetBitIndex;
            cv_Timechart.OnSetBinaryLengthData += OnSetBinaryLengthData;
            cv_Timechart.OnSetAsciiData += OnSetAsciiData;
            cv_Timechart.OnGetPortValue += OnGetPortValue;
            cv_Timechart.OnGetBitIndex += OnGetBitIndex;
            cv_Timechart.OnGetBinaryLengthData += OnGetBinaryLengthData;

            cv_Timechart.OnTimeout += OnTimeout;
            cv_Timechart.OnRunning += OnRunning;
            cv_Timechart.OnEnterStep += OnEnterStep;
            cv_Timechart.OnLeaveStep += OnLeaveStep;

            cv_Timechart.LoadXmlFromFile(m_TimechartXmlPathname);
        }

        virtual public void Open()
        {
            cv_Timechart.Open();
        }

        internal KTimeCharts GetTimeChart()
        {
            return cv_Timechart;
        }

        public KMemoryIOClient GetmemoryIoClient()
        {
            return cv_MemoryIoClient;
        }

        protected void AddTimechartInstance(int m_Timechartid, TimechartInstanceBase m_TimechartInstance)
        {
            lock (cv_TimechartInstanceSet)
            {
                cv_TimechartInstanceSet.Add(m_Timechartid, m_TimechartInstance);
            }
        }

        void OnSetBitIndex(int m_Port, int m_Index, bool m_Value)
        {
            cv_MemoryIoClient.SetBitIndex(m_Port, m_Index, m_Value);
        }

        void OnSetPortValue(int m_Port, int m_Value)
        {
            cv_MemoryIoClient.SetPortValue(m_Port, m_Value);
        }

        void OnSetBinaryLengthData(int m_Port, IntPtr m_Data, int m_WordLength, bool m_WordSwap)
        {
            int length = m_WordLength*2;
            byte[] buffer = new byte[length];
            try
            {
                Marshal.Copy(m_Data, buffer, 0, length);

                cv_MemoryIoClient.SetBinaryLengthData(m_Port, buffer, length, m_WordSwap);
            }
            catch
            {
            }
        }

        void OnSetAsciiData(int m_Port, string m_AsciiData)
        {
            byte[] buffer = SysUtils.StringToByteArray(m_AsciiData);
            cv_MemoryIoClient.SetBinaryLengthData(m_Port, buffer, buffer.Length);
        }

        int OnGetPortValue(int m_Port)
        {
            return cv_MemoryIoClient.GetPortValue(m_Port);
        }

        bool OnGetBitIndex(int m_Port, int m_Index)
        {
            return cv_MemoryIoClient.GetBitIndex(m_Port, m_Index);
        }

        void OnGetBinaryLengthData(int m_Port, IntPtr m_Data, int m_WordLength, bool m_WordSwap)
        {
            int length = m_WordLength*2;
            byte[] buffer = new byte[length];
            try
            {
                cv_MemoryIoClient.GetBinaryLengthData(m_Port, ref buffer, m_WordLength, m_WordSwap);

                Marshal.Copy(buffer, 0, m_Data, length);
            }
            catch
            {
            }
        }

        void OnTimeout(int m_TimeChartId, string m_TimeChartName, int m_TimeoutId)
        {
            TimechartInstanceBase instance = null;
            lock (cv_TimechartInstanceSet)
            {
                if (cv_TimechartInstanceSet.ContainsKey(m_TimeChartId))
                {
                    instance = cv_TimechartInstanceSet[m_TimeChartId];
                }
            }

            if (instance != null) 
            {
                instance.OnTimeout(m_TimeoutId);
            }
        }

        void OnRunning(int m_TimeChartId, string m_TimeChartName, int m_StepId, string m_StepName)
        {
            TimechartInstanceBase instance = null;
            lock (cv_TimechartInstanceSet)
            {
                if (cv_TimechartInstanceSet.ContainsKey(m_TimeChartId))
                {
                    instance = cv_TimechartInstanceSet[m_TimeChartId];
                }
            }

            if (instance != null)
            {
                instance.OnRunning(m_StepId);
            }
        }

        void OnEnterStep(int m_TimeChartId, string m_TimeChartName, int m_StepId, string m_StepName)
        {
            TimechartInstanceBase instance = null;
            lock (cv_TimechartInstanceSet)
            {
                if (cv_TimechartInstanceSet.ContainsKey(m_TimeChartId))
                {
                    instance = cv_TimechartInstanceSet[m_TimeChartId];
                }
            }

            if (instance != null)
            {
                instance.OnEnterStep(m_StepId);
            }
        }

        void OnLeaveStep(int m_TimeChartId, string m_TimeChartName, int m_StepId, string m_StepName)
        {
            TimechartInstanceBase instance = null;
            lock (cv_TimechartInstanceSet)
            {
                if (cv_TimechartInstanceSet.ContainsKey(m_TimeChartId))
                {
                    instance = cv_TimechartInstanceSet[m_TimeChartId];
                }
            }

            if (instance != null)
            {
                instance.OnLeaveStep(m_StepId);
            }
        }
    }
}

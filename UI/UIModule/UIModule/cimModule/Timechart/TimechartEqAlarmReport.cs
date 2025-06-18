using System;
using System.Collections.Generic;
using System.Text;
using KgsCommon;
using System.Linq;

namespace CIM
{
    class TimechartEqAlarmReport : TimechartControllerBase.TimechartInstanceBase
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



        public static int STEP_ID_TriggerAlarmIndex = 1;
        public static int STEP_ID_WaitTm = 2;
        public static int STEP_ID_WaitInterval = 3;

        public TimechartEqAlarmReport(TimechartControllerBase m_TimechartController, int m_TimechartId, Dictionary<string, int> m_VarPortMap)
            : base(m_TimechartController, m_TimechartId, m_VarPortMap)
        {
            AssignEnterStepEventFunction(STEP_ID_TriggerAlarmIndex, OnEnter_TriggerAlarmIndex);
            AssignEnterStepEventFunction(STEP_ID_WaitInterval, OnEnter_WaitInterval);
            AssignEnterStepEventFunction(STEP_ID_WaitTm, OnEnter_WaitTm);
        }
        protected override void DoProcess()
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                string log = "[Process][DoProcess]";
                if (1 == cv_Timechart.GetCurrentStep(cv_TimechartId) && cv_Jobs.Count != 0)
                {
                    log += "Alarm job count : " + cv_Jobs.Count;
                    CommonData.HIRATA.AlarmData tmp = cv_Jobs.Peek() as CommonData.HIRATA.AlarmData;
                    if (tmp.cv_AlarmList.Count <= 10)
                    {
                        log += "Enter ProcessJob";
                        ProcessJob(cv_Jobs.Dequeue());
                        JumpToStep(cv_TimechartId, 2);
                    }
                    else
                    {
                        log += "Enter ProcessJob";
                        ProcessJob(tmp);
                        JumpToStep(cv_TimechartId, 2);
                    }
                    CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail , log);
                }
            }
            catch (Exception ex)
            {
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        protected override bool ProcessJob(object m_obj)
        {
            bool rtn = true;
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                string log = "[Process][ProcessJob]\n";
                CommonData.HIRATA.AlarmData tmp = m_obj as CommonData.HIRATA.AlarmData;
                byte[] container = new byte[22];
                Array.Clear(container, 0, container.Length);
                if (tmp.cv_AlarmList.Count >= 10)
                {
                    List<CommonData.HIRATA.AlarmItem> list = tmp.cv_AlarmList.Take(10).ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        //cv_Driver.SetPortValue(6200 + (i), list[i].Word1);
                        int value = ((int)list[i].PStatus << 15) + (Convert.ToInt32(list[i].PCode));
                        log += "Report Alarm Code : " + (value & 0x7FFF) + " , Status : " + ( (value & 0x8000) >> 15) +  "\n";
                        container[(i << 1)] = Convert.ToByte(value & 0x00ff);
                        container[(i << 1) + 1] = Convert.ToByte((value & 0xff00) >> 8);
                    }
                    tmp.cv_AlarmList.RemoveRange(0, list.Count);
                    CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail, log);
                }
                else
                {
                    for (int i = 0; i < tmp.cv_AlarmList.Count; i++)
                    {
                        int value = ((int)tmp.cv_AlarmList[i].PStatus << 15) + (Convert.ToInt32(tmp.cv_AlarmList[i].PCode));
                        log += "Report Alarm Code : " + (value & 0x7FFF) + " , Status : " + ((value & 0x8000) >> 15) + "\n";
                        container[(i << 1)] = Convert.ToByte(value & 0x00ff);
                        container[(i << 1) + 1] = Convert.ToByte((value & 0xff00) >> 8);
                    }
                }
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail, log);
                cv_MemoryIoClient.SetBinaryLengthData(0x3454, container, 10, false);
                cv_Timechart.SetTimeLock(this.cv_TimechartId, STEP_ID_WaitTm, cv_Tm);
            }
            catch (Exception ex)
            {
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return rtn;
        }
        private int IntToHex(int m_int)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            int hex = 0;
            hex = SysUtils.HexToInt(m_int.ToString());
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return hex;
        }

        void OnEnter_WaitTm(int m_StepId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                uint index = (uint)cv_MemoryIoClient.GetPortValue(0x345E);
                if (index == 0xffff)
                {
                    index = 1;
                }
                else
                {
                    index += 1;
                }
                cv_MemoryIoClient.SetPortValue(0x345E, (int)index);
                cv_Timechart.SetTimeLock(this.cv_TimechartId, STEP_ID_WaitInterval, cv_IndexDelay);
            }
            catch (Exception ex)
            {
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void OnEnter_TriggerAlarmIndex(int m_StepId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        void OnEnter_WaitInterval(int m_StepId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
    }
}

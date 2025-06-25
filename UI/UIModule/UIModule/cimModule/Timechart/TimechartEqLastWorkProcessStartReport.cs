using System;
using System.Collections.Generic;
using System.Text;
using KgsCommon;

namespace CIM
{
    class TimechartEqLastWorkProcessStartReport : TimechartControllerBase.TimechartInstanceBase
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


        public static int STEP_ID_TriggerLastWorkProcessStartIndex = 1;
        public static int STEP_ID_WaitInterval = 3;
        public static int STEP_ID_WaitTm = 2;

        public static string LastWorkProcessStartIndex = "LastWorkProcessStartIndex";

        public TimechartEqLastWorkProcessStartReport(TimechartControllerBase m_TimechartController, int m_TimechartId, Dictionary<string, int> m_VarPortMap)
            : base(m_TimechartController, m_TimechartId, m_VarPortMap)
        {

            AssignEnterStepEventFunction(STEP_ID_TriggerLastWorkProcessStartIndex, OnEnter_TriggerLastWorkProcessStartIndex);
            AssignEnterStepEventFunction(STEP_ID_WaitInterval, OnEnter_WaitInterval);
            AssignEnterStepEventFunction(STEP_ID_WaitTm, OnEnter_WaitTm);
        }
        protected override bool ProcessJob(object m_obj)
        {
            bool rtn = true;
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                string log = "[Process][LastProcessStartReport ProcessJob]\n";
                CommonData.HIRATA.MDBCLastProcessStartReport obj = m_obj as CommonData.HIRATA.MDBCLastProcessStartReport;
                byte[] tmp = new byte[6];
                int value = (int)obj.PSlotNo + ((int)obj.PPortNo << 12);
                tmp[0] = Convert.ToByte(value & 0x00ff);
                tmp[1] = Convert.ToByte((value & 0xff00) >> 8);
                value = (((int)obj.PGlass.PCimMode) << 15) + (int)obj.PGlass.PFoupSeq;
                tmp[2] = Convert.ToByte(value & 0x00ff);
                tmp[3] = Convert.ToByte((value & 0xff00) >> 8);
                value = (((int)obj.PGlass.PWorkOrderNo) << 8) + (int)obj.PGlass.PWorkSlot;
                tmp[4] = Convert.ToByte(value & 0x00ff);
                tmp[5] = Convert.ToByte((value & 0xff00) >> 8);

                log += "Report Slot no : " + obj.PSlotNo.ToString() + "\n";
                log += "Report Port no : " + obj.PPortNo.ToString() + "\n";
                log += "Report CIM mode : " + obj.PGlass.PCimMode.ToString() + "\n";
                log += "Report Foup Seq : " + obj.PGlass.PFoupSeq.ToString() + "\n";
                log += "Report Work order : " + obj.PGlass.PWorkOrderNo.ToString() + "\n";
                log += "Report Work slot : " + obj.PGlass.PWorkSlot.ToString() + "\n";

                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail, log);

                cv_MemoryIoClient.SetBinaryLengthData(0x4314, tmp, 3, false);
                cv_Timechart.SetTimeLock(this.cv_TimechartId, STEP_ID_WaitTm, cv_Tm);
                JumpToStep(cv_TimechartId, STEP_ID_WaitTm);
            }
            catch (Exception ex)
            {
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return rtn;
        }

        void OnEnter_WaitTm(int m_StepId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                uint index = (uint)cv_MemoryIoClient.GetPortValue(0x4317);
                if (index == 0xffff)
                {
                    index = 1;
                }
                else
                {
                    index += 1;
                }
                cv_MemoryIoClient.SetPortValue(0x4317, (int)index);
                cv_Timechart.SetTimeLock(this.cv_TimechartId, STEP_ID_WaitInterval, cv_IndexDelay);
            }
            catch (Exception ex)
            {
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void OnEnter_TriggerLastWorkProcessStartIndex(int m_StepId)
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

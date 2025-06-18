using System;
using System.Collections.Generic;
using System.Text;
using KgsCommon;

namespace CIM
{
    class TimechartEqWorkDataRemoveReport : TimechartControllerBase.TimechartInstanceBase
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


        public static int STEP_ID_TriggerWorkDataRemoveIndex = 1;
        public static int STEP_ID_WaitInterval = 3;
        public static int STEP_ID_WaitTm = 2;
        public TimechartEqWorkDataRemoveReport(TimechartControllerBase m_TimechartController, int m_TimechartId, Dictionary<string, int> m_VarPortMap)
            : base(m_TimechartController, m_TimechartId, m_VarPortMap)
        {

            AssignEnterStepEventFunction(STEP_ID_TriggerWorkDataRemoveIndex, OnEnter_TriggerWorkDataRemoveIndex);
            AssignEnterStepEventFunction(STEP_ID_WaitInterval, OnEnter_WaitInterval);
            AssignEnterStepEventFunction(STEP_ID_WaitTm, OnEnter_WaitTm);
        }
        protected override bool ProcessJob(object m_obj)
        {
            bool rtn = true;
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                string log = "[Process][TimechartEqWorkDataRemoveReport ProcessJob]\n";
                CommonData.HIRATA.MDBCRemoveReport obj = m_obj as CommonData.HIRATA.MDBCRemoveReport;
                byte[] tmp = new byte[26];
                Array.Clear(tmp, 0, tmp.Length);
                int value = (int)obj.PAction;
                tmp[0] = Convert.ToByte(value & 0x00ff);
                log += "Report Action : " + obj.PAction.ToString() + "\n";

                value = (((int)obj.PGlass.PCimMode) << 15) + (int)obj.PGlass.PFoupSeq;
                tmp[2] = Convert.ToByte(value & 0x00ff);
                tmp[3] = Convert.ToByte((value & 0xff00) >> 8);
                log += "Report CIM mode : " + obj.PGlass.PCimMode.ToString() + "\n";
                log += "Report Foup Seq : " + obj.PGlass.PFoupSeq.ToString() + "\n";
                value = (((int)obj.PGlass.PWorkOrderNo) << 8) + (int)obj.PGlass.PWorkSlot;
                tmp[4] = Convert.ToByte(value & 0x00ff);
                tmp[5] = Convert.ToByte((value & 0xff00) >> 8);
                log += "Report work order no : " + obj.PGlass.PWorkOrderNo.ToString() + "\n";
                log += "Report work slot : " + obj.PGlass.PWorkSlot.ToString() + "\n";

                string id = SysUtils.GetFixedLengthString(obj.PGlass.PId, 20);
                byte[] bytes = Encoding.ASCII.GetBytes(id);
                for (int i = 0; i < 20; i++)
                {
                    tmp[6 + i] = bytes[i];
                }
                log += "Report id : " + id + "\n";
                cv_MemoryIoClient.SetBinaryLengthData(0x3497, tmp, 13, false);

                string opid = SysUtils.GetFixedLengthString(obj.POpid, 12);
                cv_MemoryIoClient.SetBinaryLengthData(0x34A4, SysUtils.StringToByteArray(opid), 6, false);
                log += "Report opid : " + opid + "\n";

                string reason = SysUtils.GetFixedLengthString(obj.PReason, 80);
                cv_MemoryIoClient.SetBinaryLengthData(0x34AA, SysUtils.StringToByteArray(reason), 40, false);
                log += "Report reason : " + reason + "\n";
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail  ,log);

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
                uint index = (uint)cv_MemoryIoClient.GetPortValue(0x34D2);
                if (index == 0xffff)
                {
                    index = 1;
                }
                else
                {
                    index += 1;
                }
                cv_MemoryIoClient.SetPortValue(0x34D2, (int)index);
                cv_Timechart.SetTimeLock(this.cv_TimechartId, STEP_ID_WaitInterval, cv_IndexDelay);
            }
            catch (Exception ex)
            {
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        void OnEnter_TriggerWorkDataRemoveIndex(int m_StepId)
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

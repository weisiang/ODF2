using System;
using System.Collections.Generic;
using System.Text;
using KgsCommon;

namespace CIM
{
    class TimechartEqReceiveReport : TimechartControllerBase.TimechartInstanceBase
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



        public static int STEP_ID_TriggerReceiveIndex = 1;
        public static int STEP_ID_WaitInterval = 3;
        public static int STEP_ID_WaitTm = 2;

        public static string ReceiveIndex = "ReceiveIndex";

        public TimechartEqReceiveReport(TimechartControllerBase m_TimechartController, int m_TimechartId, Dictionary<string, int> m_VarPortMap)
            : base(m_TimechartController, m_TimechartId, m_VarPortMap)
        {

            AssignEnterStepEventFunction(STEP_ID_TriggerReceiveIndex, OnEnter_TriggerReceiveIndex);
            AssignEnterStepEventFunction(STEP_ID_WaitInterval, OnEnter_WaitInterval);
            AssignEnterStepEventFunction(STEP_ID_WaitTm, OnEnter_WaitTm);
        }
        protected override bool ProcessJob(object m_obj)
        {
            bool rtn = true;
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                string log = "[Process][TimechartEqReceiveReport ProcessJob]\n";

                CommonData.HIRATA.MDBCWorkTransferReport obj = m_obj as CommonData.HIRATA.MDBCWorkTransferReport;
                byte[] tmp = new byte[26];
                Array.Clear(tmp, 0, tmp.Length);
                //int value = (obj.PUnitNo << 12) + obj.PSlotNo;
                int value = (obj.PUnitNo << 12) + 1;
                tmp[0] = Convert.ToByte(value & 0x00ff);
                tmp[1] = Convert.ToByte((value & 0xff00) >> 8);
                value = (((int)obj.PGlassData.PCimMode) << 15) + (int)obj.PGlassData.PFoupSeq;
                tmp[2] = Convert.ToByte(value & 0x00ff);
                tmp[3] = Convert.ToByte((value & 0xff00) >> 8);
                value = (((int)obj.PGlassData.PWorkOrderNo) << 8) + (int)obj.PGlassData.PWorkSlot;
                tmp[4] = Convert.ToByte(value & 0x00ff);
                tmp[5] = Convert.ToByte((value & 0xff00) >> 8);

                string id = SysUtils.GetFixedLengthString(obj.PGlassData.PId, 20);
                byte[] bytes = Encoding.ASCII.GetBytes(id);
                for (int i = 0; i < 20; i++)
                {
                    tmp[6 + i] = bytes[i];
                }

                log += "Report unit no : " + obj.PUnitNo + "\n";
                log += "Report Port no : " + obj.PPortNo + "\n";
                log += "Report Slot no : " + obj.PSlotNo + "\n";
                log += "Report CIM Mode : " + obj.PGlassData.PCimMode + "\n";
                log += "Report PFoup Seq : " + obj.PGlassData.PFoupSeq + "\n";
                log += "Report Work Order No : " + obj.PGlassData.PWorkOrderNo + "\n";
                log += "Report Work Slot : " + obj.PGlassData.PWorkSlot + "\n";
                log += "Report id : " + id + "\n";
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail, log);

                cv_MemoryIoClient.SetBinaryLengthData(0x347B, tmp, 13, false);

                int tmp_slot = cv_MemoryIoClient.GetPortValue(0x347B) & 0xff;
                string tmp2 = cv_MemoryIoClient.GetBinaryLengthData(0x347E, 10, false);
                log += "PLC id : " + tmp2 +  " PLC slot : " + tmp_slot;
                if ((tmp2.Trim() != id.Trim()) || (tmp_slot != obj.PSlotNo))
                {
                    Queue<object> tmp_q = new Queue<object>();
                    while (cv_Jobs.Count > 0)
                    {
                        tmp_q.Enqueue(cv_Jobs.Dequeue());
                    }
                    cv_Jobs.Clear();
                    cv_Jobs.Enqueue(obj);
                    while (tmp_q.Count > 0)
                    {
                        cv_Jobs.Enqueue(tmp_q.Dequeue());
                    }
                    log += "PLC id unmatch , Add Job";
                    rtn = false;
                }
                else
                {
                    cv_Timechart.SetTimeLock(this.cv_TimechartId, STEP_ID_WaitTm, cv_Tm);
                    JumpToStep(cv_TimechartId, STEP_ID_WaitTm);
                }
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
                uint index = (uint)cv_MemoryIoClient.GetPortValue(0x3488);
                if (index == 0xffff)
                {
                    index = 1;
                }
                else
                {
                    index += 1;
                }
                cv_MemoryIoClient.SetPortValue(0x3488, (int)index);
                cv_Timechart.SetTimeLock(this.cv_TimechartId, STEP_ID_WaitInterval, cv_IndexDelay);
            }
            catch (Exception ex)
            {
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void OnEnter_TriggerReceiveIndex(int m_StepId)
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

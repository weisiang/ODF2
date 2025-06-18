using System;
using System.Collections.Generic;
using System.Text;
using KgsCommon;
using BaseAp;

namespace CIM
{
    class TimechartEqWorkDataRequest : TimechartControllerBase.TimechartInstanceBase
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

        public static int STEP_ID_TriggerEqWorkDataRequest = 1;
        public static int STEP_ID_WaitBcWorkDataReplyOn = 2;
        public static int STEP_ID_WaitBcWorkDataReplyOff = 3;

        public static string BcWorkDataReply = "BcWorkDataReply";
        public static string EqWorkDataRequest = "EqWorkDataRequest";
        CommonData.HIRATA.MDBCDataRequest cur_job = null;
        public TimechartEqWorkDataRequest(TimechartControllerBase m_TimechartController, int m_TimechartId, Dictionary<string, int> m_VarPortMap)
            : base(m_TimechartController, m_TimechartId, m_VarPortMap)
        {
            m_VarPortMap[BcWorkDataReply] = 0x001AF;
            m_VarPortMap[EqWorkDataRequest] = 0x00384F;
            AssignEnterStepEventFunction(STEP_ID_TriggerEqWorkDataRequest, OnEnter_TriggerEqWorkDataRequest);
            AssignEnterStepEventFunction(STEP_ID_WaitBcWorkDataReplyOn, OnEnter_WaitBcWorkDataReplyOn);
            AssignEnterStepEventFunction(STEP_ID_WaitBcWorkDataReplyOff, OnEnter_WaitBcWorkDataReplyOff);
        }
        protected override bool ProcessJob(object m_obj)
        {
            bool rtn = true;
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                string log = "[Process][TimechartEqWorkDataRequest ProcessJob]\n";
                cv_MemoryIoClient.SetPortValue(0x384F, 0);
                CommonData.HIRATA.MDBCDataRequest obj = m_obj as CommonData.HIRATA.MDBCDataRequest;
                cur_job = null;
                cur_job = obj;
                byte[] tmp = new byte[3 << 1];
                Array.Clear(tmp, 0, tmp.Length);

                int value = ((int)obj.PRequestKey << 4) + 1;
                log += "Report request key : " + obj.PRequestKey.ToString() + "\n";
                tmp[0] = Convert.ToByte(value & 0x00ff);
                tmp[1] = Convert.ToByte((value & 0xff00) >> 4);
                value = (obj.PCimMode == CommonData.HIRATA.OnlineMode.Control ? 1 : 0);
                log += "Report CIM mode : " + obj.PCimMode.ToString() + "\n";
                tmp[3] = Convert.ToByte(value << 7);

                value = obj.PFoupSeq;
                tmp[2] = Convert.ToByte(value);
                log += "Report Foup seq : " + obj.PFoupSeq.ToString() + "\n";

                value = obj.PWorkdSlot;
                tmp[4] = Convert.ToByte(value);
                log += "Report work slot : " + obj.PWorkdSlot.ToString() + "\n";

                value = obj.PWorkOrderNo;
                tmp[5] = Convert.ToByte(value);
                log += "Report work order no : " + obj.PWorkOrderNo.ToString() + "\n";
                cv_MemoryIoClient.SetBinaryLengthData(0x3842, tmp, 3);

                string work_id = SysUtils.GetFixedLengthString(obj.PWorkId.Trim(), 20);
                log += "Report id : " + work_id  + "\n";
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail, log);
                cv_MemoryIoClient.SetBinaryLengthData(0x3845, SysUtils.StringToByteArray(work_id), 10, false);
            }
            catch (Exception ex)
            {
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return rtn;
        }

        public override void OnTimeout(int m_TimeoutId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                string log = "[Process][TimechartEqWorkDataRequest OnTimeout]\n";
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail, log);
                cv_MemoryIoClient.SetPortValue(0x384F, 0);
                StopTimeout(cv_TimechartId, m_TimeoutId);
                cv_Timechart.RestartTimeChart(cv_TimechartId);
                CommonData.HIRATA.MDBCAlarmReportToLGC obj = new CommonData.HIRATA.MDBCAlarmReportToLGC();
                if (m_TimeoutId == CommonData.HIRATA.Alarmtable.BcHsDataRequestTeTimeOut)
                {
                    obj.PAlarm.PMainDescription = "Data request hand shake Te Time out";
                }
                else if (m_TimeoutId == CommonData.HIRATA.Alarmtable.BcHsDataRequestTsTimeOut)
                {
                    obj.PAlarm.PMainDescription = "Data request hand shake Ts Time out";
                }
                obj.PAlarm.PCode = m_TimeoutId.ToString();
                obj.PAlarm.PStatus = CommonData.HIRATA.AlarmStatus.Occur;
                obj.PAlarm.PUnit = 0;
                obj.PAlarm.PLevel = CommonData.HIRATA.AlarmLevele.Light;
                obj.PAlarm.PTime = DateTime.Now.ToString("yyyyMMddhhmmss");
                CIMController.triggerCimEvent(typeof(CommonData.HIRATA.MDBCAlarmReportToLGC).Name, obj);
            }
            catch (Exception ex)
            {
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        void OnEnter_TriggerEqWorkDataRequest(int m_StepId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                cv_MemoryIoClient.SetPortValue(0x384F, 1);
                cv_Timechart.StartTimeout(this.cv_TimechartId, CommonData.HIRATA.Alarmtable.BcHsDataRequestTsTimeOut, cv_Ts, false);
            }
            catch (Exception ex)
            {
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        void OnEnter_WaitBcWorkDataReplyOn(int m_StepId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                cv_MemoryIoClient.SetPortValue(0x384F, 0);
                cv_Timechart.StopTimeout(this.cv_TimechartId, CommonData.HIRATA.Alarmtable.BcHsDataRequestTsTimeOut);
                Read();
                cv_Timechart.StartTimeout(this.cv_TimechartId, CommonData.HIRATA.Alarmtable.BcHsDataRequestTeTimeOut, cv_Ts, false);
            }
            catch (Exception ex)
            {
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        void OnEnter_WaitBcWorkDataReplyOff(int m_StepId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                cv_Timechart.StopTimeout(this.cv_TimechartId, CommonData.HIRATA.Alarmtable.BcHsDataRequestTeTimeOut);
                Clean();
            }
            catch (Exception ex)
            {
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        private void Read()
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                string log = "[Process][TimechartEqWorkDataRequest Read]\n";
                cur_job.PResult = (cv_MemoryIoClient.GetPortValue(0x016E) == 0 ? CommonData.HIRATA.Result.OK : CommonData.HIRATA.Result.NG);
                cur_job.PGlass = new CommonData.HIRATA.GlassData(cv_MemoryIoClient, 0x016F);
                log += "Result : " + cur_job.PResult.ToString() + "\n";
                log += cur_job.PGlass.GetGlassDataStr();
                CIMController.triggerCimEvent(typeof(CommonData.HIRATA.MDBCDataRequest).Name, cur_job);
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail, log);
            }
            catch (Exception ex)
            {
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void Clean()
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                byte[] tmp = new byte[26];
                Array.Clear(tmp, 0, tmp.Length);
                cv_MemoryIoClient.SetBinaryLengthData(0x345F, tmp, 13, false);
            }
            catch (Exception ex)
            {
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
    }
}

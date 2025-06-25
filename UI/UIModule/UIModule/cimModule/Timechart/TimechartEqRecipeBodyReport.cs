using System;
using System.Collections.Generic;
using System.Text;
using KgsCommon;

namespace CIM
{
    class TimechartEqRecipeBodyReport : TimechartControllerBase.TimechartInstanceBase
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


        public static int STEP_ID_TriggerRecipeBodyIndex = 1;
        public static int STEP_ID_WaitInterval = 3;
        public static int STEP_ID_WaitTm = 2;
        public TimechartEqRecipeBodyReport(TimechartControllerBase m_TimechartController, int m_TimechartId, Dictionary<string, int> m_VarPortMap)
            : base(m_TimechartController, m_TimechartId, m_VarPortMap)
        {
            AssignEnterStepEventFunction(STEP_ID_TriggerRecipeBodyIndex, OnEnter_TriggerRecipeBodyIndex);
            AssignEnterStepEventFunction(STEP_ID_WaitInterval, OnEnter_WaitInterval);
            AssignEnterStepEventFunction(STEP_ID_WaitTm, OnEnter_WaitTm);
        }
        protected override bool ProcessJob(object m_obj)
        {
            bool rtn = true;
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                string log = "[Process][TimechartEqRecipeBodyReport ProcessJob]\n";
                CommonData.HIRATA.MDBCRecipeBodyReport obj = m_obj as CommonData.HIRATA.MDBCRecipeBodyReport;
                DateTime time = DateTime.ParseExact(obj.PRecipe.PTime, "yyyyMMddhhmmss", null);
                byte[] tmp = new byte[9 * 2];
                Array.Clear(tmp, 0, tmp.Length);

                int value = (obj.PUnit << 4) + (int)obj.PAction;
                tmp[0] = Convert.ToByte(value & 0x00ff);
                log += "Report Action : " + obj.PAction.ToString() + "\n";

                value = Convert.ToInt32(obj.PRecipe.cv_Id);
                tmp[2] = Convert.ToByte(value & 0x00ff);
                tmp[3] = Convert.ToByte((value & 0xff00) >> 8);

                log += "Report recipe id : " + obj.PRecipe.cv_Id.ToString() + "\n";

                string year_mon = "0x" + (time.Year - 2000).ToString() + (time.Month.ToString().PadLeft(2, '0'));
                string day_hour = "0x" + time.Day.ToString().PadLeft(2, '0') + (time.Hour.ToString().PadLeft(2, '0'));
                string min_sec = "0x" + time.Minute.ToString().PadLeft(2, '0') + (time.Second.ToString().PadLeft(2, '0'));

                log += "Report date time : " + obj.PRecipe.PTime + "\n";

                tmp[4] = Convert.ToByte(Convert.ToInt32(year_mon, 16) & 0x00ff);
                tmp[5] = Convert.ToByte((Convert.ToInt32(year_mon, 16) & 0xff00) >> 8);
                tmp[6] = Convert.ToByte(Convert.ToInt32(day_hour, 16) & 0x00ff);
                tmp[7] = Convert.ToByte((Convert.ToInt32(day_hour, 16) & 0xff00) >> 8);
                tmp[8] = Convert.ToByte(Convert.ToInt32(min_sec, 16) & 0x00ff);
                tmp[9] = Convert.ToByte((Convert.ToInt32(min_sec, 16) & 0xff00) >> 8);

                //10 & 11 is full flow no.
                tmp[10] = Convert.ToByte(obj.PRecipe.PFlow);
                log += "Report Flow : " + obj.PRecipe.PFlow.ToString() +"\n";
                //tmp[11]

                value = Convert.ToInt32(obj.PRecipe.PWaferVASDegree * 10);
                tmp[12] = Convert.ToByte(value & 0x00ff);
                tmp[13] = Convert.ToByte((value & 0xff00) >> 8);
                log += "Report wafer Vas Degress : " + obj.PRecipe.PWaferVASDegree.ToString() +"\n";

                value = Convert.ToInt32(obj.PRecipe.PGlassVASDegree * 10);
                tmp[14] = Convert.ToByte(value & 0x00ff);
                tmp[15] = Convert.ToByte((value & 0xff00) >> 8);
                log += "Report wafer Ijp Degress : " + obj.PRecipe.PGlassVASDegree.ToString() + "\n";

                /*
                value = Convert.ToInt32(obj.PRecipe.PGlassVASDegree * 10);
                tmp[16] = Convert.ToByte(value & 0x00ff);
                tmp[17] = Convert.ToByte((value & 0xff00) >> 8);
                log += "Report glass vas Degress : " + obj.PRecipe.PGlassVASDegree.ToString() + "\n";
                */

                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail, log);

                cv_MemoryIoClient.SetBinaryLengthData(0x454C, tmp, 9, false);
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
            uint index = (uint)cv_MemoryIoClient.GetPortValue(0x467D);
            if (index == 0xffff)
            {
                index = 1;
            }
            else
            {
                index += 1;
            }
            cv_MemoryIoClient.SetPortValue(0x467D, (int)index);
            cv_Timechart.SetTimeLock(this.cv_TimechartId, STEP_ID_WaitInterval, cv_IndexDelay);
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        void OnEnter_TriggerRecipeBodyIndex(int m_StepId)
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

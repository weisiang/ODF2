using System;
using System.Collections.Generic;
using System.Text;
using KgsCommon;
using CommonData;
using BaseAp;

namespace CIM
{
    class TimechartBcDateTimeCalibration : TimechartControllerBase.TimechartInstanceBase
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

        public static int STEP_ID_BcDateTimeCalibration = 1;

        int cv_Value;
        int cv_Port = 0x0004;
        KDateTime cv_Date = SysUtils.Now();

        int BcSec = 0;
        public TimechartBcDateTimeCalibration(TimechartControllerBase m_TimechartController, int m_TimechartId, Dictionary<string, int> m_VarPortMap)
            : base(m_TimechartController, m_TimechartId, m_VarPortMap)
        {
            cv_Value = cv_MemoryIoClient.GetPortValue(cv_Port);
            AssignRunningStepEventFunction(STEP_ID_BcDateTimeCalibration, OnRunning_BcDateTimeCalibration);
            //AssignEnterStepEventFunction(STEP_ID_BcDateTimeCalibration, OnEnter_BcDateTimeCalibration);
            BcSec = cv_MemoryIoClient.GetPortValue(0x02) ;
            KDateTime cv_Date = SysUtils.Now();
        }

        void OnRunning_BcDateTimeCalibration(int m_StepId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                DateTime tmp = DateTime.Now;
                string year_mon = "0x" + (tmp.Year - 2000).ToString() + (tmp.Month.ToString().PadLeft(2, '0'));
                string day_hour = "0x" + tmp.Day.ToString().PadLeft(2, '0') + (tmp.Hour.ToString().PadLeft(2, '0'));
                string min_sec = "0x" + tmp.Minute.ToString().PadLeft(2, '0') + (tmp.Second.ToString().PadLeft(2, '0'));
                string week = "0x" + ((int)tmp.DayOfWeek).ToString();
                byte[] tmp2 = new byte[8];

                tmp2[0] = Convert.ToByte(Convert.ToInt32(year_mon, 16) & 0x00ff);
                tmp2[1] = Convert.ToByte((Convert.ToInt32(year_mon, 16) & 0xff00) >> 8);
                tmp2[2] = Convert.ToByte(Convert.ToInt32(day_hour, 16) & 0x00ff);
                tmp2[3] = Convert.ToByte((Convert.ToInt32(day_hour, 16) & 0xff00) >> 8);
                tmp2[4] = Convert.ToByte(Convert.ToInt32(min_sec, 16) & 0x00ff);
                tmp2[5] = Convert.ToByte((Convert.ToInt32(min_sec, 16) & 0xff00) >> 8);
                tmp2[6] = Convert.ToByte(Convert.ToInt32(week, 16) & 0x00ff);
                tmp2[7] = 0;
                cv_MemoryIoClient.SetBinaryLengthData(0x4240, tmp2, 4, false);

                int tmp3 = cv_MemoryIoClient.GetPortValue(0x02);
                if (tmp3 != BcSec)
                {
                    BcSec = tmp3;
                    cv_Date = SysUtils.Now();
                    lgcBase.PSystemData.PBcAlive = true;
                }
                else
                {
                    long diff = SysUtils.MilliSecondsBetween(SysUtils.Now(), cv_Date);
                    if (diff < 0)
                    {
                        cv_Date = SysUtils.Now();
                    }
                    else
                    {
                        if (diff > 5000)
                        {
                            lgcBase.PSystemData.PBcAlive = false;
                        }
                    }
                }
                if (cv_Value != cv_MemoryIoClient.GetPortValue(cv_Port))
                {
                    CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Detail , "[Process][OnRunning_BcDateTimeCalibration] index change to " + cv_Value.ToString());
                    cv_Value = cv_MemoryIoClient.GetPortValue(cv_Port);
                    BcDateTimeCalibration();
                }
            }
            catch(Exception ex)
            {
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Error , ex.ToString());
            }
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        void OnEnter_BcDateTimeCalibration(int m_StepId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        void BcDateTimeCalibration()
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                string log = "[Process][BcDateTimeCalibration] : \n";
                int word1 = cv_MemoryIoClient.GetPortValue(0x0);
                int word2 = cv_MemoryIoClient.GetPortValue(0x01);
                int word3 = cv_MemoryIoClient.GetPortValue(0x02);
                int word4 = cv_MemoryIoClient.GetPortValue(0x03);

                CommonData.HIRATA.MDBCTimeAdjust obj = new CommonData.HIRATA.MDBCTimeAdjust();

                string value = ((word1 & 0xFF00) >> 8).ToString("X2");
                obj.PYear = int.Parse(value, System.Globalization.NumberStyles.Integer);
                obj.PYear += 2000;
                value = (word1 & 0x00FF).ToString("X2");
                obj.PMon = int.Parse(value, System.Globalization.NumberStyles.Integer);

                value = ((word2 & 0xFF00) >> 8).ToString("X2");
                obj.PDay = int.Parse(value, System.Globalization.NumberStyles.Integer);

                value = (word2 & 0x00FF).ToString("X2");
                obj.PHour = int.Parse(value, System.Globalization.NumberStyles.Integer);

                value = ((word3 & 0xFF00) >> 8).ToString("X2");
                obj.PMin = int.Parse(value, System.Globalization.NumberStyles.Integer);

                value = (word3 & 0x00FF).ToString("X2");
                obj.PSec = int.Parse(value, System.Globalization.NumberStyles.Integer);

                value = (word4 & 0x00FF).ToString("X2");
                obj.PWeek = int.Parse(value, System.Globalization.NumberStyles.Integer);
                log += "Year : " + obj.PYear + " Mon : " + obj.PMon + " Day : " + obj.PDay + " Hour : " + obj.PHour + " Min : " + obj.PMin + " Sec : " + obj.PSec +
                    "Week : " + obj.PWeek + "\n";
                CIMController.triggerCimEvent(typeof(CommonData.HIRATA.MDBCTimeAdjust).Name, obj);
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

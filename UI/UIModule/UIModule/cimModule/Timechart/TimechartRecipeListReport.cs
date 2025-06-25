using System;
using System.Collections.Generic;
using System.Text;
using KgsCommon;
using CommonData.HIRATA;

namespace CIM
{
    class TimechartRecipeListReport : TimechartControllerBase.TimechartInstanceBase
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



        public static int STEP_ID_TriggerRecipeExistIndex = 1;
        public static int STEP_ID_WaitInterval = 3;
        public static int STEP_ID_WaitTm = 2;

        public TimechartRecipeListReport(TimechartControllerBase m_TimechartController, int m_TimechartId, Dictionary<string, int> m_VarPortMap)
            : base(m_TimechartController, m_TimechartId, m_VarPortMap)
        {
            AssignEnterStepEventFunction(STEP_ID_TriggerRecipeExistIndex, OnEnter_TriggerRecipeExistIndex);
            AssignEnterStepEventFunction(STEP_ID_WaitInterval, OnEnter_WaitInterval);
            AssignEnterStepEventFunction(STEP_ID_WaitTm, OnEnter_WaitTm);
        }
        protected override bool ProcessJob(object m_obj)
        {
            bool rtn = true;
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                string log = "[Process][TimechartRecipeListReport ]\n";
                CommonData.HIRATA.RecipeData obj = m_obj as CommonData.HIRATA.RecipeData;
                byte[] tmp = new byte[63 << 1];
                Array.Clear(tmp, 0, tmp.Length);

                bool[] container = new bool[1008];
                Array.Clear(container, 0, container.Length);
                log += " Cur Recipe : ";
                for (int i = 0; i < obj.cv_RecipeList.Count; i++)
                {
                    CommonData.HIRATA.RecipeItem item = obj.cv_RecipeList[i];
                    container[Convert.ToInt32(item.PId) - 1] = true;
                    if(i == obj.cv_RecipeList.Count - 1)
                    {
                        log += item.PId + "\n";
                }
                    else
                    {
                        log += item.PId + " , ";
                    }
                }
                CimModule.WriteLog(LogLevelType.Detail, log);

                UInt32 sum = 0;
                for (int i = 0; i < 63; i++)
                {
                    sum = 0;
                    for (int j = 1; j <= 16; j++)
                    {
                        sum += (uint)((container[i * 16 + j - 1] == true ? 1 : 0) << (j - 1));
                    }
                    tmp[i << 1] = Convert.ToByte(sum & 0x00ff);
                    tmp[(i << 1) + 1] = Convert.ToByte((sum & 0xff00) >> 8);
                }
                cv_MemoryIoClient.SetBinaryLengthData(0x431C , tmp, 63, false);
                CimModule.WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
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
                uint index = (uint)cv_MemoryIoClient.GetPortValue(0x435B);
                if (index == 0xffff)
                {
                    index = 1;
                }
                else
                {
                    index += 1;
                }
                cv_MemoryIoClient.SetPortValue(0x435B, (int)index);
                cv_Timechart.SetTimeLock(this.cv_TimechartId, STEP_ID_WaitInterval, cv_IndexDelay);
            }
            catch (Exception ex)
            {
                CimModule.WriteLog(CommonData.HIRATA.LogLevelType.Error, ex.ToString());
            }
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void OnEnter_TriggerRecipeExistIndex(int m_StepId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        void OnEnter_WaitInterval(int m_StepId)
        {
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            CimModule.WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
        }
    }
}

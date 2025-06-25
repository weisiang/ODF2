using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonData.HIRATA;
using UI;
using LGC;
using BaseAp;

namespace CIM
{
    public partial class CIMController
    {
        #region base recipe , alarm , system , timeout , glass count.
        protected override void ProcessSystemData(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessSystemData( m_SourceModule,  m_MessageId, m_Object);
            //Equipment status
            byte[] tmp = new byte[10];
            Array.Clear(tmp, 0, tmp.Length);

            if(lgcBase.PSystemData.PSystemStatus == EquipmentStatus.WaitIdle)
            {
                CimModule.PMio.SetPortValue(0x424F, (int)EquipmentStatus.Run);
                tmp[0] = Convert.ToByte((int)EquipmentStatus.Run);
            }
            else
            {
                tmp[0] = Convert.ToByte((int)lgcBase.PSystemData.PSystemStatus);
                CimModule.PMio.SetPortValue(0x424F, (int)lgcBase.PSystemData.PSystemStatus);
            }
            //TODO : 20250625, need add sub unit.(robot*2 , bf*2 , aligner*2)
            CimModule.PMio.SetBinaryLengthData(0x424F, tmp, 5, false);

            tmp = null;
            tmp = new byte[11 << 1];
            int value = ((lgcBase.PSystemData.PSystemOnlineMode == OnlineMode.Control ? 1 : 0) << 4) +
                (lgcBase.PSystemData.POperationMode == OperationMode.Manual ? 0 : 1) + (2 << 8);
            tmp[0] = Convert.ToByte(value & 0x00ff);
            tmp[1] = Convert.ToByte((value & 0xff00) >> 8);

            CimModule.PMio.SetBinaryLengthData(0x4244, tmp, 1, false);


            byte[] tmp2 = new byte[2];
            tmp2[0] = Convert.ToByte(lgcBase.PSystemData.PDataCheckRule & 0x00ff);
            tmp2[1] = Convert.ToByte((lgcBase.PSystemData.PDataCheckRule & 0xff00) >> 8);
            CimModule.PMio.SetBinaryLengthData(0x424C, tmp2, 1, false);

            CimModule.PMio.SetPortValue(0x424D, (int)lgcBase.PSystemData.POcrMode1 + (1 << 4));
            CimModule.PMio.SetPortValue(0x424E, (int)lgcBase.PSystemData.POcrMode2 + (1 << 4));

            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        protected override void ProcessAlarmChange(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessAlarmChange( m_SourceModule,  m_MessageId, m_Object);
            byte[] tmp = new byte[1 << 1];
            tmp[0] = 0;
            tmp[1] = Convert.ToByte(((lgcBase.cv_Alarms.IsHasAlarm() ? 1 : 0) << 4) + (lgcBase.cv_Alarms.IsHasWarning() ? 1 : 0));

            CimModule.PMio.SetBinaryLengthData(0x4245, tmp, 1, false);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessAlarmAction(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CommonData.HIRATA.MDAlarmAction tmp = m_Object as CommonData.HIRATA.MDAlarmAction;
            cv_TimechartController.GetTimeChartInstance(TimechartEqAlarmReport.TIMECHART_ID_EqAlarmReport).AddJob(tmp.AlarmData);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        protected override void ProcessRecipeChange(FdModule m_SourceModule, string m_MessageId, Object m_Object )
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessRecipeChange( m_SourceModule,  m_MessageId,  m_Object);
            cv_TimechartController.GetTimeChartInstance(TimechartRecipeListReport.TIMECHART_ID_EqRecipeListReport).AddJob(m_Object);

            byte[] tmp = new byte[1 << 1];
            RecipeItem cur_recipe = null;
            if (lgcBase.cv_Recipes.GetCurRecipe(out cur_recipe))
            {
                int value = Convert.ToInt32(cur_recipe.PId.Trim()); // obj.PCurReipe;
                tmp[0] = Convert.ToByte(value & 0x00ff);
                tmp[1] = Convert.ToByte((value & 0xff00) >> 8);
                CimModule.PMio.SetBinaryLengthData(0x4247, tmp, 1, false);
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }



        protected override void ProcessTimeoutData(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessTimeoutData( m_SourceModule,  m_MessageId, m_Object);
            byte[] tmp = new byte[1 << 2];
            Array.Clear(tmp, 0, tmp.Length);
            int value = (lgcBase.cv_TimeoutData.PIdleDelayTime / 1000 + ((lgcBase.cv_TimeoutData.PIntervalTime / 1000) << 12));
            tmp[0] = Convert.ToByte(value & 0x00ff);
            tmp[1] = Convert.ToByte((value & 0xff00) >> 8);
            CimModule.PMio.SetBinaryLengthData(0x4246, tmp, 1, false);

            if (lgcBase.cv_TimeoutData.PIntervalTime != cv_TimechartController.IntervalTime)
            {
                cv_TimechartController.IntervalTime = lgcBase.cv_TimeoutData.PIntervalTime;
            }
            if (lgcBase.cv_TimeoutData.PTsTime != cv_TimechartController.TsTime)
            {
                cv_TimechartController.TsTime = lgcBase.cv_TimeoutData.PTsTime;
            }
            if (lgcBase.cv_TimeoutData.PTeTime != cv_TimechartController.TeTime)
            {
                cv_TimechartController.TeTime = lgcBase.cv_TimeoutData.PTeTime;
            }
            if (lgcBase.cv_TimeoutData.PTmTime != cv_TimechartController.TmTime)
            {
                cv_TimechartController.TmTime = lgcBase.cv_TimeoutData.PTmTime;
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        protected override void ProcessGlassCountData(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessGlassCountData( m_SourceModule,  m_MessageId, m_Object);
            byte[] tmp = new byte[4 << 1];
            int value = lgcBase.cv_GlassCountData.PProductCount;
            tmp[0] = Convert.ToByte(value & 0x00ff);
            tmp[1] = Convert.ToByte((value & 0xff00) >> 8);

            value = lgcBase.cv_GlassCountData.PDummyCount;
            tmp[2] = Convert.ToByte(value & 0x00ff);
            tmp[3] = Convert.ToByte((value & 0xff00) >> 8);

            value = lgcBase.cv_GlassCountData.PHistoryCount;
            tmp[4] = Convert.ToByte(value & 0x000000ff);
            tmp[5] = Convert.ToByte((value & 0x0000ff00) >> 8);
            tmp[6] = Convert.ToByte((value & 0x00ff0000) >> 16);
            tmp[7] = Convert.ToByte((value & 0xff000000) >> 24);
            CimModule.PMio.SetBinaryLengthData(0x4248, tmp, 4, false);



            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        #endregion

        #region process robot , port , aligner , buffer , eq data
        protected override void ProcessPortData(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessPortData( m_SourceModule,  m_MessageId,  m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessRobotData(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessRobotData( m_SourceModule,  m_MessageId,  m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessBufferData(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessBufferData( m_SourceModule,  m_MessageId,  m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessAlignerData(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessAlignerData( m_SourceModule,  m_MessageId,  m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessEqData(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessEqData( m_SourceModule,  m_MessageId,  m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        #endregion
    }
}

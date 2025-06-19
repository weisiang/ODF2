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
        #region Link log in/out Event & Event function;
        //Trigger this event When AccountData login/out successful.(UI must override)
        protected override void OnLogInOutEvent(LogInOut m_Action, CommonData.HIRATA.AccountItem m_CurAccount)
        {
        }

        //Trigger this event When AccountData change.(UI must override)
        protected override void OnAccountChangeEvent()
        {
        }
        #endregion

        #region Link Alarm Event & Event function
        //Trigger this event When AlarmData add/del successful.(LGC must override)
        protected override void OnAlarmActionEvent(AlarmStatus m_Action, List<CommonData.HIRATA.AlarmItem> m_Alarms)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            cv_TimechartController.GetTimeChartInstance(TimechartEqAlarmReport.TIMECHART_ID_EqAlarmReport).AddJob(lgcBase.cv_Alarms);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        //Trigger this event When AlarmData change.(LGC must override)
        protected override void OnAlarmChange()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);

            byte[] tmp = new byte[1 << 1];
            tmp[0] = 0;
            tmp[1] = Convert.ToByte(((lgcBase.cv_Alarms.IsHasAlarm() ? 1 : 0) << 4) + (lgcBase.cv_Alarms.IsHasWarning() ? 1 : 0));

            cv_Driver.SetBinaryLengthData(0x3445, tmp, 1, false);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        #endregion

        #region Link Recipe Event & Event function
        //Trigger this event When RecipeData add/del/Modify successful.(LGC must override)
        protected override void OnRecipeActionEvent(DataEidtAction m_Action, List<RecipeItem> m_Recipes)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            cv_TimechartController.GetTimeChartInstance(TimechartRecipeListReport.TIMECHART_ID_EqRecipeListReport).AddJob(lgcBase.cv_Recipes);

            byte[] tmp = new byte[1<<1];
            RecipeItem cur_recipe = null;
            if (lgcBase.cv_Recipes.GetCurRecipe(out cur_recipe))
            {
                int value = Convert.ToInt32(cur_recipe.PId.Trim()); // obj.PCurReipe;
                tmp[0] = Convert.ToByte(value & 0x00ff);
                tmp[1] = Convert.ToByte((value & 0xff00) >> 8);
                cv_Driver.SetBinaryLengthData(0x3447, tmp, 1 , false);
            }
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        //Trigger this event When RecipeData change.(LGC must override)
        #endregion

        #region Link time out data Event & Event function
        //Trigger this event When Timeout data change.(LGC must override)
        protected override void OnTimeOutDataChange()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);

            byte[] tmp = new byte[1 << 2];
            Array.Clear(tmp, 0, tmp.Length);
            int value = (lgcBase.cv_TimeoutData.PIdleDelayTime / 1000 + ((lgcBase.cv_TimeoutData.PIntervalTime / 1000) << 12));
            tmp[0] = Convert.ToByte(value & 0x00ff);
            tmp[1] = Convert.ToByte((value & 0xff00) >> 8);
            cv_Driver.SetBinaryLengthData(0x3446, tmp, 1, false);

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

            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        #endregion

        #region Link glass count Event & Event function
        protected override void OnGlassCountDataChange()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
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
            cv_Driver.SetBinaryLengthData(0x3448, tmp, 4, false);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        #endregion

        #region Link system data , status , robot status Event & Event function
        protected override void OnSystemDataChange()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);

            //Equipment status
            byte[] tmp = new byte[10];
            Array.Clear(tmp, 0, tmp.Length);

            if (lgcBase.PSystemData.PSystemStatus == EquipmentStatus.WaitIdle)
            {
                cv_Driver.SetPortValue(0x344F, (int)EquipmentStatus.Run);
                tmp[0] = Convert.ToByte((int)EquipmentStatus.Run);
            }
            else
            {
                tmp[0] = Convert.ToByte((int)lgcBase.PSystemData.PSystemStatus);
                cv_Driver.SetPortValue(0x344F, (int)lgcBase.PSystemData.PSystemStatus);
            }
            cv_Driver.SetBinaryLengthData(0x344F, tmp, 5, false);

            tmp = null;
            tmp = new byte[11 << 1];
            int value = ((lgcBase.PSystemData.PSystemOnlineMode == OnlineMode.Control ? 1 : 0) << 4) +
                (lgcBase.PSystemData.POperationModeLeft == OperationMode.Manual ? 0 : 1) + (2 << 8);
            tmp[0] = Convert.ToByte(value & 0x00ff);
            tmp[1] = Convert.ToByte((value & 0xff00) >> 8);

            cv_Driver.SetBinaryLengthData(0x3444, tmp, 1, false);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void OnRobot1StatusChange()
        {
        }
        protected override void OnRobot2StatusChange()
        {
        }
        protected override void OnSystemStatusChange()
        {
        }
        protected override void OnApiConnected()
        {
        }
        protected override void OnApiDisconnected()
        {
        }
        protected override void OnOperationModeChangeRight()
        {
        }
        protected override void OnOperationModeChangeLeft()
        {
        }
        protected override void OnPlcConnected()
        {
        }
        protected override void OnPlcDisconnected()
        {
        }
        protected override void OnBclive()
        {
        }
        protected override void OnBcDie()
        {
        }
        #endregion
    }
}

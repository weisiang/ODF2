using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonData.HIRATA;

namespace UI
{
    public partial class UIController
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
        }

        //Trigger this event When AlarmData change.(LGC must override)
        protected override void OnAlarmChange()
        {
        }
        #endregion

        #region Link Recipe Event & Event function
        //Trigger this event When RecipeData add/del/Modify successful.(LGC must override)
        protected override void OnRecipeActionEvent(DataEidtAction m_Action, List<RecipeItem> m_Recipes)
        {
        }
        //Trigger this event When RecipeData change.(LGC must override)
        #endregion

        #region Link time out data Event & Event function
        //Trigger this event When Timeout data change.(LGC must override)
        protected override void OnTimeOutDataChange()
        {
        }
        #endregion

        #region Link glass count Event & Event function
        protected override void OnGlassCountDataChange()
        {
        }
        #endregion

        #region Link system data , status , robot status Event & Event function
        protected override void OnSystemDataChange()
        {
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

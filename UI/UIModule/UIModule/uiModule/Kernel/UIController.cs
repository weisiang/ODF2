using System;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KgsCommon;
using System.Text.RegularExpressions;
using System.Collections;
using CommonData.HIRATA;
using BaseAp;
using LGC;
using CIM;

namespace UI
{
    public partial class  UIController : BaseEventController
    {
        public static event deleSubscription eventUi;

        public delegate void DeleAppEvent(string m_MessageId, object m_Object);
        public static event DeleAppEvent EventAppEvent;

        public UIController() : base (FdModule.UI)
        {
        }
        ~UIController()
        {
        }
        public static void triggerUiEvent(string messageId, Object obj)
        {
            if (eventUi != null)
            {
                eventUi(FdModule.UI, messageId, obj);
            }
        }
        public override void linkEvent()
        {
            if (cv_module == FdModule.CIM)
            {
                LGCController.eventLgc += receiveSubcription;
                UIController.eventUi += receiveSubcription;
            }
            else if (cv_module == FdModule.UI)
            {
                LGCController.eventLgc += receiveSubcription;
                CIMController.eventCim += receiveSubcription;
            }
            else if (cv_module == FdModule.LGC)
            {
                CIMController.eventCim += receiveSubcription;
                UIController.eventUi += receiveSubcription;
            }
        }
        /*
        protected override void ProcessLgcStart(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            g_eventController.LinkCommonDataEvent(lgcBase.cv_Alarms, BaseForm.cv_AccountData, lgcBase.cv_Recipes,
                lgcBase.cv_TimeoutData, lgcBase.cv_GlassCountData, lgcBase.PSystemData);
        }
        */

        public override void addSubScription()
        {
            base.addSubScription();
            subscriptionMap.Add(typeof(CommonData.HIRATA.MDTimeChartChange).Name, ProcessMmfEvent);
            //subscriptionMap.Add(typeof(CommonData.HIRATA.MDBCTimeAdjust).Name, ProcessBcTimeAdjust); //time adjust
            subscriptionMap.Add( typeof(CommonData.HIRATA.MDShowMsg).Name , ProcessMmfEvent);
            subscriptionMap.Add(typeof(CommonData.HIRATA.MDBCMsg).Name, ProcessMmfEvent);        // show bc msg.
            subscriptionMap.Add(typeof(CommonData.HIRATA.MDInitial).Name, ProcessMsg);        // show bc msg.
            subscriptionMap.Add(typeof(CommonData.HIRATA.MDPopMonitorForm).Name, ProcessMsg);        // show bc msg.
            subscriptionMap.Add(typeof(CommonData.HIRATA.MDShowOcrDecide).Name, ProcessMsg);        // show bc msg.
            //subscriptionMap.Add(typeof(CommonData.HIRATA.PortData).Name, ProcessPortData); // data download.
            //subscriptionMap.Add(typeof(CommonData.HIRATA.MDBCIdleDelayTime).Name, ProcessBcIdleTime); // bc set idle interval.
            //subscriptionMap.Add(typeof(CommonData.HIRATA.MDBCIndexInterval).Name, ProcessBcIntervalTime);  // bc set index interval.
            //subscriptionMap.Add(typeof(CommonData.HIRATA.MDBCPortCommand).Name, ProcessBcPortCommand);    //bc set port command.
            //subscriptionMap.Add(typeof(CommonData.HIRATA.MDBCRecipeBodyQuery).Name, ProcessRecipeBodyReq); // bc recipe body query.
            //subscriptionMap.Add(typeof(CommonData.HIRATA.MDBCRecipeExist).Name, ProcessRecipeExist); // bc recipe exist.
            //subscriptionMap.Add(typeof(CommonData.HIRATA.MDBCDataRequest).Name, ProcessData); // bc reply data demand.
            //subscriptionMap.Add(typeof(CommonData.HIRATA.MDBCAlarmReportToLGC).Name, ProcessBcAlarm); //report alarm to other modules.
            /*
            //common
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.PortData).Name, ProcessMmfEvent);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.RobotData).Name, ProcessMmfEvent);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.BufferData).Name, ProcessMmfEvent);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.AlignerData).Name, ProcessMmfEvent);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.EqData).Name, ProcessMmfEvent);
            AssignMmfEventObjectFunction( typeof(CommonData.HIRATA.MDShowMsg).Name , ProcessMsg);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDPopMonitorForm).Name, ProcessMmfEvent);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDPopOpidForm).Name, ProcessMmfEvent);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDRobotjobPath).Name, ProcessMmfEvent);

            //by case , please remove following define. And Add by csae code.
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDBCDataRequest).Name, ProcessMmfEvent);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDBCMsg).Name, ProcessMmfEvent);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDEfemStatus).Name, ProcessMmfEvent);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDEfemStatusSingle).Name, ProcessMmfEvent);
            AssignMmfEventObjectFunction(typeof(CommonData.HIRATA.MDTimeChartChange).Name, ProcessMmfEvent);
            */
        }

        protected override void ProcessMmfEvent(FdModule module, string m_MessageId, Object m_Object)
        {
            Console.WriteLine("ProcessMmfEvent : " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            //WriteIn
            string log = "Recv : " + m_MessageId + Environment.NewLine;
            if (EventAppEvent != null)
            {
                log += "Exe UI Event";
                EventAppEvent(m_MessageId, m_Object);
            }
            WriteLog(LogLevelType.General, log);
            //WriteOut
        }
        void ProcessMsg(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            //WriteIn
            string log = "";
            CommonData.HIRATA.MDShowMsg obj = m_Object as CommonData.HIRATA.MDShowMsg;
            CommonData.HIRATA.Msg msg_item = obj.Msg;
            //CommonStaticData.PopForm(msg_item.Txt, msg_item.PAutoClean, msg_item.PUserRep, m_Ticket, msg_item.TimeOut);
            CommonStaticData.PopForm(msg_item.Txt, msg_item.PAutoClean, msg_item.PUserRep, 0 , msg_item.TimeOut);
            WriteLog(LogLevelType.General, log);
            //WriteOut
        }
        public static void SendOpidReply(CommonData.HIRATA.Result m_Result, int m_PortId, string m_Opid, string m_CstSeq, uint m_ticket)
        {
            //WriteIn
            CommonData.HIRATA.MDPopOpidForm obj = new CommonData.HIRATA.MDPopOpidForm();
            obj.PType = CommonData.HIRATA.MmfEventClientEventType.etReply;
            obj.PortId = m_PortId;
            CommonData.HIRATA.ReplyData rtn = new CommonData.HIRATA.ReplyData();
            rtn.CstSeq = m_CstSeq;
            rtn.OpId = m_Opid;
            obj.Reply = rtn;
           //SendMmfReplyObject(typeof(CommonData.HIRATA.MDPopOpidForm).Name, obj, 100, typeof(CommonData.HIRATA.MDPopOpidForm).Name, KParseObjToXmlPropertyType.Field);
            //WriteOut
        }
        public static void SendMonitorReply(int m_PortId, CommonData.HIRATA.Result m_Result)
        {
            //WriteIn
            CommonData.HIRATA.MDPopMonitorFormRep obj = new CommonData.HIRATA.MDPopMonitorFormRep();
            obj.PType = CommonData.HIRATA.MmfEventClientEventType.etReply;
            obj.PortId = m_PortId;
            obj.PResult = m_Result;
            //obj.Port.Add(Form1.GetPort(m_PortId).cv_Data);
            obj.PortData = UiForm.GetPort(m_PortId).cv_Data;
            //SendMmfReplyObject(typeof(CommonData.HIRATA.MDPopMonitorFormRep).Name, obj, 100, typeof(CommonData.HIRATA.MDPopMonitorFormRep).Name, KParseObjToXmlPropertyType.Field);
            //WriteOut
        }
        public void SendRobotActionReq(int m_RobotId, CommonData.HIRATA.RobotAction m_Action, CommonData.HIRATA.RobotArm m_Arm, CommonData.HIRATA.ActionTarget m_Target, int m_TargetId, int m_TargetSlot, bool m_UseHS = false , bool m_IsAlignerExch=false , string m_AlignerDeg="")
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            string log = "";
            CommonData.HIRATA.MDRobotAction obj = new CommonData.HIRATA.MDRobotAction();
            obj.PType = CommonData.HIRATA.MmfEventClientEventType.etRequest;
            obj.PAction = m_Action;
            obj.RobotId = m_RobotId;
            obj.Source.PArm = m_Arm;
            obj.Source.PTarget = m_Target;
            obj.Source.Id = m_TargetId;
            obj.Source.Slot = m_TargetSlot;
            obj.PUseHS = m_UseHS;
            obj.cv_AlignerDeg = m_AlignerDeg.Trim();
            obj.PType = CommonData.HIRATA.MmfEventClientEventType.etRequest;
            log += "Robot id : " + m_RobotId + " Action : " + m_Action.ToString() + " Arm : " + m_Arm.ToString() + " Target : " + m_Target.ToString() + " TargetId : " + m_TargetId + " Slot : " + m_TargetSlot + Environment.NewLine;

            triggerUiEvent(typeof(CommonData.HIRATA.MDRobotAction).Name, obj);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
    }
}

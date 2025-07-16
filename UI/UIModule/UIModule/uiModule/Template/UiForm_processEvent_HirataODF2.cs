using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using KgsCommon;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using CommonData.HIRATA;
using BaseAp;
using CIM;
using LGC;

namespace UI{
    public partial class UiForm : BaseForm
    {
       // Dictionary<string, DeleAppEvent> cv_AppEventMap = new Dictionary<string, DeleAppEvent>();
        public void LinkControllerEvent()
        {
            UIController.EventAppEvent += OnControllerEvent;
        }
        public void OnControllerEvent(string m_Id, object obj)
        {
            if (cv_AppEventMap.ContainsKey(m_Id))
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        OnControllerEvent(m_Id, obj);
                    }));
                }
                else
                {
                    cv_AppEventMap[m_Id](m_Id, obj);
                }
            }
        }
        private void AssignAppEvent()
        {
            //common
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.RecipeData).Name, ProcessRecipeData);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.AlarmData).Name, ProcessAlarmNoti);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.SystemData).Name, ProcessSystemData);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.TimeOutData).Name, ProcessTimeOutData);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.GlassCountData).Name, ProcessGlassCountData);

            //process robot , port , aligner , buffer , eq data
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.PortData).Name, ProcessPortData);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.RobotData).Name, ProcessRobotData);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.BufferData).Name, ProcessBufferData);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.AlignerData).Name, ProcessAlignerData);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.EqData).Name, ProcessEqData);

            //by case
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDOnlineRequest).Name, ProcessOnlineReq);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDOperationModeChange).Name, ProcessOperatorModeChange);

            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDInitial).Name, ProcessInit);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDPopOpidForm).Name, ProcessPopOpidFormReq);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDPopMonitorForm).Name, ProcessPopMonitorFormReq);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDRobotAction).Name, PrcessRobotAction);

            //by case 
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDBCMsg).Name, ProcessBcMsg);//ok
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDShowMsg).Name, ProcessMsg);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDEfemStatus).Name, ProcessEfemStatus);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDEfemStatusSingle).Name, ProcessEfemStatusSingle);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDTimeChartChange).Name, ProcessTimeChartStepChange);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDRobotjobPath).Name, ProcessRobotJobPathChange);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDChangePortSlotType).Name, ProcessChangePortSlotType);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDShowOcrDecide).Name, ProcessShowOcrDecide);
            //WriteNormalOut
        }
        #region Bc msg
        void ProcessBcMsg(string m_MessageId, object m_Object)
        {
            Console.WriteLine("ProcessBcMsg : " + System.Threading.Thread.CurrentThread.ManagedThreadId);
            //WriteNormalIn
            string log = "";
            CommonData.HIRATA.MDBCMsg obj = m_Object as CommonData.HIRATA.MDBCMsg;
            if (obj.PMsg != "")
            {
                if (obj.PMsgType == BcMsgType.Continue)
                    CommonStaticData.PopForm(obj.PMsg, false, false);
                else if (obj.PMsgType == BcMsgType.Interval)
                {
                    CommonStaticData.PopForm(obj.PMsg, true, false, (uint)obj.PIntervalSec);
                }
                if (obj.PBuzzer)
                {
                    CommandData cmd = new CommandData();
                    List<string> para = new List<string>();
                    para.Add("0");
                    cmd = new CommandData(APIEnum.CommandType.IO, APIEnum.IoCommand.Buzzer.ToString(), APIEnum.CommnadDevice.IO, 0, para);
                    CommonData.HIRATA.MDApiCommand bz_obj = new MDApiCommand();
                    bz_obj.CommandData = cmd;
                    UIController.triggerUiEvent(typeof(CommonData.HIRATA.MDApiCommand).Name, bz_obj);
                }
                cv_BcMsgLog.WriteLog("[BC MSG]\n" + obj.PMsg);
            }
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }
        #endregion

        void ProcessMsg(string m_MessageId, object m_Object)
        {
            //WriteIn
            string log = "";
            CommonData.HIRATA.MDShowMsg obj = m_Object as CommonData.HIRATA.MDShowMsg;
            CommonData.HIRATA.Msg msg_item = obj.Msg;
            //CommonStaticData.PopForm(msg_item.Txt, msg_item.PAutoClean, msg_item.PUserRep, m_Ticket, msg_item.TimeOut);
            CommonStaticData.PopForm(msg_item.Txt, msg_item.PAutoClean, msg_item.PUserRep, 0, msg_item.TimeOut);
            WriteLog(LogLevelType.General, log);
            //WriteOut
        }

        #region process MMF Event
        void ProcessShowOcrDecide(string m_MessageId, object m_Object)
        {
            if (cv_OcrDecide != null)
            {
                if (cv_OcrDecide.Visible)
                {
                    cv_OcrDecide.Focus();
                }
                else
                {
                    cv_OcrDecide.Show();
                }
            }
            else
            {
                cv_OcrDecide = new OcrDecide();
                cv_OcrDecide.Show();
            }
        }
        void ProcessGlassCountData(string m_MessageId, object m_Object)
        {
            cv_StatusTable.GetUI().SetGlassCount();
        }
        void ProcessChangePortSlotType(string m_MessageId, object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CommonData.HIRATA.MDChangePortSlotType obj = m_Object as CommonData.HIRATA.MDChangePortSlotType;
            Port port = GetPort(obj.PPortId);
            int type = obj.PSlotType;
            if (obj.PResult == Result.NG)
            {
                CommonStaticData.PopForm("Can't change port slot type", true, false);
            }
            else
            {
                if (type == 0)
                {
                    if (type == port.cv_Data.PEfemPortType)
                    {
                        if (cv_PortContainer.ContainsKey(obj.PPortId))
                        {
                            port.cv_SlotCount = 25;
                            port.cv_Data.cv_SlotCount = 25;
                            (port.cv_Ui as GUI.PortUI).ResetDataVier(25);
                            (port.cv_Ui as GUI.PortUI).SetSlotButton(true);
                        }
                    }
                }
                else if (type == 4)
                {
                    if (type == port.cv_Data.PEfemPortType)
                    {
                        if (cv_PortContainer.ContainsKey(obj.PPortId))
                        {
                            port.cv_SlotCount = 13;
                            port.cv_Data.cv_SlotCount = 13;
                            (port.cv_Ui as GUI.PortUI).ResetDataVier(13);
                            (port.cv_Ui as GUI.PortUI).SetSlotButton(false);
                        }
                    }
                }
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        void ProcessRobotJobPathChange(string m_MessageId, object m_Object)
        {
            /*
            CommonData.HIRATA.MDRobotjobPath obj = m_Object as MDRobotjobPath;
            if (cv_RobotAutoJobPathTable != null)
            {
                cv_RobotAutoJobPathTable.Refresh(obj.RobotJob);
            }
            */
        }
        void ProcessTimeChartStepChange(string m_MessageId, object m_Object)
        {
            if (cv_TimeChartStep == null)
            {
                cv_TimeChartStep = new Dictionary<EqGifTimeChartId, KeyValuePair<int, string>>();
            }
            CommonData.HIRATA.MDTimeChartChange obj = m_Object as CommonData.HIRATA.MDTimeChartChange;
            for (int i = 0; i < obj.cv_Steps.Count; i++)
            {
                EqGifTimeChartId id = (EqGifTimeChartId)(i + 1);
                KeyValuePair<int, string> pair = new KeyValuePair<int, string>(obj.cv_Steps[i], obj.cv_StepsName[i]);
                cv_TimeChartStep[id] = pair;
            }
        }
        void ProcessEfemStatusSingle(string m_MessageId, object m_Object)
        {
            StatusTable table = cv_StatusTable.GetUI();
            if (table != null)
            {
                CommonData.HIRATA.MDEfemStatusSingle obj = m_Object as CommonData.HIRATA.MDEfemStatusSingle;
                table.ProcessEfemEventSingle(obj.PStatusType, obj.PValue);
            }
        }
        void ProcessEfemStatus(string m_MessageId, object m_Object)
        {
            StatusTable table = cv_StatusTable.GetUI();
            if (table != null)
            {
                CommonData.HIRATA.MDEfemStatus obj = m_Object as CommonData.HIRATA.MDEfemStatus;
                table.ProcessEfemEvent(obj);
            }
        }

        void ProcessTimeOutData(string m_MessageId, object m_Object)
        {
            cv_TimeOutForm.Set();
        }
        void ProcessSystemData(string m_MessageId, object m_Object)
        {

            //WriteNormalIn
            CommonData.HIRATA.SystemData obj = m_Object as CommonData.HIRATA.SystemData;
            string log = "Recv : " + typeof(CommonData.HIRATA.SystemData).Name + Environment.NewLine;
            log += "Bc Alive : " + obj.PBcAlive.ToString() + Environment.NewLine;
            log += "System Status : " + obj.PSystemStatus.ToString() + Environment.NewLine;
            log += "PLC connected : " + obj.PPlcConnect.ToString() + Environment.NewLine;
            log += "Operation Mode : " + obj.POperationMode.ToString() + Environment.NewLine;
            log += "api connect  : " + obj.PapiConnect.ToString() + Environment.NewLine;
            log += "Robot version  : " + obj.PapiVersion.ToString() + Environment.NewLine;
            log += "api remote  : " + obj.PapiInlineMode.ToString() + Environment.NewLine;
            log += "Data Check Rule  : " + obj.PDataCheckRule.ToString("X4") + Environment.NewLine;
            log += "OCR mode  : " + obj.POcrMode1.ToString() + Environment.NewLine;
            log += "Initialize OK  : " + obj.PInitaiizeOkRight.ToString() + Environment.NewLine;
            log += CommonData.HIRATA.CommonStaticData.g_SplitLine + Environment.NewLine;

            //cv_btnSelectMode.Enabled = true;
            Color tmp_color = Color.Gray;
            switch (obj.PSystemStatus)
            {
                case EquipmentStatus.Down:
                    lbl_systemStatus.Text = "DOWN";
                    lbl_systemStatus.BackColor = Color.Red;
                    break;
                case EquipmentStatus.Idle:
                    lbl_systemStatus.Text = "IDLE";
                    lbl_systemStatus.BackColor = Color.Yellow;
                    break;
                case EquipmentStatus.Run:
                    lbl_systemStatus.Text = "RUN";
                    lbl_systemStatus.BackColor = Color.Lime;
                    break;
            };

            if (obj.PSystemOnlineMode == OnlineMode.Offline || obj.PSystemOnlineMode == OnlineMode.None)
            {
                lbl_SystemMode.Text = "OFF";
                lbl_SystemMode.BackColor = Color.Red;
            }
            else
            {
                lbl_SystemMode.Text = "ON";
                lbl_SystemMode.BackColor = Color.Lime;
            }

            cv_RecipeCheckForm.SetNodeChecked();

            if (lbl_Ocr.Text != obj.POcrMode1.ToString())
            {
                lbl_Ocr.Text = obj.POcrMode1.ToString();
            }

            if (lbl_RobotInline.Text != obj.PapiInlineMode.ToString())
            {
                lbl_RobotInline.Text = obj.PapiInlineMode.ToString();
            }
            if (cv_lblVersion.Text != obj.PapiVersion)
            {
                cv_lblVersion.Text = obj.PapiVersion;
            }
            if (obj.PPlcConnect)
            {
                lbl_plcStatus.BackColor = Color.Lime;
                lbl_plcStatus.Text = "Connected";
            }
            else
            {
                lbl_plcStatus.BackColor = Color.Red;
                lbl_plcStatus.Text = "Disconnected";
            }
            if (obj.PBcAlive)
            {
                lbl_hsmsStatus.BackColor = Color.Lime;
                lbl_hsmsStatus.Text = "Connected";

            }
            else
            {
                lbl_hsmsStatus.BackColor = Color.Red;
                lbl_hsmsStatus.Text = "Disconnected";
            }
            if (obj.PapiConnect)
            {
                lbl_RobotConnect.Text = "Connect";
                lbl_RobotConnect.BackColor = Color.Lime;
            }
            else
            {
                lbl_RobotConnect.Text = "Dis";
                lbl_RobotConnect.BackColor = Color.Red;
            }
            //ont mode


            //Auto / manual button
            if (obj.POperationMode == OperationMode.Auto)
            {
                if (cv_SystemAuto.BackColor != Color.Lime)
                {
                    cv_SystemAuto.BackColor = Color.Lime;
                }
            }
            else
            {
                if (cv_SystemAuto.BackColor != Color.Gray)
                {
                    cv_SystemAuto.BackColor = Color.Gray;
                }
            }
            if (obj.POperationMode.ToString() != cv_SystemAuto.Text)
            {
                cv_SystemAuto.Text = obj.POperationMode.ToString().Trim();
            }

            if (obj.PRobot1Speed.ToString() != lbl_RobotSpeed.Text.Trim())
            {
                lbl_RobotSpeed.Text = obj.PRobot1Speed.ToString();
            }

            if (obj.PFFUSpeed.ToString() != lbl_FFUSpeed.Text.Trim())
            {
                lbl_FFUSpeed.Text = obj.PFFUSpeed.ToString();
            }




            //GetRobot(1).reFresh();
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }
        void ProcessOnlineReq(string m_MessageId, object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            string log = "";
            CommonData.HIRATA.MDOnlineRequest obj = m_Object as CommonData.HIRATA.MDOnlineRequest;
            log += "Cur Mode :" + obj.PCurMode.ToString() + " Change Mode : " + obj.PChangeMode.ToString() + Environment.NewLine;

            //au require online from tcs.
            if (obj.PType == MmfEventClientEventType.etRequest)
            {
                LockOnlineButton(true);
            }
            else if (obj.PType == MmfEventClientEventType.etNotify || obj.PType == MmfEventClientEventType.etReply)
            {
                LockOnlineButton(false);
            }
            WriteLog(LogLevelType.General, log);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void ProcessOperatorModeChange(string m_MessageId, object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CommonData.HIRATA.MDOperationModeChange obj = m_Object as CommonData.HIRATA.MDOperationModeChange;
            {
                LockOperationButton(false);
            }
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void ProcessLoginOut(string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            string log = "";
            CommonData.HIRATA.MDLogInOut obj = m_Object as CommonData.HIRATA.MDLogInOut;
            log += "Log in / out :" + obj.PAction.ToString() + Environment.NewLine;

            if (obj.PAction == LogInOut.Login)
            {
                AccountItem tmp = null;
                UiForm.cv_AccountData.GetCurAccount(out tmp);
                lbl_AccountId.Text = tmp.PId;
                lbl_AccountPm.Text = tmp.PPermission.ToString();
            }
            else
            {
                lbl_AccountPm.Text = "";
                lbl_AccountId.Text = "";
            }
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }
        void ProcessAlignerData(string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            string log = "";
            CommonData.HIRATA.AlignerData obj = m_Object as CommonData.HIRATA.AlignerData;
            UI.Aligner aligner = GetAligner((int)obj.PId);
            aligner.cv_Data = null;
            aligner.cv_Data = obj;
            aligner.cv_Data.GlassDataList = obj.cv_GlassDataList;
            aligner.reFresh();
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }
        void ProcessBufferData(string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            string log = "";
            CommonData.HIRATA.BufferData obj = m_Object as CommonData.HIRATA.BufferData;
            UI.Buffer buffer = GetBuffer((int)obj.PId);
            buffer.cv_Data = null;
            buffer.cv_Data = obj;
            buffer.cv_Data.GlassDataList = obj.cv_GlassDataList;
            buffer.reFresh();
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }
        void ProcessRobotData(string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            string log = "";
            CommonData.HIRATA.RobotData obj = m_Object as CommonData.HIRATA.RobotData;
            UI.Robot robot = GetRobot((int)obj.PId);
            robot.cv_Data = null;
            robot.cv_Data = obj;
            robot.cv_Data.GlassDataList = obj.cv_GlassDataList;
            robot.reFresh();
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }
        void ProcessPortData(string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            string log = "";
            CommonData.HIRATA.PortData obj = m_Object as CommonData.HIRATA.PortData;
            UI.Port port = GetPort((int)obj.PId);
            port.cv_Data = null;
            cv_SummaryPortData[(int)obj.PId - 1] = null;
            port.cv_Data = obj;
            port.cv_Data.GlassDataList = obj.cv_GlassDataList;
            port.reFresh();
            cv_SummaryPortData[(int)obj.PId - 1] = GetPort((int)obj.PId).cv_Data;
            cv_LotSummary.reFresh();
            (port.cv_Ui as GUI.PortUI).SetSlotButton(port.cv_Data.PEfemPortType == 0 ? true : false);
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }
        void ProcessEqData(string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            string log = "";
            CommonData.HIRATA.EqData obj = m_Object as CommonData.HIRATA.EqData;
            UI.Eq eq = GetEq((int)obj.PId);
            eq.cv_Data = null;
            eq.cv_Data = obj;
            eq.cv_Data.GlassDataList = obj.cv_GlassDataList;
            eq.reFresh();
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }
        void ProcessRecipeData(string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            CommonData.HIRATA.RecipeData obj = m_Object as CommonData.HIRATA.RecipeData;
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    cv_RecipeSetting.cv_RecipeDataView.DataSource = obj.cv_RecipeList;
                    cv_RecipeSetting.UpdateCurRecipeCombobox();
                    RecipeItem recipe = new RecipeItem();
                    if (obj.GetCurRecipe(out recipe))
                    {
                        if (lbl_CurRecipe.Text != recipe.PId.Trim())
                        {
                            lbl_CurRecipe.Text = recipe.PId.Trim();
                        }
                        if (lbl_CurFlow.Text != recipe.PFlow.ToString())
                        {
                            lbl_CurFlow.Text = recipe.PFlow.ToString();
                        }
                    }
                }));
            }
            else
            {
                cv_RecipeSetting.cv_RecipeDataView.DataSource = obj.cv_RecipeList;
                cv_RecipeSetting.UpdateCurRecipeCombobox();
                RecipeItem recipe = new RecipeItem();
                if (obj.GetCurRecipe(out recipe))
                {
                    if (lbl_CurRecipe.Text != recipe.PId.Trim())
                    {
                        lbl_CurRecipe.Text = recipe.PId.Trim();
                    }
                    if (lbl_CurFlow.Text != recipe.PFlow.ToString())
                    {
                        lbl_CurFlow.Text = recipe.PFlow.ToString();
                    }
                }
            }
            //WriteNormalOut
        }
        void ProcessAlarmNoti(string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    cv_AlarmDataView.DataSource = lgcBase.cv_Alarms.cv_AlarmList;
                }));
            }
            else
            {
                cv_AlarmDataView.DataSource = lgcBase.cv_Alarms.cv_AlarmList;
            }

            if (lgcBase.cv_Alarms.IsHasAlarm() && cv_tcBar.SelectedTab != cv_tpAlarm)
            {
                cv_tcBar.SelectedTab = cv_tpAlarm;
            }

            if (lgcBase.cv_Alarms.IsHasAlarm()) lbl_alarmStatus.BackColor = Color.Red;
            else
                lbl_alarmStatus.BackColor = SystemColors.ActiveCaption;
            if (lgcBase.cv_Alarms.IsHasWarning()) lbl_warningStatus.BackColor = Color.Red;
            else
                lbl_warningStatus.BackColor = SystemColors.ActiveCaption;

            //WriteNormalOut

        }
        void ProcessInit(string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            string log = "";
            CommonData.HIRATA.MDInitial obj = m_Object as CommonData.HIRATA.MDInitial;
            if (obj.PType == MmfEventClientEventType.etReply)
            {
                if (obj.PResult == Result.NG)
                {
                    btn_ReIni.Enabled = true;
                }
            }
            else if (obj.PType == MmfEventClientEventType.etNotify)
            {
                if (obj.PAction == InitialAction.Initial)
                {
                    btn_ReIni.Enabled = false;
                }
                else if (obj.PAction == InitialAction.Complete)
                {
                    btn_ReIni.Enabled = true;
                }
            }
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }
        void ProcessPopOpidFormReq(string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            string log = "";
            CommonData.HIRATA.MDPopOpidForm obj = new CommonData.HIRATA.MDPopOpidForm();

            if (obj.PortId > 0 && obj.PortId < CommonData.HIRATA.CommonStaticData.g_PortNumber)
            {
                Port tmp = UiForm.GetPort(obj.PortId);
                tmp.ShowMgvForm();
            }
            else
            {
                log += "Port over range" + Environment.NewLine;
            }
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }
        void ProcessPopMonitorFormReq(string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            string log = "";
            CommonData.HIRATA.MDPopMonitorForm obj = m_Object as CommonData.HIRATA.MDPopMonitorForm;
            if (obj.PortId > 0 && obj.PortId <= CommonData.HIRATA.CommonStaticData.g_PortNumber)
            {
                Port tmp = UiForm.GetPort(obj.PortId);
                tmp.ShowCstDataConfirm();
            }
            else
            {
                log += "Port over range" + Environment.NewLine;
            }
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }

        void PrcessRobotAction(string m_MessageId, object m_Object)
        { //WriteNormalIn 
            string log = ""; CommonData.HIRATA.MDRobotAction obj = m_Object as CommonData.HIRATA.MDRobotAction;
            log += "Robot RobotId : " + obj.RobotId + " Arm : " + obj.Source.PArm.ToString() + " Action : " + obj.PAction.ToString() +
                " Target : " + obj.Source.PTarget.ToString() + " TargetId : " + obj.Source.Id + " TargetSlot : " + obj.Source.Slot + Environment.NewLine;
            WriteLog(LogLevelType.General, log);
            if (obj.PType == MmfEventClientEventType.etNotify)
            {
                DisplayRobotAction(obj);
            }
            else if (obj.PType == MmfEventClientEventType.etReply)
            {
                if (obj.PResult == CommonData.HIRATA.Result.OK)
                {
                    DisplayRobotAction(obj);
                }
                else
                {
                    CommonStaticData.PopForm("Robot Action reply NG!", true, false);
                    log += "Robot Action reply NG";

                }
            }
            //WriteNormalOut
        }
        void DisplayRobotAction(MDRobotAction m_obj)
        {
            if (m_obj.PAction != RobotAction.ActionComplete)
            {
                lbl_RbActionAction.Text = m_obj.PAction.ToString();
                lbl_RbActionArm.Text = m_obj.Source.PArm.ToString().Substring(3);
                lbl_RbActionTar.Text = m_obj.Source.PTarget.ToString();
                lbl_RbActionId.Text = m_obj.Source.Id.ToString();
                lbl_RbActionSlot.Text = m_obj.Source.Slot.ToString();
            }
            else
            {
                lbl_RbActionAction.Text = "";
                lbl_RbActionArm.Text = "";
                lbl_RbActionTar.Text = "";
                lbl_RbActionId.Text = "";
                lbl_RbActionSlot.Text = "";
            }
        }
        #endregion

        #region process MMF Event by case
        void ProcessRobotApiVersionNoti(string m_ChannelId, string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            string log = "";
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }
        void ProcessDeviceBackHomeNoti(string m_ChannelId, string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            string log = "";
            CommonData.HIRATA.MDDeviceHome obj = m_Object as CommonData.HIRATA.MDDeviceHome;
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }
        #endregion
    }
}

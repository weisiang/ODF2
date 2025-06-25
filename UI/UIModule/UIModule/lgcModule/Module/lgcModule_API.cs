using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonData.HIRATA;
using System.Text.RegularExpressions;
using UI;
using KgsCommon;
using CommonData;
using BaseAp;
using System.Reflection;
using System.Diagnostics;

namespace LGC
{
    public partial class LgcModule
    {
        public static RBController cv_ApiController = null;

        public void initRbController()
        {
            string ip = CommonData.HIRATA.CommonStaticData.g_RobotXml.Attributes["IP"].Trim();
            int socket_port = Convert.ToInt32(CommonData.HIRATA.CommonStaticData.g_RobotXml.Attributes["Port"].Trim());
            if (cv_ApiController == null)
            {
                cv_ApiController = new RBController(ip, socket_port);
                cv_ApiController.Open();
            }
        }

        public static bool SetRobotTransferAction(CommandData m_Command)
        {
            bool rtn = false;
            if (cv_ApiController.Connected)
            {
                if (cv_ApiController.SendCommand(m_Command))
                {
                    rtn = true;
                }
            }
            return rtn;
        }
        public static bool SetRobotCommonAction(CommandData m_Command)
        {
            bool rtn = false;
            if (cv_ApiController.Connected)
            {
                try
                {
                    if (cv_ApiController.SendCommand(m_Command))
                    {
                        rtn = true;
                    }
                }
                catch (Exception e)
                {
                }
            }
            if (!rtn)
            {
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.SendApiComandError.ToString();
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PMainDescription = "Send API Comand Error ( API disconnected maybe)";
                alarm.PStatus = AlarmStatus.Occur;
                alarm.PUnit = 0;
                EditAlarm(alarm);
            }
            return rtn;
        }

        private delegate void DeleProcessCommand(CommandData m_Command);
        private Dictionary<APIEnum.CommandType, DeleProcessCommand> cv_ProcessCommandPtr = new Dictionary<APIEnum.CommandType, DeleProcessCommand>();
        private void LinkEvent()
        {
            cv_ApiController.OnRecvEvent += OnRecvCommandReply;
            cv_ApiController.OnRecvTimeOutEvent += OnRobotCommandTimeout;
            cv_ApiController.OnConnectEvent += OnConnect;
            cv_ApiController.OnSendErrorEvent += OnSendError;
            cv_ApiController.OnRecvParseError += OnReplyParseError;
        }
        private void AssignFunciton()
        {
            cv_ProcessCommandPtr.Add(APIEnum.CommandType.API, ProcessAPICommand);
            cv_ProcessCommandPtr.Add(APIEnum.CommandType.Common, ProcessCommonCommand);
            cv_ProcessCommandPtr.Add(APIEnum.CommandType.RFID, ProcessRFIDCommand);
            cv_ProcessCommandPtr.Add(APIEnum.CommandType.LoadPort, ProcessLoadPortCommand);
            //cv_ProcessCommandPtr.Add(APIEnum.CommandType.E84, ProcessE84Command);
            cv_ProcessCommandPtr.Add(APIEnum.CommandType.Robot, ProcessRobotCommand);
            cv_ProcessCommandPtr.Add(APIEnum.CommandType.Aligner, ProcessAlignerCommand);
            cv_ProcessCommandPtr.Add(APIEnum.CommandType.IO, ProcessIOCommand);
            //cv_ProcessCommandPtr.Add(APIEnum.CommandType.Alignment, ProcessAlignmentCommand);
            //cv_ProcessCommandPtr.Add(APIEnum.CommandType.Barcode, ProcessBarcodeCommand);
            cv_ProcessCommandPtr.Add(APIEnum.CommandType.OCR, ProcessOCRCommand);
            cv_ProcessCommandPtr.Add(APIEnum.CommandType.Event, ProcessEventCommand);
        }

        private static void ApiReplyAbnormal(CommandData m_Command)
        {
            string log = "[Process API Abnormal Reply] " + m_Command.GetCommandStr() + "\n";
            CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
            int rep_code = m_Command.cv_ReturnCode;
            string str_rep_code = "";
            List<AlarmItem> list = new List<AlarmItem>();
            bool is_find = false;
            if (rep_code < 10)
            {
                str_rep_code = rep_code.ToString().Trim().PadLeft(2, '0');
            }
            else
            {
                str_rep_code = rep_code.ToString().Trim();
            }
            if (cv_ApiAlarm.ContainsKey(str_rep_code))
            {
                list = cv_ApiAlarm[str_rep_code];
            }
            else
            {
                log += "API alarm table can't find the Big Code.\n";
                ShowMsg(log, true, false);
            }
            for (int i = 0; i < list.Count; i++)
            {
                AlarmItem tmp_alarm = list.ElementAt(i);
                if (tmp_alarm.PCommandDevice == m_Command.PCommandDevice)
                {
                    if (tmp_alarm.cv_ResCode.Trim() == m_Command.cv_ReplyParaList[0].Trim())
                    {
                        alarm = list.ElementAt(i);
                        is_find = true;
                        break;
                    }
                }
            }
            ShowMsg("Command Reply Abnormal : " + m_Command.GetCommandStr(), true, false);
            if (is_find)
            {
                log += "Fined the alarm in Alarm List and report";
                alarm.PStatus = AlarmStatus.Occur;
                EditAlarm(alarm);
            }
            else
            {
                log += "Can't fine the alarm in Alarm List";
                //LgcForm.ShowMsg(log, true, false);
            }
            log += "--------------------------------";
            WriteLog(LogLevelType.General, log, FunInOut.None);
            /*
            alarm.PCode = m_Command.GetAlarmCode().ToString();
            alarm.PLevel = AlarmLevele.Serious;
            //alarm.PMainDescription = "Command Reply Abnormal : " + m_Command.GetCommandStr();
            alarm.PMainDescription = "Reply Abnormal : " + m_Command.cv_ReturnCode + "," + m_Command.cv_ReplyParaList[0] + " , " + m_Command.cv_ReplyParaList[1];
            if(m_Command.cv_ReplyParaList.Count>2)
            {
                alarm.PMainDescription += "," + m_Command.cv_ReplyParaList[2];
            }
            alarm.PStatus = AlarmStatus.Occur;
            LgcForm.ShowMsg("Command Reply Abnormal : " + m_Command.GetCommandStr(), true, false);
            LgcForm.EditAlarm(alarm);
            */
        }

        #region On Event
        private void OnRecvCommandReply(CommandData m_Command)
        {
            if (m_Command.cv_ReturnCode != 0)
            {
                if (m_Command.PCommandType != APIEnum.CommandType.Event && m_Command.PEventCommand != APIEnum.EventCommand.ERROR)
                {
                    if ((!lgcBase.PSystemData.PInitaiizingRight) || (!lgcBase.PSystemData.PInitaiizingLeft))
                    {
                        ApiReplyAbnormal(m_Command);
                        return;
                    }
                    else
                    {
                        if ((m_Command.PCommandType == APIEnum.CommandType.Common && m_Command.PCommonCommand == APIEnum.CommonCommand.ResetError) ||
                            (m_Command.PCommandType == APIEnum.CommandType.Common && m_Command.PCommonCommand == APIEnum.CommonCommand.GetStatus) ||
                            (m_Command.PCommandType == APIEnum.CommandType.Common && m_Command.PCommonCommand == APIEnum.CommonCommand.Home))
                        {
                            SendinitCompleteFail(lgcBase.PSystemData.PWhichSideInInitilation);
                            ShowMsg("At Initial , Command :" + m_Command.PCommonCommand.ToString() + "failure!!! Please check and re-initilize", false, false);
                            ApiReplyAbnormal(m_Command);
                            return;
                        }
                        else if (m_Command.PCommandType == APIEnum.CommandType.API && m_Command.PApiCommand == APIEnum.APICommand.Remote)
                        {
                            SendinitCompleteFail(enSideGroup.Both);
                            ShowMsg("At Initial , Command :" + m_Command.PCommonCommand.ToString() + "failure!!! Please check and re-initilize", false, false);
                            ApiReplyAbnormal(m_Command);
                            return;
                        }
                    }
                }
            }
            if (m_Command.PCommandType == APIEnum.CommandType.API)
            {
                cv_ProcessCommandPtr[APIEnum.CommandType.API](m_Command);
            }
            else if (m_Command.PCommandType == APIEnum.CommandType.Common)
            {
                cv_ProcessCommandPtr[APIEnum.CommandType.Common](m_Command);
            }
            else if (m_Command.PCommandType == APIEnum.CommandType.RFID)
            {
                cv_ProcessCommandPtr[APIEnum.CommandType.RFID](m_Command);
            }
            else if (m_Command.PCommandType == APIEnum.CommandType.LoadPort)
            {
                cv_ProcessCommandPtr[APIEnum.CommandType.LoadPort](m_Command);
            }
            else if (m_Command.PCommandType == APIEnum.CommandType.E84)
            {
                cv_ProcessCommandPtr[APIEnum.CommandType.E84](m_Command);
            }
            else if (m_Command.PCommandType == APIEnum.CommandType.Robot)
            {
                cv_ProcessCommandPtr[APIEnum.CommandType.Robot](m_Command);
            }
            else if (m_Command.PCommandType == APIEnum.CommandType.Aligner)
            {
                cv_ProcessCommandPtr[APIEnum.CommandType.Aligner](m_Command);
            }
            else if (m_Command.PCommandType == APIEnum.CommandType.IO)
            {
                cv_ProcessCommandPtr[APIEnum.CommandType.IO](m_Command);
            }
            else if (m_Command.PCommandType == APIEnum.CommandType.Alignment)
            {
                cv_ProcessCommandPtr[APIEnum.CommandType.Alignment](m_Command);
            }
            else if (m_Command.PCommandType == APIEnum.CommandType.Barcode)
            {
                cv_ProcessCommandPtr[APIEnum.CommandType.Barcode](m_Command);
            }
            else if (m_Command.PCommandType == APIEnum.CommandType.OCR)
            {
                cv_ProcessCommandPtr[APIEnum.CommandType.OCR](m_Command);
            }
            else if (m_Command.PCommandType == APIEnum.CommandType.Event)
            {
                cv_ProcessCommandPtr[APIEnum.CommandType.Event](m_Command);
            }
        }
        private void OnSendError(string m_CommandTxt, string m_Msg)
        {
            CommonData.HIRATA.MDShowMsg obj = new MDShowMsg();
            CommonData.HIRATA.Msg msg_obj = new Msg();
            msg_obj.PAutoClean = true;
            msg_obj.PUserRep = false;
            msg_obj.TimeOut = 10000;
            msg_obj.Txt = "API Command Send ERROR : " + m_CommandTxt;
            obj.Msg = msg_obj;
            LGCController.triggerLgcEvent(typeof(CommonData.HIRATA.MDShowMsg).Name, obj);
            //BaseAp.Global.Controller.SendMmfNotifyObject(typeof(CommonData.HIRATA.MDShowMsg).Name, obj, KParseObjToXmlPropertyType.Field);

            CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
            alarm.PCode = CommonData.HIRATA.Alarmtable.SendApiComandError.ToString();
            alarm.PLevel = AlarmLevele.Serious;
            alarm.PMainDescription = "Send API Comand Error ( API disconnected maybe)";
            alarm.PStatus = AlarmStatus.Occur;
            alarm.PUnit = 0;
            EditAlarm(alarm);
        }
        private void OnConnect(bool m_Isconnect)
        {
            lgcBase.PSystemData.PapiConnect = m_Isconnect;
            WriteLog(LogLevelType.General, "Exe OnConnect : " + m_Isconnect.ToString());
            if (m_Isconnect)
            {
                SetApiCommonCommand(APIEnum.APICommand.CurrentMode);
            }
        }
        private void OnReplyParseError(string m_Txt)
        {
            CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
            alarm.PCode = Alarmtable.RobotApiReplyParseError.ToString();
            alarm.PMainDescription = "API Event/Reply parse error : " + m_Txt;
            alarm.PSubDescription = m_Txt;
            alarm.PUnit = 0;
            alarm.PLevel = AlarmLevele.Serious;
            alarm.PStatus = AlarmStatus.Occur;
            alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
            EditAlarm(alarm);
        }
        private void OnRobotCommandTimeout(CommandData m_Command)
        {
            AlarmItem alarm = new AlarmItem();
            alarm.PUnit = 0;
            alarm.PCode = CommonData.HIRATA.Alarmtable.SendApiComandT3TimeOut.ToString();
            alarm.PLevel = AlarmLevele.Serious;
            alarm.PMainDescription = "Send API Comand T3 TimeOut : " + m_Command.GetCommandStr();
            alarm.PStatus = AlarmStatus.Occur;
            EditAlarm(alarm);
            if (lgcBase.PSystemData.PIsInInitialation)
            {
                if ((m_Command.PCommandType == APIEnum.CommandType.Common && m_Command.PCommonCommand == APIEnum.CommonCommand.ResetError) ||
                    (m_Command.PCommandType == APIEnum.CommandType.Common && m_Command.PCommonCommand == APIEnum.CommonCommand.GetStatus) ||
                    (m_Command.PCommandType == APIEnum.CommandType.Common && m_Command.PCommonCommand == APIEnum.CommonCommand.Home) ||
                    (m_Command.PCommandType == APIEnum.CommandType.API && m_Command.PApiCommand == APIEnum.APICommand.Remote)
                    )
                {
                    SendinitCompleteFail(lgcBase.PSystemData.PWhichSideInInitilation);
                    ShowMsg("At Initial , Command :" + m_Command.PCommonCommand.ToString() + "failure!!! Please check and re-initilize", false, false);
                    ApiReplyAbnormal(m_Command);
                    return;
                }
            }
        }
        #endregion

        #region process each Robot command reply
        private void ProcessAPICommand(CommandData m_Command)
        {
            if (m_Command.PApiCommand == APIEnum.APICommand.Remote)
            {
                SetApiCommonCommand(APIEnum.APICommand.Version);
                lgcBase.PSystemData.PapiInlineMode = EquipmentInlineMode.Remote;
                if (lgcBase.PSystemData.PIsInInitialation)
                {
                    foreach (Robot rb in cv_RobotContainer.Values)
                    {
                        if (lgcBase.PSystemData.PWhichSideInInitilation == rb.PSideGroup || lgcBase.PSystemData.PWhichSideInInitilation == enSideGroup.Both)
                        {
                            SetErrorReset(APIEnum.CommnadDevice.Robot, rb.cv_Id);
                        }
                    }
                }
            }
            else if (m_Command.PApiCommand == APIEnum.APICommand.Local)
            {
                lgcBase.PSystemData.PapiInlineMode = EquipmentInlineMode.Local;
            }
            else if (m_Command.PApiCommand == APIEnum.APICommand.Version)
            {
                lgcBase.PSystemData.PapiVersion = m_Command.cv_ReplyParaList[0];
            }
            else if (m_Command.PApiCommand == APIEnum.APICommand.CurrentMode)
            {
                if (Regex.Match(m_Command.cv_ReplyParaList[0], @"remote", RegexOptions.IgnoreCase).Success)
                {
                    lgcBase.PSystemData.PapiInlineMode = EquipmentInlineMode.Remote;
                }
                else
                {
                    lgcBase.PSystemData.PapiInlineMode = EquipmentInlineMode.Local;
                }
            }
        }
        private void ProcessCommonCommand(CommandData m_Command)
        {
            bool ok = true;
            if (m_Command.PCommonCommand == APIEnum.CommonCommand.Home)
            {
                if (m_Command.PCommandDevice == APIEnum.CommnadDevice.Robot)
                {
                    ok = ProcessRobotHome(m_Command);
                }
                else if (m_Command.PCommandDevice == APIEnum.CommnadDevice.P)
                {
                    ok = ProcessPortHome(m_Command);
                }
                else if (m_Command.PCommandDevice == APIEnum.CommnadDevice.Aligner)
                {
                    ok = ProcessAlignerHome(m_Command);
                }
            }
            else if (m_Command.PCommonCommand == APIEnum.CommonCommand.ResetError)
            {
                if (m_Command.PCommandDevice == APIEnum.CommnadDevice.Robot)
                {
                    ok = ProcessResetError(m_Command.PCommandDevice, m_Command);
                }
                else if (m_Command.PCommandDevice == APIEnum.CommnadDevice.P)
                {
                    ok = ProcessResetError(m_Command.PCommandDevice, m_Command);
                }
                else if (m_Command.PCommandDevice == APIEnum.CommnadDevice.Aligner)
                {
                    ok = ProcessResetError(m_Command.PCommandDevice, m_Command);
                }
            }
            else if (m_Command.PCommonCommand == APIEnum.CommonCommand.GetStatus)
            {
                if (m_Command.PCommandDevice == CommonData.HIRATA.APIEnum.CommnadDevice.Robot)
                {
                    ok = ProcessRobotStatus(m_Command);
                }
                else if (m_Command.PCommandDevice == CommonData.HIRATA.APIEnum.CommnadDevice.Aligner)
                {
                    ok = ProcessAlignerStatus(m_Command);
                }
                else if (m_Command.PCommandDevice == CommonData.HIRATA.APIEnum.CommnadDevice.P)
                {
                    ok = ProcessPortStatus(m_Command);
                }
                else if (m_Command.PCommandDevice == CommonData.HIRATA.APIEnum.CommnadDevice.EFEM)
                {
                    ok = ProcessEventStatus(m_Command);
                }
            }
        }
        private void ProcessRFIDCommand(CommandData m_Command)
        {
            if (m_Command.PRfidCommand == APIEnum.RfidCommand.ReadFoupID)
            {
                Port job_port = LgcModule.GetPortById(m_Command.cv_DeviceId);
                if (job_port.PLotStatus == LotStatus.FoupSensorOn)
                {
                    if (m_Command.cv_ReplyParaList[0].Trim() == "r")
                    {
                        ShowMsg("Command Reply Abnormal (RIFD read error ) : " + m_Command.GetCommandStr(), true, false);
                    }
                    else
                    {
                        if (job_port.PPortStatus == PortStaus.LDCM && job_port.PLotStatus == LotStatus.FoupSensorOn)
                        {
                            job_port.cv_Data.PLotId = m_Command.cv_ReplyParaList[0];
                            SetGetMappingData(m_Command.cv_DeviceId);
                            job_port.SendDataViaMmf();
                        }
                    }
                }
            }
        }
        private void ProcessLoadPortCommand(CommandData m_Command)
        {
            if (m_Command.PLoadPortCommand == APIEnum.LoadPortCommand.Load)
            {
                Port job_port = LgcModule.GetPortById(m_Command.cv_DeviceId);
                if (job_port.PPortStatus != PortStaus.LDCM)
                {
                    job_port.PPortStatus = PortStaus.LDCM;
                }
                if ((lgcBase.PSystemData.PWhichSideInInitilation == job_port.PSideGroup) || (lgcBase.PSystemData.PWhichSideInInitilation == enSideGroup.Both))
                {
                    SetGetMappingData(m_Command.cv_DeviceId);
                }
                else
                {
                    if (job_port.PLotStatus == LotStatus.FoupSensorOn)
                    {
                        job_port.cv_Data.Clear();
                        job_port.PClamp = PortClamp.Clamp;
                        SetReadRFIDRead(m_Command.cv_DeviceId);
                    }
                }
            }
            else if (m_Command.PLoadPortCommand == APIEnum.LoadPortCommand.GetWaferSlot2)
            {
                Port job_port = GetPortById(m_Command.cv_DeviceId);
                //if (job_port.PLotStatus == LotStatus.FoupSensorOn)
                {
                    int work_count = 0;
                    foreach (int key in job_port.cv_Data.GlassDataMap.Keys)
                    {
                        if (key == 0) continue;
                        if (key <= job_port.cv_Data.cv_SlotCount)
                        {
                            if (m_Command.cv_ReplyParaList[key - 1] != "1" && m_Command.cv_ReplyParaList[key - 1] != "0")
                            {
                                ShowMsg("Command reply error (mapping data abnormal) : " +
                                    string.Join(",", m_Command.cv_ReplyParaList.ToString()), true, false);
                                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                                alarm.PCode = Alarmtable.MappingDataError.ToString();
                                alarm.PMainDescription = "Mapping Data Error";
                                alarm.PSubDescription = string.Join(",", m_Command.cv_ReplyParaList);//m_Command.cv_ReplyParaList.ToString();
                                alarm.PUnit = 0;
                                alarm.PLevel = AlarmLevele.Serious;
                                alarm.PStatus = AlarmStatus.Occur;
                                alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
                                EditAlarm(alarm);
                                return;
                            }
                            if (m_Command.cv_ReplyParaList[key - 1] == "1")
                            {
                                job_port.cv_Data.GlassDataMap[key].PHasSensor = true;
                                work_count++;
                            }
                            else
                            {
                                job_port.cv_Data.GlassDataMap[key].PHasSensor = false;
                            }
                        }
                        else
                        {
                            job_port.cv_Data.GlassDataMap[key].PHasSensor = false;
                        }
                    }
                    job_port.cv_Data.PWorkCount = (uint)work_count;
                    if (!string.IsNullOrEmpty(job_port.cv_Data.PLotId))
                    {
                        if (job_port.PLotStatus == LotStatus.FoupSensorOn)
                        {
                            job_port.PLotStatus = LotStatus.MappingEnd;
                            job_port.PPortStatus = PortStaus.LDCM;
                            SetLoadUnloadLed(true, SignalTowerControl.On, m_Command.cv_DeviceId);
                        }
                        else
                        {
                            if ((lgcBase.PSystemData.PWhichSideInInitilation == job_port.PSideGroup) || (lgcBase.PSystemData.PWhichSideInInitilation == enSideGroup.Both))
                            {
                                job_port.PIsRemapping = true;
                                GetPortById(job_port.cv_Id).PIsRemapping = true;
                                SetStatus(APIEnum.CommnadDevice.P, job_port.cv_Id);
                            }
                        }
                    }
                }
            }
            if (m_Command.PLoadPortCommand == APIEnum.LoadPortCommand.Unload)
            {
                Port job_port = GetPortById(m_Command.cv_DeviceId);
                job_port.PClamp = CommonData.HIRATA.PortClamp.Unclamp;
                if (job_port.PPortStatus != PortStaus.UDRQ)
                {
                    job_port.PPortStatus = PortStaus.UDRQ;
                    SetLoadUnloadLed(false, SignalTowerControl.On, m_Command.cv_DeviceId);
                }
                RemovePortToProcessList(m_Command.cv_DeviceId);
                if (lgcBase.PSystemData.PONT)
                {
                    job_port.PLotStatus = LotStatus.FoupSensorOn;
                    job_port.cv_Data.PPortStatus = PortStaus.LDCM;
                    SetPortLoadAction(m_Command.cv_DeviceId);
                }
            }
        }
        private void ProcessE84Command(CommandData m_Command)
        {
        }
        private void ProcessRobotCommand(CommandData m_Command)
        {
            Robot rb = GetRobotById(m_Command.cv_DeviceId);
            RobotJob job = rb.CurJob;
            if (m_Command.PRobotCommand == APIEnum.RobotCommand.WaferGet)
            {
                if (job.PAction == RobotAction.Get)
                {
                    rb.ProcessRobotGet(m_Command, job);
                }
            }

            else if (m_Command.PRobotCommand == APIEnum.RobotCommand.WaferPut)
            {
                if (job.PAction == RobotAction.Put)
                {
                    rb.ProcessRobotPut(m_Command, job);
                }
            }
            else if (m_Command.PRobotCommand == APIEnum.RobotCommand.GetStandby)
            {
                if (job.PAction == RobotAction.GetWait)
                {
                    rb.ProcessRobotGetWait(m_Command, job);
                }
            }
            else if (m_Command.PRobotCommand == APIEnum.RobotCommand.PutStandby)
            {
                if (job.PAction == RobotAction.PutWait)
                {
                    rb.ProcessRobotPutWait(m_Command, job);
                }
            }
            else if (m_Command.PRobotCommand == APIEnum.RobotCommand.TopWaferGet)
            {
                if (job.PAction == RobotAction.TopGet)
                {
                    rb.ProcessRobotTopGet(m_Command, job);
                }
            }
            else if (m_Command.PRobotCommand == APIEnum.RobotCommand.TopWaferPut)
            {
                if (job.PAction == RobotAction.TopPut)
                {
                    rb.ProcessRobotTopPut(m_Command, job);
                }
            }
            else if (m_Command.PRobotCommand == APIEnum.RobotCommand.TopGetStandby)
            {
                if (job.PAction == RobotAction.TopGetWait)
                {
                    rb.ProcessRobotTopGetWait(m_Command, job);
                }
            }

            else if (m_Command.PRobotCommand == APIEnum.RobotCommand.TopPutStandby)
            {
                if (job.PAction == RobotAction.TopPutWait)
                {
                    rb.ProcessRobotTopPutWait(m_Command, job);
                }
            }
            else if (m_Command.PRobotCommand == APIEnum.RobotCommand.TopPutStandbyArmExtend)
            {
                if (job.PAction == RobotAction.TopPutStandbyArmExtend)
                {
                    rb.ProcessRobotTopPutExtend(m_Command, job);
                }
            }
            else if (m_Command.PRobotCommand == APIEnum.RobotCommand.PutStandbyArmExtend)
            {
                if (job.PAction == RobotAction.PutStandbyArmExtend)
                {
                    rb.ProcessRobotPutStandbyArmExtend(m_Command, job);
                }
            }
            else if (m_Command.PRobotCommand == APIEnum.RobotCommand.GetStandbyArmExtend)
            {
                if (job.PAction == RobotAction.GetStandbyArmExtend)
                {
                    rb.ProcessRobotGetStandbyArmExtend(m_Command, job);
                }
            }
            else if (m_Command.PRobotCommand == APIEnum.RobotCommand.Stop)
            {
                rb.ProcessRobotStop(m_Command);
            }
            else if (m_Command.PRobotCommand == APIEnum.RobotCommand.ReStart)
            {
                //if (!cv_HadInit && cv_Initilizing)
                if ((lgcBase.PSystemData.PWhichSideInInitilation == rb.PSideGroup) || (lgcBase.PSystemData.PWhichSideInInitilation == enSideGroup.Both))
                {
                    SetHome(APIEnum.CommnadDevice.Robot, rb.cv_Id);
                }
            }
            else if (m_Command.PRobotCommand == APIEnum.RobotCommand.SetRobotSpeed)
            {
                if (m_Command.cv_DeviceId == 1)
                {
                    lgcBase.PSystemData.PRobot1Speed = rb.cv_WaitRobotSpeed;
                }
                else if (m_Command.cv_DeviceId == 2)
                {
                    lgcBase.PSystemData.PRobot2Speed = rb.cv_WaitRobotSpeed;
                }
                if (lgcBase.PSystemData.PIsInInitialation)
                {
                    SetSetFFUVoltage(lgcBase.PSystemData.PFFUSpeed);
                    cv_WaitFfuSpeed = lgcBase.PSystemData.PFFUSpeed;
                }
            }
            /*
            CommonData.HIRATA.MDRobotAction obj = new CommonData.HIRATA.MDRobotAction();
            obj.PAction = CommonData.HIRATA.RobotAction.ActionComplete;
            LgcForm.cv_MmfController.SendRobotAction(obj, MmfEventClientEventType.etNotify, false);
            */
        }
        private void ProcessAlignerCommand(CommandData m_Command)
        {
            Aligner aligner = GetAlignerById(m_Command.cv_DeviceId);
            if (m_Command.PAlignerCommand == APIEnum.AlignerCommand.AlignerVacuum)
            {
                if (!aligner.cv_Data.GlassDataMap[1].PHasData && !aligner.cv_Data.GlassDataMap[1].PHasSensor)
                {
                    if (aligner.cv_Data.PPreAction == AlignerPreAction.VuccumOff1)
                    {
                        aligner.cv_Data.PPreAction = AlignerPreAction.PutAligner;
                    }
                }
                else if (aligner.cv_Data.GlassDataMap[1].PHasData && aligner.cv_Data.GlassDataMap[1].PHasSensor)
                {
                    if (aligner.cv_Data.PPreAction == AlignerPreAction.WaitVuccumOn)
                    {
                        aligner.cv_Data.PPreAction = AlignerPreAction.FindNotch;
                    }
                    else if (aligner.cv_Data.PPreAction == AlignerPreAction.WaitVuccomOff2)
                    {
                        aligner.cv_Data.PPreAction = AlignerPreAction.GetAligner;
                    }
                }
            }
            else if (m_Command.PAlignerCommand == APIEnum.AlignerCommand.Alignment)
            {
                if (aligner.cv_Data.GlassDataMap[1].PIsWaitOcr)
                {
                    SetOcrRead(aligner.cv_Id);
                }
            }
            else if (m_Command.PAlignerCommand == APIEnum.AlignerCommand.SetAlignerDegree)
            {
                if (aligner.cv_Data.PPreAction == AlignerPreAction.VuccumOff1)
                {
                    SetAlignerVaccum(false, aligner.cv_Id);
                }
            }
            else if (m_Command.PAlignerCommand == APIEnum.AlignerCommand.ToAngle)
            {
                if (aligner.cv_Data.PPreAction == AlignerPreAction.WaitToAngle)
                {
                    aligner.cv_Data.PPreAction = AlignerPreAction.VuccumOff2;
                }
            }
            else if (m_Command.PAlignerCommand == APIEnum.AlignerCommand.FindNotch)
            {
                if (aligner.cv_Data.PPreAction == AlignerPreAction.WaitFindNotch)
                {
                    aligner.cv_Data.PPreAction = AlignerPreAction.OcrConnect;
                }
            }
        }
        private void ProcessIOCommand(CommandData m_Command)
        {
            if (m_Command.PIoCommand == APIEnum.IoCommand.GetBufferStatus)
            {
                ProcessBufferStatus(m_Command);
            }
            else if (m_Command.PIoCommand == APIEnum.IoCommand.SignalTower)
            {
                Robot robot = GetRobotById(1);
                if (robot.cv_TowerJobQ.Count > 0)
                {
                    robot.cv_TowerJobQ.Dequeue();
                }
            }
            else if (m_Command.PIoCommand == APIEnum.IoCommand.SetFFUVoltage)
            {
                lgcBase.PSystemData.PFFUSpeed = cv_WaitFfuSpeed;
                if (lgcBase.PSystemData.PIsInInitialation)
                {
                    SendinitComplete(lgcBase.PSystemData.PWhichSideInInitilation);
                }
            }
        }
        private void ProcessOCRCommand(CommandData m_Command)
        {
            Aligner aligner = GetAlignerById(m_Command.cv_DeviceId);
            if (m_Command.POcrCommand == CommonData.HIRATA.APIEnum.OcrCommand.Connect)
            {
                if (aligner.cv_Data.GlassDataMap[1].PHasData &&
                    aligner.cv_Data.GlassDataMap[1].PHasSensor)
                {
                    if (aligner.cv_Data.PPreAction == AlignerPreAction.WaitConnect)
                    {
                        aligner.cv_Data.PPreAction = AlignerPreAction.ReadOcr;
                    }
                }
            }
            else if (m_Command.POcrCommand == CommonData.HIRATA.APIEnum.OcrCommand.Read)
            {
                if (aligner.cv_Data.GlassDataMap[1].PHasData &&
                    aligner.cv_Data.GlassDataMap[1].PHasSensor)
                {
                    if (m_Command.cv_ReplyParaList[0] != "r")
                    {
                        if (aligner.cv_Data.GlassDataMap[1].PId != m_Command.cv_ReplyParaList[0])
                        {
                            g_eventController.SendBcOcrReport(aligner.cv_Data.GlassDataMap[1], m_Command.cv_ReplyParaList[0].Trim());
                            aligner.cv_Data.GlassDataMap[1].PId = m_Command.cv_ReplyParaList[0];
                            aligner.cv_Data.GlassDataMap[1].POcrResult = OCRResult.Mismatch;
                            CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                            alarm.PCode = Alarmtable.OcrReadError.ToString();
                            alarm.PMainDescription = "Ocr Read Error";
                            alarm.PSubDescription = string.Join(",", m_Command.cv_ReplyParaList);//m_Command.cv_ReplyParaList.ToString();
                            alarm.PUnit = 0;
                            alarm.PLevel = AlarmLevele.Light;
                            alarm.PStatus = AlarmStatus.Occur;
                            alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
                            EditAlarm(alarm);
                            ShowMsg("OCR read Error!!!", true, false);
                            //return;
                            //report BC ocr read.
                            g_eventController.SendWorkDataUpdateReport(aligner.cv_Data.GlassDataMap[1]);

                            if (lgcBase.PSystemData.POperationModeLeft == OperationMode.Auto && lgcBase.PSystemData.POcrMode1 == OCRMode.ErrorHold)
                            {
                                g_eventController.SendShowOcrDecide();
                            }
                        }
                        else
                        {
                            aligner.cv_Data.GlassDataMap[1].POcrResult = OCRResult.OK;
                            g_eventController.SendBcOcrReport(aligner.cv_Data.GlassDataMap[1], m_Command.cv_ReplyParaList[0].Trim());
                        }

                        if (aligner.cv_Data.PPreAction == AlignerPreAction.WaitReadOct)
                        {
                            aligner.cv_Data.PPreAction = AlignerPreAction.ToAngle;
                        }
                        aligner.SendDataViaMmf();
                    }
                    else
                    {
                        aligner.cv_Data.GlassDataMap[1].POcrResult = OCRResult.Fail;
                        CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                        alarm.PCode = Alarmtable.OcrReadError.ToString();
                        alarm.PMainDescription = "Ocr Read Error";
                        alarm.PSubDescription = string.Join(",", m_Command.cv_ReplyParaList);//m_Command.cv_ReplyParaList.ToString();
                        alarm.PUnit = 0;
                        alarm.PLevel = AlarmLevele.Light;
                        alarm.PStatus = AlarmStatus.Occur;
                        alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
                        EditAlarm(alarm);
                    }
                }
            }
        }
        private void ProcessEventCommand(CommandData m_Command)
        {
            if (m_Command.PEventCommand == APIEnum.EventCommand.FoupPlace)
            {
                Port job_port = GetPortById(m_Command.cv_DeviceId);
                if (job_port.PPortStatus != PortStaus.LDRQ)
                {
                    job_port.PPortStatus = PortStaus.LDRQ;
                }
                job_port.PLotStatus = LotStatus.FoupSensorOn;
                job_port.cv_Data.PPortHasCst = PortHasCst.Has;
                job_port.SendDataViaMmf();
            }
            else if (m_Command.PEventCommand == APIEnum.EventCommand.FoupRemove)
            {
                Port job_port = GetPortById(m_Command.cv_DeviceId);
                job_port.cv_Data.PPortStatus = PortStaus.UDCM;
                job_port.cv_Data.PLotStatus = LotStatus.Empty;
                job_port.PLDRQTime = SysUtils.Now();
                job_port.cv_Data.PPortHasCst = PortHasCst.Empty;
                //SysUtils.Sleep(1000);
                job_port.cv_Data.ClearNotIncludeFoupId();
                //job_port.cv_Data.Clear();
                //job_port.cv_Data.PPortStatus = PortStaus.LDRQ;
                job_port.SendDataViaMmf();
                SetLoadUnloadLed(false, SignalTowerControl.Off, m_Command.cv_DeviceId);
            }
            else if (m_Command.PEventCommand == APIEnum.EventCommand.ERROR)
            {
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = Alarmtable.RobotApiErrorEvent.ToString();
                alarm.PMainDescription = "Robot_API_ERROR_EVENT";
                alarm.PSubDescription = string.Join(",", m_Command.cv_ReplyParaList);//m_Command.cv_ReplyParaList.ToString();
                alarm.PUnit = 0;
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PStatus = AlarmStatus.Occur;
                alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
                EditAlarm(alarm);
                //BaseForm.PSystemData.POperationModeLeft = OperationMode.Manual;
            }
            else if (m_Command.PEventCommand == APIEnum.EventCommand.FoupPresence)
            {

            }
            else if (m_Command.PEventCommand == APIEnum.EventCommand.OperatorAccessButtonClick)
            {
                Port job_port = LgcModule.GetPortById(m_Command.cv_DeviceId);
                if (job_port.cv_Data.PPortHasCst == PortHasCst.Has)
                {
                    job_port.PLotStatus = LotStatus.FoupSensorOn;
                    job_port.PPortStatus = PortStaus.LDRQ;
                    job_port.cv_Data.PWaitUnload = false;
                    SetPortLoadAction(m_Command.cv_DeviceId);
                }
            }
            else if (m_Command.PEventCommand == APIEnum.EventCommand.OperatorAccessButton2Click)
            {

            }
            else if (m_Command.PEventCommand == APIEnum.EventCommand.VasTopPutEnd)
            {
                ProcessRobotOutVas(m_Command);
            }
            else if (m_Command.PEventCommand == APIEnum.EventCommand.GetStatus)
            {
                /* i'm not sure this status event can use in ODF2.
                Robot robot = GetRobotById(1);
                WriteLog(LogLevelType.General, "[Recv] Robot Sensor event S", FunInOut.None);
                if (m_Command.cv_ReplyParaList[1].Trim() == "1")
                {
                    cv_Data.GlassDataMap[(int)RobotArm.rbaDown].PHasSensor = true;
                    //                    LgcForm.WriteLog(LogLevelType.General, "[Recv] Robot Sensor event", FunInOut.None);
                }
                else if (m_Command.cv_ReplyParaList[1].Trim() == "0")
                {
                    cv_Data.GlassDataMap[(int)RobotArm.rbaDown].PHasSensor = false;
                    //                  LgcForm.WriteLog(LogLevelType.General, "[Recv] Robot Sensor event", FunInOut.None);
                }
                else
                {
                    LgcForm.ShowMsg("Command Robot status reply error : " + m_Command.cv_ReplyParaList.ToString(), true, false);
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiRobotStatusError.ToString();
                    alarm.PLevel = AlarmLevele.Serious;
                    alarm.PMainDescription = "RobotApi Robot Status Error";
                    alarm.PUnit = 0;
                    alarm.PStatus = AlarmStatus.Occur;
                    LgcForm.EditAlarm(alarm);
                }

                if (m_Command.cv_ReplyParaList[2].Trim() == "1")
                {
                    cv_Data.GlassDataMap[(int)RobotArm.rbaUp].PHasSensor = true;
                    //                LgcForm.WriteLog(LogLevelType.General, "[Recv] Robot Sensor event", FunInOut.None);
                }

                else if (m_Command.cv_ReplyParaList[2].Trim() == "0")
                {
                    cv_Data.GlassDataMap[(int)RobotArm.rbaUp].PHasSensor = false;
                    //              LgcForm.WriteLog(LogLevelType.General, "[Recv] Robot Sensor event", FunInOut.None);
                }
                else
                {
                    LgcForm.ShowMsg("Command Robot status reply error : " + m_Command.cv_ReplyParaList.ToString(), true, false);
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiRobotStatusError.ToString();
                    alarm.PLevel = AlarmLevele.Serious;
                    alarm.PMainDescription = "RobotApi Robot Status Error";
                    alarm.PUnit = 0;
                    alarm.PStatus = AlarmStatus.Occur;
                    LgcForm.EditAlarm(alarm);
                }
                robot.SendDataViaMmf();
                robot.cv_Data.SaveToFile();
                LgcForm.WriteLog(LogLevelType.General, "[Recv] Robot Sensor event E", FunInOut.None);
                */
            }
            else if ((int)m_Command.PEventCommand >= (int)APIEnum.EventCommand.Pressure &&
                 (int)m_Command.PEventCommand <= (int)APIEnum.EventCommand.GetStatus)
            {
                CommonData.HIRATA.MDEfemStatusSingle obj = new MDEfemStatusSingle();
                obj.PStatusType = m_Command.PEventCommand;
                obj.PValue = Convert.ToInt16(m_Command.cv_ReplyParaList[1].Trim());
                /*
                if (obj.PStatusType != APIEnum.EventCommand.RobotEnable)
                {
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiBufferStatusError.ToString();
                    alarm.PMainDescription = "Robot API EFEM Status Error : " + m_Command.PEventCommand.ToString();
                    alarm.PUnit = 0;
                    alarm.PLevel = AlarmLevele.Serious;
                    alarm.PStatus = AlarmStatus.Occur;
                    alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
                    EditAlarm(alarm);
                }
                */
                LGCController.triggerLgcEvent(typeof(CommonData.HIRATA.MDEfemStatusSingle).Name, obj);
            }
        }
        private void ProcessAlignmentCommand(CommandData m_Command)
        {
        }
        private void ProcessBarcodeCommand(CommandData m_Command)
        {
        }
        #endregion

        #region HOME
        private bool ProcessRobotHome(CommandData m_Command)
        {
            bool rtn = false;
            Robot rb = GetRobotById(m_Command.cv_DeviceId);
            if (rb != null)
            {
                rb.PIsHome = true;
                //if (!cv_HadInit && cv_Initilizing)
                if ((lgcBase.PSystemData.PWhichSideInInitilation == rb.PSideGroup) || (lgcBase.PSystemData.PWhichSideInInitilation == enSideGroup.Both))
                {
                    if (!rb.PIsSensorUnmatch)
                    {
                        SetAllPortStatus();
                    }
                    else
                    {
                        SendinitCompleteFail(lgcBase.PSystemData.PWhichSideInInitilation);
                    }
                }
                rtn = true;
            }
            return rtn;
        }
        private bool ProcessAlignerHome(CommandData m_Command)
        {
            bool rtn = false;
            Aligner aligner = LgcModule.GetAlignerById(m_Command.cv_DeviceId);
            aligner.PIsHome = true;
            if ((lgcBase.PSystemData.PWhichSideInInitilation == aligner.PSideGroup) || (lgcBase.PSystemData.PWhichSideInInitilation == enSideGroup.Both))
            {
                foreach (Buffer bf in cv_BufferContainer.Values)
                {
                    if ((bf.PSideGroup == lgcBase.PSystemData.PWhichSideInInitilation) || (lgcBase.PSystemData.PWhichSideInInitilation == enSideGroup.Both))
                    {
                        SetStatus(APIEnum.CommnadDevice.Buffer, bf.cv_Id);
                    }
                }
            }
            else if (aligner.cv_Data.PPreAction == AlignerPreAction.WaitHome)
            {
                aligner.cv_Data.PPreAction = AlignerPreAction.SetToAngle;
            }
            return rtn;
        }
        private bool ProcessPortHome(CommandData m_Command)
        {
            bool rtn = false;
            Port port = GetPortById(m_Command.cv_DeviceId);
            port.PIsHome = true;
            if (port.PLotStatus == LotStatus.Process)
            {
                port.PLotStatus = LotStatus.Abort;
            }
            else if ((port.PLotStatus != LotStatus.Cancel) && (port.PLotStatus != LotStatus.Abort) && (port.PLotStatus != LotStatus.ProcessEnd))
            {
                port.PLotStatus = LotStatus.Cancel;
            }
            port.PPortStatus = PortStaus.UDRQ;
            port.PClamp = PortClamp.Unclamp;
            port.cv_Data.SaveToFile();
            if ((lgcBase.PSystemData.PWhichSideInInitilation == port.PSideGroup) || (lgcBase.PSystemData.PWhichSideInInitilation == enSideGroup.Both))
            {
                if (CheckAllPortHome())
                {
                    foreach (Aligner al in cv_AlignerContainer.Values)
                    {
                        if ((lgcBase.PSystemData.PWhichSideInInitilation == al.PSideGroup) || (lgcBase.PSystemData.PWhichSideInInitilation == enSideGroup.Both))
                        {
                            SetStatus(APIEnum.CommnadDevice.Aligner, al.cv_Id);
                        }
                    }
                }
            }
            return rtn;
        }
        #endregion

        #region Reset Error
        private bool ProcessResetError(APIEnum.CommnadDevice m_Device, CommandData m_Command)
        {
            bool rtn = true;
            switch (m_Device)
            {
                case APIEnum.CommnadDevice.Robot:
                    Robot rb = GetRobotById(m_Command.cv_DeviceId);
                    rb.PIsResetError = true;

                    if ((lgcBase.PSystemData.PWhichSideInInitilation == rb.PSideGroup) || (lgcBase.PSystemData.PWhichSideInInitilation == enSideGroup.Both))
                    {
                        for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
                        {
                            Port port1 = GetPortById(i);
                            if ((lgcBase.PSystemData.PWhichSideInInitilation == port1.PSideGroup) || (lgcBase.PSystemData.PWhichSideInInitilation == enSideGroup.Both))
                            {
                                SetErrorReset(APIEnum.CommnadDevice.P, i);
                            }
                        }
                    }
                    break;
                case APIEnum.CommnadDevice.P:
                    Port port = GetPortById(m_Command.cv_DeviceId);
                    port.PIsResetError = true;
                    if ((lgcBase.PSystemData.PWhichSideInInitilation == port.PSideGroup) || (lgcBase.PSystemData.PWhichSideInInitilation == enSideGroup.Both))
                    {
                        if (CheckAllPortResetError())
                        {
                            foreach (Aligner al1 in cv_AlignerContainer.Values)
                            {
                                if ((lgcBase.PSystemData.PWhichSideInInitilation == al1.PSideGroup) || (lgcBase.PSystemData.PWhichSideInInitilation == enSideGroup.Both))
                                {
                                    SetErrorReset(APIEnum.CommnadDevice.Aligner, 1);
                                }
                            }
                        }
                    }
                    break;
                case APIEnum.CommnadDevice.Aligner:
                    Aligner al = GetAlignerById(m_Command.cv_DeviceId);
                    al.PIsResetError = true;
                    if ((lgcBase.PSystemData.PWhichSideInInitilation == al.PSideGroup) || (lgcBase.PSystemData.PWhichSideInInitilation == enSideGroup.Both))
                    {
                        foreach (Robot rb1 in cv_RobotContainer.Values)
                        {
                            if ((lgcBase.PSystemData.PWhichSideInInitilation == rb1.PSideGroup) || (lgcBase.PSystemData.PWhichSideInInitilation == enSideGroup.Both))
                            {
                                SetStatus(APIEnum.CommnadDevice.Robot, rb1.cv_Id);
                            }
                        }
                    }
                    else
                    {
                        SetStatus(APIEnum.CommnadDevice.Aligner, m_Command.cv_DeviceId);
                    }
                    break;
            };
            return rtn;
        }
        #endregion

        #region Status
        private bool ProcessRobotOutVas(CommandData m_Command)
        {
            /*
            bool ok = false;
            EqId eq_id = EqId.VAS;
            int slot = 2;
            int eq_time_chart_cur_step = 0;
            int time_chart_id = -1;
            TimechartNormal time_chart_instance = null;

            if (eq_id == EqId.VAS)
            {
                if (slot == 2)
                {
                    eq_time_chart_cur_step = GetEqById((int)eq_id).GetTimeChatCurStep(2);
                    time_chart_id = (int)EqGifTimeChartId.TIMECHART_ID_VAS_UP;
                    time_chart_instance = (TimechartNormal)cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);

                    if (eq_time_chart_cur_step == TimechartNormal.STEP_ID_WaitRobotCompleteOn)
                    {
                        time_chart_instance.SetSignal(RobotSideBitAddressOffset.Receipt_Complete, true);
                        time_chart_instance.SetSignal(RobotSideBitAddressOffset.Interlock_2, true);
                        ok = true;
                    }
                    else
                    {
                        ShowMsg("VAS time chart not at STEP_ID_WaitRobotCommandFinish", true, false);
                    }
                }
            }
            return ok;
            */
            return false;
        }
        private bool ProcessRobotStatus(CommandData m_Command)
        {
            //00,GetStatus,Robot,StatusCode, Lower EE Wafer Presence, Upper EE Wafe Presence 
            Robot rb = GetRobotById(m_Command.cv_DeviceId);
            bool ok = true;
            rb.PIsSensorUnmatch = false;
            if (m_Command.cv_ReplyParaList[1].Trim() == "1")
            {
                rb.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].PHasSensor = true;
            }
            else if (m_Command.cv_ReplyParaList[1].Trim() == "0")
            {
                rb.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].PHasSensor = false;
            }
            else
            {
                ShowMsg("Command Robot status reply error : " + m_Command.cv_ReplyParaList.ToString(), true, false);
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiRobotStatusError.ToString();
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PMainDescription = "RobotApi Robot Status Error";
                alarm.PUnit = 0;
                alarm.PStatus = AlarmStatus.Occur;
                EditAlarm(alarm);
                ok = false;
            }

            if (m_Command.cv_ReplyParaList[2].Trim() == "1")
            {
                rb.cv_Data.GlassDataMap[(int)RobotArm.rbaUp].PHasSensor = true;
            }

            else if (m_Command.cv_ReplyParaList[2].Trim() == "0")
            {
                rb.cv_Data.GlassDataMap[(int)RobotArm.rbaUp].PHasSensor = false;
            }
            else
            {
                ShowMsg("Command Robot status reply error : " + m_Command.cv_ReplyParaList.ToString(), true, false);
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiRobotStatusError.ToString();
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PMainDescription = "RobotApi Robot Status Error";
                alarm.PUnit = 0;
                alarm.PStatus = AlarmStatus.Occur;
                EditAlarm(alarm);
                ok = false;
            }

            if (m_Command.cv_ReplyParaList[0].Trim() == "0601")
            {
                if (m_Command.cv_DeviceId == 1)
                {
                    lgcBase.PSystemData.PRobot1Status = EquipmentStatus.Idle;
                }
                else if (m_Command.cv_DeviceId == 2)
                {
                    lgcBase.PSystemData.PRobot1Status = EquipmentStatus.Idle;
                }
            }
            else if (m_Command.cv_ReplyParaList[0].Trim() == "4401")
            {
                if (m_Command.cv_DeviceId == 1)
                {
                    lgcBase.PSystemData.PRobot1Status = EquipmentStatus.Run;
                }
                else if (m_Command.cv_DeviceId == 2)
                {
                    lgcBase.PSystemData.PRobot1Status = EquipmentStatus.Run;
                }
            }
            else if (m_Command.cv_ReplyParaList[0].Trim() == "0621")
            {
                if (m_Command.cv_DeviceId == 1)
                {
                    lgcBase.PSystemData.PRobot1Status = EquipmentStatus.Stop;
                }
                else if (m_Command.cv_DeviceId == 2)
                {
                    lgcBase.PSystemData.PRobot1Status = EquipmentStatus.Stop;
                }
            }
            else
            {
                ShowMsg("Command Robot status reply error : " + m_Command.cv_ReplyParaList.ToString(), true, false);
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiRobotStatusError.ToString();
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PMainDescription = "RobotApi Robot Status Error";
                alarm.PUnit = 0;
                alarm.PStatus = AlarmStatus.Occur;
                EditAlarm(alarm);
                ok = false;
            }
            if (ok)
            {
                rb.PIsStatus = true;
                rb.SendDataViaMmf();
                if ((lgcBase.PSystemData.PWhichSideInInitilation == rb.PSideGroup) || (lgcBase.PSystemData.PWhichSideInInitilation == enSideGroup.Both))
                {
                    List<int> tmp = new List<int>();
                    if (!rb.cv_Data.IsSensorDataMatch(out tmp))
                    {
                        ShowMsg("Robot unmatch slot : " + string.Join(",", tmp), true, false);
                        CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                        alarm.PCode = CommonData.HIRATA.Alarmtable.AtInitializeRobotSensorUnmatch.ToString();
                        alarm.PLevel = AlarmLevele.Serious;
                        alarm.PMainDescription = "At Initialize Robot Sensor Unmatch";
                        alarm.PUnit = 0;
                        alarm.PStatus = AlarmStatus.Occur;
                        EditAlarm(alarm);
                        rb.PIsSensorUnmatch = true;
                    }
                    //else
                    {
                        //if(m_Command.cv_ReplyParaList[0].Trim() == "0621")
                        if (rb.cv_Id == 1)
                        {
                            if (lgcBase.PSystemData.PRobot1Status == EquipmentStatus.Stop)
                            {
                                SetRobotRestart(rb.cv_Id);
                            }
                            else
                            {
                                SetHome(APIEnum.CommnadDevice.Robot, rb.cv_Id);
                            }
                        }
                        else if (rb.cv_Id == 2)
                        {
                            if (lgcBase.PSystemData.PRobot2Status == EquipmentStatus.Stop)
                            {
                                SetRobotRestart(rb.cv_Id);
                            }
                            else
                            {
                                SetHome(APIEnum.CommnadDevice.Robot, rb.cv_Id);
                            }
                        }
                    }
                }
            }
            else
            {
                if ((lgcBase.PSystemData.PWhichSideInInitilation == rb.PSideGroup) || (lgcBase.PSystemData.PWhichSideInInitilation == enSideGroup.Both))
                {
                    SendinitCompleteFail(rb.PSideGroup);
                }
            }
            return ok;
        }
        private bool ProcessAlignerStatus(CommandData m_Command)
        {
            bool ok = true;
            Aligner aligner = GetAlignerById(m_Command.cv_DeviceId);
            if (Regex.Match(m_Command.cv_ReplyParaList[1].Trim(), @"true", RegexOptions.IgnoreCase).Success)
            {
                aligner.cv_Data.GlassDataMap[1].PHasSensor = true;
            }

            else if (Regex.Match(m_Command.cv_ReplyParaList[1].Trim(), @"false", RegexOptions.IgnoreCase).Success)
            {
                aligner.cv_Data.GlassDataMap[1].PHasSensor = false;
            }
            else
            {
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiAlignerStatusError.ToString();
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PMainDescription = "RobotApi Aligner Status Error";
                alarm.PUnit = 0;
                alarm.PStatus = AlarmStatus.Occur;
                EditAlarm(alarm);
                ok = false;
            }
            if (Regex.Match(m_Command.cv_ReplyParaList[0].Trim(), @"off", RegexOptions.IgnoreCase).Success)
            {
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiAlignerOffline.ToString();
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PMainDescription = "RobotApi Aligner Offline";
                alarm.PUnit = 0;
                alarm.PStatus = AlarmStatus.Occur;
                EditAlarm(alarm);
            }
            if (ok)
            {
                List<int> tmp = new List<int>();
                if (aligner.cv_Data.IsSensorDataMatch(out tmp))
                {
                    aligner.PIsStatus = true;
                    if(isInInitialationSide(aligner.PSideGroup))
                    {
                        SetHome(APIEnum.CommnadDevice.Aligner, aligner.cv_Id);
                    }
                }
                else
                {
                    ShowMsg(" Aligner unmatch slot : " + string.Join(",", tmp), true, false);
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PCode = CommonData.HIRATA.Alarmtable.AtInitializeAlignerSensorUnmatch.ToString();
                    alarm.PLevel = AlarmLevele.Serious;
                    alarm.PMainDescription = "At Initialize Aligner Sensor Unmatch";
                    alarm.PUnit = 0;
                    alarm.PStatus = AlarmStatus.Occur;
                    EditAlarm(alarm);
                    if(isInInitialationSide(aligner.PSideGroup))
                    {
                        SendinitCompleteFail(aligner.PSideGroup);
                    }
                }
            }
            else
            {
                if(isInInitialationSide(aligner.PSideGroup))
                {
                    SendinitCompleteFail(aligner.PSideGroup);
                }
            }
            aligner.SendDataViaMmf();
            return ok;
        }
        private bool ProcessPortStatus(CommandData m_Command)
        {
            //：00,GetStatus,P*, LP mode, LP status, Foup status, Clamp status, Door status , Port type
            /*
             *  LP mode is mean “Online/ Teaching/ Maintain/ Unknow”. 
                LP status is mean “No error/ Error code/ Unknow”. 
                Foup status is mean “Present/ Absent/ Unknow”. 
                Clamp status is mean “Clamp/ Unclamp/ Unknow”. 
                Door status is mean “Open/ Close/ Unknow”. 
                Example：00,GetStatus, P*,Online,No error,Present,Clamp,Open 
             * port type : 0 type 1 / 1 type 2 , ... , 4 type 5.
             */
            bool ok = true;
            int port_id = m_Command.cv_DeviceId;
            Port port = GetPortById(port_id);
            PortHasCst has_cst = PortHasCst.None;
            PortClamp port_clamp = PortClamp.None;
            bool door_open = false;
            bool port_status_is_load = false;
            if (!Regex.Match(m_Command.cv_ReplyParaList[0].Trim(), @"Online", RegexOptions.IgnoreCase).Success)
            {
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiPortNotInOnline.ToString();
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PMainDescription = "RobotApi Port Not In Online";
                alarm.PUnit = 0;
                alarm.PStatus = AlarmStatus.Occur;
                EditAlarm(alarm);
                ok = false;
            }
            if (!Regex.Match(m_Command.cv_ReplyParaList[1].Trim(), @"No error", RegexOptions.IgnoreCase).Success)
            {
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiPortError.ToString();
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PMainDescription = "RobotApi Port Error";
                alarm.PUnit = 0;
                alarm.PStatus = AlarmStatus.Occur;
                EditAlarm(alarm);
                ok = false;
            }
            if (Regex.Match(m_Command.cv_ReplyParaList[2].Trim(), @"Present", RegexOptions.IgnoreCase).Success)
            {
                has_cst = PortHasCst.Has;
            }
            else if (Regex.Match(m_Command.cv_ReplyParaList[2].Trim(), @"Absent", RegexOptions.IgnoreCase).Success)
            {
                has_cst = PortHasCst.Empty;
            }
            else
            {
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiPortFoupSensorError.ToString();
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PMainDescription = "RobotApi Port Foup sensor error";
                alarm.PUnit = 0;
                alarm.PStatus = AlarmStatus.Occur;
                EditAlarm(alarm);
                ok = false;
            }
            if (Regex.Match(m_Command.cv_ReplyParaList[3].Trim(), @"Unclamp", RegexOptions.IgnoreCase).Success)
            {
                port_clamp = PortClamp.Unclamp;
            }
            else if (Regex.Match(m_Command.cv_ReplyParaList[3].Trim(), @"clamp", RegexOptions.IgnoreCase).Success)
            {
                port_clamp = PortClamp.Clamp;
            }
            else
            {
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiPortFoupClampSensorError.ToString();
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PMainDescription = "RobotApi Port Foup clamp sensor error";
                alarm.PUnit = 0;
                alarm.PStatus = AlarmStatus.Occur;
                EditAlarm(alarm);
                ok = false;
            }
            if (Regex.Match(m_Command.cv_ReplyParaList[5].Trim(), @"\d", RegexOptions.IgnoreCase).Success)
            {
                int type = Convert.ToInt16(m_Command.cv_ReplyParaList[5].Trim());
                if (type < 0 && type > 4)
                {
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PCode = CommonData.HIRATA.Alarmtable.PortTypeValueOverRange.ToString();
                    alarm.PLevel = AlarmLevele.Light;
                    alarm.PMainDescription = "Port type value over range , please re-set";
                    alarm.PUnit = 0;
                    alarm.PStatus = AlarmStatus.Occur;
                    EditAlarm(alarm);
                    ok = false;
                }
                else if (port.cv_Data.PEfemPortType != type)
                {//EfemPortTypeError
                    /*
                    port.cv_Data.PEfemPortType = type;
                    if(type == 0)
                    {
                        port.cv_Data.cv_SlotCount = 25;
                        port.cv_SlotCount = 25;
                        port.SendDataViaMmf();
                    }
                    else if (type == 4)
                    {
                        port.cv_Data.cv_SlotCount = 13;
                        port.cv_SlotCount = 13;
                        port.SendDataViaMmf();
                    }
                    else
                    {
                    */
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PCode = CommonData.HIRATA.Alarmtable.EfemPortTypeError.ToString();
                    alarm.PLevel = AlarmLevele.Serious;
                    alarm.PMainDescription = "Efem Port Type Error";
                    alarm.PUnit = 0;
                    alarm.PStatus = AlarmStatus.Occur;
                    EditAlarm(alarm);
                    ok = false;
                    //}
                }
                else
                {
                    bool slot_error = false;
                    if (port.cv_Data.PEfemPortType == 0)
                    {
                        if (port.cv_Data.cv_SlotCount != 25 || port.cv_SlotCount != 25)
                        {
                            slot_error = true;
                        }
                    }
                    else if (port.cv_Data.PEfemPortType == 4)
                    {
                        if (port.cv_Data.cv_SlotCount != 13 || port.cv_SlotCount != 13)
                        {
                            slot_error = true;
                        }
                    }
                    if (slot_error)
                    {
                        CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                        alarm.PCode = CommonData.HIRATA.Alarmtable.PortTypeSlotNumberError.ToString();
                        alarm.PLevel = AlarmLevele.Serious;
                        alarm.PMainDescription = "Port Type Slot Number Error , please contact vendor";
                        alarm.PUnit = 0;
                        alarm.PStatus = AlarmStatus.Occur;
                        EditAlarm(alarm);
                        ok = false;
                    }
                }
            }
            else
            {
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiPortDoorError.ToString();
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PMainDescription = "RobotApi Port Door error";
                alarm.PUnit = 0;
                alarm.PStatus = AlarmStatus.Occur;
                EditAlarm(alarm);
                ok = false;
            }
            if (Regex.Match(m_Command.cv_ReplyParaList[4].Trim(), @"Open", RegexOptions.IgnoreCase).Success)
            {
                door_open = true;
            }
            else if (Regex.Match(m_Command.cv_ReplyParaList[4].Trim(), @"Close", RegexOptions.IgnoreCase).Success)
            {
                door_open = false; ;
            }
            else
            {
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiPortDoorError.ToString();
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PMainDescription = "RobotApi Port Door error";
                alarm.PUnit = 0;
                alarm.PStatus = AlarmStatus.Occur;
                EditAlarm(alarm);
                ok = false;
            }
            if (port_clamp == PortClamp.Clamp)
            {
                if ((!door_open) || (has_cst == PortHasCst.Empty))
                {
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PCode = CommonData.HIRATA.Alarmtable.PortClampButDoorCloseOrNoFoup.ToString();
                    alarm.PLevel = AlarmLevele.Serious;
                    alarm.PMainDescription = "Port Clamp But Door Close Or No Foup";
                    alarm.PUnit = 0;
                    alarm.PStatus = AlarmStatus.Occur;
                    EditAlarm(alarm);
                    ok = false;
                }
                else
                {
                    port_status_is_load = true;
                }
            }
            else if (port_clamp == PortClamp.Unclamp && door_open)
            {
                if (door_open)
                {
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PCode = CommonData.HIRATA.Alarmtable.PortUnClampButDoorOpen.ToString();
                    alarm.PLevel = AlarmLevele.Serious;
                    alarm.PMainDescription = "Port UnClamp But Door Open";
                    alarm.PUnit = 0;
                    alarm.PStatus = AlarmStatus.Occur;
                    EditAlarm(alarm);
                    ok = false;
                }
                else
                {
                    port_status_is_load = false;
                }
            }
            if (!ok)
            {
                if(isInInitialationSide(port.PSideGroup))
                {
                    SendinitCompleteFail(port.PSideGroup);
                }
            }
            if (ok)
            {
                if (port_status_is_load)
                {
                    if (has_cst == PortHasCst.Has)
                    {
                        port.cv_Data.PPortHasCst = PortHasCst.Has;
                        if (port.PPortStatus == PortStaus.LDCM)
                        {
                            if (!GetPortById(port.cv_Id).PIsRemapping)
                            {
                                if (!lgcBase.PSystemData.PIsForceInitial)
                                {
                                    SetPortLoadAction(port.cv_Id);
                                    ok = false;
                                }
                            }
                            else
                            {
                                List<int> tmp = new List<int>();
                                if (!port.cv_Data.IsSensorDataMatch(out tmp))
                                {
                                    ShowMsg("Port " + port_id + " unmatch slot : " + string.Join(",", tmp), true, false);
                                    GetPortById(port.cv_Id).PIsRemapping = false;
                                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                                    alarm.PCode = CommonData.HIRATA.Alarmtable.AtInitializePortSensorUnmatch.ToString();
                                    alarm.PLevel = AlarmLevele.Serious;
                                    alarm.PMainDescription = "At Initialize Port Sensor Unmatch";
                                    alarm.PUnit = 0;
                                    alarm.PStatus = AlarmStatus.Occur;
                                    EditAlarm(alarm);
                                    if (isInInitialationSide(port.PSideGroup))
                                    {
                                        SendinitCompleteFail(port.PSideGroup);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (has_cst == PortHasCst.Has)
                    {
                        if (port.PPortStatus == PortStaus.UDRQ)
                        {
                            //port.PLotStatus = LotStatus.Cancel;
                            //port.PClamp = PortClamp.Unclamp;
                            //port.cv_Data.PPortHasCst = PortHasCst.Has;
                        }
                        else if (port.PPortStatus == PortStaus.LDRQ || port.PPortStatus == PortStaus.UDCM || port.PPortStatus == PortStaus.None)
                        {
                            port.PPortStatus = PortStaus.LDRQ;
                            port.PLotStatus = LotStatus.FoupSensorOn;
                            port.PClamp = PortClamp.Unclamp;
                            port.cv_Data.ClearAllGlassData();
                            port.cv_Data.PPortHasCst = PortHasCst.Has;
                            //cv_Comm.SetOperatorAccessButton(SignalTowerControl.Flash, port.cv_Id);
                        }
                        else
                        {
                            port.PPortStatus = PortStaus.UDRQ;
                            port.PClamp = PortClamp.Unclamp;
                            port.cv_Data.PPortHasCst = PortHasCst.Has;
                        }
                    }
                    else
                    {
                        port.cv_Data.Clear();
                        port.PPortStatus = PortStaus.LDRQ;
                        port.PLotStatus = LotStatus.Empty;
                        port.PClamp = PortClamp.Unclamp;
                        port.cv_Data.PPortHasCst = PortHasCst.Empty;
                    }
                }
            }
            if (ok)
            {
                GetPortById(m_Command.cv_DeviceId).PIsStatus = true;

                if(isInInitialationSide(port.PSideGroup))
                {
                    if (CheckAllPortStatus())
                    {
                        if (!SetAllPortHome())
                        {
                            foreach (Aligner al in cv_AlignerContainer.Values)
                            {
                                if (isInInitialationSide(al.PSideGroup))
                                {
                                    SetStatus(APIEnum.CommnadDevice.Aligner, al.cv_Id);
                                }
                            }
                        }
                    }
                }
            }
            GetPortById(port_id).SendDataViaMmf();
            return ok;
        }
        private bool ProcessBufferStatus(CommandData m_Command)
        {
            bool ok = true;
            Buffer buffer = LgcModule.GetBufferById(m_Command.cv_DeviceId);
            if (m_Command.cv_ReplyParaList.Count >= buffer.cv_SlotCount)
            {
                for (int i = 0; i < m_Command.cv_ReplyParaList.Count; i++)
                {
                    if (i < LgcModule.GetBufferById(1).cv_SlotCount)
                    {
                        if (Regex.Match(m_Command.cv_ReplyParaList[i], @"1", RegexOptions.IgnoreCase).Success)
                        {
                            buffer.cv_Data.GlassDataMap[i + 1].PHasSensor = true;
                        }
                        else if (Regex.Match(m_Command.cv_ReplyParaList[i], @"0", RegexOptions.IgnoreCase).Success)
                        {
                            buffer.cv_Data.GlassDataMap[i + 1].PHasSensor = false;
                        }
                        else
                        {
                            LgcModule.ShowMsg("Command reply error (mapping data abnormal) : " +
                                string.Join(",", m_Command.cv_ReplyParaList.ToString()), true, false);
                            ok = false;
                            CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                            alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiBufferStatusError.ToString();
                            alarm.PMainDescription = "Robot API Buffer sensor Error";
                            alarm.PUnit = 0;
                            alarm.PLevel = AlarmLevele.Serious;
                            alarm.PStatus = AlarmStatus.Occur;
                            alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
                            LgcModule.EditAlarm(alarm);
                        }
                    }
                }
            }
            else
            {
                ok = false;
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiBufferStatusError.ToString();
                alarm.PMainDescription = "Robot API Buffer Status Error";
                alarm.PUnit = 0;
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PStatus = AlarmStatus.Occur;
                alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
                LgcModule.EditAlarm(alarm);
            }
            if (ok)
            {
                List<int> tmp = new List<int>();
                if (buffer.cv_Data.IsSensorDataMatch(out tmp))
                {
                    if(isInInitialationSide(buffer.PSideGroup))
                    {
                        SetStatus(APIEnum.CommnadDevice.EFEM, 0);
                    }
                }
                else
                {
                    LgcModule.ShowMsg("Buffer unmatch slot : " + string.Join(",", tmp), true, false);
                    ok = false;
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PCode = CommonData.HIRATA.Alarmtable.AtInitializeBufferSensorUnmatch.ToString();
                    alarm.PMainDescription = "At Initialize Buffer Sensor Unmatch";
                    alarm.PUnit = 0;
                    alarm.PLevel = AlarmLevele.Serious;
                    alarm.PStatus = AlarmStatus.Occur;
                    alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
                    LgcModule.EditAlarm(alarm);
                    if(isInInitialationSide(buffer.PSideGroup))
                    {
                        SendinitCompleteFail(buffer.PSideGroup);
                    }
                }
            }
            else
            {
                if(isInInitialationSide(buffer.PSideGroup))
                {
                    SendinitCompleteFail(buffer.PSideGroup);
                }
            }
            buffer.SendDataViaMmf();
            return ok;
        }
        private bool ProcessEventStatus(CommandData m_Command)
        {
            /*
             *  Normal Reply：00,GetStatus,EFEM, Pressure, Vacuum, Ionizer1, Ionizer2, Ionizer3, Ionizer4, 
             *  Ionizer5, Ionizer6, Ionizer7, Ionizer8, FFU1, 
FFU2, FFU3, FFU4, FFU5, FFU6, FFU7, FFU8, FFU9, FFU10, FFU11,Robot Mode, Robot Enable, Door, EMO, Power 
            */
            bool ok = false;
            CommonData.HIRATA.MDEfemStatus obj = new MDEfemStatus();
            //cb_ManualApi.Items.AddRange(Enum.GetNames(typeof(APIEnum.APICommand)).ToArray<string>());
            List<string> tmp = Enum.GetNames(typeof(CommonData.HIRATA.APIEnum.EventCommand)).ToList<string>();
            if (m_Command.cv_ReplyParaList.Count <= tmp.Count)
            {
                obj.cv_Pressure = Convert.ToInt16(m_Command.cv_ReplyParaList[0].Trim());
                obj.cv_Vacuum = Convert.ToInt16(m_Command.cv_ReplyParaList[1].Trim());
                obj.cv_Ionizer1 = Convert.ToInt16(m_Command.cv_ReplyParaList[2].Trim());
                obj.cv_Ionizer2 = Convert.ToInt16(m_Command.cv_ReplyParaList[3].Trim());
                obj.cv_Ionizer3 = Convert.ToInt16(m_Command.cv_ReplyParaList[4].Trim());
                obj.cv_Ionizer4 = Convert.ToInt16(m_Command.cv_ReplyParaList[5].Trim());
                obj.cv_Ionizer5 = Convert.ToInt16(m_Command.cv_ReplyParaList[6].Trim());
                obj.cv_Ionizer6 = Convert.ToInt16(m_Command.cv_ReplyParaList[7].Trim());
                obj.cv_Ionizer7 = Convert.ToInt16(m_Command.cv_ReplyParaList[8].Trim());
                obj.cv_Ionizer8 = Convert.ToInt16(m_Command.cv_ReplyParaList[9].Trim());
                obj.cv_FFU1 = Convert.ToInt16(m_Command.cv_ReplyParaList[10].Trim());
                obj.cv_FFU2 = Convert.ToInt16(m_Command.cv_ReplyParaList[11].Trim());
                obj.cv_FFU3 = Convert.ToInt16(m_Command.cv_ReplyParaList[12].Trim());
                obj.cv_FFU4 = Convert.ToInt16(m_Command.cv_ReplyParaList[13].Trim());
                obj.cv_FFU5 = Convert.ToInt16(m_Command.cv_ReplyParaList[14].Trim());
                obj.cv_FFU6 = Convert.ToInt16(m_Command.cv_ReplyParaList[15].Trim());
                obj.cv_FFU7 = Convert.ToInt16(m_Command.cv_ReplyParaList[16].Trim());
                obj.cv_FFU8 = Convert.ToInt16(m_Command.cv_ReplyParaList[17].Trim());
                obj.cv_FFU9 = Convert.ToInt16(m_Command.cv_ReplyParaList[18].Trim());
                obj.cv_FFU10 = Convert.ToInt16(m_Command.cv_ReplyParaList[19].Trim());
                obj.cv_FFU11 = Convert.ToInt16(m_Command.cv_ReplyParaList[20].Trim());
                obj.cv_RobotMode = Convert.ToInt16(m_Command.cv_ReplyParaList[21].Trim());
                obj.cv_RobotEnable = Convert.ToInt16(m_Command.cv_ReplyParaList[22].Trim());
                obj.cv_Door = Convert.ToInt16(m_Command.cv_ReplyParaList[23].Trim());
                obj.cv_EMO = Convert.ToInt16(m_Command.cv_ReplyParaList[24].Trim());
                obj.cv_Power = Convert.ToInt16(m_Command.cv_ReplyParaList[25].Trim());

                //Global.Controller.SendMmfNotifyObject(typeof(CommonData.HIRATA.MDEfemStatus).Name, obj, KParseObjToXmlPropertyType.Field);
                ok = true;

                for (int i = 0; i <= 25; i++)
                {
                    if (m_Command.cv_ReplyParaList[i].Trim() == "0")
                    {
                        if (i != 22)
                        {
                            ok = false;
                            CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                            alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiBufferStatusError.ToString();
                            alarm.PMainDescription = "Robot API EFEM Status Error";
                            alarm.PUnit = 0;
                            alarm.PLevel = AlarmLevele.Serious;
                            alarm.PStatus = AlarmStatus.Occur;
                            alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
                            EditAlarm(alarm);
                            if(lgcBase.PSystemData.PIsInInitialation)
                            {
                                SendinitCompleteFail(lgcBase.PSystemData.PWhichSideInInitilation);
                            }
                        }
                    }
                }
            }
            else
            {
                ok = false;
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = CommonData.HIRATA.Alarmtable.RobotApiBufferStatusError.ToString();
                alarm.PMainDescription = "Robot API EFEM Status Error";
                alarm.PUnit = 0;
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PStatus = AlarmStatus.Occur;
                alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
                EditAlarm(alarm);
                if(lgcBase.PSystemData.PIsInInitialation)
                {
                    SendinitCompleteFail(lgcBase.PSystemData.PWhichSideInInitilation);
                }
            }
            if (ok)
            {
                if(lgcBase.PSystemData.PIsInInitialation)
                {
                    foreach(Robot rb in cv_RobotContainer.Values)
                    {
                        if(isInInitialationSide(rb.PSideGroup))
                        {
                            if(rb.cv_Id == 1)
                            {
                                SetRobotSpeed(lgcBase.PSystemData.PRobot1Speed, rb.cv_Id);
                                rb.cv_WaitRobotSpeed = lgcBase.PSystemData.PRobot1Speed;
                            }
                            else if(rb.cv_Id == 2)
                            {
                                SetRobotSpeed(lgcBase.PSystemData.PRobot2Speed, rb.cv_Id);
                                rb.cv_WaitRobotSpeed = lgcBase.PSystemData.PRobot2Speed;
                            }
                        }
                    }
                }
            }
            return ok;
        }
        #endregion

        public static bool isInInitialationSide(enSideGroup m_Side)
        {
            bool rtn = false;
            if ((lgcBase.PSystemData.PWhichSideInInitilation == m_Side) || (lgcBase.PSystemData.PWhichSideInInitilation == enSideGroup.Both))
            {
                rtn = true;
            }
            return rtn;
        }

        #region send command.

        #region Port
        public static bool SetPortLoadAction(int m_PortId)
        {
            CommandData command = new CommandData(APIEnum.CommandType.LoadPort, APIEnum.LoadPortCommand.Load.ToString(), APIEnum.CommnadDevice.P, m_PortId);
            SetRobotCommonAction(command);
            return true;
        }
        public static bool SetPortUnloadAction(int m_PortId)
        {
            CommandData command = new CommandData(APIEnum.CommandType.LoadPort, APIEnum.LoadPortCommand.Unload.ToString(), APIEnum.CommnadDevice.P, m_PortId);
            SetRobotCommonAction(command);
            return true;
        }
        public static bool SetGetMappingData(int m_PortId)
        {
            CommandData command = new CommandData(APIEnum.CommandType.LoadPort, APIEnum.LoadPortCommand.GetWaferSlot2.ToString(), APIEnum.CommnadDevice.P, m_PortId);
            SetRobotCommonAction(command);
            return true;
        }
        public static bool SetPortSlotTypeChange(CommandData command)
        {
            //CommandData command = new CommandData(APIEnum.CommandType.LoadPort, APIEnum.LoadPortCommand.GetWaferSlot2.ToString(), APIEnum.CommnadDevice.P, m_PortId);
            SetRobotCommonAction(command);
            return true;
        }
        public static void SetLoadUnloadLed(bool isLoadLed, SignalTowerControl m_Control, int m_Id)
        {
            /*
            List<string> paras = new List<string>();
            paras.Add(m_Control.ToString());
            CommandData obj = null;
            if (isLoadLed)
                obj = new CommonData.HIRATA.CommandData(APIEnum.CommandType.LoadPort, APIEnum.LoadPortCommand.LEDLoad.ToString(), CommonData.HIRATA.APIEnum.CommnadDevice.P, m_Id, paras);
            else
                obj = new CommonData.HIRATA.CommandData(APIEnum.CommandType.LoadPort, APIEnum.LoadPortCommand.LEDUnLoad.ToString(), CommonData.HIRATA.APIEnum.CommnadDevice.P, m_Id, paras);
            SetRobotCommonAction(obj);
            */
        }
        public static bool SetOperatorAccessButton(SignalTowerControl m_Control, int m_PortId)
        {
            /*
            List<string> paras = new List<string>();
            paras.Add(m_Control.ToString());
            CommandData command = new CommandData(APIEnum.CommandType.LoadPort, APIEnum.LoadPortCommand.SetOperatorAccessButton.ToString(), APIEnum.CommnadDevice.P, m_PortId , paras);
            SetRobotCommonAction(command);
             */
            return true;
        }
        public static bool SetPortClampAction(int m_PortId, APIEnum.LoadPortCommand m_Action)
        {
            bool rtn = false;
            CommandData command = null;
            if (m_Action == APIEnum.LoadPortCommand.Clamp)
            {
                command = new CommandData(APIEnum.CommandType.LoadPort, APIEnum.LoadPortCommand.Clamp.ToString(), APIEnum.CommnadDevice.P, m_PortId);
            }
            else if (m_Action == APIEnum.LoadPortCommand.UnClamp)
            {
                command = new CommandData(APIEnum.CommandType.LoadPort, APIEnum.LoadPortCommand.UnClamp.ToString(), APIEnum.CommnadDevice.P, m_PortId);
            }
            if (command != null)
            {
                SetRobotCommonAction(command);
                rtn = true;
            }
            return rtn;
        }

        #endregion

        #region Robot
        public static void SetRobotStop(int robotId)
        {
            CommandData command = new CommandData(APIEnum.CommandType.Robot, APIEnum.RobotCommand.Stop.ToString(), APIEnum.CommnadDevice.Robot, robotId);
            SetRobotCommonAction(command);
        }
        public static void SetRobotRestart(int robotId)
        {
            CommandData command = new CommandData(APIEnum.CommandType.Robot, APIEnum.RobotCommand.ReStart.ToString(), APIEnum.CommnadDevice.Robot, robotId);
            SetRobotCommonAction(command);
        }
        public static void SetRobotSpeed(int m_Speed, int robotId)
        {
            List<string> paras = new List<string>();
            paras.Add(m_Speed.ToString());
            paras.Add(m_Speed.ToString());
            CommandData command = new CommandData(APIEnum.CommandType.Robot, APIEnum.RobotCommand.SetRobotSpeed.ToString(), APIEnum.CommnadDevice.Robot, robotId, paras);
            SetRobotCommonAction(command);
        }

        /*
        public static void SetRobotWaferGet(int robotId, RobotArm m_Arm, APIEnum.CommnadDevice m_Device, int m_DeviceId, int m_Slot)
        { // must has m_Device Id , port id / aligner id / stage id (incluse buffer )
            List<string> paras = new List<string>();
            paras.Add(((int)m_Arm).ToString());

            if (m_Device == APIEnum.CommnadDevice.Buffer)
            {
                m_Device = APIEnum.CommnadDevice.Stage;
                m_DeviceId = LgcModule.GetBufferById(1).cv_RobotPos;
            }
            else if (m_Device == APIEnum.CommnadDevice.Aligner)
            {
                m_Device = APIEnum.CommnadDevice.Aligner;
                m_DeviceId = 1;
            }
            else if (m_Device == APIEnum.CommnadDevice.P)
            {
                m_Device = APIEnum.CommnadDevice.Aligner;
            }
            else if (m_Device == APIEnum.CommnadDevice.Stage)
            {
                m_DeviceId = LgcModule.GetEqById(m_DeviceId).cv_RobotPos;
            }

            paras.Add(m_Device.ToString() + m_DeviceId.ToString());
            paras.Add(m_Slot.ToString());
            CommandData command = new CommandData(APIEnum.CommandType.Robot, APIEnum.RobotCommand.WaferGet.ToString(), APIEnum.CommnadDevice.Robot, robotId, paras);
            SetRobotCommonAction(command);
        }
        public static void SetRobotWaferPut(int robotId, RobotArm m_Arm, APIEnum.CommnadDevice m_Device, int m_DeviceId, int m_Slot)
        { // must has m_Device Id , port id / aligner id / stage id (incluse buffer )
            List<string> paras = new List<string>();
            paras.Add(((int)m_Arm).ToString());

            if (m_Device == APIEnum.CommnadDevice.Buffer)
            {
                m_Device = APIEnum.CommnadDevice.Stage;
                m_DeviceId = LgcModule.GetBufferById(1).cv_RobotPos;

            }
            else if (m_Device == APIEnum.CommnadDevice.Aligner)
            {
                m_Device = APIEnum.CommnadDevice.Aligner;
                m_DeviceId = 1;
            }
            else if (m_Device == APIEnum.CommnadDevice.P)
            {
                m_Device = APIEnum.CommnadDevice.Aligner;
            }
            else if (m_Device == APIEnum.CommnadDevice.Stage)
            {
                m_DeviceId = LgcModule.GetEqById(m_DeviceId).cv_RobotPos;
            }

            paras.Add(m_Device.ToString() + m_DeviceId.ToString());
            paras.Add(m_Slot.ToString());
            CommandData command = new CommandData(APIEnum.CommandType.Robot, APIEnum.RobotCommand.WaferPut.ToString(), APIEnum.CommnadDevice.Robot, robotId, paras);
            SetRobotCommonAction(command);
        }
        public static void SetRobotGetStandby(int robotId, RobotArm m_Arm, APIEnum.CommnadDevice m_Device, int m_DeviceId, int m_Slot)
        { // must has m_Device Id , port id / aligner id / stage id (incluse buffer )
            List<string> paras = new List<string>();
            paras.Add(((int)m_Arm).ToString());

            if (m_Device == APIEnum.CommnadDevice.Buffer)
            {
                m_Device = APIEnum.CommnadDevice.Stage;
                m_DeviceId = LgcModule.GetBufferById(1).cv_RobotPos;
            }
            else if (m_Device == APIEnum.CommnadDevice.Aligner)
            {
                m_Device = APIEnum.CommnadDevice.Aligner;
                m_DeviceId = 1;
            }
            else if (m_Device == APIEnum.CommnadDevice.P)
            {
                m_Device = APIEnum.CommnadDevice.Aligner;
            }
            else if (m_Device == APIEnum.CommnadDevice.Stage)
            {
                m_DeviceId = LgcModule.GetEqById(m_DeviceId).cv_RobotPos;
            }

            paras.Add(m_Device.ToString() + m_DeviceId.ToString());
            paras.Add(m_Slot.ToString());
            CommandData command = new CommandData(APIEnum.CommandType.Robot, APIEnum.RobotCommand.GetStandby.ToString(), APIEnum.CommnadDevice.Robot, robotId, paras);
            SetRobotCommonAction(command);
        }
        public static void SetRobotPutStandby(int robotId, RobotArm m_Arm, APIEnum.CommnadDevice m_Device, int m_DeviceId, int m_Slot)
        { // must has m_Device Id , port id / aligner id / stage id (incluse buffer )
            List<string> paras = new List<string>();
            paras.Add(((int)m_Arm).ToString());

            if (m_Device == APIEnum.CommnadDevice.Buffer)
            {
                m_Device = APIEnum.CommnadDevice.Stage;
                m_DeviceId = LgcModule.GetBufferById(1).cv_RobotPos;
            }
            else if (m_Device == APIEnum.CommnadDevice.Aligner)
            {
                m_Device = APIEnum.CommnadDevice.Aligner;
                m_DeviceId = 1;
            }
            else if (m_Device == APIEnum.CommnadDevice.P)
            {
                m_Device = APIEnum.CommnadDevice.Aligner;
            }
            else if (m_Device == APIEnum.CommnadDevice.Stage)
            {
                m_DeviceId = LgcModule.GetEqById(m_DeviceId).cv_RobotPos;
            }

            paras.Add(m_Device.ToString() + m_DeviceId.ToString());
            paras.Add(m_Slot.ToString());
            CommandData command = new CommandData(APIEnum.CommandType.Robot, APIEnum.RobotCommand.PutStandby.ToString(), APIEnum.CommnadDevice.Robot, robotId, paras);
            SetRobotCommonAction(command);
        }
        //vas down arm (glass) get from down slot
        public static void SetRobotGetStandbyArmExtend(int robotId, RobotArm m_Arm, int m_Slot, bool m_IsVas)
        { // must has m_Device Id , port id / aligner id / stage id (incluse buffer )
            List<string> paras = new List<string>();
            if (m_Arm == RobotArm.rbaDown && m_Slot == 1 && m_IsVas)
            {
                paras.Add(((int)m_Arm).ToString());
                paras.Add(APIEnum.CommnadDevice.Stage.ToString() + LgcModule.GetEqById((int)EqId.VAS).cv_RobotPos.ToString());
                paras.Add(m_Slot.ToString());
                CommandData command = new CommandData(APIEnum.CommandType.Robot, APIEnum.RobotCommand.GetStandbyArmExtend.ToString(), APIEnum.CommnadDevice.Robot, robotId, paras);
                SetRobotCommonAction(command);
            }
        }
        //vas up arm (glass) put to down slot
        public static void SetRobotPutStandbyArmExtend(int robotId, RobotArm m_Arm, int m_Slot, bool m_IsVas)
        { // must has m_Device Id , port id / aligner id / stage id (incluse buffer )
            List<string> paras = new List<string>();
            if (m_Arm == RobotArm.rbaUp && m_Slot == 1 && m_IsVas)
            {
                paras.Add(((int)m_Arm).ToString());
                paras.Add(APIEnum.CommnadDevice.Stage.ToString() + LgcModule.GetEqById((int)EqId.VAS).cv_RobotPos.ToString());
                paras.Add(m_Slot.ToString());
                CommandData command = new CommandData(APIEnum.CommandType.Robot, APIEnum.RobotCommand.PutStandbyArmExtend.ToString(), APIEnum.CommnadDevice.Robot, robotId, paras);
                SetRobotCommonAction(command);
            }
        }
        //vas down arm (glass) put to up slot ( step 1 )
        public static void SetRobotTopPutStandbyArmExtend(int robotId, RobotArm m_Arm, int m_Slot, bool m_IsVas)
        { // must has m_Device Id , port id / aligner id / stage id (incluse buffer )
            List<string> paras = new List<string>();
            if (m_Arm == RobotArm.rbaDown && m_Slot == 2 && m_IsVas)
            {
                paras.Add(((int)m_Arm).ToString());
                paras.Add(APIEnum.CommnadDevice.Stage.ToString() + LgcModule.GetEqById((int)EqId.VAS).cv_RobotPos.ToString());
                paras.Add(m_Slot.ToString());
                CommandData command = new CommandData(APIEnum.CommandType.Robot, APIEnum.RobotCommand.TopPutStandbyArmExtend.ToString(), APIEnum.CommnadDevice.Robot, robotId, paras);
                SetRobotCommonAction(command);
            }
        }
        //vas down arm (glass) put to up slot ( step 2 )
        public static void SetRobotTopWaferPut(int robotId, RobotArm m_Arm, int m_Slot, bool m_IsVas)
        { // must has m_Device Id , port id / aligner id / stage id (incluse buffer )
            List<string> paras = new List<string>();
            if (m_Arm == RobotArm.rbaDown && m_Slot == 2 && m_IsVas)
            {
                paras.Add(((int)m_Arm).ToString());
                paras.Add(APIEnum.CommnadDevice.Stage.ToString() + LgcModule.GetEqById((int)EqId.VAS).cv_RobotPos.ToString());
                paras.Add(m_Slot.ToString());
                CommandData command = new CommandData(APIEnum.CommandType.Robot, APIEnum.RobotCommand.GetStandbyArmExtend.ToString(), APIEnum.CommnadDevice.Robot, robotId, paras);
                SetRobotCommonAction(command);
            }
        }
        */

        #endregion

        #region Common
        public static bool SetAllPortStatus()
        {
            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
            {
                SetStatus(APIEnum.CommnadDevice.P, i);
            }
            return true;
        }
        public static void SetStatus(APIEnum.CommnadDevice m_Device, int m_PoerId)
        {
            CommandData command = null;
            int id = m_PoerId;
            switch (m_Device)
            {
                case APIEnum.CommnadDevice.Robot:
                    command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.Common, APIEnum.CommonCommand.GetStatus.ToString(),
                        CommonData.HIRATA.APIEnum.CommnadDevice.Robot, id);
                    break;
                case APIEnum.CommnadDevice.P:
                    command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.Common, APIEnum.CommonCommand.GetStatus.ToString(),
                        CommonData.HIRATA.APIEnum.CommnadDevice.P, id);
                    break;
                case APIEnum.CommnadDevice.Buffer:
                    command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.IO, APIEnum.IoCommand.GetBufferStatus.ToString(),
                        CommonData.HIRATA.APIEnum.CommnadDevice.IO, id);
                    break;
                case APIEnum.CommnadDevice.EFEM:
                    command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.Common, APIEnum.CommonCommand.GetStatus.ToString(),
                        CommonData.HIRATA.APIEnum.CommnadDevice.EFEM, id);
                    break;
                case APIEnum.CommnadDevice.Aligner:
                    command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.Common, APIEnum.CommonCommand.GetStatus.ToString(),
                        CommonData.HIRATA.APIEnum.CommnadDevice.Aligner, id);
                    break;
            };
            if (command != null)
            {
                SetRobotCommonAction(command);
            }
        }
        public static void SetHome(APIEnum.CommnadDevice m_Device, int m_PoerId)
        {
            CommandData command = null;
            int id = m_PoerId;
            switch (m_Device)
            {
                case APIEnum.CommnadDevice.Robot:
                    command = new CommandData(APIEnum.CommandType.Common, APIEnum.CommonCommand.Home.ToString(), APIEnum.CommnadDevice.Robot, id);
                    break;
                case APIEnum.CommnadDevice.P:
                    command = new CommandData(APIEnum.CommandType.Common, APIEnum.CommonCommand.Home.ToString(), APIEnum.CommnadDevice.P, id);
                    break;
                case APIEnum.CommnadDevice.Aligner:
                    command = new CommandData(APIEnum.CommandType.Common, APIEnum.CommonCommand.Home.ToString(), APIEnum.CommnadDevice.Aligner, id);
                    break;
            };
            if (command != null)
            {
                SetRobotCommonAction(command);
            }
        }
        public static bool SetAllPortHome()
        {
            bool is_need = false;
            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
            {
                Port port = LgcModule.GetPortById(i);
                //tmp don't check data.
                if ((port.cv_Data.PPortHasCst == PortHasCst.Has))//) && (port.PPortStatus != PortStaus.LDCM))
                {
                    if (!lgcBase.PSystemData.PIsForceInitial)
                    {
                        if (port.PPortStatus != PortStaus.LDCM)
                        {
                            SetHome(APIEnum.CommnadDevice.P, i);
                            is_need = true;
                        }
                        else
                        {
                            port.cv_IsHome = true;
                        }
                    }
                    else
                    {
                        SetHome(APIEnum.CommnadDevice.P, i);
                        is_need = true;
                    }
                }
                else
                {
                    port.cv_IsHome = true;
                }
            }
            return is_need;
        }
        public static void SetErrorReset(APIEnum.CommnadDevice m_Device, int m_PoerId)
        {
            CommandData command = null;
            int id = m_PoerId;
            switch (m_Device)
            {
                case APIEnum.CommnadDevice.Robot:
                    command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.Common, APIEnum.CommonCommand.ResetError.ToString(),
                        CommonData.HIRATA.APIEnum.CommnadDevice.Robot, id);
                    break;
                case APIEnum.CommnadDevice.P:
                    command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.Common, APIEnum.CommonCommand.ResetError.ToString(),
                        CommonData.HIRATA.APIEnum.CommnadDevice.P, id);
                    break;
                case APIEnum.CommnadDevice.Aligner:
                    command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.Common, APIEnum.CommonCommand.ResetError.ToString(),
                        CommonData.HIRATA.APIEnum.CommnadDevice.Aligner, id);
                    break;
            };
            if (command != null)
            {
                SetRobotCommonAction(command);
            }
        }
        #endregion

        #region API
        public static void SetApiCommonCommand(APIEnum.APICommand m_Command)
        {
            CommandData command = null;
            switch (m_Command)
            {
                case APIEnum.APICommand.CurrentMode:
                    command = new CommandData(APIEnum.CommandType.API, APIEnum.APICommand.CurrentMode.ToString(), APIEnum.CommnadDevice.API, 0);
                    break;
                case APIEnum.APICommand.Remote:
                    command = new CommandData(APIEnum.CommandType.API, APIEnum.APICommand.Remote.ToString(), APIEnum.CommnadDevice.API, 0);
                    break;
                case APIEnum.APICommand.Local:
                    command = new CommandData(APIEnum.CommandType.API, APIEnum.APICommand.Local.ToString(), APIEnum.CommnadDevice.API, 0);
                    break;
                case APIEnum.APICommand.Version:
                    command = new CommandData(APIEnum.CommandType.API, APIEnum.APICommand.Version.ToString(), APIEnum.CommnadDevice.API, 0);
                    break;
                case APIEnum.APICommand.Hide:
                    command = new CommandData(APIEnum.CommandType.API, APIEnum.APICommand.Hide.ToString(), APIEnum.CommnadDevice.API, 0);
                    break;
                case APIEnum.APICommand.Show:
                    command = new CommandData(APIEnum.CommandType.API, APIEnum.APICommand.Show.ToString(), APIEnum.CommnadDevice.API, 0);
                    break;
            };
            if (command != null)
            {
                SetRobotCommonAction(command);
            }
        }
        #endregion

        #region aligner
        public static void SetAlignerAlignment(float m_Value, int id)
        {
            List<string> para = new List<string>();
            para.Add(m_Value.ToString());
            CommandData command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.Aligner, APIEnum.AlignerCommand.Alignment.ToString(), CommonData.HIRATA.APIEnum.CommnadDevice.Aligner, id, para);
            SetRobotCommonAction(command);
        }
        public static void SetAlignerVaccum(bool m_IsOn, int id)
        {
            List<string> para = new List<string>();
            if (m_IsOn)
            {
                para.Add("On");
            }
            else
            {
                para.Add("Off");
            }
            CommandData command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.Aligner, APIEnum.AlignerCommand.AlignerVacuum.ToString(),
              CommonData.HIRATA.APIEnum.CommnadDevice.Aligner, id, para);
            SetRobotCommonAction(command);
        }
        public static void SetAlignerFindNotch(int id)
        {
            CommandData command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.Aligner, APIEnum.AlignerCommand.FindNotch.ToString(),
              CommonData.HIRATA.APIEnum.CommnadDevice.Aligner, id);
            SetRobotCommonAction(command);
        }
        public static void SetAlignerToAngle(int id)
        {
            CommandData command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.Aligner, APIEnum.AlignerCommand.ToAngle.ToString(),
              CommonData.HIRATA.APIEnum.CommnadDevice.Aligner, id);
            SetRobotCommonAction(command);
        }
        public static void SetAlignerReadDegree(float m_Value, int id)
        {
            List<string> para = new List<string>();
            para.Add(m_Value.ToString());
            CommandData command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.Aligner, APIEnum.AlignerCommand.SetIDReaderDegree.ToString(),
              CommonData.HIRATA.APIEnum.CommnadDevice.Aligner, id, para);
            SetRobotCommonAction(command);
        }
        public static void SetAlignerDegree(float m_Value, int id)
        {
            List<string> para = new List<string>();
            para.Add(m_Value.ToString());
            CommandData command = new CommonData.HIRATA.CommandData(APIEnum.CommandType.Aligner, APIEnum.AlignerCommand.SetAlignerDegree.ToString(),
              CommonData.HIRATA.APIEnum.CommnadDevice.Aligner, id, para);
            SetRobotCommonAction(command);
        }
        #endregion

        #region IO
        public static void SetSignalTower(SignalTowerColor m_Color, SignalTowerControl m_Control)
        {
            List<string> paras = new List<string>();
            paras.Add(m_Color.ToString() + m_Control.ToString());
            CommandData command = new CommandData(APIEnum.CommandType.IO, APIEnum.IoCommand.SignalTower.ToString(), APIEnum.CommnadDevice.IO, 0, paras);
            SetRobotCommonAction(command);
        }
        public static void SetSetFFUVoltage(int m_Speed)
        {
            List<string> paras = new List<string>();
            paras.Add(m_Speed.ToString());
            CommandData command = new CommandData(APIEnum.CommandType.IO, APIEnum.IoCommand.SetFFUVoltage.ToString(), APIEnum.CommnadDevice.IO, 0, paras);
            SetRobotCommonAction(command);
        }
        public static void SetBuzzer(bool m_IsOn)
        {
            List<string> para = new List<string>();
            if (m_IsOn) para.Add("1");
            else para.Add("0");
            CommandData command_obj = new CommandData(APIEnum.CommandType.IO, APIEnum.IoCommand.Buzzer.ToString()
                , APIEnum.CommnadDevice.IO, 0, para);
            SetRobotCommonAction(command_obj);
        }
        public static void GetBufferProtrusionSensor()
        {
            CommandData command_obj = new CommandData(APIEnum.CommandType.IO, APIEnum.IoCommand.GetBufferProtrusionSensor.ToString()
                , APIEnum.CommnadDevice.IO, 0);
            SetRobotCommonAction(command_obj);
        }
        #endregion

        #region RFID
        public static bool SetReadRFIDRead(int m_PortId)
        {
            CommandData command = new CommandData(APIEnum.CommandType.RFID, APIEnum.RfidCommand.ReadFoupID.ToString(), APIEnum.CommnadDevice.RFID, m_PortId);
            SetRobotCommonAction(command);
            return true;
        }
        #endregion

        #region OCR
        public static void SetOcrRead(int id)
        {
            CommandData command = new CommandData(APIEnum.CommandType.OCR, APIEnum.OcrCommand.Read.ToString(), APIEnum.CommnadDevice.OCRReader, id);
            SetRobotCommonAction(command);
        }
        public static void SetOcrConnect(int id)
        {
            CommandData command = new CommandData(APIEnum.CommandType.OCR, APIEnum.OcrCommand.Connect.ToString(), APIEnum.CommnadDevice.OCRReader, id);
            SetRobotCommonAction(command);
        }
        #endregion

        #endregion
    }
        
}

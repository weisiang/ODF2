using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Timers;
using System.Reflection;
using KgsCommon;
using HirataRbAPI;
using CommonData.HIRATA;
using BaseAp;

namespace LGC
{
    public class RBController : LogBase
    {
        public delegate void DeleConnect(bool m_IsConnect);
        public delegate void DeleRecv(CommandData m_Rtn);
        public delegate void DeleRecvError(string m_RecvTxt);
        public delegate void DeleSend(CommandData m_SendCommand);
        public delegate void DeleSendError(string m_CommandTxt, string m_Msg);
        public DeleConnect OnConnectEvent;
        public DeleRecv OnRecvTimeOutEvent;
        public DeleSend OnRecvEvent;
        public DeleSendError OnSendErrorEvent;
        public DeleRecvError OnRecvParseError;

        public bool IsBusy
        {
            get
            {
                if (cv_Commands.Count > 0) return true;
                else return false;
            }
        }
        private KTimer cv_Timer = null;
        private List<CommandData> cv_Commands = null;
        HIRATADevice cv_Client = null;

        public bool cv_IsReturnNotFoundExeEvent = false;
        public bool Connected
        {
            get { return cv_Client.ConnectStatus; }
        }
        public void Open()
        {
            cv_Client.Open();
        }
        public RBController(string m_Ip, int m_Port)
            : base(CommonData.HIRATA.CommonStaticData.g_FDModuleName, "RobotController")
        {
            InitLog();
            if (!IpFormatCheck(m_Ip, m_Port))
            {
                Environment.Exit(-12);
            }
            cv_Commands = new List<CommandData>();
            cv_IsReturnNotFoundExeEvent = true;
            cv_Client = new HIRATADevice(m_Ip, m_Port);
            EventLink();
            cv_Client.ClientOpenConnect();
            //setTimer();
        }
        ~RBController()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            cv_Timer.Close();
            cv_Client.ClientCloseConnect();
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        public bool IpFormatCheck(string m_Ip, int m_Port)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, CommonData.HIRATA.CommonStaticData.__FUN(), CommonData.HIRATA.FunInOut.Enter);
            bool ipResuit = false;
            int OkTotal = 0;
            string ip = m_Ip;
            string[] sArray = ip.Split('.');
            if (sArray.Length != 4)
            {
                return false;
            }
            try
            {
                for (int i = 0; i <= sArray.Length - 1; i++)
                {
                    if (Convert.ToByte(sArray[i]) >= 0 && Convert.ToByte(sArray[i]) <= 255)
                    {
                        OkTotal++;
                    }
                }
                if (OkTotal == 4)
                {
                    ipResuit = true;
                }
            }
            catch (Exception e)
            {
                ipResuit = false;
            }
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return ipResuit;
        }
        public bool SendCommand(CommandData m_Data)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            bool rtn = false;
            string txt_command = m_Data.GetCommandStr();
            string reply_string = "";
            WriteLog(LogLevelType.General, "[Send] : " + txt_command);

            if (!cv_Client.Send(txt_command, out reply_string))
            {
                rtn = false;
            }
            else
            {
                m_Data.cv_Time = SysUtils.Now();
                /*
                lock (cv_Commands)
                {
                    cv_Commands.Add(m_Data);
                }
                */
                rtn = true;
            }
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return rtn;
        }
        protected override void InitLog()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            if (cv_Log == null)
            {
                cv_Log = new KFileLog();
            }
            string enviPath = CommonData.HIRATA.CommonStaticData.g_RootLogsFolderPath + CommonData.HIRATA.CommonStaticData.g_FDModuleName;

            cv_Log.LoadFromIni(CommonData.HIRATA.CommonStaticData.g_RootConfigFolderPath + CommonData.HIRATA.CommonStaticData.g_FDModuleName + "\\logs.ini", "ControlRaw");
            cv_Log.LogFileName = enviPath + "\\ControlRaw.log";
            cv_Log.SaveToIni(CommonData.HIRATA.CommonStaticData.g_RootConfigFolderPath + CommonData.HIRATA.CommonStaticData.g_FDModuleName + "\\logs.ini", "ControlRaw");
            cv_Log.WriteLog("Create ControlRaw");
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        private void setTimer()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            if (cv_Timer == null)
            {
                cv_Timer = new KTimer();
                cv_Timer.Interval = 100;
                cv_Timer.OnTimer += Timer_OnTimer;
                cv_Timer.ThreadEventEnabled = false;
                cv_Timer.Enabled = true;
                cv_Timer.Open();
            }
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        private void EventLink()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            cv_Client.OnConnectEvent += OnConnect;
            cv_Client.OnRecvEvent += OnRecv;
            cv_Client.OnSendEvent += OnSend;
            cv_Client.OnSendErrorEvent += OnSendError;
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        #region Link Decive Event
        private void OnConnect(bool m_IsConnect)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.General, "On Robot Connect : " + m_IsConnect.ToString(), FunInOut.None);
            if (OnConnectEvent != null)
            {
                OnConnectEvent(m_IsConnect);
            }
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        private void OnSendError(string m_CommandTxt, string m_Msg)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            if (OnSendErrorEvent != null)
            {
                OnSendErrorEvent(m_CommandTxt, m_Msg);
            }
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        private void OnSend(string m_CommandTxt)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.General, "[OnSend] : " + m_CommandTxt, FunInOut.None);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        private void OnRecv(string m_Txt)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.General, "[Recv] : " + m_Txt, CommonData.HIRATA.FunInOut.None);
            List<string> tmp = m_Txt.Split(',').ToList();
            for (int i = 0; i < tmp.Count; i++)
            {
                tmp[i] = tmp[i].Trim();
            }
            if (!Regex.Match(tmp[1], @"event", RegexOptions.IgnoreCase).Success)
            {
                lock (cv_Commands)
                {
                    // if (cv_Commands.Count > 0)
                    // {
                    CommandData rtn_command = new CommandData(tmp);//, origin.cv_CommandType);
                    bool is_find = false;
                    for (int i = 0; i < cv_Commands.Count; i++)
                    {
                        if (rtn_command == cv_Commands[i])
                        {
                            cv_Commands.RemoveAt(i);
                            is_find = true;
                        }
                    }
                    /*
                        if (!is_find)
                        {
                            WriteLog(LogLevelType.General, "[Recv] Command not found , maybe time out already : " + m_Txt, FunInOut.None);
                        }
                     */
                    if (OnRecvEvent != null)
                    {

                        /*
                        if (!is_find)
                        {
                            if (cv_IsReturnNotFoundExeEvent)
                            {
                                OnRecvEvent(rtn_command);
                                WriteLog(LogLevelType.RawData, "[Recv] : " + m_Txt);
                            }
                        }
                        else
                        {
                         * */
                        OnRecvEvent(rtn_command);
                        WriteLog(LogLevelType.RawData, "[Recv] : " + m_Txt);
                        //}
                    }
                    //}
                    //else
                    //{
                    //  WriteLog(LogLevelType.General, "[Recv] Command not found , maybe already ime out : " + m_Txt, FunInOut.None);
                    //}
                }
            }
            else if (Regex.Match(tmp[1], @"event", RegexOptions.IgnoreCase).Success)
            {
                CommandData command = new CommandData(tmp);//, origin.cv_CommandType);
                if (command.cv_ReturnCode == 0)
                {
                    //if (Regex.Match(tmp[3], @"place", RegexOptions.IgnoreCase).Success || Regex.Match(tmp[3], @"remove", RegexOptions.IgnoreCase).Success)
                    //{//00, Event,P1,FoupPlace
                    if (Enum.IsDefined(typeof(APIEnum.EventCommand), tmp[3]))
                    {
                        command.PEventCommand = (APIEnum.EventCommand)Enum.Parse(typeof(APIEnum.EventCommand), tmp[3]);
                    }
                    else
                    {
                        /*
                        //00 , Event , EMO,1
                        if (Enum.IsDefined(typeof(APIEnum.EventCommand), tmp[2]))
                        {
                            command.PEventCommand = (APIEnum.EventCommand)Enum.Parse(typeof(APIEnum.EventCommand), tmp[2]);
                        }
                        else
                        {
                            if (OnRecvParseError != null)
                            {
                                OnRecvParseError(m_Txt);
                                return;
                            }
                        }
                        */
                    }
                    Match match = Match.Empty;
                    match = Regex.Match(tmp[2], @"[1-9]", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        command.cv_DeviceId = Convert.ToInt16(match.Value.Trim());
                    }
                    match = Match.Empty;
                    match = Regex.Match(tmp[2], @"\D*", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        if (Enum.IsDefined(typeof(APIEnum.CommnadDevice), match.Value))
                        {
                            command.PCommandDevice = (APIEnum.CommnadDevice)Enum.Parse(typeof(APIEnum.CommnadDevice), match.Value);
                        }
                        else
                        {
                            if (OnRecvParseError != null)
                            {
                                OnRecvParseError(m_Txt);
                                return;
                            }
                        }
                    }
                    if (OnRecvEvent != null)
                    {
                        WriteLog(LogLevelType.RawData, "[Recv] : " + m_Txt);
                        OnRecvEvent(command);
                    }
                    //}
                }
                else
                {//03,Event,EFEM,82,ADAM_A1 Connent Socket Port Error
                    //(00),(Event),(EMO,1) 
                    command.PEventCommand = APIEnum.EventCommand.ERROR;
                    Match match = Match.Empty;
                    match = Regex.Match(tmp[2], @"[1-9]", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        command.cv_DeviceId = Convert.ToInt16(match.Value.Trim());
                    }
                    match = Match.Empty;
                    match = Regex.Match(tmp[2], @"\D*", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        if (Enum.IsDefined(typeof(APIEnum.CommnadDevice), match.Value))
                        {
                            command.PCommandDevice = (APIEnum.CommnadDevice)Enum.Parse(typeof(APIEnum.CommnadDevice), match.Value);
                        }
                    }
                    command.cv_ReplyParaList.AddRange(tmp.GetRange(3, tmp.Count - 3));

                    if (OnRecvEvent != null)
                    {
                        WriteLog(LogLevelType.RawData, "[Recv] : " + m_Txt);
                        OnRecvEvent(command);
                    }
                }
            }
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        #endregion

        void Timer_OnTimer()
        {
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            try
            {
                lock (cv_Commands)
                {
                    if (cv_Commands.Count > 0)
                    {
                        for (int i = 0; i < cv_Commands.Count; i++)
                        {
                            CommandData cur_command = cv_Commands[i];
                            Int64 diff = SysUtils.MilliSecondsBetween(SysUtils.Now(), cur_command.cv_Time);
                            if (diff < 0)
                            {
                                cur_command.cv_Time = SysUtils.Now();
                            }
                            else if (diff > BaseForm.cv_TimeoutData.PApiT3TIme)
                            {
                                if (OnRecvTimeOutEvent != null)
                                {
                                    OnRecvTimeOutEvent(cur_command);
                                }
                                cv_Commands.RemoveAt(i);
                                WriteLog(LogLevelType.General, "[Timeout] Remove : " + cur_command.GetCommandStr());
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
    }
}

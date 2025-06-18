using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Timers;
using System.Net;
using KgsCommon;
using LGC;
using CommonData.HIRATA;

namespace HirataRbAPI
{
    class HIRATADevice : LogBase
    {
        KCriticalSection cv_Cs = new KCriticalSection();
        public delegate void DeleConnect(bool m_IsConnect);
        public DeleConnect OnConnectEvent;

        public delegate void DeleRecv(string m_Txt);
        public DeleRecv OnRecvEvent;

        public delegate void DeleSendError(string m_CommandTxt, string m_Msg);
        public DeleSendError OnSendErrorEvent;

        public delegate void DeleSend(string m_CommandTxt);
        public DeleSend OnSendEvent;

        public string ConnectIP { get; set; }
        public int ConnectPort { get; set; }
        public bool ConnectStatus { get; set; }
        public TcpClient g_TcpClient = new TcpClient();
        public NetworkStream network;
        private KTimer cv_ClientTimer = new KTimer();

        bool cv_IsWait = false;
        public HIRATADevice(string m_Ip, int m_Port) : base(CommonData.HIRATA.CommonStaticData.g_FDModuleName , "RobotDevice")
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.General, "Device contructor IP : " + m_Ip + " , Port : " + m_Port.ToString());
            ConnectIP = m_Ip;
            ConnectPort = m_Port;
            InitLog();
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        public void ClientOpenConnect()
        {
            WriteLog(LogLevelType.NormalFunctionInOut ,  this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name , CommonData.HIRATA.FunInOut.Enter);
            try
            {
                if (!ConnectStatus)
                {
                    if (cv_IsWait) return;
                    cv_IsWait = true;
                    ConnectStatus = false;
                    IPAddress ConnectIPAddress = IPAddress.Parse(ConnectIP);
                    IPEndPoint ConnectIPEndPoint = new IPEndPoint(ConnectIPAddress, ConnectPort);
                    g_TcpClient = null;
                    g_TcpClient = new TcpClient();
                    g_TcpClient.Connect(ConnectIP, ConnectPort);
                    Recieve();
                    //ConnectStatus = true;
                    cv_IsWait = false;
                    network = g_TcpClient.GetStream();
                    cv_ClientTimer.Interval = 200;
                    cv_ClientTimer.OnTimer += new KTimer.KTimerEvent(timer_TickTock);
                    cv_ClientTimer.Enabled = false;
                    //ClientTimer.Open();
                }
            }
            catch (Exception ex)
            {
                cv_IsWait = false;
                if (network != null)
                {
                    network.Close();
                }
                g_TcpClient.Close();
                if (!cv_ClientTimer.Enabled)
                {
                    cv_ClientTimer.Interval = 200;
                    cv_ClientTimer.OnTimer += new KTimer.KTimerEvent(timer_TickTock);
                    cv_ClientTimer.Enabled = true;
                    cv_ClientTimer.Open();
                }
            }
            WriteLog(LogLevelType.NormalFunctionInOut ,  this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name , CommonData.HIRATA.FunInOut.Leave);
        }
        public void ClientCloseConnect()
        {
            WriteLog(LogLevelType.NormalFunctionInOut ,  this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name , CommonData.HIRATA.FunInOut.Enter);
            try
            {
                cv_IsWait = false;
                if (network != null)
                    network.Close();
                if (g_TcpClient != null)
                    g_TcpClient.Close();
            }
            catch (Exception e)
            {

            }
            WriteLog(LogLevelType.NormalFunctionInOut ,  this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name , CommonData.HIRATA.FunInOut.Leave);
        }
        public void Close()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            ClientCloseConnect();
            cv_ClientTimer.Enabled = false;
            cv_ClientTimer.Close();
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        public void Open()
        {
            WriteLog(LogLevelType.NormalFunctionInOut ,  this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name , CommonData.HIRATA.FunInOut.Enter);
            ClientCloseConnect();
            ClientOpenConnect();
            cv_ClientTimer.Enabled = true;
            cv_ClientTimer.Open();
            WriteLog(LogLevelType.NormalFunctionInOut ,  this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name , CommonData.HIRATA.FunInOut.Leave);
        }
        public bool CheckConnectStatus()
        {
            WriteLog(LogLevelType.TimerFunction ,  this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name , CommonData.HIRATA.FunInOut.Enter);
            bool tmp_connect = false;
            if (g_TcpClient.Client != null)
            {
                if (g_TcpClient.Connected)
                {
                    tmp_connect = true;
                }
                else
                {
                    tmp_connect = false;
                }
            }
            if (ConnectStatus != tmp_connect)
            {
                ConnectStatus = tmp_connect;
                if (OnConnectEvent != null)
                {
                    OnConnectEvent(ConnectStatus);
                    WriteLog(LogLevelType.General, "Exe OnConnect event : " + ConnectStatus.ToString());
                }
            }
            WriteLog(LogLevelType.TimerFunction ,  this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name , CommonData.HIRATA.FunInOut.Leave);
            return ConnectStatus ? true : false;
        }

        string RecieveData = "";
        private void timer_TickTock()
        {
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            bool closed = false;
            byte[] testByte = new byte[1];
            if (g_TcpClient.Client != null)
            {
                if (!g_TcpClient.Connected)
                {
                    closed = true;
                }
                else if (g_TcpClient.Connected && g_TcpClient.Client.Poll(0, SelectMode.SelectRead))
                {
                    try
                    {
                        closed = g_TcpClient.Client.Receive(testByte, SocketFlags.Peek) == 0;
                    }
                    catch (SocketException se)
                    {
                        closed = true;
                    }
                }
            }
            else
            {
                closed = true;
            }

            if (closed)
            {
                ClientCloseConnect();
                ClientOpenConnect();
                cv_ClientTimer.Enabled = true;
                cv_ClientTimer.Open();
            }

            CheckConnectStatus();
            RecieveData += Recieve();
            if (RecieveData != "")
            {
                RecieveData = RecieveData.Trim('\0');
                WriteLog(LogLevelType.General, "[Recv] : " + RecieveData, CommonData.HIRATA.FunInOut.None);
                while (true)
                {
                    int stx_index = RecieveData.IndexOf((char)0x2);
                    int etx_index = RecieveData.IndexOf((char)0x3);
                    if ((stx_index == -1) || (etx_index == -1))
                    {
                        break;
                    }
                    string txt = RecieveData.Substring(stx_index + 1, etx_index - stx_index - 1);
                    RecieveData = RecieveData.Substring(etx_index + 1, RecieveData.Length - etx_index - 1);
                    if (OnRecvEvent != null)
                    {
                        OnRecvEvent(txt);
                    }
                }
            }
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        public bool Send(string txt, out string m_ReplyMsg)
        {
            WriteLog(LogLevelType.NormalFunctionInOut ,  this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name , CommonData.HIRATA.FunInOut.Enter);
            WriteLog(LogLevelType.General ,  "[Send] : " + txt , CommonData.HIRATA.FunInOut.None);
            bool rtn = false;
            m_ReplyMsg = String.Empty;
            if (ConnectStatus)
            {
                cv_Cs.Enter();
                try
                {
                    Byte[] myBytes = Encoding.ASCII.GetBytes(txt);
                    network.Write(myBytes, 0, myBytes.Length);
                    WriteLog(LogLevelType.RawData, "[Send] : " + txt, CommonData.HIRATA.FunInOut.None);
                    rtn = true;
                    if (OnSendEvent != null)
                    {
                        OnSendEvent(txt);
                        WriteLog(LogLevelType.RawData, "[Exe OnSendEvent] : " + txt, CommonData.HIRATA.FunInOut.None);
                    }
                }
                catch (Exception ex)
                {
                    WriteLog(LogLevelType.Error, "[ERROR] : " + "Write : " + ex.ToString() , CommonData.HIRATA.FunInOut.None);
                    m_ReplyMsg = ex.Message;
                    rtn = false;
                }
                cv_Cs.Leave();
            }
            if (!rtn)
            {
                WriteLog(LogLevelType.RawData, "[Send] ERROR : " + txt, CommonData.HIRATA.FunInOut.None);
                m_ReplyMsg = "Disconnection \r\n";
                if (OnSendErrorEvent != null)
                {
                    OnSendErrorEvent(txt, m_ReplyMsg);
                    WriteLog(LogLevelType.RawData, "[Exe OnSendErrorEvent] : " + txt, CommonData.HIRATA.FunInOut.None);
                }
            }
            WriteLog(LogLevelType.NormalFunctionInOut ,  this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name , CommonData.HIRATA.FunInOut.Leave);
            return rtn;
        }
        public string Recieve()
        {
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            String recvTxt = String.Empty;
            int bufferSize;
            byte[] myBufferBytes;
            try
            {
                if (ConnectStatus)
                {
                    //if(network.DataAvailable)
                    if (g_TcpClient.Available > 0)
                    {
                        bufferSize = g_TcpClient.ReceiveBufferSize;
                        myBufferBytes = new byte[bufferSize];
                        network.Read(myBufferBytes, 0, bufferSize);
                        recvTxt = System.Text.Encoding.UTF8.GetString(myBufferBytes);
                        WriteLog(LogLevelType.RawData, "[Recv] : " + recvTxt.Trim((char)0), CommonData.HIRATA.FunInOut.None);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            return recvTxt;
        }
        protected override void InitLog()
        {
            WriteLog(LogLevelType.NormalFunctionInOut ,  this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name , CommonData.HIRATA.FunInOut.Enter);
            if (cv_Log == null)
            {
                cv_Log = new KFileLog();
            }
            string enviPath = CommonData.HIRATA.CommonStaticData.g_RootLogsFolderPath + CommonData.HIRATA.CommonStaticData.g_FDModuleName;

            cv_Log.LoadFromIni(CommonData.HIRATA.CommonStaticData.g_RootConfigFolderPath + CommonData.HIRATA.CommonStaticData.g_FDModuleName + "\\logs.ini", "RobotRaw");
            cv_Log.LogFileName = enviPath + "\\RobotRaw.log";
            cv_Log.SaveToIni(CommonData.HIRATA.CommonStaticData.g_RootConfigFolderPath + CommonData.HIRATA.CommonStaticData.g_FDModuleName + "\\logs.ini", "RobotRaw");
            cv_Log.WriteLog("Create RobotRaw");
            WriteLog(LogLevelType.NormalFunctionInOut ,  this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name , CommonData.HIRATA.FunInOut.Leave);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using CommonData.HIRATA;
using KgsCommon;
using LGC.Comm;
using System.Linq;
using BaseAp;
namespace LGC
{
    class Robot : Obj
    {
        public bool cv_IsRestartFinished;
        public int cv_WaitRobotSpeed = 0;
        public Queue<TowerCommand> cv_TowerJobQ = new Queue<TowerCommand>();
        public  Queue<bool> cv_BuzzerQ = new Queue<bool>();
        private delegate void DeleProcessCommand(CommandData m_Command);
        private Dictionary<APIEnum.CommandType, DeleProcessCommand> cv_ProcessCommandPtr = new Dictionary<APIEnum.CommandType, DeleProcessCommand>();

        private RobotJob cv_CurJob = null;
        public RobotJob CurJob
        {
            get { return cv_CurJob; }
            set { cv_CurJob = value; }
        }
        public RobotComm cv_Comm = null;
        public RobotData cv_Data = null;
        private KTimer cv_GifTimer = null;
        public bool IsBusy
        {
            get { return CurJob != null; }
        }

        public Robot(int m_Id, int m_SlotCount)
            : base(m_Id, m_SlotCount)
        {
            cv_IsRestartFinished = false;
            InitData();
            InitComm();
            InitTimer();
        }

        protected override void InitComm()
        {
            if (cv_Comm == null)
            {
                cv_Comm = new RobotComm();
            }
        }
        protected override void InitData()
        {
            if (cv_Data == null)
            {
                cv_Data = new RobotData(cv_Id, cv_SlotCount);
            }
        }

        private void InitTimer()
        {
            if (cv_GifTimer == null)
            {
                cv_GifTimer = new KTimer();
                cv_GifTimer.Interval = 200;
                cv_GifTimer.Open();
                cv_GifTimer.Enabled = true;
                cv_GifTimer.ThreadEventEnabled = true;
                cv_GifTimer.OnTimer += OnGIFTimer;
            }
        }

        public void AddRobotJob(RobotJob m_Job)
        {
            CurJob = m_Job;
        }
        public bool IsHasAnyDataAndSensor()
        {
            bool rtn = false;
            rtn = cv_Data.TheSlotHasDataOrSensor(RobotArm.rbaUp);
            if(!rtn)
            {
                rtn = cv_Data.TheSlotHasDataOrSensor(RobotArm.rbaDown);
            }
            return rtn;
        }
        public bool SetRobotTransferAction(CommandData m_Command, RobotJob m_Job)
        {
            bool rtn = false;
            if (CanDoTransfer())
            {
                if (LgcModule.SetRobotTransferAction(m_Command))
                {
                    setRobotStatusRun();
                    AddRobotJob(m_Job);
                    rtn = true;
                }
            }
            return rtn;
        }
        private bool CanDoTransfer()
        {
            bool rtn = false;
            if ((lgcBase.PSystemData.POperationMode != OperationMode.Auto))
            {
                rtn = true;
            }
            if (rtn)
            {
                if (lgcBase.cv_Alarms.IsHasAlarm(this.PSideGroup))
                {
                    rtn = false;
                }
                else
                {
                    if (IsBusy)
                    {
                        rtn = false;
                    }
                }
            }
            return rtn;
        }

        private void setRobotStatusRun()
        {
            if(cv_Id == 1)
            {
                lgcBase.PSystemData.PRobot1Status = EquipmentStatus.Run;
            }
            else if(cv_Id == 2)
            {
                lgcBase.PSystemData.PRobot2Status = EquipmentStatus.Run;
            }
        }

        private void OnGIFTimer()
        {
        }
        protected GlassData this[int index]
        {
            get
            {
                GlassData rtn = null;
                if (index > 0 && index <= cv_SlotCount)
                {
                    rtn = cv_Data.GlassDataMap[index];
                }
                return rtn;
            }
            set
            {
                if (index > 0 && index <= cv_SlotCount)
                {
                    cv_Data.GlassDataMap[index] = value;
                    SendDataViaMmf();
                }
            }
        }
        public override void SendDataViaMmf()
        {
            cv_Data.SaveToFile();
            LGCController.triggerLgcEvent(typeof(CommonData.HIRATA.RobotData).Name, this.cv_Data);
        }

        #region process robot action complete
        public void ProcessRobotGetStandbyArmExtend(CommandData m_Command, RobotJob job)
        {
            /*
            LgcModule.WriteLog(LogLevelType.General , "Recv Robot  : " + m_Command.GetCommandStr());
            Robot robot = LgcModule.GetRobotById(job.PRobotId);
            int eq_time_chart_cur_step = 0;
            int time_chart_id = -1;
            TimechartNormal time_chart_instance = null;
            if (m_Command.PRobotCommand == APIEnum.RobotCommand.GetStandbyArmExtend)
            {
                if(job.PAction == RobotAction.GetStandbyArmExtend && job.PTarget == ActionTarget.Eq &&
                    job.PTargetId == (int)EqId.VAS && job.PTargetSlot == 1)
                {
                    eq_time_chart_cur_step = LgcModule.GetEqById(4).GetTimeChatCurStep(1);
                    time_chart_id = (int)EqGifTimeChartId.TIMECHART_ID_VAS_DOWN;
                    time_chart_instance = (TimechartNormal)LgcModule.cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
                    if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotGetVasStandByEnd)
                    {
                        LgcModule.cv_Mio.SetPortValue(time_chart_instance.cv_RobotBitStart + 
                            (int)RobotSideBitAddressOffset.Robot_Delivery_Ready, 1);
                        //time_chart_instance.SetTrigger(time_chart_id);
                    }
                }
                if(job.PAction == RobotAction.GetStandbyArmExtend)
                {
                    CurJob = null;
                }
            }
            */
        }
        public void ProcessRobotPutStandbyArmExtend(CommandData m_Command, RobotJob job)
        {
            /*
            LgcModule.WriteLog(LogLevelType.General, "Recv Robot  : " + m_Command.GetCommandStr());
            Robot robot = LgcModule.GetRobotById(job.PRobotId);
            int eq_time_chart_cur_step = 0;
            int time_chart_id = -1;
            TimechartNormal time_chart_instance = null;
            if (m_Command.PRobotCommand == APIEnum.RobotCommand.PutStandbyArmExtend)
            {
                if (job.PAction == RobotAction.PutStandbyArmExtend && job.PTarget == ActionTarget.Eq &&
                    job.PTargetId == (int)EqId.VAS)// && job.PTargetSlot == 1)
                {
                    if (job.PTargetSlot == 1)
                    {
                        eq_time_chart_cur_step = LgcModule.GetEqById((int)EqId.VAS).GetTimeChatCurStep(1);
                        time_chart_id = (int)EqGifTimeChartId.TIMECHART_ID_VAS_DOWN;
                        time_chart_instance = (TimechartNormal)LgcModule.cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
                        if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotPutVasStandByEnd)
                        {
                            LgcModule.cv_Mio.SetPortValue(time_chart_instance.cv_RobotBitStart +
                                (int)RobotSideBitAddressOffset.Robot_Delivery_Ready, 1);
                        }
                    }
                    if(job.PAction == RobotAction.PutStandbyArmExtend)
                    {
                        CurJob = null;
                    }
                }
            }
            */
        }
        public void ProcessRobotTopPutExtend(CommandData m_Command, RobotJob job)
        {
            /*
            LgcModule.WriteLog(LogLevelType.General, "Recv Robot  : " + m_Command.GetCommandStr());
            Robot robot = LgcModule.GetRobotById(job.PRobotId);
            int eq_time_chart_cur_step = 0;
            int time_chart_id = -1;
            TimechartNormal time_chart_instance = null;
            if (m_Command.PRobotCommand == APIEnum.RobotCommand.TopPutStandbyArmExtend)
            {
                if (job.PAction == RobotAction.TopPutStandbyArmExtend && job.PTarget == ActionTarget.Eq &&
                    job.PTargetId == (int)EqId.VAS)// && job.PTargetSlot == 1)
                {
                    if (job.PTargetSlot == 2)
                    {
                        eq_time_chart_cur_step = LgcModule.GetEqById((int)EqId.VAS).GetTimeChatCurStep(2);
                        time_chart_id = (int)EqGifTimeChartId.TIMECHART_ID_VAS_UP;
                        time_chart_instance = (TimechartNormal)LgcModule.cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
                        if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotPutVasStandByEnd)
                        {
                            LgcModule.cv_Mio.SetPortValue(time_chart_instance.cv_RobotBitStart +
                                (int)RobotSideBitAddressOffset.Robot_Delivery_Ready, 1);
                        }
                    }
                }
                if (job.PAction == RobotAction.TopPutStandbyArmExtend)
                {
                    CurJob = null;
                }
            }
            */
        }
        public void ProcessRobotTopPutWait(CommandData m_Command, RobotJob job)
        {
            Robot robot = LgcModule.GetRobotById(job.PRobotId);
            if (m_Command.PRobotCommand == APIEnum.RobotCommand.TopPutStandby)
            {
                if (job.PAction == RobotAction.TopPutWait)
                {
                    CurJob = null;
                }
            }
        }
        public void ProcessRobotTopGetWait(CommandData m_Command, RobotJob job)
        {
            Robot robot = LgcModule.GetRobotById(job.PRobotId);
            if (m_Command.PRobotCommand == APIEnum.RobotCommand.TopGetStandby)
            {
                if (job.PAction == RobotAction.TopGetWait)
                {
                    CurJob = null;
                }
            }
        }
        public void ProcessRobotTopPut(CommandData m_Command, RobotJob job)
        {
            //Robot robot = LgcModule.GetRobotById(job.PRobotId);
            bool robot_sensor = cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor;
            if (m_Command.PRobotCommand == APIEnum.RobotCommand.TopWaferPut)
            {
                if (job.PAction == RobotAction.TopPut)
                {
                    if (job.PTarget == ActionTarget.Eq && (job.PTargetId == (int)EqId.VAS1 || job.PTargetId == (int)EqId.VAS2) && 
                        job.PTargetSlot == 2)
                    {
                        LgcModule.g_eventController.SendBcTreansferReport(DataFlowAction.Send, cv_Data.GlassDataMap[(int)job.PPutArm]);

                        cv_Data.GlassDataMap[(int)job.PPutArm] = new GlassData();
                        cv_Data.GlassDataMap[(int)job.PPutArm].cv_SlotInEq = (uint)job.PPutArm;
                        cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor = robot_sensor;
                        SendDataViaMmf();
                        cv_Data.SaveToFile();
                    }
                    CurJob = null;
                    if(lgcBase.PSystemData.PRobot1Status == EquipmentStatus.Run)
                    {
                        lgcBase.PSystemData.PRobot1Status = EquipmentStatus.Idle;
                    }
                }
            }
        }
        public void ProcessRobotTopGet(CommandData m_Command, RobotJob job)
        {
            Robot robot = LgcModule.GetRobotById(job.PRobotId);
            if (m_Command.PRobotCommand == APIEnum.RobotCommand.TopWaferGet)
            {
                if (job.PAction == RobotAction.TopGet)
                {
                    if (job.PTarget == ActionTarget.Eq)
                    {
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm] = this.cv_Comm.cv_GlassDataGetFromEq;
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].cv_SlotInEq = (uint)job.PGetArm;
                        robot.cv_Data.GlassDataMap = robot.cv_Data.GlassDataMap;
                        //Global.Controller.SendMmfNotifyObject(typeof(RobotData).Name, robot.cv_Data, KgsCommon.KParseObjToXmlPropertyType.Field);
                    }
                    if (!job.PUseHs)
                    {
                        if (job.PAction != RobotAction.Exchange)
                        {
                            CurJob = null;
                        }
                    }
                }
            }
        }
        public void ProcessRobotPutWait(CommandData m_Command, RobotJob job)
        {
            Robot robot = LgcModule.GetRobotById(job.PRobotId);
            if (m_Command.PRobotCommand == APIEnum.RobotCommand.PutStandby)
            {
                if (job.PAction == RobotAction.PutWait)
                {
                    CurJob = null;
                }
            }
        }
        public void ProcessRobotGetWait(CommandData m_Command, RobotJob job)
        {
            Robot robot = LgcModule.GetRobotById(job.PRobotId);
            if (m_Command.PRobotCommand == APIEnum.RobotCommand.GetStandby)
            {
                if (job.PAction == RobotAction.GetWait)
                {
                    CurJob = null;
                }
            }
        }
        public void ProcessRobotPut(CommandData m_Command, RobotJob job)
        {
            LgcModule.WriteLog(LogLevelType.General , "Recv Robot put : " + m_Command.GetCommandStr());
            Robot robot = LgcModule.GetRobotById(job.PRobotId);
            if (m_Command.PRobotCommand == APIEnum.RobotCommand.WaferPut)
            {
                if (job.PAction == RobotAction.Put)
                {
                    bool robot_sensor = robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor;
                    if (job.PTarget == ActionTarget.Port)
                    {
                        Port port = LgcModule.GetPortById(job.PTargetId);
                        port.cv_Data.GlassDataMap[job.PTargetSlot] = robot.cv_Data.GlassDataMap[(int)job.PPutArm];
                        //direct copy sensror.
                        port.cv_Data.GlassDataMap[job.PTargetSlot].cv_SlotInEq = (uint)job.PTargetSlot;
                        port.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor = true;

                        robot.cv_Data.GlassDataMap[(int)job.PPutArm] = new GlassData();
                        robot.cv_Data.GlassDataMap[(int)job.PPutArm].PSlotInEq = (uint)job.PPutArm;
                        robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor = robot_sensor;

                        if (port.cv_Data.PPortMode == PortMode.Unloader)
                        {
                            if (port.PLotStatus == LotStatus.Reserved) port.PLotStatus = LotStatus.Process;
                        }

                        robot.SendDataViaMmf();
                        port.SendDataViaMmf();
                        port.cv_Data.SaveToFile();
                        robot.cv_Data.SaveToFile();
                        LgcModule.g_eventController.SendBcTreansferReport(DataFlowAction.Store, port.cv_Data.GlassDataMap[job.PTargetSlot], (int)port.cv_Data.cv_Id,
                            (int)port.cv_Data.GlassDataMap[job.PTargetSlot].cv_SlotInEq);
                           
                        if(lgcBase.PSystemData.PONT)
                        {
                            bool is_unlod = true;
                            for(int i=1 ; i<= port.cv_SlotCount ; i++)
                            {
                                if(port.cv_Data.GlassDataMap[i].POcrResult == OCRResult.None &&
                                    port.cv_Data.GlassDataMap[i].PHasSensor && port.cv_Data.GlassDataMap[i].PHasData) 
                                {
                                    is_unlod = false;
                                }
                            }
                            if(is_unlod)
                            {
                                LgcModule.SetPortUnloadAction(port.cv_Id);
                            }
                        }
                    }
                    else if (job.PTarget == ActionTarget.Aligner)
                    {
                        Aligner aligner = LgcModule.GetAlignerById(job.PTargetId);
                        aligner.cv_Data.GlassDataMap[job.PTargetSlot] = robot.cv_Data.GlassDataMap[(int)job.PPutArm];
                        aligner.cv_Data.GlassDataMap[job.PTargetSlot].cv_SlotInEq = (uint)job.PTargetSlot;
                        //aligner.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor = true;
                        LgcModule.SetStatus(APIEnum.CommnadDevice.Aligner, 1);
                        robot.cv_Data.GlassDataMap[(int)job.PPutArm] = new GlassData();
                        robot.cv_Data.GlassDataMap[(int)job.PPutArm].PSlotInEq = (uint)job.PPutArm;
                        robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor = robot_sensor;
                        aligner.SendDataViaMmf();
                        robot.SendDataViaMmf();
                        aligner.cv_Data.SaveToFile();
                        robot.cv_Data.SaveToFile();
                    }
                    else if (job.PTarget == ActionTarget.Buffer)
                    {
                        Buffer buffer = LgcModule.GetBufferById(job.PTargetId);
                        buffer.cv_Data.GlassDataMap[job.PTargetSlot] = robot.cv_Data.GlassDataMap[(int)job.PPutArm];
                        buffer.cv_Data.GlassDataMap[job.PTargetSlot].cv_SlotInEq = (uint)job.PTargetSlot;
                        buffer.cv_Data.GlassDataMap[job.PTargetSlot].PEnterBufferTime = SysUtils.Now();
                        robot.cv_Data.GlassDataMap[(int)job.PPutArm] = new GlassData();
                        robot.cv_Data.GlassDataMap[(int)job.PPutArm].PSlotInEq = (uint)job.PPutArm;
                        //robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor = false;
                        robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor = robot_sensor;
                        buffer.SendDataViaMmf();
                        robot.SendDataViaMmf();
                        //cv_Comm.SetStatus(APIEnum.CommnadDevice.Robot);
                        LgcModule.SetStatus(APIEnum.CommnadDevice.Buffer , buffer.cv_Id);
                        robot.cv_Data.SaveToFile();
                        buffer.cv_Data.SaveToFile();
                    }
                    else if (job.PTarget == ActionTarget.Eq)
                    {
                        LgcModule.g_eventController.SendBcTreansferReport(DataFlowAction.Send, robot.cv_Data.GlassDataMap[(int)job.PPutArm]);

                        robot.cv_Data.GlassDataMap[(int)job.PPutArm] = new GlassData();
                        robot.cv_Data.GlassDataMap[(int)job.PPutArm].PSlotInEq = (uint)job.PPutArm;
                        //robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor = false;
                        robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor = robot_sensor;
                        robot.SendDataViaMmf();
                        robot.cv_Data.SaveToFile();
                        //cv_Comm.SetStatus(APIEnum.CommnadDevice.Robot);
                    }
                    if (job.PAction == RobotAction.Exchange && job.PTarget == ActionTarget.Aligner)
                    {
                        LgcModule.WriteLog(LogLevelType.General, "Set robot job null.");
                    }
                    else
                    {
                        CurJob = null;
                        if (lgcBase.PSystemData.PRobot1Status == EquipmentStatus.Run)
                        {
                            lgcBase.PSystemData.PRobot1Status = EquipmentStatus.Idle;
                        }
                        LgcModule.WriteLog(LogLevelType.General, "Set robot job null.");
                    }
                }
            }
        }
        public void ProcessRobotGet(CommandData m_Command, RobotJob job)
        {
            LgcModule.WriteLog(LogLevelType.General , "Recv Robot Get : " + m_Command.GetCommandStr());
            if (m_Command.PRobotCommand == APIEnum.RobotCommand.WaferGet)
            {
                Robot robot = LgcModule.GetRobotById(job.PRobotId);
                bool robot_sensor = robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor;
                if (job.PAction == RobotAction.Get)
                {
                    if (job.PTarget == ActionTarget.Port)
                    {
                        Port port = LgcModule.GetPortById(job.PTargetId);
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm] = port.cv_Data.GlassDataMap[job.PTargetSlot];
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].cv_SlotInEq = (uint)job.PGetArm;
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor = robot_sensor;
                        port.cv_Data.GlassDataMap[job.PTargetSlot] = new GlassData();
                        port.cv_Data.GlassDataMap[job.PTargetSlot].cv_SlotInEq =(uint)job.PTargetSlot;
                        port.SendDataViaMmf();
                        robot.SendDataViaMmf();
                        port.cv_Data.SaveToFile();
                        robot.cv_Data.SaveToFile();
                        LgcModule.g_eventController.SendBcTreansferReport(DataFlowAction.Fetch, robot.cv_Data.GlassDataMap[(int)job.PGetArm] , (int)port.cv_Data.cv_Id,
                            (int)job.PTargetSlot);
                        if(!port.cv_Data.HasOtherJobHaveToDo())
                        {
                            LgcModule.g_eventController.SendBcLastSubstrateReport(robot.cv_Data.GlassDataMap[(int)job.PGetArm], (int)port.cv_Data.cv_Id, job.PTargetSlot);
                        }

                    }
                    else if (job.PTarget == ActionTarget.Buffer)
                    {
                        Buffer buffer = LgcModule.GetBufferById(job.PTargetId);
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm] = buffer.cv_Data.GlassDataMap[job.PTargetSlot];
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].cv_SlotInEq = (uint)job.PGetArm;
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor = robot_sensor;
                        buffer.cv_Data.GlassDataMap[job.PTargetSlot] = new GlassData();
                        buffer.cv_Data.GlassDataMap[job.PTargetSlot].cv_SlotInEq = (uint)job.PTargetSlot;
                        robot.SendDataViaMmf();
                        buffer.SendDataViaMmf();
                        LgcModule.SetStatus(APIEnum.CommnadDevice.Buffer , buffer.cv_Id);
                        buffer.cv_Data.SaveToFile();
                        robot.cv_Data.SaveToFile();
                    }
                    else if (job.PTarget == ActionTarget.Aligner)
                    {
                        Aligner Aligner = LgcModule.GetAlignerById(job.PTargetId);
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm] = Aligner.cv_Data.GlassDataMap[job.PTargetSlot];
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].cv_SlotInEq = (uint)job.PGetArm;
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor = robot_sensor;
                        Aligner.cv_Data.GlassDataMap[job.PTargetSlot] = new GlassData();
                        Aligner.cv_Data.GlassDataMap[job.PTargetSlot].PSlotInEq = (uint)job.PTargetSlot;
                        //Aligner.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor = false;
                        Aligner.SendDataViaMmf();
                        robot.SendDataViaMmf();
                        LgcModule.SetStatus(APIEnum.CommnadDevice.Aligner, 1);
                        Aligner.cv_Data.SaveToFile();
                        robot.cv_Data.SaveToFile();
                       // cv_Comm.SetStatus(APIEnum.CommnadDevice.Robot);
                       // cv_Comm.SetStatus(APIEnum.CommnadDevice.Aligner, 1);
                    }
                    else if (job.PTarget == ActionTarget.Eq)
                    {
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].cv_SlotInEq = (uint)job.PGetArm;
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor = robot_sensor;
                        robot.SendDataViaMmf();
                        robot.cv_Data.SaveToFile();
                    }
                    CurJob = null;
                    if (lgcBase.PSystemData.PRobot1Status == EquipmentStatus.Run)
                    {
                        lgcBase.PSystemData.PRobot1Status = EquipmentStatus.Idle;
                    }
                    LgcModule.WriteLog(LogLevelType.General , "Set robot job null.");
                }
            }
        }
        public void ProcessRobotStop(CommandData m_Command)
        {
            Robot robot = LgcModule.GetRobotById(1);
            if (m_Command.PRobotCommand == APIEnum.RobotCommand.Stop)
            {
                lgcBase.PSystemData.PRobot1Status = EquipmentStatus.Stop;
            }
        }
        #endregion


    }
}

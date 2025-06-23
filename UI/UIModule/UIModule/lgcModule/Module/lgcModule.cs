using System;
using System.Collections.Generic;
using CommonData.HIRATA;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using KgsCommon;
using CommonData;
using BaseAp;
using System.Reflection;
using System.Diagnostics;

namespace LGC
{
    public partial class LgcModule : lgcBase
    {
        public static LgcModule g_LgcModule = null;
        public static KMemoryIOClient PMio
        {
            //get { return g_LgcModule.cv_Mio1; }
            get { return g_eventController.cv_TimechartController.GetmemoryIoClient(); }
        }

        public static Dictionary<int, List<AllDevice>> cv_CurRecipeFlowStepSetting = new Dictionary<int, List<AllDevice>>();
        public static List<int> cv_InProcessPort = new List<int>();
        internal static Dictionary<int, Eq> cv_EqContainer = new Dictionary<int, Eq>();
        internal static Dictionary<int, Port> cv_PortContainer = new Dictionary<int, Port>();
        internal static Dictionary<int, Robot> cv_RobotContainer = new Dictionary<int, Robot>();
        internal static Dictionary<int, Buffer> cv_BufferContainer = new Dictionary<int, Buffer>();
        internal static Dictionary<int, Aligner> cv_AlignerContainer = new Dictionary<int, Aligner>();
        internal static Dictionary<enSideGroup, List<InitialAllDevice>> cv_SideGroup = new Dictionary<enSideGroup, List<InitialAllDevice>>();

        internal static bool cv_IsCycleStop = false;
        internal bool cv_CheckEqDataLocalMode = false;
        public static int cv_WaitFfuSpeed;
        //MMF
        internal static LGCController g_eventController = null;

        KDateTime cv_DataTime = SysUtils.Now();
        KDateTime cv_WaitUvRecordTime = SysUtils.Now();
        KTimer cv_RobotActionTimer;
        public LgcModule()
            : base(FdModule.LGC)
        {
            g_LgcModule = this;
            InitEventController();
            ReadGlassCountValueFromPlc();
            LoadAlarmTable();
            initRbController();
            g_eventController.SetTimeChartTimeOut();
            layoutInit();
            ParserFlowStep();
            initTimer();
            g_eventController.initTimer();
            PMio.SetPortValue(0x344d, (int)lgcBase.PSystemData.POcrMode + (1 << 4));
            WriteLog(LogLevelType.General, "[LGC module start]");
            MDLgcStart lgcstart = new MDLgcStart();
            LGCController.triggerLgcEvent(typeof(MDLgcStart).Name.ToString(), lgcstart);
            LGCController.triggerLgcEvent(typeof(SystemData).Name.ToString(), PSystemData);
            cv_Timer.Start();
        }

        public static void WriteLog(LogLevelType m_Type, string m_str, CommonData.HIRATA.FunInOut m_FunInOut = CommonData.HIRATA.FunInOut.None)
        {
            string log = "";
            int level = (int)(SamekLogLevelType)Enum.Parse(typeof(SamekLogLevelType), m_Type.ToString());
            if (m_Type == LogLevelType.NormalFunctionInOut)
            {
                if (m_FunInOut != CommonData.HIRATA.FunInOut.None)
                {
                    log = "[FUN_" + m_FunInOut.ToString() + " ]" + m_str;
                    if (m_FunInOut == FunInOut.Leave)
                    {
                        log += "\n---------------------------------------------";
                    }
                }
            }
            else if (m_Type == LogLevelType.TimerFunction)
            {
                if (m_FunInOut != CommonData.HIRATA.FunInOut.None)
                {
                    log = "[Timer FUN_" + m_FunInOut.ToString() + " ]" + m_str;
                }
            }
            else
            {
                log = "[" + m_Type.ToString() + " ]" + m_str;
            }

            if (g_LgcModule != null)
            {
                lock (g_LgcModule.cv_Log)
                {
                    try
                    {
                        g_LgcModule.cv_Log.WriteLog(log, level);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }
        private void InitEventController()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            if (g_eventController == null)
            {
                g_eventController = new LGCController();
            }
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void initLog()
        {
            base.initLog();
            initAlarmLog();
        }

        #region Alarm Event function
        //Trigger this event When AlarmData add/del successful.(LGC must override)
        protected override void OnAlarmActionEvent(AlarmStatus m_Action, List<CommonData.HIRATA.AlarmItem> m_Alarms)
        {
            CommonData.HIRATA.MDAlarmAction report_plc = new MDAlarmAction();
            for (int i = 0; i < m_Alarms.Count; i++)
            {
                if (m_Action == AlarmStatus.Clean)
                {
                    m_Alarms[i].PStatus = AlarmStatus.Clean;
                    report_plc.AlarmData.cv_AlarmList.Add(m_Alarms[i]);
                }
                else if (m_Action == AlarmStatus.Occur)
                {
                    m_Alarms[i].PStatus = AlarmStatus.Occur;
                    report_plc.AlarmData.cv_AlarmList.Add(m_Alarms[i]);
                }
            }
            if (g_eventController != null)
            {
                g_eventController.SendAlarmAction(report_plc.AlarmData, MmfEventClientEventType.etNotify);
            }
        }

        //Trigger this event When AlarmData change.(LGC must override)
        protected override void OnAlarmChange()
        {
            PSystemData.PSystemStatus = EquipmentStatus.Down;
            if (cv_Alarms.IsHasAlarm(enSideGroup.Both))
            {
                PSystemData.POperationModeLeft = OperationMode.Manual;
                PSystemData.POperationModeRight = OperationMode.Manual;
            }
            else if (cv_Alarms.IsHasAlarm(enSideGroup.Left))
            {
                PSystemData.POperationModeLeft = OperationMode.Manual;
            }
            else if (cv_Alarms.IsHasAlarm(enSideGroup.Right))
            {
                PSystemData.POperationModeRight = OperationMode.Manual;
            }
            if (g_eventController != null)
            {
                g_eventController.SendAlarmData();
            }
        }
        #endregion

        #region Recipe Event function
        //Trigger this event When RecipeData add/del/Modify successful.(LGC must override)
        protected override void OnRecipeActionEvent(DataEidtAction m_Action, List<RecipeItem> m_Recipes)
        {
            ParserFlowStep();
            if (g_eventController != null)
            {
                g_eventController.SendRecipeData();
                g_eventController.SendRecipeAction(m_Action, m_Recipes);
            }
        }
        #endregion

        #region Link Timeout data Event
        //Trigger this event When Time out data change(LGC must override)
        protected override void OnTimeOutDataChange()
        {
            if (cv_MmfController != null)
            {
                cv_MmfController.SendTimeoutData();
            }
        }
        #endregion

        #region Link GlassCount data Event
        //Trigger this event When glass count change.(LGC must override)
        protected override void OnGlassCountDataChange()
        {
            if (cv_MmfController != null)
            {
                cv_MmfController.SendGlassCountData();
            }
        }
        #endregion

        #region Link System data Event
        protected override void OnSystemStatusChange()
        {
            if (PSystemData.PSystemStatus == EquipmentStatus.Down)
            {
                AddTowerCommand(SignalTowerColor.All, SignalTowerControl.Off);
                AddTowerCommand(SignalTowerColor.Red, SignalTowerControl.On);
                if(PSystemData.POperationMode == OperationMode.Auto)
                {
                    PSystemData.POperationMode = OperationMode.Manual;
                }
            }
            else if (PSystemData.PSystemStatus == EquipmentStatus.Idle)
            {
                AddTowerCommand(SignalTowerColor.All, SignalTowerControl.Off);
                AddTowerCommand(SignalTowerColor.Yellow, SignalTowerControl.On);
            }
            else if (PSystemData.PSystemStatus == EquipmentStatus.Run)
            {
                AddTowerCommand(SignalTowerColor.All, SignalTowerControl.Off);
                AddTowerCommand(SignalTowerColor.Green, SignalTowerControl.On);
            }
            if (PSystemData.PRobotConnect)
            {
                for (int i = (int)EqGifTimeChartId.TIMECHART_ID_SDP1; i <= (int)EqGifTimeChartId.TIMECHART_ID_UV_2; i++)
                {
                    if (PSystemData.PSystemStatus != EquipmentStatus.Down)
                    {
                        cv_Mio.SetPortValue(cv_MmfController.cv_TimechartController.GetTimeChartInstance(i).cv_RobotBitStart + (int)RobotSideBitAddressOffset.Active_Standby, 1);
                        cv_Mio.SetPortValue(cv_MmfController.cv_TimechartController.GetTimeChartInstance(i).cv_RobotBitStart + (int)RobotSideBitAddressOffset.Interlock_2, 1);
                    }
                    else if (PSystemData.PSystemStatus == EquipmentStatus.Down)
                    {
                        cv_Mio.SetPortValue(cv_MmfController.cv_TimechartController.GetTimeChartInstance(i).cv_RobotBitStart + (int)RobotSideBitAddressOffset.Active_Standby, 0);
                        cv_Mio.SetPortValue(cv_MmfController.cv_TimechartController.GetTimeChartInstance(i).cv_RobotBitStart + (int)RobotSideBitAddressOffset.Interlock_2, 1);

                    }
                }
            }
            else
            {
                for (int i = (int)EqGifTimeChartId.TIMECHART_ID_SDP1; i <= (int)EqGifTimeChartId.TIMECHART_ID_UV_2; i++)
                {
                    cv_Mio.SetPortValue(cv_MmfController.cv_TimechartController.GetTimeChartInstance(i).cv_RobotBitStart + (int)RobotSideBitAddressOffset.Active_Standby, 0);
                    cv_Mio.SetPortValue(cv_MmfController.cv_TimechartController.GetTimeChartInstance(i).cv_RobotBitStart + (int)RobotSideBitAddressOffset.Interlock_2, 1);
                }
            }
        }
        protected override void OnRobotStatusChange()
        {
            if (PSystemData.PSystemStatus == EquipmentStatus.Down)
            {
                PSystemData.PInitaiizeOk = false;
                PSystemData.PInitaiizing = false;
                GetRobotById(1).CurJob = null; // manual is ok , auto mode : buz has check sensor , so almost ok.
                cv_RobotManaulJobPath.Clear();
                cv_RobotJobPath.Clear();
                SendRobotJobPath();
            }
            else if (PSystemData.PSystemStatus == EquipmentStatus.Idle)
            {
            }
            else if (PSystemData.PSystemStatus == EquipmentStatus.Run)
            {
            }
        }
        //Trigger this event When system data change.(LGC must override)
        protected override void OnSystemDataChange()
        {
            if (cv_MmfController != null)
            {
                cv_MmfController.SendSystemData();
            }
        }
        #endregion

        private bool FindUnloadPortToPutSubstrate(out int m_Port, out int m_Slot, RobotJob m_Job)
        {
            bool rtn = false;
            int port = 1;
            int slot = 1;
            TimechartNormal time_chart_instance = null;
            int time_chart_id = (int)EqGifTimeChartId.TIMECHART_ID_UV_1;
            GlassData glass = null;
            if (m_Job.PTarget == ActionTarget.Eq && (m_Job.PTargetId == (int)EqId.UV_1))
            {
                time_chart_instance = (TimechartNormal)g_eventController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
                try
                {
                    glass = new GlassData(PMio, time_chart_instance.cv_ReadDataStartPort);
                }
                catch (Exception e)
                {
                    WriteLog(LogLevelType.Error, "[FindUnloadPortToPutSubstrate] new glass error.");
                    //ShowMsg("[FindUnloadPortToPutSubstrate] new glass data from UV is Error", false, false);
                }
            }

            if (glass == null)
            {
                WriteLog(LogLevelType.Error, "[FindUnloadPortToPutSubstrate] new glass is null.");
                //ShowMsg("[FindUnloadPortToPutSubstrate]  glass data from UV is Error", false, false);
                m_Port = 0;
                m_Slot = 0;
                return false;
            }
            if (glass.PFoupSeq == 0)
            {
                WriteLog(LogLevelType.Error, "[FindUnloadPortToPutSubstrate] UV glass data Error(FoupSeq 0 ).");
                //ShowMsg("[FindUnloadPortToPutSubstrate]  UV glass data Error(FoupSeq 0 ).", false, false);
                m_Port = 0;
                m_Slot = 0;
                return false;
            }

            //first find the same seq port
            for (int port_id = 0; port_id < cv_InProcessPort.Count; port_id++)
            {
                Port job_port = GetPortById(cv_InProcessPort[port_id]);
                if (job_port.cv_Data.PPortMode == PortMode.Unloader)
                {
                    if (job_port.PPortStatus == PortStaus.LDCM)// && job_port.PLotStatus == LotStatus.Process)// && job_port.cv_Data.PPortMode == PortMode.Unloader)
                    {
                        if (job_port.PLotStatus == LotStatus.Process || job_port.PLotStatus == LotStatus.Reserved)
                        {
                            if (job_port.cv_Data.HasDataOrSensor())
                            {
                                if (job_port.cv_Data.SeqTheSameWithSubstrate(glass.PFoupSeq))
                                {
                                    int tmp_slot = 0;
                                    if (job_port.cv_Data.WhichSlotCanLoad(out tmp_slot))
                                    {
                                        m_Port = job_port.cv_Id;
                                        m_Slot = tmp_slot;
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // find empty foup to put.
            for (int port_id = 0; port_id < cv_InProcessPort.Count; port_id++)
            {
                Port job_port = GetPortById(cv_InProcessPort[port_id]);
                if (job_port.cv_Data.PPortMode == PortMode.Unloader)
                {
                    if (job_port.PPortStatus == PortStaus.LDCM)// && job_port.PLotStatus == LotStatus.Process)// && job_port.cv_Data.PPortMode == PortMode.Unloader)
                    {
                        if (job_port.PLotStatus == LotStatus.Process || job_port.PLotStatus == LotStatus.Reserved)
                        {
                            if (!job_port.cv_Data.HasDataOrSensor())
                            {
                                int tmp_slot = 0;
                                if (job_port.cv_Data.WhichSlotCanLoad(out tmp_slot))
                                {
                                    m_Port = job_port.cv_Id;
                                    m_Slot = tmp_slot;
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            if (!rtn)
            {
                ShowMsg("Unload Port to put substrate not found , please check!!!", true, false);
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = Alarmtable.CannotFindUnloadPortSlotToPutSubstrate.ToString();
                alarm.PMainDescription = "Cannot Find Unload Port Slot To Put Substrate";
                alarm.PUnit = 0;
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PStatus = AlarmStatus.Occur;
                alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
                EditAlarm(alarm);
            }
            m_Port = port;
            m_Slot = slot;
            return rtn;
        }

        private void ParserFlowStep()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            cv_CurRecipeFlowStepSetting = null;
            cv_CurRecipeFlowStepSetting = new Dictionary<int, List<AllDevice>>();
            KIniFile stepIni = new KIniFile(CommonStaticData.g_FlowStepSettingFile);
            Dictionary<string, string> tmp = new Dictionary<string, string>();
            RecipeItem recipe = null;
            if (lgcBase.cv_Recipes.GetCurRecipe(out recipe))
            {
                string section = recipe.PFlow.ToString().Substring(4);
                stepIni.ReadSection(section, tmp);
                foreach (KeyValuePair<string, string> pair in tmp)
                {
                    Match match = Match.Empty;
                    match = Regex.Match(pair.Key, @"\d", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        int step_id = Convert.ToInt16(match.Value);
                        List<string> steps = pair.Value.Split(',').ToList();
                        List<AllDevice> step_devices = new List<AllDevice>();
                        foreach (string step_item in steps)
                        {
                            if (Regex.Match(step_item, @"LP").Success)
                            {
                                step_devices.Add(AllDevice.LP);
                            }
                            else if (Regex.Match(step_item, @"UP").Success)
                            {
                                step_devices.Add(AllDevice.UP);
                            }
                            else if (Regex.Match(step_item, @"Buffer1").Success)
                            {
                                step_devices.Add(AllDevice.Buffer1_Left);
                            }
                            else if (Regex.Match(step_item, @"Buffer2").Success)
                            {
                                step_devices.Add(AllDevice.Buffer2_Mid);
                            }
                            else if (Regex.Match(step_item, @"Aligner1").Success)
                            {
                                step_devices.Add(AllDevice.Aligner1_Left);
                            }
                            else if (Regex.Match(step_item, @"Aligner2").Success)
                            {
                                step_devices.Add(AllDevice.Aligner2_Right);
                            }
                            else if (Regex.Match(step_item, @"EQ").Success)
                            {
                                int eq_id = Convert.ToInt16(step_item.Substring(2));
                                EqId enumid = (EqId)eq_id;
                                AllDevice all_device_item = (AllDevice)Enum.Parse(typeof(AllDevice), enumid.ToString());
                                step_devices.Add(all_device_item);
                            }
                        }
                        cv_CurRecipeFlowStepSetting[step_id] = step_devices;
                    }
                }
            }
            string log = "Set recipe flow : " + Environment.NewLine;
            foreach (KeyValuePair<int, List<AllDevice>> item in cv_CurRecipeFlowStepSetting)
            {
                log += "Step : " + item.Key + " : ";
                foreach (AllDevice device_item in item.Value)
                {
                    log += device_item.ToString() + "  ";
                }
                log += Environment.NewLine;
            }
            WriteLog(LogLevelType.General, log);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        private void AddTowerCommand(SignalTowerColor m_Color, SignalTowerControl m_Control)
        {
            Robot robot = GetRobotById(1);
            if (robot != null)
            {
                if (robot.cv_TowerJobQ != null)
                {
                    TowerCommand tmp = new TowerCommand(m_Color, m_Control);
                    robot.cv_TowerJobQ.Enqueue(tmp);
                }
            }
        }
        public static void AddBuzzerCommand(bool m_IsOn)
        {
            Robot robot = GetRobotById(1);
            if (robot != null)
            {
                if (robot.cv_BuzzerQ != null)
                {
                    robot.cv_BuzzerQ.Enqueue(m_IsOn);
                }
            }
        }
        protected override void ModuleInit()
        {
            InitRecipe();
            InitAlarmData();
            InitTimeoutData();
            InitSystemData();
            InitGlassCount();


            KIniFile ini = new KIniFile(CommonData.HIRATA.CommonStaticData.g_LgcModuleSystemIniFile);
            //check eq data at local mode.
            if (ini.ReadString("Config", "CheckEqDataLocalMode", "1").Trim() == "1")
            {
                cv_CheckEqDataLocalMode = true;
                WriteLog(LogLevelType.Detail, "Set cv_CheckEqDataLocalMode : 1");
            }
            else
            {
                cv_CheckEqDataLocalMode = false;
                WriteLog(LogLevelType.Detail, "Set cv_CheckEqDataLocalMode : 0");
            }
        }
        //when the port can start process , use this this AddPortToProcessList function.
        public static void AddPortToProcessList(int m_Port)
        {
            int index = cv_InProcessPort.FindIndex(x => x == m_Port);
            if (index != -1)
            {
                cv_InProcessPort.RemoveAt(index);
            }
            cv_InProcessPort.Add(m_Port);
        }
        public static void RemovePortToProcessList(int m_Port)
        {
            int index = cv_InProcessPort.FindIndex(x => x == m_Port);
            if (index != -1)
            {
                cv_InProcessPort.RemoveAt(index);
            }
        }
        private void CalculateSubstrateCount()
        {
            int production = 0;
            int dummy = 0;
            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
            {
                Port port = GetPortById(i);

                if(port.PPortStatus == PortStaus.LDCM)
                {
                    for (int slot = 1; slot <= port.cv_Data.cv_SlotCount; slot++)
                    {
                        if (port.cv_Data.GlassDataMap[slot].PHasData && port.cv_Data.GlassDataMap[slot].PHasSensor)
                        {
                            if (port.cv_Data.GlassDataMap[slot].PProductionCategory == ProductCategory.Dummy)
                                dummy++;
                            else if (port.cv_Data.GlassDataMap[slot].PProductionCategory == ProductCategory.Glass)
                                production++;
                            else if (port.cv_Data.GlassDataMap[slot].PProductionCategory == ProductCategory.Wafer)
                                production++;
                        }
                    }
                }
            }
            Robot robot = GetRobotById(1);
            for (int slot = 1; slot <= 2; slot++)
            {
                if (robot.cv_Data.GlassDataMap[slot].PHasData && robot.cv_Data.GlassDataMap[slot].PHasSensor)
                {
                    if (robot.cv_Data.GlassDataMap[slot].PProductionCategory == ProductCategory.Dummy)
                        dummy++;
                    else if (robot.cv_Data.GlassDataMap[slot].PProductionCategory == ProductCategory.Glass)
                        production++;
                    else if (robot.cv_Data.GlassDataMap[slot].PProductionCategory == ProductCategory.Wafer)
                        production++;
                }
            }
            Buffer buffer = GetBufferById(1);
            for (int slot = 1; slot <= buffer.cv_SlotCount; slot++)
            {
                if (buffer.cv_Data.GlassDataMap[slot].PHasData && buffer.cv_Data.GlassDataMap[slot].PHasSensor)
                {
                    if (buffer.cv_Data.GlassDataMap[slot].PProductionCategory == ProductCategory.Dummy)
                        dummy++;
                    else if (buffer.cv_Data.GlassDataMap[slot].PProductionCategory == ProductCategory.Glass)
                        production++;
                    else if (buffer.cv_Data.GlassDataMap[slot].PProductionCategory == ProductCategory.Wafer)
                        production++;
                }
            }
            Aligner aligner = GetAlignerById(1);
            for (int slot = 1; slot <= aligner.cv_SlotCount; slot++)
            {
                if (aligner.cv_Data.GlassDataMap[slot].PHasData && aligner.cv_Data.GlassDataMap[slot].PHasSensor)
                {
                    if (aligner.cv_Data.GlassDataMap[slot].PProductionCategory == ProductCategory.Dummy)
                        dummy++;
                    else if (aligner.cv_Data.GlassDataMap[slot].PProductionCategory == ProductCategory.Glass)
                        production++;
                    else if (aligner.cv_Data.GlassDataMap[slot].PProductionCategory == ProductCategory.Wafer)
                        production++;
                }
            }

            bool is_change = false;
            if (lgcBase.cv_GlassCountData.PProductCount != production)
            {
                is_change = true;
                lgcBase.cv_GlassCountData.PProductCount = production;
            }
            if (lgcBase.cv_GlassCountData.PDummyCount != dummy)
            {
                is_change = true;
                lgcBase.cv_GlassCountData.PDummyCount = dummy;
            }
            /*
            if (is_change)
                cv_MmfController.SendGlassCountData();
            */
        }
        private void initTimer()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            if (cv_RobotActionTimer == null)
            {
                cv_RobotActionTimer = new KTimer();
                cv_RobotActionTimer.Interval = 200;
                cv_RobotActionTimer.ThreadEventEnabled = false;
                cv_RobotActionTimer.Enabled = true;
                cv_RobotActionTimer.OnTimer += OnRobotActionTimer;
            }
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        private void DerivedTimer()
        {
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            SendTowerCommand();
            SendBuzzerCommand();
            CalculateSubstrateCount();
            CalculateSystemStatus();
            DoPortChangeToLDRQ();
            WriteRealDataToBc();
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        //Write real time data to BC
        private void WriteRealDataToBc()
        {
            Robot robot = GetRobotById(1);
            Aligner aligner = GetAlignerById(1);
            Buffer buffer = GetBufferById(1);

            robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].WriteWokeNoOnly(PMio, 0x381A);
            robot.cv_Data.GlassDataMap[(int)RobotArm.rbaUp].WriteWokeNoOnly(PMio, 0x381C);
            aligner.cv_Data.GlassDataMap[1].WriteWokeNoOnly(PMio, 0x381E);
            for (int i = 1; i <= buffer.cv_SlotCount; i++)
            {
                buffer.cv_Data.GlassDataMap[i].WriteWokeNoOnly(PMio, 0x3820 + ((i - 1) << 1));
            }
        }
        void SendTowerCommand()
        {
            Robot robot = GetRobotById(1);
            if (lgcBase.PSystemData.PapiInlineMode != EquipmentInlineMode.Remote) return;
            if (robot.cv_TowerJobQ.Count != 0)
            {
                TowerCommand tmp = GetRobotById(1).cv_TowerJobQ.Peek();
                if (!tmp.cv_HadSend)
                {
                    SetSignalTower(tmp.cv_Color, tmp.cv_Control);
                    tmp.cv_HadSend = true;
                }
            }
        }
        void SendBuzzerCommand()
        {
            Robot robot = GetRobotById(1);
            if (lgcBase.PSystemData.PapiInlineMode != EquipmentInlineMode.Remote) return;
            if (robot.cv_BuzzerQ.Count != 0)
            {
                //bool tmp = GetRobotById(1).cv_BuzzerQ.Peek();
                bool tmp = GetRobotById(1).cv_BuzzerQ.Dequeue();
                SetBuzzer(tmp);
            }
        }
        private void CalculateSystemStatus()
        {
            bool has_foup = false;
            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
            {
                if (GetPortById(i).PPortStatus == PortStaus.LDCM)
                {
                    has_foup = true;
                    break;
                }
            }

            if (lgcBase.PSystemData.PSystemStatus == EquipmentStatus.WaitIdle)
            {
                long diff = SysUtils.MilliSecondsBetween(SysUtils.Now(), lgcBase.PSystemData.PIdleTime);
                if (diff > lgcBase.cv_TimeoutData.PIdleDelayTime)
                {
                    lgcBase.PSystemData.PSystemStatus = EquipmentStatus.Idle;
                    AddTowerCommand(SignalTowerColor.All, SignalTowerControl.Off);
                    AddTowerCommand(SignalTowerColor.Yellow, SignalTowerControl.On);

                }
                else if (diff < 0)
                {
                    lgcBase.PSystemData.PIdleTime = SysUtils.Now();
                }
            }

            if (has_foup)
            {
                if (!lgcBase.cv_Alarms.IsHasAlarm())
                {
                    if (lgcBase.PSystemData.PSystemStatus != EquipmentStatus.Run)
                    {
                        lgcBase.PSystemData.PSystemStatus = EquipmentStatus.Run;
                        AddTowerCommand(SignalTowerColor.All, SignalTowerControl.Off);
                        AddTowerCommand(SignalTowerColor.Green, SignalTowerControl.On);
                    }
                }
                else if (lgcBase.PSystemData.PSystemStatus != EquipmentStatus.Down)
                {
                    lgcBase.PSystemData.PSystemStatus = EquipmentStatus.Down;
                    AddTowerCommand(SignalTowerColor.All, SignalTowerControl.Off);
                    AddTowerCommand(SignalTowerColor.Red, SignalTowerControl.On);
                    if (lgcBase.PSystemData.POperationModeLeft == OperationMode.Auto)
                    {
                        lgcBase.PSystemData.POperationModeLeft = OperationMode.Manual;
                    }
                }
            }
            else
            {
                if (!cv_Alarms.IsHasAlarm())
                {
                    if (PSystemData.PSystemStatus != EquipmentStatus.WaitIdle && PSystemData.PSystemStatus != EquipmentStatus.Idle)
                    {
                        if(PSystemData.PSystemStatus == EquipmentStatus.None)
                        {
                            //if(PSystemData.PRobot1Connect)
                            if(PSystemData.PapiConnect)
                            PSystemData.PSystemStatus = EquipmentStatus.Idle;
                            else
                            PSystemData.PSystemStatus = EquipmentStatus.Down;
                        }
                        else
                        {
                            PSystemData.PSystemStatus = EquipmentStatus.WaitIdle;
                        }
                    }
                }
                else if (PSystemData.PSystemStatus != EquipmentStatus.Down)
                {
                    PSystemData.PSystemStatus = EquipmentStatus.Down;
                    if (PSystemData.POperationModeLeft == OperationMode.Auto)
                    {
                        PSystemData.POperationModeLeft = OperationMode.Manual;
                    }
                }
            }
        }
        private void DoPortChangeToLDRQ()
        {
            Port job_port = null;
            for (int port_id = 1; port_id <= CommonData.HIRATA.CommonStaticData.g_PortNumber; port_id++)
            {
                job_port = GetPortById(port_id);
                if (job_port.PPortStatus == PortStaus.UDCM)
                {
                    long diff = SysUtils.MilliSecondsBetween(SysUtils.Now(), job_port.PLDRQTime);
                    if (diff > 2000)
                    {
                        job_port.PPortStatus = PortStaus.LDRQ;
                        job_port.cv_Data.Clear();
                        job_port.PLDRQTime = SysUtils.Now();
                    }
                    else if (diff < 0)
                    {
                        job_port.PLDRQTime = SysUtils.Now();
                    }
                }
            }
        }
        private void DoCstUnload()
        {
            Port job_port = null;
            if (lgcBase.PSystemData.POperationModeLeft != OperationMode.Auto)
            {
                return;
            }
            for (int port_id = 1; port_id <= CommonData.HIRATA.CommonStaticData.g_PortNumber; port_id++)
            {
                job_port = GetPortById(port_id);
                if (job_port.PPortStatus == PortStaus.LDCM)
                {
                    if (job_port.cv_Data.cv_IsWaitCancel)
                    {
                        if (CheckThePortCanUnload(port_id))
                        {
                            if (job_port.PLotStatus != LotStatus.Process && job_port.PLotStatus != LotStatus.ProcessEnd)
                            {
                                job_port.PLotStatus = LotStatus.Cancel;
                            }
                        }
                    }
                    else if (job_port.cv_Data.cv_IsWaitAbort)
                    {
                        if (CheckThePortCanUnload(port_id))
                        {
                            if (job_port.PLotStatus == LotStatus.Process)
                            {
                                job_port.PLotStatus = LotStatus.Abort;
                            }
                        }
                    }
                    if (job_port.PLotStatus == LotStatus.Process)
                    {
                        if (!job_port.cv_Data.HasOtherJobHaveToDo())
                        {
                            if (CheckThePortCanUnload(port_id))
                            {
                                if (lgcBase.PSystemData.PSystemOnlineMode == OnlineMode.Offline)
                                {
                                    job_port.PLotStatus = LotStatus.ProcessEnd;
                                    job_port.cv_Data.PWaitUnload = true;
                                }
                                else if (lgcBase.PSystemData.PSystemOnlineMode == OnlineMode.Control)
                                {
                                    job_port.PLotStatus = LotStatus.ProcessEnd;
                                }
                            }
                        }
                    }

                    if (job_port.cv_Data.PWaitUnload)
                    {
                        if (CheckThePortCanUnload(port_id))
                        {
                            if (job_port.PLotStatus == LotStatus.Process)
                            {
                                if (!job_port.cv_Data.HasOtherJobHaveToDo())
                                {
                                    job_port.PLotStatus = LotStatus.ProcessEnd;
                                }
                                else
                                {
                                    job_port.PLotStatus = LotStatus.Abort;
                                }
                            }
                            else
                            {
                                if (job_port.PLotStatus != LotStatus.Abort && job_port.PLotStatus != LotStatus.Cancel
                                    && job_port.PLotStatus != LotStatus.ProcessEnd)
                                {
                                    job_port.PLotStatus = LotStatus.Cancel;
                                }
                            }
                            RemovePortToProcessList(port_id);
                            job_port.PPortStatus = PortStaus.UDRQ;
                            job_port.cv_Data.cv_IsWaitAbort = false;
                            job_port.cv_Data.cv_IsWaitCancel = false;
                            SetPortUnloadAction(port_id);
                            job_port.cv_Data.PWaitUnload = false;
                        }
                    }
                }
            }
        }

        private ProductCategory GetSubstractTypeWantToGetFromCst()
        {
            return ProductCategory.Dummy;
            /*
            ProductCategory tmp = ProductCategory.Mask;
            int glass = 0;
            int wafer = 0;
            Buffer buffer = LgcForm.GetBufferById(1);
            for (int i = 1; i <= buffer.cv_Data.cv_SlotCount; i++)
            {
                if (buffer.cv_Data.GlassDataMap[i].PProductionCategory == ProductCategory.Glass)
                    glass++;
                if (buffer.cv_Data.GlassDataMap[i].PProductionCategory == ProductCategory.Wafer)
                    wafer++;
            }


            //
            EqId eq_id = EqId.VAS;
            int time_chart_instance = 0;
            int eq_time_chart_cur_step = 0;
            //   if (Enum.TryParse<EqId>(, out eq_id))
            {
                //  if (eq_id == EqId.VAS)
                {
                    eq_time_chart_cur_step = GetEqById((int)eq_id).GetTimeChatCurStep(2);
                    time_chart_instance = (int)EqGifTimeChartId.TIMECHART_ID_VAS_UP;
                }
            }
            if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_ActionReady)
            {
                EqInterFaceType gif_type = cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_instance).cv_ActionType;
                if (gif_type == EqInterFaceType.Load)
                {
                    //
                    bool is_first_can_load = false;
                    if (cv_CheckFirstStepWhenPutGlass)
                    {
                        List<AllDevice> dievices = cv_CurRecipeFlowStepSetting[2];
                        List<AllDevice> form_dievices = cv_CurRecipeFlowStepSetting[1];
                        foreach (AllDevice device in dievices)
                        {
                            if ((int)device >= ((int)AllDevice.SDP1) && (int)device <= ((int)AllDevice.UV_2))
                            {
                                if (!CheckFlowCanRun(1))
                                {
                                    tmp = ProductCategory.Glass;
                                    break;
                                }
                                EqId eq = (EqId)Enum.Parse(typeof(EqId), device.ToString());
                                if (GetEqById((int)eq).GetTimeChatCurStep(1) == TimechartNormal.STEP_ID_ActionReady)
                                {
                                    int first_time_chart_instance = GetEqById((int)eq).cv_Comm.cv_TimeChatId;
                                    if (eq == EqId.VAS)
                                    {
                                        first_time_chart_instance = (int)EqGifTimeChartId.TIMECHART_ID_VAS_DOWN;
                                    }
                                    EqInterFaceType first_gif_type = cv_MmfController.cv_TimechartController.GetTimeChartInstance(first_time_chart_instance).cv_ActionType;
                                    if (first_gif_type == EqInterFaceType.Load)
                                    {
                                        if (cv_RobotJobPath == null || cv_RobotJobPath.Count == 0)
                                        {
                                            is_first_can_load = true;
                                            tmp = ProductCategory.Wafer;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (!is_first_can_load)
                        {
                            tmp = ProductCategory.Glass;
                        }
                    }
                    else
                    {
                        tmp = ProductCategory.Glass;
                    }
                    //
                }
                else
                {
                    tmp = ProductCategory.Wafer;
                }
            }
            else
            {
                if (wafer < glass)
                    tmp = ProductCategory.Wafer;
                else if (wafer > glass)
                    tmp = ProductCategory.Glass;
                else
                    tmp = ProductCategory.Wafer;
            }
            //
            return tmp;
            */
        }

        #region do port unload
        private bool CheckThePortCanUnload(int m_PortId)
        {
            return false;
            /*
            bool rtn = true;
            Port port = LgcForm.GetPortById(m_PortId);
            bool is_job_port = false;
            foreach (RobotJob job in cv_RobotJobPath)
            {
                if (job.PTarget == ActionTarget.Port && job.PTargetId == m_PortId)
                {
                    is_job_port = true;
                }
            }
            if (!is_job_port)
            {
                if (port.cv_Data.PPortMode == PortMode.Loader)
                {
                    Aligner aligner = LgcForm.GetAlignerById(1);
                    Robot robot = LgcForm.GetRobotById(1);
                    if (PSystemData.POcrMode == OCRMode.ErrorReturn)
                    {
                        if (aligner.cv_Data.GlassDataMap[1].PHasData)
                        {
                            if (aligner.cv_Data.GlassDataMap[1].PSourcePort == m_PortId)
                            {
                                rtn = false;
                            }
                        }
                        else if (robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].PHasData)
                        {
                            if (robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].POcrResult == OCRResult.Fail ||
                                robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].POcrResult == OCRResult.Mismatch ||
                                robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].POcrResult == OCRResult.None)
                            {
                                rtn = false;
                            }
                        }
                        else if (robot.cv_Data.GlassDataMap[(int)RobotArm.rbaUp].PHasData)
                        {
                            if (robot.cv_Data.GlassDataMap[(int)RobotArm.rbaUp].POcrResult == OCRResult.Fail ||
                                robot.cv_Data.GlassDataMap[(int)RobotArm.rbaUp].POcrResult == OCRResult.Mismatch ||
                                robot.cv_Data.GlassDataMap[(int)RobotArm.rbaUp].POcrResult == OCRResult.None)
                            {
                                rtn = false;
                            }
                        }
                    }
                    else if (PSystemData.POcrMode == OCRMode.ErrorHold)
                    {
                        if (aligner.cv_Data.GlassDataMap[1].PHasData)
                        {
                            if (aligner.cv_Data.GlassDataMap[1].PSourcePort == m_PortId)
                            {
                                rtn = false;
                            }
                        }
                        else if (robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].PHasData)
                        {
                            if (robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].POcrDecide == OCRMode.ErrorReturn || robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].POcrDecide == OCRMode.None)
                            {
                                rtn = false;
                            }
                        }
                        else if (robot.cv_Data.GlassDataMap[(int)RobotArm.rbaUp].PHasData)
                        {
                            if (robot.cv_Data.GlassDataMap[(int)RobotArm.rbaUp].POcrDecide == OCRMode.ErrorReturn || robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].POcrDecide == OCRMode.None)
                            {
                                rtn = false;
                            }
                        }
                    }
                }
            }
            else
            {
                rtn = false;
            }
            return rtn;
            */
        }
        public bool HasProcessUnloadPort()
        {
            Port port = null;
            bool rtn = false;
            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
            {
                port = GetPortById(i);
                if (port.PPortStatus == PortStaus.LDCM && (port.PLotStatus == LotStatus.Process || port.PLotStatus == LotStatus.Reserved))
                {
                    rtn = true;
                }
            }
            return rtn;
        }
        #endregion

        #region Robot command
        public bool GetPutAligner(RobotArm m_Arm, bool IsGet)
        {
            Robot robot = GetRobotById(1);
            if (!robot.IsBusy)
            {
                APIEnum.RobotCommand robot_command = APIEnum.RobotCommand.None;
                if (IsGet)
                {
                    robot_command = APIEnum.RobotCommand.WaferGet;
                }
                else
                {
                    robot_command = APIEnum.RobotCommand.WaferPut;
                }
                List<string> para = new List<string>();
                para.Add(((int)m_Arm).ToString());
                para.Add("Aligner1");
                para.Add("1");
                RobotJob tmp_job = null;// new RobotJob(obj.RobotId, obj.Source.PArm, obj.PAction, obj.Source.PTarget, obj.Source.Id, obj.Source.Slot);
                if (IsGet)
                    tmp_job = new RobotJob(1, RobotArm.rabNone, m_Arm, RobotAction.Get, ActionTarget.Aligner, 1, 1, false);
                else
                    tmp_job = new RobotJob(1, m_Arm, RobotArm.rabNone, RobotAction.Put, ActionTarget.Aligner, 1, 1, false);
                CommandData tmp_command = new CommandData(APIEnum.CommandType.Robot, robot_command.ToString(),
                    APIEnum.CommnadDevice.Robot, 0, para);
                robot.SetRobotTransferAction(tmp_command, tmp_job);
            }
            return true;
        }
        public bool GetPutPort(RobotArm m_Arm, int m_Port, int m_Slot, bool IsGet)
        {
            Robot robot = GetRobotById(1);
            if (!robot.IsBusy)
            {
                APIEnum.RobotCommand robot_command = APIEnum.RobotCommand.None;
                if (IsGet)
                    robot_command = APIEnum.RobotCommand.WaferGet;
                else
                    robot_command = APIEnum.RobotCommand.WaferPut;
                List<string> para = new List<string>();
                para.Add(((int)m_Arm).ToString());
                para.Add("P" + m_Port.ToString());
                para.Add(m_Slot.ToString());
                RobotJob tmp_job = null;// new RobotJob(obj.RobotId, obj.Source.PArm, obj.PAction, obj.Source.PTarget, obj.Source.Id, obj.Source.Slot);
                if (IsGet)
                    tmp_job = new RobotJob(1, RobotArm.rabNone, m_Arm, RobotAction.Get, ActionTarget.Port, m_Port, m_Slot, false);
                else
                    tmp_job = new RobotJob(1, m_Arm, RobotArm.rabNone, RobotAction.Put, ActionTarget.Port, m_Port, m_Slot, false);
                CommandData tmp_command = new CommandData(APIEnum.CommandType.Robot, robot_command.ToString(),
                    APIEnum.CommnadDevice.Robot, 0, para);
                robot.SetRobotTransferAction(tmp_command, tmp_job);
            }
            return true;
        }
        public bool GetPutBuffer(RobotArm m_Arm, int m_BufferId, int m_Slot, bool IsGet)
        {
            Robot robot = GetRobotById(1);
            Buffer buffer = GetBufferById(1);
            if (!robot.IsBusy)
            {
                APIEnum.RobotCommand robot_command = APIEnum.RobotCommand.None;
                if (IsGet)
                    robot_command = APIEnum.RobotCommand.WaferGet;
                else
                    robot_command = APIEnum.RobotCommand.WaferPut;
                List<string> para = new List<string>();
                para.Add(((int)m_Arm).ToString());
                para.Add("Stage" + buffer.cv_Comm.cv_RobotPosition.ToString());//.cv_RobotPos.ToString());
                para.Add(m_Slot.ToString());
                RobotJob tmp_job = null;// new RobotJob(obj.RobotId, obj.Source.PArm, obj.PAction, obj.Source.PTarget, obj.Source.Id, obj.Source.Slot);
                if (IsGet)
                    tmp_job = new RobotJob(1, RobotArm.rabNone, m_Arm, RobotAction.Get, ActionTarget.Buffer, m_BufferId, m_Slot, false);
                else
                    tmp_job = new RobotJob(1, m_Arm, RobotArm.rabNone, RobotAction.Put, ActionTarget.Buffer, m_BufferId, m_Slot, false);
                CommandData tmp_command = new CommandData(APIEnum.CommandType.Robot, robot_command.ToString(), APIEnum.CommnadDevice.Robot, 0, para);
                robot.SetRobotTransferAction(tmp_command, tmp_job);
            }
            return true;
        }
        public bool GetPutNormalEq(RobotArm m_Arm, EqId m_EqId, int m_Slot, bool IsGet, bool m_UseHS = true)
        {
            Robot robot = GetRobotById(1);
            if (!robot.IsBusy)
            {
                int stage = GetEqById((int)m_EqId).cv_Comm.cv_RobotPosition;
                APIEnum.RobotCommand robot_command = APIEnum.RobotCommand.None;
                if (IsGet)
                    robot_command = APIEnum.RobotCommand.WaferGet;
                else
                    robot_command = APIEnum.RobotCommand.WaferPut;
                List<string> para = new List<string>();
                para.Add(((int)m_Arm).ToString());
                para.Add("Stage" + stage.ToString());
                para.Add(m_Slot.ToString());
                RobotJob tmp_job = null;// new RobotJob(obj.RobotId, obj.Source.PArm, obj.PAction, obj.Source.PTarget, obj.Source.Id, obj.Source.Slot);
                if (IsGet)
                    tmp_job = new RobotJob(1, RobotArm.rabNone, m_Arm, RobotAction.Get, ActionTarget.Eq, (int)m_EqId, m_Slot, m_UseHS);
                else
                    tmp_job = new RobotJob(1, m_Arm, RobotArm.rabNone, RobotAction.Put, ActionTarget.Eq, (int)m_EqId, m_Slot, m_UseHS);
                CommandData tmp_command = new CommandData(APIEnum.CommandType.Robot, robot_command.ToString(),
                    APIEnum.CommnadDevice.Robot, 0, para);
                robot.SetRobotTransferAction(tmp_command, tmp_job);
            }
            return true;
        }
        public bool PutVasSlot(int m_RobotId , EqId m_EqId , bool isSlot1, int m_Step, bool m_UseHS = true)
        {
            Robot robot = GetRobotById(m_RobotId);
            if (!robot.IsBusy)
            {
                int stage = GetEqById((int)m_EqId).cv_Comm.cv_RobotPosition;
                APIEnum.RobotCommand robot_command = APIEnum.RobotCommand.None;
                RobotAction action = RobotAction.None;
                if (!isSlot1) // slots 2.
                {
                    if (m_Step == 1)
                    {
                        robot_command = APIEnum.RobotCommand.TopPutStandbyArmExtend;
                        action = RobotAction.TopPutStandbyArmExtend;
                    }
                    if (m_Step == 2)
                    {
                        robot_command = APIEnum.RobotCommand.TopWaferPut;
                        action = RobotAction.TopPut;
                    }
                }
                else // slot 1.
                {
                    if (m_Step == 1)
                    {
                        robot_command = APIEnum.RobotCommand.PutStandbyArmExtend;
                        action = RobotAction.PutStandbyArmExtend;
                    }
                    if (m_Step == 2)
                    {
                        robot_command = APIEnum.RobotCommand.WaferPut;
                        action = RobotAction.Put;
                    }
                }

                List<string> para = new List<string>();
                if (isSlot1)
                    para.Add(((int)RobotArm.rbaUp).ToString());
                else
                    para.Add(((int)RobotArm.rbaDown).ToString());
                para.Add("Stage" + stage.ToString());
                //para.Add(isSlot1 ? "1" : "2");
                para.Add("1");
                RobotJob tmp_job = null;// new RobotJob(obj.RobotId, obj.Source.PArm, obj.PAction, obj.Source.PTarget, obj.Source.Id, obj.Source.Slot);
                if (isSlot1)
                {
                    tmp_job = new RobotJob(1, RobotArm.rbaUp, RobotArm.rabNone, action,
                        ActionTarget.Eq, (int)m_EqId, 1, m_UseHS);
                }
                else
                {
                    tmp_job = new RobotJob(1, RobotArm.rbaDown, RobotArm.rabNone, action,
                        ActionTarget.Eq, (int)m_EqId, 2, m_UseHS);
                }
                CommandData tmp_command = new CommandData(APIEnum.CommandType.Robot, robot_command.ToString(),
                    APIEnum.CommnadDevice.Robot, 0, para);
                robot.SetRobotTransferAction(tmp_command, tmp_job);
            }
            return true;
        }
        public static bool GetEqStandbyExceptVas(int m_EqId , int m_Slot , RobotArm m_Arm)
        {
            Robot robot = GetRobotById(1);
            RobotAction action = RobotAction.None;
            if (!robot.IsBusy)
            {
                int stage = GetEqById(m_EqId).cv_Comm.cv_RobotPosition;
                APIEnum.RobotCommand robot_command = APIEnum.RobotCommand.None;
                robot_command = APIEnum.RobotCommand.GetStandby;
                action = RobotAction.GetWait;
                List<string> para = new List<string>();
                para.Add(((int)m_Arm).ToString());
                para.Add("Stage" + stage.ToString());
                para.Add("1");
                RobotJob tmp_job = null;// new RobotJob(obj.RobotId, obj.Source.PArm, obj.PAction, obj.Source.PTarget, obj.Source.Id, obj.Source.Slot);
                tmp_job = new RobotJob(1, RobotArm.rabNone, m_Arm , action
                    , ActionTarget.Eq, m_EqId , 1, true);
                CommandData tmp_command = new CommandData(APIEnum.CommandType.Robot, robot_command.ToString(),
                    APIEnum.CommnadDevice.Robot, 0, para);
                robot.SetRobotTransferAction(tmp_command, tmp_job);
            }
            return true;
        }
        public static bool PutEqStandbyExceptVas(int m_EqId , int m_Slot , RobotArm m_Arm)
        {
            Robot robot = GetRobotById(1);
            RobotAction action = RobotAction.None;
            if (!robot.IsBusy)
            {
                int stage = GetEqById(m_EqId).cv_Comm.cv_RobotPosition;
                APIEnum.RobotCommand robot_command = APIEnum.RobotCommand.None;
                robot_command = APIEnum.RobotCommand.PutStandby;
                action = RobotAction.PutWait;
                List<string> para = new List<string>();
                para.Add(((int)m_Arm).ToString());
                para.Add("Stage" + stage.ToString());
                para.Add("1");
                RobotJob tmp_job = null;// new RobotJob(obj.RobotId, obj.Source.PArm, obj.PAction, obj.Source.PTarget, obj.Source.Id, obj.Source.Slot);
                tmp_job = new RobotJob(1, m_Arm, RobotArm.rabNone , action
                    , ActionTarget.Eq, m_EqId, 1, true);
                CommandData tmp_command = new CommandData(APIEnum.CommandType.Robot, robot_command.ToString(),
                    APIEnum.CommnadDevice.Robot, 0, para);
                robot.SetRobotTransferAction(tmp_command, tmp_job);
            }
            return true;
        }
        public static bool GetVasStandby(int m_RobotId ,EqId m_EqId)
        {
            Robot robot = GetRobotById(m_RobotId);
            RobotAction action = RobotAction.None;
            if (!robot.IsBusy)
            {
                int stage = GetEqById((int)m_EqId).cv_Comm.cv_RobotPosition;
                APIEnum.RobotCommand robot_command = APIEnum.RobotCommand.None;
                robot_command = APIEnum.RobotCommand.GetStandby;
                action = RobotAction.GetWait;
                List<string> para = new List<string>();
                para.Add(((int)RobotArm.rbaDown).ToString());
                para.Add("Stage" + stage.ToString());
                para.Add("1");
                RobotJob tmp_job = null;// new RobotJob(obj.RobotId, obj.Source.PArm, obj.PAction, obj.Source.PTarget, obj.Source.Id, obj.Source.Slot);
                tmp_job = new RobotJob(1, RobotArm.rabNone, RobotArm.rbaDown, action
                    , ActionTarget.Eq, (int)m_EqId, 1, true);
                CommandData tmp_command = new CommandData(APIEnum.CommandType.Robot, robot_command.ToString(),
                    APIEnum.CommnadDevice.Robot, 0, para);
                robot.SetRobotTransferAction(tmp_command, tmp_job);
            }
            return true;
        }
        public static bool PutVasStandby(int m_RobotId ,EqId m_EqId , bool isSlot1)
        {
            Robot robot = GetRobotById(m_RobotId);
            if (!robot.IsBusy)
            {
                int stage = GetEqById((int)m_EqId).cv_Comm.cv_RobotPosition;
                APIEnum.RobotCommand robot_command = APIEnum.RobotCommand.None;
                RobotAction action = RobotAction.None;
                if (isSlot1)
                {
                    robot_command = APIEnum.RobotCommand.PutStandby;
                    action = RobotAction.PutWait;
                }
                else
                {
                    robot_command = APIEnum.RobotCommand.TopPutStandby;
                    action = RobotAction.TopPutWait;
                }

                List<string> para = new List<string>();
                if (isSlot1)
                    para.Add(((int)RobotArm.rbaUp).ToString());
                else
                    para.Add(((int)RobotArm.rbaDown).ToString());
                para.Add("Stage" + stage.ToString());
                //para.Add(isSlot1 ? "1" : "2");
                para.Add("1");
                RobotJob tmp_job = null;// new RobotJob(obj.RobotId, obj.Source.PArm, obj.PAction, obj.Source.PTarget, obj.Source.Id, obj.Source.Slot);
                if (isSlot1)
                {
                    tmp_job = new RobotJob(1, RobotArm.rbaUp, RobotArm.rabNone, action,
                        ActionTarget.Eq, (int)m_EqId, 1, true);
                }
                else
                {
                    tmp_job = new RobotJob(1, RobotArm.rbaDown, RobotArm.rabNone, action,
                        ActionTarget.Eq, (int)m_EqId, 2, true);
                }
                CommandData tmp_command = new CommandData(APIEnum.CommandType.Robot, robot_command.ToString(),
                    APIEnum.CommnadDevice.Robot, 0, para);
                robot.SetRobotTransferAction(tmp_command, tmp_job);
            }
            return true;
        }
        public bool GetVas(int m_RobotId , EqId m_EqId , int m_Step, bool m_UseHS = true)
        {
            Robot robot = GetRobotById(m_RobotId);
            RobotAction action = RobotAction.None;
            if (!robot.IsBusy)
            {
                int stage = GetEqById((int)m_EqId).cv_Comm.cv_RobotPosition;
                APIEnum.RobotCommand robot_command = APIEnum.RobotCommand.None;
                robot_command = APIEnum.RobotCommand.GetStandbyArmExtend;
                if (m_Step == 1)
                {
                    robot_command = APIEnum.RobotCommand.GetStandbyArmExtend;
                    action = RobotAction.GetStandbyArmExtend;
                }
                if (m_Step == 2)
                {
                    robot_command = APIEnum.RobotCommand.WaferGet;
                    action = RobotAction.Get;
                }
                List<string> para = new List<string>();
                para.Add(((int)RobotArm.rbaDown).ToString());
                para.Add("Stage" + stage.ToString());
                para.Add("1");
                RobotJob tmp_job = null;// new RobotJob(obj.RobotId, obj.Source.PArm, obj.PAction, obj.Source.PTarget, obj.Source.Id, obj.Source.Slot);
                tmp_job = new RobotJob(1, RobotArm.rabNone, RobotArm.rbaDown, action
                    , ActionTarget.Eq, (int)m_EqId, 1, m_UseHS);
                CommandData tmp_command = new CommandData(APIEnum.CommandType.Robot, robot_command.ToString(),
                    APIEnum.CommnadDevice.Robot, 0, para);
                robot.SetRobotTransferAction(tmp_command, tmp_job);
            }
            return true;
        }
        #endregion

        #region Port slot to access
        public int GetPortFree(CommonData.HIRATA.ProductCategory m_Type, int m_Port)
        {
            int slot = 0;
            Port port = GetPortById(m_Port);
            if (port.PLotStatus == LotStatus.Process)
            {
                for (int index = 1; index <= port.cv_SlotCount; index++)
                {
                    if (!port.cv_Data.GlassDataMap[index].PHasSensor)
                    {
                        if (!port.cv_Data.GlassDataMap[index].PHasSensor)
                        {
                            slot = index;
                            break;
                        }
                    }
                }
            }
            return slot;
        }
        public int GetBufferFree(CommonData.HIRATA.ProductCategory m_Type)
        {
            int slot = 0;
            Buffer buffer = GetBufferById(1);
            if (m_Type == ProductCategory.Wafer)
            {
                for (int index = 1; index <= 3; index++)
                {
                    if (!buffer.cv_Data.GlassDataMap[index].PHasSensor)
                    {
                        if (!buffer.cv_Data.GlassDataMap[index].PHasSensor)
                        {
                            slot = index;
                            break;
                        }
                    }
                }
            }
            else if (m_Type == ProductCategory.Wafer)
            {
                for (int index = 4; index <= 6; index++)
                {
                    if (!buffer.cv_Data.GlassDataMap[index].PHasSensor)
                    {
                        if (!buffer.cv_Data.GlassDataMap[index].PHasSensor)
                        {
                            slot = index;
                            break;
                        }
                    }
                }
            }
            return slot;
        }
        #endregion

        private RobotArm GetRobotArmEnumString(string m_Arm)
        {
            RobotArm arm = RobotArm.rabNone;
            if (Regex.Match(m_Arm, @"up", RegexOptions.IgnoreCase).Success)
            {
                arm = RobotArm.rbaUp;
            }
            else if (Regex.Match(m_Arm, @"low", RegexOptions.IgnoreCase).Success)
            {
                arm = RobotArm.rbaDown;
            }
            else if (Regex.Match(m_Arm, @"both", RegexOptions.IgnoreCase).Success)
            {
                arm = RobotArm.rbaBoth;
            }
            return arm;
        }
        protected void layoutInit()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            int eq_number = CommonData.HIRATA.CommonStaticData.g_EqNumber;
            int port_number = CommonData.HIRATA.CommonStaticData.g_PortNumber;
            int robot_number = CommonData.HIRATA.CommonStaticData.g_RobotNumber;
            int buffer_number = CommonData.HIRATA.CommonStaticData.g_BufferNumber;
            int aligner_number = CommonData.HIRATA.CommonStaticData.g_AlignerNumber;

            for (int i = 0; i < eq_number; ++i)
            {
                int eq_no = i + 1;
                int max_slot = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_EqXml.Items[i].Attributes["Capacity"].Trim());
                int time_chat_id = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_EqXml.Items[i].Attributes["TimeChat"].Trim());
                int position = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_EqXml.Items[i].Attributes["Stage"].Trim());
                int node = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_EqXml.Items[i].Attributes["Node"].Trim());
                string tool_id = CommonData.HIRATA.CommonStaticData.g_EqXml.Items[i].Attributes["ToolId"].Trim();
                string get_arm = CommonData.HIRATA.CommonStaticData.g_EqXml.Items[i].Attributes["GetArm"].Trim();
                string put_arm = CommonData.HIRATA.CommonStaticData.g_EqXml.Items[i].Attributes["PutArm"].Trim();
                string side = CommonData.HIRATA.CommonStaticData.g_EqXml.Items[i].Attributes["SideGroup"].Trim();
                int enable = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_EqXml.Items[i].Attributes["Enable"].Trim());

                Eq eq_control = new Eq(eq_no, node, max_slot, GetRobotArmEnumString(get_arm), GetRobotArmEnumString(put_arm), tool_id);
                eq_control.cv_Comm.cv_TimeChatId = time_chat_id;
                eq_control.cv_Comm.cv_RobotPosition = position;
                eq_control.cv_Data.LoadFromFile();
                eq_control.cv_Data.PEnable = enable == 1 ? true : false;
                eq_control.cv_Data.SaveToFile();
                cv_EqContainer.Add(eq_no, eq_control);
            }
            for (int i = 0; i < aligner_number; ++i)
            {
                int eq_no = i + 1;
                int max_slot = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_AlignerXml.Items[i].Attributes["Capacity"].Trim());
                string side = CommonData.HIRATA.CommonStaticData.g_AlignerXml.Items[i].Attributes["SideGroup"].Trim();
                int enable = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_AlignerXml.Items[i].Attributes["Enable"].Trim());
                Aligner aligner_control = new Aligner(eq_no, max_slot);
                aligner_control.PSideGroup = Regex.Match(side , "left").Success ? enSideGroup.Left : enSideGroup.Right;
                aligner_control.cv_Data.LoadFromFile();
                aligner_control.cv_Data.PEnable = enable == 1 ? true : false;
                aligner_control.cv_Data.SaveToFile();
                cv_AlignerContainer.Add(eq_no, aligner_control);
                //cv_SideGroup[aligner_control.PSideGroup].Add(aligner_control);
            }

            for (int i = 0; i < port_number; ++i)
            {
                int max_slot = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_PortXml.Items[i].Attributes["Capacity"].Trim());
                int enable = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_PortXml.Items[i].Attributes["Enable"].Trim());
                string side = CommonData.HIRATA.CommonStaticData.g_PortXml.Items[i].Attributes["SideGroup"].Trim();
                int port_no = i + 1;
                Port port_control = new Port(port_no, max_slot);
                port_control.PSideGroup = Regex.Match(side , "left").Success ? enSideGroup.Left : enSideGroup.Right;
                port_control.cv_Data.LoadFromFile();
                port_control.cv_Data.PEnable = enable == 1 ? true : false;
                port_control.cv_Data.SaveToFile();
                cv_PortContainer.Add(port_no, port_control);
                //cv_SideGroup[port_control.PSideGroup].Add(port_control);
            }

            for (int i = 0; i < buffer_number; ++i)
            {
                int max_slot = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_BufferXml.Items[i].Attributes["Capacity"].Trim());
                int position = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_BufferXml.Items[i].Attributes["Stage"].Trim());
                int enable = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_BufferXml.Items[i].Attributes["Enable"].Trim());
                int buffer_no = i + 1;
                string side = CommonData.HIRATA.CommonStaticData.g_BufferXml.Items[i].Attributes["SideGroup"].Trim();
                Buffer buffer_control = new Buffer(buffer_no, max_slot);

                Dictionary<int, BufferSlotType> cv_Types = new Dictionary<int, BufferSlotType>();
                if(buffer_control.cv_Id == 1)
                {
                    cv_Types.Add(1, BufferSlotType.ReverseForRework);
                    cv_Types.Add(2, BufferSlotType.ReverseForRework);
                    cv_Types.Add(3, BufferSlotType.ReverseForRework);
                    cv_Types.Add(4, BufferSlotType.Wafer);
                    cv_Types.Add(5, BufferSlotType.Wafer);
                    cv_Types.Add(6, BufferSlotType.Wafer);
                    cv_Types.Add(7, BufferSlotType.Wafer);
                    cv_Types.Add(8, BufferSlotType.Wafer);
                    cv_Types.Add(9, BufferSlotType.Wafer);
                    cv_Types.Add(10, BufferSlotType.Wafer);
                    cv_Types.Add(11, BufferSlotType.Wafer);
                    cv_Types.Add(12, BufferSlotType.Wafer);
                }
                else if(buffer_control.cv_Id == 2)
                {
                    cv_Types.Add(1, BufferSlotType.Wafer);
                    cv_Types.Add(2, BufferSlotType.Wafer);
                    cv_Types.Add(3, BufferSlotType.Wafer);
                    cv_Types.Add(4, BufferSlotType.Wafer);
                    cv_Types.Add(5, BufferSlotType.Wafer);
                    cv_Types.Add(6, BufferSlotType.Wafer);
                    cv_Types.Add(7, BufferSlotType.Glass);
                    cv_Types.Add(8, BufferSlotType.Glass);
                    cv_Types.Add(9, BufferSlotType.Glass);
                    cv_Types.Add(10, BufferSlotType.Glass);
                    cv_Types.Add(11, BufferSlotType.Glass);
                    cv_Types.Add(12, BufferSlotType.Glass);
                }

                buffer_control.cv_Data.SetSlotType(cv_Types);

                buffer_control.cv_Data.LoadFromFile();
                buffer_control.cv_Data.PEnable = enable == 1 ? true : false;
                buffer_control.cv_Data.SaveToFile();
                buffer_control.cv_Comm.cv_RobotPosition = position;
                if(Regex.Match(side , "left" , RegexOptions.IgnoreCase).Success)
                {
                    buffer_control.PSideGroup = enSideGroup.Left;
                }
                else if(Regex.Match(side , "right" , RegexOptions.IgnoreCase).Success)
                {
                    buffer_control.PSideGroup = enSideGroup.Right;
                }
                else if(Regex.Match(side , "both" , RegexOptions.IgnoreCase).Success)
                {
                    buffer_control.PSideGroup = enSideGroup.Both;
                }
                //cv_SideGroup[buffer_control.PSideGroup].Add(buffer_control);

                cv_BufferContainer.Add(i + 1, buffer_control);
            }


            for (int i = 0; i < robot_number; ++i)
            {
                int max_slot = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_RobotXml.Items[i].Attributes["Capacity"].Trim());
                int enable = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_RobotXml.Items[i].Attributes["Enable"].Trim());
                string side = CommonData.HIRATA.CommonStaticData.g_RobotXml.Items[i].Attributes["SideGroup"].Trim();
                int robot_no = i + 1;
                //string ip = CommonData.HIRATA.CommonStaticData.g_RobotXml.Items[i].Attributes["IP"].Trim();
                //int socket_port = Convert.ToInt32(CommonData.HIRATA.CommonStaticData.g_RobotXml.Items[i].Attributes["Port"].Trim());
                Robot robot_control = new Robot(robot_no, max_slot);
                robot_control.PSideGroup = Regex.Match(side , "left").Success ? enSideGroup.Left : enSideGroup.Right;
                robot_control.cv_Data.LoadFromFile();
                robot_control.cv_Data.PEnable = enable == 1 ? true : false;
                robot_control.cv_Data.SaveToFile();
                cv_RobotContainer.Add(robot_no, robot_control);
                //cv_SideGroup[robot_control.PSideGroup].Add(robot_control);
            }

            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }



        internal static Port GetPortById(int m_Index)
        {
            Port rtn = null;
            if (cv_PortContainer.ContainsKey(m_Index))
            {
                rtn = cv_PortContainer[m_Index];
            }
            return rtn;
        }
        internal static Eq GetEqById(int m_Id)
        {
            Eq rtn = null;
            if (cv_EqContainer.ContainsKey(m_Id))
            {
                rtn = cv_EqContainer[m_Id];
            }
            return rtn;
        }
        internal static Robot GetRobotById(int i)
        {
            Robot rtn = null;
            if (cv_RobotContainer.ContainsKey(i))
            {
                rtn = cv_RobotContainer[i];
            }
            return rtn;
        }

        internal static Robot GetRobotBySide(enSideGroup m_Side)
        {
            Robot rtn = null;
            foreach(Robot rb in cv_RobotContainer.Values)
            {
                if(rb.PSideGroup == m_Side)
                {
                    rtn = rb;
                }
            }
            /*
            if (cv_RobotContainer.fin)
            {
                rtn = cv_RobotContainer[i];
            }
            */
            return rtn;
        }
        internal static Buffer GetBufferById(int i)
        {
            Buffer rtn = null;
            if (cv_BufferContainer.ContainsKey(i))
            {
                rtn = cv_BufferContainer[i];
            }
            return rtn;
        }

        internal static Buffer GetBufferBySide(enSideGroup m_Side)
        {
            Buffer rtn = null;
            foreach(Buffer bf in cv_BufferContainer.Values)
            {
                if(bf.PSideGroup == m_Side)
                {
                    rtn = bf;
                }
            }
            return rtn;
        }
        internal static Aligner GetAlignerById(int i)
        {
            Aligner rtn = null;
            if (cv_AlignerContainer.ContainsKey(i))
            {
                rtn = cv_AlignerContainer[i];
            }
            return rtn;
        }
        internal static Aligner GetAlignerBySide(enSideGroup m_Side)
        {
            Aligner rtn = null;
            foreach(Aligner aligner in cv_AlignerContainer.Values)
            {
                if(aligner.PSideGroup == m_Side)
                {
                    rtn = aligner;
                }
            }
            return rtn;
        }
        internal static void ShowMsg(string m_Txt, bool m_AutoClean, bool m_UseReply, int m_Timeout = 30000)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, CommonData.HIRATA.CommonStaticData.__FUN(), FunInOut.Enter);
            CommonData.HIRATA.MDShowMsg obj = new MDShowMsg();
            CommonData.HIRATA.Msg msg_obj = new Msg();
            msg_obj.PAutoClean = m_AutoClean;
            msg_obj.PUserRep = m_UseReply;
            msg_obj.TimeOut = (uint)m_Timeout;
            msg_obj.Txt = m_Txt;
            obj.Msg = msg_obj;
            LGCController.triggerLgcEvent(typeof(CommonData.HIRATA.MDShowMsg).Name, msg_obj);
            WriteLog(LogLevelType.NormalFunctionInOut, CommonData.HIRATA.CommonStaticData.__FUN(), FunInOut.Leave);
        }

        public static void CheckSystemStatus()
        {
        }
        public static bool CheckAllPortResetError()
        {
            bool rtn = true;
            Port port = null;
            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
            {
                port = GetPortById(i); 
                if (!port.PIsResetError)
                {
                    return false;
                }
            }
            return rtn;
        }
        public static bool CheckAllPortHome()
        {
            bool rtn = true;
            Port port = null;
            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
            {
                 port = GetPortById(i);
                if (port.cv_Data.PPortHasCst == PortHasCst.Has)
                {
                    if (!GetPortById(i).PIsHome)
                    {
                        return false;
                    }
                }
            }
            return rtn;
        }
        public static bool CheckAllPortStatus()
        {
            bool rtn = true;
            Port port = null;
            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
            {
                port = GetPortById(i);
                if (!port.PIsStatus)
                {
                    return false;
                }
            }
            return rtn;
        }
        public static void SendinitComplete(enSideGroup m_Side)
        {
            if(m_Side == enSideGroup.Both)
            {
                lgcBase.PSystemData.PInitaiizeOkLeft = true;
                lgcBase.PSystemData.PInitaiizingRight = false;
            }
            else if(m_Side == enSideGroup.Right)
            {
                lgcBase.PSystemData.PInitaiizeOkRight = true;
                lgcBase.PSystemData.PInitaiizingRight = false;
            }
            else if(m_Side == enSideGroup.Left)
            {
                lgcBase.PSystemData.PInitaiizeOkRight = true;
                lgcBase.PSystemData.PInitaiizingRight = false;
                lgcBase.PSystemData.PInitaiizeOkLeft = true;
                lgcBase.PSystemData.PInitaiizingRight = false;
            }
            //cv_MmfController.SendInitialize(InitialAction.Complete, MmfEventClientEventType.etNotify, false);
            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
            {
                Port port = GetPortById(i);
                if (port.PPortStatus == PortStaus.LDCM)
                {
                    if (port.cv_Data.PPortMode == PortMode.Loader)
                    {
                        if (port.PLotStatus == LotStatus.Process)
                        {
                            AddPortToProcessList(i);
                        }
                    }
                    else if (port.cv_Data.PPortMode == PortMode.Unloader)
                    {
                        if (port.PLotStatus == LotStatus.Process || port.PLotStatus == LotStatus.Reserved)
                        {
                            AddPortToProcessList(i);
                        }
                    }
                }
            }
            CommonData.HIRATA.MDInitial obj = new MDInitial();
            obj.PAction = InitialAction.Complete;
            obj.PResult = Result.OK;
            obj.PType = MmfEventClientEventType.etNotify;
            obj.cv_IsForce = false;
            obj.PSide = m_Side;
            LGCController.triggerLgcEvent(typeof(CommonData.HIRATA.MDInitial).Name, obj);
        }
        public static void SendinitCompleteFail(enSideGroup m_Side)
        {
            if (m_Side == enSideGroup.Both)
            {
                lgcBase.PSystemData.PInitaiizeOkRight = false;
                lgcBase.PSystemData.PInitaiizingRight = false;
                lgcBase.PSystemData.PInitaiizeOkLeft = false;
                lgcBase.PSystemData.PInitaiizingLeft = false;
                Robot rb_left = GetRobotBySide(enSideGroup.Left);
                Robot rb_right = GetRobotBySide(enSideGroup.Right);
                Aligner al_left = GetAlignerBySide(enSideGroup.Left);
                Aligner al_right = GetAlignerBySide(enSideGroup.Right);
                Buffer bf_left = GetBufferBySide(enSideGroup.Left);
                Buffer bf_both = GetBufferBySide(enSideGroup.Both);
                rb_left.PIsStatus = false;
                rb_left.PIsHome = false;
                rb_left.PIsResetError = false;
                rb_right.PIsStatus = false;
                rb_right.PIsHome = false;
                rb_right.PIsResetError = false;
                al_right.PIsStatus = false;
                al_right.PIsHome = false;
                al_right.PIsResetError = false;
                al_left.PIsStatus = false;
                al_left.PIsHome = false;
                al_left.PIsResetError = false;
                bf_left.PIsStatus = false;
                bf_left.PIsHome = false;
                bf_left.PIsResetError = false;
                bf_both.PIsStatus = false;
                bf_both.PIsHome = false;
                bf_both.PIsResetError = false;
            }
            else if (m_Side == enSideGroup.Right)
            {
                lgcBase.PSystemData.PInitaiizeOkRight = false;
                lgcBase.PSystemData.PInitaiizingRight = false;
                Robot rb_right = GetRobotBySide(enSideGroup.Right);
                Aligner al_right = GetAlignerBySide(enSideGroup.Right);
                //Buffer bf_right = GetBufferBySide(enSideGroup.Both);
                rb_right.PIsStatus = false;
                rb_right.PIsHome = false;
                rb_right.PIsResetError = false;
                al_right.PIsStatus = false;
                al_right.PIsHome = false;
                al_right.PIsResetError = false;
                //bf_both.PIsStatus = false;
                //bf_both.PIsHome = false;
            }
            else if (m_Side == enSideGroup.Left)
            {
                lgcBase.PSystemData.PInitaiizeOkLeft = false;
                lgcBase.PSystemData.PInitaiizingLeft = false;
                Robot rb_left = GetRobotBySide(enSideGroup.Left);
                Aligner al_left = GetAlignerBySide(enSideGroup.Left);
                //Buffer bf_both = GetBufferBySide(enSideGroup.Both);
                rb_left.PIsStatus = false;
                rb_left.PIsHome = false;
                rb_left.PIsResetError = false;
                al_left.PIsStatus = false;
                al_left.PIsHome = false;
                al_left.PIsResetError = false;
                //bf_both.PIsStatus = false;
                //bf_both.PIsHome = false;
            }

            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
            {
                Port port = GetPortById(i);
                if (m_Side == enSideGroup.Left || m_Side == enSideGroup.Right)
                {
                    if (port.PSideGroup == m_Side)
                    {
                        port.PIsHome = false;
                        port.PIsResetError = false;
                        port.PIsStatus = false;
                    }
                }
                else
                {
                    port.PIsHome = false;
                    port.PIsResetError = false;
                    port.PIsStatus = false;
                }
            }

            CommonData.HIRATA.MDInitial obj = new MDInitial();
            obj.PAction = InitialAction.Complete;
            obj.PResult = Result.NG;
            obj.PType = MmfEventClientEventType.etNotify;
            obj.cv_IsForce = false;
            obj.PSide = m_Side;
            LGCController.triggerLgcEvent(typeof(CommonData.HIRATA.MDInitial).Name, obj);
        }
        public bool hasGlassPort()
        {
            bool rtn = false;
            int jos_count = cv_InProcessPort.Count;
            for (int i = 0; i < jos_count; i++)
            {
                Port port = GetPortById(cv_InProcessPort[i]);
                if (port.PLotStatus == LotStatus.Process && port.PPortStatus == PortStaus.LDCM)
                {
                    if (port.cv_Data.PProductionType == ProductCategory.Glass)
                        rtn = true;
                }
            }
            return rtn;
        }
        public bool hasWaferPort()
        {
            bool rtn = false;
            int jos_count = cv_InProcessPort.Count;
            for (int i = 0; i < jos_count; i++)
            {
                if (GetPortById(cv_InProcessPort[i]).PLotStatus == LotStatus.Process && GetPortById(cv_InProcessPort[i]).PPortStatus == PortStaus.LDCM)
                {
                    if (GetPortById(cv_InProcessPort[i]).cv_Data.PProductionType == ProductCategory.Wafer)
                        rtn = true;
                }
            }
            return rtn;
        }
        public static void WritePortToPlc(int m_PortId)
        {
            WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, "WritePortToPlc", CommonData.HIRATA.FunInOut.Enter);
            string log = "[Write Port To PLC Port : " + m_PortId + " ]\n";
            int start = 0;
            if (m_PortId == 1) start = 0x355c;
            if (m_PortId == 2) start = 0x359a;
            if (m_PortId == 3) start = 0x35D8;
            if (m_PortId == 4) start = 0x3616;
            if (m_PortId == 5) start = 0x3654;
            if (m_PortId == 6) start = 0x3692;
            Port port = GetPortById(m_PortId);
            int value = ((int)port.cv_Data.PPortStatus << 4) + (1 << 8) + (m_PortId << 12) + (int)port.cv_Data.PPortMode;
            log += "Port Status : " + port.cv_Data.PPortStatus.ToString() + "\n";
            log += "Lot Status : " + port.PLotStatus.ToString() + "\n";
            log += "Port Mode : " + port.cv_Data.PPortMode.ToString() + "\n";
            log += "Port ProductionType : " + port.cv_Data.PProductionType.ToString() + "\n";
            LgcModule.PMio.SetPortValue(start + 0, value);

            value = (int)port.PLotStatus;

            LgcModule.PMio.SetPortValue(start + 1, value);

            LgcModule.PMio.SetPortValue(start + 2, 0);

            value = (int)port.cv_Data.PProductionType;
            LgcModule.PMio.SetPortValue(start + 3, value);

            int work_count = 0;
            value = 0;

            for (int i = 1; i <= 16; i++)
            {
                if (i <= GetPortById(m_PortId).cv_Data.cv_SlotCount)
                {
                    value += (Convert.ToInt32(port.cv_Data.GlassDataMap[i].PHasSensor) << (i - 1));
                    port.cv_Data.GlassDataMap[i].WriteWokeNoOnly(LgcModule.PMio, start + 12 + 2 * (i - 1));
                    log += "Slot : " + i.ToString() + "CIM Mode : " + port.cv_Data.GlassDataMap[i].PCimMode.ToString();
                    log += " Foup Seq : " + port.cv_Data.GlassDataMap[i].PFoupSeq.ToString();
                    log += " Work Order No : " + port.cv_Data.GlassDataMap[i].PWorkOrderNo.ToString();
                    log += " Work Slot : " + port.cv_Data.GlassDataMap[i].PWorkSlot.ToString() + "\n";
                    if (port.cv_Data.GlassDataMap[i].PHasSensor)
                    {
                        work_count++;
                    }
                }
            }
            LgcModule.PMio.SetPortValue(start + 4, value);
            log += "Slot 1-16 : " + SysUtils.IntToHex(value) + "\n";

            value = 0;
            for (int i = 17; i <= 25; i++)
            {
                if (i <= GetPortById(m_PortId).cv_Data.cv_SlotCount)
                {
                    value += (Convert.ToInt32(port.cv_Data.GlassDataMap[i].PHasSensor) << (i - 16 - 1));
                    port.cv_Data.GlassDataMap[i].WriteWokeNoOnly(LgcModule.PMio, start + 12 + 2 * (i - 1));
                    log += "Slot : " + i.ToString() + "CIM Mode : " + port.cv_Data.GlassDataMap[i].PCimMode.ToString();
                    log += " Foup Seq : " + port.cv_Data.GlassDataMap[i].PFoupSeq.ToString();
                    log += " Work Order No : " + port.cv_Data.GlassDataMap[i].PWorkOrderNo.ToString();
                    log += " Work Slot : " + port.cv_Data.GlassDataMap[i].PWorkSlot.ToString() + "\n";
                    if (port.cv_Data.GlassDataMap[i].PHasSensor)
                    {
                        work_count++;
                    }
                }
            }
            log += "Slot 17-25 : " + SysUtils.IntToHex(value) + "\n";
            log += "work count : " + work_count + "\n";
            LgcModule.PMio.SetPortValue(start + 5, value);
            LgcModule.PMio.SetPortValue(start + 6, work_count);

            log += "lot Id : " + port.cv_Data.PLotId + "\n";
            string id = SysUtils.GetFixedLengthString(port.cv_Data.PLotId, 10);
            LgcModule.PMio.SetBinaryLengthData(start + 7, SysUtils.StringToByteArray(id), 5);

            WriteLog(CommonData.HIRATA.LogLevelType.Detail, log);
            WriteLog(CommonData.HIRATA.LogLevelType.NormalFunctionInOut, "WritePortToPlc", CommonData.HIRATA.FunInOut.Leave);
        }
        private bool CheckEqSideData(GlassData data , EqId m_Eq)
        {
            bool rtn = true;
            if (lgcBase.PSystemData.IsCheckRecipe)
            {
                int index = data.cv_Nods.FindIndex(x => x.cv_NodeId == 2);
                GlassDataNodeItem node = data.cv_Nods[index];
                if (node.cv_Recipe != Convert.ToInt32(lgcBase.cv_Recipes.PCurRecipeId))
                {
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PStatus = AlarmStatus.Occur;
                    alarm.PUnit = 0;
                    alarm.PLevel = AlarmLevele.Light;
                    alarm.PCode = CommonData.HIRATA.Alarmtable.RecieUnmatch.ToString();
                    alarm.PMainDescription = "Recv From upstream recipe unmatch";
                    alarm.PSubDescription = "EQ : " + m_Eq;
                    EditAlarm(alarm);
                    //ShowMsg("Recv form upstream recipe un-match", true, false);
                    rtn = false;
                }
            }
            if (lgcBase.PSystemData.IsCheckSeq)
            {
                if (data.PFoupSeq == 0)
                {
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PStatus = AlarmStatus.Occur;
                    alarm.PUnit = 0;
                    alarm.PLevel = AlarmLevele.Light;
                    alarm.PCode = CommonData.HIRATA.Alarmtable.FoupSeqError.ToString();
                    alarm.PMainDescription = "FoupSeq Error";
                    alarm.PSubDescription = "EQ : " + m_Eq;
                    EditAlarm(alarm);
                    // ShowMsg("Recv form upstream Foup Seq 0", true, false);
                    rtn = false;
                }
            }
            if (lgcBase.PSystemData.IsCheckSlot)
            {
                if (data.PWorkSlot == 0)
                {
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PStatus = AlarmStatus.Occur;
                    alarm.PUnit = 0;
                    alarm.PLevel = AlarmLevele.Light;
                    alarm.PCode = CommonData.HIRATA.Alarmtable.WorkSlotError.ToString();
                    alarm.PMainDescription = "Work slot Error";
                    alarm.PSubDescription = "EQ : " + m_Eq;
                    EditAlarm(alarm);
                    // ShowMsg("Recv form upstream slot 0", true, false);
                    rtn = false;
                }
            }
            if (lgcBase.PSystemData.IsCheckId)
            {
                if (string.IsNullOrEmpty(data.PId.Trim().Trim('\0')))
                {
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PStatus = AlarmStatus.Occur;
                    alarm.PUnit = 0;
                    alarm.PLevel = AlarmLevele.Light;
                    alarm.PCode = CommonData.HIRATA.Alarmtable.WorkIdError.ToString();
                    alarm.PMainDescription = "Work Id Error";
                    alarm.PSubDescription = "EQ : " + m_Eq;
                    EditAlarm(alarm);
                    // ShowMsg("Recv form upstream Id empty", true, false);
                    rtn = false;
                }
            }
            if ((lgcBase.PSystemData.PSystemOnlineMode == OnlineMode.Control) ||
                ((lgcBase.PSystemData.PSystemOnlineMode == OnlineMode.Offline) && cv_CheckEqDataLocalMode)
                )
            {
                int node_index = data.cv_Nods.FindIndex(x => x.PNodeId == 2);
                if (node_index != -1)
                {
                    int recipe = data.cv_Nods[node_index].cv_Recipe;
                    if (recipe != Convert.ToInt32(lgcBase.cv_Recipes.PCurRecipeId.Trim()))
                    {
                        AlarmItem alarm = new AlarmItem();
                        alarm.PCode = Alarmtable.InterfaceErrorGlassDataRecipeUnmatch.ToString();
                        alarm.PLevel = AlarmLevele.Light;
                        alarm.PMainDescription = "Interface GlassData Error Recipe Unmatch with EFEM!!!";
                        alarm.PSubDescription = "EQ : " + m_Eq;
                        alarm.PStatus = AlarmStatus.Occur;
                        EditAlarm(alarm);
                        //ShowMsg(alarm.PMainDescription + "\nRecipe from EQ : " + recipe + "EFEM Cur. recipe : " + cv_Recipes.PCurRecipeId.Trim(), false, false);
                        WriteLog(LogLevelType.Warning, alarm.PMainDescription + "\nRecipe from EQ : " + recipe + " EFEM Cur. recipe : " + lgcBase.cv_Recipes.PCurRecipeId.Trim());
                        rtn = false;
                    }
                }
            }
            return rtn;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            for (int i = 3 , j=1; i < 7; i++,  j++)
            {
                GlassData tmp = new GlassData();
                tmp.PCimMode = OnlineMode.Control;
                tmp.PFoupSeq = 1;
                tmp.PWorkOrderNo = 1;
                tmp.PWorkSlot = 3;
                tmp.PId = "RSI1M145RX" ;

                CommonData.HIRATA.MDBCWorkTransferReport obj = new MDBCWorkTransferReport();
                obj.PAction = DataFlowAction.Store;
                obj.PGlassData = tmp;
                obj.PPortNo = 2;
                obj.PSlotNo = 3;
                obj.PUnitNo = 0;
                obj.PType = MmfEventClientEventType.etNotify;
                cv_MmfController.SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCWorkTransferReport).Name, obj, KParseObjToXmlPropertyType.Field);
            }
            */
        }
        public static string FindHightestPriorityPPID(int m_PortId)
        {
            //Args : 1.priority , 2.slot
            Dictionary<int, int> ppid_sort = new Dictionary<int, int>();
            Port port = GetPortById(m_PortId);
            int max_priority = 0;
            for (int slot = 1; slot <= port.cv_Data.cv_SlotCount; slot++)
            {
                GlassData glass = port.cv_Data.GlassDataMap[slot];
                if (glass.PHasData && glass.PHasSensor && glass.PProcessFlag == ProcessFlag.Need)
                {
                    if (!ppid_sort.ContainsKey((int)glass.PPriority))
                    {
                        ppid_sort.Add((int)glass.PPriority, slot);
                        if (glass.PPriority > max_priority)
                        {
                            max_priority = (int)glass.PPriority;
                        }
                    }
                }
            }
            return port.cv_Data.GlassDataMap[ppid_sort[max_priority]].PPID.Trim();
        }
        public static bool FindHightestSlotForPPID(string m_Ppid, int m_PortId, out int m_Slot)
        {
            bool rtn = false;
            Port port = GetPortById(m_PortId);

            //Args : 1.priority , 2.slot
            Dictionary<int, int> slot_sort = new Dictionary<int, int>();
            int max_priority = -1;
            for (int slot = 1; slot <= port.cv_Data.cv_SlotCount; slot++)
            {
                GlassData glass = port.cv_Data.GlassDataMap[slot];
                if (glass.PHasSensor && glass.PHasData)
                {
                    if (glass.PPID.Trim() == m_Ppid.Trim() && glass.POcrResult == OCRResult.None && glass.PProcessFlag == ProcessFlag.Need)
                    {
                        if (!slot_sort.ContainsKey((int)glass.PPriority))
                        {
                            slot_sort.Add((int)glass.PPriority, slot);
                            if ((int)glass.PPriority > max_priority)
                            {
                                max_priority = (int)glass.PPriority;
                            }

                            rtn = true;
                        }
                    }
                }
            }
            m_Slot = 0;
            if (rtn)
            {
                m_Slot = slot_sort[max_priority];
            }
            return rtn;
        }

        public static void WriteJobLog(string m_Prefix, RobotJob m_Job)
        {
            WriteLog(LogLevelType.General, "[WriteJobLog][" + m_Prefix + "] : " + m_Job.PAction.ToString() +
                " " + m_Job.PTarget.ToString() + " : " + m_Job.PTargetId + " slot : " + m_Job.PTargetSlot +
                "  Get arm : " + m_Job.PGetArm.ToString() + " Put arm : " + m_Job.PPutArm.ToString());
        }
        public static bool CheckIsVasPutUpSlotJobStatus(RobotJob m_job)
        {
            return false;
            /*
            bool rtn = false;
            if (m_job.PAction == RobotAction.Put && m_job.PTarget == ActionTarget.Eq && m_job.PTargetId == (int)EqId.VAS && m_job.PTargetSlot == 2)
            {
                EqId eq_id = EqId.VAS;
                int slot = 2;
                int eq_time_chart_cur_step = 0;
                EqInterFaceType gif_type = EqInterFaceType.None;
                int time_chart_id = -1;
                TimechartNormal time_chart_instance = null;

                if (eq_id == EqId.VAS)
                {
                    if (slot == 2)
                    {
                        eq_time_chart_cur_step = GetEqById((int)eq_id).GetTimeChatCurStep(2);
                        time_chart_id = (int)EqGifTimeChartId.TIMECHART_ID_VAS_UP;
                        time_chart_instance = (TimechartNormal)cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
                        if (eq_time_chart_cur_step == TimechartNormal.STEP_ID_WaitRobotPutEnd || eq_time_chart_cur_step == TimechartNormal.STEP_ID_WaitRobotCommandFinish ||
                            eq_time_chart_cur_step == TimechartNormal.STEP_ID_WaitEqCompleteOn)
                        {
                            rtn = true;
                        }
                    }
                }

            }
            return rtn;
            */
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*
            RobotJob job = new RobotJob(1, RobotArm.rabNone, RobotArm.rbaDown, RobotAction.Get, ActionTarget.Buffer, 1, 2, false);
            cv_RobotJobPath.Enqueue(job);

            job = new RobotJob(1, RobotArm.rbaDown, RobotArm.rbaUp, RobotAction.Exchange, ActionTarget.Aligner,1, 1, false);
            cv_RobotJobPath.Enqueue(job);

            job = new RobotJob(1, RobotArm.rbaUp, RobotArm.rabNone, RobotAction.Put, ActionTarget.Port, 6, 1, false);
            cv_RobotJobPath.Enqueue(job);
            */
        }
    }

    public class TowerCommand
    {
        public SignalTowerColor cv_Color;
        public SignalTowerControl cv_Control;
        public bool cv_HadSend = false;
        public TowerCommand(SignalTowerColor m_Color, SignalTowerControl m_Control)
        {
            cv_Control = m_Control;
            cv_Color = m_Color;
        }
    }
}
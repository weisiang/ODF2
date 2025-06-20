using System;
using System.Collections.Generic;
using System.Text;
using KgsCommon;
using CommonData.HIRATA;
using BaseAp;

namespace LGC
{
    class TimechartNormal : TimechartControllerBase.TimechartInstanceBase
    {
        KTimer cv_Timer = null;
        public static int STEP_ID_CleanSingal = 0;
        public static int STEP_ID_WaitCheckStatusOK = 1;
        public static int STEP_ID_WaitSelectMode = 2;
        public static int STEP_ID_ActionReady = 3;
        public static int STEP_ID_WaitRbReplyOn = 4;
        public static int STEP_ID_WaitEqTransferStart = 5;
        public static int STEP_ID_WaitRobotCommandStart = 6;
        public static int STEP_ID_WaitRobotCommandFinish = 7;
        public static int STEP_ID_WaitRobotCompleteOn = 8;
        public static int STEP_ID_WaitEqCompleteOn = 9;

        public static int STEP_ID_WaitRobotPutStart = 20;
        public static int STEP_ID_WaitRobotPutEnd = 21;
        public static int STEP_ID_WaitRobotGetStart = 22;
        public static int STEP_ID_WaitRobotGetEnd = 23;
        public static int STEP_ID_WaitRobotPutVasStandByStart = 24;
        public static int STEP_ID_WaitRobotPutVasStandByEnd = 25;
        public static int STEP_ID_WaitRobotGetVasStandByStart = 26;
        public static int STEP_ID_WaitRobotGetVasStandByEnd = 27;

        public static int STEP_ID_EqPause = 30;
        public static int STEP_ID_ForceIni = 33;
        public static int STEP_ID_ForceIniFinsh = 34;
        public static int STEP_ID_ForceCom = 35;
        public static int STEP_ID_ForceComFinish = 36;

        public int cv_relativeRobot = 0;
        public GlassData cv_GetData = null;
        public GlassData cv_PutData = null;
        public RobotArm cv_GetArm = RobotArm.rabNone;
        public RobotArm cv_PutArm = RobotArm.rabNone;
        public EqInterFaceType cv_Action = EqInterFaceType.None;
        public TimechartNormal(TimechartControllerBase m_TimechartController, int m_TimechartId, Dictionary<string, int> m_VarPortMap)
            : base(m_TimechartController, m_TimechartId, m_VarPortMap)
        {
            SetPortAddress();
            AssignFunction();
            InitTimer();
        }


        private void InitTimer()
        {
            if (cv_Timer == null)
            {
                cv_Timer = new KTimer();
                cv_Timer.Interval = 100;
                cv_Timer.OnTimer += MonitorEqReady;
                cv_Timer.ThreadEventEnabled = true;
                cv_Timer.Open();
                cv_Timer.Enabled = true;
            }
        }
        private void MonitorEqReady()
        {
            int step = GetCurrentStep(this.cv_TimechartId);
            bool eq_ready = GetSignal(EqSideBitAddressOffset.Equipment_Ready);
            if (!eq_ready)
            {
                if (step >= STEP_ID_WaitRobotPutStart && step <= STEP_ID_WaitRobotGetVasStandByEnd)
                {
                    LgcModule.SetRobotStop(cv_relativeRobot);

                    LgcModule.PSystemData.PSystemStatus = EquipmentStatus.Down;
                    JumpToStep(this.cv_TimechartId, STEP_ID_EqPause);

                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PCode = Alarmtable.EQReadyOffRobotStop.ToString();
                    alarm.PMainDescription = "EQ Ready Off Robot Stop";
                    alarm.PUnit = 0;
                    alarm.PLevel = AlarmLevele.Serious;
                    alarm.PStatus = AlarmStatus.Occur;
                    alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
                    LgcModule.EditAlarm(alarm);
                }
            }
        }
        private void AssignFunction()
        {
            AssignEnterStepEventFunction(STEP_ID_CleanSingal, STEP_ID_0_CleanSignal);
            AssignRunningStepEventFunction(STEP_ID_WaitCheckStatusOK, STEP_ID_1_CheckStatusOK);
            AssignRunningStepEventFunction(STEP_ID_WaitSelectMode, STEP_ID_2_SelectMode);
            AssignRunningStepEventFunction(STEP_ID_ActionReady, STEP_ID_3_ActionReady);
            AssignRunningStepEventFunction(STEP_ID_WaitRbReplyOn, STEP_ID_4_RbReplyOn);
            AssignRunningStepEventFunction(STEP_ID_WaitEqTransferStart, STEP_ID_5_EqTransferStart);
            AssignRunningStepEventFunction(STEP_ID_WaitRobotCommandStart, STEP_ID_6_WaitRobotCommandStart);
            //AssignRunningStepEventFunction(STEP_ID_WaitRobotCommandFinish, STEP_ID_7_WaitRobotCommandFinish);
            AssignRunningStepEventFunction(STEP_ID_WaitRobotCompleteOn, STEP_ID_8_WaitRobotCompleteOn);
            //AssignRunningStepEventFunction(STEP_ID_WaitEqCompleteOn, STEP_ID_9_WaitEqCompleteOn);

            AssignEnterStepEventFunction(STEP_ID_WaitRobotPutStart, STEP_ID_20_WaitRobotPutStart);
            //AssignRunningStepEventFunction(STEP_ID_WaitRobotPutStart, STEP_ID_20_WaitRobotPutStart);
            AssignRunningStepEventFunction(STEP_ID_WaitRobotPutEnd, STEP_ID_21_WaitRobotPutEnd);
            //AssignRunningStepEventFunction(STEP_ID_WaitRobotGetStart, STEP_ID_22_WaitRobotGetStart);
            AssignEnterStepEventFunction(STEP_ID_WaitRobotGetStart, STEP_ID_22_WaitRobotGetStart);
            AssignRunningStepEventFunction(STEP_ID_WaitRobotGetEnd, STEP_ID_23_WaitRobotGetEnd);
            //AssignRunningStepEventFunction(STEP_ID_WaitRobotPutVasStandByStart, STEP_ID_24_WaitRobotPutVasStandByStart);
            AssignEnterStepEventFunction(STEP_ID_WaitRobotPutVasStandByStart, STEP_ID_24_WaitRobotPutVasStandByStart);
            AssignRunningStepEventFunction(STEP_ID_WaitRobotPutVasStandByEnd, STEP_ID_25_WaitRobotPutVasStandByEnd);
            //AssignRunningStepEventFunction(STEP_ID_WaitRobotGetVasStandByStart, STEP_ID_26_WaitRobotGetVasStandByStart);
            AssignEnterStepEventFunction(STEP_ID_WaitRobotGetVasStandByStart, STEP_ID_26_WaitRobotGetVasStandByStart);
            AssignRunningStepEventFunction(STEP_ID_WaitRobotGetVasStandByEnd, STEP_ID_27_WaitRobotetVasStandByEnd);

            AssignEnterStepEventFunction(STEP_ID_WaitRobotCommandFinish, STEP_ID_7_WaitRobotCommandFinish);
            //AssignRunningStepEventFunction(STEP_ID_WaitEqCompleteOn, STEP_ID_9_WaitEqCompleteOn);
            AssignEnterStepEventFunction(STEP_ID_WaitEqCompleteOn, STEP_ID_9_WaitEqCompleteOn);
            AssignRunningStepEventFunction(STEP_ID_EqPause, STEP_ID_30_EqPause);

            AssignEnterStepEventFunction(STEP_ID_ForceIni, STEP_ID_33_ForceIni);
            AssignEnterStepEventFunction(STEP_ID_ForceCom, STEP_ID_35_ForceCom);
            AssignRunningStepEventFunction(STEP_ID_ForceIniFinsh, STEP_ID_34_ForceIniFinish);
            AssignRunningStepEventFunction(STEP_ID_ForceComFinish, STEP_ID_36_ForceComFinish);
        }
        private void SetPortAddress()
        {
            KXmlItem tmp_xml = new KXmlItem();
            tmp_xml.LoadFromFile(CommonData.HIRATA.CommonStaticData.g_SysGifPortAddrFileFile);
            string item_name = "TimerChart" + ((int)cv_TimechartId).ToString();
            if (tmp_xml.IsItemExist(item_name))
            {
                cv_RobotBitStart = CommonData.HIRATA.CommonStaticData.GetIntFormStr(tmp_xml.ItemsByName[item_name].Attributes["RbBitStart"]);
                cv_EqBitStart = CommonData.HIRATA.CommonStaticData.GetIntFormStr(tmp_xml.ItemsByName[item_name].Attributes["EqBitStart"]);
                cv_WriteDataStartPort = CommonData.HIRATA.CommonStaticData.GetIntFormStr(tmp_xml.ItemsByName[item_name].Attributes["RbWordStart"]);
                cv_ReadDataStartPort = CommonData.HIRATA.CommonStaticData.GetIntFormStr(tmp_xml.ItemsByName[item_name].Attributes["EqWordStart"]);
            }
        }
        public void STEP_ID_0_CleanSignal(int m_StepId)
        {
            if (cv_ActionType != EqInterFaceType.None)
            {
                cv_ActionType = EqInterFaceType.None;
            }
            for (int i = (int)RobotSideBitAddressOffset.Load_Only_Reply; i <= (int)RobotSideBitAddressOffset.Force_Initial_Request; i++)
            {
                if ((i != (int)RobotSideBitAddressOffset.Interlock_2) &&
                    (i != (int)RobotSideBitAddressOffset.Active_Standby))
                {
                    cv_MemoryIoClient.SetPortValue(cv_RobotBitStart + i, 0);
                }
            }
            StopTimeOut(GifTimeout.All);
        }
        public void STEP_ID_1_CheckStatusOK(int m_StepId)
        {
            if (cv_ActionType != EqInterFaceType.None)
            {
                cv_ActionType = EqInterFaceType.None;
            }
            if (cv_MemoryIoClient.GetPortValue(cv_EqBitStart + (int)EqSideBitAddressOffset.Equipment_Ready) == 1 &&
                cv_MemoryIoClient.GetPortValue(cv_EqBitStart + (int)EqSideBitAddressOffset.Interlock_1) == 1 &&
                cv_MemoryIoClient.GetPortValue(cv_EqBitStart + (int)EqSideBitAddressOffset.Transfer_Complete) == 0 &&
                cv_MemoryIoClient.GetPortValue(cv_EqBitStart + (int)EqSideBitAddressOffset.Transfer_Start) == 0 &&
                cv_MemoryIoClient.GetPortValue(cv_EqBitStart + (int)EqSideBitAddressOffset.Stop_Supply) == 0 &&
                cv_MemoryIoClient.GetPortValue(cv_EqBitStart + (int)EqSideBitAddressOffset.Force_Initial_Complete) == 0
                )
            {
                SetTrigger(this.cv_TimechartId);
            }
        }
        public void STEP_ID_2_SelectMode(int m_StepId)
        {
            if (cv_ActionType != EqInterFaceType.None)
            {
                cv_ActionType = EqInterFaceType.None;
            }
            if (!IsOnlyOneAction())
            {
                return;
            }

            if (cv_MemoryIoClient.GetPortValue(cv_EqBitStart + (int)EqSideBitAddressOffset.Equipment_Ready) == 1 &&
                cv_MemoryIoClient.GetPortValue(cv_EqBitStart + (int)EqSideBitAddressOffset.Interlock_1) == 1 &&
                cv_MemoryIoClient.GetPortValue(cv_EqBitStart + (int)EqSideBitAddressOffset.Transfer_Complete) == 0 &&
                cv_MemoryIoClient.GetPortValue(cv_EqBitStart + (int)EqSideBitAddressOffset.Transfer_Start) == 0 &&
                cv_MemoryIoClient.GetPortValue(cv_EqBitStart + (int)EqSideBitAddressOffset.Stop_Supply) == 0 &&
                cv_MemoryIoClient.GetPortValue(cv_EqBitStart + (int)EqSideBitAddressOffset.Force_Initial_Complete) == 0
                )
            {
                if (cv_MemoryIoClient.GetPortValue(cv_EqBitStart + (int)EqSideBitAddressOffset.Load_Only_Req) == 1)
                {
                    cv_ActionType = EqInterFaceType.Load;
                    SetTrigger(cv_TimechartId);
                    cv_DataTime = SysUtils.Now();
                }
                else if (cv_MemoryIoClient.GetPortValue(cv_EqBitStart + (int)EqSideBitAddressOffset.Unload_Only_Req) == 1)
                {
                    cv_ActionType = EqInterFaceType.Unload;
                    SetTrigger(cv_TimechartId);
                    cv_DataTime = SysUtils.Now();
                }
                else if (cv_MemoryIoClient.GetPortValue(cv_EqBitStart + (int)EqSideBitAddressOffset.Exchange_Req) == 1)
                {
                    cv_ActionType = EqInterFaceType.Exchange;
                    SetTrigger(cv_TimechartId);
                    cv_DataTime = SysUtils.Now();
                }
            }
            else
            {
                JumpToStep(this.cv_TimechartId, (int)STEP_ID_WaitCheckStatusOK);
            }
        }
        public void STEP_ID_3_ActionReady(int m_StepId)
        {
            bool error = false;
            if (!IsOnlyOneAction())
            {
                error = true;
            }

            if (cv_MemoryIoClient.GetPortValue(cv_EqBitStart + (int)EqSideBitAddressOffset.Equipment_Ready) != 1 ||
                cv_MemoryIoClient.GetPortValue(cv_EqBitStart + (int)EqSideBitAddressOffset.Interlock_1) != 1 ||
                cv_MemoryIoClient.GetPortValue(cv_EqBitStart + (int)EqSideBitAddressOffset.Transfer_Complete) != 0 ||
                cv_MemoryIoClient.GetPortValue(cv_EqBitStart + (int)EqSideBitAddressOffset.Force_Initial_Complete) != 0)
            {
                error = true;
            }
            else if (cv_ActionType == EqInterFaceType.Load)
            {
                if (!GetSignal(EqSideBitAddressOffset.Load_Only_Req))
                {
                    error = true;
                }
            }
            else if (cv_ActionType == EqInterFaceType.Unload)
            {
                if (!GetSignal(EqSideBitAddressOffset.Unload_Only_Req))
                {
                    error = true;
                }
            }
            else if (cv_ActionType == EqInterFaceType.Exchange)
            {
                if (!GetSignal(EqSideBitAddressOffset.Exchange_Req))
                {
                    error = true;
                }
            }
            if (error)
            {
                JumpToStep(this.cv_TimechartId, (int)STEP_ID_WaitCheckStatusOK);
                return;
            }
        }
        public void STEP_ID_4_RbReplyOn(int m_StepId)
        {
            bool rtn = false;
            {
                if (cv_ActionType == EqInterFaceType.Unload)
                {
                    if (GetSignal(RobotSideBitAddressOffset.Unload_Only_Reply))
                        rtn = true;
                }
                else if (cv_ActionType == EqInterFaceType.Load)
                {
                    if (GetSignal(RobotSideBitAddressOffset.Load_Only_Reply))
                        rtn = true;
                }
                else if (cv_ActionType == EqInterFaceType.Exchange)
                {
                    if (GetSignal(RobotSideBitAddressOffset.Exchange_Reply))
                        rtn = true;
                }
            }
            if (rtn)
            {
                SetTrigger(this.cv_TimechartId);
                StartTimeOut(GifTimeout.T1);
            }
        }
        public void STEP_ID_5_EqTransferStart(int m_StepId)
        {
            bool rtn = false;
            if (GetSignal(EqSideBitAddressOffset.Interlock_1)
                && GetSignal(EqSideBitAddressOffset.Equipment_Ready)
                && GetSignal(EqSideBitAddressOffset.Transfer_Start))
            {
                if (cv_ActionType == EqInterFaceType.Load)
                {
                    if (GetSignal(EqSideBitAddressOffset.Load_Only_Req))
                    {
                        rtn = true;
                    }
                }
                if (cv_ActionType == EqInterFaceType.Unload)
                {
                    if (GetSignal(EqSideBitAddressOffset.Unload_Only_Req))
                    {
                        rtn = true;
                    }
                }
                if (cv_ActionType == EqInterFaceType.Exchange)
                {
                    if (GetSignal(EqSideBitAddressOffset.Exchange_Req))
                    {
                        rtn = true;
                    }
                }
            }
            if (rtn)
            {
                StopTimeOut(GifTimeout.T1);
                SetTrigger(this.cv_TimechartId);
            }
        }
        public void STEP_ID_6_WaitRobotCommandStart(int m_StepId)
        {
            if (cv_ActionType == EqInterFaceType.Load)
            {
                if ( (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS1_UP) || (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS2_UP))
                {
                    JumpToStep(this.cv_TimechartId, STEP_ID_WaitRobotPutVasStandByStart);
                }
                else if ( (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS1_DOWN) || (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS2_DOWN))
                {
                    JumpToStep(this.cv_TimechartId, STEP_ID_WaitRobotPutVasStandByStart);
                }
                else
                {
                    JumpToStep(this.cv_TimechartId, STEP_ID_WaitRobotPutStart);
                }
            }
            else if (cv_ActionType == EqInterFaceType.Unload)
            {
                if ( (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS1_UP) || (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS2_UP))
                {
                }
                else if ( (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS1_DOWN) || (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS2_DOWN))
                {
                    JumpToStep(this.cv_TimechartId, STEP_ID_WaitRobotGetVasStandByStart);
                }
                else
                {
                    JumpToStep(this.cv_TimechartId, STEP_ID_WaitRobotGetStart);
                }
            }
            else if (cv_ActionType == EqInterFaceType.Exchange)
            {
                if ( (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS1_UP) || (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS2_UP))
                {
                }
                else if ( (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS1_DOWN) || (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS2_DOWN))
                {
                }
                else
                {
                    JumpToStep(this.cv_TimechartId, STEP_ID_WaitRobotGetStart);
                }
            }
        }
        public void STEP_ID_7_WaitRobotCommandFinish(int m_StepId)
        {
            SetSignal(RobotSideBitAddressOffset.Interlock_2, true);
            SetSignal(RobotSideBitAddressOffset.Receipt_Complete, true);
        }
        public void STEP_ID_8_WaitRobotCompleteOn(int m_StepId)
        {
            if (GetSignal(RobotSideBitAddressOffset.Receipt_Complete))
            {
                StartTimeOut(GifTimeout.T3);
                SetTrigger(this.TimechartId);
            }
            else
            {
                //SetSignal(RobotSideBitAddressOffset.Receipt_Complete, true);
            }
        }
        public void STEP_ID_9_WaitEqCompleteOn(int m_StepId)
        {
            if (GetSignal(EqSideBitAddressOffset.Transfer_Complete))
            {
                StopTimeOut(GifTimeout.T3);
                if (cv_ActionType == EqInterFaceType.Load)
                {
                    SetSignal(RobotSideBitAddressOffset.Load_Only_Reply, false);
                    SetSignal(RobotSideBitAddressOffset.Receipt_Complete, false);
                    SetSignal(RobotSideBitAddressOffset.Interlock_2, true);
                }
                else if (cv_ActionType == EqInterFaceType.Unload)
                {
                    SetSignal(RobotSideBitAddressOffset.Unload_Only_Reply, false);
                    SetSignal(RobotSideBitAddressOffset.Receipt_Complete, false);
                    SetSignal(RobotSideBitAddressOffset.Interlock_2, true);
                }
                else if (cv_ActionType == EqInterFaceType.Exchange)
                {
                    SetSignal(RobotSideBitAddressOffset.Exchange_Reply, false);
                    SetSignal(RobotSideBitAddressOffset.Receipt_Complete, false);
                    SetSignal(RobotSideBitAddressOffset.Interlock_2, true);
                }
            }
        }

        public void STEP_ID_20_WaitRobotPutStart(int m_StepId)
        {
            if (cv_ActionType == EqInterFaceType.Load)
            {
                if ( (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS1_UP) || (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS2_UP))
                {
                }
                else if ( (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS1_DOWN) || (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS2_DOWN))
                {
                }
                else
                {
                    SetSignal(RobotSideBitAddressOffset.Interlock_2, false);
                }
            }
            else if (cv_ActionType == EqInterFaceType.Exchange)
            {
                if ( (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS1_UP) || (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS2_UP))
                {
                }
                else if ( (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS1_DOWN) || (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS2_DOWN))
                {
                }
                else
                {
                    SetSignal(RobotSideBitAddressOffset.Interlock_2, false);
                }
            }
        }
        public void STEP_ID_21_WaitRobotPutEnd(int m_StepId)
        {
            /*
            if (GetSignal(RobotSideBitAddressOffset.Interlock_2))
            {
                SetSignal(RobotSideBitAddressOffset.Interlock_2, false);
            }
            */
        }
        public void STEP_ID_22_WaitRobotGetStart(int m_StepId)
        {
            if (cv_ActionType == EqInterFaceType.Unload)
            {
                if ( (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS1_UP) || (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS2_UP))
                {
                }
                else if ( (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS1_DOWN) || (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS2_DOWN))
                {
                }
                else
                {
                    SetSignal(RobotSideBitAddressOffset.Interlock_2, false);
                }
            }
            else if (cv_ActionType == EqInterFaceType.Exchange)
            {
                if ( (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS1_UP) || (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS2_UP))
                {
                }
                else if ( (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS1_DOWN) || (this.cv_TimechartId == (int)EqGifTimeChartId.TIMECHART_ID_VAS2_DOWN))
                {
                }
                else
                {
                    SetSignal(RobotSideBitAddressOffset.Interlock_2, false);
                }
            }
        }
        public void STEP_ID_23_WaitRobotGetEnd(int m_StepId)
        {
            /*
            if (GetSignal(RobotSideBitAddressOffset.Interlock_2))
            {
                if (cv_ActionType != EqInterFaceType.Exchange)
                {
                    SetSignal(RobotSideBitAddressOffset.Interlock_2, false);
                }
            }
            */
        }
        public void STEP_ID_24_WaitRobotPutVasStandByStart(int m_StepId)
        {
            if (GetSignal(RobotSideBitAddressOffset.Interlock_2))
            {
                SetSignal(RobotSideBitAddressOffset.Interlock_2, false);
            }
        }
        public void STEP_ID_25_WaitRobotPutVasStandByEnd(int m_StepId)
        {
            /*
            if (GetSignal(RobotSideBitAddressOffset.Interlock_2))
            {
                SetSignal(RobotSideBitAddressOffset.Interlock_2, false);
            }

            if(!GetSignal(EqSideBitAddressOffset.Interlock_1))
            {
                JumpToStep(this.cv_TimechartId, STEP_ID_EqPause);
            }
            */
        }
        public void STEP_ID_26_WaitRobotGetVasStandByStart(int m_StepId)
        {
            if (GetSignal(RobotSideBitAddressOffset.Interlock_2))
            {
                SetSignal(RobotSideBitAddressOffset.Interlock_2, false);
            }
        }
        public void STEP_ID_27_WaitRobotetVasStandByEnd(int m_StepId)
        {
            /*
            if (GetSignal(RobotSideBitAddressOffset.Interlock_2))
            {
                SetSignal(RobotSideBitAddressOffset.Interlock_2, false);
            }

            if(!GetSignal(EqSideBitAddressOffset.Interlock_1))
            {
                JumpToStep(this.cv_TimechartId, STEP_ID_EqPause);
            }
            */
        }

        public void STEP_ID_30_EqPause(int m_StepId)
        {
            if (!GetSignal(RobotSideBitAddressOffset.Interlock_2))
            {
                SetSignal(RobotSideBitAddressOffset.Interlock_2, true);
            }
        }
        public void STEP_ID_33_ForceIni(int m_StepId)
        {
            SetSignal(RobotSideBitAddressOffset.Force_Initial_Request, true);
            RecoverData(false);
            /*
            if (LgcForm.cv_RobotJobPath.Count > 1)
            {
                if (!LgcForm.CheckAutoJobPathOk(LgcForm.cv_RobotJobPath.Count - 1))
                {
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PCode = CommonData.HIRATA.Alarmtable.RobotJobPathError.ToString();
                    alarm.PLevel = AlarmLevele.Serious;
                    alarm.PMainDescription = "Robot Job Path Error";
                    alarm.PSubDescription = "Please check auto job path and robot arm.";
                    alarm.PStatus = AlarmStatus.Occur;
                    alarm.PUnit = 0;
                    LgcForm.EditAlarm(alarm);
                }
            }
            */
            if (GetSignal(RobotSideBitAddressOffset.Force_Initial_Request))
            {
                StartTimeOut(GifTimeout.ForceIni);
                SetTrigger(this.cv_TimechartId);
            }
        }
        public void STEP_ID_34_ForceIniFinish(int m_StepId)
        {
            if (GetSignal(EqSideBitAddressOffset.Force_Initial_Complete))
            {
                SetSignal(RobotSideBitAddressOffset.Force_Initial_Request, false);
                StopTimeOut(GifTimeout.ForceIni);
                SetTrigger(this.cv_TimechartId);
            }
        }
        public void STEP_ID_35_ForceCom(int m_StepId)
        {
            SetSignal(RobotSideBitAddressOffset.Force_Complete_Request, true);
            RecoverData(true);
            //LgcForm.AdjustRobotJob(cv_Action, this.TimechartId);
            //if (LgcForm.cv_RobotJobPath.Count > 1)
            {

                /*
                if (!LgcForm.CheckAutoJobPathOk(LgcForm.cv_RobotJobPath.Count - 1))
                {
                    CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                    alarm.PCode = CommonData.HIRATA.Alarmtable.RobotJobPathError.ToString();
                    alarm.PLevel = AlarmLevele.Serious;
                    alarm.PMainDescription = "Robot Job Path Error";
                    alarm.PSubDescription = "Please check auto job path and robot arm.";
                    alarm.PStatus = AlarmStatus.Occur;
                    alarm.PUnit = 0;
                    LgcForm.EditAlarm(alarm);
                }
                */
                if (GetSignal(RobotSideBitAddressOffset.Force_Complete_Request))
                {
                    //StopTimeOut(GifTimeout.ForceCom);
                    StartTimeOut(GifTimeout.ForceCom);
                    SetTrigger(this.cv_TimechartId);
                }
            }
        }
        public void STEP_ID_36_ForceComFinish(int m_StepId)
        {
            if (GetSignal(EqSideBitAddressOffset.Transfer_Complete))
            {
                StopTimeOut(GifTimeout.ForceCom);
                SetSignal(RobotSideBitAddressOffset.Force_Complete_Request, false);
                SetTrigger(this.cv_TimechartId);
            }
        }

        public override void OnTimeout(int m_TimeoutId)
        {
            StopTimeout(cv_TimechartId, m_TimeoutId);
            CommonData.HIRATA.AlarmItem obj = new AlarmItem();
            obj.PCode = m_TimeoutId.ToString();
            obj.PStatus = CommonData.HIRATA.AlarmStatus.Occur;
            if (m_TimeoutId == (int)GifTimeout.T1)
            {
                obj.PMainDescription = "Glass interface T1 time out";
            }
            else if (m_TimeoutId == (int)GifTimeout.T3)
            {
                obj.PMainDescription = "Glass interface T3 time out";
            }
            else if (m_TimeoutId == (int)GifTimeout.ForceCom)
            {
                obj.PMainDescription = "Glass interface force complete time out";
                JumpToStep(this.cv_TimechartId, STEP_ID_CleanSingal);
            }
            else if (m_TimeoutId == (int)GifTimeout.ForceIni)
            {
                obj.PMainDescription = "Glass interface force initial time out";
                JumpToStep(this.cv_TimechartId, STEP_ID_CleanSingal);
            }
            obj.PUnit = 0;
            obj.PLevel = CommonData.HIRATA.AlarmLevele.Light;
            LgcModule.EditAlarm(obj);
        }
        private void StartTimeOut(GifTimeout m_WhichOne)
        {
            if (m_WhichOne == GifTimeout.ForceCom || m_WhichOne == GifTimeout.ForceIni)
            {
                StartTimeout(this.cv_TimechartId, (int)m_WhichOne, cv_T0, false);
            }
            else if (m_WhichOne == GifTimeout.T1)
            {
                StartTimeout(this.cv_TimechartId, (int)m_WhichOne, cv_T1, false);
            }
            else if (m_WhichOne == GifTimeout.T3)
            {
                StartTimeout(this.cv_TimechartId, (int)m_WhichOne, cv_T3, false);
            }
        }
        private void StopTimeOut(GifTimeout m_WhichOne)
        {
            if (m_WhichOne == GifTimeout.All)
            {
                StopTimeout(this.cv_TimechartId, (int)GifTimeout.T1);
                StopTimeout(this.cv_TimechartId, (int)GifTimeout.T3);
                StopTimeout(this.cv_TimechartId, (int)GifTimeout.ForceIni);
                StopTimeout(this.cv_TimechartId, (int)GifTimeout.ForceCom);
            }
            else
            {
                StopTimeout(this.cv_TimechartId, (int)m_WhichOne);
            }
        }
        private bool IsOnlyOneAction()
        {
            bool rtn = false;
            int sum = 0;
            sum += cv_MemoryIoClient.GetPortValue(cv_EqBitStart + (int)EqSideBitAddressOffset.Load_Only_Req);
            sum += cv_MemoryIoClient.GetPortValue(cv_EqBitStart + (int)EqSideBitAddressOffset.Unload_Only_Req);
            sum += cv_MemoryIoClient.GetPortValue(cv_EqBitStart + (int)EqSideBitAddressOffset.Exchange_Req);
            if (sum == 1)
            {
                rtn = true;
            }
            return rtn;
        }
        private bool GetSignal(EqSideBitAddressOffset m_Signal)
        {
            bool is_on = false;
            if (cv_MemoryIoClient.GetPortValue(cv_EqBitStart + (int)m_Signal) == 1)
            {
                is_on = true;
            }
            return is_on;
        }
        private bool GetSignal(RobotSideBitAddressOffset m_Signal)
        {
            bool is_on = false;
            if (cv_MemoryIoClient.GetPortValue(cv_RobotBitStart + (int)m_Signal) == 1)
            {
                is_on = true;
            }
            return is_on;
        }
        public void SetSignal(RobotSideBitAddressOffset m_Signal, bool m_IsOn)
        {
            cv_MemoryIoClient.SetPortValue(cv_RobotBitStart + (int)m_Signal, m_IsOn ? 1 : 0);
        }
        private void CleanForceData()
        {
            cv_PutArm = RobotArm.rabNone;
            cv_GetArm = RobotArm.rabNone;
            cv_PutData = null;
            cv_GetData = null;
        }
        private void RecoverData(bool m_IsCom)
        {
            Robot rb = LgcModule.GetRobotById(cv_relativeRobot);
            if (m_IsCom)
            {
                if (cv_Action == EqInterFaceType.Load && !rb.cv_Data.GlassDataMap[(int)cv_PutArm].PHasSensor)
                {
                    rb.cv_Data.GlassDataMap[(int)cv_PutArm] = new GlassData();
                    rb.cv_Data.GlassDataMap[(int)cv_PutArm].cv_SlotInEq = (uint)cv_PutArm;
                }
                else if (cv_Action == EqInterFaceType.Unload && rb.cv_Data.GlassDataMap[(int)cv_GetArm].PHasSensor)
                {
                    rb.cv_Data.GlassDataMap[(int)cv_GetArm] = cv_GetData;
                    rb.cv_Data.GlassDataMap[(int)cv_GetArm].cv_SlotInEq = (uint)cv_GetArm;
                }
                else if (cv_Action == EqInterFaceType.Exchange)
                {
                    if(!rb.cv_Data.GlassDataMap[(int)cv_PutArm].PHasSensor)
                    {
                        rb.cv_Data.GlassDataMap[(int)cv_PutArm] = new GlassData();
                        rb.cv_Data.GlassDataMap[(int)cv_PutArm].cv_SlotInEq = (uint)cv_PutArm;
                    }

                    if (rb.cv_Data.GlassDataMap[(int)cv_GetArm].PHasSensor)
                    {
                        rb.cv_Data.GlassDataMap[(int)cv_GetArm] = cv_GetData;
                        rb.cv_Data.GlassDataMap[(int)cv_GetArm].PHasSensor = true;
                        rb.cv_Data.GlassDataMap[(int)cv_GetArm].cv_SlotInEq = (uint)cv_GetArm;
                    }
                }
            }
            LgcModule.GetRobotById(cv_relativeRobot).SendDataViaMmf();
        }
    }
}

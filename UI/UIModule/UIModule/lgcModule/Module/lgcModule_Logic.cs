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
    public partial class LgcModule
    {
        private void OnRobotActionTimer()
        {
            DerivedTimer();
            CalculateSystemStatus();
            DoLeftSideFindJob();
            DoLeftSideJob();
            DoRightSideFindJob();
            DoRightSideJob();
        }
        private bool checkConditionForFindJob(enSideGroup m_Side)
        {
            bool rtn = true;
            Robot side_robot = GetRobotBySide(m_Side);
            bool sidealarm = cv_Alarms.IsHasAlarm(m_Side);
            bool bothalarm = cv_Alarms.IsHasAlarm(enSideGroup.Both);
            bool sideinit = lgcBase.PSystemData.PInitaiizeOkLeft;
            if (!sideinit) rtn = false;
            if (sidealarm || bothalarm) rtn = false;
            if (cv_IsCycleStop)
            {
                if (!side_robot.IsBusy)
                {
                    rtn = false;
                }
            }
            if (side_robot.IsBusy) rtn = false;

            return rtn;
        }
        private void DoLeftSideFindJob( enSideGroup m_Side = enSideGroup.Left)
        {
            if (!checkConditionForFindJob(m_Side)) return;
        }
        private void DoRightSideFindJob( enSideGroup m_Side = enSideGroup.Left)
        {
            if (!checkConditionForFindJob(m_Side)) return;
        }
        private void DoLeftSideJob( enSideGroup m_Side = enSideGroup.Left)
        {
        }
        private void DoRightSideJob(enSideGroup m_Side = enSideGroup.Right)
        {
        }
        #region Do Robot Action for each Eqp.
        private void ProcessPortGetPutJob(RobotAction m_Type)
        {
            /*
            RobotJob job = cv_RobotJobPath.Peek();
            Robot robot = GetRobotById(1);
            Port port = GetPortById(job.PTargetId);
            if (m_Type == RobotAction.Get)
            {
                if (port.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor &&
                    port.cv_Data.GlassDataMap[job.PTargetSlot].PHasData &&
                    !robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor &&
                    !robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasData)
                {
                    Aligner aligner = GetAlignerById(1);
                    aligner.cv_Data.PPreAction = AlignerPreAction.WaitHome;
                    robot.cv_Comm.SetHome(APIEnum.CommnadDevice.Aligner, 1);
                    GetPutPort(job.PGetArm, job.PTargetId, job.PTargetSlot, true);
                }
                else
                {
                    if (!port.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor &&
                        !port.cv_Data.GlassDataMap[job.PTargetSlot].PHasData &&
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor &&
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasData)
                    {
                        cv_RobotJobPath.Dequeue();
                        if (cv_IsCycleStop)
                        {
                            PSystemData.POperationModeLeft = OperationMode.Manual;
                            cv_IsCycleStop = false;
                        }
                    }
                }
            }
            else if (m_Type == RobotAction.Put)
            {
                if (!port.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor &&
                    !port.cv_Data.GlassDataMap[job.PTargetSlot].PHasData &&
                    robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor &&
                    robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasData)
                {
                    GetPutPort(job.PPutArm, job.PTargetId, job.PTargetSlot, false);
                }
                else
                {
                    if (port.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor &&
                        port.cv_Data.GlassDataMap[job.PTargetSlot].PHasData &&
                        !robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor &&
                        !robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasData)
                    {
                        cv_RobotJobPath.Dequeue();
                        if (PSystemData.PSystemOnlineMode != OnlineMode.Control)
                        {
                            if (PSystemData.POperationModeLeft != OperationMode.Manual)
                            {
                                if (port.cv_Data.PPortMode == PortMode.Unloader)
                                {
                                    if (!port.cv_Data.HasOtherJobHaveToDo())
                                    {
                                        port.cv_Data.PWaitUnload = true;
                                    }
                                }
                            }
                        }
                        if (cv_IsCycleStop)
                        {
                            PSystemData.POperationModeLeft = OperationMode.Manual;
                            cv_IsCycleStop = false;
                        }
                    }
                }
            }
            */
        }
        private void ProcessBufferGetPutJob(RobotAction m_Type)
        {
            /*
            RobotJob job = cv_RobotJobPath.Peek();
            Robot robot = GetRobotById(1);
            Buffer buffer = GetBufferById(1);
            if (m_Type == RobotAction.Get)
            {
                if (buffer.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor &&
                    buffer.cv_Data.GlassDataMap[job.PTargetSlot].PHasData &&
                    !robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor &&
                    !robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasData)
                {

                    GetPutBuffer(job.PGetArm, job.PTargetId, job.PTargetSlot, true);
                }
                else
                {
                    if (!buffer.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor &&
                        !buffer.cv_Data.GlassDataMap[job.PTargetSlot].PHasData &&
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor &&
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasData)
                    {
                        cv_RobotJobPath.Dequeue();
                        if (cv_IsCycleStop)
                        {
                            PSystemData.POperationModeLeft = OperationMode.Manual;
                            cv_IsCycleStop = false;
                        }
                    }
                }
            }
            else if (m_Type == RobotAction.Put)
            {
                if (!buffer.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor &&
                    !buffer.cv_Data.GlassDataMap[job.PTargetSlot].PHasData &&
                    robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor &&
                    robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasData)
                {
                    GetPutBuffer(job.PPutArm, 1, job.PTargetSlot, false);
                }
                else
                {
                    if (buffer.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor &&
                        buffer.cv_Data.GlassDataMap[job.PTargetSlot].PHasData &&
                        !robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor &&
                        !robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasData)
                    {
                        cv_RobotJobPath.Dequeue();
                        if (cv_IsCycleStop)
                        {
                            PSystemData.POperationModeLeft = OperationMode.Manual;
                            cv_IsCycleStop = false;
                        }
                    }
                }
            }
            */
        }
        private void ProcessAlignerGetPutJob(RobotAction m_Type, bool m_IsMaunal = false)
        {
            /*
            RobotJob job = null;
            if (!m_IsMaunal)
                job = cv_RobotJobPath.Peek();
            else
                job = cv_RobotManaulJobPath.Peek();

            Aligner aligner = GetAlignerById(job.PTargetId);
            Robot robot = GetRobotById(1);
            if (job.PAction == RobotAction.Get ||
                (job.PAction == RobotAction.Exchange && job.PTarget == ActionTarget.Aligner &&
                job.PIsWaitGet && !job.PisWaitPut))
            {
                if (aligner.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor &&
                    aligner.cv_Data.GlassDataMap[job.PTargetSlot].PHasData &&
                    !robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor &&
                    !robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasData)
                {
                    if (aligner.cv_Data.PPreAction == AlignerPreAction.VuccumOn)
                    {
                        robot.cv_Comm.SetAlignerVaccum(true);
                        aligner.cv_Data.PPreAction = AlignerPreAction.WaitVuccumOn;
                    }
                    else if (aligner.cv_Data.PPreAction == AlignerPreAction.FindNotch)
                    {
                        robot.cv_Comm.SetAlignerFindNotch();
                        aligner.cv_Data.PPreAction = AlignerPreAction.WaitFindNotch;
                    }
                    else if (aligner.cv_Data.PPreAction == AlignerPreAction.OcrConnect)
                    {
                        if (job.PAction != RobotAction.Exchange)
                        {
                            if (aligner.cv_Data.GlassDataMap[1].PProductionCategory == ProductCategory.Wafer)
                            {
                                robot.cv_Comm.SetOcrConnect();
                                aligner.cv_Data.PPreAction = AlignerPreAction.WaitConnect;
                            }
                            else
                            {
                                aligner.cv_Data.GlassDataMap[1].POcrResult = OCRResult.OK;
                                aligner.cv_Data.PPreAction = AlignerPreAction.ToAngle;
                            }
                        }
                        else
                        {
                            aligner.cv_Data.PPreAction = AlignerPreAction.ToAngle;
                        }
                    }
                    else if (aligner.cv_Data.PPreAction == AlignerPreAction.ReadOcr)
                    {
                        if (PSystemData.POcrMode == OCRMode.SkipRead)
                        {
                            aligner.cv_Data.PPreAction = AlignerPreAction.ToAngle;
                        }
                        else
                        {
                            robot.cv_Comm.SetOcrRead();
                            aligner.cv_Data.PPreAction = AlignerPreAction.WaitReadOct;
                        }
                    }
                    else if (aligner.cv_Data.PPreAction == AlignerPreAction.ToAngle)
                    {
                        robot.cv_Comm.SetAlignerToAngle();
                        aligner.cv_Data.PPreAction = AlignerPreAction.WaitToAngle;
                    }
                    else if (aligner.cv_Data.PPreAction == AlignerPreAction.VuccumOff2)
                    {
                        robot.cv_Comm.SetAlignerVaccum(false);
                        aligner.cv_Data.PPreAction = AlignerPreAction.WaitVuccomOff2;
                    }
                    else if (aligner.cv_Data.PPreAction == AlignerPreAction.GetAligner)
                    {
                        //job.PIsWaitGet = false;
                        if (PSystemData.POperationModeLeft == OperationMode.Auto)
                        {
                            if (job.PAction == RobotAction.Exchange)
                            {
                                GetPutAligner(job.PGetArm, true);
                                aligner.cv_Data.PPreAction = AlignerPreAction.None;
                                return;
                            }
                            else if (aligner.cv_Data.GlassDataMap[1].POcrResult == OCRResult.Mismatch)
                            {
                                GlassData glass_tmp = aligner.cv_Data.GlassDataMap[1];
                                if (PSystemData.POcrMode == OCRMode.ErrorReturn)
                                {
                                    cv_RobotJobPath.Clear();
                                    cv_RobotJobPath.Enqueue(job);
                                    if (GetPortById((int)glass_tmp.PSourcePort).PPortStatus == PortStaus.LDCM)
                                    {
                                        //if (!GetPortById((int)glass_tmp.PSourcePort).cv_Data.GlassDataMap[(int)glass_tmp.PWorkSlot].PHasData &&
                                        //    !GetPortById((int)glass_tmp.PSourcePort).cv_Data.GlassDataMap[(int)glass_tmp.PWorkSlot].PHasSensor)
                                        //{
                                        Port port = GetPortById((int)glass_tmp.PSourcePort);
                                        int slot = 0;
                                        if (port.cv_Data.WhichSlotCanLoad(out slot))
                                        {
                                            RobotJob return_job = new RobotJob(1, job.PGetArm, RobotArm.rabNone, RobotAction.Put, ActionTarget.Port, (int)glass_tmp.PSourcePort, slot, false);
                                            cv_RobotJobPath.Enqueue(return_job);
                                            GetPutAligner(job.PGetArm, true);
                                            aligner.cv_Data.PPreAction = AlignerPreAction.None;
                                            WriteLog(LogLevelType.General, "Set return source port : " + port.cv_Id + " slot : " +
                                                 slot + " at OCR Error return mode");
                                        }
                                        else
                                        {
                                            WriteLog(LogLevelType.General, "Set return source port : " + port.cv_Id + " can't find slot to put.");
                                        }
                                        //GetPutAligner(job.PGetArm, true);
                                        //aligner.cv_Data.PPreAction = AlignerPreAction.None;
                                        //}
                                    }
                                }
                                else if (PSystemData.POcrMode == OCRMode.ErrorHold)
                                {
                                    if (glass_tmp.POcrDecide == OCRMode.ErrorReturn)
                                    {
                                        cv_RobotJobPath.Clear();
                                        cv_RobotJobPath.Enqueue(job);
                                        if (GetPortById((int)glass_tmp.PSourcePort).PPortStatus == PortStaus.LDCM)
                                        {
                                            //if (!GetPortById((int)glass_tmp.PSourcePort).cv_Data.GlassDataMap[(int)glass_tmp.PWorkSlot].PHasData &&
                                            //    !GetPortById((int)glass_tmp.PSourcePort).cv_Data.GlassDataMap[(int)glass_tmp.PWorkSlot].PHasSensor)
                                            //{
                                            Port port = GetPortById((int)glass_tmp.PSourcePort);
                                            int slot = 0;
                                            if (port.cv_Data.WhichSlotCanLoad(out slot))
                                            {
                                                RobotJob return_job = new RobotJob(1, job.PGetArm, RobotArm.rabNone, RobotAction.Put, ActionTarget.Port, (int)glass_tmp.PSourcePort, slot, false);
                                                cv_RobotJobPath.Enqueue(return_job);
                                                GetPutAligner(job.PGetArm, true);
                                                aligner.cv_Data.PPreAction = AlignerPreAction.None;
                                                WriteLog(LogLevelType.General, "Set return source port : " + port.cv_Id + " slot : " +
                                                     slot + " at OCR Error hold and User press retun mode button");
                                            }
                                            else
                                            {
                                                WriteLog(LogLevelType.General, "Set return source port : " + port.cv_Id + " can't find slot to put.");
                                            }
                                            //}
                                        }
                                    }
                                    else if (glass_tmp.POcrDecide == OCRMode.ErrorSkip || glass_tmp.POcrDecide == OCRMode.SkipRead)
                                    {
                                        if (glass_tmp.POcrDecide == OCRMode.SkipRead)
                                        {
                                            CommonData.HIRATA.MDBCWorkDataUpdateReport report_bc = new MDBCWorkDataUpdateReport();
                                            report_bc.PGlass = aligner.cv_Data.GlassDataMap[1];
                                            LgcForm.cv_MmfController.SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCWorkDataUpdateReport).Name, report_bc, KParseObjToXmlPropertyType.Field);
                                        }
                                        GetPutAligner(job.PGetArm, true);
                                        aligner.cv_Data.PPreAction = AlignerPreAction.None;
                                    }
                                }
                                else
                                {
                                    GetPutAligner(job.PGetArm, true);
                                    aligner.cv_Data.PPreAction = AlignerPreAction.None;
                                }
                            }
                            else
                            {
                                GetPutAligner(job.PGetArm, true);
                                aligner.cv_Data.PPreAction = AlignerPreAction.None;
                            }
                        }
                        else
                        {
                            GetPutAligner(job.PGetArm, true);
                            aligner.cv_Data.PPreAction = AlignerPreAction.None;
                        }
                    }
                }
                else
                {
                    if (!aligner.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor &&
                        !aligner.cv_Data.GlassDataMap[job.PTargetSlot].PHasData &&
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor &&
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasData)
                    {
                        if (!m_IsMaunal)
                        {
                            cv_RobotJobPath.Dequeue();
                            if (cv_IsCycleStop)
                            {
                                PSystemData.POperationModeLeft = OperationMode.Manual;
                                cv_IsCycleStop = false;
                            }
                        }
                        else
                            cv_RobotManaulJobPath.Dequeue();

                        job.PIsWaitGet = false;
                    }
                }
            }
            else if (job.PAction == RobotAction.Put || (
                (job.PAction == RobotAction.Exchange) &&
                (job.PTarget == ActionTarget.Aligner) && (job.PisWaitPut) && (job.PIsWaitGet))
                )
            {
                if (!aligner.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor &&
                    !aligner.cv_Data.GlassDataMap[job.PTargetSlot].PHasData &&
                    robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor &&
                    robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasData)
                {
                    GlassData glass = robot.cv_Data.GlassDataMap[(int)job.PPutArm];
                    if (glass.POcrDecide != OCRMode.None)
                    {
                        glass.POcrDecide = OCRMode.None;
                    }
                    RecipeItem recipe = null;
                    if (!cv_Recipes.GetCurRecipe(out recipe))
                    {
                        CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                        alarm.PCode = CommonData.HIRATA.Alarmtable.NotSetCurRecipe.ToString();
                        alarm.PLevel = AlarmLevele.Serious;
                        alarm.PMainDescription = "Not Set Cur Recipe , please check cur recipe.";
                        alarm.PStatus = AlarmStatus.Occur;
                        alarm.PUnit = 0;
                        LgcForm.EditAlarm(alarm);
                        PSystemData.POperationModeLeft = OperationMode.Manual;
                        return;
                    }
                    if (aligner.cv_Data.PPreAction == AlignerPreAction.None)
                    {
                        //aligner.cv_Data.PPreAction = AlignerPreAction.AlignerHome;
                        aligner.cv_Data.PPreAction = AlignerPreAction.WaitHome;
                        robot.cv_Comm.SetHome(APIEnum.CommnadDevice.Aligner, 1);
                    }
                    else if (aligner.cv_Data.PPreAction == AlignerPreAction.SetToAngle)
                    {
                        if (job.PAction == RobotAction.Exchange)
                        {
                            if (glass.PProductionCategory == ProductCategory.Wafer)
                            {
                                //if (glass.PPortProductionCategory == ProductCategory.Wafer)
                                //{
                                if(!job.PManualExchangeForAligner)
                                    robot.cv_Comm.SetAlignerDegree(recipe.PWaferVASDegree);
                                else
                                    robot.cv_Comm.SetAlignerDegree((float) Convert.ToDouble(job.PManualExchangeForAlignerDeg));
                                aligner.cv_Data.PPreAction = AlignerPreAction.VuccumOff1;
                                //}
                            }
                            else if (glass.PProductionCategory == ProductCategory.Glass)
                            {
                                //if (glass.PPortProductionCategory == ProductCategory.Wafer)
                                {
                                    if (!job.PManualExchangeForAligner)
                                        robot.cv_Comm.SetAlignerDegree(recipe.PWaferVASDegree);
                                    else
                                        robot.cv_Comm.SetAlignerDegree((float)Convert.ToDouble(job.PManualExchangeForAlignerDeg));

                                    aligner.cv_Data.PPreAction = AlignerPreAction.VuccumOff1;
                                }
                            }
                        }
                        else
                        {
                            if (glass.PProductionCategory == ProductCategory.Wafer)
                            {
                                if (glass.PPortProductionCategory == ProductCategory.Wafer)
                                {
                                    robot.cv_Comm.SetAlignerDegree(recipe.PWaferIJPDegree);
                                }
                            }
                            else if (glass.PProductionCategory == ProductCategory.Glass)
                            {
                                if (glass.PPortProductionCategory == ProductCategory.Wafer)
                                {
                                    robot.cv_Comm.SetAlignerDegree(recipe.PWaferIJPDegree);
                                }
                                else if (glass.PPortProductionCategory == ProductCategory.Glass)
                                {
                                    robot.cv_Comm.SetAlignerDegree(recipe.PGlassVASDegree);
                                }
                            }
                            aligner.cv_Data.PPreAction = AlignerPreAction.VuccumOff1;
                        }
                    }
                    else if (aligner.cv_Data.PPreAction == AlignerPreAction.PutAligner)
                    {
                        job.PisWaitPut = false;
                        GetPutAligner(job.PPutArm, false);
                        aligner.cv_Data.PPreAction = AlignerPreAction.VuccumOn;
                    }
                }
                else
                {
                    if (!robot.cv_Data.GlassDataMap[job.PTargetSlot].PHasSensor &&
                        !robot.cv_Data.GlassDataMap[job.PTargetSlot].PHasData &&
                        aligner.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor &&
                        aligner.cv_Data.GlassDataMap[(int)job.PPutArm].PHasData)
                    {
                        cv_RobotJobPath.Dequeue();
                        if (cv_IsCycleStop)
                        {
                            PSystemData.POperationModeLeft = OperationMode.Manual;
                            cv_IsCycleStop = false;
                        }
                    }
                }
            }
            */
        }
        private void SetRobotSensorToEq()
        {
            /*
            Robot robot = GetRobotById(1);
            int time_chart_id = -1;
            TimechartNormal time_chart_instance = null;

            for (int eq_index = 1; eq_index <= (int)EqId.UV_1; eq_index++)
            {
                EqId eq_id = (EqId)eq_index;
                if (eq_id == EqId.VAS)
                {
                    for (int slot = 1; slot <= 2; slot++)
                    {
                        if (slot == 1)
                        {
                            time_chart_id = (int)EqGifTimeChartId.TIMECHART_ID_VAS_DOWN;
                            time_chart_instance = (TimechartNormal)cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
                            SetRobotSensorToEq(RobotArm.rbaUp, time_chart_instance.cv_RobotBitStart + (int)RobotSideBitAddressOffset.Work_Presence_Upper_Arm);
                            SetRobotSensorToEq(RobotArm.rbaDown, time_chart_instance.cv_RobotBitStart + (int)RobotSideBitAddressOffset.Work_Presence_Low_Arm);
                        }
                        else if (slot == 2)
                        {
                            time_chart_id = (int)EqGifTimeChartId.TIMECHART_ID_VAS_UP;
                            time_chart_instance = (TimechartNormal)cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
                            SetRobotSensorToEq(RobotArm.rbaUp, time_chart_instance.cv_RobotBitStart + (int)RobotSideBitAddressOffset.Work_Presence_Upper_Arm);
                            SetRobotSensorToEq(RobotArm.rbaDown, time_chart_instance.cv_RobotBitStart + (int)RobotSideBitAddressOffset.Work_Presence_Low_Arm);
                        }
                    }
                }
                else
                {
                    time_chart_id = GetEqById((int)eq_id).cv_Comm.cv_TimeChatId;
                    time_chart_instance = (TimechartNormal)cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
                    SetRobotSensorToEq(RobotArm.rbaUp, time_chart_instance.cv_RobotBitStart + (int)RobotSideBitAddressOffset.Work_Presence_Upper_Arm);
                    SetRobotSensorToEq(RobotArm.rbaDown, time_chart_instance.cv_RobotBitStart + (int)RobotSideBitAddressOffset.Work_Presence_Low_Arm);
                }
            }
            */
        }
        private void SetRobotSensorToEq(RobotArm m_Arm, int m_PortAddress)
        {
            Robot robot = GetRobotById(1);
            bool up_sensor = robot.cv_Data.GlassDataMap[(int)RobotArm.rbaUp].PHasSensor;
            bool down_sensor = robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].PHasSensor;
            if (m_Arm == RobotArm.rbaUp)
            {
                PMio.SetPortValue(m_PortAddress, up_sensor ? 1 : 0);
                WriteLog(LogLevelType.TimerFunction, "Set GIF sensor for Up arm" + (up_sensor ? "On" : "off"), FunInOut.None);
            }
            else if (m_Arm == RobotArm.rbaDown)
            {
                PMio.SetPortValue(m_PortAddress, down_sensor ? 1 : 0);
                WriteLog(LogLevelType.TimerFunction, "Set GIF sensor for down arm" + (down_sensor ? "On" : "off"), FunInOut.None);
            }
        }
        private void ProcessEqGetPutJob(RobotAction m_Type, bool m_IsMaunal = false)
        {
            /*
            RobotJob job = null;
            Robot robot = GetRobotById(1);
            if (!m_IsMaunal)
            {
                job = cv_RobotJobPath.Peek();
                if (cv_RobotJobPath.Count >= 2)
                {
                    if( (cv_RobotJobPath.ElementAt(1).PTarget == ActionTarget.Aligner ) && (job.PTargetId != (int)EqId.IJP) )
                    {
                        Aligner aligner = GetAlignerById(1);
                        if (aligner.cv_Data.PPreAction != AlignerPreAction.WaitHome && aligner.cv_Data.PPreAction != AlignerPreAction.SetToAngle)
                        {
                            aligner.cv_Data.PPreAction = AlignerPreAction.WaitHome;
                            robot.cv_Comm.SetHome(APIEnum.CommnadDevice.Aligner, 1);
                        }
                    }
                }
            }
            else
                job = cv_RobotManaulJobPath.Peek();
            EqId eq_id = (EqId)(int)job.PTargetId;
            int slot = job.PTargetSlot;
            int eq_time_chart_cur_step = 0;
            EqInterFaceType gif_type = EqInterFaceType.None;
            int time_chart_id = -1;
            TimechartNormal time_chart_instance = null;

            if (eq_id == EqId.VAS)
            {
                if (slot == 1)
                {
                    eq_time_chart_cur_step = GetEqById((int)eq_id).GetTimeChatCurStep(1);
                    time_chart_id = (int)EqGifTimeChartId.TIMECHART_ID_VAS_DOWN;
                    time_chart_instance = (TimechartNormal)cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
                }
                else if (slot == 2)
                {
                    eq_time_chart_cur_step = GetEqById((int)eq_id).GetTimeChatCurStep(2);
                    time_chart_id = (int)EqGifTimeChartId.TIMECHART_ID_VAS_UP;
                    time_chart_instance = (TimechartNormal)cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
                }
            }
            else
            {
                eq_time_chart_cur_step = GetEqById((int)eq_id).GetTimeChatCurStep(1);
                time_chart_id = GetEqById((int)eq_id).cv_Comm.cv_TimeChatId;
                time_chart_instance = (TimechartNormal)cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_id);
            }
            if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_ActionReady)
            {
                gif_type = time_chart_instance.cv_ActionType;
            }

            if (m_Type == RobotAction.Get)// && (gif_type == EqInterFaceType.Unload || gif_type == EqInterFaceType.Exchange))
            {
                bool robot_get_arm_sensor = robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor;
                bool robot_get_arm_data = robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasData;
                GlassData glass = null;
                if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_ActionReady)
                {
                    if (cv_IsCycleStop)
                    {
                        PSystemData.POperationModeLeft = OperationMode.Manual;
                        cv_IsCycleStop = false;
                    }
                    if (gif_type != EqInterFaceType.Unload && gif_type != EqInterFaceType.Exchange)
                    {
                        return;
                    }
                    if (!robot_get_arm_data && !robot_get_arm_sensor)
                    {
                        glass = new GlassData(cv_Mio, time_chart_instance.cv_ReadDataStartPort);
                        string glass_id = glass.PId;
                        string combination = glass.PAssamblyResult.ToString();
                        for (int i = 1; i <= 15; i++)
                        {
                            int node_index = glass.cv_Nods.FindIndex(x => x.PNodeId == i);
                            if (node_index != -1)
                            {
                                int history = glass.cv_Nods[node_index].cv_ProcessHistory;
                                int recipe = glass.cv_Nods[node_index].cv_Recipe;
                            }
                        }
                        //tmp mark for uv no data case.
                        if (glass.PHasData)
                        {
                            if (!CheckEqSideData(glass, eq_id))
                            {
                                return;
                            }
                            time_chart_instance.SetTrigger(time_chart_id);
                            time_chart_instance.cv_ActionType = EqInterFaceType.Unload;
                            cv_Mio.SetPortValue(time_chart_instance.cv_RobotBitStart +
                                (int)RobotSideBitAddressOffset.Unload_Only_Reply, 1);
                            time_chart_instance.cv_GetData = glass;
                            time_chart_instance.cv_GetArm = job.PGetArm;
                            time_chart_instance.cv_Action = EqInterFaceType.Unload;
                            if (job.PTarget == ActionTarget.Eq && job.PTargetId == (int)EqId.VAS)
                            {
                                if(job.PTargetSlot == 1)
                                {
                                    GetVasStandby();
                                }
                            }
                            else
                            {
                                if ((job.PTarget == ActionTarget.Eq) && (job.PTargetId != (int)EqId.VAS))
                                {
                                    if (cv_GetPutStandbyExceptVas)
                                    {
                                        GetEqStandbyExceptVas(job.PTargetId, job.PTargetSlot, job.PGetArm);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotGetStart)
                {
                    if (!robot_get_arm_data && !robot_get_arm_sensor)
                    {
                        bool eq_ready = (cv_Mio.GetPortValue(time_chart_instance.cv_EqBitStart + (int)EqSideBitAddressOffset.Equipment_Ready) == 1);
                        bool eq_start = (cv_Mio.GetPortValue(time_chart_instance.cv_EqBitStart + (int)EqSideBitAddressOffset.Transfer_Start) == 1);
                        if (eq_ready && eq_start)
                        {
                            time_chart_instance.SetTrigger(time_chart_id);
                            if (eq_id != EqId.VAS)
                            {
                                GetPutNormalEq(job.PGetArm, eq_id, 1, true, true);
                            }
                            else
                            {
                                if (job.PGetArm == RobotArm.rbaDown)
                                {
                                    GetVas(2, false);
                                }
                            }
                        }
                    }
                }
                else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotGetEnd)
                {
                    if (robot_get_arm_sensor && !robot.IsBusy)
                    {
                        glass = new GlassData(cv_Mio, time_chart_instance.cv_ReadDataStartPort);
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm] = glass;
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor = robot_get_arm_sensor;
                        robot.cv_Data.GlassDataMap[(int)job.PGetArm].cv_SlotInEq = (uint)job.PGetArm;
                        robot.SendDataViaMmf();
                        robot.cv_Data.SaveToFile();
                        time_chart_instance.SetTrigger(time_chart_id);
                    }
                }
                else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotGetVasStandByStart)
                {
                    if (!robot_get_arm_sensor)
                    {
                        bool eq_ready = (cv_Mio.GetPortValue(time_chart_instance.cv_EqBitStart + (int)EqSideBitAddressOffset.Equipment_Ready) == 1);
                        if (eq_ready)
                        {
                            time_chart_instance.SetTrigger(time_chart_id);
                            if (eq_id == EqId.VAS)
                            {
                                if (job.PGetArm == RobotArm.rbaDown)
                                {
                                    GetVas(1, false);
                                }
                            }
                        }
                    }
                }
                else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotGetVasStandByEnd)
                {
                    if (cv_Mio.GetPortValue((int)EqSideBitAddressOffset.Stage_Delivery_Ready +
                        time_chart_instance.cv_EqBitStart) == 1)
                    {
                        time_chart_instance.SetTrigger(time_chart_id);
                        cv_Mio.SetPortValue((int)RobotSideBitAddressOffset.Robot_Delivery_Ready +
                            time_chart_instance.cv_RobotBitStart, 0);
                    }
                }
                else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotCommandFinish)
                {
                    if (robot_get_arm_sensor && !robot.IsBusy)
                    {
                        time_chart_instance.SetTrigger(time_chart_id);
                        cv_MmfController.SendBcTreansferReport(DataFlowAction.Receive, robot.cv_Data.GlassDataMap[(int)job.PGetArm]);
                        //SendBcTreansferReport()
                    }
                }
                else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitEqCompleteOn)
                {
                    if (robot_get_arm_sensor && !robot.IsBusy)
                    {
                        if (cv_Mio.GetPortValue((int)EqSideBitAddressOffset.Transfer_Complete +
                            time_chart_instance.cv_EqBitStart) == 1)
                        {
                            time_chart_instance.SetTrigger(time_chart_id);
                            if (!m_IsMaunal)
                            {
                                if (cv_IsCycleStop)
                                {
                                    PSystemData.POperationModeLeft = OperationMode.Manual;
                                    cv_IsCycleStop = false;
                                }
                                cv_RobotJobPath.Dequeue();
                            }
                            else
                                cv_RobotManaulJobPath.Dequeue();
                        }
                    }
                }
            }
            else if (m_Type == RobotAction.Put)
            {
                bool robot_put_arm_sensor = robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor;
                bool robot_put_arm_data = robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasData;
                if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_ActionReady)
                {
                    if (cv_IsCycleStop)
                    {
                        PSystemData.POperationModeLeft = OperationMode.Manual;
                        cv_IsCycleStop = false;
                    }
                    if (gif_type != EqInterFaceType.Load)// && gif_type != EqInterFaceType.Exchange)
                    {
                        return;
                    }
                    if (robot_put_arm_data && robot_put_arm_sensor)
                    {
                        //
                        int node_index = robot.cv_Data.GlassDataMap[(int)job.PPutArm].cv_Nods.FindIndex(x => x.PNodeId == 2);
                        if (node_index != -1)
                        {
                            if (robot.cv_Data.GlassDataMap[(int)job.PPutArm].cv_Nods[node_index].PProcessHistory != 1)
                            {
                                robot.cv_Data.GlassDataMap[(int)job.PPutArm].cv_Nods[node_index].PProcessHistory = 1;
                                CommonData.HIRATA.MDBCWorkDataUpdateReport report_bc = new MDBCWorkDataUpdateReport();
                                report_bc.PGlass = robot.cv_Data.GlassDataMap[(int)job.PPutArm];
                                cv_MmfController.SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCWorkDataUpdateReport).Name, report_bc, KParseObjToXmlPropertyType.Field);
                            }
                        }
                        //
                        robot.cv_Data.GlassDataMap[(int)job.PPutArm].Write(cv_Mio,
                                time_chart_instance.cv_WriteDataStartPort);
                        GlassData tmp_data = new GlassData(cv_Mio, time_chart_instance.cv_WriteDataStartPort);
                        if (tmp_data.PId.Trim() == robot.cv_Data.GlassDataMap[(int)job.PPutArm].PId.Trim())
                        {
                            time_chart_instance.cv_PutData = tmp_data;
                            time_chart_instance.cv_PutArm = job.PPutArm;
                            time_chart_instance.cv_Action = EqInterFaceType.Load;
                            cv_Mio.SetPortValue(time_chart_instance.cv_RobotBitStart +
                                (int)RobotSideBitAddressOffset.Load_Only_Reply, 1);
                            time_chart_instance.SetTrigger(time_chart_id);
                            if (job.PTargetId == (int)EqId.UV_1)
                            {
                                cv_WaitUvRecordTime = SysUtils.Now();
                            }
                            if (job.PTarget == ActionTarget.Eq && job.PTargetId == (int)EqId.VAS)
                            {
                                if(job.PTargetSlot == 1)
                                {
                                    PutVasStandby(true);
                                }
                                else if(job.PTargetSlot == 2)
                                {
                                    if(cv_PutGlassStandby)
                                    {
                                        PutVasStandby(false);
                                    }
                                }
                            }
                            else
                            {
                                if ((job.PTarget == ActionTarget.Eq) && (job.PTargetId != (int)EqId.VAS))
                                {
                                    if (cv_GetPutStandbyExceptVas)
                                    {
                                        PutEqStandbyExceptVas(job.PTargetId, job.PTargetSlot, job.PPutArm);
                                    }
                                }
                            }

                        }
                    }
                }
                else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotPutStart)
                {
                    if (robot_put_arm_sensor)
                    {
                        bool eq_ready = (cv_Mio.GetPortValue(time_chart_instance.cv_EqBitStart + (int)EqSideBitAddressOffset.Equipment_Ready) == 1);
                        bool eq_start = (cv_Mio.GetPortValue(time_chart_instance.cv_EqBitStart + (int)EqSideBitAddressOffset.Transfer_Start) == 1);
                        if (eq_ready && eq_start)
                        {
                            time_chart_instance.SetTrigger(time_chart_id);
                            if (eq_id != EqId.VAS)
                            {
                                cv_Mio.SetPortValue((int)RobotSideBitAddressOffset.Interlock_2 +
                                    time_chart_instance.cv_RobotBitStart, 0);

                                GetPutNormalEq(job.PPutArm, eq_id, 1, false, true);
                            }
                            else
                            {
                                if (job.PTargetSlot == 1)
                                {
                                    if (job.PPutArm == RobotArm.rbaUp)
                                    {
                                        PutVasSlot(true, 2, true);
                                        cv_Mio.SetPortValue((int)RobotSideBitAddressOffset.Interlock_2 +
                                            time_chart_instance.cv_RobotBitStart, 0);
                                    }
                                }
                                else if (job.PTargetSlot == 2)
                                {
                                    if (job.PPutArm == RobotArm.rbaDown)
                                    {
                                        PutVasSlot(false, 2, true);
                                        cv_Mio.SetPortValue((int)RobotSideBitAddressOffset.Interlock_2 +
                                            time_chart_instance.cv_RobotBitStart, 0);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotPutEnd)
                {
                    if (!robot_put_arm_sensor) //&& !robot.IsBusy )
                    {
                        if (!robot.IsBusy)
                        {
                            robot.cv_Data.GlassDataMap[(int)job.PPutArm] = new GlassData();
                            robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor = robot_put_arm_sensor;
                            robot.cv_Data.GlassDataMap[(int)job.PPutArm].cv_SlotInEq = (uint)job.PPutArm;
                            time_chart_instance.SetTrigger(time_chart_id);
                            robot.SendDataViaMmf();
                            robot.cv_Data.SaveToFile();
                        }
                        else if (LgcForm.CheckIsVasPutUpSlotJobStatus(job))
                        {
                            robot.cv_Data.GlassDataMap[(int)job.PPutArm] = new GlassData();
                            robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor = robot_put_arm_sensor;
                            robot.cv_Data.GlassDataMap[(int)job.PPutArm].cv_SlotInEq = (uint)job.PPutArm;
                            //time_chart_instance.SetTrigger(time_chart_id);
                            time_chart_instance.JumpToStep(time_chart_id, (int)TimechartNormal.STEP_ID_WaitRobotCompleteOn);
                            robot.SendDataViaMmf();
                            robot.cv_Data.SaveToFile();
                        }
                    }
                }
                else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotPutVasStandByStart)
                {
                    if (robot_put_arm_sensor)
                    {
                        bool eq_ready = (cv_Mio.GetPortValue(time_chart_instance.cv_EqBitStart + (int)EqSideBitAddressOffset.Equipment_Ready) == 1);
                        if (eq_ready)
                        {
                            if (eq_id == EqId.VAS)
                            {
                                if (job.PTargetSlot == 1)
                                {
                                    if (job.PPutArm == RobotArm.rbaUp)
                                    {
                                        PutVasSlot(true, 1, true);
                                        time_chart_instance.SetTrigger(time_chart_id);
                                    }
                                }
                                else if (job.PTargetSlot == 2)
                                {
                                    if (job.PPutArm == RobotArm.rbaDown)
                                    {
                                        PutVasSlot(false, 1, true);
                                        time_chart_instance.SetTrigger(time_chart_id);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotPutVasStandByEnd)
                {
                    if (cv_Mio.GetPortValue((int)EqSideBitAddressOffset.Stage_Delivery_Ready +
                        time_chart_instance.cv_EqBitStart) == 1)
                    {
                        time_chart_instance.SetTrigger(time_chart_id);
                        cv_Mio.SetPortValue((int)RobotSideBitAddressOffset.Robot_Delivery_Ready +
                            time_chart_instance.cv_RobotBitStart, 0);
                    }
                }
                else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotCommandFinish)
                {
                    if (!robot_put_arm_sensor)// && !robot.IsBusy)
                    {
                        if (!robot.IsBusy)
                        {
                            time_chart_instance.SetTrigger(time_chart_id);
                            cv_Mio.SetPortValue(time_chart_instance.cv_RobotBitStart + (int)RobotSideBitAddressOffset.Receipt_Complete, 1);
                            robot.SendDataViaMmf();
                        }
                        else
                        {
                            if (CheckIsVasPutUpSlotJobStatus(job))
                            {
                                if (cv_Mio.GetPortValue(time_chart_instance.cv_RobotBitStart + (int)RobotSideBitAddressOffset.Receipt_Complete) == 1)
                                {
                                    time_chart_instance.SetTrigger(time_chart_id);
                                    cv_Mio.SetPortValue(time_chart_instance.cv_RobotBitStart + (int)RobotSideBitAddressOffset.Receipt_Complete, 1);
                                    robot.SendDataViaMmf();
                                }
                            }
                        }
                    }
                }
                else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitEqCompleteOn)
                {
                    if (!robot_put_arm_sensor)// && !robot.IsBusy)
                    {
                        if (!robot.IsBusy)
                        {
                            if (cv_Mio.GetPortValue((int)EqSideBitAddressOffset.Transfer_Complete +
                                time_chart_instance.cv_EqBitStart) == 1)
                            {
                                time_chart_instance.SetTrigger(time_chart_id);
                                if (!m_IsMaunal)
                                {
                                    cv_RobotJobPath.Dequeue();
                                    if (cv_IsCycleStop)
                                    {
                                        PSystemData.POperationModeLeft = OperationMode.Manual;
                                        cv_IsCycleStop = false;
                                    }
                                }
                                else
                                    cv_RobotManaulJobPath.Dequeue();
                            }
                            else if (CheckIsVasPutUpSlotJobStatus(job))
                            {
                                time_chart_instance.SetTrigger(time_chart_id);
                                if (!m_IsMaunal)
                                {
                                    cv_RobotJobPath.Dequeue();
                                    if (cv_IsCycleStop)
                                    {
                                        PSystemData.POperationModeLeft = OperationMode.Manual;
                                        cv_IsCycleStop = false;
                                    }
                                }
                                else
                                    cv_RobotManaulJobPath.Dequeue();
                            }
                        }
                        else
                        {
                            if (CheckIsVasPutUpSlotJobStatus(job))
                            {
                                if (cv_Mio.GetPortValue(time_chart_instance.cv_EqBitStart + (int)EqSideBitAddressOffset.Transfer_Complete) == 1)
                                {
                                    cv_Mio.SetPortValue(time_chart_instance.cv_RobotBitStart + (int)RobotSideBitAddressOffset.Receipt_Complete, 0);
                                    cv_Mio.SetPortValue(time_chart_instance.cv_RobotBitStart + (int)RobotSideBitAddressOffset.Load_Only_Reply, 0);
                                    cv_Mio.SetPortValue(time_chart_instance.cv_RobotBitStart + (int)RobotSideBitAddressOffset.Interlock_2, 1);
                                }
                            }
                        }
                    }
                }
                else
                {
                }
            }
            else if (job.PAction == RobotAction.Exchange)
            {
                bool robot_get_arm_sensor = robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor;
                bool robot_get_arm_data = robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasData;
                bool robot_put_arm_sensor = robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor;
                bool robot_put_arm_data = robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasData;
                if (eq_id != EqId.VAS)
                {
                    GlassData glass = null;
                    if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_ActionReady)
                    {
                        if (cv_IsCycleStop)
                        {
                            PSystemData.POperationModeLeft = OperationMode.Manual;
                            cv_IsCycleStop = false;
                        }
                        if (gif_type != EqInterFaceType.Exchange)
                        {
                            return;
                        }

                        if (!robot_get_arm_data && !robot_get_arm_sensor &&
                            robot_put_arm_data && robot_put_arm_sensor)
                        {
                            robot.cv_Data.GlassDataMap[(int)job.PPutArm].Write(cv_Mio,
                                time_chart_instance.cv_WriteDataStartPort);

                            GlassData tmp_data = new GlassData(cv_Mio, time_chart_instance.cv_WriteDataStartPort);
                            if (tmp_data.PId.Trim() == robot.cv_Data.GlassDataMap[(int)job.PPutArm].PId.Trim())
                            {

                                glass = new GlassData(cv_Mio, time_chart_instance.cv_ReadDataStartPort);
                                //
                                string glass_id = glass.PId;
                                string combination = glass.PAssamblyResult.ToString();
                                for (int i = 1; i <= 15; i++)
                                {
                                    int node_index = glass.cv_Nods.FindIndex(x => x.PNodeId == i);
                                    if (node_index != -1)
                                    {
                                        int history = glass.cv_Nods[node_index].cv_ProcessHistory;
                                        int recipe = glass.cv_Nods[node_index].cv_Recipe;
                                    }
                                }
                                if (glass.PHasData)
                                {
                                    if (!CheckEqSideData(glass, eq_id))
                                    {
                                        return;
                                    }
                                    cv_Mio.SetPortValue(time_chart_instance.cv_RobotBitStart +
                                        (int)RobotSideBitAddressOffset.Exchange_Reply, 1);
                                    time_chart_instance.cv_Action = EqInterFaceType.Exchange;
                                    time_chart_instance.cv_GetData = glass;
                                    time_chart_instance.cv_GetArm = job.PGetArm;
                                    time_chart_instance.cv_PutData = tmp_data;
                                    time_chart_instance.cv_PutArm = job.PPutArm;
                                    time_chart_instance.SetTrigger(time_chart_id);

                                    if(cv_GetPutStandbyExceptVas)
                                    {
                                        GetEqStandbyExceptVas(job.cv_TargetId, job.cv_TargetSlot, job.PGetArm);
                                    }
                                }
                            }
                        }
                    }
                    else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotGetStart)
                    {
                        if (!robot_get_arm_sensor && robot_put_arm_sensor)
                        {
                            bool eq_ready = (cv_Mio.GetPortValue(time_chart_instance.cv_EqBitStart + (int)EqSideBitAddressOffset.Equipment_Ready) == 1);
                            bool eq_tr_start = (cv_Mio.GetPortValue(time_chart_instance.cv_EqBitStart + (int)EqSideBitAddressOffset.Transfer_Start) == 1);
                            if (eq_ready && eq_tr_start)
                            {
                                time_chart_instance.SetTrigger(time_chart_id);
                                GetPutNormalEq(job.PGetArm, eq_id, 1, true, true);
                            }
                        }
                    }
                    else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotGetEnd)
                    {
                        if (robot_put_arm_sensor && robot_get_arm_sensor)
                        {
                            glass = new GlassData(cv_Mio, time_chart_instance.cv_ReadDataStartPort);
                            robot.cv_Data.GlassDataMap[(int)job.PGetArm] = glass;
                            robot.cv_Data.GlassDataMap[(int)job.PGetArm].PHasSensor = robot_get_arm_sensor;
                            robot.cv_Data.GlassDataMap[(int)job.PGetArm].cv_SlotInEq = (uint)job.PGetArm;
                            time_chart_instance.JumpToStep(time_chart_id,
                                (int)TimechartNormal.STEP_ID_WaitRobotPutStart);
                            cv_MmfController.SendBcTreansferReport(DataFlowAction.Receive, robot.cv_Data.GlassDataMap[(int)job.PGetArm]);

                        }
                    }
                    else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotPutStart)
                    {
                        if (robot_put_arm_sensor && robot_get_arm_sensor)
                        {
                            bool eq_ready = (cv_Mio.GetPortValue(time_chart_instance.cv_EqBitStart + (int)EqSideBitAddressOffset.Equipment_Ready) == 1);
                            if (eq_ready)
                            {
                                time_chart_instance.SetTrigger(time_chart_id);
                                GetPutNormalEq(job.PPutArm, eq_id, 1, false, true);
                                int node_index = robot.cv_Data.GlassDataMap[(int)job.PPutArm].cv_Nods.FindIndex(x => x.PNodeId == 2);
                                if (robot.cv_Data.GlassDataMap[(int)job.PPutArm].cv_Nods[node_index].PProcessHistory != 1)
                                {
                                    robot.cv_Data.GlassDataMap[(int)job.PPutArm].cv_Nods[node_index].PProcessHistory = 1;
                                    CommonData.HIRATA.MDBCWorkDataUpdateReport report_bc = new MDBCWorkDataUpdateReport();
                                    report_bc.PGlass = robot.cv_Data.GlassDataMap[(int)job.PPutArm];
                                    cv_MmfController.SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCWorkDataUpdateReport).Name, report_bc, KParseObjToXmlPropertyType.Field);
                                }
                            }
                        }
                    }
                    else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotPutEnd)
                    {
                        if (!robot_put_arm_sensor && robot_get_arm_sensor)
                        {
                            robot.cv_Data.GlassDataMap[(int)job.PPutArm] = new GlassData();
                            robot.cv_Data.GlassDataMap[(int)job.PPutArm].PHasSensor = robot_put_arm_sensor;
                            robot.cv_Data.GlassDataMap[(int)job.PPutArm].cv_SlotInEq = (uint)job.PPutArm;
                            time_chart_instance.SetTrigger(time_chart_id);
                        }
                    }
                    else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitRobotCommandFinish)
                    {
                        if (!robot_put_arm_sensor && robot_get_arm_sensor && !robot.IsBusy)
                        {
                            time_chart_instance.SetTrigger(time_chart_id);
                        }
                    }
                    else if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_WaitEqCompleteOn)
                    {
                        if (!robot_put_arm_sensor && robot_get_arm_sensor && !robot.IsBusy)
                        {
                            if (cv_Mio.GetPortValue((int)EqSideBitAddressOffset.Transfer_Complete +
                                    time_chart_instance.cv_EqBitStart) == 1)
                            {
                                time_chart_instance.SetTrigger(time_chart_id);
                                if (!m_IsMaunal)
                                {
                                    cv_RobotJobPath.Dequeue();
                                    if (cv_IsCycleStop)
                                    {
                                        PSystemData.POperationModeLeft = OperationMode.Manual;
                                        cv_IsCycleStop = false;
                                    }
                                }
                                else
                                    cv_RobotManaulJobPath.Dequeue();
                            }
                        }
                    }
                }
            }
            */
        }
        private void DoRobotJobPath(bool m_IsManualCommand = false)
        {
            /*
            RobotJob job = null;
            if (m_IsManualCommand)
                job = cv_RobotManaulJobPath.Peek();
            else
                job = cv_RobotJobPath.Peek();
            Robot robot = GetRobotById(1);
            if (!m_IsManualCommand)
            {
                if (cv_RobotJobPath.Count == 0) return;
            }
            else
            {
                if (cv_RobotManaulJobPath.Count == 0) return;
            }
            if (robot.IsBusy && !CheckIsVasPutUpSlotJobStatus(job)) return;
            if (job.PAction == RobotAction.Get)// ||
            // (job.PAction == RobotAction.Exchange && job.PTarget == ActionTarget.Aligner &&
            //job.PIsWaitGet && !job.PisWaitPut))
            {
                if (job.PTarget == ActionTarget.Port)
                {
                    ProcessPortGetPutJob(job.PAction);
                }
                else if (job.PTarget == ActionTarget.Buffer)
                {
                    ProcessBufferGetPutJob(job.PAction);
                }
                else if (job.PTarget == ActionTarget.Aligner)
                {
                    ProcessAlignerGetPutJob(job.PAction);
                }
                else if (job.PTarget == ActionTarget.Eq)
                {
                    ProcessEqGetPutJob(job.PAction, m_IsManualCommand);
                }
            }
            else if (job.PAction == RobotAction.Put)//|| (
            //(job.PAction == RobotAction.Exchange) &&
            //(job.PTarget == ActionTarget.Aligner) && (job.PisWaitPut) && (job.PIsWaitGet))
            //)
            {
                if (job.PTarget == ActionTarget.Aligner)
                {
                    ProcessAlignerGetPutJob(job.PAction);
                }
                else if (job.PTarget == ActionTarget.Buffer)
                {
                    ProcessBufferGetPutJob(RobotAction.Put);
                }
                else if (job.PTarget == ActionTarget.Port)
                {
                    ProcessPortGetPutJob(RobotAction.Put);
                }
                else if (job.PTarget == ActionTarget.Eq)
                {
                    ProcessEqGetPutJob(job.PAction, m_IsManualCommand);
                }
            }
            else if (job.PAction == RobotAction.Exchange)
            {
                if (job.PTarget == ActionTarget.Eq)
                {
                    ProcessEqGetPutJob(job.PAction, m_IsManualCommand);
                }
                else if (job.PTarget == ActionTarget.Aligner)
                {
                    // for putgetAligner use only.
                    ProcessAlignerGetPutJob(job.PAction, m_IsManualCommand);
                }
            }
            */
        }
        private void ProcessAfterUvPutWait()
        {

        }

        #region Find start step ( not use )
        /*
        private bool FindStartStep(int m_CurStep, ref int m_StartPos, ref Dictionary<int, RobotJob> m_JobMap)
        {
            bool rtn = false;
            int next_step = m_CurStep;
            int now_step = m_CurStep - 1;
            int StartPos = now_step;
            if (cv_CurRecipeFlowStepSetting.ContainsKey(now_step))
            {
                List<AllDevice> cv_stepDevice = cv_CurRecipeFlowStepSetting[now_step];
                foreach (AllDevice device in cv_stepDevice)
                {
                    if (device == AllDevice.Aligner)
                    {
                        if (GetAlignerById(1).cv_Data.GlassDataMap[1].PHasData && GetAlignerById(1).cv_Data.GlassDataMap[1].PHasSensor)
                        {
                            m_JobMap[now_step] = new RobotJob(1, RobotArm.rabNone, m_JobMap[next_step].PPutArm, RobotAction.Get,
                                ActionTarget.Aligner, 1, 1, false);
                            rtn = FindStartStep(now_step, ref m_StartPos, ref m_JobMap);
                        }
                    }
                    else if (device == AllDevice.LP)
                    {
                        int port = 0;
                        int slot = 0;
                        if (FindUnloadPPortToPutSubstrate(out port, out slot))
                        {
                            m_JobMap[now_step] = new RobotJob(1, RobotArm.rabNone, m_JobMap[next_step].PPutArm, RobotAction.Get,
                                ActionTarget.Port, port, slot, false);
                            rtn = true;
                            StartPos = now_step;
                            break;
                        }
                    }
                    else if (device == AllDevice.Buffer)
                    {
                        int port = 0;
                        int slot = 0;
                        if (GetBufferById(1).cv_Data.GetUnloadSlot(BufferSlotType.Wafer, out slot))
                        {
                            m_JobMap[now_step] = new RobotJob(1, RobotArm.rabNone, m_JobMap[next_step].PPutArm, RobotAction.Get,
                                ActionTarget.Buffer, 1, slot, false);
                            rtn = true;
                            StartPos = now_step;
                            break;
                        }
                    }
                    else
                    {
                        EqId eq_id = EqId.None;
                        int time_chart_instance = 0;
                        int eq_time_chart_cur_step = 0;
                        if (Enum.TryParse<EqId>(device.ToString(), out eq_id))
                        {
                            if (eq_id == EqId.VAS)
                            {
                                eq_time_chart_cur_step = GetEqById((int)eq_id).GetTimeChatCurStep(1);
                                time_chart_instance = (int)EqGifTimeChartId.TIMECHART_ID_VAS_DOWN;
                            }
                            else
                            {
                                eq_time_chart_cur_step = GetEqById((int)eq_id).GetTimeChatCurStep(1);
                                time_chart_instance = GetEqById((int)eq_id).cv_Comm.cv_TimeChatId;
                            }
                        }
                        if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_ActionReady)
                        {
                            EqInterFaceType gif_type = cv_MmfController.cv_TimechartController.GetTimeChartInstance(time_chart_instance).cv_ActionType;
                            Eq eq = GetEqById((int)eq_id);
                            if (gif_type == EqInterFaceType.Unload)
                            {
                                if (m_JobMap[next_step].PTarget == ActionTarget.Aligner)
                                {
                                    m_JobMap[next_step].PPutArm = eq.PGetArm; // = new RobotJob(1, eq.PPutArm, RobotArm.rabNone, RobotAction.Put, ActionTarget.Eq, (int)eq_id, 1, true);
                                    m_JobMap[now_step] = new RobotJob(1, RobotArm.rabNone, eq.PGetArm, RobotAction.Get, ActionTarget.Eq, (int)eq_id, 1, true);
                                    rtn = true;
                                    StartPos = now_step;
                                    break;
                                }
                                else
                                {
                                    if (m_JobMap[next_step].PPutArm == eq.PGetArm)
                                    {
                                        m_JobMap[now_step] = new RobotJob(1, RobotArm.rabNone, eq.PGetArm, RobotAction.Get, ActionTarget.Eq, (int)eq_id, 1, true);
                                        rtn = true;
                                        StartPos = now_step;
                                        break;
                                    }
                                }
                            }
                            if (gif_type == EqInterFaceType.Exchange)
                            {
                                if (m_JobMap[next_step].PTarget == ActionTarget.Aligner)
                                {
                                    m_JobMap[next_step].PPutArm = eq.PGetArm;
                                    m_JobMap[now_step] = new RobotJob(1, eq.PPutArm, eq.PGetArm, RobotAction.Exchange, ActionTarget.Eq, (int)eq_id, 1, true);
                                }
                                else
                                {
                                    m_JobMap[now_step] = new RobotJob(1, eq.PPutArm, eq.PGetArm, RobotAction.Exchange, ActionTarget.Eq, (int)eq_id, 1, true);
                                }

                                if (!FindStartStep(now_step, ref m_StartPos, ref m_JobMap))
                                {
                                    m_JobMap[now_step].PAction = RobotAction.Get;
                                    m_StartPos = now_step;
                                    rtn = true;
                                }
                                else
                                {
                                    rtn = true;
                                }
                            }
                        }
                    }
                }
            }
            m_StartPos = StartPos;
            return rtn;
        }
        */
        #endregion
        #endregion
    }
}

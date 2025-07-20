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
            //FindAlignerJob just search substrate in aligner and the data not preAction.
            DerivedTimer();
            CalculateSystemStatus();

            StepJob left_job = null;
            StepJob right_job = null;

            //processLeftAlignerAction(enSideGroup.Left);
            //processLeftAlignerAction(enSideGroup.Right);

            if (left_job == null)
            {
                DoLeftSideFindJob(out left_job);
                if(left_job != null)
                {
                    Robot left_robot = GetRobotBySide(enSideGroup.Left);
                    if (left_robot.CurStepJob != null)
                    {
                        left_robot.CurStepJob = left_job;
                    }
                }
            }
            /*
            if(left_job != null)
            {
                DoLeftSideJob(left_job);
            }

            if (right_job != null)
            {
                DoRightSideFindJob(out right_job);
            }
            if (right_job != null)
            {
                DoLeftSideJob(right_job);
            }
            */
        }
        private bool checkEqAskLoad(List<AllDevice> m_Devs , out AllDevice m_Dev)
        {
            bool rtn = false;
            m_Dev = AllDevice.None;
            for(int i=0; i<m_Devs.Count;i++)
            {
                AllDevice dev = m_Devs[i];
                EqId eq_id = EqId.None;
                int time_chart_instance = 0;
                int eq_time_chart_cur_step = 0;
                if(Enum.TryParse<EqId>(dev.ToString() , out eq_id))
                {
                    if(eq_id == EqId.VAS1) //|| eq_id == EqId.VAS2)
                    {
                        eq_time_chart_cur_step = GetEqById((int)eq_id).GetTimeChatCurStep(1);
                        time_chart_instance = (int)EqGifTimeChartId.TIMECHART_ID_VAS1_DOWN;
                    }
                    else if(eq_id == EqId.VAS2) //|| eq_id == EqId.VAS2)
                    {
                        eq_time_chart_cur_step = GetEqById((int)eq_id).GetTimeChatCurStep(1);
                        time_chart_instance = (int)EqGifTimeChartId.TIMECHART_ID_VAS2_DOWN;
                    }
                    else
                    {
                        eq_time_chart_cur_step = GetEqById((int)eq_id).GetTimeChatCurStep(1);
                        time_chart_instance = GetEqById((int)eq_id).cv_Comm.cv_TimeChatId;
                    }
                }
                if (eq_time_chart_cur_step == (int)TimechartNormal.STEP_ID_ActionReady)
                {
                    EqInterFaceType gif_type = g_eventController.cv_TimechartController.GetTimeChartInstance(time_chart_instance).cv_ActionType;
                    if (gif_type == EqInterFaceType.Load ||gif_type == EqInterFaceType.Exchange )
                    {
                        rtn = true;
                        m_Dev = dev;
                        break;
                    }
                }
            }
            return rtn;
        }

        private void DoLeftSideFindJob( out StepJob m_Job , enSideGroup m_Side = enSideGroup.Left)
        {
            m_Job = null;
            if (!checkConditionForFindJob(m_Side))
            { return; } 
            Robot robot = GetRobotBySide(m_Side);
            List<int> checkdatasensormatch = new List<int>();
            if (!robot.cv_Data.IsSensorDataMatch(out checkdatasensormatch))
            {
                CommonData.HIRATA.AlarmItem alarm = new AlarmItem();
                alarm.PCode = Alarmtable.DataAndSensorUnmatch.ToString();
                alarm.PMainDescription = "Data And Sensor Unmatch. Robot : " + m_Side.ToString();
                alarm.PSubDescription = "";
                alarm.PUnit = 0;
                alarm.PLevel = AlarmLevele.Serious;
                alarm.PStatus = AlarmStatus.Occur;
                alarm.PSide = m_Side;
                alarm.PTime = DateTime.Now.ToString("yyyyMMDDHHmmss");
                EditAlarm(alarm);
                return;
            }
            Buffer left_buffer = GetBufferBySide(enSideGroup.Left);
            Buffer mid_buffer = GetBufferBySide(enSideGroup.Both);
            Aligner left_ali = GetAlignerBySide(enSideGroup.Left);
            Aligner right_ali = GetAlignerBySide(enSideGroup.Right);

            FLowFirstStepType first_type = cv_FlowData.whatIsNextPort(); //left flow has three types: 1.port->aligner->buffer. 2. port->EQ. 3. port->buffer2.

            if (robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].PHasSensor && robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].PHasData &&
                robot.cv_Data.GlassDataMap[(int)RobotArm.rbaUp].PHasSensor && robot.cv_Data.GlassDataMap[(int)RobotArm.rbaUp].PHasData)
            {
                GlassData up_glass = robot.cv_Data.GlassDataMap[(int)RobotArm.rbaUp];
                GlassData down_glass = robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown];
            }
            else if (robot.cv_Data.GlassDataMap[(int)RobotArm.rbaUp].PHasSensor && robot.cv_Data.GlassDataMap[(int)RobotArm.rbaUp].PHasData)
            {
                MakeLeftSideRobotSingleArmStepData(RobotArm.rbaUp, out m_Job);
            }
            else if (robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].PHasSensor && robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].PHasData)
            {
                MakeLeftSideRobotSingleArmStepData(RobotArm.rbaUp, out m_Job);
            }
            else if (!robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].PHasSensor && !robot.cv_Data.GlassDataMap[(int)RobotArm.rbaDown].PHasData &&
                !robot.cv_Data.GlassDataMap[(int)RobotArm.rbaUp].PHasSensor && !robot.cv_Data.GlassDataMap[(int)RobotArm.rbaUp].PHasData)
            {
                if (first_type == FLowFirstStepType.Aligner)
                {
                    List<AllDevice> after_buffer_eqs;
                    FLowStepTarget after_buffer1 = cv_FlowData.WahtIsAfterBuffer1(out after_buffer_eqs);
                    AlignerPreAction ali_status = left_ali.cv_Data.PPreAction;
                    bool enterAlignerOverOnce = cv_FlowData.IsEnterAlignerOverOnce();
                    int howmany_wafer_can_into_buffer1 = left_buffer.cv_Data.howManyFreeSlot(BufferSlotType.Wafer);
                    if (after_buffer1 == FLowStepTarget.EQ)
                    {
                        AllDevice which_eq_want_load = AllDevice.None;
                        bool eq_want_load = checkEqAskLoad(after_buffer_eqs, out which_eq_want_load);
                        if (left_ali.cv_Data.GlassDataMap[1].PHasData && left_ali.cv_Data.GlassDataMap[1].PHasSensor)
                        {
                            GlassData ali_data = left_ali.cv_Data.GlassDataMap[1];
                            if (ali_data.IsEnterEq())
                            {
                                //wait substate that already into flow.
                                m_Job = new StepJob(robot.cv_Id, RobotArm.rabNone, cv_GetAlignerArm, RobotAction.Get, ActionTarget.Aligner, left_ali.cv_Id,
                                    1, false, false, true, ali_data.IsInReowrkFlow((int)EqId.AOI1) ? FlowType.LeftRework : FlowType.LeftNormal,
                                    ali_data.PCurFlowStep);
                            }
                            else
                            {
                                Buffer bf1 = GetBufferBySide(m_Side);
                                if (bf1.cv_Data.IsFreeSlot(BufferSlotType.Wafer) == -1)
                                {
                                    //get aligner because buffer1 is FIFO. maybe can change to don't care order by setting.
                                    m_Job = new StepJob(robot.cv_Id, RobotArm.rabNone, cv_GetAlignerArm, RobotAction.Get, ActionTarget.Aligner, left_ali.cv_Id,
                                    1, false, false, true, ali_data.IsInReowrkFlow((int)EqId.AOI1) ? FlowType.LeftRework : FlowType.LeftNormal,
                                    ali_data.PCurFlowStep);
                                }
                                else
                                {
                                    //pre-action : maximum is buffer1 full and aligner has one. (aligner one step is in pre-action step).
                                    int port = 0;
                                    int slot = 0;
                                    if (GetPortUnloadSlot(enSideGroup.Left, ProductCategory.Wafer, out port, out slot))
                                    {
                                        GlassData port_substrate = GetPortById(port).cv_Data.GlassDataMap[slot];
                                        m_Job = new StepJob(robot.cv_Id, RobotArm.rabNone, cv_GetPortArm, RobotAction.Get, ActionTarget.Port, port, slot,
                                            false, false, true, FlowType.LeftNormal, port_substrate.PCurFlowStep);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (eq_want_load)
                            {
                                // if aligner1 is empty and Eq send load , we get substrate in buffer1 directly , then go EQ.
                                Buffer bf1 = GetBufferBySide(m_Side);
                                int bf_slot = 0;
                                if (bf1.cv_Data.GetUnloadSlot(BufferSlotType.Wafer, out bf_slot))
                                {
                                    GlassData bf1_substrate = bf1.cv_Data.GlassDataMap[bf_slot];
                                    m_Job = new StepJob(robot.cv_Id, RobotArm.rabNone, cv_GetBufferArm, RobotAction.Get, ActionTarget.Buffer, bf1.cv_Id,
                                        bf_slot, false, false, true, FlowType.LeftNormal, bf1_substrate.PCurFlowStep);
                                }
                            }
                            else
                            {
                                //if aligner1 and buffer1 are empty. we just get port.
                                int port = 0;
                                int slot = 0;
                                if (GetPortUnloadSlot(enSideGroup.Left, ProductCategory.Wafer, out port, out slot))
                                {
                                    GlassData port_substrate = GetPortById(port).cv_Data.GlassDataMap[slot];
                                    m_Job = new StepJob(robot.cv_Id, RobotArm.rabNone, cv_GetPortArm, RobotAction.Get, ActionTarget.Port, port, slot,
                                        false, false, true, FlowType.LeftNormal, port_substrate.PCurFlowStep);
                                }
                            }
                        }
                    }
                    else if (after_buffer1 == FLowStepTarget.LP)
                    {//flow 22. LP-aligner1-buffer1-LP
                        int port = 0;
                        int slot = 0;
                        if (GetPortUnloadSlot(enSideGroup.Left, ProductCategory.Wafer, out port, out slot))
                        {
                            GlassData port_substrate = GetPortById(port).cv_Data.GlassDataMap[slot];
                            m_Job = new StepJob(robot.cv_Id, RobotArm.rabNone, cv_GetPortArm, RobotAction.Get, ActionTarget.Port, port, slot,
                                false, false, true, FlowType.LeftNormal, port_substrate.PCurFlowStep);
                        }
                        else
                        {
                            if (left_ali.cv_Data.GlassDataMap[1].PHasData && left_ali.cv_Data.GlassDataMap[1].PHasSensor)
                            {
                                GlassData ali_substrate = left_ali.cv_Data.GlassDataMap[1];
                                m_Job = new StepJob(robot.cv_Id, RobotArm.rabNone, cv_GetAlignerArm, RobotAction.Get, ActionTarget.Aligner, left_ali.cv_Id,
                                    1, false, false, true, FlowType.LeftNormal, ali_substrate.PCurFlowStep);
                            }
                            else
                            {
                                Buffer bf1 = GetBufferBySide(m_Side);
                                int bf_slot = 0;
                                if (bf1.cv_Data.GetUnloadSlot(BufferSlotType.Wafer, out bf_slot))
                                {
                                    GlassData bf1_substrate = bf1.cv_Data.GlassDataMap[bf_slot];
                                    m_Job = new StepJob(robot.cv_Id, RobotArm.rabNone, cv_GetBufferArm, RobotAction.Get, ActionTarget.Buffer, bf1.cv_Id,
                                        bf_slot, false, false, true, FlowType.LeftNormal, bf1_substrate.PCurFlowStep);
                                }
                            }
                        }
                    }
                }
                else if (first_type == FLowFirstStepType.Buffer)
                {//flow23. LP->buffer->LP
                    int port = 0;
                    int slot = 0;
                    Buffer buffer2 = GetBufferBySide(enSideGroup.Both);
                    if(buffer2.cv_Data.GetUnloadSlot(BufferSlotType.Wafer , out slot))
                    {
                        GlassData buffer2_substrate = buffer2.cv_Data.GlassDataMap[slot];
                        m_Job = new StepJob(robot.cv_Id, RobotArm.rabNone, cv_GetBufferArm, RobotAction.Get, ActionTarget.Buffer, buffer2.cv_Id, slot,
                            false, false, true, FlowType.LeftNormal, buffer2_substrate.PCurFlowStep);
                    }
                    else if (GetPortUnloadSlot(enSideGroup.Left , ProductCategory.Wafer, out port, out slot))
                    {
                        GlassData port_substrate = GetPortById(port).cv_Data.GlassDataMap[slot];
                        m_Job = new StepJob(robot.cv_Id, RobotArm.rabNone, cv_GetPortArm, RobotAction.Get, ActionTarget.Port, port, slot,
                            false, false, true, FlowType.LeftNormal, port_substrate.PCurFlowStep);
                    }
                }
                else if (first_type == FLowFirstStepType.EQ)
                {//means this flow don't need pre-action.
                    if (left_ali.cv_Data.GlassDataMap[1].PHasData && left_ali.cv_Data.GlassDataMap[1].PHasSensor)
                    {//actually , we don't care normal and rework flow in this flow. becase in case , we just focus on aligner 1 when it has substrate.
                        GlassData ali_substrate = left_ali.cv_Data.GlassDataMap[1];
                        if (ali_substrate.IsInReowrkFlow((int)EqId.AOI1))
                        {
                            m_Job = new StepJob(robot.cv_Id, RobotArm.rabNone, cv_GetAlignerArm, RobotAction.Get, ActionTarget.Aligner, left_ali.cv_Id,
                             1, false, false, true, FlowType.LeftRework, ali_substrate.PCurFlowStep);
                        }
                        else
                        {
                            m_Job = new StepJob(robot.cv_Id, RobotArm.rabNone, cv_GetAlignerArm, RobotAction.Get, ActionTarget.Aligner, left_ali.cv_Id,
                             1, false, false, true, FlowType.LeftNormal, ali_substrate.PCurFlowStep);
                        }
                    }
                    else
                    {
                        int port = 0;
                        int slot = 0;
                        if (GetPortUnloadSlot(enSideGroup.Left, ProductCategory.Wafer, out port, out slot))
                        {
                            GlassData port_substrate = GetPortById(port).cv_Data.GlassDataMap[slot];
                            m_Job = new StepJob(robot.cv_Id, RobotArm.rabNone, cv_GetBufferArm, RobotAction.Get, ActionTarget.Port, port, slot,
                                false, false, true, FlowType.LeftNormal, port_substrate.PCurFlowStep);
                        }
                        else
                        {
                            //todo : not wafer can load to eq , we need check eq unload singnal. before that we need think throught.
                        }
                    }
                }
            }

        }
        private void MakeLeftSideRobotSingleArmStepData( RobotArm m_Arm , out StepJob m_Job , enSideGroup m_Side= enSideGroup.Left)
        {
            m_Job = null;
            Robot robot = null;
            robot = GetRobotBySide(m_Side);
            GlassData down_glass = robot.cv_Data.GlassDataMap[(int)m_Arm];
            RobotArm put_arm = m_Arm;
            RobotArm get_arm = (m_Arm == RobotArm.rbaDown ? RobotArm.rbaUp : RobotArm.rbaDown);
            int cur_step = down_glass.PCurFlowStep;
            Aligner ali = GetAlignerBySide(m_Side);
            
            if (down_glass.IsInReowrkFlow((int)EqId.AOI1))
            {
                if (cv_FlowData.cv_LeftRework.ContainsKey(cur_step))
                {
                    FLowStepTarget next_target_type = cv_FlowData.GetLeftStepTargetType(cur_step, true);
                    if (next_target_type == FLowStepTarget.EQ)
                    {
                        //For Eq , becase we don't know eq signal. so we just prepare stepjob to wait eq.(just go to standby position).
                        m_Job = new StepJob(robot.cv_Id, put_arm , get_arm , RobotAction.Exchange, ActionTarget.Eq, 0, 1,
                            false, false, false, FlowType.LeftRework, down_glass.PCurFlowStep);
                    }
                    else if (next_target_type == FLowStepTarget.Aligner)
                    {
                        if (ali.cv_Data.GlassDataMap[1].PHasData && ali.cv_Data.GlassDataMap[1].PHasSensor)
                        {
                            m_Job = new StepJob(robot.cv_Id, put_arm, get_arm , RobotAction.Exchange, ActionTarget.Aligner, 1, 1,
                                false, false, true, FlowType.LeftRework, down_glass.PCurFlowStep);
                        }
                        else
                        {
                            m_Job = new StepJob(robot.cv_Id, put_arm, get_arm, RobotAction.Put, ActionTarget.Aligner, 1, 1,
                                false, false, true, FlowType.LeftRework, down_glass.PCurFlowStep);
                        }
                    }
                    else if (next_target_type == FLowStepTarget.Buffer1)
                    {
                            m_Job = new StepJob(robot.cv_Id, put_arm, get_arm , RobotAction.Exchange, ActionTarget.Buffer, 1, 0,
                                false, false, false , FlowType.LeftRework, down_glass.PCurFlowStep);
                    }
                    else if (next_target_type == FLowStepTarget.Buffer2)
                    {
                        Buffer buffer2 = GetBufferBySide(enSideGroup.Both);
                        int slot = 0;
                        //buffer2.cv_Data.GetUnloadSlot(BufferSlotType.Wafer, out slot);
                        slot = buffer2.cv_Data.IsFreeSlot(BufferSlotType.Wafer);
                        m_Job = new StepJob(robot.cv_Id, put_arm, get_arm, RobotAction.Put, ActionTarget.Buffer, 2, 0,
                            false, false, false, FlowType.LeftRework, down_glass.PCurFlowStep);
                    }
                }
            }
            else
            {
                if (cv_FlowData.cv_LeftNormal.ContainsKey(cur_step))
                {
                    FLowStepTarget next_target_type = cv_FlowData.GetLeftStepTargetType(cur_step, false);
                    if (next_target_type == FLowStepTarget.EQ)
                    {
                        m_Job = new StepJob(robot.cv_Id, put_arm, get_arm, RobotAction.Exchange, ActionTarget.Eq, 0, 1,
                            false, false, false, FlowType.LeftRework, down_glass.PCurFlowStep);
                    }
                    else if (next_target_type == FLowStepTarget.Aligner)
                    {
                        if (ali.cv_Data.GlassDataMap[1].PHasData && ali.cv_Data.GlassDataMap[1].PHasSensor)
                        {
                            m_Job = new StepJob(robot.cv_Id, put_arm, get_arm, RobotAction.Exchange, ActionTarget.Aligner, 1, 1,
                                false, false, true, FlowType.LeftRework, down_glass.PCurFlowStep);
                        }
                        else
                        {
                            m_Job = new StepJob(robot.cv_Id, put_arm, get_arm, RobotAction.Put, ActionTarget.Aligner, 1, 1,
                                false, false, true, FlowType.LeftRework, down_glass.PCurFlowStep);
                        }
                    }
                    else if (next_target_type == FLowStepTarget.Buffer1)
                    {
                        m_Job = new StepJob(robot.cv_Id, put_arm, get_arm, RobotAction.Exchange, ActionTarget.Buffer, 1, 0,
                            false, false, false, FlowType.LeftRework, down_glass.PCurFlowStep);
                    }
                    else if (next_target_type == FLowStepTarget.Buffer2)
                    {
                        m_Job = new StepJob(robot.cv_Id, put_arm, get_arm, RobotAction.Put, ActionTarget.Buffer, 2, 0,
                            false, false, false, FlowType.LeftRework, down_glass.PCurFlowStep);
                    }
                }
            }
        }
        private void processLeftAlignerAction(enSideGroup m_Side = enSideGroup.Left)
        {
            /*
            None, WaitHome, AlignerHome, SetToAngle, VuccumOff1, PutAligner, VuccumOn,
            WaitVuccumOn, FindNotch, WaitFindNotch, OcrConnect, WaitConnect, ReadOcr, WaitReadOct, ToAngle,
            WaitToAngle, VuccumOff2, WaitVuccomOff2, GetAligner,
            */
            Aligner aligner = GetAlignerBySide(m_Side);
            //if (aligner.cv_Data.GlassDataMap[1].PHasData && aligner.cv_Data.GlassDataMap[1].PHasSensor)
            GlassData ali_data = aligner.cv_Data.GlassDataMap[1];
            if (aligner.cv_Data.PPreAction == AlignerPreAction.None)
            {
                SetHome(APIEnum.CommnadDevice.Aligner, aligner.cv_Id);
                aligner.cv_Data.PPreAction = AlignerPreAction.WaitHome;
            }
            else if (aligner.cv_Data.PPreAction == AlignerPreAction.WaitHome)
            {
                //change to alignerHome by API reply.
            }
            else if (aligner.cv_Data.PPreAction == AlignerPreAction.AlignerHome)
            {
            }
            else if (aligner.cv_Data.PPreAction == AlignerPreAction.SetToAngle)
            {
                //wait aligner job to set. and change to Vucccrm off1.
            }
            else if (aligner.cv_Data.PPreAction == AlignerPreAction.VuccumOff1)
            {
                SetAlignerVaccum(false, aligner.cv_Id);
            }
            else if (aligner.cv_Data.PPreAction == AlignerPreAction.PutAligner)
            {
            }
            else if (aligner.cv_Data.PPreAction == AlignerPreAction.VuccumOn)
            {
                SetAlignerVaccum(false, aligner.cv_Id);
                aligner.cv_Data.PPreAction = AlignerPreAction.WaitVuccumOn;
            }
            else if (aligner.cv_Data.PPreAction == AlignerPreAction.WaitVuccumOn)
            {
            }
            else if (aligner.cv_Data.PPreAction == AlignerPreAction.FindNotch)
            {
                SetAlignerFindNotch(aligner.cv_Id);
                aligner.cv_Data.PPreAction = AlignerPreAction.WaitFindNotch;
            }
            else if (aligner.cv_Data.PPreAction == AlignerPreAction.WaitFindNotch)
            {
            }
            else if (aligner.cv_Data.PPreAction == AlignerPreAction.OcrConnect)
            {
                if (!aligner.cv_Data.POcrEnable)
                {
                    aligner.cv_Data.PPreAction = AlignerPreAction.ToAngle;
                }
                else
                {
                    SetOcrConnect(aligner.cv_Id);
                    aligner.cv_Data.PPreAction = AlignerPreAction.WaitConnect;
                }
            }
            else if (aligner.cv_Data.PPreAction == AlignerPreAction.WaitConnect)
            {
            }
            else if (aligner.cv_Data.PPreAction == AlignerPreAction.ReadOcr)
            {
                SetOcrRead(aligner.cv_Id);
                aligner.cv_Data.PPreAction = AlignerPreAction.WaitReadOct;
            }
            else if (aligner.cv_Data.PPreAction == AlignerPreAction.WaitReadOct)
            {
            }
            else if (aligner.cv_Data.PPreAction == AlignerPreAction.ToAngle)
            {
                SetAlignerToAngle(aligner.cv_Id);
                aligner.cv_Data.PPreAction = AlignerPreAction.WaitToAngle;
            }
            else if (aligner.cv_Data.PPreAction == AlignerPreAction.WaitToAngle)
            {
            }
            else if (aligner.cv_Data.PPreAction == AlignerPreAction.VuccumOff2)
            {
                SetAlignerVaccum(false, aligner.cv_Id);
                aligner.cv_Data.PPreAction = AlignerPreAction.WaitVuccomOff2;
            }
            else if (aligner.cv_Data.PPreAction == AlignerPreAction.WaitVuccomOff2)
            {
            }
            else if (aligner.cv_Data.PPreAction == AlignerPreAction.GetAligner)
            {
            }
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
        private void DoRightSideFindJob(out RobotJob m_Job , enSideGroup m_Side = enSideGroup.Left)
        {
            m_Job = null;
            if (!checkConditionForFindJob(m_Side)) return;
        }
        private void DoLeftSideJob(RobotJob m_Job , enSideGroup m_Side = enSideGroup.Left)
        {
            Robot robot = GetRobotBySide(m_Side);
            if(m_Job != null)
            {

            }
        }
        private void DoRightSideJob(enSideGroup m_Side = enSideGroup.Right)
        {
        }

        private bool GetPortUnloadSlot(enSideGroup m_Side , ProductCategory m_Type , out int m_Port , out int m_Slot) 
        {
            bool rtn = false;
            m_Port = 0;
            m_Slot = 0;
            for (int port_id = 0; port_id < cv_InProcessPort.Count; port_id++)
            {
                Port port = LgcModule.GetPortById(cv_InProcessPort[port_id]);
                if (port.PSideGroup == m_Side)
                {
                    if (port.cv_Data.PPortMode == PortMode.Both || port.cv_Data.PPortMode == PortMode.Loader)
                    {
                        if (port.PLotStatus == LotStatus.Process && port.PPortStatus == PortStaus.LDCM)
                        {
                            if (port.cv_Data.PProductionType == m_Type)
                            {
                                int slot = 0;
                                if (FindHightestSlotForPPID(port.cv_Data.PCurPPID, port.cv_Id, out slot))
                                {
                                    if (!port.cv_Data.HasDataAndSensor(slot)) continue;
                                    if (port.cv_Data.GlassDataMap[slot].PProcessFlag == ProcessFlag.Need)
                                    {
                                        if (port.cv_Data.GlassDataMap[slot].POcrResult == OCRResult.None)
                                        {
                                            if (!port.cv_Data.GlassDataMap[slot].IsEnterEq())
                                            {
                                                rtn = true;
                                                m_Port = port_id;
                                                m_Slot = slot;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    port.cv_Data.PCurPPID = FindHightestPriorityPPID(port.cv_Id);
                                }
                                if (rtn)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return rtn;
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

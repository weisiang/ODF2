using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonData.HIRATA;
using BaseAp;

namespace LGC
{
    public partial class LGCController
    {
        #region base recipe , alarm , system , timeout , glass count.
        protected override void ProcessRecipeChange(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessRecipeChange(m_SourceModule, m_MessageId, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        protected override void ProcessAlarmChange(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessAlarmChange(m_SourceModule, m_MessageId, m_Object);
            AlarmData obj = m_Object as AlarmData;
            for (int i = 0; i < obj.cv_AlarmList.Count; i++)
            {
                if (obj.cv_AlarmList[i].PStatus == AlarmStatus.Clean)
                {
                    lgcBase.cv_Alarms.DelAlarm(obj.cv_AlarmList[i]);
                }
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        protected override void ProcessSystemData(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessSystemData(m_SourceModule, m_MessageId, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        protected override void ProcessTimeoutData(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessTimeoutData(m_SourceModule, m_MessageId, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        protected override void ProcessGlassCountData(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessGlassCountData(m_SourceModule, m_MessageId, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        #endregion

        #region process robot , port , aligner , buffer , eq data
        protected override void ProcessPortData(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessPortData(m_SourceModule, m_MessageId, m_Object);
            try
            {
                WriteLog(CommonData.HIRATA.LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
                string log = "";
                if (m_SourceModule == FdModule.UI)
                {
                    log = "[Recv Port data form UI]\n";
                    CommonData.HIRATA.PortData obj = m_Object as CommonData.HIRATA.PortData;
                    CommonData.HIRATA.PortData tmp = LgcModule.cv_PortContainer[(int)obj.PId].cv_Data;
                    tmp.GlassDataMap = tmp.GlassDataMap;
                }
                else if (m_SourceModule == FdModule.CIM)
                {
                    log = "[Recv Port data form CIM]\n";
                    CommonData.HIRATA.PortData obj = m_Object as CommonData.HIRATA.PortData;
                    Port job_port = LgcModule.GetPortById((int)obj.PId);
                    if (job_port != null)
                    {
                        log += job_port.cv_Data.GetPortStatusStringForLogUse() + "\n";
                        if (job_port.PLotStatus == LotStatus.MappingEnd)
                        {
                            job_port.cv_Data.PWorkCount = obj.PWorkCount;
                            job_port.cv_Data.PFoupSeq = obj.PFoupSeq;
                            job_port.cv_Data.PLotId = obj.PLotId;
                            log += "recv PWorkCount : " + obj.PWorkCount + "\n";
                            log += "recv PFoupSeq : " + obj.PFoupSeq + "\n";
                            log += "recv PLotId : " + obj.PLotId + "\n";
                            Dictionary<int, GlassData> recv_glass = new Dictionary<int, GlassData>();
                            foreach (GlassData data in obj.cv_GlassDataList)
                            {
                                recv_glass[Convert.ToInt16(data.PSlotInEq)] = data;
                                log += "Recv Slot : " + data.PSlotInEq + "\n";
                            }

                            bool error = false;
                            int sum = 0;
                            for (int i = 1; i <= job_port.cv_SlotCount; i++)
                            {
                                if (job_port.cv_Data.GlassDataMap[i].PHasSensor)
                                {
                                    if (!recv_glass[i].PHasData)
                                    {
                                        LgcModule.ShowMsg("[Foup Data ERROR] : Slot " + i.ToString() + "hasn't data , but has sensor", false, false);
                                        log += "Data unmatch : Slot " + i.ToString() + "hasn't data , but has sensor\n";
                                        error = true;
                                        break;
                                    }
                                    else
                                    {
                                        recv_glass[i].POcrResult = OCRResult.None;
                                        recv_glass[i].PPortProductionCategory = job_port
                                            .cv_Data.PProductionType;
                                        recv_glass[i].PSourcePort = (uint)job_port.cv_Id;
                                        if (recv_glass[i].PProductionCategory != ProductCategory.Glass && recv_glass[i].PProductionCategory != ProductCategory.Wafer)
                                        {
                                            LgcModule.ShowMsg("[Foup Data ERROR] : BC data  but Production Category errro : " + recv_glass[i].PProductionCategory.ToString(), false, false);
                                            log += "BC data  but Production Category errro : Slot : " + i.ToString() + " ," + recv_glass[i].PProductionCategory.ToString() + "\n";
                                            error = true;
                                            break;
                                        }
                                        sum++;
                                    }
                                }
                                else
                                {
                                    if (recv_glass[i].PHasData)
                                    {
                                        LgcModule.ShowMsg("[Foup Data ERROR] : Slot " + i.ToString() + "hasn't sensor , but has data", false, false);
                                        log += "Slot " + i.ToString() + "hasn't sensor , but has data\n";
                                        error = true;
                                        break;
                                    }
                                }
                            }

                            if (!error)
                            {
                                lgcBase.cv_GlassCountData.PHistoryCount += sum;
                            }
                            else
                            {
                                AlarmItem alarm = new AlarmItem();
                                alarm.PCode = Alarmtable.BcFoupDataError.ToString();
                                alarm.PLevel = AlarmLevele.Light;
                                alarm.PMainDescription = "Bc Foup Data Error";
                                alarm.PStatus = AlarmStatus.Occur;
                                LgcModule.EditAlarm(alarm);
                                job_port.cv_Data.cv_IsWaitCancel = true;
                                return;
                            }
                            //check every slot recipe is the same for node 2.
                            int tmp_recipe = -1;
                            for (int i = 1; i <= job_port.cv_Data.cv_SlotCount; i++)
                            {
                                if (job_port.cv_Data.GlassDataMap[i].PHasSensor && job_port.cv_Data.GlassDataMap[i].PProcessFlag == ProcessFlag.Need)
                                {
                                    int node = job_port.cv_Data.GlassDataMap[i].cv_Nods.FindIndex(x => x.cv_NodeId == 2);
                                    GlassDataNodeItem item = job_port.cv_Data.GlassDataMap[i].cv_Nods[node];
                                    if (tmp_recipe == -1)
                                    {
                                        tmp_recipe = item.cv_Recipe;
                                    }
                                    else
                                    {
                                        if (tmp_recipe != item.cv_Recipe)
                                        {
                                            log += "Slot " + i.ToString() + ". Recipe No. not the same\n";
                                            error = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (error)
                            {
                                AlarmItem alarm = new AlarmItem();
                                alarm.PCode = Alarmtable.FoupDataContainsOverOneRecipe.ToString();
                                alarm.PLevel = AlarmLevele.Light;
                                alarm.PMainDescription = "Foup Data Contains Over One Recipe";
                                alarm.PStatus = AlarmStatus.Occur;
                                LgcModule.EditAlarm(alarm);
                                LgcModule.ShowMsg(alarm.PMainDescription, false, false);
                                job_port.cv_Data.cv_IsWaitCancel = true;
                                return;
                            }

                            bool need_change_recipe = false;
                            int want_change_recipe = -1;
                            log += "change recipe : " + tmp_recipe + " Cur recipe : " + lgcBase.cv_Recipes.PCurRecipeId + "\n";
                            if (tmp_recipe != Convert.ToInt32(lgcBase.cv_Recipes.PCurRecipeId))
                            {
                                need_change_recipe = true;
                                want_change_recipe = tmp_recipe;
                                bool can_change_recipe = true;
                                Robot rb = LgcModule.GetRobotById(1);
                                Robot rb2 = LgcModule.GetRobotById(2);
                                Aligner aligner = LgcModule.GetAlignerById(1);
                                Aligner aligner2 = LgcModule.GetAlignerById(2);
                                Buffer buffer = LgcModule.GetBufferById(1);
                                Buffer buffer2 = LgcModule.GetBufferById(2);
                                for (int j = 1; j <= CommonData.HIRATA.CommonStaticData.g_PortNumber; j++)
                                {
                                    if (j == job_port.cv_Data.PId) continue;
                                    Port port = LgcModule.GetPortById(j);
                                    log += "port : " + port.cv_Data.PId + " status : " + port.PPortStatus + " foup status : " + port.PLotStatus + "\n";
                                    if (port.PPortStatus != PortStaus.LDRQ && port.PPortStatus != PortStaus.UDCM && port.PPortStatus != PortStaus.UDRQ &&
                                        port.cv_Data.PPortMode != PortMode.Unloader)
                                    {
                                        if (port.IsHasAnyDataAndSensor())
                                        {
                                            can_change_recipe = false;
                                            string tmp3 = "Port : " + port.cv_Data.PId + " has data or sensor set can_change_recipe false\n";
                                            LgcModule.ShowMsg(tmp3, true, false);
                                            log += tmp3;
                                            break;
                                        }
                                    }
                                }
                                if (can_change_recipe)
                                {
                                    if (rb.IsBusy || aligner.IsHasAnyDataAndSensor() || buffer.IsHasAnyDataAndSensor() || rb.IsHasAnyDataAndSensor() ||
                                        rb2.IsBusy || aligner2.IsHasAnyDataAndSensor() || buffer2.IsHasAnyDataAndSensor() || rb2.IsHasAnyDataAndSensor()
                                        )
                                    {
                                        can_change_recipe = false;
                                        string tmp4 = "Robot busy : " + rb.IsBusy + "\n";
                                        tmp4 += "Robot busy : " + rb.IsBusy + "\n";
                                        tmp4 += "aligner IsHasAnyDataAndSensor : " + aligner.IsHasAnyDataAndSensor() + "\n";
                                        tmp4 += "buffer IsHasAnyDataAndSensor : " + buffer.IsHasAnyDataAndSensor() + "\n";
                                        tmp4 += "rb IsHasAnyDataAndSensor : " + rb.IsHasAnyDataAndSensor() + "\n";
                                        tmp4 += "set can_change_recipe false\n";
                                        LgcModule.ShowMsg(tmp4, true, false);
                                        log += tmp4;
                                    }
                                }
                                if (!can_change_recipe)
                                {
                                    AlarmItem alarm = new AlarmItem();
                                    alarm.PCode = Alarmtable.BcDataDownLoadRecipeERROR.ToString();
                                    alarm.PLevel = AlarmLevele.Light;
                                    alarm.PMainDescription = "Bc DataDownLoad Recipe ERROR";
                                    alarm.PSubDescription = "Main S/W has substrate data or robot is busy.";
                                    alarm.PStatus = AlarmStatus.Occur;
                                    LgcModule.EditAlarm(alarm);
                                    LgcModule.ShowMsg("Data download : Recipe unmatch : Cur is " + lgcBase.cv_Recipes.PCurRecipeId.ToString() +
                                        " Recv : " + tmp_recipe.ToString(), true, false);
                                    job_port.cv_Data.cv_IsWaitCancel = true;
                                    error = true;
                                }
                            }
                            if (!error)
                            {
                                if (need_change_recipe)
                                {
                                    if (lgcBase.cv_Recipes.IsRecipeExist(want_change_recipe.ToString()))
                                    {
                                        lgcBase.cv_Recipes.SetCurRecipe(want_change_recipe.ToString());
                                        job_port.cv_Data.PCurPPID = LgcModule.FindHightestPriorityPPID(job_port.cv_Id);
                                        job_port.PLotStatus = LotStatus.WaitReserve;
                                        log += "Set Port : " + job_port.cv_Id + " WaitReserve\n";
                                    }
                                    else
                                    {
                                        AlarmItem alarm = new AlarmItem();
                                        alarm.PCode = Alarmtable.FoupDataRecipeEFEMNotHas.ToString();
                                        alarm.PLevel = AlarmLevele.Light;
                                        alarm.PMainDescription = "Foup Data Recipe EFEM Not Has";
                                        alarm.PStatus = AlarmStatus.Occur;
                                        LgcModule.EditAlarm(alarm);
                                        LgcModule.ShowMsg("Data download : Recipe unmatch : Cur is " + lgcBase.cv_Recipes.PCurRecipeId.ToString(), false, false);
                                        job_port.cv_Data.cv_IsWaitCancel = true;
                                        error = true;
                                    }
                                }
                                else
                                {
                                    job_port.cv_Data.PCurPPID = LgcModule.FindHightestPriorityPPID(job_port.cv_Id);
                                    job_port.PLotStatus = LotStatus.WaitReserve;
                                    log += "Set Port : " + job_port.cv_Id + " WaitReserve\n";
                                }
                            }
                        }
                        else
                        {
                            LgcModule.ShowMsg("Recv BC Foup Data Download , But Port Status not in Mapping End", true, false);
                            log += "Recv BC Foup Data Download , But Port Status not in Mapping End\n";
                        }
                    }
                }
                WriteLog(LogLevelType.Detail, log);
                WriteLog(CommonData.HIRATA.LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            }
            catch (Exception e)
            {
                WriteLog(LogLevelType.Error, e.StackTrace.ToString());
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessRobotData(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessRobotData(m_SourceModule, m_MessageId, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessBufferData(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessBufferData(m_SourceModule, m_MessageId, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessAlignerData(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessAlignerData(m_SourceModule, m_MessageId, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessEqData(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessEqData(m_SourceModule, m_MessageId, m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        #endregion



        #region rewrite base demand functions.
        protected override void ProcessOnlineReq(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessOnlineReq(m_SourceModule , m_MessageId ,m_Object);
            string log = "[Recv Online Req]\n";
            CommonData.HIRATA.MDOnlineRequest obj = m_Object as CommonData.HIRATA.MDOnlineRequest;
            log += "obj content : Cur Mode :" + obj.PCurMode.ToString() + " Change Mode : " + obj.PChangeMode.ToString() + Environment.NewLine;
            if (m_SourceModule == FdModule.UI)
            {
                if (obj.PChangeMode == OnlineMode.Offline)
                {
                    lgcBase.PSystemData.PSystemOnlineMode = obj.PChangeMode;
                    log += "Change online mode successful";
                }
                else if (obj.PChangeMode == OnlineMode.Control)
                {
                    if (lgcBase.PSystemData.PPlcConnect && lgcBase.PSystemData.PBcAlive)
                    {
                        lgcBase.PSystemData.PSystemOnlineMode = obj.PChangeMode;
                        log += "Change online mode successful";
                    }
                    else
                    {
                        SendMsgToUi(true, false, 10000, "Can't change CIM mode , please check BC status / PLC connection ");
                        log += "Change online mode fail , check BC/PLC status";
                    }
                }
                else
                {
                    SendMsgToUi(true, false, 10000, "Can't change CIM mode : " + obj.PChangeMode.ToString());
                    log += "Change online mode fail , check UI' msg content : " + obj.PChangeMode.ToString();
                }
            }
            WriteLog(LogLevelType.General, log);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessInitialize(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessInitialize(m_SourceModule , m_MessageId, m_Object);
            CommonData.HIRATA.MDInitial obj = m_Object as CommonData.HIRATA.MDInitial;
            if (lgcBase.PSystemData.PapiConnect && lgcBase.PSystemData.PapiInlineMode != EquipmentInlineMode.None)
            {
                if(obj.PSide == enSideGroup.Left)
                {
                    LgcModule.GetRobotBySide(obj.PSide).SetInitilize(obj.PSide, obj.cv_IsForce);
                    LgcModule.GetAlignerBySide(obj.PSide).cv_Data.PPreAction = AlignerPreAction.None;
                }
                else if(obj.PSide == enSideGroup.Right)
                {
                    LgcModule.GetRobotBySide(obj.PSide).SetInitilize(obj.PSide, obj.cv_IsForce);
                    LgcModule.GetAlignerBySide(obj.PSide).cv_Data.PPreAction = AlignerPreAction.None;
                }
                else if(obj.PSide == enSideGroup.Both)
                {
                    LgcModule.GetRobotBySide(obj.PSide).SetInitilize(obj.PSide, obj.cv_IsForce);
                    LgcModule.GetAlignerBySide(obj.PSide).cv_Data.PPreAction = AlignerPreAction.None;
                    LgcModule.GetRobotBySide(obj.PSide).SetInitilize(obj.PSide, obj.cv_IsForce);
                    LgcModule.GetAlignerBySide(obj.PSide).cv_Data.PPreAction = AlignerPreAction.None;
                }
                //LgcModule.GetRobotById(1).SetInitilize(enSideGroup.Left, obj.cv_IsForce);
                //LgcModule.GetAlignerById(1).cv_Data.PPreAction = AlignerPreAction.None;
                for (int i = (int)EqGifTimeChartId.TIMECHART_ID_SDP1; i <= (int)EqGifTimeChartId.TIMECHART_ID_VAS2_DOWN; i++)
                {
                    Eq eq = LgcModule.GetEqById(i);
                    if( (obj.PSide == eq.PSideGroup) || obj.PSide == enSideGroup.Both )
                    {
                        cv_TimeChart.RestartTimeChart(i);
                    }
                }
            }
            else
            {
                LgcModule.ShowMsg("Initialize failure , please check Robot connect / Robot current mode.", true, false);
            }
            SendTimeChartStepMsg();
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessOntMode(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CommonData.HIRATA.MDOntMode obj = m_Object as CommonData.HIRATA.MDOntMode;
            bool is_on = obj.PIsOn;
            lgcBase.PSystemData.PONT = is_on;
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override void ProcessSetTimeOut(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessSetTimeOut(m_SourceModule , m_MessageId , m_Object);
            CommonData.HIRATA.MDSetTimeOut obj = m_Object as CommonData.HIRATA.MDSetTimeOut;

            lgcBase.cv_TimeoutData.cv_IdleDelayTime = obj.PIdleTIme;
            lgcBase.cv_TimeoutData.cv_IntervalTime = obj.PIntervalTIme;
            lgcBase.cv_TimeoutData.cv_T0Time = obj.PT0TIme;
            lgcBase.cv_TimeoutData.cv_T1Time = obj.PT1TIme;
            lgcBase.cv_TimeoutData.cv_T3Time = obj.PT3TIme;
            lgcBase.cv_TimeoutData.cv_TsTime = obj.PTsTIme;
            lgcBase.cv_TimeoutData.cv_TeTime = obj.PTeTIme;
            lgcBase.cv_TimeoutData.cv_ApiT3TIme = obj.PApiT3TIme;
            lgcBase.cv_TimeoutData.cv_TmTime = obj.PTmTIme;
            lgcBase.cv_TimeoutData.cv_TmTime = obj.PTmTIme;
            lgcBase.cv_TimeoutData.SaveToFile();
            SetTimeChartTimeOut();

            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        protected override  void ProcessRecipeAction(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            base.ProcessRecipeAction(m_SourceModule , m_MessageId ,  m_Object);
            string log = "";
            CommonData.HIRATA.MDRecipeAction obj = m_Object as CommonData.HIRATA.MDRecipeAction;
            if (obj.PAction == CommonData.HIRATA.DataEidtAction.Add)
            {
                foreach (CommonData.HIRATA.RecipeItem recipe in obj.Recipes)
                {
                    if (lgcBase.cv_Recipes.AddRecipe(recipe))
                    {
                        /*
                        CommonData.HIRATA.MDBCRecipeBodyReport report = new MDBCRecipeBodyReport();
                        report.PAction = RecipeBodyReportType.New;
                        report.PUnit = 0;
                        report.PRecipe = recipe;
                        SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCRecipeBodyReport).Name, report,
                            KParseObjToXmlPropertyType.Field);
                        */
                    }
                }
            }
            else if (obj.PAction == CommonData.HIRATA.DataEidtAction.Del)
            {
                foreach (CommonData.HIRATA.RecipeItem recipe in obj.Recipes)
                {
                    if (lgcBase.cv_Recipes.DelRecipe(recipe))
                    {
                        /*
                        CommonData.HIRATA.MDBCRecipeBodyReport report = new MDBCRecipeBodyReport();
                        report.PAction = RecipeBodyReportType.Delete;
                        report.PUnit = 0;
                        report.PRecipe = recipe;
                        SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCRecipeBodyReport).Name, report,
                            KParseObjToXmlPropertyType.Field);
                        */
                    }
                }
            }
            else if (obj.PAction == CommonData.HIRATA.DataEidtAction.Edit)
            {
                foreach (CommonData.HIRATA.RecipeItem recipe in obj.Recipes)
                {
                    if (lgcBase.cv_Recipes.ModifyRecipe(recipe))
                    {
                        /*
                        CommonData.HIRATA.MDBCRecipeBodyReport report = new MDBCRecipeBodyReport();
                        report.PAction = RecipeBodyReportType.Modity;
                        report.PUnit = 0;
                        report.PRecipe = recipe;
                        SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCRecipeBodyReport).Name, report,
                            KParseObjToXmlPropertyType.Field);
                        */
                    }
                }
            }
            else if (obj.PAction == CommonData.HIRATA.DataEidtAction.SetCur)
            {
                foreach (CommonData.HIRATA.RecipeItem recipe in obj.Recipes)
                {
                    if (lgcBase.cv_Recipes.IsRecipeExist(recipe.PId))
                    {
                        lgcBase.cv_Recipes.SetCurRecipe(recipe.PId);
                    }
                    /*
                    if (LgcForm.cv_Recipes.ModifyRecipe(recipe))
                    {
                        CommonData.HIRATA.MDBCRecipeBodyReport report = new MDBCRecipeBodyReport();
                        report.PAction = RecipeBodyReportType.Modity;
                        report.PUnit = 0;
                        report.PRecipe = recipe;
                        SendMmfNotifyObject(typeof(CommonData.HIRATA.MDBCRecipeBodyReport).Name, report,
                            KParseObjToXmlPropertyType.Field);
                    }
                    */
                }
            }
            WriteLog(LogLevelType.General, log);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        #endregion


    }
}

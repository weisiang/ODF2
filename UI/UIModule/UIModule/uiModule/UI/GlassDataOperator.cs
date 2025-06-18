using System;
using KgsCommon;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BaseAp;
using CommonData.HIRATA;

namespace UI
{
    public partial class UiGlassDataOperator : Form
    {
        RequestData cv_RequestForm = new RequestData();
        CommonData.HIRATA.Source cv_Source = null;
        class History
        {
            public DateTime cv_Time = DateTime.Now;
            public  GlassData cv_Glass = null;
            public History(DateTime m_Time , GlassData m_Glass)
            {
                cv_Time = m_Time;
                cv_Glass = m_Glass;
            }
        }

        public class RecoveryData
        {
            /*
            public GBOtherPosition cv_Other = GBOtherPosition.None;
            public int cv_CarrierId = 0;
            public GlassData.GlassData cv_Glass = null;
            public RecoveryData(GBOtherPosition m_Other, GlassData.GlassData m_Glass)
            {
                cv_Other = m_Other;
                cv_Glass = m_Glass;
            }
            public RecoveryData(int m_CarrierId, GlassData.GlassData m_Glass)
            {
                cv_CarrierId = m_CarrierId;
                cv_Glass = m_Glass;
            }
            */
        }

        public static List<RecoveryData> cv_RecoveryList = new List<RecoveryData>();

        List<GlassData> cv_RemoveList = new List<GlassData>();

        List<History> cv_ReworkList = new List<History>();

        #region remove and rework histoty path and xml
        private string cv_RemoveFilePath = System.IO.Directory.GetCurrentDirectory() + "\\..\\Config\\RemoveHistory.xml";
        private string cv_ReworkFilePath = System.IO.Directory.GetCurrentDirectory() + "\\..\\Config\\ReworkHistory.xml";
        private KXmlItem cv_RemoveXml = null;
        private KXmlItem cv_ReworkXml = null;
        #endregion

        GlassData cv_CurData;
        private int cv_NoCount = 0;
        private Dictionary<int, SlotCondition> cv_SlotConditionMap = new Dictionary<int, SlotCondition>();
        private CommonData.HIRATA.ActionTarget cv_JobTaget;
        private int cv_JobId;
        private int cv_SlotNo;
        public UiGlassDataOperator(int m_NoCount)
        {
            InitializeComponent();
            cv_NoCount = m_NoCount;
            /*
            IniRemoeReworkXml();
            RecoveryRemove();
            RecoveryRework();
            */
            RecoveryRemove();
            for (int i = cv_NoCount; i > 0; i--)
            {
                AddNoContion(i);
            }
            SetCombox();
            UiForm.AddUiObjToEnableList(btn_Add, UiForm.enumGroup.Group3);
            UiForm.AddUiObjToEnableList(btn_modify, UiForm.enumGroup.Group3);
            UiForm.AddUiObjToEnableList(btn_remove, UiForm.enumGroup.Group3);
            UiForm.AddUiObjToEnableList(btn_Request, UiForm.enumGroup.Group3);
        }
        private void SetCombox()
        {
            cv_CbCimMode.Items.Clear();
            cv_CbCimMode.Items.Add("ON");
            cv_CbCimMode.Items.Add("OFF");
            cb_WorkType.Items.Clear();
            cb_WorkType.Items.AddRange(Enum.GetNames(typeof(CommonData.HIRATA.WorkType)));
            cb_Production.Items.Clear();
            cb_Production.Items.AddRange(Enum.GetNames(typeof(CommonData.HIRATA.ProductCategory)));
            cv_CmGlassjudge.Items.Clear();
            cv_CmGlassjudge.Items.AddRange(Enum.GetNames(typeof(CommonData.HIRATA.GlassJudge)));
            cv_CbProcessFlag.Items.Clear();
            cv_CbProcessFlag.Items.AddRange(Enum.GetNames(typeof(CommonData.HIRATA.ProcessFlag)));

            cb_Priority.Items.Clear();
            for (int i = 1; i < 10; i++)
            {
                cb_Priority.Items.Add(i.ToString());
            }

            cb_OcrReadFlag.Items.Clear();
            cb_OcrReadFlag.Items.AddRange(Enum.GetNames(typeof(CommonData.HIRATA.OCRRead)));

            cb_OcrResult.Items.Clear();
            cb_OcrResult.Items.AddRange(Enum.GetNames(typeof(CommonData.HIRATA.OCRResult)));
            cb_AssambleFlag.Items.Clear();
            cb_AssambleFlag.Items.AddRange(Enum.GetNames(typeof(CommonData.HIRATA.AssambleNeed)));
            cb_AssambleResult.Items.Clear();
            cb_AssambleResult.Items.AddRange(Enum.GetNames(typeof(CommonData.HIRATA.AssambleResult)));
        }
        private void AddNoContion(int m_No)
        {
            SlotCondition tmp = new SlotCondition(m_No);
            tmp.Dock = DockStyle.Top;
            pan_No.Controls.Add(tmp);
            cv_SlotConditionMap[m_No] = tmp;
        }
        public void Register(GlassData m_Data)
        {
            ClearGlassDataForm();
            UpdateNormalGlassData(m_Data);
        }
        public void Register(CommonData.HIRATA.ActionTarget m_Taget, int m_Id, int m_Slot)
        {
            cv_JobTaget = m_Taget;
            cv_JobId = m_Id;
            cv_SlotNo = m_Slot;
            ClearGlassDataForm();
            CommonData.HIRATA.GlassData glass_data = null;
            switch (m_Taget)
            {
                case CommonData.HIRATA.ActionTarget.Port:
                    glass_data = UiForm.GetPort(m_Id).cv_Data.GlassDataMap[m_Slot];
                    cv_CurData = glass_data;
                    break;
                case CommonData.HIRATA.ActionTarget.Eq:
                    glass_data = UiForm.GetEq(m_Id).cv_Data.GlassDataMap[m_Slot];
                    cv_CurData = glass_data;
                    break;
                case CommonData.HIRATA.ActionTarget.Buffer:
                    glass_data = UiForm.GetBuffer(m_Id).cv_Data.GlassDataMap[m_Slot];
                    cv_CurData = glass_data;
                    break;
                case CommonData.HIRATA.ActionTarget.Aligner:
                    glass_data = UiForm.GetAligner(m_Id).cv_Data.GlassDataMap[m_Slot];
                    cv_CurData = glass_data;
                    break;
                case CommonData.HIRATA.ActionTarget.Robot:
                    glass_data = UiForm.GetRobot(m_Id).cv_Data.GlassDataMap[m_Slot];
                    cv_CurData = glass_data;
                    break;
            };
            UpdateNormalGlassData(glass_data);
            CommonData.HIRATA.Source tmp = new CommonData.HIRATA.Source();
            tmp.PTarget = m_Taget;
            tmp.Slot = m_Slot;
            tmp.Id = m_Id;
            tmp.PArm = (CommonData.HIRATA.RobotArm)m_Slot;
            cv_Source = tmp;
        }

        private void RecoveryRemove()
        {
            if (!string.IsNullOrEmpty(cv_RemoveFilePath))
            {
                KXmlItem recipe_xml = new KXmlItem();
                recipe_xml.LoadFromFile(cv_RemoveFilePath);
                if (recipe_xml.ItemsByName["Data"].ItemType == KXmlItemType.itxList && recipe_xml.ItemsByName["Data"].ItemNumber != 0)
                {
                    EventCenterBase.ParseXmlToObject(cv_RemoveList, recipe_xml.ItemsByName["XML_LIST"]);
                }
            }

            for (int i = cv_RemoveList.Count - 1 ; i >=0; i--)
            {
                try
                {
                    GlassData tmp = cv_RemoveList[i];
                    cv_DataGridRemove.Rows.Insert(0, new object[] { tmp.PId });
                }
                catch(Exception e)
                {
                    continue;
                }
            }
        }

        private void RecoveryRework()
        {
            if (cv_ReworkXml == null) return;
            if (cv_ReworkXml.ItemsByName["GlassList"].ItemNumber == 1) return;
            if (cv_ReworkXml.ItemsByName["GlassList"].ItemNumber > 51)
            {
                int want_to_drop_count = cv_ReworkXml.ItemsByName["GlassList"].ItemNumber - 51;
                int last_index = 0;
                for (int i = 0; i <= want_to_drop_count; i++)
                {
                    last_index = cv_ReworkXml.ItemsByName["GlassList"].ItemNumber - 1;
                    cv_ReworkXml.ItemsByName["GlassList"].DeleteItem(last_index);
                }
                //cv_ReworkXml.SaveToFile(cv_RemoveFilePath, true);
                cv_ReworkXml.SaveToFile(cv_ReworkFilePath, true);
            }
            /*
            for (int i = cv_ReworkXml.ItemsByName["GlassList"].ItemNumber - 1; i >= 0;  i--)
            {
                try
                {
                    GlassData.GlassData tmp = new GlassData.GlassData();
                    KXmlItem m_Xml = cv_ReworkXml.ItemsByName["GlassList"].Items[i];
                    DateTime m_Time = DateTime.ParseExact(m_Xml.Attributes["RecordTime"].Trim(), "yyyyMMddhhmmss", null);
                    string sTime = m_Time.ToString("MM/dd hh:mm:ss");
                    for (int j = 1; j <= 64; j++)
                    {
                        tmp.cv_OriginalData[j] = SysUtils.StrToInt(m_Xml.ItemsByName["Dic"].ItemsByName["Word" + j.ToString()].AsString.Trim());
                    }
                    tmp.Dismantle();
                    tmp.PPID = m_Xml.ItemsByName["String"].Attributes["PPID"].Trim();
                    tmp.GlassId = m_Xml.ItemsByName["String"].Attributes["GlassID"].Trim();
                    tmp.GlassGrade = m_Xml.ItemsByName["String"].Attributes["GlassGrade"].Trim();
                    tmp.SortGrade = m_Xml.ItemsByName["String"].Attributes["SortGrade"].Trim();
                    tmp.TarCassetteId = m_Xml.ItemsByName["String"].Attributes["TarCasId"].Trim();

                    cv_DataGridRework.Rows.Insert(0, new object[] { sTime, tmp.GlassId });
                    History tmp_history = new History(m_Time, tmp);
                    cv_ReworkList.Insert(0, tmp_history);
                }
                catch(Exception e)
                {
                    LogicKernel.EventHandler.WriteErrorLog(e.ToString());
                    continue;
                }
            }
            */
        }

        private void IniRemoeReworkXml()
        {
            if (!string.IsNullOrEmpty(cv_RemoveFilePath))
            {
                if (SysUtils.FileExists(cv_RemoveFilePath))
                {
                    if (cv_RemoveXml == null)
                    {
                        cv_RemoveXml = new KXmlItem();
                        cv_RemoveXml.LoadFromFile(cv_RemoveFilePath);
                    }
                }
            }

            if (!string.IsNullOrEmpty(cv_ReworkFilePath))
            {
                if (SysUtils.FileExists(cv_ReworkFilePath))
                {
                    if (cv_ReworkXml == null)
                    {
                        cv_ReworkXml = new KXmlItem();
                        cv_ReworkXml.LoadFromFile(cv_ReworkFilePath);
                    }
                }
            }
        }

        private void UiGlassDataOperator_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }


        #region update UI
        private void ClearGlassDataForm()
        {
            cv_TxGlassId.Text = "";
            cv_txFoupSeq.Text = "";
            txt_WorkSlot.Text = "";
            cb_Production.Text = "";
            cv_CbProcessFlag.Text = "";
            cb_OcrReadFlag.Text = "";
            cb_AssambleFlag.Text = "";
            cv_CbCimMode.Text = "";
            txt_WorkOrder.Text = "";
            cb_WorkType.Text = "";
            cv_CmGlassjudge.Text = "";
            cb_Priority.Text = "";
            cb_OcrResult.Text = "";
            cb_AssambleResult.Text = "";
            txt_PPID.Text = "";
            foreach (SlotCondition obj in cv_SlotConditionMap.Values)
            {
                obj.Clear();
            }
        }
        private void CleanData()
        {
            cv_TxGlassId.Text = "";;
            cv_txFoupSeq.Text = "";//tmp_glass.PFoupSeq.ToString();
            txt_WorkSlot.Text = "";//tmp_glass.PWorkSlot.ToString();
            cb_Production.Text = "";//tmp_glass.PProductionCategory.ToString();
            cv_CbProcessFlag.Text = "";//tmp_glass.PProcessFlag.ToString();
            cb_OcrReadFlag.Text = "";//tmp_glass.POcrRead.ToString();
            cb_AssambleFlag.Text = "";//tmp_glass.PAssamblyFlag.ToString();
            cv_CbCimMode.Text = "";//"OFF";
                /*
            if (tmp_glass.PCimMode == CommonData.HIRATA.OnlineMode.Offline)
                cv_CbCimMode.Text = "OFF";
            else
                cv_CbCimMode.Text = "ON";
                */
            txt_WorkOrder.Text = "";//tmp_glass.PWorkOrderNo.ToString();
            cb_WorkType.Text = "";//tmp_glass.PWorkType.ToString();
            cv_CmGlassjudge.Text = "";//tmp_glass.PGlassJudge.ToString();
            cb_Priority.Text = "";//tmp_glass.PPriority.ToString();
            cb_OcrResult.Text = "";//tmp_glass.POcrResult.ToString();
            cb_AssambleResult.Text = "";//tmp_glass.PAssamblyResult.ToString();
            txt_PPID.Text = "";//tmp_glass.PPID.Trim();
            for(int i=1 ; i<=cv_SlotConditionMap.Count ; i++)
            {
                cv_SlotConditionMap[i].SetContext(0,0,false);
            }
            /*
            foreach (CommonData.HIRATA.GlassDataNodeItem obj in tmp_glass.cv_Nods)
            {
                cv_SlotConditionMap[obj.cv_NodeId].SetContext(obj.PProcessHistory, obj.PRecipe, obj.PProcessAbnormal);
            }
            */
        }
        private void UpdateNormalGlassData(CommonData.HIRATA.GlassData tmp_glass, bool m_IsUseHistory = false)
        {
            cv_TxGlassId.Text = tmp_glass.PId;
            cv_txFoupSeq.Text = tmp_glass.PFoupSeq.ToString();
            txt_WorkSlot.Text = tmp_glass.PWorkSlot.ToString();
            cb_Production.Text = tmp_glass.PProductionCategory.ToString();
            cv_CbProcessFlag.Text = tmp_glass.PProcessFlag.ToString();
            cb_OcrReadFlag.Text = tmp_glass.POcrRead.ToString();
            cb_AssambleFlag.Text = tmp_glass.PAssamblyFlag.ToString();
            if (tmp_glass.PCimMode == CommonData.HIRATA.OnlineMode.Offline)
                cv_CbCimMode.Text = "OFF";
            else
                cv_CbCimMode.Text = "ON";
            txt_WorkOrder.Text = tmp_glass.PWorkOrderNo.ToString();
            cb_WorkType.Text = tmp_glass.PWorkType.ToString();
            cv_CmGlassjudge.Text = tmp_glass.PGlassJudge.ToString();
            cb_Priority.Text = tmp_glass.PPriority.ToString();
            cb_OcrResult.Text = tmp_glass.POcrResult.ToString();
            cb_AssambleResult.Text = tmp_glass.PAssamblyResult.ToString();
            txt_PPID.Text = tmp_glass.PPID.Trim();
            foreach (CommonData.HIRATA.GlassDataNodeItem obj in tmp_glass.cv_Nods)
            {
                cv_SlotConditionMap[obj.cv_NodeId].SetContext(obj.PProcessHistory, obj.PRecipe, obj.PProcessAbnormal);
            }
        }
        #endregion

        #region Add
        private void button1_Click(object sender, EventArgs e)
        {
            /*
            if (CPC.Perssion != CPC.SystemPerssion.Root)
            {
                Common.PopForm("Please Log in with Root");
                return;
            }
            if(!CheckGBInStopOrDown())
            {
                return;
            }

            int combox_index = cv_CbPosition.SelectedIndex;
            GBPLC.Position tmp_gb_position = Position.None;
            GBOtherPosition tmp_other_position = GBOtherPosition.None;
            if (combox_index < 4)
            {
                tmp_other_position = (GBOtherPosition)combox_index;
            }
            else
            {
                tmp_gb_position = (Position)(combox_index - 4);
            }
            string user_log = "[ User push GlassDataOperator ADD button ] \n";
            string m_glassdata = "";
            if (tmp_gb_position == Position.None)
            {             
                AddGlassInOtherPosition(tmp_other_position, out m_glassdata);
                user_log += "ADD GlassData Successfull !! \n";
                user_log += m_glassdata + "\n";
            }
            else
            {
                AddGlassInNormalPosition(tmp_gb_position, out m_glassdata);
                user_log += "ADD GlassData Successfull !! \n";
                user_log += m_glassdata + "\n";
            }
            if (!string.IsNullOrEmpty(user_log))
            {
                user_log += Common.cv_SplitSymbol;
                LogicKernel.EventHandler.WriteUserLog(user_log);
            }
        */
        }

        private CommonData.HIRATA.GlassData AddGlassData()
        {
            CommonData.HIRATA.GlassData add_glass = new CommonData.HIRATA.GlassData();
            /*
                add_glass.CarrierId = SysUtils.StrToInt(cv_carrierId.Text.Trim());
                add_glass.CarrierCount = SysUtils.StrToInt(cv_TxcarrierCount.Text.Trim());
                add_glass.CarrierDirty = SysUtils.StrToInt(cv_TxcarrierDirty.Text.Trim());
                add_glass.GlassId = cv_TxGlassId.Text.Trim();
                add_glass.CassetteSequence = SysUtils.StrToInt(cv_txCasQue.Text.Trim());
                add_glass.PPID = cv_TxPPID.Text.Trim();
                add_glass.GroupNo = SysUtils.StrToInt(cv_TxGroupNo.Text.Trim());
                add_glass.GlassGrade = cv_TxGlassGrade.Text.Trim();
                add_glass.SrcPortNo = SysUtils.StrToInt(cv_CbSrcPortNo.Text.Trim());
                add_glass.SrcSlotNo = SysUtils.StrToInt(cv_CbSrcSlotNo.Text.ToString().Trim());
                add_glass.CimModeCreate = (GlassData.CimModeCreate)cv_CbCimMode.SelectedIndex;
                add_glass.GlassType = (GlassData.GlassType)cv_CbGlassType.SelectedIndex;
                add_glass.ProcessSkipFlag = (GlassData.ProcessSkipFlag)cv_CbProcessFlag.SelectedIndex;
                add_glass.LastGlassFlag = (GlassData.LastGlassFlag)cv_CbLastFlag.SelectedIndex;
                add_glass.Glassjudge = (GlassData.GlassJudge)cv_CmGlassjudge.SelectedIndex;
                add_glass.InspectionJudgeResult = (GlassData.InspectionJudgeResult)cv_CbInspectionJudge.SelectedIndex;
                add_glass.EqSpecialFlag = (GlassData.EqSpecialFlag)cv_CbSpecialFlag.SelectedIndex;
                add_glass.NgMark = (GlassData.NgMark)cv_CbNGMark.SelectedIndex;
                add_glass.TarPortNo = SysUtils.StrToInt(cv_CbTarPort.Text.Trim());
                add_glass.TarSlotNo = SysUtils.StrToInt(cv_CbTarSlot.Text.Trim());
                add_glass.TarCassetteId = cv_TxTarCasId.Text.Trim();
                add_glass.SortGrade = cv_TxSortGrade.Text.Trim();
                add_glass.InsepctionReservaion = SysUtils.StrToInt(cv_TxInspectionReservation.Text.Trim());
                add_glass.ProcessReservation = SysUtils.StrToInt(cv_TxProcessReservation.Text.Trim());
                add_glass.TrackingHistory = cv_CbTracking.SelectedIndex == 0 ? false : true;
                add_glass.NAR1TrackingHistory = cv_CbTrackingNar1.SelectedIndex == 0 ? false : true;
                add_glass.NAR2TrackingHistory = cv_CbTrackingNar2.SelectedIndex == 0 ? false : true;
            */
            return add_glass;
        }
        private CommonData.HIRATA.GlassData ModifyGlassData(CommonData.HIRATA.GlassData m_GlassData)
        {
            CommonData.HIRATA.GlassData add_glass = new CommonData.HIRATA.GlassData();
            return add_glass;
            /*
                string modify_log = "";
                modify_log += "[ CarrierId ] : " + m_GlassData.CarrierId.ToString() + " --> " ; 
                m_GlassData.CarrierId = SysUtils.StrToInt(cv_carrierId.Text.Trim());
                modify_log += m_GlassData.CarrierId.ToString() + "\n";

                modify_log += "[ CarrierCount ] : " + m_GlassData.CarrierCount.ToString() + " --> "; 
                m_GlassData.CarrierCount = SysUtils.StrToInt(cv_TxcarrierCount.Text.Trim());
                modify_log += m_GlassData.CarrierCount.ToString() + "\n";

                modify_log += "[ CarrierDirty ] : " + m_GlassData.CarrierDirty.ToString() + " --> "; 
                m_GlassData.CarrierDirty = SysUtils.StrToInt(cv_TxcarrierDirty.Text.Trim());
                modify_log += m_GlassData.CarrierDirty.ToString() + "\n";


                modify_log += "[ GlassId ] : " + m_GlassData.GlassId.ToString() + " --> "; 
                m_GlassData.GlassId = cv_TxGlassId.Text.Trim();
                modify_log += m_GlassData.GlassId.ToString() + "\n";

                modify_log += "[ CassetteSequence ] : " + m_GlassData.CassetteSequence.ToString() + " --> "; 
                m_GlassData.CassetteSequence = SysUtils.StrToInt(cv_txCasQue.Text.Trim());
                modify_log += m_GlassData.CassetteSequence.ToString() + "\n";


                modify_log += "[ PPID ] : " + m_GlassData.PPID.ToString() + " --> "; 
                m_GlassData.PPID = cv_TxPPID.Text.Trim();
                modify_log += m_GlassData.PPID.ToString() + "\n";


                modify_log += "[ GroupNo ] : " + m_GlassData.GroupNo.ToString() + " --> "; 
                m_GlassData.GroupNo = SysUtils.StrToInt(cv_TxGroupNo.Text.Trim());
                modify_log += m_GlassData.GroupNo.ToString() + "\n";


                modify_log += "[ GlassGrade ] : " + m_GlassData.GlassGrade.ToString() + " --> "; 
                m_GlassData.GlassGrade = cv_TxGlassGrade.Text.Trim();
                modify_log += m_GlassData.GlassGrade.ToString() + "\n";


                modify_log += "[ SrcPortNo ] : " + m_GlassData.SrcPortNo.ToString() + " --> "; 
                m_GlassData.SrcPortNo = SysUtils.StrToInt(cv_CbSrcPortNo.Text.Trim());
                modify_log += m_GlassData.SrcPortNo.ToString() + "\n";


                modify_log += "[ SrcSlotNo ] : " + m_GlassData.SrcSlotNo.ToString() + " --> "; 
                m_GlassData.SrcSlotNo = SysUtils.StrToInt(cv_CbSrcSlotNo.Text.ToString().Trim());
                modify_log += m_GlassData.SrcSlotNo.ToString() + "\n";


                modify_log += "[ CimModeCreate ] : " + m_GlassData.CimModeCreate.ToString() + " --> "; 
                m_GlassData.CimModeCreate = (GlassData.CimModeCreate)cv_CbCimMode.SelectedIndex;
                modify_log += m_GlassData.CimModeCreate.ToString() + "\n";


                modify_log += "[ GlassType ] : " + m_GlassData.GlassType.ToString() + " --> "; 
                m_GlassData.GlassType = (GlassData.GlassType)cv_CbGlassType.SelectedIndex;
                modify_log += m_GlassData.GlassType.ToString() + "\n";


                modify_log += "[ ProcessSkipFlag ] : " + m_GlassData.ProcessSkipFlag.ToString() + " --> "; 
                m_GlassData.ProcessSkipFlag = (GlassData.ProcessSkipFlag)cv_CbProcessFlag.SelectedIndex;
                modify_log += m_GlassData.ProcessSkipFlag.ToString() + "\n";


                modify_log += "[ LastGlassFlag ] : " + m_GlassData.LastGlassFlag.ToString() + " --> "; 
                m_GlassData.LastGlassFlag = (GlassData.LastGlassFlag)cv_CbLastFlag.SelectedIndex;
                modify_log += m_GlassData.LastGlassFlag.ToString() + "\n";


                modify_log += "[ Glassjudge ] : " + m_GlassData.Glassjudge.ToString() + " --> "; 
                m_GlassData.Glassjudge = (GlassData.GlassJudge)cv_CmGlassjudge.SelectedIndex;
                modify_log += m_GlassData.Glassjudge.ToString() + "\n";


                modify_log += "[ InspectionJudgeResult ] : " + m_GlassData.InspectionJudgeResult.ToString() + " --> "; 
                m_GlassData.InspectionJudgeResult = (GlassData.InspectionJudgeResult)cv_CbInspectionJudge.SelectedIndex;
                modify_log += m_GlassData.InspectionJudgeResult.ToString() + "\n";


                modify_log += "[ EqSpecialFlag ] : " + m_GlassData.EqSpecialFlag.ToString() + " --> "; 
                m_GlassData.EqSpecialFlag = (GlassData.EqSpecialFlag)cv_CbSpecialFlag.SelectedIndex;
                modify_log += m_GlassData.EqSpecialFlag.ToString() + "\n";


                modify_log += "[ NgMark ] : " + m_GlassData.NgMark.ToString() + " --> "; 
                m_GlassData.NgMark = (GlassData.NgMark)cv_CbNGMark.SelectedIndex;
                modify_log += m_GlassData.NgMark.ToString() + "\n";


                modify_log += "[ TarPortNo ] : " + m_GlassData.TarPortNo.ToString() + " --> "; 
                m_GlassData.TarPortNo = SysUtils.StrToInt(cv_CbTarPort.Text.Trim());
                modify_log += m_GlassData.TarPortNo.ToString() + "\n";


                modify_log += "[ TarSlotNo ] : " + m_GlassData.TarSlotNo.ToString() + " --> "; 
                m_GlassData.TarSlotNo = SysUtils.StrToInt(cv_CbTarSlot.Text.Trim());
                modify_log += m_GlassData.TarSlotNo.ToString() + "\n";


                modify_log += "[ TarCassetteId ] : " + m_GlassData.TarCassetteId.ToString() + " --> "; 
                m_GlassData.TarCassetteId = cv_TxTarCasId.Text.Trim();
                modify_log += m_GlassData.TarCassetteId.ToString() + "\n";


                modify_log += "[ SortGrade ] : " + m_GlassData.SortGrade.ToString() + " --> "; 
                m_GlassData.SortGrade = cv_TxSortGrade.Text.Trim();
                modify_log += m_GlassData.SortGrade.ToString() + "\n";


                modify_log += "[ InsepctionReservaion ] : " + m_GlassData.InsepctionReservaion.ToString() + " --> "; 
                m_GlassData.InsepctionReservaion = SysUtils.StrToInt(cv_TxInspectionReservation.Text.Trim());
                modify_log += m_GlassData.InsepctionReservaion.ToString() + "\n";


                modify_log += "[ ProcessReservation ] : " + m_GlassData.ProcessReservation.ToString() + " --> "; 
                m_GlassData.ProcessReservation = SysUtils.StrToInt(cv_TxProcessReservation.Text.Trim());
                modify_log += m_GlassData.ProcessReservation.ToString() + "\n";


                modify_log += "[ TrackingHistory ] : " + m_GlassData.TrackingHistory.ToString() + " --> "; 
                m_GlassData.TrackingHistory = cv_CbTracking.SelectedIndex == 0 ? false : true;
                modify_log += m_GlassData.TrackingHistory.ToString() + "\n";


                modify_log += "[ NAR1TrackingHistory ] : " + m_GlassData.NAR1TrackingHistory.ToString() + " --> "; 
                m_GlassData.NAR1TrackingHistory = cv_CbTrackingNar1.SelectedIndex == 0 ? false : true;
                modify_log += m_GlassData.NAR1TrackingHistory.ToString() + "\n";


                modify_log += "[ NAR2TrackingHistory ] : " + m_GlassData.NAR2TrackingHistory.ToString() + " --> "; 
                m_GlassData.NAR2TrackingHistory = cv_CbTrackingNar2.SelectedIndex == 0 ? false : true;
                modify_log += m_GlassData.NAR2TrackingHistory.ToString() + "\n";


                modify_log += Common.cv_SplitSymbol;
                LogicKernel.EventHandler.WriteUserLog(modify_log);

                return m_GlassData;
            */
        }
        #endregion

        #region Remove
        //press remove
        private void button3_Click(object sender, EventArgs e)
        {
            /*
            if (CPC.Perssion != CPC.SystemPerssion.Root)
            {
                Common.PopForm("Please Log in with Root");
                return;
            }
            if(!CheckGBInStopOrDown())
            {
                return;
            }
            if(  !((cv_CheckRecovery.Checked) || (cv_CheckScrap.Checked) || (cv_CheckTakenOut.Checked)) )
            {
                Common.PopForm("Please choice one remove option");
                return;
            }

            int combox_index = cv_CbPosition.SelectedIndex;
            GBPLC.Position tmp_gb_position = Position.None;
            GBOtherPosition tmp_other_position = GBOtherPosition.None;
            UnicomPLC.BCRemoveGlassType remove_type = GetRemoveTypeFormCheckButton();
            if (combox_index < 4)
            {
                tmp_other_position = (GBOtherPosition)combox_index;
            }
            else
            {
                tmp_gb_position = (Position)(combox_index - 4);
            }
            if (tmp_gb_position == Position.None)
            {
                if (remove_type == UnicomPLC.BCRemoveGlassType.Recovery)
                {
                    if( (LogicKernel.EventHandler.cv_AlarmTable.ContainsKey(1010)) || (LogicKernel.EventHandler.cv_AlarmTable.ContainsKey(1011)) )
                    {
                        Common.PopForm("Recovery had time out event. Please reset first");
                        return;
                    }
                    string m_Glassdata = "";
                    AddGlassInOtherPosition(tmp_other_position, out m_Glassdata, true);
                }
                else
                {
                    //remove function glass in normal position
                    RemoveGlassInOtherPosition(tmp_other_position , remove_type);
                }
            }
            else
            {
                if (remove_type == UnicomPLC.BCRemoveGlassType.Recovery)
                {
                    if( (LogicKernel.EventHandler.cv_AlarmTable.ContainsKey(1010)) || (LogicKernel.EventHandler.cv_AlarmTable.ContainsKey(1011)) )
                    {
                        Common.PopForm("Recovery had time out event. Please reset first");
                        return;
                    }
                    string m_Glassdata = "";
                    AddGlassInNormalPosition(tmp_gb_position, out m_Glassdata, true);
                }
                else
                {
                    //remove function glass in other position
                    RemoveGlassInNormalPosition(tmp_gb_position , remove_type);
                }
            }
            */
        }

        #endregion
        private void cv_BtModify_Click(object sender, EventArgs e)
        {
            /*
            string user_log = "[ User push GlassDataOperator Modify button ] \n";
            string m_GlassData = "";
            if (CPC.Perssion != CPC.SystemPerssion.Root)
            {
                user_log += "modify Glass not in Root permission!!\n";
                user_log += Common.cv_SplitSymbol;
                LogicKernel.EventHandler.WriteUserLog(user_log);
                Common.PopForm("Please Log in with Root");
                return;
            }

            int combox_index = cv_CbPosition.SelectedIndex;
            GBPLC.Position tmp_gb_position = Position.None;
            GBOtherPosition tmp_other_position = GBOtherPosition.None;
            if (combox_index < 4)
            {
                tmp_other_position = (GBOtherPosition)combox_index;
            }
            else
            {
                tmp_gb_position = (Position)(combox_index - 4);
            }
            if (tmp_gb_position == Position.None)
            {
                //remove function glass in normal position              
                ModifyInOther(tmp_other_position, out m_GlassData);
                user_log += "Modify Data Successful \n";
                user_log += m_GlassData + "\n";
            }
            else
            {
                //remove function glass in other position
                ModifyInNormal(tmp_gb_position, out m_GlassData);
                user_log += "Modify Data Successful \n";
                user_log += m_GlassData + "\n";
            }
            if (!string.IsNullOrEmpty(user_log))
            {
                if (m_GlassData != "")
                {
                    user_log += Common.cv_SplitSymbol;
                    LogicKernel.EventHandler.WriteUserLog(user_log);
                }
            }
            */
        }

        //request data
        private void button5_Click(object sender, EventArgs e)
        {
            /*
            if (CPC.cv_RemoteStatus != LogicKernel.CimMode.On)
            {
                Common.PopForm("CIM off can't request glass data !");
                return;
            }
            if (MainForm.cv_RequestDataForm != null)
            {
                int combox_index = cv_CbPosition.SelectedIndex;
                GBPLC.Position tmp_gb_position = Position.None;
                GBOtherPosition tmp_other_position = GBOtherPosition.None;
                UnicomPLC.BCRemoveGlassType remove_type = GetRemoveTypeFormCheckButton();
                if (combox_index < 4)
                {
                    tmp_other_position = (GBOtherPosition)combox_index;
                }
                else
                {
                    tmp_gb_position = (Position)(combox_index - 4);
                }
                if (MainForm.cv_RequestDataForm.Visible)
                {
                    if (tmp_gb_position == Position.None)
                    {
                        MainForm.cv_RequestDataForm.Register(tmp_other_position);
                    }
                    else
                    {
                        MainForm.cv_RequestDataForm.Register(tmp_gb_position);
                    }
                    MainForm.cv_RequestDataForm.TopMost = true;
                }
                else
                {
                    if (tmp_gb_position == Position.None)
                    {
                        MainForm.cv_RequestDataForm.Register(tmp_other_position);
                    }
                    else
                    {
                        MainForm.cv_RequestDataForm.Register(tmp_gb_position);
                    }
                    MainForm.cv_RequestDataForm.Show();
                }
            }
            */
        }

        private void CheckMaxRow(DataGridView dg_Temp, bool m_IsRemove)
        {
            /*
            while (dg_Temp.Rows.Count > 15)
            {
                DataGridViewRow row = dg_Temp.Rows[dg_Temp.Rows.Count - 1];
                dg_Temp.Rows.Remove(row);
                if (m_IsRemove)
                {
                    cv_RemoveXml.ItemsByName["GlassList"].DeleteItem(cv_RemoveXml.ItemsByName["GlassList"].ItemNumber);
                }
                else
                {
                    cv_ReworkXml.ItemsByName["GlassList"].DeleteItem(cv_RemoveXml.ItemsByName["GlassList"].ItemNumber);
                }
            }
            */
        }

        /*
        public void Add(DataGridView m_DataView ,  DateTime m_Time , GlassData.GlassData m_Glass , bool m_IsRemove)
        {
            string sTime = m_Time.ToString("MM/dd hh:mm:ss");

            //最新資料在最下方
            //DataGrid.Rows.Add(new Object[] { sTime, sCode, sText });
            //最新資料在最上方
            m_DataView.Rows.Insert(0, new object[] { sTime, m_Glass.GlassId });

            //刪除多餘的歷史紀錄
            CheckMaxRow(m_DataView , m_IsRemove);

            //儲存Alarm List
            try
            {
                History tmp = new History(DateTime.Now, m_Glass);
                if (Regex.Match(m_DataView.Name, @"remove", RegexOptions.IgnoreCase).Success)
                {
                    cv_RemoveList.Insert(0, tmp);
                }
                else
                {
                    cv_ReworkList.Insert(0, tmp);
                }
            }
            catch (FormatException e)
            {
                LogicKernel.EventHandler.WriteErrorLog(e.ToString());
                throw e;
            }
        }
        */

        private void cv_DataGridRemove_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            /*
                int row_index = cv_DataGridRemove.CurrentRow.Index;
                UpdateNormalGlassData(cv_RemoveList.ElementAt(row_index).cv_Glass , true);

            */
        }

        private void cv_DataGridRework_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            /*
                int row_index = cv_DataGridRework.CurrentRow.Index;
                UpdateNormalGlassData(cv_ReworkList.ElementAt(row_index).cv_Glass , true);
            */
        }


        private void RecordRemove(CommonData.HIRATA.GlassData m_Glass)
        {
            /*
            LogicKernel.EventHandler.cv_Cs.Enter();
            if (cv_RemoveXml == null) return;
            string msg = @"<Glass RecordTime="""" Start="""" End="""" CarId="""" CarCount="""" CarDirty="""">
		                        <Dic>
		                        </Dic>
		                        <String PPID="""" GlassID="""" GlassGrade="""" SortGrade="""" TarCasId=""""/>
	                        </Glass>";
            KXmlItem tmp = new KXmlItem();
            tmp.Text = msg;
            tmp.Attributes["RecordTime"] = DateTime.Now.ToString("yyyyMMddhhmmss");

            for (int i = 1; i <= 64; i++)
            {
                KXmlItem each_word = new KXmlItem();
                each_word.Text = @"<Word" + i + @"></Word" + i + ">";
                each_word.AsString = m_Glass.cv_OriginalData[i].ToString();
                tmp.ItemsByName["Glass"].ItemsByName["Dic"].AddItem(each_word);
            }
            tmp.ItemsByName["Glass"].ItemsByName["String"].Attributes["PPID"] = m_Glass.PPID;
            tmp.ItemsByName["Glass"].ItemsByName["String"].Attributes["GlassID"] = m_Glass.GlassId;
            tmp.ItemsByName["Glass"].ItemsByName["String"].Attributes["GlassGrade"] = m_Glass.GlassGrade;
            tmp.ItemsByName["Glass"].ItemsByName["String"].Attributes["SortGrade"] = m_Glass.SortGrade;
            tmp.ItemsByName["Glass"].ItemsByName["String"].Attributes["TarCasId"] = m_Glass.TarCassetteId;
            cv_RemoveXml.ItemsByName["GlassList"].InsertItem(tmp, 0);
            cv_RemoveXml.SaveToFile(cv_RemoveFilePath, true);
            LogicKernel.EventHandler.cv_Cs.Leave();
            */
        }
        private void RecordRework(CommonData.HIRATA.GlassData m_Glass)
        {
            /*
                LogicKernel.EventHandler.cv_Cs.Enter();
                if (cv_ReworkXml == null) return;
                string msg = @"<Glass RecordTime="""" Start="""" End="""" CarId="""" CarCount="""" CarDirty="""">
                                    <Dic>
                                    </Dic>
                                    <String PPID="""" GlassID="""" GlassGrade="""" SortGrade="""" TarCasId=""""/>
                                </Glass>";
                KXmlItem tmp = new KXmlItem();
                tmp.Text = msg;
                tmp.Attributes["RecordTime"] = DateTime.Now.ToString("yyyyMMddhhmmss");
                for (int i = 1; i <= 64; i++)
                {
                    KXmlItem each_word = new KXmlItem();
                    each_word.Text = @"<Word" + i + @"></Word" + i + ">";
                    each_word.AsString = m_Glass.cv_OriginalData[i].ToString();
                    tmp.ItemsByName["Glass"].ItemsByName["Dic"].AddItem(each_word);
                }
                tmp.ItemsByName["Glass"].ItemsByName["String"].Attributes["PPID"] = m_Glass.PPID;
                tmp.ItemsByName["Glass"].ItemsByName["String"].Attributes["GlassID"] = m_Glass.GlassId;
                tmp.ItemsByName["Glass"].ItemsByName["String"].Attributes["GlassGrade"] = m_Glass.GlassGrade;
                tmp.ItemsByName["Glass"].ItemsByName["String"].Attributes["SortGrade"] = m_Glass.SortGrade;
                tmp.ItemsByName["Glass"].ItemsByName["String"].Attributes["TarCasId"] = m_Glass.TarCassetteId;
                cv_ReworkXml.ItemsByName["GlassList"].InsertItem(tmp, 0);
                cv_ReworkXml.SaveToFile(cv_ReworkFilePath, true);
                LogicKernel.EventHandler.cv_Cs.Leave();
            */
        }

        public new void Show()
        {
            if (this.Visible)
            {
                this.Focus();
            }
            else
            {
                base.Show();
            }
        }

        private bool CheckCanEditData(CommonData.HIRATA.ActionTarget m_Target, int m_Id, int m_Slot, bool m_IsAdd)
        {
            bool rtn = true;
            switch (m_Target)
            {
                case CommonData.HIRATA.ActionTarget.Port:
                    if (m_IsAdd)
                    {
                        if (UiForm.GetPort(m_Id).cv_Data.GlassDataMap[m_Slot].PHasData)
                        {
                            CommonStaticData.PopForm("The Pos has data.", true, false);
                            rtn = false;
                        }
                    }
                    else
                    {
                        if (!UiForm.GetPort(m_Id).cv_Data.GlassDataMap[m_Slot].PHasData)
                        {
                            CommonStaticData.PopForm("The Pos has not data.", true, false);
                            rtn = false;
                        }
                    }
                    break;
                case CommonData.HIRATA.ActionTarget.Buffer:
                    if (m_IsAdd)
                    {
                        if (UiForm.GetBuffer(m_Id).cv_Data.GlassDataMap[m_Slot].PHasData)
                        {
                            CommonStaticData.PopForm("The Pos has data.", true, false);
                            rtn = false;
                        }
                    }
                    else
                    {
                        if (!UiForm.GetBuffer(m_Id).cv_Data.GlassDataMap[m_Slot].PHasData)
                        {
                            CommonStaticData.PopForm("The Pos has not data.", true, false);
                            rtn = false;
                        }
                    }
                    break;
                case CommonData.HIRATA.ActionTarget.Aligner:
                    if (m_IsAdd)
                    {
                        if (UiForm.GetAligner(m_Id).cv_Data.GlassDataMap[m_Slot].PHasData)
                        {
                            CommonStaticData.PopForm("The Pos has data.", true, false);
                            rtn = false;
                        }
                    }
                    else
                    {
                        if (!UiForm.GetAligner(m_Id).cv_Data.GlassDataMap[m_Slot].PHasData)
                        {
                            CommonStaticData.PopForm("The Pos has not data.", true, false);
                            rtn = false;
                        }
                    }
                    break;
                case CommonData.HIRATA.ActionTarget.Eq:
                    if (m_IsAdd)
                    {
                        if (UiForm.GetEq(m_Id).cv_Data.GlassDataMap[m_Slot].PHasData)
                        {
                            CommonStaticData.PopForm("The Pos has data.", true, false);
                            rtn = false;
                        }
                    }
                    else
                    {
                        if (!UiForm.GetEq(m_Id).cv_Data.GlassDataMap[m_Slot].PHasData)
                        {
                            CommonStaticData.PopForm("The Pos has not data.", true, false);
                            rtn = false;
                        }
                    }
                    break;
            };
            return rtn;
        }
        private void btn_Add_Click(object sender, EventArgs e)
        {
            if (!CheckPermission())
            {
                CommonStaticData.PopForm("Please log in", true, false);
                return;
            }
            CommonData.HIRATA.MDDataAction send_obj = new CommonData.HIRATA.MDDataAction();
            if (Regex.Match((sender as Button).Name, @"add", RegexOptions.IgnoreCase).Success)
            {
                if (!CheckCanEditData(cv_JobTaget, cv_JobId, cv_SlotNo, true))
                {
                    return;
                }
                send_obj.PAction = CommonData.HIRATA.DataEidtAction.Add;
            }
            else
            {
                if (!CheckCanEditData(cv_JobTaget, cv_JobId, cv_SlotNo, false))
                {
                    return;
                }
                send_obj.PAction = CommonData.HIRATA.DataEidtAction.Edit;
            }

            CommonData.HIRATA.GlassData add_data = new CommonData.HIRATA.GlassData();
            add_data.PHasSensor = true;
            add_data.PSlotInEq = (uint)cv_SlotNo;
            add_data.PId = cv_TxGlassId.Text.Trim();
            add_data.PFoupSeq = Convert.ToUInt32(cv_txFoupSeq.Text);
            add_data.PWorkSlot = Convert.ToUInt32(txt_WorkSlot.Text);
            add_data.PProductionCategory = (CommonData.HIRATA.ProductCategory)Enum.Parse(typeof(CommonData.HIRATA.ProductCategory), cb_Production.Text);
            add_data.PProcessFlag = (CommonData.HIRATA.ProcessFlag)Enum.Parse(typeof(CommonData.HIRATA.ProcessFlag), cv_CbProcessFlag.Text);
            add_data.POcrRead = (CommonData.HIRATA.OCRRead)Enum.Parse(typeof(CommonData.HIRATA.OCRRead), cb_OcrReadFlag.Text);
            add_data.PAssamblyFlag = (CommonData.HIRATA.AssambleNeed)Enum.Parse(typeof(CommonData.HIRATA.AssambleNeed), cb_AssambleFlag.Text);
            add_data.PPID = txt_PPID.Text.Trim();
            if (cv_CbCimMode.Text == "OFF")
            {
                add_data.PCimMode = CommonData.HIRATA.OnlineMode.Offline;
            }
            else
            {
                add_data.PCimMode = CommonData.HIRATA.OnlineMode.Control;
            }
            add_data.PWorkOrderNo = Convert.ToUInt32(txt_WorkOrder.Text);
            add_data.PWorkType = (CommonData.HIRATA.WorkType)Enum.Parse(typeof(CommonData.HIRATA.WorkType), cb_WorkType.Text);
            add_data.PGlassJudge = (CommonData.HIRATA.GlassJudge)Enum.Parse(typeof(CommonData.HIRATA.GlassJudge), cv_CmGlassjudge.Text);
            add_data.PPriority = Convert.ToUInt32(cb_Priority.Text);
            add_data.POcrResult = (CommonData.HIRATA.OCRResult)Enum.Parse(typeof(CommonData.HIRATA.OCRResult), cb_OcrResult.Text);
            add_data.PAssamblyResult = (CommonData.HIRATA.AssambleResult)Enum.Parse(typeof(CommonData.HIRATA.AssambleResult), cb_AssambleResult.Text);

            foreach (CommonData.HIRATA.GlassDataNodeItem obj in add_data.cv_Nods)
            {
                obj.PProcessHistory = cv_SlotConditionMap[obj.PNodeId].Count;
                obj.PProcessAbnormal = cv_SlotConditionMap[obj.PNodeId].Abort;
                obj.PRecipe = cv_SlotConditionMap[obj.PNodeId].Recipe;
            }
            send_obj.Source.PTarget = cv_JobTaget;
            send_obj.Source.Id = cv_JobId;
            send_obj.Source.Slot = cv_SlotNo;
            send_obj.GlassData = add_data;
            send_obj.PType = CommonData.HIRATA.MmfEventClientEventType.etRequest;

            string rtn;
            object tmp = null;
            uint ticket;
            /*
            if (!Global.Controller.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDDataAction).Name, send_obj, out ticket, out rtn, out tmp, 3000, KParseObjToXmlPropertyType.Field))
            {
                CommonStaticData.PopForm("Add data time out", true, false);
            }
            */
        }
        private void btn_remove_Click(object sender, EventArgs e)
        {
            if (!CheckPermission())
            {
                CommonStaticData.PopForm("Please log in", true, false);
                return;
            }
            if (string.IsNullOrEmpty(txt_Reason.Text.Trim()))
            {
                CommonStaticData.PopForm("Del data please fill seacon code.", true, false);
                return;
            }
            CommonData.HIRATA.MDDataAction send_obj = new CommonData.HIRATA.MDDataAction();
            send_obj.Source.PTarget = cv_JobTaget;
            send_obj.Source.Id = cv_JobId;
            send_obj.Source.Slot = cv_SlotNo;
            send_obj.Reason = txt_Reason.Text.Trim();
            send_obj.PAction = CommonData.HIRATA.DataEidtAction.Del;
            send_obj.GlassData = new CommonData.HIRATA.GlassData();
            send_obj.PType = CommonData.HIRATA.MmfEventClientEventType.etRequest;
            CommonData.HIRATA.AccountItem cur_account = null;
            if (UiForm.cv_AccountData.GetCurAccount(out cur_account))
            {
                send_obj.Opid = cur_account.PId; 
            }
            else
            {
                send_obj.Opid = "";
            }

            string rtn;
            object tmp = null;
            uint ticket;
            /*
            if (!Global.Controller.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDDataAction).Name, send_obj, out ticket, out rtn, out tmp, 3000, KParseObjToXmlPropertyType.Field))
            {
                CommonStaticData.PopForm("remove data time out", true, false);
            }
            else
            {
                cv_RemoveList.Add(cv_CurData);
                if(cv_RemoveList.Count > 50)
                {
                    cv_RemoveList.RemoveAt(cv_RemoveList.Count - 1);
                }
                KXmlItem xml = new KXmlItem();
                xml.Text = "@<Data/>";
                KXmlItem body = EventCenterBase.ParseObjectToKXmlItem(cv_RemoveList, KParseObjToXmlPropertyType.Field);
                xml.ItemsByName["Data"].AddItem(body);
                xml.SaveToFile(cv_RemoveFilePath, true);
                cv_DataGridRemove.Rows.Insert(0, new object[] { cv_CurData.PId });
                CleanData();
            }
            */
        }
        private void btn_Request_Click(object sender, EventArgs e)
        {
            if (!CheckPermission())
            {
                CommonStaticData.PopForm("Please log in", true, false);
                return;
            }
            if (!CheckCanEditData(cv_JobTaget, cv_JobId, cv_SlotNo, true))
            {
                return;
            }
            cv_RequestForm.Show(cv_Source);
        }
        private bool CheckPermission()
        {
            bool rtn = true;
            CommonData.HIRATA.AccountItem tmp = null;
            if(UiForm.cv_AccountData.GetCurAccount(out tmp))
            {
                if(tmp.PPermission == CommonData.HIRATA.UserPermission.None)
                {
                    rtn = false;
                }
            }
            else
            {
                rtn = false;
            }
            return rtn;
        }

        private void cv_DataGridRemove_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int index = cv_DataGridRemove.CurrentRow.Index;
            GlassData tmp = cv_RemoveList[index];
            Register(tmp);
        }
    }
}
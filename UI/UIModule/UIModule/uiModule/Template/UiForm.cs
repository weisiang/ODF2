using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using KgsCommon;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using CommonData.HIRATA;
using BaseAp;
using CIM;
using LGC;

namespace UI
{
    public partial class UiForm : BaseForm
    {
        #region permission
        public enum enumGroup { All = -1 , Group1 = 0, Group2, Group3, Group4, Group5, Group6 };
        public static KXmlItem cv_PermissionGroupsXml = null;
        PermissionSetting cv_PermissionSetting = new PermissionSetting();
        KDateTime cv_InitialButtonFlashTime = SysUtils.Now();
        public static Dictionary<CommonData.HIRATA.UserPermission, List<int>> cv_PermissionGroups = new Dictionary<UserPermission, List<int>>();
        public static Dictionary<enumGroup, List<Control>> cv_GroupObjs = new Dictionary<enumGroup, List<Control>>();
        #endregion

        public static Dictionary<EqGifTimeChartId, KeyValuePair<int, string>> cv_TimeChartStep = new Dictionary<EqGifTimeChartId, KeyValuePair<int, string>>();



        #region form
        public static bool cv_Isdragdrop = false;
        System.Windows.Forms.NotifyIcon notifyIcon;
        public static UiGlassDataOperator cv_GlassDataForm = new UiGlassDataOperator(15);
        palette cv_PaletteForm = null;
        Login cv_LoginForm = new Login();
        KLogPage cv_LogPage = new KLogPage();
        TimrOutForm cv_TimeOutForm = new TimrOutForm();
        CopyLogForm cv_CopyLogsForm = new CopyLogForm();
        RecipeSetting cv_RecipeSetting = new RecipeSetting();
        MonitorForm cv_MonitorForm = new MonitorForm();
        RecipeCheckForm cv_RecipeCheckForm = new RecipeCheckForm();
        private Dictionary<string, Label> cv_ModuleLbl = new Dictionary<string, Label>();
        internal LotSummary cv_LotSummary = null;
        internal Status cv_StatusTable = null;
        internal OcrDecide cv_OcrDecide = new OcrDecide();
        #endregion

        internal static BindingList<PortData> cv_SummaryPortData = new BindingList<PortData>();
        internal static Dictionary<int, Eq> cv_EqContainer = new Dictionary<int, Eq>();
        internal static Dictionary<int, Aligner> cv_AlignerContainer = new Dictionary<int, Aligner>();
        internal static Dictionary<int, Port> cv_PortContainer = new Dictionary<int, Port>();
        internal static Dictionary<int, Robot> cv_RobotContainer = new Dictionary<int, Robot>();
        internal static Dictionary<int, Buffer> cv_BufferContainer = new Dictionary<int, Buffer>();

        delegate void DeleAppEvent(string m_MessageId, object m_Object);
        Dictionary<string, DeleAppEvent> cv_AppEventMap = new Dictionary<string, DeleAppEvent>();

        public static KMemoLog cv_BcMsgLog;
        KTimer cv_Timer2 = null;
        public static List<ThreadObj> cv_threadPool = new List<ThreadObj>();

        public static UIController cv_eventController = null;

        #region lgc , cim modules.
        public CimModule cv_CimModule = null;
        public LgcModule cv_LgcModule = null;
        #endregion

        public UiForm(string[] args): base(args, FdModule.UI)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.DoubleBuffer, true);

            this.UpdateStyles();

            this.cv_dataViewAccount.AutoGenerateColumns = false;
            this.cv_AlarmDataView.AutoGenerateColumns = false;
            this.cv_AlarmDataView.AutoGenerateColumns = false;

            this.cv_AlarmHView.AutoGenerateColumns = false;
            this.cv_AlarmHView.AutoGenerateColumns = false;

            addPermisionGroupObj();

            cv_PermissionSetting.Dock = DockStyle.Fill;
            cv_PermissionGroupsXml = new KXmlItem();
            cv_PermissionGroupsXml.LoadFromFile(CommonData.HIRATA.CommonStaticData.g_SysPermissionGroupFile);
            InitPermissionGroups();
            cv_PermissionSetting.LoadFromSetting();
            cv_tpPermissionSetLog.Controls.Add(cv_PermissionSetting);

            ModuleInit();
            InitLogPage();
            this.SuspendLayout();
            layoutInit();
            this.ResumeLayout();
            InitRecipeForm();
            IniMonitorForm();
            InitRecipeCheckForm();
            setFormTitle();
            InitIcon();
            InitAllSubForm();
            AssignAppEvent();
            cv_eventController = new UIController();
            AddControllerLogToUI();
            reFreshAccountGrid();
            setVetsion();
            setManualPage();
            InitIcon();
            cv_PaletteForm = new palette();
            LinkControllerEvent();
            cv_AccountData.EventLogInOut += OnLogInOutEvent;
            ChangeObjEnable();
            CommonData.HIRATA.CommonStaticData.KillTerminal(args);
            WriteLog(LogLevelType.General, "[UI module start]");
            InitialOtherModule();
            initTimer();
            cv_Timer.Start();
        }
        public void InitialOtherModule()
        {
            if(cv_CimModule == null)
            {
                cv_CimModule = new CimModule();
            }
            if(cv_LgcModule == null)
            {
                cv_LgcModule = new LgcModule();
            }
        }
        private void InitPermissionGroups()
        {
            AddListToPermissionGroup("Root", UserPermission.Root);
            AddListToPermissionGroup("Engineer", UserPermission.Engineer);
            AddListToPermissionGroup("OP1", UserPermission.OP1);
            AddListToPermissionGroup("OP2", UserPermission.OP2);
            AddListToPermissionGroup("OP3", UserPermission.OP3);
        }
        private void addPermisionGroupObj()
        {
            AddUiObjToEnableList(cv_btnSelectMode, enumGroup.Group2);
            AddUiObjToEnableList(btn_ReIni, enumGroup.Group1);
            AddUiObjToEnableList(btn_TimeOut, enumGroup.Group4);
            AddUiObjToEnableList(btn_CopyLogs, enumGroup.Group3);
            AddUiObjToEnableList(btn_RecipeCheck, enumGroup.Group3);
            AddUiObjToEnableList(btn_rightOcrMode, enumGroup.Group3);
            AddUiObjToEnableList(btn_Exit, enumGroup.Group3);
            AddUiObjToEnableList(lbl_RobotStatus, enumGroup.Group2);
            AddUiObjToEnableList(cv_SystemAuto, enumGroup.Group2);
            AddUiObjToEnableList(btn_resetAllAlarm, enumGroup.Group1);
            AddUiObjToEnableList(btn_buzzerOff, enumGroup.Group1);

            AddUiObjToEnableList(bt_RobotExe, enumGroup.Group5);
            AddUiObjToEnableList(bt_PortExe, enumGroup.Group5);
            AddUiObjToEnableList(bt_AlignerExe, enumGroup.Group5);
            AddUiObjToEnableList(bt_IoExe, enumGroup.Group5);
            AddUiObjToEnableList(btn_ExeApiCommand, enumGroup.Group5);
            AddUiObjToEnableList(btn_ExeCommonCommand, enumGroup.Group5);
            AddUiObjToEnableList(btn_ExeOcrCommand, enumGroup.Group5);
            AddUiObjToEnableList(btn_ExtRobotSpeed, enumGroup.Group5);
            AddUiObjToEnableList(btn_ExtFFu, enumGroup.Group5);
            AddUiObjToEnableList(btn_ExtRobotVac, enumGroup.Group5);
            AddUiObjToEnableList(btn_pos, enumGroup.Group6);
        }
        private void AddListToPermissionGroup(string m_Group , CommonData.HIRATA.UserPermission m_Permission)
        {
            List<int> tmp = new List<int>();
            for (int i = 1; i <= 6; i++)
            {
                if (UiForm.cv_PermissionGroupsXml.ItemsByName[m_Group].Attributes["Group" + i.ToString()].Trim() == "1")
                {
                    tmp.Add(i);
                }
            }
            cv_PermissionGroups[m_Permission] = tmp;
        }
        private void initTimer()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            if (cv_Timer2 == null)
            {
                cv_Timer2 = new KTimer();
                cv_Timer2.Interval = 500;
                cv_Timer2.ThreadEventEnabled = false;
                cv_Timer2.OnTimer += DerivedOnTimer;
                cv_Timer2.Enabled = true;
                cv_Timer2.Open();
            }
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        private void LinkControllerEvent()
        {
           // UIController.EventAppEvent += OnControllerEvent;
        }


        #region Link log in/out Event & Event function;
        //Trigger this event When AccountData login/out successful.(UI must override)
        protected void OnLogInOutEvent(LogInOut m_Action, CommonData.HIRATA.AccountItem m_CurAccount)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, CommonData.HIRATA.CommonStaticData.__FUN(), CommonData.HIRATA.FunInOut.Enter);
            ChangeObjEnable();
           // cv_MmfController.SendAccountData();
           // cv_MmfController.SendLogInOut(m_Action, MmfEventClientEventType.etNotify);
            if (m_Action == LogInOut.Login)
            {
                AccountItem cur_account = null;
                if (UiForm.cv_AccountData.GetCurAccount(out cur_account))
                {
                    lbl_AccountId.Text = cur_account.PId;
                    lbl_AccountPm.Text = cur_account.PPermission.ToString();
                }
            }
            else
            {
                lbl_AccountPm.Text = "";
                lbl_AccountId.Text = "";
                cv_tcBar.SelectedIndex = 0;
            }
            WriteLog(LogLevelType.NormalFunctionInOut, CommonData.HIRATA.CommonStaticData.__FUN(), CommonData.HIRATA.FunInOut.Enter);
        }

        //Trigger this event When AccountData change.(UI must override)
        protected void OnAccountChangeEvent()
        {
           // cv_MmfController.SendAccountData();
            if (cv_tcBar.SelectedTab != cv_tpAccount)
            {
                cv_tcBar.SelectedTab = cv_tpAccount;
            }
            cv_dataViewAccount.DataSource = null;
            cv_dataViewAccount.DataSource = UiForm.cv_AccountData.cv_AccountList;
            cv_dataViewAccount.Refresh();
        }
        #endregion

        //public static void AddUiObjToEnableList(Control m_Control, UserPermission m_Permission)
        public static void AddUiObjToEnableList(Control m_Control, enumGroup m_GroupId)
        {
            if (cv_GroupObjs.Count == 0)
            {
                cv_GroupObjs.Add(enumGroup.Group1, new List<Control>());
                cv_GroupObjs.Add(enumGroup.Group2, new List<Control>());
                cv_GroupObjs.Add(enumGroup.Group3, new List<Control>());
                cv_GroupObjs.Add(enumGroup.Group4, new List<Control>());
                cv_GroupObjs.Add(enumGroup.Group5, new List<Control>());
                cv_GroupObjs.Add(enumGroup.Group6, new List<Control>());
                cv_GroupObjs.Add(enumGroup.All, new List<Control>());
            }
            switch (m_GroupId)
            {
                case enumGroup.Group1:
                    cv_GroupObjs[enumGroup.Group1].Add(m_Control);
                    cv_GroupObjs[enumGroup.All].Add(m_Control);
                    break;
                case enumGroup.Group2:
                    cv_GroupObjs[enumGroup.Group2].Add(m_Control);
                    cv_GroupObjs[enumGroup.All].Add(m_Control);
                    break;
                case enumGroup.Group3:
                    cv_GroupObjs[enumGroup.Group3].Add(m_Control);
                    cv_GroupObjs[enumGroup.All].Add(m_Control);
                    break;
                case enumGroup.Group4:
                    cv_GroupObjs[enumGroup.Group4].Add(m_Control);
                    cv_GroupObjs[enumGroup.All].Add(m_Control);
                    break;
                case enumGroup.Group5:
                    cv_GroupObjs[enumGroup.Group5].Add(m_Control);
                    cv_GroupObjs[enumGroup.All].Add(m_Control);
                    break;
                case enumGroup.Group6:
                    cv_GroupObjs[enumGroup.Group6].Add(m_Control);
                    cv_GroupObjs[enumGroup.All].Add(m_Control);
                    break;
            }
            /*
            if (!cv_AllUiObj.ContainsKey(m_Control))
            {
                cv_AllUiObj.Add(m_Control, m_Permission);
            }
            */
        }
        private void OnControllerEvent(string m_Id, object obj)
        {
            if (cv_AppEventMap.ContainsKey(m_Id))
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        OnControllerEvent(m_Id, obj);
                    }));
                }
                else
                {
                    cv_AppEventMap[m_Id](m_Id, obj);
                }
            }
        }
        protected override void ModuleInit()
        {
            UiForm.cv_AccountData.SetFilePath(CommonData.HIRATA.CommonStaticData.g_SysAccountFile);
            UiForm.cv_AccountData.PIsAutoSave = true;
            UiForm.cv_AccountData.LoadFromFile();

            UiForm.cv_Recipes.PIsAutoSave = false;
            UiForm.cv_Alarms.PIsAutoSave = false;
        }

        private void IniMonitorForm()
        {
            if (cv_MonitorForm == null)
            {
                cv_MonitorForm = new MonitorForm();
            }
            int w = 0, h = 0;

            for (int i = 0; i < 5; i++)
            {
                IfMonitor tmp = new IfMonitor();
                tmp.Dock = DockStyle.Left;
                cv_MonitorForm.Controls.Add(tmp);
                w = tmp.Width;
                h = tmp.Height;
            }
            cv_MonitorForm.Width = w * 5 + 20;
            cv_MonitorForm.Height = h + 10;
        }
        private void InitRecipeCheckForm()
        {
            if (cv_RecipeCheckForm == null)
            {
                cv_RecipeCheckForm = new RecipeCheckForm();
            }
        }

        private void SetTestMenual()
        {
            cb_ManualApi.Items.Add("Version,API");
            cb_ManualApi.Items.Add("Remote,API");
            cb_ManualApi.Items.Add("Local,API");
            cb_ManualApi.Items.Add("CurrentMode,API");
            cb_ManualApi.Items.Add("Hide,API");
        }

        #region Manual page
        private void CleanManualPage()
        {
            cb_RobotActionTarget.SelectedIndex = -1;
            cb_RobotActionTarget.Items.Clear();

            cb_RobotActionName.SelectedIndex = -1;
            cb_RobotActionName.Items.Clear();

            cb_RobotActionRobotId.SelectedIndex = -1;
            cb_RobotActionRobotId.Items.Clear();

            cb_RobotActionArm.SelectedIndex = -1;
            cb_RobotActionArm.Items.Clear();

            cb_PortActionPortId.SelectedIndex = -1;
            cb_PortActionPortId.Items.Clear();

            cb_PortActionName.SelectedIndex = -1;
            cb_PortActionName.Items.Clear();
        }
        private void setManualPage()
        {
            CleanManualPage();

            cb_ManualApi.Items.AddRange(Enum.GetNames(typeof(APIEnum.APICommand)).ToArray<string>());

            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_AlignerNumber; i++)
                cb_AlignerId.Items.Add(i.ToString());

            cb_AlignerAction.Items.AddRange(Enum.GetNames(typeof(APIEnum.AlignerCommand)).ToArray<string>());


            cb_DeviceCommon.Items.AddRange(Enum.GetNames(typeof(APIEnum.CommonCommand)).ToArray<string>());
            cb_CommonTarget.Items.AddRange(Enum.GetNames(typeof(APIEnum.CommnadDevice)).ToArray<string>());
            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
                cb_CommonId.Items.Add(i.ToString());

            cb_OcrAction.Items.AddRange(Enum.GetNames(typeof(APIEnum.OcrCommand)).ToArray<string>());


            List<string> target_list = Enum.GetNames(typeof(CommonData.HIRATA.ActionTarget)).ToList();
            foreach (var item in target_list)
            {
                if (Regex.Match(item.ToString(), "[^robot]", RegexOptions.IgnoreCase).Success)
                {
                    cb_RobotActionTarget.Items.Add(item.ToString());
                }
            }

            target_list = Enum.GetNames(typeof(CommonData.HIRATA.RobotAction)).ToList();

            foreach (var item in target_list)
            {
                //if (!Regex.Match(item.ToString(), @"ini|comp|none|high", RegexOptions.IgnoreCase).Success)
                if (Regex.Match(item.ToString(), @"\bget\b|\bput\b|\bPutGetAligner\b|\bExchange\b", RegexOptions.IgnoreCase).Success)
                {
                    cb_RobotActionName.Items.Add(item.ToString());
                }
            }

            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_RobotNumber; i++)
            {
                cb_RobotActionRobotId.Items.Add(i.ToString());
            }

            target_list = Enum.GetNames(typeof(CommonData.HIRATA.RobotArm)).ToList();

            foreach (var item in target_list)
            {
                if (!Regex.Match(item.ToString(), @"none|both", RegexOptions.IgnoreCase).Success)
                    cb_RobotActionArm.Items.Add(item.ToString());
            }

            for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
            {
                cb_PortActionPortId.Items.Add(i.ToString());
            }
            target_list = Enum.GetNames(typeof(CommonData.HIRATA.PortAction)).ToList();
            cb_PortActionName.Items.AddRange(Enum.GetNames(typeof(APIEnum.LoadPortCommand)).ToArray<string>());

            cb_IoAction.Items.AddRange(Enum.GetNames(typeof(APIEnum.IoCommand)).ToArray<string>());


        }
        private void cb_RobotActionTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tmp = cb_RobotActionTarget.Text;
            cb_RobotActionTargetId.SelectedIndex = -1;
            cb_RobotActionTargetId.Items.Clear();
            if (Regex.Match(tmp, @"Eq", RegexOptions.IgnoreCase).Success)
            {
                for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_EqNumber; i++)
                {
                    cb_RobotActionTargetId.Items.Add(GetEq(i).cv_Alias);
                }
            }
            else if (Regex.Match(tmp, @"Port", RegexOptions.IgnoreCase).Success)
            {
                for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_PortNumber; i++)
                {
                    cb_RobotActionTargetId.Items.Add(i.ToString());
                }
            }
            else if (Regex.Match(tmp, @"Buffer", RegexOptions.IgnoreCase).Success)
            {
                for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_BufferNumber; i++)
                {
                    cb_RobotActionTargetId.Items.Add(i.ToString());
                }
            }
            else if (Regex.Match(tmp, @"Aligner", RegexOptions.IgnoreCase).Success)
            {
                for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_AlignerNumber; i++)
                {
                    cb_RobotActionTargetId.Items.Add(i.ToString());
                }
            }
        }
        private void cb_RobotActionTargetId_SelectedValueChanged(object sender, EventArgs e)
        {
            string target = cb_RobotActionTarget.Text;
            string id = cb_RobotActionTargetId.Text;
            if (string.IsNullOrEmpty(id)) return;
            //int id_int = Convert.ToInt16(id);
            int id_int = cb_RobotActionTargetId.SelectedIndex + 1;
            cb_RobotActionTargetSlot.SelectedIndex = -1;
            cb_RobotActionTargetSlot.Items.Clear();
            int size = 0;
            if (Regex.Match(target, @"Eq", RegexOptions.IgnoreCase).Success)
            {
                if (LgcModule.GetEqById(Convert.ToInt16(id_int)) != null)
                {
                    size = LgcModule.GetEqById(id_int).cv_SlotCount;
                }
                for (int i = 1; i <= size; i++)
                {
                    cb_RobotActionTargetSlot.Items.Add(i.ToString());
                }
            }
            else if (Regex.Match(target, @"Port", RegexOptions.IgnoreCase).Success)
            {
                for (int i = 1; i <= CommonData.HIRATA.CommonStaticData.g_CstSize; i++)
                {
                    cb_RobotActionTargetSlot.Items.Add(i.ToString());
                }
            }
            else if (Regex.Match(target, @"Buffer", RegexOptions.IgnoreCase).Success)
            {
                if (UiForm.GetBuffer(Convert.ToInt16(id_int)) != null)
                {
                    size = UiForm.GetBuffer(id_int).cv_SlotCount;
                }
                for (int i = 1; i <= size; i++)
                {
                    cb_RobotActionTargetSlot.Items.Add(i.ToString());
                }
            }
            else if (Regex.Match(target, @"Aligner", RegexOptions.IgnoreCase).Success)
            {
                if (UiForm.GetAligner(Convert.ToInt16(id_int)) != null)
                {
                    size = UiForm.GetAligner(id_int).cv_SlotCount;
                }
                for (int i = 1; i <= size; i++)
                {
                    cb_RobotActionTargetSlot.Items.Add(i.ToString());
                }
            }
        }
        private void bt_RobotExe_Click(object sender, EventArgs e)
        {
            //WriteNormalIn
            string log = "User press Robot Action EXE" + Environment.NewLine;
            int robot_id = cb_RobotActionRobotId.SelectedIndex + 1;
            int tar = cb_RobotActionTarget.SelectedIndex + 1;
            int id = cb_RobotActionTargetId.SelectedIndex + 1;
            int slot = cb_RobotActionTargetSlot.SelectedIndex + 1;
            int arm = cb_RobotActionArm.SelectedIndex + 1;
            int action = cb_RobotActionName.SelectedIndex + 1;
            string vac_control = cb_RbVac.Text.Trim();

            CommonData.HIRATA.ActionTarget arg_tar = (CommonData.HIRATA.ActionTarget)tar;
            CommonData.HIRATA.RobotArm arg_arm = (CommonData.HIRATA.RobotArm)Enum.Parse(typeof(CommonData.HIRATA.RobotArm), cb_RobotActionArm.Text.Trim());
            CommonData.HIRATA.RobotAction arg_action = (CommonData.HIRATA.RobotAction)action;

            if (!Enum.TryParse<RobotAction>(cb_RobotActionName.Text, out arg_action))
            {
                CommonStaticData.PopForm("Please re-select robot action", true, false);
                return;
            }
            if (arg_action == RobotAction.PutGetAligner)
            {
                if (arg_tar != ActionTarget.Aligner)
                {
                    CommonStaticData.PopForm("Aligner use the action only", true, false);
                    return;
                }
                Double tmp_deg = 0.0;
                if ((string.IsNullOrEmpty(cb_robotActionDeg.Text)) || (!Double.TryParse(cb_robotActionDeg.Text, out tmp_deg)))
                {
                    CommonStaticData.PopForm("Use Get Put Aligner function please select correct Degreen.", true, false);
                    return;
                }
            }
            if ((arg_tar == ActionTarget.Eq) && ((id == (int)EqId.VAS1) || (id == (int)EqId.VAS2) ))
            {
                if (arg_action == RobotAction.Put)
                {
                    if (slot == 1)
                    {
                        if (arg_arm != RobotArm.rbaUp)
                        {
                            CommonStaticData.PopForm("VAS low stage only up arm can put", true, false);
                            return;
                        }
                    }
                    if (slot == 2)
                    {
                        if (arg_arm != RobotArm.rbaDown)
                        {
                            CommonStaticData.PopForm("VAS up stage only low arm can put", true, false);
                            return;
                        }
                    }
                }
                else if (arg_action == RobotAction.Get)
                {
                    if (slot == 1)
                    {
                        if (arg_arm != RobotArm.rbaDown)
                        {
                            CommonStaticData.PopForm("VAS get stage only low arm", true, false);
                            return;
                        }
                    }
                    else
                    {
                        CommonStaticData.PopForm("VAS get stage only low statge", true, false);
                        return;
                    }
                }
                else
                {
                    CommonStaticData.PopForm("VAS only load and unload action.  ", true, false);
                    return;
                }
            }
            if (arg_action == RobotAction.Exchange)
            {
                if (arg_tar != ActionTarget.Eq)
                {
                    CommonStaticData.PopForm("Exchange only EQ can use.", true, false);
                    return;
                }
            }

            log += "Action : " + arg_action.ToString() + "Robot id : " + robot_id + " Arm : " + arg_arm.ToString()
                + " Target : " + arg_tar.ToString() + " Target Id : " + id + " Slot : " + slot + Environment.NewLine;

            if ((arg_tar == ActionTarget.Eq) || (arg_tar == ActionTarget.Aligner && arg_action == RobotAction.PutGetAligner))
            {
                if (arg_tar == ActionTarget.Aligner && arg_action == RobotAction.PutGetAligner)
                {
                  //  UIController.g_Controller.SendRobotActionReq(robot_id, arg_action, arg_arm, arg_tar, id, slot, true, true, cb_robotActionDeg.Text.Trim());
                }
               // else
                  //  UIController.g_Controller.SendRobotActionReq(robot_id, arg_action, arg_arm, arg_tar, id, slot, true);
            }
            else
            {
              //  UIController.g_Controller.SendRobotActionReq(robot_id, arg_action, arg_arm, arg_tar, id, slot);
            }
            WriteLog(LogLevelType.UI, log);
            //WriteNormalOut
        }
        private void bt_PortExe_Click(object sender, EventArgs e)
        {
            /*
           //WriteNormalIn
            string log = "User press Port Action EXE" + Environment.NewLine;
            int port_id = cb_PortActionPortId.SelectedIndex + 1;
            int action = cb_PortActionName.SelectedIndex + 1;
            CommonData.PortAction arg_action = (CommonData.PortAction)action;
            log += " Port : " + port_id + " Action : " + arg_action.ToString() + Environment.NewLine;
            UIController.g_Controller.SendPortAction(port_id, arg_action);
            WriteLog(LogLevelType.UI, log);
            //WriteNormalOut
            */
        }
        private void bt_AlignerExe_Click(object sender, EventArgs e)
        {
            //WriteNormalIn
            /*
             * cb_AlignerId
               cb_AlignerDegree
               cb_AlignerAction
             * */
            double degree = 0;
            int id = 0;
            APIEnum.AlignerCommand command = APIEnum.AlignerCommand.None;
            string VacuumOn = "";
            if (!string.IsNullOrEmpty(cb_AlignerDegree.Text.Trim()))
            {
                degree = Convert.ToDouble(cb_AlignerDegree.Text.Trim());
            }
            if (!string.IsNullOrEmpty(cb_AlignerId.Text.Trim()))
            {
                id = Convert.ToInt16(cb_AlignerId.Text.Trim());
            }
            if (!string.IsNullOrEmpty(cb_AlignerAction.Text.Trim()))
            {
                command = (APIEnum.AlignerCommand)Enum.Parse(typeof(APIEnum.AlignerCommand), cb_AlignerAction.Text.Trim(), true);
            }
            if (!string.IsNullOrEmpty(cb_AlignerVacuum.Text.Trim()))
            {
                VacuumOn = (cb_AlignerVacuum.Text.Trim());
            }

            CommonData.HIRATA.CommandData command_obj = null;
            List<string> paras = new List<string>();

            switch (command)
            {
                case APIEnum.AlignerCommand.AlignerVacuum:
                    paras.Add(VacuumOn.ToString());
                    command_obj = new CommandData(APIEnum.CommandType.Aligner, command.ToString(), APIEnum.CommnadDevice.Aligner, 1, paras);
                    break;
                case APIEnum.AlignerCommand.Alignment:
                    paras.Add(degree.ToString());
                    command_obj = new CommandData(APIEnum.CommandType.Aligner, command.ToString(), APIEnum.CommnadDevice.Aligner, 1, paras);
                    break;
                case APIEnum.AlignerCommand.FindNotch:
                    command_obj = new CommandData(APIEnum.CommandType.Aligner, command.ToString(), APIEnum.CommnadDevice.Aligner, 1);
                    break;
                case APIEnum.AlignerCommand.GetAlignerDegree:
                    command_obj = new CommandData(APIEnum.CommandType.Aligner, command.ToString(), APIEnum.CommnadDevice.Aligner, 1);
                    break;
                case APIEnum.AlignerCommand.GetAlignerWaferType:
                    command_obj = new CommandData(APIEnum.CommandType.Aligner, command.ToString(), APIEnum.CommnadDevice.Aligner, 1);
                    break;
                case APIEnum.AlignerCommand.GetIDReaderDegree:
                    command_obj = new CommandData(APIEnum.CommandType.Aligner, command.ToString(), APIEnum.CommnadDevice.Aligner, 1);
                    break;
                case APIEnum.AlignerCommand.SetAlignerDegree:
                    paras.Add(degree.ToString());
                    command_obj = new CommandData(APIEnum.CommandType.Aligner, command.ToString(), APIEnum.CommnadDevice.Aligner, 1, paras);
                    break;
                case APIEnum.AlignerCommand.SetIDReaderDegree:
                    paras.Add(degree.ToString());
                    command_obj = new CommandData(APIEnum.CommandType.Aligner, command.ToString(), APIEnum.CommnadDevice.Aligner, 1, paras);
                    break;
                case APIEnum.AlignerCommand.ToAngle:
                    command_obj = new CommandData(APIEnum.CommandType.Aligner, command.ToString(), APIEnum.CommnadDevice.Aligner, 1);
                    break;
            };
            CommonData.HIRATA.MDApiCommand obj = new MDApiCommand();
            obj.CommandData = command_obj;
            string rtn;
            object tmp = null;
            uint ticket;
            /*
            if (!UiForm.cv_mmfController.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDApiCommand).Name, obj, out ticket, out rtn, out tmp, 3000))
            {
                CommonStaticData.PopForm("Manual API command time out!!", true, false);
            }
            */
            //WriteNormalOut
        }

        private void bt_PortExe_Click_1(object sender, EventArgs e)
        {/*cb_PortActionPortId
cb_PortActionName
cb_PortActionLed
cb_PortActionType
cb_PortActionSlotType
*/
            //WriteNormalIn
            int id = 0;
            int type = 0;// Convert.ToInt16(cb_PortActionType.Text.Trim());
            int slottype = 0; // Convert.ToInt16(cb_PortActionSlotType.Text.Trim());
            string led_control = ""; cb_PortActionLed.Text.Trim();
            APIEnum.LoadPortCommand command = APIEnum.LoadPortCommand.None;
            if (!string.IsNullOrEmpty(cb_PortActionPortId.Text.Trim()))
            {
                id = Convert.ToInt16(cb_PortActionPortId.Text.Trim());
            }
            if (!string.IsNullOrEmpty(cb_PortActionName.Text.Trim()))
            {
                command = (APIEnum.LoadPortCommand)Enum.Parse(typeof(APIEnum.LoadPortCommand), cb_PortActionName.Text.Trim(), true);
            }
            if (!string.IsNullOrEmpty(cb_PortActionLed.Text.Trim()))
            {
                led_control = (cb_PortActionLed.Text.Trim());
            }
            if (!string.IsNullOrEmpty(cb_PortActionType.Text.Trim()))
            {
                type = Convert.ToInt16(cb_PortActionType.Text.Trim());
            }
            if (!string.IsNullOrEmpty(cb_PortActionSlotType.Text.Trim()))
            {
                slottype = Convert.ToInt16(cb_PortActionType.Text.Trim());
            }

            CommonData.HIRATA.CommandData command_obj = null;
            List<string> paras = new List<string>();

            switch (command)
            {
                case APIEnum.LoadPortCommand.Unload:
                    command_obj = new CommandData(APIEnum.CommandType.LoadPort, command.ToString(), APIEnum.CommnadDevice.P, id);
                    break;
                case APIEnum.LoadPortCommand.LEDLoad:
                    paras.Add(led_control.Trim());
                    command_obj = new CommandData(APIEnum.CommandType.LoadPort, command.ToString(), APIEnum.CommnadDevice.P, id, paras);
                    break;
                case APIEnum.LoadPortCommand.LEDUnLoad:
                    paras.Add(led_control.Trim());
                    command_obj = new CommandData(APIEnum.CommandType.LoadPort, command.ToString(), APIEnum.CommnadDevice.P, id, paras);
                    break;
                case APIEnum.LoadPortCommand.SetOperatorAccessButton:
                    paras.Add(led_control.Trim());
                    command_obj = new CommandData(APIEnum.CommandType.LoadPort, command.ToString(), APIEnum.CommnadDevice.P, id, paras);
                    break;
                case APIEnum.LoadPortCommand.Map:
                    command_obj = new CommandData(APIEnum.CommandType.LoadPort, command.ToString(), APIEnum.CommnadDevice.P, id);
                    break;
                case APIEnum.LoadPortCommand.SetType:
                    paras.Add(type.ToString());
                    command_obj = new CommandData(APIEnum.CommandType.LoadPort, command.ToString(), APIEnum.CommnadDevice.P, id, paras);
                    break;
                case APIEnum.LoadPortCommand.UnClamp:
                    command_obj = new CommandData(APIEnum.CommandType.LoadPort, command.ToString(), APIEnum.CommnadDevice.P, id);
                    break;
                case APIEnum.LoadPortCommand.Clamp:
                    command_obj = new CommandData(APIEnum.CommandType.LoadPort, command.ToString(), APIEnum.CommnadDevice.P, id);
                    break;
                case APIEnum.LoadPortCommand.Load:
                    command_obj = new CommandData(APIEnum.CommandType.LoadPort, command.ToString(), APIEnum.CommnadDevice.P, id);
                    break;
                case APIEnum.LoadPortCommand.GetWaferSlot:
                    command_obj = new CommandData(APIEnum.CommandType.LoadPort, command.ToString(), APIEnum.CommnadDevice.P, id);
                    break;
                case APIEnum.LoadPortCommand.GetWaferSlot2:
                    command_obj = new CommandData(APIEnum.CommandType.LoadPort, command.ToString(), APIEnum.CommnadDevice.P, id);
                    break;
            };
            if (command != APIEnum.LoadPortCommand.None)
            {
                CommonData.HIRATA.MDApiCommand obj = new MDApiCommand();
                obj.CommandData = command_obj;
                string rtn;
                object tmp = null;
                uint ticket;
                /*
                if (!UiForm.cv_mmfController.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDApiCommand).Name, obj, out ticket, out rtn, out tmp, 3000))
                {
                    CommonStaticData.PopForm("Manual API command time out!!", true, false);
                }
                */
            }
            //WriteNormalOut
        }
        private void btn_HomeExe_Click(object sender, EventArgs e)
        {
            //cb_OcrAction
            APIEnum.OcrCommand command = APIEnum.OcrCommand.None;
            if (!string.IsNullOrEmpty(cb_OcrAction.Text.Trim()))
            {
                command = (APIEnum.OcrCommand)Enum.Parse(typeof(APIEnum.OcrCommand), cb_OcrAction.Text.Trim(), true);
            }
            CommonData.HIRATA.MDApiCommand obj = new MDApiCommand();

            CommonData.HIRATA.CommandData command_obj = null;

            command_obj = new CommandData(APIEnum.CommandType.OCR, command.ToString(), APIEnum.CommnadDevice.OCRReader, 1);
            obj.CommandData = command_obj;
            string rtn;
            object tmp = null;
            uint ticket;
            /*
            if (!UiForm.cv_mmfController.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDApiCommand).Name, obj, out ticket, out rtn, out tmp, 3000))
            {
                CommonStaticData.PopForm("Manual API command time out!!", true, false);
            }
            */
        }
        #endregion

        private void InitLogPage()
        {
            if (cv_LogPage == null)
            {
                cv_LogPage = new KLogPage();
            }
            cv_LogPage.Dock = DockStyle.Fill;
            cv_tpLog.Controls.Add(cv_LogPage);
        }
        private void InitRecipeForm()
        {
            if (cv_RecipeSetting == null)
            {
                cv_RecipeSetting = new RecipeSetting();
            }
            cv_RecipeSetting.Dock = DockStyle.Fill;
            cv_tpRecipe.Controls.Add(cv_RecipeSetting);
        }
        protected override void initLog()
        {
            base.initLog();
            cv_LogPage.AddMemoLog("UI", ref cv_Log);
            if (cv_BcMsgLog == null)
            {
                string enviPath = CommonData.HIRATA.CommonStaticData.g_RootLogsFolderPath + CommonData.HIRATA.CommonStaticData.g_FDModuleName;
                cv_BcMsgLog = new KMemoLog();
                cv_BcMsgLog.LoadFromIni(CommonData.HIRATA.CommonStaticData.g_ModuleLogsIniFile, "BCMSG");
                cv_BcMsgLog.LogFileName = enviPath + "\\BcMsg.log";
                cv_BcMsgLog.SaveToIni(CommonData.HIRATA.CommonStaticData.g_ModuleLogsIniFile, "BCMSG");
                cv_LogPage.AddMemoLog("BCMSG", ref cv_BcMsgLog);
            }
        }
        private void AddControllerLogToUI()
        {
            /*
            if (UIController != null)
            {
                cv_LogPage.AddMemoLog("Controller", ref UIController.cv_ControllerLog);
            }
            */
        }
        internal static Port GetPort(int m_Index)
        {
            Port rtn = null;
            if (cv_PortContainer.ContainsKey(m_Index))
            {
                rtn = cv_PortContainer[m_Index];
            }
            return rtn;
        }
        internal static Eq GetEq(int i)
        {
            Eq rtn = null;
            if (cv_EqContainer.ContainsKey(i))
            {
                rtn = cv_EqContainer[i];
            }
            return rtn;
        }
        internal static Robot GetRobot(int i)
        {
            Robot rtn = null;
            if (cv_RobotContainer.ContainsKey(i))
            {
                rtn = cv_RobotContainer[i];
            }
            return rtn;
        }
        internal static Buffer GetBuffer(int i)
        {
            Buffer rtn = null;
            if (cv_BufferContainer.ContainsKey(i))
            {
                rtn = cv_BufferContainer[i];
            }
            return rtn;
        }
        internal static Aligner GetAligner(int i)
        {
            Aligner rtn = null;
            if (cv_AlignerContainer.ContainsKey(i))
            {
                rtn = cv_AlignerContainer[i];
            }
            return rtn;
        }

        #region win32 API
        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        public const int WM_CLOSE = 0x10;

        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        #endregion


        private void DerivedOnTimer()
        {
            WriteLog(LogLevelType.TimerFunction, CommonData.HIRATA.CommonStaticData.__FUN(), CommonData.HIRATA.FunInOut.Enter);

            updateTimelbl();
            CheckInitializeColor();

            #region check pop form thread
            if (cv_threadPool.Count > 0)
            {
                foreach (ThreadObj item in cv_threadPool)
                {
                    if ((UInt64)SysUtils.MilliSecondsBetween(item.startTime, SysUtils.Now()) > item.timeout)
                    {
                        IntPtr ptr = FindWindow(null, item.title);
                        if (ptr != IntPtr.Zero)
                        {
                            //找到則關閉MessageBox視窗
                            PostMessage(ptr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                        }
                        cv_threadPool.Remove(item);
                        break;
                    }
                }
            }
            #endregion

            WriteLog(LogLevelType.TimerFunction, CommonData.HIRATA.CommonStaticData.__FUN(), CommonData.HIRATA.FunInOut.Leave);
        }

        #region timer function
        private void updateTimelbl()
        {
            WriteLog(LogLevelType.TimerFunction, CommonData.HIRATA.CommonStaticData.__FUN(), CommonData.HIRATA.FunInOut.Enter);
            string time = SysUtils.Now().DateTimeString();
            string[] tmp = time.Split(' ');
            lbl_date.Text = tmp[0];
            lbl_time.Text = tmp[1];
            WriteLog(LogLevelType.TimerFunction, CommonData.HIRATA.CommonStaticData.__FUN(), CommonData.HIRATA.FunInOut.Leave);
        }
        private void CheckInitializeColor()
        {
            if (UiForm.PSystemData.PInitaiizeOkRight)
            {
                if (btn_ReIni.BackColor != Color.Lime)
                    btn_ReIni.BackColor = Color.Lime;
            }
            else if (UiForm.PSystemData.PInitaiizingRight)
            {
                long diff = SysUtils.MilliSecondsBetween(SysUtils.Now(), cv_InitialButtonFlashTime);
                if (diff < 0)
                {
                    cv_InitialButtonFlashTime = SysUtils.Now();
                }
                if (diff > 2000)
                {
                    if (btn_ReIni.BackColor == Color.Lime)
                        btn_ReIni.BackColor = Color.Red;
                    else if (btn_ReIni.BackColor == Color.Red)
                        btn_ReIni.BackColor = Color.Lime;
                    else
                        btn_ReIni.BackColor = Color.Red;
                }
            }
            else
            {
                if (btn_ReIni.BackColor != Color.Red)
                    btn_ReIni.BackColor = Color.Red;
            }
        }
        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            this.notifyIcon.Visible = true;
        }
        ~UiForm()
        {
          //  cv_MmfController = null;
        }
        private void setVetsion()
        {
            this.lbl_Version.Text += FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion.ToString();
        }

        #region Icon
        private void InitIcon()
        {
            if (this.notifyIcon == null)
            {
                this.notifyIcon = new NotifyIcon();
                string ico_path = SysUtils.ExtractFileDir(SysUtils.GetExeName()) + "\\..\\Resources\\kgsIcon.ico";
                this.notifyIcon.Icon = new Icon(ico_path);
                this.Text = "Hirata Controller";
                this.notifyIcon.MouseDoubleClick += new MouseEventHandler(IconDoubleClick);
            }
        }
        private void IconDoubleClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.Show();
        }
        #endregion

        #region left tool bar
        private void CIMOFFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //WriteNormalIn
            string log = "User press Change to CIM OFF mode" + Environment.NewLine;
            if (UiForm.PSystemData.PSystemOnlineMode == OnlineMode.Control)
            {
                log += "Send change to CIM off msg";
                CommonData.HIRATA.MDOnlineRequest obj = new MDOnlineRequest();
                obj.PCurMode = OnlineMode.Control;
                obj.PChangeMode = OnlineMode.Offline;
                UIController.triggerUiEvent(typeof(CommonData.HIRATA.MDOnlineRequest).Name, obj);
            }
            else
            {
                log += "Cur CIM mode : " + UiForm.PSystemData.PSystemOnlineMode.ToString() + " reject user ask.";
            }
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }
        private void CIMONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //WriteNormalIn
            string log = "User press Change to CIM ON mode" + Environment.NewLine;
            if (UiForm.PSystemData.PSystemOnlineMode == OnlineMode.Offline)
            {
                log += "Send change to CIM ON msg";
                CommonData.HIRATA.MDOnlineRequest obj = new MDOnlineRequest();
                obj.PCurMode = OnlineMode.Offline;
                obj.PChangeMode = OnlineMode.Control;
                UIController.triggerUiEvent(typeof(CommonData.HIRATA.MDOnlineRequest).Name, obj);
            }
            else
            {
                log += "Cur CIM mode : " + UiForm.PSystemData.PSystemOnlineMode.ToString() + " reject user ask.";
            }
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut

        }


        private void ReInit_Click(object sender, EventArgs e  , enSideGroup m_Side)
        {

            string side = m_Side.ToString();
            //WriteNormalIn
            if (MessageBox.Show("Please adjust" + side + " robot vaccum if need first. if vaccum ok , press OK ", "Warning", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            if (UiForm.PSystemData.PapiConnect)
            {
                CommonStaticData.PopForm("Can't initialize because API disconnected!!!", false, false);
                return;
            }
            if (UiForm.PSystemData.PapiInlineMode != EquipmentInlineMode.Remote && UiForm.PSystemData.PapiInlineMode != EquipmentInlineMode.Local)
            {
                CommonStaticData.PopForm("Can't initialize because API inline mode Error , maybe re-start program can solve!!!", false, false);
                return;
            }
            if (m_Side == enSideGroup.Left && UiForm.PSystemData.POperationModeLeft != OperationMode.Manual)
            {
                CommonStaticData.PopForm("Can't initialize because " + side + " not at manual mode!!!", false, false);
                return;
            }
            if (m_Side == enSideGroup.Right && UiForm.PSystemData.POperationModeRight != OperationMode.Manual)
            {
                CommonStaticData.PopForm("Can't initialize because " + side + " not at manual mode!!!", false, false);
                return;
            }
            if (m_Side == enSideGroup.Both && (UiForm.PSystemData.POperationModeRight != OperationMode.Manual || UiForm.PSystemData.POperationModeLeft != OperationMode.Manual))
            {
                CommonStaticData.PopForm("Can't initialize because " + side + " not at manual mode!!!", false, false);
                return;
            }
            string log = "User Initialize " + Environment.NewLine;
            bool is_force = false;
            if (Control.ModifierKeys == Keys.Control)
            {
                is_force = true;
                log += "Use Force initialize. ";
            }
            //btn_ReIni.Enabled = false;

            AlarmData obj = new AlarmData();
            obj.cv_AlarmList.AddRange(new List<AlarmItem>(UiForm.cv_Alarms.cv_AlarmList));
            for (int i = 0; i < obj.cv_AlarmList.Count; i++)
            {
                obj.cv_AlarmList[i].PStatus = AlarmStatus.Clean;
            }
            UIController.triggerUiEvent(typeof(CommonData.HIRATA.AlarmData).Name, obj);
            CommonData.HIRATA.MDInitial initialize_obj = new MDInitial();
            initialize_obj.PAction = InitialAction.Initial;
            initialize_obj.PType = MmfEventClientEventType.etRequest;
            initialize_obj.cv_IsForce = is_force;
            initialize_obj.PSide = m_Side;
            UIController.triggerUiEvent(typeof(CommonData.HIRATA.MDInitial).Name, initialize_obj);

            //WriteNormalOut
        }
        private void Exit_Click(object sender, EventArgs e)
        {
            //WriteNormalIn
            string log = "User press exit" + Environment.NewLine;
            if (MessageBox.Show("Close the prograim ? ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.Yes)
            {
                log += "User press OK!" + Environment.NewLine;
              //  UIController.g_Controller = null;
                string strCmdText = "/c Stop.cmd";
                System.Diagnostics.Process.Start("CMD.exe", strCmdText);
                Environment.Exit(0);

            }
            else
            {
                log += "User press Cancel!" + Environment.NewLine;
            }
            WriteLog(LogLevelType.UI, log);
            //WriteNormalOut
        }
        private void btn_ShowManualIn_Click(object sender, EventArgs e)
        {
            /*
           //WriteNormalIn
            if (pan_Menual != null) return;
            pan_Menual = new Panel();
            pan_Menual.BackColor = Color.Red;
            pan_Menual.Parent = cv_tpLayout;
            pan_Menual.Controls.Add(gpb_RobotAction);
            //pan_Menual.Controls.Add(gpb_PortAction);
            //pan_Menual.Controls.Add(gpb_AlignerAction);
            pan_Menual.AutoSize = true;
            pan_Menual.Dock = DockStyle.Right;
            cv_tpLayout.Controls.Add(pan_Menual);
            //WriteNormalOut
            */
        }
        private void btn_ShowManualOut_Click(object sender, EventArgs e)
        {
            /*
            cv_tpManual.Controls.Add(gpb_RobotAction);
            //cv_tpManual.Controls.Add(gpb_PortAction);
            //cv_tpManual.Controls.Add(gpb_AlignerAction);
            pan_Menual = null;
            */
        }

        #endregion

        #region panel dragdrop
        private void setToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAllObjBackPanelColor();
            cv_Isdragdrop = true;
            //cv_PortContainer
        }
        private void ShowAllObjBackPanelColor()
        {
            foreach (Aligner item in cv_AlignerContainer.Values)
            {
                if (item.cv_IsShowUi)
                    item.ShowBackPanelColor();
            }

            foreach (Port port in cv_PortContainer.Values)
            {
                if (port.cv_IsShowUi)
                    port.ShowBackPanelColor();
            }
            foreach (Buffer buffer in cv_BufferContainer.Values)
            {
                if (buffer.cv_IsShowUi)
                    buffer.ShowBackPanelColor();
            }
            foreach (Eq eq in cv_EqContainer.Values)
            {
                if (eq.cv_IsShowUi)
                    eq.ShowBackPanelColor();
            }
            foreach (Robot robot in cv_RobotContainer.Values)
            {
                if (robot.cv_IsShowUi)
                    robot.ShowBackPanelColor();
            }
            if (cv_LotSummary.cv_IsShowUi)
                cv_LotSummary.ShowBackPanelColor();

            if (cv_StatusTable.cv_IsShowUi)
                cv_StatusTable.ShowBackPanelColor();

        }
        private void HideAllObjBackPanelColor()
        {
            foreach (Aligner item in cv_AlignerContainer.Values)
            {
                if (item.cv_IsShowUi)
                    item.HideBackPanelColor();
            }

            foreach (Port port in cv_PortContainer.Values)
            {
                if (port.cv_IsShowUi)
                    port.HideBackPanelColor();
            }
            foreach (Buffer buffer in cv_BufferContainer.Values)
            {
                if (buffer.cv_IsShowUi)
                    buffer.HideBackPanelColor();
            }
            foreach (Eq eq in cv_EqContainer.Values)
            {
                if (eq.cv_IsShowUi)
                    eq.HideBackPanelColor();
            }
            foreach (Robot robot in cv_RobotContainer.Values)
            {
                if (robot.cv_IsShowUi)
                    robot.HideBackPanelColor();
            }

            if (cv_LotSummary.cv_IsShowUi)
                cv_LotSummary.HideBackPanelColor();

            if (cv_StatusTable.cv_IsShowUi)
                cv_StatusTable.HideBackPanelColor();
        }
        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cv_Isdragdrop = false;
            KXmlItem tmp = new KXmlItem();
            tmp.LoadFromFile(CommonStaticData.g_LayoutPos);

            foreach (KeyValuePair<int, Eq> pair in cv_EqContainer)
            {
                if (pair.Value.cv_IsShowUi)
                    pair.Value.RecoverPanelUI(tmp.ItemsByName["EQ" + pair.Key.ToString()]);
            }
            foreach (KeyValuePair<int, Aligner> pair in cv_AlignerContainer)
            {
                if (pair.Value.cv_IsShowUi)
                    pair.Value.RecoverPanelUI(tmp.ItemsByName["ALIGNER" + pair.Key.ToString()]);
            }

            foreach (KeyValuePair<int, Port> pair in cv_PortContainer)
            {
                if (pair.Value.cv_IsShowUi)
                    pair.Value.RecoverPanelUI(tmp.ItemsByName["PORT" + pair.Key.ToString()]);
            }
            foreach (KeyValuePair<int, Buffer> pair in cv_BufferContainer)
            {
                if (pair.Value.cv_IsShowUi)
                    pair.Value.RecoverPanelUI(tmp.ItemsByName["BUFFER" + pair.Key.ToString()]);
            }

            foreach (KeyValuePair<int, Robot> pair in cv_RobotContainer)
            {
                if (pair.Value.cv_IsShowUi)
                    pair.Value.RecoverPanelUI(tmp.ItemsByName["ROBOT" + pair.Key.ToString()]);
            }

            if (cv_LotSummary.cv_IsShowUi)
                cv_LotSummary.RecoverPanelUI(tmp.ItemsByName["LOTS"].Items[0]);

            HideAllObjBackPanelColor();
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cv_Isdragdrop)
            {
                KXmlItem tmp = new KXmlItem();
                tmp.LoadFromFile(CommonStaticData.g_LayoutPos);
                cv_Isdragdrop = false;

                foreach (KeyValuePair<int, Aligner> pair in cv_AlignerContainer)
                {
                    tmp.ItemsByName["ALIGNER" + pair.Key.ToString()].Attributes["TOP"] = pair.Value.GetTop().ToString();
                    tmp.ItemsByName["ALIGNER" + pair.Key.ToString()].Attributes["LEFT"] = pair.Value.GetLeft().ToString();
                    tmp.ItemsByName["ALIGNER" + pair.Key.ToString()].Attributes["WIDTH"] = pair.Value.GetWidth().ToString();
                    tmp.ItemsByName["ALIGNER" + pair.Key.ToString()].Attributes["HEIGHT"] = pair.Value.GetHeight().ToString();
                }

                foreach (KeyValuePair<int, Eq> pair in cv_EqContainer)
                {
                    tmp.ItemsByName["EQ" + pair.Key.ToString()].Attributes["TOP"] = pair.Value.GetTop().ToString();
                    tmp.ItemsByName["EQ" + pair.Key.ToString()].Attributes["LEFT"] = pair.Value.GetLeft().ToString();
                    tmp.ItemsByName["EQ" + pair.Key.ToString()].Attributes["WIDTH"] = pair.Value.GetWidth().ToString();
                    tmp.ItemsByName["EQ" + pair.Key.ToString()].Attributes["HEIGHT"] = pair.Value.GetHeight().ToString();
                }
                foreach (KeyValuePair<int, Port> pair in cv_PortContainer)
                {
                    tmp.ItemsByName["PORT" + pair.Key.ToString()].Attributes["TOP"] = pair.Value.GetTop().ToString();
                    tmp.ItemsByName["PORT" + pair.Key.ToString()].Attributes["LEFT"] = pair.Value.GetLeft().ToString();
                    tmp.ItemsByName["PORT" + pair.Key.ToString()].Attributes["WIDTH"] = pair.Value.GetWidth().ToString();
                    tmp.ItemsByName["PORT" + pair.Key.ToString()].Attributes["HEIGHT"] = pair.Value.GetHeight().ToString();
                }
                foreach (KeyValuePair<int, Robot> pair in cv_RobotContainer)
                {
                    tmp.ItemsByName["ROBOT" + pair.Key.ToString()].Attributes["TOP"] = pair.Value.GetTop().ToString();
                    tmp.ItemsByName["ROBOT" + pair.Key.ToString()].Attributes["LEFT"] = pair.Value.GetLeft().ToString();
                    tmp.ItemsByName["ROBOT" + pair.Key.ToString()].Attributes["WIDTH"] = pair.Value.GetWidth().ToString();
                    tmp.ItemsByName["ROBOT" + pair.Key.ToString()].Attributes["HEIGHT"] = pair.Value.GetHeight().ToString();
                }
                foreach (KeyValuePair<int, Buffer> pair in cv_BufferContainer)
                {
                    tmp.ItemsByName["BUFFER" + pair.Key.ToString()].Attributes["TOP"] = pair.Value.GetTop().ToString();
                    tmp.ItemsByName["BUFFER" + pair.Key.ToString()].Attributes["LEFT"] = pair.Value.GetLeft().ToString();
                    tmp.ItemsByName["BUFFER" + pair.Key.ToString()].Attributes["WIDTH"] = pair.Value.GetWidth().ToString();
                    tmp.ItemsByName["BUFFER" + pair.Key.ToString()].Attributes["HEIGHT"] = pair.Value.GetHeight().ToString();
                }

                tmp.ItemsByName["LOT"].Attributes["TOP"] = cv_LotSummary.GetTop().ToString();
                tmp.ItemsByName["LOT"].Attributes["LEFT"] = cv_LotSummary.GetLeft().ToString();
                tmp.ItemsByName["LOT"].Attributes["WIDTH"] = cv_LotSummary.GetWidth().ToString();
                tmp.ItemsByName["LOT"].Attributes["HEIGHT"] = cv_LotSummary.GetHeight().ToString();

                tmp.ItemsByName["STATUS1"].Attributes["TOP"] = cv_StatusTable.GetTop().ToString();
                tmp.ItemsByName["STATUS1"].Attributes["LEFT"] = cv_StatusTable.GetLeft().ToString();
                tmp.ItemsByName["STATUS1"].Attributes["WIDTH"] = cv_StatusTable.GetWidth().ToString();
                tmp.ItemsByName["STATUS1"].Attributes["HEIGHT"] = cv_StatusTable.GetHeight().ToString();

                tmp.SaveToFile(CommonStaticData.g_LayoutPos);
                HideAllObjBackPanelColor();
            }
        }
        #endregion

        private void reFreshAccountGrid()
        {
            cv_dataViewAccount.DataSource = UiForm.cv_AccountData.cv_AccountList;
            cv_dataViewAccount.Refresh();
        }
        private void InitAllSubForm()
        {
            if (cv_LoginForm == null)
            {
                cv_LoginForm = new Login();
            }
        }
        protected void layoutInit()
        {
            int eq_number = CommonData.HIRATA.CommonStaticData.g_EqNumber;
            int port_number = CommonData.HIRATA.CommonStaticData.g_PortNumber;
            int robot_number = CommonData.HIRATA.CommonStaticData.g_RobotNumber;
            int buffer_number = CommonData.HIRATA.CommonStaticData.g_BufferNumber;
            int aligner_number = CommonData.HIRATA.CommonStaticData.g_AlignerNumber;

            KXmlItem pos = new KXmlItem();
            pos.LoadFromFile(CommonStaticData.g_LayoutPos);

            for (int i = 0; i < eq_number; ++i)
            {
                int eq_no = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_EqXml.Items[i].Attributes["ID"].Trim());
                int max_slot = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_EqXml.Items[i].Attributes["Capacity"].Trim());
                int stage = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_EqXml.Items[i].Attributes["Stage"].Trim());
                int node = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_EqXml.Items[i].Attributes["Node"].Trim());
                int time_chart = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_EqXml.Items[i].Attributes["TimeChat"].Trim());
                string tool_id = CommonData.HIRATA.CommonStaticData.g_EqXml.Items[i].Attributes["ToolId"].Trim();

                Eq eq_control = new Eq(eq_no, node, stage, time_chart, max_slot, tool_id);

                if (Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_EqXml.Items[i].Attributes["Show"].Trim()) == 1)
                {
                    eq_control.InitPanelUI(cv_tpLayout, pos.ItemsByName["EQ" + eq_no.ToString()]);
                    eq_control.cv_IsShowUi = true;
                }
                cv_EqContainer.Add(eq_no, eq_control);
            }
            for (int i = 0; i < aligner_number; ++i)
            {
                int aliner_no = i + 1;
                int max_slot = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_AlignerXml.Items[i].Attributes["Capacity"].Trim());
                Aligner aligner_control = new Aligner(aliner_no, max_slot);
                if (Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_AlignerXml.Items[i].Attributes["Show"].Trim()) == 1)
                {
                    aligner_control.InitPanelUI(cv_tpLayout, pos.ItemsByName["ALIGNER" + aliner_no.ToString()]);
                    aligner_control.cv_IsShowUi = true;
                }
                cv_AlignerContainer.Add(aliner_no, aligner_control);
            }

            for (int i = 0; i < port_number; ++i)
            {
                int max_slot = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_PortXml.Items[i].Attributes["Capacity"].Trim());
                int port_no = i + 1;
                Port port_control = new Port(port_no, max_slot);
                if (Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_PortXml.Items[i].Attributes["Show"].Trim()) == 1)
                {
                    port_control.InitPanelUI(cv_tpLayout, pos.ItemsByName["PORT" + port_no.ToString()]);
                    port_control.cv_IsShowUi = true;
                }
                cv_PortContainer.Add(port_no, port_control);

                cv_SummaryPortData.Add(port_control.cv_Data);
            }

            for (int i = 0; i < buffer_number; ++i)
            {
                int max_slot = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_BufferXml.Items[i].Attributes["Capacity"].Trim());
                int buffer_no = i + 1;
                Buffer buffer_control = new Buffer(buffer_no, max_slot);
                if (Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_BufferXml.Items[i].Attributes["Show"].Trim()) == 1)
                {
                    buffer_control.InitPanelUI(cv_tpLayout, pos.ItemsByName["BUFFER" + buffer_no.ToString()]);
                    buffer_control.cv_IsShowUi = true;
                }
                cv_BufferContainer.Add(i + 1, buffer_control);
            }

            for (int i = 0; i < robot_number; ++i)
            {
                int max_slot = Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_RobotXml.Items[i].Attributes["Capacity"].Trim());
                int robot_no = i + 1;
                Robot robot_control = new Robot(robot_no, max_slot, GUI.RobotUI.RobotGlassShape.rgsCircle, GUI.RobotUI.RobotGlassShape.rgsCircle);
                if (Convert.ToInt16(CommonData.HIRATA.CommonStaticData.g_RobotXml.Items[i].Attributes["Show"].Trim()) == 1)
                {
                    robot_control.InitPanelUI(cv_tpLayout, pos.ItemsByName["ROBOT" + robot_no.ToString()]);
                    robot_control.cv_IsShowUi = true;
                }
                cv_RobotContainer.Add(robot_no, robot_control);
            }

            cv_StatusTable = new Status(0, 0);
            cv_StatusTable.InitPanelUI(cv_tpLayout, pos.ItemsByName["STATUS1"]);
            cv_StatusTable.cv_IsShowUi = true;

            cv_LotSummary = new LotSummary(0, 0, cv_SummaryPortData);
            cv_LotSummary.InitPanelUI(cv_tpLayout, pos.ItemsByName["LOTS"].Items[0]);
            cv_LotSummary.cv_IsShowUi = true;

        }
        void setFormTitle(string m_RobotVersion = "")
        {
            //WirteIn
            string log = "";
            this.Text = CommonData.HIRATA.CommonStaticData.g_ToolId;
            this.Text += " Version : " + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion.ToString();
            if (m_RobotVersion != "")
            {
                this.Text += "  Robot : " + m_RobotVersion;
            }
            log += "Version : " + this.Text + Environment.NewLine;


            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }
        private void initMemoryIoUI()
        {
            /*
            string log = "";
            if (File.Exists(CommonData.HIRATA.CommonStaticData.g_SysMemoryIoClientFile))
            {
                KXmlItem tmp = new KXmlItem();
                tmp.LoadFromFile(CommonData.HIRATA.CommonStaticData.g_SysMemoryIoClientFile);
                int xml_number = tmp.ItemsByName["Bits"].ItemNumber;
                int row = (int)Math.Ceiling(((float)xml_number / 2));
                cv_dataViewIo.EnableHeadersVisualStyles = false;
                cv_dataViewIo.Rows.Add(row);
                string tmp_name = "";
                for (int i = 0, j = 0; i <= row; i++, j += 2)
                {
                    tmp_name = tmp.ItemsByName["Bits"].Items[j].Attributes["Name"];
                    cv_dataViewIo.Rows[i].Cells[0].Value = tmp_name;
                    tmp_name = tmp.ItemsByName["Bits"].Items[j].Attributes["Port"];
                    cv_dataViewIo.Rows[i].Cells[1].Value = tmp_name;
                    cv_dataViewIo.Rows[i].Cells[2].Value = "0";
                    cv_dataViewIo.Rows[i].Cells[2].Style.BackColor = Color.Gray;
                    if (i != xml_number)
                    {
                        tmp_name = tmp.ItemsByName["Bits"].Items[j + 1].Attributes["Name"];
                        cv_dataViewIo.Rows[i].Cells[3].Value = tmp_name;
                        tmp_name = tmp.ItemsByName["Bits"].Items[j].Attributes["Port"];
                        cv_dataViewIo.Rows[i].Cells[4].Value = tmp_name;
                        cv_dataViewIo.Rows[i].Cells[5].Value = "0";
                        cv_dataViewIo.Rows[i].Cells[5].Style.BackColor = Color.Gray;
                    }
                }
            }
            else
            {
                log += "MemoryIO file not found " + Environment.NewLine;
            }
            WriteLog(LogLevelType.General, log);
            */
        }
        private void refreshIoTable()
        {
            /*
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            int row = cv_dataViewIo.RowCount;
            for (int i = 0; i < row; i++)
            {
                if (cv_Mio.GetPortValueByName(cv_dataViewIo.Rows[i].Cells[0].Value.ToString().Trim()) == 0)
                {
                    if (cv_dataViewIo.Rows[i].Cells[2].Value.ToString() != "0")
                    {
                        cv_dataViewIo.Rows[i].Cells[2].Value = "0";
                        cv_dataViewIo.Rows[i].Cells[2].Style.BackColor = Color.Gray;
                    }
                }
                else
                {
                    if (cv_dataViewIo.Rows[i].Cells[2].Value.ToString() != "1")
                    {
                        cv_dataViewIo.Rows[i].Cells[2].Value = "1";
                        cv_dataViewIo.Rows[i].Cells[2].Style.BackColor = Color.Lime;
                    }
                }
                if (cv_Mio.GetPortValueByName(cv_dataViewIo.Rows[i].Cells[3].Value.ToString().Trim()) == 0)
                {
                    if (cv_dataViewIo.Rows[i].Cells[5].Value.ToString() != "0")
                    {
                        cv_dataViewIo.Rows[i].Cells[5].Value = "0";
                        cv_dataViewIo.Rows[i].Cells[5].Style.BackColor = Color.Gray;
                    }
                }
                else
                {
                    if (cv_dataViewIo.Rows[i].Cells[5].Value.ToString() != "1")
                    {
                        cv_dataViewIo.Rows[i].Cells[5].Value = "1";
                        cv_dataViewIo.Rows[i].Cells[5].Style.BackColor = Color.Lime;
                    }
                }
            }
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
            */
        }
        private void cv_dataViewAccount_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex % 2 == 0)
            {
                cv_dataViewAccount.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.PaleTurquoise;
                //cv_dataViewAccount.Rows[e.RowIndex].Cells[1].Style.BackColor = Color.PaleTurquoise;
            }
        }
        private void AssignAppEvent()
        {
            //WriteNormalIn
            //common 
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDOnlineRequest).Name, ProcessOnlineReq);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDOperationModeChangeRight).Name, ProcessOperatorModeChange);

            cv_AppEventMap.Add(typeof(CommonData.HIRATA.PortData).Name, ProcessPortData);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.RobotData).Name, ProcessRobotData);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.BufferData).Name, ProcessBufferData);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.AlignerData).Name, ProcessAlignerData);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.EqData).Name, ProcessEqData);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.RecipeData).Name, ProcessRecipeData);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.AlarmData).Name, ProcessAlarmNoti);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDInitial).Name, ProcessInit);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDPopOpidForm).Name, ProcessPopOpidFormReq);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDPopMonitorForm).Name, ProcessPopMonitorFormReq);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDRobotAction).Name, PrcessRobotAction);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.SystemData).Name, ProcessSystemData);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.TimeOutData).Name, ProcessTimeOutData);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.GlassCountData).Name, ProcessGlassCountData);

            //by case 
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDBCMsg).Name, ProcessBcMsg);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDEfemStatus).Name, ProcessEfemStatus);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDEfemStatusSingle).Name, ProcessEfemStatusSingle);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDTimeChartChange).Name, ProcessTimeChartStepChange);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDRobotjobPath).Name, ProcessRobotJobPathChange);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDChangePortSlotType).Name, ProcessChangePortSlotType);
            cv_AppEventMap.Add(typeof(CommonData.HIRATA.MDShowOcrDecide).Name, ProcessShowOcrDecide);
            //WriteNormalOut
        }

        #region Bc msg
        void ProcessBcMsg(string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            string log = "";
            CommonData.HIRATA.MDBCMsg obj = m_Object as CommonData.HIRATA.MDBCMsg;
            if (obj.PMsg != "")
            {
                if (obj.PMsgType == BcMsgType.Continue)
                    CommonStaticData.PopForm(obj.PMsg, false, false);
                else if (obj.PMsgType == BcMsgType.Interval)
                {
                    CommonStaticData.PopForm(obj.PMsg, true, false, (uint)obj.PIntervalSec);
                }
                if (obj.PBuzzer)
                {
                    CommandData cmd = new CommandData();
                    List<string> para = new List<string>();
                    para.Add("0");
                    cmd = new CommandData(APIEnum.CommandType.IO, APIEnum.IoCommand.Buzzer.ToString(), APIEnum.CommnadDevice.IO, 0, para);
                    CommonData.HIRATA.MDApiCommand bz_obj = new MDApiCommand();
                    bz_obj.CommandData = cmd;
                    string rtn;
                    object tmp = null;
                    uint ticket;
                    /*
                    if (!Global.Controller.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDApiCommand).Name, bz_obj, out ticket, out rtn, out tmp, 3000))
                    {
                        CommonStaticData.PopForm("Manual API command time out!!", true, false);
                    }
                    */
                }
                cv_BcMsgLog.WriteLog("[BC MSG]\n" + obj.PMsg);
            }
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }
        #endregion

        #region process MMF Event
        void ProcessShowOcrDecide(string m_MessageId, object m_Object)
        {
            if (cv_OcrDecide != null)
            {
                if (cv_OcrDecide.Visible)
                {
                    cv_OcrDecide.Focus();
                }
                else
                {
                    cv_OcrDecide.Show();
                }
            }
            else
            {
                cv_OcrDecide = new OcrDecide();
                cv_OcrDecide.Show();
            }
        }
        void ProcessGlassCountData(string m_MessageId, object m_Object)
        {
            cv_StatusTable.GetUI().SetGlassCount();
        }
        void ProcessChangePortSlotType(string m_MessageId, object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CommonData.HIRATA.MDChangePortSlotType obj = m_Object as CommonData.HIRATA.MDChangePortSlotType;
            Port port = GetPort(obj.PPortId);
            int type = obj.PSlotType;
            if (obj.PResult == Result.NG)
            {
                CommonStaticData.PopForm("Can't change port slot type", true, false);
            }
            if (type == 0)
            {
                if (type == port.cv_Data.PEfemPortType)
                {
                    if (cv_PortContainer.ContainsKey(obj.PPortId))
                    {
                        port.cv_SlotCount = 25;
                        port.cv_Data.cv_SlotCount = 25;
                        (port.cv_Ui as GUI.PortUI).ResetDataVier(25);
                        (port.cv_Ui as GUI.PortUI).SetSlotButton(true);
                    }
                }
            }
            else if (type == 4)
            {
                if (type == port.cv_Data.PEfemPortType)
                {
                    if (cv_PortContainer.ContainsKey(obj.PPortId))
                    {
                        port.cv_SlotCount = 13;
                        port.cv_Data.cv_SlotCount = 13;
                        (port.cv_Ui as GUI.PortUI).ResetDataVier(13);
                        (port.cv_Ui as GUI.PortUI).SetSlotButton(false);
                    }
                }
            }
            WriteLog(LogLevelType.NormalFunctionInOut, "BaseMmfController." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        void ProcessRobotJobPathChange(string m_MessageId, object m_Object)
        {
            /*
            CommonData.HIRATA.MDRobotjobPath obj = m_Object as MDRobotjobPath;
            if (cv_RobotAutoJobPathTable != null)
            {
                cv_RobotAutoJobPathTable.Refresh(obj.RobotJob);
            }
            */
        }
        void ProcessTimeChartStepChange(string m_MessageId, object m_Object)
        {
            if (cv_TimeChartStep == null)
            {
                cv_TimeChartStep = new Dictionary<EqGifTimeChartId, KeyValuePair<int, string>>();
            }
            CommonData.HIRATA.MDTimeChartChange obj = m_Object as CommonData.HIRATA.MDTimeChartChange;
            for (int i = 0; i < obj.cv_Steps.Count; i++)
            {
                EqGifTimeChartId id = (EqGifTimeChartId)(i + 1);
                KeyValuePair<int, string> pair = new KeyValuePair<int, string>(obj.cv_Steps[i], obj.cv_StepsName[i]);
                cv_TimeChartStep[id] = pair;
            }
        }
        void ProcessEfemStatusSingle(string m_MessageId, object m_Object)
        {
            StatusTable table = cv_StatusTable.GetUI();
            if (table != null)
            {
                CommonData.HIRATA.MDEfemStatusSingle obj = m_Object as CommonData.HIRATA.MDEfemStatusSingle;
                table.ProcessEfemEventSingle(obj.PStatusType, obj.PValue);
            }
        }
        void ProcessEfemStatus(string m_MessageId, object m_Object)
        {
            StatusTable table = cv_StatusTable.GetUI();
            if (table != null)
            {
                CommonData.HIRATA.MDEfemStatus obj = m_Object as CommonData.HIRATA.MDEfemStatus;
                table.ProcessEfemEvent(obj);
            }
        }

        void ProcessTimeOutData(string m_MessageId, object m_Object)
        {
            cv_TimeOutForm.Set();
        }
        void ProcessSystemData(string m_MessageId, object m_Object)
        {
            
            //WriteNormalIn
            CommonData.HIRATA.SystemData obj = m_Object as CommonData.HIRATA.SystemData;
            string log = "Recv : " + typeof(CommonData.HIRATA.SystemData).Name + Environment.NewLine;
            log += "Bc Alive : " + obj.PBcAlive.ToString() + Environment.NewLine;
            log += "System Status : " + obj.PSystemStatus.ToString() + Environment.NewLine;
            log += "PLC connected : " + obj.PPlcConnect.ToString() + Environment.NewLine;
            log += "Operation Mode : " + obj.POperationModeLeft.ToString() + Environment.NewLine;
            log += "api connect  : " + obj.PapiConnect.ToString() + Environment.NewLine;
            log += "Robot version  : " + obj.PapiVersion.ToString() + Environment.NewLine;
            log += "api remote  : " + obj.PapiInlineMode.ToString() + Environment.NewLine;
            log += "Data Check Rule  : " + obj.PDataCheckRule.ToString("X4") + Environment.NewLine;
            log += "OCR mode  : " + obj.POcrMode.ToString() + Environment.NewLine;
            log += "Initialize OK  : " + obj.PInitaiizeOkRight.ToString() + Environment.NewLine;
            log += CommonData.HIRATA.CommonStaticData.g_SplitLine + Environment.NewLine;

            //cv_btnSelectMode.Enabled = true;
            Color tmp_color = Color.Gray;
            switch (obj.PSystemStatus)
            {
                case EquipmentStatus.Down:
                    lbl_systemStatus.Text = "DOWN";
                    lbl_systemStatus.BackColor = Color.Red;
                    break;
                case EquipmentStatus.Idle:
                    lbl_systemStatus.Text = "IDLE";
                    lbl_systemStatus.BackColor = Color.Yellow;
                    break;
                case EquipmentStatus.Run:
                    lbl_systemStatus.Text = "RUN";
                    lbl_systemStatus.BackColor = Color.Lime;
                    break;
            };

            if (obj.PSystemOnlineMode == OnlineMode.Offline || obj.PSystemOnlineMode == OnlineMode.None)
            {
                lbl_SystemMode.Text = "OFF";
                lbl_SystemMode.BackColor = Color.Red;
            }
            else
            {
                lbl_SystemMode.Text = "ON";
                lbl_SystemMode.BackColor = Color.Lime;
            }

            cv_RecipeCheckForm.SetNodeChecked();

            if (lbl_Ocr.Text != obj.POcrMode.ToString())
            {
                lbl_Ocr.Text = obj.POcrMode.ToString();
            }

            if (lbl_RobotInline.Text != obj.PapiInlineMode.ToString())
            {
                lbl_RobotInline.Text = obj.PapiInlineMode.ToString();
            }
            if (cv_lblVersion.Text != obj.PapiVersion)
            {
                cv_lblVersion.Text = obj.PapiVersion;
            }
            if (obj.PPlcConnect)
            {
                lbl_plcStatus.BackColor = Color.Lime;
                lbl_plcStatus.Text = "Connected";
            }
            else
            {
                lbl_plcStatus.BackColor = Color.Red;
                lbl_plcStatus.Text = "Disconnected";
            }
            if (obj.PBcAlive)
            {
                lbl_hsmsStatus.BackColor = Color.Lime;
                lbl_hsmsStatus.Text = "Connected";

            }
            else
            {
                lbl_hsmsStatus.BackColor = Color.Red;
                lbl_hsmsStatus.Text = "Disconnected";
            }
            if (obj.PapiConnect)
            {
                lbl_RobotConnect.Text = "Connect";
                lbl_RobotConnect.BackColor = Color.Lime;
            }
            else
            {
                lbl_RobotConnect.Text = "Dis";
                lbl_RobotConnect.BackColor = Color.Red;
            }
            //ont mode


            //Auto / manual button
            if (obj.POperationModeLeft == OperationMode.Auto)
            {
                if (cv_SystemAuto.BackColor != Color.Lime)
                {
                    cv_SystemAuto.BackColor = Color.Lime;
                }
            }
            else
            {
                if (cv_SystemAuto.BackColor != Color.Gray)
                {
                    cv_SystemAuto.BackColor = Color.Gray;
                }
            }
            if (obj.POperationModeLeft.ToString() != cv_SystemAuto.Text)
            {
                cv_SystemAuto.Text = obj.POperationModeLeft.ToString().Trim();
            }

            if (obj.PRobot1Speed.ToString() != lbl_RobotSpeed.Text.Trim())
            {
                lbl_RobotSpeed.Text = obj.PRobot1Speed.ToString();
            }

            if (obj.PFFUSpeed.ToString() != lbl_FFUSpeed.Text.Trim())
            {
                lbl_FFUSpeed.Text = obj.PFFUSpeed.ToString();
            }


            GetRobot(1).reFresh();
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }
        void ProcessOnlineReq(string m_MessageId, object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            string log = "";
            CommonData.HIRATA.MDOnlineRequest obj = m_Object as CommonData.HIRATA.MDOnlineRequest;
            log += "Cur Mode :" + obj.PCurMode.ToString() + " Change Mode : " + obj.PChangeMode.ToString() + Environment.NewLine;

            //au require online from tcs.
            if (obj.PType == MmfEventClientEventType.etRequest)
            {
                LockOnlineButton(true);
            }
            else if (obj.PType == MmfEventClientEventType.etNotify || obj.PType == MmfEventClientEventType.etReply)
            {
                LockOnlineButton(false);
            }
            WriteLog(LogLevelType.General, log);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void ProcessOperatorModeChange(string m_MessageId, object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CommonData.HIRATA.MDOperationModeChangeRight obj = m_Object as CommonData.HIRATA.MDOperationModeChangeRight;
            if (obj.PType == MmfEventClientEventType.etReply || obj.PType == MmfEventClientEventType.etNotify)
            {
                LockOperationButton(false);
            }
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void ProcessLoginOut(string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            string log = "";
            CommonData.HIRATA.MDLogInOut obj = m_Object as CommonData.HIRATA.MDLogInOut;
            log += "Log in / out :" + obj.PAction.ToString() + Environment.NewLine;

            if (obj.PAction == LogInOut.Login)
            {
                AccountItem tmp = null;
                UiForm.cv_AccountData.GetCurAccount(out tmp);
                lbl_AccountId.Text = tmp.PId;
                lbl_AccountPm.Text = tmp.PPermission.ToString();
            }
            else
            {
                lbl_AccountPm.Text = "";
                lbl_AccountId.Text = "";
            }
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }
        void ProcessAlignerData(string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            string log = "";
            CommonData.HIRATA.AlignerData obj = m_Object as CommonData.HIRATA.AlignerData;
            UI.Aligner aligner = GetAligner((int)obj.PId);
            aligner.cv_Data = null;
            aligner.cv_Data = obj;
            aligner.cv_Data.GlassDataList = obj.cv_GlassDataList;
            aligner.reFresh();
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }
        void ProcessBufferData(string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            string log = "";
            CommonData.HIRATA.BufferData obj = m_Object as CommonData.HIRATA.BufferData;
            UI.Buffer buffer = GetBuffer((int)obj.PId);
            buffer.cv_Data = null;
            buffer.cv_Data = obj;
            buffer.cv_Data.GlassDataList = obj.cv_GlassDataList;
            buffer.reFresh();
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }
        void ProcessRobotData(string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            string log = "";
            CommonData.HIRATA.RobotData obj = m_Object as CommonData.HIRATA.RobotData;
            UI.Robot robot = GetRobot((int)obj.PId);
            robot.cv_Data = null;
            robot.cv_Data = obj;
            robot.cv_Data.GlassDataList = obj.cv_GlassDataList;
            robot.reFresh();
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }
        void ProcessPortData(string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            string log = "";
            CommonData.HIRATA.PortData obj = m_Object as CommonData.HIRATA.PortData;
            UI.Port port = GetPort((int)obj.PId);
            port.cv_Data = null;
            cv_SummaryPortData[(int)obj.PId - 1] = null;
            port.cv_Data = obj;
            port.cv_Data.GlassDataList = obj.cv_GlassDataList;
            port.reFresh();
            cv_SummaryPortData[(int)obj.PId - 1] = GetPort((int)obj.PId).cv_Data;
            cv_LotSummary.reFresh();
            (port.cv_Ui as GUI.PortUI).SetSlotButton(port.cv_Data.PEfemPortType == 0 ? true : false);
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }
        void ProcessEqData(string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            string log = "";
            CommonData.HIRATA.EqData obj = m_Object as CommonData.HIRATA.EqData;
            UI.Eq eq = GetEq((int)obj.PId);
            eq.cv_Data = null;
            eq.cv_Data = obj;
            eq.cv_Data.GlassDataList = obj.cv_GlassDataList;
            eq.reFresh();
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }
        void ProcessRecipeData(string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            CommonData.HIRATA.RecipeData obj = m_Object as CommonData.HIRATA.RecipeData;
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    cv_RecipeSetting.cv_RecipeDataView.DataSource = obj.cv_RecipeList;
                    cv_RecipeSetting.UpdateCurRecipeCombobox();
                    RecipeItem recipe = new RecipeItem();
                    if (obj.GetCurRecipe(out recipe))
                    {
                        if (lbl_CurRecipe.Text != recipe.PId.Trim())
                        {
                            lbl_CurRecipe.Text = recipe.PId.Trim();
                        }
                        if (lbl_CurFlow.Text != recipe.PFlow.ToString())
                        {
                            lbl_CurFlow.Text = recipe.PFlow.ToString();
                        }
                    }
                }));
            }
            else
            {
                cv_RecipeSetting.cv_RecipeDataView.DataSource = obj.cv_RecipeList;
                cv_RecipeSetting.UpdateCurRecipeCombobox();
                RecipeItem recipe = new RecipeItem();
                if (obj.GetCurRecipe(out recipe))
                {
                    if (lbl_CurRecipe.Text != recipe.PId.Trim())
                    {
                        lbl_CurRecipe.Text = recipe.PId.Trim();
                    }
                    if (lbl_CurFlow.Text != recipe.PFlow.ToString())
                    {
                        lbl_CurFlow.Text = recipe.PFlow.ToString();
                    }
                }
            }
            //WriteNormalOut
        }
        void ProcessAlarmNoti(string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    cv_AlarmDataView.DataSource = UiForm.cv_Alarms.cv_AlarmList;
                }));
            }
            else
            {
                cv_AlarmDataView.DataSource = UiForm.cv_Alarms.cv_AlarmList;
            }

            if (UiForm.cv_Alarms.IsHasAlarm() && cv_tcBar.SelectedTab != cv_tpAlarm)
            {
                cv_tcBar.SelectedTab = cv_tpAlarm;
            }

            if (UiForm.cv_Alarms.IsHasAlarm()) lbl_alarmStatus.BackColor = Color.Red;
            else
                lbl_alarmStatus.BackColor = SystemColors.ActiveCaption;
            if (UiForm.cv_Alarms.IsHasWarning()) lbl_warningStatus.BackColor = Color.Red;
            else
                lbl_warningStatus.BackColor = SystemColors.ActiveCaption;

            //WriteNormalOut

        }
        void ProcessInit(string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            string log = "";
            CommonData.HIRATA.MDInitial obj = m_Object as CommonData.HIRATA.MDInitial;
            if (obj.PType == MmfEventClientEventType.etReply)
            {
                if (obj.PResult == Result.NG)
                {
                    btn_ReIni.Enabled = true;
                }
            }
            else if (obj.PType == MmfEventClientEventType.etNotify)
            {
                if (obj.PAction == InitialAction.Initial)
                {
                    btn_ReIni.Enabled = false;
                }
                else if (obj.PAction == InitialAction.Complete)
                {
                    btn_ReIni.Enabled = true;
                }
            }
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }
        void ProcessPopOpidFormReq(string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            string log = "";
            CommonData.HIRATA.MDPopOpidForm obj = new CommonData.HIRATA.MDPopOpidForm();

            if (obj.PortId > 0 && obj.PortId < CommonData.HIRATA.CommonStaticData.g_PortNumber)
            {
                Port tmp = UiForm.GetPort(obj.PortId);
                tmp.ShowMgvForm();
            }
            else
            {
                log += "Port over range" + Environment.NewLine;
            }
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }
        void ProcessPopMonitorFormReq(string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            string log = "";
            CommonData.HIRATA.MDPopMonitorForm obj = m_Object as CommonData.HIRATA.MDPopMonitorForm;
            if (obj.PortId > 0 && obj.PortId <= CommonData.HIRATA.CommonStaticData.g_PortNumber)
            {
                Port tmp = UiForm.GetPort(obj.PortId);
                tmp.ShowCstDataConfirm();
            }
            else
            {
                log += "Port over range" + Environment.NewLine;
            }
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }

        void PrcessRobotAction(string m_MessageId, object m_Object)
        { //WriteNormalIn 
            string log = ""; CommonData.HIRATA.MDRobotAction obj = m_Object as CommonData.HIRATA.MDRobotAction;
            log += "Robot RobotId : " + obj.RobotId + " Arm : " + obj.Source.PArm.ToString() + " Action : " + obj.PAction.ToString() +
                " Target : " + obj.Source.PTarget.ToString() + " TargetId : " + obj.Source.Id + " TargetSlot : " + obj.Source.Slot + Environment.NewLine;
            WriteLog(LogLevelType.General, log);
            if (obj.PType == MmfEventClientEventType.etNotify)
            {
                DisplayRobotAction(obj);
            }
            else if (obj.PType == MmfEventClientEventType.etReply)
            {
                if (obj.PResult == CommonData.HIRATA.Result.OK)
                {
                    DisplayRobotAction(obj);
                }
                else
                {
                    CommonStaticData.PopForm("Robot Action reply NG!", true, false);
                    log += "Robot Action reply NG";

                }
            }
            //WriteNormalOut
        }
        void DisplayRobotAction(MDRobotAction m_obj)
        {
            if (m_obj.PAction != RobotAction.ActionComplete)
            {
                lbl_RbActionAction.Text = m_obj.PAction.ToString();
                lbl_RbActionArm.Text = m_obj.Source.PArm.ToString().Substring(3);
                lbl_RbActionTar.Text = m_obj.Source.PTarget.ToString();
                lbl_RbActionId.Text = m_obj.Source.Id.ToString();
                lbl_RbActionSlot.Text = m_obj.Source.Slot.ToString();
            }
            else
            {
                lbl_RbActionAction.Text = "";
                lbl_RbActionArm.Text = "";
                lbl_RbActionTar.Text = "";
                lbl_RbActionId.Text = "";
                lbl_RbActionSlot.Text = "";
            }
        }
        #endregion

        #region process MMF Event by case
        void ProcessRobotApiVersionNoti(string m_ChannelId, string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            string log = "";
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }
        void ProcessDeviceBackHomeNoti(string m_ChannelId, string m_MessageId, object m_Object)
        {
            //WriteNormalIn
            string log = "";
            CommonData.HIRATA.MDDeviceHome obj = m_Object as CommonData.HIRATA.MDDeviceHome;
            WriteLog(LogLevelType.General, log);
            //WriteNormalOut
        }
        #endregion

        #region Button Click
        //#region AlarmPage Button buzzer & reset alarm
        private void btn_buzzerOff_Click(object sender, EventArgs e)
        {
            //WriteNormalIn
            CommandData cmd = new CommandData();
            List<string> para = new List<string>();
            para.Add("0");
            cmd = new CommandData(APIEnum.CommandType.IO, APIEnum.IoCommand.Buzzer.ToString(), APIEnum.CommnadDevice.IO, 0, para);
            CommonData.HIRATA.MDApiCommand obj = new MDApiCommand();
            obj.CommandData = cmd;
            string rtn;
            object tmp = null;
            uint ticket;
            /*
            if (!Global.Controller.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDApiCommand).Name, obj, out ticket, out rtn, out tmp, 3000))
            {
                CommonStaticData.PopForm("Manual API command time out!!", true, false);
            }
            */
            //WriteNormalOut
        }
        private void btn_resetAllAlarm_Click(object sender, EventArgs e)
        {
            //WriteNormalIn
            //CommonData.HIRATA.MDApiErrorReset reset = new MDApiErrorReset();
            //Global.Controller.SendMmfNotifyObject(typeof(CommonData.HIRATA.MDApiErrorReset).Name, reset, KParseObjToXmlPropertyType.Field);

            if (UiForm.cv_Alarms.IsHasAlarm())
            {
                CommonStaticData.PopForm("Has alarms , please use initial.", true, false);
                return;
            }
            AlarmData obj = new AlarmData();
            obj.cv_AlarmList.AddRange(new List<AlarmItem>(UiForm.cv_Alarms.cv_AlarmList));
            for (int i = 0; i < obj.cv_AlarmList.Count; i++)
            {
                obj.cv_AlarmList[i].PStatus = AlarmStatus.Clean;
            }
            //Global.Controller.SendMmfNotifyObject(typeof(CommonData.HIRATA.AlarmData).Name, UiForm.cv_Alarms, KParseObjToXmlPropertyType.Field);
            //WriteNormalOut
        }
        //#endregion

        //#region Online mode change by button
        private void localToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
           //WriteNormalIn
            Global.Controller.SendRobotInlineChange(CommonData.EquipmentInlineMode.Local);
            //WriteNormalOut
            */
        }
        private void remoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
           //WriteNormalIn
            Global.Controller.SendRobotInlineChange(CommonData.EquipmentInlineMode.Remote);
            //WriteNormalOut
            */
        }
        void LockOnlineButton(bool m_IsLock)
        {
            //WriteNormalIn
            /*
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() =>
                {
                    LockOnlineButton(m_IsLock);
                }));
            }
            else
            {
                if (!m_IsLock)
                    cv_btnSelectMode.Enabled = true;
                else
                    cv_btnSelectMode.Enabled = false;
            }
            */
            //WriteNormalOut
        }
        void LockOperationButton(bool m_IsLock)
        {
            //WriteNormalIn
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() =>
                {
                    LockOperationButton(m_IsLock);
                }));
            }
            else
            {
                if (!m_IsLock)
                    cv_SystemAuto.Enabled = true;
                else
                    cv_SystemAuto.Enabled = false;
            }
            //WriteNormalOut
        }

        private void btn_TimeOut_Click(object sender, EventArgs e)
        {
            //WriteNormalIn
            if (cv_TimeOutForm == null)
            {
                cv_TimeOutForm = new TimrOutForm();
            }
            if (!cv_TimeOutForm.Visible)
            {
                cv_TimeOutForm.Show();
            }
            else
            {
                cv_TimeOutForm.Focus();
            }
            //WriteNormalOut
        }

        private void btn_CopyLogs_Click(object sender, EventArgs e)
        {
            //WriteNormalIn
            if (cv_CopyLogsForm == null)
            {
                cv_CopyLogsForm = new CopyLogForm();
            }
            if (!cv_CopyLogsForm.Visible)
            {
                cv_CopyLogsForm.Show();
            }
            //WriteNormalOut
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //cv_AccountData.Login("KGS", "!@#");
            //WriteNormalIn
            /*
            if (cv_PaletteForm == null)
            {
                cv_PaletteForm = new palette();
            }
            if (!cv_PaletteForm.Visible)
            {
                cv_PaletteForm.Show();
            }
            else
            {
                cv_PaletteForm.Focus();
            }
            */
            //WriteNormalOut
        }

        private void btn_RecipeCheck_Click(object sender, EventArgs e)
        {
            //WriteNormalIn
            if (cv_RecipeCheckForm == null)
            {
                cv_RecipeCheckForm = new RecipeCheckForm();
            }

            if (!cv_RecipeCheckForm.Visible)
            {
                cv_RecipeCheckForm.Show();
            }
            else
            {
                cv_RecipeCheckForm.Focus();
            }
            //WriteNormalOut
        }

        private void btn_monitor_Click(object sender, EventArgs e)
        {
            //WriteNormalIn
            if (cv_MonitorForm == null)
            {
                IniMonitorForm();
            }
            if (!cv_MonitorForm.Visible)
                cv_MonitorForm.Show();
            if (!cv_MonitorForm.Focused)
                cv_MonitorForm.Focus();
            //WriteNormalOut
        }
        #endregion

        private void cv_SystemAuto_Click(object sender, EventArgs e)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            string log = "[User press Operation(auto/manual) Button]" + Environment.NewLine;
            if (UiForm.PSystemData.POperationModeLeft == OperationMode.Auto)
            {
                bool is_force = false;
                if (Control.ModifierKeys == Keys.Control)
                {
                    log += "Use Force Change to Manual Mode. ";
                    is_force = true;
                }

               // cv_MmfController.SendOperationMode(is_force, OperationMode.Manual, MmfEventClientEventType.etRequest, true);
                LockOperationButton(true);
                log += "Change to Manual";
            }
            else
            {
                if (!UiForm.PSystemData.PInitaiizeOkRight)
                {
                    log += "Can't change Auto buz not initialize., Please initial first";
                    CommonStaticData.PopForm("Can't change Auto buz not initialize., Please initial first", true, false);

                }
                else if (UiForm.PSystemData.PSystemStatus == EquipmentStatus.Down)
                {
                    log += "Can't change Auto buz status down, Please initial first";
                    CommonStaticData.PopForm("Can't change Auto buz status down, Please initial first", true, false);
                }
                else
                {
                    if (UiForm.PSystemData.PapiInlineMode == EquipmentInlineMode.Remote)
                    {
                        if (UiForm.cv_Alarms.IsHasAlarm())
                        {
                            log += "Can't change Auto buz has alarm";
                            CommonStaticData.PopForm("System has alarm!!", true, false);
                        }
                        else
                        {
                            if (MessageBox.Show("Do you want change to Auto mode ?", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                LockOperationButton(true);
                                /*
                                if (cv_MmfController.SendOperationMode(false, OperationMode.Auto, MmfEventClientEventType.etRequest, true))
                                {
                                    log += "Change to Auto";
                                    //LockOperationButton(false);
                                }
                                else
                                {
                                    log += "Change to Auto fail , buz LGC time out";
                                    LockOperationButton(false);
                                }
                                */
                            }
                        }
                    }
                    else
                    {
                        log += "Can't Change to Manual , buz Robot at Local mode.";
                        CommonStaticData.PopForm("Robot not in Remote!!!", true, false);
                    }
                }
            }
            WriteLog(LogLevelType.General, log);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        private void lbl_RobotStatus_Click(object sender, EventArgs e)
        {
            //WriteNormalIn
            if (UiForm.PSystemData.PInitaiizingRight)
            {
                CommonStaticData.PopForm("Please wait initialize finish.", true, false);
                return;
            }
            CommonData.HIRATA.MDRobotInlineChange obj = new MDRobotInlineChange();
            if (Regex.Match(lbl_RobotInline.Text, @"remote", RegexOptions.IgnoreCase).Success)
            {
                obj.PChangeInlineMode = EquipmentInlineMode.Local;
                obj.PType = CommonData.HIRATA.MmfEventClientEventType.etRequest;
                UIController.triggerUiEvent(typeof(CommonData.HIRATA.MDRobotInlineChange).Name, obj);
            }
            else
            {
                obj.PChangeInlineMode = EquipmentInlineMode.Remote;
                obj.PType = CommonData.HIRATA.MmfEventClientEventType.etRequest;
                UIController.triggerUiEvent(typeof(CommonData.HIRATA.MDRobotInlineChange).Name, obj);
            }
            //WriteNormalOut
        }

        private void btn_ExeApiCommand_Click(object sender, EventArgs e)
        {
            //WriteNormalIn
            CommonData.HIRATA.MDApiCommand obj = new MDApiCommand();
            obj.CommandData.PCommandType = APIEnum.CommandType.API;
            obj.CommandData.PApiCommand = (APIEnum.APICommand)Enum.Parse(typeof(APIEnum.APICommand), cb_ManualApi.Text.Trim());
            obj.CommandData.cv_DeviceId = 0;
            obj.CommandData.PCommandDevice = APIEnum.CommnadDevice.API;
            obj.PType = MmfEventClientEventType.etRequest;

            string rtn;
            object tmp = null;
            uint ticket;
            /*
            if (!Global.Controller.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDApiCommand).Name, obj, out ticket, out rtn, out tmp, 3000))
            {
                CommonStaticData.PopForm("Manual API command time out!!", true, false);
            }
            else
            {

            }
            */
            //WriteNormalOut
        }

        private void btn_ExeCommonCommand_Click(object sender, EventArgs e)
        {
            //WriteNormalIn
            //cb_CommonId
            //cb_DeviceCommon
            //cb_CommonTarget
            int id = 0;
            APIEnum.CommonCommand command = APIEnum.CommonCommand.None;
            APIEnum.CommnadDevice device = APIEnum.CommnadDevice.None;
            if (!string.IsNullOrEmpty(cb_CommonId.Text.Trim()))
            {
                id = Convert.ToInt16(cb_CommonId.Text.Trim());
            }
            if (!string.IsNullOrEmpty(cb_DeviceCommon.Text.Trim()))
            {
                command = (APIEnum.CommonCommand)Enum.Parse(typeof(APIEnum.CommonCommand), cb_DeviceCommon.Text.Trim(), true);
            }
            if (!string.IsNullOrEmpty(cb_CommonTarget.Text.Trim()))
            {
                device = (APIEnum.CommnadDevice)Enum.Parse(typeof(APIEnum.CommnadDevice), cb_CommonTarget.Text.Trim(), true);
            }
            CommonData.HIRATA.CommandData command_obj = null;
            List<string> paras = new List<string>();

            switch (command)
            {
                case APIEnum.CommonCommand.Home:
                    if (device == APIEnum.CommnadDevice.Robot || device == APIEnum.CommnadDevice.EFEM)
                        command_obj = new CommandData(APIEnum.CommandType.Common, command.ToString(), device, 0);
                    else
                        command_obj = new CommandData(APIEnum.CommandType.Common, command.ToString(), device, id);
                    break;
                case APIEnum.CommonCommand.GetStatus:
                    if (device == APIEnum.CommnadDevice.Robot || device == APIEnum.CommnadDevice.EFEM)
                        command_obj = new CommandData(APIEnum.CommandType.Common, command.ToString(), device, 0);
                    else
                        command_obj = new CommandData(APIEnum.CommandType.Common, command.ToString(), device, id);
                    break;
                case APIEnum.CommonCommand.ResetError:
                    if (device == APIEnum.CommnadDevice.Robot || device == APIEnum.CommnadDevice.EFEM)
                        command_obj = new CommandData(APIEnum.CommandType.Common, command.ToString(), device, 0);
                    else
                        command_obj = new CommandData(APIEnum.CommandType.Common, command.ToString(), device, id);
                    break;
            };
            CommonData.HIRATA.MDApiCommand obj = new MDApiCommand();
            obj.CommandData = command_obj;
            string rtn;
            object tmp = null;
            uint ticket;
            /*
            if (!Global.Controller.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDApiCommand).Name, obj, out ticket, out rtn, out tmp, 3000))
            {
                CommonStaticData.PopForm("Manual API command time out!!", true, false);
            }
            */
            //WriteNormalOut
        }

        private void bt_IoExe_Click(object sender, EventArgs e)
        {
            /*
             * cb_IoAction
cb_IoLamp
cb_IoControl
cb_IoBuzzer
cb_IoDoor
cb_IoFfu
             */
            APIEnum.IoCommand command = APIEnum.IoCommand.None;
            string lamp = "";
            string control = "";
            string buzzer = "";
            string door = "";
            string ffu = "";
            if (!string.IsNullOrEmpty(cb_IoLamp.Text.Trim()))
            {
                lamp = (cb_IoLamp.Text.Trim());
            }
            if (!string.IsNullOrEmpty(cb_IoControl.Text.Trim()))
            {
                control = (cb_IoControl.Text.Trim());
            }
            if (!string.IsNullOrEmpty(cb_IoDoor.Text.Trim()))
            {
                door = (cb_IoDoor.Text.Trim());
            }
            if (!string.IsNullOrEmpty(cb_IoFfu.Text.Trim()))
            {
                ffu = (cb_IoFfu.Text.Trim());
            }
            if (!string.IsNullOrEmpty(cb_IoBuzzer.Text.Trim()))
            {
                buzzer = (cb_IoBuzzer.Text.Trim());
                if (buzzer == "On")
                {
                    buzzer = "1";
                }
                else
                {
                    buzzer = "0";
                }
            }
            if (!string.IsNullOrEmpty(cb_IoAction.Text.Trim()))
            {
                command = (APIEnum.IoCommand)Enum.Parse(typeof(APIEnum.IoCommand), cb_IoAction.Text.Trim(), true);
            }

            CommonData.HIRATA.CommandData command_obj = null;
            List<string> paras = new List<string>();

            switch (command)
            {
                case APIEnum.IoCommand.Buzzer:
                    paras.Add(buzzer);
                    command_obj = new CommandData(APIEnum.CommandType.IO, command.ToString(), APIEnum.CommnadDevice.IO, 0, paras);
                    break;
                case APIEnum.IoCommand.SetFFUVoltage:
                    paras.Add(ffu);
                    command_obj = new CommandData(APIEnum.CommandType.IO, command.ToString(), APIEnum.CommnadDevice.IO, 0, paras);
                    break;
                case APIEnum.IoCommand.SignalTower:
                    paras.Add(lamp + control);
                    command_obj = new CommandData(APIEnum.CommandType.IO, command.ToString(), APIEnum.CommnadDevice.IO, 0, paras);
                    break;
                case APIEnum.IoCommand.GetBufferStatus:
                    paras.Add("Buffer1");
                    command_obj = new CommandData(APIEnum.CommandType.IO, command.ToString(), APIEnum.CommnadDevice.IO, 0, paras);
                    break;

            };

            CommonData.HIRATA.MDApiCommand obj = new MDApiCommand();
            obj.CommandData = command_obj;
            string rtn;
            object tmp = null;
            uint ticket;
            /*
            if (!Global.Controller.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDApiCommand).Name, obj, out ticket, out rtn, out tmp, 3000))
            {
                CommonStaticData.PopForm("Manual API command time out!!", true, false);
            }
            */
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //comboBox1
            //    textBox1
            int speed = 0;
            APIEnum.RobotCommand command = APIEnum.RobotCommand.None;
            if (!string.IsNullOrEmpty(cb_Speed.Text.Trim()))
            {
                speed = Convert.ToInt16(cb_Speed.Text.Trim());
            }
            else
            {
                CommonStaticData.PopForm("Please select speed!!!", true, false);
                return;
            }
            if (!string.IsNullOrEmpty(cb_Speed.Text.Trim()))
            {
                //command = (APIEnum.RobotCommand)Enum.Parse(typeof(APIEnum.RobotCommand), comboBox1.Text.Trim(), true);
                command = APIEnum.RobotCommand.SetRobotSpeed;
            }
            CommonData.HIRATA.CommandData command_obj = null;
            List<string> paras = new List<string>();

            switch (command)
            {
                case APIEnum.RobotCommand.SetRobotSpeed:
                    paras.Add(speed.ToString());
                    paras.Add(speed.ToString());
                    command_obj = new CommandData(APIEnum.CommandType.Robot, command.ToString(), APIEnum.CommnadDevice.Robot, 0, paras);
                    break;
                case APIEnum.RobotCommand.ReStart:
                    //paras.Add(speed.ToString());
                    command_obj = new CommandData(APIEnum.CommandType.Robot, command.ToString(), APIEnum.CommnadDevice.Robot, 0, paras);
                    break;

            };
            CommonData.HIRATA.MDApiCommand obj = new MDApiCommand();
            obj.CommandData = command_obj;
            string rtn;
            object tmp = null;
            uint ticket;
            /*
            if (!Global.Controller.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDApiCommand).Name, obj, out ticket, out rtn, out tmp, 3000))
            {
                CommonStaticData.PopForm("Manual API command time out!!", true, false);
            }
            */
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            CommonData.HIRATA.MDOcrMode obj = new MDOcrMode();
            obj.PType = MmfEventClientEventType.etRequest;
            obj.POcrMode = OCRMode.ErrorSkip;
            string rtn;
            object tmp = null;
            uint ticket;
            string log = "";
            /*
            if (!Global.Controller.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDOcrMode).Name, obj, out ticket, out rtn, out tmp, 3000))
            {
                log += "[Time Out]Wait : " + typeof(CommonData.HIRATA.MDOcrMode).Name;
            }
            */
        }

        private void errorReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommonData.HIRATA.MDOcrMode obj = new MDOcrMode();
            obj.PType = MmfEventClientEventType.etRequest;
            obj.POcrMode = OCRMode.ErrorReturn;
            string rtn;
            object tmp = null;
            uint ticket;
            string log = "";
            /*
            if (!Global.Controller.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDOcrMode).Name, obj, out ticket, out rtn, out tmp, 3000))
            {
                log += "[Time Out]Wait : " + typeof(CommonData.HIRATA.MDOcrMode).Name;
            }
            */
        }

        private void oCROFFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommonData.HIRATA.MDOcrMode obj = new MDOcrMode();
            obj.PType = MmfEventClientEventType.etRequest;
            obj.POcrMode = OCRMode.SkipRead;
            string rtn;
            object tmp = null;
            uint ticket;
            string log = "";
            /*
            if (!Global.Controller.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDOcrMode).Name, obj, out ticket, out rtn, out tmp, 3000))
            {
                log += "[Time Out]Wait : " + typeof(CommonData.HIRATA.MDOcrMode).Name;
            }
            */
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            CommonData.HIRATA.MDOcrMode obj = new MDOcrMode();
            obj.PType = MmfEventClientEventType.etRequest;
            obj.POcrMode = OCRMode.ErrorHold;
            string rtn;
            object tmp = null;
            uint ticket;
            string log = "";
            /*
            if (!Global.Controller.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDOcrMode).Name, obj, out ticket, out rtn, out tmp, 3000))
            {
                log += "[Time Out]Wait : " + typeof(CommonData.HIRATA.MDOcrMode).Name;
            }
            */
        }

        private void cv_dataViewIo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            /*
            if (e.RowIndex != -1)
            {
                if (!cv_ManualIo)
                    return;
                if (e.ColumnIndex != -1)
                {
                    if (cv_dataViewIo.CurrentCell.ColumnIndex == 1 || cv_dataViewIo.CurrentCell.ColumnIndex == 0)
                    {
                        string tmp = cv_dataViewIo.Rows[e.RowIndex].Cells[0].Value.ToString();
                        int value = cv_Mio.GetPortValueByName(tmp);
                        cv_Mio.SetPortValueByName(tmp, value == 1 ? 0 : 1);
                        if (cv_Mio.GetPortValueByName(tmp) == 1)
                        {
                            if (cv_dataViewIo.Rows[e.RowIndex].Cells[2].Style.BackColor != Color.Lime)
                                cv_dataViewIo.Rows[e.RowIndex].Cells[2].Style.BackColor = Color.Lime;
                        }
                        else
                        {
                            if (cv_dataViewIo.Rows[e.RowIndex].Cells[2].Style.BackColor != Color.Gray)
                                cv_dataViewIo.Rows[e.RowIndex].Cells[2].Style.BackColor = Color.Gray;
                        }

                    }
                    else if (cv_dataViewIo.CurrentCell.ColumnIndex == 3 || cv_dataViewIo.CurrentCell.ColumnIndex == 4)
                    {
                        string tmp = cv_dataViewIo.Rows[e.RowIndex].Cells[3].Value.ToString();
                        int value = cv_Mio.GetPortValueByName(tmp);
                        cv_Mio.SetPortValueByName(tmp, value == 1 ? 0 : 1);
                        if (cv_Mio.GetPortValueByName(tmp) == 1)
                        {
                            if (cv_dataViewIo.Rows[e.RowIndex].Cells[5].Style.BackColor != Color.Lime)
                                cv_dataViewIo.Rows[e.RowIndex].Cells[5].Style.BackColor = Color.Lime;
                        }
                        else
                        {
                            if (cv_dataViewIo.Rows[e.RowIndex].Cells[5].Style.BackColor != Color.Gray)
                                cv_dataViewIo.Rows[e.RowIndex].Cells[5].Style.BackColor = Color.Gray;
                        }
                    }
                }
            }
            else
            {
                if (e.ColumnIndex == -1)
                {
                    if (!cv_ManualIo)
                    {
                        AccountItem tmp = null;
                        if(cv_AccountData.GetCurAccount(out tmp))
                        {
                            if(tmp.PPermission == UserPermission.Root)
                            {
                                cv_ManualIo = true;
                                cv_dataViewIo.ColumnHeadersDefaultCellStyle.BackColor = Color.Lime;
                            }
                        }
                    }
                    else
                    {
                        cv_ManualIo = false;
                        cv_dataViewIo.ColumnHeadersDefaultCellStyle.BackColor = Color.Gray;
                    }
                }
            }
            */
        }

        #region log in & out
        private void cv_btnLogin_Click(object sender, EventArgs e)
        {
            if (cv_LoginForm != null && !cv_LoginForm.Visible)
            {
                cv_LoginForm.ClearUI();
                cv_LoginForm.Show();
            }
            else
            {
                if (!cv_LoginForm.Focused)
                {
                    cv_LoginForm.Focus();
                }
            }
        }
        // change ui control obj enable or disable.
        private void ChangeObjEnable()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, CommonData.HIRATA.CommonStaticData.__FUN(), CommonData.HIRATA.FunInOut.Enter);
            AccountItem tmp = null;
            CommonData.HIRATA.UserPermission permission = UserPermission.None;
            if (UiForm.cv_AccountData.GetCurAccount(out tmp))
            {
                permission = tmp.PPermission;
            }
            else
            {
                permission = UserPermission.None;
            }

            foreach(Control control in cv_GroupObjs[enumGroup.All])
            {
                control.Enabled = false;
            }

            if(cv_PermissionGroups.ContainsKey(permission))
            {
                foreach(int group in cv_PermissionGroups[permission])
                {
                    foreach (Control control2 in cv_GroupObjs[(enumGroup)(group - 1)])
                    {
                        control2.Enabled = true;
                    }
                }
            }

            /*
            for (int i = 0; i < cv_AllUiObj.Count; i++)
            {
                if ((int)cv_AllUiObj.ElementAt(i).Value <= (int)permission)
                {
                    cv_AllUiObj.ElementAt(i).Key.Enabled = true;
                }
                else
                {
                    cv_AllUiObj.ElementAt(i).Key.Enabled = false;
                }
            }
            */
            WriteLog(LogLevelType.NormalFunctionInOut, CommonData.HIRATA.CommonStaticData.__FUN(), CommonData.HIRATA.FunInOut.Enter);
        }
        #endregion

        //unlock online button
        private void lbl_date_Click(object sender, EventArgs e)
        {
            LockOnlineButton(false);
            LockOperationButton(false);
            btn_ReIni.Enabled = true;
        }

        private void lbl_time_Click(object sender, EventArgs e)
        {
        }

        private void cv_OntMode_Click(object sender, EventArgs e)
        {
            //cv_MmfController.SendOntModeReq(!PSystemData.PONT);
        }

        private void cv_AutoJob_Click(object sender, EventArgs e)
        {
            /*
            if (cv_RobotAutoJobPathTable != null)
            {
                if (cv_RobotAutoJobPathTable.Visible)
                {
                    cv_RobotAutoJobPathTable.Focus();
                }
                else
                {
                    cv_RobotAutoJobPathTable.Show();
                }
            }
            else
            {
                cv_RobotAutoJobPathTable = new RobotJobPathForm();
                cv_RobotAutoJobPathTable.Show();
            }
            */
        }

        private void cv_ManualJob_Click(object sender, EventArgs e)
        {
            /*
            if (cv_RobotManualJobPathTable != null)
            {
                if (cv_RobotManualJobPathTable.Visible)
                {
                    cv_RobotManualJobPathTable.Focus();
                }
                else
                {
                    cv_RobotManualJobPathTable.Show();
                }
            }
            else
            {
                cv_RobotManualJobPathTable = new RobotJobPathForm(false);
                cv_RobotManualJobPathTable.Show();
            }
            */
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int speed = 0;
            APIEnum.IoCommand command = APIEnum.IoCommand.None;
            if (!string.IsNullOrEmpty(cb_FfuSpeed.Text.Trim()))
            {
                speed = Convert.ToInt16(cb_FfuSpeed.Text.Trim());
            }
            else
            {
                CommonStaticData.PopForm("Please select speed!!!", true, false);
                return;
            }
            if (!string.IsNullOrEmpty(cb_FfuSpeed.Text.Trim()))
            {
                //command = (APIEnum.RobotCommand)Enum.Parse(typeof(APIEnum.RobotCommand), comboBox1.Text.Trim(), true);
                command = APIEnum.IoCommand.SetFFUVoltage;
            }
            CommonData.HIRATA.CommandData command_obj = null;
            List<string> paras = new List<string>();

            switch (command)
            {
                case APIEnum.IoCommand.SetFFUVoltage:
                    paras.Add(speed.ToString());
                    command_obj = new CommandData(APIEnum.CommandType.IO, command.ToString(), APIEnum.CommnadDevice.IO, 0, paras);
                    break;
            };
            CommonData.HIRATA.MDApiCommand obj = new MDApiCommand();
            obj.CommandData = command_obj;
            string rtn;
            object tmp = null;
            uint ticket;
            /*
            if (!Global.Controller.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDApiCommand).Name, obj, out ticket, out rtn, out tmp, 3000))
            {
                CommonStaticData.PopForm("Manual API command time out!!", true, false);
            }
            */
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            CommonData.HIRATA.CommandData command_obj = null;
            List<string> paras = new List<string>();

            if (cb_RbVacArm.Text.Trim() != "" && cb_RbVac.Text.Trim() != "")
            {
                if (cb_RbVacArm.Text.Trim() == "UP") paras.Add("2");
                else if (cb_RbVacArm.Text.Trim() == "DOWN") paras.Add("1");

                if (cb_RbVac.Text.Trim() == "ON")
                {
                    command_obj = new CommandData(APIEnum.CommandType.Robot, APIEnum.RobotCommand.VacuumOn.ToString(), APIEnum.CommnadDevice.Robot, 0, paras);
                }
                if (cb_RbVac.Text.Trim() == "OFF")
                {
                    command_obj = new CommandData(APIEnum.CommandType.Robot, APIEnum.RobotCommand.VacuumOff.ToString(), APIEnum.CommnadDevice.Robot, 0, paras);
                }
                CommonData.HIRATA.MDApiCommand obj = new MDApiCommand();
                obj.CommandData = command_obj;
                string rtn;
                object tmp = null;
                uint ticket;
                /*
                if (!UiForm.cv_mmfController.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDApiCommand).Name, obj, out ticket, out rtn, out tmp, 3000))
                {
                    CommonStaticData.PopForm("Manual API command (Rb Vac) time out!!", true, false);
                }
                */
            }
            else
            {
                CommonStaticData.PopForm("Please select data.", true, false);
            }
        }

        private void cb_RobotActionName_SelectedIndexChanged(object sender, EventArgs e)
        {
            cb_robotActionDeg.SelectedIndex = -1;
            cb_robotActionDeg.Items.Clear();
            if (Regex.Match(cb_RobotActionName.Text.Trim(), @"aligner", RegexOptions.IgnoreCase).Success)
            {
                RecipeItem recipe = null;
                if (UiForm.cv_Recipes.GetCurRecipe(out recipe))
                {
                    cb_robotActionDeg.Items.Add(recipe.PWaferIJPDegree.ToString());
                    cb_robotActionDeg.Items.Add(recipe.PWaferVASDegree.ToString());
                    cb_robotActionDeg.Items.Add(recipe.PGlassVASDegree.ToString());
                }
                else
                {
                    WriteLog(LogLevelType.Error, "[cb_RobotActionName_SelectedIndexChanged] can find cur. reicpe", FunInOut.None);
                }
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            List<AlarmItem> a_datalist = new List<AlarmItem>();
            DateTime a_time = dateTimePicker1.Value;
            string CONFIG_PATH = CommonData.HIRATA.CommonStaticData.g_RootLogsFolderPath + "LGC\\" + a_time.ToString("yyyy") + "\\" + a_time.ToString("MM") + "\\" + a_time.ToString("dd") + "\\";
            //string CONFIG_PATH = SysUtils.ExtractFilePath(SysUtils.GetExeName()) + "..\\..\\..\\Log\\" + a_time.ToString("yyyy") + "\\" + a_time.ToString("MM") + "\\" + a_time.ToString("dd") + "\\";
            string File_Name = "Alarmlog_" + a_time.ToString("yyyyMMdd") + ".log";
            string File_PATH = CONFIG_PATH + File_Name;
            if (SysUtils.FileExists(File_PATH))
            {
                List<string> a_temp = new List<string>();
                SysUtils.LoadFromFile(File_PATH, a_temp);
                foreach (string a_str in a_temp)
                {
                    List<string> a_list = new List<string>();
                    SysUtils.StringSplit(a_str, ",", a_list, true);
                    if (a_list.Count == 5)
                    {
                        AlarmItem a_data = new AlarmItem();
                        a_data.PTime = a_list[0];
                        a_data.PCode = a_list[1];
                        if (a_list[2].Trim() == "Serious")
                            a_data.PLevel = AlarmLevele.Serious;
                        else if (a_list[2].Trim() == "Light")
                            a_data.PLevel = AlarmLevele.Light;

                        a_data.PUnit = Convert.ToInt32(a_list[3].Trim());

                        List<string> a_msg = new List<string>();
                        SysUtils.StringSplit(a_list[4].Trim(), ":", a_msg, true);
                        if (a_msg.Count == 2)
                        {
                            a_data.PMainDescription = a_msg[0].Trim();
                            a_data.PSubDescription = a_msg[1].Trim();
                        }
                        else if (a_msg.Count == 1)
                        {
                            a_data.PMainDescription = a_msg[0].Trim();
                        }
                        a_datalist.Add(a_data);
                    }
                }
                cv_AlarmHView.DataSource = a_datalist;
                if (a_datalist.Count < 1)
                {
                    CommonStaticData.PopForm("The date alarm file hasn't any alarm item!", true, false);
                    WriteLog(LogLevelType.General, "[UI] Alarm history : user choose the date : " + File_PATH + " The date alarm file hasn't any alarm item");
                }
            }
            else
            {
                CommonStaticData.PopForm("The date alarm file not found!", true, false);
                WriteLog(LogLevelType.General, "[UI] Alarm history : user choose the date : " + File_PATH + " not found!");
            }
        }

        private void cv_tcBar_Selecting(object sender, TabControlCancelEventArgs e)
        {
            CheckPermission(sender, e);
        }
        private void CheckPermission(object sender, TabControlCancelEventArgs e)
        {
            AccountItem tmp = null;
            CommonData.HIRATA.UserPermission permission = UserPermission.None;
            if(UiForm.cv_AccountData.GetCurAccount(out tmp))
            {
                permission = tmp.PPermission;
            }
            else
            {
                permission = UserPermission.None;
            }

            if (e.TabPage.Name == "cv_tpIo" || e.TabPage.Name == "cv_tpLog")
            {
                if (cv_PermissionGroups.ContainsKey(permission))
                {
                    if(!cv_PermissionGroups[permission].Contains(4))
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
            //cv_tpAccount , cv_tpPermissionSetLog
            if (e.TabPage.Name == "cv_tpAccount" || e.TabPage.Name == "cv_tpPermissionSetLog")
            {
                if (cv_PermissionGroups.ContainsKey(permission))
                {
                    if (!cv_PermissionGroups[permission].Contains(6))
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int row_count = cv_PermissionSetting.dataGridView1.RowCount;
            bool g1 = false;
            bool g2 = false;
            bool g3 = false;
            bool g4 = false;
            bool g5 = false;
            bool g6 = false;
            for (int i = 0; i < row_count; i++)
            {
                g1 = false;
                g2 = false;
                g3 = false;
                g4 = false;
                g5 = false;
                g6 = false;
                if (cv_PermissionSetting.dataGridView1.Rows[i].Cells[0].Value.ToString().ToUpper() == "ROOT")
                {
                    SetGroupsToList(i, CommonData.HIRATA.UserPermission.Root );
                }
                else if (cv_PermissionSetting.dataGridView1.Rows[i].Cells[0].Value.ToString().ToUpper() == "ENGINEER")
                {
                    SetGroupsToList(i, CommonData.HIRATA.UserPermission.Engineer );
                }
                else if (cv_PermissionSetting.dataGridView1.Rows[i].Cells[0].Value.ToString().ToUpper() == "OP1")
                {
                    SetGroupsToList(i, CommonData.HIRATA.UserPermission.OP1 );
                }
                else if (cv_PermissionSetting.dataGridView1.Rows[i].Cells[0].Value.ToString().ToUpper() == "OP2")
                {
                    SetGroupsToList(i, CommonData.HIRATA.UserPermission.OP2 );
                }
                else if (cv_PermissionSetting.dataGridView1.Rows[i].Cells[0].Value.ToString().ToUpper() == "OP3")
                {
                    SetGroupsToList(i, CommonData.HIRATA.UserPermission.OP3 );
                }
            }
            cv_PermissionGroupsXml.SaveToFile(CommonData.HIRATA.CommonStaticData.g_SysPermissionGroupFile, true);
        }

        private void SetGroupsToList(int i , CommonData.HIRATA.UserPermission m_Permission)
        {
            List<int> tmp = new List<int>();
            if ((cv_PermissionSetting.dataGridView1.Rows[i].Cells[1] as DataGridViewCheckBoxCell).Value == "T")
            {
                tmp.Add(1);
                cv_PermissionGroupsXml.ItemsByName[m_Permission.ToString()].Attributes["Group1"] = "1";
            }
            else
            {
                cv_PermissionGroupsXml.ItemsByName[m_Permission.ToString()].Attributes["Group1"] = "0";
            }
            if ((cv_PermissionSetting.dataGridView1.Rows[i].Cells[2] as DataGridViewCheckBoxCell).Value == "T")
            {
                tmp.Add(2);
                cv_PermissionGroupsXml.ItemsByName[m_Permission.ToString()].Attributes["Group2"] = "1";
            }
            else
            {
                cv_PermissionGroupsXml.ItemsByName[m_Permission.ToString()].Attributes["Group2"] = "0";
            }
            if ((cv_PermissionSetting.dataGridView1.Rows[i].Cells[3] as DataGridViewCheckBoxCell).Value == "T")
            {
                tmp.Add(3);
                cv_PermissionGroupsXml.ItemsByName[m_Permission.ToString()].Attributes["Group3"] = "1";
            }
            else
            {
                cv_PermissionGroupsXml.ItemsByName[m_Permission.ToString()].Attributes["Group3"] = "0";
            }
            if ((cv_PermissionSetting.dataGridView1.Rows[i].Cells[4] as DataGridViewCheckBoxCell).Value == "T")
            {
                tmp.Add(4);
                cv_PermissionGroupsXml.ItemsByName[m_Permission.ToString()].Attributes["Group4"] = "1";
            }
            else
            {
                cv_PermissionGroupsXml.ItemsByName[m_Permission.ToString()].Attributes["Group4"] = "0";
            }
            if ((cv_PermissionSetting.dataGridView1.Rows[i].Cells[5] as DataGridViewCheckBoxCell).Value == "T")
            {
                tmp.Add(5);
                cv_PermissionGroupsXml.ItemsByName[m_Permission.ToString()].Attributes["Group5"] = "1";
            }
            else
            {
                cv_PermissionGroupsXml.ItemsByName[m_Permission.ToString()].Attributes["Group5"] = "0";
            }
            if ((cv_PermissionSetting.dataGridView1.Rows[i].Cells[6] as DataGridViewCheckBoxCell).Value == "T")
            {
                tmp.Add(6);
                cv_PermissionGroupsXml.ItemsByName[m_Permission.ToString()].Attributes["Group6"] = "1";
            }
            else
            {
                cv_PermissionGroupsXml.ItemsByName[m_Permission.ToString()].Attributes["Group6"] = "0";
            }
            cv_PermissionGroups[m_Permission] = tmp;
        }

        private void Left_Click(object sender, EventArgs e)
        {
            ReInit_Click(sender, e, enSideGroup.Left);
        }

        private void Rght_Click(object sender, EventArgs e)
        {
            ReInit_Click(sender, e, enSideGroup.Right);
        }

        private void ALL_Click(object sender, EventArgs e)
        {
            ReInit_Click(sender, e, enSideGroup.Both);
        }

        private void panel38_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

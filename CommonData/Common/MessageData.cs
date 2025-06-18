using System;
using System.Collections.Generic;
using System.Text;

namespace CommonData.HIRATA
{
    //base defined
    #region ONT
    public class MDOntMode : MessageBase
    {
        public bool cv_IsOn;
        public bool PIsOn
        {
            get { return cv_IsOn; }
            set { cv_IsOn = value; }
        }
    }
    #endregion

    #region LogInOut
    public class MDLogInOut : MessageBase
    {
        public int Action;
        public LogInOut PAction
        {
            get { return (LogInOut)Action; }
            set { Action = (int)value; }
        }
    }
    #endregion

    #region Account Req
    public class MDAccountReq : MessageBase
    {
    }
    #endregion

    #region Recipe Action
    public class MDRecipeAction : MessageBase
    {
        public int Action;
        public List<RecipeItem> Recipes = new List<RecipeItem>();
        public DataEidtAction PAction
        {
            get { return (DataEidtAction)Action; }
            set { Action = (int)value; }
        }
    }
    #endregion

    #region Recipe Req
    public class MDRecipeReq : MessageBase
    {
    }
    #endregion

    #region Alarm action
    public class MDAlarmAction : MessageBase
    {
        public uint Tick;
        public AlarmData AlarmData = new AlarmData();  
    }
    #endregion

    #region Alarm Req
    public class MDAlarmReq : MessageBase
    {
        public uint Tick;
    }

    #endregion

    #region SystemData Req
    public class MDSystemReq : MessageBase
    {
        public uint Tick;
    }

    #endregion

    #region GlassCountData Req
    public class MDGlassCountReq : MessageBase
    {
        public uint Tick;
    }

    #endregion

    #region TimeoutData Req
    public class MDTimeoutDataReq : MessageBase
    {
        public uint Tick;
    }

    #endregion


    //base end

    #region RobotAction
    public class MDRobotAction : MessageBase
    {
        public UInt64 Tick;
        public int Action;
        public int Result;
        public int RobotId = 0;
        public int UseHS = 0;
        public Source Source = new Source();
        public string cv_AlignerDeg = "";
        public string PAlignerDeg
        {
            get { return cv_AlignerDeg; }
            set { cv_AlignerDeg = value; }
        }
        public Result PResult
        {
            get { return (Result)Result; }
            set { Result = (int)value; }
        }
        public RobotAction PAction
        {
            get { return (RobotAction)Action; }
            set { Action = (int)value; }
        } 
        public bool PUseHS
        {
            get { return UseHS == 1 ? true : false; }
            set { UseHS = Convert.ToInt16(value); }
        }
    }
    #endregion

    #region Set time out

    public class MDSetTimeOut : MessageBase
    {
        public int cv_IdleTIme;
        public int cv_IntervalTIme;
        public int cv_T0TIme;
        public int cv_T1TIme;
        public int cv_T3TIme;
        public int cv_TsTIme;
        public int cv_TeTIme;
        public int cv_TmTIme;
        public int cv_ApiT3TIme;
        public int PApiT3TIme
        {
            get { return cv_ApiT3TIme; }
            set { cv_ApiT3TIme = value; }
        }
        public int PIdleTIme
        {
            get { return cv_IdleTIme; }
            set { cv_IdleTIme = value; }
        }
        public int PIntervalTIme
        {
            get { return cv_IntervalTIme; }
            set { cv_IntervalTIme = value; }
        }
        public int PT0TIme
        {
            get { return cv_T0TIme; }
            set { cv_T0TIme = value; }
        }
        public int PT1TIme
        {
            get { return cv_T1TIme; }
            set { cv_T1TIme = value; }
        }
        public int PT3TIme
        {
            get { return cv_T3TIme; }
            set { cv_T3TIme = value; }
        }
        public int PTsTIme
        {
            get { return cv_TsTIme; }
            set { cv_TsTIme = value; }
        }
        public int PTeTIme
        {
            get { return cv_TeTIme; }
            set { cv_TeTIme = value; }
        }

        public int PTmTIme
        {
            get { return cv_TmTIme; }
            set { cv_TmTIme = value; }
        }
    }
    #endregion

    #region OCR Mode

    public class MDOcrMode : MessageBase
    {
        public int cv_OcrMode;
        public OCRMode POcrMode
        {
            get { return (OCRMode)cv_OcrMode; }
            set { cv_OcrMode = (int)value; }
        }
    }
    #endregion

    #region Glass chech data
    public class MDGlassDataCheck : MessageBase
    {
        public int cv_CheckRecipeNo;
        public int cv_CheckWorkId;
        public int cv_CheckWorkSlot;
        public int cv_CheckFoupSeq;
        public bool PCheckRecipeNo
        {
            get { return cv_CheckRecipeNo == 1 ? true : false; }
            set { cv_CheckRecipeNo = Convert.ToInt16(value); }
        }
        public bool PCheckWorkId
        {
            get { return cv_CheckWorkId == 1 ? true : false; }
            set { cv_CheckWorkId = Convert.ToInt16(value); }
        }
        public bool PCheckWorkSlot
        {
            get { return cv_CheckWorkSlot == 1 ? true : false; }
            set { cv_CheckWorkSlot = Convert.ToInt16(value); }
        }
        public bool PCheckFoupSeq
        {
            get { return cv_CheckFoupSeq == 1 ? true : false; }
            set { cv_CheckFoupSeq = Convert.ToInt16(value); }
        }
    }
    #endregion

    #region robot mode change
    public class MDRobotInlineChange : MessageBase
    {
        public int cv_ChangeInlineMode;
        public int Result;
        public EquipmentInlineMode PChangeInlineMode
        {
            get { return (EquipmentInlineMode)cv_ChangeInlineMode; }
            set { cv_ChangeInlineMode = (int)value; }
        }
        public Result PResult
        {
            get { return (Result)Result; }
            set { Result = (int)value; }
        }
    }

    #endregion

    #region Operation mode change ( auto , manual )
    public class MDOperationModeChangeRight : MessageBase
    {
        public bool cv_IsForce;
        public int cv_ChangeOperationModeRight;
        public int Result;
        public OperationMode PChangeOperationModeRight
        {
            get { return (OperationMode)cv_ChangeOperationModeRight; }
            set { cv_ChangeOperationModeRight = (int)value; }
        }
        public Result PResult
        {
            get { return (Result)Result; }
            set { Result = (int)value; }
        }
        public bool PIsForce
        {
            get { return cv_IsForce; }
            set { cv_IsForce = value; }
        }
    }
    public class MDOperationModeChangeLeft : MessageBase
    {
        public bool cv_IsForce;
        public int cv_ChangeOperationModeLeft;
        public int Result;
        public OperationMode PChangeOperationModeLeft
        {
            get { return (OperationMode)cv_ChangeOperationModeLeft; }
            set { cv_ChangeOperationModeLeft = (int)value; }
        }
        public Result PResult
        {
            get { return (Result)Result; }
            set { Result = (int)value; }
        }
        public bool PIsForce
        {
            get { return cv_IsForce; }
            set { cv_IsForce = value; }
        }
    }
    #endregion

    #region OnLineRequest
    public class MDOnlineRequest : MessageBase
    {
        public int cv_CurMode;
        public int cv_ChangeMode;
        public int cv_Result;
        public OnlineMode PCurMode
        {
            get { return (OnlineMode)cv_CurMode; }
            set { cv_CurMode = (int)value; }
        }
        public OnlineMode PChangeMode
        {
            get { return (OnlineMode)cv_ChangeMode; }
            set { cv_ChangeMode = (int)value; }
        }
        public Result PResult
        {
            get { return (Result)cv_Result; }
            set { cv_Result = (int)value; }
        }
    }
    #endregion

    #region EqStatus
    public class MDEqDataReq : MessageBase
    {
        public int tick;
        public int Id;
        public List<Eq> Eqs = new List<Eq>();
    }
    public class Eq
    {
        public int Id;
        public int PId
        {
            get { return Id; }
            set { Id = value; }
        }
    }
    #endregion

    #region Remove ShowMsg
    public class MDRemoveShowMsg : MessageBase
    {
        public int cv_TipNumber = 0;
        public int PTipNumber
        {
            set { cv_TipNumber = value; }
            get { return cv_TipNumber; }
        }
    }
    #endregion

    #region ShowMsg
    public class MDShowMsg : MessageBase
    {
        public int cv_TipNumber = 0;
        public Msg Msg = new Msg();
    }
    public class Msg
    {
        public UInt64 Tick;
        public int AutoClean;
        public UInt64 TimeOut;
        public int UserRep;
        public int Choice;
        public string Txt;
        public bool PAutoClean
        {
            get { return AutoClean == 1 ? true : false; }
            set { AutoClean = Convert.ToInt16(value); }
        }
        public bool PUserRep
        {
            get { return UserRep == 1 ? true: false; }
            set { UserRep = Convert.ToInt16(value); }
        }
        public UserChoice PChoice
        {
            get { return (UserChoice)Choice; }
            set { Choice = (int)value; }
        }
    }
    #endregion

    #region PopOpidForm
    public class MDPopOpidForm : MessageBase
    {
        public UInt64 Tick;
        public int PortId;
        public ReplyData Reply;
    }

    public class ReplyData
    {
        public string CstId;
        public string OpId;
        public string CstSeq;
        public int Result;
        public Result PResult
        {
            get { return (Result)Result; }
            set { Result = (int)value; }
        }
    }
    #endregion

    #region PopMonitorForm
    public class MDPopMonitorForm : MessageBase
    {
        public UInt64 Tick;
        public int PortId;
        public int Result;
        public Result PResult
        {
            get { return (Result)Result; }
            set { Result = (int)value; }
        }
    }
    public class MDPopMonitorFormRep : MessageBase
    {
        public UInt64 Tick;
        public int Result;
        public int PortId;
       // public List<PortData> Port = new List<PortData>();
        public PortData PortData = new PortData();
        public Result PResult
        {
            get { return (Result)Result; }
            set { Result = (int)value; }
        }
    }
    #endregion

    #region Initial
    public class MDInitial : MessageBase
    {
        public bool cv_IsForce = false;
        public int Action;
        public int Result;
        public int Side;
        public enSideGroup PSide
        {
            get { return (enSideGroup)Side; }
            set { Side = (int)value; }
        }
        public InitialAction PAction
        {
            get { return (InitialAction)Action; }
            set { Action = (int)value; }
        }
        public Result PResult
        {
            get { return (Result)Result; }
            set { Result = (int)value; }
        }
    };
    #endregion

    #region Cur. Recipe
    public class MDCurRecipe : MessageBase
    {
        public int cv_CurRecipe;
        public int PCurRecipe
        {
            get { return cv_CurRecipe; }
            set { cv_CurRecipe = value; }
        }
    }

    #endregion

    #region Robot Mapping
    public class MDRobotMapping : MessageBase
    {
        /*
        public Robot Robot = new Robot();
        public Port Port = new Port();
        public int Result;
        public string MappingData;
        public Result PResult
        {
            get { return (Result)Result; }
            set { Result = (int)value; }
        }
        */
    }
    #endregion

    #region MDRobotStatus
    public class MDRobotDataReq : MessageBase
    {
        public UInt64 Tick;
        public List<uint> Robots = new List<uint>();
    }
    #endregion

    #region Port Action
    public class MDPortAction : MessageBase
    {
        public UInt64 Tick;
        public int PortId;
        public int Action;
        public int Result;
        public PortAction PAction
        {
            get { return (PortAction)Action; }
            set { Action = (int)value; }
        }
        public Result PResult
        {
            get { return (Result)Result; }
            set { Result = (int)value; }
        }
    }
    #endregion

    #region Aligner Action
    /*
    public class MDAlignerAction : MessageBase
    {
        public UInt64 Tick;
        public int ALignerId;
        public int Action;
        public int Result;
        public double Degree;
        public HirataRbAPI.APIEnum.AlignerCommand PAction
        {
            get { return (HirataRbAPI.APIEnum.AlignerCommand)Action; }
            set { Action = (int)value; }
        }
        public Result PResult
        {
            get { return (Result)Result; }
            set { Result = (int)value; }
        }
    }
    */
    #endregion 

    #region API command
    public class MDApiCommand : MessageBase
    {
        public bool IsForce = false;
        public CommandData CommandData = new CommandData();
    };
    #endregion

    #region Port status
    public class MDPortDataReq : MessageBase
    {
        public UInt64 Tick;
        public List<uint> Ports = new List<uint>();
    }

    #endregion

    #region Data Action
    public class MDDataAction : MessageBase
    {
        public UInt64 Tick;
        public int Result;
        public int Action;
        public string Reason = "";
        public string Opid = "";
        public Source Source = new Source();
        public GlassData GlassData = new GlassData();
        public Result PResult
        {
            get { return (Result)Result; }
            set { Result = (int)value; }
        }
        public DataEidtAction PAction
        {
            get { return (DataEidtAction)Action; }
            set { Action = (int)value; }
        }
    }
    #endregion

    #region Buzzer
    public class MDBuzzerControl : MessageBase
    {
        public int Action;
        public int Result;
        public BuzzerAction PAction
        {
            get { return (BuzzerAction)Action; }
            set { Action = (int)value; }
        }
        public Result PResult
        {
            get { return (Result)Result; }
            set { Result = (int)value; }
        }
    }
    #endregion

    #region Robot API Show/Hide
    public class MDRobotApiShow : MessageBase
    {
        public int Tick;
        public int Show;
        public int Result;
        public bool PShow
        {
            get { return Show == 1 ? true : false; }
            set { Show = Convert.ToInt32 (value); }
        }
        public Result PResult
        {
            get { return (Result)Result; }
            set { Result = (int)value; }
        }
    }
    #endregion

    #region Device Home
    public class MDDeviceHome : MessageBase
    {
        public int Tick;
        public int Action;
        public int Result;
        public string Device;
        public HomeAction PAction
        {
            get { return (HomeAction)Action; }
            set { Action = Convert.ToInt32(value); }
        }
        public Result PResult
        {
            get { return (Result)Result; }
            set { Result = (int)value; }
        }
    }
    #endregion

    #region fake
    public class FAKE : MessageBase
    {
        public int Tick;
    }
    #endregion
    public class Source
    {
        public int Target;
        public int Id;
        public int Slot;
        public int Arm;
        public ActionTarget PTarget
        {
            get { return (ActionTarget)Target; }
            set { Target = (int)value; }
        }
        public RobotArm PArm
        {
            get { return (RobotArm)Arm; }
            set { Arm = (int)value; }
        }
    }
}

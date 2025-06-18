using System;
using System.Collections.Generic;
using System.Text;

namespace CommonData.HIRATA
{

    #region Alive And PlcConnect
    public class MDBCAliveAndPlcConnect : MessageBase
    {
        public int cv_PlcConnect;
        public int cv_BcAlive;
        public bool PPlcConnect
        {
            get { return cv_PlcConnect == 1 ? true : false; }
            set { cv_PlcConnect = Convert.ToInt16(value); }
        }
        public bool PBcAlive
        {
            get { return cv_BcAlive == 1 ? true : false; }
            set { cv_BcAlive = Convert.ToInt16(value); }
        }
    }
    #endregion

    #region time adjust
    public class MDBCTimeAdjust : MessageBase
    {
        public int cv_Year;
        public int cv_Mon;
        public int cv_Day;
        public int cv_Hour;
        public int cv_Min;
        public int cv_Sec;
        public int cv_Week;
        public int PYear
        {
            set { cv_Year = value; }
            get {return cv_Year;}
        }
        public int PMon
        {
            set { cv_Mon = value; }
            get { return cv_Mon; }
        }
        public int PDay
        {
            set { cv_Day = value; }
            get { return cv_Day; }
        }
        public int PHour
        {
            set { cv_Hour = value; }
            get { return cv_Hour; }
        }
        public int PMin
        {
            set { cv_Min = value; }
            get { return cv_Min; }
        }
        public int PSec
        {
            set { cv_Sec = value; }
            get { return cv_Sec; }
        }
        public int PWeek
        {
            set { cv_Week = value; }
            get { return cv_Week; }
        }
    }
    #endregion

    #region msg
    public class MDBCMsg : MessageBase
    {
        public int cv_MsgType;
        public int cv_IntervalSec;
        public int cv_Buzzer;
        public string cv_Msg;
        public CommonData.HIRATA.BcMsgType PMsgType
        {
            set { cv_MsgType = (int)value; }
            get { return (BcMsgType)cv_MsgType; }
        }
        public int PIntervalSec
        {
            set { cv_IntervalSec = value; }
            get { return cv_IntervalSec; }
        }
        public bool PBuzzer
        {
            set { cv_Buzzer = Convert.ToInt16( value); }
            get { return cv_Buzzer == 1 ? true : false; }
        }

        public string PMsg
        {
            set { cv_Msg = value; }
            get { return cv_Msg; }
        }
    }
    #endregion

    #region Port Command
    public class MDBCPortCommand : MessageBase
    {
        public int cv_PortCommand;
        public int cv_PortId;
        public int PPortId
        {
            set { cv_PortId = value; }
            get { return cv_PortId; }
        }

        public CommonData.HIRATA.BCPortCommand PPortCommand
        {
            set { cv_PortCommand = (int)value; }
            get { return (BCPortCommand)cv_PortCommand; }
        }
    }
    #endregion

    #region idle delay time
    public class MDBCIdleDelayTime : MessageBase
    {
        public int cv_IdleDelayTIme;
        public int PIdleDelayTime
        {
            set { cv_IdleDelayTIme = (int)value; }
            get { return cv_IdleDelayTIme; }
        }
    }
    #endregion

    #region recipe body query
    public class MDBCRecipeBodyQuery : MessageBase
    {
        public int cv_RecipeId;
        public int PRecipeId
        {
            set { cv_RecipeId = (int)value; }
            get { return cv_RecipeId; }
        }
    }
    #endregion

    #region Index Interval
    public class MDBCIndexInterval : MessageBase
    {
        public int cv_Interval;
        public int PInterval
        {
            set { cv_Interval = (int)value; }
            get { return cv_Interval; }
        }
    }
    #endregion

    #region Recipe exist
    public class MDBCRecipeExist : MessageBase
    {
        public int cv_PortId;
        public List<int> cv_Recipes = new List<int>();
        public int PPortId
        {
            set { cv_PortId = (int)value; }
            get { return cv_PortId; }
        }
    }
    public class MDBCRecipeExistReply : MessageBase
    {
        public int cv_PortId;
        public int cv_NodeNo;
        public List<bool> cv_RecipesExist = new List<bool>();
        public int PPortId
        {
            set { cv_PortId = (int)value; }
            get { return cv_PortId; }
        }
        public int PNode
        {
            set { cv_NodeNo = value; }
            get { return cv_NodeNo; }
        }
    }

    #endregion

    #region EquipmentInfo
    public class MDBCEquipmentInfo : MessageBase
    {
        public int cv_CimMode = 0;
        public int cv_OperationMode = 0;
        public int cv_NodeNo = 0;
        public int cv_Alarm = 0;
        public int cv_Warnning = 0;
        public int cv_IdleDelayTime = 0;
        public int cv_IndexInterval = 0;
        public int cv_CurrentRecipe = 0;
        public int cv_TotalProduct = 0;
        public int cv_TotalDummy = 0;
        public int cv_History = 0;
        public int cv_CheckRecipe = 0;
        public int cv_CheckFoupSeq = 0;
        public int cv_CheckWorkSlot = 0;
        public int cv_CheckWorkId = 0;
        public int cv_Ocr1Enable = 0;
        public int cv_Ocr1Mode = 0;
        public int cv_Ocr2Enable = 0;
        public int cv_Ocr2Mode = 0;
        public OnlineMode PCimMode
        {
            set { cv_CimMode = (int)value; }
            get { return (OnlineMode)(cv_CimMode); }
        }
        public OperationMode POperationMode
        {
            set { cv_OperationMode = (int)value; }
            get { return (OperationMode)(cv_OperationMode); }
        }
        public int PNodeNo
        {
            set { cv_NodeNo = value; }
            get { return cv_NodeNo; }
        }
        public bool PAlarm
        {
            set { cv_Alarm = Convert.ToInt16(value); }
            get { return cv_Alarm == 1 ? true : false; }
        }
        public bool PWarnning
        {
            set { cv_Warnning = Convert.ToInt16(value); }
            get { return cv_Warnning == 1 ? true : false; }
        }
        public int PIdleDelayTime
        {
            set { cv_IdleDelayTime = value; }
            get { return cv_IdleDelayTime; }
        }
        public int PIndexInterval
        {
            set { cv_IndexInterval = value; }
            get { return cv_IndexInterval; }
        }
        public int PCurrentRecipe
        {
            set { cv_CurrentRecipe = value; }
            get { return cv_CurrentRecipe; }
        }
        public int PTotalProduct
        {
            set { cv_TotalProduct = value; }
            get { return cv_TotalProduct; }
        }
        public int PTotalDummy
        {
            set { cv_TotalDummy = value; }
            get { return cv_TotalDummy; }
        }
        public int PHistory
        {
            set { cv_History = value; }
            get { return cv_History; }
        }
        public bool PCheckRecipe
        {
            set { cv_CheckRecipe =Convert.ToInt16(value); }
            get { return cv_CheckRecipe == 1 ? true : false ; }
        }
        public bool PCheckFoupSeq
        {
            set { cv_CheckFoupSeq = Convert.ToInt16(value); }
            get { return cv_CheckFoupSeq == 1 ? true : false; }
        }
        public bool PCheckWorkSlot
        {
            set { cv_CheckWorkSlot = Convert.ToInt16(value); }
            get { return cv_CheckWorkSlot == 1 ? true : false; }
        }
        public bool PCheckWorkId
        {
            set { cv_CheckWorkId = Convert.ToInt16(value); }
            get { return cv_CheckWorkId == 1 ? true : false; }
        }
        public bool POcr1Enable
        {
            set { cv_Ocr1Enable = Convert.ToInt16(value); }
            get { return cv_Ocr1Enable == 1 ? true : false; }
        }
        public bool POcr2Enable
        {
            set { cv_Ocr2Enable = Convert.ToInt16(value); }
            get { return cv_Ocr2Enable == 1 ? true : false; }
        }
        public OCRMode POcr1Mode
        {
            get { return (OCRMode)cv_Ocr1Mode; }
            set { cv_Ocr1Mode = (int)value; }
        }
        public OCRMode POcr2Mode
        {
            get { return (OCRMode)cv_Ocr2Mode; }
            set { cv_Ocr2Mode = (int)value; }
        }
    }
    #endregion

    #region  Transfer report
    public class MDBCWorkTransferReport : MessageBase
    {
        public int cv_Action;
        public int cv_UnitNo;
        public int cv_SlotNo;
        public int cv_PortNo;
        public GlassData GlassData;
        public DataFlowAction PAction
        {
            set { cv_Action = (int)value; }
            get { return (DataFlowAction)cv_Action; }
        }
        public int PUnitNo
        {
            set { cv_UnitNo = value; }
            get { return cv_UnitNo; }
        }
        public int PSlotNo
        {
            set { cv_SlotNo = value; }
            get { return cv_SlotNo; }
        }
        public int PPortNo
        {
            set { cv_PortNo = value; }
            get { return cv_PortNo; }
        }
        public GlassData PGlassData
        {
            set { GlassData = value; }
            get { return GlassData; }
        }
    }
    
    #endregion

    #region Remove report
    public class MDBCRemoveReport : MessageBase
    {
        public int cv_Action;
        public string cv_Opid ="";
        public string cv_Reason ="";
        public GlassData GlassData = new GlassData();
        public GlassData PGlass
        {
            set { GlassData = value; }
            get { return GlassData; }
        }
        public RemoveDataType PAction
        {
            set { cv_Action = (int)value; }
            get { return (RemoveDataType)cv_Action; }
        }
        public string POpid
        {
            set { cv_Opid = value; }
            get { return cv_Opid; }
        }
        public string PReason
        {
            set { cv_Reason = value; }
            get { return cv_Reason; }
        }
    }
    #endregion

    #region Work Data update
    public class MDBCWorkDataUpdateReport : MessageBase
    {
        public GlassData GlassData = new GlassData();
        public GlassData PGlass
        {
            set { GlassData = value; }
            get { return GlassData; }
        }
    }
    #endregion

    #region Last Process Start
    public class MDBCLastProcessStartReport : MessageBase
    {
        public int cv_SlotNo;
        public int cv_PortNo;
        public GlassData GlassData = new GlassData();
        public GlassData PGlass
        {
            set { GlassData = value; }
            get { return GlassData; }
        }
        public int PSlotNo
        {
            set { cv_SlotNo = value; }
            get { return cv_SlotNo; }
        }
        public int PPortNo
        {
            set { cv_PortNo = value; }
            get { return cv_PortNo; }
        }
    }
    #endregion

    #region Recipe body report
    public class MDBCRecipeBodyReport : MessageBase
    {
        public int cv_Action;
        public int cv_Unit;
        public int cv_FlowNo;
        public RecipeItem RecipeItem = new RecipeItem();
        public RecipeBodyReportType PAction
        {
            set { cv_Action = (int)value; }
            get { return (RecipeBodyReportType)cv_Action; }
        }
        public int PUnit
        {
            set { cv_Unit = value; }
            get { return cv_Unit; }
        }
        public RecipeItem PRecipe
        {
            set { RecipeItem = value; }
            get { return (RecipeItem)RecipeItem; }
        }
        public int PFlowNo
        {
            set { cv_FlowNo = value; }
            get { return cv_FlowNo; }
        }
    }
    #endregion

    #region OCR Read
    public class MDBCOcrReadReport : MessageBase
    {
        public GlassData GlassData;
        public string cv_ReadFromOCR;
        public int cv_Match;
        public int cv_OcrMode;
        public bool PMatch
        {
            set { cv_Match = Convert.ToInt16(value); }
            get { return cv_Match == 1 ? true : false; }
        }
        public string PReadFromOCR
        {
            set { cv_ReadFromOCR = value; }
            get { return cv_ReadFromOCR; }
        }
        public OCRMode POcrMode
        {
            set { cv_OcrMode = (int)value; }
            get { return (OCRMode)(cv_OcrMode); }
        }
        public GlassData PGlass
        {
            set { GlassData = value; }
            get { return (GlassData)GlassData; }
        }
    }
    #endregion

    #region Alarm report
    public class MDBCAlarmReportToLGC : MessageBase
    {
        public AlarmItem AlarmItem = new AlarmItem();
        public AlarmItem PAlarm
        {
            set { AlarmItem = value; }
            get { return AlarmItem; }
        }
    }

    #endregion

    #region Data request
    public class MDBCDataRequest : MessageBase
    {
        public int cv_Result;
        public int cv_RequestKey;
        public string cv_WorkId = "";
        public GlassData GlassData = new GlassData();
        public int cv_CimMode = 0;
        public int cv_FoupSeq = 0;
        public int cv_WorkdSlot = 0;
        public int cv_WorkOrderNo = 0;
        public Source Source = new Source();
        public OnlineMode PCimMode
        {
            set { cv_CimMode = (int)value; }
            get { return (OnlineMode)cv_CimMode; }
        }
        public int PFoupSeq
        {
            set { cv_FoupSeq = (int)value; }
            get { return cv_FoupSeq; }
        }
        public int PWorkdSlot
        {
            set { cv_WorkdSlot = (int)value; }
            get { return cv_WorkdSlot; }
        }
        public int PWorkOrderNo
        {
            set { cv_WorkOrderNo = (int)value; }
            get { return cv_WorkOrderNo; }
        }

        public string PWorkId
        {
            set { cv_WorkId = value; }
            get { return cv_WorkId; }
        }
        public DataRequestKey PRequestKey
        {
            set { cv_RequestKey = (int)value; }
            get { return (DataRequestKey)cv_RequestKey; }

        }
        public Result  PResult
        {
            set { cv_Result = (int)value; }
            get { return (Result)cv_Result; }
        }
        public GlassData PGlass
        {
            set { GlassData = value; }
            get { return GlassData; }
        }
    }
    #endregion
}

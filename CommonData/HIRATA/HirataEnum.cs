using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonData.HIRATA
{
    public enum ApiInitSqp { None , waitRemote , waitResetErrorRobot , waitResetErrorRobotFinished , waitResetErrorPort ,waitResetErrorPortFinished,
        waitResetErrorAligner, waitResetErrorAlignerFinished , waitRobotStatus , waitRobotStatusFinished,
        waitRobotRestart , waitRobotRestartFinished,  waitRobotHome , waitRobotHomeFinished, 
        waitPortStatus , waitPortStatusFinished, waitPortHome , waitPortHomeFinished,
        waitAlignerStatus, waitAlignerStatusFinished, waitAlignerHome , waitAlignerHomeFinished, waitBufferStatus , waitBufferStatusFinished,
        waitEfemStatus , waitRobotSpeed,  waitFfuSpeed,
    }
    public enum enSideGroup { None , Right , Left , Both };
    public enum AlignerPreAction { None, WaitHome, AlignerHome , SetToAngle, VuccumOff1 , PutAligner, VuccumOn,
        WaitVuccumOn, FindNotch, WaitFindNotch , OcrConnect , WaitConnect , ReadOcr, WaitReadOct , ToAngle , 
        WaitToAngle , VuccumOff2 , WaitVuccomOff2 , GetAligner,};
    public enum AlignerIJPAction { None, VuccumOff1, AlignerHome, PutAligner , VuccumOn, Alignment , GetAligner, };
    public enum RobotSideBitAddressOffset
    {
        Active_Standby = 0, Load_Only_Reply, Unload_Only_Reply, Exchange_Reply, Receipt_Complete, Interlock_2,
        Work_Presence_Upper_Arm
            , Work_Presence_Low_Arm, Spare, Robot_Delivery_Ready, Spare1, Stop_Supply, Force_Complete_Request, Force_Initial_Request,
    }
    public enum EqSideBitAddressOffset
    {
        Equipment_Ready = 0, Load_Only_Req, Unload_Only_Req, Exchange_Req, Transfer_Complete, Interlock_1, Transfer_Start,
        Stop_Supply, Force_Initial_Complete, Work_Presence, Spare1, Stage_Delivery_Ready, Spare3, Spare4,
    };
    public enum GifTimeout { All = 0, T1 = 2047, T3 = 2048, ForceIni = 2049, ForceCom = 2050, };

    public enum DataRequestKey { ByWorkNo = 1 , ByWorkId ,}
    public enum DataCheckRule { Recipe = 1, id = 2, slot = 4, seq = 8, };
    public enum WorkType { Engineer, Dummy, Test, Production, Develop, Monitoring, }
    public enum ProductCategory{None , Wafer = 1, Glass, Mask, Dummy, };
    public enum GlassJudge { OK, NG, Repair, Rework, Scrap, };
    public enum OCRRead { NotNeed, Need, };
    public enum OCRResult {None = -1 ,  OK, Mismatch, Fail, };
    public enum AssambleNeed { NotNeed, Need, };
    public enum AssambleResult { NotComplete, Complete, };
    public enum BcMsgType { Interval, Continue };
    public enum BCPortCommand
    {
        NoAction = 0, End, Cancel, Finish, Retry, Loader, Unloader, LDUD, Start, Reserve, Pause, Resume,
    }
    public enum OCRMode { None = -1, SkipRead = 0, ErrorSkip, ErrorHold, ErrorReturn, };
    public enum RemoveDataType { WorkRemove = 1, TakeOut, };
    public enum RecipeBodyReportType { New = 1, Delete, Modity, Query, }
    public enum EqInterFaceType { None, Load, Unload, Exchange, };
    public enum ApiFoupStatus { None, FoupPlace, FoupRemove, };
    public enum OdfFlow {None=0, Flow1=1, Flow2, Flow3, Flow4, Flow5 ,  Flow6, Flow7 , Flow8, Flow9,
        Flow10,
        Flow11,
        Flow12,
        Flow13,
        Flow14,
        Flow15,
        Flow16,
        Flow17,
        Flow18,
        Flow19,
        Flow20,
        Flow21,
        Flow22,
        Flow23,
        Flow24,
        Flow25,
        Flow26,
    };
    public enum EqNode { SDP1 = 3  , SDP2 =4 , SDP3 = 5, SDP4 =6 , SDP5 = 7, SDP6 = 8, AOI=9,
         IJP1 =10 , IJP2=11 , VAS1 =12 , VAS2=13 ,UV1 = 14 , UV2 =15};
    public enum EqGifTimeChartId
    {
        None = 0,
        TIMECHART_ID_SDP1 = 1,
        TIMECHART_ID_SDP2 = 2,
        TIMECHART_ID_SDP3 = 3,
        TIMECHART_ID_SDP4 = 4 ,
        TIMECHART_ID_SDP5 = 5 ,
        TIMECHART_ID_SDP6 = 6 ,

        TIMECHART_ID_AOI1 = 7,

        TIMECHART_ID_IJP1 = 8,
        TIMECHART_ID_IJP2 = 9,

        TIMECHART_ID_VAS1_UP = 10,
        TIMECHART_ID_VAS1_DOWN = 11,
        TIMECHART_ID_VAS2_UP = 12,
        TIMECHART_ID_VAS2_DOWN = 13,

        TIMECHART_ID_UV_1 = 14,
        TIMECHART_ID_UV_2 = 15,
    }
    public enum EqId
    {
        None = 0,
        SDP1 = 1,
        SDP2 = 2,
        SDP3 = 3,
        SDP4 = 4,
        SDP5 = 5,
        SDP6 = 6,
        AOI1 = 7,
        IJP1 = 8,
        IJP2 = 9,
        VAS1 = 10,
        VAS2 = 11,
        UV_1 = 12,
        UV_2 = 13,
    }
    public enum BufferSlotType {None ,  Wafer, Glass , ReverseForRework  };
    public enum AllDevice
    {
        None=0,
        LP=1,
        Buffer1,
        Buffer2,
        UP,
        Aligner1,
        Aligner2,
        SDP1 ,
        SDP2 ,
        SDP3 ,
        SDP4 ,
        SDP5 ,
        SDP6 ,
        AOI1 ,
        IJP1 ,
        IJP2 ,
        VAS1 ,
        VAS2 ,
        UV_1 ,
        UV_2 ,
    }
    public enum SignalTowerColor { All , Red , Yellow , Green , Blue, };
    public enum SignalTowerControl { On , Off , Flash , };
    public enum RobotJobAction { None, preView, Clean,DropTop };

    public enum InitialAllDevice
    {
        None=0,
        LP1,
        LP2,
        LP3,
        LP4,
        Buffer1_Left,
        Buffer2_Mid,
        UP5,
        UP6,
        UP7,
        UP8,
        Aligner1_Left,
        Aligner2_Right,
        RB1,
        RB2,
    }
}

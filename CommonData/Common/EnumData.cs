using System;
using System.Collections.Generic;
using System.Text;

namespace CommonData.HIRATA
{
    public enum FdModule { None , UI , LGC , CIM ,};
    public enum LogLevelType { Error = 1, Warning = 2, UI = 3, MsgName = 4, MsgArg = 5, General = 6, Detail = 7, 
        NormalFunctionInOut = 8, TimerFunction = 9, StateMachine = 10, RawData = 11, };

    public enum SamekLogLevelType { Error = 1, Warning = 1, UI = 1, MsgName = 2, MsgArg = 3, General = 3, Detail = 4, NormalFunctionInOut = 4, TimerFunction = 5, StateMachine = 5, RawData = 6 };
    public enum OperationMode {None, Manual , Auto , WaitManual };
    public enum ProcessFlag {  NotNeed , Need , };
    //public enum UserPermission { None, OP = 1, Engineer, Root };
//    public enum AlarmLevele { None, Light, Serious };
//    public enum AlarmStatus { Occur=1, Clean=0 };
    public enum SideGeoupEnable { None , Enable = 1 , Disable = 2};
    public enum EquipmentStatus { None , Run = 2 , Down =3 , Idle = 1 , PM =4 , WaitIdle = 5, Stop=6};
    public enum EquipmentInlineMode { None = - 1 ,WaitRemote , Remote = 1 , WaitLocal , Local ,  Offline };
    public enum EquipmentSubStatus {None, Warning = 1 , Stop, Init, Standby };
    public enum PortAction {None , Clamp =1 , Unclamp,  Cancel, Rewok};
    public enum PortStaus { None, LDRQ, LDCM, UDRQ, UDCM };

    public enum LotStatus { Empty, FoupSensorOn, MappingEnd, WaitReserve, Reserved, WaitCommand, Process, ProcessEnd 
        , Cancel , Abort , Pause };
    
    public enum PortMode { None, Both , Loader, Unloader, };
    public enum PortType { None, OK, NG, MIX };
    public enum PortEnable { Disable, Enable, None };
    public enum PortAgv { None, AGV, MGV };
    public enum PortClamp { Unclamp, Clamp, None };
    public enum PortHasCst { Empty, Has, None };
    public enum RobotAction
    {None, Put = 1, PutWait, Get, GetWait, TopGet, TopGetWait, TopPut, TopPutWait, Exchange, HighExchange, Initialize, ActionComplete,
    GetStandbyArmExtend, PutStandbyArmExtend, TopPutStandbyArmExtend, PutGetAligner};
    public enum ActionTarget { Port = 1 , Eq , Buffer , Aligner , Robot };
    public enum RobotArm { rbaUp = 2, rbaDown = 1 , rabNone = 0 , rbaBoth=3};
    public enum DataFlowAction { None , Fetch , Store , Receive , Send ,  };
    public enum OnlineMode {None,  Control = 1 , Monitor , Local , Offline};
    public enum FunInOut {None,   Enter , Leave};
    public enum DataEidtAction { None , Add , Edit , Del , SetCur};
    public enum Result { None , OK , NG};
    public enum UserChoice { None , Yes , No};
    public enum InitialAction { None , Initial , Complete};
    public enum HomeAction {  None , Hone , Complete};
    public enum BuzzerAction { None , ON , OFF};
    public enum MmfEventClientEventType { etRequest = 1, etReply = 2, etNotify = 3 };

}

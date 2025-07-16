using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using KgsCommon;

namespace CommonData.HIRATA
{
    public class APIEnum
    {
        public enum CommandType { None, API, Common, RFID, LoadPort, E84, Robot, Aligner, IO, Alignment, Barcode, OCR, Event };
        public enum EventCommand
        {
            None, FoupPlace, FoupRemove, OperatorAccessButtonClick, OperatorAccessButton2Click, LPPowerOn, AirDropped, FoupPresence,
            Pressure, Vacuum, Ionizer1, Ionizer2, Ionizer3, Ionizer4, Ionizer5 ,Ionizer6, Ionizer7, Ionizer8 ,
            FFU1, FFU2, FFU3, FFU4, FFU5, FFU6, FFU7, FFU8, FFU9, FFU10, FFU11, RobotMode, RobotEnable, Door, EMO, Power,
            CheckBernoulliIOStatus, ERROR, GetStatus, VasTopPutEnd
        };

        public enum APICommand { None, Version, Remote, Local, CurrentMode, Hide, Show, };
        public enum CommonCommand { None, Home = 7, ResetError, GetStatus, };
        public enum RfidCommand { None, ReadFoupID = 10, };
        public enum LoadPortCommand
        {
            None, Clamp = 11, UnClamp, Load, Unload, Map, GetWaferSlot, GetWaferSlot2, GetWaferThickness, GetWaferPosition, LEDLoad,
            LEDUnLoad, LEDStatus1, LEDStatus2, GetLEDStatus, SetOperatorAccessButton, GetZAxisPos, SetType, SetMapp, GetMapp,
            GetProtrusionSensor, SetSlotType, GetSlotType,
        }
        public enum E84Command
        {
            None, LinkTest = 33, Enable, Auto, Manual, Reset, SetESOn, SetESOff, SetTimeOut,
            GetFoupSensorStatus, GetControllerErrorCode, GetSensorOutputStatus, GetSensorInputStatus, GetDIStatus,
        }
        public enum RobotCommand
        {
            None, Stop = 46, ReStart, SetRobotSpeed, ReadPosition, WaferGet, WaferPut, GetStandby, PutStandby, TopWaferGet, TopWaferPut, TopGetStandby,
            TopPutStandby, BernoulliOn, BernoulliOff, EdgeGripOn, EdgeGripOff, VacuumOn, VacuumOff, SetRobotWaferInch, GetRobotWaferInch,
            ArmSafetyPosition, RobotMapping, GetRobotMappingResult, GetRobotMappingResult2, GetRobotMappingErrorResult,
            GetRobotMappingErrorResult2, CheckArmOnSafetyPos, CheckWaferPresence, GetStandbyArmExtend, PutStandbyArmExtend, TopPutStandbyArmExtend, 
        }
        public enum AlignerCommand
        {
            None, Alignment = 74, ToAngle, GetAlignerDegree, SetAlignerDegree,
            AlignerVacuum, SetAlignerWaferType, GetAlignerWaferType, FindNotch, SetIDReaderDegree,
            GetIDReaderDegree,
        }
        public enum IoCommand { None, SignalTower = 84, SetFFUVoltage, GetPressureDifference, Buzzer, GetEFEMInterlock, SetEFEMInterlock,
        GetEQInterlock, ShutterDoor, GetShutterDoorStatus, GetBufferStatus1 ,GetBufferStatus2  , GetBufferProtrusionSensor , }
        public enum AlignmentCommand { None, AlignmentClamp = 90, AlignmentUnclamp, DoAlignment, GetStaticElectricityValue, }
        public enum BarcodeCommand { None, BarcodeRead = 94, }
        public enum OcrCommand { None, Read = 95, Connect };
        public enum CommnadDevice { None, API, ALL, RFID, P, Robot, Aligner, E84, IO, Alignment, Barcode, OCRReader, Stage, EFEM , Buffer  };
    };
    public class CommandData
    {
        public static char STX = (char)0x02;
        public static char ETX = (char)0x03;

        public int cv_CommandType = 0;
        public int cv_AlignerCommand = 0;
        public int cv_AlignmentCommand = 0;
        public int cv_ApiCommand = 0;
        public int cv_BarcodeCommand = 0;
        public int cv_CommonCommand = 0;
        public int cv_E84Command = 0;
        public int cv_IoCommand = 0;
        public int cv_LoadPortCommand = 0;
        public int cv_OcrCommand = 0;
        public int cv_RfidCommand = 0;
        public int cv_RobotCommand = 0;
        public int cv_EventCommand = 0;
        public int cv_CommandDevice = 0;

        public APIEnum.CommandType PCommandType
        {
            get { return (APIEnum.CommandType)cv_CommandType; }
            set { cv_CommandType = Convert.ToInt16(value); }
        }

        public APIEnum.AlignerCommand PAlignerCommand
        {
            get { return (APIEnum.AlignerCommand)cv_AlignerCommand; }
            set { cv_AlignerCommand = Convert.ToInt16(value); }
        }

        public APIEnum.AlignmentCommand PAlignmentCommand
        {
            get { return (APIEnum.AlignmentCommand)cv_AlignmentCommand; }
            set { cv_AlignmentCommand = Convert.ToInt16(value); }
        }
        public APIEnum.APICommand PApiCommand
        {
            get { return (APIEnum.APICommand)cv_ApiCommand; }
            set { cv_ApiCommand = Convert.ToInt16(value); }
        }
        public APIEnum.BarcodeCommand PBarcodeCommand
        {
            get { return (APIEnum.BarcodeCommand)cv_BarcodeCommand; }
            set { cv_BarcodeCommand = Convert.ToInt16(value); }
        }
        public APIEnum.CommonCommand PCommonCommand
        {
            get { return (APIEnum.CommonCommand)cv_CommonCommand; }
            set { cv_CommonCommand = Convert.ToInt16(value); }
        }
        public APIEnum.E84Command PE84Command
        {
            get { return (APIEnum.E84Command)cv_E84Command; }
            set { cv_E84Command = Convert.ToInt16(value); }
        }
        public APIEnum.IoCommand PIoCommand
        {
            get { return (APIEnum.IoCommand)cv_IoCommand; }
            set { cv_IoCommand = Convert.ToInt16(value); }
        }
        public APIEnum.LoadPortCommand PLoadPortCommand
        {
            get { return (APIEnum.LoadPortCommand)cv_LoadPortCommand; }
            set { cv_LoadPortCommand = Convert.ToInt16(value); }
        }
        public APIEnum.OcrCommand POcrCommand
        {
            get { return (APIEnum.OcrCommand)cv_OcrCommand; }
            set { cv_OcrCommand = Convert.ToInt16(value); }
        }
        public APIEnum.RfidCommand PRfidCommand
        {
            get { return (APIEnum.RfidCommand)cv_RfidCommand; }
            set { cv_RfidCommand = Convert.ToInt16(value); }
        }
        public APIEnum.RobotCommand PRobotCommand
        {
            get { return (APIEnum.RobotCommand)cv_RobotCommand; }
            set { cv_RobotCommand = Convert.ToInt16(value); }
        }
        public APIEnum.EventCommand PEventCommand
        {
            get { return (APIEnum.EventCommand)cv_EventCommand; }
            set { cv_EventCommand = Convert.ToInt16(value); }
        }

        public APIEnum.CommnadDevice PCommandDevice
        {
            get { return (APIEnum.CommnadDevice)cv_CommandDevice; }
            set { cv_CommandDevice = Convert.ToInt16(value); }
        }
        public int cv_DeviceId;
        public List<string> cv_ParaList = new List<string>();
        public string cv_CommandTxt = "";
        public List<string> cv_ReplyParaList = new List<string>();
        public int cv_ReturnCode = 0;
        private Match match = Match.Empty;
        private KDateTime Time = SysUtils.Now();
        public KDateTime cv_Time
        {
            get { return Time; }
            set { Time = value; }
        }
        public CommandData()
        {
        }
        public CommandData(APIEnum.CommandType m_Type, string m_CommandEnum, APIEnum.CommnadDevice m_Device, int m_DeviceId, List<string> m_ParaList = null)
        {
            if (ParseCommandStrToEnum(m_Type, m_CommandEnum))
            {
                PCommandDevice = m_Device;
                cv_DeviceId = m_DeviceId;
                cv_ParaList = m_ParaList;
            }
        }
        public CommandData(List<string> m_SplitList)
        {
            //parse Command type , APIEnum.CommnadType
            if (ParseCommandType(m_SplitList))
            {
                ParseReply(m_SplitList, this.PCommandType);
            }
            else
            {
                PCommandType = APIEnum.CommandType.Event;
                PEventCommand = APIEnum.EventCommand.ERROR;
                ParseReply(m_SplitList, this.PCommandType);
                /*
                if (m_SplitList.Count > 3)
                {
                    int return_code = 0;
                    if (int.TryParse(m_SplitList[0], out return_code))
                    {
                        if (return_code != 0)
                        {
                            PCommandType = APIEnum.CommandType.Event;
                            PEventCommand = APIEnum.EventCommand.ERROR;
                            //this should be EFEM Device Error Event , Error code,Event,EFEM,Error Description.
                            Match p_match = Regex.Match(m_SplitList[2], @"(P\d)", RegexOptions.IgnoreCase);
                            Match rb_match = Regex.Match(m_SplitList[2], @"(robot\d)", RegexOptions.IgnoreCase);
                            Match ali_match = Regex.Match(m_SplitList[2], @"(aligner\d)", RegexOptions.IgnoreCase);
                            if (p_match.Success)
                            {
                                PCommandDevice = APIEnum.CommnadDevice.P;
                                if (int.TryParse(match.Value.Substring(match.Value.Length - 1), out cv_DeviceId))
                                {
                                    cv_ParaList.Clear();
                                    cv_ParaList.Add(m_SplitList[3]);
                                }
                            }
                            else if (rb_match.Success)
                            {
                                PCommandDevice = APIEnum.CommnadDevice.Robot;
                                if (int.TryParse(match.Value.Substring(match.Value.Length - 1), out cv_DeviceId))
                                {
                                    cv_ParaList.Clear();
                                    cv_ParaList.Add(m_SplitList[3]);
                                }
                            }
                            else if (ali_match.Success)
                            {
                                PCommandDevice = APIEnum.CommnadDevice.Aligner;
                                if (int.TryParse(match.Value.Substring(match.Value.Length - 1), out cv_DeviceId))
                                {
                                    cv_ParaList.Clear();
                                    cv_ParaList.Add(m_SplitList[3]);
                                }
                            }
                        }
                    }
                }
                */
            }
        }
        private bool ParseCommandType(APIEnum.CommandType m_Type, Type m_EnumType, string m_TypeStr)
        {
            bool rtn = false;
            if (m_EnumType.IsEnum)
            {
                List<string> commands = Enum.GetNames(m_EnumType).ToList<string>();
                if (commands.Contains(m_TypeStr.Trim()))
                {
                    if (Enum.IsDefined(m_EnumType, m_TypeStr))
                    {
                        if (Regex.Match(m_EnumType.Name, @"api", RegexOptions.IgnoreCase).Success)
                        {
                            PCommandType = m_Type;
                            PApiCommand = (APIEnum.APICommand)Enum.Parse(typeof(APIEnum.APICommand), m_TypeStr, true);
                            rtn = true;
                        }
                        else if (Regex.Match(m_EnumType.Name, @"common", RegexOptions.IgnoreCase).Success)
                        {
                            PCommandType = m_Type;
                            PCommonCommand = (APIEnum.CommonCommand)Enum.Parse(typeof(APIEnum.CommonCommand), m_TypeStr, true);
                            rtn = true;
                        }
                        else if (Regex.Match(m_EnumType.Name, @"RFID", RegexOptions.IgnoreCase).Success)
                        {
                            PCommandType = m_Type;
                            PRfidCommand = (APIEnum.RfidCommand)Enum.Parse(typeof(APIEnum.RfidCommand), m_TypeStr, true);
                            rtn = true;
                        }
                        else if (Regex.Match(m_EnumType.Name, @"LoadPort", RegexOptions.IgnoreCase).Success)
                        {
                            PCommandType = m_Type;
                            PLoadPortCommand = (APIEnum.LoadPortCommand)Enum.Parse(typeof(APIEnum.LoadPortCommand), m_TypeStr, true);
                            rtn = true;
                        }
                        else if (Regex.Match(m_EnumType.Name, @"E84", RegexOptions.IgnoreCase).Success)
                        {
                            PCommandType = m_Type;
                            PE84Command = (APIEnum.E84Command)Enum.Parse(typeof(APIEnum.E84Command), m_TypeStr, true);
                            rtn = true;
                        }
                        else if (Regex.Match(m_EnumType.Name, @"Robot", RegexOptions.IgnoreCase).Success)
                        {
                            PCommandType = m_Type;
                            PRobotCommand = (APIEnum.RobotCommand)Enum.Parse(typeof(APIEnum.RobotCommand), m_TypeStr, true);
                            rtn = true;
                        }
                        else if (Regex.Match(m_EnumType.Name, @"Aligner", RegexOptions.IgnoreCase).Success)
                        {
                            PCommandType = m_Type;
                            PAlignerCommand = (APIEnum.AlignerCommand)Enum.Parse(typeof(APIEnum.AlignerCommand), m_TypeStr, true);
                            rtn = true;
                        }
                        else if (Regex.Match(m_EnumType.Name, @"IO", RegexOptions.IgnoreCase).Success)
                        {
                            PCommandType = m_Type;
                            PIoCommand = (APIEnum.IoCommand)Enum.Parse(typeof(APIEnum.IoCommand), m_TypeStr, true);
                            rtn = true;
                        }
                        else if (Regex.Match(m_EnumType.Name, @"Alignment", RegexOptions.IgnoreCase).Success)
                        {
                            PCommandType = m_Type;
                            PAlignmentCommand = (APIEnum.AlignmentCommand)Enum.Parse(typeof(APIEnum.AlignmentCommand), m_TypeStr, true);
                            rtn = true;
                        }
                        else if (Regex.Match(m_EnumType.Name, @"Barcode", RegexOptions.IgnoreCase).Success)
                        {
                            PCommandType = m_Type;
                            PBarcodeCommand = (APIEnum.BarcodeCommand)Enum.Parse(typeof(APIEnum.BarcodeCommand), m_TypeStr, true);
                            rtn = true;
                        }
                        else if (Regex.Match(m_EnumType.Name, @"OCR", RegexOptions.IgnoreCase).Success)
                        {
                            PCommandType = m_Type;
                            POcrCommand = (APIEnum.OcrCommand)Enum.Parse(typeof(APIEnum.OcrCommand), m_TypeStr, true);
                            rtn = true;
                        }
                        else if (Regex.Match(m_EnumType.Name, @"Event", RegexOptions.IgnoreCase).Success)
                        {
                            PCommandType = m_Type;
                            PEventCommand = (APIEnum.EventCommand)Enum.Parse(typeof(APIEnum.EventCommand), m_TypeStr, true);
                            rtn = true;
                        }
                    }
                }
            }
            return rtn;
        }
        private bool ParseCommandType(List<string> m_TypeStr)
        {
            //None , API, Commond, RFID, LoadPort, E84, Robot, Aligner, IO, Alignment, Barcode, OCR, Event };
            bool rtn = false;
            if (ParseCommandType(APIEnum.CommandType.API, typeof(APIEnum.APICommand), m_TypeStr[1]))
            {
                rtn = true;
            }
            else if (ParseCommandType(APIEnum.CommandType.Common, typeof(APIEnum.CommonCommand), m_TypeStr[1]))
            {
                rtn = true;
            }
            else if (ParseCommandType(APIEnum.CommandType.RFID, typeof(APIEnum.RfidCommand), m_TypeStr[1]))
            {
                rtn = true;
            }
            else if (ParseCommandType(APIEnum.CommandType.LoadPort, typeof(APIEnum.LoadPortCommand), m_TypeStr[1]))
            {
                rtn = true;
            }
            else if (ParseCommandType(APIEnum.CommandType.E84, typeof(APIEnum.E84Command), m_TypeStr[1]))
            {
                rtn = true;
            }
            else if (ParseCommandType(APIEnum.CommandType.Robot, typeof(APIEnum.RobotCommand), m_TypeStr[1]))
            {
                rtn = true;
            }
            else if (ParseCommandType(APIEnum.CommandType.Aligner, typeof(APIEnum.AlignerCommand), m_TypeStr[1]))
            {
                rtn = true;
            }
            else if (ParseCommandType(APIEnum.CommandType.IO, typeof(APIEnum.IoCommand), m_TypeStr[1]))
            {
                rtn = true;
            }
            else if (ParseCommandType(APIEnum.CommandType.Alignment, typeof(APIEnum.AlignmentCommand), m_TypeStr[1]))
            {
                rtn = true;
            }
            else if (ParseCommandType(APIEnum.CommandType.Barcode, typeof(APIEnum.BarcodeCommand), m_TypeStr[1]))
            {
                rtn = true;
            }
            else if (ParseCommandType(APIEnum.CommandType.OCR, typeof(APIEnum.OcrCommand), m_TypeStr[1]))
            {
                rtn = true;
            }
            else if (ParseCommandType(APIEnum.CommandType.Event, typeof(APIEnum.EventCommand), m_TypeStr[3]))
            {
                rtn = true;
            }
            return rtn;
        }
        private bool ParseCommandStrToEnum(APIEnum.CommandType m_Type, string m_CommandEnum)
        {
            bool rtn = false;
            PCommandType = m_Type;
            switch (m_Type)
            {
                case APIEnum.CommandType.Aligner:
                    {
                        APIEnum.AlignerCommand tmp = APIEnum.AlignerCommand.None;
                        if (Enum.TryParse(m_CommandEnum, true, out tmp))
                        {
                            this.PAlignerCommand = tmp;
                            rtn = true;
                        }
                        break;
                    }
                case APIEnum.CommandType.Alignment:
                    {
                        APIEnum.AlignmentCommand tmp = APIEnum.AlignmentCommand.None;
                        if (Enum.TryParse(m_CommandEnum, true, out tmp))
                        {
                            this.PAlignmentCommand = tmp;
                            rtn = true;
                        }
                        break;
                    }
                case APIEnum.CommandType.API:
                    {
                        APIEnum.APICommand tmp = APIEnum.APICommand.None;
                        if (Enum.TryParse(m_CommandEnum, true, out tmp))
                        {
                            this.PApiCommand = tmp;
                            rtn = true;
                        }
                        break;
                    }
                case APIEnum.CommandType.Barcode:
                    {
                        APIEnum.BarcodeCommand tmp = APIEnum.BarcodeCommand.None;
                        if (Enum.TryParse(m_CommandEnum, true, out tmp))
                        {
                            this.PBarcodeCommand = tmp;
                            rtn = true;
                        }
                        break;
                    }
                case APIEnum.CommandType.Common:
                    {
                        APIEnum.CommonCommand tmp = APIEnum.CommonCommand.None;
                        if (Enum.TryParse(m_CommandEnum, true, out tmp))
                        {
                            this.PCommonCommand = tmp;
                            rtn = true;
                        }
                        break;
                    }
                case APIEnum.CommandType.E84:
                    {
                        APIEnum.E84Command tmp = APIEnum.E84Command.None;
                        if (Enum.TryParse(m_CommandEnum, true, out tmp))
                        {
                            PE84Command = tmp;
                            rtn = true;
                        }
                        break;
                    }
                case APIEnum.CommandType.IO:
                    {
                        APIEnum.IoCommand tmp = APIEnum.IoCommand.None;
                        if (Enum.TryParse(m_CommandEnum, true, out tmp))
                        {
                            this.PIoCommand = tmp;
                            rtn = true;
                        }
                        break;
                    }
                case APIEnum.CommandType.LoadPort:
                    {
                        APIEnum.LoadPortCommand tmp = APIEnum.LoadPortCommand.None;
                        if (Enum.TryParse(m_CommandEnum, true, out tmp))
                        {
                            this.PLoadPortCommand = tmp;
                            rtn = true;
                        }
                        break;
                    }
                case APIEnum.CommandType.OCR:
                    {
                        APIEnum.OcrCommand tmp = APIEnum.OcrCommand.None;
                        if (Enum.TryParse(m_CommandEnum, true, out tmp))
                        {
                            this.POcrCommand = tmp;
                            rtn = true;
                        }
                        break;
                    }
                case APIEnum.CommandType.RFID:
                    {
                        APIEnum.RfidCommand tmp = APIEnum.RfidCommand.None;
                        if (Enum.TryParse(m_CommandEnum, true, out tmp))
                        {
                            this.PRfidCommand = tmp;
                            rtn = true;
                        }
                        break;
                    }
                case APIEnum.CommandType.Robot:
                    {
                        APIEnum.RobotCommand tmp = APIEnum.RobotCommand.None;
                        if (Enum.TryParse(m_CommandEnum, true, out tmp))
                        {
                            this.PRobotCommand = tmp;
                            rtn = true;
                        }
                    }
                    break;
            };
            return rtn;
        }
        private bool ParseReply(List<string> m_SplitList, APIEnum.CommandType m_Type)
        {
            bool rtn = false;
            if (m_SplitList.Count >= 3)
            {
                if (this.PCommandType == APIEnum.CommandType.Event)
                {
                    cv_ReturnCode = Int16.Parse(m_SplitList[0]);
                    match = Match.Empty;
                    match = Regex.Match(m_SplitList[2], @"\D*", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        if (Enum.IsDefined(typeof(APIEnum.CommnadDevice), match.Value))
                        {
                            this.PCommandDevice = (APIEnum.CommnadDevice)Enum.Parse(typeof(APIEnum.CommnadDevice), match.Value);
                        }
                    }
                    match = Match.Empty;
                    match = Regex.Match(m_SplitList[2], @"([0-9])", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        this.cv_DeviceId = Int16.Parse(match.Value);
                    }
                    cv_ReplyParaList.Clear();
                    if (m_SplitList.Count > 3)
                    {
                        cv_ReplyParaList.AddRange(m_SplitList.GetRange(3, m_SplitList.Count - 3));
                    }
                }
                else
                {
                    cv_ReturnCode = Int16.Parse(m_SplitList[0]);
                    match = Match.Empty;
                    match = Regex.Match(m_SplitList[2], @"\D*", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        if (Enum.IsDefined(typeof(APIEnum.CommnadDevice), match.Value))
                        {
                            this.PCommandDevice = (APIEnum.CommnadDevice)Enum.Parse(typeof(APIEnum.CommnadDevice), match.Value);
                        }
                    }
                    match = Match.Empty;
                    match = Regex.Match(m_SplitList[2], @"([0-9])", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        this.cv_DeviceId = Int16.Parse(match.Value);
                    }
                    cv_ReplyParaList.Clear();
                    if (m_SplitList.Count > 3)
                    {
                        cv_ReplyParaList.AddRange(m_SplitList.GetRange(3, m_SplitList.Count - 3));
                    }
                }
            }
            return rtn;
        }
        public string GetCommandStr()
        {
            string command = "";
            switch (this.PCommandType)
            {
                case APIEnum.CommandType.Aligner:
                    command = PAlignerCommand.ToString();
                    break;
                case APIEnum.CommandType.Alignment:
                    command = PAlignmentCommand.ToString();
                    break;
                case APIEnum.CommandType.API:
                    command = PApiCommand.ToString();
                    break;
                case APIEnum.CommandType.Barcode:
                    command = PBarcodeCommand.ToString();
                    break;
                case APIEnum.CommandType.Common:
                    command = PCommonCommand.ToString();
                    break;
                case APIEnum.CommandType.E84:
                    command = PE84Command.ToString();
                    break;
                case APIEnum.CommandType.IO:
                    command = PIoCommand.ToString();
                    break;
                case APIEnum.CommandType.LoadPort:
                    command = PLoadPortCommand.ToString();
                    break;
                case APIEnum.CommandType.OCR:
                    command = POcrCommand.ToString();
                    break;
                case APIEnum.CommandType.RFID:
                    command = PRfidCommand.ToString();
                    break;
                case APIEnum.CommandType.Robot:
                    command = PRobotCommand.ToString();
                    break;
            };
            string rtn = STX.ToString() + command;
            rtn += "," + PCommandDevice;
            if (cv_DeviceId != 0) rtn += cv_DeviceId.ToString();
            if (cv_ParaList != null)
            {
                for (int i = 0; i < cv_ParaList.Count; i++)
                {
                    rtn += "," + cv_ParaList[i];
                }
            }
            rtn += ETX.ToString();
            return rtn;
        }
        public int GetAlarmCode()
        {
            int code = 0;
            switch(this.PCommandType)
            {
                case APIEnum.CommandType.Aligner:
                    code = (int)PAlignerCommand + (int)PCommandDevice * 100;
                    break;
                case APIEnum.CommandType.Alignment:
                    code = (int)PAlignmentCommand + (int)PCommandDevice * 100;
                    break;
                case APIEnum.CommandType.API:
                    code = (int)PApiCommand + (int)PCommandDevice * 100;
                    break;
                case APIEnum.CommandType.Barcode:
                    code = (int)PBarcodeCommand + (int)PCommandDevice * 100;
                    break;
                case APIEnum.CommandType.Common:
                    code = (int)PCommonCommand + (int)PCommandDevice * 100;
                    break;
                case APIEnum.CommandType.E84:
                    code = (int)PE84Command + (int)PCommandDevice * 100;
                    break;
                case APIEnum.CommandType.Event:
                    code = (int)PEventCommand + (int)PCommandDevice * 100;
                    break;
                case APIEnum.CommandType.IO:
                    code = (int)PIoCommand + (int)PCommandDevice * 100;
                    break;
                case APIEnum.CommandType.LoadPort:
                    code = (int)PLoadPortCommand + (int)PCommandDevice * 100;
                    break;
                case APIEnum.CommandType.OCR:
                    code = (int)POcrCommand + (int)PCommandDevice * 100;
                    break;
                case APIEnum.CommandType.RFID:
                    code = (int)PRfidCommand + (int)PCommandDevice * 100;
                    break;
                case APIEnum.CommandType.Robot:
                    code = (int)PRobotCommand + (int)PCommandDevice * 100;
                    break;
            };
            return code;
        }
        public override bool Equals(object Obj)
        {
            CommandData m_OtherCommand = (CommandData)Obj;
            bool rtn = false;
            if (m_OtherCommand.PCommandType == this.PCommandType)
            {
                rtn = true;
                switch (this.PCommandType)
                {
                    case APIEnum.CommandType.Aligner:
                        if (this.PAlignerCommand != m_OtherCommand.PAlignerCommand)
                            rtn = false;
                        break;
                    case APIEnum.CommandType.Alignment:
                        if (this.PAlignmentCommand != m_OtherCommand.PAlignmentCommand)
                            rtn = false;
                        break;
                    case APIEnum.CommandType.API:
                        if (this.PApiCommand != m_OtherCommand.PApiCommand)
                            rtn = false;
                        break;
                    case APIEnum.CommandType.Barcode:
                        if (this.PBarcodeCommand != m_OtherCommand.PBarcodeCommand)
                            rtn = false;
                        break;
                    case APIEnum.CommandType.Common:
                        if (this.PCommonCommand != m_OtherCommand.PCommonCommand)
                            rtn = false;
                        break;
                    case APIEnum.CommandType.E84:
                        if (this.PE84Command != m_OtherCommand.PE84Command)
                            rtn = false;
                        break;
                    case APIEnum.CommandType.IO:
                        if (this.PIoCommand != m_OtherCommand.PIoCommand)
                            rtn = false;
                        break;
                    case APIEnum.CommandType.LoadPort:
                        if (this.PLoadPortCommand != m_OtherCommand.PLoadPortCommand)
                            rtn = false;
                        break;
                    case APIEnum.CommandType.OCR:
                        if (this.POcrCommand != m_OtherCommand.POcrCommand)
                            rtn = false;
                        break;
                    case APIEnum.CommandType.RFID:
                        if (this.PRfidCommand != m_OtherCommand.PRfidCommand)
                            rtn = false;
                        break;
                    case APIEnum.CommandType.Robot:
                        if (this.PRobotCommand != m_OtherCommand.PRobotCommand)
                            rtn = false;
                        break;
                };
            }
            if (rtn)
            {
                if (m_OtherCommand.PCommandDevice != this.PCommandDevice)
                {
                    rtn = false;
                }
            }
            if (rtn)
            {
                if (m_OtherCommand.cv_DeviceId != this.cv_DeviceId)
                {
                    rtn = false;
                }
            }
            return rtn;

        }
        public static bool operator !=(CommandData m_Self, CommandData m_OtherCommand)
        {
            if (m_Self == m_OtherCommand)
            { return false; }
            else
            { return true; }
        }
        public static bool operator ==(CommandData m_Self, CommandData m_OtherCommand)
        {
            if (object.ReferenceEquals(m_OtherCommand, null))
            {
                return object.ReferenceEquals(m_Self, null);
            }

            return m_Self.Equals(m_OtherCommand);
            /*
            bool rtn = false;
            if (m_OtherCommand.PCommandType == m_Self.PCommandType)
            {
                rtn = true;
                switch (m_Self.PCommandType)
                {
                    case APIEnum.CommandType.Aligner:
                        if (m_Self.PAlignerCommand != m_OtherCommand.PAlignerCommand)
                            rtn = false;
                        break;
                    case APIEnum.CommandType.Alignment:
                        if (m_Self.PAlignmentCommand != m_OtherCommand.PAlignmentCommand)
                            rtn = false;
                        break;
                    case APIEnum.CommandType.API:
                        if (m_Self.PApiCommand != m_OtherCommand.PApiCommand)
                            rtn = false;
                        break;
                    case APIEnum.CommandType.Barcode:
                        if (m_Self.PBarcodeCommand != m_OtherCommand.PBarcodeCommand)
                            rtn = false;
                        break;
                    case APIEnum.CommandType.Common:
                        if (m_Self.PCommonCommand != m_OtherCommand.PCommonCommand)
                            rtn = false;
                        break;
                    case APIEnum.CommandType.E84:
                        if (m_Self.PE84Command != m_OtherCommand.PE84Command)
                            rtn = false;
                        break;
                    case APIEnum.CommandType.IO:
                        if (m_Self.PIoCommand != m_OtherCommand.PIoCommand)
                            rtn = false;
                        break;
                    case APIEnum.CommandType.LoadPort:
                        if (m_Self.PLoadPortCommand != m_OtherCommand.PLoadPortCommand)
                            rtn = false;
                        break;
                    case APIEnum.CommandType.OCR:
                        if (m_Self.POcrCommand != m_OtherCommand.POcrCommand)
                            rtn = false;
                        break;
                    case APIEnum.CommandType.RFID:
                        if (m_Self.PRfidCommand != m_OtherCommand.PRfidCommand)
                            rtn = false;
                        break;
                    case APIEnum.CommandType.Robot:
                        if (m_Self.PRobotCommand != m_OtherCommand.PRobotCommand)
                            rtn = false;
                        break;
                };
            }
            if (rtn)
            {
                if (m_OtherCommand.PCommandDevice != m_Self.PCommandDevice)
                {
                    rtn = false;
                }
            }
            if (rtn)
            {
                if (m_OtherCommand.cv_DeviceId != m_Self.cv_DeviceId)
                {
                    rtn = false;
                }
            }
            return rtn;
            */
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonData.HIRATA
{
    public class Alarmtable
    {
        public const int BcHsDataRequestTsTimeOut = 2001;
        public const int BcHsDataRequestTeTimeOut = 2002;
        public const int BcHsDataRequestReplyNG = 2003;
        public const int RobotApiReplyAbnormal = 2004;
        public const int RobotApiRobotStatusError = 2005;
        public const int RobotApiAlignerStatusError = 2006;
        public const int RobotApiAlignerOffline = 2007;
        public const int RobotApiPortNotInOnline = 2008;
        public const int RobotApiPortError = 2009;
        public const int RobotApiPortFoupSensorError = 2010;
        public const int RobotApiPortFoupClampSensorError = 2011;
        public const int RobotApiPortDoorError = 2012;
        public const int SensenAndDataUnmatch = 2013;
        public const int SendApiComandError = 2014;
        public const int SendApiComandT3TimeOut = 2015;
        public const int RobotApiBufferStatusError = 2016;
        public const int AlignerDataWaitVaccumError = 2017;
        public const int BcDie = 2018;
        public const int PlcDisconnect = 2019;
        public const int OcrReadERROR = 2020;
        public const int ForceCompleteHandshaketimeout = 2021;
        public const int BcDataDownLoadRecipeERROR = 2022;
        public const int RobotDisConnect = 2023;
        public const int FoupSeqError = 2024;
        public const int RecieUnmatch = 2025;
        public const int WorkSlotError = 2026;
        public const int WorkIdError = 2027;
        public const int PortUnClampButDoorOpen = 2028;
        public const int PortClampButDoorCloseOrNoFoup = 2029;
        public const int AtInitializeRobotStatusError = 2030;
        public const int AtInitializeRobotSensorUnmatch = 2031;
        public const int AtInitializeAlignerSensorUnmatch = 2032;
        public const int AtInitializeBufferSensorUnmatch = 2033;
        public const int AtInitializePortSensorUnmatch = 2034;
        public const int PortTypeValueOverRange = 2035;
        public const int OcrReadError = 2036;
        public const int NotSetCurRecipe = 2037;
        public const int MappingDataError = 2038;
        public const int EfemPortTypeError = 2039;
        public const int UpStreamDataError = 2040;
        public const int PortTypeSlotNumberError = 2041;
        public const int WaitUvOverTimer = 2042;
        public const int EQReadyOffRobotStop = 2043;
        public const int RobotJobPathError = 2044;
        public const int CannotFindUnloadPortSlotToPutSubstrate = 2045;
        public const int BcFoupDataError = 2046;
        public const int GlassInterfaceT1TimeOut = 2047;
        public const int GlassInterfaceT3TimeOut = 2048;
        public const int GlassInterfaceForceIniTimeOut = 2049;
        public const int GlassInterfaceForceComTimeOut = 2050;
        public const int RobotApiErrorEvent = 2051;
        public const int RobotApiReplyParseError = 2052;
        public const int InterfaceErrorGlassDataRecipeUnmatch = 2053;
        public const int FoupDataContainsOverOneRecipe = 2054;
        public const int FoupDataRecipeEFEMNotHas = 2055;
        public const int ParseFlowError = 2056;
    }
}

using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using KgsCommon;
using System.IO;
using System.Collections;

namespace CommonData.HIRATA
{
    public class ReturnCodeDefine
    {
        public const int LogsRootFolderPathNotDefined = -10;
        public const int ConfigsRootFolderPathNotDefined = -11;
        public const int Sys_EqLayoutNotDefined = -12;
        public const int Sys_MemoryIoFileNotDefined = -13;
        public const int Module_LogIniNotDefined = -14;
        public const int LGC_GIFTimeChartPortAddressFile = -15;
        public const int Sys_AccountNotDefined = -16;
        public const int Module_SystemIniNotDefined = -17;
        public const int Sys_PermissionGroupNotDefined = -18;

        //UI
        public const int UI_LayoutPosNotDefined = -20;

        //LGC
        public const int LGC_RecipeFileNotDefined = -30;
        public const int LGC_StatusRecordFileNotDefined = -31;
        public const int LGC_GIFTimeChartFile = -32;
        public const int LGC_TimeOutFileNotDefined = -33;
        public const int LGC_GlassCountDataFileNotDefined = -34;
        public const int LGC_FlowStepSettingFileNotDefined = -35;

    }
    public class CommonStaticData
    {
        public static readonly string g_SplitLine = "--------------------------------------\n";
        public static readonly string  g_UiModule = "UI"; 
        public static readonly string  g_CimModule = "CIM"; 
        public static readonly string  g_LgcModule = "LGC"; 

        public static PerformanceCounter g_PerformaceCount ;
        public static int g_PortNumber = 0;
        public static KXmlItem g_PortXml;
        public static int g_BufferNumber = 0;
        public static KXmlItem g_BufferXml;
        public static int g_EqNumber = 0;
        public static KXmlItem g_EqXml;
        public static int g_RobotNumber = 0;
        public static KXmlItem g_RobotXml;
        public static int g_AlignerNumber = 0;
        public static KXmlItem g_AlignerXml;


        public static int g_CstSize = 0;

        public static string g_ToolId = "";
        public static string g_SysLayoutFile;
        public static string g_SysMemoryIoClientFile;
        public static string g_SysGifPortAddrFileFile = "";
        public static string g_SysAccountFile = "";
        public static string g_SysPermissionGroupFile = "";

//        public static string g_ModuleLogsIniFile = "";
//        public static string g_ModuleSystemIniFile = "";

        public static string g_UiModuleLogsIniFile = "";
        public static string g_UiModuleSystemIniFile = "";

        public static string g_CimModuleLogsIniFile = "";
        public static string g_CimModuleSystemIniFile = "";

        public static string g_LgcModuleLogsIniFile = "";
        public static string g_LgcModuleSystemIniFile = "";

        public static string g_RootConfigFolderPath = "";
        public static string g_RootLogsFolderPath = "";
        public static string g_WorkFolder = "";


        public static UInt64 GetMemUsage()
        {
            return ((UInt64)CommonData.HIRATA.CommonStaticData.g_PerformaceCount.NextValue() >> 20);
        }

        public static void Init()
        {
            byte already_check_folder_amount = 0; // currently, total check file amount is 2. include Config and Logs folders.
            byte want_check_file_amount = 2;
            foreach (DictionaryEntry item in System.Environment.GetEnvironmentVariables())
            {
                if (Regex.Match(item.Key.ToString(), @"\bConfigFolderPath\b", RegexOptions.IgnoreCase).Success)
                {
                    g_RootConfigFolderPath = item.Value.ToString();
                    already_check_folder_amount++;
                    if (already_check_folder_amount == want_check_file_amount)
                    {
                        break;
                    }
                }
                else if (Regex.Match(item.Key.ToString(), @"\bLogFolderPath\b", RegexOptions.IgnoreCase).Success)
                {
                    g_RootLogsFolderPath = item.Value.ToString();
                    already_check_folder_amount++;
                    if (already_check_folder_amount == want_check_file_amount)
                    {
                        break;
                    }
                }
            }

            if (already_check_folder_amount != want_check_file_amount)
            {
                if(string.IsNullOrEmpty(g_RootLogsFolderPath))
                    Environment.Exit(ReturnCodeDefine.LogsRootFolderPathNotDefined);
                if (string.IsNullOrEmpty(g_RootConfigFolderPath))
                    Environment.Exit(ReturnCodeDefine.ConfigsRootFolderPathNotDefined);               
            }


            g_SysLayoutFile = g_RootConfigFolderPath +  "Sys\\EqLayout.xml";
            g_SysMemoryIoClientFile = g_RootConfigFolderPath + "Sys\\MemoryIOClient.xml";
            //g_ModuleLogsIniFile = g_RootConfigFolderPath + g_FDModuleName +"\\logs.ini";
            //g_ModuleSystemIniFile = g_RootConfigFolderPath + g_FDModuleName + "\\system.ini";
            g_UiModuleLogsIniFile = g_RootConfigFolderPath + g_UiModule +"\\logs.ini";
            g_UiModuleSystemIniFile = g_RootConfigFolderPath + g_UiModule + "\\system.ini";
            g_CimModuleLogsIniFile = g_RootConfigFolderPath + g_CimModule +"\\logs.ini";
            g_CimModuleSystemIniFile = g_RootConfigFolderPath + g_CimModule + "\\system.ini";
            g_LgcModuleLogsIniFile = g_RootConfigFolderPath + g_LgcModule +"\\logs.ini";
            g_LgcModuleSystemIniFile = g_RootConfigFolderPath + g_LgcModule + "\\system.ini";
            g_SysGifPortAddrFileFile = g_RootConfigFolderPath + "Sys\\timechartAdd.xml";
            g_SysAccountFile = g_RootConfigFolderPath + "Sys\\Account.xml";
            g_SysPermissionGroupFile = g_RootConfigFolderPath + "Sys\\PermissionGroup.xml";
            g_WorkFolder = CommonData.HIRATA.CommonStaticData.g_RootConfigFolderPath + CommonData.HIRATA.CommonStaticData.g_LgcModule + "\\Work";


            //ODF

            if (!File.Exists(g_SysLayoutFile)) Environment.Exit(ReturnCodeDefine.Sys_EqLayoutNotDefined);
            if (!File.Exists(g_SysMemoryIoClientFile)) Environment.Exit(ReturnCodeDefine.Sys_MemoryIoFileNotDefined);
            //if (!File.Exists(g_ModuleLogsIniFile)) Environment.Exit(ReturnCodeDefine.Module_LogIniNotDefined);
            if (!File.Exists(g_UiModuleLogsIniFile)) Environment.Exit(ReturnCodeDefine.Module_LogIniNotDefined);
            if (!File.Exists(g_CimModuleLogsIniFile)) Environment.Exit(ReturnCodeDefine.Module_LogIniNotDefined);
            if (!File.Exists(g_LgcModuleLogsIniFile)) Environment.Exit(ReturnCodeDefine.Module_LogIniNotDefined);
            if (!File.Exists(g_SysGifPortAddrFileFile)) Environment.Exit(ReturnCodeDefine.LGC_GIFTimeChartPortAddressFile);
            if (!File.Exists(g_SysAccountFile)) Environment.Exit(ReturnCodeDefine.Sys_AccountNotDefined);
            if (!File.Exists(g_SysPermissionGroupFile)) Environment.Exit(ReturnCodeDefine.Sys_PermissionGroupNotDefined);
            //if (!File.Exists(g_ModuleSystemIniFile)) Environment.Exit(ReturnCodeDefine.Module_SystemIniNotDefined);
            if (!File.Exists(g_UiModuleSystemIniFile)) Environment.Exit(ReturnCodeDefine.Module_SystemIniNotDefined);
            if (!File.Exists(g_CimModuleSystemIniFile)) Environment.Exit(ReturnCodeDefine.Module_SystemIniNotDefined);
            if (!File.Exists(g_LgcModuleSystemIniFile)) Environment.Exit(ReturnCodeDefine.Module_SystemIniNotDefined);

            if (!System.IO.Directory.Exists(g_WorkFolder))
            {
                System.IO.Directory.CreateDirectory(g_WorkFolder);
            }


            layoutSetting();
            g_PerformaceCount = new PerformanceCounter("Process", "Working Set - Private", Process.GetCurrentProcess().ProcessName);
        }

        public static void KillTerminal(string[] m_Pid)
        {
            string pid = "";
            if(m_Pid != null && m_Pid.Length > 0)
            {
                pid = m_Pid[0];
            }
            string strCmdText = "/C taskkill /pid " + pid;
            System.Diagnostics.Process.Start("CMD.exe", strCmdText);
        }

        public static void layoutSetting()
        {
            if (File.Exists(g_SysLayoutFile))
            {
                KXmlItem tmp = new KXmlItem();
                tmp.LoadFromFile(g_SysLayoutFile);
                g_EqNumber = tmp.ItemsByName["EquipmentList"].ItemNumber;
                g_EqXml = new KXmlItem(tmp.ItemsByName["EquipmentList"]);
                g_AlignerNumber = tmp.ItemsByName["AlignerList"].ItemNumber;
                g_AlignerXml = new KXmlItem(tmp.ItemsByName["AlignerList"]);
                g_PortNumber = tmp.ItemsByName["PortList"].ItemNumber;
                g_PortXml = new KXmlItem(tmp.ItemsByName["PortList"]);
                g_BufferNumber = tmp.ItemsByName["BufferList"].ItemNumber;
                g_BufferXml = new KXmlItem(tmp.ItemsByName["BufferList"]);
                g_RobotNumber = tmp.ItemsByName["RobotList"].ItemNumber;
                g_RobotXml = new KXmlItem(tmp.ItemsByName["RobotList"]);
                g_CstSize = Convert.ToInt32(tmp.Attributes["CassetteSlotSize"].Trim());
                g_ToolId = tmp.Attributes["ToolId"].Trim();
            }
        }

        public static int GetIntFromBCD(int m_Value)
        {
            int tmp_value = m_Value;
            string tmp_str = GetStrFromBCD(tmp_value);
            return SysUtils.StrToInt(tmp_str);
        }
        public static string GetStrFromBCD(int m_Value)
        {
            int tmp_value = m_Value;
            string hex = SysUtils.IntToHex(tmp_value);
            return hex;
        }
        public static string __FUN()
        {
            StackTrace stack = new StackTrace();
            return stack.GetFrame(1).GetMethod().Name;
        }

        public static int GetIntFormStr(string m_Str)
        {
            int rtn = 0;
            if(Regex.Match(m_Str , @"0x" , RegexOptions.IgnoreCase).Success)
            {
                rtn = Convert.ToInt32(m_Str, 16);
            }
            else
            {
                rtn = Convert.ToInt32(m_Str);
            }
            return rtn;
        }
    }
}

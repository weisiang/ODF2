using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using KgsCommon;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections;
using CommonData.HIRATA;

namespace LGC
{
    public class CommonStaticData
    {
        public static string g_TimeOutPath = "";
        public static string g_GlassCountDataPath = "";

        public static string g_RecipePath = "";

        public static string g_StatsRecordPath ="";

        public static string g_TimeChartTemplate = "";
        public static string g_TimeChart = "";

        public static string g_FlowStepSettingFile = "";


        static CommonStaticData()
        {
            g_TimeOutPath = CommonData.HIRATA.CommonStaticData.g_RootConfigFolderPath + CommonData.HIRATA.CommonStaticData.g_FDModuleName + "\\TimeOut.xml";
            g_GlassCountDataPath = CommonData.HIRATA.CommonStaticData.g_RootConfigFolderPath + CommonData.HIRATA.CommonStaticData.g_FDModuleName + "\\GlassCount.xml";
            g_TimeChartTemplate = CommonData.HIRATA.CommonStaticData.g_RootConfigFolderPath + CommonData.HIRATA.CommonStaticData.g_FDModuleName + "\\timechartsT.xml";
            g_TimeChart = CommonData.HIRATA.CommonStaticData.g_RootConfigFolderPath + CommonData.HIRATA.CommonStaticData.g_FDModuleName + "\\timecharts.xml";
            g_RecipePath = CommonData.HIRATA.CommonStaticData.g_RootConfigFolderPath + CommonData.HIRATA.CommonStaticData.g_FDModuleName + "\\PPID.xml";
            g_StatsRecordPath = CommonData.HIRATA.CommonStaticData.g_RootConfigFolderPath + CommonData.HIRATA.CommonStaticData.g_FDModuleName + "\\StatusRecord.xml";
            g_FlowStepSettingFile = CommonData.HIRATA.CommonStaticData.g_RootConfigFolderPath + CommonData.HIRATA.CommonStaticData.g_FDModuleName + "\\Flow.ini";
            if (!File.Exists(g_RecipePath)) Environment.Exit(CommonData.HIRATA.ReturnCodeDefine.LGC_RecipeFileNotDefined);
            if (!File.Exists(g_StatsRecordPath)) Environment.Exit(CommonData.HIRATA.ReturnCodeDefine.LGC_StatusRecordFileNotDefined);
            if (!File.Exists(g_TimeChartTemplate)) Environment.Exit(CommonData.HIRATA.ReturnCodeDefine.LGC_GIFTimeChartFile);
            if (!File.Exists(g_TimeOutPath)) Environment.Exit(CommonData.HIRATA.ReturnCodeDefine.LGC_TimeOutFileNotDefined);
            if (!File.Exists(g_GlassCountDataPath)) Environment.Exit(CommonData.HIRATA.ReturnCodeDefine.LGC_GlassCountDataFileNotDefined);
            if (!File.Exists(g_FlowStepSettingFile)) Environment.Exit(CommonData.HIRATA.ReturnCodeDefine.LGC_FlowStepSettingFileNotDefined);
        }
    }
}

using System;
using System.Collections.Generic;
using CommonData.HIRATA;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using KgsCommon;
using CommonData;
using BaseAp;
using System.Reflection;
using System.Diagnostics;

namespace LGC
{
    public partial class LgcModule
    {
        public static KMemoLog cv_AlarmLog;
        public static Dictionary<string, List<AlarmItem>> cv_ApiAlarm = new Dictionary<string, List<AlarmItem>>();
        public void InitAlarmData()
        {
            cv_Alarms.PIsAutoSave = false;
        }
        public virtual void initAlarmLog()
        {
            if (cv_AlarmLog == null)
            {
                string enviPath = CommonData.HIRATA.CommonStaticData.g_RootLogsFolderPath + CommonData.HIRATA.CommonStaticData.g_LgcModule;
                cv_AlarmLog = new KMemoLog();
                cv_AlarmLog.LoadFromIni(CommonData.HIRATA.CommonStaticData.g_LgcModuleLogsIniFile, "AlarmLog");
                cv_AlarmLog.LogFileName = enviPath + "\\AlarmLog.log";
                cv_AlarmLog.SaveToIni(CommonData.HIRATA.CommonStaticData.g_LgcModuleLogsIniFile, "AlarmLog");
                /*
                for(int i=1 ; i<10000 ; i++)
                {
                    AlarmItem tmp = new AlarmItem();
                    tmp.PTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    tmp.cv_Code = i.ToString();
                    if(i%2 == 0)
                    tmp.PLevel = AlarmLevele.Light;
                    else
                    tmp.PLevel = AlarmLevele.Serious;
                    tmp.PMainDescription = "test";
                    tmp.PSubDescription = i.ToString();
                    WriteAlarmLog(tmp);
                }
                */
            }
        }
        public static void EditAlarm(CommonData.HIRATA.AlarmItem m_Alarm, bool m_IsApi = false)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, CommonData.HIRATA.CommonStaticData.__FUN(), FunInOut.Enter);
            if (m_Alarm.PStatus == AlarmStatus.Occur)
            {
                if (!cv_Alarms.cv_AlarmList.Exists(x => x.PCode == m_Alarm.PCode))
                {
                    m_Alarm.PTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    cv_Alarms.AddAlarm(m_Alarm);
                    WriteAlarmLog(m_Alarm);
                    if (m_Alarm.PLevel == AlarmLevele.Serious)
                    {
                        AddBuzzerCommand(true);
                    }
                }
            }
            else if (m_Alarm.PStatus == AlarmStatus.Clean)
            {
                if (cv_Alarms.cv_AlarmList.Exists(x => x.PCode == m_Alarm.PCode))
                {
                    cv_Alarms.DelAlarm(m_Alarm);
                }
            }
            WriteLog(LogLevelType.NormalFunctionInOut, CommonData.HIRATA.CommonStaticData.__FUN(), FunInOut.Leave);
        }
        public static void EditAlarm(List<CommonData.HIRATA.AlarmItem> m_Alarms)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, CommonData.HIRATA.CommonStaticData.__FUN(), FunInOut.Enter);
            foreach (CommonData.HIRATA.AlarmItem m_Alarm in m_Alarms)
            {
                if (m_Alarm.PStatus == AlarmStatus.Occur)
                {
                    if (!cv_Alarms.cv_AlarmList.Exists(x => x.PCode == m_Alarm.PCode))
                    {
                        m_Alarm.PTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                        cv_Alarms.AddAlarm(m_Alarm);
                        WriteAlarmLog(m_Alarm);
                    }
                }
                else if (m_Alarm.PStatus == AlarmStatus.Clean)
                {
                    int index = cv_Alarms.cv_AlarmList.FindIndex(x => x.PCode == m_Alarm.PCode);
                    if (index != -1)
                    {
                        cv_Alarms.DelAlarm(m_Alarm);
                    }
                }
            }
            CheckSystemStatus();
            WriteLog(LogLevelType.NormalFunctionInOut, CommonData.HIRATA.CommonStaticData.__FUN(), FunInOut.Leave);
        }
        public static void LoadAlarmTable()
        {
            if (cv_ApiAlarm == null)
            {
                cv_ApiAlarm = new Dictionary<string, List<AlarmItem>>();
            }
            cv_ApiAlarm.Clear();
            for (int i = 1; i <= 10; i++)
            {
                if (i < 10)
                    cv_ApiAlarm[i.ToString().PadLeft(2, '0')] = new List<AlarmItem>();
                else
                    cv_ApiAlarm[i.ToString()] = new List<AlarmItem>();
            }
            KXmlItem file = new KXmlItem();

            string file_path = CommonData.HIRATA.CommonStaticData.g_RootConfigFolderPath + "\\" +
            CommonData.HIRATA.CommonStaticData.g_LgcModule + "\\Alarm.xml";
            file.LoadFromFile(file_path);
            int index = 0;
            int alarm_count = file.ItemsByName["Data"].ItemNumber;
            Match match = Match.Empty;
            while (index < alarm_count)
            {
                KXmlItem xml = file.ItemsByName["Data"].Items[index];
                AlarmItem tmp = new AlarmItem();
                tmp.cv_ApiTypeCode = xml.Attributes["Type"].Trim();
                tmp.PCode = xml.Attributes["RepCode"].Trim();
                string level = xml.Attributes["Level"].Trim();
                if (level == "L")
                    tmp.PLevel = AlarmLevele.Light;
                else if (level == "S")
                    tmp.PLevel = AlarmLevele.Serious;
                match = Match.Empty;
                match = Regex.Match(xml.Attributes["Device"].Trim(), @"\D*", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    if (Enum.IsDefined(typeof(APIEnum.CommnadDevice), match.Value))
                    {
                        tmp.PCommandDevice = (APIEnum.CommnadDevice)Enum.Parse(typeof(APIEnum.CommnadDevice), match.Value);
                    }
                    else
                    {
                        int a = 19;
                    }
                }
                else
                {
                    int a = 9;
                }
                tmp.PMainDescription = xml.Attributes["Msg"].Trim();
                tmp.cv_ResCode = xml.Attributes["DeviceCode"].Trim();
                tmp.PUnit = Convert.ToInt16(xml.Attributes["Unit"].Trim());
                cv_ApiAlarm[tmp.cv_ApiTypeCode].Add(tmp);
                index++;
            }
        }
        public static void WriteAlarmLog(CommonData.HIRATA.AlarmItem m_AlarmItem)
        {
            if (cv_AlarmLog != null)
            {
                string log = m_AlarmItem.PTime + ",";
                log += m_AlarmItem.PCode + ",";
                log += m_AlarmItem.PLevel + ",";
                log += m_AlarmItem.PUnit + ",";
                log += m_AlarmItem.PMsg;
                cv_AlarmLog.WriteLog(log);
            }
        }
    }
}

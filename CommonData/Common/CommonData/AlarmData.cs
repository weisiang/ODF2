using System;
using KgsCommon;
using System.Collections.Generic;
using System.Text;

namespace CommonData.HIRATA
{
    public enum AlarmLevele { None, Light, Serious };
    public enum AlarmStatus { Occur=1, Clean=0 };
    public enum AlarmAction { Set , Reset , };
    public class AlarmItem
    {
        public int cv_Side;
        public int cv_Level;
        public int cv_Status;
        public int cv_Unit;
        public int cv_AuType;
        public string cv_Code = "";
        public string cv_ProgramCode ="";
        public string cv_MainDescription ="";
        public string cv_SubDescription = "";
        public string cv_Time ="";
        public string cv_Msg ="";
        public string cv_ResCode = "";
        public string cv_ApiTypeCode = "";
        public int cv_CommandDevice = 0;
        public enSideGroup PSide
        {
            get { return (enSideGroup)cv_Side; }
            set { cv_Side = (int)value; }

        }
        public HIRATA.APIEnum.CommnadDevice PCommandDevice
        {
            get { return (APIEnum.CommnadDevice)cv_CommandDevice; }
            set { cv_CommandDevice = (int)value; }
        }
        public AlarmLevele PLevel
        {
            get { return (AlarmLevele)cv_Level; }
            set { cv_Level = (int)value; }
        }
        public AlarmStatus PStatus
        {
            get { return (AlarmStatus)cv_Status; }
            set { cv_Status = (int)value; }
        }
        public string PTime
        {
            get { return cv_Time; }
            set { cv_Time = value; }
        }
        public int PUnit
        {
            get { return cv_Unit; }
            set { cv_Unit = value; }
        }
        public string PMsg
        {
            get
            {
                if (!string.IsNullOrEmpty(cv_SubDescription))
                {
                    return cv_MainDescription + " : " + cv_SubDescription;
                }
                else
                {
                    return cv_MainDescription;
                }
            }
        }
        public string PCode
        {
            get { return cv_Code; }
            set { cv_Code = value; }
        }
        public int PAuType
        {
            get { return cv_AuType; }
            set { cv_AuType = value; }
        }
        public string PProgramCode
        {
            get { return cv_ProgramCode; }
            set { cv_ProgramCode = value; }
        }
        public string PMainDescription
        {
            get { return cv_MainDescription; }
            set { cv_MainDescription = value; }
        }
        public string PSubDescription
        {
            get { return cv_SubDescription; }
            set { cv_SubDescription = value; }
        }
        public KXmlItem GetXml()
        {
            KXmlItem body = EventCenterBase.ParseObjectToKXmlItem(this, KParseObjToXmlPropertyType.Field);
            return body;
        }
        public void LoadFromXml(KXmlItem m_Xml)
        {
            EventCenterBase.ParseXmlToObject(this, m_Xml);
        }
    }

    public class AlarmData : CommonDatabase
    {
        public delegate void DeleAlarmAction(AlarmStatus m_Action, List<CommonData.HIRATA.AlarmItem> m_Alarms);
        public event DeleAlarmAction EventAlarmAction;
        public delegate void DeleAlarmChange();
        public event DeleAlarmChange EventAlarmCharge;

        public List<AlarmItem> cv_AlarmList = new List<AlarmItem>();
        public bool IsHasAlarm(enSideGroup m_Side)
        {
            bool side_alarm = cv_AlarmList.Exists(x => (x.PLevel == AlarmLevele.Serious) && ( x.PSide == m_Side));
            bool both_side_alarm = cv_AlarmList.Exists(x => (x.PLevel == AlarmLevele.Serious) && ( x.PSide == enSideGroup.Both));
            return (side_alarm | both_side_alarm);
        }
        public bool IsHasAlarm()
        {
            return  cv_AlarmList.Exists(x => x.PLevel == AlarmLevele.Serious);
        }
        public bool IsHasWarning()
        {
            return cv_AlarmList.Exists(x => x.PLevel == AlarmLevele.Light);
        }
        public bool IsAlarmWarningExist(string m_Code)
        {
            return cv_AlarmList.Exists(x => x.PCode == m_Code);
        }
        public void AddAlarm(AlarmItem m_Item)
        {
            if(!IsAlarmWarningExist(m_Item.PCode))
            {
                lock (cv_obj)
                {
                    try
                    {
                        cv_AlarmList.Add(m_Item);
                    }
                    catch(Exception e)
                    {
                    }
                }
                if(EventAlarmAction != null)
                {
                    List<AlarmItem> alarms = new List<AlarmItem>();
                    alarms.Add(m_Item);
                    EventAlarmAction(AlarmStatus.Occur, alarms);
                }
                if(EventAlarmCharge != null)
                {
                    EventAlarmCharge();
                }
            }
        }
        public void AddAlarm(List<CommonData.HIRATA.AlarmItem> m_Alarms)
        {
            foreach (AlarmItem item in m_Alarms)
            {
                AddAlarm(item);
            }
        }
        public bool DelAlarm(string m_Code )
        {
            bool rtn = false;
            int index = cv_AlarmList.FindIndex(x => x.PCode == m_Code);
            if (index != -1)
            {
                AlarmItem remove_item = cv_AlarmList[index];
                lock (cv_obj)
                {
                    try
                    {
                        cv_AlarmList.RemoveAt(index);
                    }
                    catch(Exception e)
                    {
                    }
                }
                rtn = true;
                if (EventAlarmAction != null)
                {
                    List<AlarmItem> alarms = new List<AlarmItem>();
                    alarms.Add(remove_item);
                    EventAlarmAction(AlarmStatus.Clean, alarms);
                }
                if(EventAlarmCharge != null)
                {
                    EventAlarmCharge();
                }
            }
            return rtn;
        }
        public bool DelAlarm(AlarmItem m_Item)
        {
            return DelAlarm(m_Item.PCode);
        }
        public void LoadFromFile()
        {
            if (!string.IsNullOrEmpty(cv_FilePath))
            {
                KXmlItem recipe_xml = new KXmlItem();
                recipe_xml.LoadFromFile(cv_FilePath);
                if (recipe_xml.ItemsByName["Alarm"].ItemType == KXmlItemType.itxList && recipe_xml.ItemsByName["Alarm"].ItemNumber != 0)
                {
                    int recipe_count = recipe_xml.ItemsByName["Alarm"].ItemNumber;
                    lock (cv_obj)
                    {
                        try
                        {
                            for (int i = 0; i < recipe_count; i++)
                            {
                                KXmlItem item = recipe_xml.ItemsByName["Alarm"].Items[i];
                                AlarmItem tmp = new AlarmItem();
                                tmp.LoadFromXml(item);
                                if (!IsAlarmWarningExist(tmp.PCode))
                                {
                                    cv_AlarmList.Add(tmp);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                        }
                    }
                }
            }
        }
        public void SaveToFile()
        {
            KXmlItem tmp = new KXmlItem();
            tmp.Text = "@<Alarm/>";
            int recipe_count = cv_AlarmList.Count;
            lock (cv_obj)
            {
                try
                {
                    for (int i = 0; i < recipe_count; i++)
                    {
                        tmp.ItemsByName["Alarm"].AddItem(cv_AlarmList[i].GetXml());
                    }
                }
                catch (Exception e)
                {
                }
            }
            tmp.SaveToFile(cv_FilePath, true);
        }
        public void SetFilePath(string m_Path)
        {
            cv_FilePath = m_Path;
        }
        public void Clone(AlarmData m_OtherAlarmData)
        {
            this.cv_AlarmList = m_OtherAlarmData.cv_AlarmList;
        }
    }
}

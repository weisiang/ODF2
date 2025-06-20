using System;
using KgsCommon;
using System.Collections.Generic;
using System.Text;

namespace CommonData.HIRATA
{
    public class BufferData : ObjData
    {
        private Dictionary<int, BufferSlotType> cv_Types = new Dictionary<int, BufferSlotType>();
        public BufferData(int m_id, int m_SlotCount)
            : base(m_id, m_SlotCount)
        {
            cv_FilePath = CommonData.HIRATA.CommonStaticData.g_WorkFolder + "\\" + typeof(BufferData).Name + m_id.ToString();
        }
        public BufferData()
            : base(0, CommonData.HIRATA.CommonStaticData.g_CstSize)
        {
        }
        public bool PEnable
        {
            get { return cv_Enable; }
            set { cv_Enable = value; }
        }
        public BufferSlotType getBufferSlotType(int m_Slot)
        {
            BufferSlotType rtn = BufferSlotType.None;
            if(cv_Types.ContainsKey(m_Slot))
            {
                rtn = cv_Types[m_Slot];
            }
            return rtn;
        }

        public int IsFreeSlot(BufferSlotType m_Type)
        {
            int slot = -1;
            for (int i = 1; i <= (int)cv_SlotCount ; i++)
            {
                if (cv_Types[i] == m_Type && !GlassDataMap[i].PHasData && !GlassDataMap[i].PHasSensor)
                {
                    slot = i;
                    break;
                }
            }
            return slot;
        }
        public bool GetUnloadSlot(BufferSlotType m_Type , out int m_Slot)
        {
            int slot = -1;
            for (int i = 1;  i <= cv_SlotCount; i++)
            {
                if (cv_Types[i] == m_Type)
                {
                    if (GlassDataMap[i].PHasData && GlassDataMap[i].PHasSensor)
                    {
                        if(slot == -1)
                        slot = i;
                        else
                        {
                            if(GlassDataMap[i].PEnterBufferTime < GlassDataMap[slot].PEnterBufferTime)
                            {
                                slot = i;
                            }
                        }
                    }
                }
            }
            m_Slot = slot;
            return m_Slot == -1 ? false : true;
        }
        public void SetSlotType(Dictionary<int, BufferSlotType>  m_Para)
        {
            cv_Types = m_Para;
        }
        public void LoadFromFile()
        {
            if (!string.IsNullOrEmpty(cv_FilePath))
            {
                string ori_path = cv_FilePath;
                KXmlItem recipe_xml = new KXmlItem();
                recipe_xml.LoadFromFile(cv_FilePath);
                if (recipe_xml.ItemsByName["Data"].ItemType == KXmlItemType.itxList && recipe_xml.ItemsByName["Data"].ItemNumber != 0)
                {
                    EventCenterBase.ParseXmlToObject(this, recipe_xml.ItemsByName[typeof(BufferData).Name]);
                    this.GlassDataList = this.cv_GlassDataList;
                }
                if(cv_FilePath != ori_path)
                {
                    cv_FilePath = ori_path;
                }
            }
        }
        public void SaveToFile()
        {
            KXmlItem tmp = new KXmlItem();
            tmp.Text = "@<Data/>";
            KXmlItem body = EventCenterBase.ParseObjectToKXmlItem(this, KParseObjToXmlPropertyType.Field);
            tmp.ItemsByName["Data"].AddItem(body);
            lock (cv_Obj)
            {
                try
                {
                    tmp.SaveToFile(cv_FilePath, true);
                }
                catch(Exception e)
                {
                }
            }
        }
    }
}

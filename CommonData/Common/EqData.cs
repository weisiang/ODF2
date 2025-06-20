using System;
using System.Collections.Generic;
using System.Text;
using KgsCommon;


namespace CommonData.HIRATA
{
    public class EqData : ObjData
    {
        public EqData(int m_id, int m_SlotCount)
            : base(m_id, m_SlotCount)
        {
            cv_FilePath = CommonData.HIRATA.CommonStaticData.g_WorkFolder + "\\" + typeof(EqData).Name + m_id.ToString();
        }
        public EqData()
            : base(0, CommonData.HIRATA.CommonStaticData.g_CstSize)
        {
        }
        public bool PEnable
        {
            get { return cv_Enable; }
            set { cv_Enable = value; }
        }
        public void LoadFromFile()
        {
            string ori_path = cv_FilePath;

            if (!string.IsNullOrEmpty(cv_FilePath))
            {
                KXmlItem recipe_xml = new KXmlItem();
                recipe_xml.LoadFromFile(cv_FilePath);
                if (recipe_xml.ItemsByName["Data"].ItemType == KXmlItemType.itxList && recipe_xml.ItemsByName["Data"].ItemNumber != 0)
                {
                    EventCenterBase.ParseXmlToObject(this, recipe_xml.ItemsByName[typeof(EqData).Name]);
                    this.GlassDataList = this.cv_GlassDataList;
                }
            }
            if (cv_FilePath != ori_path)
            {
                cv_FilePath = ori_path;
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

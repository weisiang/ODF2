using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KgsCommon;

namespace LGC
{
    public class TimeChartParser
    {
        Dictionary<int, List<KeyValuePair<int, string>>> cv_StepIdMap = new Dictionary<int, List<KeyValuePair<int, string>>>();
        public TimeChartParser(string m_TimeChartXmlPath)
        {
            KXmlItem xml = new KXmlItem();
            xml.LoadFromFile(m_TimeChartXmlPath);
            int seq_count = xml.ItemsByName["TimeCharts"].ItemNumber;
            for (int seq_index = 0; seq_index < seq_count; seq_index++)
            {
                KXmlItem seq_xml = xml.ItemsByName["TimeCharts"].Items[seq_index];

                int seq_Id = Convert.ToInt32(seq_xml.Attributes["Id"]);

                List<KeyValuePair<int, string>> tmp = new List<KeyValuePair<int, string>>();
                if (seq_xml.IsItemExist("Sequence") && seq_xml.ItemsByName["Sequence"].ItemType == KXmlItemType.itxList)
                {
                    KXmlItem s_item = seq_xml.ItemsByName["Sequence"];
                    int step_count = s_item.ItemNumber;
                    string name = "";
                    int id;
                    for (int i = 0; i < step_count; ++i)
                    {
                        KXmlItem step_item = s_item.Items[i];
                        id = Convert.ToInt32(step_item.Attributes["Id"]);
                        name = step_item.Attributes["Name"];
                        KeyValuePair<int, string> id_map = new KeyValuePair<int, string>(id, name);
                        tmp.Add(id_map);
                    }
                }
                cv_StepIdMap.Add(seq_Id, tmp);
            }
        }
        public string GetStepName(int m_SeqId , int m_StepId)
        {
            string rtn = "";
            if(cv_StepIdMap.ContainsKey(m_SeqId))
            {
                int index = cv_StepIdMap[m_SeqId].FindIndex(x => x.Key == m_StepId);
                if(index != -1)
                {
                    rtn = cv_StepIdMap[m_SeqId][index].Value;
                }
            }
            return rtn;
        }
    }
}

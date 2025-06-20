using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CommonData.HIRATA
{
    public class ObjData
    {
        protected object cv_Obj = new object();
        public uint cv_Id = 0;
        public uint cv_SlotCount = 0;
        public string cv_FilePath = "";
        protected bool cv_Enable=true;
        public uint PId
        {
            get { return cv_Id; }
            set { cv_Id = value; }
        }


        private Dictionary<int, GlassData> cv_GlassDataMap = new Dictionary<int, GlassData>();
        public List<GlassData> cv_GlassDataList = new List<GlassData>();
        //use GlassDataList = cv_GlassDataList to set glass map.
        public List<GlassData> GlassDataList
        {
            set
            {
                //for ui module use.
                foreach(GlassData data in value)
                {
                    cv_GlassDataMap[Convert.ToInt16(data.PSlotInEq)] = data;
                }
                //GlassDataMap = cv_GlassDataMap;
            }
        }
        public Dictionary<int , GlassData> GlassDataMap
        {
            get { return cv_GlassDataMap; }
            set 
            {
                cv_GlassDataList = null;
                cv_GlassDataMap = value;
                cv_GlassDataList = cv_GlassDataMap.Values.ToList();
            }
        }
        public ObjData(int m_id, int m_MaxCount)
        {
            cv_Id = Convert.ToUInt16(m_id);
            InitDataMap(m_MaxCount);
            cv_SlotCount = (uint)m_MaxCount;
        }
        public void InitDataMap(int m_MaxCount)
        {
            cv_GlassDataMap = null;
            cv_GlassDataMap = new Dictionary<int, GlassData>();
            for (int i = 1; i <= m_MaxCount; i++)
            {
                GlassData tmp = new GlassData();
                cv_GlassDataMap[i] = tmp;
                cv_GlassDataMap[i].cv_SlotInEq = (uint)i;
            }
        }
        public virtual bool GetSlotData(int m_Slot, ref GlassData m_Data , ref bool m_HasSensor)
        {
            bool rtn = true;
            if (cv_GlassDataMap.ContainsKey(m_Slot))
            {
                if (!cv_GlassDataMap[m_Slot].PHasData)
                {
                    m_Data = cv_GlassDataMap[m_Slot];
                }
                else
                {
                    m_Data = cv_GlassDataMap[m_Slot];
                }
                if (!cv_GlassDataMap[m_Slot].PHasSensor)
                {
                    m_HasSensor = false;
                }
                else
                {
                    m_HasSensor = true;
                }
            }
            else
            {
                rtn = false;
            }
            return rtn;
        }
        public void ClearAllGlassData()
        {
            for (int i = 1; i <= cv_SlotCount; i++ )
            {
                cv_GlassDataMap[i] = new GlassData();
                cv_GlassDataMap[i].PSlotInEq = (uint)i;
            }
        }
        public bool IsSensorDataMatch(out List<int> m_UnmatchSlot)
        {
            bool rtn = true ;
            List<int> tmp = new List<int>();
            for (int i = 1; i <= cv_SlotCount; i++)
            {
                if(cv_GlassDataMap[i].PHasData != cv_GlassDataMap[i].PHasSensor)
                {
                    rtn = false;
                    tmp.Add(i);
                }
            }
            m_UnmatchSlot = tmp;
            return rtn;
        }
        public bool DelSlotData(int m_Slot)
        {
            bool rtn = false;
            cv_GlassDataMap[m_Slot] = null;
            cv_GlassDataMap[m_Slot] = new GlassData();
            cv_GlassDataMap[m_Slot].cv_SlotInEq = (uint)m_Slot;
            return true;
        }
        /*
        public bool TheSlotHasData(int m_Slot )
        {
            bool rtn = false;
            if(cv_GlassDataMap != null)
            {
                if(!cv_GlassDataMap[m_Slot].IsNull())
                {
                    rtn = true;
                }
                else
                    rtn = false;
            }
            else
            {
                rtn = false;
            }
            return rtn;
        }
        */
        /*
        public bool TheSlotHasData(RobotArm m_Arm)
        {
            bool rtn = false;
            if (cv_GlassDataMap != null)
            {
                if (!cv_GlassDataMap[(int)m_Arm].IsNull())
                {
                    rtn = true;
                }
                else
                    rtn = false;
            }
            else
            {
                rtn = false;
            }
            return rtn;
        }
        */

        public bool TheSlotHasDataOrSensor<T>(T m_Arm)
        {
            bool data = false;
            bool sensor = false;
            int slot = Convert.ToInt16(m_Arm);
            if (cv_GlassDataMap != null)
            {
                if (!cv_GlassDataMap[slot].IsNull())
                {
                    data = true;
                }
                else
                {
                    data = false;
                }
                if(cv_GlassDataMap[slot].PHasSensor)
                {
                    sensor = true;
                }
            }
            else
            {
                data = false;
                sensor = false;
            }
            return data|sensor;
        }
        public bool WhichSlotCanLoad(out int m_Slot)
        {
            int slot = 0;
            bool rtn = false;
            for(int i=1 ; i<= cv_SlotCount ; i++ )
            {
                if(!GlassDataMap[i].PHasData && !GlassDataMap[i].PHasSensor)
                {
                    slot = i;
                    rtn = true;
                    break;
                }
            }
            m_Slot = slot;
            return rtn;
        }
    }
}

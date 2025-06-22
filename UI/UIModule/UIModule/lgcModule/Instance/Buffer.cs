using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonData.HIRATA;
using LGC.Comm;
using KgsCommon;
using BaseAp;

namespace LGC
{
    class Buffer : Obj 
    {
        public BufferComm cv_Comm = null;
        public BufferData cv_Data = null;

        public Buffer(int m_Id, int m_SlotCount)
            : base(m_Id, m_SlotCount)
        {
            InitData();
            InitComm();
        }
        protected override void InitComm()
        {
            if (cv_Comm == null)
            {
                cv_Comm = new BufferComm();
            }
        }
        protected override void InitData()
        {
            if (cv_Data == null)
            {
                cv_Data = new BufferData(cv_Id, cv_SlotCount);
            }
        }
        public override void SendDataViaMmf()
        {
            //this.cv_Data.GlassDataMap = this.cv_Data.GlassDataMap;
            //Global.Controller.SendMmfNotifyObject(typeof(CommonData.HIRATA.BufferData).Name, this.cv_Data, KgsCommon.KParseObjToXmlPropertyType.Field);
            for(int i = 0 ; i< this.cv_SlotCount ; i++)
            {
                this.cv_Data.GlassDataMap[i + 1].WriteWokeNoOnly(LgcModule.PMio, 0x381A + i * 2);
            }
            cv_Data.SaveToFile();
        }
        public override bool CanAccess(bool m_IsLoad , int m_Slot , bool m_IsExchange=false)
        {
            bool rtn = false;
            if (IsSlotValid(m_Slot))
            {
                if (m_IsLoad)
                {
                    if (!cv_Data.GlassDataMap[m_Slot].PHasData && !cv_Data.GlassDataMap[m_Slot].PHasSensor)
                    {
                        rtn = true;
                    }
                }
                else
                {
                    if (cv_Data.GlassDataMap[m_Slot].PHasData && cv_Data.GlassDataMap[m_Slot].PHasSensor)
                    {
                        rtn = true;
                    }
                }
            }
            return rtn;
        }
        public bool IsHasAnyDataAndSensor()
        {
            bool rtn = false;
            for (int i = 1; i <= cv_SlotCount; i++ )
            {
                if (cv_Data.GlassDataMap[1].PHasData || cv_Data.GlassDataMap[1].PHasSensor)
                {
                    rtn = true;
                }
            }
            return rtn;
        }
    }
}

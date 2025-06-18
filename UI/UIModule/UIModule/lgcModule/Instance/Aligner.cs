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
    class Aligner : Obj 
    {
        public AlignerComm cv_Comm = null;
        public AlignerData cv_Data = null;
        public Aligner(int m_Id, int m_SlotCount)
            : base(m_Id, m_SlotCount )
        {
            InitData();
            InitComm();
        }
        protected override void InitComm()
        {
            if (cv_Comm == null)
            {
                cv_Comm = new AlignerComm();
            }
        }
        protected override void InitData()
        {
            if (cv_Data == null)
            {
                cv_Data = new AlignerData(cv_Id, cv_SlotCount);
            }
        }
        public override void  SendDataViaMmf()
        {
            //this.cv_Data.GlassDataMap = this.cv_Data.GlassDataMap;
            //Global.Controller.SendMmfNotifyObject(typeof(CommonData.HIRATA.AlignerData).Name, this.cv_Data, KgsCommon.KParseObjToXmlPropertyType.Field);
            cv_Data.SaveToFile();
        }
        public bool IsHasAnyDataAndSensor()
        {
            bool rtn = false ;
            if (cv_Data.GlassDataMap[1].PHasData || cv_Data.GlassDataMap[1].PHasSensor)
            {
                rtn = true ;
            }
            return rtn;
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
    }
}

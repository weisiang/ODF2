using System;
using System.Collections.Generic;
using System.Text;
using KgsCommon;

namespace CommonData.HIRATA
{
    public class PortData : ObjData
    {
        public string cv_LotId = "";
        public string cv_CurPPID = "";
        public uint cv_FoupSeq = 0;
        public uint cv_HasCst = 0;
        public uint cv_PortClamp = 0;
        public uint cv_PortAgvMode = 0;
        public uint cv_PortEnable = 0;
        public uint cv_PortType = 0;
        public uint cv_PortMode = 0;
        public uint cv_LotStatus = 0;
        public uint cv_PortStatus = 0;
        public uint cv_WorkCount = 0;
        public bool cv_WaitUnload = false;
        public int cv_ProductionType = 0;
        public int cv_EfemPortType = 0;
        public bool cv_IsWaitCancel = false;
        public bool cv_IsWaitAbort = false;
        public int PEfemPortType
        {
            get { return cv_EfemPortType; }
            set { cv_EfemPortType = value; }
        }
        public string PCurPPID
        {
            get { return cv_CurPPID; }
            set { cv_CurPPID = value; }
        }
        public ProductCategory PProductionType
        {
            get { return (ProductCategory)cv_ProductionType; }
            //set { cv_ProductionType = (int)value; }
        }
        public bool PWaitUnload
        {
            set { cv_WaitUnload = value; }
            get
            {
                return cv_WaitUnload;
            }
        }
        public uint PWorkCount
        {
            set { cv_WorkCount = value; }
            get { return cv_WorkCount; }
        }
        public string PLotId
        {
            get { return cv_LotId; }
            set { cv_LotId = value; }
        }
        public uint PFoupSeq
        {
            get { return cv_FoupSeq; }
            set { cv_FoupSeq = value; }
        }
        public PortHasCst PPortHasCst
        {
            get { return (CommonData.HIRATA.PortHasCst)cv_HasCst; }
            set { cv_HasCst = (UInt16)value; }
        }
        public PortClamp PPortClamp
        {
            get { return (CommonData.HIRATA.PortClamp)cv_PortClamp; }
            set { cv_PortClamp = (UInt16)value; }
        }
        public PortAgv PPortAgvMode
        {
            get { return (CommonData.HIRATA.PortAgv)cv_PortAgvMode; }
            set { cv_PortAgvMode = (UInt16)value; }
        }
        public PortEnable PPortEnable
        {
            get { return (CommonData.HIRATA.PortEnable)cv_PortEnable; }
            set { cv_PortEnable = (UInt16)value; }
        }
        public PortType PPortType
        {
            get { return (CommonData.HIRATA.PortType)cv_PortType; }
            set { cv_PortType = (UInt16)value; }
        }
        public PortMode PPortMode
        {
            get { return (CommonData.HIRATA.PortMode)cv_PortMode; }
            set { cv_PortMode = (UInt16)value; }
        }
        public LotStatus PLotStatus
        {
            get { return (CommonData.HIRATA.LotStatus)cv_LotStatus; }
            set
            {
                if ((UInt16)value != cv_LotStatus)
                {
                    cv_LotStatus = (UInt16)value;
                }
            }
        }
        public CommonData.HIRATA.PortStaus PPortStatus
        {
            get { return (CommonData.HIRATA.PortStaus)cv_PortStatus; }
            set { cv_PortStatus = (UInt16)value; }
        }

        public PortData(int m_id, int m_SlotCount)
            : base(m_id, m_SlotCount)
        {
            cv_FilePath = CommonData.HIRATA.CommonStaticData.g_WorkFolder + "\\" + typeof(PortData).Name + m_id.ToString();
        }
        public PortData()
            : base( 0 , CommonStaticData.g_CstSize)
        {

        }
        public PortData(KMemoryIOClient m_MemoIO , int m_PortAddress)
            : base(0, CommonStaticData.g_CstSize)
        {
            int slot_1_offset = 16;
            int port, count, seq, process_slot;
            port = (m_MemoIO.GetPortValue(m_PortAddress) & 0x0F00) >> 8;
            count = (m_MemoIO.GetPortValue(m_PortAddress) & 0x00FF);
            seq = (m_MemoIO.GetPortValue(m_PortAddress + 1) & 0x00FF);
            string id = m_MemoIO.GetBinaryLengthData(m_PortAddress + 2, 5).Trim();
            process_slot = m_MemoIO.GetPortValue(m_PortAddress + 12);
            process_slot += ((m_MemoIO.GetPortValue(m_PortAddress + 13) & 0x01FF) << 15);

            this.PId = (uint)port;
            this.PWorkCount = (uint)count;
            this.PFoupSeq = (uint)seq;
            this.PLotId = id;

            for(int i = 0 ; i<cv_SlotCount ; i++)
            {
                GlassData glass = new GlassData(m_MemoIO, m_PortAddress + slot_1_offset + (64 * i));
                glass.PSlotInEq =(uint) i + 1;
                //glass.PProcessFlag = (((process_slot >> i) & 0x0001) == 1 ? ProcessFlag.Need : ProcessFlag.NotNeed);
                if(glass.PHasData)
                {
                    glass.PHasSensor = true;
                }
                this.GlassDataMap[i + 1] = glass;
            }
            
            this.GlassDataMap = this.GlassDataMap;
        }
        public bool HasOtherJobHaveToDo()
        {
            bool rtn = false;
            for(int i = 1 ; i <= cv_SlotCount ; i++)
            {
                if (PPortMode == PortMode.Loader)
                {
                    if (GlassDataMap[i].PHasData && GlassDataMap[i].PHasSensor && GlassDataMap[i].POcrResult == OCRResult.None)
                    {
                        if (GlassDataMap[i].PProcessFlag == ProcessFlag.Need)
                        {
                            rtn = true;
                        }
                    }
                }
                else if (PPortMode == PortMode.Unloader)
                {
                    if (!GlassDataMap[i].PHasData && !GlassDataMap[i].PHasSensor)
                    {
                        rtn = true;
                    }
                }
            }
            return rtn;
        }
        public bool HasOtherJobHaveToDoExceptSelf(int m_Slot)
        {
            bool rtn = false;
            for (int i = 1; i <= cv_SlotCount; i++)
            {
                if (PPortMode == PortMode.Loader)
                {
                    if (GlassDataMap[i].PHasData && GlassDataMap[i].PHasSensor)
                    {
                        if (GlassDataMap[i].PProcessFlag == ProcessFlag.Need)
                        {
                            rtn = true;
                        }
                    }
                }
                else if (PPortMode == PortMode.Unloader)
                {
                    if (!GlassDataMap[i].PHasData && !GlassDataMap[i].PHasSensor)
                    {
                        rtn = true;
                    }
                }
            }
            return rtn;
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
                    EventCenterBase.ParseXmlToObject(this, recipe_xml.ItemsByName[typeof(PortData).Name]);
                    this.GlassDataList = this.cv_GlassDataList;
                }
            }
            PPortAgvMode = CommonData.HIRATA.PortAgv.MGV;
            PPortEnable = CommonData.HIRATA.PortEnable.Enable;
            //PPortMode = CommonData.HIRATA.PortMode.Loader;
            PPortType = CommonData.HIRATA.PortType.MIX;
            if ((cv_Id == 1) || (cv_Id == 2))
            {
                //PPortMode = CommonData.HIRATA.PortMode.Unloader;
                cv_ProductionType = (int)ProductCategory.Wafer;
            }
            else if ((cv_Id == 3) || (cv_Id == 4))
            {
                cv_ProductionType = (int)ProductCategory.Glass;
            }
            else if ((cv_Id == 5) || (cv_Id == 6)|| (cv_Id == 7)|| (cv_Id == 8))
            {
                cv_ProductionType = (int)ProductCategory.Wafer;
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
            lock(cv_Obj)
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
        public void Clear()
        {
            ClearAllGlassData();
            PLotId = "";
            PFoupSeq = 0;
            PWorkCount = 0;
            PWaitUnload = false;
            cv_IsWaitAbort = false;
            cv_IsWaitCancel = false;
        }
        public void ClearNotIncludeFoupId()
        {
            ClearAllGlassData();
            //PLotId = "";
            PFoupSeq = 0;
            PWorkCount = 0;
            PWaitUnload = false;
            cv_IsWaitAbort = false;
            cv_IsWaitCancel = false;
        }

        public bool HasDataAndSensor(int m_Slot)
        {
            bool rtn = false;
            if(GlassDataMap[m_Slot].PHasData && GlassDataMap[m_Slot].PHasSensor)
            {
                rtn = true;
            }
            return rtn;
        }
        public string GetPortStatusStringForLogUse()
        {
            string rtn = "Port " + cv_Id;
            rtn += " ,Lot status : " + PLotStatus.ToString();
            rtn += " ,Port status : " + PPortStatus.ToString();
            rtn += " ,Port Mode : " + PPortMode.ToString();
            return rtn;
        }
        public bool HasDataOrSensor()
        {
            bool rtn = false;
            for (int i = 1; i <= cv_SlotCount; i++)
            {
                if (GlassDataMap[i].PHasData || GlassDataMap[i].PHasSensor)
                {
                    rtn = true;
                }
            }
            return rtn;
        }
        public bool SeqTheSameWithSubstrate(uint m_FoupSeq)
        {
            bool rtn = false;
            if (HasDataOrSensor())
            {
                for (int i = 1; i <= cv_SlotCount; i++)
                {
                    if (GlassDataMap[i].PHasData)
                    {
                        if (GlassDataMap[i].PFoupSeq == m_FoupSeq)
                        {
                            rtn = true;
                        }
                    }
                }
            }
            return rtn;
        }

        public string GetPortDataStr()
        {
            string data = "Port Data : \n";
            data += "cv_LotId : " + cv_LotId + "\n";
            data += "cv_CurPPID : " + cv_CurPPID + "\n";
            data += "cv_FoupSeq : " + cv_FoupSeq + "\n";
            data += "cv_HasCst : " + cv_HasCst + "\n";
            data += "cv_PortClamp : " + cv_PortClamp + "\n";
            data += "cv_PortAgvMode : " + cv_PortAgvMode + "\n";
            data += "cv_PortEnable : " + cv_PortEnable + "\n";
            data += "cv_PortType : " + cv_PortType + "\n";
            data += "cv_PortMode : " + cv_PortMode + "\n";
            data += "cv_LotStatus : " + cv_LotStatus + "\n";
            data += "cv_PortStatus : " + cv_PortStatus + "\n";
            data += "cv_WorkCount : " + cv_WorkCount + "\n";
            data += "cv_WaitUnload : " + cv_WaitUnload + "\n";
            data += "cv_ProductionType : " + cv_ProductionType + "\n";
            data += "cv_EfemPortType : " + cv_EfemPortType + "\n";
            data += "cv_IsWaitCancel : " + cv_IsWaitCancel + "\n";
            data += "cv_IsWaitAbort : " + cv_IsWaitAbort + "\n";
            for (int i = 1; i <= cv_SlotCount; i++)
            {
                data += GlassDataMap[i].GetGlassDataStr();
            }
            return data;
        }
    }
}

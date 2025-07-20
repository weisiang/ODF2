using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonData.HIRATA;
using KgsCommon;
using LGC.Comm;
using System.Drawing;
using BaseAp;

namespace LGC
{
    abstract class Obj 
    {
        public bool cv_IsHome = false;
        public bool cv_IsStatus = false;
        public bool cv_StatusError = false;

        public bool cv_IsResetError = false;
        public bool cv_IsRemapping = false;
        public int cv_Id = 0;
        public int cv_SlotCount = 0;
        public int cv_RobotPos = 0;
        public int cv_sideGroup = 0;
        public Obj(int m_Id, int m_slotCount )
        {
            cv_Id = m_Id;
            cv_SlotCount = m_slotCount;
        }
        public bool IsSlotValid(int m_Slot)
        {
            bool rtn = false;
            if(m_Slot > 0 && m_Slot <= cv_SlotCount)
            {
                rtn = true;
            }
            return rtn;
        }
        protected virtual void InitComm()
        {
        }
        protected virtual void InitData()
        {
        }
        public abstract void SendDataViaMmf();
        public enSideGroup PSideGroup
        {
            get { return (enSideGroup)cv_sideGroup; }
            set { cv_sideGroup = (int)value; }
        }
        public bool PIsHome
        {
            get { return cv_IsHome; }
            set { cv_IsHome = value; }
        }
        public bool PIsRemapping
        {
            get { return cv_IsRemapping; }
            set { cv_IsRemapping = value; }
        }
        public bool PIsStatus
        {
            get { return cv_IsStatus; }
            set { cv_IsStatus = value; }
        }
        public bool PIsResetError
        {
            get { return cv_IsResetError; }
            set { cv_IsResetError = value; }
        }
        public bool PIsStatusError
        {
            get { return cv_StatusError; }
            set { cv_StatusError = value; }
        }
        
        public virtual bool CanAccess(bool m_IsLoad, int m_Slot , bool m_IsExchange=false)
        {
            return false;
        }
        public virtual EquipmentStatus GetStatus()
        {
            return EquipmentStatus.Down;
        }
    }
}

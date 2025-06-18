using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonData.HIRATA;

namespace LGC.Comm
{
    internal class ObjComm
    {
        public virtual bool GetSubstrateFromPort(CommandData m_Command)
        {
            return false;
        }
        public virtual bool PutSubstrateFromPort(CommandData m_Command)
        {
            return false;
        }
        public virtual bool GetStandbySubstrateFromPort(CommandData m_Command)
        {
            return false;
        }
        public virtual bool PutStandbSubstrateFromPort(CommandData m_Command)
        {
            return false;
        }
        public virtual bool GetSubstrateFromEq(CommandData m_Command)
        {
            return false;
        }
        public virtual bool PutSubstrateFromEq(CommandData m_Command)
        {
            return false;
        }
        public virtual bool GetStandbySubstrateFromEq(CommandData m_Command)
        {
            return false;
        }
        public virtual bool PutStandbSubstrateFromEq(CommandData m_Command)
        {
            return false;
        }
    }
}

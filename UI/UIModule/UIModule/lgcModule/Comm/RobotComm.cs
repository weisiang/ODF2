using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HirataRbAPI;
using CommonData.HIRATA;
using LGC;

namespace LGC.Comm
{
    internal class RobotComm : ObjComm
    {
        //RobotComm provide api for set HIRATA  api commands.
        public CommonData.HIRATA.GlassData cv_GlassDataGetFromEq = null;
        public RobotComm()
        {
        }
    }
}

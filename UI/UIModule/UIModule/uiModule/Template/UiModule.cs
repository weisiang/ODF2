using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KgsCommon;
using MxLib;
using CommonData.HIRATA;
using System.Reflection;
using System.Diagnostics;
using BaseAp;

namespace UI
{ 
    class UiModule : uiBase
    {
        public static UiModule g_UiModule = null;
        public static KMemoryIOClient PMio
        {
            get { return g_UiModule.cv_Mio1; }
        }

        public UiModule() : base(FdModule.UI)
        {
            g_UiModule = this;
            WriteLog1(LogLevelType.General, "[UI module start]");
        }
    }
}

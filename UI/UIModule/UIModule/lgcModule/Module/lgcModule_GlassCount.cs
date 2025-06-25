using System;
using System.Collections.Generic;
using CommonData.HIRATA;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using KgsCommon;
using CommonData;
using BaseAp;
using System.Reflection;
using System.Diagnostics;

namespace LGC
{
    public partial class LgcModule
    {
        public void InitGlassCount()
        {
            cv_GlassCountData.SetFilePath(CommonStaticData.g_GlassCountDataPath);
            cv_GlassCountData.PIsAutoSave = false;
            LGCController.triggerLgcEvent(typeof(GlassCountData).Name, cv_GlassCountData);
        }
        public void ReadGlassCountValueFromPlc()
        {
            int history = PMio.GetPortValue(0x424A);
            history += (PMio.GetPortValue(0x424B) << 16);
            cv_GlassCountData.PHistoryCount = history;
        }
    }
}

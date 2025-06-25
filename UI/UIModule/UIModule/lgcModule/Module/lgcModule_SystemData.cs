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
        public void InitSystemData()
        {
            PSystemData.SetFilePath(CommonStaticData.g_StatsRecordPath);
            PSystemData.PIsAutoSave = true;
            PSystemData.LoadFromFile();
            PSystemData.SaveToFile();
            LGCController.triggerLgcEvent(typeof(SystemData).Name, PSystemData);
        }
    }
}

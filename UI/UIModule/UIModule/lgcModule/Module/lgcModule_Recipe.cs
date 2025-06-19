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
        public void InitRecipe()
        {
            cv_Recipes.SetFilePath(CommonData.HIRATA.CommonStaticData.g_RootConfigFolderPath + CommonData.HIRATA.CommonStaticData.g_FDModuleName + "\\PPID.xml");
            cv_Recipes.PIsAutoSave = true;
            cv_Recipes.LoadFromFile();
            cv_Recipes.SaveToFile();
        }
    }
}

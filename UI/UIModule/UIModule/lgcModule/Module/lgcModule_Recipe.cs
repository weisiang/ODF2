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
using CommonData.HIRATA;

namespace LGC
{
    public partial class LgcModule
    {
        public void InitRecipe()
        {
            cv_Recipes.SetFilePath(CommonData.HIRATA.CommonStaticData.g_RootConfigFolderPath + CommonData.HIRATA.CommonStaticData.g_LgcModule + "\\PPID.xml");
            cv_Recipes.PIsAutoSave = true;
            cv_Recipes.LoadFromFile();
            cv_Recipes.SaveToFile();
            LGCController.triggerLgcEvent(typeof(RecipeData).Name, cv_Recipes);
        }
    }
}

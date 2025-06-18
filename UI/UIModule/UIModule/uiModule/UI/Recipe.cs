using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KgsCommon;

namespace UI
{
    public partial class Recipe : Form
    {
        private string cv_CurFilePath = System.IO.Directory.GetCurrentDirectory() + "\\..\\Config\\CurrentRecipe.xml";

        private string cv_FilePath = System.IO.Directory.GetCurrentDirectory() + "\\..\\Config\\Recipe.xml";

        private KXmlItem cv_FileXml = null;

        private KXmlItem cv_CurrentRecipeFileXml = null;

        enum OptionType { None, Create, Remove, Modify, CurrentRecipe, }

        //private OptionType cv_OptionType = OptionType.None;

        //public static Dictionary<int, UnicomPLC.Recipe> cv_EachRecipeMappingRowIndex = new Dictionary<int, UnicomPLC.Recipe>();

        public Recipe()
        {
            /*
            InitializeComponent();
            cv_ButtonPanel.Dock = DockStyle.Top;
            cv_DataView.Dock = DockStyle.Fill;
            IniXmlObj();
            IniRecipe();
            IniCurrenRecipe();
            */
        }

        private void IniRecipe()
        {
            /*
                <No109 Number="109" Thickness="109.99" Heigth="2600" Width="2250" Time="2016/10/21 10:04:59"/>
             */
            /*
            string id="";
            string thickness="";
            int height=0;
            int width=0;
            string time = "";

            for (int i = 0; i < cv_FileXml.ItemsByName["Recipe"].ItemNumber; i++)
            {
                KXmlItem each_item = cv_FileXml.ItemsByName["Recipe"].Items[i];
                id =each_item.Attributes["Number"].Trim(); 
                thickness = each_item.Attributes["Thickness"].Trim();
                height = SysUtils.StrToInt(each_item.Attributes["Heigth"].Trim());
                width = SysUtils.StrToInt(each_item.Attributes["Width"].Trim());
                time =each_item.Attributes["Time"].Trim();
                Add(id, thickness, height, width, time);
            }
            */
        }

        private void IniXmlObj()
        {
            /*
            if(!string.IsNullOrEmpty(cv_FilePath))
            {
                if(SysUtils.FileExists(cv_FilePath))
                {
                    if(cv_FileXml == null)
                    {
                        cv_FileXml = new KXmlItem();
                        cv_FileXml.LoadFromFile(cv_FilePath);
                    }
                }
            }
            if(!string.IsNullOrEmpty(cv_CurFilePath))
            {
                if(SysUtils.FileExists(cv_CurFilePath))
                {
                    if(cv_CurrentRecipeFileXml == null)
                    {
                        cv_CurrentRecipeFileXml = new KXmlItem();
                        cv_CurrentRecipeFileXml.LoadFromFile(cv_CurFilePath);
                    }
                }
            }
            */
        }

        private void IniCurrenRecipe()
        {
            /*
            bool is_find = false;
            UnicomPLC.Recipe recipe = null;
            if(!string.IsNullOrEmpty(cv_CurrentRecipeFileXml.ItemsByName["Current"].AsString.Trim() ) )
            {
                string tmp_cur_recipe = cv_CurrentRecipeFileXml.ItemsByName["Current"].AsString.Trim();
                GB.CurRecipe = tmp_cur_recipe;
                CPC.cv_BCEq.SetEqCurrentRecipeToBC(GB.CurRecipe);
                SetThicknessToGB(tmp_cur_recipe);
            }

            foreach (KeyValuePair<int, UnicomPLC.Recipe> hash in cv_EachRecipeMappingRowIndex)
            {
                if (hash.Value.RecipeNumber == GB.CurRecipe)
                {
                    is_find = true;
                    recipe = hash.Value;
                }
            }
            if(is_find)
            {
                CPC.cv_BCEq.ReportFDC(recipe);
            }
            */
        }

        private void SetThicknessToGB(string m_RecipeID)
        {
            /*
            bool is_find = false;
            foreach(UnicomPLC.Recipe each_recipe in cv_EachRecipeMappingRowIndex.Values )
            {
                if(each_recipe.RecipeNumber == GB.CurRecipe)
                {
                    is_find = true;
                    double thickness = SysUtils.StrToDouble(each_recipe.Thickness);
                    int write_thickness = Convert.ToInt32(thickness * 10);
                    GB.SetBGThickness(write_thickness);
                    break;
                }
            }
            if(!is_find)
            {
                LogicKernel.Common.PopForm("Set Glass thickness to GB , but recipe not found");
            }
            */
        }

        private void Recipe_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*
            e.Cancel = true;
            this.Hide();
            */
        }

        //add recipe info. to UI
        private void Add(string m_Number, string m_Thickness ,int m_Height , int m_Width , string m_Time)
        {
            /*
            DateTime tmp_date = DateTime.Now;
            if(string.IsNullOrEmpty(m_Time))
            {
                return;
            }
            UnicomPLC.Time tmp_Time = null;
            if (DateTime.TryParse(m_Time, out tmp_date))
            {
                tmp_Time = new Time(tmp_date);
                UnicomPLC.Recipe tmp_recipe = new UnicomPLC.Recipe(tmp_Time , m_Number, m_Thickness , m_Height , m_Width );
                string sTime = tmp_recipe.CreateTime.cv_DateTime.ToString("yyyy/MM/dd hh:mm:ss");
                Dictionary<int, UnicomPLC.Recipe> tmp_dic = new Dictionary<int, UnicomPLC.Recipe>();
                tmp_dic[0] = tmp_recipe;
                foreach (KeyValuePair<int, UnicomPLC.Recipe> hash in cv_EachRecipeMappingRowIndex)
                {
                    tmp_dic[hash.Key + 1] = hash.Value;
                }
                cv_EachRecipeMappingRowIndex = tmp_dic;
                cv_DataView.Rows.Insert(0, new object[] { m_Number, m_Thickness, m_Height , m_Width , sTime });
            }
            else
            {
                LogicKernel.EventHandler.WriteErrorLog("Recipe Date time tryparse error");
            }
            */
        }

        private void Add(string m_Number, string m_Thickness , int m_Height, int m_Width , bool m_IsModify = false)
        {
            /*
            if (!m_IsModify)
            {
                foreach (KeyValuePair<int, UnicomPLC.Recipe> hash in cv_EachRecipeMappingRowIndex)
                {
                    if (hash.Value.RecipeNumber == m_Number)
                    {
                        LogicKernel.Common.PopForm("The recipe already exist");
                        return;
                    }
                }
            }

            UnicomPLC.Recipe tmp = new UnicomPLC.Recipe(new UnicomPLC.Time(), m_Number, m_Thickness , m_Height , m_Width);
            string sTime = tmp.CreateTime.cv_DateTime.ToString("yyyy/MM/dd hh:mm:ss");

            //最新資料在最下方
            //DataGrid.Rows.Add(new Object[] { sTime, sCode, sText });
            //最新資料在最上方
            cv_DataView.Rows.Insert(0, new object[] { m_Number, m_Thickness , m_Height , m_Width , sTime });
            Dictionary<int, UnicomPLC.Recipe> tmp_dic = new Dictionary<int, UnicomPLC.Recipe>();
            tmp_dic[0] = tmp;
            foreach (KeyValuePair<int, UnicomPLC.Recipe> hash in cv_EachRecipeMappingRowIndex)
            {
                tmp_dic[hash.Key + 1] = hash.Value;
            }
            cv_EachRecipeMappingRowIndex = tmp_dic;
            if (!m_IsModify)
            {
                CPC.cv_BCEq.RecipeModifyReportToBC(tmp_dic[0].RecipeNumber, CPC.OPID, tmp_dic[0].CreateTime, RecipeModifyType.Create);
            }

            if(cv_FileXml != null)
            {
                KXmlItem recipe_item = new KXmlItem();
                recipe_item.Text = @"<No" + tmp_dic[0].RecipeNumber.ToString() + @"/>";
                recipe_item.Attributes["Number"] = tmp_dic[0].RecipeNumber.ToString();
                recipe_item.Attributes["Thickness"] = tmp_dic[0].Thickness;
                recipe_item.Attributes["Heigth"] = tmp_dic[0].Heigth.ToString();
                recipe_item.Attributes["Width"] = tmp_dic[0].Width.ToString();
                recipe_item.Attributes["Time"] = tmp_dic[0].CreateTime.cv_DateTime.ToString("yyyy/MM/dd hh:mm:ss");
                cv_FileXml.ItemsByName["Recipe"].AddItem(recipe_item);
                cv_FileXml.SaveToFile(cv_FilePath , true);
            }
            */
        }

        private void Remove(string m_Number)
        {
            /*
            int record_index = 0;
            bool is_find = false;
            foreach (KeyValuePair<int, UnicomPLC.Recipe> hash in cv_EachRecipeMappingRowIndex)
            {
                if (hash.Value.RecipeNumber == m_Number.Trim())
                {
                    is_find = true;
                    record_index = hash.Key;
                    break;
                }
            }
            if (is_find)
            {
                if (cv_EachRecipeMappingRowIndex[record_index].RecipeNumber == GB.CurRecipe)
                {
                    LogicKernel.Common.PopForm("The Recipe is Cur.Recipe. Can't remove");
                    return;
                }
                cv_DataView.Rows.RemoveAt(record_index);
                CPC.cv_BCEq.RecipeModifyReportToBC(cv_EachRecipeMappingRowIndex[record_index].RecipeNumber, CPC.OPID, cv_EachRecipeMappingRowIndex[record_index].CreateTime, RecipeModifyType.Delete);
                if(cv_FileXml != null)
                {
                    if(cv_FileXml.IsItemExist("No"+ cv_EachRecipeMappingRowIndex[record_index].RecipeNumber.ToString()))
                    {
                        cv_FileXml.ItemsByName["Recipe"].DeleteItemByLevelName("No" + cv_EachRecipeMappingRowIndex[record_index].RecipeNumber.ToString(), true);
                        cv_FileXml.SaveToFile(cv_FilePath);
                    }
                }
                Dictionary<int , UnicomPLC.Recipe> tmp_dic = new Dictionary<int,UnicomPLC.Recipe>();
                for(int i = 0 ; i < record_index ; i++)
                {
                    tmp_dic[i] = cv_EachRecipeMappingRowIndex[i];
                }
                for (int i = record_index + 1; i < cv_EachRecipeMappingRowIndex.Count; i++)
                {
                    tmp_dic[i-1] = cv_EachRecipeMappingRowIndex[i];
                }
                cv_EachRecipeMappingRowIndex = tmp_dic;
            }
            */
        }

        //若是cur recipe 且GB為RUN or NAR 有片 , 不能修改.
        private void Modify(string m_Number, string m_Thickness , int m_Height , int m_Width)
        {
            /*
            if(m_Number == GB.CurRecipe) 
            {
                if( (GB.GBStatus == GBPLC.EqStatus.Run) || (GB.cv_CarrirMappingDataInSputter.Count !=0 ) )
                {
                    LogicKernel.Common.PopForm("The recipe is current recipe and have glass in Handler or NAR");
                    return;
                }
            }

            int record_index = 0;
            bool is_find = false;
            try
            {
                foreach (KeyValuePair<int, UnicomPLC.Recipe> hash in cv_EachRecipeMappingRowIndex)
                {
                    if (hash.Value.RecipeNumber == m_Number.Trim())
                    {
                        is_find = true;
                        record_index = hash.Key;
                        break;
                    }
                }
            }
            catch(Exception e)
            {
                LogicKernel.EventHandler.WriteErrorLog("ERROR foreach loop in Modify function");
                LogicKernel.EventHandler.WriteErrorLog(e.ToString());
            }
            if (is_find)
            {
                cv_DataView.Rows.RemoveAt(record_index);
                CPC.cv_BCEq.RecipeModifyReportToBC(cv_EachRecipeMappingRowIndex[record_index].RecipeNumber, CPC.OPID, cv_EachRecipeMappingRowIndex[record_index].CreateTime, RecipeModifyType.Modify);
                if(cv_FileXml != null)
                {
                    if(cv_FileXml.IsItemExist("No" + cv_EachRecipeMappingRowIndex[record_index].RecipeNumber ))
                    {
                        cv_FileXml.ItemsByName["Recipe"].DeleteItemByLevelName("No" + cv_EachRecipeMappingRowIndex[record_index].RecipeNumber, true);
                        cv_FileXml.SaveToFile(cv_FilePath);
                    }
                }
                Dictionary<int , UnicomPLC.Recipe> tmp_dic = new Dictionary<int,UnicomPLC.Recipe>();
                for(int i = 0 ; i < record_index ; i++)
                {
                    tmp_dic[i] = cv_EachRecipeMappingRowIndex[i];
                }
                for (int i = record_index + 1; i < cv_EachRecipeMappingRowIndex.Count; i++)
                {
                    tmp_dic[i-1] = cv_EachRecipeMappingRowIndex[i];
                }
                cv_EachRecipeMappingRowIndex = tmp_dic;
                Add(m_Number, m_Thickness, m_Height, m_Width, true);
                if(m_Number == GB.CurRecipe)
                {
                    SetThicknessToGB(m_Number.Trim());
                }
            }
            */
        }

        private void cv_BtExe_Click(object sender, EventArgs e)
        {
            /*
            string user_log = "[ User push Recipe UI EXE button ] \n";
            if (cv_OptionType == OptionType.Create)
            {
                if (IsCharacterInvalid(cv_TxRecipeNumber.Text.Trim()))
                {
                    return;
                }
                else if(IsThicknessInvalid(cv_TxRecipeSize.Text.Trim()))
                {
                    return;
                }
                else
                {
                    Add(cv_TxRecipeNumber.Text.Trim(), cv_TxRecipeSize.Text.Trim(), SysUtils.StrToInt(cv_TxRecipeHeight.Text.Trim()), SysUtils.StrToInt(cv_TxRecipeWidth.Text.Trim()));
                    user_log += "ADD \n";
                    user_log += "cv_TxRecipeNumber：" + cv_TxRecipeNumber.Text.Trim() + "\n";
                    user_log += "cv_TxRecipeSize：" + cv_TxRecipeSize.Text.Trim() + "\n";
                    user_log += "cv_TxRecipeHeight：" + cv_TxRecipeHeight.Text.Trim() + "\n";
                    user_log += "cv_TxRecipeWidth：" + cv_TxRecipeWidth.Text.Trim() + "\n";
                }
            }
            else if (cv_OptionType == OptionType.Remove)
            {
                Remove(cv_TxRecipeNumber.Text.Trim());
                user_log += "Remove \n";
                user_log += "cv_TxRecipeNumber：" + cv_TxRecipeNumber.Text.Trim() + "\n";
            }
            else if (cv_OptionType == OptionType.Modify)
            {
                if(IsThicknessInvalid(cv_TxRecipeSize.Text.Trim()))
                {
                    return;
                }
                Modify(cv_TxRecipeNumber.Text.Trim() , cv_TxRecipeSize.Text.Trim() , SysUtils.StrToInt(cv_TxRecipeHeight.Text.Trim()) , SysUtils.StrToInt(cv_TxRecipeWidth.Text.Trim()));
                user_log += "Modify \n";
                user_log += "cv_TxRecipeNumber：" + cv_TxRecipeNumber.Text.Trim() + "\n";
                user_log += "cv_TxRecipeSize：" + cv_TxRecipeSize.Text.Trim() + "\n";
                user_log += "cv_TxRecipeHeight：" + cv_TxRecipeHeight.Text.Trim() + "\n";
                user_log += "cv_TxRecipeWidth：" + cv_TxRecipeWidth.Text.Trim() + "\n";
            }
            else if (cv_OptionType == OptionType.CurrentRecipe)
            {
                ChoiceCurrentRecipe(cv_TxRecipeNumber.Text.Trim());
                user_log += "CurrentRecipe \n";
                user_log += "cv_TxRecipeNumber：" + cv_TxRecipeNumber.Text.Trim() + "\n";
            }
            if (!string.IsNullOrEmpty(user_log))
            {
                user_log += LogicKernel.Common.cv_SplitSymbol;
                LogicKernel.EventHandler.WriteUserLog(user_log);
            }
            */
        }

        private void currentRecipeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
            cv_OptionType = OptionType.CurrentRecipe;
            cv_BtOption.Text = "CurrentRecipe";
            */
        }

        //監控NAR 有無玻璃 and Handler 是否為run.
        private void ChoiceCurrentRecipe(string m_Number)
        {
            /*
            int record_index = 0;
            UnicomPLC.Recipe recipe = null;
            bool is_find = false;
            try
            {
                foreach (KeyValuePair<int, UnicomPLC.Recipe> hash in cv_EachRecipeMappingRowIndex)
                {
                    if (hash.Value.RecipeNumber == m_Number.Trim())
                    {
                        is_find = true;
                        record_index = hash.Key;
                        recipe = hash.Value;
                        break;
                    }
                }
            }
            catch(Exception e)
            {
                LogicKernel.EventHandler.WriteErrorLog("ERROR foreach loop in ChoiceCurrentRecipe function");
                LogicKernel.EventHandler.WriteErrorLog(e.ToString());
            }
            if (is_find)
            {
                if ((GB.cv_CarrirMappingDataInSputter.Count == 0) &&
                    (GB.GBStatus != GBPLC.EqStatus.Run) && (GB.cv_OtherPositingMappingData.Count==0) &&
                    (GB.cv_CarrirMappingDataInSputter.Count == 0) )
                {
                    GB.CurRecipe = m_Number.Trim();
                    CPC.cv_BCEq.SetEqCurrentRecipeToBC(GB.CurRecipe);
                    cv_CurrentRecipeFileXml.ItemsByName["Current"].AsString = GB.CurRecipe;
                    cv_CurrentRecipeFileXml.SaveToFile(cv_CurFilePath);
                    SetThicknessToGB(m_Number.Trim());
                    //add report FDC to BC when change recipe.(due to FDC report recipe info. only).
                    CPC.cv_BCEq.ReportFDC(recipe);
                }
                else
                {
                    LogicKernel.Common.PopForm("NAR / Handler has glass or Handler in run. Please check");
                }
            }
            */
        }
            /*
        public static UnicomPLC.Recipe GetCurrentRecipe()
        {
            UnicomPLC.Recipe tmp = null;
            foreach (KeyValuePair<int, UnicomPLC.Recipe> hash in cv_EachRecipeMappingRowIndex)
            {
                if (hash.Value.RecipeNumber == GB.CurRecipe.Trim())
                {
                    tmp = hash.Value;
                    break;
                }
            }
            return tmp;
        }
            */


            /*
        private bool IsCharacterInvalid(string m_Str)
        {
            bool result = false;
            if (Regex.Match(m_Str, @"[oOiI]").Success)
            {
                result = true;
            }
            else if (Regex.Match(m_Str, @".*\s+.*").Success)
            {
                result = true;
            }
            else if (Regex.Match(m_Str, @"[^a-zA-Z0-9]").Success)
            {
                result = true;
            }

            if(result)
            {
                LogicKernel.Common.PopForm(m_Str + " is illegal");
            }
            return result;
        }
            */
            /*
        private bool IsThicknessInvalid(string m_Str)
        {
            bool result = false;
            string rtn_msg = "";
            float tmp_float = 0.0F;
            if(m_Str.Length != 3)
            {
                rtn_msg = "Thickness is Invalid!!!\n";
                result = true;
            }

            if(!float.TryParse(m_Str , out tmp_float) )
            {
                rtn_msg = "Thickness must digital!!!\n";
                result = true;
            }
            else
            {
                if( (tmp_float < 0.3) || (tmp_float > 1.0) )
                {
                    rtn_msg = "Thickness range : 0.3 - 1.0\n";
                    result = true;
                }
            }
            if(!string.IsNullOrEmpty(rtn_msg))
            {
                LogicKernel.Common.PopForm(rtn_msg);
            }
            return result;
        }
            */

        private void cv_DataView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            /*
            string select_str = cv_DataView.Rows[e.RowIndex].Cells[0].Value.ToString();

            foreach(UnicomPLC.Recipe each_recipe in cv_EachRecipeMappingRowIndex.Values)
            {
                if(select_str == each_recipe.RecipeNumber)
                {
                    cv_TxRecipeHeight.Text = each_recipe.Heigth.ToString();
                    cv_TxRecipeNumber.Text = each_recipe.RecipeNumber;
                    cv_TxRecipeSize.Text = each_recipe.Thickness;
                    cv_TxRecipeWidth.Text = each_recipe.Width.ToString();
                    break;
                }
            }
            */
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            /*
            //index : 0 = create , 1 = erase , 2 = modify , 3 = current
            int select_text = cv_CbRecipeOption.SelectedIndex;
            switch(select_text)
            {
                case 0:
                    cv_OptionType = OptionType.Create;
                    cv_BtOption.Text = "Create";
                    break;
                case 1:
                    cv_OptionType = OptionType.Remove;
                    cv_BtOption.Text = "Erase";
                    break;
                case 2 :
                    cv_OptionType = OptionType.Modify;
                    cv_BtOption.Text = "Modify";
                    break;
                case 3 :
                    cv_OptionType = OptionType.CurrentRecipe;
                    cv_BtOption.Text = "Current";
                    break;
            }
            */
        }


    }
}

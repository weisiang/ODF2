using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using KgsCommon;

namespace CommonData.HIRATA
{
    public class RecipeItem
    {
        public string cv_Id = "";
        public string cv_Time = "";
        public string cv_Decription = "";
        public int cv_Flow;
        public float cv_GlassVasDegree;
        public float cv_WaferIJPDegree;
        public float cv_WaferVASDegree;
        public float cv_WaferSealDegree;
        public float cv_WaferAoiDegree;
        public bool cv_VasNeedGlass;
        public List<int> cv_waferSteps = new List<int>();
        public List<int> cv_GlassSteps = new List<int>();
        public bool PVasNeedGlass
        {
            get { return cv_VasNeedGlass; }
            set { cv_VasNeedGlass = value; }
        }
        public string PId
        {
            get { return cv_Id; }
            set { cv_Id = value; }
        }
        public string PTime
        {
            get { return cv_Time; }
            set { cv_Time = value; }
        }
        public string PDecription
        {
            get { return cv_Decription; }
            set { cv_Decription = value; }
        }
        public CommonData.HIRATA.OdfFlow PFlow
        {
            get { return (CommonData.HIRATA.OdfFlow)cv_Flow; }
            set { cv_Flow = (int)value; }
        }
        public float PGlassVASDegree
        {
            get { return cv_GlassVasDegree; }
            set { cv_GlassVasDegree = value; }
        }
        public float PWaferIJPDegree
        {
            get { return cv_WaferIJPDegree; }
            set { cv_WaferIJPDegree = value; }
        }
        public float PWaferVASDegree
        {
            get { return cv_WaferVASDegree; }
            set { cv_WaferVASDegree = value; }
        }
        public float PWaferSealDegree
        {
            get { return cv_WaferSealDegree; }
            set { cv_WaferSealDegree = value; }
        }
        public float PWaferAoiDegree
        {
            get { return cv_WaferAoiDegree; }
            set { cv_WaferAoiDegree = value; }
        }
        public KXmlItem GetXml()
        {
            KXmlItem body = EventCenterBase.ParseObjectToKXmlItem(this, KParseObjToXmlPropertyType.Field);
            return body;
        }
        public void LoadFromXml(KXmlItem m_Xml)
        {
            EventCenterBase.ParseXmlToObject(this, m_Xml);
        }
    }
    public class RecipeData : CommonDatabase
    {
        public delegate void DeleRecipeAction(DataEidtAction m_Action, List<RecipeItem> m_Recipes);
        public event DeleRecipeAction EventRecipeAction;

        public List<RecipeItem> cv_RecipeList = new List<RecipeItem>();
        public string cv_CurRecipeId = "";
        public string PCurRecipeId
        {
            get { return cv_CurRecipeId; }
        }
        public RecipeData()
        {
        }

        public bool GetRecipeItem(string m_RecipeId, out RecipeItem m_RecipeItem)
        {
            bool rtn = false;
            RecipeItem result = null;
            if (!string.IsNullOrEmpty(m_RecipeId))
            {
                int index = cv_RecipeList.FindIndex(x => x.PId == m_RecipeId);
                if (index != -1)
                {
                    result = cv_RecipeList[index];
                    rtn = true;
                }
            }
            m_RecipeItem = result;
            return rtn;
        }
        public bool GetCurRecipe(out RecipeItem m_Currecipe)
        {
            bool rtn = false;
            RecipeItem result = null;
            if (!string.IsNullOrEmpty(PCurRecipeId))
            {
                int index = cv_RecipeList.FindIndex(x => x.PId == PCurRecipeId);
                if (index != -1)
                {
                    result = cv_RecipeList[index];
                    rtn = true;
                }
            }
            m_Currecipe = result;
            return rtn;
        }
        public bool SetCurRecipe(string m_RecipeId)
        {
            bool rtn = false;
            lock (cv_obj)
            {
                try
                {
                    if (cv_RecipeList.Exists(x => x.PId == m_RecipeId))
                    {
                        int index = cv_RecipeList.FindIndex(x => x.PId == m_RecipeId);
                        RecipeItem cur_recipe = cv_RecipeList[index];
                        cv_CurRecipeId = cur_recipe.PId;

                        if (cv_IsAutoSave)
                        {
                            SaveToFile();
                        }
                        if (EventRecipeAction != null)
                        {
                            List<RecipeItem> recipes = new List<RecipeItem>();
                            recipes.Add(cur_recipe);
                            EventRecipeAction(DataEidtAction.SetCur, recipes);
                        }
                    }
                }
                catch (Exception e)
                {
                }
            }
            return rtn;
        }
        public bool IsRecipeExist(string m_RecipeId)
        {
            return cv_RecipeList.Exists(x => x.PId == m_RecipeId);
        }
        public bool IsRecipeExist(RecipeItem m_RecipeItem)
        {
            return IsRecipeExist(m_RecipeItem.PId);
        }
        public bool AddRecipe(RecipeItem m_RecipeItem)
        {
            bool rtn = false;
            lock (cv_obj)
            {
                try
                {
                    if (!IsRecipeExist(m_RecipeItem))
                    {
                        if(
                            m_RecipeItem.PFlow == OdfFlow.Flow1 ||
                            m_RecipeItem.PFlow == OdfFlow.Flow2 ||
                            m_RecipeItem.PFlow == OdfFlow.Flow3 ||
                            m_RecipeItem.PFlow == OdfFlow.Flow4 ||
                            m_RecipeItem.PFlow == OdfFlow.Flow5 ||
                            m_RecipeItem.PFlow == OdfFlow.Flow6 ||
                            m_RecipeItem.PFlow == OdfFlow.Flow7 ||
                            m_RecipeItem.PFlow == OdfFlow.Flow8 ||
                            m_RecipeItem.PFlow == OdfFlow.Flow9 ||
                            m_RecipeItem.PFlow == OdfFlow.Flow10 ||
                            m_RecipeItem.PFlow == OdfFlow.Flow11 ||
                            m_RecipeItem.PFlow == OdfFlow.Flow12 ||
                            m_RecipeItem.PFlow == OdfFlow.Flow13 ||
                            m_RecipeItem.PFlow == OdfFlow.Flow14 ||
                            m_RecipeItem.PFlow == OdfFlow.Flow25 ||
                            m_RecipeItem.PFlow == OdfFlow.Flow26
                            )
                        {

                            m_RecipeItem.PVasNeedGlass = true;
                        }
                        cv_RecipeList.Add(m_RecipeItem);
                        if (cv_IsAutoSave)
                        {
                            SaveToFile();
                        }
                        if (EventRecipeAction != null)
                        {
                            List<RecipeItem> recipes = new List<RecipeItem>();
                            recipes.Add(m_RecipeItem);
                            EventRecipeAction(DataEidtAction.Add, recipes);
                        }
                        rtn = true;
                    }
                }
                catch (Exception e)
                {
                }
            }
            return rtn;
        }
        public void AddRecipe(List<RecipeItem> m_RecipeItems)
        {
            foreach (RecipeItem item in m_RecipeItems)
            {
                AddRecipe(item);
            }
        }
        public bool DelRecipe(RecipeItem m_RecipeItem)
        {
            bool rtn = false;
            lock (cv_obj)
            {
                try
                {
                    int index = cv_RecipeList.FindIndex(x => x.PId == m_RecipeItem.PId);
                    if (index != -1)
                    {
                        cv_RecipeList.RemoveAt(index);
                        if (cv_IsAutoSave)
                        {
                            SaveToFile();
                        }
                        if (EventRecipeAction != null)
                        {
                            List<RecipeItem> recipes = new List<RecipeItem>();
                            recipes.Add(m_RecipeItem);
                            EventRecipeAction(DataEidtAction.Del, recipes);
                        }
                        rtn = true;
                    }
                }
                catch (Exception e)
                {
                }
            }
            return rtn;
        }
        public void DelRecipe(List<RecipeItem> m_RecipeItems)
        {
            foreach (RecipeItem item in m_RecipeItems)
            {
                DelRecipe(item);
            }
        }
        public bool ModifyRecipe(RecipeItem m_RecipeItem)
        {
            bool rtn = false;
            lock (cv_obj)
            {
                try
                {
                    if (IsRecipeExist(m_RecipeItem))
                    {
                        int index = cv_RecipeList.FindIndex(x => x.PId == m_RecipeItem.PId);
                        if (index != -1)
                        {
                            cv_RecipeList.RemoveAt(index);
                            cv_RecipeList.Add(m_RecipeItem);
                            if (cv_IsAutoSave)
                            {
                                SaveToFile();
                            }
                            rtn = true;
                        }
                    }
                }
                catch (Exception e)
                {
                }
            }
            if (rtn)
            {

                if (EventRecipeAction != null)
                {
                    List<RecipeItem> recipes = new List<RecipeItem>();
                    recipes.Add(m_RecipeItem);
                    EventRecipeAction(DataEidtAction.Edit, recipes);
                }
            }
            return rtn;
        }
        public void LoadFromFile()
        {
            if (!string.IsNullOrEmpty(cv_FilePath))
            {
                KXmlItem recipe_xml = new KXmlItem();
                recipe_xml.LoadFromFile(cv_FilePath);
                if (recipe_xml.ItemsByName["PPID"].ItemType == KXmlItemType.itxList && recipe_xml.ItemsByName["PPID"].ItemNumber != 0)
                {

                    EventCenterBase.ParseXmlToObject(this, recipe_xml.ItemsByName["RecipeData"]);
                    /*
                    int recipe_count = recipe_xml.ItemsByName["PPID"].ItemNumber;
                    lock (cv_obj)
                    {
                        try
                        {
                            for (int i = 0; i < recipe_count; i++)
                            {
                                KXmlItem item = recipe_xml.ItemsByName["PPID"].Items[i];
                                RecipeItem tmp = new RecipeItem();
                                tmp.LoadFromXml(item);
                                if (!IsRecipeExist(tmp.PId))
                                {
                                    cv_RecipeList.Add(tmp);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    */
                }
            }
        }
        public void SaveToFile()
        {
            KXmlItem tmp = new KXmlItem();
            tmp.Text = "@<PPID/>";
            KXmlItem body = EventCenterBase.ParseObjectToKXmlItem(this, KParseObjToXmlPropertyType.Field);
            tmp.ItemsByName["PPID"].AddItem(body);
            //int recipe_count = cv_RecipeList.Count;
            /*
            lock (cv_obj)
            {
                try
                {
                    for (int i = 0; i < recipe_count; i++)
                    {
                        tmp.ItemsByName["PPID"].AddItem(cv_RecipeList[i].GetXml());
                    }
                }
                catch (Exception e)
                {
                }
            }
            */
            tmp.SaveToFile(cv_FilePath, true);
        }
        public void SetFilePath(string m_Path)
        {
            cv_FilePath = m_Path;
        }
        public void Clone(RecipeData m_OtherRecipeData)
        {
            this.cv_RecipeList = m_OtherRecipeData.cv_RecipeList;
            this.cv_CurRecipeId = m_OtherRecipeData.cv_CurRecipeId;
        }
    }
}

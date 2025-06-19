using KgsCommon;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonData.HIRATA;
using BaseAp;

namespace UI
{
    public partial class CstDataConfirm : Form
    {
        int cv_PortId;
        public CstDataConfirm(int m_PortId)
        {
            cv_PortId = m_PortId;
            InitializeComponent();
            TextBox_PortNo.Text = cv_PortId.ToString().PadLeft(2, '0');
        }
        public void Registe()
        {
            cb_Recipe.Enabled = true;
            cb_Recipe.Text = "";
            cb_Substrate.Enabled = true;
            cb_Substrate.SelectedIndex = -1;
            TextBox_LotId.Text = UiForm.GetPort(cv_PortId).cv_Data.PLotId.Trim();
            panOfflineSlot.Controls.Clear();
            int slot_number = UiForm.GetPort(cv_PortId).cv_SlotCount;
            Dictionary<int, CommonData.HIRATA.GlassData> glass_map = UiForm.GetPort(cv_PortId).cv_Data.GlassDataMap;
            for (int i = slot_number; i > 0; i--)
            {
                SlotItem tmp = new SlotItem();
                tmp.SlotNo.Text = i.ToString().PadLeft(2, '0');
                tmp.Dock = DockStyle.Bottom;
                if (!glass_map[i].PHasSensor)
                {
                    tmp.DisableControls();
                }
                panOfflineSlot.Controls.Add(tmp);
            }
            cb_Recipe.Items.Clear();
            //cb_Recipe.Items.AddRange(Form1.cv_Recipes.cv_RecipeList);
            foreach (CommonData.HIRATA.RecipeItem item in lgcBase.cv_Recipes.cv_RecipeList)
            {
                cb_Recipe.Items.Add(item.cv_Id);
            }
            RecipeItem recipe = null;
            if (lgcBase.cv_Recipes.GetCurRecipe(out recipe))
            {
                if (cb_Recipe.Items.Contains(recipe.PId))
                {
                    cb_Recipe.Text = recipe.PId;
                    cb_Recipe.Enabled = false;
                }
            }

            if (UiForm.GetPort(cv_PortId).cv_Data.PPortMode == PortMode.Unloader)
            {
                cb_Substrate.SelectedIndex = -1;
                cb_Substrate.Enabled = false;
            }
            string default_recipe = "0";
            txt_SDP1Recipe.Text = default_recipe;
            txt_SDP2Recipe.Text = default_recipe;
            txt_IJP1Recipe.Text = default_recipe;
            txt_VAS1Recipe.Text = default_recipe;
            txt_UV1Recipe.Text = default_recipe;
            txt_UV2Recipe.Text = default_recipe;
            txt_AOI1Recipe.Text = default_recipe;
            if (UiForm.GetPort(cv_PortId).cv_Data.PPortMode == PortMode.Unloader)
            {
                txt_SDP1Recipe.Enabled = false;
                txt_SDP2Recipe.Enabled = false;
                txt_IJP1Recipe.Enabled = false;
                txt_VAS1Recipe.Enabled = false;
                txt_UV1Recipe.Enabled = false;
                txt_UV2Recipe.Enabled = false;
                txt_AOI1Recipe.Enabled = false;
            }
            else if (UiForm.GetPort(cv_PortId).cv_Data.PPortMode == PortMode.Loader)
            {
                txt_SDP1Recipe.Enabled = true;
                txt_SDP2Recipe.Enabled = true;
                txt_IJP1Recipe.Enabled = true;
                txt_VAS1Recipe.Enabled = true;
                txt_UV1Recipe.Enabled = true;
                txt_UV2Recipe.Enabled = true;
                txt_AOI1Recipe.Enabled = true;
            }
        }
        private void Form_CstDataConfirm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        private void Button_Cancel_Click(object sender, EventArgs e)
        {
            CommonData.HIRATA.MDPopMonitorFormRep obj = new CommonData.HIRATA.MDPopMonitorFormRep();
            UIController.SendMonitorReply(cv_PortId, CommonData.HIRATA.Result.NG);
            this.Close();
        }
        private bool CheckData()
        {
            bool rtn = true;
            string foup_id = TextBox_LotId.Text.Trim();
            string foup_seq = TextBox_CstId.Text.Trim();
            string work_order = TextBox_WorkOrder.Text.Trim();
            if (String.IsNullOrEmpty(foup_seq) || String.IsNullOrEmpty(foup_id) || String.IsNullOrEmpty(work_order))
            {
                UI.CommonStaticData.PopForm("Data Can't empty ", true, false);
                rtn = false;
                return rtn;
            }
            if (Regex.Match(foup_seq, @"\W").Success || Regex.Match(work_order, @"\W").Success)
            {
                UI.CommonStaticData.PopForm("Foup data can input digital only ", true, false);
                rtn = false;
                return rtn;
            }
            if (Regex.Match(txt_SDP1Recipe.Text, @"\W").Success || Regex.Match(txt_SDP2Recipe.Text, @"\W").Success ||
                Regex.Match(txt_IJP1Recipe.Text, @"\W").Success || Regex.Match(txt_VAS1Recipe.Text, @"\W").Success ||
                Regex.Match(txt_UV1Recipe.Text, @"\W").Success || Regex.Match(txt_UV2Recipe.Text, @"\W").Success ||
                Regex.Match(txt_AOI1Recipe.Text, @"\W").Success)
            {
                UI.CommonStaticData.PopForm("EQ recipe can input digital only", true, false);
                rtn = false;
                return rtn;
            }

            int int_foup_seq = int.Parse(foup_seq);
            int int_work_order = int.Parse(work_order);
            if (int_foup_seq < 1 || int_foup_seq > 255 || int_work_order < 1 || int_work_order > 255)
            {
                UI.CommonStaticData.PopForm("Foup seq and work order's range is 1-255 ", true, false);
                rtn = false;
                return rtn;
            }
            bool has_process_job = false;
            for (int i = 0; i < panOfflineSlot.Controls.Count; i++)
            {
                SlotItem slot_obj = panOfflineSlot.Controls[i] as SlotItem;
                if (slot_obj.GlassId.Enabled)
                {
                    if (string.IsNullOrEmpty(slot_obj.GlassId.Text.Trim()))
                    {
                        UI.CommonStaticData.PopForm("Wafer id can't empty ", true, false);
                        rtn = false;
                        break;
                    }
                    if (slot_obj.ProcessFlag.Checked)
                    {
                        has_process_job = true;
                    }
                }
            }
            if (!has_process_job)
            {
                if (UiForm.GetPort(cv_PortId).cv_Data.PPortMode != PortMode.Unloader)
                {

                    UI.CommonStaticData.PopForm("Select one wafer to do at least", true, false);
                    rtn = false;
                }
            }
            /*
            else
            {
                if(UiForm.GetPort(cv_PortId).cv_Data.PPortMode == PortMode.Unloader)
                {
                    rtn = true;
                }
            }
             * */
            return rtn;
        }
        private void Button_OK_Click(object sender, EventArgs e)
        {
            if (!CheckData())
            {
                return;
            }
            CommonData.HIRATA.MDPopMonitorFormRep obj = new CommonData.HIRATA.MDPopMonitorFormRep();
            Dictionary<int, CommonData.HIRATA.GlassData> glass_map = new Dictionary<int, CommonData.HIRATA.GlassData>();
            Dictionary<int, CommonData.HIRATA.GlassData> ori_glass_map = UiForm.GetPort(cv_PortId).cv_Data.GlassDataMap;
            ProductCategory substrate_type = ProductCategory.Glass;
            if (Regex.Match(cb_Substrate.Text.Trim(), "wafer", RegexOptions.IgnoreCase).Success)
            {
                substrate_type = ProductCategory.Wafer;
            }
            ProductCategory port_substrate_type = ProductCategory.Glass;
            if (UiForm.GetPort(cv_PortId).cv_Data.PProductionType == ProductCategory.Wafer)
                port_substrate_type = ProductCategory.Wafer;

            for (int i = 0; i < panOfflineSlot.Controls.Count; i++)
            {
                SlotItem slot_obj = panOfflineSlot.Controls[i] as SlotItem;
                CommonData.HIRATA.GlassData glass_item = new CommonData.HIRATA.GlassData();
                int priority = 0;
                int slot = Convert.ToInt16(slot_obj.SlotNo.Text);
                if(slot_obj.cb_priority.Text != "")
                {
                    priority = Convert.ToInt16(slot_obj.cb_priority.Text);
                }
                if (ori_glass_map[slot].PHasSensor)
                {
                    glass_item.PPriority = (uint)priority;
                    glass_item.PPID = "CIMOF";
                    glass_item.PPortProductionCategory = port_substrate_type;
                    glass_item.PProductionCategory = substrate_type;
                    glass_item.PCimMode = CommonData.HIRATA.OnlineMode.Offline;
                    glass_item.PFoupSeq = (uint)Convert.ToInt64(TextBox_CstId.Text);
                    glass_item.PWorkSlot = (uint)slot;
                    glass_item.PWorkOrderNo = (uint)Convert.ToInt64(TextBox_WorkOrder.Text);
                    glass_item.PHasSensor = true;
                    glass_item.PSlotInEq = (uint)slot;
                    glass_item.PId = slot_obj.GlassId.Text.Trim();
                    glass_item.PProcessFlag = (slot_obj.ProcessFlag.Checked ? CommonData.HIRATA.ProcessFlag.Need : CommonData.HIRATA.ProcessFlag.NotNeed);
                    glass_item.PSourcePort = (uint)cv_PortId;
                    AssignNodeRecipe(glass_item);
                }
                glass_map[Convert.ToInt16(slot_obj.SlotNo.Text)] = glass_item;
            }
            UiForm.GetPort(cv_PortId).cv_Data.GlassDataMap = glass_map;
            UiForm.GetPort(cv_PortId).cv_Data.PCurPPID = "CIMOF";
            UiForm.GetPort(cv_PortId).cv_Data.PFoupSeq = (uint)Convert.ToInt64(TextBox_CstId.Text);
            UiForm.GetPort(cv_PortId).cv_Data.PLotId = TextBox_LotId.Text.Trim();
            UIController.SendMonitorReply(cv_PortId, CommonData.HIRATA.Result.OK);
            this.Close();
        }
        private void AssignNodeRecipe(GlassData m_GlassData)
        {
            int node_index = m_GlassData.cv_Nods.FindIndex(x => x.PNodeId == (int)EqNode.SDP1);
            m_GlassData.cv_Nods[node_index].PRecipe = Convert.ToInt32(txt_SDP1Recipe.Text.Trim());

            node_index = m_GlassData.cv_Nods.FindIndex(x => x.PNodeId == (int)EqNode.SDP2);
            m_GlassData.cv_Nods[node_index].PRecipe = Convert.ToInt32(txt_SDP2Recipe.Text.Trim());

            node_index = m_GlassData.cv_Nods.FindIndex(x => x.PNodeId == (int)EqNode.SDP3);
            m_GlassData.cv_Nods[node_index].PRecipe = Convert.ToInt32(txt_SDP3Recipe.Text.Trim());

            node_index = m_GlassData.cv_Nods.FindIndex(x => x.PNodeId == (int)EqNode.SDP4);
            m_GlassData.cv_Nods[node_index].PRecipe = Convert.ToInt32(txt_SDP4Recipe.Text.Trim());

            node_index = m_GlassData.cv_Nods.FindIndex(x => x.PNodeId == (int)EqNode.SDP5);
            m_GlassData.cv_Nods[node_index].PRecipe = Convert.ToInt32(txt_SDP5Recipe.Text.Trim());

            node_index = m_GlassData.cv_Nods.FindIndex(x => x.PNodeId == (int)EqNode.SDP6);
            m_GlassData.cv_Nods[node_index].PRecipe = Convert.ToInt32(txt_SDP6Recipe.Text.Trim());

            node_index = m_GlassData.cv_Nods.FindIndex(x => x.PNodeId == (int)EqNode.IJP1);
            m_GlassData.cv_Nods[node_index].PRecipe = Convert.ToInt32(txt_IJP1Recipe.Text.Trim());

            node_index = m_GlassData.cv_Nods.FindIndex(x => x.PNodeId == (int)EqNode.IJP2);
            m_GlassData.cv_Nods[node_index].PRecipe = Convert.ToInt32(txt_IJP2Recipe.Text.Trim());

            node_index = m_GlassData.cv_Nods.FindIndex(x => x.PNodeId == (int)EqNode.AOI);
            m_GlassData.cv_Nods[node_index].PRecipe = Convert.ToInt32(txt_AOI1Recipe.Text.Trim());

            node_index = m_GlassData.cv_Nods.FindIndex(x => x.PNodeId == (int)EqNode.VAS1);
            m_GlassData.cv_Nods[node_index].PRecipe = Convert.ToInt32(txt_VAS1Recipe.Text.Trim());

            node_index = m_GlassData.cv_Nods.FindIndex(x => x.PNodeId == (int)EqNode.VAS2);
            m_GlassData.cv_Nods[node_index].PRecipe = Convert.ToInt32(txt_VAS2Recipe.Text.Trim());

            node_index = m_GlassData.cv_Nods.FindIndex(x => x.PNodeId == (int)EqNode.UV1);
            m_GlassData.cv_Nods[node_index].PRecipe = Convert.ToInt32(txt_UV1Recipe.Text.Trim());

            node_index = m_GlassData.cv_Nods.FindIndex(x => x.PNodeId == (int)EqNode.UV2);
            m_GlassData.cv_Nods[node_index].PRecipe = Convert.ToInt32(txt_UV2Recipe.Text.Trim());


            node_index = m_GlassData.cv_Nods.FindIndex(x => x.PNodeId == 2);
            //m_GlassData.cv_Nods[node_index].PRecipe = Convert.ToInt32(txt_AOIRecipe.Text.Trim());
            m_GlassData.cv_Nods[node_index].PRecipe = Convert.ToInt32(lgcBase.cv_Recipes.PCurRecipeId);

            m_GlassData.cv_Nods[2].PProcessHistory = 1;
        }
        private void Button_DefaultCstData_Click(object sender, EventArgs e)
        {
            TextBox_CstId.Text = UI.CommonStaticData.CtreatFoupSeq.ToString();
            TextBox_WorkOrder.Text = UI.CommonStaticData.CtreatWorkOrder.ToString();


            if (UiForm.GetPort(cv_PortId).cv_Data.PProductionType == ProductCategory.Glass)
            {
                cb_Substrate.Text = "Glass";
            }
            else if (UiForm.GetPort(cv_PortId).cv_Data.PProductionType == ProductCategory.Wafer)
            {
                cb_Substrate.Text = "Wafer";
            }

            if (string.IsNullOrEmpty(TextBox_LotId.Text.Trim()))
            {
                TextBox_LotId.Text = "P" + cv_PortId.ToString() + TextBox_CstId.Text;
            }
            for (int i = 0; i < panOfflineSlot.Controls.Count; i++)
            {
                SlotItem slot_obj = panOfflineSlot.Controls[i] as SlotItem;
                if (slot_obj.GlassId.Enabled)
                {
                    slot_obj.GlassId.Text = "Substate" + slot_obj.SlotNo.Text;
                    slot_obj.ProcessFlag.Checked = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < panOfflineSlot.Controls.Count; i++)
            {
                SlotItem slot_obj = panOfflineSlot.Controls[i] as SlotItem;
                if (slot_obj.GlassId.Enabled)
                {
                    slot_obj.ProcessFlag.Checked = false;
                }
            }
        }
    }
}

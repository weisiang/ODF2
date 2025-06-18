using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using BaseAp;

namespace UI
{
    public partial class RecipeCheckForm : Form
    {
        public RecipeCheckForm()
        {
            InitializeComponent();
        }

        private void RecipeCheckForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        public void SetNodeChecked()
        {
            if(UiForm.PSystemData.IsCheckId)
            {
                if (btn_id.BackColor != Color.Green)
                    btn_id.BackColor = Color.Green;
            }
            else
            {
                if (btn_id.BackColor != Color.Gray)
                    btn_id.BackColor = Color.Gray;
            }
            if (UiForm.PSystemData.IsCheckRecipe)
            {
                if (btn_recipe.BackColor != Color.Green)
                    btn_recipe.BackColor = Color.Green;
            }
            else
            {
                if (btn_recipe.BackColor != Color.Gray)
                    btn_recipe.BackColor = Color.Gray;
            }
            if (UiForm.PSystemData.IsCheckSeq)
            {
                if (btn_seq.BackColor != Color.Green)
                    btn_seq.BackColor = Color.Green;
            }
            else
            {
                if (btn_seq.BackColor != Color.Gray)
                    btn_seq.BackColor = Color.Gray;
            }
            if (UiForm.PSystemData.IsCheckSlot)
            {
                if (btn_slot.BackColor != Color.Green)
                    btn_slot.BackColor = Color.Green;
            }
            else
            {
                if (btn_slot.BackColor != Color.Gray)
                    btn_slot.BackColor = Color.Gray;
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            CommonData.HIRATA.MDGlassDataCheck obj = new CommonData.HIRATA.MDGlassDataCheck();
            object tmp = null;
            uint ticket;
            string rtn;
            string log = "";
            log += "Send : " + typeof(CommonData.HIRATA.MDGlassDataCheck).Name + Environment.NewLine;
            obj.PType = CommonData.HIRATA.MmfEventClientEventType.etRequest;
            if(Regex.Match(btn.Name , @"id" , RegexOptions.IgnoreCase).Success)
            {
                obj.PCheckRecipeNo = UiForm.PSystemData.IsCheckRecipe;
                obj.PCheckWorkId = !UiForm.PSystemData.IsCheckId;
                obj.PCheckFoupSeq = UiForm.PSystemData.IsCheckSeq;
                obj.PCheckWorkSlot = UiForm.PSystemData.IsCheckSlot;
            }
            else if(Regex.Match(btn.Name , @"recipe" , RegexOptions.IgnoreCase).Success)
            {
                obj.PCheckRecipeNo = !UiForm.PSystemData.IsCheckRecipe;
                obj.PCheckWorkId = UiForm.PSystemData.IsCheckId;
                obj.PCheckFoupSeq = UiForm.PSystemData.IsCheckSeq;
                obj.PCheckWorkSlot = UiForm.PSystemData.IsCheckSlot;
            }
            else if(Regex.Match(btn.Name , @"slot" , RegexOptions.IgnoreCase).Success)
            {
                obj.PCheckRecipeNo = UiForm.PSystemData.IsCheckRecipe;
                obj.PCheckWorkId = UiForm.PSystemData.IsCheckId;
                obj.PCheckFoupSeq = UiForm.PSystemData.IsCheckSeq;
                obj.PCheckWorkSlot = !UiForm.PSystemData.IsCheckSlot;
            }
            else if(Regex.Match(btn.Name , @"seq" , RegexOptions.IgnoreCase).Success)
            {
                obj.PCheckRecipeNo = UiForm.PSystemData.IsCheckRecipe;
                obj.PCheckWorkId = UiForm.PSystemData.IsCheckId;
                obj.PCheckFoupSeq = !UiForm.PSystemData.IsCheckSeq;
                obj.PCheckWorkSlot = UiForm.PSystemData.IsCheckSlot;
            }
            if (checkData(obj))
            {
                UiForm.WriteLog(CommonData.HIRATA.LogLevelType.General, "Send MDGlassDataCheck");
                /*
                if (!UiForm.cv_mmfController.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDGlassDataCheck).Name, obj, out ticket, out rtn, out tmp, 3000))
                {
                    log += "[Time Out]Wait : " + typeof(CommonData.HIRATA.MDGlassDataCheck).Name;
                }
                */
            }
        }
        private bool checkData( CommonData.HIRATA.MDGlassDataCheck data)
        {
            bool rtn = true;
            if(!data.PCheckFoupSeq && !data.PCheckRecipeNo && !data.PCheckWorkId && !data.PCheckWorkSlot)
            {
                CommonStaticData.PopForm("Select one option at least", true, false);
                rtn = false;
            }
            return rtn;
        }
    }
}
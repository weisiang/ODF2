using System;
using CommonData.HIRATA;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KgsCommon;
using System.Text.RegularExpressions;

namespace UI.GUI
{
    public partial class AlignerUI : UserControl, AlignerReflash
    {
        int cv_SlotCount = 0;
        int cv_Id = 0;
        Match match = Match.Empty;
        public AlignerUI(int m_Id, int m_SlotCount)
        {
            InitializeComponent();
            cv_SlotCount = m_SlotCount;
            cv_Id = m_Id;
            this.gb_Buffer.Text = "Aligner " + Convert.ToString(m_Id);
            cv_glassDataView.Rows.Clear();
            cv_glassDataView.Rows.Add(m_SlotCount);
            cv_glassDataView.AutoGenerateColumns = false;
            cv_glassDataView.RowHeadersVisible = false;
            cv_glassDataView.Rows[0].Cells[0].Value = "";
            cv_glassDataView.Rows[0].Cells[1].Value = "WWWWWWWWWWW";

            for (int i = 0; i < m_SlotCount; i++)
            {
                cv_glassDataView.Rows[i].Cells[0].Value = (i + 1).ToString();
                ToolStripMenuItem tmp = new ToolStripMenuItem();
                tmp.Text = (i + 1).ToString();
                tmp.Click += dELETEToolStripMenuItem_Click;
                (cv_menuDataEdit.Items[0] as ToolStripMenuItem).DropDownItems.Add(tmp);
            }
        }
        public void refresh(AlignerData m_DataMap)
        {
            int slot = 0;
            if (m_DataMap != null)
            {
                for (int i = 0; i < cv_SlotCount ; i++)
                {
                    match = (Regex.Match(cv_glassDataView.Rows[i].Cells[0].Value.ToString(), @"[0-9]", RegexOptions.IgnoreCase));
                    if (match.Success)
                    {
                        slot = Convert.ToInt32(match.Value);
                    }
                    else
                    {
                        continue;
                    }

                    GlassData glass_data = null;
                    bool has_sensor = false;
                    if (m_DataMap.GetSlotData(slot, ref glass_data, ref has_sensor))
                    {
                        if (glass_data != null)
                        {
                            if(glass_data.PHasData)
                            cv_glassDataView.Rows[i].Cells[1].Value = glass_data.PId;
                            else
                            cv_glassDataView.Rows[i].Cells[1].Value = "";
                        }
                        else
                        {
                            cv_glassDataView.Rows[i].Cells[1].Value = "";
                        }
                        CommonStaticData.ChangeDataViewItemColor(ref cv_glassDataView, i, glass_data);
                    }
                    else
                    {
                        UiForm.WriteLog( LogLevelType.Error , "Buffer UI refresh ERROR. Slot Un-match");
                    }
                }
            }
            cv_glassDataView.ClearSelection();
        }

        private void cv_glassDataView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            cv_glassDataView.Rows[e.RowIndex].Selected = false;
        }
        private void dELETEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItem tmp = sender as ToolStripItem;
            int slot = Convert.ToInt32(tmp.Text.Trim());
            UiForm.cv_GlassDataForm.Register(ActionTarget.Aligner, cv_Id, slot);
            UiForm.cv_GlassDataForm.Show();
        }

    }
}

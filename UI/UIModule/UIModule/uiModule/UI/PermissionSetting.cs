using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class PermissionSetting : UserControl
    {
        public PermissionSetting()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();
            dataGridView1.Rows[0].Cells[0].Value = "Root";
            dataGridView1.Rows[1].Cells[0].Value = "Engineer";
            dataGridView1.Rows[2].Cells[0].Value = "OP3";
            dataGridView1.Rows[3].Cells[0].Value = "OP2";
            dataGridView1.Rows[4].Cells[0].Value = "OP1";
        }
        private void SetAllCellsToNotChecked()
        {
            int row_count = dataGridView1.RowCount;
            for(int i=0 ; i< row_count ;i++)
            {
                for (int j = 1; j <= 6; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = "F";
                }
            }
        }
        public void LoadFromSetting()
        {
            SetAllCellsToNotChecked();
            for(int i=0 ; i< UiForm.cv_PermissionGroups[CommonData.HIRATA.UserPermission.Root].Count ; i++)
            {
                dataGridView1.Rows[0].Cells[UiForm.cv_PermissionGroups[CommonData.HIRATA.UserPermission.Root][i]].Value = "T";
            }
            for (int i = 0; i < UiForm.cv_PermissionGroups[CommonData.HIRATA.UserPermission.Engineer].Count; i++)
            {
                dataGridView1.Rows[1].Cells[UiForm.cv_PermissionGroups[CommonData.HIRATA.UserPermission.Engineer][i]].Value = "T";
            }
            for (int i = 0; i < UiForm.cv_PermissionGroups[CommonData.HIRATA.UserPermission.OP3].Count; i++)
            {
                dataGridView1.Rows[2].Cells[UiForm.cv_PermissionGroups[CommonData.HIRATA.UserPermission.OP3][i]].Value = "T";
            }
            for (int i = 0; i < UiForm.cv_PermissionGroups[CommonData.HIRATA.UserPermission.OP2].Count; i++)
            {
                dataGridView1.Rows[3].Cells[UiForm.cv_PermissionGroups[CommonData.HIRATA.UserPermission.OP2][i]].Value = "T";
            }
            for (int i = 0; i < UiForm.cv_PermissionGroups[CommonData.HIRATA.UserPermission.OP1].Count; i++)
            {
                dataGridView1.Rows[4].Cells[UiForm.cv_PermissionGroups[CommonData.HIRATA.UserPermission.OP1][i]].Value = "T";
            }
        }
    }
}

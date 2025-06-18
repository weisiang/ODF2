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
    public partial class LotSummaryUI : UserControl
    {
        public LotSummaryUI(BindingList<CommonData.HIRATA.PortData> m_list)
        {
            InitializeComponent();
            //SetLotSummaryGrid();
            dataGrid_LotSummary.AutoGenerateColumns = false;
            GridBinding(m_list);
        }
        internal void GridBinding(BindingList<CommonData.HIRATA.PortData> m_list)
        {
            dataGrid_LotSummary.DataSource = m_list;
        }
        public void reFresh()
        {
            dataGrid_LotSummary.Refresh();
        }
        private void SetLotSummaryGrid()
        {
            DataGridViewTextBoxColumn IdColumn = new DataGridViewTextBoxColumn();
            IdColumn.DataPropertyName = "Id";
            IdColumn.HeaderText = "Id";
            IdColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            IdColumn.ReadOnly = true;
            dataGrid_LotSummary.Columns.Add(IdColumn);

            DataGridViewTextBoxColumn AgvColumn = new DataGridViewTextBoxColumn();
            AgvColumn.DataPropertyName = "PortAgv";
            AgvColumn.HeaderText = "AGV";
            AgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            AgvColumn.ReadOnly = true;
            dataGrid_LotSummary.Columns.Add(AgvColumn);

            DataGridViewTextBoxColumn PortEnableColumn = new DataGridViewTextBoxColumn();
            PortEnableColumn.DataPropertyName = "PortEnable";
            PortEnableColumn.HeaderText = "Enable";
            PortEnableColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            PortEnableColumn.ReadOnly = true;
            dataGrid_LotSummary.Columns.Add(PortEnableColumn);

            DataGridViewTextBoxColumn PortTypeColumn = new DataGridViewTextBoxColumn();
            PortTypeColumn.DataPropertyName = "PortType";
            PortTypeColumn.HeaderText = "Type";
            PortTypeColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            PortTypeColumn.ReadOnly = true;
            dataGrid_LotSummary.Columns.Add(PortTypeColumn);

            DataGridViewTextBoxColumn PortModeColumn = new DataGridViewTextBoxColumn();
            PortModeColumn.DataPropertyName = "PortMode";
            PortModeColumn.HeaderText = "PortMode";
            PortModeColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            PortModeColumn.ReadOnly = true;
            dataGrid_LotSummary.Columns.Add(PortModeColumn);

            DataGridViewTextBoxColumn LortStatusColumn = new DataGridViewTextBoxColumn();
            LortStatusColumn.DataPropertyName = "LortStatus";
            LortStatusColumn.HeaderText = "LortStatus";
            LortStatusColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            LortStatusColumn.ReadOnly = true;
            dataGrid_LotSummary.Columns.Add(LortStatusColumn);

            DataGridViewTextBoxColumn PortStatusColumn = new DataGridViewTextBoxColumn();
            PortStatusColumn.DataPropertyName = "PortStatus";
            PortStatusColumn.HeaderText = "PortStatus";
            PortStatusColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            PortStatusColumn.ReadOnly = true;
            dataGrid_LotSummary.Columns.Add(PortStatusColumn);
        }
    }
}

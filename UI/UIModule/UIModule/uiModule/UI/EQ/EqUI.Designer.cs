namespace UI.GUI
{
    partial class EqUI
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();                
            }
            
            base.Dispose(disposing); 
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cv_menuDataEdit = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dELETEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cv_GBName = new System.Windows.Forms.GroupBox();
            this.cv_warning = new System.Windows.Forms.Label();
            this.cv_stabdby = new System.Windows.Forms.Label();
            this.cv_init = new System.Windows.Forms.Label();
            this.cv_stop = new System.Windows.Forms.Label();
            this.cv_inlineMode = new System.Windows.Forms.Label();
            this.cv_mainStatus = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.cv_menuDataEdit.SuspendLayout();
            this.cv_GBName.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Controls.Add(this.cv_GBName);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(217, 249);
            this.panel2.TabIndex = 4;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 129);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(217, 120);
            this.dataGridView1.TabIndex = 7;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.ContextMenuStrip = this.cv_menuDataEdit;
            this.Column1.HeaderText = "SLOT";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // cv_menuDataEdit
            // 
            this.cv_menuDataEdit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dELETEToolStripMenuItem});
            this.cv_menuDataEdit.Name = "cv_menuDataEdit";
            this.cv_menuDataEdit.Size = new System.Drawing.Size(153, 48);
            // 
            // dELETEToolStripMenuItem
            // 
            this.dELETEToolStripMenuItem.Name = "dELETEToolStripMenuItem";
            this.dELETEToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.dELETEToolStripMenuItem.Text = "DataAction";
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.HeaderText = "Id";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // cv_GBName
            // 
            this.cv_GBName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cv_GBName.Controls.Add(this.cv_warning);
            this.cv_GBName.Controls.Add(this.cv_stabdby);
            this.cv_GBName.Controls.Add(this.cv_init);
            this.cv_GBName.Controls.Add(this.cv_stop);
            this.cv_GBName.Controls.Add(this.cv_inlineMode);
            this.cv_GBName.Controls.Add(this.cv_mainStatus);
            this.cv_GBName.Dock = System.Windows.Forms.DockStyle.Top;
            this.cv_GBName.Location = new System.Drawing.Point(0, 0);
            this.cv_GBName.Name = "cv_GBName";
            this.cv_GBName.Size = new System.Drawing.Size(217, 129);
            this.cv_GBName.TabIndex = 6;
            this.cv_GBName.TabStop = false;
            this.cv_GBName.Text = "groupBox1";
            // 
            // cv_warning
            // 
            this.cv_warning.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.cv_warning.Dock = System.Windows.Forms.DockStyle.Right;
            this.cv_warning.Location = new System.Drawing.Point(138, 84);
            this.cv_warning.Name = "cv_warning";
            this.cv_warning.Size = new System.Drawing.Size(38, 42);
            this.cv_warning.TabIndex = 23;
            this.cv_warning.Text = "warnning";
            this.cv_warning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cv_stabdby
            // 
            this.cv_stabdby.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.cv_stabdby.Dock = System.Windows.Forms.DockStyle.Right;
            this.cv_stabdby.Location = new System.Drawing.Point(176, 84);
            this.cv_stabdby.Name = "cv_stabdby";
            this.cv_stabdby.Size = new System.Drawing.Size(38, 42);
            this.cv_stabdby.TabIndex = 22;
            this.cv_stabdby.Text = "standby";
            this.cv_stabdby.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cv_init
            // 
            this.cv_init.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.cv_init.Dock = System.Windows.Forms.DockStyle.Left;
            this.cv_init.Location = new System.Drawing.Point(41, 84);
            this.cv_init.Name = "cv_init";
            this.cv_init.Size = new System.Drawing.Size(38, 42);
            this.cv_init.TabIndex = 21;
            this.cv_init.Text = "init";
            this.cv_init.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cv_stop
            // 
            this.cv_stop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cv_stop.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.cv_stop.Dock = System.Windows.Forms.DockStyle.Left;
            this.cv_stop.Location = new System.Drawing.Point(3, 84);
            this.cv_stop.Name = "cv_stop";
            this.cv_stop.Size = new System.Drawing.Size(38, 42);
            this.cv_stop.TabIndex = 20;
            this.cv_stop.Text = "stop";
            this.cv_stop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cv_inlineMode
            // 
            this.cv_inlineMode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cv_inlineMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.cv_inlineMode.Location = new System.Drawing.Point(3, 51);
            this.cv_inlineMode.Name = "cv_inlineMode";
            this.cv_inlineMode.Size = new System.Drawing.Size(211, 33);
            this.cv_inlineMode.TabIndex = 3;
            this.cv_inlineMode.Text = "Mode";
            this.cv_inlineMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cv_mainStatus
            // 
            this.cv_mainStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cv_mainStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.cv_mainStatus.Location = new System.Drawing.Point(3, 18);
            this.cv_mainStatus.Name = "cv_mainStatus";
            this.cv_mainStatus.Size = new System.Drawing.Size(211, 33);
            this.cv_mainStatus.TabIndex = 4;
            this.cv_mainStatus.Text = "Main Status";
            this.cv_mainStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EqUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Name = "EqUI";
            this.Size = new System.Drawing.Size(217, 249);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.cv_menuDataEdit.ResumeLayout(false);
            this.cv_GBName.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ContextMenuStrip cv_menuDataEdit;
        private System.Windows.Forms.ToolStripMenuItem dELETEToolStripMenuItem;
        private System.Windows.Forms.GroupBox cv_GBName;
        private System.Windows.Forms.Label cv_warning;
        private System.Windows.Forms.Label cv_stabdby;
        private System.Windows.Forms.Label cv_init;
        private System.Windows.Forms.Label cv_stop;
        //private WindowsFormsApplication1.UI.ModuleLabel moduleLabel2;
        //private WindowsFormsApplication1.UI.ModuleLabel moduleLabel1;
        private System.Windows.Forms.Label cv_inlineMode;
        private System.Windows.Forms.Label cv_mainStatus;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
    }
}

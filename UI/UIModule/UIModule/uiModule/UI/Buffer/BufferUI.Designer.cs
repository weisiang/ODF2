namespace UI.GUI
{
    partial class BufferUI
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gb_Buffer = new System.Windows.Forms.GroupBox();
            this.cv_glassDataView = new System.Windows.Forms.DataGridView();
            this.cv_menuDataEdit = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dELETEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gb_Buffer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cv_glassDataView)).BeginInit();
            this.cv_menuDataEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // gb_Buffer
            // 
            this.gb_Buffer.Controls.Add(this.cv_glassDataView);
            this.gb_Buffer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb_Buffer.Location = new System.Drawing.Point(0, 0);
            this.gb_Buffer.Name = "gb_Buffer";
            this.gb_Buffer.Size = new System.Drawing.Size(190, 357);
            this.gb_Buffer.TabIndex = 0;
            this.gb_Buffer.TabStop = false;
            this.gb_Buffer.Text = "groupBox1";
            // 
            // cv_glassDataView
            // 
            this.cv_glassDataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cv_glassDataView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.cv_glassDataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cv_glassDataView.Location = new System.Drawing.Point(3, 18);
            this.cv_glassDataView.Name = "cv_glassDataView";
            this.cv_glassDataView.RowTemplate.Height = 16;
            this.cv_glassDataView.Size = new System.Drawing.Size(184, 336);
            this.cv_glassDataView.TabIndex = 0;
            this.cv_glassDataView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.cv_glassDataView_CellClick);
            // 
            // cv_menuDataEdit
            // 
            this.cv_menuDataEdit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dELETEToolStripMenuItem});
            this.cv_menuDataEdit.Name = "cv_menuDataEdit";
            this.cv_menuDataEdit.Size = new System.Drawing.Size(138, 26);
            // 
            // dELETEToolStripMenuItem
            // 
            this.dELETEToolStripMenuItem.Name = "dELETEToolStripMenuItem";
            this.dELETEToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.dELETEToolStripMenuItem.Text = "DataAction";
            // 
            // Column1
            // 
            this.Column1.ContextMenuStrip = this.cv_menuDataEdit;
            this.Column1.DataPropertyName = "Slot";
            this.Column1.HeaderText = "Slot";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 40;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "Id";
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Times New Roman", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Column2.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column2.HeaderText = "Id";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // BufferUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gb_Buffer);
            this.Name = "BufferUI";
            this.Size = new System.Drawing.Size(190, 357);
            this.gb_Buffer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cv_glassDataView)).EndInit();
            this.cv_menuDataEdit.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_Buffer;
        private System.Windows.Forms.DataGridView cv_glassDataView;
        private System.Windows.Forms.ContextMenuStrip cv_menuDataEdit;
        private System.Windows.Forms.ToolStripMenuItem dELETEToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
    }
}

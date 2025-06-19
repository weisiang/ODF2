namespace UI.GUI
{
    partial class PortUI
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gb_port = new System.Windows.Forms.GroupBox();
            this.cv_glassDataView = new System.Windows.Forms.DataGridView();
            this.cv_menuDataEdit = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dELETEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lbl_LotId = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cv_25slot = new System.Windows.Forms.Button();
            this.cv_13slot = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gb_port.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cv_glassDataView)).BeginInit();
            this.cv_menuDataEdit.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gb_port
            // 
            this.gb_port.Controls.Add(this.cv_glassDataView);
            this.gb_port.Controls.Add(this.panel1);
            this.gb_port.Location = new System.Drawing.Point(0, 0);
            this.gb_port.Name = "gb_port";
            this.gb_port.Size = new System.Drawing.Size(151, 680);
            this.gb_port.TabIndex = 1;
            this.gb_port.TabStop = false;
            this.gb_port.Text = "PORT ";
            // 
            // cv_glassDataView
            // 
            this.cv_glassDataView.AllowUserToAddRows = false;
            this.cv_glassDataView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.cv_glassDataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cv_glassDataView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.cv_glassDataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cv_glassDataView.Location = new System.Drawing.Point(3, 80);
            this.cv_glassDataView.MultiSelect = false;
            this.cv_glassDataView.Name = "cv_glassDataView";
            this.cv_glassDataView.ReadOnly = true;
            this.cv_glassDataView.RowHeadersVisible = false;
            this.cv_glassDataView.RowHeadersWidth = 30;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            this.cv_glassDataView.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.cv_glassDataView.RowTemplate.Height = 10;
            this.cv_glassDataView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.cv_glassDataView.Size = new System.Drawing.Size(145, 597);
            this.cv_glassDataView.TabIndex = 5;
            // 
            // cv_menuDataEdit
            // 
            this.cv_menuDataEdit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dELETEToolStripMenuItem});
            this.cv_menuDataEdit.Name = "cv_menuDataEdit";
            this.cv_menuDataEdit.Size = new System.Drawing.Size(138, 26);
            this.cv_menuDataEdit.Text = "1111";
            // 
            // dELETEToolStripMenuItem
            // 
            this.dELETEToolStripMenuItem.Name = "dELETEToolStripMenuItem";
            this.dELETEToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.dELETEToolStripMenuItem.Text = "DataAction";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(145, 62);
            this.panel1.TabIndex = 4;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.panel4.Controls.Add(this.lbl_LotId);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 39);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(145, 27);
            this.panel4.TabIndex = 2;
            // 
            // lbl_LotId
            // 
            this.lbl_LotId.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbl_LotId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_LotId.Location = new System.Drawing.Point(0, 0);
            this.lbl_LotId.Name = "lbl_LotId";
            this.lbl_LotId.Size = new System.Drawing.Size(145, 27);
            this.lbl_LotId.TabIndex = 3;
            this.lbl_LotId.Text = "Lot Id";
            this.lbl_LotId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.panel2.Controls.Add(this.cv_25slot);
            this.panel2.Controls.Add(this.cv_13slot);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(145, 39);
            this.panel2.TabIndex = 0;
            // 
            // cv_25slot
            // 
            this.cv_25slot.Dock = System.Windows.Forms.DockStyle.Left;
            this.cv_25slot.Location = new System.Drawing.Point(93, 0);
            this.cv_25slot.Name = "cv_25slot";
            this.cv_25slot.Size = new System.Drawing.Size(50, 39);
            this.cv_25slot.TabIndex = 6;
            this.cv_25slot.Text = "25";
            this.cv_25slot.UseVisualStyleBackColor = true;
            this.cv_25slot.Click += new System.EventHandler(this.button3_Click);
            // 
            // cv_13slot
            // 
            this.cv_13slot.Dock = System.Windows.Forms.DockStyle.Left;
            this.cv_13slot.Location = new System.Drawing.Point(48, 0);
            this.cv_13slot.Name = "cv_13slot";
            this.cv_13slot.Size = new System.Drawing.Size(45, 39);
            this.cv_13slot.TabIndex = 5;
            this.cv_13slot.Text = "13";
            this.cv_13slot.UseVisualStyleBackColor = true;
            this.cv_13slot.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Left;
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(48, 39);
            this.button1.TabIndex = 4;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Column1
            // 
            this.Column1.ContextMenuStrip = this.cv_menuDataEdit;
            this.Column1.DataPropertyName = "Slot";
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Times New Roman", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Column1.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column1.HeaderText = "SLOT";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 40;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "Id";
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Times New Roman", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Column2.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column2.HeaderText = "ID";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // PortUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gb_port);
            this.Name = "PortUI";
            this.Size = new System.Drawing.Size(154, 744);
            this.gb_port.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cv_glassDataView)).EndInit();
            this.cv_menuDataEdit.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gb_port;
        private System.Windows.Forms.ContextMenuStrip cv_menuDataEdit;
        private System.Windows.Forms.ToolStripMenuItem dELETEToolStripMenuItem;
        private System.Windows.Forms.DataGridView cv_glassDataView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lbl_LotId;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button cv_25slot;
        private System.Windows.Forms.Button cv_13slot;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
    }
}

namespace UI
{
    partial class KLogPage
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
            this.tabMemoryLogs = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // tabMemoryLogs
            // 
            this.tabMemoryLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMemoryLogs.Location = new System.Drawing.Point(0, 0);
            this.tabMemoryLogs.Name = "tabMemoryLogs";
            this.tabMemoryLogs.SelectedIndex = 0;
            this.tabMemoryLogs.Size = new System.Drawing.Size(976, 459);
            this.tabMemoryLogs.TabIndex = 1;
            // 
            // KLogPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabMemoryLogs);
            this.Name = "KLogPage";
            this.Size = new System.Drawing.Size(976, 459);
            this.SizeChanged += new System.EventHandler(this.KLogPage_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabMemoryLogs;
    }
}

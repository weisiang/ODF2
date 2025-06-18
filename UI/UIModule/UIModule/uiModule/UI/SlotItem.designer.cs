namespace UI
{
    partial class SlotItem
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SlotNo = new System.Windows.Forms.Label();
            this.ProcessFlag = new System.Windows.Forms.CheckBox();
            this.Button_Copy = new System.Windows.Forms.Button();
            this.GlassId = new System.Windows.Forms.TextBox();
            this.cb_priority = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // SlotNo
            // 
            this.SlotNo.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.SlotNo.Dock = System.Windows.Forms.DockStyle.Left;
            this.SlotNo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SlotNo.Location = new System.Drawing.Point(0, 0);
            this.SlotNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SlotNo.Name = "SlotNo";
            this.SlotNo.Size = new System.Drawing.Size(30, 23);
            this.SlotNo.TabIndex = 0;
            this.SlotNo.Text = "00";
            this.SlotNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SlotNo.Click += new System.EventHandler(this.SlotNo_Click);
            // 
            // ProcessFlag
            // 
            this.ProcessFlag.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ProcessFlag.Dock = System.Windows.Forms.DockStyle.Left;
            this.ProcessFlag.Location = new System.Drawing.Point(30, 0);
            this.ProcessFlag.Name = "ProcessFlag";
            this.ProcessFlag.Size = new System.Drawing.Size(30, 23);
            this.ProcessFlag.TabIndex = 1;
            this.ProcessFlag.UseVisualStyleBackColor = true;
            // 
            // Button_Copy
            // 
            this.Button_Copy.AutoSize = true;
            this.Button_Copy.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_Copy.Location = new System.Drawing.Point(259, -2);
            this.Button_Copy.Margin = new System.Windows.Forms.Padding(0);
            this.Button_Copy.Name = "Button_Copy";
            this.Button_Copy.Size = new System.Drawing.Size(50, 24);
            this.Button_Copy.TabIndex = 4;
            this.Button_Copy.Text = "Copy";
            this.Button_Copy.UseVisualStyleBackColor = true;
            // 
            // GlassId
            // 
            this.GlassId.Dock = System.Windows.Forms.DockStyle.Left;
            this.GlassId.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GlassId.Location = new System.Drawing.Point(60, 0);
            this.GlassId.Name = "GlassId";
            this.GlassId.Size = new System.Drawing.Size(120, 24);
            this.GlassId.TabIndex = 5;
            // 
            // cb_priority
            // 
            this.cb_priority.FormattingEnabled = true;
            this.cb_priority.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.cb_priority.Location = new System.Drawing.Point(186, -2);
            this.cb_priority.Name = "cb_priority";
            this.cb_priority.Size = new System.Drawing.Size(60, 28);
            this.cb_priority.TabIndex = 6;
            // 
            // SlotItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.Button_Copy);
            this.Controls.Add(this.cb_priority);
            this.Controls.Add(this.GlassId);
            this.Controls.Add(this.ProcessFlag);
            this.Controls.Add(this.SlotNo);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SlotItem";
            this.Size = new System.Drawing.Size(312, 23);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label SlotNo;
        public System.Windows.Forms.CheckBox ProcessFlag;
        public System.Windows.Forms.Button Button_Copy;
        public System.Windows.Forms.TextBox GlassId;
        public System.Windows.Forms.ComboBox cb_priority;
    }
}

namespace UI
{
    partial class IfMonitor
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
            this.cv_GbUpStream = new System.Windows.Forms.GroupBox();
            this.cv_btnReset = new System.Windows.Forms.Button();
            this.lbl_Step = new System.Windows.Forms.Label();
            this.cv_EqNameGroup = new System.Windows.Forms.ComboBox();
            this.cv_BtUpForceIni = new System.Windows.Forms.Button();
            this.cv_BtUpForceCom = new System.Windows.Forms.Button();
            this.cv_RobotGroup = new System.Windows.Forms.GroupBox();
            this.cv_EqGroup = new System.Windows.Forms.GroupBox();
            this.lbl_StepName = new System.Windows.Forms.Label();
            this.cv_GbUpStream.SuspendLayout();
            this.SuspendLayout();
            // 
            // cv_GbUpStream
            // 
            this.cv_GbUpStream.Controls.Add(this.lbl_StepName);
            this.cv_GbUpStream.Controls.Add(this.cv_btnReset);
            this.cv_GbUpStream.Controls.Add(this.lbl_Step);
            this.cv_GbUpStream.Controls.Add(this.cv_EqNameGroup);
            this.cv_GbUpStream.Controls.Add(this.cv_BtUpForceIni);
            this.cv_GbUpStream.Controls.Add(this.cv_BtUpForceCom);
            this.cv_GbUpStream.Controls.Add(this.cv_RobotGroup);
            this.cv_GbUpStream.Controls.Add(this.cv_EqGroup);
            this.cv_GbUpStream.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cv_GbUpStream.Location = new System.Drawing.Point(0, 0);
            this.cv_GbUpStream.Name = "cv_GbUpStream";
            this.cv_GbUpStream.Size = new System.Drawing.Size(328, 576);
            this.cv_GbUpStream.TabIndex = 1;
            this.cv_GbUpStream.TabStop = false;
            this.cv_GbUpStream.Text = "Monitor";
            // 
            // cv_btnReset
            // 
            this.cv_btnReset.Location = new System.Drawing.Point(16, 528);
            this.cv_btnReset.Name = "cv_btnReset";
            this.cv_btnReset.Size = new System.Drawing.Size(130, 23);
            this.cv_btnReset.TabIndex = 44;
            this.cv_btnReset.Text = "Reset";
            this.cv_btnReset.UseVisualStyleBackColor = true;
            this.cv_btnReset.Click += new System.EventHandler(this.cv_btnReset_Click);
            // 
            // lbl_Step
            // 
            this.lbl_Step.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lbl_Step.Location = new System.Drawing.Point(169, 499);
            this.lbl_Step.Name = "lbl_Step";
            this.lbl_Step.Size = new System.Drawing.Size(100, 23);
            this.lbl_Step.TabIndex = 43;
            this.lbl_Step.Text = "Step_id";
            this.lbl_Step.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cv_EqNameGroup
            // 
            this.cv_EqNameGroup.FormattingEnabled = true;
            this.cv_EqNameGroup.Location = new System.Drawing.Point(171, 470);
            this.cv_EqNameGroup.Name = "cv_EqNameGroup";
            this.cv_EqNameGroup.Size = new System.Drawing.Size(116, 20);
            this.cv_EqNameGroup.TabIndex = 42;
            this.cv_EqNameGroup.SelectedIndexChanged += new System.EventHandler(this.cv_EqNameGroup_SelectedIndexChanged);
            // 
            // cv_BtUpForceIni
            // 
            this.cv_BtUpForceIni.Location = new System.Drawing.Point(16, 499);
            this.cv_BtUpForceIni.Name = "cv_BtUpForceIni";
            this.cv_BtUpForceIni.Size = new System.Drawing.Size(130, 23);
            this.cv_BtUpForceIni.TabIndex = 41;
            this.cv_BtUpForceIni.Text = "Force Initail";
            this.cv_BtUpForceIni.UseVisualStyleBackColor = true;
            this.cv_BtUpForceIni.Click += new System.EventHandler(this.cv_BtUpForceIni_Click);
            // 
            // cv_BtUpForceCom
            // 
            this.cv_BtUpForceCom.Location = new System.Drawing.Point(16, 470);
            this.cv_BtUpForceCom.Name = "cv_BtUpForceCom";
            this.cv_BtUpForceCom.Size = new System.Drawing.Size(130, 23);
            this.cv_BtUpForceCom.TabIndex = 40;
            this.cv_BtUpForceCom.Text = "Force Complete";
            this.cv_BtUpForceCom.UseVisualStyleBackColor = true;
            this.cv_BtUpForceCom.Click += new System.EventHandler(this.cv_BtUpForceCom_Click);
            // 
            // cv_RobotGroup
            // 
            this.cv_RobotGroup.Location = new System.Drawing.Point(6, 21);
            this.cv_RobotGroup.Name = "cv_RobotGroup";
            this.cv_RobotGroup.Size = new System.Drawing.Size(155, 443);
            this.cv_RobotGroup.TabIndex = 37;
            this.cv_RobotGroup.TabStop = false;
            this.cv_RobotGroup.Text = "Robot";
            // 
            // cv_EqGroup
            // 
            this.cv_EqGroup.Location = new System.Drawing.Point(167, 21);
            this.cv_EqGroup.Name = "cv_EqGroup";
            this.cv_EqGroup.Size = new System.Drawing.Size(155, 443);
            this.cv_EqGroup.TabIndex = 37;
            this.cv_EqGroup.TabStop = false;
            this.cv_EqGroup.Text = "EQ";
            // 
            // lbl_StepName
            // 
            this.lbl_StepName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lbl_StepName.Location = new System.Drawing.Point(169, 528);
            this.lbl_StepName.Name = "lbl_StepName";
            this.lbl_StepName.Size = new System.Drawing.Size(100, 23);
            this.lbl_StepName.TabIndex = 45;
            this.lbl_StepName.Text = "Step_name";
            this.lbl_StepName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IfMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cv_GbUpStream);
            this.Name = "IfMonitor";
            this.Size = new System.Drawing.Size(328, 576);
            this.cv_GbUpStream.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox cv_GbUpStream;
        private System.Windows.Forms.GroupBox cv_RobotGroup;
        private System.Windows.Forms.GroupBox cv_EqGroup;
        private System.Windows.Forms.Button cv_BtUpForceIni;
        private System.Windows.Forms.Button cv_BtUpForceCom;
        private System.Windows.Forms.ComboBox cv_EqNameGroup;
        private System.Windows.Forms.Label lbl_Step;
        private System.Windows.Forms.Button cv_btnReset;
        private System.Windows.Forms.Label lbl_StepName;
    }
}

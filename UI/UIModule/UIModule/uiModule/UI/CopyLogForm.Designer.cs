namespace UI
{
    partial class CopyLogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cv_DatePickStart = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cv_DatePickEnd = new System.Windows.Forms.DateTimePicker();
            this.cv_BtCopy = new System.Windows.Forms.Button();
            this.cv_BtTargetFolder = new System.Windows.Forms.Button();
            this.cv_TxTargetFolder = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cv_DatePickStart
            // 
            this.cv_DatePickStart.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.cv_DatePickStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.cv_DatePickStart.Location = new System.Drawing.Point(162, 48);
            this.cv_DatePickStart.MaxDate = new System.DateTime(2030, 12, 31, 0, 0, 0, 0);
            this.cv_DatePickStart.MinDate = new System.DateTime(2019, 1, 3, 0, 0, 0, 0);
            this.cv_DatePickStart.Name = "cv_DatePickStart";
            this.cv_DatePickStart.Size = new System.Drawing.Size(200, 22);
            this.cv_DatePickStart.TabIndex = 0;
            this.cv_DatePickStart.Value = new System.DateTime(2019, 1, 3, 0, 0, 0, 0);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(23, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "From";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(23, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "To";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cv_DatePickEnd
            // 
            this.cv_DatePickEnd.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.cv_DatePickEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.cv_DatePickEnd.Location = new System.Drawing.Point(162, 93);
            this.cv_DatePickEnd.MaxDate = new System.DateTime(2040, 1, 31, 0, 0, 0, 0);
            this.cv_DatePickEnd.MinDate = new System.DateTime(2019, 3, 1, 0, 0, 0, 0);
            this.cv_DatePickEnd.Name = "cv_DatePickEnd";
            this.cv_DatePickEnd.Size = new System.Drawing.Size(200, 22);
            this.cv_DatePickEnd.TabIndex = 2;
            this.cv_DatePickEnd.Value = new System.DateTime(2019, 3, 1, 0, 0, 0, 0);
            // 
            // cv_BtCopy
            // 
            this.cv_BtCopy.Location = new System.Drawing.Point(412, 92);
            this.cv_BtCopy.Name = "cv_BtCopy";
            this.cv_BtCopy.Size = new System.Drawing.Size(75, 23);
            this.cv_BtCopy.TabIndex = 4;
            this.cv_BtCopy.Text = "Copy";
            this.cv_BtCopy.UseVisualStyleBackColor = true;
            this.cv_BtCopy.Click += new System.EventHandler(this.cv_BtCopy_Click);
            // 
            // cv_BtTargetFolder
            // 
            this.cv_BtTargetFolder.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cv_BtTargetFolder.Location = new System.Drawing.Point(12, 151);
            this.cv_BtTargetFolder.Name = "cv_BtTargetFolder";
            this.cv_BtTargetFolder.Size = new System.Drawing.Size(133, 28);
            this.cv_BtTargetFolder.TabIndex = 5;
            this.cv_BtTargetFolder.Text = "Target Folder";
            this.cv_BtTargetFolder.UseVisualStyleBackColor = true;
            this.cv_BtTargetFolder.Click += new System.EventHandler(this.cv_BtTargetFolder_Click);
            // 
            // cv_TxTargetFolder
            // 
            this.cv_TxTargetFolder.Location = new System.Drawing.Point(162, 157);
            this.cv_TxTargetFolder.Name = "cv_TxTargetFolder";
            this.cv_TxTargetFolder.Size = new System.Drawing.Size(410, 22);
            this.cv_TxTargetFolder.TabIndex = 6;
            // 
            // CopyLogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 209);
            this.Controls.Add(this.cv_TxTargetFolder);
            this.Controls.Add(this.cv_BtTargetFolder);
            this.Controls.Add(this.cv_BtCopy);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cv_DatePickEnd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cv_DatePickStart);
            this.Name = "CopyLogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Copy Log";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CopyLogForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker cv_DatePickStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker cv_DatePickEnd;
        private System.Windows.Forms.Button cv_BtCopy;
        private System.Windows.Forms.Button cv_BtTargetFolder;
        private System.Windows.Forms.TextBox cv_TxTargetFolder;
    }
}
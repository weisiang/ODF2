namespace UI
{
    partial class LotHold
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
            this.btDontHold = new System.Windows.Forms.Button();
            this.btHold = new System.Windows.Forms.Button();
            this.btRemeasure = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btDontHold
            // 
            this.btDontHold.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btDontHold.Location = new System.Drawing.Point(23, 101);
            this.btDontHold.Name = "btDontHold";
            this.btDontHold.Size = new System.Drawing.Size(105, 47);
            this.btDontHold.TabIndex = 0;
            this.btDontHold.Text = "Don’t  Hold Lot";
            this.btDontHold.UseVisualStyleBackColor = true;
            // 
            // btHold
            // 
            this.btHold.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btHold.Location = new System.Drawing.Point(147, 102);
            this.btHold.Name = "btHold";
            this.btHold.Size = new System.Drawing.Size(105, 47);
            this.btHold.TabIndex = 1;
            this.btHold.Text = "Hold Lot";
            this.btHold.UseVisualStyleBackColor = true;
            // 
            // btRemeasure
            // 
            this.btRemeasure.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.btRemeasure.Location = new System.Drawing.Point(275, 102);
            this.btRemeasure.Name = "btRemeasure";
            this.btRemeasure.Size = new System.Drawing.Size(105, 47);
            this.btRemeasure.TabIndex = 2;
            this.btRemeasure.Text = "Re-measure";
            this.btRemeasure.UseVisualStyleBackColor = true;
            // 
            // LotHold
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 234);
            this.Controls.Add(this.btRemeasure);
            this.Controls.Add(this.btHold);
            this.Controls.Add(this.btDontHold);
            this.Name = "LotHold";
            this.Text = "LotHold";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btDontHold;
        private System.Windows.Forms.Button btHold;
        private System.Windows.Forms.Button btRemeasure;
    }
}
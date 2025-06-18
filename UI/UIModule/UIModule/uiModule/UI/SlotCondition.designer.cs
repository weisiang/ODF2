namespace UI
{
    partial class SlotCondition
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
            this.lbl_No = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_Count = new System.Windows.Forms.TextBox();
            this.txt_Recipe = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_Abort = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lbl_No
            // 
            this.lbl_No.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbl_No.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbl_No.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_No.Location = new System.Drawing.Point(0, 0);
            this.lbl_No.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_No.Name = "lbl_No";
            this.lbl_No.Size = new System.Drawing.Size(30, 23);
            this.lbl_No.TabIndex = 0;
            this.lbl_No.Text = "00";
            this.lbl_No.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(30, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Count";
            // 
            // txt_Count
            // 
            this.txt_Count.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.txt_Count.Dock = System.Windows.Forms.DockStyle.Left;
            this.txt_Count.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Count.Location = new System.Drawing.Point(77, 0);
            this.txt_Count.Name = "txt_Count";
            this.txt_Count.Size = new System.Drawing.Size(100, 22);
            this.txt_Count.TabIndex = 2;
            // 
            // txt_Recipe
            // 
            this.txt_Recipe.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.txt_Recipe.Dock = System.Windows.Forms.DockStyle.Left;
            this.txt_Recipe.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Recipe.Location = new System.Drawing.Point(235, 0);
            this.txt_Recipe.Name = "txt_Recipe";
            this.txt_Recipe.Size = new System.Drawing.Size(100, 22);
            this.txt_Recipe.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(177, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Recipe";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(335, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Abort";
            // 
            // cb_Abort
            // 
            this.cb_Abort.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.cb_Abort.Dock = System.Windows.Forms.DockStyle.Left;
            this.cb_Abort.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_Abort.FormattingEnabled = true;
            this.cb_Abort.Items.AddRange(new object[] {
            "True",
            "False"});
            this.cb_Abort.Location = new System.Drawing.Point(380, 0);
            this.cb_Abort.Name = "cb_Abort";
            this.cb_Abort.Size = new System.Drawing.Size(79, 24);
            this.cb_Abort.TabIndex = 6;
            // 
            // SlotCondition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.cb_Abort);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_Recipe);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_Count);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_No);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SlotCondition";
            this.Size = new System.Drawing.Size(461, 23);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lbl_No;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_Count;
        private System.Windows.Forms.TextBox txt_Recipe;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cb_Abort;
    }
}

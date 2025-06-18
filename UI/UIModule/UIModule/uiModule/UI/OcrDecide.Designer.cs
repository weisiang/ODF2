namespace UI
{
    partial class OcrDecide
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
            this.btn_Skip = new System.Windows.Forms.Button();
            this.btn_Keyin = new System.Windows.Forms.Button();
            this.btn_Hold = new System.Windows.Forms.Button();
            this.btn_Return = new System.Windows.Forms.Button();
            this.txt_keyin = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_Skip
            // 
            this.btn_Skip.Location = new System.Drawing.Point(41, 98);
            this.btn_Skip.Name = "btn_Skip";
            this.btn_Skip.Size = new System.Drawing.Size(75, 23);
            this.btn_Skip.TabIndex = 1;
            this.btn_Skip.Text = "Skip";
            this.btn_Skip.UseVisualStyleBackColor = true;
            this.btn_Skip.Click += new System.EventHandler(this.btn_Skip_Click);
            // 
            // btn_Keyin
            // 
            this.btn_Keyin.Location = new System.Drawing.Point(41, 40);
            this.btn_Keyin.Name = "btn_Keyin";
            this.btn_Keyin.Size = new System.Drawing.Size(75, 23);
            this.btn_Keyin.TabIndex = 2;
            this.btn_Keyin.Text = "Key in";
            this.btn_Keyin.UseVisualStyleBackColor = true;
            this.btn_Keyin.Click += new System.EventHandler(this.btn_Keyin_Click);
            // 
            // btn_Hold
            // 
            this.btn_Hold.Location = new System.Drawing.Point(41, 127);
            this.btn_Hold.Name = "btn_Hold";
            this.btn_Hold.Size = new System.Drawing.Size(75, 23);
            this.btn_Hold.TabIndex = 3;
            this.btn_Hold.Text = "Hold";
            this.btn_Hold.UseVisualStyleBackColor = true;
            this.btn_Hold.Visible = false;
            this.btn_Hold.Click += new System.EventHandler(this.btn_Hold_Click);
            // 
            // btn_Return
            // 
            this.btn_Return.Location = new System.Drawing.Point(41, 69);
            this.btn_Return.Name = "btn_Return";
            this.btn_Return.Size = new System.Drawing.Size(75, 23);
            this.btn_Return.TabIndex = 4;
            this.btn_Return.Text = "Return";
            this.btn_Return.UseVisualStyleBackColor = true;
            this.btn_Return.Click += new System.EventHandler(this.btn_Return_Click);
            // 
            // txt_keyin
            // 
            this.txt_keyin.Location = new System.Drawing.Point(129, 42);
            this.txt_keyin.Name = "txt_keyin";
            this.txt_keyin.Size = new System.Drawing.Size(146, 22);
            this.txt_keyin.TabIndex = 5;
            // 
            // OcrDecide
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 208);
            this.ControlBox = false;
            this.Controls.Add(this.txt_keyin);
            this.Controls.Add(this.btn_Return);
            this.Controls.Add(this.btn_Hold);
            this.Controls.Add(this.btn_Keyin);
            this.Controls.Add(this.btn_Skip);
            this.Name = "OcrDecide";
            this.Text = "OcrDecide";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Skip;
        private System.Windows.Forms.Button btn_Keyin;
        private System.Windows.Forms.Button btn_Hold;
        private System.Windows.Forms.Button btn_Return;
        private System.Windows.Forms.TextBox txt_keyin;

    }
}
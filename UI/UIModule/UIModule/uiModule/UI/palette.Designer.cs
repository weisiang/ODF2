namespace UI
{
    partial class palette
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
            this.cv_RedBar = new System.Windows.Forms.TrackBar();
            this.cv_GreenBar = new System.Windows.Forms.TrackBar();
            this.cv_BlueBar = new System.Windows.Forms.TrackBar();
            this.cv_red = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cv_Blue = new System.Windows.Forms.Label();
            this.cv_Green = new System.Windows.Forms.Label();
            this.cv_palette = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.cv_RedBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cv_GreenBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cv_BlueBar)).BeginInit();
            this.SuspendLayout();
            // 
            // cv_RedBar
            // 
            this.cv_RedBar.Location = new System.Drawing.Point(22, 22);
            this.cv_RedBar.Maximum = 255;
            this.cv_RedBar.Name = "cv_RedBar";
            this.cv_RedBar.Size = new System.Drawing.Size(315, 45);
            this.cv_RedBar.TabIndex = 0;
            this.cv_RedBar.TickFrequency = 10;
            this.cv_RedBar.Scroll += new System.EventHandler(this.MixColor);
            // 
            // cv_GreenBar
            // 
            this.cv_GreenBar.Location = new System.Drawing.Point(22, 63);
            this.cv_GreenBar.Maximum = 255;
            this.cv_GreenBar.Name = "cv_GreenBar";
            this.cv_GreenBar.Size = new System.Drawing.Size(315, 45);
            this.cv_GreenBar.TabIndex = 0;
            this.cv_GreenBar.TickFrequency = 10;
            this.cv_GreenBar.Scroll += new System.EventHandler(this.MixColor);
            // 
            // cv_BlueBar
            // 
            this.cv_BlueBar.Location = new System.Drawing.Point(22, 105);
            this.cv_BlueBar.Maximum = 255;
            this.cv_BlueBar.Name = "cv_BlueBar";
            this.cv_BlueBar.Size = new System.Drawing.Size(315, 45);
            this.cv_BlueBar.TabIndex = 0;
            this.cv_BlueBar.TickFrequency = 10;
            this.cv_BlueBar.Scroll += new System.EventHandler(this.MixColor);
            // 
            // cv_red
            // 
            this.cv_red.AutoSize = true;
            this.cv_red.Location = new System.Drawing.Point(331, 25);
            this.cv_red.Name = "cv_red";
            this.cv_red.Size = new System.Drawing.Size(24, 12);
            this.cv_red.TabIndex = 1;
            this.cv_red.Text = "Red";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(331, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "label1";
            // 
            // cv_Blue
            // 
            this.cv_Blue.AutoSize = true;
            this.cv_Blue.Location = new System.Drawing.Point(331, 111);
            this.cv_Blue.Name = "cv_Blue";
            this.cv_Blue.Size = new System.Drawing.Size(27, 12);
            this.cv_Blue.TabIndex = 1;
            this.cv_Blue.Text = "Blue";
            // 
            // cv_Green
            // 
            this.cv_Green.AutoSize = true;
            this.cv_Green.Location = new System.Drawing.Point(331, 63);
            this.cv_Green.Name = "cv_Green";
            this.cv_Green.Size = new System.Drawing.Size(33, 12);
            this.cv_Green.TabIndex = 1;
            this.cv_Green.Text = "Green";
            // 
            // cv_palette
            // 
            this.cv_palette.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cv_palette.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cv_palette.Location = new System.Drawing.Point(32, 172);
            this.cv_palette.Name = "cv_palette";
            this.cv_palette.Size = new System.Drawing.Size(96, 100);
            this.cv_palette.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.34855F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.77734F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.95804F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.95804F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.95804F));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(144, 172);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.19482F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.19482F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.19482F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.19482F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.19482F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.02588F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(240, 100);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // palette
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 316);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.cv_palette);
            this.Controls.Add(this.cv_Blue);
            this.Controls.Add(this.cv_Green);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cv_red);
            this.Controls.Add(this.cv_BlueBar);
            this.Controls.Add(this.cv_GreenBar);
            this.Controls.Add(this.cv_RedBar);
            this.Name = "palette";
            this.Text = "palette";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.palette_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.cv_RedBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cv_GreenBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cv_BlueBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar cv_RedBar;
        private System.Windows.Forms.TrackBar cv_GreenBar;
        private System.Windows.Forms.TrackBar cv_BlueBar;
        private System.Windows.Forms.Label cv_red;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label cv_Blue;
        private System.Windows.Forms.Label cv_Green;
        private System.Windows.Forms.Panel cv_palette;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
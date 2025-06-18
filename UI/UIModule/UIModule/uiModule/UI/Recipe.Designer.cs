namespace UI
{
    partial class Recipe
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
            this.cv_DataView = new System.Windows.Forms.DataGridView();
            this.cv_NumberColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cv_SizeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cv_HeightColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cv_WidthColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cv_TimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cv_ButtonPanel = new System.Windows.Forms.Label();
            this.cv_BtOption = new System.Windows.Forms.Label();
            this.cv_BtExe = new System.Windows.Forms.Button();
            this.cv_LbRecipeNumber = new System.Windows.Forms.Label();
            this.cv_LbRecipeSize = new System.Windows.Forms.Label();
            this.cv_TxRecipeNumber = new System.Windows.Forms.TextBox();
            this.cv_TxRecipeSize = new System.Windows.Forms.TextBox();
            this.cv_LbRecipeHeight = new System.Windows.Forms.Label();
            this.cv_TxRecipeHeight = new System.Windows.Forms.TextBox();
            this.cv_LbRecipeWidth = new System.Windows.Forms.Label();
            this.cv_TxRecipeWidth = new System.Windows.Forms.TextBox();
            this.cv_IDHint = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cv_CbRecipeOption = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.cv_DataView)).BeginInit();
            this.SuspendLayout();
            // 
            // cv_DataView
            // 
            this.cv_DataView.AllowUserToAddRows = false;
            this.cv_DataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cv_DataView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cv_NumberColumn,
            this.cv_SizeColumn,
            this.cv_HeightColumn,
            this.cv_WidthColumn,
            this.cv_TimeColumn});
            this.cv_DataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cv_DataView.Location = new System.Drawing.Point(0, 115);
            this.cv_DataView.Name = "cv_DataView";
            this.cv_DataView.RowTemplate.Height = 24;
            this.cv_DataView.Size = new System.Drawing.Size(834, 416);
            this.cv_DataView.TabIndex = 0;
            this.cv_DataView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.cv_DataView_CellDoubleClick);
            // 
            // cv_NumberColumn
            // 
            this.cv_NumberColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cv_NumberColumn.HeaderText = "ID";
            this.cv_NumberColumn.Name = "cv_NumberColumn";
            // 
            // cv_SizeColumn
            // 
            this.cv_SizeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cv_SizeColumn.HeaderText = "Thickness";
            this.cv_SizeColumn.Name = "cv_SizeColumn";
            // 
            // cv_HeightColumn
            // 
            this.cv_HeightColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cv_HeightColumn.HeaderText = "Height";
            this.cv_HeightColumn.Name = "cv_HeightColumn";
            // 
            // cv_WidthColumn
            // 
            this.cv_WidthColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cv_WidthColumn.HeaderText = "Width";
            this.cv_WidthColumn.Name = "cv_WidthColumn";
            // 
            // cv_TimeColumn
            // 
            this.cv_TimeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cv_TimeColumn.HeaderText = "Time";
            this.cv_TimeColumn.Name = "cv_TimeColumn";
            // 
            // cv_ButtonPanel
            // 
            this.cv_ButtonPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.cv_ButtonPanel.Location = new System.Drawing.Point(0, 0);
            this.cv_ButtonPanel.Name = "cv_ButtonPanel";
            this.cv_ButtonPanel.Size = new System.Drawing.Size(834, 115);
            this.cv_ButtonPanel.TabIndex = 1;
            // 
            // cv_BtOption
            // 
            this.cv_BtOption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.cv_BtOption.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.cv_BtOption.Location = new System.Drawing.Point(6, 29);
            this.cv_BtOption.Name = "cv_BtOption";
            this.cv_BtOption.Size = new System.Drawing.Size(89, 23);
            this.cv_BtOption.TabIndex = 2;
            // 
            // cv_BtExe
            // 
            this.cv_BtExe.Location = new System.Drawing.Point(101, 29);
            this.cv_BtExe.Name = "cv_BtExe";
            this.cv_BtExe.Size = new System.Drawing.Size(89, 58);
            this.cv_BtExe.TabIndex = 4;
            this.cv_BtExe.Text = "EXE";
            this.cv_BtExe.UseVisualStyleBackColor = true;
            this.cv_BtExe.Click += new System.EventHandler(this.cv_BtExe_Click);
            // 
            // cv_LbRecipeNumber
            // 
            this.cv_LbRecipeNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cv_LbRecipeNumber.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cv_LbRecipeNumber.Location = new System.Drawing.Point(213, 29);
            this.cv_LbRecipeNumber.Name = "cv_LbRecipeNumber";
            this.cv_LbRecipeNumber.Size = new System.Drawing.Size(100, 23);
            this.cv_LbRecipeNumber.TabIndex = 5;
            this.cv_LbRecipeNumber.Text = "ID";
            this.cv_LbRecipeNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cv_LbRecipeSize
            // 
            this.cv_LbRecipeSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cv_LbRecipeSize.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cv_LbRecipeSize.Location = new System.Drawing.Point(213, 64);
            this.cv_LbRecipeSize.Name = "cv_LbRecipeSize";
            this.cv_LbRecipeSize.Size = new System.Drawing.Size(100, 23);
            this.cv_LbRecipeSize.TabIndex = 6;
            this.cv_LbRecipeSize.Text = "Thickness";
            this.cv_LbRecipeSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cv_TxRecipeNumber
            // 
            this.cv_TxRecipeNumber.Location = new System.Drawing.Point(319, 29);
            this.cv_TxRecipeNumber.MaxLength = 3;
            this.cv_TxRecipeNumber.Name = "cv_TxRecipeNumber";
            this.cv_TxRecipeNumber.Size = new System.Drawing.Size(186, 22);
            this.cv_TxRecipeNumber.TabIndex = 7;
            // 
            // cv_TxRecipeSize
            // 
            this.cv_TxRecipeSize.Location = new System.Drawing.Point(319, 64);
            this.cv_TxRecipeSize.MaxLength = 16;
            this.cv_TxRecipeSize.Name = "cv_TxRecipeSize";
            this.cv_TxRecipeSize.Size = new System.Drawing.Size(186, 22);
            this.cv_TxRecipeSize.TabIndex = 8;
            // 
            // cv_LbRecipeHeight
            // 
            this.cv_LbRecipeHeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cv_LbRecipeHeight.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cv_LbRecipeHeight.Location = new System.Drawing.Point(522, 28);
            this.cv_LbRecipeHeight.Name = "cv_LbRecipeHeight";
            this.cv_LbRecipeHeight.Size = new System.Drawing.Size(100, 23);
            this.cv_LbRecipeHeight.TabIndex = 9;
            this.cv_LbRecipeHeight.Text = "Height";
            this.cv_LbRecipeHeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cv_TxRecipeHeight
            // 
            this.cv_TxRecipeHeight.Location = new System.Drawing.Point(628, 28);
            this.cv_TxRecipeHeight.MaxLength = 16;
            this.cv_TxRecipeHeight.Name = "cv_TxRecipeHeight";
            this.cv_TxRecipeHeight.Size = new System.Drawing.Size(186, 22);
            this.cv_TxRecipeHeight.TabIndex = 10;
            // 
            // cv_LbRecipeWidth
            // 
            this.cv_LbRecipeWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cv_LbRecipeWidth.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cv_LbRecipeWidth.Location = new System.Drawing.Point(522, 64);
            this.cv_LbRecipeWidth.Name = "cv_LbRecipeWidth";
            this.cv_LbRecipeWidth.Size = new System.Drawing.Size(100, 23);
            this.cv_LbRecipeWidth.TabIndex = 11;
            this.cv_LbRecipeWidth.Text = "Width";
            this.cv_LbRecipeWidth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cv_TxRecipeWidth
            // 
            this.cv_TxRecipeWidth.Location = new System.Drawing.Point(628, 64);
            this.cv_TxRecipeWidth.MaxLength = 16;
            this.cv_TxRecipeWidth.Name = "cv_TxRecipeWidth";
            this.cv_TxRecipeWidth.Size = new System.Drawing.Size(186, 22);
            this.cv_TxRecipeWidth.TabIndex = 12;
            // 
            // cv_IDHint
            // 
            this.cv_IDHint.BackColor = System.Drawing.Color.White;
            this.cv_IDHint.ForeColor = System.Drawing.Color.LightGray;
            this.cv_IDHint.Location = new System.Drawing.Point(450, 30);
            this.cv_IDHint.Name = "cv_IDHint";
            this.cv_IDHint.Size = new System.Drawing.Size(50, 20);
            this.cv_IDHint.TabIndex = 13;
            this.cv_IDHint.Text = "3 Char";
            this.cv_IDHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.ForeColor = System.Drawing.Color.LightGray;
            this.label1.Location = new System.Drawing.Point(761, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 18);
            this.label1.TabIndex = 14;
            this.label1.Text = "2600 mm";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.ForeColor = System.Drawing.Color.LightGray;
            this.label2.Location = new System.Drawing.Point(435, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 18);
            this.label2.TabIndex = 15;
            this.label2.Text = "0.3 - 0.7 mm";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.ForeColor = System.Drawing.Color.LightGray;
            this.label3.Location = new System.Drawing.Point(762, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 18);
            this.label3.TabIndex = 16;
            this.label3.Text = "2250 mm";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cv_CbRecipeOption
            // 
            this.cv_CbRecipeOption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.cv_CbRecipeOption.FormattingEnabled = true;
            this.cv_CbRecipeOption.Items.AddRange(new object[] {
            "Create",
            "Erase",
            "Modify",
            "Cur.Recipe"});
            this.cv_CbRecipeOption.Location = new System.Drawing.Point(6, 62);
            this.cv_CbRecipeOption.Name = "cv_CbRecipeOption";
            this.cv_CbRecipeOption.Size = new System.Drawing.Size(89, 20);
            this.cv_CbRecipeOption.TabIndex = 17;
            this.cv_CbRecipeOption.SelectedValueChanged += new System.EventHandler(this.comboBox1_SelectedValueChanged);
            // 
            // Recipe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 531);
            this.Controls.Add(this.cv_CbRecipeOption);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cv_IDHint);
            this.Controls.Add(this.cv_TxRecipeWidth);
            this.Controls.Add(this.cv_LbRecipeWidth);
            this.Controls.Add(this.cv_TxRecipeHeight);
            this.Controls.Add(this.cv_LbRecipeHeight);
            this.Controls.Add(this.cv_TxRecipeSize);
            this.Controls.Add(this.cv_TxRecipeNumber);
            this.Controls.Add(this.cv_LbRecipeSize);
            this.Controls.Add(this.cv_LbRecipeNumber);
            this.Controls.Add(this.cv_BtExe);
            this.Controls.Add(this.cv_BtOption);
            this.Controls.Add(this.cv_DataView);
            this.Controls.Add(this.cv_ButtonPanel);
            this.Name = "Recipe";
            this.Text = "Recipe";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Recipe_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.cv_DataView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView cv_DataView;
        private System.Windows.Forms.Label cv_ButtonPanel;
        private System.Windows.Forms.Label cv_BtOption;
        private System.Windows.Forms.Button cv_BtExe;
        private System.Windows.Forms.Label cv_LbRecipeNumber;
        private System.Windows.Forms.Label cv_LbRecipeSize;
        private System.Windows.Forms.TextBox cv_TxRecipeNumber;
        private System.Windows.Forms.TextBox cv_TxRecipeSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn cv_NumberColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cv_SizeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cv_HeightColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cv_WidthColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cv_TimeColumn;
        private System.Windows.Forms.Label cv_LbRecipeHeight;
        private System.Windows.Forms.TextBox cv_TxRecipeHeight;
        private System.Windows.Forms.Label cv_LbRecipeWidth;
        private System.Windows.Forms.TextBox cv_TxRecipeWidth;
        private System.Windows.Forms.Label cv_IDHint;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cv_CbRecipeOption;
    }
}
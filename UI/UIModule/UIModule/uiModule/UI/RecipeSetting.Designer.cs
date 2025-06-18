namespace UI
{
    partial class RecipeSetting
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cb_NeedGlass = new System.Windows.Forms.CheckBox();
            this.btn_CurRecipe = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cb_CurRecipe = new System.Windows.Forms.ComboBox();
            this.lbl_RecipeDescription = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txt_FlowDescription = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cv_TxGlassVas = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Modify = new System.Windows.Forms.Button();
            this.btn_Del = new System.Windows.Forms.Button();
            this.cb_Flow = new System.Windows.Forms.ComboBox();
            this.cv_TxWaferSdp = new System.Windows.Forms.TextBox();
            this.cv_LbRecipeWidth = new System.Windows.Forms.Label();
            this.cv_LbRecipeHeight = new System.Windows.Forms.Label();
            this.cv_TxWaferIjp = new System.Windows.Forms.TextBox();
            this.cv_TxRecipeId = new System.Windows.Forms.TextBox();
            this.cv_LbRecipeSize = new System.Windows.Forms.Label();
            this.cv_LbRecipeNumber = new System.Windows.Forms.Label();
            this.btn_Add = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cv_RecipeDataView = new System.Windows.Forms.DataGridView();
            this.cv_NumberColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cv_SizeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NeedGlass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cv_WidthColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cv_HeightColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WaferAOI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WaferVAS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cv_TimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cv_TxWaferVas = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cv_TxWaferAoi = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cv_RecipeDataView)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(252)))), ((int)(((byte)(254)))));
            this.panel1.Controls.Add(this.cv_TxWaferAoi);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.cv_TxWaferVas);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.cb_NeedGlass);
            this.panel1.Controls.Add(this.btn_CurRecipe);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cb_CurRecipe);
            this.panel1.Controls.Add(this.lbl_RecipeDescription);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.cv_TxGlassVas);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btn_Modify);
            this.panel1.Controls.Add(this.btn_Del);
            this.panel1.Controls.Add(this.cb_Flow);
            this.panel1.Controls.Add(this.cv_TxWaferSdp);
            this.panel1.Controls.Add(this.cv_LbRecipeWidth);
            this.panel1.Controls.Add(this.cv_LbRecipeHeight);
            this.panel1.Controls.Add(this.cv_TxWaferIjp);
            this.panel1.Controls.Add(this.cv_TxRecipeId);
            this.panel1.Controls.Add(this.cv_LbRecipeSize);
            this.panel1.Controls.Add(this.cv_LbRecipeNumber);
            this.panel1.Controls.Add(this.btn_Add);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1509, 259);
            this.panel1.TabIndex = 22;
            // 
            // cb_NeedGlass
            // 
            this.cb_NeedGlass.AutoSize = true;
            this.cb_NeedGlass.Enabled = false;
            this.cb_NeedGlass.Location = new System.Drawing.Point(1224, 39);
            this.cb_NeedGlass.Name = "cb_NeedGlass";
            this.cb_NeedGlass.Size = new System.Drawing.Size(48, 16);
            this.cb_NeedGlass.TabIndex = 44;
            this.cb_NeedGlass.Text = "Glass";
            this.cb_NeedGlass.UseVisualStyleBackColor = true;
            this.cb_NeedGlass.Visible = false;
            // 
            // btn_CurRecipe
            // 
            this.btn_CurRecipe.Location = new System.Drawing.Point(1075, 63);
            this.btn_CurRecipe.Name = "btn_CurRecipe";
            this.btn_CurRecipe.Size = new System.Drawing.Size(89, 22);
            this.btn_CurRecipe.TabIndex = 43;
            this.btn_CurRecipe.Text = "SET";
            this.btn_CurRecipe.UseVisualStyleBackColor = true;
            this.btn_CurRecipe.Click += new System.EventHandler(this.btn_CurRecipe_Click);
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(952, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 23);
            this.label4.TabIndex = 42;
            this.label4.Text = "Set Cur. Recipe";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cb_CurRecipe
            // 
            this.cb_CurRecipe.FormattingEnabled = true;
            this.cb_CurRecipe.Location = new System.Drawing.Point(1075, 37);
            this.cb_CurRecipe.Name = "cb_CurRecipe";
            this.cb_CurRecipe.Size = new System.Drawing.Size(121, 20);
            this.cb_CurRecipe.TabIndex = 41;
            // 
            // lbl_RecipeDescription
            // 
            this.lbl_RecipeDescription.Location = new System.Drawing.Point(485, 140);
            this.lbl_RecipeDescription.MaxLength = 30;
            this.lbl_RecipeDescription.Name = "lbl_RecipeDescription";
            this.lbl_RecipeDescription.Size = new System.Drawing.Size(186, 22);
            this.lbl_RecipeDescription.TabIndex = 40;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(362, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 23);
            this.label2.TabIndex = 39;
            this.label2.Text = "Description";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panel2.Controls.Add(this.txt_FlowDescription);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Location = new System.Drawing.Point(89, 35);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(227, 207);
            this.panel2.TabIndex = 38;
            // 
            // txt_FlowDescription
            // 
            this.txt_FlowDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txt_FlowDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_FlowDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_FlowDescription.Location = new System.Drawing.Point(0, 26);
            this.txt_FlowDescription.Name = "txt_FlowDescription";
            this.txt_FlowDescription.Size = new System.Drawing.Size(227, 181);
            this.txt_FlowDescription.TabIndex = 40;
            this.txt_FlowDescription.Text = "";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(227, 26);
            this.label3.TabIndex = 39;
            this.label3.Text = "Flow contents";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cv_TxGlassVas
            // 
            this.cv_TxGlassVas.Location = new System.Drawing.Point(822, 141);
            this.cv_TxGlassVas.MaxLength = 16;
            this.cv_TxGlassVas.Name = "cv_TxGlassVas";
            this.cv_TxGlassVas.Size = new System.Drawing.Size(106, 22);
            this.cv_TxGlassVas.TabIndex = 35;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(699, 140);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 23);
            this.label1.TabIndex = 34;
            this.label1.Text = "Glass VAS";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_Modify
            // 
            this.btn_Modify.Location = new System.Drawing.Point(456, 200);
            this.btn_Modify.Name = "btn_Modify";
            this.btn_Modify.Size = new System.Drawing.Size(89, 22);
            this.btn_Modify.TabIndex = 33;
            this.btn_Modify.Text = "MODIFY";
            this.btn_Modify.UseVisualStyleBackColor = true;
            this.btn_Modify.Click += new System.EventHandler(this.btn_Modify_Click);
            // 
            // btn_Del
            // 
            this.btn_Del.Location = new System.Drawing.Point(551, 200);
            this.btn_Del.Name = "btn_Del";
            this.btn_Del.Size = new System.Drawing.Size(89, 22);
            this.btn_Del.TabIndex = 32;
            this.btn_Del.Text = "DEL";
            this.btn_Del.UseVisualStyleBackColor = true;
            this.btn_Del.Click += new System.EventHandler(this.btn_Del_Click);
            // 
            // cb_Flow
            // 
            this.cb_Flow.FormattingEnabled = true;
            this.cb_Flow.Location = new System.Drawing.Point(487, 90);
            this.cb_Flow.Name = "cb_Flow";
            this.cb_Flow.Size = new System.Drawing.Size(184, 20);
            this.cb_Flow.TabIndex = 31;
            this.cb_Flow.SelectedIndexChanged += new System.EventHandler(this.cb_Flow_SelectedIndexChanged);
            // 
            // cv_TxWaferSdp
            // 
            this.cv_TxWaferSdp.Location = new System.Drawing.Point(821, 41);
            this.cv_TxWaferSdp.MaxLength = 16;
            this.cv_TxWaferSdp.Name = "cv_TxWaferSdp";
            this.cv_TxWaferSdp.Size = new System.Drawing.Size(106, 22);
            this.cv_TxWaferSdp.TabIndex = 30;
            // 
            // cv_LbRecipeWidth
            // 
            this.cv_LbRecipeWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cv_LbRecipeWidth.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cv_LbRecipeWidth.Location = new System.Drawing.Point(699, 89);
            this.cv_LbRecipeWidth.Name = "cv_LbRecipeWidth";
            this.cv_LbRecipeWidth.Size = new System.Drawing.Size(117, 23);
            this.cv_LbRecipeWidth.TabIndex = 29;
            this.cv_LbRecipeWidth.Text = "Wafer IJP";
            this.cv_LbRecipeWidth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cv_LbRecipeHeight
            // 
            this.cv_LbRecipeHeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cv_LbRecipeHeight.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cv_LbRecipeHeight.Location = new System.Drawing.Point(363, 89);
            this.cv_LbRecipeHeight.Name = "cv_LbRecipeHeight";
            this.cv_LbRecipeHeight.Size = new System.Drawing.Size(117, 23);
            this.cv_LbRecipeHeight.TabIndex = 27;
            this.cv_LbRecipeHeight.Text = "Flow";
            this.cv_LbRecipeHeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cv_TxWaferIjp
            // 
            this.cv_TxWaferIjp.Location = new System.Drawing.Point(822, 90);
            this.cv_TxWaferIjp.MaxLength = 16;
            this.cv_TxWaferIjp.Name = "cv_TxWaferIjp";
            this.cv_TxWaferIjp.Size = new System.Drawing.Size(106, 22);
            this.cv_TxWaferIjp.TabIndex = 26;
            // 
            // cv_TxRecipeId
            // 
            this.cv_TxRecipeId.Location = new System.Drawing.Point(485, 38);
            this.cv_TxRecipeId.MaxLength = 3;
            this.cv_TxRecipeId.Name = "cv_TxRecipeId";
            this.cv_TxRecipeId.Size = new System.Drawing.Size(186, 22);
            this.cv_TxRecipeId.TabIndex = 25;
            this.cv_TxRecipeId.TextChanged += new System.EventHandler(this.cv_TxRecipeId_TextChanged);
            // 
            // cv_LbRecipeSize
            // 
            this.cv_LbRecipeSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cv_LbRecipeSize.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cv_LbRecipeSize.Location = new System.Drawing.Point(699, 38);
            this.cv_LbRecipeSize.Name = "cv_LbRecipeSize";
            this.cv_LbRecipeSize.Size = new System.Drawing.Size(117, 23);
            this.cv_LbRecipeSize.TabIndex = 24;
            this.cv_LbRecipeSize.Text = "Wafer SDP";
            this.cv_LbRecipeSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cv_LbRecipeNumber
            // 
            this.cv_LbRecipeNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cv_LbRecipeNumber.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cv_LbRecipeNumber.Location = new System.Drawing.Point(362, 38);
            this.cv_LbRecipeNumber.Name = "cv_LbRecipeNumber";
            this.cv_LbRecipeNumber.Size = new System.Drawing.Size(117, 23);
            this.cv_LbRecipeNumber.TabIndex = 23;
            this.cv_LbRecipeNumber.Text = "Recipe No";
            this.cv_LbRecipeNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_Add
            // 
            this.btn_Add.Location = new System.Drawing.Point(362, 200);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(89, 22);
            this.btn_Add.TabIndex = 22;
            this.btn_Add.Text = "ADD";
            this.btn_Add.UseVisualStyleBackColor = true;
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // cv_RecipeDataView
            // 
            this.cv_RecipeDataView.AllowUserToAddRows = false;
            this.cv_RecipeDataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cv_RecipeDataView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cv_NumberColumn,
            this.cv_SizeColumn,
            this.NeedGlass,
            this.cv_WidthColumn,
            this.cv_HeightColumn,
            this.WaferAOI,
            this.WaferVAS,
            this.Column1,
            this.cv_TimeColumn,
            this.Column2});
            this.cv_RecipeDataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cv_RecipeDataView.Location = new System.Drawing.Point(0, 259);
            this.cv_RecipeDataView.MultiSelect = false;
            this.cv_RecipeDataView.Name = "cv_RecipeDataView";
            this.cv_RecipeDataView.ReadOnly = true;
            this.cv_RecipeDataView.RowTemplate.Height = 24;
            this.cv_RecipeDataView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.cv_RecipeDataView.Size = new System.Drawing.Size(1509, 263);
            this.cv_RecipeDataView.TabIndex = 1;
            this.cv_RecipeDataView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.cv_RecipeDataView_CellDoubleClick);
            // 
            // cv_NumberColumn
            // 
            this.cv_NumberColumn.DataPropertyName = "PId";
            this.cv_NumberColumn.HeaderText = "ID";
            this.cv_NumberColumn.Name = "cv_NumberColumn";
            this.cv_NumberColumn.ReadOnly = true;
            this.cv_NumberColumn.Width = 150;
            // 
            // cv_SizeColumn
            // 
            this.cv_SizeColumn.DataPropertyName = "PFlow";
            this.cv_SizeColumn.HeaderText = "Flow";
            this.cv_SizeColumn.Name = "cv_SizeColumn";
            this.cv_SizeColumn.ReadOnly = true;
            this.cv_SizeColumn.Width = 150;
            // 
            // NeedGlass
            // 
            this.NeedGlass.DataPropertyName = "PVasNeedGlass";
            this.NeedGlass.HeaderText = "NeedGlass";
            this.NeedGlass.Name = "NeedGlass";
            this.NeedGlass.ReadOnly = true;
            // 
            // cv_WidthColumn
            // 
            this.cv_WidthColumn.DataPropertyName = "PWaferIJPDegree";
            this.cv_WidthColumn.HeaderText = "Wafer SDP";
            this.cv_WidthColumn.Name = "cv_WidthColumn";
            this.cv_WidthColumn.ReadOnly = true;
            // 
            // cv_HeightColumn
            // 
            this.cv_HeightColumn.DataPropertyName = "PWaferVASDegree";
            this.cv_HeightColumn.HeaderText = "Wafer IJP";
            this.cv_HeightColumn.Name = "cv_HeightColumn";
            this.cv_HeightColumn.ReadOnly = true;
            // 
            // WaferAOI
            // 
            this.WaferAOI.DataPropertyName = "PWaferAoiDegree";
            this.WaferAOI.HeaderText = "Wafer AOI";
            this.WaferAOI.Name = "WaferAOI";
            this.WaferAOI.ReadOnly = true;
            // 
            // WaferVAS
            // 
            this.WaferVAS.DataPropertyName = "PWaferVASDegree";
            this.WaferVAS.HeaderText = "Wafer VAS";
            this.WaferVAS.Name = "WaferVAS";
            this.WaferVAS.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "PGlassVASDegree";
            this.Column1.HeaderText = "Glass VAS";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // cv_TimeColumn
            // 
            this.cv_TimeColumn.DataPropertyName = "PTime";
            this.cv_TimeColumn.HeaderText = "Time";
            this.cv_TimeColumn.Name = "cv_TimeColumn";
            this.cv_TimeColumn.ReadOnly = true;
            this.cv_TimeColumn.Width = 150;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.DataPropertyName = "PDecription";
            this.Column2.HeaderText = "Description";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // cv_TxWaferVas
            // 
            this.cv_TxWaferVas.Location = new System.Drawing.Point(821, 186);
            this.cv_TxWaferVas.MaxLength = 16;
            this.cv_TxWaferVas.Name = "cv_TxWaferVas";
            this.cv_TxWaferVas.Size = new System.Drawing.Size(106, 22);
            this.cv_TxWaferVas.TabIndex = 46;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(699, 183);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 23);
            this.label5.TabIndex = 45;
            this.label5.Text = "Wafer VAS";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cv_TxWaferAoi
            // 
            this.cv_TxWaferAoi.Location = new System.Drawing.Point(821, 222);
            this.cv_TxWaferAoi.MaxLength = 16;
            this.cv_TxWaferAoi.Name = "cv_TxWaferAoi";
            this.cv_TxWaferAoi.Size = new System.Drawing.Size(106, 22);
            this.cv_TxWaferAoi.TabIndex = 48;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(699, 219);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(117, 23);
            this.label6.TabIndex = 47;
            this.label6.Text = "Wafer AOI";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RecipeSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cv_RecipeDataView);
            this.Controls.Add(this.panel1);
            this.Name = "RecipeSetting";
            this.Size = new System.Drawing.Size(1509, 522);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cv_RecipeDataView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox cv_TxWaferSdp;
        private System.Windows.Forms.Label cv_LbRecipeWidth;
        private System.Windows.Forms.Label cv_LbRecipeHeight;
        private System.Windows.Forms.TextBox cv_TxWaferIjp;
        private System.Windows.Forms.TextBox cv_TxRecipeId;
        private System.Windows.Forms.Label cv_LbRecipeSize;
        private System.Windows.Forms.Label cv_LbRecipeNumber;
        private System.Windows.Forms.Button btn_Add;
        private System.Windows.Forms.Button btn_Modify;
        private System.Windows.Forms.Button btn_Del;
        private System.Windows.Forms.ComboBox cb_Flow;
        private System.Windows.Forms.TextBox cv_TxGlassVas;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        public System.Windows.Forms.DataGridView cv_RecipeDataView;
        private System.Windows.Forms.TextBox lbl_RecipeDescription;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox txt_FlowDescription;
        private System.Windows.Forms.Button btn_CurRecipe;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cb_CurRecipe;
        private System.Windows.Forms.CheckBox cb_NeedGlass;
        private System.Windows.Forms.TextBox cv_TxWaferAoi;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox cv_TxWaferVas;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewTextBoxColumn cv_NumberColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cv_SizeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn NeedGlass;
        private System.Windows.Forms.DataGridViewTextBoxColumn cv_WidthColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cv_HeightColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn WaferAOI;
        private System.Windows.Forms.DataGridViewTextBoxColumn WaferVAS;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn cv_TimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
    }
}

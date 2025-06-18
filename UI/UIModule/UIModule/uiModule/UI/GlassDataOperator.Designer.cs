namespace UI
{
    partial class UiGlassDataOperator
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.txt_Reason = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.cv_TpRemove = new System.Windows.Forms.TabPage();
            this.cv_DataGridRemove = new System.Windows.Forms.DataGridView();
            this.btn_modify = new System.Windows.Forms.Button();
            this.btn_Request = new System.Windows.Forms.Button();
            this.btn_remove = new System.Windows.Forms.Button();
            this.btn_Add = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.txt_PPID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cb_AssambleFlag = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cb_AssambleResult = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cv_TxGlassId = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cv_CbCimMode = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cv_txFoupSeq = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txt_WorkOrder = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_WorkSlot = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cb_WorkType = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.cb_Production = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cv_CmGlassjudge = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cv_CbProcessFlag = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_Priority = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_OcrReadFlag = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_OcrResult = new System.Windows.Forms.ComboBox();
            this.pan_No = new System.Windows.Forms.Panel();
            this.cv_ColumnGlassId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.cv_TpRemove.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cv_DataGridRemove)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txt_Reason);
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Controls.Add(this.btn_modify);
            this.panel1.Controls.Add(this.btn_Request);
            this.panel1.Controls.Add(this.btn_remove);
            this.panel1.Controls.Add(this.btn_Add);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(594, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(309, 569);
            this.panel1.TabIndex = 84;
            // 
            // txt_Reason
            // 
            this.txt_Reason.Location = new System.Drawing.Point(92, 104);
            this.txt_Reason.Name = "txt_Reason";
            this.txt_Reason.Size = new System.Drawing.Size(184, 22);
            this.txt_Reason.TabIndex = 58;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.cv_TpRemove);
            this.tabControl1.Location = new System.Drawing.Point(11, 197);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(286, 343);
            this.tabControl1.TabIndex = 57;
            // 
            // cv_TpRemove
            // 
            this.cv_TpRemove.BackColor = System.Drawing.Color.Khaki;
            this.cv_TpRemove.Controls.Add(this.cv_DataGridRemove);
            this.cv_TpRemove.Location = new System.Drawing.Point(4, 22);
            this.cv_TpRemove.Name = "cv_TpRemove";
            this.cv_TpRemove.Padding = new System.Windows.Forms.Padding(3);
            this.cv_TpRemove.Size = new System.Drawing.Size(278, 317);
            this.cv_TpRemove.TabIndex = 1;
            this.cv_TpRemove.Text = "Remove";
            // 
            // cv_DataGridRemove
            // 
            this.cv_DataGridRemove.AllowUserToAddRows = false;
            this.cv_DataGridRemove.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cv_DataGridRemove.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cv_ColumnGlassId});
            this.cv_DataGridRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cv_DataGridRemove.Location = new System.Drawing.Point(3, 3);
            this.cv_DataGridRemove.Name = "cv_DataGridRemove";
            this.cv_DataGridRemove.RowTemplate.Height = 24;
            this.cv_DataGridRemove.Size = new System.Drawing.Size(272, 311);
            this.cv_DataGridRemove.TabIndex = 0;
            this.cv_DataGridRemove.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.cv_DataGridRemove_CellDoubleClick_1);
            // 
            // btn_modify
            // 
            this.btn_modify.Location = new System.Drawing.Point(11, 57);
            this.btn_modify.Name = "btn_modify";
            this.btn_modify.Size = new System.Drawing.Size(75, 23);
            this.btn_modify.TabIndex = 56;
            this.btn_modify.Text = "Modify";
            this.btn_modify.UseVisualStyleBackColor = true;
            this.btn_modify.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // btn_Request
            // 
            this.btn_Request.Location = new System.Drawing.Point(11, 147);
            this.btn_Request.Name = "btn_Request";
            this.btn_Request.Size = new System.Drawing.Size(75, 23);
            this.btn_Request.TabIndex = 52;
            this.btn_Request.Text = "Request";
            this.btn_Request.UseVisualStyleBackColor = true;
            this.btn_Request.Click += new System.EventHandler(this.btn_Request_Click);
            // 
            // btn_remove
            // 
            this.btn_remove.Location = new System.Drawing.Point(11, 102);
            this.btn_remove.Name = "btn_remove";
            this.btn_remove.Size = new System.Drawing.Size(75, 23);
            this.btn_remove.TabIndex = 50;
            this.btn_remove.Text = "Remove";
            this.btn_remove.UseVisualStyleBackColor = true;
            this.btn_remove.Click += new System.EventHandler(this.btn_remove_Click);
            // 
            // btn_Add
            // 
            this.btn_Add.Location = new System.Drawing.Point(11, 12);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(75, 23);
            this.btn_Add.TabIndex = 48;
            this.btn_Add.Text = "Add";
            this.btn_Add.UseVisualStyleBackColor = true;
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label12);
            this.flowLayoutPanel1.Controls.Add(this.txt_PPID);
            this.flowLayoutPanel1.Controls.Add(this.label6);
            this.flowLayoutPanel1.Controls.Add(this.cb_AssambleFlag);
            this.flowLayoutPanel1.Controls.Add(this.label10);
            this.flowLayoutPanel1.Controls.Add(this.cb_AssambleResult);
            this.flowLayoutPanel1.Controls.Add(this.label14);
            this.flowLayoutPanel1.Controls.Add(this.cv_TxGlassId);
            this.flowLayoutPanel1.Controls.Add(this.label11);
            this.flowLayoutPanel1.Controls.Add(this.cv_CbCimMode);
            this.flowLayoutPanel1.Controls.Add(this.label4);
            this.flowLayoutPanel1.Controls.Add(this.cv_txFoupSeq);
            this.flowLayoutPanel1.Controls.Add(this.label13);
            this.flowLayoutPanel1.Controls.Add(this.txt_WorkOrder);
            this.flowLayoutPanel1.Controls.Add(this.label5);
            this.flowLayoutPanel1.Controls.Add(this.txt_WorkSlot);
            this.flowLayoutPanel1.Controls.Add(this.label7);
            this.flowLayoutPanel1.Controls.Add(this.cb_WorkType);
            this.flowLayoutPanel1.Controls.Add(this.label16);
            this.flowLayoutPanel1.Controls.Add(this.cb_Production);
            this.flowLayoutPanel1.Controls.Add(this.label8);
            this.flowLayoutPanel1.Controls.Add(this.cv_CmGlassjudge);
            this.flowLayoutPanel1.Controls.Add(this.label9);
            this.flowLayoutPanel1.Controls.Add(this.cv_CbProcessFlag);
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.cb_Priority);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.cb_OcrReadFlag);
            this.flowLayoutPanel1.Controls.Add(this.label3);
            this.flowLayoutPanel1.Controls.Add(this.cb_OcrResult);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(594, 216);
            this.flowLayoutPanel1.TabIndex = 85;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(29)))), ((int)(((byte)(132)))));
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label12.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(3, 192);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(153, 18);
            this.label12.TabIndex = 103;
            this.label12.Text = "PPID";
            // 
            // txt_PPID
            // 
            this.txt_PPID.Location = new System.Drawing.Point(162, 190);
            this.txt_PPID.MaxLength = 2;
            this.txt_PPID.Name = "txt_PPID";
            this.txt_PPID.Size = new System.Drawing.Size(130, 22);
            this.txt_PPID.TabIndex = 104;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(29)))), ((int)(((byte)(132)))));
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(3, 166);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(153, 18);
            this.label6.TabIndex = 100;
            this.label6.Text = "Assamble Flag";
            // 
            // cb_AssambleFlag
            // 
            this.cb_AssambleFlag.FormattingEnabled = true;
            this.cb_AssambleFlag.Location = new System.Drawing.Point(162, 165);
            this.cb_AssambleFlag.Name = "cb_AssambleFlag";
            this.cb_AssambleFlag.Size = new System.Drawing.Size(130, 20);
            this.cb_AssambleFlag.TabIndex = 101;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(29)))), ((int)(((byte)(132)))));
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(298, 166);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(153, 18);
            this.label10.TabIndex = 102;
            this.label10.Text = "Assamble Result";
            // 
            // cb_AssambleResult
            // 
            this.cb_AssambleResult.FormattingEnabled = true;
            this.cb_AssambleResult.Location = new System.Drawing.Point(457, 165);
            this.cb_AssambleResult.Name = "cb_AssambleResult";
            this.cb_AssambleResult.Size = new System.Drawing.Size(130, 20);
            this.cb_AssambleResult.TabIndex = 99;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(29)))), ((int)(((byte)(132)))));
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label14.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(3, 5);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(153, 18);
            this.label14.TabIndex = 32;
            this.label14.Text = "ID";
            // 
            // cv_TxGlassId
            // 
            this.cv_TxGlassId.Location = new System.Drawing.Point(162, 3);
            this.cv_TxGlassId.MaxLength = 20;
            this.cv_TxGlassId.Name = "cv_TxGlassId";
            this.cv_TxGlassId.Size = new System.Drawing.Size(130, 22);
            this.cv_TxGlassId.TabIndex = 31;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(29)))), ((int)(((byte)(132)))));
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(298, 5);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(153, 18);
            this.label11.TabIndex = 34;
            this.label11.Text = "CIM Mode";
            // 
            // cv_CbCimMode
            // 
            this.cv_CbCimMode.FormattingEnabled = true;
            this.cv_CbCimMode.Location = new System.Drawing.Point(457, 3);
            this.cv_CbCimMode.Name = "cv_CbCimMode";
            this.cv_CbCimMode.Size = new System.Drawing.Size(130, 20);
            this.cv_CbCimMode.TabIndex = 33;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(29)))), ((int)(((byte)(132)))));
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(3, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(153, 18);
            this.label4.TabIndex = 35;
            this.label4.Text = "Foup Sequence";
            // 
            // cv_txFoupSeq
            // 
            this.cv_txFoupSeq.Location = new System.Drawing.Point(162, 31);
            this.cv_txFoupSeq.Name = "cv_txFoupSeq";
            this.cv_txFoupSeq.Size = new System.Drawing.Size(130, 22);
            this.cv_txFoupSeq.TabIndex = 36;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(29)))), ((int)(((byte)(132)))));
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label13.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(298, 32);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(153, 18);
            this.label13.TabIndex = 43;
            this.label13.Text = "Work Order";
            // 
            // txt_WorkOrder
            // 
            this.txt_WorkOrder.Location = new System.Drawing.Point(457, 31);
            this.txt_WorkOrder.MaxLength = 2;
            this.txt_WorkOrder.Name = "txt_WorkOrder";
            this.txt_WorkOrder.Size = new System.Drawing.Size(130, 22);
            this.txt_WorkOrder.TabIndex = 44;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(29)))), ((int)(((byte)(132)))));
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(3, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(153, 18);
            this.label5.TabIndex = 83;
            this.label5.Text = "Work Slot";
            // 
            // txt_WorkSlot
            // 
            this.txt_WorkSlot.Location = new System.Drawing.Point(162, 59);
            this.txt_WorkSlot.Name = "txt_WorkSlot";
            this.txt_WorkSlot.Size = new System.Drawing.Size(130, 22);
            this.txt_WorkSlot.TabIndex = 84;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(29)))), ((int)(((byte)(132)))));
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(298, 59);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(153, 18);
            this.label7.TabIndex = 85;
            this.label7.Text = "Work Type";
            // 
            // cb_WorkType
            // 
            this.cb_WorkType.FormattingEnabled = true;
            this.cb_WorkType.Location = new System.Drawing.Point(457, 59);
            this.cb_WorkType.Name = "cb_WorkType";
            this.cb_WorkType.Size = new System.Drawing.Size(130, 20);
            this.cb_WorkType.TabIndex = 86;
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(29)))), ((int)(((byte)(132)))));
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label16.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label16.ForeColor = System.Drawing.Color.White;
            this.label16.Location = new System.Drawing.Point(3, 86);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(153, 18);
            this.label16.TabIndex = 88;
            this.label16.Text = "Product";
            // 
            // cb_Production
            // 
            this.cb_Production.FormattingEnabled = true;
            this.cb_Production.Location = new System.Drawing.Point(162, 87);
            this.cb_Production.Name = "cb_Production";
            this.cb_Production.Size = new System.Drawing.Size(130, 20);
            this.cb_Production.TabIndex = 87;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(29)))), ((int)(((byte)(132)))));
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(298, 86);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(153, 18);
            this.label8.TabIndex = 89;
            this.label8.Text = "Judge";
            // 
            // cv_CmGlassjudge
            // 
            this.cv_CmGlassjudge.FormattingEnabled = true;
            this.cv_CmGlassjudge.Location = new System.Drawing.Point(457, 87);
            this.cv_CmGlassjudge.Name = "cv_CmGlassjudge";
            this.cv_CmGlassjudge.Size = new System.Drawing.Size(130, 20);
            this.cv_CmGlassjudge.TabIndex = 90;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(29)))), ((int)(((byte)(132)))));
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(3, 113);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(153, 18);
            this.label9.TabIndex = 91;
            this.label9.Text = "Process Flag";
            // 
            // cv_CbProcessFlag
            // 
            this.cv_CbProcessFlag.FormattingEnabled = true;
            this.cv_CbProcessFlag.Location = new System.Drawing.Point(162, 113);
            this.cv_CbProcessFlag.Name = "cv_CbProcessFlag";
            this.cv_CbProcessFlag.Size = new System.Drawing.Size(130, 20);
            this.cv_CbProcessFlag.TabIndex = 92;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(29)))), ((int)(((byte)(132)))));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(298, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 18);
            this.label1.TabIndex = 94;
            this.label1.Text = "Priority";
            // 
            // cb_Priority
            // 
            this.cb_Priority.FormattingEnabled = true;
            this.cb_Priority.Location = new System.Drawing.Point(457, 113);
            this.cb_Priority.Name = "cb_Priority";
            this.cb_Priority.Size = new System.Drawing.Size(130, 20);
            this.cb_Priority.TabIndex = 93;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(29)))), ((int)(((byte)(132)))));
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(153, 18);
            this.label2.TabIndex = 96;
            this.label2.Text = "OCR Read Flag";
            // 
            // cb_OcrReadFlag
            // 
            this.cb_OcrReadFlag.FormattingEnabled = true;
            this.cb_OcrReadFlag.Location = new System.Drawing.Point(162, 139);
            this.cb_OcrReadFlag.Name = "cb_OcrReadFlag";
            this.cb_OcrReadFlag.Size = new System.Drawing.Size(130, 20);
            this.cb_OcrReadFlag.TabIndex = 97;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(29)))), ((int)(((byte)(132)))));
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(298, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(153, 18);
            this.label3.TabIndex = 98;
            this.label3.Text = "OCR Read Result";
            // 
            // cb_OcrResult
            // 
            this.cb_OcrResult.FormattingEnabled = true;
            this.cb_OcrResult.Location = new System.Drawing.Point(457, 139);
            this.cb_OcrResult.Name = "cb_OcrResult";
            this.cb_OcrResult.Size = new System.Drawing.Size(130, 20);
            this.cb_OcrResult.TabIndex = 95;
            // 
            // pan_No
            // 
            this.pan_No.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan_No.Location = new System.Drawing.Point(0, 216);
            this.pan_No.Name = "pan_No";
            this.pan_No.Size = new System.Drawing.Size(594, 353);
            this.pan_No.TabIndex = 86;
            // 
            // cv_ColumnGlassId
            // 
            this.cv_ColumnGlassId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cv_ColumnGlassId.HeaderText = "Id";
            this.cv_ColumnGlassId.Name = "cv_ColumnGlassId";
            this.cv_ColumnGlassId.ReadOnly = true;
            // 
            // UiGlassDataOperator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(205)))), ((int)(((byte)(242)))));
            this.ClientSize = new System.Drawing.Size(903, 569);
            this.Controls.Add(this.pan_No);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.Name = "UiGlassDataOperator";
            this.Text = "GlassData";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UiGlassDataOperator_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.cv_TpRemove.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cv_DataGridRemove)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage cv_TpRemove;
        private System.Windows.Forms.DataGridView cv_DataGridRemove;
        private System.Windows.Forms.Button btn_modify;
        private System.Windows.Forms.Button btn_Request;
        private System.Windows.Forms.Button btn_remove;
        private System.Windows.Forms.Button btn_Add;
        private System.Windows.Forms.Panel flowLayoutPanel1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox cv_TxGlassId;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cv_CbCimMode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox cv_txFoupSeq;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txt_WorkOrder;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_WorkSlot;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cb_WorkType;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cb_Production;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cv_CmGlassjudge;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cv_CbProcessFlag;
        private System.Windows.Forms.Panel pan_No;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cb_Priority;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cb_OcrReadFlag;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cb_OcrResult;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cb_AssambleFlag;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cb_AssambleResult;
        private System.Windows.Forms.TextBox txt_Reason;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txt_PPID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cv_ColumnGlassId;

    }
}
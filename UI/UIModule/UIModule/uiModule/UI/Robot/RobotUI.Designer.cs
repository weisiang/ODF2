namespace UI.GUI
{
    partial class RobotUI
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RobotUI));
            this.gb_Robot = new System.Windows.Forms.GroupBox();
            this.picB_DownWafer = new System.Windows.Forms.PictureBox();
            this.cv_menuDataEdit = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dELETEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lbl_down = new System.Windows.Forms.Label();
            this.picB_UpWafer = new System.Windows.Forms.PictureBox();
            this.lbl_up = new System.Windows.Forms.Label();
            this.lbl_DownSensor = new System.Windows.Forms.Label();
            this.lbl_UpSensor = new System.Windows.Forms.Label();
            this.lbl_DownDataId = new System.Windows.Forms.Label();
            this.lbl_UpDataId = new System.Windows.Forms.Label();
            this.lbl_DownGlass = new System.Windows.Forms.Label();
            this.lbl_UpGlass = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_RobotStatus = new System.Windows.Forms.Label();
            this.gb_Robot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picB_DownWafer)).BeginInit();
            this.cv_menuDataEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picB_UpWafer)).BeginInit();
            this.SuspendLayout();
            // 
            // gb_Robot
            // 
            this.gb_Robot.Controls.Add(this.picB_DownWafer);
            this.gb_Robot.Controls.Add(this.lbl_down);
            this.gb_Robot.Controls.Add(this.picB_UpWafer);
            this.gb_Robot.Controls.Add(this.lbl_up);
            this.gb_Robot.Controls.Add(this.lbl_DownSensor);
            this.gb_Robot.Controls.Add(this.lbl_UpSensor);
            this.gb_Robot.Controls.Add(this.lbl_DownDataId);
            this.gb_Robot.Controls.Add(this.lbl_UpDataId);
            this.gb_Robot.Controls.Add(this.lbl_DownGlass);
            this.gb_Robot.Controls.Add(this.lbl_UpGlass);
            this.gb_Robot.Controls.Add(this.flowLayoutPanel2);
            this.gb_Robot.Controls.Add(this.flowLayoutPanel1);
            this.gb_Robot.Controls.Add(this.label1);
            this.gb_Robot.Controls.Add(this.lbl_RobotStatus);
            this.gb_Robot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb_Robot.Location = new System.Drawing.Point(0, 0);
            this.gb_Robot.Name = "gb_Robot";
            this.gb_Robot.Size = new System.Drawing.Size(272, 266);
            this.gb_Robot.TabIndex = 5;
            this.gb_Robot.TabStop = false;
            this.gb_Robot.Text = "groupBox1";
            // 
            // picB_DownWafer
            // 
            this.picB_DownWafer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picB_DownWafer.BackgroundImage")));
            this.picB_DownWafer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picB_DownWafer.ContextMenuStrip = this.cv_menuDataEdit;
            this.picB_DownWafer.Location = new System.Drawing.Point(130, 144);
            this.picB_DownWafer.Name = "picB_DownWafer";
            this.picB_DownWafer.Size = new System.Drawing.Size(80, 80);
            this.picB_DownWafer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picB_DownWafer.TabIndex = 22;
            this.picB_DownWafer.TabStop = false;
            // 
            // cv_menuDataEdit
            // 
            this.cv_menuDataEdit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dELETEToolStripMenuItem});
            this.cv_menuDataEdit.Name = "cv_menuDataEdit";
            this.cv_menuDataEdit.Size = new System.Drawing.Size(153, 48);
            // 
            // dELETEToolStripMenuItem
            // 
            this.dELETEToolStripMenuItem.Name = "dELETEToolStripMenuItem";
            this.dELETEToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.dELETEToolStripMenuItem.Text = "DataAction";
            // 
            // lbl_down
            // 
            this.lbl_down.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lbl_down.Location = new System.Drawing.Point(125, 139);
            this.lbl_down.Name = "lbl_down";
            this.lbl_down.Size = new System.Drawing.Size(90, 90);
            this.lbl_down.TabIndex = 21;
            this.lbl_down.Text = "label3";
            // 
            // picB_UpWafer
            // 
            this.picB_UpWafer.BackColor = System.Drawing.Color.Transparent;
            this.picB_UpWafer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picB_UpWafer.BackgroundImage")));
            this.picB_UpWafer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picB_UpWafer.ContextMenuStrip = this.cv_menuDataEdit;
            this.picB_UpWafer.Location = new System.Drawing.Point(130, 28);
            this.picB_UpWafer.Name = "picB_UpWafer";
            this.picB_UpWafer.Size = new System.Drawing.Size(80, 80);
            this.picB_UpWafer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picB_UpWafer.TabIndex = 20;
            this.picB_UpWafer.TabStop = false;
            // 
            // lbl_up
            // 
            this.lbl_up.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lbl_up.Location = new System.Drawing.Point(125, 23);
            this.lbl_up.Name = "lbl_up";
            this.lbl_up.Size = new System.Drawing.Size(90, 90);
            this.lbl_up.TabIndex = 19;
            this.lbl_up.Text = "label2";
            // 
            // lbl_DownSensor
            // 
            this.lbl_DownSensor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.lbl_DownSensor.Location = new System.Drawing.Point(242, 174);
            this.lbl_DownSensor.Name = "lbl_DownSensor";
            this.lbl_DownSensor.Size = new System.Drawing.Size(20, 20);
            this.lbl_DownSensor.TabIndex = 16;
            // 
            // lbl_UpSensor
            // 
            this.lbl_UpSensor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.lbl_UpSensor.Location = new System.Drawing.Point(242, 56);
            this.lbl_UpSensor.Name = "lbl_UpSensor";
            this.lbl_UpSensor.Size = new System.Drawing.Size(20, 20);
            this.lbl_UpSensor.TabIndex = 15;
            // 
            // lbl_DownDataId
            // 
            this.lbl_DownDataId.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            this.lbl_DownDataId.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_DownDataId.Location = new System.Drawing.Point(93, 229);
            this.lbl_DownDataId.Name = "lbl_DownDataId";
            this.lbl_DownDataId.Size = new System.Drawing.Size(139, 23);
            this.lbl_DownDataId.TabIndex = 13;
            this.lbl_DownDataId.Text = "DOWN";
            this.lbl_DownDataId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_UpDataId
            // 
            this.lbl_UpDataId.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(231)))));
            this.lbl_UpDataId.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_UpDataId.Location = new System.Drawing.Point(93, 111);
            this.lbl_UpDataId.Name = "lbl_UpDataId";
            this.lbl_UpDataId.Size = new System.Drawing.Size(139, 23);
            this.lbl_UpDataId.TabIndex = 12;
            this.lbl_UpDataId.Text = "UP";
            this.lbl_UpDataId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_DownGlass
            // 
            this.lbl_DownGlass.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbl_DownGlass.ContextMenuStrip = this.cv_menuDataEdit;
            this.lbl_DownGlass.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_DownGlass.Location = new System.Drawing.Point(73, 158);
            this.lbl_DownGlass.Name = "lbl_DownGlass";
            this.lbl_DownGlass.Size = new System.Drawing.Size(137, 57);
            this.lbl_DownGlass.TabIndex = 9;
            this.lbl_DownGlass.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_DownGlass.Visible = false;
            // 
            // lbl_UpGlass
            // 
            this.lbl_UpGlass.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbl_UpGlass.ContextMenuStrip = this.cv_menuDataEdit;
            this.lbl_UpGlass.Font = new System.Drawing.Font("新細明體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbl_UpGlass.Location = new System.Drawing.Point(73, 41);
            this.lbl_UpGlass.Name = "lbl_UpGlass";
            this.lbl_UpGlass.Size = new System.Drawing.Size(137, 57);
            this.lbl_UpGlass.TabIndex = 8;
            this.lbl_UpGlass.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_UpGlass.Visible = false;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(165)))), ((int)(((byte)(165)))));
            this.flowLayoutPanel2.Location = new System.Drawing.Point(52, 56);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(180, 20);
            this.flowLayoutPanel2.TabIndex = 7;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(165)))), ((int)(((byte)(165)))));
            this.flowLayoutPanel1.Location = new System.Drawing.Point(28, 28);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(23, 204);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(165)))), ((int)(((byte)(165)))));
            this.label1.Location = new System.Drawing.Point(52, 174);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 20);
            this.label1.TabIndex = 5;
            // 
            // lbl_RobotStatus
            // 
            this.lbl_RobotStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lbl_RobotStatus.Location = new System.Drawing.Point(26, 22);
            this.lbl_RobotStatus.Name = "lbl_RobotStatus";
            this.lbl_RobotStatus.Size = new System.Drawing.Size(210, 210);
            this.lbl_RobotStatus.TabIndex = 14;
            // 
            // RobotUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gb_Robot);
            this.Name = "RobotUI";
            this.Size = new System.Drawing.Size(272, 266);
            this.gb_Robot.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picB_DownWafer)).EndInit();
            this.cv_menuDataEdit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picB_UpWafer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_Robot;
        private System.Windows.Forms.Label lbl_DownGlass;
        private System.Windows.Forms.Label lbl_UpGlass;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_DownDataId;
        private System.Windows.Forms.Label lbl_UpDataId;
        private System.Windows.Forms.Label lbl_RobotStatus;
        private System.Windows.Forms.ContextMenuStrip cv_menuDataEdit;
        private System.Windows.Forms.ToolStripMenuItem dELETEToolStripMenuItem;
        private System.Windows.Forms.Label lbl_DownSensor;
        private System.Windows.Forms.Label lbl_UpSensor;
        private System.Windows.Forms.PictureBox picB_UpWafer;
        private System.Windows.Forms.Label lbl_up;
        private System.Windows.Forms.PictureBox picB_DownWafer;
        private System.Windows.Forms.Label lbl_down;
    }
}

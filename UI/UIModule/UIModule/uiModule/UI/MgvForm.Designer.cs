namespace UI
{
    partial class MgvForm
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
            this.cv_BtRequest = new System.Windows.Forms.Button();
            this.cv_BtSave = new System.Windows.Forms.Button();
            this.cv_TbOpid = new System.Windows.Forms.TextBox();
            this.cv_TbCstId = new System.Windows.Forms.TextBox();
            this.cv_LbOpid = new System.Windows.Forms.Label();
            this.cv_LbCstId = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cv_BtRequest
            // 
            this.cv_BtRequest.Location = new System.Drawing.Point(190, 21);
            this.cv_BtRequest.Name = "cv_BtRequest";
            this.cv_BtRequest.Size = new System.Drawing.Size(75, 23);
            this.cv_BtRequest.TabIndex = 9;
            this.cv_BtRequest.Text = "Request Data";
            this.cv_BtRequest.UseVisualStyleBackColor = true;
            this.cv_BtRequest.Click += new System.EventHandler(this.cv_BtRequest_Click);
            // 
            // cv_BtSave
            // 
            this.cv_BtSave.Location = new System.Drawing.Point(41, 21);
            this.cv_BtSave.Name = "cv_BtSave";
            this.cv_BtSave.Size = new System.Drawing.Size(75, 23);
            this.cv_BtSave.TabIndex = 8;
            this.cv_BtSave.Text = "Save";
            this.cv_BtSave.UseVisualStyleBackColor = true;
            this.cv_BtSave.Click += new System.EventHandler(this.cv_BtSave_Click);
            // 
            // cv_TbOpid
            // 
            this.cv_TbOpid.Location = new System.Drawing.Point(121, 130);
            this.cv_TbOpid.Name = "cv_TbOpid";
            this.cv_TbOpid.Size = new System.Drawing.Size(208, 22);
            this.cv_TbOpid.TabIndex = 6;
            // 
            // cv_TbCstId
            // 
            this.cv_TbCstId.Location = new System.Drawing.Point(121, 76);
            this.cv_TbCstId.Name = "cv_TbCstId";
            this.cv_TbCstId.Size = new System.Drawing.Size(208, 22);
            this.cv_TbCstId.TabIndex = 7;
            // 
            // cv_LbOpid
            // 
            this.cv_LbOpid.AutoSize = true;
            this.cv_LbOpid.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cv_LbOpid.Location = new System.Drawing.Point(41, 133);
            this.cv_LbOpid.Name = "cv_LbOpid";
            this.cv_LbOpid.Size = new System.Drawing.Size(51, 19);
            this.cv_LbOpid.TabIndex = 4;
            this.cv_LbOpid.Text = "OPID";
            // 
            // cv_LbCstId
            // 
            this.cv_LbCstId.AutoSize = true;
            this.cv_LbCstId.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cv_LbCstId.Location = new System.Drawing.Point(41, 79);
            this.cv_LbCstId.Name = "cv_LbCstId";
            this.cv_LbCstId.Size = new System.Drawing.Size(53, 19);
            this.cv_LbCstId.TabIndex = 5;
            this.cv_LbCstId.Text = "Cst Id";
            // 
            // MgvForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 205);
            this.Controls.Add(this.cv_BtRequest);
            this.Controls.Add(this.cv_BtSave);
            this.Controls.Add(this.cv_TbOpid);
            this.Controls.Add(this.cv_TbCstId);
            this.Controls.Add(this.cv_LbOpid);
            this.Controls.Add(this.cv_LbCstId);
            this.Name = "MgvForm";
            this.Text = "MgvForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cv_BtRequest;
        private System.Windows.Forms.Button cv_BtSave;
        private System.Windows.Forms.TextBox cv_TbOpid;
        private System.Windows.Forms.TextBox cv_TbCstId;
        private System.Windows.Forms.Label cv_LbOpid;
        private System.Windows.Forms.Label cv_LbCstId;
    }
}
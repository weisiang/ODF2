namespace UI
{
    partial class Login
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
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.cv_lblId = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.cv_BtnLogin = new MaterialSkin.Controls.MaterialRaisedButton();
            this.materialDivider1 = new MaterialSkin.Controls.MaterialDivider();
            this.cv_lblpw = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.cv_BtnLogout = new MaterialSkin.Controls.MaterialRaisedButton();
            this.lbl_Permission = new MaterialSkin.Controls.MaterialLabel();
            this.cb_Permission = new System.Windows.Forms.ComboBox();
            this.cv_BtnSignUp = new System.Windows.Forms.Button();
            this.cv_BtnDel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // materialLabel1
            // 
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel1.Location = new System.Drawing.Point(18, 95);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(31, 19);
            this.materialLabel1.TabIndex = 16;
            this.materialLabel1.Text = "ID";
            // 
            // materialLabel2
            // 
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel2.Location = new System.Drawing.Point(18, 133);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(31, 19);
            this.materialLabel2.TabIndex = 14;
            this.materialLabel2.Text = "PW";
            // 
            // cv_lblId
            // 
            this.cv_lblId.BackColor = System.Drawing.SystemColors.Control;
            this.cv_lblId.Depth = 0;
            this.cv_lblId.Hint = "";
            this.cv_lblId.Location = new System.Drawing.Point(55, 91);
            this.cv_lblId.MouseState = MaterialSkin.MouseState.HOVER;
            this.cv_lblId.Name = "cv_lblId";
            this.cv_lblId.PasswordChar = '\0';
            this.cv_lblId.SelectedText = "";
            this.cv_lblId.SelectionLength = 0;
            this.cv_lblId.SelectionStart = 0;
            this.cv_lblId.Size = new System.Drawing.Size(192, 23);
            this.cv_lblId.TabIndex = 13;
            this.cv_lblId.UseSystemPasswordChar = false;
            // 
            // cv_BtnLogin
            // 
            this.cv_BtnLogin.Depth = 0;
            this.cv_BtnLogin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.cv_BtnLogin.Location = new System.Drawing.Point(172, 241);
            this.cv_BtnLogin.MouseState = MaterialSkin.MouseState.HOVER;
            this.cv_BtnLogin.Name = "cv_BtnLogin";
            this.cv_BtnLogin.Primary = true;
            this.cv_BtnLogin.Size = new System.Drawing.Size(75, 31);
            this.cv_BtnLogin.TabIndex = 18;
            this.cv_BtnLogin.Text = "Log in";
            this.cv_BtnLogin.UseVisualStyleBackColor = true;
            this.cv_BtnLogin.Click += new System.EventHandler(this.cv_BtnLogin_Click);
            // 
            // materialDivider1
            // 
            this.materialDivider1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialDivider1.Depth = 0;
            this.materialDivider1.Location = new System.Drawing.Point(22, 278);
            this.materialDivider1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialDivider1.Name = "materialDivider1";
            this.materialDivider1.Size = new System.Drawing.Size(225, 1);
            this.materialDivider1.TabIndex = 19;
            this.materialDivider1.Text = "materialDivider1";
            // 
            // cv_lblpw
            // 
            this.cv_lblpw.Depth = 0;
            this.cv_lblpw.Hint = "";
            this.cv_lblpw.Location = new System.Drawing.Point(55, 133);
            this.cv_lblpw.MouseState = MaterialSkin.MouseState.HOVER;
            this.cv_lblpw.Name = "cv_lblpw";
            this.cv_lblpw.PasswordChar = '*';
            this.cv_lblpw.SelectedText = "";
            this.cv_lblpw.SelectionLength = 0;
            this.cv_lblpw.SelectionStart = 0;
            this.cv_lblpw.Size = new System.Drawing.Size(192, 23);
            this.cv_lblpw.TabIndex = 15;
            this.cv_lblpw.UseSystemPasswordChar = false;
            // 
            // cv_BtnLogout
            // 
            this.cv_BtnLogout.Depth = 0;
            this.cv_BtnLogout.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.cv_BtnLogout.Location = new System.Drawing.Point(22, 241);
            this.cv_BtnLogout.MouseState = MaterialSkin.MouseState.HOVER;
            this.cv_BtnLogout.Name = "cv_BtnLogout";
            this.cv_BtnLogout.Primary = true;
            this.cv_BtnLogout.Size = new System.Drawing.Size(75, 31);
            this.cv_BtnLogout.TabIndex = 21;
            this.cv_BtnLogout.Text = "Log out";
            this.cv_BtnLogout.UseVisualStyleBackColor = true;
            this.cv_BtnLogout.Click += new System.EventHandler(this.cv_BtnLogout_Click);
            // 
            // lbl_Permission
            // 
            this.lbl_Permission.Depth = 0;
            this.lbl_Permission.Font = new System.Drawing.Font("Roboto", 11F);
            this.lbl_Permission.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_Permission.Location = new System.Drawing.Point(18, 169);
            this.lbl_Permission.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbl_Permission.Name = "lbl_Permission";
            this.lbl_Permission.Size = new System.Drawing.Size(89, 19);
            this.lbl_Permission.TabIndex = 23;
            this.lbl_Permission.Text = "Permission";
            this.lbl_Permission.Visible = false;
            // 
            // cb_Permission
            // 
            this.cb_Permission.BackColor = System.Drawing.SystemColors.Control;
            this.cb_Permission.Enabled = false;
            this.cb_Permission.FormattingEnabled = true;
            this.cb_Permission.Location = new System.Drawing.Point(126, 169);
            this.cb_Permission.Name = "cb_Permission";
            this.cb_Permission.Size = new System.Drawing.Size(121, 20);
            this.cb_Permission.TabIndex = 24;
            this.cb_Permission.Visible = false;
            // 
            // cv_BtnSignUp
            // 
            this.cv_BtnSignUp.Location = new System.Drawing.Point(91, 306);
            this.cv_BtnSignUp.Name = "cv_BtnSignUp";
            this.cv_BtnSignUp.Size = new System.Drawing.Size(75, 23);
            this.cv_BtnSignUp.TabIndex = 26;
            this.cv_BtnSignUp.Text = "Sign up";
            this.cv_BtnSignUp.UseVisualStyleBackColor = true;
            this.cv_BtnSignUp.Click += new System.EventHandler(this.cv_BtnSignUp_Click);
            // 
            // cv_BtnDel
            // 
            this.cv_BtnDel.Location = new System.Drawing.Point(172, 306);
            this.cv_BtnDel.Name = "cv_BtnDel";
            this.cv_BtnDel.Size = new System.Drawing.Size(75, 23);
            this.cv_BtnDel.TabIndex = 27;
            this.cv_BtnDel.Text = "Delete";
            this.cv_BtnDel.UseVisualStyleBackColor = true;
            this.cv_BtnDel.Click += new System.EventHandler(this.cv_BtnDelete_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 341);
            this.Controls.Add(this.cv_BtnDel);
            this.Controls.Add(this.cv_BtnSignUp);
            this.Controls.Add(this.cb_Permission);
            this.Controls.Add(this.lbl_Permission);
            this.Controls.Add(this.cv_BtnLogout);
            this.Controls.Add(this.materialDivider1);
            this.Controls.Add(this.cv_BtnLogin);
            this.Controls.Add(this.materialLabel1);
            this.Controls.Add(this.cv_lblpw);
            this.Controls.Add(this.materialLabel2);
            this.Controls.Add(this.cv_lblId);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Login";
            this.Sizable = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "                         Login";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Login_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private MaterialSkin.Controls.MaterialSingleLineTextField cv_lblId;
        private MaterialSkin.Controls.MaterialRaisedButton cv_BtnLogin;
        private MaterialSkin.Controls.MaterialDivider materialDivider1;
        private MaterialSkin.Controls.MaterialSingleLineTextField cv_lblpw;
        private MaterialSkin.Controls.MaterialRaisedButton cv_BtnLogout;
        private MaterialSkin.Controls.MaterialLabel lbl_Permission;
        private System.Windows.Forms.ComboBox cb_Permission;
        private System.Windows.Forms.Button cv_BtnSignUp;
        private System.Windows.Forms.Button cv_BtnDel;
    }
}
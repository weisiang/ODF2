using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using BaseAp;

namespace UI
{
    public partial class Login : MaterialSkin.Controls.MaterialForm
    {
        public Login()
        {
            InitializeComponent();

            var materialSkin = MaterialSkinManager.Instance;
            materialSkin.AddFormToManage(this);
            materialSkin.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkin.ColorScheme = new ColorScheme(Primary.Blue700, Primary.BlueGrey900, Primary.Blue500, Accent.Orange700, TextShade.BLACK);

            IniPermissionCombox();
            EnablePerssmionObj(false);
            UiForm.AddUiObjToEnableList(cv_BtnSignUp, UiForm.enumGroup.Group6);
            UiForm.AddUiObjToEnableList(cv_BtnDel, UiForm.enumGroup.Group6); 
        }
        private void EnablePerssmionObj(bool m_IsEnable)
        {
            if (m_IsEnable)
            {
                cb_Permission.Visible = true;
                lbl_Permission.Visible = true;
                cb_Permission.Enabled = true;
            }
            else
            {
                cb_Permission.Visible = false;
                lbl_Permission.Visible = false;
                cb_Permission.Enabled = false;
            }
        }
        private void IniPermissionCombox()
        {//cb_ManualApi.Items.AddRange(Enum.GetNames(typeof(APIEnum.APICommand)).ToArray<string>());
            string[] names = Enum.GetNames(typeof(CommonData.HIRATA.UserPermission)).ToArray<string>();
            for(int i=0 ; i< names.Length ; i++)
            {
                if(!Regex.Match(names[i] , @"none" , RegexOptions.IgnoreCase).Success)
                {
                    cb_Permission.Items.Add(names[i]);
                }
            }
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            ClearUI();
            this.Hide();
        }
        public void ClearUI()
        {
            cv_lblId.Text = "";
            cv_lblpw.Text = "";
            cb_Permission.SelectedIndex = -1;
            EnablePerssmionObj(false);
        }

        private void cv_BtnLogin_Click(object sender, EventArgs e)
        {
            string id = cv_lblId.Text.Trim();
            string pw = cv_lblpw.Text.Trim();
            CommonData.HIRATA.LoginResult result =  UiForm.cv_AccountData.Login(id, pw);
            if(result != CommonData.HIRATA.LoginResult.Successful)
            {
                CommonStaticData.PopForm( result.ToString(), true, false);
            }
            else
            {
                this.Hide();
            }
        }

        private void cv_BtnLogout_Click(object sender, EventArgs e)
        {
            string id = cv_lblId.Text.Trim();
            string pw = cv_lblpw.Text.Trim();
            CommonData.HIRATA.LoginResult result = UiForm.cv_AccountData.Logout(id, pw);
            if (result != CommonData.HIRATA.LoginResult.Successful)
            {
                CommonStaticData.PopForm(result.ToString(), true, false);
            }
            else
            {
                this.Hide();
            }
        }

        private void cv_BtnSignUp_Click(object sender, EventArgs e)
        {
            if (!CheckPermission(CommonData.HIRATA.UserPermission.Root))
            {
                CommonStaticData.PopForm("Please log in Root permission", true, false);
                return;
            }
            if(!cb_Permission.Visible)
            {
                EnablePerssmionObj(true);
            }
            else
            {
                string id = cv_lblId.Text.Trim();
                string pw = cv_lblpw.Text.Trim();
                string permission = cb_Permission.Text.Trim();
                if(string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw) || string.IsNullOrEmpty(permission))
                {
                    CommonStaticData.PopForm("Please fill all data", true, false);
                    return;
                }
                CommonData.HIRATA.UserPermission tmp_permission = (CommonData.HIRATA.UserPermission)Enum.Parse(typeof(CommonData.HIRATA.UserPermission), permission);
                CommonData.HIRATA.LoginResult result = UiForm.cv_AccountData.Register(id, pw, (int)tmp_permission);
                if (result !=CommonData.HIRATA.LoginResult.Successful)
                {
                    CommonStaticData.PopForm(result.ToString(), true, false);
                }
                else
                {
                    EnablePerssmionObj(false);
                    this.Hide();

                }
            }
        }

        private void cv_BtnDelete_Click(object sender, EventArgs e)
        {
            if (!CheckPermission(CommonData.HIRATA.UserPermission.Root))
            {
                CommonStaticData.PopForm("Please log in Root permission", true, false);
                return;
            }
            string id = cv_lblId.Text.Trim();
            if (string.IsNullOrEmpty(id))
            {
                CommonStaticData.PopForm("Please fill id", true, false);
                return;
            }
            CommonData.HIRATA.LoginResult tmp = CommonData.HIRATA.LoginResult.None;
            tmp = UiForm.cv_AccountData.Delete(id);
            if (tmp != CommonData.HIRATA.LoginResult.Successful)
            {
                CommonStaticData.PopForm(tmp.ToString() , true, false);
            }
            else
            {
                this.Hide();
            }
        }
        private bool CheckPermission(CommonData.HIRATA.UserPermission m_Permission)
        {
            bool rtn = false;
            CommonData.HIRATA.AccountItem tmp = null;
            if(UiForm.cv_AccountData.GetCurAccount(out tmp))
            {
                if(tmp.PPermission == m_Permission)
                {
                    rtn = true;
                }
            }
            return rtn;
        }
    }
}

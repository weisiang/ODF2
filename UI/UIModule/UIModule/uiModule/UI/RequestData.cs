using System;
using KgsCommon;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonData.HIRATA;
using BaseAp;

namespace UI
{
    public partial class RequestData : Form
    {
        CommonData.HIRATA.Source cv_Source = null;
        public RequestData()
        {
            InitializeComponent();
        }

        private void RequestData_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void cv_BtExe_Click(object sender, EventArgs e)
        {
            AskData("EXE");
        }

        private void AskData(string M_BTN)
        {
            string log = "";
            string user_log = "[ User push Request Glass Data UI : " + M_BTN + " button ] \n";
            if ((cv_Check1.Checked) && (cv_Check2.Checked))
            {
                CommonStaticData.PopForm("Please choice one method only" , true , false);
                return;
            }
            else if (!(cv_Check1.Checked) && !(cv_Check2.Checked))
            {
                CommonStaticData.PopForm("Please choice one method only" , true , false);
                return;
            }

            string opid = "", glassid = "", foup_seq = "", slot_no = "" , cim , order="";
            CommonData.HIRATA.AccountItem cur_account = null;
            if (UiForm.cv_AccountData.GetCurAccount(out cur_account))
            {
                opid = cur_account.PId;
            }
            else
            {
                opid = "";
            }

            glassid = cv_TxGlass.Text.Trim();
            foup_seq = cv_TxFoupSeq.Text.Trim();
            slot_no = cv_TxSlotNo.Text.Trim();
            cim = cb_Cim.Text.Trim();
            order = txt_Order.Text.Trim();

            if (cv_Check1.Checked)
            {
                if (string.IsNullOrEmpty(glassid))
                {
                    CommonStaticData.PopForm("Please key-in Glass ID", true, false);
                    return;
                }
                else
                {
                    CommonData.HIRATA.MDBCDataRequest tmp_data = new CommonData.HIRATA.MDBCDataRequest();
                    tmp_data.PRequestKey = CommonData.HIRATA.DataRequestKey.ByWorkId;
                    tmp_data.PWorkId = glassid;
                    tmp_data.Source = cv_Source;
                    string rtn;
                    object tmp = null;
                    uint ticket;
                    /*
                    if (!Global.Controller.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDBCDataRequest).Name, tmp_data, out ticket, out rtn, out tmp, 3000 ,
                        KParseObjToXmlPropertyType.Field))
                    {
                        CommonStaticData.PopForm("LGC time out...", true, false);
                        log += "[Time Out]Wait : " + typeof(CommonData.HIRATA.MDBCDataRequest).Name;
                    }
                    else
                    {
                        CommonStaticData.PopForm("Request data processing...", true, false);
                    }
                    */
                }
            }
            else if (cv_Check2.Checked)
            {
                if (string.IsNullOrEmpty(foup_seq))
                {
                    CommonStaticData.PopForm("Please key-in Foup Seq" , true , false);
                    return;
                }
                else if (string.IsNullOrEmpty(slot_no))
                {
                    CommonStaticData.PopForm("Please key-in slot no" , true , false);
                    return;
                }
                else if (string.IsNullOrEmpty(cim))
                {
                    CommonStaticData.PopForm("Please select CIM mode" , true , false);
                    return;
                }
                else if (string.IsNullOrEmpty(order))
                {
                    CommonStaticData.PopForm("Please key-in Order No" , true , false);
                    return;
                }
                else
                {
                    CommonData.HIRATA.MDBCDataRequest tmp_data = new CommonData.HIRATA.MDBCDataRequest();
                    tmp_data.PRequestKey = CommonData.HIRATA.DataRequestKey.ByWorkNo;
                    tmp_data.Source = cv_Source;
                    tmp_data.PWorkdSlot = Convert.ToInt32(slot_no);
                    tmp_data.PWorkOrderNo = Convert.ToInt32(order);
                    tmp_data.PFoupSeq = Convert.ToInt32(foup_seq);
                    tmp_data.PCimMode = cb_Cim.SelectedIndex == 0 ? CommonData.HIRATA.OnlineMode.Offline : CommonData.HIRATA.OnlineMode.Control;
                    string rtn;
                    object tmp = null;
                    uint ticket;
                    /*
                    if (!Global.Controller.SendMmfRequestObjectTimeout(typeof(CommonData.HIRATA.MDBCDataRequest).Name, tmp_data, out ticket, out rtn, out tmp, 3000 
                        , KParseObjToXmlPropertyType.Field))
                    {
                        log += "[Time Out]Wait : " + typeof(CommonData.HIRATA.MDBCDataRequest).Name;
                        CommonStaticData.PopForm("LGC time out...", true, false);
                    }
                    else
                    {
                        CommonStaticData.PopForm("Request data processing...", true, false);
                    }
                    */
                }
            }
            if (!string.IsNullOrEmpty(log))
            {
                UiForm.WriteLog(LogLevelType.General, log);
            }
        }

        private void cv_BtRetry_Click(object sender, EventArgs e)
        {
            AskData("Retry");
        }

        private void ClearForm()
        {
            cv_Check1.Checked = false;
            cv_Check2.Checked = false;
            cb_Cim.SelectedIndex = -1;
            txt_Order.Text = "";
            cv_TxGlass.Text = "";
            cv_TxSlotNo.Text = "";
            cv_TxFoupSeq.Text = "";
        }
        public void Show(CommonData.HIRATA.Source m_Source)
        {
            cv_Source = m_Source;
            if (!this.Visible)
            {
                ClearForm();
                base.Show();
            }
            else
            {
                this.Focus();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaseAp;


namespace UI
{
    public partial class TimrOutForm : Form
    {
        public TimrOutForm()
        {
            InitializeComponent();
        }
        public new void Show()
        {
            //WriteNormalIn
            /*
            cv_TbIDTime.Text = (Convert.ToDouble( UiForm.PSystemData.PIdleDelayTime) / 1000 ).ToString();
            txt_Interval.Text = (Convert.ToDouble(UiForm.PSystemData.PIntervalTime) / 1000).ToString();
            cv_TbHST0.Text = (Convert.ToDouble(UiForm.PSystemData.PT0Time) / 1000).ToString();
            cv_TbHST1.Text = (Convert.ToDouble(UiForm.PSystemData.PT1Time) / 1000).ToString();
            cv_TbHST3.Text = (Convert.ToDouble(UiForm.PSystemData.PT3Time) / 1000).ToString();
            cv_TbIFTs.Text = (Convert.ToDouble(UiForm.PSystemData.PTsTime) / 1000).ToString();
            cv_TbIFTe.Text = (Convert.ToDouble(UiForm.PSystemData.PTeTime) / 1000).ToString();
            txt_ApiT3.Text = (Convert.ToDouble(UiForm.PSystemData.PApiT3TIme) / 1000).ToString();
            cv_TbIFTm.Text = (Convert.ToDouble(UiForm.PSystemData.PTmTime) / 1000).ToString();
            base.Show();
            */
            cv_TbIDTime.Text = (Convert.ToDouble(lgcBase.cv_TimeoutData.PIdleDelayTime) / 1000).ToString();
            txt_Interval.Text = (Convert.ToDouble(lgcBase.cv_TimeoutData.PIntervalTime) / 1000).ToString();
            cv_TbHST0.Text = (Convert.ToDouble(lgcBase.cv_TimeoutData.PT0Time) / 1000).ToString();
            cv_TbHST1.Text = (Convert.ToDouble(lgcBase.cv_TimeoutData.PT1Time) / 1000).ToString();
            cv_TbHST3.Text = (Convert.ToDouble(lgcBase.cv_TimeoutData.PT3Time) / 1000).ToString();
            cv_TbIFTs.Text = (Convert.ToDouble(lgcBase.cv_TimeoutData.PTsTime) / 1000).ToString();
            cv_TbIFTe.Text = (Convert.ToDouble(lgcBase.cv_TimeoutData.PTeTime) / 1000).ToString();
            txt_ApiT3.Text = (Convert.ToDouble(lgcBase.cv_TimeoutData.PApiT3TIme) / 1000).ToString();
            cv_TbIFTm.Text = (Convert.ToDouble(lgcBase.cv_TimeoutData.PTmTime) / 1000).ToString();
            base.Show();

            //WriteNormalOut
        }
        public void Set()
        {
            //WriteNormalIn
            cv_TbIDTime.Text = (Convert.ToDouble(lgcBase.cv_TimeoutData.PIdleDelayTime) / 1000).ToString();
            txt_Interval.Text = (Convert.ToDouble(lgcBase.cv_TimeoutData.PIntervalTime) / 1000).ToString();
            cv_TbHST0.Text = (Convert.ToDouble(lgcBase.cv_TimeoutData.PT0Time) / 1000).ToString();
            cv_TbHST1.Text = (Convert.ToDouble(lgcBase.cv_TimeoutData.PT1Time) / 1000).ToString();
            cv_TbHST3.Text = (Convert.ToDouble(lgcBase.cv_TimeoutData.PT3Time) / 1000).ToString();
            cv_TbIFTs.Text = (Convert.ToDouble(lgcBase.cv_TimeoutData.PTsTime) / 1000).ToString();
            cv_TbIFTe.Text = (Convert.ToDouble(lgcBase.cv_TimeoutData.PTeTime) / 1000).ToString();
            txt_ApiT3.Text = (Convert.ToDouble(lgcBase.cv_TimeoutData.PApiT3TIme) / 1000).ToString();
            cv_TbIFTm.Text = (Convert.ToDouble(lgcBase.cv_TimeoutData.PTmTime) / 1000).ToString();
            //WriteNormalOut
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //WriteNormalIn
            SetTimeoutDataReq();
            //WriteNormalOut
        }
        private bool CheckInput()
        {
            //WriteNormalIn
            bool rtn = true;
            float tmp;
            if(!float.TryParse(cv_TbIDTime.Text.Trim() , out tmp))
            {
                rtn = false;
            }
            if (!float.TryParse(txt_Interval.Text.Trim(), out tmp))
            {
                rtn = false;
            }
            if (!float.TryParse(cv_TbHST0.Text.Trim(), out tmp))
            {
                rtn = false;
            }
            if (!float.TryParse(cv_TbHST1.Text.Trim(), out tmp))
            {
                rtn = false;
            }
            if (!float.TryParse(cv_TbHST3.Text.Trim(), out tmp))
            {
                rtn = false;
            }
            if (!float.TryParse(cv_TbIFTs.Text.Trim(), out tmp))
            {
                rtn = false;
            }
            if (!float.TryParse(cv_TbIFTe.Text.Trim(), out tmp))
            {
                rtn = false;
            }
            if (!float.TryParse(txt_ApiT3.Text.Trim(), out tmp))
            {
                rtn = false;
            }
            if (!float.TryParse(cv_TbIFTm.Text.Trim(), out tmp))
            {
                rtn = false;
            }
            //WriteNormalOut
            return rtn;
        }
        private void TimrOutForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        private void cv_TimeoutDefault_Click(object sender, EventArgs e)
        {
            cv_TbIDTime.Text = "10";
            txt_Interval.Text = "1" ;
            cv_TbHST0.Text = "4";
            cv_TbHST1.Text = "5";
            cv_TbHST3.Text = "10";
            cv_TbIFTs.Text = "4";
            cv_TbIFTe.Text = "4";
            txt_ApiT3.Text = "60";
            cv_TbIFTm.Text = "0.2";
            SetTimeoutDataReq();
        }
        private void SetTimeoutDataReq()
        {
            if (!CheckInput())
            {
                CommonStaticData.PopForm(" Input invalid , please check", true, false);
                return;
            }
            string log = "";
            int idle = Convert.ToInt32(Convert.ToDouble(cv_TbIDTime.Text.Trim()) * 1000);
            int interval = Convert.ToInt32(Convert.ToDouble(txt_Interval.Text.Trim()) * 1000);
            int t0 = Convert.ToInt32(Convert.ToDouble(cv_TbHST0.Text.Trim()) * 1000);
            int t1 = Convert.ToInt32(Convert.ToDouble(cv_TbHST1.Text.Trim()) * 1000);
            int t3 = Convert.ToInt32(Convert.ToDouble(cv_TbHST3.Text.Trim()) * 1000);
            int ts = Convert.ToInt32(Convert.ToDouble(cv_TbIFTs.Text.Trim()) * 1000);
            int te = Convert.ToInt32(Convert.ToDouble(cv_TbIFTe.Text.Trim()) * 1000);
            int api_t3 = Convert.ToInt32(Convert.ToDouble(txt_ApiT3.Text.Trim()) * 1000);
            int tm = Convert.ToInt32(Convert.ToDouble(cv_TbIFTm.Text.Trim()) * 1000);
            CommonData.HIRATA.MDSetTimeOut obj = new CommonData.HIRATA.MDSetTimeOut();
            obj.PIdleTIme = idle;
            obj.PIntervalTIme = interval;
            obj.PT0TIme = t0;
            obj.PT1TIme = t1;
            obj.PT3TIme = t3;
            obj.PTsTIme = ts;
            obj.PTeTIme = te;
            obj.PTmTIme = tm;
            obj.PApiT3TIme = api_t3;

            /*
            if (!UiForm.cv_mmfController.SendTimeOutSetting(CommonData.HIRATA.MmfEventClientEventType.etRequest, obj, true))
            {
                log += "[Time Out]Wait : " + typeof(CommonData.HIRATA.MDSetTimeOut).Name;
            }
            */
        }
    }
}

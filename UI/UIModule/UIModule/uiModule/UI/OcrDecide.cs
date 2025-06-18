using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonData.HIRATA;

namespace UI
{
    public partial class OcrDecide : Form
    {
        public OcrDecide()
        {
            InitializeComponent();
        }

        private void btn_Keyin_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txt_keyin.Text))
            {
                CommonStaticData.PopForm("Please key in new Id.", true, false);
                return;
            }
            //UiForm.cv_mmfController.SendShowOcrDecideReply(OCRMode.SkipRead, txt_keyin.Text.Trim());
            this.Hide();
        }

        private void btn_Hold_Click(object sender, EventArgs e)
        {
            //UiForm.cv_mmfController.SendShowOcrDecideReply(OCRMode.ErrorHold);
        }

        private void btn_Return_Click(object sender, EventArgs e)
        {
            Aligner aligner = UiForm.GetAligner(1);
            Port port = UiForm.GetPort((int)aligner.cv_Data.GlassDataMap[1].PSourcePort);
            int slot = 0;
            if (port.cv_Data.PPortStatus == PortStaus.LDCM)
            {
                if (port.cv_Data.WhichSlotCanLoad(out slot))
                {
                    //UiForm.cv_mmfController.SendShowOcrDecideReply(OCRMode.ErrorReturn);
                    Hide();
                }
                else
                {
                    CommonStaticData.PopForm("Port : " + port.cv_Id + " can't find slot to put", true, false);
                }
            }
            else
            {
                CommonStaticData.PopForm("Port : " + port.cv_Id + " status not at LDCM", true, false);
            }
        }

        private void btn_Skip_Click(object sender, EventArgs e)
        {
            //UiForm.cv_mmfController.SendShowOcrDecideReply(OCRMode.ErrorSkip);
            this.Hide();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using KgsCommon;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class UiDownLog : Form
    {
        private string cv_FilePath = System.IO.Directory.GetCurrentDirectory() + "\\..\\Config\\IFWithDownStream.xml";
        private KXmlItem cv_RecordXml = null;
        private delegate void cv_DeleUpdateUi(int Value);
        private Dictionary<int, Label> cv_DownHandler = new Dictionary<int, Label>();
        private Dictionary<int, Label> cv_DownPostCln = new Dictionary<int, Label>();
        private delegate void cv_deleUpdateDownUi(int m_Value);
        private Queue<JobData> cv_LogQueue = new Queue<JobData>();
        private List<JobData> cv_CurrentLog = new List<JobData>();

        class JobData
        {
            public DateTime cv_DateTime = DateTime.Now;
            public cv_deleUpdateDownUi cv_HandlerFun = null;
            public cv_deleUpdateDownUi cv_OtherSideFun = null;
            public int cv_HandlerValue = 0;
            public int cv_OtherSideValue = 0;
            public JobData(int m_HandlerValue , int m_OtherSideValue , cv_deleUpdateDownUi m_HandlerFun , cv_deleUpdateDownUi m_OtherSideFun )
            {
                cv_HandlerFun = m_HandlerFun;
                cv_OtherSideFun = m_OtherSideFun;
                cv_HandlerValue = m_HandlerValue;
                cv_OtherSideValue = m_OtherSideValue;
            }
        }

        public UiDownLog()
        {
            InitializeComponent();
            AddAllLableToDic();
            JoinCpcEvent();
            SetUpRecordFile();
            ReadFromXmlInProgramStart();
        }

        private void SetUpRecordFile()
        {
            if (!string.IsNullOrEmpty(cv_FilePath))
            {
                if(cv_RecordXml == null)
                {
                    cv_RecordXml = new KXmlItem();
                    cv_RecordXml.LoadFromFile(cv_FilePath);
                    if (!(cv_RecordXml.IsItemExist("History")))
                    {
                        KXmlItem tmp = new KXmlItem();
                        tmp.Text = @"<History/>";
                        cv_RecordXml.AddItem(tmp);
                    }
                }
            }
        }

        private void WriteToXmlAndSavaFile(JobData m_Job)
        {
            /*
             * <History>
	                <Item Time="" HanderValue="" OtherValue=""/>
               </History>
            */
            if (cv_RecordXml != null)
            {
                KXmlItem item = new KXmlItem();
                string text = @"<Item Time="" HandlerValue="" OtherValue=""/>";
                item.Text = text;
                item.Attributes["Time"] = m_Job.cv_DateTime.ToString("yyyyMMddhhmmss");
                item.Attributes["HandlerValue"] = m_Job.cv_HandlerValue.ToString();
                item.Attributes["OtherValue"] = m_Job.cv_OtherSideValue.ToString();
                cv_RecordXml.ItemsByName["History"].AddItem(item);
                cv_RecordXml.SaveToFile(cv_FilePath, true);
            }
        }

        private void ReadFromXmlInProgramStart()
        {
            /*
             * 
            cv_deleUpdateDownUi fun = new cv_deleUpdateDownUi(UpDownHandler);
            cv_deleUpdateDownUi fun = new cv_deleUpdateDownUi(UpDownPostCln);
            JobData tmp = new JobData(DateTime.Now, fun, m_Value , true);
            cv_LogQueue.Enqueue(tmp);
            */
            //if(cv_RecordXml == null) Common.PopForm("Ini Log Error. Record Xml is null");
            cv_deleUpdateDownUi handler_fun = new cv_deleUpdateDownUi(UpDownHandler);
            cv_deleUpdateDownUi other_fun = new cv_deleUpdateDownUi(UpDownPostCln);
            for (int i = 0; i < cv_RecordXml.ItemsByName["History"].ItemNumber; i++ )
            {
                KXmlItem item = cv_RecordXml.ItemsByName["History"].Items[i];
                try
                {
                    DateTime tmp_time = DateTime.ParseExact(item.Attributes["Time"].Trim(), "yyyyMMddhhmmss", null);
                }
                catch
                {
                    continue;
                }
                int hanlder_value = SysUtils.StrToInt(item.Attributes["HandlerValue"].Trim());
                int other_value = SysUtils.StrToInt(item.Attributes["OtherValue"].Trim());
                JobData tmp = new JobData( hanlder_value , other_value , handler_fun , other_fun);

                if (cv_LogQueue != null)
                {
                    cv_LogQueue.Enqueue(tmp);
                }
            }
        }

        private void JoinCpcEvent()
        {
            //CPC.cv_EventDownInterfaceForLog += new CPC.cv_DeleInterfaceForLog(SaveEvent);
        }

        public void InitailShow()
        {
            //TODO : read log ?
            if (cv_LogQueue.Count < 1)
            {
                cv_BtNext.Enabled = false;
                cv_BtPrevious.Enabled = false;
                cv_LbCurPage.Text = "0";
                cv_LbTotalPage.Text = "/ " + cv_CurrentLog.Count.ToString();
                return;
            }
            else
            {
                if (cv_CurrentLog.Count > 0) cv_CurrentLog.Clear();
                //for (int i = 0; i < cv_LogQueue.Count; i++)
                for (int i = cv_LogQueue.Count - 1 ; i >= 0 ; i--)
                {
                    cv_CurrentLog.Add(cv_LogQueue.ElementAt(i));
                }

                cv_CurrentLog[0].cv_HandlerFun(cv_CurrentLog[0].cv_HandlerValue);
                cv_CurrentLog[0].cv_OtherSideFun(cv_CurrentLog[0].cv_OtherSideValue);
                cv_LbTime.Text = cv_CurrentLog[0].cv_DateTime.ToString("HH:mm:ss");
                cv_LbCurPage.Text = "1";
                cv_LbTotalPage.Text = "/" + cv_CurrentLog.Count.ToString();
                cv_BtPrevious.Enabled = false;
                if (cv_CurrentLog.Count == 1)
                {
                    cv_BtNext.Enabled = false;
                }
                else
                {
                    cv_BtNext.Enabled = true ;
                }
            }
        }

        private void UpDownHandler(int m_Value)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new cv_DeleUpdateUi(UpDownHandler), new object[] { m_Value });
            }
            else
            {
                for (int i = 0; i < 16; i++)
                {
                    if (((m_Value >> i) & 0x0001) > 0)
                    {
                        if (cv_DownHandler[i].BackColor != Color.LimeGreen)
                        {
                            cv_DownHandler[i].BackColor = Color.LimeGreen;
                        }
                    }
                    else
                    {
                        if (cv_DownHandler[i].BackColor != Color.LightGray)
                        {
                            cv_DownHandler[i].BackColor = Color.LightGray;
                        }
                    }
                }
            }
        }

        private void UpDownPostCln(int m_Value)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new cv_DeleUpdateUi(UpDownPostCln), new object[] { m_Value });
            }
            else
            {
                for (int i = 0; i < 16; i++)
                {
                    if (((m_Value >> i) & 0x0001) > 0)
                    {
                        if (cv_DownPostCln[i].BackColor != Color.LimeGreen)
                        {
                            cv_DownPostCln[i].BackColor = Color.LimeGreen;
                        }
                    }
                    else
                    {
                        if (cv_DownPostCln[i].BackColor != Color.LightGray)
                        {
                            cv_DownPostCln[i].BackColor = Color.LightGray;
                        }
                    }
                }
            }
        }

        private void AddAllLableToDic()
        {
            #region Set down-stream post cln dic.
            cv_DownPostCln[0] = cv_LbDownPostCln1;
            cv_DownPostCln[1] = cv_LbDownPostCln2;
            cv_DownPostCln[2] = cv_LbDownPostCln3;
            cv_DownPostCln[3] = cv_LbDownPostCln4;
            cv_DownPostCln[4] = cv_LbDownPostCln5;
            cv_DownPostCln[5] = cv_LbDownPostCln6;
            cv_DownPostCln[6] = cv_LbDownPostCln7;
            cv_DownPostCln[7] = cv_LbDownPostCln8;
            cv_DownPostCln[8] = cv_LbDownPostCln9;
            cv_DownPostCln[9] = cv_LbDownPostCln10;
            cv_DownPostCln[10] = cv_LbDownPostCln11;
            cv_DownPostCln[11] = cv_LbDownPostCln12;
            cv_DownPostCln[12] = cv_LbDownPostCln13;
            cv_DownPostCln[13] = cv_LbDownPostCln14;
            cv_DownPostCln[14] = cv_LbDownPostCln15;
            cv_DownPostCln[15] = cv_LbDownPostCln16;
            foreach (Label tmp in cv_DownPostCln.Values)
            {
                tmp.BackColor = Color.LightGray;
            }
            #endregion

            #region Set down-stream handler dic.
            cv_DownHandler[0] = cv_LbDownHandler1;
            cv_DownHandler[1] = cv_LbDownHandler2;
            cv_DownHandler[2] = cv_LbDownHandler3;
            cv_DownHandler[3] = cv_LbDownHandler4;
            cv_DownHandler[4] = cv_LbDownHandler5;
            cv_DownHandler[5] = cv_LbDownHandler6;
            cv_DownHandler[6] = cv_LbDownHandler7;
            cv_DownHandler[7] = cv_LbDownHandler8;
            cv_DownHandler[8] = cv_LbDownHandler9;
            cv_DownHandler[9] = cv_LbDownHandler10;
            cv_DownHandler[10] = cv_LbDownHandler11;
            cv_DownHandler[11] = cv_LbDownHandler12;
            cv_DownHandler[12] = cv_LbDownHandler13;
            cv_DownHandler[13] = cv_LbDownHandler14;
            cv_DownHandler[14] = cv_LbDownHandler15;
            cv_DownHandler[15] = cv_LbDownHandler16;
            foreach (Label tmp in cv_DownHandler.Values)
            {
                tmp.BackColor = Color.LightGray;
            }
            #endregion
        }

        private void SaveEvent(int m_HandlerValue , int m_OtherSideValue)
        {
            cv_deleUpdateDownUi handler_fun = new cv_deleUpdateDownUi(UpDownHandler);
            cv_deleUpdateDownUi other_fun = new cv_deleUpdateDownUi(UpDownPostCln);

            JobData tmp = new JobData(m_HandlerValue, m_OtherSideValue, handler_fun, other_fun);
            cv_LogQueue.Enqueue(tmp);
            WriteToXmlAndSavaFile(tmp);
            CheckDicLength();
        }

        private void CheckDicLength()
        {
            if (cv_LogQueue.Count > 100)
            {
                for (; cv_LogQueue.Count > 100; )
                {
                    cv_LogQueue.Dequeue();
                }
            }
            if (cv_RecordXml.ItemsByName["History"].ItemNumber > 100)
            {
                cv_RecordXml.ItemsByName["History"].DeleteItem(0);
                cv_RecordXml.SaveToFile(cv_FilePath);
            }
        }

        private void UiDownLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void cv_BtNext_Click(object sender, EventArgs e)
        {
            int current_index = SysUtils.StrToInt(cv_LbCurPage.Text.Trim() );
            JobData tmp = cv_CurrentLog[current_index];
            tmp.cv_HandlerFun(tmp.cv_HandlerValue);
            tmp.cv_OtherSideFun(tmp.cv_OtherSideValue);
            ReSetTimeAndCurPage(tmp.cv_DateTime, (current_index + 1));

            if(current_index +1 == cv_CurrentLog.Count )
            {
                cv_BtNext.Enabled = false;
            }
            else
            {
                cv_BtNext.Enabled = true;
            }
            if (current_index != 2)
            {
                cv_BtPrevious.Enabled = true;
            }
        }

        private void ReSetTimeAndCurPage(DateTime m_Time, int m_CurPage)
        {
            cv_LbCurPage.Text = m_CurPage.ToString();
            cv_LbTime.Text = m_Time.ToString("HH:mm:ss");
        }

        private void cv_BtPrevious_Click(object sender, EventArgs e)
        {
            int current_index = SysUtils.StrToInt(cv_LbCurPage.Text.Trim());
            JobData tmp = cv_CurrentLog[current_index - 2];
            tmp.cv_HandlerFun(tmp.cv_HandlerValue);
            tmp.cv_OtherSideFun(tmp.cv_OtherSideValue);
            ReSetTimeAndCurPage(tmp.cv_DateTime, (current_index - 1));
            if ( (current_index - 2) == 0)
            {
                cv_BtPrevious.Enabled = false;
            }
            else
            {
                cv_BtPrevious.Enabled = true;
            }
            if ((current_index + 1) != cv_CurrentLog.Count)
            {
                cv_BtNext.Enabled = true;
            }
        }

        private void UiDownLog_Load(object sender, EventArgs e)
        {

        }
    }
}

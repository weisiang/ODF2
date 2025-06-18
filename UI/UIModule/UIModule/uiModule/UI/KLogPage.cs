using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KgsCommon;

namespace UI
{
    public partial class KLogPage : UserControl
    {
        private Dictionary<string, KMemoLog> cv_MemoLogs = new Dictionary<string, KMemoLog>();

        //---------------------------------------------------------------------------
        public KLogPage()
        {
            InitializeComponent();
        }

        //---------------------------------------------------------------------------
        public void AddMemoLog(string m_LogName, ref KMemoLog m_MemoLogObj)
        {
            if (cv_MemoLogs.ContainsKey(m_LogName))
            {
                // Log Name 已存在拒絕建立
                return;
            }

            //if (m_MemoLogObj == null)
            //{
                GoCreateMemoryLogObj(ref m_MemoLogObj);
            //}

            if (m_MemoLogObj == null)
            {
                return;
            }

            m_MemoLogObj.BackColor = Color.FromArgb(193, 231, 247);
            TabPage temp_page = new TabPage();
            temp_page.Location = new System.Drawing.Point(4, 22);
            temp_page.Name = m_LogName;
            temp_page.Padding = new System.Windows.Forms.Padding(3);
            temp_page.TabIndex = tabMemoryLogs.TabPages.Count;
            temp_page.Text = m_LogName;
            temp_page.UseVisualStyleBackColor = true;
            temp_page.Size = new System.Drawing.Size(650, 400);
            temp_page.AutoScroll = true;
            tabMemoryLogs.TabPages.Add(temp_page);
            tabMemoryLogs.Dock = DockStyle.Fill;
            temp_page.Controls.Add(m_MemoLogObj);
            m_MemoLogObj.DoubleClick += OnMemoLogObj_DoubleClick;

        }

        //---------------------------------------------------------------------------
        public static void GoCreateMemoryLogObj(ref KMemoLog m_MemoryLogObj)
        {
            if (m_MemoryLogObj == null)
            {
                m_MemoryLogObj = new KMemoLog();
            }
            m_MemoryLogObj.AppendLinkString = "_";
            m_MemoryLogObj.CreateDirByDay = false;
            m_MemoryLogObj.CreateDirByMonth = false;
            m_MemoryLogObj.CreateDirByYear = false;
            m_MemoryLogObj.DebugWindow = false ;
            m_MemoryLogObj.DeleteOutofDateFiles = false;
            m_MemoryLogObj.DisplayLines = 1000;
            m_MemoryLogObj.Dock = System.Windows.Forms.DockStyle.Fill;
            m_MemoryLogObj.EraseLines = 100;
            m_MemoryLogObj.Font = new System.Drawing.Font("新細明體", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            m_MemoryLogObj.FormattingEnabled = true;
            m_MemoryLogObj.ItemHeight = 11;
            m_MemoryLogObj.Location = new System.Drawing.Point(3, 3);
            m_MemoryLogObj.LogCache = true;
            m_MemoryLogObj.LogDate = true;
            //m_MemoryLogObj.LogFileName = ".log";
            //m_MemoryLogObj.LogFileSize = 0;
            m_MemoryLogObj.LogFileTime = new System.DateTime(2016, 1, 22, 16, 55, 34, 828);
            m_MemoryLogObj.LogFileTimePeriod = 60;
            //m_MemoryLogObj.LogLevel = 3;
            m_MemoryLogObj.LogMSecs = true;
            m_MemoryLogObj.LogNameAppendType = KLogNameAppendType.atByDay;
            m_MemoryLogObj.LogTime = true;
            m_MemoryLogObj.LogToFile = true;
            m_MemoryLogObj.LogToMemo = true;
            m_MemoryLogObj.Name = "MemoryLog";
            m_MemoryLogObj.OutofDateDays = 7;
            m_MemoryLogObj.RepeatLogCount = 0;
            m_MemoryLogObj.RepeatLogFilter = true;
            m_MemoryLogObj.Size = new System.Drawing.Size(1000, 382);
            m_MemoryLogObj.TabIndex = 0;
            m_MemoryLogObj.HorizontalScrollbar = true;
        }

        //---------------------------------------------------------------------------
        private void KLogPage_SizeChanged(object sender, EventArgs e)
        {
            foreach (TabPage tab_page in tabMemoryLogs.TabPages)
            {
                if (tab_page == null)
                {
                    continue;
                }

                tab_page.Size = new System.Drawing.Size(this.Width, this.Height);
            }
        }

        //---------------------------------------------------------------------------
        private void OnMemoLogObj_DoubleClick(object sender, EventArgs e)
        {
            KMemoLog temp_log = sender as KMemoLog;
            if (temp_log == null)
            {
                return;
            }
            if (temp_log.Items.Count <= 0)
            {
                return;
            }
            DialogResult result = MessageBox.Show("Do you want to clean this Log information?", "Clean Log", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                temp_log.Items.Clear();
            }
        }
    }
}

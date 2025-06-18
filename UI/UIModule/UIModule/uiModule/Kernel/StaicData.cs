using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using KgsCommon;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections;
using CommonData.HIRATA;
using BaseAp;

namespace UI
{
    public class ThreadObj
    {

        public System.Threading.Thread cv_thread;
        public int id;
        public UInt64 timeout;
        public KDateTime startTime;
        public string title = "";
        public ThreadObj(System.Threading.Thread m_thread , int m_id , UInt64 m_timeout , string m_title)
        {
            cv_thread = m_thread;
            id = m_id;
            timeout = m_timeout;
            startTime = SysUtils.Now();
            title = m_title;
        }
    }

    public class CommonStaticData
    {
        public static string g_LayoutPos;
        public static Color g_EquipmentRunColor;
        public static Color g_EquipmentDownColor;
        public static Color g_EquipmentIdleColor;
        public static Color g_DataAndSensorAllOK;
        public static Color g_DataAndSensorAllNG;
        public static Color g_DataOKAndSensorNG;
        public static Color g_DataNGAndSensorOK;
        public static int g_TipsCount = 0;
        public static KXmlItem cv_ColorXml = new KXmlItem();
        static CommonStaticData()
        {
            InitColor();
            g_LayoutPos = CommonData.HIRATA.CommonStaticData.g_RootConfigFolderPath + CommonData.HIRATA.CommonStaticData.g_FDModuleName + "\\LayoutPos.xml";
            if (!File.Exists(g_LayoutPos)) Environment.Exit(CommonData.HIRATA.ReturnCodeDefine.UI_LayoutPosNotDefined);
        }

        public static void InitColor()
        {
            g_EquipmentDownColor = Color.Red;
            g_EquipmentRunColor = Color.Lime;
            g_EquipmentIdleColor = Color.Yellow;

            g_DataAndSensorAllNG = Color.Gray;
            g_DataAndSensorAllOK = Color.LimeGreen;
            g_DataNGAndSensorOK = Color.Red;
            g_DataOKAndSensorNG = Color.Yellow;

            cv_ColorXml.LoadFromFile(CommonData.HIRATA.CommonStaticData.g_RootConfigFolderPath + "UI\\ColorRecord.xml");

        }
        public static bool PopForm(string m_Msg, bool m_autoClean , bool m_waitReply , uint m_ticket = 0, UInt64 m_timeoutSec = 10000)
        {
            bool result = true;
            string msg = m_Msg;
            if (GenerateOtherThread(msg , m_timeoutSec , m_autoClean , m_waitReply , m_ticket))
            {
                result = true;
            }
            return result;
        }
        private static bool GenerateOtherThread(string m_Msg , UInt64 m_timeoutSec , bool m_autoClean , bool m_waitReply , uint m_ticket)
        {
            bool result = true;
            string msg = m_Msg;
            g_TipsCount++;
            string title = "Tip_" + g_TipsCount.ToString();
            try
            {
                Thread tmp_thread = new Thread(() => ShowForm(title, msg , m_waitReply , m_ticket));
                if (!m_waitReply)
                {
                    if (m_autoClean)
                    {
                        ThreadObj tmp = new ThreadObj(tmp_thread, tmp_thread.ManagedThreadId, m_timeoutSec, title); //300000);
                        UiForm.cv_threadPool.Add(tmp);
                    }
                }
                tmp_thread.Start();
            }
            catch
            {
                result = false;
            }
            return result;
        }
        private static void ShowForm(string m_title , string m_Msg  , bool m_waitReply , uint m_ticket)
        {
            string msg = DateTime.Now.ToString("MM/dd HH:mm:ss") + " [" + m_Msg + "]";
            if (!m_waitReply)
                MessageBox.Show(new Form { TopMost = true } , m_Msg, m_title);
            else
            {
                if(MessageBox.Show(msg, m_title, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.Yes)
                {
                    CommonData.HIRATA.MDShowMsg obj = new MDShowMsg();
                    CommonData.HIRATA.Msg msg_item = new Msg();
                    msg_item.PAutoClean = false;
                    msg_item.PChoice = UserChoice.Yes;
                    msg_item.PUserRep = true;
                    msg_item.Tick = m_ticket;
                    msg_item.Txt = msg;
                    //Global.Controller.SendMmfReplyObject(typeof(CommonData.HIRATA.MDShowMsg).Name, obj, m_ticket, typeof(CommonData.HIRATA.MDShowMsg).Name);
                }
                else if(MessageBox.Show(msg, m_title, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.No)
                {
                    CommonData.HIRATA.MDShowMsg obj = new MDShowMsg();
                    CommonData.HIRATA.Msg msg_item = new Msg();
                    msg_item.PAutoClean = false;
                    msg_item.PChoice = UserChoice.No;
                    msg_item.PUserRep = true;
                    msg_item.Tick = m_ticket;
                    msg_item.Txt = msg;
                    //Global.Controller.SendMmfReplyObject(typeof(CommonData.HIRATA.MDShowMsg).Name, obj, m_ticket, typeof(CommonData.HIRATA.MDShowMsg).Name);
                }
            }
        }

        public static void ChangeDataViewItemColor(ref DataGridView m_Viewer , int m_RowIndex ,  GlassData m_Glass)
        {
            if(m_Glass.PHasData )
            {
                if (m_Glass.PHasSensor)
                {
                    if(m_Glass.PProcessFlag == ProcessFlag.Need)
                    m_Viewer.Rows[m_RowIndex].DefaultCellStyle.BackColor = palette.cv_HasProcessed;
                    else
                    m_Viewer.Rows[m_RowIndex].DefaultCellStyle.BackColor = palette.cv_NotProcess;
                }
                else
                    m_Viewer.Rows[m_RowIndex].DefaultCellStyle.BackColor = g_DataOKAndSensorNG;
            }
            else
            {
                if (!m_Glass.PHasSensor)
                    m_Viewer.Rows[m_RowIndex].DefaultCellStyle.BackColor = g_DataAndSensorAllNG;
                else
                    m_Viewer.Rows[m_RowIndex].DefaultCellStyle.BackColor = g_DataNGAndSensorOK;
            }
        }
        public static void ChangeRobotItemColor(ref Control m_Viewer,  bool m_HasData, bool m_HasSensor)
        {
            if (m_HasData)
            {
                if (m_HasSensor)
                    m_Viewer.BackColor = g_DataAndSensorAllOK;
                else
                    m_Viewer.BackColor = g_DataOKAndSensorNG;
            }
            else
            {
                if (m_HasSensor)
                    m_Viewer.BackColor = g_DataNGAndSensorOK;
                else
                    m_Viewer.BackColor = g_DataAndSensorAllNG;
            }
        }

        private static int cv_FoupSeq = 1;
        private static int cv_WorkOrder = 1;
        public static int CtreatFoupSeq
        {
            get
            {
                if(cv_FoupSeq == 255)
                {
                    cv_FoupSeq = 1;
                }
                return cv_FoupSeq++;
            }
        }
        public static int CtreatWorkOrder
        {
            get
            {
                if (cv_WorkOrder == 255)
                {
                    cv_WorkOrder = 1;
                }
                return cv_WorkOrder++;
            }
        }

    }
}

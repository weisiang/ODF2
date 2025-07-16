using System;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KgsCommon;
using BaseAp;


namespace UI
{
    public partial class CopyLogForm : Form
    {
        static DateTime start = DateTime.Now;
        static DateTime end = DateTime.Now;
        FolderBrowserDialog cv_FolderBroswer = new FolderBrowserDialog();
        delegate void cv_DeleCopyLog();

        public CopyLogForm()
        {
            InitializeComponent();
        }

        public void  Show()
        {
            this.cv_DatePickStart.Value = DateTime.Today;
            this.cv_DatePickEnd.Value = DateTime.Today;
            base.Show();
        }
        private void CopyLogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void cv_BtCopy_Click(object sender, EventArgs e)
        {
            if(lgcBase.PSystemData.POperationMode != CommonData.HIRATA.OperationMode.Manual)
            {
                CommonStaticData.PopForm("Please change to mnaual mode.", true
                    , false);
                return;
            }
            if(string.IsNullOrEmpty(cv_TxTargetFolder.Text))
            {
                return;
            }
            UpdateTimeObj();
            //CheckRootTargetIsExistAndCopy(cv_TxTargetFolder.Text);
            Thread tmp_thread = new Thread(() => Compress());
            tmp_thread.Start();
            this.Hide();
        }
        private void Compress()
        {
            CheckRootTargetIsExistAndCopy(cv_TxTargetFolder.Text);
            Process process = new Process();
            //process.StartInfo.FileName = SysUtils.ExtractFileDir(SysUtils.GetExeName());//@"C:\Program Files\7-zip\7z.exe";
            //process.StartInfo.FileName = CommonData.HIRATA.CommonStaticData.g_RootConfigFolderPath + @"..\dll\7z.exe";//@"C:\Program Files\7-zip\7z.exe";
            process.StartInfo.FileName = @"C:\Program Files\7-zip\7z.exe";
            process.StartInfo.Arguments = @"a -mx9 -tzip " + cv_TxTargetFolder.Text + "\\COPYLOG.7z " + cv_TxTargetFolder.Text + "\\COPYLOG";  
            if (File.Exists(process.StartInfo.FileName) && Directory.Exists(cv_TxTargetFolder.Text + "\\COPYLOG"))
            {
                process.Start();
            }
        }

        //if match condition to return true.
        private bool IsNeedCopy(string m_FileName)
        {
            bool result = false;
            DateTime tmp = File.GetLastWriteTime(m_FileName);
            if ((tmp >= start) && (tmp <= end))
            {
                result = true;
            }
            return result;
        }
        private void UpdateTimeObj()
        {
            start = cv_DatePickStart.Value;
            end = cv_DatePickEnd.Value;
            end = end.AddDays(1);
        }
        private void CheckRootTargetIsExistAndCopy(string m_TarPath)
        {
            if (string.IsNullOrEmpty(m_TarPath))
            {
                return;
            }
            else
            {
                //create target folder
                m_TarPath += @"\COPYLOG";
                if (System.IO.Directory.Exists(m_TarPath))
                {
                    System.IO.Directory.Delete(m_TarPath, true);
                }
                string log_path = CommonData.HIRATA.CommonStaticData.g_RootLogsFolderPath;

                if (!SysUtils.DirExists(log_path))
                {
                    return;
                }
                try
                {
                    ExeCopyFileToTarget(m_TarPath, log_path);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    CommonStaticData.PopForm("Copy logs error!!! Please copy by manual.", true , false);
                }

                //copy config
                m_TarPath += @"\Config";
                string config_path = CommonData.HIRATA.CommonStaticData.g_RootConfigFolderPath;
                if (!SysUtils.DirExists(config_path))
                {
                    return;
                }
                try
                {
                    ExeCopyFileToTarget(m_TarPath, config_path , false);
                }
                catch (Exception e)
                {
                    CommonStaticData.PopForm("Copy configs error!!! Please copy by manual.", true , false);
                }
            }
        }
        private void ExeCopyFileToTarget(string m_TarPath, string m_SrcPath , bool m_IsCheckDate = true)
        {
            System.IO.DirectoryInfo src_info = new System.IO.DirectoryInfo(m_SrcPath);
            try
            {
                if (!System.IO.Directory.Exists(m_TarPath))
                {
                    System.IO.Directory.CreateDirectory(m_TarPath);
                }
                foreach (System.IO.FileInfo file in src_info.GetFiles())
                {
                    if ( (m_IsCheckDate & IsNeedCopy(file.FullName)) || !m_IsCheckDate) 
                    System.IO.File.Copy(file.FullName, m_TarPath +"\\" +  file.Name, true);
                }

                foreach (System.IO.DirectoryInfo di in src_info.GetDirectories())
                {
                    ExeCopyFileToTarget(m_TarPath + "\\" + di.Name , di.FullName , m_IsCheckDate);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }


        private void cv_BtTargetFolder_Click(object sender, EventArgs e)
        {
            Thread tmp = new Thread(ShowSelectForm);
            tmp.SetApartmentState(ApartmentState.STA);
            tmp.Start();
        }
        private void ShowSelectForm()
        {
            if (this.InvokeRequired)
            {
                cv_FolderBroswer.ShowDialog();
                cv_DeleCopyLog dele = new cv_DeleCopyLog(ShowSelectForm);
                this.BeginInvoke(dele);
            }
            else
            {
                cv_TxTargetFolder.Text = cv_FolderBroswer.SelectedPath;
            }
        }
    }
}

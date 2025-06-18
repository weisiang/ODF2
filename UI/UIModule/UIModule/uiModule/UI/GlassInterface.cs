using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KgsCommon;

namespace UI
{
    public partial class UIGlassInterface : Form
    {
        Dictionary<int, Label> cv_UpHandler = new Dictionary<int, Label>();
        Dictionary<int, Label> cv_UpPreCln = new Dictionary<int, Label>();
        Dictionary<int, Label> cv_DownHandler = new Dictionary<int, Label>();
        Dictionary<int, Label> cv_DownPostCln = new Dictionary<int, Label>();
        private delegate void cv_DeleUpdateInterface(int m_Value);
        private KTimer cv_Timer = null;

        public UIGlassInterface()
        {
            InitializeComponent();
            AddAllLableToEachDic();
            InitialForm();
            JoinEvent();
            SetTimer();
        }

        private void SetTimer()
        {
            cv_Timer = new KTimer();
            cv_Timer.Interval = 200;
            cv_Timer.OnTimer += new KTimer.KTimerEvent(UpdateUi);
            cv_Timer.ThreadEventEnabled = false ;
            cv_Timer.Enabled = true;
            cv_Timer.Open();
        }

        private void UpdateUi()
        {
            #region
            //if ( GB.GetGBFeStop) //&& 
            //if( (GB.GBStatus == GBPLC.EqStatus.Stop) && (CPC.Perssion == CPC.SystemPerssion.Root ) )
            /*
            if(CPC.Perssion == CPC.SystemPerssion.Root)
            {
                cv_BtUpForceCom.Enabled = true;
                cv_BtUpForceIni.Enabled = true;
                cv_BtDownForceCom.Enabled = true;
                cv_BtDownForceIni.Enabled = true;
            }
            else
            {
                cv_BtUpForceCom.Enabled = false;
                cv_BtUpForceIni.Enabled = false;
                cv_BtDownForceCom.Enabled = false;
                cv_BtDownForceIni.Enabled = false;
            }
            #endregion check force job enable & disable

            #region check 4 bc stop command

            if (CPC.PostClnStop)
            {
                cv_PostClnStop.BackColor = Color.Red;
            }
            else
            {
                cv_PostClnStop.BackColor = Color.LightGray;
            }
            if(CPC.ProcessStop)
            {
                cv_LbProcessStop.BackColor = Color.Red;
            }
            else
            {
                cv_LbProcessStop.BackColor = Color.LightGray;
            }
            if(CPC.Stop)
            {
                cv_LbStop.BackColor = Color.Red;
            }
            else
            {
                cv_LbStop.BackColor = Color.LightGray;
            }
            if(CPC.TransferStop)
            {
                cv_LbTransferStop.BackColor = Color.Red;
            }
            else
            {
                cv_LbTransferStop.BackColor = Color.LightGray;
            }
            */
            #endregion

        }

        private void InitialForm()
        {
            #region
            /*
            cv_BtUpForceCom.Click += new System.EventHandler(PressUpForceComplete);
            cv_BtUpForceIni.Click += new System.EventHandler(PressUpForceIni);
            cv_BtDownForceCom.Click += new System.EventHandler(PressDownForceComplete);
            cv_BtDownForceIni.Click += new System.EventHandler(PressDownForceIni);
            */
            /*
            #endregion
            #region Stop Send & Receive
            cv_BtStopReceive.Click += new System.EventHandler(PressStopReceive);
            cv_BtStopSend.Click += new System.EventHandler(PressStopSend);
            if (CPC.StopSend)
                cv_BtStopSend.BackColor = Color.Red;
            else
                cv_BtStopSend.BackColor = Color.LightGray;

            if (CPC.StopReceive)
                cv_BtStopReceive.BackColor = Color.Red;
            else
                cv_BtStopReceive.BackColor = Color.LightGray;
            */
            #endregion
        }

        private void AddAllLableToEachDic()
        {
            #region Set up-stream handler dic.
            cv_UpHandler[0] = cv_LbUpHandler1;
            cv_UpHandler[1] = cv_LbUpHandler2;
            cv_UpHandler[2] = cv_LbUpHandler3;
            cv_UpHandler[3] = cv_LbUpHandler4;
            cv_UpHandler[4] = cv_LbUpHandler5;
            cv_UpHandler[5] = cv_LbUpHandler6;
            cv_UpHandler[6] = cv_LbUpHandler7;
            cv_UpHandler[7] = cv_LbUpHandler8;
            cv_UpHandler[8] = cv_LbUpHandler9;
            cv_UpHandler[9] = cv_LbUpHandler10;
            cv_UpHandler[10] = cv_LbUpHandler11;
            cv_UpHandler[11] = cv_LbUpHandler12;
            cv_UpHandler[12] = cv_LbUpHandler13;
            cv_UpHandler[13] = cv_LbUpHandler14;
            cv_UpHandler[14] = cv_LbUpHandler15;
            cv_UpHandler[15] = cv_LbUpHandler16;
            foreach(Label tmp in cv_UpHandler.Values)
            {
                tmp.BackColor = Color.LightGray;
            }
            #endregion

            #region Set up-stream pre-cln dic.
            cv_UpPreCln[0] = cv_LbUpPreCln1;
            cv_UpPreCln[1] = cv_LbUpPreCln2;
            cv_UpPreCln[2] = cv_LbUpPreCln3;
            cv_UpPreCln[3] = cv_LbUpPreCln4;
            cv_UpPreCln[4] = cv_LbUpPreCln5;
            cv_UpPreCln[5] = cv_LbUpPreCln6;
            cv_UpPreCln[6] = cv_LbUpPreCln7;
            cv_UpPreCln[7] = cv_LbUpPreCln8;
            cv_UpPreCln[8] = cv_LbUpPreCln9;
            cv_UpPreCln[9] = cv_LbUpPreCln10;
            cv_UpPreCln[10] = cv_LbUpPreCln11;
            cv_UpPreCln[11] = cv_LbUpPreCln12;
            cv_UpPreCln[12] = cv_LbUpPreCln13;
            cv_UpPreCln[13] = cv_LbUpPreCln14;
            cv_UpPreCln[14] = cv_LbUpPreCln15;
            cv_UpPreCln[15] = cv_LbUpPreCln16;
            foreach (Label tmp in cv_UpPreCln.Values)
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
        }

        private void JoinEvent()
        {
            /*
            CPC.cv_EventDownInterfaceHandler += new CPC.cv_DeleInterface(UpdateDownStreamInterfaceHandler);
            CPC.cv_EventDownInterfacePostCln += new CPC.cv_DeleInterface(UpdateDownStreamInterfacePostCln);
            CPC.cv_EventUpInterfaceHandler += new CPC.cv_DeleInterface(UpdateUpStreamInterfaceHandler);
            CPC.cv_EventUpInterfacePreCln += new CPC.cv_DeleInterface(UpdateUpStreamInterfacePreCln);
            */
        }

        private void UpdateUpStreamInterfaceHandler(int m_Value)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new cv_DeleUpdateInterface(UpdateUpStreamInterfaceHandler), new object[] { m_Value });
            }
            else
            {
                for (int i = 0; i < 16; i++)
                {
                    if (((m_Value >> i) & 0x0001) > 0)
                    {
                        if (cv_UpHandler[i].BackColor != Color.LimeGreen)
                        {
                            cv_UpHandler[i].BackColor = Color.LimeGreen;
                        }
                    }
                    else
                    {
                        if (cv_UpHandler[i].BackColor != Color.LightGray)
                        {
                            cv_UpHandler[i].BackColor = Color.LightGray;
                        }
                    }
                }
            }
        }

        private void UpdateUpStreamInterfacePreCln(int m_Value)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new cv_DeleUpdateInterface(UpdateUpStreamInterfacePreCln), new object[] { m_Value });
            }
            else
            {
                for (int i = 0; i < 16; i++)
                {
                    if (((m_Value >> i) & 0x0001) > 0)
                    {
                        if (cv_UpPreCln[i].BackColor != Color.LimeGreen)
                        {
                            cv_UpPreCln[i].BackColor = Color.LimeGreen;
                        }
                    }
                    else
                    {
                        if (cv_UpPreCln[i].BackColor != Color.LightGray)
                        {
                            cv_UpPreCln[i].BackColor = Color.LightGray;
                        }
                    }
                }
            }
        }

        private void UpdateDownStreamInterfaceHandler(int m_Value)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new cv_DeleUpdateInterface(UpdateDownStreamInterfaceHandler), new object[] { m_Value });
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

        private void UpdateDownStreamInterfacePostCln(int m_Value)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new cv_DeleUpdateInterface(UpdateDownStreamInterfacePostCln), new object[] { m_Value });
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

        private void UIGlassInterface_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void PressStopReceive(object sender, EventArgs e)
        {
            /*
            if (CPC.Perssion != CPC.SystemPerssion.Root)
            {
                Common.PopForm("Please login with root");
                return;
            }
            string user_log = "[ User push GlassIF StopReceive button ] \n";
            if (!CPC.StopReceive)
            {
                CPC.StopReceive = true; // property auto set to GB
                cv_BtStopReceive.BackColor = Color.Red;
                user_log += "Change to : " + CPC.StopReceive.ToString() + "\n";
            }
            else
            {
                CPC.StopReceive = false;
                cv_BtStopReceive.BackColor = Color.LightGray;
                user_log += "Change to : " + CPC.StopReceive.ToString() + "\n";
            }
            if (!string.IsNullOrEmpty(user_log))
            {
                user_log += Common.cv_SplitSymbol;
                LogicKernel.EventHandler.WriteUserLog(user_log);
            }
            */
        }

        private void PressStopSend(object sender, EventArgs e)
        {
            /*
            if (CPC.Perssion != CPC.SystemPerssion.Root)
            {
                Common.PopForm("Please login with root");
                return;
            }
            string user_log = "[ User push GlassIF StopDeliver button ] \n";
            if (!CPC.StopSend)
            {
                CPC.StopSend = true;
                cv_BtStopSend.BackColor = Color.Red;
                user_log += "Change to : " + CPC.StopSend.ToString() + "\n";
            }
            else
            {
                CPC.StopSend = false;
                cv_BtStopSend.BackColor = Color.LightGray;
                user_log += "Change to : " + CPC.StopSend.ToString() + "\n";
            }
            if (!string.IsNullOrEmpty(user_log))
            {
                user_log += Common.cv_SplitSymbol;
                LogicKernel.EventHandler.WriteUserLog(user_log);
            }
            */
        }

        private void cv_BtUpForceCom_Click(object sender, EventArgs e)
        {
            /*
            string user_log = "[ User push GlassIF UpForceComplele button ] \n";
            if(CPC.Perssion != CPC.SystemPerssion.Root)
            {
                Common.PopForm("Please Log in with root!");
                user_log += "Please Log in with root! \n";
            }
            else if (!(GB.GetGBMainStatus == GBPLC.EqStatus.Stop || GB.GetGBMainStatus == GBPLC.EqStatus.Down))
            {
                Common.PopForm("EQ not in Stop");
                user_log += "EQ not in Stop \n";
            }
            else
            {
                GB.SetGBUpForceJob(GBPLC.UpForceType.UpCom);
                user_log += "Change to : " + "Successful !! " + "\n";
            }
            if (!string.IsNullOrEmpty(user_log))
            {
                user_log += Common.cv_SplitSymbol;
                LogicKernel.EventHandler.WriteUserLog(user_log);
            }
            */
        }

        private void cv_BtUpForceIni_Click(object sender, EventArgs e)
        {
            /*
            string user_log = "[ User push GlassIF UpForceInitail button ] \n";
            if(CPC.Perssion != CPC.SystemPerssion.Root)
            {
                Common.PopForm("Please Log in with root!");
                user_log += "Please Log in with root! \n";
            }
            else if (!(GB.GetGBMainStatus == GBPLC.EqStatus.Stop || GB.GetGBMainStatus == GBPLC.EqStatus.Down))
            {
                Common.PopForm("EQ not in Stop");
                user_log += "EQ not in Stop \n";
            }
            else
            {
                GB.SetGBUpForceJob(GBPLC.UpForceType.UpInit);
                user_log += "Change to : " + "Successful !! " + "\n";
            }
            if (!string.IsNullOrEmpty(user_log))
            {
                user_log += Common.cv_SplitSymbol;
                LogicKernel.EventHandler.WriteUserLog(user_log);
            }
            */
        }

        private void cv_BtDownForceCom_Click(object sender, EventArgs e)
        {
            /*
            string user_log = "[ User push GlassIF DownForceComplele button ] \n";
            if(CPC.Perssion != CPC.SystemPerssion.Root)
            {
                Common.PopForm("Please Log in with root!");
                user_log += "Please Log in with root! \n";
            }
            else if(!(GB.GetGBMainStatus == GBPLC.EqStatus.Stop || GB.GetGBMainStatus == GBPLC.EqStatus.Down))
            {
                Common.PopForm("EQ not in Stop");
                user_log += "EQ not in Stop \n";
            }
            else
            {
                GB.SetGBDownForceJob(GBPLC.DownForceType.DownCom);
                user_log += "Change to : " + "Successful !! " + "\n";
            }
            if (!string.IsNullOrEmpty(user_log))
            {
                user_log += Common.cv_SplitSymbol;
                LogicKernel.EventHandler.WriteUserLog(user_log);
            }

            */
        }

        private void cv_BtDownForceIni_Click(object sender, EventArgs e)
        {
            /*
            string user_log = "[ User push GlassIF DownForceInitail button ] \n";
            if(CPC.Perssion != CPC.SystemPerssion.Root)
            {
                Common.PopForm("Please Log in with root!");
                user_log += "Please Log in with root! \n";
            }
            else if (!(GB.GetGBMainStatus == GBPLC.EqStatus.Stop || GB.GetGBMainStatus == GBPLC.EqStatus.Down))
            {
                Common.PopForm("EQ not in Stop");
                user_log += "EQ not in Stop \n";
            }
            else
            {
                GB.SetGBDownForceJob(GBPLC.DownForceType.DownInit);
                user_log += "Change to : " + "Successful !! " + "\n";
            }
            if (!string.IsNullOrEmpty(user_log))
            {
                user_log += Common.cv_SplitSymbol;
                LogicKernel.EventHandler.WriteUserLog(user_log);
            }
            */
        }

        private void cv_GlassInterfaceInit_Click(object sender, EventArgs e)
        {
            /*
            CPC.InitialInterfaceForUI();
            string user_log = "[ User push GlassIF InitialInterface For UI button ] \n";
            if (!string.IsNullOrEmpty(user_log))
            {
                user_log += Common.cv_SplitSymbol;
                LogicKernel.EventHandler.WriteUserLog(user_log);
            }
            */
        }
        //press this button to resume transfer stop from BC
        private void cv_BtTransferStopResume_Click(object sender, EventArgs e)
        {
            /*
            CPC.TransferStop = false;
            string user_log = "[ User push GlassIF TransferResume button ] \n";
            if (!string.IsNullOrEmpty(user_log))
            {
                user_log += Common.cv_SplitSymbol;
                LogicKernel.EventHandler.WriteUserLog(user_log);
            }
            */
        }

    }
}

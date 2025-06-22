using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KgsCommon;
using MxLib;
using CommonData.HIRATA;
using System.Reflection;
using System.Diagnostics;
using BaseAp;

namespace CIM
{
    public partial class CimModule : cimBase
    {
        public static CimModule g_cimModule = null;
        public static KMemoryIOClient PMio
        {
            //get { return g_cimModule.cv_Mio1; }
            get { return g_eventController.cv_TimechartController.GetmemoryIoClient(); }
        }

        KDateTime cv_TimerTime = SysUtils.Now();

        private MDFun cv_MdFun = null;

        public static CIMController g_eventController = null;

        public CimModule() : base( FdModule.CIM)
        {
            g_cimModule = this;
            WriteLog(LogLevelType.General, "[CIM module start]");
            InitEventController();
            InitMdFun();

            cv_Timer.Start();
        }
        ~CimModule()
        {
        }
        public static void WriteLog(LogLevelType m_Type, string m_str, CommonData.HIRATA.FunInOut m_FunInOut = CommonData.HIRATA.FunInOut.None)
        {
            string log = "";
            int level = (int)(SamekLogLevelType)Enum.Parse(typeof(SamekLogLevelType), m_Type.ToString());
            if (m_Type == LogLevelType.NormalFunctionInOut)
            {
                if (m_FunInOut != CommonData.HIRATA.FunInOut.None)
                {
                    log = "[FUN_" + m_FunInOut.ToString() + " ]" + m_str;
                    if (m_FunInOut == FunInOut.Leave)
                    {
                        log += "\n---------------------------------------------";
                    }
                }
            }
            else if (m_Type == LogLevelType.TimerFunction)
            {
                if (m_FunInOut != CommonData.HIRATA.FunInOut.None)
                {
                    log = "[Timer FUN_" + m_FunInOut.ToString() + " ]" + m_str;
                }
            }
            else
            {
                log = "[" + m_Type.ToString() + " ]" + m_str;
            }

            if (g_cimModule != null)
            {
                lock (g_cimModule.cv_Log)
                {
                    try
                    {
                        g_cimModule.cv_Log.WriteLog(log, level);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }
        private void InitEventController()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            if(g_eventController == null)
            {
                g_eventController = new CIMController();
            }
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        private void InitMdFun()
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            if (cv_MdFun == null)
            {
                cv_MdFun = new MDFun();
            }
            try
            {
                cv_MdFun.Open();
            }
            catch(Exception e)
            {
            }
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        private void UpdateSomeStatus()
        {
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);

            //gt 0 is connected. lt doesnot.
            try
            {
                if ((cv_MdFun.mdBdLedRead() & 0x0020) > 0)
                {
                    if (!lgcBase.PSystemData.PPlcConnect)
                    {
                        lgcBase.PSystemData.PPlcConnect= true;
                    }
                }
                else
                {
                    if (lgcBase.PSystemData.PPlcConnect)
                    {
                        lgcBase.PSystemData.PPlcConnect= false;
                    }
                }
            }
            catch(Exception e)
            {
            }
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        protected override void DerivedOnTimer()
        {
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            Int64 diff = SysUtils.MilliSecondsBetween(SysUtils.Now(), cv_TimerTime);
            if (diff > 1000)
            {
                UpdateSomeStatus();
            }
            WriteLog(LogLevelType.TimerFunction, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

    }
}

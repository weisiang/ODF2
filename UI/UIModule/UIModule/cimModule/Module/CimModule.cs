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
        KDateTime cv_TimerTime = SysUtils.Now();

        private MDFun cv_MdFun = null;

        public static CIMController g_eventController = null;

        public CimModule() : base( FdModule.CIM)
        {
            WriteLog(LogLevelType.General, "[CIM module start]");
            InitEventController();
            InitMdFun();

            cv_Timer.Start();
        }
        ~CimModule()
        {
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

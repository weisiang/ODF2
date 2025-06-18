using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KgsCommon;
using CommonData.HIRATA;

namespace LGC
{
    abstract public class LogBase
    {
        protected KFileLog cv_Log = null;
        protected string cv_Module = "";
        protected string cv_Subtitle = "";

        public LogBase(string m_Modeue , string m_title)
        {
            cv_Module = m_Modeue;
            cv_Subtitle = m_title;
        }

        public  void WriteLog(LogLevelType m_Type, string m_str, CommonData.HIRATA.FunInOut m_FunInOut = CommonData.HIRATA.FunInOut.None)
        {
            string log = "";
            int level = (int)(SamekLogLevelType)Enum.Parse(typeof(SamekLogLevelType), m_Type.ToString());
            if (m_Type == LogLevelType.NormalFunctionInOut)
            {
                if (m_FunInOut != CommonData.HIRATA.FunInOut.None)
                {
                    log = "[FUN_" + m_FunInOut.ToString() + " ]" + m_str;
                    if(m_FunInOut == FunInOut.Leave)
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

            if (cv_Log != null)
            {
                lock (cv_Log)
                {
                    try
                    {
                        cv_Log.WriteLog(log, level);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }
        /*
        public void WriteLog(KLogLevelType m_Type, string m_str, CommonData.HIRATA.FunInOut m_FunInOut = CommonData.HIRATA.FunInOut.None)
        {
            string log = "";
            int level = (int)m_Type;
            if (m_Type == KLogLevelType.NormalFunctionInOut)
            {
                log = "[FUN_" + m_FunInOut.ToString() + " ]" + m_str;
            }
            else if (m_Type == KLogLevelType.TimerFunction && m_FunInOut != CommonData.HIRATA.FunInOut.None)
            {
                log = "[Timer FUN_" + m_FunInOut.ToString() + " ]"  + m_str;
            }
            else
            {
                log = m_str;
            }

            try
            {
                cv_Log.WriteLog(log, level);
                SysUtils.DebugWindowTrace(cv_Module, cv_Subtitle, level, log);
            }
            catch (Exception e)
            {
            }
        }
        */
        abstract protected void InitLog();
    }
}

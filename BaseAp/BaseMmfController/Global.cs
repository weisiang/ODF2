using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using KgsCommon;
using System.Runtime.InteropServices;

namespace BaseAp
{
    public struct SYSTEMTIME
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMilliseconds;
    }  

    public class Global : GlobalBase
    {
        /*
        static public string ConfigPathname = string.Empty;
        public static BaseEventController Controller = null;

        static public KFileLog DebugLog;
        */

        [DllImport("kernel32.dll")]
        public extern static uint SetSystemTime(ref SYSTEMTIME lpSystemTime);
        public static bool SystemTimeAdjust(int m_Year, int m_Mon, int m_Day, int m_Hour, int m_Min, int m_Sec)
        {
            bool result = true;
            int Year = m_Year, Month = m_Mon, Day = m_Day
                , Hour = m_Hour, Minute = m_Min, Second = m_Sec;
            SYSTEMTIME a_systime = new SYSTEMTIME();
            KDateTime a_timetemp = new KDateTime();
            a_timetemp = SysUtils.EncodeDateTime(Year, Month, Day, Hour, Minute, Second, 0);
            try
            {
                SysUtils.DecHour(a_timetemp, 8);
            }
            catch
            {
                result = false;
            }

            if (result)
            {
                a_systime.wYear = (ushort)(a_timetemp.Year);
                a_systime.wMonth = (ushort)(a_timetemp.Month);
                a_systime.wDay = (ushort)(a_timetemp.Day);
                a_systime.wHour = (ushort)(a_timetemp.Hour);
                a_systime.wMinute = (ushort)(a_timetemp.Minute);
                a_systime.wSecond = (ushort)(a_timetemp.Second);

                uint set_code = 0;
                try
                {
                    set_code = SetSystemTime(ref a_systime);
                }
                catch
                {
                    result = false;
                }
            }
            return result;
        }
    }
}

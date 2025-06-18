// Copyright (c) 2000-2018, Kingroup Systems Corporation. All rights reserved.
//
// History:
// Date         Reference       Person          Descriptions
// ---------- 	-------------- 	--------------  ---------------------------
// 2018/07/17    	            Cassius         Initial Implementation
//
//---------------------------------------------------------------------------
// 若需修改請回報
//---------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace KgsCommon
{
    public abstract class EventCenterBase
    {
        public delegate void AppEventMessageHandler(string m_MessageId, KXmlItem m_Message);
        public delegate void AppEventObjectMessageHandler(string m_MessageId, Object m_Object);
        protected Dictionary<string, AppEventMessageHandler> cv_AppEventHandlerSet = new Dictionary<string, AppEventMessageHandler>();
        protected Dictionary<string, AppEventObjectMessageHandler> cv_AppEventObjectHandlerSet = new Dictionary<string, AppEventObjectMessageHandler>();
        private static KCriticalSection cv_GlobalAppEventBusCs = new KCriticalSection();

        // used for WaitAppMessageTimeout
        protected class AppEventWaitingMessageData
        {
            public string MessageId;
            public bool IsObject;
            public KXmlItem Message;
            public Object Object;
            public Int64 StartTime;
        }
        List<AppEventWaitingMessageData> cv_BaseAppEventWaitingMessageList = new List<AppEventWaitingMessageData>();
        KCriticalSection cv_BaseAppEventWaitingCountCs = new KCriticalSection();
        int cv_BaseAppEventWaitingCount = 0;

        //---------------------------------------------------------------------------
        public EventCenterBase()
        {
            cv_GlobalAppEventBusCs.Enter();
            try
            {
                if( GlobalBase.AppEventBus == null )
                {
                    GlobalBase.AppEventBus = new KAppEventBus();
                    GlobalBase.AppEventBus.ThreadEventEnabled = true;
                    GlobalBase.AppEventBus.AcceptSentEvent = true;
                }
            }
            catch
            {
            }
            cv_GlobalAppEventBusCs.Leave();
            GlobalBase.AppEventBus.OnMessageReceived += OnAppEventMessageReceived;
            GlobalBase.AppEventBus.OnObjectMessageReceived += OnAppEventObjectMessageReceived;
        }

        //---------------------------------------------------------------------------
        virtual public void WriteDebugLog(List<string> m_Logs, KLogLevelType m_Level)
        {
            WriteDebugLog(m_Logs, (int)m_Level);
        }

        //---------------------------------------------------------------------------
        virtual public void WriteDebugLog(string m_Log, KLogLevelType m_Level)
        {
            WriteDebugLog(m_Log, (int)m_Level);
        }

        abstract public void WriteDebugLog(List<string> m_Logs, int m_Level);

        //---------------------------------------------------------------------------
        abstract public void WriteDebugLog(string m_Log, int m_Level);

        //---------------------------------------------------------------------------
        public virtual void Open()
        {
            GlobalBase.AppEventBus.Open();
        }

        //---------------------------------------------------------------------------
        public void SendAppMessage(string m_ChannelId, string m_MessageId, KXmlItem m_Message)
        {
            try
            {
                GlobalBase.AppEventBus.SendMessage(m_ChannelId, m_MessageId, m_Message);
            }
            catch
            {
            }
        }

        //---------------------------------------------------------------------------
        public void SendAppObjectMessage(string m_ChannelId, string m_MessageId, Object m_Object)
        {
            try
            {
                GlobalBase.AppEventBus.SendObjectMessage(m_ChannelId, m_MessageId, m_Object);
            }
            catch
            {
            }
        }


        public bool WaitAppMessageTimeout(string m_MessageId, out KXmlItem m_Message, int m_Timeout)
        {
            m_Message = null;
            if ( m_Timeout <= 0 )
            {
                return false;
            }

            bool result = false;
            cv_BaseAppEventWaitingCountCs.Enter();
            try
            {
                ++cv_BaseAppEventWaitingCount;
            }
            catch
            {
            }
            cv_BaseAppEventWaitingCountCs.Leave();

            try
            {
                int i, count;
                Int64 start_time, current_time, diff;
                start_time = ((Int64)SysUtils.GetTickCount()) & 0x00000000FFFFFFFF;
                while(true)
                {
                    lock (cv_BaseAppEventWaitingMessageList)
                    {
                        count = cv_BaseAppEventWaitingMessageList.Count;
                        for (i = count - 1; i >= 0; --i)
                        {
                            AppEventWaitingMessageData item = cv_BaseAppEventWaitingMessageList[i];
                            if( item.MessageId == m_MessageId )
                            {
                                if (!item.IsObject && item.StartTime > start_time)
                                {
                                    result = true;
                                    m_Message = item.Message;
                                    cv_BaseAppEventWaitingMessageList.RemoveAt(i);
                                    break;
                                }
                            }
                        }
                    }

                    if (result)
                    {
                        break;
                    }
                    else
                    {
                        current_time = ((Int64)SysUtils.GetTickCount()) & 0x00000000FFFFFFFF;
                        diff = current_time - start_time;
                        if (diff < 0)
                        {
                            start_time = current_time;
                        }

                        if (diff >= m_Timeout)
                        {
                            break;
                        }
                        SysUtils.Sleep(200);
                    }
                }
            }
            catch
            {
            }

            cv_BaseAppEventWaitingCountCs.Enter();
            try
            {
                --cv_BaseAppEventWaitingCount;
                if( cv_BaseAppEventWaitingCount < 0 )
                {
                    cv_BaseAppEventWaitingCount = 0;
                }
            }
            catch
            {
            }
            cv_BaseAppEventWaitingCountCs.Leave();

            return result;
        }


        public bool WaitAppMessageTimeout(Set<string> m_MessageIdSet, out KXmlItem m_Message, int m_Timeout)
        {
            m_Message = null;
            if (m_Timeout <= 0)
            {
                return false;
            }

            bool result = false;
            cv_BaseAppEventWaitingCountCs.Enter();
            try
            {
                ++cv_BaseAppEventWaitingCount;
            }
            catch
            {
            }
            cv_BaseAppEventWaitingCountCs.Leave();

            try
            {
                int i, count;
                Int64 start_time, current_time, diff;
                start_time = ((Int64)SysUtils.GetTickCount()) & 0x00000000FFFFFFFF;
                while (true)
                {
                    lock (cv_BaseAppEventWaitingMessageList)
                    {
                        count = cv_BaseAppEventWaitingMessageList.Count;
                        for (i = count - 1; i >= 0; --i)
                        {
                            AppEventWaitingMessageData item = cv_BaseAppEventWaitingMessageList[i];
                            if ( m_MessageIdSet.ContainsKey(item.MessageId) )
                            {
                                if (!item.IsObject && item.StartTime > start_time)
                                {
                                    result = true;
                                    m_Message = item.Message;
                                    cv_BaseAppEventWaitingMessageList.RemoveAt(i);
                                    break;
                                }
                            }
                        }
                    }

                    if (result)
                    {
                        break;
                    }
                    else
                    {
                        current_time = ((Int64)SysUtils.GetTickCount()) & 0x00000000FFFFFFFF;
                        diff = current_time - start_time;
                        if (diff < 0)
                        {
                            start_time = current_time;
                        }

                        if (diff >= m_Timeout)
                        {
                            break;
                        }
                        SysUtils.Sleep(200);
                    }
                }
            }
            catch
            {
            }

            cv_BaseAppEventWaitingCountCs.Enter();
            try
            {
                --cv_BaseAppEventWaitingCount;
                if (cv_BaseAppEventWaitingCount < 0)
                {
                    cv_BaseAppEventWaitingCount = 0;
                }
            }
            catch
            {
            }
            cv_BaseAppEventWaitingCountCs.Leave();

            return result;
        }

        public bool WaitAppMessageTimeout(string m_MessageId, out Object m_Object, int m_Timeout)
        {
            m_Object = null;
            if (m_Timeout <= 0)
            {
                return false;
            }

            bool result = false;
            cv_BaseAppEventWaitingCountCs.Enter();
            try
            {
                ++cv_BaseAppEventWaitingCount;
            }
            catch
            {
            }
            cv_BaseAppEventWaitingCountCs.Leave();

            try
            {
                int i, count;
                Int64 start_time, current_time, diff;
                start_time = ((Int64)SysUtils.GetTickCount()) & 0x00000000FFFFFFFF;
                while (true)
                {
                    lock (cv_BaseAppEventWaitingMessageList)
                    {
                        count = cv_BaseAppEventWaitingMessageList.Count;
                        for (i = count - 1; i >= 0; --i)
                        {
                            AppEventWaitingMessageData item = cv_BaseAppEventWaitingMessageList[i];
                            if (item.MessageId == m_MessageId)
                            {
                                if (item.IsObject && item.StartTime > start_time)
                                {
                                    result = true;
                                    m_Object = item.Object;
                                    cv_BaseAppEventWaitingMessageList.RemoveAt(i);
                                    break;
                                }
                            }
                        }
                    }

                    if (result)
                    {
                        break;
                    }
                    else
                    {
                        current_time = ((Int64)SysUtils.GetTickCount()) & 0x00000000FFFFFFFF;
                        diff = current_time - start_time;
                        if (diff < 0)
                        {
                            start_time = current_time;
                        }

                        if (diff >= m_Timeout)
                        {
                            break;
                        }
                        SysUtils.Sleep(200);
                    }
                }
            }
            catch
            {
            }

            cv_BaseAppEventWaitingCountCs.Enter();
            try
            {
                --cv_BaseAppEventWaitingCount;
                if (cv_BaseAppEventWaitingCount < 0)
                {
                    cv_BaseAppEventWaitingCount = 0;
                }
            }
            catch
            {
            }
            cv_BaseAppEventWaitingCountCs.Leave();

            return result;
        }


        public bool WaitAppMessageTimeout(Set<string> m_MessageIdSet, out Object m_Object, int m_Timeout)
        {
            m_Object = null;
            if (m_Timeout <= 0)
            {
                return false;
            }

            bool result = false;
            cv_BaseAppEventWaitingCountCs.Enter();
            try
            {
                ++cv_BaseAppEventWaitingCount;
            }
            catch
            {
            }
            cv_BaseAppEventWaitingCountCs.Leave();

            try
            {
                int i, count;
                Int64 start_time, current_time, diff;
                start_time = ((Int64)SysUtils.GetTickCount()) & 0x00000000FFFFFFFF;
                while (true)
                {
                    lock (cv_BaseAppEventWaitingMessageList)
                    {
                        count = cv_BaseAppEventWaitingMessageList.Count;
                        for (i = count - 1; i >= 0; --i)
                        {
                            AppEventWaitingMessageData item = cv_BaseAppEventWaitingMessageList[i];
                            if (m_MessageIdSet.ContainsKey(item.MessageId))
                            {
                                if (item.IsObject && item.StartTime > start_time)
                                {
                                    result = true;
                                    m_Object = item.Object;
                                    cv_BaseAppEventWaitingMessageList.RemoveAt(i);
                                    break;
                                }
                            }
                        }
                    }

                    if (result)
                    {
                        break;
                    }
                    else
                    {
                        current_time = ((Int64)SysUtils.GetTickCount()) & 0x00000000FFFFFFFF;
                        diff = current_time - start_time;
                        if (diff < 0)
                        {
                            start_time = current_time;
                        }

                        if (diff >= m_Timeout)
                        {
                            break;
                        }
                        SysUtils.Sleep(200);
                    }
                }
            }
            catch
            {
            }

            cv_BaseAppEventWaitingCountCs.Enter();
            try
            {
                --cv_BaseAppEventWaitingCount;
                if (cv_BaseAppEventWaitingCount < 0)
                {
                    cv_BaseAppEventWaitingCount = 0;
                }
            }
            catch
            {
            }
            cv_BaseAppEventWaitingCountCs.Leave();

            return result;
        }

        //---------------------------------------------------------------------------
        private void OnAppEventMessageReceived(string m_ChannelId, string m_MessageId, KXmlItem m_Message)
        {
            AppEventMessageHandler handler = null;
            lock (cv_AppEventHandlerSet)
            {
                if (cv_AppEventHandlerSet.ContainsKey(m_MessageId))
                {
                    handler = cv_AppEventHandlerSet[m_MessageId];
                }
            }

            if( handler != null )
            {
                WriteDebugLog("[EventCenterBase][AppEvent] Run handler for " + m_MessageId, 4);
                handler(m_MessageId, m_Message);
            }
            else
            {
                // Not really a warning
                WriteDebugLog("[EventCenterBase][AppEvent] No handler for " + m_MessageId, 4);
            }

            // remove timeout item
            if (cv_BaseAppEventWaitingMessageList.Count > 0)
            {
                lock (cv_BaseAppEventWaitingMessageList)
                {
                    Int64 current_time, diff;
                    int i, count;

                    current_time = ((Int64)SysUtils.GetTickCount()) & 0x00000000FFFFFFFF;

                    count = cv_BaseAppEventWaitingMessageList.Count;
                    for (i = count - 1; i >= 0; --i)
                    {
                        AppEventWaitingMessageData item = cv_BaseAppEventWaitingMessageList[i];
                        diff = current_time - item.StartTime;
                        if (diff < 0)
                        {
                            item.StartTime = current_time;
                        }
                        if (diff > 3000)
                        {
                            cv_BaseAppEventWaitingMessageList.RemoveAt(i);
                        }
                    }
                }
            }

            if (cv_BaseAppEventWaitingCount > 0)
            {
                AppEventWaitingMessageData item = new AppEventWaitingMessageData();

                item.MessageId = m_MessageId;
                item.IsObject = false;
                item.Message = m_Message;
                item.Object = null;
                item.StartTime = ((Int64)SysUtils.GetTickCount()) & 0x00000000FFFFFFFF;
                try
                {
                    lock (cv_BaseAppEventWaitingMessageList)
                    {
                        cv_BaseAppEventWaitingMessageList.Add(item);
                    }
                }
                catch
                {
                }
            }
        }

        //---------------------------------------------------------------------------
        private void OnAppEventObjectMessageReceived(string m_ChannelId, string m_MessageId, Object m_Object)
        {
            AppEventObjectMessageHandler handler = null;
            lock (cv_AppEventObjectHandlerSet)
            {
                if (cv_AppEventObjectHandlerSet.ContainsKey(m_MessageId))
                {
                    handler = cv_AppEventObjectHandlerSet[m_MessageId];
                }
            }

            if( handler != null )
            {
                WriteDebugLog("[EventCenterBase][AppEvent] Run object handler for " + m_MessageId, 4);
                handler(m_MessageId, m_Object);
            }
            else
            {
                // Not really a warning
                WriteDebugLog("[EventCenterBase][AppEvent] No object handler for " + m_MessageId, 4);
            }

            // remove timeout item
            if (cv_BaseAppEventWaitingMessageList.Count > 0)
            {
                lock (cv_BaseAppEventWaitingMessageList)
                {
                    Int64 current_time, diff;
                    int i, count;

                    current_time = ((Int64)SysUtils.GetTickCount()) & 0x00000000FFFFFFFF;

                    count = cv_BaseAppEventWaitingMessageList.Count;
                    for (i = count - 1; i >= 0; --i)
                    {
                        AppEventWaitingMessageData item = cv_BaseAppEventWaitingMessageList[i];
                        diff = current_time - item.StartTime;
                        if (diff < 0)
                        {
                            item.StartTime = current_time;
                        }
                        if (diff > 3000)
                        {
                            cv_BaseAppEventWaitingMessageList.RemoveAt(i);
                        }
                    }
                }
            }

            if (cv_BaseAppEventWaitingCount > 0)
            {
                AppEventWaitingMessageData item = new AppEventWaitingMessageData();

                item.MessageId = m_MessageId;
                item.IsObject = true;
                item.Message = null;
                item.Object = m_Object;
                item.StartTime = ((Int64)SysUtils.GetTickCount()) & 0x00000000FFFFFFFF;
                try
                {
                    lock (cv_BaseAppEventWaitingMessageList)
                    {
                        cv_BaseAppEventWaitingMessageList.Add(item);
                    }
                }
                catch
                {
                }
            }
        }

        //---------------------------------------------------------------------------
        protected void AssignAppEventMessageFunction(string m_MessageId, AppEventMessageHandler m_Handler)
        {
            lock (cv_AppEventHandlerSet)
            {
                cv_AppEventHandlerSet.Add(m_MessageId, m_Handler);
            }
        }

        //---------------------------------------------------------------------------
        protected void AssignAppEventObjectFunction(string m_MessageId, AppEventObjectMessageHandler m_Handler)
        {
            lock (cv_AppEventObjectHandlerSet)
            {
                cv_AppEventObjectHandlerSet.Add(m_MessageId, m_Handler);
            }
        }
        
        //---------------------------------------------------------------------------
        /// <summary>
        /// xml 字串轉物件 
        /// </summary>
        /// <typeparam name="T">類別</typeparam>
        /// <param name="m_XmlString">xml 字串</param>
        /// <returns>物件</returns>
        public static T ParseXmlToObject<T>(string m_XmlString) where T : class, new()
        {
            KXmlItem item = new KXmlItem(m_XmlString);
            T obj = new T();
            if( ParseXmlToObject(obj, item) )
            {
                return obj;
            }
            else
            {
                return null;
            }
        }

        //---------------------------------------------------------------------------
        /// <summary>
        /// KXmlItem 轉物件 
        /// </summary>
        /// <typeparam name="T">類別</typeparam>
        /// <param name="m_XmlItem">KXmlItem</param>
        /// <returns>物件</returns>
        public static T ParseXmlToObject<T>(KXmlItem m_XmlItem) where T : class, new()
        {
            T obj = new T();
            if (ParseXmlToObject(obj, m_XmlItem))
            {
                return obj;
            }
            else
            {
                return null;
            }
        }

        //---------------------------------------------------------------------------
        /// <summary>
        /// Parse KXmlItem to Object
        /// </summary>
        /// <param name="m_Object"></param>
        /// <param name="m_XmlItem"></param>
        /// <returns>true/false</returns>
        public static bool ParseXmlToObject(Object m_Object, KXmlItem m_XmlItem)
        {
            return GlobalBase.ParseXmlToObject(m_Object, m_XmlItem);
        }

        //---------------------------------------------------------------------------
        /// <summary>
        /// Parse Object to KXmlItem 
        /// </summary>
        /// <param name="m_XmlItem"></param>
        /// <param name="">m_Object</param>
        /// <returns>true/false</returns>
        public static KXmlItem ParseObjectToKXmlItem(Object m_Object, KParseObjToXmlPropertyType m_PropertyType)
        {
            return GlobalBase.ParseObjectToKXmlItem(m_Object, m_PropertyType);
        }

        public static KXmlItem ParseObjectToKXmlItem(Object m_Object)
        {
            return GlobalBase.ParseObjectToKXmlItem(m_Object, KParseObjToXmlPropertyType.All);
        }

        //---------------------------------------------------------------------------
        /// <summary>
        /// Parse Object to KXmlItem 
        /// </summary>
        /// <param name="m_XmlItem"></param>
        /// <param name="">m_Object</param>
        /// <returns>true/false</returns>
        public static bool ParseObjectToXml(KXmlItem m_XmlItem, Object m_Object, KParseObjToXmlPropertyType m_PropertyType)
        {
            return GlobalBase.ParseObjectToXml(m_XmlItem, m_Object, m_PropertyType);
        }
        public static bool ParseObjectToXml(KXmlItem m_XmlItem, Object m_Object)
        {
            return GlobalBase.ParseObjectToXml(m_XmlItem, m_Object);
        }

        //---------------------------------------------------------------------------
        /// <summary>
        /// Parse Object to XML string 
        /// </summary>
        /// <param name="">m_Object</param>
        /// <returns>xml string</returns>
        public static string ParseObjectToXml(Object m_Object, KParseObjToXmlPropertyType m_PropertyType)
        {
            return GlobalBase.ParseObjectToXml(m_Object, m_PropertyType);
        }

        public static string ParseObjectToXml(Object m_Object)
        {
            return GlobalBase.ParseObjectToXml(m_Object);
        }

        //---------------------------------------------------------------------------
        /// <summary>
        /// Parse Object to XML string 
        /// </summary>
        /// <param name="m_XmlItem"></param>
        /// <param name="">m_Object</param>
        /// <returns>true/false</returns>
        public static bool ParseObjectToXml(StringBuilder m_StringBuilder, Object m_Object, KParseObjToXmlPropertyType m_PropertyType)
        {
            return GlobalBase.ParseObjectToXml(m_StringBuilder, m_Object, m_PropertyType);
        }

        public static bool ParseObjectToXml(StringBuilder m_StringBuilder, Object m_Object)
        {
            return GlobalBase.ParseObjectToXml(m_StringBuilder, m_Object);
        }
    }
}

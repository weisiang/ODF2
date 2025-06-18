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
    public enum KParseObjToXmlPropertyType { All = 0, Property = 1, Field = 2 };
    public enum KLogLevelType { Error = 1, Warning = 1, UI = 1, MsgName = 2, MsgArg = 3, General = 3, Detail = 4, NormalFunctionInOut = 4, TimerFunction = 5, StateMachine = 5, RawData = 6 };


    public class GlobalBase
    {
        public static KAppEventBus AppEventBus = null;
        
        /// <summary>
        /// 以下為需要初始化的項目
        /// </summary>

        public static string LogIniPathname = string.Empty;
        public static string SystemIniPathname = string.Empty;

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
            if( ParseXmlToObject(obj, m_XmlItem) )
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
            bool result = false;

            if (m_XmlItem != null && m_Object != null)
            {
                Type obj_type = m_Object.GetType();

                if (obj_type.IsGenericType)
                {
                    IList list_obj = m_Object as IList;
                    if (list_obj != null && m_XmlItem.ItemType == KXmlItemType.itxList && m_XmlItem.ItemNumber > 0)
                    {
                        int i, count;
                        count = m_XmlItem.ItemNumber;

                        KXmlItem first_sub_item = m_XmlItem.Items[0];
                        if (first_sub_item.ItemType == KXmlItemType.itxList)
                        {
                            Type sub_item_type = list_obj.GetType().GetGenericArguments()[0];
                            if (sub_item_type != null)
                            {
                                for (i = 0; i < count; ++i)
                                {
                                    KXmlItem sub_item = m_XmlItem.Items[i];

                                    Object child_obj = Activator.CreateInstance(sub_item_type);
                                    ParseXmlToObject(child_obj, sub_item);
                                    list_obj.Add(child_obj);
                                }
                            }
                        }
                        else
                        {
                            Type sub_type = list_obj.GetType().GetGenericArguments()[0];
                            if (sub_type == typeof(string))
                            {
                                for (i = 0; i < count; ++i)
                                {
                                    KXmlItem sub_item = m_XmlItem.Items[i];
                                    list_obj.Add(sub_item.AsString);
                                }
                            }
                            else if (sub_type == typeof(bool))
                            {
                                bool tmp_data = false;
                                for (i = 0; i < count; ++i)
                                {
                                    KXmlItem sub_item = m_XmlItem.Items[i];
                                    bool.TryParse(sub_item.AsString, out tmp_data);
                                    list_obj.Add(tmp_data);
                                }
                            }
                            else if (sub_type == typeof(uint))
                            {
                                uint tmp_data = 0;
                                for (i = 0; i < count; ++i)
                                {
                                    KXmlItem sub_item = m_XmlItem.Items[i];
                                    uint.TryParse(sub_item.AsString, out tmp_data);
                                    list_obj.Add(tmp_data);
                                }
                            }
                            else if (sub_type == typeof(int))
                            {
                                int tmp_data = 0;
                                for (i = 0; i < count; ++i)
                                {
                                    KXmlItem sub_item = m_XmlItem.Items[i];
                                    int.TryParse(sub_item.AsString, out tmp_data);
                                    list_obj.Add(tmp_data);
                                }
                            }
                            else if (sub_type == typeof(double))
                            {
                                double tmp_data = 0;
                                for (i = 0; i < count; ++i)
                                {
                                    KXmlItem sub_item = m_XmlItem.Items[i];
                                    double.TryParse(sub_item.AsString, out tmp_data);
                                    list_obj.Add(tmp_data);
                                }
                            }
                            else if (sub_type == typeof(decimal))
                            {
                                decimal tmp_data = 0;
                                for (i = 0; i < count; ++i)
                                {
                                    KXmlItem sub_item = m_XmlItem.Items[i];
                                    decimal.TryParse(sub_item.AsString, out tmp_data);
                                    list_obj.Add(tmp_data);
                                }
                            }
                            else if (sub_type == typeof(float))
                            {
                                float tmp_data = 0;
                                for (i = 0; i < count; ++i)
                                {
                                    KXmlItem sub_item = m_XmlItem.Items[i];
                                    float.TryParse(sub_item.AsString, out tmp_data);
                                    list_obj.Add(tmp_data);
                                }
                            }
                            else if (sub_type == typeof(long))
                            {
                                long tmp_data = 0;
                                for (i = 0; i < count; ++i)
                                {
                                    KXmlItem sub_item = m_XmlItem.Items[i];
                                    long.TryParse(sub_item.AsString, out tmp_data);
                                    list_obj.Add(tmp_data);
                                }
                            }
                            else if (sub_type == typeof(ulong))
                            {
                                long tmp_data = 0;
                                for (i = 0; i < count; ++i)
                                {
                                    KXmlItem sub_item = m_XmlItem.Items[i];
                                    long.TryParse(sub_item.AsString, out tmp_data);
                                    list_obj.Add(tmp_data);
                                }
                            }
                        }
                    }
                    result = true;
                }
                else
                {
                    if (m_XmlItem.ItemType == KXmlItemType.itxList)
                    {
                        int i, count;
                        count = m_XmlItem.ItemNumber;
                        for (i = 0; i < count; ++i)
                        {
                            KXmlItem sub_item = m_XmlItem.Items[i];
                            PropertyInfo prop_info = null;
                            FieldInfo field_info = null;

                            field_info = obj_type.GetField(sub_item.Name);
                            if (field_info == null)
                            {
                                prop_info = obj_type.GetProperty(sub_item.Name);
                            }

                            if (field_info != null)
                            {
                                // found a field
                                result = true;
                                if (sub_item.ItemType == KXmlItemType.itxList)
                                {
                                    Type sub_item_type = field_info.FieldType;
                                    if (sub_item_type.IsGenericType)
                                    {
                                        Object child_obj = Activator.CreateInstance(sub_item_type);
                                        IList child_list_obj = child_obj as IList;
                                        if (child_list_obj != null)
                                        {
                                            ParseXmlToObject(child_list_obj, sub_item);
                                            field_info.SetValue(m_Object, child_obj);
                                        }
                                    }
                                    else
                                    {
                                        Object child_obj = Activator.CreateInstance(sub_item_type);
                                        ParseXmlToObject(child_obj, sub_item);
                                        field_info.SetValue(m_Object, child_obj);
                                    }
                                }
                                else
                                {
                                    string str = sub_item.AsString;
                                    if (str == null)
                                    {
                                        str = string.Empty;
                                    }

                                    if (field_info.FieldType == typeof(string))
                                    {
                                        field_info.SetValue(m_Object, str);
                                    }
                                    else if (field_info.FieldType == typeof(bool))
                                    {
                                        bool tmp_data = false;
                                        bool.TryParse(str, out tmp_data);
                                        field_info.SetValue(m_Object, tmp_data);
                                    }
                                    else if (field_info.FieldType == typeof(uint))
                                    {
                                        uint tmp_data = 0;
                                        uint.TryParse(str, out tmp_data);
                                        field_info.SetValue(m_Object, tmp_data);
                                    }
                                    else if (field_info.FieldType == typeof(int))
                                    {
                                        int tmp_data = 0;
                                        int.TryParse(str, out tmp_data);
                                        field_info.SetValue(m_Object, tmp_data);
                                    }
                                    else if (field_info.FieldType == typeof(double))
                                    {
                                        double tmp_data = 0;
                                        double.TryParse(str, out tmp_data);
                                        field_info.SetValue(m_Object, tmp_data);
                                    }
                                    else if (field_info.FieldType == typeof(decimal))
                                    {
                                        decimal tmp_data = 0;
                                        decimal.TryParse(str, out tmp_data);
                                        field_info.SetValue(m_Object, tmp_data);
                                    }
                                    else if (field_info.FieldType == typeof(float))
                                    {
                                        float tmp_data = 0;
                                        float.TryParse(str, out tmp_data);
                                        field_info.SetValue(m_Object, tmp_data);
                                    }
                                    else if (field_info.FieldType == typeof(long))
                                    {
                                        long tmp_data = 0;
                                        long.TryParse(str, out tmp_data);
                                        field_info.SetValue(m_Object, tmp_data);
                                    }
                                    else if (field_info.FieldType == typeof(ulong))
                                    {
                                        ulong tmp_data = 0;
                                        ulong.TryParse(str, out tmp_data);
                                        field_info.SetValue(m_Object, tmp_data);
                                    }
                                }
                            }
                            else if (prop_info != null)
                            {
                                // found a property
                                result = true;
                                if (sub_item.ItemType == KXmlItemType.itxList)
                                {
                                    Type sub_item_type = prop_info.PropertyType;
                                    if (sub_item_type.IsGenericType)
                                    {
                                        Object child_obj = Activator.CreateInstance(sub_item_type);
                                        IList child_list_obj = child_obj as IList;
                                        if (child_list_obj != null)
                                        {
                                            ParseXmlToObject(child_list_obj, sub_item);
                                            prop_info.SetValue(m_Object, child_obj, null);
                                        }
                                    }
                                    else
                                    {
                                        Object child_obj = Activator.CreateInstance(sub_item_type);
                                        ParseXmlToObject(child_obj, sub_item);
                                        prop_info.SetValue(m_Object, child_obj, null);
                                    }
                                }
                                else
                                {
                                    string str = sub_item.AsString;
                                    if (str == null)
                                    {
                                        str = string.Empty;
                                    }

                                    if (prop_info.PropertyType == typeof(string))
                                    {
                                        prop_info.SetValue(m_Object, str, null);
                                    }
                                    else if (prop_info.PropertyType == typeof(bool))
                                    {
                                        bool tmp_data = false;
                                        bool.TryParse(str, out tmp_data);
                                        prop_info.SetValue(m_Object, tmp_data, null);
                                    }
                                    else if (prop_info.PropertyType == typeof(uint))
                                    {
                                        uint tmp_data = 0;
                                        uint.TryParse(str, out tmp_data);
                                        prop_info.SetValue(m_Object, tmp_data, null);
                                    }
                                    else if (prop_info.PropertyType == typeof(int))
                                    {
                                        int tmp_data = 0;
                                        int.TryParse(str, out tmp_data);
                                        prop_info.SetValue(m_Object, tmp_data, null);
                                    }
                                    else if (prop_info.PropertyType == typeof(double))
                                    {
                                        double tmp_data = 0;
                                        double.TryParse(str, out tmp_data);
                                        prop_info.SetValue(m_Object, tmp_data, null);
                                    }
                                    else if (prop_info.PropertyType == typeof(decimal))
                                    {
                                        decimal tmp_data = 0;
                                        decimal.TryParse(str, out tmp_data);
                                        prop_info.SetValue(m_Object, tmp_data, null);
                                    }
                                    else if (prop_info.PropertyType == typeof(float))
                                    {
                                        float tmp_data = 0;
                                        float.TryParse(str, out tmp_data);
                                        prop_info.SetValue(m_Object, tmp_data, null);
                                    }
                                    else if (prop_info.PropertyType == typeof(long))
                                    {
                                        long tmp_data = 0;
                                        long.TryParse(str, out tmp_data);
                                        prop_info.SetValue(m_Object, tmp_data, null);
                                    }
                                    else if (prop_info.PropertyType == typeof(ulong))
                                    {
                                        ulong tmp_data = 0;
                                        ulong.TryParse(str, out tmp_data);
                                        prop_info.SetValue(m_Object, tmp_data, null);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result;
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
            KXmlItem item = new KXmlItem();
            ParseObjectToXml(item, m_Object, m_PropertyType);

            return item;
        }
        
        public static KXmlItem ParseObjectToKXmlItem(Object m_Object)
        {
            return ParseObjectToKXmlItem(m_Object, KParseObjToXmlPropertyType.All);
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
            bool result = false;

            if (m_XmlItem != null && m_Object != null)
            {
                Type obj_type = m_Object.GetType();

                if (obj_type.IsGenericType)
                {
                    IList list_obj = m_Object as IList;
                    if (list_obj != null)
                    {
                        m_XmlItem.ItemType = KXmlItemType.itxList;
                        m_XmlItem.ItemNumber = list_obj.Count;

                        int i;

                        Type child_type = list_obj.GetType().GetGenericArguments()[0];
                        if (child_type.IsClass && child_type != typeof(string))
                        {
                            i = 0;
                            foreach (Object obj in list_obj)
                            {
                                ParseObjectToXml(m_XmlItem.Items[i], obj, m_PropertyType);
                                ++i;
                            }
                        }
                        else
                        {
                            i = 0;
                            foreach (Object obj in list_obj)
                            {
                                m_XmlItem.Items[i].Name = "Item";
                                m_XmlItem.Items[i].AsString = obj.ToString();
                                ++i;
                            }
                        }
                        result = true;
                    }
                }
                else if (obj_type.IsClass)
                {
                    PropertyInfo[] properties = obj_type.GetProperties();
                    FieldInfo[] fields = obj_type.GetFields();

                    int total_count = 0;

                    if ( m_PropertyType == KParseObjToXmlPropertyType.Field )
                    {
                        total_count = fields.Length;
                    }
                    else if (m_PropertyType == KParseObjToXmlPropertyType.Property)
                    {
                        total_count = properties.Length;
                    }
                    else //if (m_PropertyType == KParseObjToXmlPropertyType.All)
                    {
                        total_count = properties.Length + fields.Length;
                    }

                    m_XmlItem.Name = obj_type.Name;
                    m_XmlItem.ItemType = KXmlItemType.itxList;
                    m_XmlItem.ItemNumber = total_count;

                    Object child_obj;
                    Type child_type;
                    int count = 0;
                    int i = 0;

                    if (m_PropertyType == KParseObjToXmlPropertyType.All || m_PropertyType == KParseObjToXmlPropertyType.Property )
                    {
                        foreach (PropertyInfo prop in properties)
                        {
                            child_obj = prop.GetValue(m_Object, null);
                            if (child_obj != null)
                            {
                                m_XmlItem.Items[i].Name = prop.Name;

                                child_type = child_obj.GetType();
                                if (child_type == typeof(string))
                                {
                                    m_XmlItem.Items[i].AsString = child_obj.ToString();
                                }
                                else if (child_type.IsClass || child_type.IsGenericType)
                                {
                                    ParseObjectToXml(m_XmlItem.Items[i], child_obj, m_PropertyType);
                                }
                                else
                                {
                                    m_XmlItem.Items[i].AsString = child_obj.ToString();
                                }
                                ++count;
                            }
                            ++i;
                        }
                    }

                    if (m_PropertyType == KParseObjToXmlPropertyType.All || m_PropertyType == KParseObjToXmlPropertyType.Field)
                    {
                        foreach (FieldInfo field in fields)
                        {
                            child_obj = field.GetValue(m_Object);
                            if (field.IsPublic && child_obj != null)
                            {
                                m_XmlItem.Items[i].Name = field.Name;

                                child_type = child_obj.GetType();
                                if (child_type == typeof(string))
                                {
                                    m_XmlItem.Items[i].AsString = child_obj.ToString();
                                }
                                else if (child_type.IsClass || child_type.IsGenericType)
                                {
                                    ParseObjectToXml(m_XmlItem.Items[i], child_obj, m_PropertyType);
                                }
                                else
                                {
                                    m_XmlItem.Items[i].AsString = child_obj.ToString();
                                }
                                ++count;
                            }
                            ++i;
                        }
                    }

                    if (count != total_count)
                    {
                        m_XmlItem.ItemNumber = count;
                    }

                    result = true;
                }
            }

            return result;
        }

        public static bool ParseObjectToXml(KXmlItem m_XmlItem, Object m_Object)
        {
            return ParseObjectToXml(m_XmlItem, m_Object, KParseObjToXmlPropertyType.All);
        }

        //---------------------------------------------------------------------------
        /// <summary>
        /// Parse Object to XML string 
        /// </summary>
        /// <param name="">m_Object</param>
        /// <returns>xml string</returns>
        public static string ParseObjectToXml(Object m_Object, KParseObjToXmlPropertyType m_PropertyType)
        {
            StringBuilder sb = new StringBuilder();
            if (ParseObjectToXml(sb, m_Object, m_PropertyType))
            {
                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public static string ParseObjectToXml(Object m_Object)
        {
            return ParseObjectToXml(m_Object, KParseObjToXmlPropertyType.All);
        }

        /// <summary>
        /// Parse Object to XML string 
        /// </summary>
        /// <param name="m_XmlItem"></param>
        /// <param name="">m_Object</param>
        /// <returns>true/false</returns>
        public static bool ParseObjectToXml(StringBuilder m_StringBuilder, Object m_Object, KParseObjToXmlPropertyType m_PropertyType)
        {
            bool result = false;

            if (m_StringBuilder != null && m_Object != null)
            {
                Type obj_type = m_Object.GetType();

                if (obj_type.IsGenericType)
                {
                    IList list_obj = m_Object as IList;
                    if (list_obj != null)
                    {
                        Type child_type = list_obj.GetType().GetGenericArguments()[0];
                        if (child_type.IsClass && child_type != typeof(string))
                        {
                            foreach (Object obj in list_obj)
                            {
                                ParseObjectToXml(m_StringBuilder, obj, m_PropertyType);
                            }
                        }
                        else
                        {
                            foreach (Object obj in list_obj)
                            {
                                m_StringBuilder.Append("<Item>");
                                m_StringBuilder.Append(obj.ToString());
                                m_StringBuilder.Append("</Item>");
                            }
                        }
                        result = true;
                    }
                }
                else if (obj_type.IsClass)
                {
                    PropertyInfo[] properties = obj_type.GetProperties();
                    FieldInfo[] fields = obj_type.GetFields();
                    
                    m_StringBuilder.Append("<");
                    m_StringBuilder.Append(obj_type.Name);
                    m_StringBuilder.Append(">");

                    Object child_obj;
                    Type child_type;

                    if (m_PropertyType == KParseObjToXmlPropertyType.All || m_PropertyType == KParseObjToXmlPropertyType.Property)
                    {
                        foreach (PropertyInfo prop in properties)
                        {
                            child_obj = prop.GetValue(m_Object, null);
                            if (child_obj != null)
                            {
                                m_StringBuilder.Append("<");
                                m_StringBuilder.Append(prop.Name);
                                m_StringBuilder.Append(">");

                                child_type = child_obj.GetType();
                                if (child_type == typeof(string))
                                {
                                    m_StringBuilder.Append(child_obj.ToString());
                                }
                                else if (child_type.IsClass || child_type.IsGenericType)
                                {
                                    ParseObjectToXml(m_StringBuilder, child_obj, m_PropertyType);
                                }
                                else
                                {
                                    m_StringBuilder.Append(child_obj.ToString());
                                }

                                m_StringBuilder.Append("</");
                                m_StringBuilder.Append(prop.Name);
                                m_StringBuilder.Append(">");
                            }
                        }
                    }

                    if (m_PropertyType == KParseObjToXmlPropertyType.All || m_PropertyType == KParseObjToXmlPropertyType.Field)
                    {
                        foreach (FieldInfo field in fields)
                        {
                            child_obj = field.GetValue(m_Object);
                            if (field.IsPublic && child_obj != null)
                            {
                                m_StringBuilder.Append("<");
                                m_StringBuilder.Append(field.Name);
                                m_StringBuilder.Append(">");


                                child_type = child_obj.GetType();
                                if (child_type == typeof(string))
                                {
                                    m_StringBuilder.Append(child_obj.ToString());
                                }
                                else if (child_type.IsClass || child_type.IsGenericType)
                                {
                                    ParseObjectToXml(m_StringBuilder, child_obj, m_PropertyType);
                                }
                                else
                                {
                                    m_StringBuilder.Append(child_obj.ToString());
                                }

                                m_StringBuilder.Append("</");
                                m_StringBuilder.Append(field.Name);
                                m_StringBuilder.Append(">");
                            }
                        }
                    }

                    m_StringBuilder.Append("</");
                    m_StringBuilder.Append(obj_type.Name);
                    m_StringBuilder.Append(">");

                    result = true;
                }
            }

            return result;
        }
        public static bool ParseObjectToXml(StringBuilder m_StringBuilder, Object m_Object)
        {
            return ParseObjectToXml(m_StringBuilder, m_Object, KParseObjToXmlPropertyType.All);
        }
    }
}

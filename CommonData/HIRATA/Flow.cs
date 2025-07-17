using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KgsCommon;
using System.IO;
using System.Text.RegularExpressions;

namespace CommonData.HIRATA
{
    public class Flow
    {
        public  Dictionary<int, List<AllDevice>> cv_LeftNormal = new Dictionary<int, List<AllDevice>>();
        public  Dictionary<int, List<AllDevice>> cv_LeftRework = new Dictionary<int, List<AllDevice>>();
        public  Dictionary<int, List<AllDevice>> cv_RightWafer = new Dictionary<int, List<AllDevice>>();
        public  Dictionary<int, List<AllDevice>> cv_RightGlass = new Dictionary<int, List<AllDevice>>();
        public  Dictionary<int, List<AllDevice>> cv_RightCombination = new Dictionary<int, List<AllDevice>>();
        public bool Parser(string m_flowRootPath , string m_FlowName)
        {
            bool rtn = true;
            if(m_flowRootPath != "" && m_FlowName != "")
            {
                string flowpath = m_flowRootPath + @"\" + m_FlowName +".xml";
                if (File.Exists(flowpath))
                {
                    KXmlItem flowxml = new KXmlItem();
                    flowxml.LoadFromFile(flowpath);
                    KXmlItem left = flowxml.ItemsByName["LefSteps"];
                    KXmlItem left_normal = flowxml.ItemsByName["NormalSteps"];
                    KXmlItem left_rework = flowxml.ItemsByName["ReworkSteps"];
                    KXmlItem right = flowxml.ItemsByName["RightSteps"];
                    KXmlItem right_glass = flowxml.ItemsByName["Glass"];
                    KXmlItem right_wafer = flowxml.ItemsByName["Wafer"];
                    KXmlItem right_combination = flowxml.ItemsByName["Combination"];
                    
                    if(!ParseToDic(left_normal, out cv_LeftNormal))
                    {
                        rtn = false;
                    }
                    if(rtn) 
                    {
                        if(!ParseToDic(left_rework, out cv_LeftRework))
                        {
                            rtn = false;
                        }
                    }
                    if (rtn)
                    {
                        if (!ParseToDic(right_glass, out cv_RightGlass))
                        {
                            rtn = false;
                        }
                    }
                    if (rtn)
                    {
                        if (!ParseToDic(right_wafer, out cv_RightWafer))
                        {
                            rtn = false;
                        }
                    }
                    if (rtn)
                    {
                        if (!ParseToDic(right_combination, out cv_RightCombination))
                        {
                            rtn = false;
                        }
                    }
                }
                else
                {
                    rtn = false;
                }
            }
            else
            {
                rtn = false;
            }
            return rtn;
        }
        private bool ParseToDic(KXmlItem m_Xml, out Dictionary<int, List<AllDevice>> m_Dic)
        {
            bool rtn = true;
            Dictionary<int, List<AllDevice>> tmp = new Dictionary<int, List<AllDevice>>();
            int itemnumber = m_Xml.ItemNumber;
            for (int i = 0; i < itemnumber; i++)
            {
                KXmlItem step = m_Xml.Items[i];
                int step_no = int.Parse(step.Attributes["Order"]);
                if(tmp.ContainsKey(step_no))
                {
                    rtn = false;
                    break;
                }
                string devicestr = step.AsString;
                List<string> devices = devicestr.Split(',').ToList();
                for(int devicenumber = 0; devicenumber<devices.Count; devicenumber++)
                {
                    AllDevice tmp2 = getDevice(devices[devicenumber]);
                    if(tmp2 == AllDevice.None)
                    {
                        rtn = false;
                        break;
                    }
                    else
                    {
                        if(tmp.ContainsKey(step_no))
                        {
                            tmp[step_no].Add(tmp2);
                        }
                        else
                        {
                            tmp.Add(step_no, new List<AllDevice>());
                            tmp[step_no].Add(tmp2);
                        }
                    }
                }
            }
            m_Dic = tmp;
            return rtn;
        }
        AllDevice getDevice(string m_Device)
        {
            AllDevice tmp = AllDevice.None;
            if (Regex.Match(m_Device, @"LP").Success)
            {
                tmp = AllDevice.LP;
            }
            else if (Regex.Match(m_Device, @"UP").Success)
            {
                tmp = AllDevice.UP;
            }
            else if (Regex.Match(m_Device, @"Buffer1").Success)
            {
                tmp = AllDevice.Buffer1;
            }
            else if (Regex.Match(m_Device, @"Buffer2").Success)
            {
                tmp = AllDevice.Buffer2;
            }
            else if (Regex.Match(m_Device, @"Aligner1").Success)
            {
                tmp = AllDevice.Aligner1;
            }
            else if (Regex.Match(m_Device, @"Aligner2").Success)
            {
                tmp = AllDevice.Aligner2;
            }
            else if (Regex.Match(m_Device, @"EQ").Success)
            {
                int eq_id = Convert.ToInt16(m_Device.Substring(2));
                EqId enumid = (EqId)eq_id;
                tmp = (AllDevice)Enum.Parse(typeof(AllDevice), enumid.ToString());
            }
            return tmp;
        }
        public string getLogStr()
        {
            /*
            cv_LeftNormal.Clear();
            cv_LeftRework.Clear();
            cv_RightWafer.Clear();
            cv_RightGlass.Clear();
            cv_RightCombination.Clear();
            */
            string log = "[cv_LeftNormal]" + Environment.NewLine;
            foreach (KeyValuePair<int, List<AllDevice>> item in cv_LeftNormal)
            {
                log += "Step : " + item.Key + " : ";
                foreach (AllDevice device_item in item.Value)
                {
                    log += device_item.ToString() + "  ";
                }
                log += Environment.NewLine;
            }
            log += Environment.NewLine;

            log += "[cv_LeftRework]" + Environment.NewLine;
            foreach (KeyValuePair<int, List<AllDevice>> item in cv_LeftRework)
            {
                log += "Step : " + item.Key + " : ";
                foreach (AllDevice device_item in item.Value)
                {
                    log += device_item.ToString() + "  ";
                }
                log += Environment.NewLine;
            }
            log += Environment.NewLine;

            log += "[cv_RightWafer]" + Environment.NewLine;
            foreach (KeyValuePair<int, List<AllDevice>> item in cv_RightWafer)
            {
                log += "Step : " + item.Key + " : ";
                foreach (AllDevice device_item in item.Value)
                {
                    log += device_item.ToString() + "  ";
                }
                log += Environment.NewLine;
            }
            log += Environment.NewLine;

            log += "[cv_RightGlass]" + Environment.NewLine; ;
            foreach (KeyValuePair<int, List<AllDevice>> item in cv_RightGlass)
            {
                log += "Step : " + item.Key + " : ";
                foreach (AllDevice device_item in item.Value)
                {
                    log += device_item.ToString() + "  ";
                }
                log += Environment.NewLine;
            }
            log += Environment.NewLine;

            log += "[cv_RightCombination]" + Environment.NewLine; ;
            foreach (KeyValuePair<int, List<AllDevice>> item in cv_RightCombination)
            {
                log += "Step : " + item.Key + " : ";
                foreach (AllDevice device_item in item.Value)
                {
                    log += device_item.ToString() + "  ";
                }
                log += Environment.NewLine;
            }
            log += Environment.NewLine;

            return log;
        }
        public void clearDic()
        {
            cv_LeftNormal.Clear();
            cv_LeftRework.Clear();
            cv_RightWafer.Clear();
            cv_RightGlass.Clear();
            cv_RightCombination.Clear();
        }
    }
}

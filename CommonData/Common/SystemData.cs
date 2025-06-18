using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KgsCommon;

namespace CommonData.HIRATA
{
    public class SystemData : CommonDatabase
    {
        public delegate void SendDataViaMmf();
        public event SendDataViaMmf OnRobot1StatusChange;
        public event SendDataViaMmf OnRobot2StatusChange;
        public event SendDataViaMmf OnSystemStatusChange;
        public SendDataViaMmf OnSystemDataChange
        {
            set;
            get;
        }

        //add
        public event SendDataViaMmf OnApiConnected;
        public event SendDataViaMmf OnApiDisconnected;
        public event SendDataViaMmf OnOperationModeChangeLeft;
        public event SendDataViaMmf OnOperationModeChangeRight;
        public event SendDataViaMmf OnPlcConnected;
        public event SendDataViaMmf OnPlcDisconnected;
        public event SendDataViaMmf OnBclive;
        public event SendDataViaMmf OnBcDie;

        public int cv_SystemStatus = 0;
        public CommonData.HIRATA.EquipmentStatus PSystemStatus
        {
            get { return (CommonData.HIRATA.EquipmentStatus)cv_SystemStatus; }
            set
            {
                if ((CommonData.HIRATA.EquipmentStatus)cv_SystemStatus != value)
                {
                    if (value == EquipmentStatus.WaitIdle)
                    {
                        PIdleTime = SysUtils.Now();
                        cv_SystemStatus = (int)value;
                    }
                    else
                    {
                        cv_SystemStatus = (int)value;
                    }
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                    if(OnSystemStatusChange != null)
                    {
                        OnSystemStatusChange();
                    }
                }
            }
        }

        public int cv_SystemOnlineMode = 0;
        public CommonData.HIRATA.OnlineMode PSystemOnlineMode
        {
            get { return (CommonData.HIRATA.OnlineMode)cv_SystemOnlineMode; }
            set
            {
                if ((CommonData.HIRATA.OnlineMode)cv_SystemOnlineMode != value)
                {
                    cv_SystemOnlineMode = (int)value;
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }

        public int cv_apiInlineMode = 0;
        public CommonData.HIRATA.EquipmentInlineMode PapiInlineMode
        {
            get { return (CommonData.HIRATA.EquipmentInlineMode)cv_apiInlineMode; }
            set
            {
                if ((CommonData.HIRATA.EquipmentInlineMode)cv_apiInlineMode != value)
                {
                     cv_apiInlineMode= (int)value;
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }

        public string cv_apiVersion = "";
        public string PapiVersion
        {
            get { return cv_apiVersion; }
            set
            {
                if (cv_apiVersion != value)
                {
                    cv_apiVersion = value;
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }

        public bool cv_apiConnect = false;
        public bool PapiConnect
        {
            get { return cv_apiConnect; }
            set
            {
                if (cv_apiConnect != value)
                {
                     cv_apiConnect= value;
                    if(value)
                    {
                        if(OnApiConnected != null)
                        {
                            OnApiConnected();
                        }
                    }
                    else
                    {
                        if(OnApiDisconnected != null)
                        {
                            OnApiDisconnected();
                        }
                    }
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }

        //robot 1 and left side.
        public int cv_Robot1Status = 0;
        public CommonData.HIRATA.EquipmentStatus PRobot1Status
        {
            get { return (CommonData.HIRATA.EquipmentStatus)cv_Robot1Status; }
            set
            {
                if ((CommonData.HIRATA.EquipmentStatus)cv_Robot1Status != value)
                {
                    cv_Robot1Status = (int)value;
                    /*
                    cv_SystemStatus = (int)value;
                    */
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                    if(OnRobot1StatusChange != null)
                    {
                        OnRobot1StatusChange();
                    }
                }
            }
        }

        public int cv_Robot1Speed = 0;
        public int PRobot1Speed
        {
            get { return cv_Robot1Speed; }
            set 
            {
                if (cv_Robot1Speed != value)
                {
                    cv_Robot1Speed = value;
                    SaveToFile();
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }

        public int cv_OperationModeLeft = 0;
        public CommonData.HIRATA.OperationMode POperationModeLeft
        {
            get { return (CommonData.HIRATA.OperationMode)cv_OperationModeLeft; }
            set
            {
                if ((CommonData.HIRATA.OperationMode)cv_OperationModeLeft != value)
                {
                    cv_OperationModeLeft = (int)value;
                    if(OnOperationModeChangeLeft != null)
                    {
                        OnOperationModeChangeLeft();
                    }
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }

        //robot 2 and right side.
        public int cv_Robot2Status = 0;
        public CommonData.HIRATA.EquipmentStatus PRobot2Status
        {
            get { return (CommonData.HIRATA.EquipmentStatus)cv_Robot2Status; }
            set
            {
                if ((CommonData.HIRATA.EquipmentStatus)cv_Robot2Status != value)
                {
                    cv_Robot2Status = (int)value;
                    /*
                    cv_SystemStatus = (int)value;
                    */
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                    if(OnRobot2StatusChange != null)
                    {
                        OnRobot2StatusChange();
                    }
                }
            }
        }

        public int cv_Robot2Speed = 0;
        public int PRobot2Speed
        {
            get { return cv_Robot2Speed; }
            set 
            {
                if (cv_Robot2Speed != value)
                {
                    cv_Robot2Speed = value;
                    SaveToFile();
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }

        public int cv_OperationModeRight = 0;
        public CommonData.HIRATA.OperationMode POperationModeRight
        {
            get { return (CommonData.HIRATA.OperationMode)cv_OperationModeRight; }
            set
            {
                if ((CommonData.HIRATA.OperationMode)cv_OperationModeRight != value)
                {
                    cv_OperationModeRight = (int)value;
                    if(OnOperationModeChangeRight != null)
                    {
                        OnOperationModeChangeRight();
                    }
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }

        public int cv_OcrMode = 0;
        public CommonData.HIRATA.OCRMode POcrMode
        {
            get { return (CommonData.HIRATA.OCRMode)cv_OcrMode; }
            set
            {
                if ((CommonData.HIRATA.OCRMode)cv_OcrMode != value)
                {
                    cv_OcrMode = (int)value;
                    if (cv_IsAutoSave)
                    {
                        SaveToFile();
                    }
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }

        public bool cv_IsPlcConnect = false;
        public bool PPlcConnect
        {
            get { return cv_IsPlcConnect; }
            set
            {
                if (cv_IsPlcConnect != value)
                {
                    cv_IsPlcConnect = value;
                    if(value)
                    {
                        if(OnPlcConnected != null)
                        {
                            OnPlcConnected();
                        }
                    }
                    else
                    {
                        if(OnPlcDisconnected != null)
                        {
                            OnPlcDisconnected();
                        }
                    }
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }

        public bool cv_IsBcAlive = false;
        public bool PBcAlive
        {
            get { return cv_IsBcAlive; }
            set
            {
                if (cv_IsBcAlive != value)
                {
                    cv_IsBcAlive = value;
                    if(value)
                    {
                        if(OnBclive != null)
                        {
                            OnBclive();
                        }
                    }
                    else
                    {
                        if(OnBcDie != null)
                        {
                            OnBcDie();
                        }
                    }
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }

        public bool cv_InitializeOkRight = false;
        public bool PInitaiizeOkRight
        {
            get { return cv_InitializeOkRight; }
            set
            {
                if (cv_InitializeOkRight != value)
                {
                    cv_InitializeOkRight = value;
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }

        public bool cv_InitializeOkLeft = false;
        public bool PInitaiizeOkLeft
        {
            get { return cv_InitializeOkLeft; }
            set
            {
                if (cv_InitializeOkLeft != value)
                {
                    cv_InitializeOkLeft = value;
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }


        public bool cv_InitializingRight = false;
        public bool PInitaiizingRight
        {
            get { return cv_InitializingRight; }
            set
            {
                if (cv_InitializingRight != value)
                {
                    cv_InitializingRight = value;
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }

        public bool PIsInInitialation
        {
            get { return PInitaiizingLeft || PInitaiizingRight; }
        }

        public enSideGroup PWhichSideInInitilation
        {
            get
            {
                enSideGroup side = enSideGroup.None;
                if(PInitaiizingRight && PInitaiizingLeft)
                {
                    side = enSideGroup.Both;
                }
                else if(PInitaiizingRight && (!PInitaiizingLeft))
                {
                    side = enSideGroup.Right;
                }
                else if(PInitaiizingLeft && (!PInitaiizingRight))
                {
                    side = enSideGroup.Left;
                }
                return side;
            }
        }

        public bool PIsForceInitial
        {
            get;
            set;
        }

        public bool cv_InitializingLeft = false;
        public bool PInitaiizingLeft
        {
            get { return cv_InitializingLeft; }
            set
            {
                if (cv_InitializingLeft != value)
                {
                    cv_InitializingLeft = value;
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }

        public bool cv_OntMode = true;
        public bool PONT
        {
            get { return cv_OntMode; }
            set
            {
                if (cv_OntMode != value)
                {
                    cv_OntMode = value;
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                        SaveToFile();
                    }
                }
            }
        }

        public int cv_FFUSpeed = 0;
        public int PFFUSpeed
        {
            get { return cv_FFUSpeed; }
            set 
            {
                if (cv_FFUSpeed != value)
                {
                    cv_FFUSpeed = value;
                    SaveToFile();
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }


        private KDateTime cv_IdleTime = SysUtils.Now();
        public KDateTime PIdleTime
        {
            get { return cv_IdleTime; }
            set { cv_IdleTime = value; }
        }

        public int cv_DataCheckRule = 0;
        public bool IsCheckRecipe
        {
            get { return (cv_DataCheckRule & 0x1) == 1 ? true : false; }
            set { PDataCheckRule = (cv_DataCheckRule & 0xFFFE) + Convert.ToInt16(value); }
        }
        public bool IsCheckId
        {
            get { return ((cv_DataCheckRule & 0x2) >> 1) == 1 ? true : false; }
            set { PDataCheckRule = (cv_DataCheckRule & 0xFFFD) + (Convert.ToInt16(value) << 1); }

        }
        public bool IsCheckSlot
        {
            get { return ((cv_DataCheckRule & 0x4) >> 2) == 1 ? true : false; }
            set { PDataCheckRule = (cv_DataCheckRule & 0xFFFB) + (Convert.ToInt16(value) << 2); }
        }
        public bool IsCheckSeq
        {
            get { return ((cv_DataCheckRule & 0x8) >> 3) == 1 ? true : false; }
            set { PDataCheckRule = (cv_DataCheckRule & 0xFFF7) + (Convert.ToInt16(value) << 3); }
        }
        public int PDataCheckRule
        {
            get { return cv_DataCheckRule; }
            set
            {
                if (cv_IsAutoSave)
                {
                    SaveToFile();
                }
                if (cv_DataCheckRule != value)
                {
                    cv_DataCheckRule = value;
                    if (OnSystemDataChange != null)
                    {
                        OnSystemDataChange();
                    }
                }
            }
        }


        public void Clone(SystemData m_OtherData)
        {
            cv_SystemStatus = m_OtherData.cv_SystemStatus;
            cv_SystemOnlineMode = m_OtherData.cv_SystemOnlineMode;
            cv_OperationModeLeft = m_OtherData.cv_OperationModeLeft;
            cv_OperationModeRight = m_OtherData.cv_OperationModeRight;
            cv_OcrMode = m_OtherData.cv_OcrMode;
            cv_IsPlcConnect = m_OtherData.cv_IsPlcConnect;
            cv_IsBcAlive = m_OtherData.cv_IsBcAlive;
            cv_apiVersion = m_OtherData.cv_apiVersion;
            cv_DataCheckRule = m_OtherData.cv_DataCheckRule;
            cv_InitializeOkRight = m_OtherData.cv_InitializeOkRight;
            cv_OntMode = m_OtherData.cv_OntMode;
            cv_Robot2Status = m_OtherData.cv_Robot2Status;
            cv_Robot1Status = m_OtherData.cv_Robot1Status;
            cv_Robot2Speed = m_OtherData.cv_Robot2Speed;
            cv_Robot1Speed = m_OtherData.cv_Robot1Speed;
            cv_apiConnect = m_OtherData.cv_apiConnect;
            cv_apiInlineMode = m_OtherData.cv_apiInlineMode;
        }
        public void LoadFromFile()
        {
            string ori_path = cv_FilePath;

            if (!string.IsNullOrEmpty(cv_FilePath))
            {
                KXmlItem recipe_xml = new KXmlItem();
                recipe_xml.LoadFromFile(cv_FilePath);
                if (recipe_xml.ItemsByName["System"].ItemType == KXmlItemType.itxList && recipe_xml.ItemsByName["System"].ItemNumber != 0)
                {
                    EventCenterBase.ParseXmlToObject(this, recipe_xml.ItemsByName["SystemData"]);
                }
            }
            PSystemStatus = EquipmentStatus.None;
            PSystemOnlineMode = OnlineMode.Offline;
            PRobot1Status = EquipmentStatus.Down;
            PRobot2Status = EquipmentStatus.Down;
            POperationModeLeft = OperationMode.Manual;
            POperationModeRight = OperationMode.Manual;
            PPlcConnect = false;
            PBcAlive = false;
            cv_apiVersion = "";
            cv_InitializeOkRight = false;
            cv_InitializingRight = false;
            if (cv_FilePath != ori_path)
            {
                cv_FilePath = ori_path;
            }

        }
        public void SaveToFile()
        {
            KXmlItem tmp = new KXmlItem();
            tmp.Text = "@<System/>";
            KXmlItem body = EventCenterBase.ParseObjectToKXmlItem(this, KParseObjToXmlPropertyType.Field);
            tmp.ItemsByName["System"].AddItem(body);
            lock (cv_obj)
            {
                try
                {
                    tmp.SaveToFile(cv_FilePath, true);
                }
                catch (Exception e)
                {
                }
            }
        }
        public void SetFilePath(string m_Path)
        {
            cv_FilePath = m_Path;
        }
    }
}

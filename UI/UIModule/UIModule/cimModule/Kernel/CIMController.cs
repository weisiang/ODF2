using System;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KgsCommon;
using System.Text.RegularExpressions;
using System.Collections;
using CommonData;
using CommonData.HIRATA;
using BaseAp;
using UI;
using LGC;

namespace CIM
{
    public partial class CIMController : BaseEventController
    {
        public static event deleSubscription eventCim;

        public KTimeCharts cv_TimeChart;
        public TimechartController cv_TimechartController;
        //KMemoryIOClient cv_Driver;

        public CIMController():base(FdModule.CIM)
        {
            CimModule.g_eventController = this;
            InitTimeChart();
        }
        ~CIMController()
        {
        }

        public override void linkEvent()
        {
            if (cv_module == FdModule.CIM)
            {
                LGCController.eventLgc += receiveSubcription;
                UIController.eventUi += receiveSubcription;
            }
            else if (cv_module == FdModule.UI)
            {
                LGCController.eventLgc += receiveSubcription;
                CIMController.eventCim += receiveSubcription;
            }
            else if (cv_module == FdModule.LGC)
            {
                CIMController.eventCim += receiveSubcription;
                UIController.eventUi += receiveSubcription;
            }
        }

        public static void triggerCimEvent(string messageId , Object obj)
        {
            if(eventCim != null)
            {
                eventCim(FdModule.CIM, messageId, obj);
                // CIMController.triggerCimEvent(typeof(CommonData.HIRATA.MDBCTimeAdjust).Name, obj); //time adjust
                //CIMController.triggerCimEvent(typeof(CommonData.HIRATA.MDBCMsg).Name, obj);        // show bc msg.
                //CIMController.triggerCimEvent(typeof(CommonData.HIRATA.PortData).Name, port_data); // data download.
                //CIMController.triggerCimEvent(typeof(CommonData.HIRATA.MDBCIdleDelayTime).Name, obj); // bc set idle interval.
                //CIMController.triggerCimEvent(typeof(CommonData.HIRATA.MDBCIndexInterval).Name, obj);  // bc set index interval.
                //CIMController.triggerCimEvent(typeof(CommonData.HIRATA.MDBCPortCommand).Name, obj);    //bc set port command.
                //CIMController.triggerCimEvent(typeof(CommonData.HIRATA.MDBCRecipeBodyQuery).Name, obj); // bc recipe body query.
                //CIMController.triggerCimEvent(typeof(CommonData.HIRATA.MDBCRecipeExist).Name, obj); // bc recipe exist.
                //CIMController.triggerCimEvent(typeof(CommonData.HIRATA.MDBCDataRequest).Name, cur_job); // bc reply data demand.
                //CIMController.triggerCimEvent(typeof(CommonData.HIRATA.MDBCAlarmReportToLGC).Name, obj); //report alarm to other modules.
            }
        }
        /*
        protected override void ProcessLgcStart(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            LinkCommonDataEvent(lgcBase.cv_Alarms, BaseForm.cv_AccountData, lgcBase.cv_Recipes,
                lgcBase.cv_TimeoutData, lgcBase.cv_GlassCountData, lgcBase.PSystemData);
        }
        */
        void InitTimeChart()
        {
            cv_TimechartController = new TimechartController(CommonData.HIRATA.CommonStaticData.g_RootConfigFolderPath + "\\" +
               CommonData.HIRATA.CommonStaticData.g_CimModule + "\\timecharts.xml");
            cv_TimeChart = cv_TimechartController.GetTimeChart();
            //cv_Driver = cv_TimechartController.GetmemoryIoClient();
        }

        public override void addSubScription()
        {
            base.addSubScription();
            #region from lgc.
            subscriptionMap.Add(typeof(CommonData.HIRATA.MDBCWorkTransferReport).Name, ProcessGlassDataTransferReport);
            subscriptionMap.Add(typeof(CommonData.HIRATA.MDBCLastProcessStartReport).Name, ProcessLastWorkProcessStart);
            subscriptionMap.Add(typeof(CommonData.HIRATA.MDBCOcrReadReport).Name, ProcessVCRReadOutReport);
            #endregion

            subscriptionMap.Add(typeof( CommonData.HIRATA.MDBCEquipmentInfo).Name , ProcessEquipmentInfo);
            subscriptionMap.Add(typeof(CommonData.HIRATA.MDBCRemoveReport).Name, ProcessWorkDataRemoveReport);
            subscriptionMap.Add(typeof(CommonData.HIRATA.MDBCWorkDataUpdateReport).Name, ProcessWorkDataUpdateReport);
            subscriptionMap.Add(typeof(CommonData.HIRATA.MDBCRecipeBodyReport).Name, ProcessRecipeBodyReport);
            subscriptionMap.Add(typeof(CommonData.HIRATA.MDBCRecipeExistReply).Name, ProcessEqRecipeExistReport);
            subscriptionMap.Add(typeof(CommonData.HIRATA.MDBCDataRequest).Name, ProcessWorkDataRequest);
        }

        #region process project's demand function. these function we didn't writted in BaseMmfController class. So don't use override.
        //Equipment status
        public void ProcessEquipmentInfo(FdModule m_SourceModule,string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CommonData.HIRATA.MDBCEquipmentInfo obj = m_Object as CommonData.HIRATA.MDBCEquipmentInfo;
            byte[] tmp = new byte[22];
            Array.Clear(tmp, 0, tmp.Length);

            int value = ((int)obj.POperationMode) + (((int)obj.PCimMode) << 4) + (((int)obj.PNodeNo) << 8);
            tmp[0] = Convert.ToByte(value & 0x00ff);
            tmp[1] = Convert.ToByte( (value & 0xff00) >> 8 );

            value = ( Convert.ToInt32( obj.PAlarm) << 12) + ( Convert.ToInt32( obj.PWarnning) <<8);
            tmp[3] = Convert.ToByte((value & 0xff00) >> 8);

            value = ( Convert.ToInt32( obj.PIdleDelayTime) ) + ( Convert.ToInt32( obj.PIndexInterval) <<12);
            tmp[4] = Convert.ToByte(value & 0x00ff);
            tmp[5] = Convert.ToByte((value & 0xff00) >> 8);

            value = (Convert.ToInt32(obj.PCurrentRecipe));
            tmp[6] = Convert.ToByte(value & 0x00ff);
            tmp[7] = Convert.ToByte((value & 0xff00) >> 8);

            value = (Convert.ToInt32(obj.PCheckRecipe) << 3) + (Convert.ToInt32(obj.PCheckFoupSeq) << 2) + (Convert.ToInt32(obj.PCheckWorkSlot) << 1) + Convert.ToInt32(obj.PCheckWorkId);
            tmp[16] = Convert.ToByte(value & 0x00ff);

            value = (Convert.ToInt32(obj.POcr1Enable) << 4) + ((int)obj.POcr1Mode);
            tmp[18] = Convert.ToByte(value & 0x00ff);
            tmp[19] = Convert.ToByte((value & 0xff00) >> 8);

            value = (Convert.ToInt32(obj.POcr2Enable) << 4) + ((int)obj.POcr2Mode);
            tmp[20] = Convert.ToByte(value & 0x00ff);
            tmp[21] = Convert.ToByte((value & 0xff00) >> 8);

            //cv_Driver.SetBinaryLengthData(0x3444, tmp, 11, false);
            CimModule.PMio.SetBinaryLengthData(0x3444, tmp, 11, false);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        public void ProcessGlassDataTransferReport(FdModule m_SourceModule,string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            CommonData.HIRATA.MDBCWorkTransferReport obj = m_Object as CommonData.HIRATA.MDBCWorkTransferReport;
            if(obj.PAction == CommonData.HIRATA.DataFlowAction.Fetch)
            cv_TimechartController.GetTimeChartInstance(TimechartEqReceiveReport.TIMECHART_ID_EqFetchReport).AddJob(m_Object);
            else if (obj.PAction == CommonData.HIRATA.DataFlowAction.Store)
            cv_TimechartController.GetTimeChartInstance(TimechartEqReceiveReport.TIMECHART_ID_EqStoreReport).AddJob(m_Object);
            else if (obj.PAction == CommonData.HIRATA.DataFlowAction.Receive)
            cv_TimechartController.GetTimeChartInstance(TimechartEqReceiveReport.TIMECHART_ID_EqReceiveReport).AddJob(m_Object);
            else if (obj.PAction == CommonData.HIRATA.DataFlowAction.Send)
            cv_TimechartController.GetTimeChartInstance(TimechartEqReceiveReport.TIMECHART_ID_EqSendReport).AddJob(m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void ProcessWorkDataRemoveReport(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            cv_TimechartController.GetTimeChartInstance(TimechartEqFetchReport.TIMECHART_ID_EqWorkDataRemoveReport).AddJob(m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void ProcessWorkDataUpdateReport(FdModule m_SourceModule,  string m_MessageId , Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            cv_TimechartController.GetTimeChartInstance(TimechartEqFetchReport.TIMECHART_ID_EqWorkDataUpdateReport).AddJob(m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void ProcessLastWorkProcessStart(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            cv_TimechartController.GetTimeChartInstance(TimechartEqFetchReport.TIMECHART_ID_EqLastWorkProcessStartReport).AddJob(m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void ProcessEqRecipeExistReport(FdModule m_SourceModule, string m_MessageId,Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            cv_TimechartController.GetTimeChartInstance(TimechartEqRecipeBodyReport.TIMECHART_ID_EqRecipeExistReport).AddJob(m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void ProcessRecipeBodyReport(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            cv_TimechartController.GetTimeChartInstance(TimechartEqRecipeBodyReport.TIMECHART_ID_EqRecipeBodyReport).AddJob(m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void ProcessVCRReadOutReport(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            cv_TimechartController.GetTimeChartInstance(TimechartEqVCRReadReport.TIMECHART_ID_EqVCRReadReport).AddJob(m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void ProcessWorkDataRequest(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            //CommonData.HIRATA.FAKE tmp = new FAKE();
            //SendMmfReplyObject(typeof(CommonData.HIRATA.FAKE).Name, tmp, m_Ticket, typeof(CommonData.HIRATA.MDBCDataRequest).Name , KParseObjToXmlPropertyType.Field);
            cv_TimechartController.GetTimeChartInstance(TimechartEqVCRReadReport.TIMECHART_ID_EqWorkDataRequest).AddJob(m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }

        #endregion
    }
}

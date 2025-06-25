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
            }
        }
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
            subscriptionMap.Add(typeof(CommonData.HIRATA.MDBCWorkTransferReport).Name, ProcessGlassDataTransferReport);//ok
            subscriptionMap.Add(typeof(CommonData.HIRATA.MDBCLastProcessStartReport).Name, ProcessLastWorkProcessStart);//ok
            subscriptionMap.Add(typeof(CommonData.HIRATA.MDBCRecipeBodyReport).Name, ProcessRecipeBodyReport); //ok
            subscriptionMap.Add(typeof(CommonData.HIRATA.MDBCRecipeExistReply).Name, ProcessEqRecipeExistReport);//ok
            subscriptionMap.Add(typeof(CommonData.HIRATA.MDBCOcrReadReport).Name, ProcessVCRReadOutReport); //ok
            subscriptionMap.Add(typeof(CommonData.HIRATA.MDBCRemoveReport).Name, ProcessWorkDataRemoveReport);//ok
            subscriptionMap.Add(typeof(CommonData.HIRATA.MDBCDataRequest).Name, ProcessWorkDataRequest); //ok
            subscriptionMap.Add(typeof(CommonData.HIRATA.MDBCWorkDataUpdateReport).Name, ProcessWorkDataUpdateReport);
        }

        #region process project's demand function. these function we didn't writted in BaseMmfController class. So don't use override.
        //Equipment status
        void ProcessGlassDataTransferReport(FdModule m_SourceModule,string m_MessageId, Object m_Object)
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
        void ProcessLastWorkProcessStart(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            cv_TimechartController.GetTimeChartInstance(TimechartEqFetchReport.TIMECHART_ID_EqLastWorkProcessStartReport).AddJob(m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void ProcessRecipeBodyReport(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            cv_TimechartController.GetTimeChartInstance(TimechartEqRecipeBodyReport.TIMECHART_ID_EqRecipeBodyReport).AddJob(m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void ProcessEqRecipeExistReport(FdModule m_SourceModule, string m_MessageId,Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            cv_TimechartController.GetTimeChartInstance(TimechartEqRecipeBodyReport.TIMECHART_ID_EqRecipeExistReport).AddJob(m_Object);
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Leave);
        }
        void ProcessVCRReadOutReport(FdModule m_SourceModule, string m_MessageId, Object m_Object)
        {
            WriteLog(LogLevelType.NormalFunctionInOut, this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name, CommonData.HIRATA.FunInOut.Enter);
            cv_TimechartController.GetTimeChartInstance(TimechartEqVCRReadReport.TIMECHART_ID_EqVCRReadReport).AddJob(m_Object);
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

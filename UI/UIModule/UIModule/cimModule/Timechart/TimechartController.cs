using System;
using System.Collections.Generic;
using System.Text;
using KgsCommon;

namespace CIM
{
    class TimechartController : TimechartControllerBase
    {
        public TimechartController(string m_TimechartXmlPathname)
            : base(m_TimechartXmlPathname)
        {
            Dictionary<string, int> port_map;
            TimechartInstanceBase instance;

            port_map = new Dictionary<string, int>();
            instance = new TimechartBcDateTimeCalibration(this, TimechartBcDateTimeCalibration.TIMECHART_ID_BcDateTimeCalibration, port_map);
            AddTimechartInstance(instance.TimechartId, instance);

            port_map = new Dictionary<string, int>();
            instance = new TimechartBcDisplayMessage(this, TimechartBcDisplayMessage.TIMECHART_ID_BcDisplayMessage, port_map);
            AddTimechartInstance(instance.TimechartId, instance);

            port_map = new Dictionary<string, int>();
            instance = new TimechartBcFoupDataDownload(this, TimechartBcFoupDataDownload.TIMECHART_ID_BcFoupDataDownload, port_map);
            AddTimechartInstance(instance.TimechartId, instance);

            port_map = new Dictionary<string, int>();
            instance = new TimechartBcIdleDelayCommand(this, TimechartBcIdleDelayCommand.TIMECHART_ID_BcIdleDelayCommand, port_map);
            AddTimechartInstance(instance.TimechartId, instance);

            port_map = new Dictionary<string, int>();
            instance = new TimechartBcIndexIntervalCommand(this, TimechartBcIndexIntervalCommand.TIMECHART_ID_BcIndexIntervalCommand, port_map);
            AddTimechartInstance(instance.TimechartId, instance);

            port_map = new Dictionary<string, int>();
            instance = new TimechartBcPortCommand(this, TimechartBcPortCommand.TIMECHART_ID_BcPortCommand, port_map);
            AddTimechartInstance(instance.TimechartId, instance);

            port_map = new Dictionary<string, int>();
            instance = new TimechartBcRecipeBodyQuery(this, TimechartBcRecipeBodyQuery.TIMECHART_ID_BcRecipeBodyQuery, port_map);
            AddTimechartInstance(instance.TimechartId, instance);

            port_map = new Dictionary<string, int>();
            instance = new TimechartBcRecipeExistCommand(this, TimechartBcRecipeExistCommand.TIMECHART_ID_BcRecipeExistCommand, port_map);
            AddTimechartInstance(instance.TimechartId, instance);

            port_map = new Dictionary<string, int>();
            instance = new TimechartEqAlarmReport(this, TimechartEqAlarmReport.TIMECHART_ID_EqAlarmReport, port_map);
            AddTimechartInstance(instance.TimechartId, instance);

            port_map = new Dictionary<string, int>();
            instance = new TimechartEqFetchReport(this, TimechartEqFetchReport.TIMECHART_ID_EqFetchReport, port_map);
            AddTimechartInstance(instance.TimechartId, instance);

            port_map = new Dictionary<string, int>();
            instance = new TimechartEqLastWorkProcessStartReport(this, TimechartEqLastWorkProcessStartReport.TIMECHART_ID_EqLastWorkProcessStartReport, port_map);
            AddTimechartInstance(instance.TimechartId, instance);

            port_map = new Dictionary<string, int>();
            instance = new TimechartEqReceiveReport(this, TimechartEqReceiveReport.TIMECHART_ID_EqReceiveReport, port_map);
            AddTimechartInstance(instance.TimechartId, instance);

            port_map = new Dictionary<string, int>();
            instance = new TimechartEqRecipeBodyReport(this, TimechartEqRecipeBodyReport.TIMECHART_ID_EqRecipeBodyReport, port_map);
            AddTimechartInstance(instance.TimechartId, instance);

            port_map = new Dictionary<string, int>();
            instance = new TimechartEqRecipeExistReport(this, TimechartEqRecipeExistReport.TIMECHART_ID_EqRecipeExistReport, port_map);
            AddTimechartInstance(instance.TimechartId, instance);

            port_map = new Dictionary<string, int>();
            instance = new TimechartEqSendReport(this, TimechartEqSendReport.TIMECHART_ID_EqSendReport, port_map);
            AddTimechartInstance(instance.TimechartId, instance);

            port_map = new Dictionary<string, int>();
            instance = new TimechartEqStoreReport(this, TimechartEqStoreReport.TIMECHART_ID_EqStoreReport, port_map);
            AddTimechartInstance(instance.TimechartId, instance);

            port_map = new Dictionary<string, int>();
            instance = new TimechartEqVCRReadReport(this, TimechartEqVCRReadReport.TIMECHART_ID_EqVCRReadReport, port_map);
            AddTimechartInstance(instance.TimechartId, instance);

            port_map = new Dictionary<string, int>();
            instance = new TimechartEqWorkDataRemoveReport(this, TimechartEqWorkDataRemoveReport.TIMECHART_ID_EqWorkDataRemoveReport, port_map);
            AddTimechartInstance(instance.TimechartId, instance);

            port_map = new Dictionary<string, int>();
            instance = new TimechartEqWorkDataRequest(this, TimechartEqWorkDataRequest.TIMECHART_ID_EqWorkDataRequest, port_map);
            AddTimechartInstance(instance.TimechartId, instance);

            port_map = new Dictionary<string, int>();
            instance = new TimechartEqWorkDataUpdateReport(this, TimechartEqWorkDataUpdateReport.TIMECHART_ID_EqWorkDataUpdateReport, port_map);
            AddTimechartInstance(instance.TimechartId, instance);

            port_map = new Dictionary<string, int>();
            instance = new TimechartRecipeListReport(this, TimechartRecipeListReport.TIMECHART_ID_EqRecipeListReport, port_map);
            AddTimechartInstance(instance.TimechartId, instance);

            this.Open();
        }

        public int IntervalTime
        {
            set { TimechartInstanceBase.cv_IndexDelay = value;}
            get { return TimechartInstanceBase.cv_IndexDelay ; }
        }
        public int TsTime
        {
            set { TimechartInstanceBase.cv_Ts = value;}
            get { return TimechartInstanceBase.cv_Ts ; }
        }
        public int TeTime
        {
            set { TimechartInstanceBase.cv_Te = value; }
            get { return TimechartInstanceBase.cv_Te; }
        }
        public int TmTime
        {
            set { TimechartInstanceBase.cv_Tm = value; }
            get { return TimechartInstanceBase.cv_Tm; }
        }
    }
}

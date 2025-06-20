using System;
using System.Collections.Generic;
using System.Text;
using KgsCommon;
using CommonData.HIRATA;

namespace LGC
{
    public class TimechartController : TimechartControllerBase
    {
        public TimechartController(string m_TimechartXmlPathname)
            : base(m_TimechartXmlPathname)
        {
            Dictionary<string, int> port_map;
            TimechartInstanceBase instance;

            for (int i = (int)EqGifTimeChartId.TIMECHART_ID_SDP1; i <= (int)EqGifTimeChartId.TIMECHART_ID_UV_2; i++ )
            {
                port_map = new Dictionary<string, int>();
                instance = new TimechartNormal(this, i , port_map);
                AddTimechartInstance(instance.TimechartId, instance);
            }
            this.Open();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NFTIntegration.Data
{
    public class ZapAlerts
    {
        public string Risk { get; set; } = "";
        public int Alerts { get; set; } = 0;

        public List<ZapAlerts> getZapAlerts()
        {
            var zapAlerts = new List<ZapAlerts>();

            var recentAlert = new DataAdapter().GetRecentZapReport();

            zapAlerts.Add(new ZapAlerts() { Risk = "High", Alerts = recentAlert[0].High });
            zapAlerts.Add(new ZapAlerts() { Risk = "Medium", Alerts = recentAlert[0].Medium });
            zapAlerts.Add(new ZapAlerts() { Risk = "Low", Alerts = recentAlert[0].Low });
            zapAlerts.Add(new ZapAlerts() { Risk = "Information", Alerts = recentAlert[0].Information });

            return zapAlerts;
        }

    }
}

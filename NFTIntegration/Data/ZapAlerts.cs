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

        public List<ZapAlerts> GetZapAlerts()
        {
            var zapAlerts = new List<ZapAlerts>();

            var recentAlert = new DataAdapter().GetLastRunZapReport();

            zapAlerts.Add(new ZapAlerts() { Risk = "High", Alerts = recentAlert[0].High });
            zapAlerts.Add(new ZapAlerts() { Risk = "Medium", Alerts = recentAlert[0].Medium });
            zapAlerts.Add(new ZapAlerts() { Risk = "Low", Alerts = recentAlert[0].Low });
            zapAlerts.Add(new ZapAlerts() { Risk = "Information", Alerts = recentAlert[0].Information });

            return zapAlerts;
        }

        public List<ZapAlerts> GetZapAlerts(string zapid)
        {
            var zapAlerts = new List<ZapAlerts>();

            var recentAlert = new DataAdapter().GetZapReportDetails(zapid);

            zapAlerts.Add(new ZapAlerts() { Risk = "High", Alerts = recentAlert.High });
            zapAlerts.Add(new ZapAlerts() { Risk = "Medium", Alerts = recentAlert.Medium });
            zapAlerts.Add(new ZapAlerts() { Risk = "Low", Alerts = recentAlert.Low });
            zapAlerts.Add(new ZapAlerts() { Risk = "Information", Alerts = recentAlert.Information });

            return zapAlerts;
        }

    }
}

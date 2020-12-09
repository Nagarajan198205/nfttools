using System.Collections.Generic;

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

            zapAlerts.Add(new ZapAlerts { Risk = "High", Alerts = recentAlert.High });
            zapAlerts.Add(new ZapAlerts { Risk = "Medium", Alerts = recentAlert.Medium });
            zapAlerts.Add(new ZapAlerts { Risk = "Low", Alerts = recentAlert.Low });
            zapAlerts.Add(new ZapAlerts { Risk = "Information", Alerts = recentAlert.Information });

            return zapAlerts;
        }

        public List<ZapAlerts> GetZapAlerts(string reportid)
        {
            var zapAlerts = new List<ZapAlerts>();

            var recentAlert = new DataAdapter().GetZapReportDetails(reportid);

            zapAlerts.Add(new ZapAlerts { Risk = "High", Alerts = recentAlert.High });
            zapAlerts.Add(new ZapAlerts { Risk = "Medium", Alerts = recentAlert.Medium });
            zapAlerts.Add(new ZapAlerts { Risk = "Low", Alerts = recentAlert.Low });
            zapAlerts.Add(new ZapAlerts { Risk = "Information", Alerts = recentAlert.Information });

            return zapAlerts;
        }

    }
}

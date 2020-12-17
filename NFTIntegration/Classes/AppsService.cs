using NFTIntegration.Data;
using NFTIntegration.Models;
using NFTIntegration.Models.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFTIntegration.Classes
{
    public interface IAppsService
    {
        Task CreateProject(CreateProjectModel model);
        Task<List<Project>> GetProjectList(int userId);
        Task<List<ZapAlerts>> GetZapAlerts(User user);
        Task<List<ZapAlerts>> GetZapAlerts(User user, int projectId);
        Task<List<ZapAlerts>> GetZapAlerts(string reportid, User user);
        Task<List<ReportData>> GetDastReportListByProject(User user, int projectId);
        Task<List<ProjectWiseIssues>> GetIssuesCountByProject(User user);
        Task<List<ReportData>> GetDastReportList(User user);
        Task<List<ReportData>> GetDastReportListForReport(User user);
    }

    public class AppsService : IAppsService
    {
        public AppsService()
        { }

        public async Task CreateProject(CreateProjectModel model)
        {
            await Task.Run(() => new DataAdapter().CreateProject(model)).ConfigureAwait(false);
        }

        public async Task<List<Project>> GetProjectList(int userId)
        {
            return await Task.Run(() => new DataAdapter().GetProjectList(userId)).ConfigureAwait(false);
        }

        public async Task<List<ZapAlerts>> GetZapAlerts(User user,int projectId)
        {
            var zapAlerts = new List<ZapAlerts>();

            var recentAlert = new ReportData();

            if (user.Role.ToUpper().Equals("TESTER"))
            {
                recentAlert = await Task.Run(() => new DataAdapter().GetLastRunZapReport(user.UserId, projectId));
            }
            else
            {
                recentAlert = new DataAdapter().GetLastRunZapReport(projectId);
            }

            zapAlerts.Add(new ZapAlerts { Risk = "High", Alerts = recentAlert.High });
            zapAlerts.Add(new ZapAlerts { Risk = "Medium", Alerts = recentAlert.Medium });
            zapAlerts.Add(new ZapAlerts { Risk = "Low", Alerts = recentAlert.Low });
            zapAlerts.Add(new ZapAlerts { Risk = "Information", Alerts = recentAlert.Information });

            return zapAlerts;
        }

        public async Task<List<ZapAlerts>> GetZapAlerts(User user)
        {
            var zapAlerts = new List<ZapAlerts>();

            var recentAlert = new ReportData();

            if (user.Role.ToUpper().Equals("TESTER"))
            {
                recentAlert = await Task.Run(() => new DataAdapter().GetLastRunZapReport(user.UserId.ToString()));
            }
            else
            {
                recentAlert = new DataAdapter().GetLastRunZapReport();
            }

            zapAlerts.Add(new ZapAlerts { Risk = "High", Alerts = recentAlert.High });
            zapAlerts.Add(new ZapAlerts { Risk = "Medium", Alerts = recentAlert.Medium });
            zapAlerts.Add(new ZapAlerts { Risk = "Low", Alerts = recentAlert.Low });
            zapAlerts.Add(new ZapAlerts { Risk = "Information", Alerts = recentAlert.Information });

            return zapAlerts;
        }

        public async Task<List<ProjectWiseIssues>> GetIssuesCountByProject(User user)
        {
            var issueList = new List<ProjectWiseIssues>();

            if (user.Role.ToUpper().Equals("TESTER"))
            {
                issueList = await Task.Run(() => new DataAdapter().GetIssuesCountByProject(user.UserId));
            }
            else
            {
                issueList = new DataAdapter().GetIssuesCountByProject();
            }

            return issueList;
        }

        public async Task<List<ZapAlerts>> GetZapAlerts(string reportid, User user)
        {
            var zapAlerts = new List<ZapAlerts>();
            var recentAlert = new ReportData();

            if (user.Role.ToUpper().Equals("TESTER"))
            {
                recentAlert = await Task.Run(() => new DataAdapter().GetZapReportDetails(reportid, user.UserId));
            }
            else
            {
                recentAlert = await Task.Run(() => new DataAdapter().GetZapReportDetails(reportid));
            }

            zapAlerts.Add(new ZapAlerts { Risk = "High", Alerts = recentAlert.High });
            zapAlerts.Add(new ZapAlerts { Risk = "Medium", Alerts = recentAlert.Medium });
            zapAlerts.Add(new ZapAlerts { Risk = "Low", Alerts = recentAlert.Low });
            zapAlerts.Add(new ZapAlerts { Risk = "Information", Alerts = recentAlert.Information });

            return zapAlerts;
        }

        public async Task<List<ReportData>> GetDastReportListByProject(User user,int projectId)
        {
            var reportList = new List<ReportData>();

            if (user.Role.ToUpper().Equals("TESTER"))
            {
                reportList = await Task.Run(() => new DataAdapter().GetDastReportListByProject(user.UserId, projectId));
            }
            else
            {
                reportList = await Task.Run(() => new DataAdapter().GetDastReportListByProject(projectId));
            }

            return reportList;
        }

        public async Task<List<ReportData>> GetDastReportList(User user)
        {
            var reportList = new List<ReportData>();

            if (user.Role.ToUpper().Equals("TESTER"))
            {
                reportList = await Task.Run(() => new DataAdapter().GetDastReportList(user.UserId));
            }
            else
            {
                reportList = await Task.Run(() => new DataAdapter().GetDastReportList());
            }

            return reportList;
        }

        public async Task<List<ReportData>> GetDastReportListForReport(User user)
        {
            var reportList = new List<ReportData>();

            if (user.Role.ToUpper().Equals("TESTER"))
            {
                reportList = await Task.Run(() => new DataAdapter().GetDastReportListForReport(user.UserId));
            }
            else
            {
                reportList = await Task.Run(() => new DataAdapter().GetDastReportListForReport());
            }

            return reportList;
        }
    }
}

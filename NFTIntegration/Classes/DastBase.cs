using Microsoft.AspNetCore.Components;
using NFTIntegration.Data;
using NFTIntegration.Model;
using NFTIntegration.Models.Account;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace NFTIntegration.Classes
{
    public class DastBase : ComponentBase
    {

        protected bool IsScanning { get; set; }
        protected string ReportFileContent { get; set; }
        protected DastModel ZapModel;
        protected bool IsLoaded = false;
        protected string ErrorMessage = string.Empty;

        [Parameter]
        public string ProjectId { get; set; }
        protected Projects ProjectList { get; set; }

        [Inject]
        public ILocalStorageService LocalStorageService { get; set; }

        public DastBase()
        {
        }

        protected override async Task OnInitializedAsync()
        {
            await Task.Run(() => GetLatestRunReport());
        }

        private async Task InitalizeDastTool()
        {
            if (!IsLoaded)
            {
                IsLoaded = true;

                await Task.Run(() =>
                {
                    var fileToRun = $"{Directory.GetCurrentDirectory()}\\Tools\\zap\\zaprun.bat";
                    Process process = new Process();

                    process.StartInfo.FileName = fileToRun;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.WorkingDirectory = Path.GetDirectoryName(fileToRun);
                    process.Start();

                    System.Threading.Thread.Sleep(10000);

                    //process.WaitForExit(); 
                });
            }
        }

        private void GetLatestRunReport()
        {
            if (!IsLoaded)
            {
                var reportFileName = new DataAdapter().GetLastRunZapReport()?.ReportFileName;
                var filePath = $"{Directory.GetCurrentDirectory()}\\Reports\\{reportFileName}";

                if (File.Exists(filePath))
                {
                    var htmlString = File.ReadAllText(filePath);
                    ReportFileContent = NormalizeReport(htmlString);
                }

                ZapModel = new DastModel();
            }
        }

        public string GetRawZapHTMLReport(string reportId)
        {
            var reportFileName = new DataAdapter().GetZapReportDetails(reportId);
            var reportDetails = string.Empty;

            var filePath = $"{Directory.GetCurrentDirectory()}\\Reports\\{reportFileName.ReportFileName}";

            if (File.Exists(filePath))
            {
                var htmlString = File.ReadAllText(filePath);
                reportDetails = NormalizeReportDetails(htmlString);
            }

            return reportDetails;
        }

        public async Task HandleValidSubmit()
        {
            try
            {
                ReportFileContent = string.Empty;

                IsScanning = true;
                StateHasChanged();

                var zapClient = new DastClient();

                var user = await LocalStorageService.GetItem<User>("user");

                await Task.Run(() => zapClient.Scan(ZapModel.Url, user.UserId, ZapModel.ProjectId));

                ReportFileContent = zapClient.ReportFileContent;

                IsScanning = false;
                ErrorMessage = string.Empty;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                IsScanning = false;
                ErrorMessage = ex.Message;
            }
        }

        private string NormalizeReport(string reportDetails)
        {
            var details = reportDetails.Replace("#info", "apps/dast#info").Replace("#low", "apps/dast#low").Replace("#medium", "apps/dast#medium").Replace("#high", "apps/dast#high");
            //remove logo
            details = details.Replace(details.Substring(details.IndexOf("<img"), details.IndexOf("ggg==") + 7 - details.IndexOf("<img")), string.Empty);
            //rename report name
            details = details.Replace("ZAP Scanning Report", "DAST Scanning Report");

            return details;
        }

        private string NormalizeReportDetails(string reportDetails)
        {
            var details = reportDetails.Replace("#info", "apps/dast#info").Replace("#low", "apps/dast#low").Replace("#medium", "apps/dast#medium").Replace("#high", "apps/dast#high");
            //remove logo
            details = details.Replace(details.Substring(details.IndexOf("<img"), details.IndexOf("ggg==") + 7 - details.IndexOf("<img")), string.Empty);
            //rename report name
            details = details.Replace("ZAP Scanning Report", string.Empty);

            //remove Alert Summary
            details = details.Replace(details.Substring(details.IndexOf("<h3>"), details.IndexOf("</h3>") - details.IndexOf("<h3>")), string.Empty);
            details = details.Replace(details.Substring(details.IndexOf("<table"), details.IndexOf("</table>") - details.IndexOf("<table")), string.Empty);

            return details;
        }
    }
}

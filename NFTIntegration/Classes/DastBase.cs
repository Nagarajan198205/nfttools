using Microsoft.AspNetCore.Components;
using NFTIntegration.Data;
using NFTIntegration.Model;
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

        public DastBase()
        {
        }

        protected override async Task OnInitializedAsync()
        {
            //await Task.WhenAll(InitalizeZapTool, GetLatestRunReport);
            await Task.Run(() => Parallel.Invoke(() => InitalizeDastTool(), () => GetLatestRunReport()));
        }

        private void InitalizeDastTool()
        {
            var fileToRun = $"{Directory.GetCurrentDirectory()}\\Tools\\zap\\zaprun.bat";
            Process process = new Process();

            process.StartInfo.FileName = fileToRun;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WorkingDirectory = Path.GetDirectoryName(fileToRun);
            // Run the process and wait for it to complete
            process.Start();
            //process.WaitForExit();
            ZapModel = new DastModel();
        }

        private void GetLatestRunReport()
        {
            var reportFileName = new DataAdapter().GetLastRunZapReport()?.ReportFileName;
            var filePath = $"{Directory.GetCurrentDirectory()}\\Reports\\{reportFileName}";

            if (File.Exists(filePath))
            {
                var htmlString = File.ReadAllText(filePath);
                ReportFileContent = NormalizeReport(htmlString);
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
            ReportFileContent = string.Empty;

            IsScanning = true;
            StateHasChanged();

            var zapClient = new DastClient();

            await Task.Run(() => zapClient.Scan(ZapModel.Url));

            ReportFileContent = zapClient.ReportFileContent;

            IsScanning = false;
            StateHasChanged();
        }

        private string NormalizeReport(string reportDetails)
        {
            var details = reportDetails.Replace("#info", "dast#info").Replace("#low", "dast#low").Replace("#medium", "dast#medium").Replace("#high", "dast#high");
            //remove logo
            details = details.Replace(details.Substring(details.IndexOf("<img"), details.IndexOf("ggg==") + 7 - details.IndexOf("<img")), string.Empty);
            //rename report name
            details = details.Replace("ZAP Scanning Report", "DAST Scanning Report");

            return details;
        }

        private string NormalizeReportDetails(string reportDetails)
        {
            var details = reportDetails.Replace("#info", "dast#info").Replace("#low", "dast#low").Replace("#medium", "dast#medium").Replace("#high", "dast#high");
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

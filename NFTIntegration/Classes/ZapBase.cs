using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using NFTIntegration.Data;
using NFTIntegration.Model;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace NFTIntegration.Classes
{
    public class ZapBase : ComponentBase
    {

        protected bool IsScanning { get; set; }
        protected string ReportFileContent { get; set; }
        protected ZapModel ZapModel;
        protected bool IsLoaded = false;

        public ZapBase()
        {

        }

        protected override async Task OnInitializedAsync()
        {
            //await Task.WhenAll(InitalizeZapTool, GetLatestRunReport);
            await Task.Run(() => Parallel.Invoke(() => InitalizeZapTool(), () => GetLatestRunReport()));
        }

        private void InitalizeZapTool()
        {
            var sessionPath = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["SessionPath"];
            var attributes = $"-daemon -newsession {sessionPath}_{DateTime.Now:ddMMyyyyHHmmss}";
            var processList = Process.GetProcessesByName("java");

            if (processList.Length == 0)
            {
                var shell = System.Management.Automation.PowerShell.Create();
                var currentDir = $"{Directory.GetCurrentDirectory()}\\Tools\\zap\\";
                var driveLabel = currentDir.Substring(0, currentDir.IndexOf(":") + 1);

                shell.Commands.AddScript(driveLabel);
                shell.Commands.AddScript($"cd {currentDir}");
                shell.Commands.AddScript($"zap.bat {attributes}");

                shell.Invoke();                 
            }

            ZapModel = new ZapModel();
        }

        private void GetLatestRunReport()
        {
            var reportFileName = new DataAdapter().GetLastRunZapReport()?.ReportFileName;
            ReportFileContent = File.ReadAllText($"{Directory.GetCurrentDirectory()}\\Reports\\{reportFileName}");
        }

        public string GetRawZapHTMLReport(string reportID)
        {
            var reportFileName = new DataAdapter().GetZapReportDetails(reportID);

            var htmlText = "";

            var borderLine = "<tr valign=\"top\">"
                             + "   <td colspan=\"2\"></td>"
                             + "</tr >";

            using (StreamReader sr = new StreamReader($"{System.IO.Directory.GetCurrentDirectory()}\\Reports\\{reportFileName.ReportFileName}"))
            {
                String line;
                Boolean tablesFound = false;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains("class=\"results\"") || tablesFound)
                    {
                        tablesFound = true;

                        if (line.Contains("</body>"))
                        {
                            break;
                        }

                        var htmlLine = line.Contains("</tr>")
                            ? line.ToString() + Environment.NewLine + borderLine
                            : line.ToString();

                        htmlText = htmlText == ""
                            ? htmlLine
                            : htmlText + Environment.NewLine + htmlLine;
                    }
                }
            }
            return htmlText;
        }

        public async Task HandleValidSubmit()
        {
            ReportFileContent = string.Empty;

            IsScanning = true;
            StateHasChanged();

            var zapClient = new ZAPClient();

            await Task.Run(() => zapClient.Scan(ZapModel.Url));

            ReportFileContent = zapClient.ReportFileContent;

            IsScanning = false;
            StateHasChanged();
        }
    }
}

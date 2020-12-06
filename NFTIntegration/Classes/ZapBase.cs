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
            var executableFile = $"{System.IO.Directory.GetCurrentDirectory()}\\Tools\\zap\\zap.bat";
            var attributes = $"-daemon -newsession {sessionPath}_{DateTime.Now:ddMMyyyyHHmmss}";

            var processList = Process.GetProcessesByName("java");

            if (processList.Length == 0)
            {

                using var process = new Process();
                process.StartInfo.FileName = executableFile;
                process.StartInfo.Arguments = attributes;
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                System.Threading.Thread.Sleep(10000);
            }

            ZapModel = new ZapModel();
        }

        private void GetLatestRunReport()
        {
            var reportFileName = new DataAdapter().GetLastRunZapReport()[0].ReportFileName;
            ReportFileContent = File.ReadAllText($"{System.IO.Directory.GetCurrentDirectory()}\\Reports\\{reportFileName}");
        }

        public void GetHTMLReport(string reportFileNamePath)
        {
            ReportFileContent = File.ReadAllText(reportFileNamePath);
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

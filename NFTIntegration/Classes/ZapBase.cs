using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using NFTIntegration.Data;
using NFTIntegration.Model;
using System;
using System.Diagnostics;
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
            await Task.Run(InitalizeZapTool); 
        }

        private async Task InitalizeZapTool()
        {
            var sessionPath = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["SessionPath"];
            var executableFile = $"{System.IO.Directory.GetCurrentDirectory()}\\Tools\\zap\\zap.bat";
            var attributes = $"-cmd -quickprogress -newsession {sessionPath}_{DateTime.Now:ddMMyyyyHHmmss}";

            var processInfo = new ProcessStartInfo(executableFile, attributes);
            processInfo.WindowStyle = ProcessWindowStyle.Hidden;

            var runningProcessByName = Process.GetProcessesByName("iexplore");
            if (runningProcessByName.Length == 0)
            {
                var shell = System.Management.Automation.PowerShell.Create();
                var currentDir = $"{System.IO.Directory.GetCurrentDirectory()}\\Tools\\zap\\";
                var driveLabel = currentDir.Substring(0, currentDir.IndexOf(":")+1);

                shell.Commands.AddScript(driveLabel);
                await shell.InvokeAsync().ConfigureAwait(true);

                shell.Commands.AddScript($"{currentDir}zap.bat {attributes}");

                await shell.InvokeAsync().ConfigureAwait(true);
            }

            System.Threading.Thread.Sleep(10000);

            ZapModel = new ZapModel();
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

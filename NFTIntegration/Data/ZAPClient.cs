using OWASPZAPDotNetAPI;
using System;
using System.Collections.Generic;
using System.IO;

namespace NFTIntegration.Data
{
    public class ZAPClient
    {

        private static string _apikey = "u6gcd2d204ab86qt6mciducp4i";
        private ClientApi _api = new ClientApi("localhost", 8090, _apikey);
        private IApiResponse _apiResponse;
        private string _target = string.Empty;
        private string ReportFileName { get; set; }
        public string ReportFileContent { get; set; }

        public void Scan(string targetUrl)
        {
            _target = targetUrl;
            ReportFileName = $"{Directory.GetCurrentDirectory()}\\Reports\\report-{DateTime.Now:dd-MMM-yyyy-hh-mm-ss}";

            string spiderScanId = StartSpidering();
            PollTheSpiderTillCompletion(spiderScanId);

            StartAjaxSpidering();
            PollTheAjaxSpiderTillCompletion();

            string activeScanId = StartActiveScanning();
            PollTheActiveScannerTillCompletion(activeScanId);

           // WriteXmlReport(ReportFileName);
            WriteHtmlReport(ReportFileName);
            PrintAlertsToConsole();
        }

        private void ShutdownZAP()
        {
            _apiResponse = _api.core.shutdown();
            if ("OK" == ((ApiResponseElement)_apiResponse).Value)
                Console.WriteLine("ZAP shutdown success " + _target);
        }

        private void PrintAlertsToConsole()
        {
            List<Alert> alerts = _api.GetAlerts(_target, 0, 0, string.Empty);
            foreach (var alert in alerts)
            {
                Console.WriteLine(alert.AlertMessage
                    + Environment.NewLine
                    + alert.CWEId
                    + Environment.NewLine
                    + alert.Url
                    + Environment.NewLine
                    + alert.WASCId
                    + Environment.NewLine
                    + alert.Evidence
                    + Environment.NewLine
                    + alert.Parameter
                    + Environment.NewLine
                );
            }
        }

        private void WriteHtmlReport(string reportFileName)
        {
            File.WriteAllBytes(reportFileName + ".html", _api.core.htmlreport());

            ReportFileContent = File.ReadAllText(reportFileName + ".html");
        }

        private void WriteXmlReport(string reportFileName)
        {
            File.WriteAllBytes(reportFileName + ".xml", _api.core.xmlreport());
        }

        private void PollTheActiveScannerTillCompletion(string activeScanId)
        {
            int activeScannerprogress;
            while (true)
            {
                activeScannerprogress = int.Parse(((ApiResponseElement)_api.ascan.status(activeScanId)).Value);
                Console.WriteLine("Active scanner progress: {0}%", activeScannerprogress);
                if (activeScannerprogress >= 100)
                    break;
            }
            Console.WriteLine("Active scanner complete");
        }

        private string StartActiveScanning()
        {
            Console.WriteLine("Active Scanner: " + _target);
            _apiResponse = _api.ascan.scan(_target, "", "", "", "", "", "");

            string activeScanId = ((ApiResponseElement)_apiResponse).Value;
            return activeScanId;
        }

        private void PollTheAjaxSpiderTillCompletion()
        {
            try
            {
                while (true)
                {
                    string ajaxSpiderStatusText = Convert.ToString(((ApiResponseElement)_api.ajaxspider.status()).Value);
                    if (ajaxSpiderStatusText.IndexOf("running", StringComparison.InvariantCultureIgnoreCase) > -1)
                        Console.WriteLine("Ajax Spider running");
                    else
                        break;
                }

                Console.WriteLine("Ajax Spider complete");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ajax Spider: " + ex.Message);
            }
        }

        private void StartAjaxSpidering()
        {
            try
            {
                Console.WriteLine("Ajax Spider: " + _target);
                _apiResponse = _api.ajaxspider.scan(_target, "", "", "");

                if ("OK" == ((ApiResponseElement)_apiResponse).Value)
                    Console.WriteLine("Ajax Spider started for " + _target);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ajax Spider: " + ex.Message);
            }
        }

        private void PollTheSpiderTillCompletion(string scanid)
        {
            int spiderProgress;
            while (true)
            {
                spiderProgress = int.Parse(((ApiResponseElement)_api.spider.status(scanid)).Value);
                Console.WriteLine("Spider progress: {0}%", spiderProgress);
                if (spiderProgress >= 100)
                    break;
            }

            Console.WriteLine("Spider complete");
        }

        private string StartSpidering()
        {
            Console.WriteLine("Spider: " + _target);
            _apiResponse = _api.spider.scan(_target, "", "", "", "");
            string scanid = ((ApiResponseElement)_apiResponse).Value;
            return scanid;
        }

        private void LoadTargetUrlToSitesTree()
        {
            _api.AccessUrl(_target);
        }
    }
}
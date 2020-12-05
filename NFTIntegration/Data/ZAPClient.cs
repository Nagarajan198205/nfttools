using OWASPZAPDotNetAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NFTIntegration.Data
{
    public class ZAPClient
    {

        private static string _apikey = "u6gcd2d204ab86qt6mciducp4i";
        private ClientApi _api = new ClientApi("localhost", 8090, _apikey);
        private IApiResponse _apiResponse;
        private string _target = string.Empty;
        private string _reportFilePath;
        private string _reportFileName;
        private string _runDate;
        private DateTime _dtRunDate;
        public string ReportFileContent { get; set; }


        public void Scan(string targetUrl)
        {
            _target = targetUrl;

            _dtRunDate = DateTime.Now;

            _runDate = _dtRunDate.ToString("dd-MMM-yyyy hh:mm:ss");
            _reportFileName = $"zapreport-{_dtRunDate:dd-MMM-yyyy-hh-mm-ss}.html";
            _reportFilePath = $"{Directory.GetCurrentDirectory()}\\Reports\\{_reportFileName}";

            string spiderScanId = StartSpidering();

            if (!string.IsNullOrEmpty(spiderScanId))
            {
                PollTheSpiderTillCompletion(spiderScanId);
            }

            StartAjaxSpidering();
            PollTheAjaxSpiderTillCompletion();

            string activeScanId = StartActiveScanning();
            if (!string.IsNullOrEmpty(activeScanId))
            {
                PollTheActiveScannerTillCompletion(activeScanId);
            }

            WriteHtmlReport(_reportFilePath);
            SaveAlertDetails();
        }

        private void SaveAlertDetails()
        {
            List<Alert> alerts = _api.GetAlerts(_target, 0, 0, string.Empty);

            if (alerts.Count > 0)
            {
                var low = alerts.Where(x => x.Risk == Alert.RiskLevel.Low).GroupBy(x => x.AlertMessage).Count();
                var medium = alerts.Where(x => x.Risk == Alert.RiskLevel.Medium).GroupBy(x => x.AlertMessage).Count();
                var high = alerts.Where(x => x.Risk == Alert.RiskLevel.High).GroupBy(x => x.AlertMessage).Count();
                var informational = alerts.Where(x => x.Risk == Alert.RiskLevel.Informational).GroupBy(x => x.AlertMessage).Count();

                var reportId = Convert.ToInt64(_dtRunDate.ToString("yyyyMMddHHmmss"));

                new DataAdapter().AddReportDetails(new ReportData
                {
                    ReportId = reportId,
                    RunDate = _runDate,
                    ReportFileName = _reportFileName,
                    High = high,
                    Medium = medium,
                    Low = low,
                    Information = informational
                });
            }
        }

        private void WriteHtmlReport(string reportFilePath)
        {
            File.WriteAllBytes(reportFilePath, _api.core.htmlreport());

            ReportFileContent = File.ReadAllText(reportFilePath);
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
            try
            {
                Console.WriteLine("Active Scanner: " + _target);
                _apiResponse = _api.ascan.scan(_target, "", "", "", "", "", "");

                string activeScanId = ((ApiResponseElement)_apiResponse).Value;
                return activeScanId;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
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
            try
            {
                Console.WriteLine("Spider: " + _target);
                _apiResponse = _api.spider.scan(_target, "", "", "", "");
                string scanid = ((ApiResponseElement)_apiResponse).Value;
                return scanid;
            }
            catch
            {
                return string.Empty;
            }
        }

        private void LoadTargetUrlToSitesTree()
        {
            _api.AccessUrl(_target);
        }
    }
}
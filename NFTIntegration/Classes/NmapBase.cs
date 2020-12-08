using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Management.Automation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NFTIntegration.Classes
{
    public class NmapBase : ComponentBase
    {
        public bool Disabled { get; set; } = true;
        public string Output { get; set; } = string.Empty;
        public string Input { get; set; } = string.Empty;
        public bool IsFirstTimeExecution { get; set; } = false;
        public string processStyle { get; set; }

        private List<string> _history = new List<string>();
        private int _historyIndex = 0;

        public NmapBase()
        {
        }

        public void OnKeyDown(KeyboardEventArgs e)
        {

            if (e.Key == "ArrowUp" && _historyIndex > 0)
            {
                _historyIndex--;
                Input = _history[_historyIndex];
            }
            else if (e.Key == "ArrowDown" && _historyIndex + 1 < _history.Count)
            {
                _historyIndex++;
                Input = _history[_historyIndex];
            }

            else if (e.Key == "Escape")
            {
                Input = "";
                _historyIndex = _history.Count;
            }
        }

        public async Task Run(KeyboardEventArgs e)
        {
            if (e.Key != "Enter")
            {
                return;
            }

            var code = Input;
            if (!string.IsNullOrEmpty(code))
            {
                _history.Add(code);
            }
            _historyIndex = _history.Count;
            Input = "";

            processStyle = "cursor-progress";

            await RunSubmission(code);

            processStyle = "cursor-default";
        }

        private async Task RunSubmission(string cmdWithArgs)
        {


            Output += $@"<br /><span class=""info"">{System.Web.HttpUtility.HtmlEncode(cmdWithArgs)}</span>";

            var previousOut = Console.Out;

            try
            {
                var writer = new System.IO.StringWriter();
                Console.SetOut(writer);

                if (cmdWithArgs.ToUpper().Equals("CLS") || cmdWithArgs.ToUpper().Equals("CLEAR"))
                {
                    Output = String.Empty;
                    return;
                }


                var shell = System.Management.Automation.PowerShell.Create();

                if (cmdWithArgs.Contains("nmap"))
                {
                    var currentDir = $"{System.IO.Directory.GetCurrentDirectory()}\\Tools\\nmap\\";
                    shell.Commands.AddScript($"{currentDir}{cmdWithArgs}");
                }
                else
                {
                    shell.Commands.AddScript(cmdWithArgs);
                }

                shell.Streams.Error.DataAdded += Error_DataAdded;
                shell.Streams.Warning.DataAdded += Warning_DataAdded;
                shell.Streams.Information.DataAdded += Information_DataAdded;

                var results = await shell.InvokeAsync().ConfigureAwait(false);

                if (results.Count > 0)
                {
                    var builder = new StringBuilder();

                    foreach (var psObject in results)
                    {
                        builder.AppendLine(psObject.BaseObject.ToString());
                    }

                    Output += $@"<br /><span>{System.Web.HttpUtility.HtmlEncode(builder.ToString())}</span>";
                }

            }
            catch (Exception ex)
            {
                Output += $@"<br /><span class=""error"">{System.Web.HttpUtility.HtmlEncode(ex.Message)}</span>";
            }
            finally
            {
                Console.SetOut(previousOut);
            }
        }

        private void Information_DataAdded(object sender, DataAddedEventArgs e)
        {
            var streamObjectsReceived = sender as PSDataCollection<InformationRecord>;
            var currentStreamRecord = streamObjectsReceived[e.Index];

            //Output += currentStreamRecord.MessageData.ToString();
            Output += $@"<br /><span class=""info"">{System.Web.HttpUtility.HtmlEncode(currentStreamRecord.MessageData.ToString())}</span>";
        }

        private void Warning_DataAdded(object sender, DataAddedEventArgs e)
        {
            var streamObjectsReceived = sender as PSDataCollection<WarningRecord>;
            var currentStreamRecord = streamObjectsReceived[e.Index];

            //Output += currentStreamRecord.Message;
            Output += $@"<br /><span class=""info"">{System.Web.HttpUtility.HtmlEncode(currentStreamRecord.Message)}</span>";
        }

        private void Error_DataAdded(object sender, DataAddedEventArgs e)
        {
            var streamObjectsReceived = sender as PSDataCollection<ErrorRecord>;
            var currentStreamRecord = streamObjectsReceived[e.Index];

            //Output += currentStreamRecord.Exception.Message;
            Output += $@"<br /><span class=""error"">{System.Web.HttpUtility.HtmlEncode(currentStreamRecord.Exception.Message)}</span>";
        }

        public void CaptureScreenShot()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                }
                bitmap.Save("C://test.jpg", ImageFormat.Jpeg);
            }


        }
    }
}

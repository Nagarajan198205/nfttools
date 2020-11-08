using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;

namespace NFTIntegration.Data
{
    public class ProcessInfo
    {
        private RunspacePool RsPool { get; set; }
        public string ProcessOutput { get; set; }

        public void InitializeRunspaces(int minRunspaces, int maxRunspaces, string[] modulesToLoad)
        {
            var defaultSessionState = InitialSessionState.CreateDefault();
            defaultSessionState.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.Unrestricted;

            foreach (var moduleName in modulesToLoad)
            {
                defaultSessionState.ImportPSModule(moduleName);
            }

            RsPool = RunspaceFactory.CreateRunspacePool(defaultSessionState);
            RsPool.SetMinRunspaces(minRunspaces);
            RsPool.SetMaxRunspaces(maxRunspaces);

            RsPool.ThreadOptions = PSThreadOptions.UseNewThread;

            RsPool.Open();
        }

        public async Task RunScript(string scriptContents, Dictionary<string, object> scriptParameters)
        {
            if (RsPool == null)
            {
                throw new ApplicationException("Runspace Pool must be initialized before calling RunScript().");
            }

            using (PowerShell ps = PowerShell.Create())
            {
                ps.RunspacePool = RsPool;

                ps.AddScript(scriptContents);

                ps.AddParameters(scriptParameters);

                ps.Streams.Error.DataAdded += Error_DataAdded;
                ps.Streams.Warning.DataAdded += Warning_DataAdded;
                ps.Streams.Information.DataAdded += Information_DataAdded;

                var pipelineObjects = await ps.InvokeAsync().ConfigureAwait(false);

                foreach (var item in pipelineObjects)
                {
                    ProcessOutput += item.BaseObject.ToString() + Environment.NewLine;
                }
            }
        }

        private void Information_DataAdded(object sender, DataAddedEventArgs e)
        {
            var streamObjectsReceived = sender as PSDataCollection<InformationRecord>;
            var currentStreamRecord = streamObjectsReceived[e.Index];

            //Console.WriteLine($"InfoStreamEvent: {currentStreamRecord.MessageData}");
            ProcessOutput = currentStreamRecord.MessageData.ToString();
            //Console.SetOut(new System.IO.StreamWriter(currentStreamRecord.MessageData.ToString()));
        }

        private void Warning_DataAdded(object sender, DataAddedEventArgs e)
        {
            var streamObjectsReceived = sender as PSDataCollection<WarningRecord>;
            var currentStreamRecord = streamObjectsReceived[e.Index];

            //Console.WriteLine($"WarningStreamEvent: {currentStreamRecord.Message}");
            ProcessOutput = currentStreamRecord.Message;
            //Console.SetOut(new System.IO.StreamWriter(currentStreamRecord.Message));
        }

        private void Error_DataAdded(object sender, DataAddedEventArgs e)
        {
            var streamObjectsReceived = sender as PSDataCollection<ErrorRecord>;
            var currentStreamRecord = streamObjectsReceived[e.Index];

            //Console.WriteLine($"ErrorStreamEvent: {currentStreamRecord.Exception}");
            ProcessOutput = currentStreamRecord.Exception.Message;
        }

        public Int32 RunProcess(string path, string args, Action<string> onStdOut = null, Action<string> onStdErr = null)
        {
            var readStdOut = onStdOut != null;
            var readStdErr = onStdErr != null;

            var process = new Process
            {
                StartInfo =
            {
                FileName = path,
                Arguments = args,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = readStdOut,
                RedirectStandardError = readStdErr,
                WindowStyle= ProcessWindowStyle.Hidden,
                WorkingDirectory = Directory.GetCurrentDirectory()
            }
            };

            process.Start();

            if (readStdOut) Task.Run(() => ReadStream(process.StandardOutput, onStdOut));
            if (readStdErr) Task.Run(() => ReadStream(process.StandardError, onStdErr));

            process.WaitForExit();

            return process.ExitCode;
        }

        private static void ReadStream(TextReader textReader, Action<String> callback)
        {
            while (true)
            {
                var line = textReader.ReadLine();
                if (line == null)
                    break;

                callback(line);
            }
        }
    }
}

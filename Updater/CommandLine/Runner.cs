using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Updater.CommandLine
{
    internal class Runner : IDisposable
    {
        private DirectoryInfo mTargetDirectory;
        private FileInfo mArchiveFile;
        private FileInfo mCallerFile;
        private Guid mInstanceId;

        public RunnerLog Log { get; set; }

        public Runner(string[] arguments)
        {
            string documentFile = arguments[0];

            if (File.Exists(documentFile))
            {
                XDocument document;

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationFlags = System.Xml.Schema.XmlSchemaValidationFlags.None;
                settings.XmlResolver = null;

                using (FileStream fs = new FileStream(documentFile, FileMode.Open, FileAccess.Read))
                {
                    using (XmlReader reader = XmlReader.Create(fs, settings))
                    {
                        document = XDocument.Load(reader);
                    }
                }

                XElement xUpdate = document.Root.Element("update");
                this.mTargetDirectory = new DirectoryInfo(xUpdate.Element("applicationDirectory").Value);
                this.mArchiveFile = new FileInfo(xUpdate.Element("archiveFile").Value);
                this.mInstanceId = new Guid(xUpdate.Element("instanceId").Value);
                this.mCallerFile = new FileInfo(xUpdate.Element("callerFile").Value);
            }
        }

        internal void Run()
        {
            Task task = new Task(this.RunInTask);
            task.Start();
        }

        private void RunInTask()
        {
            if (this.WaitForCallerExit(this.mCallerFile))
            {
            }
        }

        private bool WaitForCallerExit(FileInfo callerFile)
        {
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddSeconds(10);
            TimeSpan spanTotal = endTime.Subtract(startTime);
            bool result = false;

            while (DateTime.Now < endTime)
            {
                TimeSpan spanFromStart = DateTime.Now.Subtract(startTime);

                this.Log(
                    Stage.WaitForCallerExit,
                    String.Format("{0}: {1}...", Stage.WaitForCallerExit, "Waiting for caller exit"));

                // Try open for write
                try
                {
                    using (FileStream fs = callerFile.Open(FileMode.Open, FileAccess.Write, FileShare.None))
                    {
                        result = true;
                        break;
                    }
                }
                catch// (Exception exc)
                {
                }

                Thread.Sleep(500);
            }

            if (result)
            {
                this.Log(
                    Stage.WaitForCallerExit,
                    String.Format("{0}: Ok.", Stage.WaitForCallerExit));
            }
            else
            {
                this.Log(
                    Stage.WaitForCallerExit,
                    String.Format("{0}: Failed.", Stage.WaitForCallerExit));
            }

            return result;
        }

        public void Dispose()
        {
        }

        public enum Stage
        {
            Parsing,
            WaitForCallerExit
        }

        public delegate bool RunnerLog(Stage stage, string message);
    }
}

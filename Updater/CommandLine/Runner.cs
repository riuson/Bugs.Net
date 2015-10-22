using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Updater.Tarx;

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
            try
            {
                if (this.WaitForCallerExit(this.mCallerFile))
                {
                    if (this.RemoveFiles(this.mTargetDirectory))
                    {
                        this.Unpack(this.mTargetDirectory, this.mArchiveFile);
                    }
                }
            }
            catch (Exception exc)
            {
                this.Log(
                    Stage.Removing,
                    String.Format("Removing failed: {0}{1}{2}",
                        exc.Message,
                        Environment.NewLine,
                        exc.StackTrace),
                    Color.Red);
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
                    "Waiting for caller exit",
                    Color.Blue);

                if (this.CanWrite(callerFile))
                {
                    result = true;
                    break;
                }

                Thread.Sleep(500);
            }

            if (result)
            {
                this.Log(
                    Stage.WaitForCallerExit,
                    "Ok.",
                    Color.Green);
            }
            else
            {
                this.Log(
                    Stage.WaitForCallerExit,
                    "Failed.",
                    Color.Red);
            }

            return result;
        }

        private bool CanWrite(FileInfo file)
        {
            bool result;

            // Try open for write
            try
            {
                using (FileStream fs = file.Open(FileMode.Open, FileAccess.Write, FileShare.None))
                {
                    result = true;
                }
            }
            catch (Exception exc)
            {
                // Cannot delete
                //this.Log(
                //    Stage.Removing,
                //    String.Format("Removing failed: {0}{1}{2}",
                //        exc.Message,
                //        Environment.NewLine,
                //        exc.StackTrace));
                result = false;
            }

            return result;
        }

        private bool RemoveFiles(DirectoryInfo directory)
        {
            this.Log(Stage.Removing, String.Format("Removing (*.dll, *.exe, *.pdb) files in target directory: {0}...", directory), Color.Blue);

            var files = directory.GetFiles("*.*", SearchOption.AllDirectories)
                .Where(file => file.Extension == ".dll" || file.Extension == ".exe" || file.Extension == ".pdb");

            bool canDeleteAll = files.AsParallel().All(file =>
                {
                    // Check access for write (delete)
                    return this.CanWrite(file);
                });

            if (canDeleteAll)
            {
                files.AsParallel().ForAll(file =>
                    {
                        try
                        {
                            this.Log(Stage.Removing, String.Format("{0}...", file), Color.Gray);
                            file.Delete();
                        }
                        catch (Exception exc)
                        {
                            this.Log(
                                Stage.Removing,
                                String.Format("Removing failed: {0}{1}{2}",
                                    exc.Message,
                                    Environment.NewLine,
                                    exc.StackTrace),
                                Color.Red);
                        }
                    });

                this.Log(Stage.Removing, "Completed.", Color.Green);
            }
            else
            {
                this.Log(Stage.Removing, "Cannot get write access for all files. Removing canceled.", Color.Red);
            }

            return true;
        }

        private void Unpack(DirectoryInfo targetDirectory, FileInfo sourceFile)
        {
            using (FileStream fs = new FileStream(sourceFile.FullName, FileMode.Open, FileAccess.Read))
            {
                using (GZipStream gs = new GZipStream(fs, CompressionMode.Decompress))
                {
                    using (Unpacker unpacker = new Unpacker(gs, this.UnpackerLog, null))
                    {
                        XDocument xHeader = unpacker.XHeader;

                        this.Log(Stage.Unpacking, "Starting...", Color.Blue);

                        unpacker.UnpackTo(targetDirectory, item =>
                        {
                            this.Log(Stage.Unpacking, String.Format("{0}...", item.Element("path").Value), Color.Gray);
                            return true;
                        });

                        this.Log(Stage.Unpacking, "Completed.", Color.Green);
                    }
                }
            }
        }

        public void Dispose()
        {
        }

        private void UnpackerLog(string message)
        {
            this.Log(Stage.Unpacking, message, Color.Gray);
        }


        public enum Stage
        {
            Parsing,
            WaitForCallerExit,
            Removing,
            Unpacking
        }

        public delegate bool RunnerLog(Stage stage, string message, Color color);
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    internal class Updater : IDisposable
    {
        private DirectoryInfo mTargetDirectory;
        private FileInfo mArchiveFile;
        private FileInfo mAppStarterFile;
        private Guid mInstanceId;

        public RunnerLog Log { get; set; }
        public Action Completed { get; set; }
        public Action Failed { get; set; }

        public Updater(string[] arguments)
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
                this.mAppStarterFile = new FileInfo(xUpdate.Element("appStarterFile").Value);
            }
        }

        internal void Start()
        {
            Task<Boolean> task = new Task<Boolean>(this.ProcessInTask);
            task.ContinueWith((t) =>
                {
                    Boolean result = (Boolean)t.Result;

                    if (result)
                    {
                        if (this.Completed != null)
                        {
                            this.Completed();
                        }
                    }
                    else
                    {
                        if (this.Failed != null)
                        {
                            this.Failed();
                        }
                    }
                });
            task.Start();
        }

        private Boolean ProcessInTask()
        {
            try
            {
                bool result = false;

                if (this.WaitForAppExit(this.mAppStarterFile))
                {
                    if (this.CanRemoveFiles(this.mTargetDirectory))
                    {
                        if (this.RemoveFiles(this.mTargetDirectory))
                        {
                            if (this.Unpack(this.mTargetDirectory, this.mArchiveFile))
                            {
                                if (this.RunAppStarter(this.mAppStarterFile))
                                {
                                    result = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        this.Log(Stage.Removing, "Cannot get write access for all files. Removing canceled.", Color.Red);
                        this.RunAppStarter(this.mAppStarterFile);
                        return false;
                    }
                }

                if (result)
                {
                    this.Log(Stage.Completing, "Procedure completed successfully", Color.DarkGreen);
                }
                else
                {
                    this.Log(Stage.Completing, "Procedure failed", Color.DarkRed);
                }

                Thread.Sleep(2000);

                return result;
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
                Thread.Sleep(2000);
                return false;
            }
        }

        private bool WaitForAppExit(FileInfo appStarterFile)
        {
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddSeconds(10);
            TimeSpan spanTotal = endTime.Subtract(startTime);
            bool result = false;

            while (DateTime.Now < endTime)
            {
                TimeSpan spanFromStart = DateTime.Now.Subtract(startTime);

                this.Log(
                    Stage.WaitForAppExit,
                    "Waiting for application exit...",
                    Color.Blue);

                if (this.CanWrite(appStarterFile, false))
                {
                    result = true;
                    break;
                }

                Thread.Sleep(500);
            }

            if (result)
            {
                this.Log(
                    Stage.WaitForAppExit,
                    "Ok.",
                    Color.Green);
            }
            else
            {
                this.Log(
                    Stage.WaitForAppExit,
                    "Failed.",
                    Color.Red);
            }

            return result;
        }

        private bool CanWrite(FileInfo file, bool showLog)
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
                if (showLog)
                {
                    this.Log(
                        Stage.Removing,
                        String.Format("Access denied to {0}:{2}{1}",
                            file,
                            exc.Message,
                            Environment.NewLine),
                        Color.Red);
                }
                result = false;
            }

            return result;
        }

        private IEnumerable<FileInfo> GetFilesToRemove(DirectoryInfo directory)
        {
            var files = directory.GetFiles("*.*", SearchOption.AllDirectories)
                .Where(file => file.Extension == ".dll" || file.Extension == ".exe" || file.Extension == ".pdb");

            return files;
        }

        private bool CanRemoveFiles(DirectoryInfo directory)
        {
            var files = this.GetFilesToRemove(directory);
            bool canRemoveAll = files.AsParallel().All(file =>
            {
                // Check access for write (delete)
                return this.CanWrite(file, true);
            });
            return canRemoveAll;
        }

        private bool RemoveFiles(DirectoryInfo directory)
        {
            bool result = true;
            this.Log(Stage.Removing, String.Format("Removing (*.dll, *.exe, *.pdb) files in target directory: {0}...", directory), Color.Blue);

            var files = this.GetFilesToRemove(directory);
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
                        result = false;
                    }
                });

            this.Log(Stage.Removing, "Completed.", Color.Green);
            return result;
        }

        private bool Unpack(DirectoryInfo targetDirectory, FileInfo sourceFile)
        {
            bool result = false;

            using (FileStream fs = new FileStream(sourceFile.FullName, FileMode.Open, FileAccess.Read))
            {
                using (GZipStream gs = new GZipStream(fs, CompressionMode.Decompress))
                {
                    using (Unpacker unpacker = new Unpacker(gs, this.UnpackerLog, null))
                    {
                        XDocument xHeader = unpacker.XHeader;

                        this.Log(Stage.Unpacking, "Starting...", Color.Blue);

                        result = unpacker.UnpackTo(targetDirectory, item =>
                        {
                            this.Log(Stage.Unpacking, String.Format("{0}...", item.Element("path").Value), Color.Gray);
                            return true;
                        });

                        this.Log(Stage.Unpacking, "Completed.", Color.Green);
                    }
                }
            }

            return result;
        }

        private bool RunAppStarter(FileInfo appStarterFile)
        {
            this.Log(Stage.Starting, "Starting application...", Color.Blue);

            if (appStarterFile.Exists)
            {
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo(appStarterFile.FullName);
                process.StartInfo.WorkingDirectory = Path.GetDirectoryName(appStarterFile.FullName);

                if (process.Start())
                {
                    this.Log(Stage.Starting, "Application started.", Color.Green);
                    return true;
                }
            }

            this.Log(Stage.Starting, "Application not started.", Color.Red);

            return false;
        }

        public void Dispose()
        {
        }

        private void UnpackerLog(string message)
        {
            this.Log(Stage.Unpacking, message, Color.Gray);
        }

        public delegate bool RunnerLog(Stage stage, string message, Color color);
    }

    public enum Stage
    {
        Parsing,
        WaitForAppExit,
        Removing,
        Unpacking,
        Starting,
        Completing
    }
}

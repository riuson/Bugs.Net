using AppCore;
using AppCore.Classes;
using AppCore.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Updater.Classes;
using Updater.Events;

namespace Updater.FileSystem.Classes
{
    internal class FileUpdater : IDisposable
    {
        private IApplication mApp;
        private Task<Updater.Classes.DownloadResult> mUpdaterTask;
        private CancellationTokenSource mToken;

        public FileUpdater(IApplication app)
        {
            this.mApp = app;
            this.mUpdaterTask = null;
        }

        public void Dispose()
        {
            this.Stop();
        }

        public void Start()
        {
            if (this.mUpdaterTask == null)
            {
                this.mToken = new CancellationTokenSource();
                this.mUpdaterTask = new Task<Updater.Classes.DownloadResult>(this.Process, this.mToken.Token, this.mToken.Token);
                this.mUpdaterTask.ContinueWith(this.ProcessCompleted, TaskScheduler.FromCurrentSynchronizationContext());
                this.mUpdaterTask.Start();
            }
        }

        public void Stop()
        {
            if (this.mUpdaterTask != null)
            {
                try
                {
                    this.mToken.Cancel();
                    this.mUpdaterTask.Wait();
                }
                catch (AggregateException exc)
                {
                    if (this.mUpdaterTask.IsCanceled)
                    {
                    }
                }
                catch (Exception exc)
                {
                    throw;
                }
                finally
                {
                    this.mUpdaterTask.Dispose();
                    this.mToken.Dispose();
                    this.mUpdaterTask = null;
                    this.mToken = null;
                }
            }
        }

        private Updater.Classes.DownloadResult Process(object obj)
        {
            if (!Directory.Exists(Saved<Options>.Instance.SourceDirectory))
            {
                return null;
            }

            CancellationToken token = (CancellationToken)obj;

            token.ThrowIfCancellationRequested();

            Thread.Sleep(2000);

            HistoryParser history = (from filename in Directory.EnumerateFiles(Saved<Options>.Instance.SourceDirectory, "*.xml", SearchOption.AllDirectories)
                                     let parser = new HistoryParser(filename)
                                     where parser.IsValid
                                     select parser).FirstOrDefault();

            if (history != null)
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                AssemblyGitCommitAuthorDateAttribute attributeAuthorDate = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyGitCommitAuthorDateAttribute>();
                DateTime currentCommitDate = attributeAuthorDate.CommitAuthorDate;

                if (history.LatestCommitDate > currentCommitDate)
                {
                    try
                    {
                        FileInfo sourceFile = new FileInfo(history.LatestFilePath);

                        if (!sourceFile.Exists)
                        {
                            sourceFile = new FileInfo(Path.Combine(Saved<Options>.Instance.SourceDirectory, history.LatestFilePath));
                        }

                        if (sourceFile.Exists)
                        {
                            string tempPath = Path.GetTempFileName();
                            FileInfo tempFile = sourceFile.CopyTo(tempPath, true);

                            using (var md5 = MD5.Create())
                            {
                                using (var stream = tempFile.OpenRead())
                                {
                                    byte[] tempHash = md5.ComputeHash(stream);

                                    if (tempHash.SequenceEqual(history.LatestFileHash))
                                    {
                                        return new Updater.Classes.DownloadResult(history, tempFile);
                                    }
                                }
                            }
                        }
                    }
                    catch // (Exception exc)
                    {
                    }
                }
            }

            return null;
        }

        private void ProcessCompleted(Task<Updater.Classes.DownloadResult> task)
        {
            Updater.Classes.DownloadResult result = task.Result;

            if (result != null)
            {
                UpdateReceivedEventArgs ea = new UpdateReceivedEventArgs(result);
                this.mApp.Messages.Send(this, ea);
            }

            if (!task.IsCanceled)
            {
                this.mUpdaterTask.Dispose();
                this.mToken.Dispose();
                this.mUpdaterTask = null;
                this.mToken = null;
            }
        }
    }
}

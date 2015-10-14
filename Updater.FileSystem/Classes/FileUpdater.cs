using AppCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            CancellationToken token = (CancellationToken)obj;

            token.ThrowIfCancellationRequested();

            Thread.Sleep(2000);
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
            }

            return new Updater.Classes.DownloadResult();
        }

        private void ProcessCompleted(Task<Updater.Classes.DownloadResult> task)
        {
            Updater.Classes.DownloadResult result = task.Result;

            UpdateReceivedEventArgs ea = new UpdateReceivedEventArgs(result);
            this.mApp.Messages.Send(this, ea);

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

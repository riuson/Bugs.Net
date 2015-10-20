using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Classes
{
    internal class InstanceMonitor : IDisposable
    {
        private Task mMonitorTask;
        private CancellationTokenSource mToken;
        private Semaphore mSemaphore;

        public Action AnotherInstanceStarted;

        public InstanceMonitor(Semaphore semaphore)
        {
            this.mSemaphore = semaphore;
            this.mToken = new CancellationTokenSource();
            this.mMonitorTask = new Task(this.MonitorMethod, this.mToken.Token, this.mToken.Token);
            this.mMonitorTask.Start();
        }

        public void Dispose()
        {
            if (this.mMonitorTask != null)
            {
                try
                {
                    this.mToken.Cancel();
                    this.mMonitorTask.Wait();
                }
                catch (AggregateException)
                {
                    if (this.mMonitorTask.IsCanceled)
                    {
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    this.mMonitorTask.Dispose();
                    this.mToken.Dispose();
                    this.mMonitorTask = null;
                    this.mToken = null;
                }
            }
        }

        private void MonitorMethod(object obj)
        {
            CancellationToken token = (CancellationToken)obj;
            token.ThrowIfCancellationRequested();

            while (true)
            {
                // Sleep 1000ms
                if (token.WaitHandle.WaitOne(1000))
                {
                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }
                    break;
                }

                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                if (this.mSemaphore != null)
                {
                    // If acquired, other copy was started
                    if (this.mSemaphore.WaitOne(1, false))
                    {
                        if (AnotherInstanceStarted != null)
                        {
                            this.AnotherInstanceStarted();
                        }
                    }
                }
            }
        }
    }
}

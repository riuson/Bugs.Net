using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AppCore.Main
{
    [Serializable]
    public class AppStartInfo
    {
        public Guid InstanceId
        {
            get { return new Guid("79520ea5-a9e9-4169-a490-80901cb7093e"); }
        }
        public Semaphore InstanceSemaphore { get; private set; }
        public bool AlreadyRunned { get; private set; }
        public string[] Arguments { get; set; }
        public string ExecutableDir { get; set; }
        public string ExecutablePath { get; set; }

        public AppStartInfo()
        {
            bool createdNew;
            this.InstanceSemaphore = new Semaphore(0, 1, this.InstanceId.ToString(), out createdNew);
            this.AlreadyRunned = !createdNew;

            this.Arguments = new string[] { };
            this.ExecutableDir = String.Empty;
            this.ExecutablePath = String.Empty;
        }

        public void SignalToExistingInstance()
        {
            if (!this.InstanceSemaphore.WaitOne(1, false))
            {
                this.InstanceSemaphore.Release();
                this.AlreadyRunned = true;
            }
        }
    }
}

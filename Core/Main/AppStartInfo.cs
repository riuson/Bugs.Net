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
        public string[] Arguments { get; set; }
        public string ExecutableDir { get; set; }
        public string ExecutablePath { get; set; }

        public AppStartInfo()
        {
            this.Arguments = new string[] { };
            this.ExecutableDir = String.Empty;
            this.ExecutablePath = String.Empty;
        }
    }
}

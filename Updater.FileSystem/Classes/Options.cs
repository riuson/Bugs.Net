using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Updater.FileSystem.Classes
{
    public class Options
    {
        public string SourceDirectory { get; set; }

        public Options()
        {
            this.SourceDirectory = String.Empty;
        }
    }
}

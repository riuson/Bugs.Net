using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Updater.Classes
{
    public class Options
    {
        public bool CheckUpdatesOnStart { get; set; }

        public Options()
        {
            this.CheckUpdatesOnStart = true;
        }
    }
}

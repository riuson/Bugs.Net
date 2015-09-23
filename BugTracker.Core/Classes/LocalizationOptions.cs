using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.Core.Classes
{
    public class LocalizationOptions
    {
        public string CultureName { get; set; }

        public LocalizationOptions()
        {
            this.CultureName = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
        }
    }
}

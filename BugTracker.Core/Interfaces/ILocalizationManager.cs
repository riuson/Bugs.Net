using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BugTracker.Core.Interfaces
{
    public interface ILocalizationManager
    {
        CultureInfo ActiveUICulture { get; set; }
        IEnumerable<CultureInfo> FoundCultures { get; }
    }
}

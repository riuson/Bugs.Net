using BugTracker.Core.Classes;
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
        IEnumerable<string> GetModules(CultureInfo culture);
        IEnumerable<TranslationUnit> GetTranslationUnits(CultureInfo culture, string assemblyName);
        void AddCulture(CultureInfo cultureNew, CultureInfo cultureSource);
    }
}

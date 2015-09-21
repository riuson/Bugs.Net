using BugTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Translator.Classes
{
    internal class LanguagesListData
    {
        private IApplication mApp;
        private IEnumerable<CultureInfo> mCultures;
        public BindingSource Data { get; private set; }

        public LanguagesListData(IApplication app)
        {
            this.mApp = app;

            this.mCultures = app.Localization.FoundCultures;
            this.Data = new BindingSource();
            this.Data.DataSource = this.mCultures;
        }

    }
}

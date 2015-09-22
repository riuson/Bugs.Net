using BugTracker.Core.Interfaces;
using BugTracker.Translator.Controls;
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

            this.Data = new BindingSource();
            this.UpdateList();
        }

        private void UpdateList()
        {
            this.mCultures = this.mApp.Localization.FoundCultures;
            this.Data.DataSource = this.mCultures;
        }

        public void Add()
        {
            ControlLanguages controlSelectNewLanguages = new ControlLanguages(this.mApp);
            controlSelectNewLanguages.Confirmed += this.controlSelectNewLanguages_Confirmed;
            this.mApp.Controls.Show(controlSelectNewLanguages);
        }

        public void Edit(CultureInfo culture)
        {
            ControlTranslate controlTranslate = new ControlTranslate(this.mApp, culture);
            //controlTranslate.Confirmed += this.controlSelectNewLanguages_Confirmed;
            this.mApp.Controls.Show(controlTranslate);
        }

        public void Remove(CultureInfo culture)
        {
            this.mApp.Localization.RemoveCulture(culture);
            this.UpdateList();
        }

        private void controlSelectNewLanguages_Confirmed(object sender, EventArgs e)
        {
            ControlLanguages control = sender as ControlLanguages;

            if (control != null)
            {
                IEnumerable<CultureInfo> cultures = control.SelectedCultures;
                IEnumerable<CultureInfo> culturesNew = cultures.Except(this.mCultures);

                foreach (var cultureNew in culturesNew)
                {
                    this.CreateNewCultureFiles(cultureNew);
                }
                this.UpdateList();

                this.mApp.Controls.Hide(control);
            }
        }

        private void CreateNewCultureFiles(CultureInfo culture)
        {
            CultureInfo cultureSource = null;

            // If parent exists
            if (this.mCultures.Contains(culture.Parent))
            {
                // Get parent
                cultureSource = culture.Parent;
            }
            else
            {
                // If culture with same parent exists
                if (this.mCultures.Any(existing => existing.Parent == culture.Parent))
                {
                    // Get it
                    cultureSource = this.mCultures.First(existing => existing.Parent == culture.Parent);
                }
                else
                {
                    CultureInfo cultureEn = new CultureInfo("en");
                    CultureInfo cultureEnUs = new CultureInfo("en-US");

                    // Get En-Us if exists
                    if (this.mCultures.Contains(cultureEnUs))
                    {
                        cultureSource = cultureEnUs;
                    }
                    else if (this.mCultures.Contains(cultureEn))
                    {
                        cultureSource = cultureEn;
                    }
                    else
                    {
                        cultureSource = null;
                    }
                }
            }

            this.mApp.Localization.AddCulture(culture, cultureSource);
        }
    }
}

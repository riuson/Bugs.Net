using BugTracker.Core.Classes;
using BugTracker.Core.Extensions;
using BugTracker.Core.Interfaces;
using BugTracker.Translator.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Translator.Classes
{
    internal class Plugin : IPlugin
    {
        private IApplication mApp;

        public void Initialize(IApplication app)
        {
            this.mApp = app;
        }

        public IButton[] GetCommandLinks(string tag)
        {
            switch (tag)
            {
                case "settings":
                    {
                        IButton menuItemSettings = MenuPanelFabric.CreateMenuItem(
                            "Language".Tr(),
                            "Select interface language".Tr(),
                            BugTracker.Translator.Properties.Resources.icon_language_008000_48);
                        menuItemSettings.Click += delegate(object sender, EventArgs ea)
                        {
                            this.ShowLanguageSelect();
                        };

                        return new IButton[] { menuItemSettings };
                    }
                default:
                    return new IButton[] { };
            }
        }

        private void ShowLanguageSelect()
        {
            ControlLanguage control = new ControlLanguage(this.mApp);
            control.Confirmed += this.control_Confirmed;
            control.Rejected += this.control_Rejected;
            this.mApp.Controls.Show(control);
        }

        private void control_Confirmed(object sender, EventArgs e)
        {
            ControlLanguage control = sender as ControlLanguage;

            if (control != null)
            {
                CultureInfo selectedCulture = control.SelectedCulture;

                if (selectedCulture != null)
                {
                    this.mApp.Localization.ActiveUICulture = selectedCulture;
                }

                this.mApp.Controls.Hide(control);
            }
        }

        private void control_Rejected(object sender, EventArgs e)
        {
            ControlLanguage control = sender as ControlLanguage;

            if (control != null)
            {
                this.mApp.Controls.Hide(control);
            }
        }
    }
}

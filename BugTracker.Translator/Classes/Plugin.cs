﻿using BugTracker.Core;
using BugTracker.Core.Classes;
using BugTracker.Core.Extensions;
using BugTracker.Core.Menus;
using BugTracker.Core.Plugins;
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
                        IButton menuItemSelectLanguage = MenuPanelFabric.CreateMenuItem(
                            "Language".Tr(),
                            "Select interface language".Tr(),
                            BugTracker.Translator.Properties.Resources.icon_language_008000_48);
                        menuItemSelectLanguage.Click += delegate(object sender, EventArgs ea)
                        {
                            this.ShowLanguageSelector();
                        };

                        IButton menuItemTranslation = MenuPanelFabric.CreateMenuItem(
                            "Translation".Tr(),
                            "Create and translate language files".Tr(),
                            BugTracker.Translator.Properties.Resources.icon_language_ff0000_48);
                        menuItemTranslation.Click += delegate(object sender, EventArgs ea)
                        {
                            this.ShowLanguageEditor();
                        };

                        return new IButton[] { menuItemSelectLanguage, menuItemTranslation };
                    }
                default:
                    return new IButton[] { };
            }
        }

        private void ShowLanguageSelector()
        {
            ControlLanguage control = new ControlLanguage(this.mApp);
            control.Confirmed += this.controlSelector_Confirmed;
            control.Rejected += this.controlSelector_Rejected;
            this.mApp.Controls.Show(control);
        }

        private void controlSelector_Confirmed(object sender, EventArgs e)
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

        private void controlSelector_Rejected(object sender, EventArgs e)
        {
            ControlLanguage control = sender as ControlLanguage;

            if (control != null)
            {
                this.mApp.Controls.Hide(control);
            }
        }

        private void ShowLanguageEditor()
        {
            ControlTranslations control = new ControlTranslations(this.mApp);
            this.mApp.Controls.Show(control);
        }
    }
}

using BugTracker.Core.Classes;
using BugTracker.Core.Extensions;
using BugTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
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
                        };

                        return new IButton[] { menuItemSettings };
                    }
                default:
                    return new IButton[] { };
            }
        }
    }
}

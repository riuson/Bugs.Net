using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using BugTracker.Settings.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BugTracker.Core.Extensions;

namespace BugTracker.Settings
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
                case "startpage":
                    {
                        IButton menuItemSettings = MenuPanelFabric.CreateMenuItem("Settings".Tr(), "Application options".Tr(), BugTracker.Settings.Properties.Resources.icon_gears_920000_48);
                        menuItemSettings.Click += delegate(object sender, EventArgs ea)
                        {
                            ControlSettings controlSettings = new ControlSettings(this.mApp);
                            this.mApp.Controls.Show(controlSettings);
                        };

                        return new IButton[] { menuItemSettings };
                    }
                default:
                    return new IButton[] { };
            }
        }
    }
}

using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using BugTracker.Settings.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugTracker.Settings
{
    internal class PluginInfo : IPlugin
    {
        private IApplication mApp;

        public void Initialize(IApplication app)
        {
            this.mApp = app;
        }

        public Button[] GetCommandLinks(string tag)
        {
            switch (tag)
            {
                case "startpage":
                    {
                        Button buttonSettings = MenuButton.Create("Settings");
                        buttonSettings.Click += delegate(object sender, EventArgs ea)
                        {
                            ControlSettings controlSettings = new ControlSettings(this.mApp);
                            this.mApp.Controls.Show(controlSettings);
                        };

                        return new Button[] { buttonSettings };
                    }
                default:
                    return new Button[] { };
            }
        }
    }
}

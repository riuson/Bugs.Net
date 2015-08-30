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
    public class PluginInfo : IPluginInfo
    {
        public Button[] GetCommandLinks(IApplication app, string tag)
        {
            switch (tag)
            {
                case "startpage":
                    {
                        Button buttonSettings = MenuButton.Create("Settings");
                        buttonSettings.Click += delegate(object sender, EventArgs ea)
                        {
                            ControlSettings controlSettings = new ControlSettings(app);
                            app.Controls.Show(controlSettings);
                        };

                        return new Button[] { buttonSettings };
                    }
                default:
                    return new Button[] { };
            }
        }
    }
}

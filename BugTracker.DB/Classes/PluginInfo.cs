using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using BugTracker.DB.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugTracker.DB.Classes
{
    internal class PluginInfo : IPluginInfo
    {
        public System.Windows.Forms.Button[] GetCommandLinks(IApplication app, string tag)
        {
            switch (tag)
            {
                case "settings":
                    {
                        Button buttonSettings = MenuButton.Create("Database", "Configure database");
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

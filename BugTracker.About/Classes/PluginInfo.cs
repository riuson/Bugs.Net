using BugTracker.About.Controls;
using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugTracker.About.Classes
{
    internal class PluginInfo : IPluginInfo
    {
        public System.Windows.Forms.Button[] GetCommandLinks(IApplication app, string tag)
        {
            if (tag == "startpage")
            {
                Button buttonAbout = MenuButton.Create("About", "About application");
                buttonAbout.Click += delegate(object sender, EventArgs ea)
                {
                    ControlAbout controlAbout = new ControlAbout(app);
                    app.Controls.Show(controlAbout);
                };

                return new Button[] { buttonAbout };
            }

            return new Button[] { };
        }
    }
}

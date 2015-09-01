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
    internal class Plugin : IPlugin
    {
        private IApplication mApp;

        public void Initialize(IApplication app)
        {
            this.mApp = app;
        }

        public System.Windows.Forms.Button[] GetCommandLinks(string tag)
        {
            switch (tag)
            {
                case "startpage":
                    {
                        Button buttonAbout = MenuButton.Create("About", "About application");
                        buttonAbout.Click += delegate(object sender, EventArgs ea)
                        {
                            ControlAbout controlAbout = new ControlAbout(this.mApp);
                            this.mApp.Controls.Show(controlAbout);
                        };

                        return new Button[] { buttonAbout };
                    }
                default:
                    return new Button[] { };
            }
        }
    }
}

using BugTracker.About.Controls;
using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BugTracker.Core.Extensions;

namespace BugTracker.About.Classes
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
                        IButton menuItemAbout = MenuPanelFabric.CreateMenuItem("About".Tr(), "About application".Tr(), BugTracker.About.Properties.Resources.icon_info_circle_005719_48);
                        menuItemAbout.Click += delegate(object sender, EventArgs ea)
                        {
                            ControlAbout controlAbout = new ControlAbout(this.mApp);
                            this.mApp.Controls.Show(controlAbout);
                        };

                        return new IButton[] { menuItemAbout };
                    }
                default:
                    return new IButton[] { };
            }
        }
    }
}

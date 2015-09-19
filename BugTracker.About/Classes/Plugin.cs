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
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("ru");
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("ru");
            BugTracker.About.Properties.Strings.Culture = System.Globalization.CultureInfo.GetCultureInfo("ru");
        }

        public IButton[] GetCommandLinks(string tag)
        {
            switch (tag)
            {
                case "startpage":
                    {
                        IButton menuItemAbout = MenuPanelFabric.CreateMenuItem(
                            BugTracker.About.Properties.Strings.BugTracker_About_Classes_Plugin_About,
                            BugTracker.About.Properties.Strings.BugTracker_About_Classes_Plugin_About_Application,
                            BugTracker.About.Properties.Resources.icon_info_circle_005719_48);
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

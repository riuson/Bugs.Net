using BugTracker.Core;
using BugTracker.Core.Classes;
using BugTracker.Core.Extensions;
using BugTracker.Core.Menus;
using BugTracker.Core.Plugins;
using BugTracker.DB.DataAccess;
using BugTracker.DB.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugTracker.DB.Classes
{
    internal class Plugin : IPlugin
    {
        private IApplication mApp;

        public void Initialize(IApplication app)
        {
            this.mApp = app;
            SessionManager.Instance.Configure(new SessionOptions(Saved<Options>.Instance.FileName));
        }

        public IButton[] GetCommandLinks(string tag)
        {
            switch (tag)
            {
                case "settings":
                    {
                        IButton menuItemSettings = MenuPanelFabric.CreateMenuItem("Database".Tr(), "Configure database".Tr(), BugTracker.DB.Properties.Resources.icon_database_1d257b_48);
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

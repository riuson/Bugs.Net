using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using BugTracker.Projects.Controls;
using BugTracker.Projects.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugTracker.Projects.Classes
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
                        IButton menuItemProjects = MenuPanelFabric.CreateMenuItem("Projects", "Manage projects", BugTracker.Projects.Properties.Resources.icon_files_o_2c3699_48);
                        menuItemProjects.Click += this.ShowProjectsList;
                        return new IButton[] { menuItemProjects };
                    }
                default:
                    return new IButton[] { };
            }
        }

        private void ShowProjectsList(object sender, EventArgs ea)
        {
            ControlProjectsList controlList = new ControlProjectsList(this.mApp);
            this.mApp.Controls.Show(controlList);
        }
    }
}

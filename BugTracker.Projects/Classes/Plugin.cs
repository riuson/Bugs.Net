using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
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

            this.mApp.Messages.Subscribe(typeof(ShowProjectListEventArgs), this.ShowProjectsList);
        }

        public IButton[] GetCommandLinks(string tag)
        {
            switch (tag)
            {
                case "startpage":
                    {
                        IButton menuItemProjects = MenuPanelFabric.CreateMenuItem("Projects", "Manage projects");
                        menuItemProjects.Click += delegate(object sender, EventArgs ea)
                        {
                            this.mApp.Messages.Send(this, new ShowProjectListEventArgs());
                        };

                        return new IButton[] { menuItemProjects };
                    }
                default:
                    return new IButton[] { };
            }
        }

        private void ShowProjectsList(object sender, MessageEventArgs ea)
        {
        }
    }
}

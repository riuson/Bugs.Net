using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using BugTracker.Members.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Members.Classes
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
                case "settings":
                    {
                        IButton menuItemMembers = MenuPanelFabric.CreateMenuItem("Members", "Manage member's list");
                        menuItemMembers.Click += delegate(object sender, EventArgs ea)
                        {
                            this.ShowMembersList();
                        };

                        return new IButton[] { menuItemMembers };
                    }
                default:
                    return new IButton[] { };
            }
        }

        private void ShowMembersList()
        {
            ControlMembersList controlMembers = new ControlMembersList(this.mApp);
            this.mApp.Controls.Show(controlMembers);
        }
    }
}

using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using BugTracker.DB.Entities;
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
            this.mApp.Messages.Subscribe(typeof(LoginAnswerEventArgs), this.LoginAnswer);
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
            LoginRequestEventArgs loginRequestEventArgs = new LoginRequestEventArgs();
            this.mApp.Messages.Send(this, loginRequestEventArgs);

            if (!loginRequestEventArgs.Processed)
            {
                MessageBox.Show(this.mApp.OwnerWindow, "Login handler not installed!", "Login required", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void LoginAnswer(object sender, MessageEventArgs e)
        {
            LoginAnswerEventArgs ea = e as LoginAnswerEventArgs;

            if (ea.LoggedMember != null)
            {
                ControlProjectsList controlList = new ControlProjectsList(this.mApp, ea.LoggedMember);
                this.mApp.Controls.Show(controlList);
            }
        }
    }
}

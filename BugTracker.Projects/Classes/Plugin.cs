using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using BugTracker.DB.Entities;
using BugTracker.Members.Events;
using BugTracker.Projects.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BugTracker.Core.Extensions;

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
                        IButton menuItemProjects = MenuPanelFabric.CreateMenuItem("Projects".Tr(), "Manage projects".Tr(), BugTracker.Projects.Properties.Resources.icon_files_o_2c3699_48);
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

            if (!loginRequestEventArgs.Handled)
            {
                MessageBox.Show(this.mApp.OwnerWindow, "Login handler not installed!".Tr(), "Login required".Tr(), MessageBoxButtons.OK, MessageBoxIcon.Stop);
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

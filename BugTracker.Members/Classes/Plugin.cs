using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using BugTracker.Members.Controls;
using BugTracker.Projects.Events;
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
            this.mApp.Messages.Subscribe(typeof(LoginRequestEventArgs), this.LoginRequired);
        }

        public IButton[] GetCommandLinks(string tag)
        {
            switch (tag)
            {
                case "settings":
                    {
                        IButton menuItemMembers = MenuPanelFabric.CreateMenuItem("Members", "Manage member's list", BugTracker.Members.Properties.Resources.icon_users_07572d_48);
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

        private void LoginRequired(object sender, MessageEventArgs e)
        {
            e.Processed = true;

            ControlLogin loginControl = new ControlLogin(this.mApp);
            loginControl.LoginConfirmed += loginControl_LoginConfirmed;
            loginControl.LoginRejected += loginControl_LoginRejected;
            this.mApp.Controls.Show(loginControl);
        }

        private void loginControl_LoginConfirmed(object sender, EventArgs e)
        {
            ControlLogin loginControl = sender as ControlLogin;

            if (loginControl != null)
            {
                LoginAnswerEventArgs ea = new LoginAnswerEventArgs(loginControl.SelectedMember);
                this.mApp.Controls.Hide(loginControl);
                this.mApp.Messages.Send(this, ea);
            }
        }

        private void loginControl_LoginRejected(object sender, EventArgs e)
        {
            ControlLogin loginControl = sender as ControlLogin;

            if (loginControl != null)
            {
                LoginAnswerEventArgs ea = new LoginAnswerEventArgs();
                this.mApp.Controls.Hide(loginControl);
                this.mApp.Messages.Send(this, ea);
            }
        }
    }
}

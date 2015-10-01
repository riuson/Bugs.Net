using BugTracker.Core;
using BugTracker.Core.Classes;
using BugTracker.Core.Extensions;
using BugTracker.Core.Menus;
using BugTracker.Core.Messages;
using BugTracker.Core.Plugins;
using BugTracker.DB.DataAccess;
using BugTracker.DB.Entities;
using BugTracker.DB.Events;
using BugTracker.Members.Controls;
using BugTracker.Members.Events;
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
            this.mApp.Messages.Subscribe(typeof(EntityShowEventArgs<Member>), this.ShowMembersList);
        }

        public IButton[] GetCommandLinks(string tag)
        {
            switch (tag)
            {
                case "settings":
                    {
                        IButton menuItemMembers = MenuPanelFabric.CreateMenuItem("Members".Tr(), "Manage member's list".Tr(), BugTracker.Members.Properties.Resources.icon_users_07572d_48);
                        menuItemMembers.Click += delegate(object sender, EventArgs ea)
                        {
                            this.mApp.Messages.Send(this, new EntityShowEventArgs<Member>());
                        };

                        return new IButton[] { menuItemMembers };
                    }
                default:
                    return new IButton[] { };
            }
        }

        private void ShowMembersList(object sender, MessageEventArgs ea)
        {
            if (!SessionManager.Instance.IsConfigured)
            {
                this.mApp.Messages.Send(this, new ConfigurationRequiredEventArgs());
                return;
            }

            ControlMembersList controlMembers = new ControlMembersList(this.mApp);
            controlMembers.Disposed += delegate(object s, EventArgs e)
            {
                ea.Completed();
            };
            this.mApp.Controls.Show(controlMembers);
        }

        private void LoginRequired(object sender, MessageEventArgs e)
        {
            if (!SessionManager.Instance.IsConfigured)
            {
                this.mApp.Messages.Send(this, new ConfigurationRequiredEventArgs());
                return;
            }

            e.Handled = true;

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

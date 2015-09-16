using BugTracker.Core.Interfaces;
using BugTracker.DB.Entities;
using BugTracker.DB.Events;
using BugTracker.Tickets.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Tickets.Classes
{
    internal class Plugin : IPlugin
    {
        private IApplication mApp;

        public void Initialize(IApplication app)
        {
            this.mApp = app;

            this.mApp.Messages.Subscribe(typeof(EntityShowEventArgs<Project>), this.ShowTicketsList);
        }

        public IButton[] GetCommandLinks(string tag)
        {
            return new IButton[] { };
        }

        private void ShowTicketsList(object sender, MessageEventArgs e)
        {
            EntityShowEventArgs<Project> ea = e as EntityShowEventArgs<Project>;

            if (ea != null)
            {
                ControlTicketsList controlList = new ControlTicketsList(this.mApp, ea.Member, ea.Entity);
                this.mApp.Controls.Show(controlList);
            }
        }
    }
}

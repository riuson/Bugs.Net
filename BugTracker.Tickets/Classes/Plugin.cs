using BugTracker.Core.Interfaces;
using BugTracker.Projects.Events;
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

            this.mApp.Messages.Subscribe(typeof(ShowProjectTicketsEventArgs), this.ShowTicketsList);
        }

        public IButton[] GetCommandLinks(string tag)
        {
            return new IButton[] { };
        }

        private void ShowTicketsList(object sender, MessageEventArgs e)
        {
            ShowProjectTicketsEventArgs ea = e as ShowProjectTicketsEventArgs;

            if (ea != null)
            {
                ControlTicketsList controlList = new ControlTicketsList(this.mApp);
                this.mApp.Controls.Show(controlList);
            }
        }
    }
}

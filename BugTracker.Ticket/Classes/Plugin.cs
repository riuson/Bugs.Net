using BugTracker.Core.Interfaces;
using BugTracker.Tickets.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Ticket.Classes
{
    internal class Plugin : IPlugin
    {
        private IApplication mApp;

        public void Initialize(IApplication app)
        {
            this.mApp = app;

            this.mApp.Messages.Subscribe(typeof(AddTicketEventArgs), this.AddTicketList);
            this.mApp.Messages.Subscribe(typeof(EditTicketEventArgs), this.EditTicketList);
        }

        public IButton[] GetCommandLinks(string tag)
        {
            return new IButton[] { };
        }

        private void AddTicketList(object sender, MessageEventArgs e)
        {
        }

        private void EditTicketList(object sender, MessageEventArgs e)
        {
        }
    }
}

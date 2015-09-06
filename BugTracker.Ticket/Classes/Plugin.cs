using BugTracker.Core.Interfaces;
using BugTracker.Members.Controls;
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
            AddTicketEventArgs ea = e as AddTicketEventArgs;
            ControlTicketEdit controlEdit = new ControlTicketEdit(this.mApp, ea.LoggedMember);
            controlEdit.ClickOK += controlEdit_ClickOK;
            controlEdit.ClickCancel += controlEdit_ClickCancel;
            this.mApp.Controls.Show(controlEdit);
            e.Processed = true;
        }

        private void EditTicketList(object sender, MessageEventArgs e)
        {
        }

        private void controlEdit_ClickOK(object sender, EventArgs e)
        {
            ControlTicketEdit controlEdit = sender as ControlTicketEdit;

            if (controlEdit != null)
            {
                //LoginAnswerEventArgs ea = new LoginAnswerEventArgs();
                this.mApp.Controls.Hide(controlEdit);
                //this.mApp.Messages.Send(this, ea);
            }
        }

        private void controlEdit_ClickCancel(object sender, EventArgs e)
        {
            ControlTicketEdit controlEdit = sender as ControlTicketEdit;

            if (controlEdit != null)
            {
                //LoginAnswerEventArgs ea = new LoginAnswerEventArgs();
                this.mApp.Controls.Hide(controlEdit);
                //this.mApp.Messages.Send(this, ea);
            }
        }
    }
}

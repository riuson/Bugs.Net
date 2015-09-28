using BugTracker.Core;
using BugTracker.Core.Menus;
using BugTracker.Core.Messages;
using BugTracker.Core.Plugins;
using BugTracker.DB.Entities;
using BugTracker.DB.Events;
using BugTracker.TicketEditor.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.TicketEditor.Classes
{
    internal class Plugin : IPlugin
    {
        private IApplication mApp;

        public void Initialize(IApplication app)
        {
            this.mApp = app;

            this.mApp.Messages.Subscribe(typeof(EntityAddEventArgs<Ticket>), this.AddTicketList);
            this.mApp.Messages.Subscribe(typeof(EntityEditEventArgs<Ticket>), this.EditTicketList);
        }

        public IButton[] GetCommandLinks(string tag)
        {
            return new IButton[] { };
        }

        private void AddTicketList(object sender, MessageEventArgs e)
        {
            EntityAddEventArgs<Ticket> ea = e as EntityAddEventArgs<Ticket>;
            ControlTicketEdit controlEdit = new ControlTicketEdit(this.mApp, ea.Member);
            controlEdit.ClickOK += controlEdit_ClickOK;
            controlEdit.ClickCancel += controlEdit_ClickCancel;
            this.mApp.Controls.Show(controlEdit);
            e.Handled = true;
        }

        private void EditTicketList(object sender, MessageEventArgs e)
        {
            EntityEditEventArgs<Ticket> ea = e as EntityEditEventArgs<Ticket>;
            ControlTicketEdit controlEdit = new ControlTicketEdit(this.mApp, ea.Member, ea.Entity);
            controlEdit.ClickOK += controlEdit_ClickOK;
            controlEdit.ClickCancel += controlEdit_ClickCancel;
            this.mApp.Controls.Show(controlEdit);
            e.Handled = true;
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

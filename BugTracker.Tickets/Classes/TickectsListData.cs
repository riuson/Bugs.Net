using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using BugTracker.DB;
using BugTracker.DB.Entities;
using BugTracker.DB.Interfaces;
using BugTracker.DB.Repositories;
using BugTracker.Projects.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BugTracker.Tickets.Classes;
using BugTracker.Tickets.Events;

namespace BugTracker.Tickets.Classes
{
    internal class TicketsListData : IDisposable
    {
        private IApplication mApp;
        private ICollection<Ticket> mInternalData;
        private Member mLoggedMember;

        public BindingSource Data { get; private set; }

        public TicketsListData(IApplication app, Member loggedMember)
        {
            this.mApp = app;
            this.mLoggedMember = loggedMember;
            this.Data = new BindingSource();
            this.Data.AllowNew = false;
            this.UpdateList();
        }

        public void Dispose()
        {
            this.Data.Dispose();
        }

        public void UpdateList()
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                TicketRepository repository = new TicketRepository(session);

                this.Data.DataSource = null;
                this.mInternalData = repository.List();
                this.Data.DataSource = this.mInternalData;
            }
        }

        public void Add()
        {
            AddTicketEventArgs ea = new AddTicketEventArgs(this.mLoggedMember);
            this.mApp.Messages.Send(this, ea);

            if (ea.Processed)
            {
                this.UpdateList();
            }
        }

        public void Edit(Ticket item)
        {
            EditTicketEventArgs ea = new EditTicketEventArgs(item, this.mLoggedMember);
            this.mApp.Messages.Send(this, ea);

            if (ea.Processed)
            {
                this.UpdateList();
            }
        }

        public void Remove(Ticket item)
        {
            RemoveTicketEventArgs ea = new RemoveTicketEventArgs(item, this.mLoggedMember);
            this.mApp.Messages.Send(this, ea);

            if (!ea.Processed)
            {
                if (MessageBox.Show(
                    this.mApp.OwnerWindow,
                    String.Format(
                        "Do you really want remove ticket '{0}'?",
                        item.Title),
                    "Remove ticket",
                    MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    using (ISession session = SessionManager.Instance.OpenSession())
                    {
                        TicketRepository repository = new TicketRepository(session);
                        repository.Delete(item);
                    }

                    ea.Processed = true;
                }
            }

            if (ea.Processed)
            {
                this.UpdateList();
            }
        }
    }
}

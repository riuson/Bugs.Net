using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using BugTracker.DB;
using BugTracker.DB.Entities;
using BugTracker.DB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BugTracker.Tickets.Classes;
using BugTracker.DB.Events;
using BugTracker.DB.Classes;

namespace BugTracker.Tickets.Classes
{
    internal class TicketsListData : IDisposable
    {
        private IApplication mApp;
        private ICollection<Ticket> mInternalData;
        private Member mLoggedMember;
        private Project mProject;

        public BindingSource Data { get; private set; }

        public TicketsListData(IApplication app, Member loggedMember, Project project)
        {
            this.mApp = app;
            this.mLoggedMember = loggedMember;
            this.mProject = project;
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
            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<Project> projectRepository = new Repository<Project>(session);

                this.mProject = projectRepository.Load(this.mProject.Id);

                this.Data.DataSource = null;
                this.mInternalData = this.mProject.Tickets;
                this.Data.DataSource = this.mInternalData;
            }
        }

        public void Add()
        {
            Ticket ticket = new Ticket();
            ticket.Author = this.mLoggedMember;
            ticket.Project = this.mProject;

            EntityEditEventArgs<Ticket> ea = new EntityEditEventArgs<Ticket>(ticket, this.mLoggedMember);
            this.mApp.Messages.Send(this, ea);

            if (ea.Processed)
            {
                this.UpdateList();
            }
        }

        public void Edit(Ticket item)
        {
            EntityEditEventArgs<Ticket> ea = new EntityEditEventArgs<Ticket>(item, this.mLoggedMember);
            this.mApp.Messages.Send(this, ea);

            if (ea.Processed)
            {
                this.UpdateList();
            }
        }

        public void Remove(Ticket item)
        {
            EntityRemoveEventArgs<Ticket> ea = new EntityRemoveEventArgs<Ticket>(item, this.mLoggedMember);
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
                    using (ISession session = SessionManager.Instance.OpenSession(true))
                    {
                        IRepository<Ticket> repository = new Repository<Ticket>(session);

                        repository.Delete(item);

                        session.Transaction.Commit();
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

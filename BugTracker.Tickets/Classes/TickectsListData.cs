using BugTracker.Core.Classes;
using BugTracker.Core.Extensions;
using BugTracker.Core.Interfaces;
using BugTracker.DB;
using BugTracker.DB.Dao;
using BugTracker.DB.Entities;
using BugTracker.DB.Events;
using BugTracker.DB.Extensions;
using BugTracker.DB.Interfaces;
using BugTracker.Tickets.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;

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

            if (ea.Handled)
            {
                this.UpdateList();
            }
        }

        public void Edit(Ticket item)
        {
            EntityEditEventArgs<Ticket> ea = new EntityEditEventArgs<Ticket>(item, this.mLoggedMember);
            this.mApp.Messages.Send(this, ea);

            if (ea.Handled)
            {
                this.UpdateList();
            }
        }

        public void Remove(Ticket item)
        {
            EntityRemoveEventArgs<Ticket> ea = new EntityRemoveEventArgs<Ticket>(item, this.mLoggedMember);
            this.mApp.Messages.Send(this, ea);

            if (!ea.Handled)
            {
                if (MessageBox.Show(
                    this.mApp.OwnerWindow,
                    String.Format(
                        "Do you really want remove ticket '{0}'?".Tr(),
                        item.Title),
                    "Remove ticket".Tr(),
                    MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    using (ISession session = SessionManager.Instance.OpenSession(true))
                    {
                        IRepository<Ticket> repository = new Repository<Ticket>(session);

                        repository.Delete(item);

                        session.Transaction.Commit();
                    }

                    ea.Handled = true;
                }
            }

            if (ea.Handled)
            {
                this.UpdateList();
            }
        }

        internal void ApplyFilter(string fieldName, SortOrder sortOrder)
        {
            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<Project> projectRepository = new Repository<Project>(session);
                IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);

                this.mProject = projectRepository.GetById(this.mProject.Id);
                IQueryable<Ticket> tickets = ticketRepository.Query()
                    .Where(item => item.Project == this.mProject)
                    .FetchField(related => related.Author)
                    .FetchField(related => related.Status);

                if (!String.IsNullOrEmpty(fieldName) && sortOrder != SortOrder.None)
                {
                    // This property is not in database
                    if (fieldName == "Author.FullName")
                    {
                        tickets = tickets.OrderBy("Author.LastName", sortOrder == SortOrder.Ascending);
                        tickets = tickets.OrderBy("Author.FirstName", sortOrder == SortOrder.Ascending);
                    }
                    else
                    {
                        tickets = tickets.OrderBy(fieldName, sortOrder == SortOrder.Ascending);
                    }
                }

                this.mInternalData = tickets.ToList();
                this.Data.DataSource = this.mInternalData;
            }
        }
    }
}

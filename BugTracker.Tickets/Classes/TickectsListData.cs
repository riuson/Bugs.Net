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

        private string mSortField;
        private SortOrder mSortOrder;
        private string mFilterTitle;

        public BindingSource Data { get; private set; }

        public TicketsListData(IApplication app, Member loggedMember, Project project)
        {
            this.mApp = app;
            this.mLoggedMember = loggedMember;
            this.mProject = project;
            this.Data = new BindingSource();
            this.Data.AllowNew = false;

            this.ApplyFilter(String.Empty, SortOrder.None, String.Empty);
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
                IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);

                this.mProject = projectRepository.GetById(this.mProject.Id);
                IQueryable<Ticket> tickets = ticketRepository.Query()
                    .Where(item => item.Project == this.mProject);

                if (!String.IsNullOrEmpty(this.mFilterTitle))
                {
                    tickets = tickets
                        .Where(item => item.Title.Contains(this.mFilterTitle));
                }

                tickets = tickets
                    .FetchField(related => related.Author)
                    .FetchField(related => related.Status);

                if (!String.IsNullOrEmpty(this.mSortField) && this.mSortOrder != SortOrder.None)
                {
                    // This property is not in database
                    if (this.mSortField == "Author.FullName")
                    {
                        tickets = tickets.OrderBy("Author.LastName", this.mSortOrder == SortOrder.Ascending);
                        tickets = tickets.OrderBy("Author.FirstName", this.mSortOrder == SortOrder.Ascending);
                    }
                    else
                    {
                        tickets = tickets.OrderBy(this.mSortField, this.mSortOrder == SortOrder.Ascending);
                    }
                }

                this.mInternalData = tickets.ToList();
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

        internal void ApplyFilter(string fieldName, SortOrder sortOrder, string titleFitler)
        {
            this.mSortField = fieldName;
            this.mSortOrder = sortOrder;
            this.mFilterTitle = titleFitler;
            this.UpdateList();
        }
    }
}

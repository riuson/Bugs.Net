using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using BugTracker.DB;
using BugTracker.DB.Dao;
using BugTracker.DB.Entities;
using BugTracker.DB.Events;
using BugTracker.DB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BugTracker.Core.Extensions;

namespace BugTracker.Projects.Classes
{
    internal class ProjectsListData : IDisposable
    {
        private IApplication mApp;
        private ICollection<Project> mInternalData;
        private Member mLoggedMember;

        public BindingSource Data { get; private set; }

        public ProjectsListData(IApplication app, Member loggedMember)
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
            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<Project> repository = new Repository<Project>(session);

                this.mInternalData = repository.List();
                this.Data.DataSource = this.mInternalData;
            }
        }

        public void Add()
        {
            EntityAddEventArgs<Project> ea = new EntityAddEventArgs<Project>(this.mLoggedMember);
            this.mApp.Messages.Send(this, ea);

            if (!ea.Handled)
            {
                string newName;

                if (InputBox.Show("New project name:".Tr(), "Add project".Tr(), String.Empty, out newName) == DialogResult.OK)
                {
                    using (ISession session = SessionManager.Instance.OpenSession(true))
                    {
                        IRepository<Project> repository = new Repository<Project>(session);

                        Project item = new Project();
                        item.Name = newName;
                        repository.Save(item);

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

        public void Edit(Project item)
        {
            EntityEditEventArgs<Project> ea = new EntityEditEventArgs<Project>(item, this.mLoggedMember);
            this.mApp.Messages.Send(this, ea);

            if (!ea.Handled)
            {
                string newName;

                if (InputBox.Show("Change project name:".Tr(), "Edit project".Tr(), item.Name, out newName) == DialogResult.OK)
                {
                    using (ISession session = SessionManager.Instance.OpenSession(true))
                    {
                        IRepository<Project> repository = new Repository<Project>(session);

                        item.Name = newName;
                        repository.SaveOrUpdate(item);

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

        public void Remove(Project item)
        {
            EntityRemoveEventArgs<Project> ea = new EntityRemoveEventArgs<Project>(item, this.mLoggedMember);
            this.mApp.Messages.Send(this, ea);

            if (!ea.Handled)
            {
                if (MessageBox.Show(
                    this.mApp.OwnerWindow,
                    String.Format(
                        "Do you really want remove project '{0}'?".Tr(),
                        item.Name),
                    "Remove project".Tr(),
                    MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    using (ISession session = SessionManager.Instance.OpenSession(true))
                    {
                        IRepository<Project> repository = new Repository<Project>(session);

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

        public void ShowTickets(Project item)
        {
            EntityShowEventArgs<Project> ea = new EntityShowEventArgs<Project>(item, this.mLoggedMember);
            this.mApp.Messages.Send(this, ea);

            if (ea.Handled)
            {
                this.UpdateList();
            }
        }
    }
}

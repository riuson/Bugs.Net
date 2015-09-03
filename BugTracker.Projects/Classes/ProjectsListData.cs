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

namespace BugTracker.Projects.Classes
{
    internal class ProjectsListData : IDisposable
    {
        private IApplication mApp;
        private ICollection<Project> mData;
        private BindingSource mBS;

        public ProjectsListData(IApplication app)
        {
            this.mApp = app;
            this.mBS = new BindingSource();
            this.mBS.AllowNew = false;
            this.UpdateList();
        }

        public void Dispose()
        {
            this.mBS.Dispose();
        }

        public void UpdateList()
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                ProjectRepository repository = new ProjectRepository(session);

                this.mBS.DataSource = null;
                this.mData = repository.List();
                this.mBS.DataSource = this.mData;
            }
        }

        public void Add()
        {
            AddProjectEventArgs ea = new AddProjectEventArgs();
            this.mApp.Messages.Send(this, ea);

            if (!ea.Processed)
            {
                string newName;

                if (InputBox.Show("New project name:", "Add project", String.Empty, out newName) == DialogResult.OK)
                {
                    using (ISession session = SessionManager.Instance.OpenSession())
                    {
                        ProjectRepository repository = new ProjectRepository(session);
                        Project item = new Project();
                        item.Name = newName;
                        repository.Save(item);
                    }
                    ea.Processed = true;
                }
            }

            if (ea.Processed)
            {
                this.UpdateList();
            }
        }

        public void Edit(Project item)
        {
            EditProjectEventArgs ea = new EditProjectEventArgs(item);
            this.mApp.Messages.Send(this, ea);

            if (!ea.Processed)
            {
                string newName;

                if (InputBox.Show("Change project name:", "Edit project", item.Name, out newName) == DialogResult.OK)
                {
                    using (ISession session = SessionManager.Instance.OpenSession())
                    {
                        ProjectRepository repository = new ProjectRepository(session);
                        item.Name = newName;
                        repository.SaveOrUpdate(item);
                    }
                    ea.Processed = true;
                }
            }

            if (ea.Processed)
            {
                this.UpdateList();
            }
        }

        public void Remove(Project item)
        {
            RemoveProjectEventArgs ea = new RemoveProjectEventArgs(item);
            this.mApp.Messages.Send(this, ea);

            if (!ea.Processed)
            {
                if (MessageBox.Show(
                    String.Format(
                        "Do you really want remove project '{0}'?",
                        item.Name),
                    "Remove project",
                    MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    using (ISession session = SessionManager.Instance.OpenSession())
                    {
                        ProjectRepository repository = new ProjectRepository(session);
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

        public BindingSource Data { get { return this.mBS; } }
    }
}

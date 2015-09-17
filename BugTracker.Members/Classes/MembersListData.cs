using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using BugTracker.DB;
using BugTracker.DB.Dao;
using BugTracker.DB.Entities;
using BugTracker.DB.Events;
using BugTracker.DB.Interfaces;
using BugTracker.Members.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Members.Classes
{
    internal class MembersListData : IDisposable
    {
        private IApplication mApp;
        private ICollection<Member> mInternalData;
        private ControlMemberEdit mEditor;

        public BindingSource Data { get; private set; }

        public MembersListData(IApplication app)
        {
            this.mApp = app;
            this.Data = new BindingSource();
            this.Data.AllowNew = false;
            this.mEditor = null;
            this.UpdateList();
        }

        public void Dispose()
        {
            if (this.mEditor != null)
            {
                this.mEditor.Dispose();
            }
            this.Data.Dispose();
        }

        public void UpdateList()
        {
            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<Member> repository = new Repository<Member>(session);

                this.mInternalData = repository.List();
                this.Data.DataSource = this.mInternalData;
            }
        }

        public void Add()
        {
            EntityAddEventArgs<Member> ea = new EntityAddEventArgs<Member>();
            this.mApp.Messages.Send(this, ea);

            if (!ea.Handled)
            {
                this.mEditor = new ControlMemberEdit();
                this.mEditor.ClickOK += this.mEditorAdd_ClickOK;
                this.mEditor.ClickCancel += this.mEditor_ClickCancel;
                this.mEditor.Entity = null;
                this.mApp.Controls.Show(this.mEditor);
                ea.Handled = true;
            }

            if (ea.Handled)
            {
                this.UpdateList();
            }
        }

        public void Edit(Member item)
        {
            EntityEditEventArgs<Member> ea = new EntityEditEventArgs<Member>(item);
            this.mApp.Messages.Send(this, ea);

            if (!ea.Handled)
            {
                this.mEditor = new ControlMemberEdit(item.FirstName, item.LastName, item.EMail);
                this.mEditor.ClickOK += this.mEditorEdit_ClickOK;
                this.mEditor.ClickCancel += this.mEditor_ClickCancel;
                this.mEditor.Entity = item;
                this.mApp.Controls.Show(this.mEditor);
                ea.Handled = true;
            }

            if (ea.Handled)
            {
                this.UpdateList();
            }
        }

        public void Remove(Member item)
        {
            EntityRemoveEventArgs<Member> ea = new EntityRemoveEventArgs<Member>(item);
            this.mApp.Messages.Send(this, ea);

            if (!ea.Handled)
            {
                if (MessageBox.Show(
                    this.mApp.OwnerWindow,
                    String.Format(
                        "Do you really want remove Member '{0} {1}, {2}'?",
                        item.FirstName, item.LastName, item.EMail),
                    "Remove Member",
                    MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    using (ISession session = SessionManager.Instance.OpenSession(true))
                    {
                        IRepository<Member> repository = new Repository<Member>(session);
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

        private void mEditorAdd_ClickOK(object sender, EventArgs e)
        {
            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Member> repository = new Repository<Member>(session);

                Member item = new Member();
                item.FirstName = this.mEditor.FirstName;
                item.LastName = this.mEditor.LastName;
                item.EMail = this.mEditor.Email;
                repository.Save(item);

                session.Transaction.Commit();
            }

            this.mApp.Controls.Hide(this.mEditor);
            this.mEditor = null;

            this.UpdateList();
        }

        private void mEditorEdit_ClickOK(object sender, EventArgs e)
        {
            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Member> repository = new Repository<Member>(session);

                Member item = this.mEditor.Entity;
                item.FirstName = this.mEditor.FirstName;
                item.LastName = this.mEditor.LastName;
                item.EMail = this.mEditor.Email;
                repository.SaveOrUpdate(item);

                session.Transaction.Commit();
            }

            this.mApp.Controls.Hide(this.mEditor);
            this.mEditor = null;

            this.UpdateList();
        }

        private void mEditor_ClickCancel(object sender, EventArgs e)
        {
            this.mApp.Controls.Hide(this.mEditor);
            this.mEditor = null;
        }
    }
}

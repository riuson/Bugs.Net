using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using BugTracker.DB;
using BugTracker.DB.Entities;
using BugTracker.DB.Interfaces;
using BugTracker.DB.Repositories;
using BugTracker.Members.Controls;
using BugTracker.Members.Events;
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
            this.Data.Dispose();
        }

        public void UpdateList()
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                MemberRepository repository = new MemberRepository(session);

                this.Data.DataSource = null;
                this.mInternalData = repository.List();
                this.Data.DataSource = this.mInternalData;
            }
        }

        public void Add()
        {
            AddMemberEventArgs ea = new AddMemberEventArgs();
            this.mApp.Messages.Send(this, ea);

            if (!ea.Processed)
            {
                this.mEditor = new ControlMemberEdit();
                this.mEditor.ClickOK += this.mEditorAdd_ClickOK;
                this.mEditor.ClickCancel += this.mEditor_ClickCancel;
                this.mEditor.Entity = null;
                this.mApp.Controls.Show(this.mEditor);
                ea.Processed = true;
            }

            if (ea.Processed)
            {
                this.UpdateList();
            }
        }

        public void Edit(Member item)
        {
            EditMemberEventArgs ea = new EditMemberEventArgs(item);
            this.mApp.Messages.Send(this, ea);

            if (!ea.Processed)
            {
                this.mEditor = new ControlMemberEdit(item.FirstName, item.LastName, item.EMail);
                this.mEditor.ClickOK += this.mEditorEdit_ClickOK;
                this.mEditor.ClickCancel += this.mEditor_ClickCancel;
                this.mEditor.Entity = item;
                this.mApp.Controls.Show(this.mEditor);
                ea.Processed = true;
            }

            if (ea.Processed)
            {
                this.UpdateList();
            }
        }

        public void Remove(Member item)
        {
            RemoveMemberEventArgs ea = new RemoveMemberEventArgs(item);
            this.mApp.Messages.Send(this, ea);

            if (!ea.Processed)
            {
                if (MessageBox.Show(
                    String.Format(
                        "Do you really want remove Member '{0} {1}, {2}'?",
                        item.FirstName, item.LastName, item.EMail),
                    "Remove Member",
                    MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    using (ISession session = SessionManager.Instance.OpenSession())
                    {
                        MemberRepository repository = new MemberRepository(session);
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

        private void mEditorAdd_ClickOK(object sender, EventArgs e)
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                MemberRepository repository = new MemberRepository(session);
                Member item = new Member();
                item.FirstName = this.mEditor.FirstName;
                item.LastName = this.mEditor.LastName;
                item.EMail = this.mEditor.Email;
                repository.Save(item);
            }

            this.mApp.Controls.Hide(this.mEditor);
            this.mEditor = null;

            this.UpdateList();
        }

        private void mEditorEdit_ClickOK(object sender, EventArgs e)
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                MemberRepository repository = new MemberRepository(session);
                Member item = this.mEditor.Entity;
                item.FirstName = this.mEditor.FirstName;
                item.LastName = this.mEditor.LastName;
                item.EMail = this.mEditor.Email;
                repository.SaveOrUpdate(item);
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

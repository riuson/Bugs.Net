using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using BugTracker.DB;
using BugTracker.DB.Entities;
using BugTracker.DB.Interfaces;
using BugTracker.DB.Repositories;
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

        public BindingSource Data { get; private set; }

        public MembersListData(IApplication app)
        {
            this.mApp = app;
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

            if (ea.Processed)
            {
                this.UpdateList();
            }
        }

        public void Edit(Member item)
        {
            EditMemberEventArgs ea = new EditMemberEventArgs(item);
            this.mApp.Messages.Send(this, ea);

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
    }
}

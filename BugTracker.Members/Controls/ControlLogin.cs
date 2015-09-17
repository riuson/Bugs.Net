using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BugTracker.DB.Entities;
using BugTracker.Core.Interfaces;
using BugTracker.DB.Interfaces;
using BugTracker.DB;
using BugTracker.DB.Dao;
using BugTracker.Members.Classes;
using BugTracker.DB.Events;

namespace BugTracker.Members.Controls
{
    public partial class ControlLogin : UserControl
    {
        private IApplication mApp;
        private ICollection<Member> mMembers;
        private IEnumerable<MemberDisplay> mMembersDisplay;
        private BindingSource mBS;

        public event EventHandler LoginConfirmed;
        public event EventHandler LoginRejected;

        public ControlLogin(IApplication app)
        {
            InitializeComponent();
            this.Text = "Login";
            this.mApp = app;

            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<Member> repository = new Repository<Member>(session);
                this.mMembers = repository.List();
            }

            this.mMembersDisplay = this.mMembers.Select<Member, MemberDisplay>(m => new MemberDisplay(m));
            this.mBS = new BindingSource();
            this.mBS.DataSource = this.mMembersDisplay;
            this.comboBoxMember.DataSource = this.mBS;
        }

        public Member SelectedMember { get; private set; }

        private class MemberDisplay
        {
            public Member Member { get; private set; }

            public MemberDisplay(Member member)
            {
                this.Member = member;
            }

            public override string ToString()
            {
                return String.Format("{0} {1} ({2})", this.Member.FirstName, this.Member.LastName, this.Member.EMail);
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (this.comboBoxMember.SelectedIndex >= 0)
            {
                MemberDisplay md = this.comboBoxMember.SelectedItem as MemberDisplay;
                this.SelectedMember = md.Member;

                if (this.LoginConfirmed != null)
                {
                    this.LoginConfirmed(this, EventArgs.Empty);
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (this.LoginRejected != null)
            {
                this.LoginRejected(this, EventArgs.Empty);
            }
        }

        private void buttonMembersList_Click(object sender, EventArgs e)
        {
            this.mApp.Messages.Send(this, new EntityShowEventArgs<Member>());
        }
    }
}

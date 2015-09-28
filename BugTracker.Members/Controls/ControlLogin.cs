using BugTracker.Core;
using BugTracker.Core.Classes;
using BugTracker.Core.Extensions;
using BugTracker.Core.Messages;
using BugTracker.DB;
using BugTracker.DB.Dao;
using BugTracker.DB.Entities;
using BugTracker.DB.Events;
using BugTracker.DB.Interfaces;
using BugTracker.Members.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Members.Controls
{
    public partial class ControlLogin : UserControl
    {
        private IApplication mApp;
        private MembersListData mData;

        public event EventHandler LoginConfirmed;
        public event EventHandler LoginRejected;

        public ControlLogin(IApplication app)
        {
            InitializeComponent();
            this.Text = "Login".Tr();
            this.buttonOk.Text = this.buttonOk.Text.Tr();
            this.buttonCancel.Text = this.buttonCancel.Text.Tr();
            this.buttonMembersList.Text = this.buttonMembersList.Text.Tr();
            this.labelMember.Text = this.labelMember.Text.Tr();
            this.labelPassword.Text = this.labelPassword.Text.Tr();
            this.mApp = app;

            this.mData = new MembersListData(app);
            this.comboBoxMember.DataSource = this.mData.Data;
            this.comboBoxMember.DisplayMember = "FullName";
            this.mData.Data.ListChanged += this.Data_ListChanged;

            this.UpdateButtons();
            this.LoadSettings();
        }

        private void BeforeDisposing()
        {
            this.SaveSettings();
            this.mData.Dispose();
        }

        public Member SelectedMember { get; private set; }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (this.comboBoxMember.SelectedIndex >= 0)
            {
                this.SelectedMember = this.comboBoxMember.SelectedItem as Member;

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
            EntityShowEventArgs<Member> ea = new EntityShowEventArgs<Member>();
            ea.Completed += new MessageProcessCompleted(this.mData.UpdateList);
            this.mApp.Messages.Send(this, ea);
        }

        private void Data_ListChanged(object sender, ListChangedEventArgs e)
        {
            this.UpdateButtons();
        }

        private void UpdateButtons()
        {
            this.buttonOk.Enabled = (this.comboBoxMember.SelectedIndex >= 0);
        }

        private void LoadSettings()
        {
            long id = Saved<LoginOptions>.Instance.MemberId;

            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<Member> repository = new Repository<Member>(session);
                Member member = repository.GetById(id);

                if (member != null)
                {
                    this.SelectedMember = member;
                    this.comboBoxMember.SelectedItem = member;
                }
            }
        }

        private void SaveSettings()
        {
            if (this.SelectedMember != null)
            {
                Saved<LoginOptions>.Instance.MemberId = this.SelectedMember.Id;
            }
            else
            {
                Saved<LoginOptions>.Instance.MemberId = 0;
            }

            Saved<LoginOptions>.Save();
        }
    }
}

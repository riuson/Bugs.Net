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
        private MembersListData mData;

        public event EventHandler LoginConfirmed;
        public event EventHandler LoginRejected;

        public ControlLogin(IApplication app)
        {
            InitializeComponent();
            this.Text = "Login";
            this.mApp = app;

            this.mData = new MembersListData(app);
            this.comboBoxMember.DataSource = this.mData.Data;
            this.comboBoxMember.DisplayMember = "FullName";
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
    }
}

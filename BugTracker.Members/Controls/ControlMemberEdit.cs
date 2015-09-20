using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BugTracker.DB.Entities;
using BugTracker.Core.Extensions;

namespace BugTracker.Members.Controls
{
    public partial class ControlMemberEdit : UserControl
    {
        public event EventHandler ClickOK;
        public event EventHandler ClickCancel;
        public Member Entity { get; set; }

        public ControlMemberEdit()
        {
            InitializeComponent();
            this.Text = "Add member".Tr();
            this.buttonOk.Text = this.buttonOk.Text.Tr();
            this.buttonCancel.Text = this.buttonCancel.Text.Tr();
            this.labelFirstName.Text = this.labelFirstName.Text.Tr();
            this.labelLastName.Text = this.labelLastName.Text.Tr();
            this.labelEmail.Text = this.labelEmail.Text.Tr();
            this.Entity = null;
        }

        public ControlMemberEdit(string firstName, string lastName, string email)
            : this()
        {
            this.Text = "Edit member".Tr();
            this.textBoxFirstName.Text = firstName;
            this.textBoxLastName.Text = lastName;
            this.textBoxEmail.Text = email;
        }

        public string FirstName
        {
            get
            {
                return this.textBoxFirstName.Text;
            }
        }

        public string LastName
        {
            get
            {
                return this.textBoxLastName.Text;
            }
        }

        public string Email
        {
            get
            {
                return this.textBoxEmail.Text;
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (this.ClickOK != null)
            {
                this.ClickOK(this, EventArgs.Empty);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (this.ClickCancel != null)
            {
                this.ClickCancel(this, EventArgs.Empty);
            }
        }
    }
}

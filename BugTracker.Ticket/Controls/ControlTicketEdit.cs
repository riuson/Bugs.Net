using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BugTracker.DB.Entities;

namespace BugTracker.Members.Controls
{
    public partial class ControlTicketEdit : UserControl
    {
        public event EventHandler ClickOK;
        public event EventHandler ClickCancel;
        public Member LoggedMember { get; private set; }
        public DB.Entities.Ticket Ticket { get; private set; }

        public ControlTicketEdit(Member loggedMember)
        {
            InitializeComponent();
            this.Text = "Add ticket";
            this.LoggedMember = loggedMember;
        }

        public ControlTicketEdit(Member loggedMember, DB.Entities.Ticket ticket)
            : this(loggedMember)
        {
            this.Text = "Edit ticket";
            this.Ticket = ticket;
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BugTracker.DB.Entities;
using BugTracker.Ticket.Controls;
using BugTracker.Core.Interfaces;

namespace BugTracker.Members.Controls
{
    public partial class ControlTicketEdit : UserControl
    {
        private IApplication mApp;
        private VocabularyBox<Priority> mPriorityBox;
        private VocabularyBox<Solution> mSolutionBox;
        private VocabularyBox<Status> mStatusBox;
        private VocabularyBox<ProblemType> mProblemTypeBox;

        public event EventHandler ClickOK;
        public event EventHandler ClickCancel;
        public Member LoggedMember { get; private set; }
        public DB.Entities.Ticket Ticket { get; private set; }

        public ControlTicketEdit(IApplication app, Member loggedMember)
        {
            InitializeComponent();
            this.Text = "Add ticket";
            this.mApp = app;
            this.LoggedMember = loggedMember;

            this.mProblemTypeBox = new VocabularyBox<ProblemType>(this.mApp);
            this.mPriorityBox = new VocabularyBox<Priority>(this.mApp);
            this.mStatusBox = new VocabularyBox<Status>(this.mApp);
            this.mSolutionBox = new VocabularyBox<Solution>(this.mApp);

            this.tableLayoutPanel1.Controls.Add(this.mProblemTypeBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.mPriorityBox, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.mStatusBox, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.mSolutionBox, 1, 6);

            this.mProblemTypeBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.mPriorityBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.mStatusBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.mSolutionBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        }

        public ControlTicketEdit(IApplication app, Member loggedMember, DB.Entities.Ticket ticket)
            : this(app, loggedMember)
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

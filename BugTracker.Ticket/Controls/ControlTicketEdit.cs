using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BugTracker.DB.Entities;
using BugTracker.TicketEditor.Controls;
using BugTracker.Core.Interfaces;
using BugTracker.DB.Interfaces;
using BugTracker.DB;
using BugTracker.DB.Repositories;
using BugTracker.TicketEditor.Classes;
using BugTracker.TicketEditor.Events;
using BugTracker.Core.Classes;

namespace BugTracker.TicketEditor.Controls
{
    public partial class ControlTicketEdit : UserControl
    {
        private IApplication mApp;
        private VocabularyBox<Priority> mPriorityBox;
        private VocabularyBox<Solution> mSolutionBox;
        private VocabularyBox<Status> mStatusBox;
        private VocabularyBox<ProblemType> mProblemTypeBox;

        private TicketData mTicketData;
        private ControlTicketChanges mTicketChangesDisplay;
        private ControlTicketAttachments mTicketAttachmentsDisplay;

        public event EventHandler ClickOK;
        public event EventHandler ClickCancel;
        public Member LoggedMember { get; private set; }
        public Ticket Ticket { get; private set; }

        public ControlTicketEdit(IApplication app, Member loggedMember)
        {
            InitializeComponent();
            this.Text = "Add ticket";
            this.mApp = app;
            this.LoggedMember = loggedMember;
            this.Ticket = null;

            this.labelLoggedMember.Text = String.Format("{0} {1}", this.LoggedMember.FirstName, this.LoggedMember.LastName);
            this.labelCreated.Text = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);

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

            this.mTicketData = new TicketData();

            this.mTicketChangesDisplay = new ControlTicketChanges();
            this.tableLayoutPanel1.Controls.Add(this.mTicketChangesDisplay, 2, 1);
            this.tableLayoutPanel1.SetRowSpan(this.mTicketChangesDisplay, 7);
            this.mTicketChangesDisplay.Dock = DockStyle.Fill;

            this.mTicketAttachmentsDisplay = new ControlTicketAttachments(this.mApp);
            this.tableLayoutPanel1.Controls.Add(this.mTicketAttachmentsDisplay, 0, 7);
            this.tableLayoutPanel1.SetColumnSpan(this.mTicketAttachmentsDisplay, 2);
            this.mTicketAttachmentsDisplay.Dock = DockStyle.Fill;
            this.mTicketAttachmentsDisplay.SaveAttachment += this.OnSaveAttachment;
            this.mTicketAttachmentsDisplay.LoadAttachments += this.OnLoadAttachments;
        }

        public ControlTicketEdit(IApplication app, Member loggedMember, Ticket ticket)
            : this(app, loggedMember)
        {
            this.Text = "Edit ticket";

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                TicketRepository ticketRepository = new TicketRepository(session);
                this.Ticket = ticketRepository.Load(ticket.Id);

                this.mProblemTypeBox.SelectedValue = this.Ticket.Type;
                this.mPriorityBox.SelectedValue = this.Ticket.Priority;
                this.mStatusBox.SelectedValue = this.Ticket.Status;
                this.mSolutionBox.SelectedValue = this.Ticket.Solution;
                this.textBoxTitle.Text = this.Ticket.Title;
                this.labelCreated.Text = String.Format("{0:yyyy-MM-dd HH:mm:ss}", this.Ticket.Created);

                this.mTicketData = new TicketData(this.Ticket, session);
                this.mTicketChangesDisplay.UpdateTicketData(this.Ticket, session);
                this.mTicketAttachmentsDisplay.UpdateTicketData(this.Ticket, session);
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                TicketRepository ticketRepository = new TicketRepository(session);

                if (this.Ticket == null)
                {
                    this.Ticket = new Ticket();
                    this.Ticket.Created = DateTime.Now;
                    this.Ticket.Author = this.LoggedMember;

                    this.Ticket.Title = this.textBoxTitle.Text;
                    this.Ticket.Type = this.mProblemTypeBox.SelectedValue;
                    this.Ticket.Priority = this.mPriorityBox.SelectedValue;
                    this.Ticket.Status = this.mStatusBox.SelectedValue;
                    this.Ticket.Solution = this.mSolutionBox.SelectedValue;
                }
                else
                {
                    this.Ticket = ticketRepository.Load(this.Ticket.Id);

                    StringBuilder changedFieldsDescription = new StringBuilder();

                    if (this.Ticket.Title != this.textBoxTitle.Text)
                    {
                        changedFieldsDescription.AppendFormat(
                            "Title changed from '{0}' to '{1}'\n",
                            this.Ticket.Title,
                            this.textBoxTitle.Text);
                    }

                    if (this.Ticket.Type != this.mProblemTypeBox.SelectedValue)
                    {
                        changedFieldsDescription.AppendFormat(
                            "Type changed from '{0}' to '{1}'\n",
                            this.Ticket.Type.Value,
                            this.mProblemTypeBox.SelectedValue.Value);
                    }

                    if (this.Ticket.Priority != this.mPriorityBox.SelectedValue)
                    {
                        changedFieldsDescription.AppendFormat(
                            "Priority changed from '{0}' to '{1}'\n",
                            this.Ticket.Priority.Value,
                            this.mPriorityBox.SelectedValue.Value);
                    }

                    if (this.Ticket.Status != this.mStatusBox.SelectedValue)
                    {
                        changedFieldsDescription.AppendFormat(
                            "Status changed from '{0}' to '{1}'\n",
                            this.Ticket.Status.Value,
                            this.mStatusBox.SelectedValue.Value);
                    }

                    if (this.Ticket.Solution != this.mSolutionBox.SelectedValue)
                    {
                        changedFieldsDescription.AppendFormat(
                            "Solution changed from '{0}' to '{1}'\n",
                            this.Ticket.Solution.Value,
                            this.mSolutionBox.SelectedValue.Value);
                    }

                    if (changedFieldsDescription.Length > 0)
                    {
                        this.mTicketData.CommentAdd(changedFieldsDescription.ToString());
                    }
                }

                ticketRepository.SaveOrUpdate(this.Ticket);

                if (this.mTicketChangesDisplay.HasNewComment)
                {
                    this.mTicketData.CommentAdd(this.mTicketChangesDisplay.NewComment);
                }

                this.mTicketData.ApplyChanges(session, this.LoggedMember, this.Ticket);
            }

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

        private void OnSaveAttachment(object sender, SaveAttachmentEventArgs ea)
        {
            this.mTicketData.SaveAttachmentToFile(ea.Attachment, ea.Filename);
        }

        private void OnLoadAttachments(object sender, LoadAttachmentsEventArgs ea)
        {
            Dictionary<string, string> comments = new Dictionary<string, string>();

            foreach (var filename in ea.Filenames)
            {
                string comment;

                if (InputBox.Show("File: " + filename, "Enter comment", String.Empty, out comment) == DialogResult.OK)
                {
                    comments.Add(filename, comment);
                }
                else
                {
                    comments.Add(filename, String.Empty);
                }
            }

            foreach (var filename in ea.Filenames)
            {
                this.mTicketData.AttachmentAdd(filename, comments[filename]);
            }
        }
    }
}

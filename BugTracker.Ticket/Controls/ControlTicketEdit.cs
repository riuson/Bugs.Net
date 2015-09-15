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
using BugTracker.TicketEditor.Classes;
using BugTracker.TicketEditor.Events;
using BugTracker.Core.Classes;
using BugTracker.DB.Classes;

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
            //this.tableLayoutPanel1.Controls.Add(this.mTicketChangesDisplay, 2, 1);
            //this.tableLayoutPanel1.SetRowSpan(this.mTicketChangesDisplay, 7);
            this.mTicketChangesDisplay.Dock = DockStyle.Fill;
            this.tabPageChangelog.Controls.Add(this.mTicketChangesDisplay);

            this.mTicketAttachmentsDisplay = new ControlTicketAttachments(this.mApp);
            //this.tableLayoutPanel1.Controls.Add(this.mTicketAttachmentsDisplay, 0, 7);
            //this.tableLayoutPanel1.SetColumnSpan(this.mTicketAttachmentsDisplay, 2);
            this.mTicketAttachmentsDisplay.Dock = DockStyle.Fill;
            this.tabPageAttachments.Controls.Add(this.mTicketAttachmentsDisplay);
            this.mTicketAttachmentsDisplay.SaveAttachment += this.OnSaveAttachment;
        }

        public ControlTicketEdit(IApplication app, Member loggedMember, Ticket ticket)
            : this(app, loggedMember)
        {
            this.Text = "Edit ticket";

            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);
                this.Ticket = ticketRepository.GetById(ticket.Id);

                if (this.Ticket != null)
                {
                    this.mProblemTypeBox.SelectedValue = this.Ticket.Type;
                    this.mPriorityBox.SelectedValue = this.Ticket.Priority;
                    this.mStatusBox.SelectedValue = this.Ticket.Status;
                    this.mSolutionBox.SelectedValue = this.Ticket.Solution;
                    this.textBoxTitle.Text = this.Ticket.Title;
                    this.labelCreated.Text = String.Format("{0:yyyy-MM-dd HH:mm:ss}", this.Ticket.Created);

                    this.mTicketData = new TicketData(session, this.Ticket);
                    this.mTicketChangesDisplay.UpdateTicketData(session, this.Ticket);
                    this.mTicketAttachmentsDisplay.UpdateTicketData(session, this.Ticket);
                }
                else
                {
                    this.Ticket = ticket;
                }
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);

                // New item
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
                // Not saved item
                else if (ticketRepository.GetById(this.Ticket.Id) == null)
                {
                    this.Ticket.Created = DateTime.Now;
                    this.Ticket.Author = this.LoggedMember;

                    this.Ticket.Title = this.textBoxTitle.Text;
                    this.Ticket.Type = this.mProblemTypeBox.SelectedValue;
                    this.Ticket.Priority = this.mPriorityBox.SelectedValue;
                    this.Ticket.Status = this.mStatusBox.SelectedValue;
                    this.Ticket.Solution = this.mSolutionBox.SelectedValue;
                }
                else // Existing item
                {
                    this.Ticket = ticketRepository.Load(this.Ticket.Id);

                    StringBuilder changedFieldsDescription = new StringBuilder();

                    if (this.Ticket.Title != this.textBoxTitle.Text)
                    {
                        changedFieldsDescription.AppendFormat(
                            "Title changed from '{0}' to '{1}'\n",
                            this.Ticket.Title,
                            this.textBoxTitle.Text);
                        this.Ticket.Title = this.textBoxTitle.Text;
                    }

                    if (this.Ticket.Type != this.mProblemTypeBox.SelectedValue)
                    {
                        changedFieldsDescription.AppendFormat(
                            "Type changed from '{0}' to '{1}'\n",
                            this.Ticket.Type.Value,
                            this.mProblemTypeBox.SelectedValue.Value);
                        this.Ticket.Type = this.mProblemTypeBox.SelectedValue;
                    }

                    if (this.Ticket.Priority != this.mPriorityBox.SelectedValue)
                    {
                        changedFieldsDescription.AppendFormat(
                            "Priority changed from '{0}' to '{1}'\n",
                            this.Ticket.Priority.Value,
                            this.mPriorityBox.SelectedValue.Value);
                        this.Ticket.Priority = this.mPriorityBox.SelectedValue;
                    }

                    if (this.Ticket.Status != this.mStatusBox.SelectedValue)
                    {
                        changedFieldsDescription.AppendFormat(
                            "Status changed from '{0}' to '{1}'\n",
                            this.Ticket.Status.Value,
                            this.mStatusBox.SelectedValue.Value);
                        this.Ticket.Status = this.mStatusBox.SelectedValue;
                    }

                    if (this.Ticket.Solution != this.mSolutionBox.SelectedValue)
                    {
                        changedFieldsDescription.AppendFormat(
                            "Solution changed from '{0}' to '{1}'\n",
                            this.Ticket.Solution.Value,
                            this.mSolutionBox.SelectedValue.Value);
                        this.Ticket.Solution = this.mSolutionBox.SelectedValue;
                    }

                    if (this.mTicketAttachmentsDisplay.AddedAttachments.Length > 0)
                    {
                        foreach (Attachment attachment in this.mTicketAttachmentsDisplay.AddedAttachments)
                        {
                            changedFieldsDescription.AppendFormat(
                                "Added attachment '{0}' with comment '{1}'\n",
                                attachment.Filename,
                                attachment.Comment);
                        }
                    }

                    if (this.mTicketAttachmentsDisplay.RemovedAttachments.Length > 0)
                    {
                        foreach (Attachment attachment in this.mTicketAttachmentsDisplay.RemovedAttachments)
                        {
                            changedFieldsDescription.AppendFormat(
                                "Removed attachment '{0}' with comment '{1}'\n",
                                attachment.Filename,
                                attachment.Comment);
                        }
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

                this.mTicketData.AttachmentsAdd(this.mTicketAttachmentsDisplay.AddedAttachments);
                this.mTicketData.AttachmentsRemove(this.mTicketAttachmentsDisplay.RemovedAttachments);

                this.mTicketData.ApplyChanges(session, this.LoggedMember, this.Ticket);

                session.Transaction.Commit();
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
    }
}

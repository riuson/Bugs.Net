using BugTracker.DB.DataAccess;
using BugTracker.DB.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BugTracker.TicketEditor.Classes
{
    internal class TicketData
    {
        private ICollection<Attachment> mAttachmentsOriginal;
        private ICollection<Attachment> mAttachmentsAdd;
        private ICollection<Attachment> mAttachmentsRemove;

        private ICollection<Change> mChangesOriginal;
        private ICollection<Change> mChangesAdd;

        public TicketData()
        {
            this.mAttachmentsOriginal = new List<Attachment>();
            this.mAttachmentsAdd = new List<Attachment>();
            this.mAttachmentsRemove = new List<Attachment>();

            this.mChangesOriginal = new List<Change>();
            this.mChangesAdd = new List<Change>();
        }

        public TicketData(ISession session, Ticket ticket)
            : this()
        {
            this.mAttachmentsOriginal = ticket.Attachments;
            this.mChangesOriginal = ticket.Changes;
        }

        public void ApplyChanges(ISession session, Member loggedMember, Ticket ticket)
        {
            IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);
            IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
            IRepository<Change> changeRepository = new Repository<Change>(session);

            foreach (var attachment in this.mAttachmentsRemove)
            {
                Attachment a = attachmentRepository.Load(attachment.Id);
                attachmentRepository.Delete(a);
                ticket.Attachments.Remove(a);
            }

            foreach (var attachment in this.mAttachmentsAdd)
            {
                attachment.Author = loggedMember;
                attachment.Ticket = ticket;
                attachmentRepository.Save(attachment);
                ticket.Attachments.Add(attachment);
            }

            foreach (var change in this.mChangesAdd)
            {
                change.Author = loggedMember;
                change.Ticket = ticket;
                changeRepository.Save(change);
                ticket.Changes.Add(change);
            }

            ticketRepository.SaveOrUpdate(ticket);
        }

        public void CommentAdd(string text)
        {
            Change change = new Change();
            change.Created = DateTime.Now;
            change.Description = new BlobContent()
            {
                Content = Encoding.UTF8.GetBytes(text)
            };
            this.mChangesAdd.Add(change);
        }

        public void AttachmentsAdd(Attachment[] attachments)
        {
            foreach (Attachment attachment in attachments)
            {
                this.mAttachmentsAdd.Add(attachment);
            }
        }

        public void AttachmentsRemove(Attachment[] attachments)
        {
            foreach (Attachment attachment in attachments)
            {
                this.mAttachmentsRemove.Add(attachment);
            }
        }

        public void SaveAttachmentToFile(Attachment attachment, string filename)
        {
            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);
                BlobContent blob = blobRepository.Load(attachment.File.Id);

                using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
                {
                    blob.WriteTo(fs);
                }
            }
        }
    }
}

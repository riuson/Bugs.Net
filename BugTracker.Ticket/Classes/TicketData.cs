﻿using BugTracker.DB;
using BugTracker.DB.Classes;
using BugTracker.DB.Entities;
using BugTracker.DB.Interfaces;
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

        private ICollection<BlobContent> mBlobOriginal;
        private ICollection<BlobContent> mBlobAdd;
        private ICollection<BlobContent> mBlobRemove;

        public TicketData()
        {
            this.mAttachmentsOriginal = new List<Attachment>();
            this.mAttachmentsAdd = new List<Attachment>();
            this.mAttachmentsRemove = new List<Attachment>();

            this.mChangesOriginal = new List<Change>();
            this.mChangesAdd = new List<Change>();

            this.mBlobOriginal = new List<BlobContent>();
            this.mBlobAdd = new List<BlobContent>();
            this.mBlobRemove = new List<BlobContent>();
        }

        public TicketData(ISession session, Ticket ticket)
            : this()
        {
            this.mAttachmentsOriginal = ticket.Attachments;
            this.mChangesOriginal = ticket.Changes;

            foreach (var change in this.mChangesOriginal)
            {
                this.mBlobOriginal.Add(change.Description);
            }
        }

        public void ApplyChanges(ISession session, Member loggedMember, Ticket ticket)
        {
            IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);
            IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);
            IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
            IRepository<Change> changeRepository = new Repository<Change>(session);

            foreach (var attachment in this.mAttachmentsRemove)
            {
                Attachment a = attachmentRepository.Load(attachment.Id);
                attachmentRepository.Delete(a);
                ticket.Attachments.Remove(a);
            }

            foreach (var blob in this.mBlobRemove)
            {
                BlobContent b = blobRepository.Load(blob.Id);
                blobRepository.Delete(b);
            }

            foreach (var blob in this.mBlobAdd)
            {
                blobRepository.Save(blob);
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
            BlobContent blob = new BlobContent();
            blob.Content = Encoding.UTF8.GetBytes(text);
            this.mBlobAdd.Add(blob);

            Change change = new Change();
            change.Created = DateTime.Now;
            change.Description = blob;
            this.mChangesAdd.Add(change);
        }

        public void AttachmentsAdd(Attachment[] attachments)
        {
            foreach (Attachment attachment in attachments)
            {
                this.mBlobAdd.Add(attachment.File);
                this.mAttachmentsAdd.Add(attachment);
            }
        }

        public void AttachmentsRemove(Attachment[] attachments)
        {
            foreach (Attachment attachment in attachments)
            {
                this.mBlobRemove.Add(attachment.File);
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

﻿using BugTracker.DB;
using BugTracker.DB.Entities;
using BugTracker.DB.Interfaces;
using BugTracker.DB.Repositories;
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
            TicketRepository ticketRepository = new TicketRepository(session);
            BlobContentRepository blobRepository = new BlobContentRepository(session);
            AttachmentRepository attachmentRepository = new AttachmentRepository(session);
            ChangeRepository changeRepository = new ChangeRepository(session);

            foreach (var blob in this.mBlobAdd)
            {
                blobRepository.Save(blob);
            }

            foreach (var blob in this.mBlobRemove)
            {
                blobRepository.Delete(blob);
            }

            foreach (var attachment in this.mAttachmentsAdd)
            {
                attachment.Author = loggedMember;
                attachmentRepository.Save(attachment);
                ticket.Attachments.Add(attachment);
            }

            foreach (var attachment in this.mAttachmentsRemove)
            {
                attachmentRepository.Delete(attachment);
                ticket.Attachments.Remove(attachment);
            }

            foreach (var change in this.mChangesAdd)
            {
                change.Author = loggedMember;
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

        public void AttachmentAdd(string filename, string comment)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                BlobContent blob = new BlobContent();
                blob.ReadFrom(fs);
                this.mBlobAdd.Add(blob);

                Attachment attachment = new Attachment();
                attachment.Created = DateTime.Now;
                attachment.Comment = comment;
                attachment.Filename = Path.GetFileName(filename);
                attachment.File = blob;
                this.mAttachmentsAdd.Add(attachment);
            }
        }

        public void AttachmentRemove(Attachment entity)
        {
            this.mBlobRemove.Add(entity.File);
            this.mAttachmentsRemove.Add(entity);
        }

        public void SaveAttachmentToFile(Attachment attachment, string filename)
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                BlobContentRepository blobRepository = new BlobContentRepository(session);
                BlobContent blob = blobRepository.Load(attachment.File.Id);

                using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
                {
                    blob.WriteTo(fs);
                }
            }
        }
    }
}

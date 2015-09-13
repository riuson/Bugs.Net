using BugTracker.DB.Classes;
using BugTracker.DB.Dao.Test;
using BugTracker.DB.Entities;
using BugTracker.DB.Interfaces;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Repositories.Test
{
    [TestFixture]
    internal class TicketRepositoryTest
    {
        [SetUp]
        public virtual void Configure()
        {
            SessionManager.Instance.Configure("test.db");
            Assert.IsTrue(SessionManager.Instance.IsConfigured);
        }

        [Test]
        public virtual void CanSave()
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);
                IRepository<Change> changeRepository = new Repository<Change>(session);
                IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);
                IRepository<Priority> priorityRepository = new Repository<Priority>(session);
                IRepository<ProblemType> problemTypeRepository = new Repository<ProblemType>(session);
                IRepository<Solution> solutionRepository = new Repository<Solution>(session);
                IRepository<Status> statusRepository = new Repository<Status>(session);

                long membersBefore = memberRepository.RowCount();
                long blobsBefore = blobRepository.RowCount();
                long attachmentsBefore = attachmentRepository.RowCount();

                Ticket ticket = this.CreateAndSave(session);

                long membersAfter = memberRepository.RowCount();
                long blobsAfter = blobRepository.RowCount();
                long attachmentsAfter = attachmentRepository.RowCount();

                Assert.AreEqual(membersBefore + 1, membersAfter);
                Assert.AreEqual(blobsBefore + 2, blobsAfter);
                Assert.AreEqual(attachmentsBefore + 1, attachmentsAfter);
            }
        }

        private Ticket CreateAndSave(ISession session)
        {
            IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
            IRepository<Member> memberRepository = new Repository<Member>(session);
            IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);
            IRepository<Change> changeRepository = new Repository<Change>(session);
            IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);
            IRepository<Priority> priorityRepository = new Repository<Priority>(session);
            IRepository<ProblemType> problemTypeRepository = new Repository<ProblemType>(session);
            IRepository<Solution> solutionRepository = new Repository<Solution>(session);
            IRepository<Status> statusRepository = new Repository<Status>(session);

            Member author = new Member();
            BlobContent blob = new BlobContent();

            Attachment attachment = new Attachment();
            attachment.Author = author;
            attachment.Created = DateTime.Now;
            attachment.File = blob;

            BlobContent description = new BlobContent();
            Change change = new Change();
            change.Author = author;
            change.Created = DateTime.Now;
            change.Description = description;

            Priority priority = new Priority
            {
                Value = "priority"
            };
            Solution solution = new Solution
            {
                Value = "solution"
            };
            Status status = new Status
            {
                Value = "status"
            };
            ProblemType problemType = new ProblemType
            {
                Value = "type"
            };

            Ticket ticket = new Ticket();
            ticket.Author = author;
            ticket.Attachments.Add(attachment);
            ticket.Changes.Add(change);
            ticket.Created = DateTime.Now;
            ticket.Priority = priority;
            ticket.Solution = solution;
            ticket.Status = status;
            ticket.Type = problemType;

            attachment.Ticket = ticket;
            change.Ticket = ticket;

            priorityRepository.Save(priority);
            problemTypeRepository.Save(problemType);
            solutionRepository.Save(solution);
            statusRepository.Save(status);

            memberRepository.Save(author);
            attachmentRepository.Save(attachment);
            changeRepository.Save(change);
            ticketRepository.Save(ticket);

            Console.WriteLine(String.Format(
                "Created ticket with {0} attachments and {1} changes",
                ticket.Attachments.Count,
                ticket.Changes.Count));

            return ticket;
        }

        [Test]
        public virtual void CanGet()
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);
                IRepository<Change> changeRepository = new Repository<Change>(session);
                IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);
                IRepository<Priority> priorityRepository = new Repository<Priority>(session);
                IRepository<ProblemType> problemTypeRepository = new Repository<ProblemType>(session);
                IRepository<Solution> solutionRepository = new Repository<Solution>(session);
                IRepository<Status> statusRepository = new Repository<Status>(session);

                long membersBefore = memberRepository.RowCount();
                long blobsBefore = blobRepository.RowCount();
                long attachmentsBefore = attachmentRepository.RowCount();

                Ticket ticket = this.CreateAndSave(session);
                ticket.Title = "new title";
                ticketRepository.SaveOrUpdate(ticket);

                Ticket ticket2 = ticketRepository.GetById(ticket.Id);

                long membersAfter = memberRepository.RowCount();
                long blobsAfter = blobRepository.RowCount();
                long attachmentsAfter = attachmentRepository.RowCount();

                Assert.AreEqual(membersBefore + 1, membersAfter);
                Assert.AreEqual(blobsBefore + 2, blobsAfter);
                Assert.AreEqual(attachmentsBefore + 1, attachmentsAfter);

                Assert.AreEqual(ticket2.Title, "new title");
            }
        }

        [Test]
        public virtual void CanUpdate()
        {
            long ticketId = 0;

            long membersBefore = 0;
            long blobsBefore = 0;
            long attachmentsBefore = 0;
            long ticketsBefore = 0;

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);
                IRepository<Change> changeRepository = new Repository<Change>(session);
                IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);
                IRepository<Priority> priorityRepository = new Repository<Priority>(session);
                IRepository<ProblemType> problemTypeRepository = new Repository<ProblemType>(session);
                IRepository<Solution> solutionRepository = new Repository<Solution>(session);
                IRepository<Status> statusRepository = new Repository<Status>(session);

                membersBefore = memberRepository.RowCount();
                blobsBefore = blobRepository.RowCount();
                attachmentsBefore = attachmentRepository.RowCount();
                ticketsBefore = ticketRepository.RowCount();

                Ticket ticket = this.CreateAndSave(session);
                ticketId = ticket.Id;
            }

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);
                IRepository<Change> changeRepository = new Repository<Change>(session);
                IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);
                IRepository<Priority> priorityRepository = new Repository<Priority>(session);
                IRepository<ProblemType> problemTypeRepository = new Repository<ProblemType>(session);
                IRepository<Solution> solutionRepository = new Repository<Solution>(session);
                IRepository<Status> statusRepository = new Repository<Status>(session);

                Ticket ticket = ticketRepository.GetById(ticketId);

                Console.WriteLine(String.Format(
                    "Loaded ticket with {0} attachments and {1} changes",
                    ticket.Attachments.Count,
                    ticket.Changes.Count));
                
                ticket.Title = "new title";
                ticketRepository.SaveOrUpdate(ticket);

                //Attachment attachment = ticket.Attachments.ElementAt(0);
                //ticket.Attachments.Remove(attachment);
                //attachmentRepository.Delete(attachment);
                //ticketRepository.SaveOrUpdate(ticket);

                long membersAfter = memberRepository.RowCount();
                long blobsAfter = blobRepository.RowCount();
                long attachmentsAfter = attachmentRepository.RowCount();
                long ticketsAfter = ticketRepository.RowCount();

                //Assert.AreEqual(membersBefore + 1, membersAfter);
                //Assert.AreEqual(blobsBefore + 1, blobsAfter);
                //Assert.AreEqual(attachmentsBefore, attachmentsAfter);
                //Assert.AreEqual(0, ticket.Attachments.Count);

                //Change change = ticket.Changes.ElementAt(0);
                //ticket.Changes.Remove(change);
                //changeRepository.Delete(change);
            }

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);
                IRepository<Change> changeRepository = new Repository<Change>(session);
                IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);
                IRepository<Priority> priorityRepository = new Repository<Priority>(session);
                IRepository<ProblemType> problemTypeRepository = new Repository<ProblemType>(session);
                IRepository<Solution> solutionRepository = new Repository<Solution>(session);
                IRepository<Status> statusRepository = new Repository<Status>(session);

                Ticket ticket = ticketRepository.GetById(ticketId);

                Console.WriteLine(String.Format(
                    "Loaded ticket with {0} attachments and {1} changes",
                    ticket.Attachments.Count,
                    ticket.Changes.Count));

                ticketRepository.Delete(ticket);

                long membersAfter = memberRepository.RowCount();
                long blobsAfter = blobRepository.RowCount();
                long attachmentsAfter = attachmentRepository.RowCount();
                long ticketsAfter = ticketRepository.RowCount();

                Assert.AreEqual(membersBefore + 1, membersAfter);
                Assert.AreEqual(blobsBefore, blobsAfter);
                Assert.AreEqual(attachmentsBefore, attachmentsAfter);
                Assert.AreEqual(ticketsBefore, ticketsAfter);
            }
        }

        //[Test]
        //public virtual void CanDelete()
        //{
        //    using (ISession session = SessionManager.Instance.OpenSession())
        //    {
        //        IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
        //        IRepository<Member> memberRepository = new Repository<Member>(session);
        //        IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);

        //        Member author = new Member();
        //        BlobContent blob = new BlobContent();

        //        var attachment = new Attachment();
        //        attachment.Author = author;
        //        attachment.Created = DateTime.Now;
        //        attachment.File = blob;

        //        long membersBefore = memberRepository.RowCount();
        //        long blobsBefore = blobRepository.RowCount();
        //        long attachmentsBefore = attachmentRepository.RowCount();

        //        memberRepository.Save(author);
        //        attachmentRepository.Save(attachment);

        //        long membersAfter = memberRepository.RowCount();
        //        long blobsAfter = blobRepository.RowCount();
        //        long attachmentsAfter = attachmentRepository.RowCount();

        //        Assert.AreEqual(membersBefore + 1, membersAfter);
        //        Assert.AreEqual(blobsBefore + 1, blobsAfter);
        //        Assert.AreEqual(attachmentsBefore + 1, attachmentsAfter);

        //        attachmentRepository.Delete(attachment);

        //        membersAfter = memberRepository.RowCount();
        //        blobsAfter = blobRepository.RowCount();
        //        attachmentsAfter = attachmentRepository.RowCount();

        //        Assert.AreEqual(membersBefore + 1, membersAfter);
        //        Assert.AreEqual(blobsBefore, blobsAfter);
        //        Assert.AreEqual(attachmentsBefore, attachmentsAfter);
        //    }
        //}
    }
}

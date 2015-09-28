using BugTracker.DB.DataAccess;
using BugTracker.DB.Entities;
using NHibernate.Proxy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Tests.Entities
{
    [TestFixture]
    internal class EntityList
    {
        private Ticket CreateAndSave(ISession session, int changesCount, int attachmentsCount)
        {
            IRepository<Member> memberRepository = new Repository<Member>(session);
            IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);
            IRepository<Priority> priorityRepository = new Repository<Priority>(session);
            IRepository<ProblemType> problemTypeRepository = new Repository<ProblemType>(session);
            IRepository<Solution> solutionRepository = new Repository<Solution>(session);
            IRepository<Status> statusRepository = new Repository<Status>(session);

            Member author = new Member();

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
            ticket.Created = DateTime.Now;
            ticket.Priority = priority;
            ticket.Solution = solution;
            ticket.Status = status;
            ticket.Type = problemType;

            for (int i = 0; i < attachmentsCount; i++)
            {
                Attachment attachment = new Attachment();
                attachment.Author = author;
                attachment.Created = DateTime.Now;
                attachment.File = new BlobContent();
                attachment.Ticket = ticket;
                ticket.Attachments.Add(attachment);
            }

            for (int i = 0; i < changesCount; i++)
            {
                Change change = new Change();
                change.Author = author;
                change.Created = DateTime.Now;
                change.Description = new BlobContent();
                change.Ticket = ticket;
                ticket.Changes.Add(change);
            }

            priorityRepository.Save(priority);
            problemTypeRepository.Save(problemType);
            solutionRepository.Save(solution);
            statusRepository.Save(status);

            memberRepository.Save(author);

            ticketRepository.Save(ticket);

            Console.WriteLine(String.Format(
                "Created ticket with {0} attachments and {1} changes",
                ticket.Attachments.Count,
                ticket.Changes.Count));

            return ticket;
        }

        [Test]
        public virtual void CanSave()
        {
            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);
                IRepository<Change> changeRepository = new Repository<Change>(session);
                IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);

                long membersBefore = memberRepository.RowCount();
                long blobsBefore = blobRepository.RowCount();
                long attachmentsBefore = attachmentRepository.RowCount();
                long changesBefore = changeRepository.RowCount();
                long ticketsBefore = ticketRepository.RowCount();

                Ticket ticket = this.CreateAndSave(session, 3, 5);

                long membersAfter = memberRepository.RowCount();
                long blobsAfter = blobRepository.RowCount();
                long attachmentsAfter = attachmentRepository.RowCount();
                long changesAfter = changeRepository.RowCount();
                long ticketsAfter = ticketRepository.RowCount();

                Assert.That(membersAfter, Is.EqualTo(membersBefore + 1));
                Assert.That(blobsAfter, Is.EqualTo(blobsBefore + 8));
                Assert.That(changesAfter, Is.EqualTo(changesBefore + 3));
                Assert.That(attachmentsAfter, Is.EqualTo(attachmentsBefore + 5));
                Assert.That(ticketsAfter, Is.EqualTo(ticketsBefore + 1));

                session.Transaction.Commit();
            }
        }

        [Test]
        public void CanRemoveItemFromList()
        {
            long id = 0;
            Attachment attachment;

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                Ticket ticket = this.CreateAndSave(session, 2, 2);
                id = ticket.Id;
                attachment = ticket.Attachments.ElementAt(0);
                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);
                Ticket ticket = ticketRepository.GetById(id);

                long before = ticket.Attachments.Count;
                ticket.Attachments.Remove(attachment);
                long after = ticket.Attachments.Count;

                Assert.That(after, Is.EqualTo(before - 1));
            }
        }
    }
}

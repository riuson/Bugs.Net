using BugTracker.DB.Dao;
using BugTracker.DB.Tests.Dao;
using BugTracker.DB.Entities;
using BugTracker.DB.Interfaces;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Tests.Repositories
{
    [TestFixture]
    internal class TicketRepositoryTest
    {
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
        public virtual void CanGet()
        {
            long ticketId = 0;

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                Ticket ticket = this.CreateAndSave(session, 1, 1);
                ticketId = ticket.Id;

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);

                Ticket ticket = ticketRepository.GetById(ticketId);
                Assert.That(ticket, Is.Not.Null);
            }
        }

        [Test]
        public virtual void CanUpdate()
        {
            long ticketId = 0;

            long membersBefore = 0;
            long blobsBefore = 0;
            long attachmentsBefore = 0;
            long changesBefore = 0;
            long ticketsBefore = 0;

            int changesCount = 5;
            int attachmentsCount = 9;

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);
                IRepository<Change> changeRepository = new Repository<Change>(session);
                IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);

                membersBefore = memberRepository.RowCount();
                blobsBefore = blobRepository.RowCount();
                attachmentsBefore = attachmentRepository.RowCount();
                changesBefore = changeRepository.RowCount();
                ticketsBefore = ticketRepository.RowCount();

                Ticket ticket = this.CreateAndSave(session, changesCount, attachmentsCount);
                ticketId = ticket.Id;

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);
                IRepository<Change> changeRepository = new Repository<Change>(session);
                IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);

                Ticket ticket = ticketRepository.GetById(ticketId);

                Assert.That(ticket.Changes.Count, Is.EqualTo(changesCount));
                Assert.That(ticket.Attachments.Count, Is.EqualTo(attachmentsCount));

                ticket.Title = "new title";
                ticket.Attachments.ElementAt(0).Filename = "file name";
                ticketRepository.SaveOrUpdate(ticket);

                long membersAfter = memberRepository.RowCount();
                long blobsAfter = blobRepository.RowCount();
                long attachmentsAfter = attachmentRepository.RowCount();
                long changesAfter = changeRepository.RowCount();
                long ticketsAfter = ticketRepository.RowCount();

                Assert.That(membersAfter, Is.EqualTo(membersBefore + 1));
                Assert.That(blobsAfter, Is.EqualTo(blobsBefore + changesCount + attachmentsCount));
                Assert.That(changesAfter, Is.EqualTo(changesBefore + changesCount));
                Assert.That(attachmentsAfter, Is.EqualTo(attachmentsBefore + attachmentsCount));
                Assert.That(ticketsAfter, Is.EqualTo(ticketsBefore + 1));

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);

                Ticket ticket = ticketRepository.GetById(ticketId);

                Assert.That(ticket.Changes.Count, Is.EqualTo(changesCount));
                Assert.That(ticket.Attachments.Count, Is.EqualTo(attachmentsCount));

                Assert.That(ticket.Title, Is.EqualTo("new title"));
                Assert.That(ticket.Attachments.ElementAt(0).Filename, Is.EqualTo("file name"));
            }
        }

        [Test]
        public virtual void CanDelete()
        {
            long ticketId = 0;

            long membersBefore = 0;
            long blobsBefore = 0;
            long attachmentsBefore = 0;
            long changesBefore = 0;
            long ticketsBefore = 0;

            int changesCount = 5;
            int attachmentsCount = 9;

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);
                IRepository<Change> changeRepository = new Repository<Change>(session);
                IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);

                membersBefore = memberRepository.RowCount();
                blobsBefore = blobRepository.RowCount();
                attachmentsBefore = attachmentRepository.RowCount();
                changesBefore = changeRepository.RowCount();
                ticketsBefore = ticketRepository.RowCount();

                Ticket ticket = this.CreateAndSave(session, changesCount, attachmentsCount);
                ticketId = ticket.Id;

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);

                Ticket ticket = ticketRepository.GetById(ticketId);

                Assert.That(ticket.Changes.Count, Is.EqualTo(changesCount));
                Assert.That(ticket.Attachments.Count, Is.EqualTo(attachmentsCount));

                ticketRepository.Delete(ticket);

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);
                IRepository<Change> changeRepository = new Repository<Change>(session);
                IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);

                long membersAfter = memberRepository.RowCount();
                long blobsAfter = blobRepository.RowCount();
                long attachmentsAfter = attachmentRepository.RowCount();
                long changesAfter = changeRepository.RowCount();
                long ticketsAfter = ticketRepository.RowCount();

                Assert.That(membersAfter, Is.EqualTo(membersBefore + 1));
                Assert.That(blobsAfter, Is.EqualTo(blobsBefore));
                Assert.That(changesAfter, Is.EqualTo(changesBefore));
                Assert.That(attachmentsAfter, Is.EqualTo(attachmentsBefore));
                Assert.That(ticketsAfter, Is.EqualTo(ticketsBefore));
            }
        }

        [Test]
        public virtual void CanKeepConstraint()
        {
            long ticketId = 0;

            long membersBefore = 0;
            long blobsBefore = 0;
            long attachmentsBefore = 0;
            long changesBefore = 0;
            long ticketsBefore = 0;

            int changesCount = 5;
            int attachmentsCount = 9;

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);
                IRepository<Change> changeRepository = new Repository<Change>(session);
                IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);

                membersBefore = memberRepository.RowCount();
                blobsBefore = blobRepository.RowCount();
                attachmentsBefore = attachmentRepository.RowCount();
                changesBefore = changeRepository.RowCount();
                ticketsBefore = ticketRepository.RowCount();

                Ticket ticket = this.CreateAndSave(session, changesCount, attachmentsCount);
                ticketId = ticket.Id;

                session.Transaction.Commit();
            }

            Assert.That(delegate()
                {
                    using (ISession session = SessionManager.Instance.OpenSession(true))
                    {
                        IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                        IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);

                        Ticket ticket = ticketRepository.GetById(ticketId);
                        attachmentRepository.Delete(ticket.Attachments.ElementAt(0));

                        session.Transaction.Commit();
                    }
                },
                Throws.Exception);

            Assert.That(delegate()
            {
                using (ISession session = SessionManager.Instance.OpenSession(true))
                {
                    IRepository<Change> changeRepository = new Repository<Change>(session);
                    IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);

                    Ticket ticket = ticketRepository.GetById(ticketId);
                    changeRepository.Delete(ticket.Changes.ElementAt(0));

                    session.Transaction.Commit();
                }
            },
                Throws.Exception);

            Assert.That(delegate()
            {
                using (ISession session = SessionManager.Instance.OpenSession(true))
                {
                    IRepository<Member> memberRepository = new Repository<Member>(session);
                    IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);

                    Ticket ticket = ticketRepository.GetById(ticketId);
                    memberRepository.Delete(ticket.Author);

                    session.Transaction.Commit();
                }
            },
                Throws.Exception);

            Assert.That(delegate()
            {
                using (ISession session = SessionManager.Instance.OpenSession(true))
                {
                    IRepository<Priority> priorityRepository = new Repository<Priority>(session);
                    IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);

                    Ticket ticket = ticketRepository.GetById(ticketId);
                    priorityRepository.Delete(ticket.Priority);

                    session.Transaction.Commit();
                }
            },
                Throws.Exception);

            Assert.That(delegate()
            {
                using (ISession session = SessionManager.Instance.OpenSession(true))
                {
                    IRepository<ProblemType> problemTypeRepository = new Repository<ProblemType>(session);
                    IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);

                    Ticket ticket = ticketRepository.GetById(ticketId);
                    problemTypeRepository.Delete(ticket.Type);

                    session.Transaction.Commit();
                }
            },
                Throws.Exception);

            Assert.That(delegate()
            {
                using (ISession session = SessionManager.Instance.OpenSession(true))
                {
                    IRepository<Solution> solutionTypeRepository = new Repository<Solution>(session);
                    IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);

                    Ticket ticket = ticketRepository.GetById(ticketId);
                    solutionTypeRepository.Delete(ticket.Solution);

                    session.Transaction.Commit();
                }
            },
                Throws.Exception);

            Assert.That(delegate()
            {
                using (ISession session = SessionManager.Instance.OpenSession(true))
                {
                    IRepository<Status> statusTypeRepository = new Repository<Status>(session);
                    IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);

                    Ticket ticket = ticketRepository.GetById(ticketId);
                    statusTypeRepository.Delete(ticket.Status);

                    session.Transaction.Commit();
                }
            },
                Throws.Exception);

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<Change> changeRepository = new Repository<Change>(session);
                IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);

                Ticket ticket = ticketRepository.GetById(ticketId);

                Assert.That(ticket.Changes.Count, Is.EqualTo(changesCount));
                Assert.That(ticket.Attachments.Count, Is.EqualTo(attachmentsCount));

                long membersAfter = memberRepository.RowCount();
                long attachmentsAfter = attachmentRepository.RowCount();
                long changesAfter = changeRepository.RowCount();
                long ticketsAfter = ticketRepository.RowCount();

                Assert.That(membersAfter, Is.EqualTo(membersBefore + 1));
                Assert.That(changesAfter, Is.EqualTo(changesBefore + changesCount));
                Assert.That(attachmentsAfter, Is.EqualTo(attachmentsBefore + attachmentsCount));
                Assert.That(ticketsAfter, Is.EqualTo(ticketsBefore + 1));

                session.Transaction.Commit();
            }
        }

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
    }
}

using BugTracker.DB.Classes;
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
    internal class ProjectRepositoryTest
    {
        [Test]
        public virtual void CanSave()
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);
                IRepository<Project> projectRepository = new Repository<Project>(session);

                long membersBefore = memberRepository.RowCount();
                long ticketsBefore = ticketRepository.RowCount();
                long projectsBefore = projectRepository.RowCount();

                Project project = this.CreateAndSave(session, 3);

                long membersAfter = memberRepository.RowCount();
                long ticketsAfter = ticketRepository.RowCount();
                long projectsAfter = projectRepository.RowCount();

                Assert.That(membersAfter, Is.EqualTo(membersBefore + 1));
                Assert.That(ticketsAfter, Is.EqualTo(ticketsBefore + 3));
                Assert.That(projectsAfter, Is.EqualTo(projectsBefore + 1));
            }
        }

        [Test]
        public virtual void CanGet()
        {
            long id = 0;
            int ticketsCount = 5;

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                Project project = this.CreateAndSave(session, ticketsCount);
                id = project.Id;
            }

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Project> projectepository = new Repository<Project>(session);

                Project project = projectepository.GetById(id);
                Assert.That(project, Is.Not.Null);
                Assert.That(project.Tickets.Count, Is.EqualTo(ticketsCount));
            }
        }

        [Test]
        public virtual void CanUpdate()
        {
            long id = 0;

            long membersBefore = 0;
            long ticketsBefore = 0;
            long projectsBefore = 0;

            int ticketsCount = 5;

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);
                IRepository<Project> projectRepository = new Repository<Project>(session);

                membersBefore = memberRepository.RowCount();
                ticketsBefore = ticketRepository.RowCount();
                projectsBefore = projectRepository.RowCount();

                Project project = this.CreateAndSave(session, ticketsCount);
                id = project.Id;
            }

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);
                IRepository<Project> projectRepository = new Repository<Project>(session);

                Project project = projectRepository.GetById(id);

                Assert.That(project.Tickets.Count, Is.EqualTo(ticketsCount));

                project.Name = "new name";
                projectRepository.SaveOrUpdate(project);

                long membersAfter = memberRepository.RowCount();
                long ticketsAfter = ticketRepository.RowCount();
                long projectsAfter = projectRepository.RowCount();

                Assert.That(membersAfter, Is.EqualTo(membersBefore + 1));
                Assert.That(ticketsAfter, Is.EqualTo(ticketsBefore + ticketsCount));
                Assert.That(projectsAfter, Is.EqualTo(projectsBefore + 1));
            }

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Project> projectRepository = new Repository<Project>(session);

                Project project = projectRepository.GetById(id);

                Assert.That(project.Tickets.Count, Is.EqualTo(ticketsCount));

                Assert.That(project.Name, Is.EqualTo("new name"));
            }
        }

        [Test]
        public virtual void CanDelete()
        {
            long id = 0;

            long membersBefore = 0;
            long ticketsBefore = 0;
            long projectsBefore = 0;

            int ticketsCount = 5;

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);
                IRepository<Project> projectRepository = new Repository<Project>(session);

                membersBefore = memberRepository.RowCount();
                ticketsBefore = ticketRepository.RowCount();
                projectsBefore = projectRepository.RowCount();

                Project project = this.CreateAndSave(session, ticketsCount);
                id = project.Id;
            }

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<Ticket> ticketRepository = new Repository<Ticket>(session);
                IRepository<Project> projectRepository = new Repository<Project>(session);

                Project project = projectRepository.GetById(id);

                Assert.That(project.Tickets.Count, Is.EqualTo(ticketsCount));

                projectRepository.Delete(project);

                long membersAfter = memberRepository.RowCount();
                long ticketsAfter = ticketRepository.RowCount();
                long projectsAfter = projectRepository.RowCount();

                Assert.That(membersAfter, Is.EqualTo(membersBefore + 1));
                Assert.That(ticketsAfter, Is.EqualTo(ticketsBefore));
                Assert.That(projectsAfter, Is.EqualTo(projectsBefore));
            }
        }

        private Project CreateAndSave(ISession session, int ticketsCount)
        {
            IRepository<Member> memberRepository = new Repository<Member>(session);
            IRepository<Project> projectRepository = new Repository<Project>(session);
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

            Project project = new Project
            {
                Name = "project"
            };

            for (int i = 0; i < ticketsCount; i++)
            {
                Ticket ticket = new Ticket();
                ticket.Author = author;
                ticket.Created = DateTime.Now;
                ticket.Priority = priority;
                ticket.Solution = solution;
                ticket.Status = status;
                ticket.Type = problemType;
                project.Tickets.Add(ticket);
                ticket.Project = project;
            }

            priorityRepository.Save(priority);
            problemTypeRepository.Save(problemType);
            solutionRepository.Save(solution);
            statusRepository.Save(status);

            memberRepository.Save(author);

            projectRepository.Save(project);

            Console.WriteLine(String.Format(
                "Created project with {0} tickets",
                project.Tickets.Count));

            return project;
        }
    }
}

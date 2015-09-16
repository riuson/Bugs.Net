using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Entities.Tests
{
    [TestFixture]
    internal class TicketTest
    {
        [Test]
        public void Operation()
        {
            Member author = new Member();
            Attachment attachment = new Attachment();
            Change change = new Change();
            DateTime now = DateTime.Now;
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

            var ticket = new Ticket
            {
                Author = author,
                Created = now,
                Priority = priority,
                Solution = solution,
                Status = status,
                Type = problemType,
                Title = "title"
            };

            ticket.Changes.Add(change);
            ticket.Attachments.Add(attachment);

            Assert.That(ticket.Author.GetFullName(), Is.EqualTo(author.GetFullName()));
            Assert.That(ticket.Created, Is.EqualTo(now));
            Assert.That(ticket.Priority.Value, Is.EqualTo("priority"));
            Assert.That(ticket.Solution.Value, Is.EqualTo("solution"));
            Assert.That(ticket.Status.Value, Is.EqualTo("status"));
            Assert.That(ticket.Type.Value, Is.EqualTo("type"));
            Assert.That(ticket.Title, Is.EqualTo("title"));
            Assert.That(ticket.Attachments.Count, Is.EqualTo(1));
            Assert.That(ticket.Changes.Count, Is.EqualTo(1));
        }
    }
}

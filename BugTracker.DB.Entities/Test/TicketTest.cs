using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Entities.Test
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

            Assert.AreEqual(ticket.Author.GetFullName(), author.GetFullName());
            Assert.AreEqual(ticket.Created, now);
            Assert.AreEqual(ticket.Priority.Value, "priority");
            Assert.AreEqual(ticket.Solution.Value, "solution");
            Assert.AreEqual(ticket.Status.Value, "status");
            Assert.AreEqual(ticket.Type.Value, "type");
            Assert.AreEqual(ticket.Title, "title");
            Assert.AreEqual(ticket.Attachments.Count, 1);
            Assert.AreEqual(ticket.Changes.Count, 1);
        }
    }
}

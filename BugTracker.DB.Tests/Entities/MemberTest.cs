using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Entities.Tests
{
    [TestFixture]
    internal class MemberTest
    {
        [Test]
        public void Operation()
        {
            var member = new Member
            {
                FirstName = "First",
                LastName = "Last",
                EMail = "Email"
            };

            Assert.That(member.FirstName, Is.EqualTo("First"));
            Assert.That(member.LastName, Is.EqualTo("Last"));
            Assert.That(member.EMail, Is.EqualTo("Email"));
            Assert.That(member.GetFullName(), Is.EqualTo("First Last"));
        }
    }
}

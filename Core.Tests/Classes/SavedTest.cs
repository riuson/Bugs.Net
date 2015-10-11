using AppCore.Classes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Tests.Classes
{
    public class TestOptions
    {
        public string StringValue { get; set; }
        public int IntValue { get; set; }

        public TestOptions()
        {
            this.StringValue = "String1";
            this.IntValue = 31415926;
        }
    }

    [TestFixture]
    internal class SavedTest
    {
        [Test]
        public void CanSave()
        {
            Saved<TestOptions>.Instance.IntValue = 1;
            Saved<TestOptions>.Instance.StringValue = "abcdef";
            Saved<TestOptions>.Save();
        }


        [Test]
        public void CanLoad()
        {
            Saved<TestOptions>.Instance.IntValue = 4646;
            Saved<TestOptions>.Instance.StringValue = "aww5s";
            Saved<TestOptions>.Save();

            Saved<TestOptions>.Instance.IntValue = 78780;
            Saved<TestOptions>.Instance.StringValue = "445sd5s4d5";
            Saved<TestOptions>.Reload();

            int i = Saved<TestOptions>.Instance.IntValue;
            string s = Saved<TestOptions>.Instance.StringValue;

            Assert.That(i, Is.EqualTo(4646));
            Assert.That(s, Is.EqualTo("aww5s"));
        }
    }
}

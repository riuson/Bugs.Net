using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Entities.Tests
{
    [TestFixture]
    internal class BlobContentTest
    {
        [Test]
        public void Operation()
        {
            var blob = new BlobContent();

            byte[] buffer = new byte[10000];
            Random rnd = new Random(Convert.ToInt32(DateTime.Now.TimeOfDay.TotalMilliseconds));
            rnd.NextBytes(buffer);

            using (MemoryStream ms = new MemoryStream(buffer))
            {
                blob.ReadFrom(ms);
            }

            byte[] buffer2 = new byte[blob.Content.Length];

            using (MemoryStream ms = new MemoryStream(buffer2))
            {
                blob.WriteTo(ms);
            }

            bool success = true;

            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] != buffer2[i])
                {
                    success = false;
                    break;
                }
            }

            Assert.IsTrue(success);
        }
    }
}

using BugTracker.DB.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Tests.Entities
{
    [TestFixture]
    internal class BlobContentTest
    {
        [Test]
        public void CanWriteFile()
        {
            var blob = new BlobContent();

            byte[] buffer = new byte[10000];
            Random rnd = new Random(Convert.ToInt32(DateTime.Now.TimeOfDay.TotalMilliseconds));
            rnd.NextBytes(buffer);

            blob.Content = buffer;

            FileInfo file = new FileInfo(Path.GetTempFileName());

            using (FileStream fs = file.OpenWrite())
            {
                blob.WriteTo(fs);
                fs.Flush();
            }

            using (MemoryStream ms = new MemoryStream(buffer))
            {
                using (FileStream fs = file.OpenRead())
                {
                    Assert.That(fs, Is.EqualTo(ms));
                }
            }

            file.Delete();
        }

        [Test]
        public void CanReadFile()
        {
            byte[] buffer = new byte[10000];
            Random rnd = new Random(Convert.ToInt32(DateTime.Now.TimeOfDay.TotalMilliseconds));
            rnd.NextBytes(buffer);

            FileInfo file = new FileInfo(Path.GetTempFileName());

            using (FileStream fs = file.OpenWrite())
            {
                fs.Write(buffer, 0, buffer.Length);
                fs.Flush();
            }

            var blob = new BlobContent();

            using (FileStream fs = file.OpenRead())
            {
                blob.ReadFrom(fs);
            }

            Assert.That(blob.Content, Is.EqualTo(buffer));
        }
    }
}

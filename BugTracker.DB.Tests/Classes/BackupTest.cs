using BugTracker.DB.Classes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Tests.Classes
{
    [TestFixture]
    internal class BackupTest
    {
        [Test]
        public void CanFilterFiles()
        {
            FileInfo testfile = new FileInfo(Path.GetTempFileName());
            DirectoryInfo directory = testfile.Directory;

            string filename = Path.GetFileNameWithoutExtension(testfile.FullName);

            Backup backup = new Backup(testfile.FullName);

            var spanToRemove = backup.TimeSpanToRemove;
            var spanToObsolete = backup.TimeSpanToObsolete;

            DateTime timeNew = DateTime.Now.Subtract(spanToObsolete).AddMinutes(10);
            DateTime timeObsolete = DateTime.Now.Subtract(spanToObsolete).Subtract(TimeSpan.FromMinutes(10));
            DateTime timeRemove = DateTime.Now.Subtract(spanToRemove).Subtract(TimeSpan.FromMinutes(10));

            List<FileInfo> files = new List<FileInfo>();
            files.Add(new FileInfo(String.Format("{0}-{1:yyyyMMdd-HHmmss}", filename, timeNew)));
            files.Add(new FileInfo(String.Format("{0}-{1:yyyyMMdd-HHmmss}", filename, timeObsolete)));
            files.Add(new FileInfo(String.Format("{0}-{1:yyyyMMdd-HHmmss}", filename, timeRemove)));

            IEnumerable<FileInfo> filesNew = backup.GetFilesNew(files);
            IEnumerable<FileInfo> filesToRemove = backup.GetFilesToRemove(files);

            Assert.That(filesNew.Count(), Is.EqualTo(1));
            Assert.That(filesToRemove.Count(), Is.EqualTo(1));
        }
    }
}

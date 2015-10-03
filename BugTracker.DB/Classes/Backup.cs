using BugTracker.Core.Classes;
using BugTracker.DB.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BugTracker.DB.Classes
{
    internal class Backup
    {
        public void Process(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return;
            }

            // Settings
            TimeSpan spanToRemove = TimeSpan.FromDays(Saved<Options>.Instance.BackupKeepMaxDays);
            TimeSpan spanToObsolete = TimeSpan.FromDays(Saved<Options>.Instance.BackupKeepMinDays);

            FileInfo databaseFile = new FileInfo(value);
            DirectoryInfo backupToDirectory = BugTracker.Core.Classes.Saved<BugTracker.DB.Settings.Options>.SettinsDirectory;
            var databaseFilename = Path.GetFileNameWithoutExtension(databaseFile.FullName);
            var regDate = new Regex(@"\d{8}\-\d{6}");

            // Collect archive files
            var files = from item in backupToDirectory.GetFiles("*.gz")
                        let filename = Path.GetFileNameWithoutExtension(item.FullName)
                        where filename.Contains(databaseFilename + "-")
                        let stringDate = filename.Replace(databaseFilename + "-", String.Empty)
                        where regDate.IsMatch(stringDate)
                        let datetime = this.ParseDateTime(stringDate)
                        select new { File = item, Time = datetime };

            // Obsolete archives
            DateTime timeToRemove = DateTime.Now.Subtract(spanToRemove);
            var filesToRemove = from item in files
                                where item.Time < timeToRemove
                                select item.File;

            // Latest archives. If >= 1, do not backup
            DateTime timeToObsolete = DateTime.Now.Subtract(spanToObsolete);
            var filesNotObsolete = from item in files
                              where item.Time > timeToObsolete
                              select item.File;

            // If no one latest archive, or all existing archives are obsolete
            if (filesNotObsolete.Count() == 0 ||
                filesToRemove.Count() == files.Count())
            {
                FileInfo backupFile = new FileInfo(
                    Path.Combine(
                        backupToDirectory.FullName,
                        String.Format("{0}-{1:yyyyMMdd-HHmmss}.gz", databaseFilename, DateTime.Now)));

                this.MakeArchive(databaseFile, backupFile);
            }

            // Remove obsolete
            foreach (var file in filesToRemove)
            {
                file.Delete();
            }
        }

        private void MakeArchive(FileInfo databaseFile, FileInfo backupFile)
        {
            using (FileStream originalFileStream = databaseFile.OpenRead())
            {
                using (FileStream compressedFileStream = backupFile.OpenWrite())
                {
                    using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                    {
                        originalFileStream.CopyTo(compressionStream);
                    }
                }
            }
        }

        private DateTime ParseDateTime(string value)
        {
            DateTime dt;

            if (DateTime.TryParseExact(value, "yyyyMMdd-HHmmss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeLocal, out dt))
            {
                return dt;
            }

            return DateTime.MinValue;
        }
    }
}

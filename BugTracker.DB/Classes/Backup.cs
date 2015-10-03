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
        private FileInfo mDatabaseFile;

        public Backup(string filename)
        {
            this.mDatabaseFile = new FileInfo(filename);
        }

        public void Process(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return;
            }

            FileInfo databaseFile = new FileInfo(value);
            DirectoryInfo backupToDirectory = BugTracker.Core.Classes.Saved<BugTracker.DB.Settings.Options>.SettinsDirectory;
            var databaseFilename = Path.GetFileNameWithoutExtension(databaseFile.FullName);

            // Collect archive files
            var files = this.GetAllArchiveFiles();

            // Obsolete archives
            var filesToRemove = this.GetFilesToRemove(files);
            // Latest archives. If >= 1, do not backup
            var filesNew = this.GetFilesNew(files);

            // If no one latest archive, or all existing archives are obsolete
            if (filesNew.Count() == 0 ||
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
            if (databaseFile.Exists)
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

        public IEnumerable<FileInfo> GetAllArchiveFiles()
        {
            DirectoryInfo backupToDirectory = BugTracker.Core.Classes.Saved<BugTracker.DB.Settings.Options>.SettinsDirectory;

            var databaseFilenameOnly = Path.GetFileNameWithoutExtension(this.mDatabaseFile.FullName);
            var regDate = new Regex(@"\d{8}\-\d{6}");

            // Collect archive files
            var files = from item in backupToDirectory.GetFiles("*.gz")
                        let filename = Path.GetFileNameWithoutExtension(item.FullName)
                        where filename.Contains(databaseFilenameOnly + "-")
                        let stringDate = filename.Replace(databaseFilenameOnly + "-", String.Empty)
                        where regDate.IsMatch(stringDate)
                        orderby stringDate
                        select item;

            return files;
        }

        public IEnumerable<FileInfo> GetFilesToRemove(IEnumerable<FileInfo> archiveFiles)
        {
            var databaseFilenameOnly = Path.GetFileNameWithoutExtension(this.mDatabaseFile.FullName);
            var spanToRemove = this.TimeSpanToRemove;

            // Collect archive files
            var files = from item in archiveFiles
                        let filename = Path.GetFileNameWithoutExtension(item.FullName)
                        let stringDate = filename.Replace(databaseFilenameOnly + "-", String.Empty)
                        let datetime = this.ParseDateTime(stringDate)
                        select new { File = item, Time = datetime };

            // Obsolete archives
            var timeToRemove = DateTime.Now.Subtract(spanToRemove);
            var result = from item in files
                         where item.Time < timeToRemove
                         select item.File;

            return result;
        }

        public IEnumerable<FileInfo> GetFilesNew(IEnumerable<FileInfo> archiveFiles)
        {
            var databaseFilenameOnly = Path.GetFileNameWithoutExtension(this.mDatabaseFile.FullName);
            var spanToObsolete = this.TimeSpanToObsolete;

            // Collect archive files
            var files = from item in archiveFiles
                        let filename = Path.GetFileNameWithoutExtension(item.FullName)
                        let stringDate = filename.Replace(databaseFilenameOnly + "-", String.Empty)
                        let datetime = this.ParseDateTime(stringDate)
                        select new { File = item, Time = datetime };

            // New archives
            var timeToObsolete = DateTime.Now.Subtract(spanToObsolete);
            var result = from item in files
                         where item.Time > timeToObsolete
                         select item.File;

            return result;
        }

        public TimeSpan TimeSpanToRemove
        {
            get
            {
                return TimeSpan.FromDays(Saved<Options>.Instance.BackupKeepMaxDays);
            }
        }

        public TimeSpan TimeSpanToObsolete
        {
            get
            {
                return TimeSpan.FromDays(Saved<Options>.Instance.BackupKeepMinDays);
            }
        }
    }
}

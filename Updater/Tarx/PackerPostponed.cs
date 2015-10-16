using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Updater.Tarx
{
    public class PackerPostponed : IDisposable
    {
        private Stream mStreamOut;
        private List<Record> mRecords;
        private Log mLog;

        public delegate void Log(string message);

        public PackerPostponed(Stream streamOut, Log log = null)
        {
            this.mStreamOut = streamOut;

            if (log != null)
            {
                this.mLog = log;
            }
            else
            {
                this.mLog = (message) =>
                {
                };
            }

            this.mRecords = new List<Record>();
        }

        public void Dispose()
        {
            this.WriteAll();
        }

        public string BaseDirectory { get; set; }

        public void AddFile(string path)
        {
            this.AddFile(new FileInfo(path));
        }

        public void AddFile(FileInfo file)
        {
            if (file.Exists)
            {
                long offset = this.mStreamOut.Position;
                byte[] hash = null;

                using (FileStream fs = file.OpenRead())
                {
                    using (var md5 = MD5.Create())
                    {
                        hash = md5.ComputeHash(fs);
                    }
                }

                this.mRecords.Add(new FileRecord()
                {
                    FullPath = file.FullName,
                    RelativePath = file.FullName.Replace(this.BaseDirectory, String.Empty),
                    Length = file.Length,
                    Offset = offset,
                    Hash = this.BytesToHexString(hash)
                });

                this.mLog(file.FullName);
            }
        }

        public void AddDirectory(string path)
        {
            this.AddDirectory(new DirectoryInfo(path));
        }

        public void AddDirectory(DirectoryInfo directory)
        {
            var files = from item in directory.GetFiles("*", SearchOption.AllDirectories)
                        where item.FullName.StartsWith(this.BaseDirectory)
                        orderby item.FullName
                        select item;

            var dirs = from item in directory.GetDirectories("*", SearchOption.AllDirectories)
                       where item.FullName.StartsWith(this.BaseDirectory)
                       orderby item.FullName
                       select item;

            foreach (var item in dirs)
            {
                this.mRecords.Add(new DirectoryRecord()
                    {
                        FullPath = item.FullName,
                        RelativePath = item.FullName.Replace(this.BaseDirectory, String.Empty)
                    });
                this.mLog(item.FullName);
            }

            foreach (var file in files)
            {
                this.AddFile(file);
            }
        }

        private string BytesToHexString(byte[] array)
        {
            if (array == null || array.Length == 0)
            {
                return String.Empty;
            }

            var result = from b in array
                         let hex = String.Format("{0:X2}", b)
                         select hex;

            return String.Join(String.Empty, result.ToArray());
        }

        private void WriteAll()
        {
            this.mStreamOut.Flush();

            long documentOffset = this.mStreamOut.Position;
            byte[] zeroes = new byte[512];

            // Header document

            // Get header length
            XDocument xHeader = this.CreateHeaderDocument();

            XElement xContent = new XElement("content",
                from item in this.mRecords
                orderby item.Order
                select item.CreateElement()
            );

            xHeader.Root.Add(xContent);

            long headerLength = this.GetDocumentSize(xHeader);

            // Create actual header
            long offset = headerLength;

            this.mRecords.ForEach(item =>
            {
                if (item is FileRecord)
                {
                    (item as FileRecord).Offset = offset;
                    offset += (item as FileRecord).Length;
                    offset = this.RoundUpTo512(offset);
                }
            });

            xHeader = this.CreateHeaderDocument();
            xContent = new XElement("content",
                from item in this.mRecords
                orderby item.Order
                select item.CreateElement()
            );
            xHeader.Root.Add(xContent);

            // Write header
            this.WriteDocument(xHeader);

            // Copy files
            this.mRecords.ForEach(item =>
            {
                FileRecord filerecord = item as FileRecord;

                if (filerecord != null)
                {
                    FileInfo file = new FileInfo(filerecord.FullPath);

                    using (FileStream fs = file.OpenRead())
                    {
                        fs.CopyTo(this.mStreamOut);

                        int mod = (int)(this.mStreamOut.Position % 512);

                        if (mod != 0)
                        {
                            this.mStreamOut.Write(zeroes, 0, 512 - mod);
                        }
                    }
                }
            });
        }

        private XDocument CreateHeaderDocument()
        {
            XDocument result = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement("data",
                    new XElement("header",
                        new XElement("format", "tarx"),
                        new XElement("version", "1"),
                        new XElement("created", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss zzz")),
                        new XElement("updated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss zzz")),
                        new XElement("items",
                            new XElement("count", String.Format(CultureInfo.InvariantCulture, "0x{0:X16}", this.mRecords.Count)),
                            new XElement("totalSize",
                                String.Format(CultureInfo.InvariantCulture, "0x{0:X16}",
                                    this.mRecords
                                        .Where(item => item is FileRecord)
                                        .Select(item => item)
                                        .Sum(item => (item as FileRecord).Length))
                            )
                        )
                    )
                )
            );

            return result;
        }

        /// <summary>
        /// Get size of document, rounded to 512 bytes
        /// </summary>
        /// <param name="document">Xdocument to write</param>
        /// <returns>Length of document block</returns>
        private long GetDocumentSize(XDocument document)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = " ";
                settings.Encoding = Encoding.UTF8;
                settings.NewLineChars = Environment.NewLine;

                using (XmlWriter writer = XmlWriter.Create(ms, settings))
                {
                    document.Save(writer);
                    writer.Flush();
                }

                // Round up to 512 bytes
                long length = this.RoundUpTo512(ms.Length);

                ms.Close();

                return length;
            }
        }

        /// <summary>
        /// Write document with rounded to 512 bytes
        /// </summary>
        /// <param name="document">Xdocument to write</param>
        /// <returns>Position before, position after, length of document block</returns>
        private void WriteDocument(XDocument document)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = " ";
                settings.Encoding = Encoding.UTF8;
                settings.NewLineChars = Environment.NewLine;

                using (XmlWriter writer = XmlWriter.Create(ms, settings))
                {
                    document.Save(writer);
                    writer.Flush();
                }

                // Round up to 512 bytes
                ms.SetLength(ms.Length + ((ms.Length % 512 == 0) ? (0) : (512 - ms.Length % 512)));
                ms.Close();

                byte[] bytes = ms.ToArray();

                this.mStreamOut.Write(bytes, 0, bytes.Length);
            }
        }

        private long RoundUpTo512(long value)
        {
            long result = value + ((value % 512 == 0) ? (0) : (512 - value % 512));
            return result;
        }
    }
}

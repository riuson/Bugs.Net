using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Updater.BArchive
{
    public class Packer : IDisposable
    {
        private Stream mStreamOut;
        private long mStartOffset;
        private List<Record> mRecords;
        private Log mLog;

        public delegate void Log(string message);

        public Packer(Stream streamOut, Log log = null)
        {
            this.mStreamOut = streamOut;

            if (log == null)
            {
                log = (message) =>
                {
                };
            }

            this.mLog = log;

            // Offset to description document and its length
            this.mStartOffset = this.mStreamOut.Position;
            this.mStreamOut.Write(new byte[16], 0, 8); // long + long

            this.mLog(String.Format("Save space for offset at {0}. Current position: {1}", this.mStartOffset, this.mStreamOut.Position));

            this.mRecords = new List<Record>();
        }

        public void Dispose()
        {
            this.WriteDocument();
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
                    // Copy to output
                    fs.CopyTo(this.mStreamOut);

                    // Calculate MD5
                    fs.Seek(0, SeekOrigin.Begin);

                    using (var md5 = MD5.Create())
                    {
                        hash = md5.ComputeHash(fs);
                    }
                }

                this.mRecords.Add(new FileRecord()
                {
                    Path = file.FullName.Replace(this.BaseDirectory, String.Empty),
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
                        Path = item.FullName.Replace(this.BaseDirectory, String.Empty)
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

        private void WriteDocument()
        {
            this.mStreamOut.Flush();

            long documentOffset = this.mStreamOut.Position;

            // Description document
            {
                XDocument document = new XDocument(
                    new XDeclaration("1.0", "UTF-8", null),
                    new XElement("data",
                        new XElement("header",
                            new XElement("format", "barchive"),
                            new XElement("version", "1"),
                            new XElement("hash",
                                new XElement("algorithm", "MD5"),
                                new XElement("format", "hex")
                            ),
                            new XElement("offset",
                                new XElement("format", "decimal")
                            ),
                            new XElement("length",
                                new XElement("format", "decimal")
                            )
                        ),
                        new XElement("content",
                            from item in this.mRecords
                            orderby item.Order
                            select item.CreateElement()
                        )
                    )
                );

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = " ";
                settings.Encoding = Encoding.UTF8;
                settings.NewLineChars = Environment.NewLine;

                using (XmlWriter writer = XmlWriter.Create(this.mStreamOut, settings))
                {
                    document.Save(writer);
                }
            }

            long endOffset = this.mStreamOut.Position;
            long documentLength = endOffset - documentOffset;

            this.mStreamOut.Position = this.mStartOffset;

            byte[] documentOffsetBytes = BitConverter.GetBytes(documentOffset);
            this.mStreamOut.Write(documentOffsetBytes, 0, documentOffsetBytes.Length);

            byte[] documentLengthBytes = BitConverter.GetBytes(documentLength);
            this.mStreamOut.Write(documentLengthBytes, 0, documentLengthBytes.Length);

            this.mStreamOut.Position = endOffset;
        }
    }
}

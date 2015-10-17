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
    public class PackerLinear : IDisposable
    {
        private Stream mStreamOut;
        private long mStartPosition;
        private Log mLog;
        private bool mHeaderWritten;

        public delegate void Log(string message);

        public PackerLinear(Stream streamOut, Log log = null)
        {
            this.mStreamOut = streamOut;
            this.mStartPosition = this.mStreamOut.Position;
            this.mHeaderWritten = false;

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

        }

        public void Dispose()
        {
        }

        public string BaseDirectory { get; set; }

        public void AddFile(string path)
        {
            this.AddFile(new FileInfo(path));
        }

        public void AddFile(FileInfo file)
        {
            this.WriteHeader();

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

                FileRecord record = new FileRecord()
                {
                    FullPath = file.FullName,
                    RelativePath = file.FullName.Replace(this.BaseDirectory, String.Empty),
                    Length = file.Length,
                    Offset = offset,
                    Hash = this.BytesToHexString(hash)
                };
                this.WriteFile(file, record);

                this.mLog(file.FullName);
            }
        }

        public void AddDirectory(string path)
        {
            this.AddDirectory(new DirectoryInfo(path));
        }

        public void AddDirectory(DirectoryInfo directory)
        {
            this.WriteHeader();

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
                DirectoryRecord record = new DirectoryRecord()
                    {
                        FullPath = item.FullName,
                        RelativePath = item.FullName.Replace(this.BaseDirectory, String.Empty)
                    };
                this.WriteDirectory(item, record);
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

        private void WriteHeader()
        {
            if (!this.mHeaderWritten)
            {
                this.mHeaderWritten = true;
                // Write header
                XDocument xHeader = this.CreateHeaderDocument();
                this.WriteDocument(xHeader);
            }
        }

        private void WriteDirectory(DirectoryInfo file, DirectoryRecord record)
        {
            XElement xDirectory = record.CreateElement();
            XDocument document = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement("data",
                    new XElement("content",
                        xDirectory
                    )
                )
            );

            // Add user data here

            // Write document
            this.WriteDocument(document);
        }

        private void WriteFile(FileInfo file, FileRecord record)
        {
            XElement xFile = record.CreateElement();
            XDocument document = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement("data",
                    new XElement("content",
                        xFile
                    )
                )
            );

            // Add user data here

            // Update data
            long length = this.GetDocumentSize(document);
            xFile.Element("offset").Value = String.Format(CultureInfo.InvariantCulture, "0x{0:X16}", this.mStreamOut.Position + length);

            // Write document
            this.WriteDocument(document);
            // Write file
            using (FileStream fs = file.OpenRead())
            {
                fs.CopyTo(this.mStreamOut);

                byte[] zeroes = new byte[512];
                int mod = (int)(this.mStreamOut.Position % 512);

                if (mod != 0)
                {
                    this.mStreamOut.Write(zeroes, 0, 512 - mod);
                }
            }
        }

        private XDocument CreateHeaderDocument()
        {
            XDocument result = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement("data",
                    new XElement("header",
                        new XElement("format", "tarx"),
                        new XElement("version", "1"),
                        new XElement("mode", "linear"),
                        new XElement("created", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss zzz")),
                        new XElement("updated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss zzz"))
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
                long length = this.RoundUpTo(ms.Length, 512);

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

        private long RoundUpTo(long value, long mod)
        {
            long result = value + ((value % mod == 0) ? (0) : (mod - value % mod));
            return result;
        }
    }
}

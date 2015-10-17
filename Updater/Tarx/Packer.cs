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
    public class Packer : IDisposable
    {
        private Stream mStreamOut;
        private Log mLog;
        private IPacker mPackerPrivate;

        public Packer(Stream streamOut, bool postpone, Log log = null)
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

            if (postpone)
            {
                this.mPackerPrivate = new PackerPostponed(this.mStreamOut, this.CreateHeaderDocument(), this.mLog);
            }
            else
            {
                this.mPackerPrivate = new PackerLinear(this.mStreamOut, this.CreateHeaderDocument(), this.mLog);
            }
        }

        public void Dispose()
        {
            this.mPackerPrivate.Dispose();
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

                FileRecord record = new FileRecord()
                {
                    FullPath = file.FullName,
                    RelativePath = file.FullName.Replace(this.BaseDirectory, String.Empty),
                    Length = file.Length,
                    Offset = offset,
                    Hash = PackerHelper.BytesToHexString(hash)
                };

                this.mPackerPrivate.AddFile(file, record);
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
                DirectoryRecord record = new DirectoryRecord()
                {
                    FullPath = item.FullName,
                    RelativePath = item.FullName.Replace(this.BaseDirectory, String.Empty)
                };

                this.mPackerPrivate.AddDirectory(item, record);
            }

            foreach (var file in files)
            {
                this.AddFile(file);
            }
        }

        public XDocument XHeader
        {
            get { return this.mPackerPrivate.XHeader; }
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
                        new XElement("updated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss zzz"))
                    )
                )
            );

            return result;
        }
    }
}

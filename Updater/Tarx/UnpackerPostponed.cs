using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Updater.Tarx
{
    internal class UnpackerPostponed : IUnpacker
    {
        private Stream mStreamIn;
        private XDocument mHeaderDocument;

        public XDocument XContent { get; private set; }

        public UnpackerPostponed(Stream streamIn, XDocument header)
        {
            this.mStreamIn = streamIn;
            this.mHeaderDocument = header;
            this.CollectContentInfo();
        }

        public void Dispose()
        {
        }

        public bool CollectContentInfo()
        {
            this.XContent = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement("data")
            );

            this.XContent.Root.Add(this.mHeaderDocument.Descendants("content").First());

            return true;
        }

        public bool UnpackTo(DirectoryInfo directory, Func<XElement, Boolean> xitemCallback)
        {
            if (xitemCallback == null)
            {
                xitemCallback = (item) =>
                    {
                        return true;
                    };
            }

            var xDirs = from item in this.XContent.Root.Element("content").Elements("directory")
                        let dirPath = item.Element("path").Value
                        orderby dirPath
                        select item;

            var xFiles = from item in this.XContent.Root.Element("content").Elements("file")
                         let filePath = item.Element("path").Value
                         let offset = Int64.Parse(item.Element("offset").Value.Remove(0, 2), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture)
                         orderby offset, filePath
                         select item;

            Func<XElement, string> fillPath = (xItem) =>
                {
                    string fullPath = this.CombinePath(directory.FullName, xItem.Element("path").Value);
                    XElement xFullPath = xItem.Element("fullpath");

                    if (xFullPath == null)
                    {
                        xFullPath = new XElement("fullpath");
                        xItem.Add(xFullPath);
                    }

                    xFullPath.SetValue(fullPath);

                    return fullPath;
                };

            foreach (var item in xDirs)
            {
                fillPath(item);

                if (xitemCallback(item))
                {
                    this.ProcessDir(item);
                }
            }

            foreach (var item in xFiles)
            {
                fillPath(item);

                if (xitemCallback(item))
                {
                    this.ProcessFile(item);
                }
            }

            return true;
        }

        private string CombinePath(params string[] paths)
        {
            var p = from item in paths
                    let needFix = item.StartsWith(Convert.ToString(Path.DirectorySeparatorChar))
                    let pathFixed = needFix ? item.Trim(Path.DirectorySeparatorChar) : item
                    select pathFixed;

            return Path.Combine(p.ToArray());
        }

        private void ProcessDir(XElement xItem)
        {
            DirectoryInfo directory = new DirectoryInfo(xItem.Element("fullpath").Value);

            if (!directory.Exists)
            {
                directory.Create();
            }
        }

        private void ProcessFile(XElement xItem)
        {
            FileInfo file = new FileInfo(xItem.Element("fullpath").Value);

            long offset = Int64.Parse(xItem.Element("offset").Value.Remove(0, 2), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            long length = Int64.Parse(xItem.Element("length").Value.Remove(0, 2), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            byte[] buffer = new byte[65536];

            using (FileStream fs = file.Open(FileMode.Create, FileAccess.Write))
            {
                long count = length;

                do
                {
                    long toRead = count > buffer.Length ? buffer.LongLength : count;

                    int readed = this.mStreamIn.Read(buffer, 0, (int)toRead);
                    fs.Write(buffer, 0, readed);

                    count -= readed;
                } while (count > 0);
            }

            // Round stream position to 512
            long tail = this.RoundUpTo(length, 512) - length;
            this.mStreamIn.Read(buffer, 0, (int)tail);
        }

        private long RoundUpTo(long value, long mod)
        {
            long result = value + ((value % mod == 0) ? (0) : (mod - value % mod));
            return result;
        }
    }
}

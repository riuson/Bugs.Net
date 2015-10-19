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
    internal class PackerPostponed : IPacker
    {
        private Stream mStreamOut;
        private List<Record> mRecords;
        private Log mLog;

        public XDocument XHeader { get; private set; }

        public PackerPostponed(Stream streamOut, XDocument header, Log log = null)
        {
            this.mStreamOut = streamOut;
            this.XHeader = header;
            this.XHeader.Root.Element("header").Add(
                new XElement("mode", "postponed"));

            this.mLog = log;

            this.mRecords = new List<Record>();
        }

        public void Dispose()
        {
            this.WriteAll();
        }

        public void AddFile(FileInfo file, FileRecord record)
        {
            if (file.Exists)
            {
                this.mRecords.Add(record);
            }
        }

        public void AddDirectory(DirectoryInfo directory, DirectoryRecord record)
        {
            this.mRecords.Add(record);
        }

        private void WriteAll()
        {
            // Header document

            // Get header length
            XDocument xHeader = this.XHeader;

            var orderedRecords = from item in this.mRecords
                                 orderby item.Order, item.FullPath
                                 select item;

            XElement xItems = new XElement("items",
                 new XElement("count", String.Format(CultureInfo.InvariantCulture, "0x{0:X16}", this.mRecords.Count)),
                 new XElement("totalSize",
                     String.Format(CultureInfo.InvariantCulture, "0x{0:X16}",
                         orderedRecords
                             .Where(item => item is FileRecord)
                             .Select(item => item)
                             .Sum(item => (item as FileRecord).Length))
                 )
             );

            xHeader.Root.Element("header").Add(xItems);

            XElement xContent = new XElement("content",
                from item in orderedRecords
                select item.CreateElement()
            );

            xHeader.Root.Add(xContent);

            long headerLength = PackerHelper.GetDocumentSize(xHeader, 512);

            // Create actual header
            long offset = headerLength;

            foreach (var item in orderedRecords)
            {
                if (item is FileRecord)
                {
                    (item as FileRecord).Offset = offset;
                    offset += (item as FileRecord).Length;
                    offset = PackerHelper.RoundUpTo(offset, 512);
                }
            };

            xContent.RemoveAll();
            xContent.Add(
                from item in this.mRecords
                orderby item.Order
                select item.CreateElement());

            // Write header
            PackerHelper.WriteDocument(xHeader, 512, this.mStreamOut);

            // Copy files
            foreach (var item in orderedRecords)
            {
                FileRecord filerecord = item as FileRecord;

                if (filerecord != null)
                {
                    FileInfo file = new FileInfo(filerecord.FullPath);
                    PackerHelper.FileToStream(file, this.mStreamOut, filerecord.Offset, 512);
                }
            };
        }
    }
}

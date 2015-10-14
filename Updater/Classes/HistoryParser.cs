using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Updater.Classes
{
    public class HistoryParser
    {
        private XDocument mDocument;
        private const int Version = 1;

        public bool IsValid { get; private set; }
        public string LatestFilePath { get; private set; }
        public byte[] LatestFileHash { get; private set; }
        public DateTime LatestCommitDate { get; private set; }

        private HistoryParser()
        {
            this.mDocument = null;
            this.IsValid = false;
            this.LatestFilePath = String.Empty;
            this.LatestFileHash = new byte[] { };
        }

        public HistoryParser(XDocument document)
            : this()
        {
            this.mDocument = document;
            this.IsValid = this.Parse();
        }

        public HistoryParser(string filename)
            : this()
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                this.mDocument = XDocument.Load(reader);
                this.IsValid = this.Parse();
            }
        }

        private bool Parse()
        {
            try
            {
                XElement xHistory = (from item in this.mDocument.Root.Elements("history")
                                     where (string)item.Attribute("version") == Version.ToString()
                                     select item).FirstOrDefault();

                if (xHistory != null)
                {
                    IEnumerable<XElement> xOrderedHistory = from item in xHistory.Elements("record")
                                                            let date = DateTime.ParseExact(item.Element("commit").Element("date").Value, "yyyy-MM-dd HH:mm:ss zzz", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)
                                                            orderby date descending
                                                            select item;

                    XElement xNewestRecord = xOrderedHistory.FirstOrDefault();

                    if (xNewestRecord != null)
                    {
                        this.LatestFilePath = xNewestRecord.Element("downloads").Element("file").Element("path").Value;
                        this.LatestFileHash = this.HexStringToBytes(xNewestRecord.Element("downloads").Element("file").Element("hash").Value);
                        this.LatestCommitDate = DateTime.ParseExact(xNewestRecord.Element("commit").Element("date").Value, "yyyy-MM-dd HH:mm:ss zzz", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                        return true;
                    }
                }
            }
            catch
            {
            }

            return false;
        }

        private byte[] HexStringToBytes(string value)
        {
            var result = from index in Enumerable.Range(0, value.Length)
                         where (index & 0x01) == 0
                         let b = Convert.ToByte(value.Substring(index, 2), 16)
                         select b;
            return result.ToArray();
        }
    }
}

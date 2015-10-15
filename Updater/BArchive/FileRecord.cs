using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Updater.BArchive
{
    internal class FileRecord : Record
    {
        public override int Order { get { return 2; } }
        public long Offset { get; set; }
        public long Length { get; set; }
        public string Hash { get; set; }

        public override System.Xml.Linq.XElement CreateElement()
        {
            XElement result = new XElement("file",
                new XElement("path", this.Path),
                new XElement("offset", Convert.ToString(this.Offset, CultureInfo.InvariantCulture)),
                new XElement("length", Convert.ToString(this.Length, CultureInfo.InvariantCulture)),
                new XElement("hash", this.Hash)
            );

            return result;
        }
    }
}

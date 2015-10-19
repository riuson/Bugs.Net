using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Updater.Tarx
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
                new XElement("path", this.RelativePath),
                new XElement("offset", String.Format(CultureInfo.InvariantCulture, "0x{0:X16}", this.Offset)),
                new XElement("length", String.Format(CultureInfo.InvariantCulture, "0x{0:X16}", this.Length)),
                new XElement("hash", this.Hash)
            );

            return result;
        }
    }
}

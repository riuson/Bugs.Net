using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Updater.BArchive
{
    internal class DirectoryRecord : Record
    {
        public override int Order { get { return 1; } }

        public override System.Xml.Linq.XElement CreateElement()
        {
            XElement result = new XElement("directory",
                new XElement("path", this.Path)
            );

            return result;
        }
    }
}

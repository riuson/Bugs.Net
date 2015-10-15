using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Updater.BArchive
{
    internal abstract class Record
    {
        public abstract int Order { get; }
        public string Path { get; set; }
        public abstract XElement CreateElement();
    }
}

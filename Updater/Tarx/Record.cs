using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Updater.Tarx
{
    internal abstract class Record
    {
        public abstract int Order { get; }
        public string RelativePath { get; set; }
        public string FullPath { get; set; }
        public abstract XElement CreateElement();
    }
}

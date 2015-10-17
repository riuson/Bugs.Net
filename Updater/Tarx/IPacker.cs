using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Updater.Tarx
{
    internal interface IPacker : IDisposable
    {
        XDocument XHeader { get; }
        void AddDirectory(DirectoryInfo directory, DirectoryRecord record);
        void AddFile(FileInfo file, FileRecord record);
    }
}

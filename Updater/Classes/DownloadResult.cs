using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Updater.Classes
{
    public class DownloadResult
    {
        public FileInfo TempFile { get; private set; }
        public HistoryParser History { get; private set; }

        public DownloadResult(HistoryParser history, FileInfo file)
        {
            this.History = history;
            this.TempFile = file;
        }
    }
}

using AppCore.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Updater.Classes;

namespace Updater.Events
{
    public class UpdateReceivedEventArgs : MessageEventArgs
    {
        public DownloadResult Result { get; set; }

        public UpdateReceivedEventArgs(DownloadResult result)
        {
            this.Result = result;
        }
    }
}

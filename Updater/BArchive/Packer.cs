using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Updater.BArchive
{
    public class Packer : IDisposable
    {
        private Stream mStreamOut;

        public Packer(Stream streamOut)
        {
            this.mStreamOut = streamOut;
        }

        public void Dispose()
        {
        }
    }
}

using FluentNHibernate.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Entities
{
    public class BlobContent : Entity
    {
        public virtual byte[] Content { get; set; }

        public virtual void ReadFrom(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                this.Content = ms.ToArray();
            }
        }

        public virtual void WriteTo(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream(this.Content))
            {
                ms.CopyTo(stream);
            }
        }
    }
}

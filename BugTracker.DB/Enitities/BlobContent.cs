using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Entities
{
    public class BlobContent : Entity<long>
    {
        public virtual byte[] Content { get; set; }

        /// <summary>
        /// Set Content from stream
        /// </summary>
        /// <param name="stream">Source stream</param>
        public virtual void ReadFrom(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                this.Content = ms.ToArray();
            }
        }

        /// <summary>
        /// Set Content from value
        /// </summary>
        /// <param name="value">Source string</param>
        public virtual void SetString(string value)
        {
            this.Content = Encoding.UTF8.GetBytes(value);
        }

        /// <summary>
        /// Write Content to output stream
        /// </summary>
        /// <param name="stream">Output stream</param>
        public virtual void WriteTo(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream(this.Content))
            {
                ms.CopyTo(stream);
            }
        }

        /// <summary>
        /// Get Content as string
        /// </summary>
        /// <returns>Content string</returns>
        public virtual string GetString()
        {
            return Encoding.UTF8.GetString(this.Content);
        }
    }
}

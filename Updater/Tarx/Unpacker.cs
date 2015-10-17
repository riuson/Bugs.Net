using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Updater.Tarx
{
    public class Unpacker : IDisposable
    {
        private Stream mStreamIn;
        private long mStartPosition;
        private Log mLog;
        private IUnpacker mUnpackerPrivate;

        public XDocument XHeader { get; private set; }

        public delegate void Log(string message);

        public Unpacker(Stream streamIn, Log log = null)
        {
            this.mStreamIn = streamIn;
            this.mStartPosition = this.mStreamIn.Position;

            if (log != null)
            {
                this.mLog = log;
            }
            else
            {
                this.mLog = (message) =>
                {
                };
            }

            this.XHeader = this.GetHeader();
            this.mUnpackerPrivate = new UnpackerPostponed(this.mStreamIn, this.XHeader);
        }

        public void Dispose()
        {
            this.mUnpackerPrivate.Dispose();
        }

        private XDocument GetHeader()
        {
            XDocument xHeader = this.ReadXDocument();
            return xHeader;
        }

        private XDocument ReadXDocument()
        {
            List<byte> buffer = new List<byte>();

            {
                // Do not use 'using' to leave base stream opened
                BinaryReader reader = new BinaryReader(this.mStreamIn);
                // Read bytes until 0x00 - zeroes after document
                while (true)
                {
                    byte[] bytes = reader.ReadBytes(512);
                    buffer.AddRange(bytes);

                    if (bytes[511] == 0x00)
                    {
                        break;
                    }
                }
            }

            // Remove zeroes from end
            buffer = buffer.TakeWhile(item => item != 0x00).ToList();

            // Convert byte array to XDocument
            using (MemoryStream ms = new MemoryStream(buffer.ToArray()))
            {
                using (XmlReader reader = XmlReader.Create(ms))
                {
                    var xDocument = XDocument.Load(reader);
                    return xDocument;
                }
            }
        }

        public bool CollectContentInfo()
        {
            return this.mUnpackerPrivate.CollectContentInfo();
        }

        public XDocument XContent
        {
            get
            {
                return this.mUnpackerPrivate.XContent;
            }
        }

        public bool UnpackTo(DirectoryInfo directory, Func<XElement, Boolean> xitemCallback)
        {
            return this.mUnpackerPrivate.UnpackTo(directory, xitemCallback);
        }
    }
}

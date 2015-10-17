using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Updater.Tarx
{
    internal class PackerLinear : IPacker
    {
        private Stream mStreamOut;
        private Log mLog;
        private bool mHeaderWritten;

        public XDocument XHeader { get; private set; }

        public PackerLinear(Stream streamOut, XDocument header, Log log = null)
        {
            this.mStreamOut = streamOut;
            this.mHeaderWritten = false;
            this.XHeader = header;
            this.XHeader.Root.Element("header").Add(
                new XElement("mode", "linear"));

            this.mLog = log;

        }

        public void Dispose()
        {
        }

        public void AddFile(FileInfo file, FileRecord record)
        {
            this.WriteHeader();

            if (file.Exists)
            {
                long offset = this.mStreamOut.Position;
                this.WriteFile(file, record);
            }
        }

        public void AddDirectory(DirectoryInfo directory, DirectoryRecord record)
        {
            this.WriteHeader();

            this.WriteDirectory(directory, record);
        }

        private void WriteHeader()
        {
            if (!this.mHeaderWritten)
            {
                this.mHeaderWritten = true;
                // Write header
                PackerHelper.WriteDocument(this.XHeader, 512, this.mStreamOut);
            }
        }

        private void WriteDirectory(DirectoryInfo file, DirectoryRecord record)
        {
            XElement xDirectory = record.CreateElement();
            XDocument document = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement("data",
                    new XElement("content",
                        xDirectory
                    )
                )
            );

            // Add user data here

            // Write document
            PackerHelper.WriteDocument(document, 512, this.mStreamOut);
        }

        private void WriteFile(FileInfo file, FileRecord record)
        {
            XElement xFile = record.CreateElement();
            XDocument document = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement("data",
                    new XElement("content",
                        xFile
                    )
                )
            );

            // Add user data here

            // Update data
            long length = PackerHelper.GetDocumentSize(document, 512);
            xFile.Element("offset").Value = String.Format(CultureInfo.InvariantCulture, "0x{0:X16}", this.mStreamOut.Position + length);

            // Write document
            PackerHelper.WriteDocument(document, 512, this.mStreamOut);
            // Write file
            PackerHelper.FileToStream(file, this.mStreamOut, 512);
        }
    }
}

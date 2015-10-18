using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Updater.Tarx
{
    internal class UnpackerLinear : IUnpacker
    {
        private Stream mStreamIn;
        private XDocument mHeaderDocument;
        private Log mLog;

        public XDocument XContent { get; private set; }

        public UnpackerLinear(Stream streamIn, XDocument header, Log log)
        {
            this.mStreamIn = streamIn;
            this.mHeaderDocument = header;
            this.mLog = log;
        }

        public void Dispose()
        {
        }

        public bool CollectContentInfo()
        {
            XElement collectedContent = new XElement("content");
            this.XContent = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement("data",
                    collectedContent
                )
            );

            this.mLog("Collecting content info...");

            while (this.mStreamIn.CanRead)
            {
                XDocument document = PackerHelper.ReadNextDocument(this.mStreamIn);

                if (document.Root == null)
                {
                    break;
                }

                var contentElements = document.Root.Element("content").Elements();

                collectedContent.Add(contentElements);

                XElement xFile = contentElements.Where(element => element.Name == "file").LastOrDefault();

                if (xFile != null)
                {
                    long offset = Int64.Parse(xFile.Element("offset").Value.Remove(0, 2), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                    long length = Int64.Parse(xFile.Element("length").Value.Remove(0, 2), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                    string path = xFile.Element("path").Value;
                    long offsetFromCurrent = PackerHelper.RoundUpTo(length, 512);
                    //this.mLog(String.Format("Offset: {0:X16}, Length: {1:X16}, Path: {2}", offset, length, path));
                    this.mStreamIn.Seek(offsetFromCurrent, SeekOrigin.Current);
                }
                //this.XContent.Root.Add(this.mHeaderDocument.Descendants("content").First());
            }
            this.mLog(this.XContent.ToString());

            return true;
        }

        public bool UnpackTo(DirectoryInfo directory, Func<XElement, Boolean> xitemCallback)
        {
            while (true)
            {
                XDocument document = PackerHelper.ReadNextDocument(this.mStreamIn);

                if (document.Root == null)
                {
                    break;
                }

                var contentElements = document.Root.Element("content").Elements();
                XElement xElement = contentElements.LastOrDefault();

                Func<XElement, string> fillPath = (xItem) =>
                {
                    string fullPath = PackerHelper.CombinePath(directory.FullName, xItem.Element("path").Value);
                    XElement xFullPath = xItem.Element("fullpath");

                    if (xFullPath == null)
                    {
                        xFullPath = new XElement("fullpath");
                        xItem.Add(xFullPath);
                    }

                    xFullPath.SetValue(fullPath);

                    return fullPath;
                };

                fillPath(xElement);

                if (xitemCallback(xElement))
                {
                    switch (xElement.Name.LocalName)
                    {
                        case "directory":
                            {
                                this.ProcessDir(xElement);
                                break;
                            }
                        case "file":
                            {
                                this.ProcessFile(xElement);
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
            }

            return true;
        }

        private void ProcessDir(XElement xItem)
        {
            DirectoryInfo directory = new DirectoryInfo(xItem.Element("fullpath").Value);

            if (!directory.Exists)
            {
                directory.Create();
            }
        }

        private void ProcessFile(XElement xItem)
        {
            FileInfo file = new FileInfo(xItem.Element("fullpath").Value);

            long offset = Int64.Parse(xItem.Element("offset").Value.Remove(0, 2), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            long length = Int64.Parse(xItem.Element("length").Value.Remove(0, 2), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            byte[] buffer = new byte[65536];

            using (FileStream fs = file.Open(FileMode.Create, FileAccess.Write))
            {
                long count = length;

                do
                {
                    long toRead = count > buffer.Length ? buffer.LongLength : count;

                    int readed = this.mStreamIn.Read(buffer, 0, (int)toRead);
                    fs.Write(buffer, 0, readed);

                    count -= readed;
                } while (count > 0);
            }

            // Round stream position to 512
            long tail = this.RoundUpTo(length, 512) - length;
            this.mStreamIn.Read(buffer, 0, (int)tail);
        }

        private long RoundUpTo(long value, long mod)
        {
            long result = value + ((value % mod == 0) ? (0) : (mod - value % mod));
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Updater.CommandLine
{
    internal class Runner : IDisposable
    {
        private DirectoryInfo mTargetDirectory;
        private FileInfo mArchiveFile;
        private Guid mInstanceId;

        public Runner(string[] arguments)
        {
            string documentFile = arguments[0];

            if (File.Exists(documentFile))
            {
                XDocument document;

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationFlags = System.Xml.Schema.XmlSchemaValidationFlags.None;
                settings.XmlResolver = null;

                using (FileStream fs = new FileStream(documentFile, FileMode.Open, FileAccess.Read))
                {
                    using (XmlReader reader = XmlReader.Create(fs, settings))
                    {
                        document = XDocument.Load(reader);
                    }
                }

                XElement xUpdate = document.Root.Element("update");
                this.mTargetDirectory = new DirectoryInfo(xUpdate.Element("applicationDirectory").Value);
                this.mArchiveFile = new FileInfo(xUpdate.Element("archiveFile").Value);
                this.mInstanceId = new Guid(xUpdate.Element("instanceId").Value);
            }
        }

        internal void Run()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormUpdater());
        }

        public void Dispose()
        {
        }
    }
}

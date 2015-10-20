using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Updater.CommandLine
{
    internal static class StartUpdater
    {
        private static string GetExecutablePath()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return path;
        }

        private static string GetTempPath()
        {
            string path = Path.Combine(Path.GetTempPath(), "Updater.exe");
            return path;
        }

        private static string CopyToTemp()
        {
            string temp = GetTempPath();
            File.Copy(GetExecutablePath(), temp, true);
            return temp;
        }

        private static XDocument CreateOptionsDocument(DirectoryInfo applicationDirectory, FileInfo archiveFile, Guid instanceId)
        {
            XDocument document = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement("data",
                    new XElement("update",
                        new XElement("applicationDirectory", applicationDirectory.FullName),
                        new XElement("archiveFile", archiveFile.FullName),
                        new XElement("instanceId", instanceId.ToString())
                    )
                )
            );

            return document;
        }

        private static string SaveOptionsDocument(XDocument document)
        {
            string path = Path.ChangeExtension(GetTempPath(), ".xml");

            using (FileStream fs = File.Open(path, FileMode.Create))
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = " ";
                settings.Encoding = Encoding.UTF8;
                settings.NewLineChars = Environment.NewLine;

                using (XmlWriter writer = XmlWriter.Create(fs, settings))
                {
                    document.Save(writer);
                }
            }

            return path;
        }

        public static void Run(DirectoryInfo applicationDirectory, FileInfo archiveFile, Guid instanceId)
        {
            string tempExe = CopyToTemp();
            string tempXml = SaveOptionsDocument(CreateOptionsDocument(applicationDirectory, archiveFile, instanceId));

            //Process process = new Process();
            //process.StartInfo = new ProcessStartInfo(tempExe);
            //process.StartInfo.Arguments = tempXml;
            //process.StartInfo.WorkingDirectory = Path.GetDirectoryName(tempExe);
            //process.Start();
            //process.WaitForExit();
        }
    }
}

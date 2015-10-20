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
    internal static class Preparer
    {
        private static FileInfo GetAssemblyFile(Assembly assembly)
        {
            string codeBase = assembly.CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return new FileInfo(path);
        }

        private static FileInfo[] GetRequiredFiles()
        {
            FileInfo[] result = new FileInfo[]{
                GetAssemblyFile(Assembly.GetExecutingAssembly()),
                GetAssemblyFile(typeof(AppCore.IApplication).Assembly)
            };

            return result;
        }

        private static string GetTempDir()
        {
            return Path.GetTempPath();
        }

        private static string GetTempExe()
        {
            string tempExe = Path.Combine(GetTempDir(), Path.GetFileName(GetRequiredFiles()[0].FullName));
            return tempExe;
        }

        private static void CopyToTemp()
        {
            string temp = GetTempDir();
            FileInfo[] files = GetRequiredFiles();

            foreach (var file in files)
            {
                file.CopyTo(Path.Combine(temp, Path.GetFileName(file.FullName)), true);
            }
        }

        private static XDocument CreateOptionsDocument(
            DirectoryInfo applicationDirectory,
            FileInfo archiveFile,
            FileInfo callerFile,
            Guid instanceId)
        {
            XDocument document = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement("data",
                    new XElement("update",
                        new XElement("applicationDirectory", applicationDirectory.FullName),
                        new XElement("archiveFile", archiveFile.FullName),
                        new XElement("callerFile", callerFile.FullName),
                        new XElement("instanceId", instanceId.ToString())
                    )
                )
            );

            return document;
        }

        private static string SaveOptionsDocument(XDocument document)
        {
            string path = Path.ChangeExtension(GetTempDir(), ".xml");

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

        public static void Run(DirectoryInfo applicationDirectory, FileInfo archiveFile, FileInfo callerFile, Guid instanceId)
        {
            CopyToTemp();
            string tempExe = GetTempExe();
            string tempXml = SaveOptionsDocument(
                CreateOptionsDocument(
                    applicationDirectory,
                    archiveFile,
                    callerFile,
                    instanceId));

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(tempExe);
            process.StartInfo.Arguments = tempXml;
            process.StartInfo.WorkingDirectory = GetTempDir();
            process.Start();
            //process.WaitForExit();
        }
    }
}

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Updater.Tarx;

namespace Updater.Tests.Tarx
{
    [TestFixture]
    internal class PackerTest
    {
        [Test]
        public void CanPackPostponed()
        {
            this.Pack(
                new DirectoryInfo(Path.GetDirectoryName(this.GetThisDirectory())),
                new FileInfo(Path.Combine(Path.GetTempPath(), "packed.postpone.tarx")),
                true);
        }

        [Test]
        public void CanPackLinear()
        {
            this.Pack(
                new DirectoryInfo(Path.GetDirectoryName(this.GetThisDirectory())),
                new FileInfo(Path.Combine(Path.GetTempPath(), "packed.linear.tarx")),
                false);
        }

        [Test]
        public void CanUnpackPostponed()
        {
            this.Pack(
                new DirectoryInfo(Path.GetDirectoryName(this.GetThisDirectory())),
                new FileInfo(Path.Combine(Path.GetTempPath(), "packed.postpone.tarx")),
                true);

            this.Unpack(
                new FileInfo(Path.Combine(Path.GetTempPath(), "packed.postpone.tarx")),
                new DirectoryInfo(Path.Combine(Path.GetTempPath(), "unpacked.postpone")));
        }

        [Test]
        public void CanUnpackLinear()
        {
            this.Pack(
                new DirectoryInfo(Path.GetDirectoryName(this.GetThisDirectory())),
                new FileInfo(Path.Combine(Path.GetTempPath(), "packed.linear.tarx")),
                true);

            this.Unpack(
                new FileInfo(Path.Combine(Path.GetTempPath(), "packed.linear.tarx")),
                new DirectoryInfo(Path.Combine(Path.GetTempPath(), "unpacked.linear")));
        }

        private string GetThisDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            path = Path.GetDirectoryName(path);
            return path;
        }

        private void Log(string message)
        {
            Console.WriteLine(message);
        }

        public void Pack(DirectoryInfo sourceDirectory, FileInfo targetFile, bool postpone)
        {
            this.Log(String.Format("From {0} to {1}", sourceDirectory, targetFile));

            using (FileStream fs = new FileStream(targetFile.FullName, FileMode.Create, FileAccess.ReadWrite))
            {
                using (Packer packer = new Packer(fs, postpone, this.Log))
                {
                    packer.BaseDirectory = sourceDirectory.FullName;
                    packer.AddDirectory(sourceDirectory);
                }
            }
        }

        public void Unpack(FileInfo sourceFile, DirectoryInfo targetDirectory)
        {
            this.Log(String.Format("From {0} to {1}", sourceFile, targetDirectory));

            using (FileStream fs = new FileStream(sourceFile.FullName, FileMode.Open, FileAccess.Read))
            {
                using (Unpacker unpacker = new Unpacker(fs, this.Log))
                {
                    XDocument xHeader = unpacker.XHeader;

                    this.Log("Extracting:");
                    unpacker.UnpackTo(targetDirectory, item =>
                        {
                            Console.WriteLine(item.ToString());
                            return true;
                        });
                }
            }
        }
    }
}

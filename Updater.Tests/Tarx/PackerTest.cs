using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
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
            DirectoryInfo sourceDirectory = new DirectoryInfo(Path.GetDirectoryName(this.GetThisDirectory()));
            DirectoryInfo targetDirectory = new DirectoryInfo(Path.Combine(Path.GetTempPath(), "unpacked.postpone"));
            FileInfo packedFile = new FileInfo(Path.Combine(Path.GetTempPath(), "packed.postpone.tarx"));

            this.Pack(
                sourceDirectory,
                packedFile,
                true);

            this.Unpack(
                packedFile,
                targetDirectory);

            this.CompareDirectories(sourceDirectory, targetDirectory);
        }

        [Test]
        public void CanUnpackLinear()
        {
            DirectoryInfo sourceDirectory = new DirectoryInfo(Path.GetDirectoryName(this.GetThisDirectory()));
            DirectoryInfo targetDirectory = new DirectoryInfo(Path.Combine(Path.GetTempPath(), "unpacked.linear"));
            FileInfo packedFile = new FileInfo(Path.Combine(Path.GetTempPath(), "packed.linear.tarx"));

            this.Pack(
                sourceDirectory,
                packedFile,
                false);

            this.Unpack(
                packedFile,
                targetDirectory);

            this.CompareDirectories(sourceDirectory, targetDirectory);
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

        private void Pack(DirectoryInfo sourceDirectory, FileInfo targetFile, bool postpone)
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

        private void Unpack(FileInfo sourceFile, DirectoryInfo targetDirectory)
        {
            this.Log(String.Format("From {0} to {1}", sourceFile, targetDirectory));
            targetDirectory.Delete(true);

            using (FileStream fs = new FileStream(sourceFile.FullName, FileMode.Open, FileAccess.Read))
            {
                using (Unpacker unpacker = new Unpacker(fs, this.Log))
                {
                    XDocument xHeader = unpacker.XHeader;

                    this.Log("Extracting...");
                    unpacker.UnpackTo(targetDirectory, item =>
                        {
                            Console.WriteLine(item.Element("path").Value);
                            return true;
                        });
                }
            }
        }

        private void CompareDirectories(DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory)
        {
            var sourceFiles = from item in sourceDirectory.GetFiles("*.*", SearchOption.AllDirectories)
                              orderby item.FullName
                              select item;

            var targetFiles = from item in targetDirectory.GetFiles("*.*", SearchOption.AllDirectories)
                              orderby item.FullName
                              select item;

            Assert.That(sourceFiles.Count(), Is.EqualTo(targetFiles.Count()));

            var sourceFileNames = from item in sourceFiles
                                  let path = item.FullName.Replace(sourceDirectory.FullName, String.Empty)
                                  orderby path
                                  select path;

            var targetFileNames = from item in targetFiles
                                  let path = item.FullName.Replace(targetDirectory.FullName, String.Empty)
                                  orderby path
                                  select path;

            Assert.That(sourceFileNames, Is.EqualTo(targetFileNames));

            Assert.That(sourceFiles, Is.EqualTo(targetFiles).Using<FileInfo>(this.FilesComparer));
        }

        private int FilesComparer(FileInfo file1, FileInfo file2)
        {
            byte[] hash1;
            byte[] hash2;

            using (var md5 = MD5.Create())
            {
                using (FileStream fs = file1.OpenRead())
                {
                    hash1 = md5.ComputeHash(fs);
                }

                using (FileStream fs = file2.OpenRead())
                {
                    hash2 = md5.ComputeHash(fs);
                }
            }

            Assert.That(hash1, Is.EqualTo(hash2), String.Format("Failure on compare files {0} and {1}", file1, file2));

            return 0;
        }
    }
}

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Updater.BArchive;

namespace Updater.Tests.BArchive
{
    [TestFixture]
    internal class PackerTest
    {
        [Test]
        public void CanPack()
        {
            string path = this.GetThisDirectory();
            path = Path.GetDirectoryName(path);
            this.Log("Source path: " + path);

            string temp = Path.Combine(Path.GetTempPath(), "packer.barc");
            this.Log("Output file: " + temp);

            using (FileStream fs = new FileStream(temp, FileMode.Create, FileAccess.ReadWrite))
            {
                using (Packer packer = new Packer(fs, this.Log))
                {
                    packer.BaseDirectory = path;
                    packer.AddDirectory(path);
                }
            }
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
    }
}

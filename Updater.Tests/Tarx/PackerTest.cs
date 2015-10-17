using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Updater.Tarx;

namespace Updater.Tests.Tarx
{
    [TestFixture]
    internal class PackerTest
    {
        [Test]
        public void CanPackPostponed()
        {
            string path = this.GetThisDirectory();
            path = Path.GetDirectoryName(path);
            this.Log("Source path: " + path);

            string temp = Path.Combine(Path.GetTempPath(), "packer.tarx");
            this.Log("Output file: " + temp);

            using (FileStream fs = new FileStream(temp, FileMode.Create, FileAccess.ReadWrite))
            {
                using (PackerPostponed packer = new PackerPostponed(fs, this.Log))
                {
                    packer.BaseDirectory = path;
                    packer.AddDirectory(path);
                }
            }
        }

        [Test]
        public void CanPackLinear()
        {
            string path = this.GetThisDirectory();
            path = Path.GetDirectoryName(path);
            this.Log("Source path: " + path);

            string temp = Path.Combine(Path.GetTempPath(), "packer.tarx");
            this.Log("Output file: " + temp);

            using (FileStream fs = new FileStream(temp, FileMode.Create, FileAccess.ReadWrite))
            {
                using (PackerLinear packer = new PackerLinear(fs, this.Log))
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

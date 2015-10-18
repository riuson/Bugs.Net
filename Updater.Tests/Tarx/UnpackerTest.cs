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
    internal class UnpackerTest
    {
        [Test]
        public void CanUnpack()
        {
            string extractPath = Path.Combine(Path.GetTempPath(), "tarx");
            //path = Path.GetDirectoryName(path);
            this.Log("Extract path: " + extractPath);
            DirectoryInfo extractDir = new DirectoryInfo(extractPath);

            string temp = Path.Combine(Path.GetTempPath(), "packer.tarx");
            this.Log("Source file: " + temp);

            using (FileStream fs = new FileStream(temp, FileMode.Open, FileAccess.Read))
            {
                using (Unpacker unpacker = new Unpacker(fs, this.Log))
                {
                    XDocument xHeader = unpacker.XHeader;
                    //this.Log("Header:");
                    //this.Log(xHeader.ToString());

                    //if (unpacker.CollectContentInfo())
                    {
                        //XDocument xContent = unpacker.XContent;
                        //this.Log("Content:");
                        //this.Log(xContent.ToString());

                        this.Log("Extracting:");
                        unpacker.UnpackTo(extractDir, item =>
                            {
                                Console.WriteLine(item.ToString());
                                return true;
                            });
                    }
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

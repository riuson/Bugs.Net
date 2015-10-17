using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Updater.Tarx
{
    internal class PackerHelper
    {
        /// <summary>
        /// Convert byte array to hex string
        /// </summary>
        /// <param name="array">Source byte array</param>
        /// <returns>Hex string</returns>
        public static string BytesToHexString(byte[] array)
        {
            if (array == null || array.Length == 0)
            {
                return String.Empty;
            }

            var result = from b in array
                         let hex = String.Format("{0:X2}", b)
                         select hex;

            return String.Join(String.Empty, result.ToArray());
        }

        /// <summary>
        /// Round value to top nearest
        /// </summary>
        /// <param name="value">Original value</param>
        /// <param name="mod">Count of bytes round to</param>
        /// <returns>Result value</returns>
        public static long RoundUpTo(long value, long mod)
        {
            long result = value + ((value % mod == 0) ? (0) : (mod - value % mod));
            return result;
        }

        /// <summary>
        /// Get size of document, rounded to 512 bytes
        /// </summary>
        /// <param name="document">Xdocument to write</param>
        /// <param name="mod">Count of bytes round to</param>
        /// <returns>Length of document block</returns>
        public static long GetDocumentSize(XDocument document, int mod)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = " ";
                settings.Encoding = Encoding.UTF8;
                settings.NewLineChars = Environment.NewLine;

                using (XmlWriter writer = XmlWriter.Create(ms, settings))
                {
                    document.Save(writer);
                    writer.Flush();
                }

                // Round up to 512 bytes
                long length = RoundUpTo(ms.Length, mod);

                ms.Close();

                return length;
            }
        }

        /// <summary>
        /// Write document with rounded to 512 bytes
        /// </summary>
        /// <param name="document">Xdocument to write</param>
        /// <param name="mod">Count of bytes round to</param>
        /// <returns>Position before, position after, length of document block</returns>
        public static void WriteDocument(XDocument document, int mod, Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = " ";
                settings.Encoding = Encoding.UTF8;
                settings.NewLineChars = Environment.NewLine;

                using (XmlWriter writer = XmlWriter.Create(ms, settings))
                {
                    document.Save(writer);
                    writer.Flush();
                }

                // Round up to 512 bytes
                long length = RoundUpTo(ms.Length, 512);
                ms.SetLength(length);
                ms.Close();

                byte[] bytes = ms.ToArray();

                stream.Write(bytes, 0, bytes.Length);
            }
        }

        public static void FileToStream(FileInfo file, Stream stream, int mod)
        {
            using (FileStream fs = file.OpenRead())
            {
                fs.CopyTo(stream);

                int a = (int)(stream.Position % mod);

                if (a != 0)
                {
                    byte[] zeroes = new byte[mod - a];
                    stream.Write(zeroes, 0, zeroes.Length);
                }
            }

        }
    }
}

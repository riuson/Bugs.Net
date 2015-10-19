using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Updater.Tarx
{
    internal interface IUnpacker : IDisposable
    {
        /// <summary>
        /// Collect information from archive about all included content
        /// </summary>
        /// <returns>True if successfull</returns>
        bool CollectContentInfo();
        /// <summary>
        /// XDocument with collected content information
        /// </summary>
        XDocument XContent { get; }
        /// <summary>
        /// Start unpacking process to specified directory
        /// </summary>
        /// <param name="directory">Target directory</param>
        /// <param name="xitemCallback">callback for each unpacked item. Returns true, if unpacking confirmed.</param>
        /// <returns>True if successfull</returns>
        bool UnpackTo(DirectoryInfo directory, Func<XElement, Boolean> xitemCallback);
        /// <summary>
        /// Action on header XElement after reading
        /// </summary>
        Action<XElement> HeaderAfter { get; set; }
    }
}

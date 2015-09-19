using BugTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;

namespace BugTracker.Core.Classes
{
    internal class LocalizationManager
    {
        /// <summary>
        /// Creator
        /// </summary>
        private sealed class LocalizationManagerCreator
        {
            /// <summary>
            /// Lazy initializer
            /// </summary>
            public static LocalizationManager Instance
            {
                get { return mInstance; }
            }
            static readonly LocalizationManager mInstance = new LocalizationManager();
        }

        /// <summary>
        /// Private constructor
        /// </summary>
        private LocalizationManager()
        {
        }

        /// <summary>
        /// Singletone instance of Database object
        /// </summary>
        public static LocalizationManager Instance
        {
            get { return LocalizationManagerCreator.Instance; }
        }

        public string Translate(Assembly assembly, MethodBase method, string value)
        {
            string id = ReplaceChars(value);
            CultureInfo culture = Thread.CurrentThread.CurrentUICulture;
            Tuple<XmlDocument, FileInfo> document = GetDocument(assembly, culture);
            string result = this.GetTranslation(document, method, value, id);
            return result;
        }

        private Tuple<XmlDocument, FileInfo> GetDocument(Assembly assembly, CultureInfo culture)
        {
            UriBuilder uri = new UriBuilder(assembly.CodeBase);
            string path = Uri.UnescapeDataString(uri.Path);

            DirectoryInfo dir = new DirectoryInfo(Path.Combine(Path.GetDirectoryName(path), culture.Name));
            if (!dir.Exists)
            {
                dir.Create();
            }

            FileInfo file = new FileInfo(Path.Combine(dir.FullName, Path.ChangeExtension(Path.GetFileName(path), ".xml")));

            XmlDocument doc = new XmlDocument();

            if (file.Exists)
            {
                using (FileStream fs = file.OpenRead())
                {
                    doc.Load(fs);
                }
            }
            else
            {
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(xmlDeclaration, root);

                XmlElement rootElement = doc.CreateElement(string.Empty, "root", string.Empty);
                doc.AppendChild(rootElement);

                rootElement.Attributes.Append(doc.CreateAttribute("culture")).InnerText = culture.Name; ;

                using (FileStream fs = file.OpenWrite())
                {
                    doc.Save(fs);
                }
            }

            return new Tuple<XmlDocument, FileInfo>(doc, file);
        }

        private string GetTranslation(Tuple<XmlDocument, FileInfo> document, MethodBase method, string valueDefault, string valueId)
        {
            XmlDocument doc = document.Item1;
            FileInfo file = document.Item2;
            string className = method.ReflectedType.Name;
            string methodName = ReplaceChars(className + "_" + method.Name);

            XmlNode nodeMethod = doc.DocumentElement.SelectSingleNode(methodName);

            if (nodeMethod == null)
            {
                nodeMethod = doc.CreateElement(methodName);
                doc.DocumentElement.AppendChild(nodeMethod);
            }

            XmlNode node = nodeMethod.SelectSingleNode(valueId);


            if (node != null)
            {
                XmlNode cdata = node.FirstChild;
                return cdata.InnerText;
            }
            else
            {
                node = nodeMethod.AppendChild(doc.CreateElement(valueId));
                Regex reg = new Regex("[\\<\\>\\\"\\\']");
                if (reg.IsMatch(valueDefault))
                {
                    XmlNode cdata = doc.CreateCDataSection(valueDefault);
                    node.AppendChild(cdata);
                }
                else
                {
                    node.InnerText = valueDefault;
                }

                using (FileStream fs = file.OpenWrite())
                {
                    doc.Save(fs);
                }

                return valueDefault;
            }

        }

        private string ReplaceChars(string value)
        {
            Regex reg = new Regex("[\\W]");
            return reg.Replace(value, "_");
        }
    }
}

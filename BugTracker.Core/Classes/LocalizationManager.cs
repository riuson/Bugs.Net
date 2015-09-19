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
            this.mTranslations = new Dictionary<string, TranslationData>();
        }

        /// <summary>
        /// Singletone instance of Database object
        /// </summary>
        public static LocalizationManager Instance
        {
            get { return LocalizationManagerCreator.Instance; }
        }

        private Dictionary<string, TranslationData> mTranslations;

        public string Translate(Assembly assembly, MethodBase method, string value, string comment = "")
        {
            CultureInfo culture = Thread.CurrentThread.CurrentUICulture;

            TranslationData data = this.GetData(assembly, culture);

            string className = method.ReflectedType.Name;
            string methodName = this.CleanString(className + "_" + method.Name);
            string id = this.GetHash(methodName + value);

            return data.GetTranslation(id, methodName, value, comment);
        }

        public void Save()
        {
            foreach (var value in this.mTranslations.Values)
            {
                value.SaveChanges();
            }
        }

        private TranslationData GetData(Assembly assembly, CultureInfo culture)
        {
            UriBuilder uri = new UriBuilder(assembly.CodeBase);
            string path = Uri.UnescapeDataString(uri.Path);

            DirectoryInfo dir = new DirectoryInfo(Path.Combine(Path.GetDirectoryName(path), culture.Name));
            if (!dir.Exists)
            {
                dir.Create();
            }

            FileInfo file = new FileInfo(Path.Combine(dir.FullName, Path.ChangeExtension(Path.GetFileName(path), ".xml")));

            TranslationData data = null;

            if (this.mTranslations.ContainsKey(file.FullName))
            {
                data = this.mTranslations[file.FullName];
            }
            else
            {
                data = new TranslationData(file, culture);
                this.mTranslations.Add(file.FullName, data);
            }

            return data;
        }

        private string CleanString(string value)
        {
            Regex reg = new Regex("[\\W]");
            string result = reg.Replace(value, "_");

            while (result.Contains("__"))
            {
                result = result.Replace("__", "_");
            }

            return result;
        }

        private string GetHash(string value)
        {
            return value.GetHashCode().ToString();
        }

        private class TranslationData
        {
            private FileInfo mFile;
            private CultureInfo mCulture;
            private XmlDocument mDocument;
            private bool mChanged;

            public TranslationData(FileInfo file, CultureInfo culture)
            {
                this.mCulture = culture;
                this.mFile = file;

                if (file.Exists)
                {
                    this.mDocument = new XmlDocument();
                    this.mDocument.Load(file.FullName);
                    this.mChanged = false;
                }
                else
                {
                    this.mDocument = new XmlDocument();
                    this.mChanged = true;

                    // Write down the XML declaration
                    XmlDeclaration xmlDeclaration = this.mDocument.CreateXmlDeclaration("1.0", "utf-8", null);

                    // Create the root element
                    XmlElement rootElement = this.mDocument.CreateElement("messages");
                    this.mDocument.InsertBefore(xmlDeclaration, this.mDocument.DocumentElement);
                    this.mDocument.AppendChild(rootElement);

                    rootElement.Attributes.Append(this.mDocument.CreateAttribute("culture")).InnerText = culture.Name;
                }
            }

            internal string GetTranslation(string id, string methodName, string valueDefault, string comment)
            {
                XmlNode nodeMethod = this.mDocument.DocumentElement.SelectSingleNode(
                    String.Format("message[id={0}]", id));

                if (nodeMethod != null)
                {
                    XmlNode cdata = nodeMethod.SelectSingleNode("translated");
                    return cdata.InnerText;
                }
                else
                {
                    XmlNode nodeMessage = this.mDocument.DocumentElement.AppendChild(this.mDocument.CreateElement("message"));

                    {
                        XmlNode nodeId = this.GetNode(nodeMessage, "id", true);
                        nodeId.InnerText = id;
                    }

                    {
                        XmlNode nodeSource = this.GetNode(nodeMessage, "source", true);
                        XmlNode cdata = this.mDocument.CreateCDataSection(valueDefault);
                        nodeSource.AppendChild(cdata);
                    }

                    {
                        XmlNode nodeTranslated = this.GetNode(nodeMessage, "translated", true);
                        XmlNode cdata = this.mDocument.CreateCDataSection(valueDefault);
                        nodeTranslated.AppendChild(cdata);
                    }

                    if (comment != String.Empty)
                    {
                        XmlNode nodeComment = this.GetNode(nodeMessage, "comment", true);
                        XmlNode cdata = this.mDocument.CreateCDataSection(comment);
                        nodeComment.AppendChild(cdata);
                    }

                    this.mChanged = true;

                    return valueDefault;
                }
            }

            private XmlNode GetNode(XmlNode parent, string name, bool create)
            {
                XmlNode result = parent.SelectSingleNode(name);

                if (result == null)
                {
                    result = parent.OwnerDocument.CreateElement(name);
                    parent.AppendChild(result);
                }
                return result;
            }

            public void SaveChanges()
            {
                if (this.mChanged)
                {
                    this.mDocument.Save(this.mFile.FullName);
                    this.mChanged = false;
                }
            }
        }
    }
}

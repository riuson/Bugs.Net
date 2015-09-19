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

        public string Translate(Assembly assembly, MethodBase method, string value)
        {
            CultureInfo culture = Thread.CurrentThread.CurrentUICulture;

            TranslationData data = this.GetData(assembly, culture);

            string result = this.GetTranslation(data, method, value, this.CleanString(value));

            return result;
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

        private string GetTranslation(TranslationData data, MethodBase method, string valueDefault, string valueId)
        {
            string className = method.ReflectedType.Name;
            string methodName = this.CleanString(className + "_" + method.Name);

            return data.GetTranslation(methodName, valueId, valueDefault);
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

                    XmlDeclaration xmlDeclaration = this.mDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
                    XmlElement root = this.mDocument.DocumentElement;
                    this.mDocument.InsertBefore(xmlDeclaration, root);

                    XmlElement rootElement = this.mDocument.CreateElement(string.Empty, "root", string.Empty);
                    this.mDocument.AppendChild(rootElement);

                    rootElement.Attributes.Append(this.mDocument.CreateAttribute("culture")).InnerText = culture.Name;
                }
            }

            internal string GetTranslation(string methodName, string valueId, string valueDefault)
            {
                XmlNode nodeMethod = this.mDocument.DocumentElement.SelectSingleNode(methodName);

                if (nodeMethod == null)
                {
                    nodeMethod = this.mDocument.CreateElement(methodName);
                    this.mDocument.DocumentElement.AppendChild(nodeMethod);
                }

                XmlNode node = nodeMethod.SelectSingleNode(valueId);

                if (node != null)
                {
                    XmlNode cdata = node.FirstChild;
                    return cdata.InnerText;
                }
                else
                {
                    node = nodeMethod.AppendChild(this.mDocument.CreateElement(valueId));
                    Regex reg = new Regex("[\\<\\>\\\"\\\']");
                    if (reg.IsMatch(valueDefault))
                    {
                        XmlNode cdata = this.mDocument.CreateCDataSection(valueDefault);
                        node.AppendChild(cdata);
                    }
                    else
                    {
                        node.InnerText = valueDefault;
                    }

                    this.mChanged = true;

                    return valueDefault;
                }
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

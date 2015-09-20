﻿using BugTracker.Core.Interfaces;
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
        private string LanguagesDir
        {
            get
            {
                UriBuilder uri = new UriBuilder(Assembly.GetExecutingAssembly().CodeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                path = Path.GetDirectoryName(path);
                return Path.Combine(path, "Languages");
            }
        }

        public string GetTranslation(Assembly assembly, MethodBase method, string value, string comment = "")
        {
            CultureInfo culture = Thread.CurrentThread.CurrentUICulture;

            TranslationData data = this.GetData(assembly, culture);

            string className = method.ReflectedType.Name;
            string methodName = this.CleanString(className + "_" + method.Name);
            string id = this.GetHash(methodName + value);

            return data.GetTranslation(id, methodName, value, comment);
        }

        public void SetTranslation(Assembly assembly, MethodBase method, string value, string translation, string comment = "")
        {
            CultureInfo culture = Thread.CurrentThread.CurrentUICulture;

            TranslationData data = this.GetData(assembly, culture);

            string className = method.ReflectedType.Name;
            string methodName = this.CleanString(className + "_" + method.Name);
            string id = this.GetHash(methodName + value);

            data.SetTranslation(id, methodName, value, translation, comment);
        }

        public void Save()
        {
            foreach (var value in this.mTranslations.Values)
            {
                value.SaveChanges();
            }
        }

        public IEnumerable<CultureInfo> FoundCultures
        {
            get
            {
                DirectoryInfo directory = new DirectoryInfo(this.LanguagesDir);
                DirectoryInfo []subdirs = directory.GetDirectories();
                IEnumerable<CultureInfo> cultures = subdirs.Select<DirectoryInfo, CultureInfo>(dir => new CultureInfo(dir.Name));
                return cultures;
            }
        }

        private TranslationData GetData(Assembly assembly, CultureInfo culture)
        {
            string path = this.LanguagesDir;
            string assemblyFilename = Path.GetFileName(assembly.CodeBase);

            string translationFileTemplate = Path.Combine(path, "{0}", Path.ChangeExtension(assemblyFilename, ".xml"));

            string filename = String.Format(translationFileTemplate, culture.Name);

            TranslationData data = null;

            if (this.mTranslations.ContainsKey(filename))
            {
                data = this.mTranslations[filename];
            }
            else
            {
                data = new TranslationData(translationFileTemplate, culture);
                this.mTranslations.Add(filename, data);
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
            private string mFilename;
            private CultureInfo mCulture;
            private XmlDocument mDocument;
            private bool mChanged;

            public TranslationData(string filenameTemplate, CultureInfo culture)
            {
                this.mCulture = culture;

                string translationDirTemplate = Path.GetDirectoryName(filenameTemplate);
                DirectoryInfo dir = new DirectoryInfo(String.Format(translationDirTemplate, culture.Name));
                if (!dir.Exists)
                {
                    dir.Create();
                }

                this.mFilename = String.Format(filenameTemplate, culture.Name);
                bool fileCopiedFromParent = false;

                if (!File.Exists(this.mFilename))
                {
                    if (culture != CultureInfo.InvariantCulture && culture.Parent != CultureInfo.InvariantCulture)
                    {
                        string filenameParent = String.Format(filenameTemplate, culture.Parent.Name);

                        if (File.Exists(filenameParent))
                        {
                            File.Copy(filenameParent, this.mFilename);
                            fileCopiedFromParent = true;
                        }
                    }
                }

                if (File.Exists(this.mFilename))
                {
                    this.mDocument = new XmlDocument();
                    this.mDocument.Load(this.mFilename);
                    this.mChanged = false;

                    if (fileCopiedFromParent)
                    {
                        XmlNode nodeRoot = this.mDocument.DocumentElement;
                        nodeRoot.Attributes["culture"].InnerText = culture.Name;
                    }
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

            internal string GetTranslation(string id, string methodName, string source, string comment)
            {
                XmlNode nodeMethod = this.mDocument.DocumentElement.SelectSingleNode(
                    String.Format("message[id={0}]", id));

                if (nodeMethod != null)
                {
                    XmlNode nodeTranslated = nodeMethod.SelectSingleNode("translated");
                    XmlCDataSection cdata = (XmlCDataSection)nodeTranslated.FirstChild;
                    return cdata.Value;
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
                        XmlCDataSection cdata = this.mDocument.CreateCDataSection(source);
                        nodeSource.AppendChild(cdata);
                    }

                    {
                        XmlNode nodeTranslated = this.GetNode(nodeMessage, "translated", true);
                        XmlCDataSection cdata = this.mDocument.CreateCDataSection(source);
                        nodeTranslated.AppendChild(cdata);
                    }

                    if (comment != String.Empty)
                    {
                        XmlNode nodeComment = this.GetNode(nodeMessage, "comment", true);
                        XmlCDataSection cdata = this.mDocument.CreateCDataSection(comment);
                        nodeComment.AppendChild(cdata);
                    }

                    this.mChanged = true;

                    return source;
                }
            }

            internal void SetTranslation(string id, string methodName, string source, string translation, string comment)
            {
                XmlNode nodeMessage = this.mDocument.DocumentElement.SelectSingleNode(
                    String.Format("message[id={0}]", id));

                if (nodeMessage != null)
                {
                    XmlNode nodeTranslated = this.GetNode(nodeMessage, "translated", true);
                    nodeTranslated.RemoveAll();
                    XmlNode cdata = this.mDocument.CreateCDataSection(translation);
                    nodeTranslated.AppendChild(cdata);
                    this.mChanged = true;
                }
                else
                {
                    nodeMessage = this.mDocument.DocumentElement.AppendChild(this.mDocument.CreateElement("message"));

                    {
                        XmlNode nodeId = this.GetNode(nodeMessage, "id", true);
                        nodeId.InnerText = id;
                    }

                    {
                        XmlNode nodeSource = this.GetNode(nodeMessage, "source", true);
                        nodeSource.RemoveAll();
                        XmlNode cdata = this.mDocument.CreateCDataSection(source);
                        nodeSource.AppendChild(cdata);
                    }

                    {
                        XmlNode nodeTranslated = this.GetNode(nodeMessage, "translated", true);
                        nodeTranslated.RemoveAll();
                        XmlNode cdata = this.mDocument.CreateCDataSection(translation);
                        nodeTranslated.AppendChild(cdata);
                    }

                    if (comment != String.Empty)
                    {
                        XmlNode nodeComment = this.GetNode(nodeMessage, "comment", true);
                        nodeComment.RemoveAll();
                        XmlNode cdata = this.mDocument.CreateCDataSection(comment);
                        nodeComment.AppendChild(cdata);
                    }

                    this.mChanged = true;
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
                    this.mDocument.Save(this.mFilename);
                    this.mChanged = false;
                }
            }
        }
    }
}

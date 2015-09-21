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
using System.Xml.Serialization;

namespace BugTracker.Core.Classes
{
    internal class LocalizationManager : ILocalizationManager
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

            this.mActiveCulture = new CultureInfo(Saved<LocalizationOptions>.Instance.CultureName);
            Thread.CurrentThread.CurrentUICulture = this.mActiveCulture;
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
        private CultureInfo mActiveCulture;

        public TranslationUnit GetTranslation(CultureInfo culture, string assemblyName, string methodName, string source, string comment = "")
        {
            TranslationData data = this.GetData(assemblyName, culture);

            string id = this.GetHash(methodName + source);

            TranslationUnit unit = data.GetTranslation(id);

            if (unit == null)
            {
                unit = new TranslationUnit(id, source, source, comment);
                data.SetTranslation(unit);
            }

            return unit;
        }

        public void SetTranslation(CultureInfo culture, string assemblyName, string methodName, string source, string translated, string comment = "")
        {
            TranslationData data = this.GetData(assemblyName, culture);

            string id = this.GetHash(methodName + source);

            TranslationUnit unit = new TranslationUnit(id, source, translated, comment);
            data.SetTranslation(unit);
        }

        public void Save()
        {
            foreach (var value in this.mTranslations.Values)
            {
                value.SaveChanges();
            }
        }

        public CultureInfo ActiveUICulture
        {
            get
            {
                return this.mActiveCulture;
            }
            set
            {
                this.mActiveCulture = value;
                Saved<LocalizationOptions>.Instance.CultureName = value.Name;
                Saved<LocalizationOptions>.Save();
            }
        }

        public IEnumerable<CultureInfo> FoundCultures
        {
            get
            {
                DirectoryInfo directory = new DirectoryInfo(this.LanguagesDir);
                DirectoryInfo[] subdirs = directory.GetDirectories();
                IEnumerable<CultureInfo> cultures = subdirs.Select<DirectoryInfo, CultureInfo>(dir => new CultureInfo(dir.Name));
                return cultures;
            }
        }

        /// <summary>
        /// Get translation data by assembly name and culture
        /// </summary>
        /// <param name="assemblyName">Assembly file name without extension.</param>
        /// <param name="culture">Selected culture</param>
        /// <returns>Translation data</returns>
        private TranslationData GetData(string assemblyName, CultureInfo culture)
        {
            string path = this.LanguagesDir;

            string translationFileTemplate = Path.Combine(path, "{0}", assemblyName + ".xml");
            string translationDirTemplate = Path.GetDirectoryName(translationFileTemplate);

            string filename = String.Format(translationFileTemplate, culture.Name);
            string dirname = String.Format(translationDirTemplate, culture.Name);

            TranslationData data = null;

            if (this.mTranslations.ContainsKey(filename))
            {
                data = this.mTranslations[filename];
            }
            else
            {
                if (!Directory.Exists(dirname))
                {
                    Directory.CreateDirectory(dirname);
                }

                if (File.Exists(filename))
                {
                    try
                    {
                        data = TranslationData.LoadFrom(filename);
                        this.mTranslations.Add(filename, data);
                    }
                    catch(Exception exc)
                    {
                        data = new TranslationData(filename, culture);
                        this.mTranslations.Add(filename, data);
                    }
                }
                else
                {
                    data = new TranslationData(filename, culture);
                    this.mTranslations.Add(filename, data);
                }
            }

            return data;
        }

        private string GetHash(string value)
        {
            return value.GetHashCode().ToString();
        }
    }
}

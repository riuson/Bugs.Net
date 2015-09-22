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
            TranslationData data = this.GetData(culture, assemblyName);

            string id = this.GetHash(methodName + source);

            TranslationUnit unit = data.GetTranslation(id);

            if (unit == null)
            {
                unit = new TranslationUnit(id, methodName, source, source, comment);
                data.AddTranslation(unit);
            }

            return unit;
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

        public IEnumerable<string> GetModules(CultureInfo culture)
        {
            DirectoryInfo directory = new DirectoryInfo(Path.Combine(this.LanguagesDir, culture.Name));
            FileInfo[] files = directory.GetFiles("*.xml", SearchOption.TopDirectoryOnly);
            IEnumerable<string> modules = files.Select<FileInfo, string>(file => Path.GetFileNameWithoutExtension(file.Name));
            return modules;
        }

        public IEnumerable<TranslationUnit> GetTranslationUnits(CultureInfo culture, string assemblyName)
        {
            TranslationData data = this.GetData(culture, assemblyName);
            return data.Units;
        }

        public void AddCulture(CultureInfo cultureNew, CultureInfo cultureSource = null)
        {
            DirectoryInfo directoryNew = new DirectoryInfo(Path.Combine(this.LanguagesDir, cultureNew.Name));
            directoryNew.Create();

            if (cultureSource != null)
            {
                DirectoryInfo directorySource = new DirectoryInfo(Path.Combine(this.LanguagesDir, cultureSource.Name));
                FileInfo[] files = directorySource.GetFiles("*.xml", SearchOption.TopDirectoryOnly);

                foreach (var file in files)
                {
                    file.CopyTo(Path.Combine(directoryNew.FullName, Path.GetFileName(file.FullName)));
                }
            }
        }

        public void RemoveCulture(CultureInfo culture)
        {
            if (this.FoundCultures.Contains(culture))
            {
                DirectoryInfo directory = new DirectoryInfo(Path.Combine(this.LanguagesDir, culture.Name));

                if (directory.Exists)
                {
                    directory.Delete(true);
                }
            }
        }

        /// <summary>
        /// Get translation data by assembly name and culture
        /// </summary>
        /// <param name="assemblyName">Assembly file name without extension.</param>
        /// <param name="culture">Selected culture</param>
        /// <returns>Translation data</returns>
        private TranslationData GetData(CultureInfo culture, string assemblyName)
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
                    catch (Exception exc)
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

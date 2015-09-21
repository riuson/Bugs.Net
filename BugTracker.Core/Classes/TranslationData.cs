using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;

namespace BugTracker.Core.Classes
{
    [DataContract(Name = "Translation")]
    internal class TranslationData
    {
        private string mFilename { get; set; }

        private CultureInfo mCulture;

        [OnDeserialized]
        private void OnSerializingMethod(StreamingContext context)
        {
            if (this.mUnits == null)
            {
                this.mUnits = new List<TranslationUnit>();
            }
        }

        [DataMember(Name = "Culture")]
        private string XCulture
        {
            get
            {
                return this.mCulture.Name;
            }
            set
            {
                this.mCulture = new CultureInfo(value);
            }
        }

        [DataMember(Name="Messages")]
        private List<TranslationUnit> mUnits;

        private bool mChanged;

        public TranslationData()
        {
            this.mFilename = String.Empty;
            this.mCulture = null;
            this.mUnits = new List<TranslationUnit>();
            this.mChanged = false;
        }

        public TranslationData(string filename, CultureInfo culture) :
            this()
        {
            this.mFilename = filename;
            this.mCulture = culture;
        }

        public TranslationUnit GetTranslation(string id)
        {
            if (this.mUnits.Any(u => u.Id == id))
            {
                TranslationUnit result = this.mUnits.First(unit => unit.Id == id);
                return result;
            }

            return null;
        }

        public void SetTranslation(TranslationUnit unit)
        {
            if (this.mUnits.Any(u => u.Id == unit.Id))
            {
                TranslationUnit existing = this.mUnits.First(u => u.Id == unit.Id);
                this.mUnits.Remove(existing);
                this.mUnits.Add(unit);
                this.mChanged = true;
            }
            else
            {
                this.mUnits.Add(unit);
                this.mChanged = true;
            }
        }

        public void SaveChanges()
        {
            if (this.mChanged)
            {
                using (Stream stream = File.Open(this.mFilename, FileMode.Create))
                {
                    //XmlSerializer formatter = new XmlSerializer(typeof(TranslationData));
                    //formatter.Serialize(stream, this);

                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.IndentChars = " ";
                    settings.Encoding = Encoding.UTF8;
                    settings.NewLineChars = Environment.NewLine;

                    using (XmlWriter writer = XmlDictionaryWriter.Create(stream, settings))
                    {
                        var serialzer = new DataContractSerializer(typeof(TranslationData));
                        serialzer.WriteObject(writer, this);
                    }

                    stream.Close();
                    this.mChanged = false;
                }
            }
        }

        public static TranslationData LoadFrom(string filename)
        {
            using (Stream stream = File.Open(filename, FileMode.Open))
            {
                XmlReaderSettings settings = new XmlReaderSettings();

                using (XmlReader reader = XmlDictionaryReader.Create(stream, settings))
                {
                    var serialzer = new DataContractSerializer(typeof(TranslationData));
                    object o = serialzer.ReadObject(reader);

                    return o as TranslationData;
                }
            }
        }
    }

    [DataContract(Name="Message")]
    internal class TranslationUnit
    {
        [DataMember()]
        public string Id { get; set; }
        [DataMember()]
        public string Source { get; set; }
        [DataMember()]
        public string Translated { get; set; }
        [DataMember()]
        public string Comment { get; set; }

        public TranslationUnit(string id, string source, string translated, string comment)
        {
            this.Id = id;
            this.Source = source;
            this.Translated = translated;
            this.Comment = comment;
        }
    }
}

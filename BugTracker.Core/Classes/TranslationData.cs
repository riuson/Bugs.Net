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

        [DataMember(Name = "Messages")]
        private List<TranslationUnit> mUnits;

        public IEnumerable<TranslationUnit> Units
        {
            get
            {
                return this.mUnits;
            }
        }

        public TranslationData()
        {
            this.mFilename = String.Empty;
            this.mCulture = null;
            this.mUnits = new List<TranslationUnit>();
        }

        public TranslationData(string filename, CultureInfo culture) :
            this()
        {
            this.mFilename = filename;
            this.mCulture = culture;
        }

        public TranslationUnit GetTranslation(string id)
        {
            var result = (from unit in this.mUnits
                          where unit.Id == id
                          select unit).SingleOrDefault();

            return result;
        }

        public void AddTranslation(TranslationUnit unit)
        {
            var count = (from u in this.mUnits
                         where u.Id == unit.Id
                         select u).Count();

            if (count == 0)
            {
                this.mUnits.Add(unit);
            }
        }

        public void SaveChanges()
        {
            var count = (from u in this.mUnits
                         where u.Changed == true
                         select u).Count();

            if (count > 0)
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

                    foreach (var unit in this.mUnits)
                    {
                        unit.Changed = false;
                    }
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
                    TranslationData result = o as TranslationData;
                    result.mFilename = filename;

                    foreach (var unit in result.Units)
                    {
                        unit.Changed = false;
                    }

                    return result;
                }
            }
        }
    }

    [DataContract(Name = "Message")]
    public class TranslationUnit
    {
        private bool mChanged;
        private string mTranslated;

        [DataMember()]
        public string Id { get; set; }
        [DataMember()]
        public string Method { get; set; }
        [DataMember()]
        public string Source { get; set; }
        [DataMember()]
        public string Translated
        {
            get
            {
                return this.mTranslated;
            }
            set
            {
                if (this.mTranslated != value)
                {
                    this.mTranslated = value;
                    this.mChanged = true;
                }
            }
        }
        [DataMember()]
        public string Comment { get; set; }

        public bool Changed
        {
            get
            {
                return this.mChanged;
            }
            set
            {
                this.mChanged = value;
            }
        }

        public TranslationUnit(string id, string method, string source, string translated, string comment)
        {
            this.Id = id;
            this.Method = method;
            this.Source = source;
            this.Translated = translated;
            this.Comment = comment;
            this.mChanged = false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace BugTracker.Core.Localization
{
    internal class TranslationData
    {
        private string mFilename { get; set; }

        private CultureInfo mCulture;

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

            FileInfo file = new FileInfo(filename);

            if (file.Exists)
            {
                try
                {
                    using (FileStream fs = file.OpenRead())
                    {
                        using (XmlReader reader = XmlReader.Create(fs))
                        {
                            var xDocument = XDocument.Load(reader);
                            var xRoot = xDocument.Root;// Element("Translation");
                            var xCulture = xRoot.Element("Culture");
                            var xMessages = xRoot.Element("Messages");
                            var xMessagesList = xMessages.Elements("Message");
                            var a = xMessagesList.ToArray();

                            foreach (var xMessage in xMessagesList)
                            {
                                string id = (string)xMessage.Element("Id");
                                string method = (string)xMessage.Element("Method");
                                string lineNumber = (string)xMessage.Element("LineNumber");
                                string sourceString = (string)xMessage.Element("SourceString");
                                string translatedString = (string)xMessage.Element("TranslatedString");
                                string comment = (string)xMessage.Element("Comment");

                                if (!String.IsNullOrEmpty(id))
                                {
                                    TranslationUnit unit = new TranslationUnit(
                                        id,
                                        method,
                                        String.IsNullOrEmpty(lineNumber) ? 0 : Convert.ToInt32(lineNumber),
                                        sourceString,
                                        translatedString,
                                        comment);
                                    unit.Changed = false;

                                    this.AddTranslation(unit);
                                }
                            }

                            reader.Close();
                        }

                        fs.Close();
                    }
                }
                catch (Exception exc)
                {
                }
            }
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
                using (FileStream fs = File.Open(this.mFilename, FileMode.Create))
                {
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.IndentChars = " ";
                    settings.Encoding = Encoding.UTF8;
                    settings.NewLineChars = Environment.NewLine;

                    using (XmlWriter writer = XmlDictionaryWriter.Create(fs, settings))
                    {
                        XDocument document = new XDocument(
                            new XDeclaration("1.0", "UTF-8", null),
                            new XElement("Translation",
                                new XElement("Culture")
                                {
                                    Value = this.mCulture.Name
                                },
                                new XElement("Messages",
                                    from item in this.mUnits
                                    select new XElement("Message",
                                        new XElement("Id")
                                        {
                                            Value = item.Id
                                        },
                                        new XElement("Method")
                                        {
                                            Value = item.Method
                                        },
                                        new XElement("LineNumber")
                                        {
                                            Value = Convert.ToString(item.SourceLineNumber)
                                        },
                                        new XElement("SourceString")
                                        {
                                            Value = item.SourceString
                                        },
                                        new XElement("TranslatedString")
                                        {
                                            Value = item.TranslatedString
                                        },
                                        new XElement("Comment")
                                        {
                                            Value = item.Comment
                                        }
                                    ))));

                        document.Save(writer);
                        writer.Close();
                    }

                    fs.Close();

                    foreach (var unit in this.mUnits)
                    {
                        unit.Changed = false;
                    }
                }
            }
        }
    }

    public class TranslationUnit
    {
        private bool mChanged;
        private string mTranslated;

        public string Id { get; set; }
        public string Method { get; set; }
        public int SourceLineNumber { get; set; }
        public string SourceString { get; set; }
        public string TranslatedString
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Id of the translation unit. Computed as Hash of SourceFilename + MemberName</param>
        /// <param name="method">Method name (Member name)</param>
        /// <param name="sourceLineNumber">Line number in source file</param>
        /// <param name="sourceString">Original string</param>
        /// <param name="translatedString">Translated string</param>
        /// <param name="comment">Optional comment for translator</param>
        public TranslationUnit(
            string id,
            string method,
            int sourceLineNumber,
            string sourceString,
            string translatedString,
            string comment)
        {
            this.Id = id;
            this.Method = method;
            this.SourceLineNumber = sourceLineNumber;
            this.SourceString = sourceString;
            this.TranslatedString = translatedString;
            this.Comment = comment;
            this.mChanged = false;
        }
    }
}

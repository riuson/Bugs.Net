using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace BugTracker.Core.Classes
{
    /// <summary>
    /// Singleton class to load/save settings in xml format.
    /// </summary>
    /// <typeparam name="T">Type of settings class, must be public accessible.</typeparam>
    public static class Saved<T>
    {
        private static T mInstance;
        private static object mLocker = new object();

        private static string SettinsDirectory
        {
            get
            {
                // With VS Host Process enabled will return other directory
                string appdata = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string directory = Path.Combine(appdata, Application.CompanyName);
                directory = Path.Combine(directory, Application.ProductName);
                return directory;
            }
        }

        private static string Filename
        {
            get
            {
                string typename = typeof(T).FullName;
                string filename = Path.Combine(SettinsDirectory, typename + ".xml");
                return filename;
            }
        }

        /// <summary>
        /// Singleton instance of settings class
        /// </summary>
        public static T Instance
        {
            get
            {
                lock (mLocker)
                {
                    if (mInstance == null)
                    {
                        mInstance = Load();
                    }

                    return mInstance;
                }
            }
        }

        private static T Load()
        {
            T obj = default(T);

            try
            {
                if (File.Exists(Filename))
                {
                    using (FileStream fs = new FileStream(Filename, FileMode.Open))
                    {
                        using (XmlReader xr = new XmlTextReader(fs))
                        {
                            XmlSerializer ser = new XmlSerializer(typeof(T));

                            if (ser.CanDeserialize(xr))
                            {
                                obj = (T)ser.Deserialize(xr);
                            }
                        }
                    }
                }
            }
            catch (InvalidOperationException)
            {
            }
            catch (XmlException)
            {
            }

            if (obj == null)
                obj = (T)Activator.CreateInstance(typeof(T));
            return obj;
        }

        /// <summary>
        /// Save settings to file
        /// </summary>
        public static void Save()
        {
            if (mInstance != null)
            {
                if (!Directory.Exists(SettinsDirectory))
                {
                    Directory.CreateDirectory(SettinsDirectory);
                }

                try
                {
                    XmlSerializer ser = new XmlSerializer(typeof(T));

                    // Temp buffer
                    using (MemoryStream ms = new MemoryStream())
                    {
                        // Try serialize
                        using (TextWriter wr = new StreamWriter(ms))
                        {
                            ser.Serialize(wr, mInstance);
                            wr.Flush();

                            // If serialized successfully, try write to file
                            using (FileStream fs = new FileStream(Filename, FileMode.Create))
                            {
                                ms.WriteTo(fs);
                                fs.Flush();
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    System.Diagnostics.Debug.WriteLine(exc.Message);
                }
            }
        }

        /// <summary>
        /// Reload settings from file
        /// </summary>
        public static void Reload()
        {
            lock (mLocker)
            {
                mInstance = Load();
            }
        }
    }
}

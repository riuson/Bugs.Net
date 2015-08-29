using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Classes
{
    public class Domain<T> : IDisposable where T : MarshalByRefObject
    {
        private AppDomain mDomain;

        public Domain()
        {
            Type type = typeof(T);

            this.mDomain = AppDomain.CreateDomain(
                String.Format("Object {0} in domain {1}", type, Guid.NewGuid()),
                null,
                this.CreateSetup());

            this.Object = (T)this.mDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
        }

        public void Dispose()
        {
            if (this.mDomain != null)
            {
                AppDomain.Unload(this.mDomain);
                this.mDomain = null;
            }
        }

        public T Object { get; private set; }

        private AppDomainSetup CreateSetup()
        {
            AppDomainSetup setup = new AppDomainSetup();

            setup.ApplicationName = Application.ProductName;
            setup.ApplicationBase = this.GetExeDirectory();
            setup.PrivateBinPath = this.GetPrivateBinDirectories();

            return setup;
        }

        private string GetExeDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            path = Path.GetDirectoryName(path);
            return path;
        }

        private string GetPrivateBinDirectories()
        {
            // List of files
            IEnumerable<string> dirs = new List<string>(Directory.GetFiles(this.GetExeDirectory(), "*.*", SearchOption.AllDirectories));

            // Get directory names
            dirs = dirs.Select<string, string>(delegate(string filename)
            {
                return Path.GetDirectoryName(filename);
            });

            // Remove duplicates
            dirs = dirs.Distinct<string>();

            // Remove x86 and x64 directories
            dirs = dirs.Where<string>(delegate(string dir)
            {
                if (dir.ToLower().EndsWith("x86") | dir.ToLower().EndsWith("x64"))
                    return false;

                return true;
            });

            // Join to string
            string result = string.Join(";", dirs);

            return result;
        }
    }
}

using BugTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Classes
{
    internal class Plugins : IPlugins
    {
        private List<IPlugin> mPlugins;
        private IApplication mApp;

        public Plugins(IApplication app)
        {
            this.mApp = app;
            this.mPlugins = this.ScanPlugins(this.mApp);
        }

        public Button[] CollectCommandLinks(string tag)
        {
            List<Button> list = new List<Button>();

            foreach (var plugin in this.mPlugins)
            {
                list.AddRange(plugin.GetCommandLinks(tag));
            }

            return list.ToArray();
        }

        private List<IPlugin> ScanPlugins(IApplication app)
        {
            List<IPlugin> result = new List<IPlugin>();

            string[] files = this.GetLibraries();

            foreach (var filename in files)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFile(filename);
                    Type[] types = assembly.GetTypes();

                    foreach (var type in types)
                    {
                        Type t = type.GetInterface(typeof(IPlugin).FullName);

                        if (t != null && !type.IsAbstract)
                        {
                            IPlugin pluginInfo = (IPlugin)Activator.CreateInstance(type);
                            pluginInfo.Initialize(app);
                            result.Add(pluginInfo);
                        }
                    }
                }
                catch (Exception exc)
                {
                    System.Diagnostics.Debug.WriteLine(exc.Message);
                }
            }

            return result;
        }

        private string GetExeDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            path = Path.GetDirectoryName(path);
            return path;
        }

        private string[] GetLibraries()
        {
            // List of dll files
            IEnumerable<string> files = new List<string>(Directory.GetFiles(this.GetExeDirectory(), "*.dll", SearchOption.AllDirectories));

            // Remove x86 and x64 directories
            files = files.Where<string>(delegate(string filename)
            {
                string dir = Path.GetDirectoryName(filename);

                if (dir.ToLower().EndsWith("x86") | dir.ToLower().EndsWith("x64"))
                    return false;

                return true;
            });

            return files.ToArray<string>();
        }
    }
}

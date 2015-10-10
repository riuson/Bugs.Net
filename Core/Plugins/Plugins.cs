using BugTracker.Core.Menus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Plugins
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

        public IButton[] CollectCommandLinks(string tag)
        {
            List<IButton> list = new List<IButton>();

            foreach (var plugin in this.mPlugins)
            {
                list.AddRange(plugin.GetCommandLinks(tag));
            }

            return list.ToArray();
        }

        private List<IPlugin> ScanPlugins(IApplication app)
        {
            List<IPlugin> result = new List<IPlugin>();

            var files = this.GetLibraries();

            foreach (var filename in files)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFile(filename);
                    object[] attributesPlugin = assembly.GetCustomAttributes(typeof(AssemblyPluginTypeAttribute), false);

                    foreach (var attributePlugin in attributesPlugin)
                    {
                        Type pluginType = (attributePlugin as AssemblyPluginTypeAttribute).PluginType;
                        Type t = pluginType.GetInterface(typeof(IPlugin).FullName);

                        if (t != null && !pluginType.IsAbstract)
                        {
                            IPlugin pluginInfo = (IPlugin)Activator.CreateInstance(pluginType);
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

        private IEnumerable<String> GetLibraries()
        {
            // List of files
            var files = Directory.GetFiles(this.GetExeDirectory(), "*.dll", SearchOption.AllDirectories);

            // Remove x86/x64
            var result = from file in files
                         let dir = Path.GetDirectoryName(file)
                         where !dir.EndsWith("x86") && !dir.EndsWith("x64")
                         select file;

            return result;
        }
    }
}

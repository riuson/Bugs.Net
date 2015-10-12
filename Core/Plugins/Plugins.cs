using AppCore.Menus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppCore.Plugins
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

            try
            {
                var plugins = from filename in files.AsParallel()
                              let assembly = Assembly.LoadFile(filename)
                              from attribute in assembly.GetCustomAttributes(typeof(AssemblyPluginTypeAttribute), false).AsParallel()
                              let pluginType = (attribute as AssemblyPluginTypeAttribute).PluginType
                              let interfaceType = pluginType.GetInterface(typeof(IPlugin).FullName)
                              where interfaceType != null && !pluginType.IsAbstract
                              let plugin = (IPlugin)Activator.CreateInstance(pluginType)
                              select plugin;

                result = plugins.ToList();

                Parallel.ForEach(result, (plugin) =>
                    {
                        plugin.Initialize(app);
                    });
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
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

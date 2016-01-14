﻿using AppCore.Extensions;
using AppCore.Menus;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppCore.Plugins
{
    internal class Plugins : IPlugins, IDisposable
    {
        private CompositionContainer mContainer;
        [ImportMany]
        private IEnumerable<Lazy<IPlugin>> mLazyPlugins;
        private IEnumerable<IPlugin> mPlugins;

        private IApplication mApp;

        public Plugins(IApplication app)
        {
            this.mApp = app;
            this.mPlugins = this.ScanPluginsMEF(this.mApp);
        }

        public void Dispose()
        {
            this.ShutdownAll(this.mPlugins);
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

        public void Start()
        {
            this.StartAll(this.mPlugins);
        }

        private List<IPlugin> ScanPlugins(IApplication app)
        {
            List<IPlugin> result = new List<IPlugin>();

            try
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();

                var files = this.GetLibraries();

                var plugins = from filename in files.AsParallel()
                              let assembly = Assembly.LoadFile(filename)
                              from attribute in assembly.GetCustomAttributes(typeof(AssemblyPluginTypeAttribute), false).AsParallel()
                              let pluginType = (attribute as AssemblyPluginTypeAttribute).PluginType
                              let interfaceType = pluginType.GetInterface(typeof(IPlugin).FullName)
                              where interfaceType != null && !pluginType.IsAbstract
                              let plugin = (IPlugin)Activator.CreateInstance(pluginType)
                              select plugin;

                result = plugins.ToList();

                this.InitializeAll(app, result);

                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
            }

            return result;
        }

        private List<IPlugin> ScanPluginsMEF(IApplication app)
        {
            List<IPlugin> result = new List<IPlugin>();
            try
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();

                var catalog = new AggregateCatalog();

                foreach (var dir in this.GetDirectories())
                {
                    catalog.Catalogs.Add(new DirectoryCatalog(dir));
                }

                this.mContainer = new CompositionContainer(catalog);
                this.mContainer.ComposeParts(this);
                result = this.mLazyPlugins.Select(x => x.Value).ToList();
                this.InitializeAll(app, result);

                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
            }
            catch (CompositionException exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
            }

            return result;
        }

        private void InitializeAll(IApplication app, IEnumerable<IPlugin> plugins)
        {
            Parallel.ForEach(plugins, (plugin) =>
            {
                plugin.Initialize(app);
            });
        }

        private void StartAll(IEnumerable<IPlugin> plugins)
        {
            foreach (var plugin in plugins)
            {
                plugin.Start();
            }
        }

        private void ShutdownAll(IEnumerable<IPlugin> plugins)
        {
            foreach (var plugin in plugins)
            {
                plugin.Shutdown();
            }
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
            var files = from file in Directory.EnumerateFiles(this.GetExeDirectory(), "*.*", SearchOption.AllDirectories)
                        let dir = Path.GetDirectoryName(file)
                        where !dir.EndsWith("x86") && !dir.EndsWith("x64") // Remove x86/x64
                        let extension = Path.GetExtension(file).ToLower()
                        where extension == ".dll" || extension == ".exe"
                        select file;

            return files;
        }

        private IEnumerable<String> GetDirectories()
        {
            // List of files
            var files = from file in Directory.EnumerateFiles(this.GetExeDirectory(), "*.*", SearchOption.AllDirectories)
                        let dir = Path.GetDirectoryName(file)
                        where !dir.EndsWith("x86") && !dir.EndsWith("x64") // Remove x86/x64
                        select dir;
            return files.Distinct();
        }
    }
}

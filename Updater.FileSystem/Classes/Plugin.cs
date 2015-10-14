using AppCore;
using AppCore.Classes;
using AppCore.Extensions;
using AppCore.Menus;
using AppCore.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Updater.FileSystem.Classes
{
    internal class Plugin : IPlugin
    {
        private IApplication mApp;

        public void Initialize(IApplication app)
        {
            this.mApp = app;
        }

        public IButton[] GetCommandLinks(string tag)
        {
            switch (tag)
            {
                case "updater":
                    {
                        IButton menuItemUpdate = MenuPanelFabric.CreateMenuItem("File system".Tr(), "Setup update from file system".Tr(),
                            Updater.FileSystem.Properties.Resources.icon_fa_files_o_48_0_005719_none);
                        menuItemUpdate.Click += delegate(object sender, EventArgs ea)
                        {
                            //ControlAbout controlAbout = new ControlAbout(this.mApp);
                            //this.mApp.Controls.Show(controlAbout);
                        };

                        return new IButton[] { menuItemUpdate };
                    }
                default:
                    return new IButton[] { };
            }
        }

        public void Start()
        {
        }

        public void Shutdown()
        {
        }
    }
}

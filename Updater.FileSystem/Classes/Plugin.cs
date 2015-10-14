using AppCore;
using AppCore.Classes;
using AppCore.Extensions;
using AppCore.Menus;
using AppCore.Messages;
using AppCore.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Updater.Events;
using Updater.FileSystem.Setup;

namespace Updater.FileSystem.Classes
{
    internal class Plugin : IPlugin
    {
        private IApplication mApp;

        public void Initialize(IApplication app)
        {
            this.mApp = app;
            this.mApp.Messages.Subscribe(typeof(UpdateStartEventArgs), this.UpdateStart);
        }

        public IButton[] GetCommandLinks(string tag)
        {
            switch (tag)
            {
                case "update_sources":
                    {
                        IButton menuItemUpdate = MenuPanelFabric.CreateMenuItem("File system".Tr(), "Setup update from file system".Tr(),
                            Updater.FileSystem.Properties.Resources.icon_fa_files_o_48_0_005719_none);
                        menuItemUpdate.Click += delegate(object sender, EventArgs ea)
                        {
                            ControlSetup controlSetup = new ControlSetup(this.mApp);
                            this.mApp.Controls.Show(controlSetup);
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

        private void UpdateStart(object sender, MessageEventArgs ea)
        {
        }
    }
}

using AppCore;
using AppCore.Classes;
using AppCore.Extensions;
using AppCore.Menus;
using AppCore.Plugins;
using BugTracker.DB.DataAccess;
using BugTracker.DB.Errors;
using BugTracker.DB.Events;
using BugTracker.DB.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugTracker.DB.Classes
{
    internal class Plugin : IPlugin
    {
        private IApplication mApp;

        public void Initialize(IApplication app)
        {
            this.mApp = app;
            //SessionManager.Instance.Configure(new SessionOptions(Saved<Options>.Instance.FileName));
            this.mApp.Messages.Subscribe(typeof(ConfigurationRequiredEventArgs), this.ConfigurationRequired);
        }

        public IButton[] GetCommandLinks(string tag)
        {
            switch (tag)
            {
                case "settings":
                    {
                        IButton menuItemDBSettings = MenuPanelFabric.CreateMenuItem(
                            "Database".Tr(),
                            "Configure database".Tr(),
                            BugTracker.DB.Properties.Resources.icon_database_1d257b_48);
                        menuItemDBSettings.Click += delegate(object sender, EventArgs ea)
                        {
                            ControlSettings controlSettings = new ControlSettings(this.mApp);
                            this.mApp.Controls.Show(controlSettings);
                        };

                        IButton menuItemBackupSettings = MenuPanelFabric.CreateMenuItem(
                            "Backup".Tr(),
                            "Configure database backup".Tr(),
                            BugTracker.DB.Properties.Resources.icon_archive_48_0_1d257b_none);
                        menuItemBackupSettings.Click += delegate(object sender, EventArgs ea)
                        {
                            ControlBackup controlBackup = new ControlBackup(this.mApp);
                            this.mApp.Controls.Show(controlBackup);
                        };

                        return new IButton[] { menuItemDBSettings, menuItemBackupSettings };
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

        private void ConfigurationRequired(object sender, AppCore.Messages.MessageEventArgs e)
        {
            ControlError controlError = new ControlError(this.mApp);
            this.mApp.Controls.Show(controlError);
        }
    }
}

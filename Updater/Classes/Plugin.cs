using AppCore;
using AppCore.Classes;
using AppCore.Extensions;
using AppCore.Menus;
using AppCore.Messages;
using AppCore.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Updater.Events;
using Updater.Setup;

namespace Updater.Classes
{
    internal class Plugin : IPlugin
    {
        private IApplication mApp;

        public void Initialize(IApplication app)
        {
            this.mApp = app;
            this.mApp.Messages.Subscribe(typeof(UpdateReceivedEventArgs), this.UpdateReceived);
        }

        public IButton[] GetCommandLinks(string tag)
        {
            switch (tag)
            {
                case "settings":
                    {
                        IButton menuItemUpdate = MenuPanelFabric.CreateMenuItem("Updates".Tr(), "Updater settings".Tr(), Updater.Properties.Resources.icon_fa_refresh_48_0_005719_none);
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
            if (Saved<Options>.Instance.CheckUpdatesOnStart)
            {
                this.mApp.Messages.Send(this, new UpdateStartEventArgs());
            }
        }

        public void Shutdown()
        {
        }

        private void UpdateReceived(object sender, MessageEventArgs e)
        {
            UpdateReceivedEventArgs ea = e as UpdateReceivedEventArgs;

            if (ea != null)
            {
                object[] attributesAuthorDate = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyGitCommitAuthorDateAttribute), false);
                object[] attributesRevision = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyGitRevisionAttribute), false);
                DateTime currentCommitDate = (attributesAuthorDate[0] as AssemblyGitCommitAuthorDateAttribute).CommitAuthorDate;
                string currentRevision = (attributesRevision[0] as AssemblyGitRevisionAttribute).RevisionHash;

                string message = String.Format("Current version: {1} from {2:yyyy-MM-dd HH:mm:ss}{0}New version: {3} from {4:yyyy-MM-dd HH:mm:ss}{0}Apply update?".Tr(),
                    Environment.NewLine,
                    currentRevision.Substring(0, 7),
                    currentCommitDate,
                    ea.Result.History.LatestRevision.Substring(0, 7),
                    ea.Result.History.LatestCommitDate);

                if (MessageBox.Show(this.mApp.OwnerWindow, message, "Update received".Tr(), MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    DirectoryInfo applicationDirectory = new DirectoryInfo(this.mApp.StartInfo.ExecutableDir);
                    FileInfo applicationFile = new FileInfo(this.mApp.StartInfo.ExecutablePath);
                    Guid instanceId = this.mApp.StartInfo.InstanceId;

                    Updater.CommandLine.Preparer.Run(
                        applicationDirectory,
                        ea.Result.TempFile,
                        applicationFile,
                        instanceId);

                    this.mApp.Exit();
                }
            }
        }
    }
}

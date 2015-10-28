using AppCore;
using AppCore.Classes;
using AppCore.Extensions;
using AppCore.Menus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Updater.Classes;

namespace Updater.Setup
{
    public partial class ControlSetup : UserControl
    {
        private IApplication mApp;
        private IMenuPanel mMenu;

        public ControlSetup(IApplication app)
        {
            InitializeComponent();
            this.mApp = app;

            this.Text = "Updater settings".Tr();
            this.checkBoxCheckUpdatesOnStart.Text = this.checkBoxCheckUpdatesOnStart.Text.Tr();

            this.mMenu = MenuPanelFabric.CreateMenuPanel();
            this.panelMenu.Controls.Add(this.mMenu.AsControl);
            this.mMenu.AsControl.Dock = System.Windows.Forms.DockStyle.Fill;

            this.mMenu.Add(this.mApp, "update_settings");

            this.LoadSettings();
        }

        private void BeforeDisposing()
        {
            this.SaveSettings();
        }

        private void LoadSettings()
        {
            this.checkBoxCheckUpdatesOnStart.Checked = Saved<Options>.Instance.CheckUpdatesOnStart;
        }

        private void SaveSettings()
        {
            Saved<Options>.Instance.CheckUpdatesOnStart = this.checkBoxCheckUpdatesOnStart.Checked;
            Saved<Options>.Save();
        }
    }
}

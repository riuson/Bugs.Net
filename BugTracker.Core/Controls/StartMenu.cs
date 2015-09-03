using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Controls
{
    internal class StartMenu : UserControl
    {
        private IApplication mApp;
        private IMenuPanel mMenu;

        public StartMenu(IApplication app)
        {
            this.Text = "Start";
            this.mApp = app;
            this.mMenu = MenuPanelFabric.CreateMenuPanel();
            this.Controls.Add(this.mMenu.AsControl);
            this.mMenu.AsControl.Dock = DockStyle.Fill;

            this.mMenu.Add(this.mApp, "startpage");
        }
    }
}

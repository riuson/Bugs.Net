﻿using BugTracker.Core;
using BugTracker.Core.Extensions;
using BugTracker.Core.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.Settings.Controls
{
    internal class ControlSettings : System.Windows.Forms.UserControl
    {
        private IApplication mApp;
        private IMenuPanel mMenu;

        public ControlSettings(IApplication app)
        {
            this.Text = "Settings".Tr();

            this.mApp = app;
            this.mMenu = MenuPanelFabric.CreateMenuPanel();
            this.Controls.Add(this.mMenu.AsControl);
            this.mMenu.AsControl.Dock = System.Windows.Forms.DockStyle.Fill;

            this.mMenu.Add(this.mApp, "settings");
        }
    }
}

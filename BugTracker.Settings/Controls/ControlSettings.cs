using BugTracker.Core.Controls;
using BugTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.Settings.Controls
{
    internal class ControlSettings : MenuPanel
    {
        private IApplication mApp;

        public ControlSettings(IApplication app)
        {
            this.Text = "Settings";

            this.mApp = app;

            this.Add(this.mApp, "settings");
        }
    }
}

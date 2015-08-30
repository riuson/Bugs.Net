using BugTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Controls
{
    internal class StartMenu : MenuPanel
    {
        private IApplication mApp;

        public StartMenu(IApplication app)
        {
            this.Text = "Start";
            this.mApp = app;

            this.Add(this.mApp, "startpage");
        }
    }
}

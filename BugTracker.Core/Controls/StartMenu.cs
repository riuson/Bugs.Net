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
            this.mApp = app;

            Button[] btns = this.mApp.Plugins.CollectCommandLinks(this.mApp, "startpage");

            foreach (var btn in btns)
            {
                this.Add(btn);
            }

        }
    }
}

using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Controls
{
    internal class MenuPanel : FlowLayoutPanel, IMenuPanel
    {
        public MenuPanel()
        {
        }

        public void Add(IButton button)
        {
            this.Controls.Add(button as Control);
        }

        public void Add(IApplication app, string tag)
        {
            IButton[] btns = app.Plugins.CollectCommandLinks(tag);

            foreach (var btn in btns)
            {
                this.Add(btn);
            }
        }

        public Control AsControl
        {
            get { return this; }
        }
    }
}

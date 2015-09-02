using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Controls
{
    public class MenuPanel : FlowLayoutPanel
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
    }
}

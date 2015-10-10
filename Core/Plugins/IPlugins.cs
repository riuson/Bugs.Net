using BugTracker.Core.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Plugins
{
    public interface IPlugins
    {
        IButton[] CollectCommandLinks(string tag);
    }
}

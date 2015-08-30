using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Interfaces
{
    public interface IPluginInfo
    {
        Button[] GetCommandLinks(IApplication app, string tag);
    }
}

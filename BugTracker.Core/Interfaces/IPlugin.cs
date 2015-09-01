using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Interfaces
{
    public interface IPlugin
    {
        void Initialize(IApplication app);
        Button[] GetCommandLinks(string tag);
    }
}

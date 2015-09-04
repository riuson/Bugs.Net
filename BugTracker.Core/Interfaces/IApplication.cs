using BugTracker.Core.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Interfaces
{
    public interface IApplication
    {
        IPlugins Plugins { get; }
        IControlManager Controls { get; }
        IMessageCenter Messages { get; }
        IWin32Window OwnerWindow { get; }

        void Exit();
    }
}

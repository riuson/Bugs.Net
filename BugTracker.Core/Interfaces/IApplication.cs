using BugTracker.Core.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.Core.Interfaces
{
    public interface IApplication
    {
        IPlugins Plugins { get; }
        IControlManager Controls { get; }

        void Exit();
    }
}

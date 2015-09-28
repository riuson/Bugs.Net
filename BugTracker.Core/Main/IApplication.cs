using BugTracker.Core.Classes;
using BugTracker.Core.Controls;
using BugTracker.Core.Localization;
using BugTracker.Core.Messages;
using BugTracker.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core
{
    public interface IApplication
    {
        IPlugins Plugins { get; }
        IControlManager Controls { get; }
        IMessageCenter Messages { get; }
        IWin32Window OwnerWindow { get; }
        ILocalizationManager Localization { get; }

        void Exit();
    }
}

using AppCore.Classes;
using AppCore.Controls;
using AppCore.Localization;
using AppCore.Messages;
using AppCore.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AppCore
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

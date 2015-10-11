using AppCore.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AppCore.Plugins
{
    public interface IPlugin
    {
        void Initialize(IApplication app);
        IButton[] GetCommandLinks(string tag);
    }
}

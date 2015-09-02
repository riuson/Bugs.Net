using BugTracker.Core.Controls;
using BugTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace BugTracker.Core.Classes
{
    public static class MenuPanelFabric
    {
        public static IMenuPanel CreateMenuPanel()
        {
            return new MenuPanel();
        }

        public static IButton CreateMenuItem(string text, string note = "", Image image = null)
        {
            return MenuPanel.CreateMenuItem(text, note, image);
        }
    }
}

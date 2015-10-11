using AppCore.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AppCore.Menus
{
    public static class MenuPanelFabric
    {
        public static IMenuPanel CreateMenuPanel()
        {
            return new MenuCommandLink();
            //return new MenuListView();
        }

        public static IButton CreateMenuItem(string text, string note = "", Image image = null)
        {
            return MenuCommandLink.CreateMenuItem(text, note, image);
            //return MenuListView.CreateMenuItem(text, note, image);
        }
    }
}

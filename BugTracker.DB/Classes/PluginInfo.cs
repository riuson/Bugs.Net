﻿using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugTracker.DB.Classes
{
    internal class PluginInfo : IPluginInfo
    {
        public System.Windows.Forms.Button[] GetCommandLinks(IApplication app, string tag)
        {
            if (tag == "startpage")
            {
                Button btn = MenuButton.Create("Database", "Configure database");
                return new Button[] { btn };
            }

            return new Button[] { };
        }
    }
}
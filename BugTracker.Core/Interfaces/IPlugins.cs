﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Interfaces
{
    public interface IPlugins
    {
        IButton[] CollectCommandLinks(string tag);
    }
}

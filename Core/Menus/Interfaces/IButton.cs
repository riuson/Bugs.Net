﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppCore.Menus
{
    public interface IButton
    {
        event EventHandler Click;
    }
}
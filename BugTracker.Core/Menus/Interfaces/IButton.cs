using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.Core.Menus
{
    public interface IButton
    {
        event EventHandler Click;
    }
}

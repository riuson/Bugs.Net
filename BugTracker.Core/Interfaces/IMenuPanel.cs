using BugTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Interfaces
{
    public interface IMenuPanel
    {
        void Add(IButton button);
        void Add(IApplication app, string tag);

        Control AsControl { get; }
    }
}

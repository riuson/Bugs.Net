using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Interfaces
{
    public interface IControlManager
    {
        void Show(Control ctrl);
        void Hide(Control ctrl);
        void Hide(int steps);
        void Hide();
        IEnumerable<string> Titles { get; }
    }
}

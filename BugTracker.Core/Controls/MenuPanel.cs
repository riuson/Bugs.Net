using BugTracker.Core.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Controls
{
    public class MenuPanel : FlowLayoutPanel
    {
        public MenuPanel()
        {
        }

        public void Add(Button button)
        {
            this.Controls.Add(button);
        }
    }
}

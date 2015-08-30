using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BugTracker.Core.Interfaces;

namespace BugTracker.About.Controls
{
    public partial class ControlAbout : UserControl
    {
        public ControlAbout(IApplication app)
        {
            InitializeComponent();
        }
    }
}

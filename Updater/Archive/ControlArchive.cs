using AppCore;
using AppCore.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Updater.Archive
{
    public partial class ControlArchive : UserControl
    {
        private IApplication mApp;

        public ControlArchive(IApplication app)
        {
            InitializeComponent();
            this.Text = "Make archive".Tr();

            this.mApp = app;
        }
    }
}

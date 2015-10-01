using BugTracker.Core;
using BugTracker.Core.Classes;
using BugTracker.Core.Extensions;
using BugTracker.DB.Classes;
using BugTracker.DB.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.DB.Errors
{
    internal partial class ControlError : UserControl
    {
        IApplication mApp;

        public ControlError(IApplication app)
        {
            InitializeComponent();
            this.Text = "Database error".Tr();
            this.labelErrorMessage.Text = this.labelErrorMessage.Text.Tr();
            this.mApp = app;
        }
    }
}

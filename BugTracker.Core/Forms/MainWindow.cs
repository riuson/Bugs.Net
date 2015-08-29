using BugTracker.Core.Classes;
using BugTracker.Core.Controls;
using BugTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Forms
{
    public partial class MainWindow : Form
    {
        private IApplication mApp;

        public MainWindow(IApplication app)
        {
            InitializeComponent();

            this.mApp = app;

            StartMenu menu = new StartMenu(this.mApp);
            this.Controls.Add(menu);
            menu.Dock = DockStyle.Fill;
        }
    }
}

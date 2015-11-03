﻿using AppCore.Classes;
using AppCore.Controls;
using AppCore.Extensions;
using AppCore.Menus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppCore.Main
{
    internal partial class MainWindow : Form
    {
        private IApplication mApp;
        private ControlManager mControls;
        private TaskScheduler mContext;

        public MainWindow(IApplication app)
        {
            InitializeComponent();
            this.Text = "Bugs.Net";

            this.mApp = app;
            this.mControls = new ControlManager();
            this.mControls.ControlShow += this.mControls_ControlShow;
            this.mControls.ControlHide += this.mControls_ControlHide;
            this.mContext = TaskScheduler.FromCurrentSynchronizationContext();

            StartMenu menu = new StartMenu(this.mApp);
            this.mControls.Show(menu);
        }

        public IControlManager ControlManager
        {
            get { return this.mControls; }
        }

        private void mControls_ControlHide(object sender, ControlManager.ControlChangeEventArgs e)
        {
            e.Control.Hide();
            this.panelControls.Controls.Remove(e.Control);
            this.navigationBar.UpdateTitles(this.mControls.Titles);
        }

        private void mControls_ControlShow(object sender, ControlManager.ControlChangeEventArgs e)
        {
            this.panelControls.Controls.Add(e.Control);
            e.Control.Show();
            e.Control.Dock = DockStyle.Fill;
            this.navigationBar.UpdateTitles(this.mControls.Titles);
        }

        private void navigationBar_Navigate(object sender, NavigationBar.NavigateEventArgs e)
        {
            this.mControls.Hide(e.Steps);
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            int count = this.mControls.Titles.Count();

            if (count > 1)
            {
                this.mControls.Hide(count - 1);
            }
        }

        internal void ActivateFromThread()
        {
            Task task = new Task(() => { });
            task.ContinueWith(o =>
                {
                    this.Activate();
                }, this.mContext);
            task.Start();
        }
    }
}

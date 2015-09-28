﻿using BugTracker.Core.Classes;
using BugTracker.Core.Controls;
using BugTracker.Core.Localization;
using BugTracker.Core.Main;
using BugTracker.Core.Messages;
using BugTracker.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugTracker.Core.Main
{
    public class App : MarshalByRefObject, IDisposable, IApplication
    {
        private ControlContainer mContainer;
        private MainWindow mWindow;
        private Plugins.Plugins mPlugins;
        private MessageCenter mMessages;

        public App()
        {
            this.mContainer = new ControlContainer();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            System.Threading.Thread.CurrentThread.CurrentCulture = LocalizationManager.Instance.ActiveUICulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = LocalizationManager.Instance.ActiveUICulture;

            this.mMessages = new MessageCenter();
            this.mPlugins = new Plugins.Plugins(this);

            this.mWindow = new MainWindow(this);
            this.mWindow.FormClosed += this.mWindow_FormClosed;
            this.mWindow.Show();
        }

        public void Dispose()
        {
            LocalizationManager.Instance.Flush();
            this.mWindow.Close();
            this.mWindow.Dispose();
            this.mContainer.Dispose();
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void Run()
        {
            try
            {
                Application.Run();
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
                System.Diagnostics.Debug.WriteLine(exc.Source);
                System.Diagnostics.Debug.WriteLine(exc.StackTrace);

                Exceptions.Handler.Handle(exc);
            }
        }

        public void Exit()
        {
            Application.Exit();
        }

        private void mWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Exit();
        }

        public IPlugins Plugins { get { return this.mPlugins; } }

        public IControlManager Controls { get { return this.mWindow.ControlManager; } }

        public IMessageCenter Messages { get { return this.mMessages; } }

        public IWin32Window OwnerWindow { get { return this.mWindow; } }

        public ILocalizationManager Localization { get { return LocalizationManager.Instance; } }
    }
}

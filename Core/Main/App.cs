using AppCore.Classes;
using AppCore.Controls;
using AppCore.Localization;
using AppCore.Main;
using AppCore.Messages;
using AppCore.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppCore.Main
{
    public class App : MarshalByRefObject, IDisposable, IApplication
    {
        private ControlContainer mContainer;
        private MainWindow mWindow;
        private Plugins.Plugins mPlugins;
        private MessageCenter mMessages;
        private static LookupBugWorkaround mLookupBugWorkAround = new LookupBugWorkaround();
        private InstanceMonitor mInstanceMonitor;

        public App()
        {
            this.mContainer = new ControlContainer();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            this.mInstanceMonitor = null;

            System.Threading.Thread.CurrentThread.CurrentCulture = LocalizationManager.Instance.ActiveUICulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = LocalizationManager.Instance.ActiveUICulture;

            this.mMessages = new MessageCenter();
            this.mPlugins = new Plugins.Plugins(this);

            this.mWindow = new MainWindow(this);
            this.mWindow.FormClosed += this.mWindow_FormClosed;
            this.mWindow.Show();

            this.mPlugins.Start();
        }

        public void Dispose()
        {
            if (this.mInstanceMonitor != null)
            {
                this.mInstanceMonitor.Dispose();
                this.mInstanceMonitor = null;
            }

            LocalizationManager.Instance.Flush();
            this.mWindow.Close();
            this.mWindow.Dispose();
            this.mWindow = null;

            this.mPlugins.Dispose();
            this.mPlugins = null;

            this.mContainer.Dispose();
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void Run(AppStartInfo startInfo)
        {
            try
            {
                this.StartInfo = startInfo;
                this.mInstanceMonitor = new InstanceMonitor(this.StartInfo.InstanceSemaphore);
                this.mInstanceMonitor.AnotherInstanceStarted = () =>
                    {
                        if (this.mWindow != null)
                        {
                            this.mWindow.ActivateFromThread();
                        }
                    };

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

        public AppStartInfo StartInfo { get; private set; }

        public IPlugins Plugins { get { return this.mPlugins; } }

        public IControlManager Controls { get { return this.mWindow.ControlManager; } }

        public IMessageCenter Messages { get { return this.mMessages; } }

        public IWin32Window OwnerWindow { get { return this.mWindow; } }

        public ILocalizationManager Localization { get { return LocalizationManager.Instance; } }
    }
}

using BugTracker.Core.Classes;
using BugTracker.Core.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugTracker.Core
{
    public class App : IDisposable
    {
        private ControlContainer mContainer;
        private MainWindow mWindow;

        public App()
        {
            this.mContainer = new ControlContainer();

            this.mWindow = new MainWindow();
            this.mWindow.FormClosed += this.mWindow_FormClosed;
            this.mWindow.Show();
        }

        public void Dispose()
        {
            this.mWindow.Close();
            this.mWindow.Dispose();
            this.mContainer.Dispose();
        }

        public void Exit()
        {
            Application.Exit();
        }

        private void mWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Exit();
        }
    }
}

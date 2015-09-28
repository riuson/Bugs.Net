using BugTracker.Classes;
using BugTracker.Core;
using BugTracker.Core.Main;
using System;
using System.Windows.Forms;

namespace BugTracker
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                using (Domain<App> appInDomain = new Domain<App>())
                {
                    appInDomain.Object.Run();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}

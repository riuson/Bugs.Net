using System;
using System.Windows.Forms;
using BugTracker.Core;
using BugTracker.Classes;

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

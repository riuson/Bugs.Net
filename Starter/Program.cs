using AppCore.Main;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace AppStarter
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] arguments)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                AppStartInfo info = new AppStartInfo();
                info.Arguments = arguments;
                info.ExecutablePath = GetExecutablePath();
                info.ExecutableDir = Path.GetDirectoryName(info.ExecutablePath);

                if (info.AlreadyRunned)
                {
                    info.SignalToExistingInstance();
                }
                else
                {
                    using (Domain<App> appInDomain = new Domain<App>())
                    {
                        appInDomain.Object.Run(info);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private static string GetExecutablePath()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return path;
        }
    }
}

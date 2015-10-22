using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Updater.CommandLine;

namespace Updater
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] arguments)
        {
            if (arguments.Length == 1)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                try
                {
                    FormUpdater form = new FormUpdater();
                    form.Show();
                    form.FormClosing += form_FormClosing;
                    Runner updater = new Runner(arguments);
                    updater.Completed = () =>
                    {
                        Application.Exit();
                    };
                    updater.Failed = () =>
                    {
                    };
                    updater.Log = form.Log;
                    updater.Run();
                    Application.Run();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(
                        exc.Message + Environment.NewLine + exc.StackTrace,
                        "Exception occured");
                }
            }
        }

        static void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}

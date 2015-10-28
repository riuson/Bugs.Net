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

                    CommandLine.Updater updater = new CommandLine.Updater(arguments);
                    updater.Log = form.Log;

                    updater.Completed = () =>
                    {
                        Application.Exit();
                    };
                    updater.Failed = () =>
                    {
                    };

                    updater.Start();
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

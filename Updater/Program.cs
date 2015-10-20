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
                try
                {
                    Runner updater = new Runner(arguments);
                    updater.Run();
                }
                catch(Exception exc)
                {
                    MessageBox.Show(
                        exc.Message + Environment.NewLine + exc.StackTrace,
                        "Exception occured");
                }
            }
        }
    }
}

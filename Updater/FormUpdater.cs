using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Updater.CommandLine;

namespace Updater
{
    public partial class FormUpdater : Form
    {
        private TaskScheduler mContext;

        public FormUpdater(string[] arguments)
        {
            InitializeComponent();

            this.mContext = TaskScheduler.FromCurrentSynchronizationContext();

            try
            {
                Runner updater = new Runner(arguments);
                updater.Log = this.Log;
                updater.Run();
            }
            catch (Exception exc)
            {
                MessageBox.Show(
                    exc.Message + Environment.NewLine + exc.StackTrace,
                    "Exception occured");
            }
        }

        private bool Log(Updater.CommandLine.Runner.Stage stage, string message)
        {
            Task task = new Task(() => { });
            task.ContinueWith(
                (o) =>
                {
                    this.richTextBoxLog.AppendText(String.Format("{0}...", message));
                    this.richTextBoxLog.AppendText(Environment.NewLine);
                }, this.mContext);
            task.Start();
            return true;
        }
    }
}

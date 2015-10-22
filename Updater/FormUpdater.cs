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
using AppCore.Extensions;

namespace Updater
{
    public partial class FormUpdater : Form
    {
        private TaskScheduler mContext;

        public FormUpdater()
        {
            InitializeComponent();

            this.mContext = TaskScheduler.FromCurrentSynchronizationContext();
        }

        public bool Log(Updater.CommandLine.Stage stage, string message, Color color)
        {
            Task task = new Task(() => { });
            task.ContinueWith(
                (o) =>
                {
                    this.richTextBoxLog.AppendText(String.Format("{0}: {1}", stage, message), color);
                    this.richTextBoxLog.AppendText(Environment.NewLine);
                }, this.mContext);
            task.Start();
            return true;
        }
    }
}

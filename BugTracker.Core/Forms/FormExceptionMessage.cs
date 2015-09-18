using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BugTracker.Core.Extensions;

namespace BugTracker.Core.Forms
{
    internal partial class FormExceptionMessage : Form
    {
        private FormExceptionMessage()
        {
            InitializeComponent();
        }

        public FormExceptionMessage(Exception exc)
            : this()
        {
            Exception e = exc;

            while (e != null)
            {
                this.richTextBox1.AppendText("Exception ", SystemColors.ControlText);
                this.richTextBox1.AppendText(e.Message, Color.Red);
                this.richTextBox1.AppendText(" at ", SystemColors.ControlText);
                this.richTextBox1.AppendText(e.Source, Color.Blue);
                this.richTextBox1.AppendText(Environment.NewLine, SystemColors.ControlText);
                this.richTextBox1.AppendText(e.StackTrace, Color.Gray);
                this.richTextBox1.AppendText(Environment.NewLine, SystemColors.ControlText);
                e = e.InnerException;
            }
        }
    }
}

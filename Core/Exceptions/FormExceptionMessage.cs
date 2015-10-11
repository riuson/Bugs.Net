﻿using BugTracker.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Exceptions
{
    internal partial class FormExceptionMessage : Form
    {
        private FormExceptionMessage()
        {
            InitializeComponent();
            this.Text = "Exception handled".Tr();
            this.buttonOk.Text = this.buttonOk.Text.Tr();
        }

        public FormExceptionMessage(Exception exc)
            : this()
        {
            Exception e = exc;

            while (e != null)
            {
                this.richTextBox1.AppendText("Exception ".Tr(), SystemColors.ControlText);
                this.richTextBox1.AppendText(e.Message, Color.Red);
                this.richTextBox1.AppendText(" at ".Tr(), SystemColors.ControlText);
                this.richTextBox1.AppendText(e.Source, Color.Blue);
                this.richTextBox1.AppendText(Environment.NewLine, SystemColors.ControlText);
                this.richTextBox1.AppendText(e.StackTrace, Color.Gray);
                this.richTextBox1.AppendText(Environment.NewLine, SystemColors.ControlText);
                e = e.InnerException;
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Forms
{
    internal partial class FormInputBox : Form
    {
        private FormInputBox()
        {
            InitializeComponent();
        }

        public FormInputBox(string prompt, string title = "", string defaultValue = "")
            : this()
        {
            this.textBox.Text = defaultValue;
            this.Text = title;
            this.labelPrompt.Text = prompt;

            this.buttonOk.Enabled = this.textBox.Text != String.Empty;
        }

        public string Result
        {
            get
            {
                return this.textBox.Text;
            }
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            this.buttonOk.Enabled = this.textBox.Text != String.Empty;
        }
    }
}
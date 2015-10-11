using AppCore.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AppCore.Dialogs
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
            this.buttonOk.Text = this.buttonOk.Text.Tr();
            this.buttonCancel.Text = this.buttonCancel.Text.Tr();
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

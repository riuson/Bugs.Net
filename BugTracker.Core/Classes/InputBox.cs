using BugTracker.Core.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Classes
{
    public static class InputBox
    {
        public static DialogResult Show(string prompt, string title, string defaultValue, out string result)
        {
            result = String.Empty;

            using (FormInputBox dialog = new FormInputBox(prompt, title, defaultValue))
            {
                DialogResult dr = dialog.ShowDialog();

                if (dr == DialogResult.OK)
                {
                    result = dialog.Result;
                }

                return dr;
            }
        }
    }
}

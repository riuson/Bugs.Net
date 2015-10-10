using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Extensions
{
    public static class RichTextBoxExt
    {
        public static void AppendText(this RichTextBox box, string text, Color clr)
        {
            if (box != null && text != null && text != String.Empty)
            {
                int len = box.Text.Length;
                box.AppendText(text);
                box.Select(len, text.Length);
                box.SelectionColor = clr;
                box.DeselectAll();
            }
        }

        public static void ProtectContent(this RichTextBox box)
        {
            box.Select(0, box.Text.Length);
            box.SelectionProtected = true;
        }
    }
}

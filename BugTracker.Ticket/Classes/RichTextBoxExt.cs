using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.TicketEditor.Classes
{
    internal static class RichTextBoxExt
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
    }
}

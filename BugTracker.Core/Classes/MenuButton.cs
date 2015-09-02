using BugTracker.Core.Controls;
using BugTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Classes
{
    public class MenuButtonData
    {
        public string Text { get; private set; }
        public string Note { get; private set; }
        public Image Image { get; private set; }

        public MenuButtonData(string text, string note = "", Image image = null)
        {
            this.Text = text;
            this.Note = note;
            this.Image = image;
        }
    }

    public static class MenuButton
    {
        public static IButton Create(string text, string note = "", Image image = null)
        {
            CommandLink result = new CommandLink();
            result.Text = text;
            result.NoteText = note;

            if (image != null)
            {
                result.Image = new Bitmap(image);
            }
            else
            {
                result.Image = SystemIcons.Information.ToBitmap();
            }

            return result;
        }

        public static IButton Create(MenuButtonData data)
        {
            return MenuButton.Create(data.Text, data.Note, data.Image);
        }

        private static Size DefaultSize
        {
            get
            {
                return new Size(200, 60);
            }
        }
    }
}

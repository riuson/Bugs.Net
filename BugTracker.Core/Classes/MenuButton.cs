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
        public static Button Create(string text, string note = "", Image image = null)
        {
            Button result;

            if (System.Environment.OSVersion.Version.Major >= 6)
            {
                result = new BugTracker.Core.Controls.CommandLinkButton(text, note, false);
            }
            else
            {
                result = new Button();
                result.Text = text;
                result.TextImageRelation = TextImageRelation.ImageBeforeText;
                result.ImageAlign = ContentAlignment.MiddleLeft;

                if (image != null)
                {
                    result.Image = image;
                }
            }

            result.Size = MenuButton.DefaultSize;

            return result;
        }

        public static Button Create(MenuButtonData data)
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

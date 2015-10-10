using System;
using System.Drawing;
using System.Windows.Forms;

namespace BugTracker.Core.Menus
{
    internal interface ICommandLinkBackend : IDisposable
    {
        string Text { get; set; }
        string Note { get; set; }
        bool UseStandardIcon { get; set; }
        bool ShieldIcon { get; set; }
        Image Image { get; set; }

        bool OnPaint(PaintEventArgs e);
        void OnMouseEnter(EventArgs e);
        void OnMouseLeave(EventArgs e);
        void OnMouseDown(MouseEventArgs e);
        void OnMouseUp(MouseEventArgs e);
        void OnEnabledChanged(EventArgs e);
    }
}

using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Controls
{
    public class CommandLink : Button, IButton
    {
        private ICommandLinkBackend mBackend;

        public CommandLink()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.StandardClick, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            if (this.UseEmulation)
            {
                this.mBackend = new CommandLinkBackendEmulation(this);
            }
            else
            {
                this.mBackend = new CommandLinkBackendNative(this);
            }

            this.mBackend.UseStandardIcon = false;

            this.Size = new Size(250, 70);
        }

        protected override void Dispose(bool disposing)
        {
            this.mBackend.Dispose();
            base.Dispose(disposing);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                if (this.UseEmulation)
                {
                    return CommandLinkBackendEmulation.UpdateParams(base.CreateParams);
                }
                else
                {
                    return CommandLinkBackendNative.UpdateParams(base.CreateParams);
                }
            }
        }

        private bool UseEmulation
        {
            get
            {
                return (Environment.OSVersion.Version.Major < 6);
            }
        }

        public string NoteText
        {
            get
            {
                return this.mBackend.Note;
            }
            set
            {
                this.mBackend.Note = value;
                this.Invalidate();
            }
        }

        public new Image Image
        {
            get
            {
                return this.mBackend.Image;
            }
            set
            {
                this.mBackend.Image = value;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!this.mBackend.OnPaint(e))
            {
                base.OnPaint(e);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            this.mBackend.OnMouseEnter(e);
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.mBackend.OnMouseLeave(e);
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.mBackend.OnMouseDown(e);
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.mBackend.OnMouseUp(e);
            base.OnMouseUp(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            this.mBackend.OnEnabledChanged(e);
            base.OnEnabledChanged(e);
        }
    }
}

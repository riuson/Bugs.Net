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
    public class CommandLink : Button
    {
        private enum State
        {
            Disabled,
            Normal,
            Hover,
            Pushed
        }

        private State mState;

        private string mNoteText { get; set; }

        private Bitmap mGrayscaleImage;

        public CommandLink()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.StandardClick, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            Font font = SystemFonts.DialogFont;
            this.Font = new Font(font.FontFamily, font.Size + 5, font.Style);
            this.NoteFont = new Font(font.FontFamily, font.Size, font.Style);
            this.mNoteText = String.Empty;
            this.mState = State.Normal;

            this.Size = new Size(250, 70);
        }

        public string NoteText
        {
            get
            {
                return this.mNoteText;
            }
            set
            {
                this.mNoteText = value;
                this.Invalidate();
            }
        }

        public Font NoteFont { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Clean background
            using (Brush brush = new SolidBrush(this.BackColor))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if (this.Focused && this.mState == State.Normal)
            {
                this.DrawHighlighted(e.Graphics);
            }

            switch (this.mState)
            {
                case State.Normal:
                case State.Disabled:
                    this.DrawForeground(e.Graphics);
                    break;
                case State.Hover:
                    this.DrawHoveredBackground(e.Graphics);
                    this.DrawForeground(e.Graphics);
                    break;
                case State.Pushed:
                    this.DrawPushedBackground(e.Graphics);
                    this.DrawForeground(e.Graphics);
                    break;
                default:
                    break;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (this.Enabled)
            {
                this.mState = State.Hover;
            }

            this.Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (this.Enabled)
            {
                this.mState = State.Normal;
            }

            this.Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (this.Enabled)
            {
                this.mState = State.Pushed;
            }

            this.Invalidate();
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (this.Enabled)
            {
                if (this.RectangleToScreen(this.ClientRectangle).Contains(Cursor.Position))
                {
                    this.mState = State.Hover;
                }
                else
                {
                    this.mState = State.Normal;
                }
            }

            this.Invalidate();
            base.OnMouseUp(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (this.Enabled)
            {
                this.mState = State.Normal;
            }
            else
            {
                this.mState = State.Disabled;
            }

            this.Invalidate();

            base.OnEnabledChanged(e);
        }

        private void DrawHighlighted(Graphics graphics)
        {
            using (GraphicsPath innerRegion = this.GetRoundedRect(this.Width - 1, this.Height - 1, 3))
            {
                using (Pen inlinePen = new Pen(Color.SkyBlue, 1.0f))
                {
                    graphics.DrawPath(inlinePen, innerRegion);
                }
            }
        }

        private void DrawHoveredBackground(Graphics graphics)
        {
            using (GraphicsPath outerRegion = this.GetRoundedRect(this.Width - 1, this.Height - 1, 3),
                   innerRegion = this.GetRoundedRect(this.Width - 3, this.Height - 3, 2))
            {
                using (Matrix translate = new Matrix())
                {
                    translate.Translate(1, 1);
                    innerRegion.Transform(translate);
                }

                RectangleF backgroundRect = new RectangleF(1, 1, this.Width - 2, (this.Height * 0.75f) - 2);

                using (Pen outlinePen = new Pen(SystemColors.ControlDark),
                       inlinePen = new Pen(Color.FromArgb(240, 255, 255, 255)))
                {
                    using (LinearGradientBrush backgroundBrush = new LinearGradientBrush(new PointF(0, 0), new PointF(0, backgroundRect.Height), Color.White, Color.Transparent))
                    {
                        backgroundBrush.WrapMode = WrapMode.TileFlipXY;
                        graphics.FillRectangle(backgroundBrush, backgroundRect);
                    }

                    graphics.DrawPath(inlinePen, innerRegion);
                    graphics.DrawPath(outlinePen, outerRegion);
                }
            }
        }

        private void DrawPushedBackground(Graphics graphics)
        {
            using (GraphicsPath outerRegion = this.GetRoundedRect(this.Width - 1, this.Height - 1, 3),
                   innerRegion = this.GetRoundedRect(this.Width - 3, this.Height - 3, 2))
            {
                using (Matrix translate = new Matrix())
                {
                    translate.Translate(1, 1);
                    innerRegion.Transform(translate);
                }

                Rectangle backgroundRect = new Rectangle(1, 1, this.Width - 3, this.Height - 3);

                using (Pen outlinePen = new Pen(SystemColors.ButtonShadow),
                       inlinePen = new Pen(SystemColors.ControlLight))
                {
                    SolidBrush backBrush = new SolidBrush(SystemColors.ControlLight);

                    graphics.FillRectangle(backBrush, backgroundRect);
                    graphics.DrawPath(inlinePen, innerRegion);
                    graphics.DrawPath(outlinePen, outerRegion);
                }
            }
        }

        private void DrawForeground(Graphics graphics)
        {
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            PointF imageLocation = new PointF(9, 0);

            // Size of texts
            string descriptionText = this.mNoteText == String.Empty ? " " : this.mNoteText; ;
            SizeF textLayout = graphics.MeasureString(this.Text, this.Font);
            SizeF noteLayout = graphics.MeasureString(descriptionText, this.NoteFont);

            // Combined rectangle for text
            RectangleF totalRect = new RectangleF(0, 0, Math.Max(textLayout.Width, noteLayout.Width), (textLayout.Height + noteLayout.Height) - 4);

            // Allocate space for image
            if (this.Image != null)
                totalRect.X = imageLocation.X + this.Image.Width + 5;
            else
                totalRect.X = 20;

            // Vertical center
            totalRect.Y = (this.Height / 2) - (totalRect.Height / 2);

            // Offset pushed
            if (this.mState == State.Pushed)
            {
                int offset = 1;
                totalRect.Offset(offset, offset);
                imageLocation.X += offset;
            }

            // Align image by vertical
            if (this.Image != null)
            {
                switch (this.ImageAlign)
                {
                    case ContentAlignment.TopLeft:
                    default:
                        imageLocation.Y = totalRect.Y;
                        break;
                    case ContentAlignment.MiddleLeft:
                        imageLocation.Y = totalRect.Y + (totalRect.Height / 2) - (this.Image.Height / 2);
                        break;
                    case ContentAlignment.BottomLeft:
                        imageLocation.Y = totalRect.Y + totalRect.Height - this.Image.Height;
                        break;
                }
            }

            // Select text color
            Color textColor = this.ForeColor;

            if (!this.Enabled)
            {
                textColor = SystemColors.GrayText;
            }
            else if (this.mState == State.Hover)
            {
                textColor = Color.Navy;
            }

            using (SolidBrush textBrush = new SolidBrush(textColor))
            {
                graphics.DrawString(this.Text, this.Font, textBrush, totalRect.Left, totalRect.Top);
                graphics.DrawString(descriptionText, this.NoteFont, textBrush, totalRect.Left + 1, totalRect.Bottom - noteLayout.Height);

                if (this.Image != null)
                {
                    if (this.Enabled)
                    {
                        graphics.DrawImage(this.Image, imageLocation);
                    }
                    else
                    {
                        // Create grayscale if not exists
                        if (this.mGrayscaleImage == null)
                        {
                            this.mGrayscaleImage = this.ToGrayscale(this.Image);
                        }

                        graphics.DrawImage(this.mGrayscaleImage, imageLocation);
                    }
                }
            }
        }

        private GraphicsPath GetRoundedRect(int width, int height, int radius)
        {
            RectangleF baseRect = new RectangleF(0, 0, width, height);
            float diameter = radius * 2.0f;
            SizeF sizeF = new SizeF(diameter, diameter);
            RectangleF arc = new RectangleF(baseRect.Location, sizeF);
            GraphicsPath path = new GraphicsPath();

            // top left arc 
            path.AddArc(arc, 180, 90);

            // top right arc 
            arc.X = baseRect.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc 
            arc.Y = baseRect.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc
            arc.X = baseRect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        private Bitmap ToGrayscale(Image source)
        {
            // Create blank image
            Bitmap result = new Bitmap(source.Width, source.Height);

            // Get graphics
            using (Graphics graphics = Graphics.FromImage(result))
            {
                // Conversion matrix
                ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                {
                    new float[] { 0.3f, 0.3f, 0.3f, 0, 0 },
                    new float[] { 0.59f, 0.59f, 0.59f, 0, 0 },
                    new float[] { 0.11f, 0.11f, 0.11f, 0, 0 },
                    new float[] { 0, 0, 0, 1, 0 },
                    new float[] { 0, 0, 0, 0, 1 }
                });

                // Create attributes
                using (ImageAttributes attributes = new ImageAttributes())
                {
                    attributes.SetColorMatrix(colorMatrix);

                    // Draw image with grayscale matrix
                    graphics.DrawImage(source, new Rectangle(0, 0, source.Width, source.Height), 0, 0, source.Width, source.Height, GraphicsUnit.Pixel, attributes);
                }
            }

            return result;
        }
    }
}

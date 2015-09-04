using BugTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Classes
{
    internal class CommandLinkBackendNative : ICommandLinkBackend
    {
        private Button mButton;
        private Bitmap mBitmapGrayscale;
        private bool mUseStandardIcon;
        private bool mShieldIcon;

        public CommandLinkBackendNative(Button button)
        {
            this.mButton = button;
            this.mButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.mButton.Image = null;
            this.mBitmapGrayscale = null;
        }

        public void Dispose()
        {
            if (this.mBitmapGrayscale != null)
            {
                this.mBitmapGrayscale.Dispose();
            }
        }

        public string Text
        {
            get
            {
                return this.mButton.Text;
            }
            set
            {
                this.mButton.Text = value;
            }
        }

        public string Note
        {
            get
            {
                int length = SendMessage(new HandleRef(this, this.mButton.Handle),
                    BCM_GETNOTELENGTH,
                    IntPtr.Zero, IntPtr.Zero) + 1;

                StringBuilder sb = new StringBuilder(length);

                SendMessage(new HandleRef(this, this.mButton.Handle),
                    BCM_GETNOTE,
                    ref length, sb);

                return sb.ToString();
            }
            set
            {
                SendMessage(new HandleRef(this, this.mButton.Handle),
                    BCM_SETNOTE,
                    IntPtr.Zero,
                    value);
            }
        }

        public bool UseStandardIcon
        {
            get
            {
                return this.mUseStandardIcon;
            }
            set
            {
                this.mUseStandardIcon = value;

                if (this.mUseStandardIcon)
                {
                    this.SetImage(null);

                    this.SetShield(this.mShieldIcon);
                }
                else
                {
                    if (this.mButton.Image != null)
                    {
                        this.SetImage(new Bitmap(this.mButton.Image));
                    }
                    else
                    {
                        this.SetImage(null);
                    }
                }
            }
        }

        public bool ShieldIcon
        {
            get
            {
                return this.mShieldIcon;
            }
            set
            {
                this.mShieldIcon = value;
                this.SetShield(value);
            }
        }

        public System.Drawing.Image Image
        {
            get
            {
                return this.mButton.Image;
            }
            set
            {
                this.SaveImage(value);

                if (value != null)
                {
                    this.SetImage(new Bitmap(value));
                }
                else
                {
                    this.SetImage(new Bitmap(1, 1, PixelFormat.Format32bppArgb));
                }
            }
        }

        public static CreateParams UpdateParams(CreateParams value)
        {
            CreateParams result = value;
            result.Style |= BS_COMMANDLINK;
            return result;
        }

        public bool OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            return false;
        }

        public void OnMouseEnter(EventArgs e)
        {
        }

        public void OnMouseLeave(EventArgs e)
        {
        }

        public void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
        }

        public void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
        }

        public void OnEnabledChanged(EventArgs e)
        {
        }

        private void SaveImage(Image image)
        {
            if (image != null)
            {
                this.mButton.Image = new Bitmap(image);
                this.mBitmapGrayscale = this.ToGrayscale(this.mButton.Image);
            }
            else
            {
                this.mButton.Image = null;
                this.mBitmapGrayscale = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
            }
        }

        private void SetImage(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                IntPtr iconHandle = bitmap.GetHicon();
                SendMessage(new HandleRef(this, this.mButton.Handle),
                    BM_SETIMAGE,
                    (IntPtr)1,
                    iconHandle);
            }
            else
            {
                SendMessage(new HandleRef(this, this.mButton.Handle),
                    BM_SETIMAGE,
                    (IntPtr)1,
                    IntPtr.Zero);
            }
        }

        private void SetShield(bool value)
        {
            SendMessage(new HandleRef(this, this.mButton.Handle), BCM_SETSHIELD, IntPtr.Zero, value);
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

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int SendMessage(HandleRef hWnd, UInt32 Msg, ref int wParam, StringBuilder lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int SendMessage(HandleRef hWnd, UInt32 Msg, IntPtr wParam, string lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int SendMessage(HandleRef hWnd, UInt32 Msg, IntPtr wParam, bool lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int SendMessage(HandleRef hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        private const int BS_COMMANDLINK = 0x0000000e;
        private const uint BCM_SETSHIELD = 0x0000160c;
        private const uint BCM_SETNOTE = 0x00001609;
        private const uint BCM_GETNOTE = 0x0000160a;
        private const uint BCM_GETNOTELENGTH = 0x0000160b;
        private const uint BM_SETIMAGE = 0x000000f7;
    }
}

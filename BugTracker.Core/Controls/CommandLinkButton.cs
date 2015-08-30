using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Controls
{
    internal class CommandLinkButton : Button
    {
        public CommandLinkButton()
        {
            this.FlatStyle = System.Windows.Forms.FlatStyle.System;
        }

        public CommandLinkButton(string text, string description = "", bool showShield = false)
            : this()
        {
            this.Text = text;
            this.SetDescription(description);
            this.SetShield(showShield);
        }

        private void SetShield(bool value)
        {
            SendMessage(new HandleRef(this, this.Handle), BCM_SETSHIELD, IntPtr.Zero, value);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams result = base.CreateParams;
                result.Style |= BS_COMMANDLINK;
                return result;
            }
        }

        private void SetDescription(string value)
        {
            SendMessage(new HandleRef(this, this.Handle),
                BCM_SETNOTE,
                IntPtr.Zero,
                value);
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern int SendMessage(HandleRef hWnd, UInt32 Msg, IntPtr wParam, string lParam);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern int SendMessage(HandleRef hWnd, UInt32 Msg, IntPtr wParam, bool lParam);

        private const int BS_COMMANDLINK = 0x0000000e;
        private const int BCM_SETNOTE = 0x00001609;
        private const uint BCM_SETSHIELD = 0x0000160c;
    }
}

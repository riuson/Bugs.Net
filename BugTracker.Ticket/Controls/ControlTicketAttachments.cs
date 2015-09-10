using BugTracker.DB.Entities;
using BugTracker.DB.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.TicketEditor.Controls
{
    internal class ControlTicketAttachments : FlowLayoutPanel
    {
        private ToolTip mToolTip;

        public ControlTicketAttachments()
        {
            this.mToolTip = new ToolTip();
        }

        public void UpdateTicketData(Ticket ticket, ISession session)
        {
            foreach (Attachment att in ticket.Attachments)
            {
                Button btn = new Button();
                btn.Text = att.Filename;
                btn.Tag = att;
                Icon icon = this.GetFileIcon(att.Filename);
                btn.Image = icon.ToBitmap();
                btn.TextImageRelation = TextImageRelation.ImageBeforeText;
                string tip = String.Format(
                    "{0}\nUploaded by {1} {2} at {3}\nComment: {4}",
                    att.Filename,
                    att.Author.FirstName, att.Author.LastName,
                    att.Created,
                    att.Comment);
                this.mToolTip.SetToolTip(btn, tip);
                this.Controls.Add(btn);
            }
        }

        private Icon GetFileIcon(string name)
        {
            ControlTicketAttachments.SHFILEINFO shfi = new ControlTicketAttachments.SHFILEINFO();
            uint flags = ControlTicketAttachments.SHGFI_ICON | ControlTicketAttachments.SHGFI_USEFILEATTRIBUTES | ControlTicketAttachments.SHGFI_SMALLICON;

            ControlTicketAttachments.SHGetFileInfo(name,
                ControlTicketAttachments.FILE_ATTRIBUTE_NORMAL,
                ref shfi,
                (uint)System.Runtime.InteropServices.Marshal.SizeOf(shfi),
                flags);


            Icon icon = (Icon)Icon.FromHandle(shfi.hIcon).Clone();
            ControlTicketAttachments.DestroyIcon(shfi.hIcon);
            return icon;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        private const uint SHGFI_ICON = 0x000000100;
        private const uint SHGFI_SMALLICON = 0x000000001;
        private const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;
        private const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;

        [DllImport("Shell32.dll")]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        [DllImport("User32.dll")]
        private static extern int DestroyIcon(IntPtr hIcon);
    }
}

using BugTracker.Core.Interfaces;
using BugTracker.DB;
using BugTracker.DB.Entities;
using BugTracker.DB.Interfaces;
using BugTracker.DB.Repositories;
using BugTracker.TicketEditor.Events;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.TicketEditor.Controls
{
    internal class ControlTicketAttachments : FlowLayoutPanel
    {
        private IApplication mApp;
        private ToolTip mToolTip;

        public event EventHandler<SaveAttachmentEventArgs> SaveAttachment;
        public event EventHandler<LoadAttachmentsEventArgs> LoadAttachments;

        public ControlTicketAttachments(IApplication app)
        {
            this.mApp = app;
            this.mToolTip = new ToolTip();
            this.AutoScroll = true;

            this.AllowDrop = true;
            this.DragEnter += this.ControlTicketAttachments_DragEnter;
            this.DragDrop += this.ControlTicketAttachments_DragDrop;
        }

        public void UpdateTicketData(ISession session, Ticket ticket)
        {
            foreach (Attachment att in ticket.Attachments)
            {
                Button btn = new Button();
                btn.Text = att.Filename;
                btn.Tag = att;
                Icon icon = this.GetFileIcon(att.Filename);
                btn.Image = icon.ToBitmap();
                btn.MinimumSize = new System.Drawing.Size(0, icon.Height + 8);
                btn.TextImageRelation = TextImageRelation.ImageBeforeText;
                btn.AutoSize = true;
                string tip = String.Format(
                    "{0}\nUploaded by {1} {2} at {3}\nComment: {4}",
                    att.Filename,
                    att.Author.FirstName, att.Author.LastName,
                    att.Created,
                    att.Comment);
                this.mToolTip.SetToolTip(btn, tip);
                btn.Click += this.btnFile_Click;
                this.Controls.Add(btn);
            }
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender is Button)
                {
                    Button btn = sender as Button;

                    if (btn.Tag is Attachment)
                    {
                        Attachment attachment = btn.Tag as Attachment;

                        if (this.SaveAttachment != null)
                        {
                            using (SaveFileDialog dialog = new SaveFileDialog())
                            {
                                dialog.CheckPathExists = true;
                                dialog.FileName = attachment.Filename;
                                dialog.OverwritePrompt = true;
                                dialog.RestoreDirectory = true;
                                dialog.Title = "Save attachment";
                                dialog.DefaultExt = Path.GetExtension(attachment.Filename);
                                dialog.Filter = String.Format("*.{0}|*.{0}", dialog.DefaultExt);
                                dialog.FilterIndex = 1;

                                if (dialog.ShowDialog() == DialogResult.OK)
                                {
                                    this.SaveAttachment(this, new SaveAttachmentEventArgs(attachment, dialog.FileName));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(this.mApp.OwnerWindow, "Can't save file:\n" + exc.Message, "File save error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void ControlTicketAttachments_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void ControlTicketAttachments_DragDrop(object sender, DragEventArgs e)
        {
            string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (this.LoadAttachments != null)
            {
                this.LoadAttachments(this, new LoadAttachmentsEventArgs(filenames));
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

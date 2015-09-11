using BugTracker.Core.Interfaces;
using BugTracker.DB;
using BugTracker.DB.Entities;
using BugTracker.DB.Interfaces;
using BugTracker.DB.Repositories;
using BugTracker.TicketEditor.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.TicketEditor.Controls
{
    internal class ControlTicketAttachments : Panel
    {
        private IApplication mApp;
        private DataTable mTableAttachments;
        private DataGridView mDataGridView;

        public event EventHandler<SaveAttachmentEventArgs> SaveAttachment;
        public Attachment[] AddedAttachments
        {
            get
            {
                List<Attachment> result = new List<Attachment>();

                foreach (DataRow row in this.mTableAttachments.Rows)
                {
                    Attachment attachment = row["attachment"] as Attachment;

                    if (attachment.Id == 0)
                    {
                        attachment.Comment = Convert.ToString(row["comment"]);
                        result.Add(attachment);
                    }
                }

                return result.ToArray();
            }
        }
        public Attachment[] RemovedAttachments
        {
            get
            {
                List<Attachment> result = new List<Attachment>();

                foreach (DataRow row in this.mTableAttachments.Rows)
                {
                    Attachment attachment = row["attachment"] as Attachment;

                    if (Convert.ToBoolean(row["removed"]))
                    {
                        result.Add(attachment);
                    }
                }

                return result.ToArray();
            }
        }

        public ControlTicketAttachments(IApplication app)
        {
            this.mApp = app;

            {
                this.mTableAttachments = new DataTable("attachments");
                this.mTableAttachments.Columns.Add("icon", typeof(Icon));
                this.mTableAttachments.Columns.Add("filename", typeof(String));
                this.mTableAttachments.Columns.Add("path", typeof(String));
                this.mTableAttachments.Columns.Add("comment", typeof(String));
                this.mTableAttachments.Columns.Add("attachment", typeof(Attachment));
                this.mTableAttachments.Columns.Add("download", typeof(String)).DefaultValue = "Download";
                this.mTableAttachments.Columns.Add("remove", typeof(String)).DefaultValue = "Remove";
                this.mTableAttachments.Columns.Add("removed", typeof(Boolean)).DefaultValue = false;
            }

            {
                this.mDataGridView = new DataGridView();
                this.mDataGridView.AutoGenerateColumns = false;
                this.mDataGridView.AllowUserToAddRows = false;
                this.mDataGridView.AllowUserToDeleteRows = false;
                this.mDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
                this.mDataGridView.BackgroundColor = SystemColors.Window;
                this.mDataGridView.Columns.Add(new DataGridViewImageColumn()
                {
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                    DataPropertyName = "icon",
                    HeaderText = "Type"
                });
                this.mDataGridView.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                    DataPropertyName = "filename",
                    HeaderText = "Filename"
                });
                this.mDataGridView.Columns.Add(new DataGridViewTextBoxColumn()
                {
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    DataPropertyName = "comment",
                    HeaderText = "Comment"
                });
                this.mDataGridView.Columns.Add(new DataGridViewButtonColumn()
                {
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                    DataPropertyName = "download"
                });
                this.mDataGridView.Columns.Add(new DataGridViewButtonColumn()
                {
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                    DataPropertyName = "remove"
                });

                this.mDataGridView.CellContentClick += this.mDataGridView_CellContentClick;
                this.mDataGridView.CellBeginEdit += this.mDataGridView_CellBeginEdit;
                this.mDataGridView.CellFormatting += this.mDataGridView_CellFormatting;

                this.mDataGridView.Dock = DockStyle.Fill;
                this.Controls.Add(this.mDataGridView);

                this.mDataGridView.DataSource = this.mTableAttachments;
            }

            this.AllowDrop = true;
            this.DragEnter += this.ControlTicketAttachments_DragEnter;
            this.DragDrop += this.ControlTicketAttachments_DragDrop;
        }

        private void mDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                DataRowView drv = this.mDataGridView.Rows[e.RowIndex].DataBoundItem as DataRowView;
                DataRow row = drv.Row as DataRow;
                Attachment attachment = row["attachment"] as Attachment;
                //string propertyName = this.mDataGridView.Columns[e.ColumnIndex].DataPropertyName;

                if (Convert.ToBoolean(row["removed"]))
                {
                    e.CellStyle.BackColor = Color.Red;
                }

                if (attachment.Id == 0)
                {
                    e.CellStyle.BackColor = Color.Green;
                }
            }
        }

        private void mDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                DataRowView drv = this.mDataGridView.Rows[e.RowIndex].DataBoundItem as DataRowView;
                DataRow row = drv.Row as DataRow;

                if (Convert.ToString(row["path"]) == String.Empty)
                {
                    e.Cancel = true;
                }

                if (this.mDataGridView.Columns[e.ColumnIndex].DataPropertyName != "comment")
                {
                    e.Cancel = true;
                }
            }
        }

        private void mDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                if (this.mDataGridView.Columns[e.ColumnIndex].DataPropertyName == "download")
                {
                    DataRowView drv = this.mDataGridView.Rows[e.RowIndex].DataBoundItem as DataRowView;
                    DataRow row = drv.Row as DataRow;
                    Attachment attachment = row["attachment"] as Attachment;

                    if (this.SaveAttachment != null && attachment != null && attachment.Id != 0)
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

                if (this.mDataGridView.Columns[e.ColumnIndex].DataPropertyName == "remove")
                {
                    DataRowView drv = this.mDataGridView.Rows[e.RowIndex].DataBoundItem as DataRowView;
                    DataRow row = drv.Row as DataRow;
                    Attachment attachment = row["attachment"] as Attachment;

                    // If record exists
                    if (attachment.Id != 0)
                    {
                        // Restore
                        if (Convert.ToBoolean(row["removed"]))
                        {
                            row["removed"] = false;
                        }
                        else // Add to list for remove
                        {
                            row["removed"] = true;
                        }

                        this.mDataGridView.InvalidateRow(e.RowIndex);
                    }
                    else
                    {
                        // Just remove row
                        this.mTableAttachments.Rows.Remove(row);
                    }
                }
            }
        }

        public void UpdateTicketData(ISession session, Ticket ticket)
        {
            this.mTableAttachments.Clear();

            foreach (Attachment att in ticket.Attachments)
            {
                DataRow row = this.mTableAttachments.NewRow();
                row["icon"] = this.GetFileIcon(att.Filename); ;
                row["filename"] = att.Filename;
                row["comment"] = att.Comment;
                row["attachment"] = att;
                this.mTableAttachments.Rows.Add(row);
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

            foreach (var filename in filenames)
            {
                BlobContent blob = new BlobContent();

                using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    blob.ReadFrom(fs);
                }

                Attachment attachment = new Attachment();
                attachment.File = blob;
                attachment.Created = DateTime.Now;
                attachment.Filename = Path.GetFileName(filename);

                DataRow row = this.mTableAttachments.NewRow();
                row["icon"] = this.GetFileIcon(filename);
                row["filename"] = Path.GetFileName(filename);
                row["path"] = filename;
                row["attachment"] = attachment;
                this.mTableAttachments.Rows.Add(row);
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

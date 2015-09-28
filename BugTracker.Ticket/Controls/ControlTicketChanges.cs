using BugTracker.Core.Extensions;
using BugTracker.DB.DataAccess;
using BugTracker.DB.Entities;
using BugTracker.TicketEditor.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.TicketEditor.Controls
{
    internal class ControlTicketChanges : Panel
    {
        private RichTextBox mTextBox;
        private int mCommentStartPos;

        public ControlTicketChanges()
        {
            this.mTextBox = new RichTextBox();
            this.mTextBox.Dock = DockStyle.Fill;
            this.mTextBox.Margin = new Padding(5);
            this.mTextBox.MinimumSize = new System.Drawing.Size(100, 50);
            this.mTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Controls.Add(this.mTextBox);

            this.PreparePrompt();
        }

        public void UpdateTicketData(ISession session, Ticket ticket)
        {
            this.mTextBox.Clear();

            foreach (var change in ticket.Changes)
            {
                this.mTextBox.AppendText(change.Author.FullName, Color.Navy);

                this.mTextBox.AppendText(
                    " at ".Tr(),
                    Color.Silver);

                this.mTextBox.AppendText(
                    String.Format(
                        "{0}",
                        change.Created),
                    Color.DarkGreen);

                this.mTextBox.AppendText(Environment.NewLine);

                this.mTextBox.AppendText(
                    Encoding.UTF8.GetString(change.Description.Content),
                    Color.Gray);

                this.mTextBox.AppendText(Environment.NewLine);
                this.mTextBox.AppendText(Environment.NewLine);
            }

            this.PreparePrompt();
        }

        private void PreparePrompt()
        {
            this.mTextBox.AppendText("Add new comment here:".Tr(), Color.Red);

            this.mTextBox.AppendText(Environment.NewLine);

            this.mTextBox.Select(0, this.mTextBox.Text.Length);
            this.mTextBox.SelectionProtected = true;

            this.mTextBox.Focus();
            this.mTextBox.SelectionStart = this.mTextBox.Text.Length;
            this.mTextBox.ScrollToCaret();

            this.mCommentStartPos = this.mTextBox.Text.Length;
        }

        public bool HasNewComment
        {
            get
            {
                return (this.mCommentStartPos < this.mTextBox.Text.Length);
            }
        }

        public string NewComment
        {
            get
            {
                return this.mTextBox.Text.Substring(this.mCommentStartPos);
            }
        }
    }
}

using BugTracker.DB;
using BugTracker.DB.Entities;
using BugTracker.DB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.TicketEditor.Controls
{
    internal class ControlTicketChanges : Panel
    {
        private RichTextBox mTextBox;

        public ControlTicketChanges()
        {
            this.AutoScroll = true;

            this.mTextBox = new RichTextBox();
            this.mTextBox.Dock = DockStyle.Fill;
            this.mTextBox.Margin = new Padding(5);
            this.mTextBox.MinimumSize = new System.Drawing.Size(100, 50);
            this.mTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.mTextBox);
        }

        public void UpdateTicketData(Ticket ticket, ISession session)
        {
            this.Controls.Clear();
            this.Controls.Remove(this.mTextBox);

            foreach (var change in ticket.Changes)
            {
                Label label = new Label();
                label.AutoSize = true;
                label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                label.Margin = new System.Windows.Forms.Padding(5);
                label.Padding = new System.Windows.Forms.Padding(5);
                label.MaximumSize = new System.Drawing.Size(200, 0);
                label.MinimumSize = new System.Drawing.Size(0, 50);
                label.Text = String.Format(
                    "{0} {1} at {2}\n\n{3}",
                    change.Author.FirstName,
                    change.Author.LastName,
                    change.Created,
                    Encoding.UTF8.GetString(change.Description.Content));

                this.Controls.Add(label);
                label.Dock = DockStyle.Top;
                label.BringToFront();
            }

            this.Controls.Add(this.mTextBox);
            this.mTextBox.Dock = DockStyle.Fill;
            this.mTextBox.BringToFront();
        }

        public string NewComment
        {
            get
            {
                return this.mTextBox.Text;
            }
        }

        protected override void OnResize(EventArgs eventargs)
        {
            foreach (Control ctrl in this.Controls)
            {
                ctrl.MaximumSize = new System.Drawing.Size(this.Width - 1, 0);
            }

            base.OnResize(eventargs);
        }
    }
}

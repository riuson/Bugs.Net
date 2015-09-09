using BugTracker.TicketEditor.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.TicketEditor.Controls
{
    internal class ControlTicketContent : FlowLayoutPanel
    {
        public void UpdateTicketData(TicketData value)
        {
            this.Controls.Clear();

            foreach (var change in value.ChangeList)
            {
                Label label = new Label()
                {
                    AutoSize = true,
                    Text = change
                };

                this.Controls.Add(label);
                this.SetFlowBreak(label, true);
            }
        }
    }
}

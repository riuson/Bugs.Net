using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BugTracker.Core.Interfaces;
using BugTracker.Projects.Classes;
using BugTracker.DB.Entities;
using BugTracker.Tickets.Classes;

namespace BugTracker.Tickets.Controls
{
    public partial class ControlTicketsList : UserControl
    {
        private IApplication mApp;
        private TicketsListData mTicketsList;

        public ControlTicketsList(IApplication app)
        {
            InitializeComponent();
            this.Text = "Tickets list";
            this.mApp = app;

            this.mTicketsList = new TicketsListData(this.mApp);

            this.dgvList.AutoGenerateColumns = false;
            this.dgvList.DataSource = this.mTicketsList.Data;

            this.UpdateButtons();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            this.mTicketsList.Add();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (this.dgvList.SelectedCells.Count > 0)
            {
                int rowIndex = this.dgvList.SelectedCells[0].RowIndex;
                Ticket item = this.dgvList.Rows[rowIndex].DataBoundItem as Ticket;
                this.mTicketsList.Edit(item);
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (this.dgvList.SelectedCells.Count > 0)
            {
                int rowIndex = this.dgvList.SelectedCells[0].RowIndex;
                Ticket item = this.dgvList.Rows[rowIndex].DataBoundItem as Ticket;
                this.mTicketsList.Remove(item);
            }
        }

        private void UpdateButtons()
        {
            if (this.dgvList.SelectedCells.Count > 0)
            {
                this.buttonEdit.Enabled = true;
                this.buttonRemove.Enabled = true;
                this.buttonView.Enabled = true;
            }
            else
            {
                this.buttonEdit.Enabled = false;
                this.buttonRemove.Enabled = false;
                this.buttonView.Enabled = false;
            }
        }

        private void dgvList_SelectionChanged(object sender, EventArgs e)
        {
            this.UpdateButtons();
        }

        private void buttonTickets_Click(object sender, EventArgs e)
        {
            if (this.dgvList.SelectedCells.Count > 0)
            {
                int rowIndex = this.dgvList.SelectedCells[0].RowIndex;
                Ticket item = this.dgvList.Rows[rowIndex].DataBoundItem as Ticket;
                this.mTicketsList.ShowTicket(item);
            }
        }
    }
}

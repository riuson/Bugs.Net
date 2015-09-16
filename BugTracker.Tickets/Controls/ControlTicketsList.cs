using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BugTracker.Core.Interfaces;
using BugTracker.DB.Entities;
using BugTracker.Tickets.Classes;
using BugTracker.DB.Classes;

namespace BugTracker.Tickets.Controls
{
    public partial class ControlTicketsList : UserControl
    {
        private IApplication mApp;
        private TicketsListData mTicketsList;
        private Member mLoggedMember;
        private Project mProject;

        private DataGridViewTextBoxColumn mColumnTitle;
        private DataGridViewColumn mColumnAuthor;
        private DataGridViewTextBoxColumn mColumnCreated;
        private DataGridViewColumn mColumnStatus;

        public ControlTicketsList(IApplication app, Member loggedMember, Project project)
        {
            InitializeComponent();
            this.Text = "Tickets list";
            this.mApp = app;
            this.mLoggedMember = loggedMember;
            this.mProject = project;

            this.mTicketsList = new TicketsListData(this.mApp, this.mLoggedMember, this.mProject);

            this.dgvList.AutoGenerateColumns = false;
            this.CreateColumns();
            this.dgvList.DataSource = this.mTicketsList.Data;

            this.UpdateButtons();

            this.VisibleChanged += this.ControlTicketsList_VisibleChanged;
        }

        private void CreateColumns()
        {
            // 
            // columnTitle
            // 
            this.mColumnTitle = new DataGridViewTextBoxColumn();
            this.mColumnTitle.DataPropertyName = "Title";
            this.mColumnTitle.HeaderText = "Title";
            this.mColumnTitle.Name = "columnTitle";
            this.mColumnTitle.ReadOnly = true;
            // 
            // columnAuthor
            // 
            this.mColumnAuthor = DataGridViewColumnFabric.CreateSubColumn(BugTracker.DB.Classes.DataGridViewColumnFabric.ColumnType.SubColumn);
            this.mColumnAuthor.DataPropertyName = "Author.FullName";
            this.mColumnAuthor.HeaderText = "Author";
            this.mColumnAuthor.Name = "columnAuthor";
            this.mColumnAuthor.ReadOnly = true;
            // 
            // columnCreated
            // 
            this.mColumnCreated = new DataGridViewTextBoxColumn();
            this.mColumnCreated.DataPropertyName = "Created";
            this.mColumnCreated.HeaderText = "Created";
            this.mColumnCreated.Name = "columnCreated";
            this.mColumnCreated.ReadOnly = true;
            // 
            // columnStatus
            // 
            this.mColumnStatus = DataGridViewColumnFabric.CreateSubColumn(BugTracker.DB.Classes.DataGridViewColumnFabric.ColumnType.SubColumn);
            this.mColumnStatus.DataPropertyName = "Status.Value";
            this.mColumnStatus.HeaderText = "Status";
            this.mColumnStatus.Name = "columnStatus";
            this.mColumnStatus.ReadOnly = true;

            this.dgvList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.mColumnTitle,
            this.mColumnAuthor,
            this.mColumnCreated,
            this.mColumnStatus});
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

        private void dgvList_DoubleClick(object sender, EventArgs e)
        {
            if (this.dgvList.SelectedCells.Count > 0)
            {
                int rowIndex = this.dgvList.SelectedCells[0].RowIndex;
                Ticket item = this.dgvList.Rows[rowIndex].DataBoundItem as Ticket;
                this.mTicketsList.Edit(item);
            }
        }

        private void UpdateButtons()
        {
            if (this.dgvList.SelectedCells.Count > 0)
            {
                this.buttonEdit.Enabled = true;
                this.buttonRemove.Enabled = true;
            }
            else
            {
                this.buttonEdit.Enabled = false;
                this.buttonRemove.Enabled = false;
            }
        }

        private void dgvList_SelectionChanged(object sender, EventArgs e)
        {
            this.UpdateButtons();
        }

        private void ControlTicketsList_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.UpdateButtons();
                this.mTicketsList.UpdateList();
            }
        }
    }
}

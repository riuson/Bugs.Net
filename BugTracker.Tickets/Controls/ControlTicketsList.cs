using BugTracker.Core;
using BugTracker.Core.Extensions;
using BugTracker.DB.Classes;
using BugTracker.DB.Entities;
using BugTracker.Tickets.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

        private DataGridViewColumn mSortColumn;
        private SortOrder mSortOrder;
        private string mTitleFilter;

        public ControlTicketsList(IApplication app, Member loggedMember, Project project)
        {
            InitializeComponent();
            this.Text = "Tickets list".Tr();
            this.buttonAdd.Text = this.buttonAdd.Text.Tr();
            this.buttonEdit.Text = this.buttonEdit.Text.Tr();
            this.buttonRemove.Text = this.buttonRemove.Text.Tr();
            this.mApp = app;
            this.mLoggedMember = loggedMember;
            this.mProject = project;

            this.mTicketsList = new TicketsListData(this.mApp, this.mLoggedMember, this.mProject);

            this.dgvList.AutoGenerateColumns = false;
            this.CreateColumns();
            this.dgvList.ColumnHeaderMouseClick += this.dgvList_ColumnHeaderMouseClick;
            this.dgvList.DataSource = this.mTicketsList.Data;

            this.VisibleChanged += this.ControlTicketsList_VisibleChanged;

            this.mSortColumn = this.mColumnTitle;
            this.mSortOrder = SortOrder.Ascending;
            this.mTitleFilter = String.Empty;
            this.ApplyFilter();
        }

        private void CreateColumns()
        {
            // 
            // columnTitle
            // 
            this.mColumnTitle = new DataGridViewTextBoxColumn();
            this.mColumnTitle.DataPropertyName = "Title";
            this.mColumnTitle.HeaderText = "Title".Tr();
            this.mColumnTitle.Name = "columnTitle";
            this.mColumnTitle.ReadOnly = true;
            // 
            // columnAuthor
            // 
            this.mColumnAuthor = DataGridViewColumnFabric.CreateSubColumn(BugTracker.DB.Classes.DataGridViewColumnFabric.ColumnType.SubColumn);
            this.mColumnAuthor.DataPropertyName = "Author.FullName";
            this.mColumnAuthor.HeaderText = "Author".Tr();
            this.mColumnAuthor.Name = "columnAuthor";
            this.mColumnAuthor.ReadOnly = true;
            // 
            // columnCreated
            // 
            this.mColumnCreated = new DataGridViewTextBoxColumn();
            this.mColumnCreated.DataPropertyName = "Created";
            this.mColumnCreated.HeaderText = "Created".Tr();
            this.mColumnCreated.Name = "columnCreated";
            this.mColumnCreated.ReadOnly = true;
            // 
            // columnStatus
            // 
            this.mColumnStatus = DataGridViewColumnFabric.CreateSubColumn(BugTracker.DB.Classes.DataGridViewColumnFabric.ColumnType.SubColumn);
            this.mColumnStatus.DataPropertyName = "Status.Value";
            this.mColumnStatus.HeaderText = "Status".Tr();
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

        private void dgvList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.mSortColumn == this.dgvList.Columns[e.ColumnIndex])
            {
                switch (this.mSortOrder)
                {
                    case SortOrder.Ascending:
                        this.mSortOrder = SortOrder.Descending;
                        break;
                    case SortOrder.Descending:
                        this.mSortOrder = SortOrder.None;
                        break;
                    case SortOrder.None:
                        this.mSortOrder = SortOrder.Ascending;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (this.mSortColumn != null)
                {
                    this.mSortColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                }

                this.mSortColumn = this.dgvList.Columns[e.ColumnIndex];
                this.mSortOrder = SortOrder.Ascending;
            }

            this.ApplyFilter();
        }

        private void UpdateColumns()
        {
            // Update sorting glyphs
            foreach (DataGridViewColumn column in this.dgvList.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Programmatic;
                column.HeaderCell.SortGlyphDirection = (column == this.mSortColumn ? this.mSortOrder : SortOrder.None);
            }
        }

        private void textBoxTitleFilter_TextChanged(object sender, EventArgs e)
        {
            this.mTitleFilter = this.textBoxTitleFilter.Text;
            this.ApplyFilter();
        }

        private void ApplyFilter()
        {
            this.mTicketsList.ApplyFilter(this.mSortColumn.DataPropertyName, this.mSortOrder, this.mTitleFilter);
            this.UpdateColumns();
            this.UpdateButtons();
        }
    }
}

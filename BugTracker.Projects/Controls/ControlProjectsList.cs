using BugTracker.Core;
using BugTracker.Core.Extensions;
using BugTracker.DB.Entities;
using BugTracker.Projects.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Projects.Controls
{
    public partial class ControlProjectsList : UserControl
    {
        private IApplication mApp;
        private ProjectsListData mProjectsList;
        private Member mLoggedMember;

        public ControlProjectsList(IApplication app, Member loggedMember)
        {
            InitializeComponent();
            this.Text = "Projects list".Tr();
            this.buttonAdd.Text = this.buttonAdd.Text.Tr();
            this.buttonEdit.Text = this.buttonEdit.Text.Tr();
            this.buttonRemove.Text = this.buttonRemove.Text.Tr();
            this.buttonTickets.Text = this.buttonTickets.Text.Tr();
            this.columnName.HeaderText = this.columnName.HeaderText.Tr();
            this.mApp = app;
            this.mLoggedMember = loggedMember;

            this.mProjectsList = new ProjectsListData(this.mApp, this.mLoggedMember);

            this.dgvList.AutoGenerateColumns = false;
            this.dgvList.DataSource = this.mProjectsList.Data;

            this.UpdateButtons();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            this.mProjectsList.Add();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (this.dgvList.SelectedCells.Count > 0)
            {
                int rowIndex = this.dgvList.SelectedCells[0].RowIndex;
                Project item = this.dgvList.Rows[rowIndex].DataBoundItem as Project;
                this.mProjectsList.Edit(item);
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (this.dgvList.SelectedCells.Count > 0)
            {
                int rowIndex = this.dgvList.SelectedCells[0].RowIndex;
                Project item = this.dgvList.Rows[rowIndex].DataBoundItem as Project;
                this.mProjectsList.Remove(item);
            }
        }

        private void UpdateButtons()
        {
            if (this.dgvList.SelectedCells.Count > 0)
            {
                this.buttonEdit.Enabled = true;
                this.buttonRemove.Enabled = true;
                this.buttonTickets.Enabled = true;
            }
            else
            {
                this.buttonEdit.Enabled = false;
                this.buttonRemove.Enabled = false;
                this.buttonTickets.Enabled = false;
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
                Project item = this.dgvList.Rows[rowIndex].DataBoundItem as Project;
                this.mProjectsList.ShowTickets(item);
            }
        }

        private void dgvList_DoubleClick(object sender, EventArgs e)
        {
            if (this.dgvList.SelectedCells.Count > 0)
            {
                int rowIndex = this.dgvList.SelectedCells[0].RowIndex;
                Project item = this.dgvList.Rows[rowIndex].DataBoundItem as Project;
                this.mProjectsList.ShowTickets(item);
            }
        }
    }
}

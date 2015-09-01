using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BugTracker.Core.Interfaces;
using BugTracker.Projects.Classes;
using BugTracker.DB.Entities;

namespace BugTracker.Projects.Controls
{
    public partial class ControlProjectsList : UserControl
    {
        private IApplication mApp;
        private ProjectsListData mProjectsList;

        public ControlProjectsList(IApplication app)
        {
            InitializeComponent();
            this.Text = "Projects list";
            this.mApp = app;

            this.mProjectsList = new ProjectsListData(this.mApp);

            this.dgvList.AutoGenerateColumns = false;
            this.dgvList.DataSource = this.mProjectsList.Data;
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
    }
}

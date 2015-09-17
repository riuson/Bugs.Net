using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BugTracker.Core.Interfaces;
using BugTracker.Members.Classes;
using BugTracker.DB.Entities;

namespace BugTracker.Members.Controls
{
    public partial class ControlMembersList : UserControl
    {
        private IApplication mApp;
        private MembersListData mMembersList;

        public ControlMembersList(IApplication app)
        {
            InitializeComponent();
            this.Text = "Members list";
            this.mApp = app;

            this.mMembersList = new MembersListData(this.mApp);

            this.dgvList.AutoGenerateColumns = false;
            this.dgvList.DataSource = this.mMembersList.Data;
            this.mMembersList.Data.ListChanged += this.Data_ListChanged;
            this.UpdateButtons();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            this.mMembersList.Add();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (this.dgvList.SelectedCells.Count > 0)
            {
                int rowIndex = this.dgvList.SelectedCells[0].RowIndex;
                Member item = this.dgvList.Rows[rowIndex].DataBoundItem as Member;
                this.mMembersList.Edit(item);
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (this.dgvList.SelectedCells.Count > 0)
            {
                int rowIndex = this.dgvList.SelectedCells[0].RowIndex;
                Member item = this.dgvList.Rows[rowIndex].DataBoundItem as Member;
                this.mMembersList.Remove(item);
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

        void Data_ListChanged(object sender, ListChangedEventArgs e)
        {
            this.UpdateButtons();
        }
    }
}

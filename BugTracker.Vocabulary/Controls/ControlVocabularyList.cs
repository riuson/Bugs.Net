using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BugTracker.Core.Interfaces;
using BugTracker.Vocabulary.Classes;
using BugTracker.DB.Entities;

namespace BugTracker.Vocabulary.Controls
{
    public partial class ControlVocabularyList<T> : UserControl where T : new()
    {
        private IApplication mApp;
        private VocabularyListData<T> mVocabularyList;

        public ControlVocabularyList(IApplication app)
        {
            InitializeComponent();
            this.Text = String.Format("Vocabulary list <{0}>", typeof(T).Name);
            this.mApp = app;

            this.mVocabularyList = new VocabularyListData<T>(this.mApp);

            this.dgvList.AutoGenerateColumns = false;
            this.dgvList.DataSource = this.mVocabularyList.Data;

            this.UpdateButtons();

            this.VisibleChanged += this.ControlVocabularyList_VisibleChanged;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            this.mVocabularyList.Add();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (this.dgvList.SelectedCells.Count > 0)
            {
                int rowIndex = this.dgvList.SelectedCells[0].RowIndex;
                T item = (T)this.dgvList.Rows[rowIndex].DataBoundItem;
                this.mVocabularyList.Edit(item);
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (this.dgvList.SelectedCells.Count > 0)
            {
                int rowIndex = this.dgvList.SelectedCells[0].RowIndex;
                T item = (T)this.dgvList.Rows[rowIndex].DataBoundItem;
                this.mVocabularyList.Remove(item);
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

        private void ControlVocabularyList_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.UpdateButtons();
                this.mVocabularyList.UpdateList();
            }
        }
    }
}

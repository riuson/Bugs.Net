using BugTracker.Core;
using BugTracker.Core.Extensions;
using BugTracker.DB.Entities;
using BugTracker.Vocabulary.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Vocabulary.Controls
{
    public partial class ControlVocabularyList<T> : UserControl where T : class, new()
    {
        private IApplication mApp;
        private VocabularyListData<T> mVocabularyList;

        public ControlVocabularyList(IApplication app)
        {
            InitializeComponent();
            this.Text = String.Format("Vocabulary list <{0}>".Tr(), typeof(T).Name);
            this.buttonAdd.Text = this.buttonAdd.Text.Tr();
            this.buttonEdit.Text = this.buttonEdit.Text.Tr();
            this.buttonRemove.Text = this.buttonRemove.Text.Tr();
            this.columnValue.HeaderText = this.columnValue.HeaderText.Tr();
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

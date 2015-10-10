using BugTracker.Core;
using BugTracker.Core.Extensions;
using BugTracker.Translator.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Translator.Controls
{
    public partial class ControlTranslations : UserControl
    {
        private IApplication mApp;
        private LanguagesListData mData;

        public ControlTranslations(IApplication app)
        {
            InitializeComponent();
            this.Text = "Languages".Tr();
            this.buttonAdd.Text = this.buttonAdd.Text.Tr();
            this.buttonEdit.Text = this.buttonEdit.Text.Tr();
            this.buttonRemove.Text = this.buttonRemove.Text.Tr();
            this.columnName.HeaderText = this.columnName.HeaderText.Tr();
            this.columnDisplayName.HeaderText = this.columnDisplayName.HeaderText.Tr();
            this.columnNativeName.HeaderText = this.columnNativeName.HeaderText.Tr();


            this.mApp = app;

            this.mData = new LanguagesListData(this.mApp);
            this.mData.Data.ListChanged += this.Data_ListChanged;

            this.dgvList.AutoGenerateColumns = false;
            this.dgvList.DataSource = this.mData.Data;
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

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            this.mData.Add();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (this.dgvList.SelectedCells.Count > 0)
            {
                int rowIndex = this.dgvList.SelectedCells[0].RowIndex;

                if (rowIndex >= 0)
                {
                    DataGridViewRow dgvr = this.dgvList.Rows[rowIndex];
                    CultureInfo culture = dgvr.DataBoundItem as CultureInfo;

                    this.mData.Edit(culture);
                }
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (this.dgvList.SelectedCells.Count > 0)
            {
                int rowIndex = this.dgvList.SelectedCells[0].RowIndex;

                if (rowIndex >= 0)
                {
                    DataGridViewRow dgvr = this.dgvList.Rows[rowIndex];
                    CultureInfo culture = dgvr.DataBoundItem as CultureInfo;

                    if (MessageBox.Show(
                        this.mApp.OwnerWindow,
                        String.Format(
                            "Do you really want remove language files for '{0}'?".Tr(),
                            culture),
                        "Remove language files".Tr(),
                        MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        this.mData.Remove(culture);
                    }
                }
            }
        }

        private void Data_ListChanged(object sender, ListChangedEventArgs e)
        {
            this.UpdateButtons();
        }
    }
}

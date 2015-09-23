using BugTracker.Core.Extensions;
using BugTracker.Core.Interfaces;
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
    public partial class ControlLanguages : UserControl
    {
        private IApplication mApp;
        private BindingSource mBS;

        public event EventHandler Confirmed;
        public event EventHandler Rejected;

        public ControlLanguages(IApplication app)
        {
            InitializeComponent();
            this.Text = "Select new languages".Tr();
            this.buttonOk.Text = this.buttonOk.Text.Tr();
            this.buttonCancel.Text = this.buttonCancel.Text.Tr();
            this.columnName.HeaderText = this.columnName.HeaderText.Tr();
            this.columnDisplayName.HeaderText = this.columnDisplayName.HeaderText.Tr();
            this.columnNativeName.HeaderText = this.columnNativeName.HeaderText.Tr();

            this.mApp = app;

            this.mBS = new BindingSource();

            this.dgvList.AutoGenerateColumns = false;
            this.dgvList.DataSource = this.mBS;

            this.UpdateList();
        }

        public IEnumerable<CultureInfo> SelectedCultures
        {
            get
            {
                IEnumerable<DataGridViewRow> rows = this.dgvList.SelectedRows.Cast<DataGridViewRow>();
                rows = rows.Where<DataGridViewRow>(row => row.Index >= 0 && row.DataBoundItem is CultureInfo);
                IEnumerable<CultureInfo> cultures = rows.Select<DataGridViewRow, CultureInfo>(row => row.DataBoundItem as CultureInfo);
                return cultures;
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (this.Confirmed != null)
            {
                this.Confirmed(this, EventArgs.Empty);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (this.Rejected != null)
            {
                this.Rejected(this, EventArgs.Empty);
            }
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            this.UpdateList(this.textBoxFilter.Text);
        }

        private void UpdateList(string filter = "")
        {
            if (filter == String.Empty)
            {
                this.mBS.DataSource = CultureInfo.GetCultures(CultureTypes.AllCultures);
            }
            else
            {
                this.mBS.DataSource = CultureInfo.GetCultures(CultureTypes.AllCultures)
                    .Where(
                        culture =>
                            culture.Name.ToLower().Contains(filter.ToLower()) ||
                            culture.DisplayName.ToLower().Contains(filter.ToLower()) ||
                            culture.NativeName.ToLower().Contains(filter.ToLower()));
            }
        }
    }
}

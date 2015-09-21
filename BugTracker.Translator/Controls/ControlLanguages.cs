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
            this.mBS.DataSource = CultureInfo.GetCultures(CultureTypes.AllCultures);

            this.dgvList.AutoGenerateColumns = false;
            this.dgvList.DataSource = this.mBS;
        }

        //public CultureInfo SelectedCulture
        //{
        //    get
        //    {
        //        return this.comboBoxLangauge.SelectedItem as CultureInfo;
        //    }
        //}

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
    }
}

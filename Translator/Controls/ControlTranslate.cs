using AppCore;
using AppCore.Classes;
using AppCore.Extensions;
using AppCore.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Translator.Controls
{
    public partial class ControlTranslate : UserControl
    {
        private IApplication mApp;
        private CultureInfo mCulture;

        private IEnumerable<string> mModules;
        private BindingSource mBSModules;

        private IEnumerable<TranslationUnit> mUnits;
        private BindingSource mBSUnits;

        private string mSelectedModule;
        private TranslationUnit mSelectedUnit;

        public ControlTranslate(IApplication app, CultureInfo culture)
        {
            InitializeComponent();
            this.Text = "Translate".Tr();

            this.mApp = app;
            this.mCulture = culture;

            this.mModules = app.Localization.GetModules(this.mCulture);
            this.mBSModules = new BindingSource();
            this.mBSModules.DataSource = this.mModules;

            this.listBoxModules.DataSource = this.mBSModules;
            this.mBSUnits = new BindingSource();

            this.mSelectedModule = String.Empty;
            this.mSelectedUnit = null;
        }

        private void listBoxModules_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxModules.SelectedIndex >= 0)
            {
                string moduleName = this.listBoxModules.SelectedItem.ToString();
                this.mSelectedModule = moduleName;

                this.mUnits = this.mApp.Localization.GetTranslationUnits(this.mCulture, moduleName);
                this.mBSUnits.DataSource = this.mUnits;
                this.dgvList.DataSource = this.mBSUnits;
            }
        }

        private void dgvList_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dgvList.SelectedCells.Count > 0)
            {
                int rowIndex = this.dgvList.SelectedCells[0].RowIndex;

                if (rowIndex >= 0)
                {
                    DataGridViewRow dgvr = this.dgvList.Rows[rowIndex];
                    TranslationUnit unit = dgvr.DataBoundItem as TranslationUnit;
                    this.mSelectedUnit = unit;

                    this.richTextBoxSource.Text = unit.SourceString;
                    this.richTextBoxSource.ProtectContent();

                    this.richTextBoxTranslated.Text = unit.TranslatedString;
                }
            }
        }

        private void richTextBoxTranslated_Leave(object sender, EventArgs e)
        {
            if (this.mSelectedUnit != null)
            {
                this.mSelectedUnit.TranslatedString = this.richTextBoxTranslated.Text;
            }
        }
    }
}

using AppCore;
using AppCore.Extensions;
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
    public partial class ControlLanguage : UserControl
    {
        private IApplication mApp;
        private BindingSource mBS;

        public event EventHandler Confirmed;
        public event EventHandler Rejected;

        public ControlLanguage(IApplication app)
        {
            InitializeComponent();
            this.Text = "Select language".Tr();
            this.labelLanguageTitle.Text = this.labelLanguageTitle.Text.Tr();
            this.buttonOk.Text = this.buttonOk.Text.Tr();
            this.buttonCancel.Text = this.buttonCancel.Text.Tr();
            this.labelRestartNote.Text = this.labelRestartNote.Text.Tr();

            this.mApp = app;

            this.mBS = new BindingSource();
            this.mBS.DataSource = this.mApp.Localization.FoundCultures;
            this.comboBoxLangauge.DataSource = this.mBS;
            this.comboBoxLangauge.DisplayMember = "DisplayName";

            this.comboBoxLangauge.SelectedItem = this.mApp.Localization.ActiveUICulture;
        }

        public CultureInfo SelectedCulture
        {
            get
            {
                return this.comboBoxLangauge.SelectedItem as CultureInfo;
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
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BugTracker.Core.Interfaces;
using BugTracker.Core.Extensions;
using BugTracker.Translator.Classes;

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

            this.dgvList.AutoGenerateColumns = false;
            this.dgvList.DataSource = this.mData.Data;
        }
    }
}

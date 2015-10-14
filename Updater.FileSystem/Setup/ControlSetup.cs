using AppCore;
using AppCore.Classes;
using AppCore.Extensions;
using AppCore.Menus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Updater.FileSystem.Classes;

namespace Updater.FileSystem.Setup
{
    public partial class ControlSetup : UserControl
    {
        private IApplication mApp;

        public ControlSetup(IApplication app)
        {
            InitializeComponent();
            this.mApp = app;

            this.Text = "File system".Tr();
            this.labelSourceDirectory.Text = this.labelSourceDirectory.Text.Tr();

            this.LoadSettings();
        }

        private void BeforeDisposing()
        {
            this.SaveSettings();
        }

        private void LoadSettings()
        {
            this.textBoxSourceDirectory.Text = Saved<Options>.Instance.SourceDirectory;
        }

        private void SaveSettings()
        {
            Saved<Options>.Instance.SourceDirectory = this.textBoxSourceDirectory.Text;
            Saved<Options>.Save();
        }

        private void buttonBrowseDirectory_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select directory for updates source".Tr();
                dialog.RootFolder = Environment.SpecialFolder.Desktop;
                dialog.SelectedPath = this.textBoxSourceDirectory.Text;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.textBoxSourceDirectory.Text = dialog.SelectedPath;
                }
            }
        }
    }
}

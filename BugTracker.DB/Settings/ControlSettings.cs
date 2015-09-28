using BugTracker.Core;
using BugTracker.Core.Classes;
using BugTracker.Core.Extensions;
using BugTracker.DB.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.DB.Settings
{
    internal partial class ControlSettings : UserControl
    {
        IApplication mApp;

        public ControlSettings(IApplication app)
        {
            InitializeComponent();
            this.Text = "Database settings".Tr();
            this.mApp = app;

            this.LoadSettings();
        }

        private void BeforeDisposing()
        {
            this.SaveSettings();
        }

        private void LoadSettings()
        {
            this.textBoxFilename.Text = Saved<Options>.Instance.FileName;
        }

        private void SaveSettings()
        {
            try
            {
                string filename = this.textBoxFilename.Text;
                string dir = Path.GetDirectoryName(filename);

                if (File.Exists(filename))
                {
                    Saved<Options>.Instance.FileName = filename;
                    Saved<Options>.Save();
                }
                else
                {
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    using (FileStream fs = File.Create(filename))
                    {
                        fs.Close();
                    }

                    File.Delete(filename);

                    Saved<Options>.Instance.FileName = filename;
                    Saved<Options>.Save();
                }
            }
            catch (Exception exc)
            {
                BugTracker.Core.Exceptions.Handler.Handle(exc);
            }
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.CheckFileExists = false;
                dialog.CheckPathExists = true;
                dialog.DefaultExt = "sqlite";
                dialog.FileName = Saved<Options>.Instance.FileName;
                dialog.Filter = "SQLite files (*.sqlite)|*.sqlite|All files (*.*)|*.*".Tr();
                dialog.InitialDirectory = Path.GetDirectoryName(dialog.FileName);
                dialog.Multiselect = false;
                dialog.Title = "Select database file".Tr();

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.textBoxFilename.Text = dialog.FileName;
                }
            }
        }
    }
}

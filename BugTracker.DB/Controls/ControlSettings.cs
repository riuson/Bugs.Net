using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BugTracker.Core.Interfaces;
using BugTracker.Core.Classes;
using BugTracker.DB.Classes;
using System.IO;

namespace BugTracker.DB.Controls
{
    internal partial class ControlSettings : UserControl
    {
        IApplication mApp;

        public ControlSettings(IApplication app)
        {
            InitializeComponent();
            this.Text = "Database settings";
            this.mApp = app;

            this.LoadSettings();
        }

        private void Disposing()
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
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.CheckFileExists = true;
                dialog.CheckPathExists = true;
                dialog.DefaultExt = "sqlite";
                dialog.FileName = Saved<Options>.Instance.FileName;
                dialog.Filter = "SQLite files (*.sqlite)|*.sqlite|All files (*.*)|*.*";
                dialog.InitialDirectory = Path.GetDirectoryName(dialog.FileName);
                dialog.Multiselect = false;
                dialog.Title = "Select database file";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.textBoxFilename.Text = dialog.FileName;
                }
            }
        }
    }
}

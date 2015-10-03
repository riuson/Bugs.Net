using BugTracker.Core;
using BugTracker.Core.Classes;
using BugTracker.Core.Extensions;
using BugTracker.DB.Classes;
using BugTracker.DB.DataAccess;
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
    internal partial class ControlBackup : UserControl
    {
        IApplication mApp;

        public ControlBackup(IApplication app)
        {
            InitializeComponent();
            this.Text = "Backup settings".Tr();
            this.buttonCheckBackup.Text = this.buttonCheckBackup.Text.Tr();
            this.mApp = app;

            this.LoadSettings();
        }

        private void BeforeDisposing()
        {
        }

        private void LoadSettings()
        {
            this.textBoxBackupDirectory.Text = Saved<Options>.Instance.BackupToDirectory;
            this.numericUpDownRepeat.Value = Saved<Options>.Instance.BackupKeepMinDays;
            this.numericUpDownRemove.Value = Saved<Options>.Instance.BackupKeepMaxDays;
        }

        private void SaveSettings()
        {
            try
            {
                string backupDirectory = this.textBoxBackupDirectory.Text;
                int backupMinDays = Convert.ToInt32(this.numericUpDownRepeat.Value);
                int backupMaxDays = Convert.ToInt32(this.numericUpDownRemove.Value);

                if (!Directory.Exists(backupDirectory))
                {
                    Directory.CreateDirectory(backupDirectory);
                }

                Saved<Options>.Instance.BackupToDirectory = backupDirectory;
                Saved<Options>.Instance.BackupKeepMinDays = backupMinDays;
                Saved<Options>.Instance.BackupKeepMaxDays = backupMaxDays;
                Saved<Options>.Save();
            }
            catch (Exception exc)
            {
                BugTracker.Core.Exceptions.Handler.Handle(exc);
            }
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Selct directory to save backups".Tr();
                dialog.RootFolder = Environment.SpecialFolder.DesktopDirectory;
                dialog.SelectedPath = this.textBoxBackupDirectory.Text;
                dialog.ShowNewFolderButton = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.textBoxBackupDirectory.Text = dialog.SelectedPath;
                }
            }
        }

        private void buttonCheckBackup_Click(object sender, EventArgs e)
        {
            this.richTextBoxLog.Clear();
            this.Log("Check backup...".Tr());

            try
            {
                this.SaveSettings();

                Backup backup = new Backup(Saved<Options>.Instance.DatabaseFileName);

                this.Log("Files before:".Tr());
                this.ShowFiles(backup);

                this.Log(String.Empty);
                this.Log("Processing...".Tr());
                this.Log(String.Empty);

                backup.Process(Saved<Options>.Instance.DatabaseFileName);

                this.Log("Files after:".Tr());
                this.ShowFiles(backup);
            }
            catch (Exception exc)
            {
                this.Log(Environment.NewLine);
                this.Log("Check row count failed.".Tr());
                this.Log(exc.Message);
            }

            this.richTextBoxLog.ProtectContent();
        }

        private void Log(string message)
        {
            this.richTextBoxLog.AppendText(message);
            this.richTextBoxLog.AppendText(Environment.NewLine);
        }

        private void ShowFiles(Backup backup)
        {
            var files = backup.GetAllArchiveFiles();
            this.Log("All archives:".Tr());

            foreach (var item in files)
            {
                this.richTextBoxLog.AppendText(item.FullName);
                this.richTextBoxLog.AppendText(Environment.NewLine);
            }

            this.Log("Removing:".Tr());

            foreach (var item in backup.GetFilesToRemove(files))
            {
                this.richTextBoxLog.AppendText(item.FullName);
                this.richTextBoxLog.AppendText(Environment.NewLine);
            }

            this.Log("New:".Tr());

            foreach (var item in backup.GetFilesNew(files))
            {
                this.richTextBoxLog.AppendText(item.FullName);
                this.richTextBoxLog.AppendText(Environment.NewLine);
            }
        }
    }
}

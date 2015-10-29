using AppCore;
using AppCore.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Updater.Tarx;

namespace Updater.Archive
{
    public partial class ControlArchive : UserControl
    {
        private IApplication mApp;
        private IEnumerable<FileData> mFiles;
        private BindingSource mBS;
        private bool mCheckedByCodeFlag;
        private TaskScheduler mContext;

        public ControlArchive(IApplication app)
        {
            this.mCheckedByCodeFlag = true;
            InitializeComponent();
            this.Text = "Make archive".Tr();
            this.buttonSaveArchive.Text = this.buttonSaveArchive.Text.Tr();
            this.columnIncluded.HeaderText = this.columnIncluded.HeaderText.Tr();
            this.columnPath.HeaderText = this.columnPath.HeaderText.Tr();

            this.mApp = app;

            DirectoryInfo rootDirectory = new DirectoryInfo(this.mApp.StartInfo.ExecutableDir);
            this.mFiles = (from item in rootDirectory.GetFiles("*.*", SearchOption.AllDirectories)
                           orderby item.FullName
                           select new FileData(rootDirectory, item)).ToList();
            this.mBS = new BindingSource();
            this.mBS.DataSource = this.mFiles;
            this.dgvFiles.AutoGenerateColumns = false;
            this.dgvFiles.DataSource = this.mBS;

            this.mContext = TaskScheduler.FromCurrentSynchronizationContext();
        }

        private void ControlArchive_Load(object sender, EventArgs e)
        {
            this.columnIncluded.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.columnPath.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.mCheckedByCodeFlag = false;
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            if (this.textBoxFilter.Text == String.Empty)
            {
                this.mBS.DataSource = this.mFiles;
            }
            else
            {
                string filter = this.textBoxFilter.Text.ToLower();
                this.mBS.DataSource = from item in this.mFiles
                                      where item.RelativePath.ToLower().Contains(filter)
                                      select item;
            }
        }

        private IEnumerable<FileData> GetSelectedFiles()
        {
            var files = this.dgvFiles.SelectedCells.OfType<DataGridViewCell>()
                .Select(item => item.OwningRow)
                .Distinct()
                .Where(item => item.DataBoundItem is FileData)
                .Select(item => item.DataBoundItem as FileData);

            return files;
        }

        private void dgvFiles_SelectionChanged(object sender, EventArgs e)
        {
            var files = this.GetSelectedFiles();

            this.mCheckedByCodeFlag = true;

            if (files.All(item => item.Included))
            {
                this.checkBoxSelecteed.CheckState = CheckState.Checked;
            }
            else if (!files.Any(item => item.Included))
            {
                this.checkBoxSelecteed.CheckState = CheckState.Unchecked;
            }
            else
            {
                this.checkBoxSelecteed.CheckState = CheckState.Indeterminate;
            }

            this.mCheckedByCodeFlag = false;
        }

        private void checkBoxSelecteed_CheckStateChanged(object sender, EventArgs e)
        {
            if (!this.mCheckedByCodeFlag)
            {
                var files = this.GetSelectedFiles();

                bool chk = this.checkBoxSelecteed.Checked;

                foreach (var item in files)
                {
                    item.Included = chk;
                }

                this.dgvFiles.InvalidateColumn(0);
            }
        }

        private void buttonSaveArchive_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.AddExtension = true;
                dialog.CheckFileExists = false;
                dialog.CheckPathExists = true;
                dialog.CreatePrompt = false;
                dialog.DefaultExt = ".tarx.gz";
                dialog.Filter = "Archives (*.tarx.gz)|*.tarx.gz".Tr(); ;
                dialog.FilterIndex = 0;
                dialog.OverwritePrompt = true;
                dialog.Title = "Save archive as".Tr();

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string filename = dialog.FileName;

                    this.CreateArchive(filename);
                }
            }
            //DirectoryInfo directory = 
        }

        private void CreateArchive(string filename)
        {
            this.buttonSaveArchive.Enabled = false;

            Task task = new Task(() =>
                {
                    using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
                    {
                        using (GZipStream gs = new GZipStream(fs, CompressionMode.Compress))
                        {
                            using (Packer packer = new Packer(gs, true, null))
                            {
                                packer.BaseDirectory = this.mApp.StartInfo.ExecutableDir;

                                foreach (var item in this.mFiles.Where(item => item.Included))
                                {
                                    packer.AddFile(item.File);
                                }
                            }
                        }
                    }

                }
            );

            task.ContinueWith((o) =>
                {
                    this.buttonSaveArchive.Enabled = true;
                },
                this.mContext);

            task.Start();
        }

        private class FileData
        {
            public FileInfo File { get; private set; }
            public DirectoryInfo RootDirectory { get; private set; }
            public bool Included
            {
                get
                {
                    return this.mIncluded;
                }
                set
                {
                    this.mIncluded = value;
                }
            }
            private bool mIncluded;

            public FileData(DirectoryInfo rootDirectory, FileInfo file)
            {
                this.RootDirectory = rootDirectory;
                this.File = file;
                this.Included = true;
            }

            public string RelativePath
            {
                get
                {
                    string result = this.File.FullName.Replace(this.RootDirectory.FullName, String.Empty);
                    return result;
                }
            }

            public override string ToString()
            {
                return this.RelativePath;
            }
        }
    }
}

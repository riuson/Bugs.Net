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
    internal partial class ControlSettings : UserControl
    {
        IApplication mApp;

        public ControlSettings(IApplication app)
        {
            InitializeComponent();
            this.Text = "Database settings".Tr();
            this.buttonCheckConnection.Text = this.buttonCheckConnection.Text.Tr();
            this.mApp = app;

            this.LoadSettings();
        }

        private void BeforeDisposing()
        {
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

        private void buttonCheckConnection_Click(object sender, EventArgs e)
        {
            this.richTextBoxLog.Clear();
            this.Log("Check file: ".Tr() + this.textBoxFilename.Text);

            bool result = SessionManager.Instance.Configure(new SessionOptions(this.textBoxFilename.Text)
                {
                    Log = this.Log
                });

            if (result)
            {
                var types = SessionConfiguration.GetEntityTypes();

                try
                {
                    using (ISession session = SessionManager.Instance.OpenSession(false))
                    {
                        foreach (var type in types)
                        {
                            Type generic = typeof(Repository<>);
                            Type[] typeArgs = { type };
                            Type constructed = generic.MakeGenericType(typeArgs);
                            dynamic repository = Activator.CreateInstance(constructed, session);
                            int rows = Convert.ToInt32(repository.RowCount());

                            this.Log(String.Format("Table {0}: {1} row(s)".Tr(), type.Name, rows));
                        }
                    }
                }
                catch (Exception exc)
                {
                    this.Log("Check row count failed.".Tr());
                    this.Log(exc.Message);
                    result = false;
                }
            }

            if (result)
            {
                this.Log("Success.".Tr());
                this.SaveSettings();
                this.Log("Configuration saved.".Tr());
            }
            else
            {
                this.Log("Failed.".Tr());
            }
        }

        private void Log(string message)
        {
            this.richTextBoxLog.AppendText(message);
            this.richTextBoxLog.AppendText(Environment.NewLine);
        }
    }
}

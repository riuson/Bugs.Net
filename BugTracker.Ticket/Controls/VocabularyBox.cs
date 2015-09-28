using BugTracker.Core;
using BugTracker.Core.Messages;
using BugTracker.DB.DataAccess;
using BugTracker.DB.Entities;
using BugTracker.DB.Events;
using BugTracker.TicketEditor.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.TicketEditor.Controls
{
    public partial class VocabularyBox<T> : UserControl where T : class, new()
    {
        private ICollection<T> mEntityList;
        private BindingSource mBS;
        private IApplication mApp;
        public event EventHandler DataUpdated;

        public VocabularyBox(IApplication app)
        {
            InitializeComponent();
            this.mApp = app;
            this.mBS = new BindingSource();
            this.mBS.AllowNew = true;

            this.UpdateList();
        }

        private void BeforeDisposing()
        {
        }

        private void UpdateList()
        {
            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                Repository<T> repository = new Repository<T>(session);
                this.mEntityList = repository.List();
                this.mBS.DataSource = this.mEntityList;
                this.comboBox1.DataSource = this.mBS; ;
                this.comboBox1.DisplayMember = "Value";
            }
        }

        public T SelectedValue
        {
            get
            {
                T d = this.comboBox1.SelectedItem as T;

                if (d != null)
                {
                    return d;
                }

                return default(T);
            }
            set
            {
                T d = value;

                if (this.comboBox1.Items.Contains(d))
                {
                    this.comboBox1.SelectedItem = d;
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            this.MinimumSize = new Size(0, this.comboBox1.Height);
            this.MaximumSize = new Size(0, this.comboBox1.Height);
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;

            base.OnLoad(e);
        }

        private void VocabularyUpdated()
        {
            this.UpdateList();

            if (this.DataUpdated!= null)
            {
                this.DataUpdated(this, EventArgs.Empty);
            }
        }

        private void linkLabelEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            T data = this.comboBox1.SelectedItem as T;

            if (data != null)
            {
                EntityShowEventArgs<T> ea = new EntityShowEventArgs<T>(data);
                ea.Completed += new MessageProcessCompleted(this.VocabularyUpdated);
                this.mApp.Messages.Send(this, ea);
            }
            else
            {
                EntityShowEventArgs<T> ea = new EntityShowEventArgs<T>();
                ea.Completed += new MessageProcessCompleted(this.VocabularyUpdated);
                this.mApp.Messages.Send(this, ea);
            }
        }
    }
}

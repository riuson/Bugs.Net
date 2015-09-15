using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BugTracker.DB.Interfaces;
using BugTracker.DB;
using BugTracker.DB.Classes;
using BugTracker.DB.Entities;
using BugTracker.Core.Interfaces;
using BugTracker.TicketEditor.Classes;
using BugTracker.DB.Events;

namespace BugTracker.TicketEditor.Controls
{
    public partial class VocabularyBox<T> : UserControl where T : class, new()
    {
        private ICollection<T> mEntityList;
        private IEnumerable<VocabularyDisplayData<T>> mDisplayList;
        private BindingSource mBS;
        private IApplication mApp;

        public VocabularyBox(IApplication app)
        {
            InitializeComponent();
            this.mApp = app;
            this.mBS = new BindingSource();
            this.mBS.AllowNew = true;

            this.comboBox1.DataSource = this.mBS;
            this.UpdateList();

            this.mApp.Messages.Subscribe(typeof(EntityEditedEventArgs<T>), this.VocabularyUpdated);
        }

        private void BeforeDisposing()
        {
            this.mApp.Messages.Unsubscribe(typeof(EntityEditedEventArgs<T>), this.VocabularyUpdated);
        }

        private void UpdateList()
        {
            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                Repository<T> repository = new Repository<T>(session);
                this.mEntityList = repository.List();
                this.mDisplayList = this.mEntityList.Select<T, VocabularyDisplayData<T>>(e => new VocabularyDisplayData<T>(e));
                this.mBS.DataSource = this.mDisplayList;
            }
        }

        public T SelectedValue
        {
            get
            {
                VocabularyDisplayData<T> d = this.comboBox1.SelectedItem as VocabularyDisplayData<T>;

                if (d != null)
                {
                    return d.Value;
                }

                return default(T);
            }
            set
            {
                VocabularyDisplayData<T> d = new VocabularyDisplayData<T>(value);

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

        private void VocabularyUpdated(object sender, MessageEventArgs e)
        {
            this.UpdateList();
        }

        private void linkLabelEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            VocabularyDisplayData<T> data = this.comboBox1.SelectedItem as VocabularyDisplayData<T>;

            if (data != null)
            {
                this.mApp.Messages.Send(this, new EntityShowEventArgs<T>(data.Value));
            }
        }
    }
}

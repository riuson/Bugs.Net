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
using BugTracker.Vocabulary.Events;

namespace BugTracker.Ticket.Controls
{
    public partial class VocabularyBox<T> : UserControl where T : new()
    {
        private ICollection<T> mEntityList;
        private IEnumerable<DisplayData> mDisplayList;
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

            this.mApp.Messages.Subscribe(typeof(UpdatedVocabularyEventArgs<T>), this.VocabularyUpdated);
        }

        private void BeforeDisposing()
        {
            this.mApp.Messages.Unsubscribe(typeof(UpdatedVocabularyEventArgs<T>), this.VocabularyUpdated);
        }

        private void UpdateList()
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                Repository<T> repository = new Repository<T>(session);
                this.mEntityList = repository.List();
                this.mDisplayList = this.mEntityList.Select<T, DisplayData>(e => new DisplayData(e));
                this.mBS.DataSource = this.mDisplayList;
            }
        }

        public T SelectedValue
        {
            get
            {
                return default(T);
            }
            set
            {

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
            DisplayData data = this.comboBox1.SelectedItem as DisplayData;

            if (data != null)
            {
                this.mApp.Messages.Send(this, new ShowVocabularyEditorEventArgs<T>(data.Value));
            }
        }

        private class DisplayData
        {
            public T Value { get; private set; }

            public DisplayData(T value)
            {
                this.Value = value;
            }

            public override string ToString()
            {
                IVocabulary vocabulary = (IVocabulary)this.Value;
                return vocabulary.Value;
            }
        }
    }
}

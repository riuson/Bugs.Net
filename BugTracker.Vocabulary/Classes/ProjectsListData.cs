using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using BugTracker.DB;
using BugTracker.DB.Classes;
using BugTracker.DB.Entities;
using BugTracker.DB.Interfaces;
using BugTracker.DB.Repositories;
using BugTracker.Vocabulary.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Vocabulary.Classes
{
    internal class VocabularyListData<T> : IDisposable where T : new()
    {
        private IApplication mApp;
        private ICollection<T> mInternalData;

        public BindingSource Data { get; private set; }

        public VocabularyListData(IApplication app)
        {
            this.mApp = app;
            this.Data = new BindingSource();
            this.Data.AllowNew = false;
            this.UpdateList();
        }

        public void Dispose()
        {
            this.Data.Dispose();
        }

        public void UpdateList()
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                Repository<T> repository = new Repository<T>(session);

                this.Data.DataSource = null;
                this.mInternalData = repository.List();
                this.Data.DataSource = this.mInternalData;
            }
        }

        public void Add()
        {
            AddVocabularyEventArgs<T> ea = new AddVocabularyEventArgs<T>();
            this.mApp.Messages.Send(this, ea);

            if (!ea.Processed)
            {
                string newValue;

                if (InputBox.Show("New item:", "Add item", String.Empty, out newValue) == DialogResult.OK)
                {
                    using (ISession session = SessionManager.Instance.OpenSession())
                    {
                        Repository<T> repository = new Repository<T>(session);
                        T item = new T();
                        IVocabulary v = item as IVocabulary;
                        v.Value = newValue;
                        repository.Save(item);
                    }
                    ea.Processed = true;
                }
            }

            if (ea.Processed)
            {
                this.UpdateList();
            }
        }

        public void Edit(T item)
        {
            EditVocabularyEventArgs<T> ea = new EditVocabularyEventArgs<T>(item);
            this.mApp.Messages.Send(this, ea);

            if (!ea.Processed)
            {
                string newValue;
                IVocabulary v = item as IVocabulary;

                if (InputBox.Show("Change item:", "Edit item", v.Value, out newValue) == DialogResult.OK)
                {
                    using (ISession session = SessionManager.Instance.OpenSession())
                    {
                        Repository<T> repository = new Repository<T>(session);
                        v.Value = newValue;
                        repository.SaveOrUpdate(item);
                    }
                    ea.Processed = true;
                }
            }

            if (ea.Processed)
            {
                this.UpdateList();
            }
        }

        public void Remove(T item)
        {
            RemoveVocabularyEventArgs<T> ea = new RemoveVocabularyEventArgs<T>(item);
            this.mApp.Messages.Send(this, ea);

            if (!ea.Processed)
            {
                IVocabulary v = item as IVocabulary;

                if (MessageBox.Show(
                    String.Format(
                        "Do you really want remove item '{0}'?",
                        v.Value),
                    "Remove item",
                    MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    using (ISession session = SessionManager.Instance.OpenSession())
                    {
                        Repository<T> repository = new Repository<T>(session);
                        repository.Delete(item);
                    }

                    ea.Processed = true;
                }
            }

            if (ea.Processed)
            {
                this.UpdateList();
            }
        }
    }
}

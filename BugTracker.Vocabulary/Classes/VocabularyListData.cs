using BugTracker.Core;
using BugTracker.Core.Classes;
using BugTracker.Core.Dialogs;
using BugTracker.Core.Extensions;
using BugTracker.Core.Messages;
using BugTracker.DB.DataAccess;
using BugTracker.DB.Entities;
using BugTracker.DB.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Vocabulary.Classes
{
    internal class VocabularyListData<T> : IDisposable where T : class, new()
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
            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<T> repository = new Repository<T>(session);

                this.mInternalData = repository.List();
                this.Data.DataSource = this.mInternalData;
            }
        }

        public void Add()
        {
            EntityAddEventArgs<T> ea = new EntityAddEventArgs<T>();
            this.mApp.Messages.Send(this, ea);

            if (!ea.Handled)
            {
                string newValue;

                if (InputBox.Show("New item:".Tr(), "Add item".Tr(), String.Empty, out newValue) == DialogResult.OK)
                {
                    using (ISession session = SessionManager.Instance.OpenSession(true))
                    {
                        Repository<T> repository = new Repository<T>(session);
                        T item = new T();
                        IVocabulary v = item as IVocabulary;
                        v.Value = newValue;
                        repository.Save(item);

                        session.Transaction.Commit();
                    }
                    ea.Handled = true;
                }
            }

            if (ea.Handled)
            {
                this.UpdateList();
            }
        }

        public void Edit(T item)
        {
            EntityEditEventArgs<T> ea = new EntityEditEventArgs<T>(item);
            this.mApp.Messages.Send(this, ea);

            if (!ea.Handled)
            {
                string newValue;
                IVocabulary v = item as IVocabulary;

                if (InputBox.Show("Change item:".Tr(), "Edit item".Tr(), v.Value, out newValue) == DialogResult.OK)
                {
                    using (ISession session = SessionManager.Instance.OpenSession(true))
                    {
                        Repository<T> repository = new Repository<T>(session);
                        v.Value = newValue;
                        repository.SaveOrUpdate(item);

                        session.Transaction.Commit();
                    }
                    ea.Handled = true;
                }
            }

            if (ea.Handled)
            {
                this.UpdateList();
            }
        }

        public void Remove(T item)
        {
            EntityRemoveEventArgs<T> ea = new EntityRemoveEventArgs<T>(item);
            ea.Completed += new MessageProcessCompleted(this.UpdateList);
            this.mApp.Messages.Send(this, ea);

            if (!ea.Handled)
            {
                IVocabulary v = item as IVocabulary;

                if (MessageBox.Show(
                    this.mApp.OwnerWindow,
                    String.Format(
                        "Do you really want remove item '{0}'?".Tr(),
                        v.Value),
                    "Remove item".Tr(),
                    MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    try
                    {
                        using (ISession session = SessionManager.Instance.OpenSession(true))
                        {
                            Repository<T> repository = new Repository<T>(session);
                            repository.Delete(item);

                            session.Transaction.Commit();
                        }
                    }
                    catch(Exception exc)
                    {
                        BugTracker.Core.Exceptions.Handler.Handle(exc);
                    }

                    ea.Handled = true;
                    ea.Completed();
                }
            }
        }
    }
}

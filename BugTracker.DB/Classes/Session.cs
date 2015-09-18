using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Classes
{
    internal class Session : BugTracker.DB.Interfaces.ISession
    {
        private Transaction mTransaction;

        public NHibernate.ISession NHSession { get; private set; }
        public NHibernate.ITransaction NHTransaction { get; private set; }
        public bool IsUseTransaction { get { return this.NHTransaction != null; } }
        public BugTracker.DB.Interfaces.ITransaction Transaction { get { return this.mTransaction; } }

        public Session(NHibernate.ISession session, bool beginTransaction)
        {
            this.NHSession = session;

            if (beginTransaction)
            {
                this.NHTransaction = this.NHSession.BeginTransaction();
                this.mTransaction = new Transaction();
            }
            else
            {
                this.NHTransaction = null;
                this.mTransaction = null;
            }
        }

        public void Dispose()
        {
            if (this.IsUseTransaction)
            {
                try
                {
                    if (this.mTransaction.State == Classes.Transaction.TransactionState.Confirmed)
                    {
                        this.NHTransaction.Commit();
                    }
                    else
                    {
                        this.NHTransaction.Rollback();
                    }
                }
                catch (Exception exc)
                {
                    System.Diagnostics.Debug.WriteLine(String.Format(
                        "Exception occured on transaction commit:{0}{1}{0}{2}{0}{3}",
                        Environment.NewLine,
                        exc.Source,
                        exc.Message,
                        exc.StackTrace));
                    this.NHTransaction.Rollback();
                    throw exc;
                }
            }

            this.NHSession.Dispose();
        }
    }
}

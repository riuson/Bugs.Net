using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.DataAccess
{
    internal class Session : BugTracker.DB.DataAccess.ISession
    {
        private Transaction mTransaction;
        private NHibernate.ITransaction NHTransaction { get; set; }
        private bool IsTransactionUsed { get { return this.NHTransaction != null; } }

        public NHibernate.ISession NHSession { get; private set; }
        public BugTracker.DB.DataAccess.ITransaction Transaction { get { return this.mTransaction; } }

        public Session(NHibernate.ISession session, bool beginTransaction)
        {
            this.NHSession = session;

            if (beginTransaction)
            {
                this.NHTransaction = this.NHSession.BeginTransaction();
                this.mTransaction = new Transaction(this.NHTransaction);
            }
            else
            {
                this.NHTransaction = null;
                this.mTransaction = null;
            }
        }

        public void Dispose()
        {
            if (this.IsTransactionUsed)
            {
                try
                {
                    if (!this.NHTransaction.WasCommitted && !this.NHTransaction.WasRolledBack)
                    {
                        this.NHTransaction.Rollback();
                    }
                }
                catch (Exception exc)
                {
                    System.Diagnostics.Debug.WriteLine(String.Format(
                        "Exception occured on session close with transaction : {0}{1}{0}{2}{0}{3}",
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

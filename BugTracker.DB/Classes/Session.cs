using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Classes
{
    internal class Session : BugTracker.DB.Interfaces.ISession
    {
        public NHibernate.ISession NHSession { get; private set; }
        public NHibernate.ITransaction NHTransaction { get; private set; }
        public bool IsUseTransaction { get { return this.NHTransaction != null; } }

        public Session(NHibernate.ISession session, bool beginTransaction)
        {
            this.NHSession = session;

            if (beginTransaction)
            {
                this.NHTransaction = this.NHSession.BeginTransaction();
            }
            else
            {
                this.NHTransaction = null;
            }
        }

        public void Dispose()
        {
            if (this.IsUseTransaction)
            {
                try
                {
                    this.NHTransaction.Commit();
                }
                catch // (Exception exc)
                {
                    this.NHTransaction.Rollback();
                }
            }

            this.NHSession.Dispose();
        }
    }
}

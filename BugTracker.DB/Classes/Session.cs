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

        public Session(NHibernate.ISession session)
        {
            this.NHSession = session;
            this.NHTransaction = this.NHSession.BeginTransaction();
        }

        public void Dispose()
        {
            this.NHTransaction.Commit();
            this.NHSession.Dispose();
        }
    }
}

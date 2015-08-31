using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Classes
{
    internal class Session : BugTracker.DB.Interfaces.ISession
    {
        private NHibernate.ISession mSession;

        public Session(NHibernate.ISession session)
        {
            this.mSession = session;
        }

        public void Dispose()
        {
            this.mSession.Dispose();
        }
    }
}

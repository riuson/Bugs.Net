using BugTracker.Core.Classes;
using BugTracker.DB.Classes;
using BugTracker.DB.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.DB
{
    /// <summary>
    /// Database class
    /// </summary>
    public class SessionManager : ISessionManager
    {
        /// <summary>
        /// Creator
        /// </summary>
        private sealed class SessionManagerCreator
        {
            /// <summary>
            /// Lazy initializer
            /// </summary>
            public static SessionManager Instance
            {
                get { return mInstance; }
            }
            static readonly SessionManager mInstance = new SessionManager();
        }

        /// <summary>
        /// Private constructor
        /// </summary>
        private SessionManager()
        {
            this.mSessionManagerPrivate = new BugTracker.DB.Dao.SessionManagerPrivate();
        }

        /// <summary>
        /// Singletone instance of Database object
        /// </summary>
        public static SessionManager Instance
        {
            get { return SessionManagerCreator.Instance; }
        }

        public bool Configure(string filename)
        {
            return this.mSessionManagerPrivate.Configure(filename);
        }

        public ISession OpenSession()
        {
            return this.mSessionManagerPrivate.OpenSession();
        }

        public bool IsConfigured
        {
            get { return this.mSessionManagerPrivate.IsConfigured; }
        }

        private BugTracker.DB.Dao.SessionManagerPrivate mSessionManagerPrivate;
    }
}

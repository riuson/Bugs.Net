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
            this.mSessionManagerInternal = new SessionManagerInternal();
        }

        /// <summary>
        /// Singletone instance of Database object
        /// </summary>
        public static SessionManager Instance
        {
            get { return SessionManagerCreator.Instance; }
        }

        public ISession OpenSession()
        {
            return this.mSessionManagerInternal.OpenSession();
        }

        public bool IsConfigured
        {
            get { return this.mSessionManagerInternal.IsConfigured; }
        }

        private SessionManagerInternal mSessionManagerInternal;
    }
}

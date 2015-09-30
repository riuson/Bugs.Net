using BugTracker.Core.Classes;
using BugTracker.DB.Classes;
using BugTracker.DB.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.DB.DataAccess
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
            this.mSessionManagerPrivate = new SessionManagerPrivate();
        }

        /// <summary>
        /// Singletone instance of Database object
        /// </summary>
        public static SessionManager Instance
        {
            get { return SessionManagerCreator.Instance; }
        }

        public bool Configure(SessionOptions sessionOptions)
        {
            return this.mSessionManagerPrivate.Configure(sessionOptions);
        }

        public ISession OpenSession(bool beginTransaction)
        {
            if (!this.IsConfigured)
            {
                this.Configure(new SessionOptions(Saved<Options>.Instance.FileName));
            }

            return this.mSessionManagerPrivate.OpenSession(beginTransaction);
        }

        public bool IsConfigured
        {
            get { return this.mSessionManagerPrivate.IsConfigured; }
        }

        private SessionManagerPrivate mSessionManagerPrivate;
    }
}

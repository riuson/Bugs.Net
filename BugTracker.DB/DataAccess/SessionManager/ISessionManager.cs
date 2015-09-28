using BugTracker.DB.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.DataAccess
{
    public interface ISessionManager
    {
        /// <summary>
        /// Configure use specified database file
        /// </summary>
        /// <param name="sessopnOptions">Options for session</param>
        /// <returns>Is successfully configured</returns>
        bool Configure(SessionOptions sessionOptions);
        /// <summary>
        /// Open session with transaction
        /// </summary>
        /// <param name="useTransaction">Begin transaction or not. For write operations transaction should be used.</param>
        /// <returns>Session</returns>
        ISession OpenSession(bool beginTransaction);
        /// <summary>
        /// Is session manager successfully configured
        /// </summary>
        bool IsConfigured { get; }
    }
}

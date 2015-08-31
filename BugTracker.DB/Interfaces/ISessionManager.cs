using BugTracker.DB.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Interfaces
{
    public interface ISessionManager
    {
        ISession OpenSession();
        bool IsConfigured { get; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Interfaces
{
    public interface ISession : IDisposable
    {
        ITransaction Transaction { get; }
    }
}

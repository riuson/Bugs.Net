using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Migrations
{
    internal interface IMigrationPart
    {
        int Version { get; }
    }
}

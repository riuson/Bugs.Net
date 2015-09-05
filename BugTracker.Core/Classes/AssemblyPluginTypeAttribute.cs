using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.Core.Classes
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class AssemblyPluginTypeAttribute : Attribute
    {
        public Type PluginType { get; private set; }

        public AssemblyPluginTypeAttribute(Type value) { this.PluginType = value; }
    }
}

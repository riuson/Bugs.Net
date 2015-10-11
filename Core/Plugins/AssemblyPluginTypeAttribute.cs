using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppCore.Plugins
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class AssemblyPluginTypeAttribute : Attribute
    {
        public Type PluginType { get; private set; }

        public AssemblyPluginTypeAttribute(Type value) { this.PluginType = value; }
    }
}

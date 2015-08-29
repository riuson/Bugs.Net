using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Core.Classes
{
    public class ControlContainer : IContainer
    {
        private ComponentCollection mComponents;

        public ControlContainer()
        {
            this.mComponents = new ComponentCollection(new IComponent[] { });
        }

        public void Add(IComponent component)
        { }

        public void Add(IComponent component, string Name)
        { }

        public void Remove(IComponent component)
        { }

        public ComponentCollection Components
        {
            get { return this.mComponents; }
        }

        public void Dispose()
        {
            this.mComponents = null;
        }
    }

}

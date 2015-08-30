using BugTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Classes
{
    internal class ControlManager : IControlManager
    {
        private Stack<Control> mControlStack;
        private Control mCurrentControl;

        public ControlManager()
        {
            this.mControlStack = new Stack<Control>();
            this.mCurrentControl = null;
        }

        public void Show(Control ctrl)
        {
            this.mControlStack.Push(ctrl);

            if (this.ControlHide != null && this.mCurrentControl != null)
            {
                this.ControlHide(this, new ControlChangeEventArgs(this.mCurrentControl));
            }

            this.mCurrentControl = ctrl;

            if (this.ControlShow != null)
            {
                this.ControlShow(this, new ControlChangeEventArgs(ctrl));
            }
        }

        public void Hide(Control ctrl)
        {
            if (this.ControlHide != null && ctrl != null)
            {
                this.ControlHide(this, new ControlChangeEventArgs(ctrl));
            }

            Control previous = this.mControlStack.Pop();
            this.mCurrentControl = previous;

            if (this.ControlShow != null && previous != null)
            {
                this.ControlShow(this, new ControlChangeEventArgs(previous));
            }
        }

        public event EventHandler<ControlChangeEventArgs> ControlHide;
        public event EventHandler<ControlChangeEventArgs> ControlShow;

        internal class ControlChangeEventArgs : EventArgs
        {
            public Control Control { get; private set; }

            public ControlChangeEventArgs(Control ctrl)
            {
                this.Control = ctrl;
            }
        }
    }
}

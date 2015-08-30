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
            if (this.mCurrentControl != null)
            {
                this.mControlStack.Push(this.mCurrentControl);

                if (this.ControlHide != null)
                {
                    this.ControlHide(this, new ControlChangeEventArgs(this.mCurrentControl));
                }
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

            ctrl.Dispose();

            Control previous = this.mControlStack.Pop();
            this.mCurrentControl = previous;

            if (this.ControlShow != null && previous != null)
            {
                this.ControlShow(this, new ControlChangeEventArgs(previous));
            }
        }

        public void Hide(int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                this.Hide();
            }
        }

        public void Hide()
        {
            this.Hide(this.mCurrentControl);
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

        public IEnumerable<string> Titles
        {
            get
            {
                IEnumerable<String> list = this.mControlStack.Select<Control, string>(delegate(Control ctrl)
                {
                    return ctrl.Text;
                });

                list = list.Reverse();

                List<string> result = new List<string>(list);
                result.Add(this.mCurrentControl.Text);

                return result;
            }
        }
    }
}

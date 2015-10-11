using AppCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AppCore.Controls
{
    internal class ControlManager : IControlManager
    {
        private List<Control> mControls;

        public ControlManager()
        {
            this.mControls = new List<Control>();
        }

        public void Show(Control ctrl)
        {
            if (this.mControls.Count > 0)
            {
                Control previousControl = this.mControls.Last();

                if (this.ControlHide != null)
                {
                    this.ControlHide(this, new ControlChangeEventArgs(previousControl));
                }
            }

            if (ctrl != null)
            {
                this.mControls.Add(ctrl);

                if (this.ControlShow != null)
                {
                    this.ControlShow(this, new ControlChangeEventArgs(ctrl));
                }
            }
        }

        public void Hide(Control ctrl)
        {
            if (this.mControls.Count > 0)
            {
                Control lastControl = this.mControls.Last();

                if (lastControl == ctrl)
                {
                    if (this.ControlHide != null)
                    {
                        this.ControlHide(this, new ControlChangeEventArgs(ctrl));
                    }

                    this.mControls.Remove(ctrl);
                    ctrl.Dispose();
                }
                else
                {
                    throw new ArgumentException("Cannot hide not last control.".Tr());
                }
            }
            else
            {
                throw new ArgumentException("Nothing to hide.".Tr());
            }

            if (this.mControls.Count > 0)
            {
                Control previousControl = this.mControls.Last();

                if (this.ControlShow != null)
                {
                    this.ControlShow(this, new ControlChangeEventArgs(previousControl));
                }
            }
        }

        public void Hide(int steps)
        {
            if (steps > this.mControls.Count)
            {
                throw new ArgumentOutOfRangeException(String.Format("{0} step(s) specified, but {1} control(s) saved.".Tr(), steps, this.mControls.Count));
            }

            for (int i = 0; i < steps; i++)
            {
                this.Hide();
            }
        }

        public void Hide()
        {
            if (this.mControls.Count == 0)
            {
                throw new ArgumentOutOfRangeException("Cannot hide, 0 controls in stack.".Tr());
            }

            this.Hide(this.mControls.Last());
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
                var list = from ctrl in this.mControls
                           select ctrl.Text;

                return list;
            }
        }
    }
}

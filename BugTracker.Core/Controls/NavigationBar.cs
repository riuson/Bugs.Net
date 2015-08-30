using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Controls
{
    internal class NavigationBar : FlowLayoutPanel
    {
        public NavigationBar()
        {

        }

        public void UpdateTitles(IEnumerable<string> titles)
        {
            this.Controls.Clear();

            if (titles.Count() > 1)
            {
                for (int i = 0; i < titles.Count(); i++)
                {
                    string title = titles.ElementAt(i);

                    if (i < titles.Count() - 1)
                    {
                        LinkLabel label = new LinkLabel();
                        label.AutoSize = true;
                        label.Click += this.label_Click;
                        label.Tag = titles.Count() - i - 1;
                        label.Text = title;
                        this.Controls.Add(label);
                    }
                    else
                    {
                        Label label = new Label();
                        label.AutoSize = true;
                        label.Click += this.label_Click;
                        label.Tag = titles.Count() - i - 1;
                        label.Text = title;
                        this.Controls.Add(label);
                    }
                }

                LinkLabel labelBack = new LinkLabel();
                labelBack.AutoSize = true;
                labelBack.Click += this.label_Click;
                labelBack.Tag = 1;
                labelBack.Text = "Back";
                this.Controls.Add(labelBack);
            }
        }

        private void label_Click(object sender, EventArgs e)
        {
            if (this.Navigate != null)
            {
                int steps = Convert.ToInt32((sender as LinkLabel).Tag);

                this.Navigate(this, new NavigateEventArgs(steps));
            }
        }

        public event EventHandler<NavigateEventArgs> Navigate;

        public class NavigateEventArgs : EventArgs
        {
            public int Steps { get; private set; }

            public NavigateEventArgs(int steps)
            {
                this.Steps = steps;
            }
        }
    }
}

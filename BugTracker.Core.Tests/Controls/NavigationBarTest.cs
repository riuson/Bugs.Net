using BugTracker.Core.Controls;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugTracker.Core.Tests.Controls
{
    [TestFixture]
    internal class NavigationBarTest
    {
        private class NavigationBarEx : NavigationBar
        {
            public void ClickOnLabel(Label label)
            {
                base.label_Click(label, EventArgs.Empty);
            }
            public int Count { get { return this.Controls.Count; } }
            public IEnumerable<string> Titles
            {
                get
                {
                    var result = from Control ctrl in this.Controls
                                 let label = ctrl as Label
                                 where label != null
                                 select label.Text;
                    return result;
                }
            }
            public Label BackLink
            {
                get
                {
                    var result = (from Control ctrl in this.Controls
                                  where ctrl is Label
                                  select ctrl as Label).LastOrDefault();
                    return result;
                }
            }
            public IEnumerable<Label> Links
            {
                get
                {
                    var result = from Control ctrl in this.Controls
                                 where ctrl is Label
                                 select ctrl as Label;
                    return result;
                }
            }
        }

        [Test]
        public void CanGenerateLabels()
        {
            NavigationBarEx navbar = new NavigationBarEx();

            string[] titles = new string[] { "#3", "#2", "#1" };

            navbar.UpdateTitles(titles);

            // Titles + 'Back'
            Assert.That(navbar.Count, Is.EqualTo(titles.Length + 1));

            titles = new string[] { };

            navbar.UpdateTitles(titles);

            // Titles + 'Back'
            Assert.That(navbar.Count, Is.EqualTo(0));
        }

        [Test]
        public void CanGenerateLabelsText()
        {
            NavigationBarEx navbar = new NavigationBarEx();

            IEnumerable<string> titles = new string[] { "#3", "#2", "#1" };

            navbar.UpdateTitles(titles);
            IEnumerable<string> titles2 = navbar.Titles;
            IEnumerable<string> diff = titles2.Except(titles);

            Assert.That(diff.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CanClickLinks()
        {
            int steps = 0;

            NavigationBarEx navbar = new NavigationBarEx();
            navbar.Navigate += delegate(object sender, NavigationBar.NavigateEventArgs e)
            {
                Assert.That(e.Steps, Is.EqualTo(steps));
            };

            string[] titles = new string[] { "#3", "#2", "#1" };

            navbar.UpdateTitles(titles);

            IEnumerable<Label> links = navbar.Links;

            Assert.That(links.Count(), Is.EqualTo(titles.Count() + 1));

            steps = 2;
            navbar.ClickOnLabel(links.ElementAt(0));

            steps = 1;
            navbar.ClickOnLabel(links.ElementAt(1));

            steps = 0;
            navbar.ClickOnLabel(links.ElementAt(2));

            steps = 1;
            navbar.ClickOnLabel(navbar.BackLink);
        }
    }
}

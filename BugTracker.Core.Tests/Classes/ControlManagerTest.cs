using BugTracker.Core.Classes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugTracker.Core.Tests.Classes
{
    [TestFixture]
    internal class ControlManagerTest
    {
        [Test]
        public void CanAdd()
        {
            ControlManager cm = new ControlManager();

            for (int i = 0; i < 5; i++)
            {
                cm.Show(new Control(String.Format("Control #{0}", i)));
            }

            Assert.That(cm.Titles.Count(), Is.EqualTo(5));
        }

        [Test]
        public void CanRemove()
        {
            ControlManager cm = new ControlManager();

            for (int i = 0; i < 5; i++)
            {
                cm.Show(new Control(String.Format("Control #{0}", i)));
            }

            for (int i = 0; i < 5; i++)
            {
                cm.Hide();
            }

            Assert.That(cm.Titles.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CanRemoveByRef()
        {
            ControlManager cm = new ControlManager();

            Control ctrl1 = new Control("1");
            Control ctrl2 = new Control("2");
            Control ctrl3 = new Control("3");

            cm.Show(ctrl1);
            cm.Show(ctrl2);
            cm.Show(ctrl3);

            Assert.That(cm.Titles.Count(), Is.EqualTo(3));

            cm.Hide(ctrl3);
            cm.Hide(ctrl2);
            cm.Hide(ctrl1);

            Assert.That(cm.Titles.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CanRemoveOnlyByOrder()
        {
            ControlManager cm = new ControlManager();

            Control ctrl1 = new Control("1");
            Control ctrl2 = new Control("2");
            Control ctrl3 = new Control("3");

            cm.Show(ctrl1);
            cm.Show(ctrl2);
            cm.Show(ctrl3);

            Assert.That(delegate()
                {
                    cm.Hide(ctrl3);
                    cm.Hide(ctrl1);
                    cm.Hide(ctrl2);
                },
                Throws.Exception.TypeOf<ArgumentException>()
            );
        }

        [Test]
        public void CanRemoveByNumber()
        {
            ControlManager cm = new ControlManager();

            Control ctrl1 = new Control("1");
            Control ctrl2 = new Control("2");
            Control ctrl3 = new Control("3");

            cm.Show(ctrl1);
            cm.Show(ctrl2);
            cm.Show(ctrl3);

            Assert.That(cm.Titles.Count(), Is.EqualTo(3));

            cm.Hide(3);

            Assert.That(cm.Titles.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CannotOverRemove()
        {
            ControlManager cm = new ControlManager();

            Control ctrl1 = new Control("1");
            Control ctrl2 = new Control("2");
            Control ctrl3 = new Control("3");

            cm.Show(ctrl1);
            cm.Show(ctrl2);
            cm.Show(ctrl3);

            Assert.That(cm.Titles.Count(), Is.EqualTo(3));

            Assert.That(delegate()
                {
                    cm.Hide(4);
                },
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>()
            );
        }

        [Test]
        public void CanCollectTitles()
        {
            ControlManager cm = new ControlManager();

            Control ctrl1 = new Control("#1");
            Control ctrl2 = new Control("#2");
            Control ctrl3 = new Control("#3");

            cm.Show(ctrl1);
            cm.Show(ctrl2);
            cm.Show(ctrl3);

            Assert.That(cm.Titles.Count(), Is.EqualTo(3));

            string[] expected = new string[] { "#1", "#2", "#3" };

            Assert.That(cm.Titles, Is.EqualTo(expected));
        }

        [Test]
        public void CanEmitEvents()
        {
            ControlManager cm = new ControlManager();
            int countShow = 0;
            cm.ControlShow += delegate(object sender, ControlManager.ControlChangeEventArgs e)
            {
                countShow++;
            };

            for (int i = 0; i < 5; i++)
            {
                cm.Show(new Control(String.Format("Control #{0}", i)));
            }

            Assert.That(countShow, Is.EqualTo(5));

            int countHide = 0;
            cm.ControlHide += delegate(object sender, ControlManager.ControlChangeEventArgs e)
            {
                countHide++;
            };

            for (int i = 0; i < 5; i++)
            {
                cm.Hide();
            }

            Assert.That(countHide, Is.EqualTo(5));
        }
    }
}

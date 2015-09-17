using BugTracker.Core.Classes;
using BugTracker.Core.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Core.Tests.Classes
{
    [TestFixture]
    internal class MessageCenterTest
    {
        private int mValue1;
        private int mValue2;

        public MessageCenterTest()
        {
            this.mValue1 = 0;
            this.mValue2 = 0;
        }

        [Test]
        public void CanCreate()
        {
            Assert.That(
                delegate()
                {
                    MessageCenter mc = new MessageCenter();
                },
                Throws.Nothing
            );
        }

        [Test]
        public void CanSend()
        {
            Assert.That(
                delegate()
                {
                    MessageCenter mc = new MessageCenter();
                    mc.Send(this, new MessageEventArgs());
                },
                Throws.Nothing
            );
        }

        [Test]
        public void CanSubscribe()
        {
            MessageCenter mc = new MessageCenter();
            mc.Subscribe(typeof(MessageEventArgs), this.Callback);

            MessageEventArgs ea = new MessageEventArgs();
            ea.Handled = false;

            this.mValue1 = 0;
            mc.Send(this, ea);

            Assert.That(ea.Handled, Is.True);
            Assert.That(this.mValue1, Is.EqualTo(1));
        }

        [Test]
        public void CanUnSubscribe()
        {
            MessageCenter mc = new MessageCenter();
            mc.Subscribe(typeof(MessageEventArgs), this.Callback);

            MessageEventArgs ea = new MessageEventArgs();
            ea.Handled = false;

            this.mValue1 = 0;
            mc.Send(this, ea);

            mc.Unsubscribe(typeof(MessageEventArgs), this.Callback);

            ea.Handled = false;
            this.mValue1 = 0;
            mc.Send(this, ea);

            Assert.That(ea.Handled, Is.False);
            Assert.That(this.mValue1, Is.EqualTo(0));
        }

        [Test]
        public void CanSubscribeInherited()
        {
            MessageCenter mc = new MessageCenter();
            mc.Subscribe(typeof(MessageEventArgsInherited), this.CallbackInherited);

            MessageEventArgsInherited ea = new MessageEventArgsInherited();
            ea.Handled = false;
            ea.Value = 0;

            this.mValue2 = 0;
            mc.Send(this, ea);

            Assert.That(ea.Handled, Is.True);
            Assert.That(this.mValue2, Is.EqualTo(2));
        }

        [Test]
        public void CanUnSubscribeInherited()
        {
            MessageCenter mc = new MessageCenter();
            mc.Subscribe(typeof(MessageEventArgsInherited), this.CallbackInherited);

            MessageEventArgsInherited ea = new MessageEventArgsInherited();
            ea.Handled = false;

            this.mValue2 = 0;
            mc.Send(this, ea);

            mc.Unsubscribe(typeof(MessageEventArgsInherited), this.CallbackInherited);

            ea.Handled = false;
            this.mValue2 = 0;
            mc.Send(this, ea);

            Assert.That(ea.Handled, Is.False);
            Assert.That(this.mValue2, Is.EqualTo(0));
        }

        [Test]
        public void CanSelectCallback()
        {
            MessageCenter mc = new MessageCenter();
            mc.Subscribe(typeof(MessageEventArgs), this.Callback);
            mc.Subscribe(typeof(MessageEventArgsInherited), this.CallbackInherited);

            MessageEventArgs ea = new MessageEventArgs();
            ea.Handled = false;
            
            MessageEventArgsInherited ea2 = new MessageEventArgsInherited();
            ea2.Handled = false;
            ea2.Value = 0;

            this.mValue1 = 0;
            this.mValue2 = 0;
            mc.Send(this, ea);

            Assert.That(ea.Handled, Is.True);
            Assert.That(this.mValue1, Is.EqualTo(1));
            Assert.That(this.mValue2, Is.EqualTo(0));

            this.mValue1 = 0;
            this.mValue2 = 0;
            mc.Send(this, ea2);

            Assert.That(ea2.Handled, Is.True);
            Assert.That(this.mValue1, Is.EqualTo(0));
            Assert.That(this.mValue2, Is.EqualTo(2));
        }

        [Test]
        public void CanCallback()
        {
            MessageCenter mc = new MessageCenter();
            mc.Subscribe(typeof(MessageEventArgs), this.MessageCallbackTest);

            MessageEventArgs ea = new MessageEventArgs();
            ea.Callback += this.MessageCallBack;
            ea.Handled = false;

            this.mValue1 = 0;
            mc.Send(this, ea);

            Assert.That(ea.Handled, Is.True);
            Assert.That(this.mValue1, Is.EqualTo(1));
        }

        private void Callback(object sender, MessageEventArgs ea)
        {
            this.mValue1++;
            ea.Handled = true;
        }

        private void CallbackInherited(object sender, MessageEventArgs ea)
        {
            this.mValue2 += 2;
            ea.Handled = true;
        }

        private void MessageCallbackTest(object sender, MessageEventArgs ea)
        {
            ea.Callback();
            ea.Handled = true;
        }

        private void MessageCallBack()
        {
            this.mValue1++;
        }

        private class MessageEventArgsInherited : MessageEventArgs
        {
            public int Value { get; set; }

            public MessageEventArgsInherited()
            {
                this.Value = 31415926;
            }
        }
    }
}

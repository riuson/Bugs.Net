using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.Core.Messages
{
    public interface IMessageCenter
    {
        void Subscribe(Type eventArgsType, EventHandler<MessageEventArgs> d);
        void Unsubscribe(Type eventArgsType, EventHandler<MessageEventArgs> d);
        void Send(object sender, MessageEventArgs ea);
    }

    public delegate void MessageProcessCompleted();

    public class MessageEventArgs : EventArgs
    {
        public bool Handled { get; set; }
        public MessageProcessCompleted Completed { get; set; }

        public MessageEventArgs()
        {
            this.Handled = false;
            this.Completed = new MessageProcessCompleted(this.CallbackStub);
        }

        private void CallbackStub()
        {
        }
    }
}

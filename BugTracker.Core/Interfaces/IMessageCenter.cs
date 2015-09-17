using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.Core.Interfaces
{
    public interface IMessageCenter
    {
        void Subscribe(Type eventArgsType, EventHandler<MessageEventArgs> d);
        void Unsubscribe(Type eventArgsType, EventHandler<MessageEventArgs> d);
        void Send(object sender, MessageEventArgs ea);
    }

    public delegate void MessageCallback();

    public class MessageEventArgs : EventArgs
    {
        public bool Processed { get; set; }
        public MessageCallback Callback { get; set; }

        public MessageEventArgs()
        {
            this.Processed = false;
            this.Callback = null;
        }
    }
}

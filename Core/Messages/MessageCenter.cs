using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AppCore.Messages
{
    internal class MessageCenter : IMessageCenter
    {
        private class HandlerData : IEquatable<HandlerData>
        {
            public HandlerData(Type eventArgsType, EventHandler<MessageEventArgs> func)
            {
                this.EventArgsType = eventArgsType;
                this.Func = func;
            }

            public Type EventArgsType { get; private set; }
            public EventHandler<MessageEventArgs> Func { get; private set; }

            public bool Equals(HandlerData other)
            {
                if (ReferenceEquals(null, other))
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                if (GetType() != other.GetType())
                {
                    return false;
                }

                if (other.EventArgsType == this.EventArgsType)
                {
                    if (other.Func == this.Func)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private List<HandlerData> mHandlers;

        public MessageCenter()
        {
            this.mHandlers = new List<HandlerData>();
        }

        public void Subscribe(Type eventArgsType, EventHandler<MessageEventArgs> func)
        {
            this.mHandlers.Add(new HandlerData(eventArgsType, func));
        }

        public void Unsubscribe(Type eventArgsType, EventHandler<MessageEventArgs> func)
        {
            this.mHandlers.Remove(new HandlerData(eventArgsType, func));
        }

        public void Send(object sender, MessageEventArgs ea)
        {
            foreach (var handler in this.mHandlers)
            {
                if (ea.GetType() == handler.EventArgsType)
                {
                    handler.Func(sender, ea);

                    if (ea.Handled)
                    {
                        break;
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Entities
{
    public abstract class Entity<T>
    {
        public virtual T Id { get; protected set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Entity<T>);
        }

        private static bool IsTransient(Entity<T> obj)
        {
            return obj != null && Equals(obj.Id, default(T));
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }

        public virtual bool Equals(Entity<T> other)
        {
            if (other == null) return false;

            if (ReferenceEquals(this, other)) return true;

            if (!IsTransient(this) && !IsTransient(this) && Equals(Id, other.Id))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();

                bool result = thisType.IsAssignableFrom(otherType) ||
                   otherType.IsAssignableFrom(thisType);

                return result;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (this.Id.GetHashCode() * 31415926) & GetType().GetHashCode();
        }

        public static bool operator ==(Entity<T> left, Entity<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity<T> left, Entity<T> right)
        {
            return !Equals(left, right);
        }
    }
}

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
            Console.WriteLine("check other == null");
            if (other == null) return false;

            Console.WriteLine("check reference equals");
            if (ReferenceEquals(this, other)) return true;

            Console.WriteLine("check transient");
            if (!IsTransient(this) && !IsTransient(this) && Equals(Id, other.Id))
            {
                Console.WriteLine("get unproxied types");
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();

                bool result = thisType.IsAssignableFrom(otherType) ||
                   otherType.IsAssignableFrom(thisType);

                Console.WriteLine(String.Format("this {0} and other {1}",
                    thisType,
                    otherType));
                Console.WriteLine("result: " + result);

                return result;
            }

            Console.WriteLine("return false");
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

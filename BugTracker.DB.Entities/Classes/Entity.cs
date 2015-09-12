﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Entities
{
    public abstract class Entity : IEquatable<Entity>
    {
        public virtual long Id { get; protected set; }

        public override bool Equals(object obj)
        {
            if (obj is Entity)
            {
                return this.Equals(obj as Entity);
            }

            return false;
        }

        public virtual bool Equals(Entity other)
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

            return other.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return (this.Id.GetHashCode() * 31415926) & GetType().GetHashCode();
        }

        public static bool operator ==(Entity left, Entity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !Equals(left, right);
        }
    }
}

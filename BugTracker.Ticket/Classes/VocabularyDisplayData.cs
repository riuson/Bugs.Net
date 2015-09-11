using BugTracker.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.TicketEditor.Classes
{
    public class VocabularyDisplayData<T> : IEquatable<VocabularyDisplayData<T>> where T : new()
    {
        public T Value { get; private set; }

        public VocabularyDisplayData(T value)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            IVocabulary vocabulary = (IVocabulary)this.Value;
            return vocabulary.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is VocabularyDisplayData<T>)
            {
                return this.Equals(obj as VocabularyDisplayData<T>);
            }

            return false;
        }

        public virtual bool Equals(VocabularyDisplayData<T> other)
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

            return (other.Value as Entity).Id == (this.Value as Entity).Id;
        }

        public override int GetHashCode()
        {
            return ((this.Value as Entity).Id.GetHashCode() * 31415926) & GetType().GetHashCode();
        }

        public static bool operator ==(VocabularyDisplayData<T> left, VocabularyDisplayData<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(VocabularyDisplayData<T> left, VocabularyDisplayData<T> right)
        {
            return !Equals(left, right);
        }
    }
}

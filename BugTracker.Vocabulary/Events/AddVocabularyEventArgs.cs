using BugTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.Vocabulary.Events
{
    public class AddVocabularyEventArgs<T> : MessageEventArgs where T : new()
    {
    }
}

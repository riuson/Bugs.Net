using BugTracker.Core.Interfaces;
using BugTracker.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.Vocabulary.Events
{
    public class UpdatedVocabularyEventArgs<T> : MessageEventArgs where T : new()
    {
    }
}

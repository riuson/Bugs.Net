﻿using BugTracker.Core.Interfaces;
using BugTracker.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.Vocabulary.Events
{
    public class EditVocabularyEventArgs<T> : MessageEventArgs where T : new()
    {
        public T Item { get; private set; }

        public EditVocabularyEventArgs(T item)
        {
            this.Item = item;
        }
    }
}

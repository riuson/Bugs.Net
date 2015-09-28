using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.DataAccess
{
    internal class Transaction : ITransaction
    {
        public enum TransactionState
        {
            None,
            Confirmed,
            Cancelled
        }

        public TransactionState State { get; private set; }

        public Transaction()
        {
            this.State = Transaction.TransactionState.None;
        }

        public void Commit()
        {
            this.State = Transaction.TransactionState.Confirmed;
        }

        public void Rollback()
        {
            this.State = Transaction.TransactionState.Cancelled;
        }
    }
}

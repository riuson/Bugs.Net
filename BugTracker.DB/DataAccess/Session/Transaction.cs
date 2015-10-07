using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.DataAccess
{
    internal class Transaction : ITransaction
    {
        private NHibernate.ITransaction mTransaction;

        public Transaction(NHibernate.ITransaction transaction)
        {
            this.mTransaction = transaction;
        }

        public void Commit()
        {
            this.mTransaction.Commit();
        }

        public void Rollback()
        {
            this.mTransaction.Rollback();
        }
    }
}

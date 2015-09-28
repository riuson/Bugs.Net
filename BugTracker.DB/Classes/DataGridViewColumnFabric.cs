using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.DB.Classes
{
    public static class DataGridViewColumnFabric
    {
        public enum ColumnType
        {
            SubColumn
        }

        public static DataGridViewColumn CreateSubColumn(ColumnType columnType)
        {
            DataGridViewColumn result = null;

            switch (columnType)
            {
                case ColumnType.SubColumn:
                    result = new DataGridViewSubColumn();
                    break;
                default:
                    result = new DataGridViewTextBoxColumn();
                    break;
            }

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.DB.Classes
{
    internal class DataGridViewSubColumn : DataGridViewColumn
    {
        public DataGridViewSubColumn()
            : base(new DataGridViewSubCell())
        {
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                /*if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(DataGridViewSubCell)))
                {
                    throw new InvalidCastException("Must be a DataGridViewVocabularyCell");
                }*/
                if ((value == null) || !(value is DataGridViewSubCell))
                {
                    throw new InvalidCastException("Must be a DataGridViewVocabularyCell");
                }

                base.CellTemplate = value;
            }
        }
    }

    internal class DataGridViewSubCell : DataGridViewTextBoxCell
    {
        public DataGridViewSubCell()
            : base()
        {
        }

        public override Type ValueType
        {
            get
            {
                return typeof(String);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                return String.Empty;
            }
        }

        protected override object GetValue(int rowIndex)
        {
            try
            {
                if (rowIndex >= 0 && rowIndex < this.DataGridView.Rows.Count)
                {
                    string[] names = this.OwningColumn.DataPropertyName.Split('.');
                    object obj = this.OwningRow.DataBoundItem;

                    if (obj != null)
                    {
                        foreach (var name in names)
                        {
                            obj = this.GetValueByName(obj, name);
                        }

                        return Convert.ToString(obj);
                    }
                }
            }
            catch
            {
            }

            return base.GetValue(rowIndex);
        }

        protected override bool SetValue(int rowIndex, object value)
        {
            try
            {
                if (rowIndex >= 0 && rowIndex < this.DataGridView.Rows.Count)
                {
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

        protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, System.ComponentModel.TypeConverter valueTypeConverter, System.ComponentModel.TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
        {
            if (rowIndex >= 0 && rowIndex < this.DataGridView.Rows.Count)
            {
                try
                {
                    string[] names = this.OwningColumn.DataPropertyName.Split('.');
                    DataGridViewRow dgrv = this.DataGridView.Rows[rowIndex];
                    object obj = dgrv.DataBoundItem;

                    if (obj != null)
                    {
                        foreach (var name in names)
                        {
                            Type[] ts = obj.GetType().GetInterfaces();
                            obj = this.GetValueByName(obj, name);
                        }

                        return Convert.ToString(obj);
                    }
                }
                catch (Exception exc)
                {
                    return exc.Message;
                }
            }

            return base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
        }

        private object GetValueByName(object obj, string name)
        {
            Type type = obj.GetType();
            PropertyInfo pi = type.GetProperty(name);

            if (pi != null)
            {
                object result = pi.GetValue(obj, null);
                return result;
            }

            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Xwt.Ext.Bindings
{
    public class ListBinding<T>: IList<T>, Xwt.IListDataSource
    {
        public FieldInfo[] Fields = typeof(T).GetFields();
        public List<T> Base;

        public Type[] ColumnTypes
        {
            get
            {
                return Fields.Select(X => X.FieldType).ToArray();
            }
        }

        public ListBinding () {
            Fields = typeof(T).GetFields();
            Base = new List<T>();
        }

        public ListBinding(List<T> Base)
        {
            this.Base = Base;
            Fields = typeof(T).GetFields();
        }

        public object GetValue(int row, int column)
        {
            return Fields[column].GetValue(this[row]);
        }

        public int RowCount
        {
            get { return this.Count(); }
        }

        public event EventHandler<Xwt.ListRowEventArgs> RowChanged;

        public event EventHandler<Xwt.ListRowEventArgs> RowDeleted;

        public event EventHandler<Xwt.ListRowEventArgs> RowInserted;

        public event EventHandler<Xwt.ListRowOrderEventArgs> RowsReordered;

        public void SetValue(int row, int column, object value)
        {
            Fields[column].SetValue(this[row], value);
        }

        public int IndexOf(T item)
        {
            return Base.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            Base.Insert(index, item);
            if (RowInserted != null)
                RowInserted(this, new Xwt.ListRowEventArgs(index));
        }

        public void RemoveAt(int index)
        {
            Base.RemoveAt(index);
            RowDeleted(this, new Xwt.ListRowEventArgs(index));
        }

        public T this[int index]
        {
            get
            {
                return Base[index];
            }
            set
            {
                Base[index] = value;
                if (RowChanged != null)
                    RowChanged(this, new Xwt.ListRowEventArgs(index));
            }
        }

        public void Add(T item)
        {
            Base.Add(item);
            if (RowInserted != null)
                RowInserted(this, new Xwt.ListRowEventArgs(Base.Count()));
        }

        public void Clear()
        {
            for (int i = 0; i < Base.Count(); i++)
                if (RowDeleted != null)
                    RowDeleted(this, new Xwt.ListRowEventArgs(i));
                Base.Clear();
        }

        public bool Contains(T item)
        {
            return Base.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Base.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return Base.Count(); }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            int i = Base.IndexOf(item);
            if (Base.Remove(item))
            {
                RowDeleted(this, new Xwt.ListRowEventArgs(i));
                return true;
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Base.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Base.GetEnumerator();
        }
    }
}

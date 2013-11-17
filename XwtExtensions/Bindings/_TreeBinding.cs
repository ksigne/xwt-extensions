using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XwtExtensions.Bindings
{
    public class _TreeBinding: IList<ITreeNode>, Xwt.ITreeDataSource
    {
        public List<ITreeNode> Base;
        public Type BackingType;

        public Xwt.TreePosition GetChild(Xwt.TreePosition pos, int index)
        {
            return Base.Where(X => X.Parent == (pos as ITreeNode)).ToArray()[index];
        }

        public int GetChildrenCount(Xwt.TreePosition pos)
        {
            return Base.Where(X => X.Parent == (pos as ITreeNode)).Count();
        }

        public Xwt.TreePosition GetParent(Xwt.TreePosition pos)
        {
            return ((pos as ITreeNode).Parent);
        }

            public Type[] ColumnTypes
            {
                get
                {
                    return BackingType.GetFields().Select(X => X.FieldType).ToArray();
                }
            }

            public _TreeBinding(Type Backingtype)
            {
                this.BackingType = Backingtype;
            }

            public _TreeBinding(List<ITreeNode> Base, Type Backingtype)
            {
                this.Base = Base;
                this.BackingType = Backingtype;
            }         

            public int IndexOf(ITreeNode item)
            {
                return Base.IndexOf(item);
            }

            public void Insert(int index, ITreeNode item)
            {
                Base.Insert(index, item);
                NodeInserted(this, new Xwt.TreeNodeEventArgs(Base[Base.Count()]));
            }

            int ChildIndex(ITreeNode item)
            {
                return Base.Where(X => X.Parent == item.Parent).ToList().IndexOf(item);
            }

            public void RemoveAt(int index)
            {
                NodeDeleted(this, new Xwt.TreeNodeChildEventArgs(Base[index].Parent, ChildIndex(Base[index])));
                Base.RemoveAt(index);

            }

            public ITreeNode this[int index]
            {
                get
                {
                    return Base[index];
                }
                set
                {
                    Base[index] = value;
                    NodeChanged(this, new Xwt.TreeNodeEventArgs(Base[index]));
                }
            }

            public void Add(ITreeNode item)
            {
                Base.Add(item);
                NodeInserted(this, new Xwt.TreeNodeEventArgs(item));
            }

            public void Clear()
            {
                for (int i = 0; i < Base.Count(); i++)
                {
                    NodeDeleted(this, new Xwt.TreeNodeChildEventArgs(Base[i].Parent, ChildIndex(Base[i])));
                    Base.Remove(Base[i]);
                }
            }

            public bool Contains(ITreeNode item)
            {
                return Base.Contains(item);
            }

            public void CopyTo(ITreeNode[] array, int arrayIndex)
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

            public bool Remove(ITreeNode item)
            {
                int i = Base.IndexOf(item);
                if (Base.Remove(item))
                {
                    NodeDeleted(this, new Xwt.TreeNodeChildEventArgs(item.Parent, ChildIndex(item)));
                    return true;
                }
                return false;
            }

            public IEnumerator<ITreeNode> GetEnumerator()
            {
                return Base.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return Base.GetEnumerator();
            }

        public object GetValue(Xwt.TreePosition pos, int column)
        {
            return pos.GetType().GetFields()[column].GetValue(pos);
        }

        public event EventHandler<Xwt.TreeNodeEventArgs> NodeChanged;

        public event EventHandler<Xwt.TreeNodeChildEventArgs> NodeDeleted;

        public event EventHandler<Xwt.TreeNodeEventArgs> NodeInserted;

        public event EventHandler<Xwt.TreeNodeOrderEventArgs> NodesReordered;

        public void SetValue(Xwt.TreePosition pos, int column, object value)
        {
            pos.GetType().GetFields()[column].SetValue(pos, value);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xwt;

namespace XwtExtensions.Bindings
{
    public abstract class TreeNode: INotifyPropertyChanged
    {
        public TreeNode Parent;
        public TreeStoreWrapper Container;
        public Xwt.TreePosition Position;

        public void Write(string FieldName) {
            if (Container != null)
            {
                object NewValue = (this).GetType().GetProperty(FieldName).GetValue(this);
                if (Container.Fields.ContainsKey(FieldName))
                {
                    IDataField F = Container.Fields[FieldName];
                    if (F.FieldType == typeof(bool))
                        Container.TreeStore.GetNavigatorAt(Position).SetValue<bool>((F as IDataField<bool>), (bool)NewValue);
                    if (F.FieldType == typeof(string))
                        Container.TreeStore.GetNavigatorAt(Position).SetValue<string>((F as IDataField<string>), (string)NewValue);
                    if (F.FieldType == typeof(DateTime))
                    {
                        Container.TreeStore.GetNavigatorAt(Position).SetValue<DateTime>((F as IDataField<DateTime>), (DateTime)NewValue);
                        Container.TreeStore.GetNavigatorAt(Position).SetValue<double>((Container.Fields[FieldName+"_sort"] as IDataField<double>), ((DateTime)NewValue).Ticks);
                        Container.TreeStore.GetNavigatorAt(Position).SetValue<string>((Container.Fields[FieldName + "_text"] as IDataField<string>), ((DateTime)NewValue) != DateTime.MinValue ? ((DateTime)NewValue).ToString() : "");
                    }
                    if (F.FieldType == typeof(Xwt.Drawing.Image))
                        Container.TreeStore.GetNavigatorAt(Position).SetValue<Xwt.Drawing.Image>((F as IDataField<Xwt.Drawing.Image>), (Xwt.Drawing.Image)NewValue);
                }
            }
        }
        public TreeNode()
        {
            this.PropertyChanged += (o, e) =>
            {
                Write(e.PropertyName);
            };
        }

        public virtual event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

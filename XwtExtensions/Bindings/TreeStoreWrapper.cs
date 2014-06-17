using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xwt;

namespace Xwt.Ext.Bindings
{
    public class TreeStoreWrapper {
        public TreeStore TreeStore;
        public Dictionary<string, IDataField> Fields = new Dictionary<string, IDataField>();
        public Xwt.DataField<TreeNode> SelfField = new Xwt.DataField<TreeNode>();
        public List<TreeNode> Mirror = new List<TreeNode>();

        public TreeStoreWrapper(Type NodeType)
        {
            foreach (PropertyInfo Field in NodeType.GetProperties()) {
                if (Field.PropertyType == typeof(string))
                    Fields.Add(Field.Name, new Xwt.DataField<string>());
                if (Field.PropertyType == typeof(bool))
                    Fields.Add(Field.Name, new Xwt.DataField<bool>());
                if (Field.PropertyType == typeof(DateTime))
                {
                    Fields.Add(Field.Name, new Xwt.DataField<DateTime>());
                    Fields.Add(Field.Name+"_sort", new Xwt.DataField<double>());
                    Fields.Add(Field.Name + "_text", new Xwt.DataField<string>());
                }
                if (Field.PropertyType == typeof(Xwt.Drawing.Image))
                    Fields.Add(Field.Name, new Xwt.DataField<Xwt.Drawing.Image>());
            }
            List<IDataField> TargetField = Fields.Values.ToList();
            TargetField.Add(SelfField);
            this.TreeStore = new Xwt.TreeStore(TargetField.ToArray());
        }

        public TreeNode GetObject(Xwt.TreePosition Position)
        {
            return TreeStore.GetNavigatorAt(Position).GetValue(SelfField);
        }

        public void AddItem(Xwt.TreePosition Parent, TreeNode Item) {
            Xwt.TreeNavigator P;
            if (Parent!=null)
                P = TreeStore.AddNode(Parent);
            else P = TreeStore.AddNode();
            Item.Container = this;
            Item.Position = P.CurrentPosition;
            Item.GetType().GetProperties().ToList().ForEach(X => Item.Write(X.Name));
            P.SetValue<TreeNode>(SelfField, Item);
            Mirror.Add(Item);
            if (P.MoveToParent())
                Item.Parent = P.GetValue<TreeNode>(SelfField);
            else Item.Parent = null;
        }

    }
}

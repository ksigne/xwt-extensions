using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xwt;

namespace XwtExtensions.Bindings
{
    public class TreeStoreWrapper {
        public TreeStore TreeStore;
        public Dictionary<string, IDataField> Fields = new Dictionary<string, IDataField>();
        public Xwt.DataField<TreeNode> SelfField = new Xwt.DataField<TreeNode>();

        public TreeStoreWrapper(Type NodeType)
        {
            foreach (PropertyInfo Field in NodeType.GetProperties()) {
                if (Field.PropertyType == typeof(string))
                    Fields.Add(Field.Name, new Xwt.DataField<string>());
                if (Field.PropertyType == typeof(bool))
                    Fields.Add(Field.Name, new Xwt.DataField<bool>());
                if (Field.PropertyType == typeof(Xwt.Drawing.Image))
                    Fields.Add(Field.Name, new Xwt.DataField<Xwt.Drawing.Image>());
            }
            List<IDataField> TargetField = Fields.Values.ToList();
            TargetField.Add(SelfField);
            this.TreeStore = new Xwt.TreeStore(TargetField.ToArray());
        } 

        public void AddItem(Xwt.TreePosition Parent, TreeNode Item) {
            Xwt.TreeNavigator P = TreeStore.AddNode(Parent);
            Item.Container = this;
            Item.Position = P.CurrentPosition;
            Item.GetType().GetProperties().ToList().ForEach(X => Item.Write(X.Name));
            P.SetValue<TreeNode>(SelfField, Item);
            if (P.MoveToParent())
                Item.Parent = P.GetValue<TreeNode>(SelfField);
            else Item.Parent = null;
        }

    }
}

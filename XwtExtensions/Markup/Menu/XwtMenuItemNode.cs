using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using YAXLib;

namespace Xwt.Ext.Markup
{
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AllFields)]
    [YAXSerializeAs("Item")]
    public class XwtMenuItemNode
    {
        [YAXAttributeForClass]
        public string Name = "";
        [YAXAttributeForClass]
        public string Text = "";
        [YAXAttributeForClass]
        public bool UseMnemonic = false;
        [YAXAttributeForClass]
        public bool Visible = true;
        [YAXAttributeForClass]
        public bool Sensitive = true;
        [YAXAttributeForClass]
        public string Activated = "";

        [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement)]
        public List<XwtMenuItemNode> Subitems;

        protected void RegisterItem(IXwtWrapper Parent, Xwt.MenuItem Widget)
        {
            if (this.Name != "")
            {
                Parent.Widgets.Add(this.Name, Widget);
                if (Parent.GetType().GetField(this.Name) != null)
                    Parent.GetType().GetField(this.Name).SetValue(Parent, Widget);
                WindowController.RegisterWidget(this.Name, Parent, Widget);
            }
        }

        public virtual Xwt.MenuItem Makeup(IXwtWrapper Parent)
        {
            Xwt.MenuItem Target = new Xwt.MenuItem() {
                Label = Text,
                Sensitive = this.Sensitive,
                Visible = this.Visible,
                UseMnemonic = this.UseMnemonic,
            };

            if (Subitems != null && Subitems.Count() > 0)
            {
                Target.SubMenu = new Xwt.Menu();
                foreach (var Subitem in Subitems)
                    Target.SubMenu.Items.Add(Subitem.Makeup(Parent));
            }

            RegisterItem(Parent, Target);

            WindowController.TryAttachEvent(Target, "Clicked", Parent, Activated);

            return Target;
        }
    }
}

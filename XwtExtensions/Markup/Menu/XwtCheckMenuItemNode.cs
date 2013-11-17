using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using YAXLib;

namespace XwtExtensions.Markup
{
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AllFields)]
    [YAXSerializeAs("CheckItem")]
    public class XwtCheckItemMenuNode: XwtMenuItemNode
    {
        [YAXAttributeForClass]
        public bool Checked = false;
        public override Xwt.MenuItem Makeup(WindowWrapper Parent)
        {
            Xwt.CheckBoxMenuItem Target = new Xwt.CheckBoxMenuItem() {
                Label = Text,
                Sensitive = this.Sensitive,
                Visible = this.Visible,
                UseMnemonic = this.UseMnemonic,
                Checked = this.Checked
            };

            Target.Clicked += (o, e) =>
            {

            };

            WindowController.TryAttachEvent(Target, "Clicked", Parent, Activated);

            if (Subitems != null && Subitems.Count() > 0)
            {
                Target.SubMenu = new Xwt.Menu();
                foreach (var Subitem in Subitems)
                    Target.SubMenu.Items.Add(Subitem.Makeup(Parent));
            }

            RegisterItem(Parent, Target);

            return Target;
        }
    }
}

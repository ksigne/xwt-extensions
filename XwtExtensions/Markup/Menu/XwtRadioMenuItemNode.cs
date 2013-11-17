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
    [YAXSerializeAs("RadioItem")]
    public class XwtRadioMenuItemNode: XwtMenuItemNode
    {
        [YAXAttributeForClass]
        public string Group = "";
        [YAXAttributeForClass]
        public bool Checked = false;
        public override Xwt.MenuItem Makeup(WindowWrapper Parent)
        {
            Xwt.RadioButtonMenuItem Target = new Xwt.RadioButtonMenuItem() {
                Label = Text,
                Sensitive = this.Sensitive,
                Visible = this.Visible,
                UseMnemonic = this.UseMnemonic,
                Checked = this.Checked
            };

            if (this.Group != "")
            {
                if (!WindowController.MenuRadioGroups.ContainsKey(this.Group))
                    WindowController.MenuRadioGroups.Add(this.Group, new Xwt.RadioButtonMenuItemGroup());
                Target.Group = WindowController.MenuRadioGroups[this.Group];
            }

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

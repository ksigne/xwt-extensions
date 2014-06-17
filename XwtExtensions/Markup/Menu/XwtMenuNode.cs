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
    [YAXSerializeAs("Menu")]
    public class XwtMenuNode
    {
        [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement)]
        public List<XwtMenuItemNode> Subitems;

        public Xwt.Menu Makeup(IXwtWrapper Parent)
        {
            Xwt.Menu Target = new Xwt.Menu();

            if (Subitems != null && Subitems.Count() > 0)
            {
                foreach (var Subitem in Subitems)
                    Target.Items.Add(Subitem.Makeup(Parent));
            }

            return Target;
        }
    }
}

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
    [YAXSerializeAs("Separator")]
    public class XwtSeparatorMenuItem: XwtMenuItemNode
    {
        public override Xwt.MenuItem Makeup(WindowWrapper Parent)
        {
            return new Xwt.SeparatorMenuItem();
        }
    }
}

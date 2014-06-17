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
    [YAXSerializeAs("Separator")]
    public class XwtSeparatorMenuItem: XwtMenuItemNode
    {
        public override Xwt.MenuItem Makeup(IXwtWrapper Parent)
        {
            return new Xwt.SeparatorMenuItem();
        }
    }
}

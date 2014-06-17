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
    public class XwtSimpleBindingNode
    {
        [YAXAttributeForClass]
        public string Value = "";
        [YAXValueForClass]
        public string Text = "";
    }
}

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
    [YAXSerializeAs("Column")]
    public class XwtColumnDefinitionNode
    {
        [YAXAttributeForClass]
        public string Source = "";
        [YAXAttributeForClass]
        public bool Editable = false;
        [YAXAttributeForClass]
        public string Title = "";
        [YAXAttributeForClass]
        public bool Sortable = false;
        [YAXAttributeForClass]
        public bool Resizable = true;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace XwtExtensions.Markup.Layouts
{
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AllFields)]
    [YAXSerializeAs("Separator")]
    public class XwtButton : XwtWidgetNode
    {
        [YAXAttributeForClass]
        public Xwt.Backends.Orientation Orientation;

        public override Xwt.Widget Makeup(WindowWrapper Parent)
        {
            Xwt.Separator Target;
            if (this.Orientation == Xwt.Backends.Orientation.Horizontal)
                Target = new Xwt.HSeparator();
            else Target = new Xwt.VSeparator();
            InitWidget(Target, Parent);
            return Target;
        }
    }
}

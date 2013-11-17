using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAXLib;

namespace XwtExtensions.Markup.Widgets
{
    [YAXSerializableType(FieldsToSerialize=YAXSerializationFields.AllFields)]
    [YAXSerializeAs("Spinner")]
    public class XwtSpinnerNode: XwtWidgetNode
    {
        [YAXAttributeForClass]
        public bool Animate = true;

        public override Xwt.Widget Makeup(WindowWrapper Parent)
        {
            Xwt.Spinner Target = new Xwt.Spinner() {
                Animate = this.Animate
            };
            InitWidget(Target, Parent);
            return Target;
        }
    }
}

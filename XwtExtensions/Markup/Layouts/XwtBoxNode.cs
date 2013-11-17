using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using YAXLib;

namespace XwtExtensions.Markup.Widgets
{
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AllFields)]
    [YAXSerializeAs("Box")]
    public class XwtBoxNode: XwtWidgetNode
    {
        [YAXAttributeForClass]
        public double Spacing = 10;
        [YAXAttributeForClass]
        public Xwt.Backends.Orientation Orientation = Xwt.Backends.Orientation.Horizontal;

        [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement)]
        public List<XwtWidgetNode> Items;

        public override Xwt.Widget Makeup(WindowWrapper Parent)
        {
            Xwt.Box Target;

            if (Orientation == Xwt.Backends.Orientation.Horizontal)
                Target = new Xwt.HBox() {
                    Spacing = this.Spacing
                };
            else Target = new Xwt.VBox() {
                Spacing = this.Spacing
            };

            InitWidget(Target, Parent);

            foreach (XwtWidgetNode Node in this.Items)
            {
                Target.PackStart(Node.Makeup(Parent), Node.Expand, Node.Fill);
            }

            return Target;
        }
    }
}

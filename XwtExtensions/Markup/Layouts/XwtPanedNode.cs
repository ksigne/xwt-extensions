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
    [YAXSerializeAs("Paned")]
    public class XwtPanedNode: XwtWidgetNode
    {
        [YAXAttributeForClass]
        public double Position = 0.5;
        [YAXAttributeForClass]
        public Xwt.Backends.Orientation Orientation = Xwt.Backends.Orientation.Horizontal;
        [YAXAttributeForClass]
        public string Resized = "";
        [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement)]
        public List<XwtWidgetNode> Items;

        public override Xwt.Widget Makeup(WindowWrapper Parent)
        {
            Xwt.Paned Target;

            if (Orientation == Xwt.Backends.Orientation.Horizontal)
                Target = new Xwt.HPaned();
            else Target = new Xwt.VPaned();

            Target.PositionFraction = this.Position;

            if (Items != null && Items.Count() > 1) {
                Target.Panel1.Content = Items[0].Makeup(Parent);
                Target.Panel2.Content = Items[1].Makeup(Parent);
            }

            WindowController.TryAttachEvent(Target, "PositionChanged", Parent, Resized);

            InitWidget(Target, Parent);

            return Target;
        }
    }
}

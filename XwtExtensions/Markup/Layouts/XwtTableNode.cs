using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using YAXLib;

namespace Xwt.Ext.Markup.Widgets
{
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AllFields)]
    [YAXSerializeAs("Table")]
    public class XwtTableNode: XwtWidgetNode
    {
        [YAXAttributeForClass]
        public double RowSpacing = 10;
        [YAXAttributeForClass]
        public double ColSpacing = 10;

        [YAXCollection(YAXCollectionSerializationTypes.RecursiveWithNoContainingElement)]
        public List<XwtWidgetNode> Items;

        public override Xwt.Widget Makeup(IXwtWrapper Parent)
        {
            Xwt.Table Target = new Xwt.Table()
            {
                DefaultRowSpacing = RowSpacing,
                DefaultColumnSpacing = ColSpacing
            };

            InitWidget(Target, Parent);

            foreach (XwtWidgetNode Node in this.Items)
            {
                Target.Attach(Node.Makeup(Parent), Node.Row, Node.Column);
            }

            return Target;
        }
    }
}
